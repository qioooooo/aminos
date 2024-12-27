using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000087 RID: 135
	internal class WmiEventSink : IWmiEventSource
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060003C9 RID: 969 RVA: 0x00010993 File Offset: 0x0000F993
		// (remove) Token: 0x060003CA RID: 970 RVA: 0x000109AC File Offset: 0x0000F9AC
		internal event InternalObjectPutEventHandler InternalObjectPut;

		// Token: 0x060003CB RID: 971 RVA: 0x000109C8 File Offset: 0x0000F9C8
		internal static WmiEventSink GetWmiEventSink(ManagementOperationObserver watcher, object context, ManagementScope scope, string path, string className)
		{
			if (MTAHelper.IsNoContextMTA())
			{
				return new WmiEventSink(watcher, context, scope, path, className);
			}
			WmiEventSink.watcherParameter = watcher;
			WmiEventSink.contextParameter = context;
			WmiEventSink.scopeParameter = scope;
			WmiEventSink.pathParameter = path;
			WmiEventSink.classNameParameter = className;
			ThreadDispatch threadDispatch = new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethod(WmiEventSink.HackToCreateWmiEventSink));
			threadDispatch.Start();
			return WmiEventSink.wmiEventSinkNew;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00010A24 File Offset: 0x0000FA24
		private static void HackToCreateWmiEventSink()
		{
			WmiEventSink.wmiEventSinkNew = new WmiEventSink(WmiEventSink.watcherParameter, WmiEventSink.contextParameter, WmiEventSink.scopeParameter, WmiEventSink.pathParameter, WmiEventSink.classNameParameter);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00010A4C File Offset: 0x0000FA4C
		protected WmiEventSink(ManagementOperationObserver watcher, object context, ManagementScope scope, string path, string className)
		{
			try
			{
				this.context = context;
				this.watcher = watcher;
				this.className = className;
				this.isLocal = false;
				if (path != null)
				{
					this.path = new ManagementPath(path);
					if (string.Compare(this.path.Server, ".", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(this.path.Server, Environment.MachineName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.isLocal = true;
					}
				}
				if (scope != null)
				{
					this.scope = scope.Clone();
					if (path == null && (string.Compare(this.scope.Path.Server, ".", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(this.scope.Path.Server, Environment.MachineName, StringComparison.OrdinalIgnoreCase) == 0))
					{
						this.isLocal = true;
					}
				}
				WmiNetUtilsHelper.GetDemultiplexedStub_f(this, this.isLocal, out this.stub);
				this.hash = Interlocked.Increment(ref WmiEventSink.s_hash);
			}
			catch
			{
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00010B58 File Offset: 0x0000FB58
		public override int GetHashCode()
		{
			return this.hash;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00010B60 File Offset: 0x0000FB60
		public IWbemObjectSink Stub
		{
			get
			{
				IWbemObjectSink wbemObjectSink;
				try
				{
					wbemObjectSink = ((this.stub != null) ? ((IWbemObjectSink)this.stub) : null);
				}
				catch
				{
					wbemObjectSink = null;
				}
				return wbemObjectSink;
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00010B9C File Offset: 0x0000FB9C
		public virtual void Indicate(IntPtr pIWbemClassObject)
		{
			Marshal.AddRef(pIWbemClassObject);
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(pIWbemClassObject);
			try
			{
				ObjectReadyEventArgs objectReadyEventArgs = new ObjectReadyEventArgs(this.context, ManagementBaseObject.GetBaseObject(wbemClassObjectFreeThreaded, this.scope));
				this.watcher.FireObjectReady(objectReadyEventArgs);
			}
			catch
			{
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00010BF0 File Offset: 0x0000FBF0
		public void SetStatus(int flags, int hResult, string message, IntPtr pErrorObj)
		{
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = null;
			if (pErrorObj != IntPtr.Zero)
			{
				Marshal.AddRef(pErrorObj);
				wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(pErrorObj);
			}
			try
			{
				if (flags == 0)
				{
					if (this.path != null)
					{
						if (this.className == null)
						{
							this.path.RelativePath = message;
						}
						else
						{
							this.path.RelativePath = this.className;
						}
						if (this.InternalObjectPut != null)
						{
							try
							{
								InternalObjectPutEventArgs internalObjectPutEventArgs = new InternalObjectPutEventArgs(this.path);
								this.InternalObjectPut(this, internalObjectPutEventArgs);
							}
							catch
							{
							}
						}
						ObjectPutEventArgs objectPutEventArgs = new ObjectPutEventArgs(this.context, this.path);
						this.watcher.FireObjectPut(objectPutEventArgs);
					}
					CompletedEventArgs completedEventArgs;
					if (wbemClassObjectFreeThreaded != null)
					{
						completedEventArgs = new CompletedEventArgs(this.context, hResult, new ManagementBaseObject(wbemClassObjectFreeThreaded));
					}
					else
					{
						completedEventArgs = new CompletedEventArgs(this.context, hResult, null);
					}
					this.watcher.FireCompleted(completedEventArgs);
					this.watcher.RemoveSink(this);
				}
				else if ((flags & 2) != 0)
				{
					ProgressEventArgs progressEventArgs = new ProgressEventArgs(this.context, (int)((uint)(hResult & -65536) >> 16), hResult & 65535, message);
					this.watcher.FireProgress(progressEventArgs);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00010D2C File Offset: 0x0000FD2C
		internal void Cancel()
		{
			try
			{
				this.scope.GetIWbemServices().CancelAsyncCall_((IWbemObjectSink)this.stub);
			}
			catch
			{
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00010D6C File Offset: 0x0000FD6C
		internal void ReleaseStub()
		{
			try
			{
				if (this.stub != null)
				{
					Marshal.ReleaseComObject(this.stub);
					this.stub = null;
				}
			}
			catch
			{
			}
		}

		// Token: 0x040001F2 RID: 498
		private static int s_hash;

		// Token: 0x040001F3 RID: 499
		private int hash;

		// Token: 0x040001F4 RID: 500
		private ManagementOperationObserver watcher;

		// Token: 0x040001F5 RID: 501
		private object context;

		// Token: 0x040001F6 RID: 502
		private ManagementScope scope;

		// Token: 0x040001F7 RID: 503
		private object stub;

		// Token: 0x040001F9 RID: 505
		private ManagementPath path;

		// Token: 0x040001FA RID: 506
		private string className;

		// Token: 0x040001FB RID: 507
		private bool isLocal;

		// Token: 0x040001FC RID: 508
		private static ManagementOperationObserver watcherParameter;

		// Token: 0x040001FD RID: 509
		private static object contextParameter;

		// Token: 0x040001FE RID: 510
		private static ManagementScope scopeParameter;

		// Token: 0x040001FF RID: 511
		private static string pathParameter;

		// Token: 0x04000200 RID: 512
		private static string classNameParameter;

		// Token: 0x04000201 RID: 513
		private static WmiEventSink wmiEventSinkNew;
	}
}

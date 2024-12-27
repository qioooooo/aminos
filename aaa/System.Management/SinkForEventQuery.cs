using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management
{
	// Token: 0x0200001A RID: 26
	internal class SinkForEventQuery : IWmiEventSource
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00006A10 File Offset: 0x00005A10
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00006A18 File Offset: 0x00005A18
		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006A24 File Offset: 0x00005A24
		public SinkForEventQuery(ManagementEventWatcher eventWatcher, object context, IWbemServices services)
		{
			this.services = services;
			this.context = context;
			this.eventWatcher = eventWatcher;
			this.status = 0;
			this.isLocal = false;
			if (string.Compare(eventWatcher.Scope.Path.Server, ".", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(eventWatcher.Scope.Path.Server, Environment.MachineName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.isLocal = true;
			}
			if (MTAHelper.IsNoContextMTA())
			{
				this.HackToCreateStubInMTA(this);
				return;
			}
			new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethodWithParam(this.HackToCreateStubInMTA))
			{
				Parameter = this
			}.Start();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006ACC File Offset: 0x00005ACC
		private void HackToCreateStubInMTA(object param)
		{
			SinkForEventQuery sinkForEventQuery = (SinkForEventQuery)param;
			object obj = null;
			sinkForEventQuery.Status = WmiNetUtilsHelper.GetDemultiplexedStub_f(sinkForEventQuery, sinkForEventQuery.isLocal, out obj);
			sinkForEventQuery.stub = (IWbemObjectSink)obj;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00006B07 File Offset: 0x00005B07
		internal IWbemObjectSink Stub
		{
			get
			{
				return this.stub;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00006B10 File Offset: 0x00005B10
		public void Indicate(IntPtr pWbemClassObject)
		{
			Marshal.AddRef(pWbemClassObject);
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(pWbemClassObject);
			try
			{
				EventArrivedEventArgs eventArrivedEventArgs = new EventArrivedEventArgs(this.context, new ManagementBaseObject(wbemClassObjectFreeThreaded));
				this.eventWatcher.FireEventArrived(eventArrivedEventArgs);
			}
			catch
			{
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00006B60 File Offset: 0x00005B60
		public void SetStatus(int flags, int hResult, string message, IntPtr pErrObj)
		{
			try
			{
				this.eventWatcher.FireStopped(new StoppedEventArgs(this.context, hResult));
				if (hResult != -2147217358 && hResult != 262150)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(this.Cancel2));
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006BBC File Offset: 0x00005BBC
		private void Cancel2(object o)
		{
			try
			{
				this.Cancel();
			}
			catch
			{
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00006BE4 File Offset: 0x00005BE4
		internal void Cancel()
		{
			if (this.stub != null)
			{
				lock (this)
				{
					if (this.stub != null)
					{
						int num = this.services.CancelAsyncCall_(this.stub);
						this.ReleaseStub();
						if (num < 0)
						{
							if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
							{
								ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
							}
							else
							{
								Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
							}
						}
					}
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006C68 File Offset: 0x00005C68
		internal void ReleaseStub()
		{
			if (this.stub != null)
			{
				lock (this)
				{
					if (this.stub != null)
					{
						try
						{
							Marshal.ReleaseComObject(this.stub);
							this.stub = null;
						}
						catch
						{
						}
					}
				}
			}
		}

		// Token: 0x04000094 RID: 148
		private ManagementEventWatcher eventWatcher;

		// Token: 0x04000095 RID: 149
		private object context;

		// Token: 0x04000096 RID: 150
		private IWbemServices services;

		// Token: 0x04000097 RID: 151
		private IWbemObjectSink stub;

		// Token: 0x04000098 RID: 152
		private int status;

		// Token: 0x04000099 RID: 153
		private bool isLocal;
	}
}

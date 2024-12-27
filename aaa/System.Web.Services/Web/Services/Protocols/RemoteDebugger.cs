using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web.Services.Diagnostics;
using System.Web.Services.Interop;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000052 RID: 82
	internal class RemoteDebugger : INotifySource2
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00007D38 File Offset: 0x00006D38
		private static object InternalSyncObject
		{
			get
			{
				if (RemoteDebugger.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref RemoteDebugger.s_InternalSyncObject, obj, null);
				}
				return RemoteDebugger.s_InternalSyncObject;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007D64 File Offset: 0x00006D64
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal RemoteDebugger()
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00007D6C File Offset: 0x00006D6C
		~RemoteDebugger()
		{
			this.Close();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00007D98 File Offset: 0x00006D98
		internal static bool IsClientCallOutEnabled()
		{
			bool flag = false;
			try
			{
				flag = !CompModSwitches.DisableRemoteDebugging.Enabled && Debugger.IsAttached && RemoteDebugger.Connection != null;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "IsClientCallOutEnabled", ex);
				}
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00007E28 File Offset: 0x00006E28
		internal static bool IsServerCallInEnabled(ServerProtocol protocol, out string stringBuffer)
		{
			stringBuffer = null;
			bool flag = false;
			try
			{
				if (CompModSwitches.DisableRemoteDebugging.Enabled)
				{
					return false;
				}
				flag = protocol.Context.IsDebuggingEnabled && RemoteDebugger.Connection != null;
				if (flag)
				{
					stringBuffer = protocol.Request.Headers[RemoteDebugger.debuggerHeader];
					flag = stringBuffer != null && stringBuffer.Length > 0;
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "IsServerCallInEnabled", ex);
				}
				flag = false;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007EF8 File Offset: 0x00006EF8
		private static INotifyConnection2 Connection
		{
			get
			{
				if (RemoteDebugger.connection == null && RemoteDebugger.getConnection)
				{
					lock (RemoteDebugger.InternalSyncObject)
					{
						if (RemoteDebugger.connection == null)
						{
							AppDomain.CurrentDomain.DomainUnload += RemoteDebugger.OnAppDomainUnload;
							AppDomain.CurrentDomain.ProcessExit += RemoteDebugger.OnProcessExit;
							TraceMethod traceMethod = (Tracing.On ? new TraceMethod(typeof(RemoteDebugger), "get_Connection", new object[0]) : null);
							if (Tracing.On)
							{
								Tracing.Enter("RemoteDebugger", traceMethod);
							}
							object obj;
							int num = UnsafeNativeMethods.CoCreateInstance(ref RemoteDebugger.IID_NotifyConnectionClassGuid, null, 1, ref RemoteDebugger.IID_NotifyConnection2Guid, out obj);
							if (Tracing.On)
							{
								Tracing.Exit("RemoteDebugger", traceMethod);
							}
							if (num >= 0)
							{
								RemoteDebugger.connection = (INotifyConnection2)obj;
							}
							else
							{
								RemoteDebugger.connection = null;
							}
						}
						RemoteDebugger.getConnection = false;
					}
				}
				return RemoteDebugger.connection;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00007FF4 File Offset: 0x00006FF4
		private INotifySink2 NotifySink
		{
			get
			{
				if (this.notifySink == null && RemoteDebugger.Connection != null)
				{
					TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "get_NotifySink", new object[0]) : null);
					if (Tracing.On)
					{
						Tracing.Enter("RemoteDebugger", traceMethod);
					}
					this.notifySink = UnsafeNativeMethods.RegisterNotifySource(RemoteDebugger.Connection, this);
					if (Tracing.On)
					{
						Tracing.Exit("RemoteDebugger", traceMethod);
					}
				}
				return this.notifySink;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008068 File Offset: 0x00007068
		private static void CloseSharedResources()
		{
			if (RemoteDebugger.connection != null)
			{
				lock (RemoteDebugger.InternalSyncObject)
				{
					if (RemoteDebugger.connection != null)
					{
						TraceMethod traceMethod = (Tracing.On ? new TraceMethod(typeof(RemoteDebugger), "CloseSharedResources", new object[0]) : null);
						if (Tracing.On)
						{
							Tracing.Enter("RemoteDebugger", traceMethod);
						}
						try
						{
							Marshal.ReleaseComObject(RemoteDebugger.connection);
						}
						catch (Exception ex)
						{
							if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
							{
								throw;
							}
							if (Tracing.On)
							{
								Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "CloseSharedResources", ex);
							}
						}
						catch
						{
						}
						if (Tracing.On)
						{
							Tracing.Exit("RemoteDebugger", traceMethod);
						}
						RemoteDebugger.connection = null;
					}
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008160 File Offset: 0x00007160
		private void Close()
		{
			if (this.notifySink != null && RemoteDebugger.connection != null)
			{
				lock (RemoteDebugger.InternalSyncObject)
				{
					if (this.notifySink != null && RemoteDebugger.connection != null)
					{
						TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "Close", new object[0]) : null);
						if (Tracing.On)
						{
							Tracing.Enter("RemoteDebugger", traceMethod);
						}
						try
						{
							UnsafeNativeMethods.UnregisterNotifySource(RemoteDebugger.connection, this);
						}
						catch (Exception ex)
						{
							if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
							{
								throw;
							}
							if (Tracing.On)
							{
								Tracing.ExceptionCatch(TraceEventType.Warning, traceMethod, ex);
							}
						}
						catch
						{
						}
						if (Tracing.On)
						{
							Tracing.Exit("RemoteDebugger", traceMethod);
						}
						this.notifySink = null;
					}
				}
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008258 File Offset: 0x00007258
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal void NotifyClientCallOut(WebRequest request)
		{
			try
			{
				if (this.NotifySink != null)
				{
					int num = 0;
					CallId callId = new CallId(null, 0, (IntPtr)0, 0L, null, request.RequestUri.Host);
					TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "NotifyClientCallOut", new object[0]) : null);
					if (Tracing.On)
					{
						Tracing.Enter("RemoteDebugger", traceMethod);
					}
					IntPtr intPtr;
					UnsafeNativeMethods.OnSyncCallOut(this.NotifySink, callId, out intPtr, ref num);
					if (Tracing.On)
					{
						Tracing.Exit("RemoteDebugger", traceMethod);
					}
					if (!(intPtr == IntPtr.Zero))
					{
						byte[] array = null;
						try
						{
							array = new byte[num];
							Marshal.Copy(intPtr, array, 0, num);
						}
						finally
						{
							Marshal.FreeCoTaskMem(intPtr);
						}
						string text = Convert.ToBase64String(array);
						request.Headers.Add(RemoteDebugger.debuggerHeader, text);
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "NotifyClientCallOut", ex);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008398 File Offset: 0x00007398
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal void NotifyClientCallReturn(WebResponse response)
		{
			try
			{
				if (this.NotifySink == null)
				{
					return;
				}
				byte[] array = new byte[0];
				if (response != null)
				{
					string text = response.Headers[RemoteDebugger.debuggerHeader];
					if (text != null && text.Length != 0)
					{
						array = Convert.FromBase64String(text);
					}
				}
				CallId callId = new CallId(null, 0, (IntPtr)0, 0L, null, null);
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "NotifyClientCallReturn", new object[0]) : null);
				if (Tracing.On)
				{
					Tracing.Enter("RemoteDebugger", traceMethod);
				}
				UnsafeNativeMethods.OnSyncCallReturn(this.NotifySink, callId, array, array.Length);
				if (Tracing.On)
				{
					Tracing.Exit("RemoteDebugger", traceMethod);
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "NotifyClientCallReturn", ex);
				}
			}
			catch
			{
			}
			this.Close();
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000084AC File Offset: 0x000074AC
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal void NotifyServerCallEnter(ServerProtocol protocol, string stringBuffer)
		{
			try
			{
				if (this.NotifySink != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(protocol.Type.FullName);
					stringBuilder.Append('.');
					stringBuilder.Append(protocol.MethodInfo.Name);
					stringBuilder.Append('(');
					ParameterInfo[] parameters = protocol.MethodInfo.Parameters;
					for (int i = 0; i < parameters.Length; i++)
					{
						if (i != 0)
						{
							stringBuilder.Append(',');
						}
						stringBuilder.Append(parameters[i].ParameterType.FullName);
					}
					stringBuilder.Append(')');
					byte[] array = Convert.FromBase64String(stringBuffer);
					CallId callId = new CallId(null, 0, (IntPtr)0, 0L, stringBuilder.ToString(), null);
					TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "NotifyServerCallEnter", new object[0]) : null);
					if (Tracing.On)
					{
						Tracing.Enter("RemoteDebugger", traceMethod);
					}
					UnsafeNativeMethods.OnSyncCallEnter(this.NotifySink, callId, array, array.Length);
					if (Tracing.On)
					{
						Tracing.Exit("RemoteDebugger", traceMethod);
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "NotifyServerCallEnter", ex);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008634 File Offset: 0x00007634
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal void NotifyServerCallExit(HttpResponse response)
		{
			try
			{
				if (this.NotifySink == null)
				{
					return;
				}
				int num = 0;
				CallId callId = new CallId(null, 0, (IntPtr)0, 0L, null, null);
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "NotifyServerCallExit", new object[0]) : null);
				if (Tracing.On)
				{
					Tracing.Enter("RemoteDebugger", traceMethod);
				}
				IntPtr intPtr;
				UnsafeNativeMethods.OnSyncCallExit(this.NotifySink, callId, out intPtr, ref num);
				if (Tracing.On)
				{
					Tracing.Exit("RemoteDebugger", traceMethod);
				}
				if (intPtr == IntPtr.Zero)
				{
					return;
				}
				byte[] array = null;
				try
				{
					array = new byte[num];
					Marshal.Copy(intPtr, array, 0, num);
				}
				finally
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				string text = Convert.ToBase64String(array);
				response.AddHeader(RemoteDebugger.debuggerHeader, text);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, typeof(RemoteDebugger), "NotifyServerCallExit", ex);
				}
			}
			catch
			{
			}
			this.Close();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000876C File Offset: 0x0000776C
		private static void OnAppDomainUnload(object sender, EventArgs args)
		{
			RemoteDebugger.CloseSharedResources();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008773 File Offset: 0x00007773
		private static void OnProcessExit(object sender, EventArgs args)
		{
			RemoteDebugger.CloseSharedResources();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000877A File Offset: 0x0000777A
		void INotifySource2.SetNotifyFilter(NotifyFilter in_NotifyFilter, UserThread in_pUserThreadFilter)
		{
			this.notifyFilter = in_NotifyFilter;
			this.userThread = in_pUserThreadFilter;
		}

		// Token: 0x040002B8 RID: 696
		private const int INPROC_SERVER = 1;

		// Token: 0x040002B9 RID: 697
		private static INotifyConnection2 connection;

		// Token: 0x040002BA RID: 698
		private static bool getConnection = true;

		// Token: 0x040002BB RID: 699
		private INotifySink2 notifySink;

		// Token: 0x040002BC RID: 700
		private NotifyFilter notifyFilter;

		// Token: 0x040002BD RID: 701
		private UserThread userThread;

		// Token: 0x040002BE RID: 702
		private static Guid IID_NotifyConnectionClassGuid = new Guid("12A5B9F0-7A1C-4fcb-8163-160A30F519B5");

		// Token: 0x040002BF RID: 703
		private static Guid IID_NotifyConnection2Guid = new Guid("1AF04045-6659-4aaa-9F4B-2741AC56224B");

		// Token: 0x040002C0 RID: 704
		private static string debuggerHeader = "VsDebuggerCausalityData";

		// Token: 0x040002C1 RID: 705
		private static object s_InternalSyncObject;
	}
}

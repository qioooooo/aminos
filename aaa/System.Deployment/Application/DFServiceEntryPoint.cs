using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Internal.Performance;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x020000DC RID: 220
	internal static class DFServiceEntryPoint
	{
		// Token: 0x060005B6 RID: 1462 RVA: 0x0001DF04 File Offset: 0x0001CF04
		private static void MessageLoopThread()
		{
			if (DFServiceEntryPoint._dfsvcForm != null)
			{
				return;
			}
			DFServiceEntryPoint._dfsvcForm = new DFServiceEntryPoint.DfsvcForm();
			SystemEvents.SessionEnded += DFServiceEntryPoint._dfsvcForm.SessionEndedEventHandler;
			SystemEvents.SessionEnding += DFServiceEntryPoint._dfsvcForm.SessionEndingEventHandler;
			Application.Run(DFServiceEntryPoint._dfsvcForm);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001DF58 File Offset: 0x0001CF58
		private static object GetMethodDelegate(IntPtr handle, string methodName, Type methodDelegateType)
		{
			IntPtr procAddress = NativeMethods.GetProcAddress(handle, methodName);
			if (procAddress == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return Marshal.GetDelegateForFunctionPointer(procAddress, methodDelegateType);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001DF8C File Offset: 0x0001CF8C
		private static void ObtainDfdllExports()
		{
			DFServiceEntryPoint.DfdllHandle = NativeMethods.LoadLibrary(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "dfdll.dll"));
			if (DFServiceEntryPoint.DfdllHandle == IntPtr.Zero)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			DFServiceEntryPoint.RegisterDeploymentServiceCom = (DFServiceEntryPoint.RegisterDeploymentServiceComDelegate)DFServiceEntryPoint.GetMethodDelegate(DFServiceEntryPoint.DfdllHandle, "RegisterDeploymentServiceCom", typeof(DFServiceEntryPoint.RegisterDeploymentServiceComDelegate));
			DFServiceEntryPoint.UnregisterDeploymentServiceCom = (DFServiceEntryPoint.UnregisterDeploymentServiceComDelegate)DFServiceEntryPoint.GetMethodDelegate(DFServiceEntryPoint.DfdllHandle, "UnregisterDeploymentServiceCom", typeof(DFServiceEntryPoint.UnregisterDeploymentServiceComDelegate));
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001E01C File Offset: 0x0001D01C
		public static void Initialize(string[] args)
		{
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfNewTaskBegin);
			if (PlatformSpecific.OnWin9x)
			{
				Thread thread = new Thread(new ThreadStart(DFServiceEntryPoint.MessageLoopThread));
				thread.Start();
			}
			DFServiceEntryPoint.ObtainDfdllExports();
			DFServiceEntryPoint.s_createDeploymentServiceComDelegate = () => new DeploymentServiceComWrapper();
			int num = DFServiceEntryPoint.RegisterDeploymentServiceCom(DFServiceEntryPoint.s_createDeploymentServiceComDelegate);
			if (num < 0)
			{
				throw Marshal.GetExceptionForHR(num);
			}
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfNewTaskEnd);
			bool flag = LifetimeManager.WaitForEnd();
			if (DFServiceEntryPoint._dfsvcForm != null)
			{
				DFServiceEntryPoint._dfsvcForm.Invoke(new DFServiceEntryPoint.DfsvcForm.CloseFormDelegate(DFServiceEntryPoint._dfsvcForm.CloseForm), new object[] { true });
			}
			DFServiceEntryPoint.UnregisterDeploymentServiceCom();
			if (!flag && PlatformSpecific.OnWin9x)
			{
				Thread.Sleep(5000);
			}
			CodeMarker_Singleton.Instance.UninitializePerformanceDLL(CodeMarkerApp.CLICKONCEPERF);
			Environment.Exit(0);
		}

		// Token: 0x040004A4 RID: 1188
		private static DFServiceEntryPoint.CreateDeploymentServiceComDelegate s_createDeploymentServiceComDelegate;

		// Token: 0x040004A5 RID: 1189
		private static DFServiceEntryPoint.RegisterDeploymentServiceComDelegate RegisterDeploymentServiceCom;

		// Token: 0x040004A6 RID: 1190
		private static DFServiceEntryPoint.UnregisterDeploymentServiceComDelegate UnregisterDeploymentServiceCom;

		// Token: 0x040004A7 RID: 1191
		private static IntPtr DfdllHandle;

		// Token: 0x040004A8 RID: 1192
		private static DFServiceEntryPoint.DfsvcForm _dfsvcForm;

		// Token: 0x040004A9 RID: 1193
		[CompilerGenerated]
		private static DFServiceEntryPoint.CreateDeploymentServiceComDelegate <>9__CachedAnonymousMethodDelegate1;

		// Token: 0x020000DD RID: 221
		// (Invoke) Token: 0x060005BC RID: 1468
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IManagedDeploymentServiceCom CreateDeploymentServiceComDelegate();

		// Token: 0x020000DE RID: 222
		// (Invoke) Token: 0x060005C0 RID: 1472
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int RegisterDeploymentServiceComDelegate([MarshalAs(UnmanagedType.FunctionPtr)] DFServiceEntryPoint.CreateDeploymentServiceComDelegate createDeploymentServiceComDelegate);

		// Token: 0x020000DF RID: 223
		// (Invoke) Token: 0x060005C4 RID: 1476
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int UnregisterDeploymentServiceComDelegate();

		// Token: 0x020000E0 RID: 224
		private class DfsvcForm : Form
		{
			// Token: 0x060005C7 RID: 1479 RVA: 0x0001E112 File Offset: 0x0001D112
			public DfsvcForm()
			{
				this.InitializeComponent();
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x0001E120 File Offset: 0x0001D120
			protected override void Dispose(bool disposing)
			{
				if (disposing && this.components != null)
				{
					this.components.Dispose();
				}
				base.Dispose(disposing);
			}

			// Token: 0x060005C9 RID: 1481 RVA: 0x0001E140 File Offset: 0x0001D140
			private void InitializeComponent()
			{
				base.ClientSize = new Size(292, 266);
				base.ShowInTaskbar = false;
				base.WindowState = FormWindowState.Minimized;
				base.TopMost = true;
				base.Closing += this.DfsvcForm_Closing;
				base.Closed += this.DfsvcForm_Closed;
			}

			// Token: 0x060005CA RID: 1482 RVA: 0x0001E19B File Offset: 0x0001D19B
			private void DfsvcForm_Closing(object sender, CancelEventArgs e)
			{
				e.Cancel = false;
				this.TerminateLifetimeManager(true);
			}

			// Token: 0x060005CB RID: 1483 RVA: 0x0001E1AB File Offset: 0x0001D1AB
			private void DfsvcForm_Closed(object sender, EventArgs e)
			{
				this.TerminateLifetimeManager(true);
			}

			// Token: 0x060005CC RID: 1484 RVA: 0x0001E1B4 File Offset: 0x0001D1B4
			public void SessionEndedEventHandler(object sender, SessionEndedEventArgs e)
			{
				this.TerminateLifetimeManager(false);
			}

			// Token: 0x060005CD RID: 1485 RVA: 0x0001E1BD File Offset: 0x0001D1BD
			public void SessionEndingEventHandler(object sender, SessionEndingEventArgs e)
			{
				e.Cancel = false;
				this.TerminateLifetimeManager(false);
			}

			// Token: 0x060005CE RID: 1486 RVA: 0x0001E1D0 File Offset: 0x0001D1D0
			public void CloseForm(bool lifetimeManagerAlreadyTerminated)
			{
				if (!this._formClosed)
				{
					lock (this)
					{
						if (lifetimeManagerAlreadyTerminated)
						{
							this._lifetimeManagerTerminated = true;
						}
						if (!this._formClosed)
						{
							this._formClosed = true;
							base.Close();
						}
					}
				}
			}

			// Token: 0x060005CF RID: 1487 RVA: 0x0001E228 File Offset: 0x0001D228
			private void TerminateLifetimeManager(bool formAlreadyClosed)
			{
				if (!this._lifetimeManagerTerminated)
				{
					lock (this)
					{
						if (formAlreadyClosed)
						{
							this._formClosed = true;
						}
						if (!this._lifetimeManagerTerminated)
						{
							this._lifetimeManagerTerminated = true;
							LifetimeManager.EndImmediately();
						}
					}
				}
			}

			// Token: 0x040004AA RID: 1194
			private Container components;

			// Token: 0x040004AB RID: 1195
			private bool _lifetimeManagerTerminated;

			// Token: 0x040004AC RID: 1196
			private bool _formClosed;

			// Token: 0x020000E1 RID: 225
			// (Invoke) Token: 0x060005D1 RID: 1489
			public delegate void CloseFormDelegate(bool lifetimeManagerAlreadyTerminated);
		}
	}
}

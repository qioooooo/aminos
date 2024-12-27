using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000088 RID: 136
	internal class WmiGetEventSink : WmiEventSink
	{
		// Token: 0x060003D4 RID: 980 RVA: 0x00010DAC File Offset: 0x0000FDAC
		internal static WmiGetEventSink GetWmiGetEventSink(ManagementOperationObserver watcher, object context, ManagementScope scope, ManagementObject managementObject)
		{
			if (MTAHelper.IsNoContextMTA())
			{
				return new WmiGetEventSink(watcher, context, scope, managementObject);
			}
			WmiGetEventSink.watcherParameter = watcher;
			WmiGetEventSink.contextParameter = context;
			WmiGetEventSink.scopeParameter = scope;
			WmiGetEventSink.managementObjectParameter = managementObject;
			ThreadDispatch threadDispatch = new ThreadDispatch(new ThreadDispatch.ThreadWorkerMethod(WmiGetEventSink.HackToCreateWmiGetEventSink));
			threadDispatch.Start();
			return WmiGetEventSink.wmiGetEventSinkNew;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00010DFF File Offset: 0x0000FDFF
		private static void HackToCreateWmiGetEventSink()
		{
			WmiGetEventSink.wmiGetEventSinkNew = new WmiGetEventSink(WmiGetEventSink.watcherParameter, WmiGetEventSink.contextParameter, WmiGetEventSink.scopeParameter, WmiGetEventSink.managementObjectParameter);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00010E1F File Offset: 0x0000FE1F
		private WmiGetEventSink(ManagementOperationObserver watcher, object context, ManagementScope scope, ManagementObject managementObject)
			: base(watcher, context, scope, null, null)
		{
			this.managementObject = managementObject;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00010E34 File Offset: 0x0000FE34
		public override void Indicate(IntPtr pIWbemClassObject)
		{
			Marshal.AddRef(pIWbemClassObject);
			IWbemClassObjectFreeThreaded wbemClassObjectFreeThreaded = new IWbemClassObjectFreeThreaded(pIWbemClassObject);
			if (this.managementObject != null)
			{
				try
				{
					this.managementObject.wbemObject = wbemClassObjectFreeThreaded;
				}
				catch
				{
				}
			}
		}

		// Token: 0x04000202 RID: 514
		private ManagementObject managementObject;

		// Token: 0x04000203 RID: 515
		private static ManagementOperationObserver watcherParameter;

		// Token: 0x04000204 RID: 516
		private static object contextParameter;

		// Token: 0x04000205 RID: 517
		private static ManagementScope scopeParameter;

		// Token: 0x04000206 RID: 518
		private static ManagementObject managementObjectParameter;

		// Token: 0x04000207 RID: 519
		private static WmiGetEventSink wmiGetEventSinkNew;
	}
}

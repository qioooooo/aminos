using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002A6 RID: 678
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class TimerElapsedEventArgs : EventArgs
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x0004822E File Offset: 0x0004722E
		public TimerElapsedEventArgs(IntPtr timerId)
		{
			this.timerId = timerId;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x0004823D File Offset: 0x0004723D
		public IntPtr TimerId
		{
			get
			{
				return this.timerId;
			}
		}

		// Token: 0x040015EE RID: 5614
		private readonly IntPtr timerId;
	}
}

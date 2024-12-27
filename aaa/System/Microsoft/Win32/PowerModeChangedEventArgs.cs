using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000297 RID: 663
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class PowerModeChangedEventArgs : EventArgs
	{
		// Token: 0x06001609 RID: 5641 RVA: 0x0004644A File Offset: 0x0004544A
		public PowerModeChangedEventArgs(PowerModes mode)
		{
			this.mode = mode;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x00046459 File Offset: 0x00045459
		public PowerModes Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x040015A0 RID: 5536
		private readonly PowerModes mode;
	}
}

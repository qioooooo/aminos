using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x020002A1 RID: 673
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SessionSwitchEventArgs : EventArgs
	{
		// Token: 0x06001631 RID: 5681 RVA: 0x000464CE File Offset: 0x000454CE
		public SessionSwitchEventArgs(SessionSwitchReason reason)
		{
			this.reason = reason;
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001632 RID: 5682 RVA: 0x000464DD File Offset: 0x000454DD
		public SessionSwitchReason Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x040015B8 RID: 5560
		private readonly SessionSwitchReason reason;
	}
}

using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200029C RID: 668
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SessionEndedEventArgs : EventArgs
	{
		// Token: 0x06001623 RID: 5667 RVA: 0x0004648F File Offset: 0x0004548F
		public SessionEndedEventArgs(SessionEndReasons reason)
		{
			this.reason = reason;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001624 RID: 5668 RVA: 0x0004649E File Offset: 0x0004549E
		public SessionEndReasons Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x040015B2 RID: 5554
		private readonly SessionEndReasons reason;
	}
}

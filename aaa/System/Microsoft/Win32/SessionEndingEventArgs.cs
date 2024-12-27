using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x0200029E RID: 670
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class SessionEndingEventArgs : EventArgs
	{
		// Token: 0x06001629 RID: 5673 RVA: 0x000464A6 File Offset: 0x000454A6
		public SessionEndingEventArgs(SessionEndReasons reason)
		{
			this.reason = reason;
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600162A RID: 5674 RVA: 0x000464B5 File Offset: 0x000454B5
		// (set) Token: 0x0600162B RID: 5675 RVA: 0x000464BD File Offset: 0x000454BD
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600162C RID: 5676 RVA: 0x000464C6 File Offset: 0x000454C6
		public SessionEndReasons Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x040015B3 RID: 5555
		private bool cancel;

		// Token: 0x040015B4 RID: 5556
		private readonly SessionEndReasons reason;
	}
}

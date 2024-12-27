using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x0200015E RID: 350
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class BuildManagerHostUnloadEventArgs : EventArgs
	{
		// Token: 0x06000FF1 RID: 4081 RVA: 0x00046C49 File Offset: 0x00045C49
		public BuildManagerHostUnloadEventArgs(ApplicationShutdownReason reason)
		{
			this._reason = reason;
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00046C58 File Offset: 0x00045C58
		public ApplicationShutdownReason Reason
		{
			get
			{
				return this._reason;
			}
		}

		// Token: 0x04001625 RID: 5669
		private ApplicationShutdownReason _reason;
	}
}

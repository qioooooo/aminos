using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005D4 RID: 1492
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LoginCancelEventArgs : EventArgs
	{
		// Token: 0x0600492A RID: 18730 RVA: 0x0012A6EB File Offset: 0x001296EB
		public LoginCancelEventArgs()
			: this(false)
		{
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x0012A6F4 File Offset: 0x001296F4
		public LoginCancelEventArgs(bool cancel)
		{
			this._cancel = cancel;
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x0600492C RID: 18732 RVA: 0x0012A703 File Offset: 0x00129703
		// (set) Token: 0x0600492D RID: 18733 RVA: 0x0012A70B File Offset: 0x0012970B
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		// Token: 0x04002B19 RID: 11033
		private bool _cancel;
	}
}

using System;
using System.Net.Mail;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005DD RID: 1501
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class MailMessageEventArgs : LoginCancelEventArgs
	{
		// Token: 0x0600498E RID: 18830 RVA: 0x0012B8C4 File Offset: 0x0012A8C4
		public MailMessageEventArgs(MailMessage message)
		{
			this._message = message;
		}

		// Token: 0x1700123F RID: 4671
		// (get) Token: 0x0600498F RID: 18831 RVA: 0x0012B8D3 File Offset: 0x0012A8D3
		public MailMessage Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x04002B39 RID: 11065
		private MailMessage _message;
	}
}

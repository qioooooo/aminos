using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000720 RID: 1824
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartDisplayModeCancelEventArgs : CancelEventArgs
	{
		// Token: 0x0600587D RID: 22653 RVA: 0x00163C34 File Offset: 0x00162C34
		public WebPartDisplayModeCancelEventArgs(WebPartDisplayMode newDisplayMode)
		{
			this._newDisplayMode = newDisplayMode;
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x0600587E RID: 22654 RVA: 0x00163C43 File Offset: 0x00162C43
		// (set) Token: 0x0600587F RID: 22655 RVA: 0x00163C4B File Offset: 0x00162C4B
		public WebPartDisplayMode NewDisplayMode
		{
			get
			{
				return this._newDisplayMode;
			}
			set
			{
				this._newDisplayMode = value;
			}
		}

		// Token: 0x04002FE8 RID: 12264
		private WebPartDisplayMode _newDisplayMode;
	}
}

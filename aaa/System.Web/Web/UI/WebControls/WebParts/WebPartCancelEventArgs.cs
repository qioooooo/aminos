using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000705 RID: 1797
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartCancelEventArgs : CancelEventArgs
	{
		// Token: 0x060057A8 RID: 22440 RVA: 0x00161405 File Offset: 0x00160405
		public WebPartCancelEventArgs(WebPart webPart)
		{
			this._webPart = webPart;
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x060057A9 RID: 22441 RVA: 0x00161414 File Offset: 0x00160414
		// (set) Token: 0x060057AA RID: 22442 RVA: 0x0016141C File Offset: 0x0016041C
		public WebPart WebPart
		{
			get
			{
				return this._webPart;
			}
			set
			{
				this._webPart = value;
			}
		}

		// Token: 0x04002FAD RID: 12205
		private WebPart _webPart;
	}
}

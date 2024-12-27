using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A2 RID: 1186
	[ValidationProperty("Value")]
	[SupportsEventValidation]
	[DefaultEvent("ServerChange")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputPassword : HtmlInputText, IPostBackDataHandler
	{
		// Token: 0x06003772 RID: 14194 RVA: 0x000EDF49 File Offset: 0x000ECF49
		public HtmlInputPassword()
			: base("password")
		{
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000EDF56 File Offset: 0x000ECF56
		protected override void RenderAttributes(HtmlTextWriter writer)
		{
			this.ViewState.Remove("value");
			base.RenderAttributes(writer);
		}

		// Token: 0x040025CA RID: 9674
		private static readonly object EventServerChange = new object();
	}
}

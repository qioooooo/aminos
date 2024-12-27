using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004A5 RID: 1189
	[SupportsEventValidation]
	[DefaultEvent("ServerClick")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlInputSubmit : HtmlInputButton, IPostBackEventHandler
	{
		// Token: 0x06003790 RID: 14224 RVA: 0x000EE28F File Offset: 0x000ED28F
		public HtmlInputSubmit()
			: base("submit")
		{
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x000EE29C File Offset: 0x000ED29C
		public HtmlInputSubmit(string type)
			: base(type)
		{
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x000EE2A5 File Offset: 0x000ED2A5
		internal override void RenderAttributesInternal(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				Util.WriteOnClickAttribute(writer, this, true, false, this.CausesValidation && this.Page.GetValidators(this.ValidationGroup).Count > 0, this.ValidationGroup);
			}
		}
	}
}

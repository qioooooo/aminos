using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200070C RID: 1804
	internal sealed class WebPartCatalogCloseVerb : WebPartActionVerb
	{
		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x060057C8 RID: 22472 RVA: 0x00161554 File Offset: 0x00160554
		// (set) Token: 0x060057C9 RID: 22473 RVA: 0x00161586 File Offset: 0x00160586
		[WebSysDefaultValue("WebPartCatalogCloseVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartCatalogCloseVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x060057CA RID: 22474 RVA: 0x0016159C File Offset: 0x0016059C
		// (set) Token: 0x060057CB RID: 22475 RVA: 0x001615CE File Offset: 0x001605CE
		[WebSysDefaultValue("WebPartCatalogCloseVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartCatalogCloseVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

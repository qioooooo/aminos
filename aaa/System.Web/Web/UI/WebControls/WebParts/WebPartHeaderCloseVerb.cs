using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200072D RID: 1837
	internal sealed class WebPartHeaderCloseVerb : WebPartActionVerb
	{
		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x060058BF RID: 22719 RVA: 0x00164278 File Offset: 0x00163278
		// (set) Token: 0x060058C0 RID: 22720 RVA: 0x001642AA File Offset: 0x001632AA
		[WebSysDefaultValue("WebPartHeaderCloseVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartHeaderCloseVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x060058C1 RID: 22721 RVA: 0x001642C0 File Offset: 0x001632C0
		// (set) Token: 0x060058C2 RID: 22722 RVA: 0x001642F2 File Offset: 0x001632F2
		[WebSysDefaultValue("WebPartHeaderCloseVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartHeaderCloseVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

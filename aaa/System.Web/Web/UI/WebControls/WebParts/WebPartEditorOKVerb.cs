using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000727 RID: 1831
	internal sealed class WebPartEditorOKVerb : WebPartActionVerb
	{
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x060058A6 RID: 22694 RVA: 0x0016402C File Offset: 0x0016302C
		// (set) Token: 0x060058A7 RID: 22695 RVA: 0x0016405E File Offset: 0x0016305E
		[WebSysDefaultValue("WebPartEditorOKVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartEditorOKVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x060058A8 RID: 22696 RVA: 0x00164074 File Offset: 0x00163074
		// (set) Token: 0x060058A9 RID: 22697 RVA: 0x001640A6 File Offset: 0x001630A6
		[WebSysDefaultValue("WebPartEditorOKVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartEditorOKVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

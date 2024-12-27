using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000726 RID: 1830
	internal sealed class WebPartEditorCancelVerb : WebPartActionVerb
	{
		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x060058A1 RID: 22689 RVA: 0x00163F94 File Offset: 0x00162F94
		// (set) Token: 0x060058A2 RID: 22690 RVA: 0x00163FC6 File Offset: 0x00162FC6
		[WebSysDefaultValue("WebPartEditorCancelVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartEditorCancelVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x060058A3 RID: 22691 RVA: 0x00163FDC File Offset: 0x00162FDC
		// (set) Token: 0x060058A4 RID: 22692 RVA: 0x0016400E File Offset: 0x0016300E
		[WebSysDefaultValue("WebPartEditorCancelVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartEditorCancelVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

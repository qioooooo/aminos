using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000725 RID: 1829
	internal sealed class WebPartEditorApplyVerb : WebPartActionVerb
	{
		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x0600589C RID: 22684 RVA: 0x00163EFC File Offset: 0x00162EFC
		// (set) Token: 0x0600589D RID: 22685 RVA: 0x00163F2E File Offset: 0x00162F2E
		[WebSysDefaultValue("WebPartEditorApplyVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartEditorApplyVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x0600589E RID: 22686 RVA: 0x00163F44 File Offset: 0x00162F44
		// (set) Token: 0x0600589F RID: 22687 RVA: 0x00163F76 File Offset: 0x00162F76
		[WebSysDefaultValue("WebPartEditorApplyVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartEditorApplyVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

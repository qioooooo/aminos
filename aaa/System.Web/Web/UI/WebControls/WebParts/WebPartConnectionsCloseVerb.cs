using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000715 RID: 1813
	internal sealed class WebPartConnectionsCloseVerb : WebPartActionVerb
	{
		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x0600583A RID: 22586 RVA: 0x001634D8 File Offset: 0x001624D8
		// (set) Token: 0x0600583B RID: 22587 RVA: 0x0016350A File Offset: 0x0016250A
		[WebSysDefaultValue("WebPartConnectionsCloseVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsCloseVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x0600583C RID: 22588 RVA: 0x00163520 File Offset: 0x00162520
		// (set) Token: 0x0600583D RID: 22589 RVA: 0x00163552 File Offset: 0x00162552
		[WebSysDefaultValue("WebPartConnectionsCloseVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsCloseVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

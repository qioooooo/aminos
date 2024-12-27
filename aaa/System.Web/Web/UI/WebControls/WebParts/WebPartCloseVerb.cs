using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200070E RID: 1806
	internal sealed class WebPartCloseVerb : WebPartActionVerb
	{
		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x0016282D File Offset: 0x0016182D
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartCloseVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x060057EA RID: 22506 RVA: 0x0016284D File Offset: 0x0016184D
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartCloseVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x00162870 File Offset: 0x00161870
		// (set) Token: 0x060057EC RID: 22508 RVA: 0x0016289E File Offset: 0x0016189E
		[WebSysDefaultValue("WebPartCloseVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return this.DefaultDescription;
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x060057ED RID: 22509 RVA: 0x001628B4 File Offset: 0x001618B4
		// (set) Token: 0x060057EE RID: 22510 RVA: 0x001628E2 File Offset: 0x001618E2
		[WebSysDefaultValue("WebPartCloseVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return this.DefaultText;
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}

		// Token: 0x04002FC3 RID: 12227
		private string _defaultDescription;

		// Token: 0x04002FC4 RID: 12228
		private string _defaultText;
	}
}

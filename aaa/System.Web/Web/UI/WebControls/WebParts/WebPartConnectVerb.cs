using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200071B RID: 1819
	internal sealed class WebPartConnectVerb : WebPartActionVerb
	{
		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x06005859 RID: 22617 RVA: 0x00163797 File Offset: 0x00162797
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartConnectVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x0600585A RID: 22618 RVA: 0x001637B7 File Offset: 0x001627B7
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartConnectVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x0600585B RID: 22619 RVA: 0x001637D8 File Offset: 0x001627D8
		// (set) Token: 0x0600585C RID: 22620 RVA: 0x00163806 File Offset: 0x00162806
		[WebSysDefaultValue("WebPartConnectVerb_Description")]
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

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x0600585D RID: 22621 RVA: 0x0016381C File Offset: 0x0016281C
		// (set) Token: 0x0600585E RID: 22622 RVA: 0x0016384A File Offset: 0x0016284A
		[WebSysDefaultValue("WebPartConnectVerb_Text")]
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

		// Token: 0x04002FDD RID: 12253
		private string _defaultDescription;

		// Token: 0x04002FDE RID: 12254
		private string _defaultText;
	}
}

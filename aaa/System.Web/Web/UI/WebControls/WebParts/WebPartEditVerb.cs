using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000728 RID: 1832
	internal sealed class WebPartEditVerb : WebPartActionVerb
	{
		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x060058AB RID: 22699 RVA: 0x001640C1 File Offset: 0x001630C1
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartEditVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x060058AC RID: 22700 RVA: 0x001640E1 File Offset: 0x001630E1
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartEditVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x060058AD RID: 22701 RVA: 0x00164104 File Offset: 0x00163104
		// (set) Token: 0x060058AE RID: 22702 RVA: 0x00164132 File Offset: 0x00163132
		[WebSysDefaultValue("WebPartEditVerb_Description")]
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

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x060058AF RID: 22703 RVA: 0x00164148 File Offset: 0x00163148
		// (set) Token: 0x060058B0 RID: 22704 RVA: 0x00164176 File Offset: 0x00163176
		[WebSysDefaultValue("WebPartEditVerb_Text")]
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

		// Token: 0x04002FEC RID: 12268
		private string _defaultDescription;

		// Token: 0x04002FED RID: 12269
		private string _defaultText;
	}
}

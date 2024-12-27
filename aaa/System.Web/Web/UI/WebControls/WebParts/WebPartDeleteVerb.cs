using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200071C RID: 1820
	internal sealed class WebPartDeleteVerb : WebPartActionVerb
	{
		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06005860 RID: 22624 RVA: 0x00163865 File Offset: 0x00162865
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartDeleteVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06005861 RID: 22625 RVA: 0x00163885 File Offset: 0x00162885
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartDeleteVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06005862 RID: 22626 RVA: 0x001638A8 File Offset: 0x001628A8
		// (set) Token: 0x06005863 RID: 22627 RVA: 0x001638D6 File Offset: 0x001628D6
		[WebSysDefaultValue("WebPartDeleteVerb_Description")]
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

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x06005864 RID: 22628 RVA: 0x001638EC File Offset: 0x001628EC
		// (set) Token: 0x06005865 RID: 22629 RVA: 0x0016391A File Offset: 0x0016291A
		[WebSysDefaultValue("WebPartDeleteVerb_Text")]
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

		// Token: 0x04002FDF RID: 12255
		private string _defaultDescription;

		// Token: 0x04002FE0 RID: 12256
		private string _defaultText;
	}
}

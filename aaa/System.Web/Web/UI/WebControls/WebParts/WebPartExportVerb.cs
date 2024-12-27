using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200072C RID: 1836
	internal sealed class WebPartExportVerb : WebPartActionVerb
	{
		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x060058B8 RID: 22712 RVA: 0x001641A8 File Offset: 0x001631A8
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartExportVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x060058B9 RID: 22713 RVA: 0x001641C8 File Offset: 0x001631C8
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartExportVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x060058BA RID: 22714 RVA: 0x001641E8 File Offset: 0x001631E8
		// (set) Token: 0x060058BB RID: 22715 RVA: 0x00164216 File Offset: 0x00163216
		[WebSysDefaultValue("WebPartExportVerb_Description")]
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

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x060058BC RID: 22716 RVA: 0x0016422C File Offset: 0x0016322C
		// (set) Token: 0x060058BD RID: 22717 RVA: 0x0016425A File Offset: 0x0016325A
		[WebSysDefaultValue("WebPartExportVerb_Text")]
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

		// Token: 0x04002FF3 RID: 12275
		private string _defaultDescription;

		// Token: 0x04002FF4 RID: 12276
		private string _defaultText;
	}
}

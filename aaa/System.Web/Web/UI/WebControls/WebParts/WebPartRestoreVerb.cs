using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000740 RID: 1856
	internal sealed class WebPartRestoreVerb : WebPartActionVerb
	{
		// Token: 0x17001747 RID: 5959
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x0016BF71 File Offset: 0x0016AF71
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartRestoreVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x17001748 RID: 5960
		// (get) Token: 0x06005A1E RID: 23070 RVA: 0x0016BF91 File Offset: 0x0016AF91
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartRestoreVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x06005A1F RID: 23071 RVA: 0x0016BFB4 File Offset: 0x0016AFB4
		// (set) Token: 0x06005A20 RID: 23072 RVA: 0x0016BFE2 File Offset: 0x0016AFE2
		[WebSysDefaultValue("WebPartRestoreVerb_Description")]
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

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x06005A21 RID: 23073 RVA: 0x0016BFF8 File Offset: 0x0016AFF8
		// (set) Token: 0x06005A22 RID: 23074 RVA: 0x0016C026 File Offset: 0x0016B026
		[WebSysDefaultValue("WebPartRestoreVerb_Text")]
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

		// Token: 0x0400307B RID: 12411
		private string _defaultDescription;

		// Token: 0x0400307C RID: 12412
		private string _defaultText;
	}
}

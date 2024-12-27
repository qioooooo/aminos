using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200073B RID: 1851
	internal sealed class WebPartMinimizeVerb : WebPartActionVerb
	{
		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x060059E3 RID: 23011 RVA: 0x0016B3BC File Offset: 0x0016A3BC
		private string DefaultDescription
		{
			get
			{
				if (this._defaultDescription == null)
				{
					this._defaultDescription = SR.GetString("WebPartMinimizeVerb_Description");
				}
				return this._defaultDescription;
			}
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x060059E4 RID: 23012 RVA: 0x0016B3DC File Offset: 0x0016A3DC
		private string DefaultText
		{
			get
			{
				if (this._defaultText == null)
				{
					this._defaultText = SR.GetString("WebPartMinimizeVerb_Text");
				}
				return this._defaultText;
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x060059E5 RID: 23013 RVA: 0x0016B3FC File Offset: 0x0016A3FC
		// (set) Token: 0x060059E6 RID: 23014 RVA: 0x0016B42A File Offset: 0x0016A42A
		[WebSysDefaultValue("WebPartMinimizeVerb_Description")]
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

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x060059E7 RID: 23015 RVA: 0x0016B440 File Offset: 0x0016A440
		// (set) Token: 0x060059E8 RID: 23016 RVA: 0x0016B46E File Offset: 0x0016A46E
		[WebSysDefaultValue("WebPartMinimizeVerb_Text")]
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

		// Token: 0x04003067 RID: 12391
		private string _defaultDescription;

		// Token: 0x04003068 RID: 12392
		private string _defaultText;
	}
}

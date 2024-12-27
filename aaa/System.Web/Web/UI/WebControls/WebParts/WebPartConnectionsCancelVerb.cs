using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000714 RID: 1812
	internal sealed class WebPartConnectionsCancelVerb : WebPartActionVerb
	{
		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x06005835 RID: 22581 RVA: 0x00163440 File Offset: 0x00162440
		// (set) Token: 0x06005836 RID: 22582 RVA: 0x00163472 File Offset: 0x00162472
		[WebSysDefaultValue("WebPartConnectionsCancelVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsCancelVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x06005837 RID: 22583 RVA: 0x00163488 File Offset: 0x00162488
		// (set) Token: 0x06005838 RID: 22584 RVA: 0x001634BA File Offset: 0x001624BA
		[WebSysDefaultValue("WebPartConnectionsCancelVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsCancelVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

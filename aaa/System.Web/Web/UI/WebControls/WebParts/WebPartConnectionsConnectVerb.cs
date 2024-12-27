using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000717 RID: 1815
	internal sealed class WebPartConnectionsConnectVerb : WebPartActionVerb
	{
		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06005844 RID: 22596 RVA: 0x00163608 File Offset: 0x00162608
		// (set) Token: 0x06005845 RID: 22597 RVA: 0x0016363A File Offset: 0x0016263A
		[WebSysDefaultValue("WebPartConnectionsConnectVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsConnectVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x06005846 RID: 22598 RVA: 0x00163650 File Offset: 0x00162650
		// (set) Token: 0x06005847 RID: 22599 RVA: 0x00163682 File Offset: 0x00162682
		[WebSysDefaultValue("WebPartConnectionsConnectVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsConnectVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

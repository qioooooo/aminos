using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000718 RID: 1816
	internal sealed class WebPartConnectionsDisconnectVerb : WebPartActionVerb
	{
		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x06005849 RID: 22601 RVA: 0x001636A0 File Offset: 0x001626A0
		// (set) Token: 0x0600584A RID: 22602 RVA: 0x001636D2 File Offset: 0x001626D2
		[WebSysDefaultValue("WebPartConnectionsDisconnectVerb_Description")]
		public override string Description
		{
			get
			{
				object obj = base.ViewState["Description"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsDisconnectVerb_Description");
			}
			set
			{
				base.ViewState["Description"] = value;
			}
		}

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x0600584B RID: 22603 RVA: 0x001636E8 File Offset: 0x001626E8
		// (set) Token: 0x0600584C RID: 22604 RVA: 0x0016371A File Offset: 0x0016271A
		[WebSysDefaultValue("WebPartConnectionsDisconnectVerb_Text")]
		public override string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return SR.GetString("WebPartConnectionsDisconnectVerb_Text");
			}
			set
			{
				base.ViewState["Text"] = value;
			}
		}
	}
}

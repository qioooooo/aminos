using System;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006CF RID: 1743
	internal interface IWebPartMenuUser
	{
		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x060055A5 RID: 21925
		Style CheckImageStyle { get; }

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x060055A6 RID: 21926
		string CheckImageUrl { get; }

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x060055A7 RID: 21927
		string ClientID { get; }

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x060055A8 RID: 21928
		Style ItemHoverStyle { get; }

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x060055A9 RID: 21929
		Style ItemStyle { get; }

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x060055AA RID: 21930
		Style LabelHoverStyle { get; }

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x060055AB RID: 21931
		string LabelImageUrl { get; }

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x060055AC RID: 21932
		Style LabelStyle { get; }

		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x060055AD RID: 21933
		string LabelText { get; }

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x060055AE RID: 21934
		WebPartMenuStyle MenuPopupStyle { get; }

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x060055AF RID: 21935
		Page Page { get; }

		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x060055B0 RID: 21936
		string PopupImageUrl { get; }

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x060055B1 RID: 21937
		string PostBackTarget { get; }

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x060055B2 RID: 21938
		IUrlResolutionService UrlResolver { get; }

		// Token: 0x060055B3 RID: 21939
		void OnBeginRender(HtmlTextWriter writer);

		// Token: 0x060055B4 RID: 21940
		void OnEndRender(HtmlTextWriter writer);
	}
}

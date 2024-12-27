using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006BF RID: 1727
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebPart
	{
		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x060054E1 RID: 21729
		// (set) Token: 0x060054E2 RID: 21730
		string CatalogIconImageUrl { get; set; }

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x060054E3 RID: 21731
		// (set) Token: 0x060054E4 RID: 21732
		string Description { get; set; }

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x060054E5 RID: 21733
		string Subtitle { get; }

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x060054E6 RID: 21734
		// (set) Token: 0x060054E7 RID: 21735
		string Title { get; set; }

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x060054E8 RID: 21736
		// (set) Token: 0x060054E9 RID: 21737
		string TitleIconImageUrl { get; set; }

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x060054EA RID: 21738
		// (set) Token: 0x060054EB RID: 21739
		string TitleUrl { get; set; }
	}
}

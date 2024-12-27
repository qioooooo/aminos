using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004F1 RID: 1265
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IRepeatInfoUser
	{
		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06003DA6 RID: 15782
		bool HasHeader { get; }

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06003DA7 RID: 15783
		bool HasFooter { get; }

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06003DA8 RID: 15784
		bool HasSeparators { get; }

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06003DA9 RID: 15785
		int RepeatedItemCount { get; }

		// Token: 0x06003DAA RID: 15786
		Style GetItemStyle(ListItemType itemType, int repeatIndex);

		// Token: 0x06003DAB RID: 15787
		void RenderItem(ListItemType itemType, int repeatIndex, RepeatInfo repeatInfo, HtmlTextWriter writer);
	}
}

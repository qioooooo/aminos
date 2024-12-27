using System;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Web.UI.Design
{
	// Token: 0x02000375 RID: 885
	public interface IControlDesignerView
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002110 RID: 8464
		DesignerRegion ContainingRegion { get; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002111 RID: 8465
		IDesigner NamingContainerDesigner { get; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002112 RID: 8466
		bool SupportsRegions { get; }

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06002113 RID: 8467
		// (remove) Token: 0x06002114 RID: 8468
		event ViewEventHandler ViewEvent;

		// Token: 0x06002115 RID: 8469
		Rectangle GetBounds(DesignerRegion region);

		// Token: 0x06002116 RID: 8470
		void Invalidate(Rectangle rectangle);

		// Token: 0x06002117 RID: 8471
		void SetFlags(ViewFlags viewFlags, bool setFlag);

		// Token: 0x06002118 RID: 8472
		void SetRegionContent(EditableDesignerRegion region, string content);

		// Token: 0x06002119 RID: 8473
		void Update();
	}
}

using System;

namespace System.Windows.Forms
{
	// Token: 0x02000244 RID: 580
	internal interface ISupportToolStripPanel
	{
		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001BB4 RID: 7092
		// (set) Token: 0x06001BB5 RID: 7093
		ToolStripPanelRow ToolStripPanelRow { get; set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001BB6 RID: 7094
		ToolStripPanelCell ToolStripPanelCell { get; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001BB7 RID: 7095
		// (set) Token: 0x06001BB8 RID: 7096
		bool Stretch { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001BB9 RID: 7097
		bool IsCurrentlyDragging { get; }

		// Token: 0x06001BBA RID: 7098
		void BeginDrag();

		// Token: 0x06001BBB RID: 7099
		void EndDrag();
	}
}

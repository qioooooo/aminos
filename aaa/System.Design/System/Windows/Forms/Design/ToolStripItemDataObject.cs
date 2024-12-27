using System;
using System.Collections;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002C6 RID: 710
	internal class ToolStripItemDataObject : DataObject
	{
		// Token: 0x06001AE7 RID: 6887 RVA: 0x00093D55 File Offset: 0x00092D55
		internal ToolStripItemDataObject(ArrayList dragComponents, ToolStripItem primarySelection, ToolStrip owner)
		{
			this.dragComponents = dragComponents;
			this.owner = owner;
			this.primarySelection = primarySelection;
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x00093D72 File Offset: 0x00092D72
		internal ArrayList DragComponents
		{
			get
			{
				return this.dragComponents;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x00093D7A File Offset: 0x00092D7A
		internal ToolStrip Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x00093D82 File Offset: 0x00092D82
		internal ToolStripItem PrimarySelection
		{
			get
			{
				return this.primarySelection;
			}
		}

		// Token: 0x0400154C RID: 5452
		private ArrayList dragComponents;

		// Token: 0x0400154D RID: 5453
		private ToolStrip owner;

		// Token: 0x0400154E RID: 5454
		private ToolStripItem primarySelection;
	}
}

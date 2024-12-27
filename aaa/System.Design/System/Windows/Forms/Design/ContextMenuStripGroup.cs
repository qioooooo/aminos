using System;
using System.Collections.Generic;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001CE RID: 462
	internal class ContextMenuStripGroup
	{
		// Token: 0x060011F6 RID: 4598 RVA: 0x000573C9 File Offset: 0x000563C9
		public ContextMenuStripGroup(string name)
		{
			this.name = name;
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x000573D8 File Offset: 0x000563D8
		public List<ToolStripItem> Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new List<ToolStripItem>();
				}
				return this.items;
			}
		}

		// Token: 0x040010F7 RID: 4343
		private List<ToolStripItem> items;

		// Token: 0x040010F8 RID: 4344
		private string name;
	}
}

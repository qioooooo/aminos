using System;
using System.Collections.Generic;

namespace System.Windows.Forms.Design
{
	internal class ContextMenuStripGroup
	{
		public ContextMenuStripGroup(string name)
		{
			this.name = name;
		}

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

		private List<ToolStripItem> items;

		private string name;
	}
}

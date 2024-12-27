using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000277 RID: 631
	internal class NewItemsContextMenuStrip : GroupedContextMenuStrip
	{
		// Token: 0x060017AC RID: 6060 RVA: 0x0007B204 File Offset: 0x0007A204
		public NewItemsContextMenuStrip(IComponent component, ToolStripItem currentItem, EventHandler onClick, bool convertTo, IServiceProvider serviceProvider)
		{
			this.component = component;
			this.onClick = onClick;
			this.convertTo = convertTo;
			this.serviceProvider = serviceProvider;
			this.currentItem = currentItem;
			IUIService iuiservice = serviceProvider.GetService(typeof(IUIService)) as IUIService;
			if (iuiservice != null)
			{
				base.Renderer = (ToolStripProfessionalRenderer)iuiservice.Styles["VsRenderer"];
			}
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0007B274 File Offset: 0x0007A274
		protected override void OnOpening(CancelEventArgs e)
		{
			base.Groups["StandardList"].Items.Clear();
			base.Groups["CustomList"].Items.Clear();
			base.Populated = false;
			foreach (ToolStripItem toolStripItem in ToolStripDesignerUtils.GetStandardItemMenuItems(this.component, this.onClick, this.convertTo))
			{
				base.Groups["StandardList"].Items.Add(toolStripItem);
				if (this.convertTo)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem = toolStripItem as ItemTypeToolStripMenuItem;
					if (itemTypeToolStripMenuItem != null && this.currentItem != null && itemTypeToolStripMenuItem.ItemType == this.currentItem.GetType())
					{
						itemTypeToolStripMenuItem.Enabled = false;
					}
				}
			}
			foreach (ToolStripItem toolStripItem2 in ToolStripDesignerUtils.GetCustomItemMenuItems(this.component, this.onClick, this.convertTo, this.serviceProvider))
			{
				base.Groups["CustomList"].Items.Add(toolStripItem2);
				if (this.convertTo)
				{
					ItemTypeToolStripMenuItem itemTypeToolStripMenuItem2 = toolStripItem2 as ItemTypeToolStripMenuItem;
					if (itemTypeToolStripMenuItem2 != null && this.currentItem != null && itemTypeToolStripMenuItem2.ItemType == this.currentItem.GetType())
					{
						itemTypeToolStripMenuItem2.Enabled = false;
					}
				}
			}
			base.OnOpening(e);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x0007B3CC File Offset: 0x0007A3CC
		protected override bool ProcessDialogKey(Keys keyData)
		{
			switch (keyData & Keys.KeyCode)
			{
			case Keys.Left:
			case Keys.Right:
				base.Close();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x0400139A RID: 5018
		private IComponent component;

		// Token: 0x0400139B RID: 5019
		private EventHandler onClick;

		// Token: 0x0400139C RID: 5020
		private bool convertTo;

		// Token: 0x0400139D RID: 5021
		private IServiceProvider serviceProvider;

		// Token: 0x0400139E RID: 5022
		private ToolStripItem currentItem;
	}
}

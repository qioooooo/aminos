using System;
using System.ComponentModel;

namespace System.Windows.Forms.Design
{
	internal class NewItemsContextMenuStrip : GroupedContextMenuStrip
	{
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

		private IComponent component;

		private EventHandler onClick;

		private bool convertTo;

		private IServiceProvider serviceProvider;

		private ToolStripItem currentItem;
	}
}

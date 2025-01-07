using System;
using System.Drawing;
using System.Drawing.Design;

namespace System.Windows.Forms.Design
{
	internal class ItemTypeToolStripMenuItem : ToolStripMenuItem
	{
		public ItemTypeToolStripMenuItem(Type t)
		{
			this._itemType = t;
		}

		public Type ItemType
		{
			get
			{
				return this._itemType;
			}
		}

		public bool ConvertTo
		{
			get
			{
				return this.convertTo;
			}
			set
			{
				this.convertTo = value;
			}
		}

		public override Image Image
		{
			get
			{
				if (this._image == null)
				{
					this._image = ToolStripDesignerUtils.GetToolboxBitmap(this.ItemType);
				}
				return this._image;
			}
			set
			{
			}
		}

		public override string Text
		{
			get
			{
				return ToolStripDesignerUtils.GetToolboxDescription(this.ItemType);
			}
			set
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.tbxItem = null;
			}
			base.Dispose(disposing);
		}

		private static string systemWindowsFormsNamespace = typeof(ToolStripItem).Namespace;

		private static ToolboxItem invalidToolboxItem = new ToolboxItem();

		private Type _itemType;

		private bool convertTo;

		private ToolboxItem tbxItem = ItemTypeToolStripMenuItem.invalidToolboxItem;

		private Image _image;
	}
}

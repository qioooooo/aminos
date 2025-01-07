using System;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.Design
{
	internal class ToolStripItemGlyph : ControlBodyGlyph
	{
		public ToolStripItemGlyph(ToolStripItem item, ToolStripItemDesigner itemDesigner, Rectangle bounds, Behavior b)
			: base(bounds, Cursors.Default, item, b)
		{
			this._item = item;
			this._bounds = bounds;
			this._itemDesigner = itemDesigner;
		}

		public ToolStripItem Item
		{
			get
			{
				return this._item;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				return this._bounds;
			}
		}

		public ToolStripItemDesigner ItemDesigner
		{
			get
			{
				return this._itemDesigner;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			if (this._item.Visible && this._bounds.Contains(p))
			{
				return Cursors.Default;
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			if (this._item is ToolStripControlHost && this._item.IsOnDropDown)
			{
				if (this._item is ToolStripComboBox && VisualStyleRenderer.IsSupported)
				{
					return;
				}
				this._item.Invalidate();
			}
		}

		private ToolStripItem _item;

		private Rectangle _bounds;

		private ToolStripItemDesigner _itemDesigner;
	}
}

using System;
using System.Drawing;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms.Design
{
	// Token: 0x020002CA RID: 714
	internal class ToolStripItemGlyph : ControlBodyGlyph
	{
		// Token: 0x06001AFA RID: 6906 RVA: 0x00093E73 File Offset: 0x00092E73
		public ToolStripItemGlyph(ToolStripItem item, ToolStripItemDesigner itemDesigner, Rectangle bounds, Behavior b)
			: base(bounds, Cursors.Default, item, b)
		{
			this._item = item;
			this._bounds = bounds;
			this._itemDesigner = itemDesigner;
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x00093E99 File Offset: 0x00092E99
		public ToolStripItem Item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x00093EA1 File Offset: 0x00092EA1
		public override Rectangle Bounds
		{
			get
			{
				return this._bounds;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x00093EA9 File Offset: 0x00092EA9
		public ToolStripItemDesigner ItemDesigner
		{
			get
			{
				return this._itemDesigner;
			}
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x00093EB1 File Offset: 0x00092EB1
		public override Cursor GetHitTest(Point p)
		{
			if (this._item.Visible && this._bounds.Contains(p))
			{
				return Cursors.Default;
			}
			return null;
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x00093ED5 File Offset: 0x00092ED5
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

		// Token: 0x04001554 RID: 5460
		private ToolStripItem _item;

		// Token: 0x04001555 RID: 5461
		private Rectangle _bounds;

		// Token: 0x04001556 RID: 5462
		private ToolStripItemDesigner _itemDesigner;
	}
}

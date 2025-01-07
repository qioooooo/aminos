using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class NoResizeHandleGlyph : SelectionGlyphBase
	{
		internal NoResizeHandleGlyph(Rectangle controlBounds, SelectionRules selRules, bool primarySelection, Behavior behavior)
			: base(behavior)
		{
			this.isPrimary = primarySelection;
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			if ((selRules & SelectionRules.Moveable) != SelectionRules.None)
			{
				this.rules = SelectionRules.Moveable;
				this.hitTestCursor = Cursors.SizeAll;
			}
			this.bounds = new Rectangle(controlBounds.X - DesignerUtils.NORESIZEHANDLESIZE, controlBounds.Y - DesignerUtils.NORESIZEHANDLESIZE, DesignerUtils.NORESIZEHANDLESIZE, DesignerUtils.NORESIZEHANDLESIZE);
			this.hitBounds = this.bounds;
		}

		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawNoResizeHandle(pe.Graphics, this.bounds, this.isPrimary, this);
		}

		private bool isPrimary;
	}
}

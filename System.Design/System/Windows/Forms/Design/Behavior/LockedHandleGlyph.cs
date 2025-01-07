using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class LockedHandleGlyph : SelectionGlyphBase
	{
		internal LockedHandleGlyph(Rectangle controlBounds, bool primarySelection)
			: base(null)
		{
			this.isPrimary = primarySelection;
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			this.bounds = new Rectangle(controlBounds.X + DesignerUtils.LOCKHANDLEOVERLAP - DesignerUtils.LOCKHANDLEWIDTH, controlBounds.Y + DesignerUtils.LOCKHANDLEOVERLAP - DesignerUtils.LOCKHANDLEHEIGHT, DesignerUtils.LOCKHANDLEWIDTH, DesignerUtils.LOCKHANDLEHEIGHT);
			this.hitBounds = this.bounds;
		}

		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawLockedHandle(pe.Graphics, this.bounds, this.isPrimary, this);
		}

		private bool isPrimary;
	}
}

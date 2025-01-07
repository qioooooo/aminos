using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class LockedBorderGlyph : SelectionGlyphBase
	{
		internal LockedBorderGlyph(Rectangle controlBounds, SelectionBorderGlyphType type)
			: base(null)
		{
			this.InitializeGlyph(controlBounds, type);
		}

		private void InitializeGlyph(Rectangle controlBounds, SelectionBorderGlyphType type)
		{
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			this.bounds = DesignerUtils.GetBoundsForSelectionType(controlBounds, type);
			this.hitBounds = this.bounds;
		}

		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawSelectionBorder(pe.Graphics, this.bounds);
		}
	}
}

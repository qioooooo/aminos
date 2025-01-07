using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal class MiniLockedBorderGlyph : SelectionGlyphBase
	{
		internal MiniLockedBorderGlyph(Rectangle controlBounds, SelectionBorderGlyphType type, Behavior behavior, bool primarySelection)
			: base(behavior)
		{
			this.InitializeGlyph(controlBounds, type, primarySelection);
		}

		private void InitializeGlyph(Rectangle controlBounds, SelectionBorderGlyphType type, bool primarySelection)
		{
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			int num = 1;
			this.type = type;
			this.bounds = DesignerUtils.GetBoundsForSelectionType(controlBounds, type, num);
			this.hitBounds = this.bounds;
		}

		public override void Paint(PaintEventArgs pe)
		{
			pe.Graphics.FillRectangle(Brushes.Black, this.bounds);
		}

		private SelectionBorderGlyphType type;
	}
}

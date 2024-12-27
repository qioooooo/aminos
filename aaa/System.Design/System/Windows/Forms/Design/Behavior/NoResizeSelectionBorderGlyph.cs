using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x02000300 RID: 768
	internal class NoResizeSelectionBorderGlyph : SelectionGlyphBase
	{
		// Token: 0x06001D8B RID: 7563 RVA: 0x000A657A File Offset: 0x000A557A
		internal NoResizeSelectionBorderGlyph(Rectangle controlBounds, SelectionRules rules, SelectionBorderGlyphType type, Behavior behavior)
			: base(behavior)
		{
			this.InitializeGlyph(controlBounds, rules, type);
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000A6590 File Offset: 0x000A5590
		private void InitializeGlyph(Rectangle controlBounds, SelectionRules selRules, SelectionBorderGlyphType type)
		{
			this.rules = SelectionRules.None;
			this.hitTestCursor = Cursors.Default;
			if ((selRules & SelectionRules.Moveable) != SelectionRules.None)
			{
				this.rules = SelectionRules.Moveable;
				this.hitTestCursor = Cursors.SizeAll;
			}
			this.bounds = DesignerUtils.GetBoundsForNoResizeSelectionType(controlBounds, type);
			this.hitBounds = this.bounds;
			switch (type)
			{
			case SelectionBorderGlyphType.Top:
			case SelectionBorderGlyphType.Bottom:
				this.hitBounds.Y = this.hitBounds.Y - (DesignerUtils.SELECTIONBORDERHITAREA - DesignerUtils.SELECTIONBORDERSIZE) / 2;
				this.hitBounds.Height = this.hitBounds.Height + (DesignerUtils.SELECTIONBORDERHITAREA - DesignerUtils.SELECTIONBORDERSIZE);
				return;
			case SelectionBorderGlyphType.Left:
			case SelectionBorderGlyphType.Right:
				this.hitBounds.X = this.hitBounds.X - (DesignerUtils.SELECTIONBORDERHITAREA - DesignerUtils.SELECTIONBORDERSIZE) / 2;
				this.hitBounds.Width = this.hitBounds.Width + (DesignerUtils.SELECTIONBORDERHITAREA - DesignerUtils.SELECTIONBORDERSIZE);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000A6679 File Offset: 0x000A5679
		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawSelectionBorder(pe.Graphics, this.bounds);
		}
	}
}

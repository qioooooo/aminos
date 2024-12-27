using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002FE RID: 766
	internal class MiniLockedBorderGlyph : SelectionGlyphBase
	{
		// Token: 0x06001D86 RID: 7558 RVA: 0x000A6466 File Offset: 0x000A5466
		internal MiniLockedBorderGlyph(Rectangle controlBounds, SelectionBorderGlyphType type, Behavior behavior, bool primarySelection)
			: base(behavior)
		{
			this.InitializeGlyph(controlBounds, type, primarySelection);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000A647C File Offset: 0x000A547C
		private void InitializeGlyph(Rectangle controlBounds, SelectionBorderGlyphType type, bool primarySelection)
		{
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			int num = 1;
			this.type = type;
			this.bounds = DesignerUtils.GetBoundsForSelectionType(controlBounds, type, num);
			this.hitBounds = this.bounds;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000A64BE File Offset: 0x000A54BE
		public override void Paint(PaintEventArgs pe)
		{
			pe.Graphics.FillRectangle(Brushes.Black, this.bounds);
		}

		// Token: 0x0400169B RID: 5787
		private SelectionBorderGlyphType type;
	}
}

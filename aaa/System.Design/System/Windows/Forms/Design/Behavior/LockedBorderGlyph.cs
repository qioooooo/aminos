using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002FC RID: 764
	internal class LockedBorderGlyph : SelectionGlyphBase
	{
		// Token: 0x06001D81 RID: 7553 RVA: 0x000A6384 File Offset: 0x000A5384
		internal LockedBorderGlyph(Rectangle controlBounds, SelectionBorderGlyphType type)
			: base(null)
		{
			this.InitializeGlyph(controlBounds, type);
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000A6395 File Offset: 0x000A5395
		private void InitializeGlyph(Rectangle controlBounds, SelectionBorderGlyphType type)
		{
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			this.bounds = DesignerUtils.GetBoundsForSelectionType(controlBounds, type);
			this.hitBounds = this.bounds;
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000A63C2 File Offset: 0x000A53C2
		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawSelectionBorder(pe.Graphics, this.bounds);
		}
	}
}

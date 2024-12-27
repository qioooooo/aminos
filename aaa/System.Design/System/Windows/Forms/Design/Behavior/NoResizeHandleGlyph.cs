using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002FF RID: 767
	internal class NoResizeHandleGlyph : SelectionGlyphBase
	{
		// Token: 0x06001D89 RID: 7561 RVA: 0x000A64D8 File Offset: 0x000A54D8
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

		// Token: 0x06001D8A RID: 7562 RVA: 0x000A6560 File Offset: 0x000A5560
		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawNoResizeHandle(pe.Graphics, this.bounds, this.isPrimary, this);
		}

		// Token: 0x0400169C RID: 5788
		private bool isPrimary;
	}
}

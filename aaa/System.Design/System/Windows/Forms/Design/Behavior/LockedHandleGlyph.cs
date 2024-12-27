using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002FD RID: 765
	internal class LockedHandleGlyph : SelectionGlyphBase
	{
		// Token: 0x06001D84 RID: 7556 RVA: 0x000A63D8 File Offset: 0x000A53D8
		internal LockedHandleGlyph(Rectangle controlBounds, bool primarySelection)
			: base(null)
		{
			this.isPrimary = primarySelection;
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			this.bounds = new Rectangle(controlBounds.X + DesignerUtils.LOCKHANDLEOVERLAP - DesignerUtils.LOCKHANDLEWIDTH, controlBounds.Y + DesignerUtils.LOCKHANDLEOVERLAP - DesignerUtils.LOCKHANDLEHEIGHT, DesignerUtils.LOCKHANDLEWIDTH, DesignerUtils.LOCKHANDLEHEIGHT);
			this.hitBounds = this.bounds;
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000A644C File Offset: 0x000A544C
		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawLockedHandle(pe.Graphics, this.bounds, this.isPrimary, this);
		}

		// Token: 0x0400169A RID: 5786
		private bool isPrimary;
	}
}

using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002FA RID: 762
	internal class GrabHandleGlyph : SelectionGlyphBase
	{
		// Token: 0x06001D7F RID: 7551 RVA: 0x000A601C File Offset: 0x000A501C
		internal GrabHandleGlyph(Rectangle controlBounds, GrabHandleGlyphType type, Behavior behavior, bool primarySelection)
			: base(behavior)
		{
			this.isPrimary = primarySelection;
			this.hitTestCursor = Cursors.Default;
			this.rules = SelectionRules.None;
			switch (type)
			{
			case GrabHandleGlyphType.UpperLeft:
				this.bounds = new Rectangle(controlBounds.X + DesignerUtils.HANDLEOVERLAP - DesignerUtils.HANDLESIZE, controlBounds.Y + DesignerUtils.HANDLEOVERLAP - DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
				this.hitTestCursor = Cursors.SizeNWSE;
				this.rules = SelectionRules.TopSizeable | SelectionRules.LeftSizeable;
				break;
			case GrabHandleGlyphType.UpperRight:
				this.bounds = new Rectangle(controlBounds.Right - DesignerUtils.HANDLEOVERLAP, controlBounds.Y + DesignerUtils.HANDLEOVERLAP - DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
				this.hitTestCursor = Cursors.SizeNESW;
				this.rules = SelectionRules.TopSizeable | SelectionRules.RightSizeable;
				break;
			case GrabHandleGlyphType.LowerLeft:
				this.bounds = new Rectangle(controlBounds.X + DesignerUtils.HANDLEOVERLAP - DesignerUtils.HANDLESIZE, controlBounds.Bottom - DesignerUtils.HANDLEOVERLAP, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
				this.hitTestCursor = Cursors.SizeNESW;
				this.rules = SelectionRules.BottomSizeable | SelectionRules.LeftSizeable;
				break;
			case GrabHandleGlyphType.LowerRight:
				this.bounds = new Rectangle(controlBounds.Right - DesignerUtils.HANDLEOVERLAP, controlBounds.Bottom - DesignerUtils.HANDLEOVERLAP, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
				this.hitTestCursor = Cursors.SizeNWSE;
				this.rules = SelectionRules.BottomSizeable | SelectionRules.RightSizeable;
				break;
			case GrabHandleGlyphType.MiddleTop:
				if (controlBounds.Width >= 2 * DesignerUtils.HANDLEOVERLAP + 2 * DesignerUtils.HANDLESIZE)
				{
					this.bounds = new Rectangle(controlBounds.X + controlBounds.Width / 2 - DesignerUtils.HANDLESIZE / 2, controlBounds.Y + DesignerUtils.HANDLEOVERLAP - DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
					this.hitTestCursor = Cursors.SizeNS;
					this.rules = SelectionRules.TopSizeable;
				}
				break;
			case GrabHandleGlyphType.MiddleBottom:
				if (controlBounds.Width >= 2 * DesignerUtils.HANDLEOVERLAP + 2 * DesignerUtils.HANDLESIZE)
				{
					this.bounds = new Rectangle(controlBounds.X + controlBounds.Width / 2 - DesignerUtils.HANDLESIZE / 2, controlBounds.Bottom - DesignerUtils.HANDLEOVERLAP, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
					this.hitTestCursor = Cursors.SizeNS;
					this.rules = SelectionRules.BottomSizeable;
				}
				break;
			case GrabHandleGlyphType.MiddleLeft:
				if (controlBounds.Height >= 2 * DesignerUtils.HANDLEOVERLAP + 2 * DesignerUtils.HANDLESIZE)
				{
					this.bounds = new Rectangle(controlBounds.X + DesignerUtils.HANDLEOVERLAP - DesignerUtils.HANDLESIZE, controlBounds.Y + controlBounds.Height / 2 - DesignerUtils.HANDLESIZE / 2, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
					this.hitTestCursor = Cursors.SizeWE;
					this.rules = SelectionRules.LeftSizeable;
				}
				break;
			case GrabHandleGlyphType.MiddleRight:
				if (controlBounds.Height >= 2 * DesignerUtils.HANDLEOVERLAP + 2 * DesignerUtils.HANDLESIZE)
				{
					this.bounds = new Rectangle(controlBounds.Right - DesignerUtils.HANDLEOVERLAP, controlBounds.Y + controlBounds.Height / 2 - DesignerUtils.HANDLESIZE / 2, DesignerUtils.HANDLESIZE, DesignerUtils.HANDLESIZE);
					this.hitTestCursor = Cursors.SizeWE;
					this.rules = SelectionRules.RightSizeable;
				}
				break;
			}
			this.hitBounds = this.bounds;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000A636A File Offset: 0x000A536A
		public override void Paint(PaintEventArgs pe)
		{
			DesignerUtils.DrawGrabHandle(pe.Graphics, this.bounds, this.isPrimary, this);
		}

		// Token: 0x04001690 RID: 5776
		private bool isPrimary;
	}
}

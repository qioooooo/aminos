using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002EC RID: 748
	internal sealed class ContainerSelectorGlyph : Glyph
	{
		// Token: 0x06001CFD RID: 7421 RVA: 0x000A1DC0 File Offset: 0x000A0DC0
		internal ContainerSelectorGlyph(Rectangle containerBounds, int glyphSize, int glyphOffset, ContainerSelectorBehavior behavior)
			: base(behavior)
		{
			this.relatedBehavior = behavior;
			this.glyphBounds = new Rectangle(containerBounds.X + glyphOffset, containerBounds.Y - (int)((double)glyphSize * 0.5), glyphSize, glyphSize);
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x000A1DFD File Offset: 0x000A0DFD
		public override Rectangle Bounds
		{
			get
			{
				return this.glyphBounds;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x000A1E05 File Offset: 0x000A0E05
		public Behavior RelatedBehavior
		{
			get
			{
				return this.relatedBehavior;
			}
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000A1E0D File Offset: 0x000A0E0D
		public override Cursor GetHitTest(Point p)
		{
			if (this.glyphBounds.Contains(p) || this.relatedBehavior.OkToMove)
			{
				return Cursors.SizeAll;
			}
			return null;
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000A1E31 File Offset: 0x000A0E31
		private Bitmap MoveGlyph
		{
			get
			{
				if (this.glyph == null)
				{
					this.glyph = new Bitmap(typeof(ContainerSelectorGlyph), "MoverGlyph.bmp");
					this.glyph.MakeTransparent();
				}
				return this.glyph;
			}
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000A1E66 File Offset: 0x000A0E66
		public override void Paint(PaintEventArgs pe)
		{
			pe.Graphics.DrawImage(this.MoveGlyph, this.glyphBounds);
		}

		// Token: 0x04001617 RID: 5655
		private Rectangle glyphBounds;

		// Token: 0x04001618 RID: 5656
		private ContainerSelectorBehavior relatedBehavior;

		// Token: 0x04001619 RID: 5657
		private Bitmap glyph;
	}
}

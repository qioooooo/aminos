using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal sealed class ContainerSelectorGlyph : Glyph
	{
		internal ContainerSelectorGlyph(Rectangle containerBounds, int glyphSize, int glyphOffset, ContainerSelectorBehavior behavior)
			: base(behavior)
		{
			this.relatedBehavior = behavior;
			this.glyphBounds = new Rectangle(containerBounds.X + glyphOffset, containerBounds.Y - (int)((double)glyphSize * 0.5), glyphSize, glyphSize);
		}

		public override Rectangle Bounds
		{
			get
			{
				return this.glyphBounds;
			}
		}

		public Behavior RelatedBehavior
		{
			get
			{
				return this.relatedBehavior;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			if (this.glyphBounds.Contains(p) || this.relatedBehavior.OkToMove)
			{
				return Cursors.SizeAll;
			}
			return null;
		}

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

		public override void Paint(PaintEventArgs pe)
		{
			pe.Graphics.DrawImage(this.MoveGlyph, this.glyphBounds);
		}

		private Rectangle glyphBounds;

		private ContainerSelectorBehavior relatedBehavior;

		private Bitmap glyph;
	}
}

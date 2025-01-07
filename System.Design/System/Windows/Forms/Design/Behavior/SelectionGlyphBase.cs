using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	internal abstract class SelectionGlyphBase : Glyph
	{
		internal SelectionGlyphBase(Behavior behavior)
			: base(behavior)
		{
		}

		public SelectionRules SelectionRules
		{
			get
			{
				return this.rules;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			if (this.hitBounds.Contains(p))
			{
				return this.hitTestCursor;
			}
			return null;
		}

		public Cursor HitTestCursor
		{
			get
			{
				return this.hitTestCursor;
			}
		}

		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		public override void Paint(PaintEventArgs pe)
		{
		}

		protected Rectangle bounds;

		protected Rectangle hitBounds;

		protected Cursor hitTestCursor;

		protected SelectionRules rules;
	}
}

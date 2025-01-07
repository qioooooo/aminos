using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	public abstract class Glyph
	{
		protected Glyph(Behavior behavior)
		{
			this.behavior = behavior;
		}

		public virtual Behavior Behavior
		{
			get
			{
				return this.behavior;
			}
		}

		public virtual Rectangle Bounds
		{
			get
			{
				return Rectangle.Empty;
			}
		}

		public abstract Cursor GetHitTest(Point p);

		public abstract void Paint(PaintEventArgs pe);

		protected void SetBehavior(Behavior behavior)
		{
			this.behavior = behavior;
		}

		private Behavior behavior;
	}
}

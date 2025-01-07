using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	public class ControlBodyGlyph : ComponentGlyph
	{
		public ControlBodyGlyph(Rectangle bounds, Cursor cursor, IComponent relatedComponent, ControlDesigner designer)
			: base(relatedComponent, new ControlDesigner.TransparentBehavior(designer))
		{
			this.bounds = bounds;
			this.hitTestCursor = cursor;
			this.component = relatedComponent;
		}

		public ControlBodyGlyph(Rectangle bounds, Cursor cursor, IComponent relatedComponent, Behavior behavior)
			: base(relatedComponent, behavior)
		{
			this.bounds = bounds;
			this.hitTestCursor = cursor;
			this.component = relatedComponent;
		}

		public override Cursor GetHitTest(Point p)
		{
			bool flag = !(this.component is Control) || ((Control)this.component).Visible;
			if (flag && this.bounds.Contains(p))
			{
				return this.hitTestCursor;
			}
			return null;
		}

		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		private Rectangle bounds;

		private Cursor hitTestCursor;

		private IComponent component;
	}
}

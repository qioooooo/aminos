using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	public class ComponentGlyph : Glyph
	{
		public ComponentGlyph(IComponent relatedComponent, Behavior behavior)
			: base(behavior)
		{
			this.relatedComponent = relatedComponent;
		}

		public ComponentGlyph(IComponent relatedComponent)
			: base(null)
		{
			this.relatedComponent = relatedComponent;
		}

		public IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
		}

		private IComponent relatedComponent;
	}
}

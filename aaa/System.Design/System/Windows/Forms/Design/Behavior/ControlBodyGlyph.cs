using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002C9 RID: 713
	public class ControlBodyGlyph : ComponentGlyph
	{
		// Token: 0x06001AF6 RID: 6902 RVA: 0x00093DDE File Offset: 0x00092DDE
		public ControlBodyGlyph(Rectangle bounds, Cursor cursor, IComponent relatedComponent, ControlDesigner designer)
			: base(relatedComponent, new ControlDesigner.TransparentBehavior(designer))
		{
			this.bounds = bounds;
			this.hitTestCursor = cursor;
			this.component = relatedComponent;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00093E03 File Offset: 0x00092E03
		public ControlBodyGlyph(Rectangle bounds, Cursor cursor, IComponent relatedComponent, Behavior behavior)
			: base(relatedComponent, behavior)
		{
			this.bounds = bounds;
			this.hitTestCursor = cursor;
			this.component = relatedComponent;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x00093E24 File Offset: 0x00092E24
		public override Cursor GetHitTest(Point p)
		{
			bool flag = !(this.component is Control) || ((Control)this.component).Visible;
			if (flag && this.bounds.Contains(p))
			{
				return this.hitTestCursor;
			}
			return null;
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x00093E6B File Offset: 0x00092E6B
		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x04001551 RID: 5457
		private Rectangle bounds;

		// Token: 0x04001552 RID: 5458
		private Cursor hitTestCursor;

		// Token: 0x04001553 RID: 5459
		private IComponent component;
	}
}

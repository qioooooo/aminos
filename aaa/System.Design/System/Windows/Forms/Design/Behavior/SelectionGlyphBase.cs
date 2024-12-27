using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002F9 RID: 761
	internal abstract class SelectionGlyphBase : Glyph
	{
		// Token: 0x06001D79 RID: 7545 RVA: 0x000A5FE0 File Offset: 0x000A4FE0
		internal SelectionGlyphBase(Behavior behavior)
			: base(behavior)
		{
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x000A5FE9 File Offset: 0x000A4FE9
		public SelectionRules SelectionRules
		{
			get
			{
				return this.rules;
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000A5FF1 File Offset: 0x000A4FF1
		public override Cursor GetHitTest(Point p)
		{
			if (this.hitBounds.Contains(p))
			{
				return this.hitTestCursor;
			}
			return null;
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x000A6009 File Offset: 0x000A5009
		public Cursor HitTestCursor
		{
			get
			{
				return this.hitTestCursor;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x000A6011 File Offset: 0x000A5011
		public override Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000A6019 File Offset: 0x000A5019
		public override void Paint(PaintEventArgs pe)
		{
		}

		// Token: 0x0400168C RID: 5772
		protected Rectangle bounds;

		// Token: 0x0400168D RID: 5773
		protected Rectangle hitBounds;

		// Token: 0x0400168E RID: 5774
		protected Cursor hitTestCursor;

		// Token: 0x0400168F RID: 5775
		protected SelectionRules rules;
	}
}

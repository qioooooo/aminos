using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002C7 RID: 711
	public abstract class Glyph
	{
		// Token: 0x06001AEB RID: 6891 RVA: 0x00093D8A File Offset: 0x00092D8A
		protected Glyph(Behavior behavior)
		{
			this.behavior = behavior;
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001AEC RID: 6892 RVA: 0x00093D99 File Offset: 0x00092D99
		public virtual Behavior Behavior
		{
			get
			{
				return this.behavior;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x00093DA1 File Offset: 0x00092DA1
		public virtual Rectangle Bounds
		{
			get
			{
				return Rectangle.Empty;
			}
		}

		// Token: 0x06001AEE RID: 6894
		public abstract Cursor GetHitTest(Point p);

		// Token: 0x06001AEF RID: 6895
		public abstract void Paint(PaintEventArgs pe);

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00093DA8 File Offset: 0x00092DA8
		protected void SetBehavior(Behavior behavior)
		{
			this.behavior = behavior;
		}

		// Token: 0x0400154F RID: 5455
		private Behavior behavior;
	}
}

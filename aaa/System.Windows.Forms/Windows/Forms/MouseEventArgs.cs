using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200031A RID: 794
	[ComVisible(true)]
	public class MouseEventArgs : EventArgs
	{
		// Token: 0x06003357 RID: 13143 RVA: 0x000B4116 File Offset: 0x000B3116
		public MouseEventArgs(MouseButtons button, int clicks, int x, int y, int delta)
		{
			this.button = button;
			this.clicks = clicks;
			this.x = x;
			this.y = y;
			this.delta = delta;
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x000B4143 File Offset: 0x000B3143
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003359 RID: 13145 RVA: 0x000B414B File Offset: 0x000B314B
		public int Clicks
		{
			get
			{
				return this.clicks;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x0600335A RID: 13146 RVA: 0x000B4153 File Offset: 0x000B3153
		public int X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x0600335B RID: 13147 RVA: 0x000B415B File Offset: 0x000B315B
		public int Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600335C RID: 13148 RVA: 0x000B4163 File Offset: 0x000B3163
		public int Delta
		{
			get
			{
				return this.delta;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x0600335D RID: 13149 RVA: 0x000B416B File Offset: 0x000B316B
		public Point Location
		{
			get
			{
				return new Point(this.x, this.y);
			}
		}

		// Token: 0x04001AB8 RID: 6840
		private readonly MouseButtons button;

		// Token: 0x04001AB9 RID: 6841
		private readonly int clicks;

		// Token: 0x04001ABA RID: 6842
		private readonly int x;

		// Token: 0x04001ABB RID: 6843
		private readonly int y;

		// Token: 0x04001ABC RID: 6844
		private readonly int delta;
	}
}

using System;

namespace System.Drawing.Internal
{
	// Token: 0x020000B1 RID: 177
	internal struct GPRECTF
	{
		// Token: 0x06000ADA RID: 2778 RVA: 0x00020263 File Offset: 0x0001F263
		internal GPRECTF(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00020282 File Offset: 0x0001F282
		internal GPRECTF(RectangleF rect)
		{
			this.X = rect.X;
			this.Y = rect.Y;
			this.Width = rect.Width;
			this.Height = rect.Height;
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000202B8 File Offset: 0x0001F2B8
		internal SizeF SizeF
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000202CB File Offset: 0x0001F2CB
		internal RectangleF ToRectangleF()
		{
			return new RectangleF(this.X, this.Y, this.Width, this.Height);
		}

		// Token: 0x040009C5 RID: 2501
		internal float X;

		// Token: 0x040009C6 RID: 2502
		internal float Y;

		// Token: 0x040009C7 RID: 2503
		internal float Width;

		// Token: 0x040009C8 RID: 2504
		internal float Height;
	}
}

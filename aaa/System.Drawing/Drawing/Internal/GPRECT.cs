using System;

namespace System.Drawing.Internal
{
	// Token: 0x020000B0 RID: 176
	internal struct GPRECT
	{
		// Token: 0x06000AD7 RID: 2775 RVA: 0x000201EF File Offset: 0x0001F1EF
		internal GPRECT(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002020E File Offset: 0x0001F20E
		internal GPRECT(Rectangle rect)
		{
			this.X = rect.X;
			this.Y = rect.Y;
			this.Width = rect.Width;
			this.Height = rect.Height;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00020244 File Offset: 0x0001F244
		internal Rectangle ToRectangle()
		{
			return new Rectangle(this.X, this.Y, this.Width, this.Height);
		}

		// Token: 0x040009C1 RID: 2497
		internal int X;

		// Token: 0x040009C2 RID: 2498
		internal int Y;

		// Token: 0x040009C3 RID: 2499
		internal int Width;

		// Token: 0x040009C4 RID: 2500
		internal int Height;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x020000AE RID: 174
	[StructLayout(LayoutKind.Sequential)]
	internal class GPPOINT
	{
		// Token: 0x06000ACF RID: 2767 RVA: 0x0002012B File Offset: 0x0001F12B
		internal GPPOINT()
		{
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00020133 File Offset: 0x0001F133
		internal GPPOINT(PointF pt)
		{
			this.X = (int)pt.X;
			this.Y = (int)pt.Y;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00020157 File Offset: 0x0001F157
		internal GPPOINT(Point pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00020179 File Offset: 0x0001F179
		internal PointF ToPoint()
		{
			return new PointF((float)this.X, (float)this.Y);
		}

		// Token: 0x040009BD RID: 2493
		internal int X;

		// Token: 0x040009BE RID: 2494
		internal int Y;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x020000AF RID: 175
	[StructLayout(LayoutKind.Sequential)]
	internal class GPPOINTF
	{
		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002018E File Offset: 0x0001F18E
		internal GPPOINTF()
		{
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00020196 File Offset: 0x0001F196
		internal GPPOINTF(PointF pt)
		{
			this.X = pt.X;
			this.Y = pt.Y;
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000201B8 File Offset: 0x0001F1B8
		internal GPPOINTF(Point pt)
		{
			this.X = (float)pt.X;
			this.Y = (float)pt.Y;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000201DC File Offset: 0x0001F1DC
		internal PointF ToPoint()
		{
			return new PointF(this.X, this.Y);
		}

		// Token: 0x040009BF RID: 2495
		internal float X;

		// Token: 0x040009C0 RID: 2496
		internal float Y;
	}
}

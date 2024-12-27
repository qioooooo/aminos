using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x02000072 RID: 114
	public sealed class ColorBlend
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x0001BAB7 File Offset: 0x0001AAB7
		public ColorBlend()
		{
			this.colors = new Color[1];
			this.positions = new float[1];
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001BAD7 File Offset: 0x0001AAD7
		public ColorBlend(int count)
		{
			this.colors = new Color[count];
			this.positions = new float[count];
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x0001BAF7 File Offset: 0x0001AAF7
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x0001BAFF File Offset: 0x0001AAFF
		public Color[] Colors
		{
			get
			{
				return this.colors;
			}
			set
			{
				this.colors = value;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0001BB08 File Offset: 0x0001AB08
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0001BB10 File Offset: 0x0001AB10
		public float[] Positions
		{
			get
			{
				return this.positions;
			}
			set
			{
				this.positions = value;
			}
		}

		// Token: 0x040004AC RID: 1196
		private Color[] colors;

		// Token: 0x040004AD RID: 1197
		private float[] positions;
	}
}

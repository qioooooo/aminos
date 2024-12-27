using System;

namespace System.Drawing.Imaging
{
	// Token: 0x02000074 RID: 116
	public sealed class ColorMap
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0001BB19 File Offset: 0x0001AB19
		public ColorMap()
		{
			this.oldColor = default(Color);
			this.newColor = default(Color);
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0001BB39 File Offset: 0x0001AB39
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0001BB41 File Offset: 0x0001AB41
		public Color OldColor
		{
			get
			{
				return this.oldColor;
			}
			set
			{
				this.oldColor = value;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0001BB4A File Offset: 0x0001AB4A
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0001BB52 File Offset: 0x0001AB52
		public Color NewColor
		{
			get
			{
				return this.newColor;
			}
			set
			{
				this.newColor = value;
			}
		}

		// Token: 0x040004B4 RID: 1204
		private Color oldColor;

		// Token: 0x040004B5 RID: 1205
		private Color newColor;
	}
}

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x0200006E RID: 110
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BitmapData
	{
		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001B8AC File Offset: 0x0001A8AC
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001B8B4 File Offset: 0x0001A8B4
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001B8BD File Offset: 0x0001A8BD
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001B8C5 File Offset: 0x0001A8C5
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001B8CE File Offset: 0x0001A8CE
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001B8D6 File Offset: 0x0001A8D6
		public int Stride
		{
			get
			{
				return this.stride;
			}
			set
			{
				this.stride = value;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001B8DF File Offset: 0x0001A8DF
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001B8E8 File Offset: 0x0001A8E8
		public PixelFormat PixelFormat
		{
			get
			{
				return (PixelFormat)this.pixelFormat;
			}
			set
			{
				if (value <= PixelFormat.Format8bppIndexed)
				{
					if (value <= PixelFormat.Format16bppRgb565)
					{
						if (value <= PixelFormat.Max)
						{
							if (value == PixelFormat.Undefined || value == PixelFormat.Max)
							{
								goto IL_012F;
							}
						}
						else
						{
							if (value == PixelFormat.Indexed || value == PixelFormat.Gdi)
							{
								goto IL_012F;
							}
							switch (value)
							{
							case PixelFormat.Format16bppRgb555:
							case PixelFormat.Format16bppRgb565:
								goto IL_012F;
							}
						}
					}
					else if (value <= PixelFormat.Format32bppRgb)
					{
						if (value == PixelFormat.Format24bppRgb || value == PixelFormat.Format32bppRgb)
						{
							goto IL_012F;
						}
					}
					else if (value == PixelFormat.Format1bppIndexed || value == PixelFormat.Format4bppIndexed || value == PixelFormat.Format8bppIndexed)
					{
						goto IL_012F;
					}
				}
				else if (value <= PixelFormat.Extended)
				{
					if (value <= PixelFormat.Format16bppArgb1555)
					{
						if (value == PixelFormat.Alpha || value == PixelFormat.Format16bppArgb1555)
						{
							goto IL_012F;
						}
					}
					else if (value == PixelFormat.PAlpha || value == PixelFormat.Format32bppPArgb || value == PixelFormat.Extended)
					{
						goto IL_012F;
					}
				}
				else if (value <= PixelFormat.Format64bppPArgb)
				{
					if (value == PixelFormat.Format16bppGrayScale || value == PixelFormat.Format48bppRgb || value == PixelFormat.Format64bppPArgb)
					{
						goto IL_012F;
					}
				}
				else if (value == PixelFormat.Canonical || value == PixelFormat.Format32bppArgb || value == PixelFormat.Format64bppArgb)
				{
					goto IL_012F;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(PixelFormat));
				IL_012F:
				this.pixelFormat = (int)value;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001BA2B File Offset: 0x0001AA2B
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001BA33 File Offset: 0x0001AA33
		public IntPtr Scan0
		{
			get
			{
				return this.scan0;
			}
			set
			{
				this.scan0 = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001BA3C File Offset: 0x0001AA3C
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001BA44 File Offset: 0x0001AA44
		public int Reserved
		{
			get
			{
				return this.reserved;
			}
			set
			{
				this.reserved = value;
			}
		}

		// Token: 0x04000496 RID: 1174
		private int width;

		// Token: 0x04000497 RID: 1175
		private int height;

		// Token: 0x04000498 RID: 1176
		private int stride;

		// Token: 0x04000499 RID: 1177
		private int pixelFormat;

		// Token: 0x0400049A RID: 1178
		private IntPtr scan0;

		// Token: 0x0400049B RID: 1179
		private int reserved;
	}
}

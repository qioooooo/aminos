using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000CE RID: 206
	[StructLayout(LayoutKind.Sequential)]
	internal class MetafileHeaderEmf
	{
		// Token: 0x04000A94 RID: 2708
		public MetafileType type;

		// Token: 0x04000A95 RID: 2709
		public int size;

		// Token: 0x04000A96 RID: 2710
		public int version;

		// Token: 0x04000A97 RID: 2711
		public EmfPlusFlags emfPlusFlags;

		// Token: 0x04000A98 RID: 2712
		public float dpiX;

		// Token: 0x04000A99 RID: 2713
		public float dpiY;

		// Token: 0x04000A9A RID: 2714
		public int X;

		// Token: 0x04000A9B RID: 2715
		public int Y;

		// Token: 0x04000A9C RID: 2716
		public int Width;

		// Token: 0x04000A9D RID: 2717
		public int Height;

		// Token: 0x04000A9E RID: 2718
		public SafeNativeMethods.ENHMETAHEADER EmfHeader;

		// Token: 0x04000A9F RID: 2719
		public int EmfPlusHeaderSize;

		// Token: 0x04000AA0 RID: 2720
		public int LogicalDpiX;

		// Token: 0x04000AA1 RID: 2721
		public int LogicalDpiY;
	}
}

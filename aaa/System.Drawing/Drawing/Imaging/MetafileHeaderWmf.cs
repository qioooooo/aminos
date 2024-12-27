using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000CF RID: 207
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal class MetafileHeaderWmf
	{
		// Token: 0x04000AA2 RID: 2722
		public MetafileType type;

		// Token: 0x04000AA3 RID: 2723
		public int size = Marshal.SizeOf(typeof(MetafileHeaderWmf));

		// Token: 0x04000AA4 RID: 2724
		public int version;

		// Token: 0x04000AA5 RID: 2725
		public EmfPlusFlags emfPlusFlags;

		// Token: 0x04000AA6 RID: 2726
		public float dpiX;

		// Token: 0x04000AA7 RID: 2727
		public float dpiY;

		// Token: 0x04000AA8 RID: 2728
		public int X;

		// Token: 0x04000AA9 RID: 2729
		public int Y;

		// Token: 0x04000AAA RID: 2730
		public int Width;

		// Token: 0x04000AAB RID: 2731
		public int Height;

		// Token: 0x04000AAC RID: 2732
		[MarshalAs(UnmanagedType.Struct)]
		public MetaHeader WmfHeader = new MetaHeader();

		// Token: 0x04000AAD RID: 2733
		public int dummy1;

		// Token: 0x04000AAE RID: 2734
		public int dummy2;

		// Token: 0x04000AAF RID: 2735
		public int dummy3;

		// Token: 0x04000AB0 RID: 2736
		public int dummy4;

		// Token: 0x04000AB1 RID: 2737
		public int EmfPlusHeaderSize;

		// Token: 0x04000AB2 RID: 2738
		public int LogicalDpiX;

		// Token: 0x04000AB3 RID: 2739
		public int LogicalDpiY;
	}
}

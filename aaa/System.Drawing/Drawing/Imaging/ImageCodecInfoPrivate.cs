using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x020000BE RID: 190
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal class ImageCodecInfoPrivate
	{
		// Token: 0x04000A37 RID: 2615
		[MarshalAs(UnmanagedType.Struct)]
		public Guid Clsid;

		// Token: 0x04000A38 RID: 2616
		[MarshalAs(UnmanagedType.Struct)]
		public Guid FormatID;

		// Token: 0x04000A39 RID: 2617
		public IntPtr CodecName = IntPtr.Zero;

		// Token: 0x04000A3A RID: 2618
		public IntPtr DllName = IntPtr.Zero;

		// Token: 0x04000A3B RID: 2619
		public IntPtr FormatDescription = IntPtr.Zero;

		// Token: 0x04000A3C RID: 2620
		public IntPtr FilenameExtension = IntPtr.Zero;

		// Token: 0x04000A3D RID: 2621
		public IntPtr MimeType = IntPtr.Zero;

		// Token: 0x04000A3E RID: 2622
		public int Flags;

		// Token: 0x04000A3F RID: 2623
		public int Version;

		// Token: 0x04000A40 RID: 2624
		public int SigCount;

		// Token: 0x04000A41 RID: 2625
		public int SigSize;

		// Token: 0x04000A42 RID: 2626
		public IntPtr SigPattern = IntPtr.Zero;

		// Token: 0x04000A43 RID: 2627
		public IntPtr SigMask = IntPtr.Zero;
	}
}

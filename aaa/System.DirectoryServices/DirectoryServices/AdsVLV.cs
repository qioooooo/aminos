using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices
{
	// Token: 0x0200001A RID: 26
	[StructLayout(LayoutKind.Sequential)]
	internal class AdsVLV
	{
		// Token: 0x04000150 RID: 336
		public int beforeCount;

		// Token: 0x04000151 RID: 337
		public int afterCount;

		// Token: 0x04000152 RID: 338
		public int offset;

		// Token: 0x04000153 RID: 339
		public int contentCount;

		// Token: 0x04000154 RID: 340
		public IntPtr target;

		// Token: 0x04000155 RID: 341
		public int contextIDlength;

		// Token: 0x04000156 RID: 342
		public IntPtr contextID;
	}
}

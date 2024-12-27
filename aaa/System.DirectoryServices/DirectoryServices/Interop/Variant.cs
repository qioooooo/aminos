using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Interop
{
	// Token: 0x0200005F RID: 95
	[StructLayout(LayoutKind.Explicit)]
	internal struct Variant
	{
		// Token: 0x04000293 RID: 659
		[FieldOffset(0)]
		public ushort varType;

		// Token: 0x04000294 RID: 660
		[FieldOffset(2)]
		public ushort reserved1;

		// Token: 0x04000295 RID: 661
		[FieldOffset(4)]
		public ushort reserved2;

		// Token: 0x04000296 RID: 662
		[FieldOffset(6)]
		public ushort reserved3;

		// Token: 0x04000297 RID: 663
		[FieldOffset(8)]
		public short boolvalue;

		// Token: 0x04000298 RID: 664
		[FieldOffset(8)]
		public IntPtr ptr1;

		// Token: 0x04000299 RID: 665
		[FieldOffset(12)]
		public IntPtr ptr2;
	}
}

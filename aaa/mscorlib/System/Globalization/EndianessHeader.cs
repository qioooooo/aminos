using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C4 RID: 964
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct EndianessHeader
	{
		// Token: 0x040012D8 RID: 4824
		internal uint leOffset;

		// Token: 0x040012D9 RID: 4825
		internal uint beOffset;
	}
}

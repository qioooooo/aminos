using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000009 RID: 9
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct EndianessHeader
	{
		// Token: 0x040000E6 RID: 230
		internal uint leOffset;

		// Token: 0x040000E7 RID: 231
		internal uint beOffset;
	}
}

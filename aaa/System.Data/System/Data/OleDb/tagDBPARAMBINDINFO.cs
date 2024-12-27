using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000241 RID: 577
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct tagDBPARAMBINDINFO
	{
		// Token: 0x040014A0 RID: 5280
		internal IntPtr pwszDataSourceType;

		// Token: 0x040014A1 RID: 5281
		internal IntPtr pwszName;

		// Token: 0x040014A2 RID: 5282
		internal IntPtr ulParamSize;

		// Token: 0x040014A3 RID: 5283
		internal int dwFlags;

		// Token: 0x040014A4 RID: 5284
		internal byte bPrecision;

		// Token: 0x040014A5 RID: 5285
		internal byte bScale;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000243 RID: 579
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct tagDBCOLUMNACCESS
	{
		// Token: 0x040014B5 RID: 5301
		internal IntPtr pData;

		// Token: 0x040014B6 RID: 5302
		internal tagDBIDX columnid;

		// Token: 0x040014B7 RID: 5303
		internal IntPtr cbDataLen;

		// Token: 0x040014B8 RID: 5304
		internal int dwStatus;

		// Token: 0x040014B9 RID: 5305
		internal IntPtr cbMaxLen;

		// Token: 0x040014BA RID: 5306
		internal IntPtr dwReserved;

		// Token: 0x040014BB RID: 5307
		internal short wType;

		// Token: 0x040014BC RID: 5308
		internal byte bPrecision;

		// Token: 0x040014BD RID: 5309
		internal byte bScale;
	}
}

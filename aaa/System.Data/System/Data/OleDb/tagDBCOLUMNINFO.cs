using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200024A RID: 586
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBCOLUMNINFO
	{
		// Token: 0x06002072 RID: 8306 RVA: 0x00262B8C File Offset: 0x00261F8C
		internal tagDBCOLUMNINFO()
		{
		}

		// Token: 0x040014D5 RID: 5333
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pwszName;

		// Token: 0x040014D6 RID: 5334
		internal IntPtr pTypeInfo = (IntPtr)0;

		// Token: 0x040014D7 RID: 5335
		internal IntPtr iOrdinal = (IntPtr)0;

		// Token: 0x040014D8 RID: 5336
		internal int dwFlags;

		// Token: 0x040014D9 RID: 5337
		internal IntPtr ulColumnSize = (IntPtr)0;

		// Token: 0x040014DA RID: 5338
		internal short wType;

		// Token: 0x040014DB RID: 5339
		internal byte bPrecision;

		// Token: 0x040014DC RID: 5340
		internal byte bScale;

		// Token: 0x040014DD RID: 5341
		internal tagDBIDX columnid;
	}
}

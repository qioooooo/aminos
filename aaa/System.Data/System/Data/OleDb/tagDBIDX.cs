using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000244 RID: 580
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct tagDBIDX
	{
		// Token: 0x040014BE RID: 5310
		internal Guid uGuid;

		// Token: 0x040014BF RID: 5311
		internal int eKind;

		// Token: 0x040014C0 RID: 5312
		internal IntPtr ulPropid;
	}
}

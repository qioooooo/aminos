using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000249 RID: 585
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBPARAMS
	{
		// Token: 0x06002071 RID: 8305 RVA: 0x00262B78 File Offset: 0x00261F78
		internal tagDBPARAMS()
		{
		}

		// Token: 0x040014D2 RID: 5330
		internal IntPtr pData;

		// Token: 0x040014D3 RID: 5331
		internal int cParamSets;

		// Token: 0x040014D4 RID: 5332
		internal IntPtr hAccessor;
	}
}

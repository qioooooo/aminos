using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200024B RID: 587
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBPROPINFOSET
	{
		// Token: 0x06002073 RID: 8307 RVA: 0x00262BC4 File Offset: 0x00261FC4
		internal tagDBPROPINFOSET()
		{
		}

		// Token: 0x040014DE RID: 5342
		internal IntPtr rgPropertyInfos;

		// Token: 0x040014DF RID: 5343
		internal int cPropertyInfos;

		// Token: 0x040014E0 RID: 5344
		internal Guid guidPropertySet;
	}
}

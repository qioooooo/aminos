using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200024C RID: 588
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBPROPINFO
	{
		// Token: 0x06002074 RID: 8308 RVA: 0x00262BD8 File Offset: 0x00261FD8
		internal tagDBPROPINFO()
		{
		}

		// Token: 0x040014E1 RID: 5345
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string pwszDescription;

		// Token: 0x040014E2 RID: 5346
		internal int dwPropertyID;

		// Token: 0x040014E3 RID: 5347
		internal int dwFlags;

		// Token: 0x040014E4 RID: 5348
		internal short vtType;

		// Token: 0x040014E5 RID: 5349
		[MarshalAs(UnmanagedType.Struct)]
		internal object vValue;
	}
}

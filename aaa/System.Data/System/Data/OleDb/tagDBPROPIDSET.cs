using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200024D RID: 589
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct tagDBPROPIDSET
	{
		// Token: 0x040014E6 RID: 5350
		internal IntPtr rgPropertyIDs;

		// Token: 0x040014E7 RID: 5351
		internal int cPropertyIDs;

		// Token: 0x040014E8 RID: 5352
		internal Guid guidPropertySet;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000245 RID: 581
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBID
	{
		// Token: 0x040014C1 RID: 5313
		internal Guid uGuid;

		// Token: 0x040014C2 RID: 5314
		internal int eKind;

		// Token: 0x040014C3 RID: 5315
		internal IntPtr ulPropid;
	}
}

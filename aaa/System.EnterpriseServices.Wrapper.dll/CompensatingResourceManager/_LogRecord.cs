using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x02000080 RID: 128
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct _LogRecord
	{
		// Token: 0x040000AF RID: 175
		public int dwCrmFlags;

		// Token: 0x040000B0 RID: 176
		public int dwSequenceNumber;

		// Token: 0x040000B1 RID: 177
		public _BLOB blobUserData;
	}
}

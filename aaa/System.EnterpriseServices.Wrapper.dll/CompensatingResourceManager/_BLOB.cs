using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x0200007F RID: 127
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct _BLOB
	{
		// Token: 0x040000AD RID: 173
		public int cbSize;

		// Token: 0x040000AE RID: 174
		public IntPtr pBlobData;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000115 RID: 277
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_NAME_RESULT_ITEM
	{
		// Token: 0x04000690 RID: 1680
		public DS_NAME_ERROR status;

		// Token: 0x04000691 RID: 1681
		public IntPtr pDomain;

		// Token: 0x04000692 RID: 1682
		public IntPtr pName;
	}
}

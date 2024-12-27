using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000116 RID: 278
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_NAME_RESULT
	{
		// Token: 0x04000693 RID: 1683
		public int cItems;

		// Token: 0x04000694 RID: 1684
		public IntPtr rItems;
	}
}

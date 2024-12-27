using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000102 RID: 258
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class FileTime
	{
		// Token: 0x0400063B RID: 1595
		public int lower;

		// Token: 0x0400063C RID: 1596
		public int higher;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200011B RID: 283
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LARGE_INTEGER
	{
		// Token: 0x06000775 RID: 1909 RVA: 0x00027708 File Offset: 0x00026708
		public LARGE_INTEGER()
		{
			this.lowPart = 0;
			this.highPart = 0;
		}

		// Token: 0x040006AC RID: 1708
		public int lowPart;

		// Token: 0x040006AD RID: 1709
		public int highPart;
	}
}

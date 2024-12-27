using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B9 RID: 185
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DsNameResultItem
	{
		// Token: 0x040004BC RID: 1212
		public int status;

		// Token: 0x040004BD RID: 1213
		public string domain;

		// Token: 0x040004BE RID: 1214
		public string name;
	}
}

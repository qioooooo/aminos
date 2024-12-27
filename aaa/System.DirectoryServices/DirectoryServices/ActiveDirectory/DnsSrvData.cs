using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000BC RID: 188
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DnsSrvData
	{
		// Token: 0x040004CF RID: 1231
		public string targetName;

		// Token: 0x040004D0 RID: 1232
		public short priority;

		// Token: 0x040004D1 RID: 1233
		public short weight;

		// Token: 0x040004D2 RID: 1234
		public short port;

		// Token: 0x040004D3 RID: 1235
		public short pad;
	}
}

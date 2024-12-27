using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000BA RID: 186
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class DnsRecord
	{
		// Token: 0x040004BF RID: 1215
		public IntPtr next;

		// Token: 0x040004C0 RID: 1216
		public string name;

		// Token: 0x040004C1 RID: 1217
		public short type;

		// Token: 0x040004C2 RID: 1218
		public short dataLength;

		// Token: 0x040004C3 RID: 1219
		public int flags;

		// Token: 0x040004C4 RID: 1220
		public int ttl;

		// Token: 0x040004C5 RID: 1221
		public int reserved;

		// Token: 0x040004C6 RID: 1222
		public DnsSrvData data;
	}
}

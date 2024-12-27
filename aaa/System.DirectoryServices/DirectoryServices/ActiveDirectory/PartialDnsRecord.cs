using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000BB RID: 187
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal sealed class PartialDnsRecord
	{
		// Token: 0x040004C7 RID: 1223
		public IntPtr next;

		// Token: 0x040004C8 RID: 1224
		public string name;

		// Token: 0x040004C9 RID: 1225
		public short type;

		// Token: 0x040004CA RID: 1226
		public short dataLength;

		// Token: 0x040004CB RID: 1227
		public int flags;

		// Token: 0x040004CC RID: 1228
		public int ttl;

		// Token: 0x040004CD RID: 1229
		public int reserved;

		// Token: 0x040004CE RID: 1230
		public IntPtr data;
	}
}

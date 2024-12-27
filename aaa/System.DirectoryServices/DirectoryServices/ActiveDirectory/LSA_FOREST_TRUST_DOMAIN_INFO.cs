using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200011D RID: 285
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_FOREST_TRUST_DOMAIN_INFO
	{
		// Token: 0x040006B1 RID: 1713
		public IntPtr sid;

		// Token: 0x040006B2 RID: 1714
		public short DNSNameLength;

		// Token: 0x040006B3 RID: 1715
		public short DNSNameMaximumLength;

		// Token: 0x040006B4 RID: 1716
		public IntPtr DNSNameBuffer;

		// Token: 0x040006B5 RID: 1717
		public short NetBIOSNameLength;

		// Token: 0x040006B6 RID: 1718
		public short NetBIOSNameMaximumLength;

		// Token: 0x040006B7 RID: 1719
		public IntPtr NetBIOSNameBuffer;
	}
}

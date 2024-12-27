using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000120 RID: 288
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class TRUSTED_DOMAIN_INFORMATION_EX
	{
		// Token: 0x040006C0 RID: 1728
		public LSA_UNICODE_STRING Name;

		// Token: 0x040006C1 RID: 1729
		public LSA_UNICODE_STRING FlatName;

		// Token: 0x040006C2 RID: 1730
		public IntPtr Sid;

		// Token: 0x040006C3 RID: 1731
		public int TrustDirection;

		// Token: 0x040006C4 RID: 1732
		public int TrustType;

		// Token: 0x040006C5 RID: 1733
		public TRUST_ATTRIBUTE TrustAttributes;
	}
}

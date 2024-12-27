using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000128 RID: 296
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class TRUSTED_DOMAIN_FULL_INFORMATION
	{
		// Token: 0x040006E0 RID: 1760
		public TRUSTED_DOMAIN_INFORMATION_EX Information;

		// Token: 0x040006E1 RID: 1761
		internal TRUSTED_POSIX_OFFSET_INFO PosixOffset;

		// Token: 0x040006E2 RID: 1762
		public TRUSTED_DOMAIN_AUTH_INFORMATION AuthInformation;
	}
}

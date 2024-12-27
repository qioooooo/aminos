using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000124 RID: 292
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class TRUSTED_DOMAIN_AUTH_INFORMATION
	{
		// Token: 0x040006D0 RID: 1744
		public int IncomingAuthInfos;

		// Token: 0x040006D1 RID: 1745
		public IntPtr IncomingAuthenticationInformation;

		// Token: 0x040006D2 RID: 1746
		public IntPtr IncomingPreviousAuthenticationInformation;

		// Token: 0x040006D3 RID: 1747
		public int OutgoingAuthInfos;

		// Token: 0x040006D4 RID: 1748
		public IntPtr OutgoingAuthenticationInformation;

		// Token: 0x040006D5 RID: 1749
		public IntPtr OutgoingPreviousAuthenticationInformation;
	}
}

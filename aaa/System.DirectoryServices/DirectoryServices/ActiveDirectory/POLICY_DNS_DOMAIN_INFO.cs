using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000126 RID: 294
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class POLICY_DNS_DOMAIN_INFO
	{
		// Token: 0x040006DA RID: 1754
		public LSA_UNICODE_STRING Name;

		// Token: 0x040006DB RID: 1755
		public LSA_UNICODE_STRING DnsDomainName;

		// Token: 0x040006DC RID: 1756
		public LSA_UNICODE_STRING DnsForestName;

		// Token: 0x040006DD RID: 1757
		public Guid DomainGuid;

		// Token: 0x040006DE RID: 1758
		public IntPtr Sid;
	}
}

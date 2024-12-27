using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000117 RID: 279
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_DOMAIN_TRUSTS
	{
		// Token: 0x04000695 RID: 1685
		public IntPtr NetbiosDomainName;

		// Token: 0x04000696 RID: 1686
		public IntPtr DnsDomainName;

		// Token: 0x04000697 RID: 1687
		public int Flags;

		// Token: 0x04000698 RID: 1688
		public int ParentIndex;

		// Token: 0x04000699 RID: 1689
		public int TrustType;

		// Token: 0x0400069A RID: 1690
		public int TrustAttributes;

		// Token: 0x0400069B RID: 1691
		public IntPtr DomainSid;

		// Token: 0x0400069C RID: 1692
		public Guid DomainGuid;
	}
}

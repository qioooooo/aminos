using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000118 RID: 280
	internal sealed class TrustObject
	{
		// Token: 0x0400069D RID: 1693
		public string NetbiosDomainName;

		// Token: 0x0400069E RID: 1694
		public string DnsDomainName;

		// Token: 0x0400069F RID: 1695
		public int Flags;

		// Token: 0x040006A0 RID: 1696
		public int ParentIndex;

		// Token: 0x040006A1 RID: 1697
		public TrustType TrustType;

		// Token: 0x040006A2 RID: 1698
		public int TrustAttributes;

		// Token: 0x040006A3 RID: 1699
		public int OriginalIndex;
	}
}

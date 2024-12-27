using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000322 RID: 802
	[Flags]
	public enum X500DistinguishedNameFlags
	{
		// Token: 0x04001A80 RID: 6784
		None = 0,
		// Token: 0x04001A81 RID: 6785
		Reversed = 1,
		// Token: 0x04001A82 RID: 6786
		UseSemicolons = 16,
		// Token: 0x04001A83 RID: 6787
		DoNotUsePlusSign = 32,
		// Token: 0x04001A84 RID: 6788
		DoNotUseQuotes = 64,
		// Token: 0x04001A85 RID: 6789
		UseCommas = 128,
		// Token: 0x04001A86 RID: 6790
		UseNewLines = 256,
		// Token: 0x04001A87 RID: 6791
		UseUTF8Encoding = 4096,
		// Token: 0x04001A88 RID: 6792
		UseT61Encoding = 8192,
		// Token: 0x04001A89 RID: 6793
		ForceUTF8Encoding = 16384
	}
}

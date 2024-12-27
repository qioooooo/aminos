using System;

namespace System.Net
{
	// Token: 0x0200040B RID: 1035
	internal enum CertificateEncoding
	{
		// Token: 0x04002097 RID: 8343
		Zero,
		// Token: 0x04002098 RID: 8344
		X509AsnEncoding,
		// Token: 0x04002099 RID: 8345
		X509NdrEncoding,
		// Token: 0x0400209A RID: 8346
		Pkcs7AsnEncoding = 65536,
		// Token: 0x0400209B RID: 8347
		Pkcs7NdrEncoding = 131072,
		// Token: 0x0400209C RID: 8348
		AnyAsnEncoding = 65537
	}
}

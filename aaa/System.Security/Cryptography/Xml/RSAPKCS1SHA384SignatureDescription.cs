using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000CA RID: 202
	internal class RSAPKCS1SHA384SignatureDescription : RSAPKCS1SignatureDescription
	{
		// Token: 0x060004FD RID: 1277 RVA: 0x000192C9 File Offset: 0x000182C9
		public RSAPKCS1SHA384SignatureDescription()
			: base("SHA384")
		{
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000192D6 File Offset: 0x000182D6
		public sealed override HashAlgorithm CreateDigest()
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName("http://www.w3.org/2001/04/xmldsig-more#sha384");
		}
	}
}

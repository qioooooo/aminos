using System;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C9 RID: 201
	internal class RSAPKCS1SHA256SignatureDescription : RSAPKCS1SignatureDescription
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x000192AB File Offset: 0x000182AB
		public RSAPKCS1SHA256SignatureDescription()
			: base("SHA256")
		{
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x000192B8 File Offset: 0x000182B8
		public sealed override HashAlgorithm CreateDigest()
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName("http://www.w3.org/2001/04/xmlenc#sha256");
		}
	}
}

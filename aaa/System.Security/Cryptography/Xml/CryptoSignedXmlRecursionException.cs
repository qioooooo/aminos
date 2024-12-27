using System;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C6 RID: 198
	[Serializable]
	internal class CryptoSignedXmlRecursionException : XmlException
	{
		// Token: 0x060004CF RID: 1231 RVA: 0x00017FDC File Offset: 0x00016FDC
		public CryptoSignedXmlRecursionException()
		{
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00017FE4 File Offset: 0x00016FE4
		public CryptoSignedXmlRecursionException(string message)
			: base(message)
		{
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00017FED File Offset: 0x00016FED
		public CryptoSignedXmlRecursionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00017FF7 File Offset: 0x00016FF7
		protected CryptoSignedXmlRecursionException(SerializationInfo info, StreamingContext context)
		{
		}
	}
}

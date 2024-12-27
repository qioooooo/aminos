using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000879 RID: 2169
	[ComVisible(true)]
	public abstract class MD5 : HashAlgorithm
	{
		// Token: 0x06004F71 RID: 20337 RVA: 0x00115396 File Offset: 0x00114396
		protected MD5()
		{
			this.HashSizeValue = 128;
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x001153A9 File Offset: 0x001143A9
		public new static MD5 Create()
		{
			return MD5.Create("System.Security.Cryptography.MD5");
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x001153B5 File Offset: 0x001143B5
		public new static MD5 Create(string algName)
		{
			return (MD5)CryptoConfig.CreateFromName(algName);
		}
	}
}

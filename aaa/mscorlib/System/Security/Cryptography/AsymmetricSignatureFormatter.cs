using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000856 RID: 2134
	[ComVisible(true)]
	public abstract class AsymmetricSignatureFormatter
	{
		// Token: 0x06004E51 RID: 20049
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E52 RID: 20050
		public abstract void SetHashAlgorithm(string strName);

		// Token: 0x06004E53 RID: 20051 RVA: 0x0010FDEA File Offset: 0x0010EDEA
		public virtual byte[] CreateSignature(HashAlgorithm hash)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			this.SetHashAlgorithm(hash.ToString());
			return this.CreateSignature(hash.Hash);
		}

		// Token: 0x06004E54 RID: 20052
		public abstract byte[] CreateSignature(byte[] rgbHash);
	}
}

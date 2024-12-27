using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000855 RID: 2133
	[ComVisible(true)]
	public abstract class AsymmetricSignatureDeformatter
	{
		// Token: 0x06004E4C RID: 20044
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E4D RID: 20045
		public abstract void SetHashAlgorithm(string strName);

		// Token: 0x06004E4E RID: 20046 RVA: 0x0010FDB9 File Offset: 0x0010EDB9
		public virtual bool VerifySignature(HashAlgorithm hash, byte[] rgbSignature)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			this.SetHashAlgorithm(hash.ToString());
			return this.VerifySignature(hash.Hash, rgbSignature);
		}

		// Token: 0x06004E4F RID: 20047
		public abstract bool VerifySignature(byte[] rgbHash, byte[] rgbSignature);
	}
}

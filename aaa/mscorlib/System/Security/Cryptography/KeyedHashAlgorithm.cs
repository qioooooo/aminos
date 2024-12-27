using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086D RID: 2157
	[ComVisible(true)]
	public abstract class KeyedHashAlgorithm : HashAlgorithm
	{
		// Token: 0x06004F22 RID: 20258 RVA: 0x00114372 File Offset: 0x00113372
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.KeyValue != null)
				{
					Array.Clear(this.KeyValue, 0, this.KeyValue.Length);
				}
				this.KeyValue = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06004F23 RID: 20259 RVA: 0x001143A1 File Offset: 0x001133A1
		// (set) Token: 0x06004F24 RID: 20260 RVA: 0x001143B3 File Offset: 0x001133B3
		public virtual byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.State != 0)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_HashKeySet"));
				}
				this.KeyValue = (byte[])value.Clone();
			}
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x001143DE File Offset: 0x001133DE
		public new static KeyedHashAlgorithm Create()
		{
			return KeyedHashAlgorithm.Create("System.Security.Cryptography.KeyedHashAlgorithm");
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x001143EA File Offset: 0x001133EA
		public new static KeyedHashAlgorithm Create(string algName)
		{
			return (KeyedHashAlgorithm)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x040028AE RID: 10414
		protected byte[] KeyValue;
	}
}

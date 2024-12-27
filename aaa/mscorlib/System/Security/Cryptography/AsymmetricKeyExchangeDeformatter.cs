using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000853 RID: 2131
	[ComVisible(true)]
	public abstract class AsymmetricKeyExchangeDeformatter
	{
		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06004E42 RID: 20034
		// (set) Token: 0x06004E43 RID: 20035
		public abstract string Parameters { get; set; }

		// Token: 0x06004E44 RID: 20036
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E45 RID: 20037
		public abstract byte[] DecryptKeyExchange(byte[] rgb);
	}
}

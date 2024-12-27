using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000854 RID: 2132
	[ComVisible(true)]
	public abstract class AsymmetricKeyExchangeFormatter
	{
		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004E47 RID: 20039
		public abstract string Parameters { get; }

		// Token: 0x06004E48 RID: 20040
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06004E49 RID: 20041
		public abstract byte[] CreateKeyExchange(byte[] data);

		// Token: 0x06004E4A RID: 20042
		public abstract byte[] CreateKeyExchange(byte[] data, Type symAlgType);
	}
}

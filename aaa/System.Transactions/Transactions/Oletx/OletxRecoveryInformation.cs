using System;

namespace System.Transactions.Oletx
{
	// Token: 0x0200008C RID: 140
	[Serializable]
	internal class OletxRecoveryInformation
	{
		// Token: 0x0600038C RID: 908 RVA: 0x00034BD8 File Offset: 0x00033FD8
		internal OletxRecoveryInformation(byte[] proxyRecoveryInformation)
		{
			this.proxyRecoveryInformation = proxyRecoveryInformation;
		}

		// Token: 0x040001D8 RID: 472
		internal byte[] proxyRecoveryInformation;
	}
}

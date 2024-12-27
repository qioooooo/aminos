using System;
using System.Runtime.InteropServices;
using System.Security.Authentication;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000088 RID: 136
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public class SecurityPackageContextConnectionInformation
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000DE88 File Offset: 0x0000CE88
		internal SecurityPackageContextConnectionInformation()
		{
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000DE90 File Offset: 0x0000CE90
		public SecurityProtocol Protocol
		{
			get
			{
				return this.securityProtocol;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000DE98 File Offset: 0x0000CE98
		public CipherAlgorithmType AlgorithmIdentifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000DEA0 File Offset: 0x0000CEA0
		public int CipherStrength
		{
			get
			{
				return this.strength;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000DEA8 File Offset: 0x0000CEA8
		public HashAlgorithmType Hash
		{
			get
			{
				return this.hashAlgorithm;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000DEB0 File Offset: 0x0000CEB0
		public int HashStrength
		{
			get
			{
				return this.hashStrength;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000DEB8 File Offset: 0x0000CEB8
		public int KeyExchangeAlgorithm
		{
			get
			{
				return this.keyExchangeAlgorithm;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000DEC0 File Offset: 0x0000CEC0
		public int ExchangeStrength
		{
			get
			{
				return this.exchangeStrength;
			}
		}

		// Token: 0x04000291 RID: 657
		private SecurityProtocol securityProtocol;

		// Token: 0x04000292 RID: 658
		private CipherAlgorithmType identifier;

		// Token: 0x04000293 RID: 659
		private int strength;

		// Token: 0x04000294 RID: 660
		private HashAlgorithmType hashAlgorithm;

		// Token: 0x04000295 RID: 661
		private int hashStrength;

		// Token: 0x04000296 RID: 662
		private int keyExchangeAlgorithm;

		// Token: 0x04000297 RID: 663
		private int exchangeStrength;
	}
}

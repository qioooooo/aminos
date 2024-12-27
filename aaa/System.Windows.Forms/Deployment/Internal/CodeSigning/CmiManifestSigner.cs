using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200013D RID: 317
	internal class CmiManifestSigner
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x0000AFD9 File Offset: 0x00009FD9
		private CmiManifestSigner()
		{
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000AFE1 File Offset: 0x00009FE1
		internal CmiManifestSigner(AsymmetricAlgorithm strongNameKey)
			: this(strongNameKey, null)
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000AFEC File Offset: 0x00009FEC
		internal CmiManifestSigner(AsymmetricAlgorithm strongNameKey, X509Certificate2 certificate)
		{
			if (strongNameKey == null)
			{
				throw new ArgumentNullException("strongNameKey");
			}
			if (!(strongNameKey is RSA))
			{
				throw new ArgumentNullException("strongNameKey");
			}
			this.m_strongNameKey = strongNameKey;
			this.m_certificate = certificate;
			this.m_certificates = new X509Certificate2Collection();
			this.m_includeOption = X509IncludeOption.ExcludeRoot;
			this.m_signerFlag = CmiManifestSignerFlag.None;
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0000B049 File Offset: 0x0000A049
		internal AsymmetricAlgorithm StrongNameKey
		{
			get
			{
				return this.m_strongNameKey;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x0000B051 File Offset: 0x0000A051
		internal X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0000B059 File Offset: 0x0000A059
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0000B061 File Offset: 0x0000A061
		internal string Description
		{
			get
			{
				return this.m_description;
			}
			set
			{
				this.m_description = value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0000B06A File Offset: 0x0000A06A
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0000B072 File Offset: 0x0000A072
		internal string DescriptionUrl
		{
			get
			{
				return this.m_url;
			}
			set
			{
				this.m_url = value;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0000B07B File Offset: 0x0000A07B
		internal X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0000B083 File Offset: 0x0000A083
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0000B08B File Offset: 0x0000A08B
		internal X509IncludeOption IncludeOption
		{
			get
			{
				return this.m_includeOption;
			}
			set
			{
				if (value < X509IncludeOption.None || value > X509IncludeOption.WholeChain)
				{
					throw new ArgumentException("value");
				}
				if (this.m_includeOption == X509IncludeOption.None)
				{
					throw new NotSupportedException();
				}
				this.m_includeOption = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0000B0B5 File Offset: 0x0000A0B5
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0000B0BD File Offset: 0x0000A0BD
		internal CmiManifestSignerFlag Flag
		{
			get
			{
				return this.m_signerFlag;
			}
			set
			{
				if ((value & ~CmiManifestSignerFlag.DontReplacePublicKeyToken) != CmiManifestSignerFlag.None)
				{
					throw new ArgumentException("value");
				}
				this.m_signerFlag = value;
			}
		}

		// Token: 0x04000ECA RID: 3786
		internal const uint CimManifestSignerFlagMask = 1U;

		// Token: 0x04000ECB RID: 3787
		private AsymmetricAlgorithm m_strongNameKey;

		// Token: 0x04000ECC RID: 3788
		private X509Certificate2 m_certificate;

		// Token: 0x04000ECD RID: 3789
		private string m_description;

		// Token: 0x04000ECE RID: 3790
		private string m_url;

		// Token: 0x04000ECF RID: 3791
		private X509Certificate2Collection m_certificates;

		// Token: 0x04000ED0 RID: 3792
		private X509IncludeOption m_includeOption;

		// Token: 0x04000ED1 RID: 3793
		private CmiManifestSignerFlag m_signerFlag;
	}
}

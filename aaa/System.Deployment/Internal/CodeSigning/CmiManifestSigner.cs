using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001E5 RID: 485
	internal class CmiManifestSigner
	{
		// Token: 0x0600089C RID: 2204 RVA: 0x00022879 File Offset: 0x00021879
		private CmiManifestSigner()
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00022881 File Offset: 0x00021881
		internal CmiManifestSigner(AsymmetricAlgorithm strongNameKey)
			: this(strongNameKey, null)
		{
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002288C File Offset: 0x0002188C
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

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x000228E9 File Offset: 0x000218E9
		internal AsymmetricAlgorithm StrongNameKey
		{
			get
			{
				return this.m_strongNameKey;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x000228F1 File Offset: 0x000218F1
		internal X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x000228F9 File Offset: 0x000218F9
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00022901 File Offset: 0x00021901
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

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0002290A File Offset: 0x0002190A
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x00022912 File Offset: 0x00021912
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

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0002291B File Offset: 0x0002191B
		internal X509Certificate2Collection ExtraStore
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00022923 File Offset: 0x00021923
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x0002292B File Offset: 0x0002192B
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

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00022955 File Offset: 0x00021955
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0002295D File Offset: 0x0002195D
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

		// Token: 0x04000827 RID: 2087
		internal const uint CimManifestSignerFlagMask = 1U;

		// Token: 0x04000828 RID: 2088
		private AsymmetricAlgorithm m_strongNameKey;

		// Token: 0x04000829 RID: 2089
		private X509Certificate2 m_certificate;

		// Token: 0x0400082A RID: 2090
		private string m_description;

		// Token: 0x0400082B RID: 2091
		private string m_url;

		// Token: 0x0400082C RID: 2092
		private X509Certificate2Collection m_certificates;

		// Token: 0x0400082D RID: 2093
		private X509IncludeOption m_includeOption;

		// Token: 0x0400082E RID: 2094
		private CmiManifestSignerFlag m_signerFlag;
	}
}

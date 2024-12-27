using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000062 RID: 98
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CmsSigner
	{
		// Token: 0x060000FD RID: 253 RVA: 0x000064E3 File Offset: 0x000054E3
		public CmsSigner()
			: this(SubjectIdentifierType.IssuerAndSerialNumber, null)
		{
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000064ED File Offset: 0x000054ED
		public CmsSigner(SubjectIdentifierType signerIdentifierType)
			: this(signerIdentifierType, null)
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000064F7 File Offset: 0x000054F7
		public CmsSigner(X509Certificate2 certificate)
			: this(SubjectIdentifierType.IssuerAndSerialNumber, certificate)
		{
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006501 File Offset: 0x00005501
		public CmsSigner(CspParameters parameters)
			: this(SubjectIdentifierType.SubjectKeyIdentifier, PkcsUtils.CreateDummyCertificate(parameters))
		{
			this.m_dummyCert = true;
			this.IncludeOption = X509IncludeOption.None;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006520 File Offset: 0x00005520
		public CmsSigner(SubjectIdentifierType signerIdentifierType, X509Certificate2 certificate)
		{
			switch (signerIdentifierType)
			{
			case SubjectIdentifierType.Unknown:
				this.SignerIdentifierType = SubjectIdentifierType.IssuerAndSerialNumber;
				this.IncludeOption = X509IncludeOption.ExcludeRoot;
				break;
			case SubjectIdentifierType.IssuerAndSerialNumber:
				this.SignerIdentifierType = signerIdentifierType;
				this.IncludeOption = X509IncludeOption.ExcludeRoot;
				break;
			case SubjectIdentifierType.SubjectKeyIdentifier:
				this.SignerIdentifierType = signerIdentifierType;
				this.IncludeOption = X509IncludeOption.ExcludeRoot;
				break;
			case SubjectIdentifierType.NoSignature:
				this.SignerIdentifierType = signerIdentifierType;
				this.IncludeOption = X509IncludeOption.None;
				break;
			default:
				this.SignerIdentifierType = SubjectIdentifierType.IssuerAndSerialNumber;
				this.IncludeOption = X509IncludeOption.ExcludeRoot;
				break;
			}
			this.Certificate = certificate;
			this.DigestAlgorithm = new Oid("1.3.14.3.2.26");
			this.m_signedAttributes = new CryptographicAttributeObjectCollection();
			this.m_unsignedAttributes = new CryptographicAttributeObjectCollection();
			this.m_certificates = new X509Certificate2Collection();
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000065D3 File Offset: 0x000055D3
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000065DC File Offset: 0x000055DC
		public SubjectIdentifierType SignerIdentifierType
		{
			get
			{
				return this.m_signerIdentifierType;
			}
			set
			{
				if (value != SubjectIdentifierType.IssuerAndSerialNumber && value != SubjectIdentifierType.SubjectKeyIdentifier && value != SubjectIdentifierType.NoSignature)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				if (this.m_dummyCert && value != SubjectIdentifierType.SubjectKeyIdentifier)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_signerIdentifierType = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000665C File Offset: 0x0000565C
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00006664 File Offset: 0x00005664
		public X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
			set
			{
				this.m_certificate = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000666D File Offset: 0x0000566D
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00006675 File Offset: 0x00005675
		public Oid DigestAlgorithm
		{
			get
			{
				return this.m_digestAlgorithm;
			}
			set
			{
				this.m_digestAlgorithm = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000108 RID: 264 RVA: 0x0000667E File Offset: 0x0000567E
		public CryptographicAttributeObjectCollection SignedAttributes
		{
			get
			{
				return this.m_signedAttributes;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00006686 File Offset: 0x00005686
		public CryptographicAttributeObjectCollection UnsignedAttributes
		{
			get
			{
				return this.m_unsignedAttributes;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600010A RID: 266 RVA: 0x0000668E File Offset: 0x0000568E
		public X509Certificate2Collection Certificates
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00006696 File Offset: 0x00005696
		// (set) Token: 0x0600010C RID: 268 RVA: 0x000066A0 File Offset: 0x000056A0
		public X509IncludeOption IncludeOption
		{
			get
			{
				return this.m_includeOption;
			}
			set
			{
				if (value < X509IncludeOption.None || value > X509IncludeOption.WholeChain)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Arg_EnumIllegalVal"), new object[] { "value" }));
				}
				this.m_includeOption = value;
			}
		}

		// Token: 0x04000457 RID: 1111
		private SubjectIdentifierType m_signerIdentifierType;

		// Token: 0x04000458 RID: 1112
		private X509Certificate2 m_certificate;

		// Token: 0x04000459 RID: 1113
		private Oid m_digestAlgorithm;

		// Token: 0x0400045A RID: 1114
		private CryptographicAttributeObjectCollection m_signedAttributes;

		// Token: 0x0400045B RID: 1115
		private CryptographicAttributeObjectCollection m_unsignedAttributes;

		// Token: 0x0400045C RID: 1116
		private X509Certificate2Collection m_certificates;

		// Token: 0x0400045D RID: 1117
		private X509IncludeOption m_includeOption;

		// Token: 0x0400045E RID: 1118
		private bool m_dummyCert;
	}
}

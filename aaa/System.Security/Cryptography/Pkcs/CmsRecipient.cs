using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200005F RID: 95
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CmsRecipient
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00006204 File Offset: 0x00005204
		private CmsRecipient()
		{
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000620C File Offset: 0x0000520C
		public CmsRecipient(X509Certificate2 certificate)
			: this(SubjectIdentifierType.IssuerAndSerialNumber, certificate)
		{
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006216 File Offset: 0x00005216
		public CmsRecipient(SubjectIdentifierType recipientIdentifierType, X509Certificate2 certificate)
		{
			this.Reset(recipientIdentifierType, certificate);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006226 File Offset: 0x00005226
		public SubjectIdentifierType RecipientIdentifierType
		{
			get
			{
				return this.m_recipientIdentifierType;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x0000622E File Offset: 0x0000522E
		public X509Certificate2 Certificate
		{
			get
			{
				return this.m_certificate;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006238 File Offset: 0x00005238
		private void Reset(SubjectIdentifierType recipientIdentifierType, X509Certificate2 certificate)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			switch (recipientIdentifierType)
			{
			case SubjectIdentifierType.Unknown:
				recipientIdentifierType = SubjectIdentifierType.IssuerAndSerialNumber;
				break;
			case SubjectIdentifierType.IssuerAndSerialNumber:
				break;
			case SubjectIdentifierType.SubjectKeyIdentifier:
				if (!PkcsUtils.CmsSupported())
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Not_Supported"));
				}
				break;
			default:
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type"), recipientIdentifierType.ToString());
			}
			this.m_recipientIdentifierType = recipientIdentifierType;
			this.m_certificate = certificate;
		}

		// Token: 0x04000452 RID: 1106
		private SubjectIdentifierType m_recipientIdentifierType;

		// Token: 0x04000453 RID: 1107
		private X509Certificate2 m_certificate;
	}
}

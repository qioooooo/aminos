using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000075 RID: 117
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SubjectIdentifierOrKey
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00007A3A File Offset: 0x00006A3A
		private SubjectIdentifierOrKey()
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007A42 File Offset: 0x00006A42
		internal SubjectIdentifierOrKey(SubjectIdentifierOrKeyType type, object value)
		{
			this.Reset(type, value);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007A54 File Offset: 0x00006A54
		internal SubjectIdentifierOrKey(CAPIBase.CERT_ID certId)
		{
			switch (certId.dwIdChoice)
			{
			case 1U:
			{
				X509IssuerSerial x509IssuerSerial = PkcsUtils.DecodeIssuerSerial(certId.Value.IssuerSerialNumber);
				this.Reset(SubjectIdentifierOrKeyType.IssuerAndSerialNumber, x509IssuerSerial);
				return;
			}
			case 2U:
			{
				byte[] array = new byte[certId.Value.KeyId.cbData];
				Marshal.Copy(certId.Value.KeyId.pbData, array, 0, array.Length);
				this.Reset(SubjectIdentifierOrKeyType.SubjectKeyIdentifier, X509Utils.EncodeHexString(array));
				return;
			}
			default:
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type"), certId.dwIdChoice.ToString(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007B02 File Offset: 0x00006B02
		internal SubjectIdentifierOrKey(CAPIBase.CERT_PUBLIC_KEY_INFO publicKeyInfo)
		{
			this.Reset(SubjectIdentifierOrKeyType.PublicKeyInfo, new PublicKeyInfo(publicKeyInfo));
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00007B17 File Offset: 0x00006B17
		public SubjectIdentifierOrKeyType Type
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007B1F File Offset: 0x00006B1F
		public object Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007B28 File Offset: 0x00006B28
		internal void Reset(SubjectIdentifierOrKeyType type, object value)
		{
			switch (type)
			{
			case SubjectIdentifierOrKeyType.Unknown:
				break;
			case SubjectIdentifierOrKeyType.IssuerAndSerialNumber:
				if (value.GetType() != typeof(X509IssuerSerial))
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch"), value.GetType().ToString());
				}
				break;
			case SubjectIdentifierOrKeyType.SubjectKeyIdentifier:
				if (!PkcsUtils.CmsSupported())
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Not_Supported"));
				}
				if (value.GetType() != typeof(string))
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch"), value.GetType().ToString());
				}
				break;
			case SubjectIdentifierOrKeyType.PublicKeyInfo:
				if (!PkcsUtils.CmsSupported())
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Not_Supported"));
				}
				if (value.GetType() != typeof(PublicKeyInfo))
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch"), value.GetType().ToString());
				}
				break;
			default:
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type"), type.ToString());
			}
			this.m_type = type;
			this.m_value = value;
		}

		// Token: 0x0400049D RID: 1181
		private SubjectIdentifierOrKeyType m_type;

		// Token: 0x0400049E RID: 1182
		private object m_value;
	}
}

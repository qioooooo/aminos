using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000072 RID: 114
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SubjectIdentifier
	{
		// Token: 0x06000164 RID: 356 RVA: 0x000075AB File Offset: 0x000065AB
		private SubjectIdentifier()
		{
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000075B3 File Offset: 0x000065B3
		internal SubjectIdentifier(CAPIBase.CERT_INFO certInfo)
			: this(certInfo.Issuer, certInfo.SerialNumber)
		{
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000075C9 File Offset: 0x000065C9
		internal SubjectIdentifier(CAPIBase.CMSG_SIGNER_INFO signerInfo)
			: this(signerInfo.Issuer, signerInfo.SerialNumber)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000075DF File Offset: 0x000065DF
		internal SubjectIdentifier(SubjectIdentifierType type, object value)
		{
			this.Reset(type, value);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000075F0 File Offset: 0x000065F0
		internal unsafe SubjectIdentifier(CAPIBase.CRYPTOAPI_BLOB issuer, CAPIBase.CRYPTOAPI_BLOB serialNumber)
		{
			bool flag = true;
			byte* ptr = (byte*)(void*)serialNumber.pbData;
			for (uint num = 0U; num < serialNumber.cbData; num += 1U)
			{
				if (*(ptr++) != 0)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				byte[] array = new byte[issuer.cbData];
				Marshal.Copy(issuer.pbData, array, 0, array.Length);
				X500DistinguishedName x500DistinguishedName = new X500DistinguishedName(array);
				if (string.Compare("CN=Dummy Signer", x500DistinguishedName.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.Reset(SubjectIdentifierType.NoSignature, null);
					return;
				}
			}
			if (flag)
			{
				this.m_type = SubjectIdentifierType.SubjectKeyIdentifier;
				this.m_value = string.Empty;
				uint num2 = 0U;
				SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
				if (CAPI.DecodeObject(new IntPtr(7L), issuer.pbData, issuer.cbData, out invalidHandle, out num2))
				{
					using (invalidHandle)
					{
						CAPIBase.CERT_NAME_INFO cert_NAME_INFO = (CAPIBase.CERT_NAME_INFO)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_NAME_INFO));
						for (uint num3 = 0U; num3 < cert_NAME_INFO.cRDN; num3 += 1U)
						{
							CAPIBase.CERT_RDN cert_RDN = (CAPIBase.CERT_RDN)Marshal.PtrToStructure(new IntPtr((long)cert_NAME_INFO.rgRDN + (long)((ulong)num3 * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CERT_RDN))))), typeof(CAPIBase.CERT_RDN));
							for (uint num4 = 0U; num4 < cert_RDN.cRDNAttr; num4 += 1U)
							{
								CAPIBase.CERT_RDN_ATTR cert_RDN_ATTR = (CAPIBase.CERT_RDN_ATTR)Marshal.PtrToStructure(new IntPtr((long)cert_RDN.rgRDNAttr + (long)((ulong)num4 * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CERT_RDN_ATTR))))), typeof(CAPIBase.CERT_RDN_ATTR));
								if (string.Compare("1.3.6.1.4.1.311.10.7.1", cert_RDN_ATTR.pszObjId, StringComparison.OrdinalIgnoreCase) == 0 && cert_RDN_ATTR.dwValueType == 2U)
								{
									byte[] array2 = new byte[cert_RDN_ATTR.Value.cbData];
									Marshal.Copy(cert_RDN_ATTR.Value.pbData, array2, 0, array2.Length);
									this.Reset(SubjectIdentifierType.SubjectKeyIdentifier, X509Utils.EncodeHexString(array2));
									return;
								}
							}
						}
					}
				}
			}
			CAPIBase.CERT_ISSUER_SERIAL_NUMBER cert_ISSUER_SERIAL_NUMBER;
			cert_ISSUER_SERIAL_NUMBER.Issuer = issuer;
			cert_ISSUER_SERIAL_NUMBER.SerialNumber = serialNumber;
			X509IssuerSerial x509IssuerSerial = PkcsUtils.DecodeIssuerSerial(cert_ISSUER_SERIAL_NUMBER);
			this.Reset(SubjectIdentifierType.IssuerAndSerialNumber, x509IssuerSerial);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000783C File Offset: 0x0000683C
		internal SubjectIdentifier(CAPIBase.CERT_ID certId)
		{
			switch (certId.dwIdChoice)
			{
			case 1U:
			{
				X509IssuerSerial x509IssuerSerial = PkcsUtils.DecodeIssuerSerial(certId.Value.IssuerSerialNumber);
				this.Reset(SubjectIdentifierType.IssuerAndSerialNumber, x509IssuerSerial);
				return;
			}
			case 2U:
			{
				byte[] array = new byte[certId.Value.KeyId.cbData];
				Marshal.Copy(certId.Value.KeyId.pbData, array, 0, array.Length);
				this.Reset(SubjectIdentifierType.SubjectKeyIdentifier, X509Utils.EncodeHexString(array));
				return;
			}
			default:
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type"), certId.dwIdChoice.ToString(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600016A RID: 362 RVA: 0x000078EA File Offset: 0x000068EA
		public SubjectIdentifierType Type
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000078F2 File Offset: 0x000068F2
		public object Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000078FC File Offset: 0x000068FC
		internal void Reset(SubjectIdentifierType type, object value)
		{
			switch (type)
			{
			case SubjectIdentifierType.Unknown:
			case SubjectIdentifierType.NoSignature:
				break;
			case SubjectIdentifierType.IssuerAndSerialNumber:
				if (value.GetType() != typeof(X509IssuerSerial))
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch"), value.GetType().ToString());
				}
				break;
			case SubjectIdentifierType.SubjectKeyIdentifier:
				if (!PkcsUtils.CmsSupported())
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Not_Supported"));
				}
				if (value.GetType() != typeof(string))
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

		// Token: 0x04000494 RID: 1172
		private SubjectIdentifierType m_type;

		// Token: 0x04000495 RID: 1173
		private object m_value;
	}
}

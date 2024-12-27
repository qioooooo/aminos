using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200005C RID: 92
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EnvelopedCms
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000040EB File Offset: 0x000030EB
		public EnvelopedCms()
			: this(SubjectIdentifierType.IssuerAndSerialNumber, new ContentInfo("1.2.840.113549.1.7.1", new byte[0]), new AlgorithmIdentifier("1.2.840.113549.3.7"))
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000410E File Offset: 0x0000310E
		public EnvelopedCms(ContentInfo contentInfo)
			: this(SubjectIdentifierType.IssuerAndSerialNumber, contentInfo, new AlgorithmIdentifier("1.2.840.113549.3.7"))
		{
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004122 File Offset: 0x00003122
		public EnvelopedCms(SubjectIdentifierType recipientIdentifierType, ContentInfo contentInfo)
			: this(recipientIdentifierType, contentInfo, new AlgorithmIdentifier("1.2.840.113549.3.7"))
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004136 File Offset: 0x00003136
		public EnvelopedCms(ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm)
			: this(SubjectIdentifierType.IssuerAndSerialNumber, contentInfo, encryptionAlgorithm)
		{
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004144 File Offset: 0x00003144
		public EnvelopedCms(SubjectIdentifierType recipientIdentifierType, ContentInfo contentInfo, AlgorithmIdentifier encryptionAlgorithm)
		{
			if (contentInfo == null)
			{
				throw new ArgumentNullException("contentInfo");
			}
			if (contentInfo.Content == null)
			{
				throw new ArgumentNullException("contentInfo.Content");
			}
			if (encryptionAlgorithm == null)
			{
				throw new ArgumentNullException("encryptionAlgorithm");
			}
			this.m_safeCryptMsgHandle = SafeCryptMsgHandle.InvalidHandle;
			this.m_version = ((recipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier) ? 2 : 0);
			this.m_recipientIdentifierType = recipientIdentifierType;
			this.m_contentInfo = contentInfo;
			this.m_encryptionAlgorithm = encryptionAlgorithm;
			this.m_encryptionAlgorithm.Parameters = new byte[0];
			this.m_certificates = new X509Certificate2Collection();
			this.m_unprotectedAttributes = new CryptographicAttributeObjectCollection();
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000041DB File Offset: 0x000031DB
		public int Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000041E3 File Offset: 0x000031E3
		public ContentInfo ContentInfo
		{
			get
			{
				return this.m_contentInfo;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000041EB File Offset: 0x000031EB
		public AlgorithmIdentifier ContentEncryptionAlgorithm
		{
			get
			{
				return this.m_encryptionAlgorithm;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000041F3 File Offset: 0x000031F3
		public X509Certificate2Collection Certificates
		{
			get
			{
				return this.m_certificates;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000041FB File Offset: 0x000031FB
		public CryptographicAttributeObjectCollection UnprotectedAttributes
		{
			get
			{
				return this.m_unprotectedAttributes;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00004203 File Offset: 0x00003203
		public RecipientInfoCollection RecipientInfos
		{
			get
			{
				if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
				{
					return new RecipientInfoCollection();
				}
				return new RecipientInfoCollection(this.m_safeCryptMsgHandle);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000422B File Offset: 0x0000322B
		public byte[] Encode()
		{
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_MessageNotEncrypted"));
			}
			return PkcsUtils.GetContent(this.m_safeCryptMsgHandle);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004260 File Offset: 0x00003260
		public void Decode(byte[] encodedMessage)
		{
			if (encodedMessage == null)
			{
				throw new ArgumentNullException("encodedMessage");
			}
			if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
			{
				this.m_safeCryptMsgHandle.Dispose();
			}
			this.m_safeCryptMsgHandle = EnvelopedCms.OpenToDecode(encodedMessage);
			this.m_version = (int)PkcsUtils.GetVersion(this.m_safeCryptMsgHandle);
			Oid contentType = PkcsUtils.GetContentType(this.m_safeCryptMsgHandle);
			byte[] content = PkcsUtils.GetContent(this.m_safeCryptMsgHandle);
			this.m_contentInfo = new ContentInfo(contentType, content);
			this.m_encryptionAlgorithm = PkcsUtils.GetAlgorithmIdentifier(this.m_safeCryptMsgHandle);
			this.m_certificates = PkcsUtils.GetCertificates(this.m_safeCryptMsgHandle);
			this.m_unprotectedAttributes = PkcsUtils.GetUnprotectedAttributes(this.m_safeCryptMsgHandle);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004310 File Offset: 0x00003310
		public void Encrypt()
		{
			this.Encrypt(new CmsRecipientCollection());
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000431D File Offset: 0x0000331D
		public void Encrypt(CmsRecipient recipient)
		{
			if (recipient == null)
			{
				throw new ArgumentNullException("recipient");
			}
			this.Encrypt(new CmsRecipientCollection(recipient));
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000433C File Offset: 0x0000333C
		public void Encrypt(CmsRecipientCollection recipients)
		{
			if (recipients == null)
			{
				throw new ArgumentNullException("recipients");
			}
			if (this.ContentInfo.Content.Length == 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Envelope_Empty_Content"));
			}
			if (recipients.Count == 0)
			{
				recipients = PkcsUtils.SelectRecipients(this.m_recipientIdentifierType);
			}
			this.EncryptContent(recipients);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004392 File Offset: 0x00003392
		public void Decrypt()
		{
			this.DecryptContent(this.RecipientInfos, null);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000043A1 File Offset: 0x000033A1
		public void Decrypt(RecipientInfo recipientInfo)
		{
			if (recipientInfo == null)
			{
				throw new ArgumentNullException("recipientInfo");
			}
			this.DecryptContent(new RecipientInfoCollection(recipientInfo), null);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000043BE File Offset: 0x000033BE
		public void Decrypt(X509Certificate2Collection extraStore)
		{
			if (extraStore == null)
			{
				throw new ArgumentNullException("extraStore");
			}
			this.DecryptContent(this.RecipientInfos, extraStore);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000043DB File Offset: 0x000033DB
		public void Decrypt(RecipientInfo recipientInfo, X509Certificate2Collection extraStore)
		{
			if (recipientInfo == null)
			{
				throw new ArgumentNullException("recipientInfo");
			}
			if (extraStore == null)
			{
				throw new ArgumentNullException("extraStore");
			}
			this.DecryptContent(new RecipientInfoCollection(recipientInfo), extraStore);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004408 File Offset: 0x00003408
		private unsafe void DecryptContent(RecipientInfoCollection recipientInfos, X509Certificate2Collection extraStore)
		{
			int num = -2146889717;
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_NoEncryptedMessageToEncode"));
			}
			for (int i = 0; i < recipientInfos.Count; i++)
			{
				RecipientInfo recipientInfo = recipientInfos[i];
				EnvelopedCms.CMSG_DECRYPT_PARAM cmsg_DECRYPT_PARAM = default(EnvelopedCms.CMSG_DECRYPT_PARAM);
				int num2 = EnvelopedCms.GetCspParams(recipientInfo, extraStore, ref cmsg_DECRYPT_PARAM);
				if (num2 == 0)
				{
					CspParameters cspParameters = new CspParameters();
					if (!X509Utils.GetPrivateKeyInfo(cmsg_DECRYPT_PARAM.safeCertContextHandle, ref cspParameters))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
					KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(cspParameters, KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Decrypt);
					keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
					keyContainerPermission.Demand();
					switch (recipientInfo.Type)
					{
					case RecipientInfoType.KeyTransport:
					{
						CAPIBase.CMSG_CTRL_DECRYPT_PARA cmsg_CTRL_DECRYPT_PARA = new CAPIBase.CMSG_CTRL_DECRYPT_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_DECRYPT_PARA)));
						cmsg_CTRL_DECRYPT_PARA.hCryptProv = cmsg_DECRYPT_PARAM.safeCryptProvHandle.DangerousGetHandle();
						cmsg_CTRL_DECRYPT_PARA.dwKeySpec = cmsg_DECRYPT_PARAM.keySpec;
						cmsg_CTRL_DECRYPT_PARA.dwRecipientIndex = recipientInfo.Index;
						if (!CAPI.CryptMsgControl(this.m_safeCryptMsgHandle, 0U, 2U, new IntPtr((void*)(&cmsg_CTRL_DECRYPT_PARA))))
						{
							num2 = Marshal.GetHRForLastWin32Error();
						}
						GC.KeepAlive(cmsg_CTRL_DECRYPT_PARA);
						break;
					}
					case RecipientInfoType.KeyAgreement:
					{
						SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
						KeyAgreeRecipientInfo keyAgreeRecipientInfo = (KeyAgreeRecipientInfo)recipientInfo;
						CAPIBase.CMSG_CMS_RECIPIENT_INFO cmsg_CMS_RECIPIENT_INFO = (CAPIBase.CMSG_CMS_RECIPIENT_INFO)Marshal.PtrToStructure(keyAgreeRecipientInfo.pCmsgRecipientInfo.DangerousGetHandle(), typeof(CAPIBase.CMSG_CMS_RECIPIENT_INFO));
						CAPIBase.CMSG_CTRL_KEY_AGREE_DECRYPT_PARA cmsg_CTRL_KEY_AGREE_DECRYPT_PARA = new CAPIBase.CMSG_CTRL_KEY_AGREE_DECRYPT_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_KEY_AGREE_DECRYPT_PARA)));
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.hCryptProv = cmsg_DECRYPT_PARAM.safeCryptProvHandle.DangerousGetHandle();
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.dwKeySpec = cmsg_DECRYPT_PARAM.keySpec;
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.pKeyAgree = cmsg_CMS_RECIPIENT_INFO.pRecipientInfo;
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.dwRecipientIndex = keyAgreeRecipientInfo.Index;
						cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.dwRecipientEncryptedKeyIndex = keyAgreeRecipientInfo.SubIndex;
						if (keyAgreeRecipientInfo.SubType == RecipientSubType.CertIdKeyAgreement)
						{
							CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO = (CAPIBase.CMSG_KEY_AGREE_CERT_ID_RECIPIENT_INFO)keyAgreeRecipientInfo.CmsgRecipientInfo;
							SafeCertStoreHandle safeCertStoreHandle = EnvelopedCms.BuildOriginatorStore(this.Certificates, extraStore);
							safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 1048576U, new IntPtr((void*)(&cmsg_KEY_AGREE_CERT_ID_RECIPIENT_INFO.OriginatorCertId)), SafeCertContextHandle.InvalidHandle);
							if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
							{
								num2 = -2146885628;
								break;
							}
							cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.OriginatorPublicKey = ((CAPIBase.CERT_INFO)Marshal.PtrToStructure(((CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(safeCertContextHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT))).pCertInfo, typeof(CAPIBase.CERT_INFO))).SubjectPublicKeyInfo.PublicKey;
						}
						else
						{
							cmsg_CTRL_KEY_AGREE_DECRYPT_PARA.OriginatorPublicKey = ((CAPIBase.CMSG_KEY_AGREE_PUBLIC_KEY_RECIPIENT_INFO)keyAgreeRecipientInfo.CmsgRecipientInfo).OriginatorPublicKeyInfo.PublicKey;
						}
						if (!CAPI.CryptMsgControl(this.m_safeCryptMsgHandle, 0U, 17U, new IntPtr((void*)(&cmsg_CTRL_KEY_AGREE_DECRYPT_PARA))))
						{
							num2 = Marshal.GetHRForLastWin32Error();
						}
						GC.KeepAlive(cmsg_CTRL_KEY_AGREE_DECRYPT_PARA);
						GC.KeepAlive(safeCertContextHandle);
						break;
					}
					default:
						throw new CryptographicException(-2147483647);
					}
					GC.KeepAlive(cmsg_DECRYPT_PARAM);
				}
				if (num2 == 0)
				{
					uint num3 = 0U;
					SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
					PkcsUtils.GetParam(this.m_safeCryptMsgHandle, 2U, 0U, out invalidHandle, out num3);
					if (num3 > 0U)
					{
						Oid contentType = PkcsUtils.GetContentType(this.m_safeCryptMsgHandle);
						byte[] array = new byte[num3];
						Marshal.Copy(invalidHandle.DangerousGetHandle(), array, 0, (int)num3);
						this.m_contentInfo = new ContentInfo(contentType, array);
					}
					invalidHandle.Dispose();
					num = 0;
					break;
				}
				num = num2;
			}
			if (num != 0)
			{
				throw new CryptographicException(num);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000478C File Offset: 0x0000378C
		private unsafe void EncryptContent(CmsRecipientCollection recipients)
		{
			EnvelopedCms.CMSG_ENCRYPT_PARAM cmsg_ENCRYPT_PARAM = default(EnvelopedCms.CMSG_ENCRYPT_PARAM);
			if (recipients.Count < 1)
			{
				throw new CryptographicException(-2146889717);
			}
			foreach (CmsRecipient cmsRecipient in recipients)
			{
				if (cmsRecipient.Certificate == null)
				{
					throw new ArgumentNullException(SecurityResources.GetResourceString("Cryptography_Cms_RecipientCertificateNotFound"));
				}
				if (PkcsUtils.GetRecipientInfoType(cmsRecipient.Certificate) == RecipientInfoType.KeyAgreement || cmsRecipient.RecipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
				{
					cmsg_ENCRYPT_PARAM.useCms = true;
				}
			}
			if (!cmsg_ENCRYPT_PARAM.useCms && (this.Certificates.Count > 0 || this.UnprotectedAttributes.Count > 0))
			{
				cmsg_ENCRYPT_PARAM.useCms = true;
			}
			if (cmsg_ENCRYPT_PARAM.useCms && !PkcsUtils.CmsSupported())
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Not_Supported"));
			}
			CAPIBase.CMSG_ENVELOPED_ENCODE_INFO cmsg_ENVELOPED_ENCODE_INFO = new CAPIBase.CMSG_ENVELOPED_ENCODE_INFO(Marshal.SizeOf(typeof(CAPIBase.CMSG_ENVELOPED_ENCODE_INFO)));
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_ENVELOPED_ENCODE_INFO))));
			EnvelopedCms.SetCspParams(this.ContentEncryptionAlgorithm, ref cmsg_ENCRYPT_PARAM);
			cmsg_ENVELOPED_ENCODE_INFO.ContentEncryptionAlgorithm.pszObjId = this.ContentEncryptionAlgorithm.Oid.Value;
			if (cmsg_ENCRYPT_PARAM.pvEncryptionAuxInfo != null && !cmsg_ENCRYPT_PARAM.pvEncryptionAuxInfo.IsInvalid)
			{
				cmsg_ENVELOPED_ENCODE_INFO.pvEncryptionAuxInfo = cmsg_ENCRYPT_PARAM.pvEncryptionAuxInfo.DangerousGetHandle();
			}
			cmsg_ENVELOPED_ENCODE_INFO.cRecipients = (uint)recipients.Count;
			List<SafeCertContextHandle> list = null;
			if (cmsg_ENCRYPT_PARAM.useCms)
			{
				EnvelopedCms.SetCmsRecipientParams(recipients, this.Certificates, this.UnprotectedAttributes, this.ContentEncryptionAlgorithm, ref cmsg_ENCRYPT_PARAM);
				cmsg_ENVELOPED_ENCODE_INFO.rgCmsRecipients = cmsg_ENCRYPT_PARAM.rgpRecipients.DangerousGetHandle();
				if (cmsg_ENCRYPT_PARAM.rgCertEncoded != null && !cmsg_ENCRYPT_PARAM.rgCertEncoded.IsInvalid)
				{
					cmsg_ENVELOPED_ENCODE_INFO.cCertEncoded = (uint)this.Certificates.Count;
					cmsg_ENVELOPED_ENCODE_INFO.rgCertEncoded = cmsg_ENCRYPT_PARAM.rgCertEncoded.DangerousGetHandle();
				}
				if (cmsg_ENCRYPT_PARAM.rgUnprotectedAttr != null && !cmsg_ENCRYPT_PARAM.rgUnprotectedAttr.IsInvalid)
				{
					cmsg_ENVELOPED_ENCODE_INFO.cUnprotectedAttr = (uint)this.UnprotectedAttributes.Count;
					cmsg_ENVELOPED_ENCODE_INFO.rgUnprotectedAttr = cmsg_ENCRYPT_PARAM.rgUnprotectedAttr.DangerousGetHandle();
				}
			}
			else
			{
				EnvelopedCms.SetPkcs7RecipientParams(recipients, ref cmsg_ENCRYPT_PARAM, out list);
				cmsg_ENVELOPED_ENCODE_INFO.rgpRecipients = cmsg_ENCRYPT_PARAM.rgpRecipients.DangerousGetHandle();
			}
			Marshal.StructureToPtr(cmsg_ENVELOPED_ENCODE_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
			try
			{
				SafeCryptMsgHandle safeCryptMsgHandle = CAPI.CryptMsgOpenToEncode(65537U, 0U, 3U, safeLocalAllocHandle.DangerousGetHandle(), this.ContentInfo.ContentType.Value, IntPtr.Zero);
				if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
				{
					this.m_safeCryptMsgHandle.Dispose();
				}
				this.m_safeCryptMsgHandle = safeCryptMsgHandle;
			}
			finally
			{
				Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_ENVELOPED_ENCODE_INFO));
				safeLocalAllocHandle.Dispose();
			}
			byte[] array = new byte[0];
			if (string.Compare(this.ContentInfo.ContentType.Value, "1.2.840.113549.1.7.1", StringComparison.OrdinalIgnoreCase) == 0)
			{
				byte[] content = this.ContentInfo.Content;
				fixed (byte* ptr = content)
				{
					CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
					cryptoapi_BLOB.cbData = (uint)content.Length;
					cryptoapi_BLOB.pbData = new IntPtr((void*)ptr);
					if (!CAPI.EncodeObject(new IntPtr(25L), new IntPtr((void*)(&cryptoapi_BLOB)), out array))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
				}
			}
			else
			{
				array = this.ContentInfo.Content;
			}
			if (array.Length > 0 && !CAPISafe.CryptMsgUpdate(this.m_safeCryptMsgHandle, array, (uint)array.Length, true))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			GC.KeepAlive(cmsg_ENCRYPT_PARAM);
			GC.KeepAlive(recipients);
			GC.KeepAlive(list);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004B48 File Offset: 0x00003B48
		private static SafeCryptMsgHandle OpenToDecode(byte[] encodedMessage)
		{
			SafeCryptMsgHandle safeCryptMsgHandle = CAPISafe.CryptMsgOpenToDecode(65537U, 0U, 0U, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (!CAPISafe.CryptMsgUpdate(safeCryptMsgHandle, encodedMessage, (uint)encodedMessage.Length, true))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (3U != PkcsUtils.GetMessageType(safeCryptMsgHandle))
			{
				throw new CryptographicException(-2146889724);
			}
			return safeCryptMsgHandle;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004BB8 File Offset: 0x00003BB8
		private unsafe static int GetCspParams(RecipientInfo recipientInfo, X509Certificate2Collection extraStore, ref EnvelopedCms.CMSG_DECRYPT_PARAM cmsgDecryptParam)
		{
			int num = -2146889717;
			SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
			SafeCertStoreHandle safeCertStoreHandle = EnvelopedCms.BuildDecryptorStore(extraStore);
			switch (recipientInfo.Type)
			{
			case RecipientInfoType.KeyTransport:
				if (recipientInfo.SubType == RecipientSubType.Pkcs7KeyTransport)
				{
					safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 720896U, recipientInfo.pCmsgRecipientInfo.DangerousGetHandle(), SafeCertContextHandle.InvalidHandle);
				}
				else
				{
					safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 1048576U, new IntPtr((void*)(&((CAPIBase.CMSG_KEY_TRANS_RECIPIENT_INFO)recipientInfo.CmsgRecipientInfo).RecipientId)), SafeCertContextHandle.InvalidHandle);
				}
				break;
			case RecipientInfoType.KeyAgreement:
			{
				KeyAgreeRecipientInfo keyAgreeRecipientInfo = (KeyAgreeRecipientInfo)recipientInfo;
				CAPIBase.CERT_ID recipientId = keyAgreeRecipientInfo.RecipientId;
				safeCertContextHandle = CAPI.CertFindCertificateInStore(safeCertStoreHandle, 65537U, 0U, 1048576U, new IntPtr((void*)(&recipientId)), SafeCertContextHandle.InvalidHandle);
				break;
			}
			default:
				num = -2147483647;
				break;
			}
			if (safeCertContextHandle != null && !safeCertContextHandle.IsInvalid)
			{
				SafeCryptProvHandle invalidHandle = SafeCryptProvHandle.InvalidHandle;
				uint num2 = 0U;
				bool flag = false;
				CspParameters cspParameters = new CspParameters();
				if (!X509Utils.GetPrivateKeyInfo(safeCertContextHandle, ref cspParameters))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				if (string.Compare(cspParameters.ProviderName, "Microsoft Base Cryptographic Provider v1.0", StringComparison.OrdinalIgnoreCase) == 0 && (CAPI.CryptAcquireContext(ref invalidHandle, cspParameters.KeyContainerName, "Microsoft Enhanced Cryptographic Provider v1.0", 1U, 0U) || CAPI.CryptAcquireContext(ref invalidHandle, cspParameters.KeyContainerName, "Microsoft Strong Cryptographic Provider", 1U, 0U)))
				{
					cmsgDecryptParam.safeCryptProvHandle = invalidHandle;
				}
				cmsgDecryptParam.safeCertContextHandle = safeCertContextHandle;
				cmsgDecryptParam.keySpec = (uint)cspParameters.KeyNumber;
				num = 0;
				if (invalidHandle == null || invalidHandle.IsInvalid)
				{
					if (CAPISafe.CryptAcquireCertificatePrivateKey(safeCertContextHandle, 6U, IntPtr.Zero, ref invalidHandle, ref num2, ref flag))
					{
						if (!flag)
						{
							GC.SuppressFinalize(invalidHandle);
						}
						cmsgDecryptParam.safeCryptProvHandle = invalidHandle;
					}
					else
					{
						num = Marshal.GetHRForLastWin32Error();
					}
				}
			}
			return num;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004D64 File Offset: 0x00003D64
		private static void SetCspParams(AlgorithmIdentifier contentEncryptionAlgorithm, ref EnvelopedCms.CMSG_ENCRYPT_PARAM encryptParam)
		{
			encryptParam.safeCryptProvHandle = SafeCryptProvHandle.InvalidHandle;
			encryptParam.pvEncryptionAuxInfo = SafeLocalAllocHandle.InvalidHandle;
			SafeCryptProvHandle invalidHandle = SafeCryptProvHandle.InvalidHandle;
			if (!CAPI.CryptAcquireContext(ref invalidHandle, IntPtr.Zero, IntPtr.Zero, 1U, 4026531840U) && !CAPI.CryptAcquireContext(ref invalidHandle, IntPtr.Zero, IntPtr.Zero, 1U, 0U))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			uint num = X509Utils.OidToAlgId(contentEncryptionAlgorithm.Oid.Value);
			if (num == 26114U || num == 26625U)
			{
				CAPIBase.CMSG_RC2_AUX_INFO cmsg_RC2_AUX_INFO = new CAPIBase.CMSG_RC2_AUX_INFO(Marshal.SizeOf(typeof(CAPIBase.CMSG_RC2_AUX_INFO)));
				uint num2 = (uint)contentEncryptionAlgorithm.KeyLength;
				if (num2 == 0U)
				{
					num2 = (uint)PkcsUtils.GetMaxKeyLength(invalidHandle, num);
				}
				cmsg_RC2_AUX_INFO.dwBitLen = num2;
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_RC2_AUX_INFO))));
				Marshal.StructureToPtr(cmsg_RC2_AUX_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
				encryptParam.pvEncryptionAuxInfo = safeLocalAllocHandle;
			}
			encryptParam.safeCryptProvHandle = invalidHandle;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004E58 File Offset: 0x00003E58
		private unsafe static void SetCmsRecipientParams(CmsRecipientCollection recipients, X509Certificate2Collection certificates, CryptographicAttributeObjectCollection unprotectedAttributes, AlgorithmIdentifier contentEncryptionAlgorithm, ref EnvelopedCms.CMSG_ENCRYPT_PARAM encryptParam)
		{
			uint[] array = new uint[recipients.Count];
			int num = 0;
			int num2 = recipients.Count * Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCODE_INFO));
			int num3 = num2;
			for (int i = 0; i < recipients.Count; i++)
			{
				array[i] = (uint)PkcsUtils.GetRecipientInfoType(recipients[i].Certificate);
				if (array[i] == 1U)
				{
					num3 += Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO));
				}
				else
				{
					if (array[i] != 2U)
					{
						throw new CryptographicException(-2146889726);
					}
					num++;
					num3 += Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO));
				}
			}
			encryptParam.rgpRecipients = CAPI.LocalAlloc(64U, new IntPtr(num3));
			encryptParam.rgCertEncoded = SafeLocalAllocHandle.InvalidHandle;
			encryptParam.rgUnprotectedAttr = SafeLocalAllocHandle.InvalidHandle;
			encryptParam.rgSubjectKeyIdentifier = new SafeLocalAllocHandle[recipients.Count];
			encryptParam.rgszObjId = new SafeLocalAllocHandle[recipients.Count];
			if (num > 0)
			{
				encryptParam.rgszKeyWrapObjId = new SafeLocalAllocHandle[num];
				encryptParam.rgKeyWrapAuxInfo = new SafeLocalAllocHandle[num];
				encryptParam.rgEphemeralIdentifier = new SafeLocalAllocHandle[num];
				encryptParam.rgszEphemeralObjId = new SafeLocalAllocHandle[num];
				encryptParam.rgUserKeyingMaterial = new SafeLocalAllocHandle[num];
				encryptParam.prgpEncryptedKey = new SafeLocalAllocHandle[num];
				encryptParam.rgpEncryptedKey = new SafeLocalAllocHandle[num];
			}
			if (certificates.Count > 0)
			{
				encryptParam.rgCertEncoded = CAPI.LocalAlloc(64U, new IntPtr(certificates.Count * Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
				for (int i = 0; i < certificates.Count; i++)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = (CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(X509Utils.GetCertContext(certificates[i]).DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT));
					CAPIBase.CRYPTOAPI_BLOB* ptr = (CAPIBase.CRYPTOAPI_BLOB*)(void*)new IntPtr((long)encryptParam.rgCertEncoded.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
					ptr->cbData = cert_CONTEXT.cbCertEncoded;
					ptr->pbData = cert_CONTEXT.pbCertEncoded;
				}
			}
			if (unprotectedAttributes.Count > 0)
			{
				encryptParam.rgUnprotectedAttr = new SafeLocalAllocHandle(PkcsUtils.CreateCryptAttributes(unprotectedAttributes));
			}
			num = 0;
			IntPtr intPtr = new IntPtr((long)encryptParam.rgpRecipients.DangerousGetHandle() + (long)num2);
			for (int i = 0; i < recipients.Count; i++)
			{
				CmsRecipient cmsRecipient = recipients[i];
				X509Certificate2 certificate = cmsRecipient.Certificate;
				CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(((CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(X509Utils.GetCertContext(certificate).DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT))).pCertInfo, typeof(CAPIBase.CERT_INFO));
				CAPIBase.CMSG_RECIPIENT_ENCODE_INFO* ptr2 = (CAPIBase.CMSG_RECIPIENT_ENCODE_INFO*)(void*)new IntPtr((long)encryptParam.rgpRecipients.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCODE_INFO))));
				ptr2->dwRecipientChoice = array[i];
				ptr2->pRecipientInfo = intPtr;
				if (array[i] == 1U)
				{
					IntPtr intPtr2 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "cbSize"));
					Marshal.WriteInt32(intPtr2, Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO)));
					IntPtr intPtr3 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "KeyEncryptionAlgorithm"));
					byte[] bytes = Encoding.ASCII.GetBytes(cert_INFO.SubjectPublicKeyInfo.Algorithm.pszObjId);
					encryptParam.rgszObjId[i] = CAPI.LocalAlloc(64U, new IntPtr(bytes.Length + 1));
					Marshal.Copy(bytes, 0, encryptParam.rgszObjId[i].DangerousGetHandle(), bytes.Length);
					IntPtr intPtr4 = new IntPtr((long)intPtr3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(intPtr4, encryptParam.rgszObjId[i].DangerousGetHandle());
					IntPtr intPtr5 = new IntPtr((long)intPtr3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "Parameters"));
					IntPtr intPtr6 = new IntPtr((long)intPtr5 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
					Marshal.WriteInt32(intPtr6, (int)cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.cbData);
					IntPtr intPtr7 = new IntPtr((long)intPtr5 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
					Marshal.WriteIntPtr(intPtr7, cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.pbData);
					IntPtr intPtr8 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "RecipientPublicKey"));
					intPtr6 = new IntPtr((long)intPtr8 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cbData"));
					Marshal.WriteInt32(intPtr6, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cbData);
					intPtr7 = new IntPtr((long)intPtr8 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "pbData"));
					Marshal.WriteIntPtr(intPtr7, cert_INFO.SubjectPublicKeyInfo.PublicKey.pbData);
					IntPtr intPtr9 = new IntPtr((long)intPtr8 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cUnusedBits"));
					Marshal.WriteInt32(intPtr9, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cUnusedBits);
					IntPtr intPtr10 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO), "RecipientId"));
					if (cmsRecipient.RecipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
					{
						uint num4 = 0U;
						SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle, ref num4))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num4)));
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle, ref num4))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						encryptParam.rgSubjectKeyIdentifier[i] = safeLocalAllocHandle;
						IntPtr intPtr11 = new IntPtr((long)intPtr10 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "dwIdChoice"));
						Marshal.WriteInt32(intPtr11, 2);
						IntPtr intPtr12 = new IntPtr((long)intPtr10 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						intPtr6 = new IntPtr((long)intPtr12 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(intPtr6, (int)num4);
						intPtr7 = new IntPtr((long)intPtr12 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(intPtr7, safeLocalAllocHandle.DangerousGetHandle());
					}
					else
					{
						IntPtr intPtr13 = new IntPtr((long)intPtr10 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "dwIdChoice"));
						Marshal.WriteInt32(intPtr13, 1);
						IntPtr intPtr14 = new IntPtr((long)intPtr10 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						IntPtr intPtr15 = new IntPtr((long)intPtr14 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "Issuer"));
						intPtr6 = new IntPtr((long)intPtr15 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(intPtr6, (int)cert_INFO.Issuer.cbData);
						intPtr7 = new IntPtr((long)intPtr15 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(intPtr7, cert_INFO.Issuer.pbData);
						IntPtr intPtr16 = new IntPtr((long)intPtr14 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "SerialNumber"));
						intPtr6 = new IntPtr((long)intPtr16 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(intPtr6, (int)cert_INFO.SerialNumber.cbData);
						intPtr7 = new IntPtr((long)intPtr16 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(intPtr7, cert_INFO.SerialNumber.pbData);
					}
					intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_TRANS_RECIPIENT_ENCODE_INFO)));
				}
				else if (array[i] == 2U)
				{
					IntPtr intPtr17 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "cbSize"));
					Marshal.WriteInt32(intPtr17, Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO)));
					IntPtr intPtr18 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "KeyEncryptionAlgorithm"));
					byte[] array2 = Encoding.ASCII.GetBytes("1.2.840.113549.1.9.16.3.5");
					encryptParam.rgszObjId[i] = CAPI.LocalAlloc(64U, new IntPtr(array2.Length + 1));
					Marshal.Copy(array2, 0, encryptParam.rgszObjId[i].DangerousGetHandle(), array2.Length);
					IntPtr intPtr19 = new IntPtr((long)intPtr18 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(intPtr19, encryptParam.rgszObjId[i].DangerousGetHandle());
					IntPtr intPtr20 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "KeyWrapAlgorithm"));
					uint num5 = X509Utils.OidToAlgId(contentEncryptionAlgorithm.Oid.Value);
					if (num5 == 26114U)
					{
						array2 = Encoding.ASCII.GetBytes("1.2.840.113549.1.9.16.3.7");
					}
					else
					{
						array2 = Encoding.ASCII.GetBytes("1.2.840.113549.1.9.16.3.6");
					}
					encryptParam.rgszKeyWrapObjId[num] = CAPI.LocalAlloc(64U, new IntPtr(array2.Length + 1));
					Marshal.Copy(array2, 0, encryptParam.rgszKeyWrapObjId[num].DangerousGetHandle(), array2.Length);
					intPtr19 = new IntPtr((long)intPtr20 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(intPtr19, encryptParam.rgszKeyWrapObjId[num].DangerousGetHandle());
					if (num5 == 26114U)
					{
						IntPtr intPtr21 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "pvKeyWrapAuxInfo"));
						Marshal.WriteIntPtr(intPtr21, encryptParam.pvEncryptionAuxInfo.DangerousGetHandle());
					}
					IntPtr intPtr22 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "dwKeyChoice"));
					Marshal.WriteInt32(intPtr22, 1);
					IntPtr intPtr23 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "pEphemeralAlgorithmOrSenderId"));
					encryptParam.rgEphemeralIdentifier[num] = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER))));
					Marshal.WriteIntPtr(intPtr23, encryptParam.rgEphemeralIdentifier[num].DangerousGetHandle());
					array2 = Encoding.ASCII.GetBytes(cert_INFO.SubjectPublicKeyInfo.Algorithm.pszObjId);
					encryptParam.rgszEphemeralObjId[num] = CAPI.LocalAlloc(64U, new IntPtr(array2.Length + 1));
					Marshal.Copy(array2, 0, encryptParam.rgszEphemeralObjId[num].DangerousGetHandle(), array2.Length);
					intPtr19 = new IntPtr((long)encryptParam.rgEphemeralIdentifier[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "pszObjId"));
					Marshal.WriteIntPtr(intPtr19, encryptParam.rgszEphemeralObjId[num].DangerousGetHandle());
					IntPtr intPtr24 = new IntPtr((long)encryptParam.rgEphemeralIdentifier[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "Parameters"));
					IntPtr intPtr25 = new IntPtr((long)intPtr24 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
					Marshal.WriteInt32(intPtr25, (int)cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.cbData);
					IntPtr intPtr26 = new IntPtr((long)intPtr24 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
					Marshal.WriteIntPtr(intPtr26, cert_INFO.SubjectPublicKeyInfo.Algorithm.Parameters.pbData);
					IntPtr intPtr27 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "cRecipientEncryptedKeys"));
					Marshal.WriteInt32(intPtr27, 1);
					encryptParam.prgpEncryptedKey[num] = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(IntPtr))));
					IntPtr intPtr28 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO), "rgpRecipientEncryptedKeys"));
					Marshal.WriteIntPtr(intPtr28, encryptParam.prgpEncryptedKey[num].DangerousGetHandle());
					encryptParam.rgpEncryptedKey[num] = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO))));
					Marshal.WriteIntPtr(encryptParam.prgpEncryptedKey[num].DangerousGetHandle(), encryptParam.rgpEncryptedKey[num].DangerousGetHandle());
					intPtr17 = new IntPtr((long)encryptParam.rgpEncryptedKey[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO), "cbSize"));
					Marshal.WriteInt32(intPtr17, Marshal.SizeOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO)));
					IntPtr intPtr29 = new IntPtr((long)encryptParam.rgpEncryptedKey[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO), "RecipientPublicKey"));
					intPtr25 = new IntPtr((long)intPtr29 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cbData"));
					Marshal.WriteInt32(intPtr25, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cbData);
					intPtr26 = new IntPtr((long)intPtr29 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "pbData"));
					Marshal.WriteIntPtr(intPtr26, cert_INFO.SubjectPublicKeyInfo.PublicKey.pbData);
					IntPtr intPtr30 = new IntPtr((long)intPtr29 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_BIT_BLOB), "cUnusedBits"));
					Marshal.WriteInt32(intPtr30, (int)cert_INFO.SubjectPublicKeyInfo.PublicKey.cUnusedBits);
					IntPtr intPtr31 = new IntPtr((long)encryptParam.rgpEncryptedKey[num].DangerousGetHandle() + (long)Marshal.OffsetOf(typeof(CAPIBase.CMSG_RECIPIENT_ENCRYPTED_KEY_ENCODE_INFO), "RecipientId"));
					IntPtr intPtr32 = new IntPtr((long)intPtr31 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "dwIdChoice"));
					if (cmsRecipient.RecipientIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
					{
						Marshal.WriteInt32(intPtr32, 2);
						IntPtr intPtr33 = new IntPtr((long)intPtr31 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						uint num6 = 0U;
						SafeLocalAllocHandle safeLocalAllocHandle2 = SafeLocalAllocHandle.InvalidHandle;
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle2, ref num6))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						safeLocalAllocHandle2 = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num6)));
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 20U, safeLocalAllocHandle2, ref num6))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						encryptParam.rgSubjectKeyIdentifier[num] = safeLocalAllocHandle2;
						intPtr25 = new IntPtr((long)intPtr33 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(intPtr25, (int)num6);
						intPtr26 = new IntPtr((long)intPtr33 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(intPtr26, safeLocalAllocHandle2.DangerousGetHandle());
					}
					else
					{
						Marshal.WriteInt32(intPtr32, 1);
						IntPtr intPtr34 = new IntPtr((long)intPtr31 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ID), "Value"));
						IntPtr intPtr35 = new IntPtr((long)intPtr34 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "Issuer"));
						intPtr25 = new IntPtr((long)intPtr35 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(intPtr25, (int)cert_INFO.Issuer.cbData);
						intPtr26 = new IntPtr((long)intPtr35 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(intPtr26, cert_INFO.Issuer.pbData);
						IntPtr intPtr36 = new IntPtr((long)intPtr34 + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_ISSUER_SERIAL_NUMBER), "SerialNumber"));
						intPtr25 = new IntPtr((long)intPtr36 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
						Marshal.WriteInt32(intPtr25, (int)cert_INFO.SerialNumber.cbData);
						intPtr26 = new IntPtr((long)intPtr36 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
						Marshal.WriteIntPtr(intPtr26, cert_INFO.SerialNumber.pbData);
					}
					num++;
					intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(CAPIBase.CMSG_KEY_AGREE_RECIPIENT_ENCODE_INFO)));
				}
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005FFC File Offset: 0x00004FFC
		private static void SetPkcs7RecipientParams(CmsRecipientCollection recipients, ref EnvelopedCms.CMSG_ENCRYPT_PARAM encryptParam, out List<SafeCertContextHandle> certContexts)
		{
			int count = recipients.Count;
			certContexts = new List<SafeCertContextHandle>();
			uint num = (uint)(count * Marshal.SizeOf(typeof(IntPtr)));
			encryptParam.rgpRecipients = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num)));
			IntPtr intPtr = encryptParam.rgpRecipients.DangerousGetHandle();
			for (int i = 0; i < count; i++)
			{
				SafeCertContextHandle certContext = X509Utils.GetCertContext(recipients[i].Certificate);
				certContexts.Add(certContext);
				IntPtr intPtr2 = certContext.DangerousGetHandle();
				CAPIBase.CERT_CONTEXT cert_CONTEXT = (CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(intPtr2, typeof(CAPIBase.CERT_CONTEXT));
				Marshal.WriteIntPtr(intPtr, cert_CONTEXT.pCertInfo);
				intPtr = new IntPtr((long)intPtr + (long)Marshal.SizeOf(typeof(IntPtr)));
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000060C0 File Offset: 0x000050C0
		private static SafeCertStoreHandle BuildDecryptorStore(X509Certificate2Collection extraStore)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			try
			{
				X509Store x509Store = new X509Store("MY", StoreLocation.CurrentUser);
				x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store.Certificates);
			}
			catch (SecurityException)
			{
			}
			try
			{
				X509Store x509Store2 = new X509Store("MY", StoreLocation.LocalMachine);
				x509Store2.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store2.Certificates);
			}
			catch (SecurityException)
			{
			}
			if (extraStore != null)
			{
				x509Certificate2Collection.AddRange(extraStore);
			}
			if (x509Certificate2Collection.Count == 0)
			{
				throw new CryptographicException(-2146889717);
			}
			return X509Utils.ExportToMemoryStore(x509Certificate2Collection);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000615C File Offset: 0x0000515C
		private static SafeCertStoreHandle BuildOriginatorStore(X509Certificate2Collection bagOfCerts, X509Certificate2Collection extraStore)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			try
			{
				X509Store x509Store = new X509Store("AddressBook", StoreLocation.CurrentUser);
				x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store.Certificates);
			}
			catch (SecurityException)
			{
			}
			try
			{
				X509Store x509Store2 = new X509Store("AddressBook", StoreLocation.LocalMachine);
				x509Store2.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
				x509Certificate2Collection.AddRange(x509Store2.Certificates);
			}
			catch (SecurityException)
			{
			}
			if (bagOfCerts != null)
			{
				x509Certificate2Collection.AddRange(bagOfCerts);
			}
			if (extraStore != null)
			{
				x509Certificate2Collection.AddRange(extraStore);
			}
			if (x509Certificate2Collection.Count == 0)
			{
				throw new CryptographicException(-2146885628);
			}
			return X509Utils.ExportToMemoryStore(x509Certificate2Collection);
		}

		// Token: 0x04000439 RID: 1081
		private SafeCryptMsgHandle m_safeCryptMsgHandle;

		// Token: 0x0400043A RID: 1082
		private int m_version;

		// Token: 0x0400043B RID: 1083
		private SubjectIdentifierType m_recipientIdentifierType;

		// Token: 0x0400043C RID: 1084
		private ContentInfo m_contentInfo;

		// Token: 0x0400043D RID: 1085
		private AlgorithmIdentifier m_encryptionAlgorithm;

		// Token: 0x0400043E RID: 1086
		private X509Certificate2Collection m_certificates;

		// Token: 0x0400043F RID: 1087
		private CryptographicAttributeObjectCollection m_unprotectedAttributes;

		// Token: 0x0200005D RID: 93
		private struct CMSG_DECRYPT_PARAM
		{
			// Token: 0x04000440 RID: 1088
			internal SafeCertContextHandle safeCertContextHandle;

			// Token: 0x04000441 RID: 1089
			internal SafeCryptProvHandle safeCryptProvHandle;

			// Token: 0x04000442 RID: 1090
			internal uint keySpec;
		}

		// Token: 0x0200005E RID: 94
		private struct CMSG_ENCRYPT_PARAM
		{
			// Token: 0x04000443 RID: 1091
			internal bool useCms;

			// Token: 0x04000444 RID: 1092
			internal SafeCryptProvHandle safeCryptProvHandle;

			// Token: 0x04000445 RID: 1093
			internal SafeLocalAllocHandle pvEncryptionAuxInfo;

			// Token: 0x04000446 RID: 1094
			internal SafeLocalAllocHandle rgpRecipients;

			// Token: 0x04000447 RID: 1095
			internal SafeLocalAllocHandle rgCertEncoded;

			// Token: 0x04000448 RID: 1096
			internal SafeLocalAllocHandle rgUnprotectedAttr;

			// Token: 0x04000449 RID: 1097
			internal SafeLocalAllocHandle[] rgSubjectKeyIdentifier;

			// Token: 0x0400044A RID: 1098
			internal SafeLocalAllocHandle[] rgszObjId;

			// Token: 0x0400044B RID: 1099
			internal SafeLocalAllocHandle[] rgszKeyWrapObjId;

			// Token: 0x0400044C RID: 1100
			internal SafeLocalAllocHandle[] rgKeyWrapAuxInfo;

			// Token: 0x0400044D RID: 1101
			internal SafeLocalAllocHandle[] rgEphemeralIdentifier;

			// Token: 0x0400044E RID: 1102
			internal SafeLocalAllocHandle[] rgszEphemeralObjId;

			// Token: 0x0400044F RID: 1103
			internal SafeLocalAllocHandle[] rgUserKeyingMaterial;

			// Token: 0x04000450 RID: 1104
			internal SafeLocalAllocHandle[] prgpEncryptedKey;

			// Token: 0x04000451 RID: 1105
			internal SafeLocalAllocHandle[] rgpEncryptedKey;
		}
	}
}

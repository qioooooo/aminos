using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200007A RID: 122
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SignedCms
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00009AC5 File Offset: 0x00008AC5
		public SignedCms()
			: this(SubjectIdentifierType.IssuerAndSerialNumber, new ContentInfo(new Oid("1.2.840.113549.1.7.1"), new byte[0]), false)
		{
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009AE4 File Offset: 0x00008AE4
		public SignedCms(SubjectIdentifierType signerIdentifierType)
			: this(signerIdentifierType, new ContentInfo(new Oid("1.2.840.113549.1.7.1"), new byte[0]), false)
		{
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009B03 File Offset: 0x00008B03
		public SignedCms(ContentInfo contentInfo)
			: this(SubjectIdentifierType.IssuerAndSerialNumber, contentInfo, false)
		{
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009B0E File Offset: 0x00008B0E
		public SignedCms(SubjectIdentifierType signerIdentifierType, ContentInfo contentInfo)
			: this(signerIdentifierType, contentInfo, false)
		{
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009B19 File Offset: 0x00008B19
		public SignedCms(ContentInfo contentInfo, bool detached)
			: this(SubjectIdentifierType.IssuerAndSerialNumber, contentInfo, detached)
		{
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009B24 File Offset: 0x00008B24
		public SignedCms(SubjectIdentifierType signerIdentifierType, ContentInfo contentInfo, bool detached)
		{
			if (contentInfo == null)
			{
				throw new ArgumentNullException("contentInfo");
			}
			if (contentInfo.Content == null)
			{
				throw new ArgumentNullException("contentInfo.Content");
			}
			if (signerIdentifierType != SubjectIdentifierType.SubjectKeyIdentifier && signerIdentifierType != SubjectIdentifierType.IssuerAndSerialNumber && signerIdentifierType != SubjectIdentifierType.NoSignature)
			{
				signerIdentifierType = SubjectIdentifierType.IssuerAndSerialNumber;
			}
			this.m_safeCryptMsgHandle = SafeCryptMsgHandle.InvalidHandle;
			this.m_signerIdentifierType = signerIdentifierType;
			this.m_version = 0;
			this.m_contentInfo = contentInfo;
			this.m_detached = detached;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00009B8E File Offset: 0x00008B8E
		public int Version
		{
			get
			{
				if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
				{
					return this.m_version;
				}
				return (int)PkcsUtils.GetVersion(this.m_safeCryptMsgHandle);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00009BB7 File Offset: 0x00008BB7
		public ContentInfo ContentInfo
		{
			get
			{
				return this.m_contentInfo;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00009BBF File Offset: 0x00008BBF
		public bool Detached
		{
			get
			{
				return this.m_detached;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00009BC7 File Offset: 0x00008BC7
		public X509Certificate2Collection Certificates
		{
			get
			{
				if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
				{
					return new X509Certificate2Collection();
				}
				return PkcsUtils.GetCertificates(this.m_safeCryptMsgHandle);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00009BEF File Offset: 0x00008BEF
		public SignerInfoCollection SignerInfos
		{
			get
			{
				if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
				{
					return new SignerInfoCollection();
				}
				return new SignerInfoCollection(this);
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009C12 File Offset: 0x00008C12
		public byte[] Encode()
		{
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_MessageNotSigned"));
			}
			return PkcsUtils.GetMessage(this.m_safeCryptMsgHandle);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009C44 File Offset: 0x00008C44
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
			this.m_safeCryptMsgHandle = SignedCms.OpenToDecode(encodedMessage, this.ContentInfo, this.Detached);
			if (!this.Detached)
			{
				Oid contentType = PkcsUtils.GetContentType(this.m_safeCryptMsgHandle);
				byte[] content = PkcsUtils.GetContent(this.m_safeCryptMsgHandle);
				this.m_contentInfo = new ContentInfo(contentType, content);
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009CC4 File Offset: 0x00008CC4
		public void ComputeSignature()
		{
			this.ComputeSignature(new CmsSigner(this.m_signerIdentifierType), true);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009CD8 File Offset: 0x00008CD8
		public void ComputeSignature(CmsSigner signer)
		{
			this.ComputeSignature(signer, true);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009CE4 File Offset: 0x00008CE4
		public void ComputeSignature(CmsSigner signer, bool silent)
		{
			if (signer == null)
			{
				throw new ArgumentNullException("signer");
			}
			if (this.ContentInfo.Content.Length == 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Sign_Empty_Content"));
			}
			if (SubjectIdentifierType.NoSignature == signer.SignerIdentifierType)
			{
				if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Cms_Sign_No_Signature_First_Signer"));
				}
				this.Sign(signer, silent);
				return;
			}
			else
			{
				if (signer.Certificate == null)
				{
					if (silent)
					{
						throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_RecipientCertificateNotFound"));
					}
					signer.Certificate = PkcsUtils.SelectSignerCertificate();
				}
				if (!signer.Certificate.HasPrivateKey)
				{
					throw new CryptographicException(-2146893811);
				}
				CspParameters cspParameters = new CspParameters();
				if (!X509Utils.GetPrivateKeyInfo(X509Utils.GetCertContext(signer.Certificate), ref cspParameters))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
				KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(cspParameters, KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Sign);
				keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
				keyContainerPermission.Demand();
				if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
				{
					this.Sign(signer, silent);
					return;
				}
				this.CoSign(signer, silent);
				return;
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009E04 File Offset: 0x00008E04
		public unsafe void RemoveSignature(int index)
		{
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_MessageNotSigned"));
			}
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			if (!CAPISafe.CryptMsgGetParam(this.m_safeCryptMsgHandle, 5U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (index < 0 || index >= (int)num)
			{
				throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (!CAPI.CryptMsgControl(this.m_safeCryptMsgHandle, 0U, 7U, new IntPtr((void*)(&index))))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009EB1 File Offset: 0x00008EB1
		public void RemoveSignature(SignerInfo signerInfo)
		{
			if (signerInfo == null)
			{
				throw new ArgumentNullException("signerInfo");
			}
			this.RemoveSignature(PkcsUtils.GetSignerIndex(this.m_safeCryptMsgHandle, signerInfo, 0));
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009ED4 File Offset: 0x00008ED4
		public void CheckSignature(bool verifySignatureOnly)
		{
			this.CheckSignature(new X509Certificate2Collection(), verifySignatureOnly);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00009EE4 File Offset: 0x00008EE4
		public void CheckSignature(X509Certificate2Collection extraStore, bool verifySignatureOnly)
		{
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_MessageNotSigned"));
			}
			if (extraStore == null)
			{
				throw new ArgumentNullException("extraStore");
			}
			SignedCms.CheckSignatures(this.SignerInfos, extraStore, verifySignatureOnly);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009F31 File Offset: 0x00008F31
		public void CheckHash()
		{
			if (this.m_safeCryptMsgHandle == null || this.m_safeCryptMsgHandle.IsInvalid)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Cryptography_Cms_MessageNotSigned"));
			}
			SignedCms.CheckHashes(this.SignerInfos);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009F63 File Offset: 0x00008F63
		internal SafeCryptMsgHandle GetCryptMsgHandle()
		{
			return this.m_safeCryptMsgHandle;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00009F6C File Offset: 0x00008F6C
		internal void ReopenToDecode()
		{
			byte[] message = PkcsUtils.GetMessage(this.m_safeCryptMsgHandle);
			if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
			{
				this.m_safeCryptMsgHandle.Dispose();
			}
			this.m_safeCryptMsgHandle = SignedCms.OpenToDecode(message, this.ContentInfo, this.Detached);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009FC0 File Offset: 0x00008FC0
		private unsafe void Sign(CmsSigner signer, bool silent)
		{
			CAPIBase.CMSG_SIGNED_ENCODE_INFO cmsg_SIGNED_ENCODE_INFO = new CAPIBase.CMSG_SIGNED_ENCODE_INFO(Marshal.SizeOf(typeof(CAPIBase.CMSG_SIGNED_ENCODE_INFO)));
			CAPIBase.CMSG_SIGNER_ENCODE_INFO cmsg_SIGNER_ENCODE_INFO = PkcsUtils.CreateSignerEncodeInfo(signer, silent);
			byte[] array = null;
			SafeCryptMsgHandle safeCryptMsgHandle;
			try
			{
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO))));
				try
				{
					Marshal.StructureToPtr(cmsg_SIGNER_ENCODE_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
					X509Certificate2Collection x509Certificate2Collection = PkcsUtils.CreateBagOfCertificates(signer);
					SafeLocalAllocHandle safeLocalAllocHandle2 = PkcsUtils.CreateEncodedCertBlob(x509Certificate2Collection);
					cmsg_SIGNED_ENCODE_INFO.cSigners = 1U;
					cmsg_SIGNED_ENCODE_INFO.rgSigners = safeLocalAllocHandle.DangerousGetHandle();
					cmsg_SIGNED_ENCODE_INFO.cCertEncoded = (uint)x509Certificate2Collection.Count;
					if (x509Certificate2Collection.Count > 0)
					{
						cmsg_SIGNED_ENCODE_INFO.rgCertEncoded = safeLocalAllocHandle2.DangerousGetHandle();
					}
					if (string.Compare(this.ContentInfo.ContentType.Value, "1.2.840.113549.1.7.1", StringComparison.OrdinalIgnoreCase) == 0)
					{
						safeCryptMsgHandle = CAPI.CryptMsgOpenToEncode(65537U, this.Detached ? 4U : 0U, 2U, new IntPtr((void*)(&cmsg_SIGNED_ENCODE_INFO)), IntPtr.Zero, IntPtr.Zero);
					}
					else
					{
						safeCryptMsgHandle = CAPI.CryptMsgOpenToEncode(65537U, this.Detached ? 4U : 0U, 2U, new IntPtr((void*)(&cmsg_SIGNED_ENCODE_INFO)), this.ContentInfo.ContentType.Value, IntPtr.Zero);
					}
					if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					if (this.ContentInfo.Content.Length > 0 && !CAPISafe.CryptMsgUpdate(safeCryptMsgHandle, this.ContentInfo.pContent, (uint)this.ContentInfo.Content.Length, true))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					array = PkcsUtils.GetContent(safeCryptMsgHandle);
					safeCryptMsgHandle.Dispose();
					safeLocalAllocHandle2.Dispose();
				}
				finally
				{
					Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO));
					safeLocalAllocHandle.Dispose();
				}
			}
			finally
			{
				cmsg_SIGNER_ENCODE_INFO.Dispose();
			}
			safeCryptMsgHandle = SignedCms.OpenToDecode(array, this.ContentInfo, this.Detached);
			if (this.m_safeCryptMsgHandle != null && !this.m_safeCryptMsgHandle.IsInvalid)
			{
				this.m_safeCryptMsgHandle.Dispose();
			}
			this.m_safeCryptMsgHandle = safeCryptMsgHandle;
			GC.KeepAlive(signer);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A1F4 File Offset: 0x000091F4
		private void CoSign(CmsSigner signer, bool silent)
		{
			using (CAPIBase.CMSG_SIGNER_ENCODE_INFO cmsg_SIGNER_ENCODE_INFO = PkcsUtils.CreateSignerEncodeInfo(signer, silent))
			{
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO))));
				try
				{
					Marshal.StructureToPtr(cmsg_SIGNER_ENCODE_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
					if (!CAPI.CryptMsgControl(this.m_safeCryptMsgHandle, 0U, 6U, safeLocalAllocHandle.DangerousGetHandle()))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
				}
				finally
				{
					Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO));
					safeLocalAllocHandle.Dispose();
				}
			}
			PkcsUtils.AddCertsToMessage(this.m_safeCryptMsgHandle, this.Certificates, PkcsUtils.CreateBagOfCertificates(signer));
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A2B4 File Offset: 0x000092B4
		private static SafeCryptMsgHandle OpenToDecode(byte[] encodedMessage, ContentInfo contentInfo, bool detached)
		{
			SafeCryptMsgHandle safeCryptMsgHandle = CAPISafe.CryptMsgOpenToDecode(65537U, detached ? 4U : 0U, 0U, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			if (safeCryptMsgHandle == null || safeCryptMsgHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (!CAPISafe.CryptMsgUpdate(safeCryptMsgHandle, encodedMessage, (uint)encodedMessage.Length, true))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (2U != PkcsUtils.GetMessageType(safeCryptMsgHandle))
			{
				throw new CryptographicException(-2146889724);
			}
			if (detached)
			{
				byte[] content = contentInfo.Content;
				if (content != null && content.Length > 0 && !CAPISafe.CryptMsgUpdate(safeCryptMsgHandle, content, (uint)content.Length, true))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return safeCryptMsgHandle;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A354 File Offset: 0x00009354
		private static void CheckSignatures(SignerInfoCollection signers, X509Certificate2Collection extraStore, bool verifySignatureOnly)
		{
			if (signers == null || signers.Count < 1)
			{
				throw new CryptographicException(-2146885618);
			}
			foreach (SignerInfo signerInfo in signers)
			{
				signerInfo.CheckSignature(extraStore, verifySignatureOnly);
				if (signerInfo.CounterSignerInfos.Count > 0)
				{
					SignedCms.CheckSignatures(signerInfo.CounterSignerInfos, extraStore, verifySignatureOnly);
				}
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A3B4 File Offset: 0x000093B4
		private static void CheckHashes(SignerInfoCollection signers)
		{
			if (signers == null || signers.Count < 1)
			{
				throw new CryptographicException(-2146885618);
			}
			foreach (SignerInfo signerInfo in signers)
			{
				if (signerInfo.SignerIdentifier.Type == SubjectIdentifierType.NoSignature)
				{
					signerInfo.CheckHash();
				}
			}
		}

		// Token: 0x040004AA RID: 1194
		private SafeCryptMsgHandle m_safeCryptMsgHandle;

		// Token: 0x040004AB RID: 1195
		private int m_version;

		// Token: 0x040004AC RID: 1196
		private SubjectIdentifierType m_signerIdentifierType;

		// Token: 0x040004AD RID: 1197
		private ContentInfo m_contentInfo;

		// Token: 0x040004AE RID: 1198
		private bool m_detached;
	}
}

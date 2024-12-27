using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x0200007B RID: 123
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class SignerInfo
	{
		// Token: 0x060001CE RID: 462 RVA: 0x0000A404 File Offset: 0x00009404
		private SignerInfo()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A40C File Offset: 0x0000940C
		internal SignerInfo(SignedCms signedCms, SafeLocalAllocHandle pbCmsgSignerInfo)
		{
			this.m_signedCms = signedCms;
			this.m_parentSignerInfo = null;
			this.m_encodedSignerInfo = null;
			this.m_pbCmsgSignerInfo = pbCmsgSignerInfo;
			this.m_cmsgSignerInfo = (CAPIBase.CMSG_SIGNER_INFO)Marshal.PtrToStructure(pbCmsgSignerInfo.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_INFO));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A45C File Offset: 0x0000945C
		internal unsafe SignerInfo(SignedCms signedCms, SignerInfo parentSignerInfo, byte[] encodedSignerInfo)
		{
			uint num = 0U;
			SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
			fixed (byte* ptr = &encodedSignerInfo[0])
			{
				if (!CAPI.DecodeObject(new IntPtr(500L), new IntPtr((void*)ptr), (uint)encodedSignerInfo.Length, out invalidHandle, out num))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			this.m_signedCms = signedCms;
			this.m_parentSignerInfo = parentSignerInfo;
			this.m_encodedSignerInfo = (byte[])encodedSignerInfo.Clone();
			this.m_pbCmsgSignerInfo = invalidHandle;
			this.m_cmsgSignerInfo = (CAPIBase.CMSG_SIGNER_INFO)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_INFO));
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000A4F3 File Offset: 0x000094F3
		public int Version
		{
			get
			{
				return (int)this.m_cmsgSignerInfo.dwVersion;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000A500 File Offset: 0x00009500
		public X509Certificate2 Certificate
		{
			get
			{
				if (this.m_certificate == null)
				{
					this.m_certificate = PkcsUtils.FindCertificate(this.SignerIdentifier, this.m_signedCms.Certificates);
				}
				return this.m_certificate;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000A52C File Offset: 0x0000952C
		public SubjectIdentifier SignerIdentifier
		{
			get
			{
				if (this.m_signerIdentifier == null)
				{
					this.m_signerIdentifier = new SubjectIdentifier(this.m_cmsgSignerInfo);
				}
				return this.m_signerIdentifier;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000A54D File Offset: 0x0000954D
		public Oid DigestAlgorithm
		{
			get
			{
				return new Oid(this.m_cmsgSignerInfo.HashAlgorithm.pszObjId);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000A564 File Offset: 0x00009564
		public CryptographicAttributeObjectCollection SignedAttributes
		{
			get
			{
				if (this.m_signedAttributes == null)
				{
					this.m_signedAttributes = new CryptographicAttributeObjectCollection(this.m_cmsgSignerInfo.AuthAttrs);
				}
				return this.m_signedAttributes;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000A58A File Offset: 0x0000958A
		public CryptographicAttributeObjectCollection UnsignedAttributes
		{
			get
			{
				if (this.m_unsignedAttributes == null)
				{
					this.m_unsignedAttributes = new CryptographicAttributeObjectCollection(this.m_cmsgSignerInfo.UnauthAttrs);
				}
				return this.m_unsignedAttributes;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000A5B0 File Offset: 0x000095B0
		public SignerInfoCollection CounterSignerInfos
		{
			get
			{
				if (this.m_parentSignerInfo != null)
				{
					return new SignerInfoCollection();
				}
				return new SignerInfoCollection(this.m_signedCms, this);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A5CC File Offset: 0x000095CC
		public void ComputeCounterSignature()
		{
			this.ComputeCounterSignature(new CmsSigner((this.m_signedCms.Version == 2) ? SubjectIdentifierType.SubjectKeyIdentifier : SubjectIdentifierType.IssuerAndSerialNumber));
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A5EC File Offset: 0x000095EC
		public void ComputeCounterSignature(CmsSigner signer)
		{
			if (this.m_parentSignerInfo != null)
			{
				throw new CryptographicException(-2147483647);
			}
			if (signer == null)
			{
				throw new ArgumentNullException("signer");
			}
			if (signer.Certificate == null)
			{
				signer.Certificate = PkcsUtils.SelectSignerCertificate();
			}
			if (!signer.Certificate.HasPrivateKey)
			{
				throw new CryptographicException(-2146893811);
			}
			this.CounterSign(signer);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A64C File Offset: 0x0000964C
		public void RemoveCounterSignature(int index)
		{
			if (this.m_parentSignerInfo != null)
			{
				throw new CryptographicException(-2147483647);
			}
			this.RemoveCounterSignature(PkcsUtils.GetSignerIndex(this.m_signedCms.GetCryptMsgHandle(), this, 0), index);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A67C File Offset: 0x0000967C
		public void RemoveCounterSignature(SignerInfo counterSignerInfo)
		{
			if (this.m_parentSignerInfo != null)
			{
				throw new CryptographicException(-2147483647);
			}
			if (counterSignerInfo == null)
			{
				throw new ArgumentNullException("counterSignerInfo");
			}
			foreach (CryptographicAttributeObject cryptographicAttributeObject in this.UnsignedAttributes)
			{
				if (string.Compare(cryptographicAttributeObject.Oid.Value, "1.2.840.113549.1.9.6", StringComparison.OrdinalIgnoreCase) == 0)
				{
					for (int i = 0; i < cryptographicAttributeObject.Values.Count; i++)
					{
						AsnEncodedData asnEncodedData = cryptographicAttributeObject.Values[i];
						SignerInfo signerInfo = new SignerInfo(this.m_signedCms, this.m_parentSignerInfo, asnEncodedData.RawData);
						if (counterSignerInfo.SignerIdentifier.Type == SubjectIdentifierType.IssuerAndSerialNumber && signerInfo.SignerIdentifier.Type == SubjectIdentifierType.IssuerAndSerialNumber)
						{
							X509IssuerSerial x509IssuerSerial = (X509IssuerSerial)counterSignerInfo.SignerIdentifier.Value;
							X509IssuerSerial x509IssuerSerial2 = (X509IssuerSerial)signerInfo.SignerIdentifier.Value;
							if (string.Compare(x509IssuerSerial.IssuerName, x509IssuerSerial2.IssuerName, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(x509IssuerSerial.SerialNumber, x509IssuerSerial2.SerialNumber, StringComparison.OrdinalIgnoreCase) == 0)
							{
								this.RemoveCounterSignature(PkcsUtils.GetSignerIndex(this.m_signedCms.GetCryptMsgHandle(), this, 0), i);
								return;
							}
						}
						else if (counterSignerInfo.SignerIdentifier.Type == SubjectIdentifierType.SubjectKeyIdentifier && signerInfo.SignerIdentifier.Type == SubjectIdentifierType.SubjectKeyIdentifier)
						{
							string text = counterSignerInfo.SignerIdentifier.Value as string;
							string text2 = signerInfo.SignerIdentifier.Value as string;
							if (string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) == 0)
							{
								this.RemoveCounterSignature(PkcsUtils.GetSignerIndex(this.m_signedCms.GetCryptMsgHandle(), this, 0), i);
								return;
							}
						}
					}
				}
			}
			throw new CryptographicException(-2146889714);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A831 File Offset: 0x00009831
		public void CheckSignature(bool verifySignatureOnly)
		{
			this.CheckSignature(new X509Certificate2Collection(), verifySignatureOnly);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000A840 File Offset: 0x00009840
		public void CheckSignature(X509Certificate2Collection extraStore, bool verifySignatureOnly)
		{
			if (extraStore == null)
			{
				throw new ArgumentNullException("extraStore");
			}
			X509Certificate2 x509Certificate = this.Certificate;
			if (x509Certificate == null)
			{
				x509Certificate = PkcsUtils.FindCertificate(this.SignerIdentifier, extraStore);
				if (x509Certificate == null)
				{
					throw new CryptographicException(-2146889714);
				}
			}
			this.Verify(extraStore, x509Certificate, verifySignatureOnly);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000A88C File Offset: 0x0000988C
		public unsafe void CheckHash()
		{
			int num = Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA));
			CAPIBase.CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA cmsg_CTRL_VERIFY_SIGNATURE_EX_PARA = new CAPIBase.CMSG_CTRL_VERIFY_SIGNATURE_EX_PARA(num);
			cmsg_CTRL_VERIFY_SIGNATURE_EX_PARA.dwSignerType = 4U;
			cmsg_CTRL_VERIFY_SIGNATURE_EX_PARA.dwSignerIndex = (uint)PkcsUtils.GetSignerIndex(this.m_signedCms.GetCryptMsgHandle(), this, 0);
			if (!CAPI.CryptMsgControl(this.m_signedCms.GetCryptMsgHandle(), 0U, 19U, new IntPtr((void*)(&cmsg_CTRL_VERIFY_SIGNATURE_EX_PARA))))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A8FF File Offset: 0x000098FF
		internal CAPIBase.CMSG_SIGNER_INFO GetCmsgSignerInfo()
		{
			return this.m_cmsgSignerInfo;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000A908 File Offset: 0x00009908
		private void CounterSign(CmsSigner signer)
		{
			CspParameters cspParameters = new CspParameters();
			if (!X509Utils.GetPrivateKeyInfo(X509Utils.GetCertContext(signer.Certificate), ref cspParameters))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(cspParameters, KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Sign);
			keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
			keyContainerPermission.Demand();
			uint signerIndex = (uint)PkcsUtils.GetSignerIndex(this.m_signedCms.GetCryptMsgHandle(), this, 0);
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO))));
			CAPIBase.CMSG_SIGNER_ENCODE_INFO cmsg_SIGNER_ENCODE_INFO = PkcsUtils.CreateSignerEncodeInfo(signer);
			try
			{
				Marshal.StructureToPtr(cmsg_SIGNER_ENCODE_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
				if (!CAPI.CryptMsgCountersign(this.m_signedCms.GetCryptMsgHandle(), signerIndex, 1U, safeLocalAllocHandle.DangerousGetHandle()))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				this.m_signedCms.ReopenToDecode();
			}
			finally
			{
				Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO));
				safeLocalAllocHandle.Dispose();
				cmsg_SIGNER_ENCODE_INFO.Dispose();
			}
			PkcsUtils.AddCertsToMessage(this.m_signedCms.GetCryptMsgHandle(), this.m_signedCms.Certificates, PkcsUtils.CreateBagOfCertificates(signer));
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000AA38 File Offset: 0x00009A38
		private unsafe void Verify(X509Certificate2Collection extraStore, X509Certificate2 certificate, bool verifySignatureOnly)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = (CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(X509Utils.GetCertContext(certificate).DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT));
			IntPtr intPtr = new IntPtr((long)cert_CONTEXT.pCertInfo + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_INFO), "SubjectPublicKeyInfo"));
			IntPtr intPtr2 = new IntPtr((long)intPtr + (long)Marshal.OffsetOf(typeof(CAPIBase.CERT_PUBLIC_KEY_INFO), "Algorithm"));
			IntPtr intPtr3 = new IntPtr((long)intPtr2 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER), "Parameters"));
			IntPtr intPtr4 = Marshal.ReadIntPtr(intPtr2);
			if (CAPI.CryptFindOIDInfo(1U, intPtr4, 3U).Algid == 8704U)
			{
				bool flag = false;
				IntPtr intPtr5 = new IntPtr((long)intPtr3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "cbData"));
				IntPtr intPtr6 = new IntPtr((long)intPtr3 + (long)Marshal.OffsetOf(typeof(CAPIBase.CRYPTOAPI_BLOB), "pbData"));
				if (Marshal.ReadInt32(intPtr5) == 0)
				{
					flag = true;
				}
				else if (Marshal.ReadIntPtr(intPtr6) == IntPtr.Zero)
				{
					flag = true;
				}
				else
				{
					IntPtr intPtr7 = Marshal.ReadIntPtr(intPtr6);
					if (Marshal.ReadInt32(intPtr7) == 5)
					{
						flag = true;
					}
				}
				if (flag)
				{
					SafeCertChainHandle invalidHandle = SafeCertChainHandle.InvalidHandle;
					X509Utils.BuildChain(new IntPtr(0L), X509Utils.GetCertContext(certificate), null, null, null, X509RevocationMode.NoCheck, X509RevocationFlag.ExcludeRoot, DateTime.Now, new TimeSpan(0, 0, 0), ref invalidHandle);
					invalidHandle.Dispose();
					uint num = 0U;
					if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 22U, safeLocalAllocHandle, ref num))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					if (num > 0U)
					{
						safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num)));
						if (!CAPISafe.CertGetCertificateContextProperty(X509Utils.GetCertContext(certificate), 22U, safeLocalAllocHandle, ref num))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						Marshal.WriteInt32(intPtr5, (int)num);
						Marshal.WriteIntPtr(intPtr6, safeLocalAllocHandle.DangerousGetHandle());
					}
				}
			}
			if (this.m_parentSignerInfo == null)
			{
				if (!CAPI.CryptMsgControl(this.m_signedCms.GetCryptMsgHandle(), 0U, 1U, cert_CONTEXT.pCertInfo))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			else
			{
				int num2 = -1;
				int num3 = 0;
				SafeLocalAllocHandle invalidHandle2;
				for (;;)
				{
					try
					{
						num2 = PkcsUtils.GetSignerIndex(this.m_signedCms.GetCryptMsgHandle(), this.m_parentSignerInfo, num2 + 1);
					}
					catch (CryptographicException)
					{
						if (num3 == 0)
						{
							throw;
						}
						throw new CryptographicException(num3);
					}
					uint num4 = 0U;
					invalidHandle2 = SafeLocalAllocHandle.InvalidHandle;
					PkcsUtils.GetParam(this.m_signedCms.GetCryptMsgHandle(), 28U, (uint)num2, out invalidHandle2, out num4);
					if (num4 != 0U)
					{
						try
						{
							fixed (byte* ptr = this.m_encodedSignerInfo)
							{
								if (!CAPISafe.CryptMsgVerifyCountersignatureEncoded(IntPtr.Zero, 65537U, invalidHandle2.DangerousGetHandle(), num4, new IntPtr((void*)ptr), (uint)this.m_encodedSignerInfo.Length, cert_CONTEXT.pCertInfo))
								{
									num3 = Marshal.GetLastWin32Error();
									continue;
								}
							}
						}
						finally
						{
							byte* ptr = null;
						}
						break;
					}
					num3 = -2146885618;
				}
				invalidHandle2.Dispose();
			}
			if (!verifySignatureOnly)
			{
				int num5 = SignerInfo.VerifyCertificate(certificate, extraStore);
				if (num5 != 0)
				{
					throw new CryptographicException(num5);
				}
			}
			safeLocalAllocHandle.Dispose();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000AD78 File Offset: 0x00009D78
		private unsafe void RemoveCounterSignature(int parentIndex, int childIndex)
		{
			if (parentIndex < 0)
			{
				throw new ArgumentOutOfRangeException("parentIndex");
			}
			if (childIndex < 0)
			{
				throw new ArgumentOutOfRangeException("childIndex");
			}
			uint num = 0U;
			SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
			uint num2 = 0U;
			SafeLocalAllocHandle invalidHandle2 = SafeLocalAllocHandle.InvalidHandle;
			IntPtr zero = IntPtr.Zero;
			SafeCryptMsgHandle cryptMsgHandle = this.m_signedCms.GetCryptMsgHandle();
			uint num3;
			if (PkcsUtils.CmsSupported())
			{
				PkcsUtils.GetParam(cryptMsgHandle, 39U, (uint)parentIndex, out invalidHandle, out num);
				CAPIBase.CMSG_CMS_SIGNER_INFO cmsg_CMS_SIGNER_INFO = (CAPIBase.CMSG_CMS_SIGNER_INFO)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_CMS_SIGNER_INFO));
				num3 = cmsg_CMS_SIGNER_INFO.UnauthAttrs.cAttr;
				zero = new IntPtr((long)cmsg_CMS_SIGNER_INFO.UnauthAttrs.rgAttr);
			}
			else
			{
				PkcsUtils.GetParam(cryptMsgHandle, 6U, (uint)parentIndex, out invalidHandle2, out num2);
				CAPIBase.CMSG_SIGNER_INFO cmsg_SIGNER_INFO = (CAPIBase.CMSG_SIGNER_INFO)Marshal.PtrToStructure(invalidHandle2.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_INFO));
				num3 = cmsg_SIGNER_INFO.UnauthAttrs.cAttr;
				zero = new IntPtr((long)cmsg_SIGNER_INFO.UnauthAttrs.rgAttr);
			}
			for (uint num4 = 0U; num4 < num3; num4 += 1U)
			{
				CAPIBase.CRYPT_ATTRIBUTE crypt_ATTRIBUTE = (CAPIBase.CRYPT_ATTRIBUTE)Marshal.PtrToStructure(zero, typeof(CAPIBase.CRYPT_ATTRIBUTE));
				if (string.Compare(crypt_ATTRIBUTE.pszObjId, "1.2.840.113549.1.9.6", StringComparison.OrdinalIgnoreCase) == 0 && crypt_ATTRIBUTE.cValue > 0U)
				{
					if (childIndex < (int)crypt_ATTRIBUTE.cValue)
					{
						CAPIBase.CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA cmsg_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA = new CAPIBase.CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA)));
						cmsg_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA.dwSignerIndex = (uint)parentIndex;
						cmsg_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA.dwUnauthAttrIndex = num4;
						if (!CAPI.CryptMsgControl(cryptMsgHandle, 0U, 9U, new IntPtr((void*)(&cmsg_CTRL_DEL_SIGNER_UNAUTH_ATTR_PARA))))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						if (crypt_ATTRIBUTE.cValue > 1U)
						{
							try
							{
								uint num5 = (uint)((ulong)(crypt_ATTRIBUTE.cValue - 1U) * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
								SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num5)));
								CAPIBase.CRYPTOAPI_BLOB* ptr = (CAPIBase.CRYPTOAPI_BLOB*)(void*)crypt_ATTRIBUTE.rgValue;
								CAPIBase.CRYPTOAPI_BLOB* ptr2 = (CAPIBase.CRYPTOAPI_BLOB*)(void*)safeLocalAllocHandle.DangerousGetHandle();
								int i = 0;
								while (i < (int)crypt_ATTRIBUTE.cValue)
								{
									if (i != childIndex)
									{
										*ptr2 = *ptr;
									}
									i++;
									ptr++;
									ptr2++;
								}
								CAPIBase.CRYPT_ATTRIBUTE crypt_ATTRIBUTE2 = default(CAPIBase.CRYPT_ATTRIBUTE);
								crypt_ATTRIBUTE2.pszObjId = crypt_ATTRIBUTE.pszObjId;
								crypt_ATTRIBUTE2.cValue = crypt_ATTRIBUTE.cValue - 1U;
								crypt_ATTRIBUTE2.rgValue = safeLocalAllocHandle.DangerousGetHandle();
								SafeLocalAllocHandle safeLocalAllocHandle2 = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_ATTRIBUTE))));
								Marshal.StructureToPtr(crypt_ATTRIBUTE2, safeLocalAllocHandle2.DangerousGetHandle(), false);
								byte[] array;
								try
								{
									if (!CAPI.EncodeObject(new IntPtr(22L), safeLocalAllocHandle2.DangerousGetHandle(), out array))
									{
										throw new CryptographicException(Marshal.GetLastWin32Error());
									}
								}
								finally
								{
									Marshal.DestroyStructure(safeLocalAllocHandle2.DangerousGetHandle(), typeof(CAPIBase.CRYPT_ATTRIBUTE));
									safeLocalAllocHandle2.Dispose();
								}
								try
								{
									fixed (byte* ptr3 = &array[0])
									{
										CAPIBase.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA = new CAPIBase.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA)));
										cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA.dwSignerIndex = (uint)parentIndex;
										cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA.blob.cbData = (uint)array.Length;
										cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA.blob.pbData = new IntPtr((void*)ptr3);
										if (!CAPI.CryptMsgControl(cryptMsgHandle, 0U, 8U, new IntPtr((void*)(&cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA))))
										{
											throw new CryptographicException(Marshal.GetLastWin32Error());
										}
									}
								}
								finally
								{
									byte* ptr3 = null;
								}
								safeLocalAllocHandle.Dispose();
							}
							catch (CryptographicException)
							{
								byte[] array2;
								if (CAPI.EncodeObject(new IntPtr(22L), zero, out array2))
								{
									fixed (byte* ptr4 = &array2[0])
									{
										CAPIBase.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA2 = new CAPIBase.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA(Marshal.SizeOf(typeof(CAPIBase.CMSG_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA)));
										cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA2.dwSignerIndex = (uint)parentIndex;
										cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA2.blob.cbData = (uint)array2.Length;
										cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA2.blob.pbData = new IntPtr((void*)ptr4);
										CAPI.CryptMsgControl(cryptMsgHandle, 0U, 8U, new IntPtr((void*)(&cmsg_CTRL_ADD_SIGNER_UNAUTH_ATTR_PARA2)));
									}
								}
								throw;
							}
						}
						return;
					}
					else
					{
						childIndex -= (int)crypt_ATTRIBUTE.cValue;
					}
				}
				zero = new IntPtr((long)zero + (long)Marshal.SizeOf(typeof(CAPIBase.CRYPT_ATTRIBUTE)));
			}
			if (invalidHandle != null && !invalidHandle.IsInvalid)
			{
				invalidHandle.Dispose();
			}
			if (invalidHandle2 != null && !invalidHandle2.IsInvalid)
			{
				invalidHandle2.Dispose();
			}
			throw new CryptographicException(-2146885618);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000B200 File Offset: 0x0000A200
		private unsafe static int VerifyCertificate(X509Certificate2 certificate, X509Certificate2Collection extraStore)
		{
			int num2;
			int num = X509Utils.VerifyCertificate(X509Utils.GetCertContext(certificate), null, null, X509RevocationMode.Online, X509RevocationFlag.ExcludeRoot, DateTime.Now, new TimeSpan(0, 0, 0), extraStore, new IntPtr(1L), new IntPtr((void*)(&num2)));
			if (num != 0)
			{
				return num2;
			}
			foreach (X509Extension x509Extension in certificate.Extensions)
			{
				if (string.Compare(x509Extension.Oid.Value, "2.5.29.15", StringComparison.OrdinalIgnoreCase) == 0)
				{
					X509KeyUsageExtension x509KeyUsageExtension = new X509KeyUsageExtension();
					x509KeyUsageExtension.CopyFrom(x509Extension);
					if ((x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.DigitalSignature) == X509KeyUsageFlags.None && (x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.NonRepudiation) == X509KeyUsageFlags.None)
					{
						num = -2146762480;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x040004AF RID: 1199
		private X509Certificate2 m_certificate;

		// Token: 0x040004B0 RID: 1200
		private SubjectIdentifier m_signerIdentifier;

		// Token: 0x040004B1 RID: 1201
		private CryptographicAttributeObjectCollection m_signedAttributes;

		// Token: 0x040004B2 RID: 1202
		private CryptographicAttributeObjectCollection m_unsignedAttributes;

		// Token: 0x040004B3 RID: 1203
		private SignedCms m_signedCms;

		// Token: 0x040004B4 RID: 1204
		private SignerInfo m_parentSignerInfo;

		// Token: 0x040004B5 RID: 1205
		private byte[] m_encodedSignerInfo;

		// Token: 0x040004B6 RID: 1206
		private SafeLocalAllocHandle m_pbCmsgSignerInfo;

		// Token: 0x040004B7 RID: 1207
		private CAPIBase.CMSG_SIGNER_INFO m_cmsgSignerInfo;
	}
}

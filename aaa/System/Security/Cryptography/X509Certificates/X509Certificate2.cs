using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000327 RID: 807
	public class X509Certificate2 : X509Certificate
	{
		// Token: 0x06001947 RID: 6471 RVA: 0x00056315 File Offset: 0x00055315
		public X509Certificate2()
		{
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x00056328 File Offset: 0x00055328
		public X509Certificate2(byte[] rawData)
			: base(rawData)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x00056354 File Offset: 0x00055354
		public X509Certificate2(byte[] rawData, string password)
			: base(rawData, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00056381 File Offset: 0x00055381
		public X509Certificate2(byte[] rawData, SecureString password)
			: base(rawData, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x000563AE File Offset: 0x000553AE
		public X509Certificate2(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
			: base(rawData, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x000563DC File Offset: 0x000553DC
		public X509Certificate2(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
			: base(rawData, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0005640A File Offset: 0x0005540A
		public X509Certificate2(string fileName)
			: base(fileName)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00056436 File Offset: 0x00055436
		public X509Certificate2(string fileName, string password)
			: base(fileName, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00056463 File Offset: 0x00055463
		public X509Certificate2(string fileName, SecureString password)
			: base(fileName, password)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00056490 File Offset: 0x00055490
		public X509Certificate2(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
			: base(fileName, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x000564BE File Offset: 0x000554BE
		public X509Certificate2(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
			: base(fileName, password, keyStorageFlags)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			this.m_randomKeyContainer = true;
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x000564EC File Offset: 0x000554EC
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Certificate2(IntPtr handle)
			: base(handle)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x00056514 File Offset: 0x00055514
		public X509Certificate2(X509Certificate certificate)
			: base(certificate)
		{
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
			X509Certificate2 x509Certificate = certificate as X509Certificate2;
			if (x509Certificate != null)
			{
				this.m_randomKeyContainer = x509Certificate.m_randomKeyContainer;
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0005655A File Offset: 0x0005555A
		public override string ToString()
		{
			return base.ToString(true);
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x00056564 File Offset: 0x00055564
		public override string ToString(bool verbose)
		{
			if (!verbose || this.m_safeCertContext.IsInvalid)
			{
				return this.ToString();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Version]" + Environment.NewLine + "  ");
			stringBuilder.Append("V" + this.Version);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Subject]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.SubjectName.Name);
			string text = this.GetNameInfo(X509NameType.SimpleName, false);
			if (text.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  Simple Name: ");
				stringBuilder.Append(text);
			}
			string text2 = this.GetNameInfo(X509NameType.EmailName, false);
			if (text2.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  Email Name: ");
				stringBuilder.Append(text2);
			}
			string text3 = this.GetNameInfo(X509NameType.UpnName, false);
			if (text3.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  UPN Name: ");
				stringBuilder.Append(text3);
			}
			string text4 = this.GetNameInfo(X509NameType.DnsName, false);
			if (text4.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  DNS Name: ");
				stringBuilder.Append(text4);
			}
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Issuer]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.IssuerName.Name);
			text = this.GetNameInfo(X509NameType.SimpleName, true);
			if (text.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  Simple Name: ");
				stringBuilder.Append(text);
			}
			text2 = this.GetNameInfo(X509NameType.EmailName, true);
			if (text2.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  Email Name: ");
				stringBuilder.Append(text2);
			}
			text3 = this.GetNameInfo(X509NameType.UpnName, true);
			if (text3.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  UPN Name: ");
				stringBuilder.Append(text3);
			}
			text4 = this.GetNameInfo(X509NameType.DnsName, true);
			if (text4.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine + "  DNS Name: ");
				stringBuilder.Append(text4);
			}
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Serial Number]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.SerialNumber);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Not Before]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.NotBefore);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Not After]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.NotAfter);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Thumbprint]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.Thumbprint);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Signature Algorithm]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.SignatureAlgorithm.FriendlyName + "(" + this.SignatureAlgorithm.Value + ")");
			PublicKey publicKey = this.PublicKey;
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Public Key]",
				Environment.NewLine,
				"  Algorithm: "
			}));
			stringBuilder.Append(publicKey.Oid.FriendlyName);
			stringBuilder.Append(Environment.NewLine + "  Length: ");
			stringBuilder.Append(publicKey.Key.KeySize);
			stringBuilder.Append(Environment.NewLine + "  Key Blob: ");
			stringBuilder.Append(publicKey.EncodedKeyValue.Format(true));
			stringBuilder.Append(Environment.NewLine + "  Parameters: ");
			stringBuilder.Append(publicKey.EncodedParameters.Format(true));
			this.AppendPrivateKeyInfo(stringBuilder);
			X509ExtensionCollection extensions = this.Extensions;
			if (extensions.Count > 0)
			{
				stringBuilder.Append(Environment.NewLine + Environment.NewLine + "[Extensions]");
				foreach (X509Extension x509Extension in extensions)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						Environment.NewLine,
						"* ",
						x509Extension.Oid.FriendlyName,
						"(",
						x509Extension.Oid.Value,
						"):",
						Environment.NewLine,
						"  ",
						x509Extension.Format(true)
					}));
				}
			}
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x00056B70 File Offset: 0x00055B70
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x00056BB8 File Offset: 0x00055BB8
		public bool Archived
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				uint num = 0U;
				return CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 19U, SafeLocalAllocHandle.InvalidHandle, ref num);
			}
			set
			{
				SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
				if (value)
				{
					safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
				}
				if (!CAPI.CertSetCertificateContextProperty(this.m_safeCertContext, 19U, 0U, safeLocalAllocHandle))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				safeLocalAllocHandle.Dispose();
			}
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x00056C0C File Offset: 0x00055C0C
		public X509ExtensionCollection Extensions
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_extensions == null)
				{
					this.m_extensions = new X509ExtensionCollection(this.m_safeCertContext);
				}
				return this.m_extensions;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x00056C5C File Offset: 0x00055C5C
		// (set) Token: 0x0600195A RID: 6490 RVA: 0x00056CE4 File Offset: 0x00055CE4
		public string FriendlyName
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
				uint num = 0U;
				if (!CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 11U, safeLocalAllocHandle, ref num))
				{
					return string.Empty;
				}
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
				if (!CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 11U, safeLocalAllocHandle, ref num))
				{
					return string.Empty;
				}
				string text = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
				safeLocalAllocHandle.Dispose();
				return text;
			}
			set
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (value == null)
				{
					value = string.Empty;
				}
				X509Certificate2.SetFriendlyNameExtendedProperty(this.m_safeCertContext, value);
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x00056D20 File Offset: 0x00055D20
		public unsafe X500DistinguishedName IssuerName
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_issuerName == null)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					this.m_issuerName = new X500DistinguishedName(((CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO))).Issuer);
				}
				return this.m_issuerName;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x00056DA4 File Offset: 0x00055DA4
		public unsafe DateTime NotAfter
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_notAfter == DateTime.MinValue)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
					long num = (long)(((ulong)cert_INFO.NotAfter.dwHighDateTime << 32) | (ulong)cert_INFO.NotAfter.dwLowDateTime);
					this.m_notAfter = DateTime.FromFileTime(num);
				}
				return this.m_notAfter;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x00056E48 File Offset: 0x00055E48
		public unsafe DateTime NotBefore
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_notBefore == DateTime.MinValue)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
					long num = (long)(((ulong)cert_INFO.NotBefore.dwHighDateTime << 32) | (ulong)cert_INFO.NotBefore.dwLowDateTime);
					this.m_notBefore = DateTime.FromFileTime(num);
				}
				return this.m_notBefore;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x00056EEC File Offset: 0x00055EEC
		public bool HasPrivateKey
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				uint num = 0U;
				return CAPISafe.CertGetCertificateContextProperty(this.m_safeCertContext, 2U, SafeLocalAllocHandle.InvalidHandle, ref num);
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x00056F30 File Offset: 0x00055F30
		private static uint RandomKeyContainerFlag
		{
			get
			{
				if (X509Certificate2.randomKeyContainerFlag == 4294967295U)
				{
					FieldInfo field = typeof(RSACryptoServiceProvider).GetField("RandomKeyContainerFlag", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
					if (field != null)
					{
						X509Certificate2.randomKeyContainerFlag = (uint)field.GetValue(null);
					}
					else
					{
						X509Certificate2.randomKeyContainerFlag = 0U;
					}
				}
				return X509Certificate2.randomKeyContainerFlag;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x00056F80 File Offset: 0x00055F80
		// (set) Token: 0x06001961 RID: 6497 RVA: 0x00057064 File Offset: 0x00056064
		public AsymmetricAlgorithm PrivateKey
		{
			get
			{
				if (!this.HasPrivateKey)
				{
					return null;
				}
				if (this.m_privateKey == null)
				{
					CspParameters cspParameters = new CspParameters();
					if (!X509Certificate2.GetPrivateKeyInfo(this.m_safeCertContext, ref cspParameters))
					{
						return null;
					}
					cspParameters.Flags |= CspProviderFlags.UseExistingKey;
					uint algorithmId = this.PublicKey.AlgorithmId;
					if (algorithmId != 8704U)
					{
						if (algorithmId != 9216U && algorithmId != 41984U)
						{
							throw new NotSupportedException(SR.GetString("NotSupported_KeyAlgorithm"));
						}
						if (this.m_randomKeyContainer)
						{
							new KeyContainerPermission(PermissionState.None)
							{
								AccessEntries = 
								{
									new KeyContainerPermissionAccessEntry(cspParameters, KeyContainerPermissionFlags.Open)
								}
							}.Assert();
							cspParameters.Flags |= (CspProviderFlags)X509Certificate2.RandomKeyContainerFlag;
						}
						this.m_privateKey = new RSACryptoServiceProvider(cspParameters);
						if (this.m_randomKeyContainer)
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					else
					{
						this.m_privateKey = new DSACryptoServiceProvider(cspParameters);
					}
				}
				return this.m_privateKey;
			}
			set
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				ICspAsymmetricAlgorithm cspAsymmetricAlgorithm = value as ICspAsymmetricAlgorithm;
				if (value != null && cspAsymmetricAlgorithm == null)
				{
					throw new NotSupportedException(SR.GetString("NotSupported_InvalidKeyImpl"));
				}
				if (cspAsymmetricAlgorithm != null)
				{
					if (cspAsymmetricAlgorithm.CspKeyContainerInfo == null)
					{
						throw new ArgumentException("CspKeyContainerInfo");
					}
					if (X509Certificate2.s_publicKeyOffset == 0)
					{
						X509Certificate2.s_publicKeyOffset = Marshal.SizeOf(typeof(CAPIBase.BLOBHEADER));
					}
					ICspAsymmetricAlgorithm cspAsymmetricAlgorithm2 = this.PublicKey.Key as ICspAsymmetricAlgorithm;
					byte[] array = cspAsymmetricAlgorithm2.ExportCspBlob(false);
					byte[] array2 = cspAsymmetricAlgorithm.ExportCspBlob(false);
					if (array == null || array2 == null || array.Length != array2.Length || array.Length <= X509Certificate2.s_publicKeyOffset)
					{
						throw new CryptographicUnexpectedOperationException(SR.GetString("Cryptography_X509_KeyMismatch"));
					}
					for (int i = X509Certificate2.s_publicKeyOffset; i < array.Length; i++)
					{
						if (array[i] != array2[i])
						{
							throw new CryptographicUnexpectedOperationException(SR.GetString("Cryptography_X509_KeyMismatch"));
						}
					}
				}
				X509Certificate2.SetPrivateKeyProperty(this.m_safeCertContext, cspAsymmetricAlgorithm);
				this.m_privateKey = value;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x00057170 File Offset: 0x00056170
		public PublicKey PublicKey
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_publicKey == null)
				{
					string keyAlgorithm = this.GetKeyAlgorithm();
					byte[] keyAlgorithmParameters = this.GetKeyAlgorithmParameters();
					byte[] publicKey = this.GetPublicKey();
					Oid oid = new Oid(keyAlgorithm, OidGroup.PublicKeyAlgorithm, true);
					this.m_publicKey = new PublicKey(oid, new AsnEncodedData(oid, keyAlgorithmParameters), new AsnEncodedData(oid, publicKey));
				}
				return this.m_publicKey;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x000571E5 File Offset: 0x000561E5
		public byte[] RawData
		{
			get
			{
				return this.GetRawCertData();
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x000571ED File Offset: 0x000561ED
		public string SerialNumber
		{
			get
			{
				return this.GetSerialNumberString();
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001965 RID: 6501 RVA: 0x000571F8 File Offset: 0x000561F8
		public unsafe X500DistinguishedName SubjectName
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_subjectName == null)
				{
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
					this.m_subjectName = new X500DistinguishedName(((CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO))).Subject);
				}
				return this.m_subjectName;
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x0005727C File Offset: 0x0005627C
		public Oid SignatureAlgorithm
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_signatureAlgorithm == null)
				{
					this.m_signatureAlgorithm = X509Certificate2.GetSignatureAlgorithm(this.m_safeCertContext);
				}
				return this.m_signatureAlgorithm;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x000572CA File Offset: 0x000562CA
		public string Thumbprint
		{
			get
			{
				return this.GetCertHashString();
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x000572D4 File Offset: 0x000562D4
		public int Version
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_version == 0)
				{
					this.m_version = (int)X509Certificate2.GetVersion(this.m_safeCertContext);
				}
				return this.m_version;
			}
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x00057324 File Offset: 0x00056324
		public unsafe string GetNameInfo(X509NameType nameType, bool forIssuer)
		{
			uint num = (forIssuer ? 1U : 0U);
			uint num2 = X509Utils.MapNameType(nameType);
			uint num3 = num2;
			if (num3 == 1U)
			{
				return CAPI.GetCertNameInfo(this.m_safeCertContext, num, num2);
			}
			if (num3 == 4U)
			{
				return CAPI.GetCertNameInfo(this.m_safeCertContext, num, num2);
			}
			string text = string.Empty;
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)this.m_safeCertContext.DangerousGetHandle();
			CAPIBase.CERT_INFO cert_INFO = (CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO));
			IntPtr[] array = new IntPtr[]
			{
				CAPISafe.CertFindExtension(forIssuer ? "2.5.29.8" : "2.5.29.7", cert_INFO.cExtension, cert_INFO.rgExtension),
				CAPISafe.CertFindExtension(forIssuer ? "2.5.29.18" : "2.5.29.17", cert_INFO.cExtension, cert_INFO.rgExtension)
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != IntPtr.Zero)
				{
					CAPIBase.CERT_EXTENSION cert_EXTENSION = (CAPIBase.CERT_EXTENSION)Marshal.PtrToStructure(array[i], typeof(CAPIBase.CERT_EXTENSION));
					byte[] array2 = new byte[cert_EXTENSION.Value.cbData];
					Marshal.Copy(cert_EXTENSION.Value.pbData, array2, 0, array2.Length);
					uint num4 = 0U;
					SafeLocalAllocHandle safeLocalAllocHandle = null;
					SafeLocalAllocHandle safeLocalAllocHandle2 = X509Utils.StringToAnsiPtr(cert_EXTENSION.pszObjId);
					bool flag = CAPI.DecodeObject(safeLocalAllocHandle2.DangerousGetHandle(), array2, out safeLocalAllocHandle, out num4);
					safeLocalAllocHandle2.Dispose();
					if (flag)
					{
						CAPIBase.CERT_ALT_NAME_INFO cert_ALT_NAME_INFO = (CAPIBase.CERT_ALT_NAME_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CERT_ALT_NAME_INFO));
						int num5 = 0;
						while ((long)num5 < (long)((ulong)cert_ALT_NAME_INFO.cAltEntry))
						{
							IntPtr intPtr = new IntPtr((long)cert_ALT_NAME_INFO.rgAltEntry + (long)(num5 * Marshal.SizeOf(typeof(CAPIBase.CERT_ALT_NAME_ENTRY))));
							CAPIBase.CERT_ALT_NAME_ENTRY cert_ALT_NAME_ENTRY = (CAPIBase.CERT_ALT_NAME_ENTRY)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CERT_ALT_NAME_ENTRY));
							switch (num2)
							{
							case 6U:
								if (cert_ALT_NAME_ENTRY.dwAltNameChoice == 3U)
								{
									text = Marshal.PtrToStringUni(cert_ALT_NAME_ENTRY.Value.pwszDNSName);
								}
								break;
							case 7U:
								if (cert_ALT_NAME_ENTRY.dwAltNameChoice == 7U)
								{
									text = Marshal.PtrToStringUni(cert_ALT_NAME_ENTRY.Value.pwszURL);
								}
								break;
							case 8U:
								if (cert_ALT_NAME_ENTRY.dwAltNameChoice == 1U)
								{
									CAPIBase.CERT_OTHER_NAME cert_OTHER_NAME = (CAPIBase.CERT_OTHER_NAME)Marshal.PtrToStructure(cert_ALT_NAME_ENTRY.Value.pOtherName, typeof(CAPIBase.CERT_OTHER_NAME));
									if (cert_OTHER_NAME.pszObjId == "1.3.6.1.4.1.311.20.2.3")
									{
										uint num6 = 0U;
										SafeLocalAllocHandle safeLocalAllocHandle3 = null;
										flag = CAPI.DecodeObject(new IntPtr(24L), X509Utils.PtrToByte(cert_OTHER_NAME.Value.pbData, cert_OTHER_NAME.Value.cbData), out safeLocalAllocHandle3, out num6);
										if (flag)
										{
											CAPIBase.CERT_NAME_VALUE cert_NAME_VALUE = (CAPIBase.CERT_NAME_VALUE)Marshal.PtrToStructure(safeLocalAllocHandle3.DangerousGetHandle(), typeof(CAPIBase.CERT_NAME_VALUE));
											if (X509Utils.IsCertRdnCharString(cert_NAME_VALUE.dwValueType))
											{
												text = Marshal.PtrToStringUni(cert_NAME_VALUE.Value.pbData);
											}
											safeLocalAllocHandle3.Dispose();
										}
									}
								}
								break;
							}
							num5++;
						}
						safeLocalAllocHandle.Dispose();
					}
				}
			}
			if (nameType == X509NameType.DnsName && (text == null || text.Length == 0))
			{
				text = CAPI.GetCertNameInfo(this.m_safeCertContext, num, 3U);
			}
			return text;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00057682 File Offset: 0x00056682
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(byte[] rawData)
		{
			this.Reset();
			base.Import(rawData);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x000576A2 File Offset: 0x000566A2
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(rawData, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x000576C4 File Offset: 0x000566C4
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(rawData, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x000576E6 File Offset: 0x000566E6
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void Import(string fileName)
		{
			this.Reset();
			base.Import(fileName);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00057706 File Offset: 0x00056706
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(fileName, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00057728 File Offset: 0x00056728
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			base.Import(fileName, password, keyStorageFlags);
			this.m_safeCertContext = CAPI.CertDuplicateCertificateContext(base.Handle);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0005774C File Offset: 0x0005674C
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void Reset()
		{
			this.m_version = 0;
			this.m_notBefore = DateTime.MinValue;
			this.m_notAfter = DateTime.MinValue;
			this.m_privateKey = null;
			this.m_publicKey = null;
			this.m_extensions = null;
			this.m_signatureAlgorithm = null;
			this.m_subjectName = null;
			this.m_issuerName = null;
			if (!this.m_safeCertContext.IsInvalid)
			{
				this.m_safeCertContext.Dispose();
				this.m_safeCertContext = SafeCertContextHandle.InvalidHandle;
			}
			base.Reset();
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000577CC File Offset: 0x000567CC
		public bool Verify()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			int num = X509Utils.VerifyCertificate(this.CertContext, null, null, X509RevocationMode.Online, X509RevocationFlag.ExcludeRoot, DateTime.Now, new TimeSpan(0, 0, 0), null, new IntPtr(1L), IntPtr.Zero);
			return num == 0;
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0005782C File Offset: 0x0005682C
		public static X509ContentType GetCertContentType(byte[] rawData)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Arg_EmptyOrNullArray"), "rawData");
			}
			uint num = X509Certificate2.QueryCertBlobType(rawData);
			return X509Utils.MapContentType(num);
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00057864 File Offset: 0x00056864
		public static X509ContentType GetCertContentType(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			string fullPath = Path.GetFullPath(fileName);
			new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
			uint num = X509Certificate2.QueryCertFileType(fileName);
			return X509Utils.MapContentType(num);
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x0005789F File Offset: 0x0005689F
		internal SafeCertContextHandle CertContext
		{
			get
			{
				return this.m_safeCertContext;
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x000578A8 File Offset: 0x000568A8
		internal static bool GetPrivateKeyInfo(SafeCertContextHandle safeCertContext, ref CspParameters parameters)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			uint num = 0U;
			if (!CAPISafe.CertGetCertificateContextProperty(safeCertContext, 2U, safeLocalAllocHandle, ref num))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == -2146885628)
				{
					return false;
				}
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			else
			{
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
				if (CAPISafe.CertGetCertificateContextProperty(safeCertContext, 2U, safeLocalAllocHandle, ref num))
				{
					CAPIBase.CRYPT_KEY_PROV_INFO crypt_KEY_PROV_INFO = (CAPIBase.CRYPT_KEY_PROV_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_KEY_PROV_INFO));
					parameters.ProviderName = crypt_KEY_PROV_INFO.pwszProvName;
					parameters.KeyContainerName = crypt_KEY_PROV_INFO.pwszContainerName;
					parameters.ProviderType = (int)crypt_KEY_PROV_INFO.dwProvType;
					parameters.KeyNumber = (int)crypt_KEY_PROV_INFO.dwKeySpec;
					parameters.Flags = (((crypt_KEY_PROV_INFO.dwFlags & 32U) == 32U) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
					safeLocalAllocHandle.Dispose();
					return true;
				}
				int lastWin32Error2 = Marshal.GetLastWin32Error();
				if (lastWin32Error2 == -2146885628)
				{
					return false;
				}
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00057990 File Offset: 0x00056990
		private void AppendPrivateKeyInfo(StringBuilder sb)
		{
			CspKeyContainerInfo cspKeyContainerInfo = null;
			try
			{
				if (this.HasPrivateKey)
				{
					CspParameters cspParameters = new CspParameters();
					if (X509Certificate2.GetPrivateKeyInfo(this.m_safeCertContext, ref cspParameters))
					{
						cspKeyContainerInfo = new CspKeyContainerInfo(cspParameters);
					}
				}
			}
			catch (SecurityException)
			{
			}
			catch (CryptographicException)
			{
			}
			if (cspKeyContainerInfo == null)
			{
				return;
			}
			sb.Append(Environment.NewLine + Environment.NewLine + "[Private Key]");
			sb.Append(Environment.NewLine + "  Key Store: ");
			sb.Append(cspKeyContainerInfo.MachineKeyStore ? "Machine" : "User");
			sb.Append(Environment.NewLine + "  Provider Name: ");
			sb.Append(cspKeyContainerInfo.ProviderName);
			sb.Append(Environment.NewLine + "  Provider type: ");
			sb.Append(cspKeyContainerInfo.ProviderType);
			sb.Append(Environment.NewLine + "  Key Spec: ");
			sb.Append(cspKeyContainerInfo.KeyNumber);
			sb.Append(Environment.NewLine + "  Key Container Name: ");
			sb.Append(cspKeyContainerInfo.KeyContainerName);
			try
			{
				string uniqueKeyContainerName = cspKeyContainerInfo.UniqueKeyContainerName;
				sb.Append(Environment.NewLine + "  Unique Key Container Name: ");
				sb.Append(uniqueKeyContainerName);
			}
			catch (CryptographicException)
			{
			}
			catch (NotSupportedException)
			{
			}
			try
			{
				bool flag = cspKeyContainerInfo.HardwareDevice;
				sb.Append(Environment.NewLine + "  Hardware Device: ");
				sb.Append(flag);
			}
			catch (CryptographicException)
			{
			}
			try
			{
				bool flag = cspKeyContainerInfo.Removable;
				sb.Append(Environment.NewLine + "  Removable: ");
				sb.Append(flag);
			}
			catch (CryptographicException)
			{
			}
			try
			{
				bool flag = cspKeyContainerInfo.Protected;
				sb.Append(Environment.NewLine + "  Protected: ");
				sb.Append(flag);
			}
			catch (CryptographicException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00057BC0 File Offset: 0x00056BC0
		private unsafe static Oid GetSignatureAlgorithm(SafeCertContextHandle safeCertContextHandle)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)safeCertContextHandle.DangerousGetHandle();
			return new Oid(((CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO))).SignatureAlgorithm.pszObjId, OidGroup.SignatureAlgorithm, false);
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00057C10 File Offset: 0x00056C10
		private unsafe static uint GetVersion(SafeCertContextHandle safeCertContextHandle)
		{
			CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)safeCertContextHandle.DangerousGetHandle();
			return ((CAPIBase.CERT_INFO)Marshal.PtrToStructure(cert_CONTEXT.pCertInfo, typeof(CAPIBase.CERT_INFO))).dwVersion + 1U;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00057C54 File Offset: 0x00056C54
		private unsafe static uint QueryCertBlobType(byte[] rawData)
		{
			uint num = 0U;
			if (!CAPI.CryptQueryObject(2U, rawData, 16382U, 14U, 0U, IntPtr.Zero, new IntPtr((void*)(&num)), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return num;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00057CA4 File Offset: 0x00056CA4
		private unsafe static uint QueryCertFileType(string fileName)
		{
			uint num = 0U;
			if (!CAPI.CryptQueryObject(1U, fileName, 16382U, 14U, 0U, IntPtr.Zero, new IntPtr((void*)(&num)), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			return num;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00057CF4 File Offset: 0x00056CF4
		private unsafe static void SetFriendlyNameExtendedProperty(SafeCertContextHandle safeCertContextHandle, string name)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.StringToUniPtr(name);
			using (safeLocalAllocHandle)
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB.cbData = (uint)(2 * (name.Length + 1));
				cryptoapi_BLOB.pbData = safeLocalAllocHandle.DangerousGetHandle();
				if (!CAPI.CertSetCertificateContextProperty(safeCertContextHandle, 11U, 0U, new IntPtr((void*)(&cryptoapi_BLOB))))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00057D6C File Offset: 0x00056D6C
		private static void SetPrivateKeyProperty(SafeCertContextHandle safeCertContextHandle, ICspAsymmetricAlgorithm asymmetricAlgorithm)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (asymmetricAlgorithm != null)
			{
				CAPIBase.CRYPT_KEY_PROV_INFO crypt_KEY_PROV_INFO = default(CAPIBase.CRYPT_KEY_PROV_INFO);
				crypt_KEY_PROV_INFO.pwszContainerName = asymmetricAlgorithm.CspKeyContainerInfo.KeyContainerName;
				crypt_KEY_PROV_INFO.pwszProvName = asymmetricAlgorithm.CspKeyContainerInfo.ProviderName;
				crypt_KEY_PROV_INFO.dwProvType = (uint)asymmetricAlgorithm.CspKeyContainerInfo.ProviderType;
				crypt_KEY_PROV_INFO.dwFlags = (asymmetricAlgorithm.CspKeyContainerInfo.MachineKeyStore ? 32U : 0U);
				crypt_KEY_PROV_INFO.cProvParam = 0U;
				crypt_KEY_PROV_INFO.rgProvParam = IntPtr.Zero;
				crypt_KEY_PROV_INFO.dwKeySpec = (uint)asymmetricAlgorithm.CspKeyContainerInfo.KeyNumber;
				safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_KEY_PROV_INFO))));
				Marshal.StructureToPtr(crypt_KEY_PROV_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
			}
			try
			{
				if (!CAPI.CertSetCertificateContextProperty(safeCertContextHandle, 2U, 0U, safeLocalAllocHandle))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			finally
			{
				if (!safeLocalAllocHandle.IsInvalid)
				{
					Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_KEY_PROV_INFO));
					safeLocalAllocHandle.Dispose();
				}
			}
		}

		// Token: 0x04001A9D RID: 6813
		private int m_version;

		// Token: 0x04001A9E RID: 6814
		private DateTime m_notBefore;

		// Token: 0x04001A9F RID: 6815
		private DateTime m_notAfter;

		// Token: 0x04001AA0 RID: 6816
		private AsymmetricAlgorithm m_privateKey;

		// Token: 0x04001AA1 RID: 6817
		private PublicKey m_publicKey;

		// Token: 0x04001AA2 RID: 6818
		private X509ExtensionCollection m_extensions;

		// Token: 0x04001AA3 RID: 6819
		private Oid m_signatureAlgorithm;

		// Token: 0x04001AA4 RID: 6820
		private X500DistinguishedName m_subjectName;

		// Token: 0x04001AA5 RID: 6821
		private X500DistinguishedName m_issuerName;

		// Token: 0x04001AA6 RID: 6822
		private SafeCertContextHandle m_safeCertContext = SafeCertContextHandle.InvalidHandle;

		// Token: 0x04001AA7 RID: 6823
		private bool m_randomKeyContainer;

		// Token: 0x04001AA8 RID: 6824
		private static int s_publicKeyOffset;

		// Token: 0x04001AA9 RID: 6825
		private static uint randomKeyContainerFlag = uint.MaxValue;
	}
}

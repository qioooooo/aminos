using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;

namespace System.Security.Cryptography.Pkcs
{
	// Token: 0x02000078 RID: 120
	internal class PkcsUtils
	{
		// Token: 0x0600018E RID: 398 RVA: 0x000081FE File Offset: 0x000071FE
		private PkcsUtils()
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008206 File Offset: 0x00007206
		internal static uint AlignedLength(uint length)
		{
			return (length + 7U) & 4294967288U;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008210 File Offset: 0x00007210
		internal static bool CmsSupported()
		{
			if (PkcsUtils.m_cmsSupported == -1)
			{
				IntPtr intPtr = CAPISafe.LoadLibrary("Crypt32.dll");
				if (intPtr != IntPtr.Zero)
				{
					IntPtr procAddress = CAPISafe.GetProcAddress(intPtr, "CryptMsgVerifyCountersignatureEncodedEx");
					PkcsUtils.m_cmsSupported = ((procAddress == IntPtr.Zero) ? 0 : 1);
					CAPISafe.FreeLibrary(intPtr);
				}
			}
			return PkcsUtils.m_cmsSupported != 0;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008270 File Offset: 0x00007270
		internal static RecipientInfoType GetRecipientInfoType(X509Certificate2 certificate)
		{
			RecipientInfoType recipientInfoType = RecipientInfoType.Unknown;
			if (certificate != null)
			{
				uint num = X509Utils.OidToAlgId(((CAPIBase.CERT_INFO)Marshal.PtrToStructure(((CAPIBase.CERT_CONTEXT)Marshal.PtrToStructure(X509Utils.GetCertContext(certificate).DangerousGetHandle(), typeof(CAPIBase.CERT_CONTEXT))).pCertInfo, typeof(CAPIBase.CERT_INFO))).SubjectPublicKeyInfo.Algorithm.pszObjId);
				if (num == 41984U)
				{
					recipientInfoType = RecipientInfoType.KeyTransport;
				}
				else if (num == 43521U || num == 43522U)
				{
					recipientInfoType = RecipientInfoType.KeyAgreement;
				}
				else
				{
					recipientInfoType = RecipientInfoType.Unknown;
				}
			}
			return recipientInfoType;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000082F8 File Offset: 0x000072F8
		internal unsafe static int GetMaxKeyLength(SafeCryptProvHandle safeCryptProvHandle, uint algId)
		{
			uint num = 1U;
			uint num2 = (uint)Marshal.SizeOf(typeof(CAPIBase.PROV_ENUMALGS_EX));
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.PROV_ENUMALGS_EX))));
			using (safeLocalAllocHandle)
			{
				while (CAPISafe.CryptGetProvParam(safeCryptProvHandle, 22U, safeLocalAllocHandle.DangerousGetHandle(), new IntPtr((void*)(&num2)), num))
				{
					CAPIBase.PROV_ENUMALGS_EX prov_ENUMALGS_EX = (CAPIBase.PROV_ENUMALGS_EX)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.PROV_ENUMALGS_EX));
					if (prov_ENUMALGS_EX.aiAlgid == algId)
					{
						return (int)prov_ENUMALGS_EX.dwMaxLen;
					}
					num = 0U;
				}
			}
			throw new CryptographicException(-2146889726);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000083B0 File Offset: 0x000073B0
		internal unsafe static uint GetVersion(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 30U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				PkcsUtils.checkErr(Marshal.GetLastWin32Error());
			}
			return num;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000083F8 File Offset: 0x000073F8
		internal unsafe static uint GetMessageType(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 1U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				PkcsUtils.checkErr(Marshal.GetLastWin32Error());
			}
			return num;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000843C File Offset: 0x0000743C
		internal unsafe static AlgorithmIdentifier GetAlgorithmIdentifier(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier();
			uint num = 0U;
			if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 15U, 0U, IntPtr.Zero, new IntPtr((void*)(&num))))
			{
				PkcsUtils.checkErr(Marshal.GetLastWin32Error());
			}
			if (num > 0U)
			{
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
				if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 15U, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
				{
					PkcsUtils.checkErr(Marshal.GetLastWin32Error());
				}
				CAPIBase.CRYPT_ALGORITHM_IDENTIFIER crypt_ALGORITHM_IDENTIFIER = (CAPIBase.CRYPT_ALGORITHM_IDENTIFIER)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER));
				algorithmIdentifier = new AlgorithmIdentifier(crypt_ALGORITHM_IDENTIFIER);
				safeLocalAllocHandle.Dispose();
			}
			return algorithmIdentifier;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000084CC File Offset: 0x000074CC
		internal unsafe static void GetParam(SafeCryptMsgHandle safeCryptMsgHandle, uint paramType, uint index, out SafeLocalAllocHandle pvData, out uint cbData)
		{
			cbData = 0U;
			pvData = SafeLocalAllocHandle.InvalidHandle;
			fixed (uint* ptr = &cbData)
			{
				if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, paramType, index, pvData, new IntPtr((void*)ptr)))
				{
					PkcsUtils.checkErr(Marshal.GetLastWin32Error());
				}
				if (cbData > 0U)
				{
					pvData = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)cbData)));
					if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, paramType, index, pvData, new IntPtr((void*)ptr)))
					{
						PkcsUtils.checkErr(Marshal.GetLastWin32Error());
					}
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000853C File Offset: 0x0000753C
		internal unsafe static void GetParam(SafeCryptMsgHandle safeCryptMsgHandle, uint paramType, uint index, out byte[] pvData, out uint cbData)
		{
			cbData = 0U;
			pvData = new byte[0];
			fixed (uint* ptr = &cbData)
			{
				if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, paramType, index, IntPtr.Zero, new IntPtr((void*)ptr)))
				{
					PkcsUtils.checkErr(Marshal.GetLastWin32Error());
				}
				if (cbData > 0U)
				{
					pvData = new byte[cbData];
					fixed (byte* ptr2 = &pvData[0])
					{
						if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, paramType, index, new IntPtr((void*)ptr2), new IntPtr((void*)ptr)))
						{
							PkcsUtils.checkErr(Marshal.GetLastWin32Error());
						}
					}
				}
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000085BC File Offset: 0x000075BC
		internal unsafe static X509Certificate2Collection GetCertificates(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 11U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				PkcsUtils.checkErr(Marshal.GetLastWin32Error());
			}
			for (uint num3 = 0U; num3 < num; num3 += 1U)
			{
				uint num4 = 0U;
				SafeLocalAllocHandle invalidHandle = SafeLocalAllocHandle.InvalidHandle;
				PkcsUtils.GetParam(safeCryptMsgHandle, 12U, num3, out invalidHandle, out num4);
				if (num4 > 0U)
				{
					SafeCertContextHandle safeCertContextHandle = CAPISafe.CertCreateCertificateContext(65537U, invalidHandle, num4);
					if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					x509Certificate2Collection.Add(new X509Certificate2(safeCertContextHandle.DangerousGetHandle()));
					safeCertContextHandle.Dispose();
				}
			}
			return x509Certificate2Collection;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00008674 File Offset: 0x00007674
		internal static byte[] GetContent(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			byte[] array = new byte[0];
			PkcsUtils.GetParam(safeCryptMsgHandle, 2U, 0U, out array, out num);
			return array;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008698 File Offset: 0x00007698
		internal static Oid GetContentType(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			byte[] array = new byte[0];
			PkcsUtils.GetParam(safeCryptMsgHandle, 4U, 0U, out array, out num);
			if (array.Length > 0 && array[array.Length - 1] == 0)
			{
				byte[] array2 = new byte[array.Length - 1];
				Array.Copy(array, 0, array2, 0, array2.Length);
				array = array2;
			}
			return new Oid(Encoding.ASCII.GetString(array));
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000086F4 File Offset: 0x000076F4
		internal static byte[] GetMessage(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			byte[] array = new byte[0];
			PkcsUtils.GetParam(safeCryptMsgHandle, 29U, 0U, out array, out num);
			return array;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008718 File Offset: 0x00007718
		internal unsafe static int GetSignerIndex(SafeCryptMsgHandle safeCrytpMsgHandle, SignerInfo signerInfo, int startIndex)
		{
			uint num = 0U;
			uint num2 = (uint)Marshal.SizeOf(typeof(uint));
			if (!CAPISafe.CryptMsgGetParam(safeCrytpMsgHandle, 5U, 0U, new IntPtr((void*)(&num)), new IntPtr((void*)(&num2))))
			{
				PkcsUtils.checkErr(Marshal.GetLastWin32Error());
			}
			for (int i = startIndex; i < (int)num; i++)
			{
				uint num3 = 0U;
				if (!CAPISafe.CryptMsgGetParam(safeCrytpMsgHandle, 6U, (uint)i, IntPtr.Zero, new IntPtr((void*)(&num3))))
				{
					PkcsUtils.checkErr(Marshal.GetLastWin32Error());
				}
				if (num3 > 0U)
				{
					SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num3)));
					if (!CAPISafe.CryptMsgGetParam(safeCrytpMsgHandle, 6U, (uint)i, safeLocalAllocHandle, new IntPtr((void*)(&num3))))
					{
						PkcsUtils.checkErr(Marshal.GetLastWin32Error());
					}
					CAPIBase.CMSG_SIGNER_INFO cmsgSignerInfo = signerInfo.GetCmsgSignerInfo();
					CAPIBase.CMSG_SIGNER_INFO cmsg_SIGNER_INFO = (CAPIBase.CMSG_SIGNER_INFO)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CMSG_SIGNER_INFO));
					if (X509Utils.MemEqual((byte*)(void*)cmsgSignerInfo.Issuer.pbData, cmsgSignerInfo.Issuer.cbData, (byte*)(void*)cmsg_SIGNER_INFO.Issuer.pbData, cmsg_SIGNER_INFO.Issuer.cbData) && X509Utils.MemEqual((byte*)(void*)cmsgSignerInfo.SerialNumber.pbData, cmsgSignerInfo.SerialNumber.cbData, (byte*)(void*)cmsg_SIGNER_INFO.SerialNumber.pbData, cmsg_SIGNER_INFO.SerialNumber.cbData))
					{
						return i;
					}
					safeLocalAllocHandle.Dispose();
				}
			}
			throw new CryptographicException(-2146889714);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008880 File Offset: 0x00007880
		internal unsafe static CryptographicAttributeObjectCollection GetUnprotectedAttributes(SafeCryptMsgHandle safeCryptMsgHandle)
		{
			uint num = 0U;
			CryptographicAttributeObjectCollection cryptographicAttributeObjectCollection = new CryptographicAttributeObjectCollection();
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 37U, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != -2146889713)
				{
					PkcsUtils.checkErr(Marshal.GetLastWin32Error());
				}
			}
			if (num > 0U)
			{
				SafeLocalAllocHandle safeLocalAllocHandle2;
				safeLocalAllocHandle = (safeLocalAllocHandle2 = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num))));
				try
				{
					if (!CAPISafe.CryptMsgGetParam(safeCryptMsgHandle, 37U, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
					{
						PkcsUtils.checkErr(Marshal.GetLastWin32Error());
					}
					cryptographicAttributeObjectCollection = new CryptographicAttributeObjectCollection(safeLocalAllocHandle);
				}
				finally
				{
					if (safeLocalAllocHandle2 != null)
					{
						((IDisposable)safeLocalAllocHandle2).Dispose();
					}
				}
			}
			return cryptographicAttributeObjectCollection;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008924 File Offset: 0x00007924
		internal unsafe static X509IssuerSerial DecodeIssuerSerial(CAPIBase.CERT_ISSUER_SERIAL_NUMBER pIssuerAndSerial)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			uint num = CAPISafe.CertNameToStrW(65537U, new IntPtr((void*)(&pIssuerAndSerial.Issuer)), 33554435U, safeLocalAllocHandle, 0U);
			if (num <= 1U)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)(2U * num))));
			num = CAPISafe.CertNameToStrW(65537U, new IntPtr((void*)(&pIssuerAndSerial.Issuer)), 33554435U, safeLocalAllocHandle, num);
			if (num <= 1U)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			X509IssuerSerial x509IssuerSerial = default(X509IssuerSerial);
			x509IssuerSerial.IssuerName = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
			byte[] array = new byte[pIssuerAndSerial.SerialNumber.cbData];
			Marshal.Copy(pIssuerAndSerial.SerialNumber.pbData, array, 0, array.Length);
			x509IssuerSerial.SerialNumber = X509Utils.EncodeHexStringFromInt(array);
			safeLocalAllocHandle.Dispose();
			return x509IssuerSerial;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000089FC File Offset: 0x000079FC
		internal static string DecodeOctetString(byte[] encodedOctetString)
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(25L), encodedOctetString, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (num == 0U)
			{
				return string.Empty;
			}
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPTOAPI_BLOB));
			if (cryptoapi_BLOB.cbData == 0U)
			{
				return string.Empty;
			}
			int num2 = (int)(cryptoapi_BLOB.cbData / 2U);
			for (int i = 0; i < num2; i++)
			{
				if (Marshal.ReadInt16(cryptoapi_BLOB.pbData, i * 2) == 0)
				{
					num2 = i;
					break;
				}
			}
			string text = Marshal.PtrToStringUni(cryptoapi_BLOB.pbData, num2);
			safeLocalAllocHandle.Dispose();
			return text;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008AA8 File Offset: 0x00007AA8
		internal static byte[] DecodeOctetBytes(byte[] encodedOctetString)
		{
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = null;
			if (!CAPI.DecodeObject(new IntPtr(25L), encodedOctetString, out safeLocalAllocHandle, out num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			if (num == 0U)
			{
				return new byte[0];
			}
			byte[] array;
			using (safeLocalAllocHandle)
			{
				array = CAPI.BlobToByteArray(safeLocalAllocHandle.DangerousGetHandle());
			}
			return array;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008B10 File Offset: 0x00007B10
		internal static byte[] EncodeOctetString(string octetString)
		{
			byte[] array = new byte[2 * (octetString.Length + 1)];
			Encoding.Unicode.GetBytes(octetString, 0, octetString.Length, array, 0);
			return PkcsUtils.EncodeOctetString(array);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008B48 File Offset: 0x00007B48
		internal unsafe static byte[] EncodeOctetString(byte[] octets)
		{
			fixed (byte* ptr = octets)
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB.cbData = (uint)octets.Length;
				cryptoapi_BLOB.pbData = new IntPtr((void*)ptr);
				byte[] array = new byte[0];
				if (!CAPI.EncodeObject(new IntPtr(25L), new IntPtr(&cryptoapi_BLOB), out array))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				return array;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008BC0 File Offset: 0x00007BC0
		internal static string DecodeObjectIdentifier(byte[] encodedObjId, int offset)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			if (0 < encodedObjId.Length - offset)
			{
				byte b = encodedObjId[offset];
				stringBuilder.Append((b / 40).ToString(null, null));
				stringBuilder.Append(".");
				stringBuilder.Append((b % 40).ToString(null, null));
				ulong num = 0UL;
				for (int i = offset + 1; i < encodedObjId.Length; i++)
				{
					byte b2 = encodedObjId[i];
					num = (num << 7) + (ulong)((long)(b2 & 127));
					if ((b2 & 128) == 0)
					{
						stringBuilder.Append(".");
						stringBuilder.Append(num.ToString(null, null));
						num = 0UL;
					}
				}
				if (0UL != num)
				{
					throw new CryptographicException(-2146885630);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008C84 File Offset: 0x00007C84
		internal static CmsRecipientCollection SelectRecipients(SubjectIdentifierType recipientIdentifierType)
		{
			X509Store x509Store = new X509Store("AddressBook");
			x509Store.Open(OpenFlags.OpenExistingOnly);
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection(x509Store.Certificates);
			foreach (X509Certificate2 x509Certificate in x509Store.Certificates)
			{
				if (x509Certificate.NotBefore <= DateTime.Now && x509Certificate.NotAfter >= DateTime.Now)
				{
					bool flag = true;
					foreach (X509Extension x509Extension in x509Certificate.Extensions)
					{
						if (string.Compare(x509Extension.Oid.Value, "2.5.29.15", StringComparison.OrdinalIgnoreCase) == 0)
						{
							X509KeyUsageExtension x509KeyUsageExtension = new X509KeyUsageExtension();
							x509KeyUsageExtension.CopyFrom(x509Extension);
							if ((x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.KeyEncipherment) == X509KeyUsageFlags.None && (x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.KeyAgreement) == X509KeyUsageFlags.None)
							{
								flag = false;
								break;
							}
							break;
						}
					}
					if (flag)
					{
						x509Certificate2Collection.Add(x509Certificate);
					}
				}
			}
			if (x509Certificate2Collection.Count < 1)
			{
				throw new CryptographicException(-2146889717);
			}
			X509Certificate2Collection x509Certificate2Collection2 = X509Certificate2UI.SelectFromCollection(x509Certificate2Collection, null, null, X509SelectionFlag.MultiSelection);
			if (x509Certificate2Collection2.Count < 1)
			{
				throw new CryptographicException(1223);
			}
			return new CmsRecipientCollection(recipientIdentifierType, x509Certificate2Collection2);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00008DAC File Offset: 0x00007DAC
		internal static X509Certificate2 SelectSignerCertificate()
		{
			X509Store x509Store = new X509Store();
			x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			foreach (X509Certificate2 x509Certificate in x509Store.Certificates)
			{
				if (x509Certificate.HasPrivateKey && x509Certificate.NotBefore <= DateTime.Now && x509Certificate.NotAfter >= DateTime.Now)
				{
					bool flag = true;
					foreach (X509Extension x509Extension in x509Certificate.Extensions)
					{
						if (string.Compare(x509Extension.Oid.Value, "2.5.29.15", StringComparison.OrdinalIgnoreCase) == 0)
						{
							X509KeyUsageExtension x509KeyUsageExtension = new X509KeyUsageExtension();
							x509KeyUsageExtension.CopyFrom(x509Extension);
							if ((x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.DigitalSignature) == X509KeyUsageFlags.None && (x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.NonRepudiation) == X509KeyUsageFlags.None)
							{
								flag = false;
								break;
							}
							break;
						}
					}
					if (flag)
					{
						x509Certificate2Collection.Add(x509Certificate);
					}
				}
			}
			if (x509Certificate2Collection.Count < 1)
			{
				throw new CryptographicException(-2146889714);
			}
			x509Certificate2Collection = X509Certificate2UI.SelectFromCollection(x509Certificate2Collection, null, null, X509SelectionFlag.SingleSelection);
			if (x509Certificate2Collection.Count < 1)
			{
				throw new CryptographicException(1223);
			}
			return x509Certificate2Collection[0];
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008ED4 File Offset: 0x00007ED4
		internal static AsnEncodedDataCollection GetAsnEncodedDataCollection(CAPIBase.CRYPT_ATTRIBUTE cryptAttribute)
		{
			AsnEncodedDataCollection asnEncodedDataCollection = new AsnEncodedDataCollection();
			Oid oid = new Oid(cryptAttribute.pszObjId);
			string value = oid.Value;
			for (uint num = 0U; num < cryptAttribute.cValue; num += 1U)
			{
				IntPtr intPtr = new IntPtr((long)cryptAttribute.rgValue + (long)((ulong)num * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB)))));
				Pkcs9AttributeObject pkcs9AttributeObject = new Pkcs9AttributeObject(oid, CAPI.BlobToByteArray(intPtr));
				Pkcs9AttributeObject pkcs9AttributeObject2 = CryptoConfig.CreateFromName(value) as Pkcs9AttributeObject;
				if (pkcs9AttributeObject2 != null)
				{
					pkcs9AttributeObject2.CopyFrom(pkcs9AttributeObject);
					pkcs9AttributeObject = pkcs9AttributeObject2;
				}
				asnEncodedDataCollection.Add(pkcs9AttributeObject);
			}
			return asnEncodedDataCollection;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008F6C File Offset: 0x00007F6C
		internal static AsnEncodedDataCollection GetAsnEncodedDataCollection(CAPIBase.CRYPT_ATTRIBUTE_TYPE_VALUE cryptAttribute)
		{
			return new AsnEncodedDataCollection
			{
				new Pkcs9AttributeObject(new Oid(cryptAttribute.pszObjId), CAPI.BlobToByteArray(cryptAttribute.Value))
			};
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008FA4 File Offset: 0x00007FA4
		internal unsafe static IntPtr CreateCryptAttributes(CryptographicAttributeObjectCollection attributes)
		{
			if (attributes.Count == 0)
			{
				return IntPtr.Zero;
			}
			uint num = 0U;
			uint num2 = PkcsUtils.AlignedLength((uint)Marshal.SizeOf(typeof(PkcsUtils.I_CRYPT_ATTRIBUTE)));
			uint num3 = PkcsUtils.AlignedLength((uint)Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB)));
			foreach (CryptographicAttributeObject cryptographicAttributeObject in attributes)
			{
				num += num2;
				num += PkcsUtils.AlignedLength((uint)(cryptographicAttributeObject.Oid.Value.Length + 1));
				foreach (AsnEncodedData asnEncodedData in cryptographicAttributeObject.Values)
				{
					num += num3;
					num += PkcsUtils.AlignedLength((uint)asnEncodedData.RawData.Length);
				}
			}
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num)));
			PkcsUtils.I_CRYPT_ATTRIBUTE* ptr = (PkcsUtils.I_CRYPT_ATTRIBUTE*)(void*)safeLocalAllocHandle.DangerousGetHandle();
			IntPtr intPtr = new IntPtr((long)safeLocalAllocHandle.DangerousGetHandle() + (long)((ulong)num2 * (ulong)((long)attributes.Count)));
			foreach (CryptographicAttributeObject cryptographicAttributeObject2 in attributes)
			{
				byte* ptr2 = (byte*)(void*)intPtr;
				byte[] array = new byte[cryptographicAttributeObject2.Oid.Value.Length + 1];
				CAPIBase.CRYPTOAPI_BLOB* ptr3 = (CAPIBase.CRYPTOAPI_BLOB*)(ptr2 + PkcsUtils.AlignedLength((uint)array.Length));
				ptr->pszObjId = (IntPtr)((void*)ptr2);
				ptr->cValue = (uint)cryptographicAttributeObject2.Values.Count;
				ptr->rgValue = (IntPtr)((void*)ptr3);
				Encoding.ASCII.GetBytes(cryptographicAttributeObject2.Oid.Value, 0, cryptographicAttributeObject2.Oid.Value.Length, array, 0);
				Marshal.Copy(array, 0, ptr->pszObjId, array.Length);
				IntPtr intPtr2 = new IntPtr(ptr3 + (long)cryptographicAttributeObject2.Values.Count * (long)((ulong)num3) / (long)sizeof(CAPIBase.CRYPTOAPI_BLOB));
				foreach (AsnEncodedData asnEncodedData2 in cryptographicAttributeObject2.Values)
				{
					byte[] rawData = asnEncodedData2.RawData;
					if (rawData.Length > 0)
					{
						ptr3->cbData = (uint)rawData.Length;
						ptr3->pbData = intPtr2;
						Marshal.Copy(rawData, 0, intPtr2, rawData.Length);
						intPtr2 = new IntPtr((long)intPtr2 + (long)((ulong)PkcsUtils.AlignedLength((uint)rawData.Length)));
					}
					ptr3++;
				}
				ptr++;
				intPtr = intPtr2;
			}
			GC.SuppressFinalize(safeLocalAllocHandle);
			return safeLocalAllocHandle.DangerousGetHandle();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009207 File Offset: 0x00008207
		internal static CAPIBase.CMSG_SIGNER_ENCODE_INFO CreateSignerEncodeInfo(CmsSigner signer)
		{
			return PkcsUtils.CreateSignerEncodeInfo(signer, false);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009210 File Offset: 0x00008210
		internal unsafe static CAPIBase.CMSG_SIGNER_ENCODE_INFO CreateSignerEncodeInfo(CmsSigner signer, bool silent)
		{
			CAPIBase.CMSG_SIGNER_ENCODE_INFO cmsg_SIGNER_ENCODE_INFO = new CAPIBase.CMSG_SIGNER_ENCODE_INFO(Marshal.SizeOf(typeof(CAPIBase.CMSG_SIGNER_ENCODE_INFO)));
			SafeCryptProvHandle invalidHandle = SafeCryptProvHandle.InvalidHandle;
			uint num = 0U;
			bool flag = false;
			cmsg_SIGNER_ENCODE_INFO.HashAlgorithm.pszObjId = signer.DigestAlgorithm.Value;
			if (string.Compare(signer.Certificate.PublicKey.Oid.Value, "1.2.840.10040.4.1", StringComparison.Ordinal) == 0)
			{
				cmsg_SIGNER_ENCODE_INFO.HashEncryptionAlgorithm.pszObjId = "1.2.840.10040.4.3";
			}
			cmsg_SIGNER_ENCODE_INFO.cAuthAttr = (uint)signer.SignedAttributes.Count;
			cmsg_SIGNER_ENCODE_INFO.rgAuthAttr = PkcsUtils.CreateCryptAttributes(signer.SignedAttributes);
			cmsg_SIGNER_ENCODE_INFO.cUnauthAttr = (uint)signer.UnsignedAttributes.Count;
			cmsg_SIGNER_ENCODE_INFO.rgUnauthAttr = PkcsUtils.CreateCryptAttributes(signer.UnsignedAttributes);
			if (signer.SignerIdentifierType == SubjectIdentifierType.NoSignature)
			{
				cmsg_SIGNER_ENCODE_INFO.HashEncryptionAlgorithm.pszObjId = "1.3.6.1.5.5.7.6.2";
				cmsg_SIGNER_ENCODE_INFO.pCertInfo = IntPtr.Zero;
				cmsg_SIGNER_ENCODE_INFO.dwKeySpec = num;
				if (!CAPI.CryptAcquireContext(ref invalidHandle, null, null, 1U, 4026531840U) && !CAPI.CryptAcquireContext(ref invalidHandle, null, null, 1U, 0U))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				cmsg_SIGNER_ENCODE_INFO.hCryptProv = invalidHandle.DangerousGetHandle();
				GC.SuppressFinalize(invalidHandle);
				cmsg_SIGNER_ENCODE_INFO.SignerId.dwIdChoice = 1U;
				X500DistinguishedName x500DistinguishedName = new X500DistinguishedName("CN=Dummy Signer");
				x500DistinguishedName.Oid = new Oid("1.3.6.1.4.1.311.21.9");
				cmsg_SIGNER_ENCODE_INFO.SignerId.Value.IssuerSerialNumber.Issuer.cbData = (uint)x500DistinguishedName.RawData.Length;
				SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)cmsg_SIGNER_ENCODE_INFO.SignerId.Value.IssuerSerialNumber.Issuer.cbData)));
				Marshal.Copy(x500DistinguishedName.RawData, 0, safeLocalAllocHandle.DangerousGetHandle(), x500DistinguishedName.RawData.Length);
				cmsg_SIGNER_ENCODE_INFO.SignerId.Value.IssuerSerialNumber.Issuer.pbData = safeLocalAllocHandle.DangerousGetHandle();
				GC.SuppressFinalize(safeLocalAllocHandle);
				cmsg_SIGNER_ENCODE_INFO.SignerId.Value.IssuerSerialNumber.SerialNumber.cbData = 1U;
				SafeLocalAllocHandle safeLocalAllocHandle2 = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)cmsg_SIGNER_ENCODE_INFO.SignerId.Value.IssuerSerialNumber.SerialNumber.cbData)));
				byte* ptr = (byte*)(void*)safeLocalAllocHandle2.DangerousGetHandle();
				*ptr = 0;
				cmsg_SIGNER_ENCODE_INFO.SignerId.Value.IssuerSerialNumber.SerialNumber.pbData = safeLocalAllocHandle2.DangerousGetHandle();
				GC.SuppressFinalize(safeLocalAllocHandle2);
				return cmsg_SIGNER_ENCODE_INFO;
			}
			else
			{
				SafeCertContextHandle certContext = X509Utils.GetCertContext(signer.Certificate);
				if (!CAPISafe.CryptAcquireCertificatePrivateKey(certContext, silent ? 70U : 6U, IntPtr.Zero, ref invalidHandle, ref num, ref flag))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
				cmsg_SIGNER_ENCODE_INFO.dwKeySpec = num;
				cmsg_SIGNER_ENCODE_INFO.hCryptProv = invalidHandle.DangerousGetHandle();
				GC.SuppressFinalize(invalidHandle);
				CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)certContext.DangerousGetHandle();
				cmsg_SIGNER_ENCODE_INFO.pCertInfo = cert_CONTEXT.pCertInfo;
				if (signer.SignerIdentifierType == SubjectIdentifierType.SubjectKeyIdentifier)
				{
					uint num2 = 0U;
					SafeLocalAllocHandle safeLocalAllocHandle3 = SafeLocalAllocHandle.InvalidHandle;
					if (!CAPISafe.CertGetCertificateContextProperty(certContext, 20U, safeLocalAllocHandle3, ref num2))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					if (num2 > 0U)
					{
						safeLocalAllocHandle3 = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)num2)));
						if (!CAPISafe.CertGetCertificateContextProperty(certContext, 20U, safeLocalAllocHandle3, ref num2))
						{
							throw new CryptographicException(Marshal.GetLastWin32Error());
						}
						cmsg_SIGNER_ENCODE_INFO.SignerId.dwIdChoice = 2U;
						cmsg_SIGNER_ENCODE_INFO.SignerId.Value.KeyId.cbData = num2;
						cmsg_SIGNER_ENCODE_INFO.SignerId.Value.KeyId.pbData = safeLocalAllocHandle3.DangerousGetHandle();
						GC.SuppressFinalize(safeLocalAllocHandle3);
					}
				}
				return cmsg_SIGNER_ENCODE_INFO;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009598 File Offset: 0x00008598
		internal static X509Certificate2Collection CreateBagOfCertificates(CmsSigner signer)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			x509Certificate2Collection.AddRange(signer.Certificates);
			if (signer.IncludeOption != X509IncludeOption.None)
			{
				if (signer.IncludeOption == X509IncludeOption.EndCertOnly)
				{
					x509Certificate2Collection.Add(signer.Certificate);
				}
				else
				{
					int num = 1;
					X509Chain x509Chain = new X509Chain();
					x509Chain.Build(signer.Certificate);
					if (x509Chain.ChainStatus.Length > 0 && (x509Chain.ChainStatus[0].Status & X509ChainStatusFlags.PartialChain) == X509ChainStatusFlags.PartialChain)
					{
						throw new CryptographicException(-2146762486);
					}
					if (signer.IncludeOption == X509IncludeOption.WholeChain)
					{
						num = x509Chain.ChainElements.Count;
					}
					else if (x509Chain.ChainElements.Count > 1)
					{
						num = x509Chain.ChainElements.Count - 1;
					}
					for (int i = 0; i < num; i++)
					{
						x509Certificate2Collection.Add(x509Chain.ChainElements[i].Certificate);
					}
				}
			}
			return x509Certificate2Collection;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00009680 File Offset: 0x00008680
		internal unsafe static SafeLocalAllocHandle CreateEncodedCertBlob(X509Certificate2Collection certificates)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (certificates.Count > 0)
			{
				safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr(certificates.Count * Marshal.SizeOf(typeof(CAPIBase.CRYPTOAPI_BLOB))));
				CAPIBase.CRYPTOAPI_BLOB* ptr = (CAPIBase.CRYPTOAPI_BLOB*)(void*)safeLocalAllocHandle.DangerousGetHandle();
				foreach (X509Certificate2 x509Certificate in certificates)
				{
					SafeCertContextHandle certContext = X509Utils.GetCertContext(x509Certificate);
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)certContext.DangerousGetHandle();
					ptr->cbData = cert_CONTEXT.cbCertEncoded;
					ptr->pbData = cert_CONTEXT.pbCertEncoded;
					ptr++;
				}
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00009728 File Offset: 0x00008728
		internal unsafe static uint AddCertsToMessage(SafeCryptMsgHandle safeCryptMsgHandle, X509Certificate2Collection bagOfCerts, X509Certificate2Collection chainOfCerts)
		{
			uint num = 0U;
			foreach (X509Certificate2 x509Certificate in chainOfCerts)
			{
				X509Certificate2Collection x509Certificate2Collection = bagOfCerts.Find(X509FindType.FindByThumbprint, x509Certificate.Thumbprint, false);
				if (x509Certificate2Collection.Count == 0)
				{
					SafeCertContextHandle certContext = X509Utils.GetCertContext(x509Certificate);
					CAPIBase.CERT_CONTEXT cert_CONTEXT = *(CAPIBase.CERT_CONTEXT*)(void*)certContext.DangerousGetHandle();
					CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
					cryptoapi_BLOB.cbData = cert_CONTEXT.cbCertEncoded;
					cryptoapi_BLOB.pbData = cert_CONTEXT.pbCertEncoded;
					if (!CAPI.CryptMsgControl(safeCryptMsgHandle, 0U, 10U, new IntPtr(&cryptoapi_BLOB)))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					num += 1U;
				}
			}
			return num;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x000097D0 File Offset: 0x000087D0
		internal static X509Certificate2 FindCertificate(SubjectIdentifier identifier, X509Certificate2Collection certificates)
		{
			X509Certificate2 x509Certificate = null;
			if (certificates != null && certificates.Count > 0)
			{
				switch (identifier.Type)
				{
				case SubjectIdentifierType.IssuerAndSerialNumber:
				{
					X509Certificate2Collection x509Certificate2Collection = certificates.Find(X509FindType.FindByIssuerDistinguishedName, ((X509IssuerSerial)identifier.Value).IssuerName, false);
					if (x509Certificate2Collection.Count > 0)
					{
						x509Certificate2Collection = x509Certificate2Collection.Find(X509FindType.FindBySerialNumber, ((X509IssuerSerial)identifier.Value).SerialNumber, false);
						if (x509Certificate2Collection.Count > 0)
						{
							x509Certificate = x509Certificate2Collection[0];
						}
					}
					break;
				}
				case SubjectIdentifierType.SubjectKeyIdentifier:
				{
					X509Certificate2Collection x509Certificate2Collection = certificates.Find(X509FindType.FindBySubjectKeyIdentifier, identifier.Value, false);
					if (x509Certificate2Collection.Count > 0)
					{
						x509Certificate = x509Certificate2Collection[0];
					}
					break;
				}
				}
			}
			return x509Certificate;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009881 File Offset: 0x00008881
		private static void checkErr(int err)
		{
			if (-2146889724 != err)
			{
				throw new CryptographicException(err);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009894 File Offset: 0x00008894
		internal unsafe static X509Certificate2 CreateDummyCertificate(CspParameters parameters)
		{
			SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
			SafeCryptProvHandle invalidHandle = SafeCryptProvHandle.InvalidHandle;
			uint num = 0U;
			if ((parameters.Flags & CspProviderFlags.UseMachineKeyStore) != CspProviderFlags.NoFlags)
			{
				num |= 32U;
			}
			if ((parameters.Flags & CspProviderFlags.UseDefaultKeyContainer) != CspProviderFlags.NoFlags)
			{
				num |= 4026531840U;
			}
			if ((parameters.Flags & CspProviderFlags.NoPrompt) != CspProviderFlags.NoFlags)
			{
				num |= 64U;
			}
			if (!CAPI.CryptAcquireContext(ref invalidHandle, parameters.KeyContainerName, parameters.ProviderName, (uint)parameters.ProviderType, num))
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			CAPIBase.CRYPT_KEY_PROV_INFO crypt_KEY_PROV_INFO = default(CAPIBase.CRYPT_KEY_PROV_INFO);
			crypt_KEY_PROV_INFO.pwszProvName = parameters.ProviderName;
			crypt_KEY_PROV_INFO.pwszContainerName = parameters.KeyContainerName;
			crypt_KEY_PROV_INFO.dwProvType = (uint)parameters.ProviderType;
			crypt_KEY_PROV_INFO.dwKeySpec = (uint)parameters.KeyNumber;
			crypt_KEY_PROV_INFO.dwFlags = (((parameters.Flags & CspProviderFlags.UseMachineKeyStore) == CspProviderFlags.UseMachineKeyStore) ? 32U : 0U);
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_KEY_PROV_INFO))));
			Marshal.StructureToPtr(crypt_KEY_PROV_INFO, safeLocalAllocHandle.DangerousGetHandle(), false);
			CAPIBase.CRYPT_ALGORITHM_IDENTIFIER crypt_ALGORITHM_IDENTIFIER = default(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER);
			crypt_ALGORITHM_IDENTIFIER.pszObjId = "1.3.14.3.2.29";
			SafeLocalAllocHandle safeLocalAllocHandle2 = CAPI.LocalAlloc(64U, new IntPtr(Marshal.SizeOf(typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER))));
			Marshal.StructureToPtr(crypt_ALGORITHM_IDENTIFIER, safeLocalAllocHandle2.DangerousGetHandle(), false);
			X500DistinguishedName x500DistinguishedName = new X500DistinguishedName("cn=CMS Signer Dummy Certificate");
			fixed (byte* rawData = x500DistinguishedName.RawData)
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = default(CAPIBase.CRYPTOAPI_BLOB);
				cryptoapi_BLOB.cbData = (uint)x500DistinguishedName.RawData.Length;
				cryptoapi_BLOB.pbData = new IntPtr((void*)rawData);
				safeCertContextHandle = CAPIUnsafe.CertCreateSelfSignCertificate(invalidHandle, new IntPtr((void*)(&cryptoapi_BLOB)), 1U, safeLocalAllocHandle.DangerousGetHandle(), safeLocalAllocHandle2.DangerousGetHandle(), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			}
			Marshal.DestroyStructure(safeLocalAllocHandle.DangerousGetHandle(), typeof(CAPIBase.CRYPT_KEY_PROV_INFO));
			safeLocalAllocHandle.Dispose();
			Marshal.DestroyStructure(safeLocalAllocHandle2.DangerousGetHandle(), typeof(CAPIBase.CRYPT_ALGORITHM_IDENTIFIER));
			safeLocalAllocHandle2.Dispose();
			if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			X509Certificate2 x509Certificate = new X509Certificate2(safeCertContextHandle.DangerousGetHandle());
			safeCertContextHandle.Dispose();
			return x509Certificate;
		}

		// Token: 0x040004A6 RID: 1190
		private static int m_cmsSupported = -1;

		// Token: 0x02000079 RID: 121
		private struct I_CRYPT_ATTRIBUTE
		{
			// Token: 0x040004A7 RID: 1191
			internal IntPtr pszObjId;

			// Token: 0x040004A8 RID: 1192
			internal uint cValue;

			// Token: 0x040004A9 RID: 1193
			internal IntPtr rgValue;
		}
	}
}

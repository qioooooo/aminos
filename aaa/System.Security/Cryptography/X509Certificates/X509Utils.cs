using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200007E RID: 126
	internal class X509Utils
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x0000B5E3 File Offset: 0x0000A5E3
		private X509Utils()
		{
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B5EC File Offset: 0x0000A5EC
		internal static uint MapRevocationFlags(X509RevocationMode revocationMode, X509RevocationFlag revocationFlag)
		{
			uint num = 0U;
			if (revocationMode == X509RevocationMode.NoCheck)
			{
				return num;
			}
			if (revocationMode == X509RevocationMode.Offline)
			{
				num |= 2147483648U;
			}
			if (revocationFlag == X509RevocationFlag.EndCertificateOnly)
			{
				num |= 268435456U;
			}
			else if (revocationFlag == X509RevocationFlag.EntireChain)
			{
				num |= 536870912U;
			}
			else
			{
				num |= 1073741824U;
			}
			return num;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B630 File Offset: 0x0000A630
		internal static string EncodeHexString(byte[] sArray)
		{
			return X509Utils.EncodeHexString(sArray, 0U, (uint)sArray.Length);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B63C File Offset: 0x0000A63C
		internal static string EncodeHexString(byte[] sArray, uint start, uint end)
		{
			string text = null;
			if (sArray != null)
			{
				char[] array = new char[(end - start) * 2U];
				uint num = start;
				uint num2 = 0U;
				while (num < end)
				{
					uint num3 = (uint)((sArray[(int)((UIntPtr)num)] & 240) >> 4);
					array[(int)((UIntPtr)(num2++))] = X509Utils.hexValues[(int)((UIntPtr)num3)];
					num3 = (uint)(sArray[(int)((UIntPtr)num)] & 15);
					array[(int)((UIntPtr)(num2++))] = X509Utils.hexValues[(int)((UIntPtr)num3)];
					num += 1U;
				}
				text = new string(array);
			}
			return text;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B6AA File Offset: 0x0000A6AA
		internal static string EncodeHexStringFromInt(byte[] sArray)
		{
			return X509Utils.EncodeHexStringFromInt(sArray, 0U, (uint)sArray.Length);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B6B8 File Offset: 0x0000A6B8
		internal static string EncodeHexStringFromInt(byte[] sArray, uint start, uint end)
		{
			string text = null;
			if (sArray != null)
			{
				char[] array = new char[(end - start) * 2U];
				uint num = end;
				uint num2 = 0U;
				while (num-- > start)
				{
					uint num3 = (uint)(sArray[(int)((UIntPtr)num)] & 240) >> 4;
					array[(int)((UIntPtr)(num2++))] = X509Utils.hexValues[(int)((UIntPtr)num3)];
					num3 = (uint)(sArray[(int)((UIntPtr)num)] & 15);
					array[(int)((UIntPtr)(num2++))] = X509Utils.hexValues[(int)((UIntPtr)num3)];
				}
				text = new string(array);
			}
			return text;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B726 File Offset: 0x0000A726
		internal static byte HexToByte(char val)
		{
			if (val <= '9' && val >= '0')
			{
				return (byte)(val - '0');
			}
			if (val >= 'a' && val <= 'f')
			{
				return (byte)(val - 'a' + '\n');
			}
			if (val >= 'A' && val <= 'F')
			{
				return (byte)(val - 'A' + '\n');
			}
			return byte.MaxValue;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000B764 File Offset: 0x0000A764
		internal static byte[] DecodeHexString(string s)
		{
			string text = Utils.DiscardWhiteSpaces(s);
			uint num = (uint)(text.Length / 2);
			byte[] array = new byte[num];
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)num))
			{
				array[num3] = (byte)(((int)X509Utils.HexToByte(text[num2]) << 4) | (int)X509Utils.HexToByte(text[num2 + 1]));
				num2 += 2;
				num3++;
			}
			return array;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B7C4 File Offset: 0x0000A7C4
		internal unsafe static bool MemEqual(byte* pbBuf1, uint cbBuf1, byte* pbBuf2, uint cbBuf2)
		{
			if (cbBuf1 != cbBuf2)
			{
				return false;
			}
			while (cbBuf1-- > 0U)
			{
				if (*(pbBuf1++) != *(pbBuf2++))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000B7EC File Offset: 0x0000A7EC
		internal static SafeLocalAllocHandle StringToAnsiPtr(string s)
		{
			byte[] array = new byte[s.Length + 1];
			Encoding.ASCII.GetBytes(s, 0, s.Length, array, 0);
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr(array.Length));
			Marshal.Copy(array, 0, safeLocalAllocHandle.DangerousGetHandle(), array.Length);
			return safeLocalAllocHandle;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000B83C File Offset: 0x0000A83C
		internal static SafeCertContextHandle GetCertContext(X509Certificate2 certificate)
		{
			SafeCertContextHandle safeCertContextHandle = CAPI.CertDuplicateCertificateContext(certificate.Handle);
			GC.KeepAlive(certificate);
			return safeCertContextHandle;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000B85C File Offset: 0x0000A85C
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

		// Token: 0x06000201 RID: 513 RVA: 0x0000B944 File Offset: 0x0000A944
		internal static SafeCertStoreHandle ExportToMemoryStore(X509Certificate2Collection collection)
		{
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AllFlags);
			storePermission.Assert();
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			safeCertStoreHandle = CAPI.CertOpenStore(new IntPtr(2L), 65537U, IntPtr.Zero, 8704U, null);
			if (safeCertStoreHandle == null || safeCertStoreHandle.IsInvalid)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			foreach (X509Certificate2 x509Certificate in collection)
			{
				if (!CAPI.CertAddCertificateLinkToStore(safeCertStoreHandle, X509Utils.GetCertContext(x509Certificate), 4U, SafeCertContextHandle.InvalidHandle))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return safeCertStoreHandle;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000B9D4 File Offset: 0x0000A9D4
		internal static uint OidToAlgId(string value)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.StringToAnsiPtr(value);
			return CAPI.CryptFindOIDInfo(1U, safeLocalAllocHandle, 0U).Algid;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000B9F8 File Offset: 0x0000A9F8
		internal static bool IsSelfSigned(X509Chain chain)
		{
			X509ChainElementCollection chainElements = chain.ChainElements;
			if (chainElements.Count != 1)
			{
				return false;
			}
			X509Certificate2 certificate = chainElements[0].Certificate;
			return string.Compare(certificate.SubjectName.Name, certificate.IssuerName.Name, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000BA48 File Offset: 0x0000AA48
		internal static SafeLocalAllocHandle CopyOidsToUnmanagedMemory(OidCollection oids)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (oids == null || oids.Count == 0)
			{
				return safeLocalAllocHandle;
			}
			int num = oids.Count * Marshal.SizeOf(typeof(IntPtr));
			int num2 = 0;
			foreach (Oid oid in oids)
			{
				num2 += oid.Value.Length + 1;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((ulong)(num + num2))));
			IntPtr intPtr = new IntPtr((long)safeLocalAllocHandle.DangerousGetHandle() + (long)num);
			for (int i = 0; i < oids.Count; i++)
			{
				Marshal.WriteIntPtr(new IntPtr((long)safeLocalAllocHandle.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(IntPtr)))), intPtr);
				byte[] bytes = Encoding.ASCII.GetBytes(oids[i].Value);
				Marshal.Copy(bytes, 0, intPtr, bytes.Length);
				intPtr = new IntPtr((long)intPtr + (long)oids[i].Value.Length + 1L);
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000BB64 File Offset: 0x0000AB64
		internal static X509Certificate2Collection GetCertificates(SafeCertStoreHandle safeCertStoreHandle)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			IntPtr intPtr = CAPI.CertEnumCertificatesInStore(safeCertStoreHandle, IntPtr.Zero);
			while (intPtr != IntPtr.Zero)
			{
				X509Certificate2 x509Certificate = new X509Certificate2(intPtr);
				x509Certificate2Collection.Add(x509Certificate);
				intPtr = CAPI.CertEnumCertificatesInStore(safeCertStoreHandle, intPtr);
			}
			return x509Certificate2Collection;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000BBAC File Offset: 0x0000ABAC
		internal unsafe static int BuildChain(IntPtr hChainEngine, SafeCertContextHandle pCertContext, X509Certificate2Collection extraStore, OidCollection applicationPolicy, OidCollection certificatePolicy, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, DateTime verificationTime, TimeSpan timeout, ref SafeCertChainHandle ppChainContext)
		{
			if (pCertContext == null || pCertContext.IsInvalid)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_InvalidContextHandle"), "pCertContext");
			}
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			if (extraStore != null && extraStore.Count > 0)
			{
				safeCertStoreHandle = X509Utils.ExportToMemoryStore(extraStore);
			}
			CAPIBase.CERT_CHAIN_PARA cert_CHAIN_PARA = default(CAPIBase.CERT_CHAIN_PARA);
			cert_CHAIN_PARA.cbSize = (uint)Marshal.SizeOf(cert_CHAIN_PARA);
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (applicationPolicy != null && applicationPolicy.Count > 0)
			{
				cert_CHAIN_PARA.RequestedUsage.dwType = 0U;
				cert_CHAIN_PARA.RequestedUsage.Usage.cUsageIdentifier = (uint)applicationPolicy.Count;
				safeLocalAllocHandle = X509Utils.CopyOidsToUnmanagedMemory(applicationPolicy);
				cert_CHAIN_PARA.RequestedUsage.Usage.rgpszUsageIdentifier = safeLocalAllocHandle.DangerousGetHandle();
			}
			SafeLocalAllocHandle safeLocalAllocHandle2 = SafeLocalAllocHandle.InvalidHandle;
			if (certificatePolicy != null && certificatePolicy.Count > 0)
			{
				cert_CHAIN_PARA.RequestedIssuancePolicy.dwType = 0U;
				cert_CHAIN_PARA.RequestedIssuancePolicy.Usage.cUsageIdentifier = (uint)certificatePolicy.Count;
				safeLocalAllocHandle2 = X509Utils.CopyOidsToUnmanagedMemory(certificatePolicy);
				cert_CHAIN_PARA.RequestedIssuancePolicy.Usage.rgpszUsageIdentifier = safeLocalAllocHandle2.DangerousGetHandle();
			}
			cert_CHAIN_PARA.dwUrlRetrievalTimeout = (uint)timeout.Milliseconds;
			global::System.Runtime.InteropServices.ComTypes.FILETIME filetime = default(global::System.Runtime.InteropServices.ComTypes.FILETIME);
			*(long*)(&filetime) = verificationTime.ToFileTime();
			uint num = X509Utils.MapRevocationFlags(revocationMode, revocationFlag);
			if (!CAPISafe.CertGetCertificateChain(hChainEngine, pCertContext, ref filetime, safeCertStoreHandle, ref cert_CHAIN_PARA, num, IntPtr.Zero, ref ppChainContext))
			{
				return Marshal.GetHRForLastWin32Error();
			}
			safeLocalAllocHandle.Dispose();
			safeLocalAllocHandle2.Dispose();
			return 0;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000BD10 File Offset: 0x0000AD10
		internal unsafe static int VerifyCertificate(SafeCertContextHandle pCertContext, OidCollection applicationPolicy, OidCollection certificatePolicy, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, DateTime verificationTime, TimeSpan timeout, X509Certificate2Collection extraStore, IntPtr pszPolicy, IntPtr pdwErrorStatus)
		{
			if (pCertContext == null || pCertContext.IsInvalid)
			{
				throw new ArgumentException("pCertContext");
			}
			CAPIBase.CERT_CHAIN_POLICY_PARA cert_CHAIN_POLICY_PARA = new CAPIBase.CERT_CHAIN_POLICY_PARA(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_PARA)));
			CAPIBase.CERT_CHAIN_POLICY_STATUS cert_CHAIN_POLICY_STATUS = new CAPIBase.CERT_CHAIN_POLICY_STATUS(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_STATUS)));
			SafeCertChainHandle invalidHandle = SafeCertChainHandle.InvalidHandle;
			int num = X509Utils.BuildChain(new IntPtr(0L), pCertContext, extraStore, applicationPolicy, certificatePolicy, revocationMode, revocationFlag, verificationTime, timeout, ref invalidHandle);
			if (num != 0)
			{
				return num;
			}
			if (!CAPISafe.CertVerifyCertificateChainPolicy(pszPolicy, invalidHandle, ref cert_CHAIN_POLICY_PARA, ref cert_CHAIN_POLICY_STATUS))
			{
				return Marshal.GetHRForLastWin32Error();
			}
			if (pdwErrorStatus != IntPtr.Zero)
			{
				*(int*)(void*)pdwErrorStatus = (int)cert_CHAIN_POLICY_STATUS.dwError;
			}
			if (cert_CHAIN_POLICY_STATUS.dwError != 0U)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x040004BB RID: 1211
		private static readonly char[] hexValues = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};
	}
}

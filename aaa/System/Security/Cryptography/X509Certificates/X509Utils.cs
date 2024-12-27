using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000347 RID: 839
	internal class X509Utils
	{
		// Token: 0x06001A58 RID: 6744 RVA: 0x0005BCC3 File Offset: 0x0005ACC3
		private X509Utils()
		{
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x0005BCCB File Offset: 0x0005ACCB
		internal static bool IsCertRdnCharString(uint dwValueType)
		{
			return (dwValueType & 255U) >= 3U;
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0005BCDC File Offset: 0x0005ACDC
		internal static X509ContentType MapContentType(uint contentType)
		{
			switch (contentType)
			{
			case 1U:
				return X509ContentType.Cert;
			case 4U:
				return X509ContentType.SerializedStore;
			case 5U:
				return X509ContentType.SerializedCert;
			case 8U:
			case 9U:
				return X509ContentType.Pkcs7;
			case 10U:
				return X509ContentType.Authenticode;
			case 12U:
				return X509ContentType.Pfx;
			}
			return X509ContentType.Unknown;
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x0005BD34 File Offset: 0x0005AD34
		internal static uint MapKeyStorageFlags(X509KeyStorageFlags keyStorageFlags)
		{
			uint num = 0U;
			if ((keyStorageFlags & X509KeyStorageFlags.UserKeySet) == X509KeyStorageFlags.UserKeySet)
			{
				num |= 4096U;
			}
			else if ((keyStorageFlags & X509KeyStorageFlags.MachineKeySet) == X509KeyStorageFlags.MachineKeySet)
			{
				num |= 32U;
			}
			if ((keyStorageFlags & X509KeyStorageFlags.Exportable) == X509KeyStorageFlags.Exportable)
			{
				num |= 1U;
			}
			if ((keyStorageFlags & X509KeyStorageFlags.UserProtected) == X509KeyStorageFlags.UserProtected)
			{
				num |= 2U;
			}
			return num;
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x0005BD74 File Offset: 0x0005AD74
		internal static uint MapX509StoreFlags(StoreLocation storeLocation, OpenFlags flags)
		{
			uint num = 0U;
			switch (flags & (OpenFlags.ReadWrite | OpenFlags.MaxAllowed))
			{
			case OpenFlags.ReadOnly:
				num |= 32768U;
				break;
			case OpenFlags.MaxAllowed:
				num |= 4096U;
				break;
			}
			if ((flags & OpenFlags.OpenExistingOnly) == OpenFlags.OpenExistingOnly)
			{
				num |= 16384U;
			}
			if ((flags & OpenFlags.IncludeArchived) == OpenFlags.IncludeArchived)
			{
				num |= 512U;
			}
			if (storeLocation == StoreLocation.LocalMachine)
			{
				num |= 131072U;
			}
			else if (storeLocation == StoreLocation.CurrentUser)
			{
				num |= 65536U;
			}
			return num;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x0005BDE8 File Offset: 0x0005ADE8
		internal static uint MapNameType(X509NameType nameType)
		{
			uint num;
			switch (nameType)
			{
			case X509NameType.SimpleName:
				num = 4U;
				break;
			case X509NameType.EmailName:
				num = 1U;
				break;
			case X509NameType.UpnName:
				num = 8U;
				break;
			case X509NameType.DnsName:
			case X509NameType.DnsFromAlternativeName:
				num = 6U;
				break;
			case X509NameType.UrlName:
				num = 7U;
				break;
			default:
				throw new ArgumentException(SR.GetString("Argument_InvalidNameType"));
			}
			return num;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0005BE40 File Offset: 0x0005AE40
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

		// Token: 0x06001A5F RID: 6751 RVA: 0x0005BE84 File Offset: 0x0005AE84
		internal static string EncodeHexString(byte[] sArray)
		{
			return X509Utils.EncodeHexString(sArray, 0U, (uint)sArray.Length);
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x0005BE90 File Offset: 0x0005AE90
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

		// Token: 0x06001A61 RID: 6753 RVA: 0x0005BF00 File Offset: 0x0005AF00
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

		// Token: 0x06001A62 RID: 6754 RVA: 0x0005BF6E File Offset: 0x0005AF6E
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

		// Token: 0x06001A63 RID: 6755 RVA: 0x0005BFAB File Offset: 0x0005AFAB
		internal static uint AlignedLength(uint length)
		{
			return (length + 7U) & 4294967288U;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x0005BFB3 File Offset: 0x0005AFB3
		internal static string DiscardWhiteSpaces(string inputBuffer)
		{
			return X509Utils.DiscardWhiteSpaces(inputBuffer, 0, inputBuffer.Length);
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x0005BFC4 File Offset: 0x0005AFC4
		internal static string DiscardWhiteSpaces(string inputBuffer, int inputOffset, int inputCount)
		{
			int num = 0;
			for (int i = 0; i < inputCount; i++)
			{
				if (char.IsWhiteSpace(inputBuffer[inputOffset + i]))
				{
					num++;
				}
			}
			char[] array = new char[inputCount - num];
			num = 0;
			for (int i = 0; i < inputCount; i++)
			{
				if (!char.IsWhiteSpace(inputBuffer[inputOffset + i]))
				{
					array[num++] = inputBuffer[inputOffset + i];
				}
			}
			return new string(array);
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x0005C030 File Offset: 0x0005B030
		internal static byte[] DecodeHexString(string s)
		{
			string text = X509Utils.DiscardWhiteSpaces(s);
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

		// Token: 0x06001A67 RID: 6759 RVA: 0x0005C090 File Offset: 0x0005B090
		internal static int GetHexArraySize(byte[] hex)
		{
			int num = hex.Length;
			while (num-- > 0 && hex[num] == 0)
			{
			}
			return num + 1;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x0005C0B4 File Offset: 0x0005B0B4
		internal static SafeLocalAllocHandle ByteToPtr(byte[] managed)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr(managed.Length));
			Marshal.Copy(managed, 0, safeLocalAllocHandle.DangerousGetHandle(), managed.Length);
			return safeLocalAllocHandle;
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0005C0E4 File Offset: 0x0005B0E4
		internal unsafe static void memcpy(IntPtr source, IntPtr dest, uint size)
		{
			for (uint num = 0U; num < size; num += 1U)
			{
				*(UIntPtr)((long)dest + (long)((ulong)num)) = Marshal.ReadByte(new IntPtr((long)source + (long)((ulong)num)));
			}
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0005C11C File Offset: 0x0005B11C
		internal static byte[] PtrToByte(IntPtr unmanaged, uint size)
		{
			byte[] array = new byte[size];
			Marshal.Copy(unmanaged, array, 0, array.Length);
			return array;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0005C13C File Offset: 0x0005B13C
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

		// Token: 0x06001A6C RID: 6764 RVA: 0x0005C164 File Offset: 0x0005B164
		internal static SafeLocalAllocHandle StringToAnsiPtr(string s)
		{
			byte[] array = new byte[s.Length + 1];
			Encoding.ASCII.GetBytes(s, 0, s.Length, array, 0);
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr(array.Length));
			Marshal.Copy(array, 0, safeLocalAllocHandle.DangerousGetHandle(), array.Length);
			return safeLocalAllocHandle;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0005C1B4 File Offset: 0x0005B1B4
		internal static SafeLocalAllocHandle StringToUniPtr(string s)
		{
			byte[] array = new byte[2 * (s.Length + 1)];
			Encoding.Unicode.GetBytes(s, 0, s.Length, array, 0);
			SafeLocalAllocHandle safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr(array.Length));
			Marshal.Copy(array, 0, safeLocalAllocHandle.DangerousGetHandle(), array.Length);
			return safeLocalAllocHandle;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0005C208 File Offset: 0x0005B208
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
				if (!CAPI.CertAddCertificateLinkToStore(safeCertStoreHandle, x509Certificate.CertContext, 4U, SafeCertContextHandle.InvalidHandle))
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return safeCertStoreHandle;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0005C298 File Offset: 0x0005B298
		internal static uint OidToAlgId(string value)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = X509Utils.StringToAnsiPtr(value);
			return CAPI.CryptFindOIDInfo(1U, safeLocalAllocHandle, OidGroup.AllGroups).Algid;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0005C2BC File Offset: 0x0005B2BC
		internal static string FindOidInfo(uint keyType, string keyValue, OidGroup oidGroup)
		{
			if (keyValue == null)
			{
				throw new ArgumentNullException("keyValue");
			}
			if (keyValue.Length == 0)
			{
				return null;
			}
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			switch (keyType)
			{
			case 1U:
				safeLocalAllocHandle = X509Utils.StringToAnsiPtr(keyValue);
				break;
			case 2U:
				safeLocalAllocHandle = X509Utils.StringToUniPtr(keyValue);
				break;
			}
			CAPIBase.CRYPT_OID_INFO crypt_OID_INFO = CAPI.CryptFindOIDInfo(keyType, safeLocalAllocHandle, oidGroup);
			if (crypt_OID_INFO.pszOID == null && oidGroup != OidGroup.AllGroups)
			{
				crypt_OID_INFO = CAPI.CryptFindOIDInfo(keyType, safeLocalAllocHandle, OidGroup.AllGroups);
			}
			if (keyType == 1U)
			{
				return crypt_OID_INFO.pwszName;
			}
			return crypt_OID_INFO.pszOID;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0005C33C File Offset: 0x0005B33C
		internal static void ValidateOidValue(string keyValue)
		{
			if (keyValue == null)
			{
				throw new ArgumentNullException("keyValue");
			}
			int length = keyValue.Length;
			if (length >= 2)
			{
				char c = keyValue[0];
				if ((c == '0' || c == '1' || c == '2') && keyValue[1] == '.' && keyValue[length - 1] != '.')
				{
					bool flag = false;
					for (int i = 1; i < length; i++)
					{
						if (!char.IsDigit(keyValue[i]))
						{
							if (keyValue[i] != '.' || keyValue[i + 1] == '.')
							{
								goto IL_0082;
							}
							flag = true;
						}
					}
					if (flag)
					{
						return;
					}
				}
			}
			IL_0082:
			throw new ArgumentException(SR.GetString("Argument_InvalidOidValue"));
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0005C3DC File Offset: 0x0005B3DC
		internal static SafeLocalAllocHandle CopyOidsToUnmanagedMemory(OidCollection oids)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (oids == null || oids.Count == 0)
			{
				return safeLocalAllocHandle;
			}
			List<string> list = new List<string>();
			foreach (Oid oid in oids)
			{
				list.Add(oid.Value);
			}
			IntPtr zero = IntPtr.Zero;
			checked
			{
				int num = list.Count * Marshal.SizeOf(typeof(IntPtr));
				int num2 = 0;
				foreach (string text in list)
				{
					num2 += text.Length + 1;
				}
				safeLocalAllocHandle = CAPI.LocalAlloc(64U, new IntPtr((long)((uint)num + (uint)num2)));
				zero = new IntPtr((long)safeLocalAllocHandle.DangerousGetHandle() + unchecked((long)num));
			}
			for (int i = 0; i < list.Count; i++)
			{
				Marshal.WriteIntPtr(new IntPtr((long)safeLocalAllocHandle.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(IntPtr)))), zero);
				byte[] bytes = Encoding.ASCII.GetBytes(list[i]);
				Marshal.Copy(bytes, 0, zero, bytes.Length);
				zero = new IntPtr((long)zero + (long)list[i].Length + 1L);
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0005C540 File Offset: 0x0005B540
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

		// Token: 0x06001A74 RID: 6772 RVA: 0x0005C588 File Offset: 0x0005B588
		internal unsafe static int VerifyCertificate(SafeCertContextHandle pCertContext, OidCollection applicationPolicy, OidCollection certificatePolicy, X509RevocationMode revocationMode, X509RevocationFlag revocationFlag, DateTime verificationTime, TimeSpan timeout, X509Certificate2Collection extraStore, IntPtr pszPolicy, IntPtr pdwErrorStatus)
		{
			if (pCertContext == null || pCertContext.IsInvalid)
			{
				throw new ArgumentException("pCertContext");
			}
			CAPIBase.CERT_CHAIN_POLICY_PARA cert_CHAIN_POLICY_PARA = new CAPIBase.CERT_CHAIN_POLICY_PARA(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_PARA)));
			CAPIBase.CERT_CHAIN_POLICY_STATUS cert_CHAIN_POLICY_STATUS = new CAPIBase.CERT_CHAIN_POLICY_STATUS(Marshal.SizeOf(typeof(CAPIBase.CERT_CHAIN_POLICY_STATUS)));
			SafeCertChainHandle invalidHandle = SafeCertChainHandle.InvalidHandle;
			int num = X509Chain.BuildChain(new IntPtr(0L), pCertContext, extraStore, applicationPolicy, certificatePolicy, revocationMode, revocationFlag, verificationTime, timeout, ref invalidHandle);
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

		// Token: 0x06001A75 RID: 6773 RVA: 0x0005C644 File Offset: 0x0005B644
		internal static string GetSystemErrorString(int hr)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			uint num = CAPISafe.FormatMessage(4608U, IntPtr.Zero, (uint)hr, CAPISafe.GetUserDefaultLCID(), stringBuilder, 511U, IntPtr.Zero);
			if (num != 0U)
			{
				return stringBuilder.ToString();
			}
			return SR.GetString("Unknown_Error");
		}

		// Token: 0x04001B39 RID: 6969
		private static readonly char[] hexValues = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};
	}
}

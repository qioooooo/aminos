using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008A9 RID: 2217
	internal static class X509Utils
	{
		// Token: 0x06005110 RID: 20752 RVA: 0x00123A65 File Offset: 0x00122A65
		internal static int OidToAlgId(string oid)
		{
			return X509Utils.OidToAlgId(oid, OidGroup.AllGroups);
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x00123A70 File Offset: 0x00122A70
		internal static int OidToAlgId(string oid, OidGroup group)
		{
			if (oid == null)
			{
				return 32772;
			}
			string text = CryptoConfig.MapNameToOID(oid, group);
			if (text == null)
			{
				text = oid;
			}
			return X509Utils.OidToAlgIdStrict(text, group);
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x00123A9C File Offset: 0x00122A9C
		internal static int OidToAlgIdStrict(string oid, OidGroup group)
		{
			int num;
			if (string.Equals(oid, "2.16.840.1.101.3.4.2.1", StringComparison.Ordinal))
			{
				num = 32780;
			}
			else if (string.Equals(oid, "2.16.840.1.101.3.4.2.2", StringComparison.Ordinal))
			{
				num = 32781;
			}
			else if (string.Equals(oid, "2.16.840.1.101.3.4.2.3", StringComparison.Ordinal))
			{
				num = 32782;
			}
			else
			{
				num = X509Utils._GetAlgIdFromOid(oid, group);
			}
			if (num == 0 || num == -1)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidOID"));
			}
			return num;
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x00123B10 File Offset: 0x00122B10
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

		// Token: 0x06005114 RID: 20756 RVA: 0x00123B68 File Offset: 0x00122B68
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

		// Token: 0x06005115 RID: 20757 RVA: 0x00123BA8 File Offset: 0x00122BA8
		internal static SafeCertStoreHandle ExportCertToMemoryStore(X509Certificate certificate)
		{
			SafeCertStoreHandle invalidHandle = SafeCertStoreHandle.InvalidHandle;
			X509Utils._OpenX509Store(2U, 8704U, null, ref invalidHandle);
			X509Utils._AddCertificateToStore(invalidHandle, certificate.CertContext);
			return invalidHandle;
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00123BD8 File Offset: 0x00122BD8
		internal static IntPtr PasswordToCoTaskMemUni(object password)
		{
			if (password != null)
			{
				string text = password as string;
				if (text != null)
				{
					return Marshal.StringToCoTaskMemUni(text);
				}
				SecureString secureString = password as SecureString;
				if (secureString != null)
				{
					return Marshal.SecureStringToCoTaskMemUnicode(secureString);
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06005117 RID: 20759
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _AddCertificateToStore(SafeCertStoreHandle safeCertStoreHandle, SafeCertContextHandle safeCertContext);

		// Token: 0x06005118 RID: 20760
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _DuplicateCertContext(IntPtr handle, ref SafeCertContextHandle safeCertContext);

		// Token: 0x06005119 RID: 20761
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _ExportCertificatesToBlob(SafeCertStoreHandle safeCertStoreHandle, X509ContentType contentType, IntPtr password);

		// Token: 0x0600511A RID: 20762
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int _GetAlgIdFromOid(string oid, OidGroup group);

		// Token: 0x0600511B RID: 20763
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetCertRawData(SafeCertContextHandle safeCertContext);

		// Token: 0x0600511C RID: 20764
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetDateNotAfter(SafeCertContextHandle safeCertContext, ref Win32Native.FILE_TIME fileTime);

		// Token: 0x0600511D RID: 20765
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _GetDateNotBefore(SafeCertContextHandle safeCertContext, ref Win32Native.FILE_TIME fileTime);

		// Token: 0x0600511E RID: 20766
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetFriendlyNameFromOid(string oid);

		// Token: 0x0600511F RID: 20767
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetIssuerName(SafeCertContextHandle safeCertContext, bool legacyV1Mode);

		// Token: 0x06005120 RID: 20768
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetOidFromFriendlyName(string oid, OidGroup group);

		// Token: 0x06005121 RID: 20769
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetPublicKeyOid(SafeCertContextHandle safeCertContext);

		// Token: 0x06005122 RID: 20770
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetPublicKeyParameters(SafeCertContextHandle safeCertContext);

		// Token: 0x06005123 RID: 20771
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetPublicKeyValue(SafeCertContextHandle safeCertContext);

		// Token: 0x06005124 RID: 20772
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string _GetSubjectInfo(SafeCertContextHandle safeCertContext, uint displayType, bool legacyV1Mode);

		// Token: 0x06005125 RID: 20773
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetSerialNumber(SafeCertContextHandle safeCertContext);

		// Token: 0x06005126 RID: 20774
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern byte[] _GetThumbprint(SafeCertContextHandle safeCertContext);

		// Token: 0x06005127 RID: 20775
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _LoadCertFromBlob(byte[] rawData, IntPtr password, uint dwFlags, bool persistKeySet, ref SafeCertContextHandle pCertCtx);

		// Token: 0x06005128 RID: 20776
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _LoadCertFromFile(string fileName, IntPtr password, uint dwFlags, bool persistKeySet, ref SafeCertContextHandle pCertCtx);

		// Token: 0x06005129 RID: 20777
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _OpenX509Store(uint storeType, uint flags, string storeName, ref SafeCertStoreHandle safeCertStoreHandle);

		// Token: 0x0600512A RID: 20778
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint _QueryCertBlobType(byte[] rawData);

		// Token: 0x0600512B RID: 20779
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint _QueryCertFileType(string fileName);
	}
}

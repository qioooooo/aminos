using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x0200004E RID: 78
	internal sealed class CAPI : CAPIMethods
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00002DDE File Offset: 0x00001DDE
		private CAPI()
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002DE8 File Offset: 0x00001DE8
		internal static byte[] BlobToByteArray(IntPtr pBlob)
		{
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(pBlob, typeof(CAPIBase.CRYPTOAPI_BLOB));
			if (cryptoapi_BLOB.cbData == 0U)
			{
				return new byte[0];
			}
			return CAPI.BlobToByteArray(cryptoapi_BLOB);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002E24 File Offset: 0x00001E24
		internal static byte[] BlobToByteArray(CAPIBase.CRYPTOAPI_BLOB blob)
		{
			if (blob.cbData == 0U)
			{
				return new byte[0];
			}
			byte[] array = new byte[blob.cbData];
			Marshal.Copy(blob.pbData, array, 0, array.Length);
			return array;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002E64 File Offset: 0x00001E64
		internal unsafe static bool DecodeObject(IntPtr pszStructType, IntPtr pbEncoded, uint cbEncoded, out SafeLocalAllocHandle decodedValue, out uint cbDecodedValue)
		{
			decodedValue = SafeLocalAllocHandle.InvalidHandle;
			cbDecodedValue = 0U;
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptDecodeObject(65537U, pszStructType, pbEncoded, cbEncoded, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CryptDecodeObject(65537U, pszStructType, pbEncoded, cbEncoded, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			decodedValue = safeLocalAllocHandle;
			cbDecodedValue = num;
			return true;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002ED4 File Offset: 0x00001ED4
		internal unsafe static bool DecodeObject(IntPtr pszStructType, byte[] pbEncoded, out SafeLocalAllocHandle decodedValue, out uint cbDecodedValue)
		{
			decodedValue = SafeLocalAllocHandle.InvalidHandle;
			cbDecodedValue = 0U;
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptDecodeObject(65537U, pszStructType, pbEncoded, (uint)pbEncoded.Length, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CryptDecodeObject(65537U, pszStructType, pbEncoded, (uint)pbEncoded.Length, 0U, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			decodedValue = safeLocalAllocHandle;
			cbDecodedValue = num;
			return true;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002F44 File Offset: 0x00001F44
		internal unsafe static bool EncodeObject(IntPtr lpszStructType, IntPtr pvStructInfo, out byte[] encodedData)
		{
			encodedData = new byte[0];
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptEncodeObject(65537U, lpszStructType, pvStructInfo, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CryptEncodeObject(65537U, lpszStructType, pvStructInfo, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			encodedData = new byte[num];
			Marshal.Copy(safeLocalAllocHandle.DangerousGetHandle(), encodedData, 0, (int)num);
			safeLocalAllocHandle.Dispose();
			return true;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002FC0 File Offset: 0x00001FC0
		internal unsafe static bool EncodeObject(string lpszStructType, IntPtr pvStructInfo, out byte[] encodedData)
		{
			encodedData = new byte[0];
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptEncodeObject(65537U, lpszStructType, pvStructInfo, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CryptEncodeObject(65537U, lpszStructType, pvStructInfo, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return false;
			}
			encodedData = new byte[num];
			Marshal.Copy(safeLocalAllocHandle.DangerousGetHandle(), encodedData, 0, (int)num);
			safeLocalAllocHandle.Dispose();
			return true;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000303C File Offset: 0x0000203C
		internal new static SafeLocalAllocHandle LocalAlloc(uint uFlags, IntPtr sizetdwBytes)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = CAPISafe.LocalAlloc(uFlags, sizetdwBytes);
			if (safeLocalAllocHandle == null || safeLocalAllocHandle.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003064 File Offset: 0x00002064
		internal new static bool CryptAcquireContext([In] [Out] ref SafeCryptProvHandle hCryptProv, [MarshalAs(UnmanagedType.LPStr)] [In] string pwszContainer, [MarshalAs(UnmanagedType.LPStr)] [In] string pwszProvider, [In] uint dwProvType, [In] uint dwFlags)
		{
			CspParameters cspParameters = new CspParameters();
			cspParameters.ProviderName = pwszProvider;
			cspParameters.KeyContainerName = pwszContainer;
			cspParameters.ProviderType = (int)dwProvType;
			cspParameters.KeyNumber = -1;
			cspParameters.Flags = (((dwFlags & 32U) == 32U) ? CspProviderFlags.UseMachineKeyStore : CspProviderFlags.NoFlags);
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
			KeyContainerPermissionAccessEntry keyContainerPermissionAccessEntry = new KeyContainerPermissionAccessEntry(cspParameters, KeyContainerPermissionFlags.Open);
			keyContainerPermission.AccessEntries.Add(keyContainerPermissionAccessEntry);
			keyContainerPermission.Demand();
			bool flag = CAPIUnsafe.CryptAcquireContext(ref hCryptProv, pwszContainer, pwszProvider, dwProvType, dwFlags);
			if (!flag && Marshal.GetLastWin32Error() == -2146893802)
			{
				flag = CAPIUnsafe.CryptAcquireContext(ref hCryptProv, pwszContainer, pwszProvider, dwProvType, dwFlags | 8U);
			}
			return flag;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000030F4 File Offset: 0x000020F4
		internal static bool CryptAcquireContext(ref SafeCryptProvHandle hCryptProv, IntPtr pwszContainer, IntPtr pwszProvider, uint dwProvType, uint dwFlags)
		{
			string text = null;
			if (pwszContainer != IntPtr.Zero)
			{
				text = Marshal.PtrToStringUni(pwszContainer);
			}
			string text2 = null;
			if (pwszProvider != IntPtr.Zero)
			{
				text2 = Marshal.PtrToStringUni(pwszProvider);
			}
			return CAPI.CryptAcquireContext(ref hCryptProv, text, text2, dwProvType, dwFlags);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003138 File Offset: 0x00002138
		internal new static CAPIBase.CRYPT_OID_INFO CryptFindOIDInfo([In] uint dwKeyType, [In] IntPtr pvKey, [In] uint dwGroupId)
		{
			if (pvKey == IntPtr.Zero)
			{
				throw new ArgumentNullException("pvKey");
			}
			CAPIBase.CRYPT_OID_INFO crypt_OID_INFO = new CAPIBase.CRYPT_OID_INFO(Marshal.SizeOf(typeof(CAPIBase.CRYPT_OID_INFO)));
			IntPtr intPtr = CAPISafe.CryptFindOIDInfo(dwKeyType, pvKey, dwGroupId);
			if (intPtr != IntPtr.Zero)
			{
				crypt_OID_INFO = (CAPIBase.CRYPT_OID_INFO)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CRYPT_OID_INFO));
			}
			return crypt_OID_INFO;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000031A0 File Offset: 0x000021A0
		internal new static CAPIBase.CRYPT_OID_INFO CryptFindOIDInfo([In] uint dwKeyType, [In] SafeLocalAllocHandle pvKey, [In] uint dwGroupId)
		{
			if (pvKey == null)
			{
				throw new ArgumentNullException("pvKey");
			}
			if (pvKey.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "pvKey");
			}
			CAPIBase.CRYPT_OID_INFO crypt_OID_INFO = new CAPIBase.CRYPT_OID_INFO(Marshal.SizeOf(typeof(CAPIBase.CRYPT_OID_INFO)));
			IntPtr intPtr = CAPISafe.CryptFindOIDInfo(dwKeyType, pvKey, dwGroupId);
			if (intPtr != IntPtr.Zero)
			{
				crypt_OID_INFO = (CAPIBase.CRYPT_OID_INFO)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CRYPT_OID_INFO));
			}
			return crypt_OID_INFO;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000321B File Offset: 0x0000221B
		internal new static bool CryptMsgControl([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara)
		{
			return CAPIUnsafe.CryptMsgControl(hCryptMsg, dwFlags, dwCtrlType, pvCtrlPara);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003226 File Offset: 0x00002226
		internal new static bool CryptMsgCountersign([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwIndex, [In] uint cCountersigners, [In] IntPtr rgCountersigners)
		{
			return CAPIUnsafe.CryptMsgCountersign(hCryptMsg, dwIndex, cCountersigners, rgCountersigners);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003231 File Offset: 0x00002231
		internal new static SafeCryptMsgHandle CryptMsgOpenToEncode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr pvMsgEncodeInfo, [In] IntPtr pszInnerContentObjID, [In] IntPtr pStreamInfo)
		{
			return CAPIUnsafe.CryptMsgOpenToEncode(dwMsgEncodingType, dwFlags, dwMsgType, pvMsgEncodeInfo, pszInnerContentObjID, pStreamInfo);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003240 File Offset: 0x00002240
		internal new static SafeCryptMsgHandle CryptMsgOpenToEncode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr pvMsgEncodeInfo, [In] string pszInnerContentObjID, [In] IntPtr pStreamInfo)
		{
			return CAPIUnsafe.CryptMsgOpenToEncode(dwMsgEncodingType, dwFlags, dwMsgType, pvMsgEncodeInfo, pszInnerContentObjID, pStreamInfo);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000324F File Offset: 0x0000224F
		internal new static SafeCertContextHandle CertDuplicateCertificateContext([In] IntPtr pCertContext)
		{
			if (pCertContext == IntPtr.Zero)
			{
				return SafeCertContextHandle.InvalidHandle;
			}
			return CAPISafe.CertDuplicateCertificateContext(pCertContext);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000326C File Offset: 0x0000226C
		internal new static IntPtr CertEnumCertificatesInStore([In] SafeCertStoreHandle hCertStore, [In] IntPtr pPrevCertContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "hCertStore");
			}
			if (pPrevCertContext == IntPtr.Zero)
			{
				StorePermission storePermission = new StorePermission(StorePermissionFlags.EnumerateCertificates);
				storePermission.Demand();
			}
			IntPtr intPtr = CAPIUnsafe.CertEnumCertificatesInStore(hCertStore, pPrevCertContext);
			if (intPtr == IntPtr.Zero)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != -2146885628)
				{
					CAPISafe.CertFreeCertificateContext(intPtr);
					throw new CryptographicException(lastWin32Error);
				}
			}
			return intPtr;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000032F4 File Offset: 0x000022F4
		internal new static bool CertAddCertificateContextToStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pCertContext, [In] uint dwAddDisposition, [In] [Out] SafeCertContextHandle ppStoreContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "hCertStore");
			}
			if (pCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (pCertContext.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "pCertContext");
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AddToStore);
			storePermission.Demand();
			return CAPIUnsafe.CertAddCertificateContextToStore(hCertStore, pCertContext, dwAddDisposition, ppStoreContext);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003370 File Offset: 0x00002370
		internal new static bool CertAddCertificateLinkToStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pCertContext, [In] uint dwAddDisposition, [In] [Out] SafeCertContextHandle ppStoreContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "hCertStore");
			}
			if (pCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (pCertContext.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "pCertContext");
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AddToStore);
			storePermission.Demand();
			return CAPIUnsafe.CertAddCertificateLinkToStore(hCertStore, pCertContext, dwAddDisposition, ppStoreContext);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000033EC File Offset: 0x000023EC
		internal new static SafeCertStoreHandle CertOpenStore([In] IntPtr lpszStoreProvider, [In] uint dwMsgAndCertEncodingType, [In] IntPtr hCryptProv, [In] uint dwFlags, [In] string pvPara)
		{
			if (lpszStoreProvider != new IntPtr(2L) && lpszStoreProvider != new IntPtr(10L))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Argument_InvalidValue"), "lpszStoreProvider");
			}
			if (((dwFlags & 131072U) == 131072U || (dwFlags & 524288U) == 524288U || (dwFlags & 589824U) == 589824U) && pvPara != null && pvPara.StartsWith("\\\\", StringComparison.Ordinal))
			{
				new PermissionSet(PermissionState.Unrestricted).Demand();
			}
			if ((dwFlags & 16U) == 16U)
			{
				StorePermission storePermission = new StorePermission(StorePermissionFlags.DeleteStore);
				storePermission.Demand();
			}
			else
			{
				StorePermission storePermission2 = new StorePermission(StorePermissionFlags.OpenStore);
				storePermission2.Demand();
			}
			if ((dwFlags & 8192U) == 8192U)
			{
				StorePermission storePermission3 = new StorePermission(StorePermissionFlags.CreateStore);
				storePermission3.Demand();
			}
			if ((dwFlags & 16384U) == 0U)
			{
				StorePermission storePermission4 = new StorePermission(StorePermissionFlags.CreateStore);
				storePermission4.Demand();
			}
			return CAPIUnsafe.CertOpenStore(lpszStoreProvider, dwMsgAndCertEncodingType, hCryptProv, dwFlags | 4U, pvPara);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000034D8 File Offset: 0x000024D8
		internal new static bool CryptProtectData([In] IntPtr pDataIn, [In] string szDataDescr, [In] IntPtr pOptionalEntropy, [In] IntPtr pvReserved, [In] IntPtr pPromptStruct, [In] uint dwFlags, [In] [Out] IntPtr pDataBlob)
		{
			DataProtectionPermission dataProtectionPermission = new DataProtectionPermission(DataProtectionPermissionFlags.ProtectData);
			dataProtectionPermission.Demand();
			return CAPIUnsafe.CryptProtectData(pDataIn, szDataDescr, pOptionalEntropy, pvReserved, pPromptStruct, dwFlags, pDataBlob);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003504 File Offset: 0x00002504
		internal new static bool CryptUnprotectData([In] IntPtr pDataIn, [In] IntPtr ppszDataDescr, [In] IntPtr pOptionalEntropy, [In] IntPtr pvReserved, [In] IntPtr pPromptStruct, [In] uint dwFlags, [In] [Out] IntPtr pDataBlob)
		{
			DataProtectionPermission dataProtectionPermission = new DataProtectionPermission(DataProtectionPermissionFlags.UnprotectData);
			dataProtectionPermission.Demand();
			return CAPIUnsafe.CryptUnprotectData(pDataIn, ppszDataDescr, pOptionalEntropy, pvReserved, pPromptStruct, dwFlags, pDataBlob);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003530 File Offset: 0x00002530
		internal new static int SystemFunction040([In] [Out] byte[] pDataIn, [In] uint cbDataIn, [In] uint dwFlags)
		{
			DataProtectionPermission dataProtectionPermission = new DataProtectionPermission(DataProtectionPermissionFlags.ProtectMemory);
			dataProtectionPermission.Demand();
			return CAPIUnsafe.SystemFunction040(pDataIn, cbDataIn, dwFlags);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003554 File Offset: 0x00002554
		internal new static int SystemFunction041([In] [Out] byte[] pDataIn, [In] uint cbDataIn, [In] uint dwFlags)
		{
			DataProtectionPermission dataProtectionPermission = new DataProtectionPermission(DataProtectionPermissionFlags.UnprotectMemory);
			dataProtectionPermission.Demand();
			return CAPIUnsafe.SystemFunction041(pDataIn, cbDataIn, dwFlags);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003578 File Offset: 0x00002578
		internal new static SafeCertContextHandle CryptUIDlgSelectCertificateW([MarshalAs(UnmanagedType.LPStruct)] [In] [Out] CAPIBase.CRYPTUI_SELECTCERTIFICATE_STRUCTW csc)
		{
			if (!Environment.UserInteractive)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Environment_NotInteractive"));
			}
			UIPermission uipermission = new UIPermission(UIPermissionWindow.SafeTopLevelWindows);
			uipermission.Demand();
			return CAPIUnsafe.CryptUIDlgSelectCertificateW(csc);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000035B0 File Offset: 0x000025B0
		internal new static bool CryptUIDlgViewCertificateW([MarshalAs(UnmanagedType.LPStruct)] [In] CAPIBase.CRYPTUI_VIEWCERTIFICATE_STRUCTW ViewInfo, [In] [Out] IntPtr pfPropertiesChanged)
		{
			if (!Environment.UserInteractive)
			{
				throw new InvalidOperationException(SecurityResources.GetResourceString("Environment_NotInteractive"));
			}
			UIPermission uipermission = new UIPermission(UIPermissionWindow.SafeTopLevelWindows);
			uipermission.Demand();
			return CAPIUnsafe.CryptUIDlgViewCertificateW(ViewInfo, pfPropertiesChanged);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000035E8 File Offset: 0x000025E8
		internal new static SafeCertContextHandle CertFindCertificateInStore([In] SafeCertStoreHandle hCertStore, [In] uint dwCertEncodingType, [In] uint dwFindFlags, [In] uint dwFindType, [In] IntPtr pvFindPara, [In] SafeCertContextHandle pPrevCertContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_InvalidHandle"), "hCertStore");
			}
			return CAPIUnsafe.CertFindCertificateInStore(hCertStore, dwCertEncodingType, dwFindFlags, dwFindType, pvFindPara, pPrevCertContext);
		}
	}
}

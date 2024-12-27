using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000317 RID: 791
	internal sealed class CAPI : CAPIMethods
	{
		// Token: 0x060018D5 RID: 6357 RVA: 0x000547FE File Offset: 0x000537FE
		private CAPI()
		{
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00054808 File Offset: 0x00053808
		internal static byte[] BlobToByteArray(IntPtr pBlob)
		{
			CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB = (CAPIBase.CRYPTOAPI_BLOB)Marshal.PtrToStructure(pBlob, typeof(CAPIBase.CRYPTOAPI_BLOB));
			if (cryptoapi_BLOB.cbData == 0U)
			{
				return new byte[0];
			}
			return CAPI.BlobToByteArray(cryptoapi_BLOB);
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x00054844 File Offset: 0x00053844
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

		// Token: 0x060018D8 RID: 6360 RVA: 0x00054884 File Offset: 0x00053884
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

		// Token: 0x060018D9 RID: 6361 RVA: 0x000548F4 File Offset: 0x000538F4
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

		// Token: 0x060018DA RID: 6362 RVA: 0x00054964 File Offset: 0x00053964
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

		// Token: 0x060018DB RID: 6363 RVA: 0x000549E0 File Offset: 0x000539E0
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

		// Token: 0x060018DC RID: 6364 RVA: 0x00054A5C File Offset: 0x00053A5C
		internal unsafe static string GetCertNameInfo([In] SafeCertContextHandle safeCertContext, [In] uint dwFlags, [In] uint dwDisplayType)
		{
			if (safeCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (safeCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "safeCertContext");
			}
			uint num = 33554435U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (dwDisplayType == 3U)
			{
				safeLocalAllocHandle = X509Utils.StringToAnsiPtr("2.5.4.3");
			}
			SafeLocalAllocHandle safeLocalAllocHandle2 = SafeLocalAllocHandle.InvalidHandle;
			uint num2 = CAPISafe.CertGetNameStringW(safeCertContext, dwDisplayType, dwFlags, (dwDisplayType == 3U) ? safeLocalAllocHandle.DangerousGetHandle() : new IntPtr((void*)(&num)), safeLocalAllocHandle2, 0U);
			if (num2 == 0U)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			safeLocalAllocHandle2 = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)(2U * num2))));
			if (CAPISafe.CertGetNameStringW(safeCertContext, dwDisplayType, dwFlags, (dwDisplayType == 3U) ? safeLocalAllocHandle.DangerousGetHandle() : new IntPtr((void*)(&num)), safeLocalAllocHandle2, num2) == 0U)
			{
				throw new CryptographicException(Marshal.GetLastWin32Error());
			}
			string text = Marshal.PtrToStringUni(safeLocalAllocHandle2.DangerousGetHandle());
			safeLocalAllocHandle2.Dispose();
			safeLocalAllocHandle.Dispose();
			return text;
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x00054B3C File Offset: 0x00053B3C
		internal new static SafeLocalAllocHandle LocalAlloc(uint uFlags, IntPtr sizetdwBytes)
		{
			SafeLocalAllocHandle safeLocalAllocHandle = CAPISafe.LocalAlloc(uFlags, sizetdwBytes);
			if (safeLocalAllocHandle == null || safeLocalAllocHandle.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			return safeLocalAllocHandle;
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x00054B64 File Offset: 0x00053B64
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

		// Token: 0x060018DF RID: 6367 RVA: 0x00054BF4 File Offset: 0x00053BF4
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

		// Token: 0x060018E0 RID: 6368 RVA: 0x00054C38 File Offset: 0x00053C38
		internal new static CAPIBase.CRYPT_OID_INFO CryptFindOIDInfo([In] uint dwKeyType, [In] IntPtr pvKey, [In] OidGroup dwGroupId)
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

		// Token: 0x060018E1 RID: 6369 RVA: 0x00054CA0 File Offset: 0x00053CA0
		internal new static CAPIBase.CRYPT_OID_INFO CryptFindOIDInfo([In] uint dwKeyType, [In] SafeLocalAllocHandle pvKey, [In] OidGroup dwGroupId)
		{
			if (pvKey == null)
			{
				throw new ArgumentNullException("pvKey");
			}
			if (pvKey.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "pvKey");
			}
			CAPIBase.CRYPT_OID_INFO crypt_OID_INFO = new CAPIBase.CRYPT_OID_INFO(Marshal.SizeOf(typeof(CAPIBase.CRYPT_OID_INFO)));
			IntPtr intPtr = CAPISafe.CryptFindOIDInfo(dwKeyType, pvKey, dwGroupId);
			if (intPtr != IntPtr.Zero)
			{
				crypt_OID_INFO = (CAPIBase.CRYPT_OID_INFO)Marshal.PtrToStructure(intPtr, typeof(CAPIBase.CRYPT_OID_INFO));
			}
			return crypt_OID_INFO;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00054D1C File Offset: 0x00053D1C
		internal unsafe static string CryptFormatObject([In] uint dwCertEncodingType, [In] uint dwFormatStrType, [In] string lpszStructType, [In] byte[] rawData)
		{
			if (rawData == null)
			{
				throw new ArgumentNullException("rawData");
			}
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptFormatObject(dwCertEncodingType, 0U, dwFormatStrType, IntPtr.Zero, lpszStructType, rawData, (uint)rawData.Length, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return X509Utils.EncodeHexString(rawData);
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CryptFormatObject(dwCertEncodingType, 0U, dwFormatStrType, IntPtr.Zero, lpszStructType, rawData, (uint)rawData.Length, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return X509Utils.EncodeHexString(rawData);
			}
			string text = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
			safeLocalAllocHandle.Dispose();
			return text;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00054DA8 File Offset: 0x00053DA8
		internal unsafe static string CryptFormatObject([In] uint dwCertEncodingType, [In] uint dwFormatStrType, [In] IntPtr lpszStructType, [In] byte[] rawData)
		{
			if (rawData == null)
			{
				throw new ArgumentNullException("rawData");
			}
			uint num = 0U;
			SafeLocalAllocHandle safeLocalAllocHandle = SafeLocalAllocHandle.InvalidHandle;
			if (!CAPISafe.CryptFormatObject(dwCertEncodingType, 0U, dwFormatStrType, IntPtr.Zero, lpszStructType, rawData, (uint)rawData.Length, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return X509Utils.EncodeHexString(rawData);
			}
			safeLocalAllocHandle = CAPI.LocalAlloc(0U, new IntPtr((long)((ulong)num)));
			if (!CAPISafe.CryptFormatObject(dwCertEncodingType, 0U, dwFormatStrType, IntPtr.Zero, lpszStructType, rawData, (uint)rawData.Length, safeLocalAllocHandle, new IntPtr((void*)(&num))))
			{
				return X509Utils.EncodeHexString(rawData);
			}
			string text = Marshal.PtrToStringUni(safeLocalAllocHandle.DangerousGetHandle());
			safeLocalAllocHandle.Dispose();
			return text;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00054E34 File Offset: 0x00053E34
		internal new static bool CryptMsgControl([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara)
		{
			return CAPIUnsafe.CryptMsgControl(hCryptMsg, dwFlags, dwCtrlType, pvCtrlPara);
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00054E3F File Offset: 0x00053E3F
		internal new static bool CryptMsgCountersign([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwIndex, [In] uint cCountersigners, [In] IntPtr rgCountersigners)
		{
			return CAPIUnsafe.CryptMsgCountersign(hCryptMsg, dwIndex, cCountersigners, rgCountersigners);
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00054E4A File Offset: 0x00053E4A
		internal new static SafeCryptMsgHandle CryptMsgOpenToEncode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr pvMsgEncodeInfo, [In] IntPtr pszInnerContentObjID, [In] IntPtr pStreamInfo)
		{
			return CAPIUnsafe.CryptMsgOpenToEncode(dwMsgEncodingType, dwFlags, dwMsgType, pvMsgEncodeInfo, pszInnerContentObjID, pStreamInfo);
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x00054E59 File Offset: 0x00053E59
		internal new static SafeCryptMsgHandle CryptMsgOpenToEncode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr pvMsgEncodeInfo, [In] string pszInnerContentObjID, [In] IntPtr pStreamInfo)
		{
			return CAPIUnsafe.CryptMsgOpenToEncode(dwMsgEncodingType, dwFlags, dwMsgType, pvMsgEncodeInfo, pszInnerContentObjID, pStreamInfo);
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x00054E68 File Offset: 0x00053E68
		internal new static bool CertSetCertificateContextProperty([In] IntPtr pCertContext, [In] uint dwPropId, [In] uint dwFlags, [In] IntPtr pvData)
		{
			if (pvData == IntPtr.Zero)
			{
				throw new ArgumentNullException("pvData");
			}
			if (dwPropId != 19U && dwPropId != 11U && dwPropId != 101U && dwPropId != 2U)
			{
				throw new ArgumentException(SR.GetString("Security_InvalidValue"), "dwFlags");
			}
			if (dwPropId == 19U || dwPropId == 11U || dwPropId == 2U)
			{
				new PermissionSet(PermissionState.Unrestricted).Demand();
			}
			return CAPIUnsafe.CertSetCertificateContextProperty(pCertContext, dwPropId, dwFlags, pvData);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00054ED8 File Offset: 0x00053ED8
		internal new static bool CertSetCertificateContextProperty([In] SafeCertContextHandle pCertContext, [In] uint dwPropId, [In] uint dwFlags, [In] IntPtr pvData)
		{
			if (pvData == IntPtr.Zero)
			{
				throw new ArgumentNullException("pvData");
			}
			if (dwPropId != 19U && dwPropId != 11U && dwPropId != 101U && dwPropId != 2U)
			{
				throw new ArgumentException(SR.GetString("Security_InvalidValue"), "dwFlags");
			}
			if (dwPropId == 19U || dwPropId == 11U || dwPropId == 2U)
			{
				new PermissionSet(PermissionState.Unrestricted).Demand();
			}
			return CAPIUnsafe.CertSetCertificateContextProperty(pCertContext, dwPropId, dwFlags, pvData);
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00054F48 File Offset: 0x00053F48
		internal new static bool CertSetCertificateContextProperty([In] SafeCertContextHandle pCertContext, [In] uint dwPropId, [In] uint dwFlags, [In] SafeLocalAllocHandle safeLocalAllocHandle)
		{
			if (pCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (pCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "pCertContext");
			}
			if (dwPropId != 19U && dwPropId != 11U && dwPropId != 101U && dwPropId != 2U)
			{
				throw new ArgumentException(SR.GetString("Security_InvalidValue"), "dwFlags");
			}
			if (dwPropId == 19U || dwPropId == 11U || dwPropId == 2U)
			{
				new PermissionSet(PermissionState.Unrestricted).Demand();
			}
			return CAPIUnsafe.CertSetCertificateContextProperty(pCertContext, dwPropId, dwFlags, safeLocalAllocHandle);
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00054FCA File Offset: 0x00053FCA
		internal new static SafeCertContextHandle CertDuplicateCertificateContext([In] IntPtr pCertContext)
		{
			if (pCertContext == IntPtr.Zero)
			{
				return SafeCertContextHandle.InvalidHandle;
			}
			return CAPISafe.CertDuplicateCertificateContext(pCertContext);
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x00054FE5 File Offset: 0x00053FE5
		internal new static SafeCertContextHandle CertDuplicateCertificateContext([In] SafeCertContextHandle pCertContext)
		{
			if (pCertContext == null || pCertContext.IsInvalid)
			{
				return SafeCertContextHandle.InvalidHandle;
			}
			return CAPISafe.CertDuplicateCertificateContext(pCertContext);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00055000 File Offset: 0x00054000
		internal new static IntPtr CertEnumCertificatesInStore([In] SafeCertStoreHandle hCertStore, [In] IntPtr pPrevCertContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
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

		// Token: 0x060018EE RID: 6382 RVA: 0x00055088 File Offset: 0x00054088
		internal new static SafeCertContextHandle CertEnumCertificatesInStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pPrevCertContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
			}
			if (pPrevCertContext.IsInvalid)
			{
				StorePermission storePermission = new StorePermission(StorePermissionFlags.EnumerateCertificates);
				storePermission.Demand();
			}
			SafeCertContextHandle safeCertContextHandle = CAPIUnsafe.CertEnumCertificatesInStore(hCertStore, pPrevCertContext);
			if (safeCertContextHandle == null || safeCertContextHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != -2146885628)
				{
					throw new CryptographicException(Marshal.GetLastWin32Error());
				}
			}
			return safeCertContextHandle;
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x00055108 File Offset: 0x00054108
		internal unsafe static bool CryptQueryObject([In] uint dwObjectType, [In] object pvObject, [In] uint dwExpectedContentTypeFlags, [In] uint dwExpectedFormatTypeFlags, [In] uint dwFlags, [Out] IntPtr pdwMsgAndCertEncodingType, [Out] IntPtr pdwContentType, [Out] IntPtr pdwFormatType, [In] [Out] IntPtr phCertStore, [In] [Out] IntPtr phMsg, [In] [Out] IntPtr ppvContext)
		{
			bool flag = false;
			GCHandle gchandle = GCHandle.Alloc(pvObject, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			try
			{
				if (pvObject == null)
				{
					throw new ArgumentNullException("pvObject");
				}
				if (dwObjectType == 1U)
				{
					string fullPath = Path.GetFullPath((string)pvObject);
					new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
				}
				else
				{
					CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB;
					cryptoapi_BLOB.cbData = (uint)((byte[])pvObject).Length;
					cryptoapi_BLOB.pbData = intPtr;
					intPtr = new IntPtr((void*)(&cryptoapi_BLOB));
				}
				flag = CAPIUnsafe.CryptQueryObject(dwObjectType, intPtr, dwExpectedContentTypeFlags, dwExpectedFormatTypeFlags, dwFlags, pdwMsgAndCertEncodingType, pdwContentType, pdwFormatType, phCertStore, phMsg, ppvContext);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return flag;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x000551B4 File Offset: 0x000541B4
		internal unsafe static bool CryptQueryObject([In] uint dwObjectType, [In] object pvObject, [In] uint dwExpectedContentTypeFlags, [In] uint dwExpectedFormatTypeFlags, [In] uint dwFlags, [Out] IntPtr pdwMsgAndCertEncodingType, [Out] IntPtr pdwContentType, [Out] IntPtr pdwFormatType, [In] [Out] ref SafeCertStoreHandle phCertStore, [In] [Out] IntPtr phMsg, [In] [Out] IntPtr ppvContext)
		{
			bool flag = false;
			GCHandle gchandle = GCHandle.Alloc(pvObject, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			try
			{
				if (pvObject == null)
				{
					throw new ArgumentNullException("pvObject");
				}
				if (dwObjectType == 1U)
				{
					string fullPath = Path.GetFullPath((string)pvObject);
					new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
				}
				else
				{
					CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB;
					cryptoapi_BLOB.cbData = (uint)((byte[])pvObject).Length;
					cryptoapi_BLOB.pbData = intPtr;
					intPtr = new IntPtr((void*)(&cryptoapi_BLOB));
				}
				flag = CAPIUnsafe.CryptQueryObject(dwObjectType, intPtr, dwExpectedContentTypeFlags, dwExpectedFormatTypeFlags, dwFlags, pdwMsgAndCertEncodingType, pdwContentType, pdwFormatType, ref phCertStore, phMsg, ppvContext);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			return flag;
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x00055260 File Offset: 0x00054260
		internal unsafe static SafeCertStoreHandle PFXImportCertStore([In] uint dwObjectType, [In] object pvObject, [In] string szPassword, [In] uint dwFlags, [In] bool persistKeyContainers)
		{
			if (pvObject == null)
			{
				throw new ArgumentNullException("pvObject");
			}
			byte[] array;
			if (dwObjectType == 1U)
			{
				FileStream fileStream = new FileStream((string)pvObject, FileMode.Open, FileAccess.Read);
				int num = (int)fileStream.Length;
				array = new byte[num];
				fileStream.Read(array, 0, num);
				fileStream.Close();
			}
			else
			{
				array = (byte[])pvObject;
			}
			if (persistKeyContainers)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.Create);
				keyContainerPermission.Demand();
			}
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			IntPtr intPtr = gchandle.AddrOfPinnedObject();
			try
			{
				CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB;
				cryptoapi_BLOB.cbData = (uint)array.Length;
				cryptoapi_BLOB.pbData = intPtr;
				safeCertStoreHandle = CAPIUnsafe.PFXImportCertStore(new IntPtr((void*)(&cryptoapi_BLOB)), szPassword, dwFlags);
			}
			finally
			{
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
			}
			if (!safeCertStoreHandle.IsInvalid && !persistKeyContainers)
			{
				IntPtr intPtr2 = CAPI.CertEnumCertificatesInStore(safeCertStoreHandle, IntPtr.Zero);
				while (intPtr2 != IntPtr.Zero)
				{
					CAPIBase.CRYPTOAPI_BLOB cryptoapi_BLOB2 = default(CAPIBase.CRYPTOAPI_BLOB);
					if (!CAPI.CertSetCertificateContextProperty(intPtr2, 101U, 1073741824U, new IntPtr((void*)(&cryptoapi_BLOB2))))
					{
						throw new CryptographicException(Marshal.GetLastWin32Error());
					}
					intPtr2 = CAPI.CertEnumCertificatesInStore(safeCertStoreHandle, intPtr2);
				}
			}
			return safeCertStoreHandle;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0005538C File Offset: 0x0005438C
		internal new static bool CertAddCertificateContextToStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pCertContext, [In] uint dwAddDisposition, [In] [Out] SafeCertContextHandle ppStoreContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
			}
			if (pCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (pCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "pCertContext");
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AddToStore);
			storePermission.Demand();
			return CAPIUnsafe.CertAddCertificateContextToStore(hCertStore, pCertContext, dwAddDisposition, ppStoreContext);
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00055408 File Offset: 0x00054408
		internal new static bool CertAddCertificateLinkToStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pCertContext, [In] uint dwAddDisposition, [In] [Out] SafeCertContextHandle ppStoreContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
			}
			if (pCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (pCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "pCertContext");
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.AddToStore);
			storePermission.Demand();
			return CAPIUnsafe.CertAddCertificateLinkToStore(hCertStore, pCertContext, dwAddDisposition, ppStoreContext);
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00055484 File Offset: 0x00054484
		internal new static bool CertDeleteCertificateFromStore([In] SafeCertContextHandle pCertContext)
		{
			if (pCertContext == null)
			{
				throw new ArgumentNullException("pCertContext");
			}
			if (pCertContext.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "pCertContext");
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.RemoveFromStore);
			storePermission.Demand();
			return CAPIUnsafe.CertDeleteCertificateFromStore(pCertContext);
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x000554D0 File Offset: 0x000544D0
		internal new static SafeCertStoreHandle CertOpenStore([In] IntPtr lpszStoreProvider, [In] uint dwMsgAndCertEncodingType, [In] IntPtr hCryptProv, [In] uint dwFlags, [In] string pvPara)
		{
			if (lpszStoreProvider != new IntPtr(2L) && lpszStoreProvider != new IntPtr(10L))
			{
				throw new ArgumentException(SR.GetString("Security_InvalidValue"), "lpszStoreProvider");
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

		// Token: 0x060018F6 RID: 6390 RVA: 0x000555BC File Offset: 0x000545BC
		internal new static SafeCertContextHandle CertFindCertificateInStore([In] SafeCertStoreHandle hCertStore, [In] uint dwCertEncodingType, [In] uint dwFindFlags, [In] uint dwFindType, [In] IntPtr pvFindPara, [In] SafeCertContextHandle pPrevCertContext)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
			}
			return CAPIUnsafe.CertFindCertificateInStore(hCertStore, dwCertEncodingType, dwFindFlags, dwFindType, pvFindPara, pPrevCertContext);
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x000555F8 File Offset: 0x000545F8
		internal new static bool PFXExportCertStore([In] SafeCertStoreHandle hCertStore, [In] [Out] IntPtr pPFX, [In] string szPassword, [In] uint dwFlags)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
			}
			KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Export);
			keyContainerPermission.Demand();
			return CAPIUnsafe.PFXExportCertStore(hCertStore, pPFX, szPassword, dwFlags);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x00055648 File Offset: 0x00054648
		internal new static bool CertSaveStore([In] SafeCertStoreHandle hCertStore, [In] uint dwMsgAndCertEncodingType, [In] uint dwSaveAs, [In] uint dwSaveTo, [In] [Out] IntPtr pvSaveToPara, [In] uint dwFlags)
		{
			if (hCertStore == null)
			{
				throw new ArgumentNullException("hCertStore");
			}
			if (hCertStore.IsInvalid)
			{
				throw new CryptographicException(SR.GetString("Cryptography_InvalidHandle"), "hCertStore");
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.EnumerateCertificates);
			storePermission.Demand();
			if (dwSaveTo == 3U || dwSaveTo == 4U)
			{
				throw new ArgumentException(SR.GetString("Security_InvalidValue"), "pvSaveToPara");
			}
			return CAPIUnsafe.CertSaveStore(hCertStore, dwMsgAndCertEncodingType, dwSaveAs, dwSaveTo, pvSaveToPara, dwFlags);
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000315 RID: 789
	[SuppressUnmanagedCodeSecurity]
	internal abstract class CAPIUnsafe : CAPISafe
	{
		// Token: 0x060018BC RID: 6332
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "CryptAcquireContextA")]
		protected internal static extern bool CryptAcquireContext([In] [Out] ref SafeCryptProvHandle hCryptProv, [MarshalAs(UnmanagedType.LPStr)] [In] string pszContainer, [MarshalAs(UnmanagedType.LPStr)] [In] string pszProvider, [In] uint dwProvType, [In] uint dwFlags);

		// Token: 0x060018BD RID: 6333
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertAddCertificateContextToStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pCertContext, [In] uint dwAddDisposition, [In] [Out] SafeCertContextHandle ppStoreContext);

		// Token: 0x060018BE RID: 6334
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertAddCertificateLinkToStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pCertContext, [In] uint dwAddDisposition, [In] [Out] SafeCertContextHandle ppStoreContext);

		// Token: 0x060018BF RID: 6335
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertDeleteCertificateFromStore([In] SafeCertContextHandle pCertContext);

		// Token: 0x060018C0 RID: 6336
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern IntPtr CertEnumCertificatesInStore([In] SafeCertStoreHandle hCertStore, [In] IntPtr pPrevCertContext);

		// Token: 0x060018C1 RID: 6337
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern SafeCertContextHandle CertEnumCertificatesInStore([In] SafeCertStoreHandle hCertStore, [In] SafeCertContextHandle pPrevCertContext);

		// Token: 0x060018C2 RID: 6338
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern SafeCertContextHandle CertFindCertificateInStore([In] SafeCertStoreHandle hCertStore, [In] uint dwCertEncodingType, [In] uint dwFindFlags, [In] uint dwFindType, [In] IntPtr pvFindPara, [In] SafeCertContextHandle pPrevCertContext);

		// Token: 0x060018C3 RID: 6339
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		protected internal static extern SafeCertStoreHandle CertOpenStore([In] IntPtr lpszStoreProvider, [In] uint dwMsgAndCertEncodingType, [In] IntPtr hCryptProv, [In] uint dwFlags, [In] string pvPara);

		// Token: 0x060018C4 RID: 6340
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertSaveStore([In] SafeCertStoreHandle hCertStore, [In] uint dwMsgAndCertEncodingType, [In] uint dwSaveAs, [In] uint dwSaveTo, [In] [Out] IntPtr pvSaveToPara, [In] uint dwFlags);

		// Token: 0x060018C5 RID: 6341
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertSetCertificateContextProperty([In] IntPtr pCertContext, [In] uint dwPropId, [In] uint dwFlags, [In] IntPtr pvData);

		// Token: 0x060018C6 RID: 6342
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertSetCertificateContextProperty([In] SafeCertContextHandle pCertContext, [In] uint dwPropId, [In] uint dwFlags, [In] IntPtr pvData);

		// Token: 0x060018C7 RID: 6343
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertSetCertificateContextProperty([In] SafeCertContextHandle pCertContext, [In] uint dwPropId, [In] uint dwFlags, [In] SafeLocalAllocHandle safeLocalAllocHandle);

		// Token: 0x060018C8 RID: 6344
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern SafeCertContextHandle CertCreateSelfSignCertificate([In] SafeCryptProvHandle hProv, [In] IntPtr pSubjectIssuerBlob, [In] uint dwFlags, [In] IntPtr pKeyProvInfo, [In] IntPtr pSignatureAlgorithm, [In] IntPtr pStartTime, [In] IntPtr pEndTime, [In] IntPtr pExtensions);

		// Token: 0x060018C9 RID: 6345
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CryptMsgControl([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara);

		// Token: 0x060018CA RID: 6346
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CryptMsgCountersign([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwIndex, [In] uint cCountersigners, [In] IntPtr rgCountersigners);

		// Token: 0x060018CB RID: 6347
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern SafeCryptMsgHandle CryptMsgOpenToEncode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr pvMsgEncodeInfo, [In] IntPtr pszInnerContentObjID, [In] IntPtr pStreamInfo);

		// Token: 0x060018CC RID: 6348
		[DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern SafeCryptMsgHandle CryptMsgOpenToEncode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr pvMsgEncodeInfo, [MarshalAs(UnmanagedType.LPStr)] [In] string pszInnerContentObjID, [In] IntPtr pStreamInfo);

		// Token: 0x060018CD RID: 6349
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CryptQueryObject([In] uint dwObjectType, [In] IntPtr pvObject, [In] uint dwExpectedContentTypeFlags, [In] uint dwExpectedFormatTypeFlags, [In] uint dwFlags, [Out] IntPtr pdwMsgAndCertEncodingType, [Out] IntPtr pdwContentType, [Out] IntPtr pdwFormatType, [In] [Out] IntPtr phCertStore, [In] [Out] IntPtr phMsg, [In] [Out] IntPtr ppvContext);

		// Token: 0x060018CE RID: 6350
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CryptQueryObject([In] uint dwObjectType, [In] IntPtr pvObject, [In] uint dwExpectedContentTypeFlags, [In] uint dwExpectedFormatTypeFlags, [In] uint dwFlags, [Out] IntPtr pdwMsgAndCertEncodingType, [Out] IntPtr pdwContentType, [Out] IntPtr pdwFormatType, [In] [Out] ref SafeCertStoreHandle phCertStore, [In] [Out] IntPtr phMsg, [In] [Out] IntPtr ppvContext);

		// Token: 0x060018CF RID: 6351
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptProtectData([In] IntPtr pDataIn, [In] string szDataDescr, [In] IntPtr pOptionalEntropy, [In] IntPtr pvReserved, [In] IntPtr pPromptStruct, [In] uint dwFlags, [In] [Out] IntPtr pDataBlob);

		// Token: 0x060018D0 RID: 6352
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptUnprotectData([In] IntPtr pDataIn, [In] IntPtr ppszDataDescr, [In] IntPtr pOptionalEntropy, [In] IntPtr pvReserved, [In] IntPtr pPromptStruct, [In] uint dwFlags, [In] [Out] IntPtr pDataBlob);

		// Token: 0x060018D1 RID: 6353
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		protected internal static extern bool PFXExportCertStore([In] SafeCertStoreHandle hStore, [In] [Out] IntPtr pPFX, [In] string szPassword, [In] uint dwFlags);

		// Token: 0x060018D2 RID: 6354
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		protected internal static extern SafeCertStoreHandle PFXImportCertStore([In] IntPtr pPFX, [In] string szPassword, [In] uint dwFlags);
	}
}

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.Security.Cryptography
{
	// Token: 0x02000314 RID: 788
	[SuppressUnmanagedCodeSecurity]
	internal abstract class CAPISafe : CAPINative
	{
		// Token: 0x0600188B RID: 6283
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint FormatMessage([In] uint dwFlags, [In] IntPtr lpSource, [In] uint dwMessageId, [In] uint dwLanguageId, [In] [Out] StringBuilder lpBuffer, [In] uint nSize, [In] IntPtr Arguments);

		// Token: 0x0600188C RID: 6284
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool FreeLibrary([In] IntPtr hModule);

		// Token: 0x0600188D RID: 6285
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetProcAddress([In] IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] [In] string lpProcName);

		// Token: 0x0600188E RID: 6286
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint GetUserDefaultLCID();

		// Token: 0x0600188F RID: 6287
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeLocalAllocHandle LocalAlloc([In] uint uFlags, [In] IntPtr sizetdwBytes);

		// Token: 0x06001890 RID: 6288
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "LoadLibraryA", SetLastError = true)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] [In] string lpFileName);

		// Token: 0x06001891 RID: 6289
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertControlStore([In] SafeCertStoreHandle hCertStore, [In] uint dwFlags, [In] uint dwCtrlType, [In] IntPtr pvCtrlPara);

		// Token: 0x06001892 RID: 6290
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertContextHandle CertCreateCertificateContext([In] uint dwCertEncodingType, [In] SafeLocalAllocHandle pbCertEncoded, [In] uint cbCertEncoded);

		// Token: 0x06001893 RID: 6291
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertContextHandle CertDuplicateCertificateContext([In] IntPtr pCertContext);

		// Token: 0x06001894 RID: 6292
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertContextHandle CertDuplicateCertificateContext([In] SafeCertContextHandle pCertContext);

		// Token: 0x06001895 RID: 6293
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertChainHandle CertDuplicateCertificateChain([In] IntPtr pChainContext);

		// Token: 0x06001896 RID: 6294
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertChainHandle CertDuplicateCertificateChain([In] SafeCertChainHandle pChainContext);

		// Token: 0x06001897 RID: 6295
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertStoreHandle CertDuplicateStore([In] IntPtr hCertStore);

		// Token: 0x06001898 RID: 6296
		[DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CertFindExtension([MarshalAs(UnmanagedType.LPStr)] [In] string pszObjId, [In] uint cExtensions, [In] IntPtr rgExtensions);

		// Token: 0x06001899 RID: 6297
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertFreeCertificateContext([In] IntPtr pCertContext);

		// Token: 0x0600189A RID: 6298
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertGetCertificateChain([In] IntPtr hChainEngine, [In] SafeCertContextHandle pCertContext, [In] ref global::System.Runtime.InteropServices.ComTypes.FILETIME pTime, [In] SafeCertStoreHandle hAdditionalStore, [In] ref CAPIBase.CERT_CHAIN_PARA pChainPara, [In] uint dwFlags, [In] IntPtr pvReserved, [In] [Out] ref SafeCertChainHandle ppChainContext);

		// Token: 0x0600189B RID: 6299
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertGetCertificateContextProperty([In] SafeCertContextHandle pCertContext, [In] uint dwPropId, [In] [Out] SafeLocalAllocHandle pvData, [In] [Out] ref uint pcbData);

		// Token: 0x0600189C RID: 6300
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertGetIntendedKeyUsage([In] uint dwCertEncodingType, [In] IntPtr pCertInfo, [In] IntPtr pbKeyUsage, [In] [Out] uint cbKeyUsage);

		// Token: 0x0600189D RID: 6301
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint CertGetNameStringW([In] SafeCertContextHandle pCertContext, [In] uint dwType, [In] uint dwFlags, [In] IntPtr pvTypePara, [In] [Out] SafeLocalAllocHandle pszNameString, [In] uint cchNameString);

		// Token: 0x0600189E RID: 6302
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint CertGetPublicKeyLength([In] uint dwCertEncodingType, [In] IntPtr pPublicKey);

		// Token: 0x0600189F RID: 6303
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertGetValidUsages([In] uint cCerts, [In] IntPtr rghCerts, [Out] IntPtr cNumOIDs, [In] [Out] SafeLocalAllocHandle rghOIDs, [In] [Out] IntPtr pcbOIDs);

		// Token: 0x060018A0 RID: 6304
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint CertNameToStrW([In] uint dwCertEncodingType, [In] IntPtr pName, [In] uint dwStrType, [In] [Out] SafeLocalAllocHandle psz, [In] uint csz);

		// Token: 0x060018A1 RID: 6305
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertSerializeCertificateStoreElement([In] SafeCertContextHandle pCertContext, [In] uint dwFlags, [In] [Out] SafeLocalAllocHandle pbElement, [In] [Out] IntPtr pcbElement);

		// Token: 0x060018A2 RID: 6306
		[DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertStrToNameW([In] uint dwCertEncodingType, [In] string pszX500, [In] uint dwStrType, [In] IntPtr pvReserved, [In] [Out] IntPtr pbEncoded, [In] [Out] ref uint pcbEncoded, [In] [Out] IntPtr ppszError);

		// Token: 0x060018A3 RID: 6307
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertVerifyTimeValidity([In] [Out] ref global::System.Runtime.InteropServices.ComTypes.FILETIME pTimeToVerify, [In] IntPtr pCertInfo);

		// Token: 0x060018A4 RID: 6308
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertVerifyCertificateChainPolicy([In] IntPtr pszPolicyOID, [In] SafeCertChainHandle pChainContext, [In] ref CAPIBase.CERT_CHAIN_POLICY_PARA pPolicyPara, [In] [Out] ref CAPIBase.CERT_CHAIN_POLICY_STATUS pPolicyStatus);

		// Token: 0x060018A5 RID: 6309
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptAcquireCertificatePrivateKey([In] SafeCertContextHandle pCert, [In] uint dwFlags, [In] IntPtr pvReserved, [In] [Out] ref SafeCryptProvHandle phCryptProv, [In] [Out] ref uint pdwKeySpec, [In] [Out] ref bool pfCallerFreeProv);

		// Token: 0x060018A6 RID: 6310
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptDecodeObject([In] uint dwCertEncodingType, [In] IntPtr lpszStructType, [In] IntPtr pbEncoded, [In] uint cbEncoded, [In] uint dwFlags, [In] [Out] SafeLocalAllocHandle pvStructInfo, [In] [Out] IntPtr pcbStructInfo);

		// Token: 0x060018A7 RID: 6311
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptDecodeObject([In] uint dwCertEncodingType, [In] IntPtr lpszStructType, [In] byte[] pbEncoded, [In] uint cbEncoded, [In] uint dwFlags, [In] [Out] SafeLocalAllocHandle pvStructInfo, [In] [Out] IntPtr pcbStructInfo);

		// Token: 0x060018A8 RID: 6312
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptEncodeObject([In] uint dwCertEncodingType, [In] IntPtr lpszStructType, [In] IntPtr pvStructInfo, [In] [Out] SafeLocalAllocHandle pbEncoded, [In] [Out] IntPtr pcbEncoded);

		// Token: 0x060018A9 RID: 6313
		[DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptEncodeObject([In] uint dwCertEncodingType, [MarshalAs(UnmanagedType.LPStr)] [In] string lpszStructType, [In] IntPtr pvStructInfo, [In] [Out] SafeLocalAllocHandle pbEncoded, [In] [Out] IntPtr pcbEncoded);

		// Token: 0x060018AA RID: 6314
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CryptFindOIDInfo([In] uint dwKeyType, [In] IntPtr pvKey, [In] OidGroup dwGroupId);

		// Token: 0x060018AB RID: 6315
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CryptFindOIDInfo([In] uint dwKeyType, [In] SafeLocalAllocHandle pvKey, [In] OidGroup dwGroupId);

		// Token: 0x060018AC RID: 6316
		[DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptFormatObject([In] uint dwCertEncodingType, [In] uint dwFormatType, [In] uint dwFormatStrType, [In] IntPtr pFormatStruct, [MarshalAs(UnmanagedType.LPStr)] [In] string lpszStructType, [In] byte[] pbEncoded, [In] uint cbEncoded, [In] [Out] SafeLocalAllocHandle pbFormat, [In] [Out] IntPtr pcbFormat);

		// Token: 0x060018AD RID: 6317
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptFormatObject([In] uint dwCertEncodingType, [In] uint dwFormatType, [In] uint dwFormatStrType, [In] IntPtr pFormatStruct, [In] IntPtr lpszStructType, [In] byte[] pbEncoded, [In] uint cbEncoded, [In] [Out] SafeLocalAllocHandle pbFormat, [In] [Out] IntPtr pcbFormat);

		// Token: 0x060018AE RID: 6318
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptGetProvParam([In] SafeCryptProvHandle hProv, [In] uint dwParam, [In] IntPtr pbData, [In] IntPtr pdwDataLen, [In] uint dwFlags);

		// Token: 0x060018AF RID: 6319
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptHashCertificate([In] IntPtr hCryptProv, [In] uint Algid, [In] uint dwFlags, [In] IntPtr pbEncoded, [In] uint cbEncoded, [Out] IntPtr pbComputedHash, [In] [Out] IntPtr pcbComputedHash);

		// Token: 0x060018B0 RID: 6320
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptHashPublicKeyInfo([In] IntPtr hCryptProv, [In] uint Algid, [In] uint dwFlags, [In] uint dwCertEncodingType, [In] IntPtr pInfo, [Out] IntPtr pbComputedHash, [In] [Out] IntPtr pcbComputedHash);

		// Token: 0x060018B1 RID: 6321
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgGetParam([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwParamType, [In] uint dwIndex, [In] [Out] IntPtr pvData, [In] [Out] IntPtr pcbData);

		// Token: 0x060018B2 RID: 6322
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgGetParam([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwParamType, [In] uint dwIndex, [In] [Out] SafeLocalAllocHandle pvData, [In] [Out] IntPtr pcbData);

		// Token: 0x060018B3 RID: 6323
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCryptMsgHandle CryptMsgOpenToDecode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr hCryptProv, [In] IntPtr pRecipientInfo, [In] IntPtr pStreamInfo);

		// Token: 0x060018B4 RID: 6324
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgUpdate([In] SafeCryptMsgHandle hCryptMsg, [In] byte[] pbData, [In] uint cbData, [In] bool fFinal);

		// Token: 0x060018B5 RID: 6325
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgUpdate([In] SafeCryptMsgHandle hCryptMsg, [In] IntPtr pbData, [In] uint cbData, [In] bool fFinal);

		// Token: 0x060018B6 RID: 6326
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgVerifyCountersignatureEncoded([In] IntPtr hCryptProv, [In] uint dwEncodingType, [In] IntPtr pbSignerInfo, [In] uint cbSignerInfo, [In] IntPtr pbSignerInfoCountersignature, [In] uint cbSignerInfoCountersignature, [In] IntPtr pciCountersigner);

		// Token: 0x060018B7 RID: 6327
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void SetLastError(uint dwErrorCode);

		// Token: 0x060018B8 RID: 6328
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x060018B9 RID: 6329
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void ZeroMemory(IntPtr handle, uint length);

		// Token: 0x060018BA RID: 6330
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern int LsaNtStatusToWinError([In] int status);
	}
}

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Security.Cryptography
{
	// Token: 0x0200004B RID: 75
	[SuppressUnmanagedCodeSecurity]
	internal abstract class CAPISafe : CAPINative
	{
		// Token: 0x06000036 RID: 54
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool FreeLibrary([In] IntPtr hModule);

		// Token: 0x06000037 RID: 55
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetProcAddress([In] IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] [In] string lpProcName);

		// Token: 0x06000038 RID: 56
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeLocalAllocHandle LocalAlloc([In] uint uFlags, [In] IntPtr sizetdwBytes);

		// Token: 0x06000039 RID: 57
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "LoadLibraryA", SetLastError = true)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] [In] string lpFileName);

		// Token: 0x0600003A RID: 58
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertContextHandle CertCreateCertificateContext([In] uint dwCertEncodingType, [In] SafeLocalAllocHandle pbCertEncoded, [In] uint cbCertEncoded);

		// Token: 0x0600003B RID: 59
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCertContextHandle CertDuplicateCertificateContext([In] IntPtr pCertContext);

		// Token: 0x0600003C RID: 60
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected internal static extern bool CertFreeCertificateContext([In] IntPtr pCertContext);

		// Token: 0x0600003D RID: 61
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertGetCertificateChain([In] IntPtr hChainEngine, [In] SafeCertContextHandle pCertContext, [In] ref global::System.Runtime.InteropServices.ComTypes.FILETIME pTime, [In] SafeCertStoreHandle hAdditionalStore, [In] ref CAPIBase.CERT_CHAIN_PARA pChainPara, [In] uint dwFlags, [In] IntPtr pvReserved, [In] [Out] ref SafeCertChainHandle ppChainContext);

		// Token: 0x0600003E RID: 62
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertGetCertificateContextProperty([In] SafeCertContextHandle pCertContext, [In] uint dwPropId, [In] [Out] SafeLocalAllocHandle pvData, [In] [Out] ref uint pcbData);

		// Token: 0x0600003F RID: 63
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint CertGetPublicKeyLength([In] uint dwCertEncodingType, [In] IntPtr pPublicKey);

		// Token: 0x06000040 RID: 64
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint CertNameToStrW([In] uint dwCertEncodingType, [In] IntPtr pName, [In] uint dwStrType, [In] [Out] SafeLocalAllocHandle psz, [In] uint csz);

		// Token: 0x06000041 RID: 65
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CertVerifyCertificateChainPolicy([In] IntPtr pszPolicyOID, [In] SafeCertChainHandle pChainContext, [In] ref CAPIBase.CERT_CHAIN_POLICY_PARA pPolicyPara, [In] [Out] ref CAPIBase.CERT_CHAIN_POLICY_STATUS pPolicyStatus);

		// Token: 0x06000042 RID: 66
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptAcquireCertificatePrivateKey([In] SafeCertContextHandle pCert, [In] uint dwFlags, [In] IntPtr pvReserved, [In] [Out] ref SafeCryptProvHandle phCryptProv, [In] [Out] ref uint pdwKeySpec, [In] [Out] ref bool pfCallerFreeProv);

		// Token: 0x06000043 RID: 67
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptDecodeObject([In] uint dwCertEncodingType, [In] IntPtr lpszStructType, [In] IntPtr pbEncoded, [In] uint cbEncoded, [In] uint dwFlags, [In] [Out] SafeLocalAllocHandle pvStructInfo, [In] [Out] IntPtr pcbStructInfo);

		// Token: 0x06000044 RID: 68
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptDecodeObject([In] uint dwCertEncodingType, [In] IntPtr lpszStructType, [In] byte[] pbEncoded, [In] uint cbEncoded, [In] uint dwFlags, [In] [Out] SafeLocalAllocHandle pvStructInfo, [In] [Out] IntPtr pcbStructInfo);

		// Token: 0x06000045 RID: 69
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptEncodeObject([In] uint dwCertEncodingType, [In] IntPtr lpszStructType, [In] IntPtr pvStructInfo, [In] [Out] SafeLocalAllocHandle pbEncoded, [In] [Out] IntPtr pcbEncoded);

		// Token: 0x06000046 RID: 70
		[DllImport("crypt32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptEncodeObject([In] uint dwCertEncodingType, [MarshalAs(UnmanagedType.LPStr)] [In] string lpszStructType, [In] IntPtr pvStructInfo, [In] [Out] SafeLocalAllocHandle pbEncoded, [In] [Out] IntPtr pcbEncoded);

		// Token: 0x06000047 RID: 71
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CryptFindOIDInfo([In] uint dwKeyType, [In] IntPtr pvKey, [In] uint dwGroupId);

		// Token: 0x06000048 RID: 72
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CryptFindOIDInfo([In] uint dwKeyType, [In] SafeLocalAllocHandle pvKey, [In] uint dwGroupId);

		// Token: 0x06000049 RID: 73
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptGetProvParam([In] SafeCryptProvHandle hProv, [In] uint dwParam, [In] IntPtr pbData, [In] IntPtr pdwDataLen, [In] uint dwFlags);

		// Token: 0x0600004A RID: 74
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgGetParam([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwParamType, [In] uint dwIndex, [In] [Out] IntPtr pvData, [In] [Out] IntPtr pcbData);

		// Token: 0x0600004B RID: 75
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgGetParam([In] SafeCryptMsgHandle hCryptMsg, [In] uint dwParamType, [In] uint dwIndex, [In] [Out] SafeLocalAllocHandle pvData, [In] [Out] IntPtr pcbData);

		// Token: 0x0600004C RID: 76
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeCryptMsgHandle CryptMsgOpenToDecode([In] uint dwMsgEncodingType, [In] uint dwFlags, [In] uint dwMsgType, [In] IntPtr hCryptProv, [In] IntPtr pRecipientInfo, [In] IntPtr pStreamInfo);

		// Token: 0x0600004D RID: 77
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgUpdate([In] SafeCryptMsgHandle hCryptMsg, [In] byte[] pbData, [In] uint cbData, [In] bool fFinal);

		// Token: 0x0600004E RID: 78
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgUpdate([In] SafeCryptMsgHandle hCryptMsg, [In] IntPtr pbData, [In] uint cbData, [In] bool fFinal);

		// Token: 0x0600004F RID: 79
		[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CryptMsgVerifyCountersignatureEncoded([In] IntPtr hCryptProv, [In] uint dwEncodingType, [In] IntPtr pbSignerInfo, [In] uint cbSignerInfo, [In] IntPtr pbSignerInfoCountersignature, [In] uint cbSignerInfoCountersignature, [In] IntPtr pciCountersigner);

		// Token: 0x06000050 RID: 80
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x06000051 RID: 81
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void ZeroMemory(IntPtr handle, uint length);

		// Token: 0x06000052 RID: 82
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern int LsaNtStatusToWinError([In] int status);
	}
}

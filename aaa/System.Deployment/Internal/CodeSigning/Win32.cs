using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x020001DD RID: 477
	internal static class Win32
	{
		// Token: 0x06000872 RID: 2162
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetProcessHeap();

		// Token: 0x06000873 RID: 2163
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool HeapFree([In] IntPtr hHeap, [In] uint dwFlags, [In] IntPtr lpMem);

		// Token: 0x06000874 RID: 2164
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertTimestampAuthenticodeLicense([In] ref Win32.CRYPT_DATA_BLOB pSignedLicenseBlob, [In] string pwszTimestampURI, [In] [Out] ref Win32.CRYPT_DATA_BLOB pTimestampSignatureBlob);

		// Token: 0x06000875 RID: 2165
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertVerifyAuthenticodeLicense([In] ref Win32.CRYPT_DATA_BLOB pLicenseBlob, [In] uint dwFlags, [In] [Out] ref Win32.AXL_SIGNER_INFO pSignerInfo, [In] [Out] ref Win32.AXL_TIMESTAMPER_INFO pTimestamperInfo);

		// Token: 0x06000876 RID: 2166
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertFreeAuthenticodeSignerInfo([In] ref Win32.AXL_SIGNER_INFO pSignerInfo);

		// Token: 0x06000877 RID: 2167
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertFreeAuthenticodeTimestamperInfo([In] ref Win32.AXL_TIMESTAMPER_INFO pTimestamperInfo);

		// Token: 0x06000878 RID: 2168
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlGetIssuerPublicKeyHash([In] IntPtr pCertContext, [In] [Out] ref IntPtr ppwszPublicKeyHash);

		// Token: 0x06000879 RID: 2169
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlRSAKeyValueToPublicKeyToken([In] ref Win32.CRYPT_DATA_BLOB pModulusBlob, [In] ref Win32.CRYPT_DATA_BLOB pExponentBlob, [In] [Out] ref IntPtr ppwszPublicKeyToken);

		// Token: 0x0600087A RID: 2170
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlPublicKeyBlobToPublicKeyToken([In] ref Win32.CRYPT_DATA_BLOB pCspPublicKeyBlob, [In] [Out] ref IntPtr ppwszPublicKeyToken);

		// Token: 0x040007E7 RID: 2023
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x040007E8 RID: 2024
		internal const string MSCORWKS = "mscorwks.dll";

		// Token: 0x040007E9 RID: 2025
		internal const int S_OK = 0;

		// Token: 0x040007EA RID: 2026
		internal const int NTE_BAD_KEY = -2146893821;

		// Token: 0x040007EB RID: 2027
		internal const int TRUST_E_SYSTEM_ERROR = -2146869247;

		// Token: 0x040007EC RID: 2028
		internal const int TRUST_E_NO_SIGNER_CERT = -2146869246;

		// Token: 0x040007ED RID: 2029
		internal const int TRUST_E_COUNTER_SIGNER = -2146869245;

		// Token: 0x040007EE RID: 2030
		internal const int TRUST_E_CERT_SIGNATURE = -2146869244;

		// Token: 0x040007EF RID: 2031
		internal const int TRUST_E_TIME_STAMP = -2146869243;

		// Token: 0x040007F0 RID: 2032
		internal const int TRUST_E_BAD_DIGEST = -2146869232;

		// Token: 0x040007F1 RID: 2033
		internal const int TRUST_E_BASIC_CONSTRAINTS = -2146869223;

		// Token: 0x040007F2 RID: 2034
		internal const int TRUST_E_FINANCIAL_CRITERIA = -2146869218;

		// Token: 0x040007F3 RID: 2035
		internal const int TRUST_E_PROVIDER_UNKNOWN = -2146762751;

		// Token: 0x040007F4 RID: 2036
		internal const int TRUST_E_ACTION_UNKNOWN = -2146762750;

		// Token: 0x040007F5 RID: 2037
		internal const int TRUST_E_SUBJECT_FORM_UNKNOWN = -2146762749;

		// Token: 0x040007F6 RID: 2038
		internal const int TRUST_E_SUBJECT_NOT_TRUSTED = -2146762748;

		// Token: 0x040007F7 RID: 2039
		internal const int TRUST_E_NOSIGNATURE = -2146762496;

		// Token: 0x040007F8 RID: 2040
		internal const int CERT_E_UNTRUSTEDROOT = -2146762487;

		// Token: 0x040007F9 RID: 2041
		internal const int TRUST_E_FAIL = -2146762485;

		// Token: 0x040007FA RID: 2042
		internal const int TRUST_E_EXPLICIT_DISTRUST = -2146762479;

		// Token: 0x040007FB RID: 2043
		internal const int CERT_E_CHAINING = -2146762486;

		// Token: 0x040007FC RID: 2044
		internal const int AXL_REVOCATION_NO_CHECK = 1;

		// Token: 0x040007FD RID: 2045
		internal const int AXL_REVOCATION_CHECK_END_CERT_ONLY = 2;

		// Token: 0x040007FE RID: 2046
		internal const int AXL_REVOCATION_CHECK_ENTIRE_CHAIN = 4;

		// Token: 0x040007FF RID: 2047
		internal const int AXL_URL_CACHE_ONLY_RETRIEVAL = 8;

		// Token: 0x04000800 RID: 2048
		internal const int AXL_LIFETIME_SIGNING = 16;

		// Token: 0x04000801 RID: 2049
		internal const int AXL_TRUST_MICROSOFT_ROOT_ONLY = 32;

		// Token: 0x020001DE RID: 478
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_DATA_BLOB
		{
			// Token: 0x04000802 RID: 2050
			internal uint cbData;

			// Token: 0x04000803 RID: 2051
			internal IntPtr pbData;
		}

		// Token: 0x020001DF RID: 479
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct AXL_SIGNER_INFO
		{
			// Token: 0x04000804 RID: 2052
			internal uint cbSize;

			// Token: 0x04000805 RID: 2053
			internal uint dwError;

			// Token: 0x04000806 RID: 2054
			internal uint algHash;

			// Token: 0x04000807 RID: 2055
			internal IntPtr pwszHash;

			// Token: 0x04000808 RID: 2056
			internal IntPtr pwszDescription;

			// Token: 0x04000809 RID: 2057
			internal IntPtr pwszDescriptionUrl;

			// Token: 0x0400080A RID: 2058
			internal IntPtr pChainContext;
		}

		// Token: 0x020001E0 RID: 480
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct AXL_TIMESTAMPER_INFO
		{
			// Token: 0x0400080B RID: 2059
			internal uint cbSize;

			// Token: 0x0400080C RID: 2060
			internal uint dwError;

			// Token: 0x0400080D RID: 2061
			internal uint algHash;

			// Token: 0x0400080E RID: 2062
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME ftTimestamp;

			// Token: 0x0400080F RID: 2063
			internal IntPtr pChainContext;
		}
	}
}

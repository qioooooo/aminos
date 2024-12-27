using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000135 RID: 309
	internal static class Win32
	{
		// Token: 0x06000474 RID: 1140
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetProcessHeap();

		// Token: 0x06000475 RID: 1141
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool HeapFree([In] IntPtr hHeap, [In] uint dwFlags, [In] IntPtr lpMem);

		// Token: 0x06000476 RID: 1142
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertTimestampAuthenticodeLicense([In] ref Win32.CRYPT_DATA_BLOB pSignedLicenseBlob, [In] string pwszTimestampURI, [In] [Out] ref Win32.CRYPT_DATA_BLOB pTimestampSignatureBlob);

		// Token: 0x06000477 RID: 1143
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertVerifyAuthenticodeLicense([In] ref Win32.CRYPT_DATA_BLOB pLicenseBlob, [In] uint dwFlags, [In] [Out] ref Win32.AXL_SIGNER_INFO pSignerInfo, [In] [Out] ref Win32.AXL_TIMESTAMPER_INFO pTimestamperInfo);

		// Token: 0x06000478 RID: 1144
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertFreeAuthenticodeSignerInfo([In] ref Win32.AXL_SIGNER_INFO pSignerInfo);

		// Token: 0x06000479 RID: 1145
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int CertFreeAuthenticodeTimestamperInfo([In] ref Win32.AXL_TIMESTAMPER_INFO pTimestamperInfo);

		// Token: 0x0600047A RID: 1146
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlGetIssuerPublicKeyHash([In] IntPtr pCertContext, [In] [Out] ref IntPtr ppwszPublicKeyHash);

		// Token: 0x0600047B RID: 1147
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlRSAKeyValueToPublicKeyToken([In] ref Win32.CRYPT_DATA_BLOB pModulusBlob, [In] ref Win32.CRYPT_DATA_BLOB pExponentBlob, [In] [Out] ref IntPtr ppwszPublicKeyToken);

		// Token: 0x0600047C RID: 1148
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int _AxlPublicKeyBlobToPublicKeyToken([In] ref Win32.CRYPT_DATA_BLOB pCspPublicKeyBlob, [In] [Out] ref IntPtr ppwszPublicKeyToken);

		// Token: 0x04000E8A RID: 3722
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x04000E8B RID: 3723
		internal const string MSCORWKS = "mscorwks.dll";

		// Token: 0x04000E8C RID: 3724
		internal const int S_OK = 0;

		// Token: 0x04000E8D RID: 3725
		internal const int NTE_BAD_KEY = -2146893821;

		// Token: 0x04000E8E RID: 3726
		internal const int TRUST_E_SYSTEM_ERROR = -2146869247;

		// Token: 0x04000E8F RID: 3727
		internal const int TRUST_E_NO_SIGNER_CERT = -2146869246;

		// Token: 0x04000E90 RID: 3728
		internal const int TRUST_E_COUNTER_SIGNER = -2146869245;

		// Token: 0x04000E91 RID: 3729
		internal const int TRUST_E_CERT_SIGNATURE = -2146869244;

		// Token: 0x04000E92 RID: 3730
		internal const int TRUST_E_TIME_STAMP = -2146869243;

		// Token: 0x04000E93 RID: 3731
		internal const int TRUST_E_BAD_DIGEST = -2146869232;

		// Token: 0x04000E94 RID: 3732
		internal const int TRUST_E_BASIC_CONSTRAINTS = -2146869223;

		// Token: 0x04000E95 RID: 3733
		internal const int TRUST_E_FINANCIAL_CRITERIA = -2146869218;

		// Token: 0x04000E96 RID: 3734
		internal const int TRUST_E_PROVIDER_UNKNOWN = -2146762751;

		// Token: 0x04000E97 RID: 3735
		internal const int TRUST_E_ACTION_UNKNOWN = -2146762750;

		// Token: 0x04000E98 RID: 3736
		internal const int TRUST_E_SUBJECT_FORM_UNKNOWN = -2146762749;

		// Token: 0x04000E99 RID: 3737
		internal const int TRUST_E_SUBJECT_NOT_TRUSTED = -2146762748;

		// Token: 0x04000E9A RID: 3738
		internal const int TRUST_E_NOSIGNATURE = -2146762496;

		// Token: 0x04000E9B RID: 3739
		internal const int CERT_E_UNTRUSTEDROOT = -2146762487;

		// Token: 0x04000E9C RID: 3740
		internal const int TRUST_E_FAIL = -2146762485;

		// Token: 0x04000E9D RID: 3741
		internal const int TRUST_E_EXPLICIT_DISTRUST = -2146762479;

		// Token: 0x04000E9E RID: 3742
		internal const int CERT_E_CHAINING = -2146762486;

		// Token: 0x04000E9F RID: 3743
		internal const int AXL_REVOCATION_NO_CHECK = 1;

		// Token: 0x04000EA0 RID: 3744
		internal const int AXL_REVOCATION_CHECK_END_CERT_ONLY = 2;

		// Token: 0x04000EA1 RID: 3745
		internal const int AXL_REVOCATION_CHECK_ENTIRE_CHAIN = 4;

		// Token: 0x04000EA2 RID: 3746
		internal const int AXL_URL_CACHE_ONLY_RETRIEVAL = 8;

		// Token: 0x04000EA3 RID: 3747
		internal const int AXL_LIFETIME_SIGNING = 16;

		// Token: 0x04000EA4 RID: 3748
		internal const int AXL_TRUST_MICROSOFT_ROOT_ONLY = 32;

		// Token: 0x02000136 RID: 310
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct CRYPT_DATA_BLOB
		{
			// Token: 0x04000EA5 RID: 3749
			internal uint cbData;

			// Token: 0x04000EA6 RID: 3750
			internal IntPtr pbData;
		}

		// Token: 0x02000137 RID: 311
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct AXL_SIGNER_INFO
		{
			// Token: 0x04000EA7 RID: 3751
			internal uint cbSize;

			// Token: 0x04000EA8 RID: 3752
			internal uint dwError;

			// Token: 0x04000EA9 RID: 3753
			internal uint algHash;

			// Token: 0x04000EAA RID: 3754
			internal IntPtr pwszHash;

			// Token: 0x04000EAB RID: 3755
			internal IntPtr pwszDescription;

			// Token: 0x04000EAC RID: 3756
			internal IntPtr pwszDescriptionUrl;

			// Token: 0x04000EAD RID: 3757
			internal IntPtr pChainContext;
		}

		// Token: 0x02000138 RID: 312
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct AXL_TIMESTAMPER_INFO
		{
			// Token: 0x04000EAE RID: 3758
			internal uint cbSize;

			// Token: 0x04000EAF RID: 3759
			internal uint dwError;

			// Token: 0x04000EB0 RID: 3760
			internal uint algHash;

			// Token: 0x04000EB1 RID: 3761
			internal global::System.Runtime.InteropServices.ComTypes.FILETIME ftTimestamp;

			// Token: 0x04000EB2 RID: 3762
			internal IntPtr pChainContext;
		}
	}
}

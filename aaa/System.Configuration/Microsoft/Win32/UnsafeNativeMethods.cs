using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Win32
{
	// Token: 0x020000CA RID: 202
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06000789 RID: 1929
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetFileAttributesEx(string name, int fileInfoLevel, out UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA data);

		// Token: 0x0600078A RID: 1930
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

		// Token: 0x0600078B RID: 1931
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptProtectData(ref DATA_BLOB inputData, string description, ref DATA_BLOB entropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT promptStruct, uint flags, ref DATA_BLOB outputData);

		// Token: 0x0600078C RID: 1932
		[DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CryptUnprotectData(ref DATA_BLOB inputData, ref string description, ref DATA_BLOB entropy, IntPtr pReserved, ref CRYPTPROTECT_PROMPTSTRUCT promptStruct, uint flags, ref DATA_BLOB outputData);

		// Token: 0x0600078D RID: 1933
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int CryptAcquireContext(out SafeCryptContextHandle phProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

		// Token: 0x0600078E RID: 1934
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int CryptReleaseContext(SafeCryptContextHandle hProv, uint dwFlags);

		// Token: 0x0600078F RID: 1935
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr LocalFree(IntPtr buf);

		// Token: 0x06000790 RID: 1936
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, int dwFlags);

		// Token: 0x04000431 RID: 1073
		internal const int GetFileExInfoStandard = 0;

		// Token: 0x04000432 RID: 1074
		internal const int MOVEFILE_REPLACE_EXISTING = 1;

		// Token: 0x020000CB RID: 203
		internal struct WIN32_FILE_ATTRIBUTE_DATA
		{
			// Token: 0x04000433 RID: 1075
			internal int fileAttributes;

			// Token: 0x04000434 RID: 1076
			internal uint ftCreationTimeLow;

			// Token: 0x04000435 RID: 1077
			internal uint ftCreationTimeHigh;

			// Token: 0x04000436 RID: 1078
			internal uint ftLastAccessTimeLow;

			// Token: 0x04000437 RID: 1079
			internal uint ftLastAccessTimeHigh;

			// Token: 0x04000438 RID: 1080
			internal uint ftLastWriteTimeLow;

			// Token: 0x04000439 RID: 1081
			internal uint ftLastWriteTimeHigh;

			// Token: 0x0400043A RID: 1082
			internal uint fileSizeHigh;

			// Token: 0x0400043B RID: 1083
			internal uint fileSizeLow;
		}
	}
}

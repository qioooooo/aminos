using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Configuration
{
	// Token: 0x0200006A RID: 106
	internal static class FileUtil
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x00013C54 File Offset: 0x00012C54
		internal static bool FileExists(string filename, bool trueOnError)
		{
			UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA;
			bool fileAttributesEx = UnsafeNativeMethods.GetFileAttributesEx(filename, 0, out win32_FILE_ATTRIBUTE_DATA);
			if (fileAttributesEx)
			{
				return (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) != 16;
			}
			if (!trueOnError)
			{
				return false;
			}
			int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
			return hrforLastWin32Error != -2147024894 && hrforLastWin32Error != -2147024893;
		}

		// Token: 0x04000314 RID: 788
		private const int HRESULT_WIN32_FILE_NOT_FOUND = -2147024894;

		// Token: 0x04000315 RID: 789
		private const int HRESULT_WIN32_PATH_NOT_FOUND = -2147024893;
	}
}

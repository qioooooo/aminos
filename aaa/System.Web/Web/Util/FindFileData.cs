using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Web.Util
{
	// Token: 0x02000769 RID: 1897
	internal sealed class FindFileData
	{
		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x06005C1C RID: 23580 RVA: 0x00171A27 File Offset: 0x00170A27
		internal string FileNameLong
		{
			get
			{
				return this._fileNameLong;
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06005C1D RID: 23581 RVA: 0x00171A2F File Offset: 0x00170A2F
		internal string FileNameShort
		{
			get
			{
				return this._fileNameShort;
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x06005C1E RID: 23582 RVA: 0x00171A37 File Offset: 0x00170A37
		internal FileAttributesData FileAttributesData
		{
			get
			{
				return this._fileAttributesData;
			}
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x00171A40 File Offset: 0x00170A40
		internal static int FindFile(string path, out FindFileData data)
		{
			data = null;
			path = FileUtil.RemoveTrailingDirectoryBackSlash(path);
			UnsafeNativeMethods.WIN32_FIND_DATA win32_FIND_DATA;
			IntPtr intPtr = UnsafeNativeMethods.FindFirstFile(path, out win32_FIND_DATA);
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (intPtr == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
			{
				return HttpException.HResultFromLastError(lastWin32Error);
			}
			UnsafeNativeMethods.FindClose(intPtr);
			data = new FindFileData(ref win32_FIND_DATA);
			return 0;
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x00171A8C File Offset: 0x00170A8C
		internal static int FindFile(string fullPath, string rootDirectoryPath, out FindFileData data)
		{
			int num = FindFileData.FindFile(fullPath, out data);
			if (num != 0 || string.IsNullOrEmpty(rootDirectoryPath))
			{
				return num;
			}
			rootDirectoryPath = FileUtil.RemoveTrailingDirectoryBackSlash(rootDirectoryPath);
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = Path.GetDirectoryName(fullPath);
			while (text3 != null && text3.Length > rootDirectoryPath.Length + 1 && text3.IndexOf(rootDirectoryPath, StringComparison.OrdinalIgnoreCase) == 0)
			{
				UnsafeNativeMethods.WIN32_FIND_DATA win32_FIND_DATA;
				IntPtr intPtr = UnsafeNativeMethods.FindFirstFile(text3, out win32_FIND_DATA);
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (intPtr == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
				{
					return HttpException.HResultFromLastError(lastWin32Error);
				}
				UnsafeNativeMethods.FindClose(intPtr);
				text = win32_FIND_DATA.cFileName + Path.DirectorySeparatorChar + text;
				if (!string.IsNullOrEmpty(win32_FIND_DATA.cAlternateFileName))
				{
					text2 = win32_FIND_DATA.cAlternateFileName + Path.DirectorySeparatorChar + text2;
				}
				else
				{
					text2 = win32_FIND_DATA.cFileName + Path.DirectorySeparatorChar + text2;
				}
				text3 = Path.GetDirectoryName(text3);
			}
			if (!string.IsNullOrEmpty(text))
			{
				data.PrependRelativePath(text, text2);
			}
			return num;
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x00171B90 File Offset: 0x00170B90
		internal FindFileData(ref UnsafeNativeMethods.WIN32_FIND_DATA wfd)
		{
			this._fileAttributesData = new FileAttributesData(ref wfd);
			this._fileNameLong = wfd.cFileName;
			if (wfd.cAlternateFileName != null && wfd.cAlternateFileName.Length > 0 && !StringUtil.EqualsIgnoreCase(wfd.cFileName, wfd.cAlternateFileName))
			{
				this._fileNameShort = wfd.cAlternateFileName;
			}
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x00171BF0 File Offset: 0x00170BF0
		private void PrependRelativePath(string relativePathLong, string relativePathShort)
		{
			this._fileNameLong = relativePathLong + this._fileNameLong;
			string text = (string.IsNullOrEmpty(this._fileNameShort) ? this._fileNameLong : this._fileNameShort);
			this._fileNameShort = relativePathShort + text;
			if (StringUtil.EqualsIgnoreCase(this._fileNameShort, this._fileNameLong))
			{
				this._fileNameShort = null;
			}
		}

		// Token: 0x04003140 RID: 12608
		private FileAttributesData _fileAttributesData;

		// Token: 0x04003141 RID: 12609
		private string _fileNameLong;

		// Token: 0x04003142 RID: 12610
		private string _fileNameShort;
	}
}

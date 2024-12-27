using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Web.Util
{
	// Token: 0x0200076A RID: 1898
	internal sealed class FileAttributesData
	{
		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x06005C23 RID: 23587 RVA: 0x00171C52 File Offset: 0x00170C52
		internal static FileAttributesData NonExistantAttributesData
		{
			get
			{
				return new FileAttributesData();
			}
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x00171C5C File Offset: 0x00170C5C
		internal static int GetFileAttributes(string path, out FileAttributesData fad)
		{
			fad = null;
			UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA;
			if (!UnsafeNativeMethods.GetFileAttributesEx(path, 0, out win32_FILE_ATTRIBUTE_DATA))
			{
				return HttpException.HResultFromLastError(Marshal.GetLastWin32Error());
			}
			fad = new FileAttributesData(ref win32_FILE_ATTRIBUTE_DATA);
			return 0;
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x00171C8C File Offset: 0x00170C8C
		private FileAttributesData()
		{
			this.FileSize = -1L;
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x00171C9C File Offset: 0x00170C9C
		private FileAttributesData(ref UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA data)
		{
			this.FileAttributes = (FileAttributes)data.fileAttributes;
			this.UtcCreationTime = DateTimeUtil.FromFileTimeToUtc((long)(((ulong)data.ftCreationTimeHigh << 32) | (ulong)data.ftCreationTimeLow));
			this.UtcLastAccessTime = DateTimeUtil.FromFileTimeToUtc((long)(((ulong)data.ftLastAccessTimeHigh << 32) | (ulong)data.ftLastAccessTimeLow));
			this.UtcLastWriteTime = DateTimeUtil.FromFileTimeToUtc((long)(((ulong)data.ftLastWriteTimeHigh << 32) | (ulong)data.ftLastWriteTimeLow));
			this.FileSize = (long)(((ulong)data.fileSizeHigh << 32) | (ulong)data.fileSizeLow);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x00171D2C File Offset: 0x00170D2C
		internal FileAttributesData(ref UnsafeNativeMethods.WIN32_FIND_DATA wfd)
		{
			this.FileAttributes = (FileAttributes)wfd.dwFileAttributes;
			this.UtcCreationTime = DateTimeUtil.FromFileTimeToUtc((long)(((ulong)wfd.ftCreationTime_dwHighDateTime << 32) | (ulong)wfd.ftCreationTime_dwLowDateTime));
			this.UtcLastAccessTime = DateTimeUtil.FromFileTimeToUtc((long)(((ulong)wfd.ftLastAccessTime_dwHighDateTime << 32) | (ulong)wfd.ftLastAccessTime_dwLowDateTime));
			this.UtcLastWriteTime = DateTimeUtil.FromFileTimeToUtc((long)(((ulong)wfd.ftLastWriteTime_dwHighDateTime << 32) | (ulong)wfd.ftLastWriteTime_dwLowDateTime));
			this.FileSize = (long)(((ulong)wfd.nFileSizeHigh << 32) | (ulong)wfd.nFileSizeLow);
		}

		// Token: 0x04003143 RID: 12611
		internal readonly FileAttributes FileAttributes;

		// Token: 0x04003144 RID: 12612
		internal readonly DateTime UtcCreationTime;

		// Token: 0x04003145 RID: 12613
		internal readonly DateTime UtcLastAccessTime;

		// Token: 0x04003146 RID: 12614
		internal readonly DateTime UtcLastWriteTime;

		// Token: 0x04003147 RID: 12615
		internal readonly long FileSize;
	}
}

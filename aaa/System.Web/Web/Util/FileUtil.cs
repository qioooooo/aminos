using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.Util
{
	// Token: 0x02000768 RID: 1896
	internal class FileUtil
	{
		// Token: 0x06005C0D RID: 23565 RVA: 0x00171667 File Offset: 0x00170667
		private FileUtil()
		{
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x00171670 File Offset: 0x00170670
		internal static bool FileExists(string filename)
		{
			bool flag = false;
			try
			{
				flag = File.Exists(filename);
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x0017169C File Offset: 0x0017069C
		internal static string RemoveTrailingDirectoryBackSlash(string path)
		{
			if (path == null)
			{
				return null;
			}
			int length = path.Length;
			if (length > 3 && path[length - 1] == '\\')
			{
				path = path.Substring(0, length - 1);
			}
			return path;
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x001716D4 File Offset: 0x001706D4
		internal static string TruncatePathIfNeeded(string path, int reservedLength)
		{
			int num = FileUtil._maxPathLength - reservedLength;
			if (path.Length > num)
			{
				path = path.Substring(0, num - 13) + path.GetHashCode().ToString(CultureInfo.InvariantCulture);
			}
			return path;
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x00171718 File Offset: 0x00170718
		internal static string FixUpPhysicalDirectory(string dir)
		{
			if (dir == null)
			{
				return null;
			}
			dir = Path.GetFullPath(dir);
			if (!StringUtil.StringEndsWith(dir, "\\"))
			{
				dir += "\\";
			}
			return dir;
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x00171742 File Offset: 0x00170742
		internal static void CheckSuspiciousPhysicalPath(string physicalPath)
		{
			if (FileUtil.IsSuspiciousPhysicalPath(physicalPath))
			{
				throw new HttpException(404, string.Empty);
			}
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x0017175C File Offset: 0x0017075C
		internal static bool IsSuspiciousPhysicalPath(string physicalPath)
		{
			bool flag;
			if (!FileUtil.IsSuspiciousPhysicalPath(physicalPath, out flag))
			{
				return false;
			}
			if (!flag)
			{
				return true;
			}
			if (physicalPath.IndexOf('/') >= 0 || physicalPath.IndexOf("\\..", StringComparison.Ordinal) >= 0)
			{
				return true;
			}
			for (int i = physicalPath.LastIndexOf('\\'); i >= 0; i = physicalPath.LastIndexOf('\\', i - 1))
			{
				string text = physicalPath.Substring(0, i);
				if (!FileUtil.IsSuspiciousPhysicalPath(text, out flag))
				{
					return false;
				}
				if (!flag)
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x001717CC File Offset: 0x001707CC
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		private static bool IsSuspiciousPhysicalPath(string physicalPath, out bool pathTooLong)
		{
			bool flag;
			try
			{
				flag = !string.IsNullOrEmpty(physicalPath) && string.Compare(physicalPath, Path.GetFullPath(physicalPath), StringComparison.OrdinalIgnoreCase) != 0;
				pathTooLong = false;
			}
			catch (PathTooLongException)
			{
				flag = true;
				pathTooLong = true;
			}
			catch (ArgumentException)
			{
				flag = true;
				pathTooLong = true;
			}
			return flag;
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x0017182C File Offset: 0x0017082C
		private static bool HasInvalidLastChar(string physicalPath)
		{
			if (string.IsNullOrEmpty(physicalPath))
			{
				return false;
			}
			char c = physicalPath[physicalPath.Length - 1];
			return c == ' ' || c == '.';
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x00171860 File Offset: 0x00170860
		internal static bool DirectoryExists(string dirname)
		{
			bool flag = false;
			dirname = FileUtil.RemoveTrailingDirectoryBackSlash(dirname);
			if (FileUtil.HasInvalidLastChar(dirname))
			{
				return false;
			}
			try
			{
				flag = Directory.Exists(dirname);
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x001718A0 File Offset: 0x001708A0
		internal static bool DirectoryAccessible(string dirname)
		{
			bool flag = false;
			dirname = FileUtil.RemoveTrailingDirectoryBackSlash(dirname);
			if (FileUtil.HasInvalidLastChar(dirname))
			{
				return false;
			}
			try
			{
				flag = new DirectoryInfo(dirname).Exists;
			}
			catch
			{
			}
			return flag;
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x001718E4 File Offset: 0x001708E4
		internal static bool IsValidDirectoryName(string name)
		{
			return !string.IsNullOrEmpty(name) && name.IndexOfAny(FileUtil._invalidFileNameChars, 0) == -1 && !name.Equals(".") && !name.Equals("..");
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x00171920 File Offset: 0x00170920
		internal static void PhysicalPathStatus(string physicalPath, bool directoryExistsOnError, bool fileExistsOnError, out bool exists, out bool isDirectory)
		{
			exists = false;
			isDirectory = true;
			if (string.IsNullOrEmpty(physicalPath))
			{
				return;
			}
			using (new ApplicationImpersonationContext())
			{
				UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA;
				bool fileAttributesEx = UnsafeNativeMethods.GetFileAttributesEx(physicalPath, 0, out win32_FILE_ATTRIBUTE_DATA);
				if (fileAttributesEx)
				{
					exists = true;
					isDirectory = (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) == 16;
					if (isDirectory && FileUtil.HasInvalidLastChar(physicalPath))
					{
						exists = false;
					}
				}
				else if (directoryExistsOnError || fileExistsOnError)
				{
					int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
					if (hrforLastWin32Error != -2147024894 && hrforLastWin32Error != -2147024893)
					{
						exists = true;
						isDirectory = directoryExistsOnError;
					}
				}
			}
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x001719B8 File Offset: 0x001709B8
		internal static bool DirectoryExists(string filename, bool trueOnError)
		{
			filename = FileUtil.RemoveTrailingDirectoryBackSlash(filename);
			if (FileUtil.HasInvalidLastChar(filename))
			{
				return false;
			}
			UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA;
			bool fileAttributesEx = UnsafeNativeMethods.GetFileAttributesEx(filename, 0, out win32_FILE_ATTRIBUTE_DATA);
			if (fileAttributesEx)
			{
				return (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) == 16;
			}
			if (!trueOnError)
			{
				return false;
			}
			int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
			return hrforLastWin32Error != -2147024894 && hrforLastWin32Error != -2147024893;
		}

		// Token: 0x0400313E RID: 12606
		private static int _maxPathLength = 259;

		// Token: 0x0400313F RID: 12607
		private static char[] _invalidFileNameChars = Path.GetInvalidFileNameChars();
	}
}

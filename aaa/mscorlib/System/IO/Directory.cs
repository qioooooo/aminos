using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x02000593 RID: 1427
	[ComVisible(true)]
	public static class Directory
	{
		// Token: 0x060034E1 RID: 13537 RVA: 0x000B034C File Offset: 0x000AF34C
		public static DirectoryInfo GetParent(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathEmpty"), "path");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			string directoryName = Path.GetDirectoryName(fullPathInternal);
			if (directoryName == null)
			{
				return null;
			}
			return new DirectoryInfo(directoryName);
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x000B039D File Offset: 0x000AF39D
		public static DirectoryInfo CreateDirectory(string path)
		{
			return Directory.CreateDirectory(path, null);
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x000B03A8 File Offset: 0x000AF3A8
		public static DirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathEmpty"));
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			string demandDir = Directory.GetDemandDir(fullPathInternal, true);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { demandDir }, false, false).Demand();
			Directory.InternalCreateDirectory(fullPathInternal, path, directorySecurity);
			return new DirectoryInfo(fullPathInternal, false);
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x000B0414 File Offset: 0x000AF414
		internal static string GetDemandDir(string fullPath, bool thisDirOnly)
		{
			string text;
			if (thisDirOnly)
			{
				if (fullPath.EndsWith(Path.DirectorySeparatorChar) || fullPath.EndsWith(Path.AltDirectorySeparatorChar))
				{
					text = fullPath + '.';
				}
				else
				{
					text = fullPath + Path.DirectorySeparatorChar + '.';
				}
			}
			else if (!fullPath.EndsWith(Path.DirectorySeparatorChar) && !fullPath.EndsWith(Path.AltDirectorySeparatorChar))
			{
				text = fullPath + Path.DirectorySeparatorChar;
			}
			else
			{
				text = fullPath;
			}
			return text;
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x000B0498 File Offset: 0x000AF498
		internal unsafe static void InternalCreateDirectory(string fullPath, string path, DirectorySecurity dirSecurity)
		{
			int num = fullPath.Length;
			if (num >= 2 && Path.IsDirectorySeparator(fullPath[num - 1]))
			{
				num--;
			}
			int rootLength = Path.GetRootLength(fullPath);
			if (num == 2 && Path.IsDirectorySeparator(fullPath[1]))
			{
				throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.IO_CannotCreateDirectory"), new object[] { path }));
			}
			List<string> list = new List<string>();
			bool flag = false;
			if (num > rootLength)
			{
				for (int i = num - 1; i >= rootLength; i--)
				{
					string text = fullPath.Substring(0, i + 1);
					if (!Directory.InternalExists(text))
					{
						list.Add(text);
					}
					else
					{
						flag = true;
					}
					while (i > rootLength && fullPath[i] != Path.DirectorySeparatorChar && fullPath[i] != Path.AltDirectorySeparatorChar)
					{
						i--;
					}
				}
			}
			int count = list.Count;
			if (list.Count != 0)
			{
				string[] array = new string[list.Count];
				list.CopyTo(array, 0);
				for (int j = 0; j < array.Length; j++)
				{
					string[] array2;
					IntPtr intPtr;
					(array2 = array)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + "\\.";
				}
				AccessControlActions accessControlActions = ((dirSecurity == null) ? AccessControlActions.None : AccessControlActions.Change);
				new FileIOPermission(FileIOPermissionAccess.Write, accessControlActions, array, false, false).Demand();
			}
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			if (dirSecurity != null)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				byte[] securityDescriptorBinaryForm = dirSecurity.GetSecurityDescriptorBinaryForm();
				byte* ptr = stackalloc byte[1 * securityDescriptorBinaryForm.Length];
				Buffer.memcpy(securityDescriptorBinaryForm, 0, ptr, 0, securityDescriptorBinaryForm.Length);
				security_ATTRIBUTES.pSecurityDescriptor = ptr;
			}
			bool flag2 = true;
			int num2 = 0;
			string text2 = path;
			while (list.Count > 0)
			{
				string text3 = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				if (text3.Length > 248)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				flag2 = Win32Native.CreateDirectory(text3, security_ATTRIBUTES);
				if (!flag2 && num2 == 0)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error != 183)
					{
						num2 = lastWin32Error;
					}
					else if (File.InternalExists(text3))
					{
						num2 = lastWin32Error;
						try
						{
							new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Directory.GetDemandDir(text3, true)).Demand();
							text2 = text3;
						}
						catch (SecurityException)
						{
						}
					}
				}
			}
			if (count == 0 && !flag)
			{
				string text4 = Directory.InternalGetDirectoryRoot(fullPath);
				if (!Directory.InternalExists(text4))
				{
					__Error.WinIOError(3, Directory.InternalGetDirectoryRoot(path));
				}
				return;
			}
			if (!flag2 && num2 != 0)
			{
				__Error.WinIOError(num2, text2);
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x000B0714 File Offset: 0x000AF714
		public static bool Exists(string path)
		{
			try
			{
				if (path == null)
				{
					return false;
				}
				if (path.Length == 0)
				{
					return false;
				}
				string fullPathInternal = Path.GetFullPathInternal(path);
				string demandDir = Directory.GetDemandDir(fullPathInternal, true);
				new FileIOPermission(FileIOPermissionAccess.Read, new string[] { demandDir }, false, false).Demand();
				return Directory.InternalExists(fullPathInternal);
			}
			catch (ArgumentException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			return false;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x000B07B8 File Offset: 0x000AF7B8
		internal static bool InternalExists(string path)
		{
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			return File.FillAttributeInfo(path, ref win32_FILE_ATTRIBUTE_DATA, false, true) == 0 && win32_FILE_ATTRIBUTE_DATA.fileAttributes != -1 && (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) != 0;
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x000B07F7 File Offset: 0x000AF7F7
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			Directory.SetCreationTimeUtc(path, creationTime.ToUniversalTime());
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x000B0808 File Offset: 0x000AF808
		public unsafe static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			if ((Environment.OSInfo & Environment.OSName.WinNT) == Environment.OSName.WinNT)
			{
				using (SafeFileHandle safeFileHandle = Directory.OpenHandle(path))
				{
					Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(creationTimeUtc.ToFileTimeUtc());
					if (!Win32Native.SetFileTime(safeFileHandle, &file_TIME, null, null))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						__Error.WinIOError(lastWin32Error, path);
					}
				}
			}
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x000B087C File Offset: 0x000AF87C
		public static DateTime GetCreationTime(string path)
		{
			return File.GetCreationTime(path);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x000B0884 File Offset: 0x000AF884
		public static DateTime GetCreationTimeUtc(string path)
		{
			return File.GetCreationTimeUtc(path);
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x000B088C File Offset: 0x000AF88C
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			Directory.SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x000B089C File Offset: 0x000AF89C
		public unsafe static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			if ((Environment.OSInfo & Environment.OSName.WinNT) == Environment.OSName.WinNT)
			{
				using (SafeFileHandle safeFileHandle = Directory.OpenHandle(path))
				{
					Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(lastWriteTimeUtc.ToFileTimeUtc());
					if (!Win32Native.SetFileTime(safeFileHandle, null, null, &file_TIME))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						__Error.WinIOError(lastWin32Error, path);
					}
				}
			}
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x000B0910 File Offset: 0x000AF910
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTime(path);
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x000B0918 File Offset: 0x000AF918
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			return File.GetLastWriteTimeUtc(path);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x000B0920 File Offset: 0x000AF920
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			Directory.SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x000B0930 File Offset: 0x000AF930
		public unsafe static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			if ((Environment.OSInfo & Environment.OSName.WinNT) == Environment.OSName.WinNT)
			{
				using (SafeFileHandle safeFileHandle = Directory.OpenHandle(path))
				{
					Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(lastAccessTimeUtc.ToFileTimeUtc());
					if (!Win32Native.SetFileTime(safeFileHandle, null, &file_TIME, null))
					{
						int lastWin32Error = Marshal.GetLastWin32Error();
						__Error.WinIOError(lastWin32Error, path);
					}
				}
			}
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x000B09A4 File Offset: 0x000AF9A4
		public static DateTime GetLastAccessTime(string path)
		{
			return File.GetLastAccessTime(path);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000B09AC File Offset: 0x000AF9AC
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			return File.GetLastAccessTimeUtc(path);
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000B09B4 File Offset: 0x000AF9B4
		public static DirectorySecurity GetAccessControl(string path)
		{
			return new DirectorySecurity(path, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x000B09BE File Offset: 0x000AF9BE
		public static DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			return new DirectorySecurity(path, includeSections);
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x000B09C8 File Offset: 0x000AF9C8
		public static void SetAccessControl(string path, DirectorySecurity directorySecurity)
		{
			if (directorySecurity == null)
			{
				throw new ArgumentNullException("directorySecurity");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			directorySecurity.Persist(fullPathInternal);
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x000B09F1 File Offset: 0x000AF9F1
		public static string[] GetFiles(string path)
		{
			return Directory.GetFiles(path, "*");
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x000B09FE File Offset: 0x000AF9FE
		public static string[] GetFiles(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x000B0A08 File Offset: 0x000AFA08
		public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			return Directory.InternalGetFileDirectoryNames(path, path, searchPattern, true, false, searchOption);
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x000B0A31 File Offset: 0x000AFA31
		public static string[] GetDirectories(string path)
		{
			return Directory.GetDirectories(path, "*");
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000B0A3E File Offset: 0x000AFA3E
		public static string[] GetDirectories(string path, string searchPattern)
		{
			return Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x000B0A48 File Offset: 0x000AFA48
		public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			return Directory.InternalGetFileDirectoryNames(path, path, searchPattern, false, true, searchOption);
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x000B0A71 File Offset: 0x000AFA71
		public static string[] GetFileSystemEntries(string path)
		{
			return Directory.GetFileSystemEntries(path, "*");
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x000B0A7E File Offset: 0x000AFA7E
		public static string[] GetFileSystemEntries(string path, string searchPattern)
		{
			return Directory.GetFileSystemEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x000B0A88 File Offset: 0x000AFA88
		private static string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			return Directory.InternalGetFileDirectoryNames(path, path, searchPattern, true, true, searchOption);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x000B0AB4 File Offset: 0x000AFAB4
		internal static string[] InternalGetFileDirectoryNames(string path, string userPathOriginal, string searchPattern, bool includeFiles, bool includeDirs, SearchOption searchOption)
		{
			int num = 0;
			if (searchOption != SearchOption.TopDirectoryOnly && searchOption != SearchOption.AllDirectories)
			{
				throw new ArgumentOutOfRangeException("searchOption", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			searchPattern = searchPattern.TrimEnd(new char[0]);
			if (searchPattern.Length == 0)
			{
				return new string[0];
			}
			Path.CheckSearchPattern(searchPattern);
			string text = Path.GetFullPathInternal(path);
			string[] array = new string[] { Directory.GetDemandDir(text, true) };
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, array, false, false).Demand();
			string text2 = userPathOriginal;
			string directoryName = Path.GetDirectoryName(searchPattern);
			if (directoryName != null && directoryName.Length != 0)
			{
				array = new string[] { Directory.GetDemandDir(Path.InternalCombine(text, directoryName), true) };
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, array, false, false).Demand();
				text2 = Path.Combine(text2, directoryName);
			}
			string text3 = Path.InternalCombine(text, searchPattern);
			char c = text3[text3.Length - 1];
			if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar || c == Path.VolumeSeparatorChar)
			{
				text3 += '*';
			}
			text = Path.GetDirectoryName(text3);
			bool flag = false;
			bool flag2 = false;
			c = text[text.Length - 1];
			flag = c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
			string text4;
			if (flag)
			{
				text4 = text3.Substring(text.Length);
			}
			else
			{
				text4 = text3.Substring(text.Length + 1);
			}
			Win32Native.WIN32_FIND_DATA win32_FIND_DATA = new Win32Native.WIN32_FIND_DATA();
			SafeFindHandle safeFindHandle = null;
			Directory.SearchData searchData = new Directory.SearchData(text, text2, searchOption);
			List<Directory.SearchData> list = new List<Directory.SearchData>();
			list.Add(searchData);
			List<string> list2 = new List<string>();
			int num2 = 0;
			string[] array2 = new string[10];
			int num3 = Win32Native.SetErrorMode(1);
			try
			{
				while (list.Count > 0)
				{
					searchData = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
					c = searchData.fullPath[searchData.fullPath.Length - 1];
					flag = c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
					if (searchData.userPath.Length > 0)
					{
						c = searchData.userPath[searchData.userPath.Length - 1];
						flag2 = c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
					}
					if (searchData.searchOption != SearchOption.TopDirectoryOnly)
					{
						try
						{
							string text5;
							if (flag)
							{
								text5 = searchData.fullPath + "*";
							}
							else
							{
								text5 = searchData.fullPath + Path.DirectorySeparatorChar + "*";
							}
							safeFindHandle = Win32Native.FindFirstFile(text5, win32_FIND_DATA);
							if (safeFindHandle.IsInvalid)
							{
								num = Marshal.GetLastWin32Error();
								if (num == 2)
								{
									continue;
								}
								__Error.WinIOError(num, searchData.fullPath);
							}
							do
							{
								if ((win32_FIND_DATA.dwFileAttributes & 16) != 0 && !win32_FIND_DATA.cFileName.Equals(".") && !win32_FIND_DATA.cFileName.Equals(".."))
								{
									Directory.SearchData searchData2 = new Directory.SearchData();
									StringBuilder stringBuilder = new StringBuilder(searchData.fullPath);
									if (!flag)
									{
										stringBuilder.Append(Path.DirectorySeparatorChar);
									}
									stringBuilder.Append(win32_FIND_DATA.cFileName);
									searchData2.fullPath = stringBuilder.ToString();
									stringBuilder.Length = 0;
									stringBuilder.Append(searchData.userPath);
									if (!flag2)
									{
										stringBuilder.Append(Path.DirectorySeparatorChar);
									}
									stringBuilder.Append(win32_FIND_DATA.cFileName);
									searchData2.userPath = stringBuilder.ToString();
									searchData2.searchOption = searchData.searchOption;
									list.Add(searchData2);
								}
							}
							while (Win32Native.FindNextFile(safeFindHandle, win32_FIND_DATA));
						}
						finally
						{
							if (safeFindHandle != null)
							{
								safeFindHandle.Dispose();
							}
						}
					}
					try
					{
						string text5;
						if (flag)
						{
							text5 = searchData.fullPath + text4;
						}
						else
						{
							text5 = searchData.fullPath + Path.DirectorySeparatorChar + text4;
						}
						safeFindHandle = Win32Native.FindFirstFile(text5, win32_FIND_DATA);
						if (safeFindHandle.IsInvalid)
						{
							num = Marshal.GetLastWin32Error();
							if (num == 2)
							{
								continue;
							}
							__Error.WinIOError(num, searchData.fullPath);
						}
						int num4 = 0;
						do
						{
							bool flag3 = false;
							if (includeFiles)
							{
								flag3 = 0 == (win32_FIND_DATA.dwFileAttributes & 16);
							}
							if (includeDirs && (win32_FIND_DATA.dwFileAttributes & 16) != 0 && !win32_FIND_DATA.cFileName.Equals(".") && !win32_FIND_DATA.cFileName.Equals(".."))
							{
								flag3 = true;
							}
							if (flag3)
							{
								num4++;
								if (num2 == array2.Length)
								{
									string[] array3 = new string[array2.Length * 2];
									Array.Copy(array2, 0, array3, 0, num2);
									array2 = array3;
								}
								array2[num2++] = Path.InternalCombine(searchData.userPath, win32_FIND_DATA.cFileName);
							}
						}
						while (Win32Native.FindNextFile(safeFindHandle, win32_FIND_DATA));
						num = Marshal.GetLastWin32Error();
						if (num4 > 0)
						{
							list2.Add(Directory.GetDemandDir(searchData.fullPath, true));
						}
					}
					finally
					{
						if (safeFindHandle != null)
						{
							safeFindHandle.Dispose();
						}
					}
				}
			}
			finally
			{
				Win32Native.SetErrorMode(num3);
			}
			if (num != 0 && num != 18 && num != 2)
			{
				__Error.WinIOError(num, searchData.fullPath);
			}
			if (list2.Count > 0)
			{
				array = new string[list2.Count];
				list2.CopyTo(array, 0);
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, array, false, false).Demand();
			}
			if (num2 == array2.Length)
			{
				return array2;
			}
			string[] array4 = new string[num2];
			Array.Copy(array2, 0, array4, 0, num2);
			return array4;
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x000B1078 File Offset: 0x000B0078
		public static string[] GetLogicalDrives()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			int logicalDrives = Win32Native.GetLogicalDrives();
			if (logicalDrives == 0)
			{
				__Error.WinIOError();
			}
			uint num = (uint)logicalDrives;
			int num2 = 0;
			while (num != 0U)
			{
				if ((num & 1U) != 0U)
				{
					num2++;
				}
				num >>= 1;
			}
			string[] array = new string[num2];
			char[] array2 = new char[] { 'A', ':', '\\' };
			num = (uint)logicalDrives;
			num2 = 0;
			while (num != 0U)
			{
				if ((num & 1U) != 0U)
				{
					array[num2++] = new string(array2);
				}
				num >>= 1;
				char[] array3 = array2;
				int num3 = 0;
				array3[num3] += '\u0001';
			}
			return array;
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x000B1108 File Offset: 0x000B0108
		public static string GetDirectoryRoot(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			string text = fullPathInternal.Substring(0, Path.GetRootLength(fullPathInternal));
			string demandDir = Directory.GetDemandDir(text, true);
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { demandDir }, false, false).Demand();
			return text;
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x000B115A File Offset: 0x000B015A
		internal static string InternalGetDirectoryRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			return path.Substring(0, Path.GetRootLength(path));
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x000B1170 File Offset: 0x000B0170
		public static string GetCurrentDirectory()
		{
			StringBuilder stringBuilder = new StringBuilder(261);
			if (Win32Native.GetCurrentDirectory(stringBuilder.Capacity, stringBuilder) == 0)
			{
				__Error.WinIOError();
			}
			string text = stringBuilder.ToString();
			if (text.IndexOf('~') >= 0)
			{
				int longPathName = Win32Native.GetLongPathName(text, stringBuilder, stringBuilder.Capacity);
				if (longPathName == 0 || longPathName >= 260)
				{
					int num = Marshal.GetLastWin32Error();
					if (longPathName >= 260)
					{
						num = 206;
					}
					if (num != 2 && num != 3 && num != 1 && num != 5)
					{
						__Error.WinIOError(num, string.Empty);
					}
				}
				text = stringBuilder.ToString();
			}
			string demandDir = Directory.GetDemandDir(text, true);
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { demandDir }, false, false).Demand();
			return text;
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x000B1224 File Offset: 0x000B0224
		public static void SetCurrentDirectory(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("value");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_PathEmpty"));
			}
			if (path.Length >= 260)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			string fullPathInternal = Path.GetFullPathInternal(path);
			if (Environment.IsWin9X() && !Directory.InternalExists(Path.GetPathRoot(fullPathInternal)))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.PathNotFound_Path"), new object[] { path }));
			}
			if (!Win32Native.SetCurrentDirectory(fullPathInternal))
			{
				int num = Marshal.GetLastWin32Error();
				if (num == 2)
				{
					num = 3;
				}
				__Error.WinIOError(num, fullPathInternal);
			}
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x000B12DC File Offset: 0x000B02DC
		public static void Move(string sourceDirName, string destDirName)
		{
			if (sourceDirName == null)
			{
				throw new ArgumentNullException("sourceDirName");
			}
			if (sourceDirName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "sourceDirName");
			}
			if (destDirName == null)
			{
				throw new ArgumentNullException("destDirName");
			}
			if (destDirName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "destDirName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceDirName);
			string demandDir = Directory.GetDemandDir(fullPathInternal, false);
			if (demandDir.Length >= 249)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			string fullPathInternal2 = Path.GetFullPathInternal(destDirName);
			string demandDir2 = Directory.GetDemandDir(fullPathInternal2, false);
			if (demandDir2.Length >= 249)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[] { demandDir }, false, false).Demand();
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { demandDir2 }, false, false).Demand();
			if (CultureInfo.InvariantCulture.CompareInfo.Compare(demandDir, demandDir2, CompareOptions.IgnoreCase) == 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SourceDestMustBeDifferent"));
			}
			string pathRoot = Path.GetPathRoot(demandDir);
			string pathRoot2 = Path.GetPathRoot(demandDir2);
			if (CultureInfo.InvariantCulture.CompareInfo.Compare(pathRoot, pathRoot2, CompareOptions.IgnoreCase) != 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_SourceDestMustHaveSameRoot"));
			}
			if (Environment.IsWin9X() && !Directory.InternalExists(Path.GetPathRoot(fullPathInternal2)))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.PathNotFound_Path"), new object[] { destDirName }));
			}
			if (!Win32Native.MoveFile(sourceDirName, destDirName))
			{
				int num = Marshal.GetLastWin32Error();
				if (num == 2)
				{
					num = 3;
					__Error.WinIOError(num, fullPathInternal);
				}
				if (num == 5)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[] { sourceDirName }), Win32Native.MakeHRFromErrorCode(num));
				}
				__Error.WinIOError(num, string.Empty);
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x000B14C8 File Offset: 0x000B04C8
		public static void Delete(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			Directory.Delete(fullPathInternal, path, false);
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x000B14E4 File Offset: 0x000B04E4
		public static void Delete(string path, bool recursive)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			Directory.Delete(fullPathInternal, path, recursive);
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x000B1500 File Offset: 0x000B0500
		internal static void Delete(string fullPath, string userPath, bool recursive)
		{
			string demandDir = Directory.GetDemandDir(fullPath, !recursive);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { demandDir }, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPath, ref win32_FILE_ATTRIBUTE_DATA, false, true);
			if (num != 0)
			{
				if (num == 2)
				{
					num = 3;
				}
				__Error.WinIOError(num, fullPath);
			}
			if ((win32_FILE_ATTRIBUTE_DATA.fileAttributes & 1024) != 0)
			{
				recursive = false;
			}
			Directory.DeleteHelper(fullPath, userPath, recursive);
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x000B1570 File Offset: 0x000B0570
		private static void DeleteHelper(string fullPath, string userPath, bool recursive)
		{
			Exception ex = null;
			if (recursive)
			{
				Win32Native.WIN32_FIND_DATA win32_FIND_DATA = new Win32Native.WIN32_FIND_DATA();
				int num;
				using (SafeFindHandle safeFindHandle = Win32Native.FindFirstFile(fullPath + Path.DirectorySeparatorChar + "*", win32_FIND_DATA))
				{
					if (safeFindHandle.IsInvalid)
					{
						num = Marshal.GetLastWin32Error();
						__Error.WinIOError(num, fullPath);
					}
					for (;;)
					{
						bool flag = 0 != (win32_FIND_DATA.dwFileAttributes & 16);
						if (!flag)
						{
							goto IL_015A;
						}
						if (!win32_FIND_DATA.cFileName.Equals(".") && !win32_FIND_DATA.cFileName.Equals(".."))
						{
							bool flag2 = 0 == (win32_FIND_DATA.dwFileAttributes & 1024);
							if (flag2)
							{
								string text = Path.InternalCombine(fullPath, win32_FIND_DATA.cFileName);
								string text2 = Path.InternalCombine(userPath, win32_FIND_DATA.cFileName);
								try
								{
									Directory.DeleteHelper(text, text2, recursive);
									goto IL_0191;
								}
								catch (Exception ex2)
								{
									if (ex == null)
									{
										ex = ex2;
									}
									goto IL_0191;
								}
							}
							if (win32_FIND_DATA.dwReserved0 == -1610612733)
							{
								string text3 = Path.InternalCombine(fullPath, win32_FIND_DATA.cFileName + Path.DirectorySeparatorChar);
								if (!Win32Native.DeleteVolumeMountPoint(text3))
								{
									num = Marshal.GetLastWin32Error();
									try
									{
										__Error.WinIOError(num, win32_FIND_DATA.cFileName);
									}
									catch (Exception ex3)
									{
										if (ex == null)
										{
											ex = ex3;
										}
									}
								}
							}
							string text4 = Path.InternalCombine(fullPath, win32_FIND_DATA.cFileName);
							if (!Win32Native.RemoveDirectory(text4))
							{
								num = Marshal.GetLastWin32Error();
								try
								{
									__Error.WinIOError(num, win32_FIND_DATA.cFileName);
									goto IL_0191;
								}
								catch (Exception ex4)
								{
									if (ex == null)
									{
										ex = ex4;
									}
									goto IL_0191;
								}
								goto IL_015A;
							}
						}
						IL_0191:
						if (!Win32Native.FindNextFile(safeFindHandle, win32_FIND_DATA))
						{
							break;
						}
						continue;
						IL_015A:
						string text5 = Path.InternalCombine(fullPath, win32_FIND_DATA.cFileName);
						if (!Win32Native.DeleteFile(text5))
						{
							num = Marshal.GetLastWin32Error();
							try
							{
								__Error.WinIOError(num, win32_FIND_DATA.cFileName);
							}
							catch (Exception ex5)
							{
								if (ex == null)
								{
									ex = ex5;
								}
							}
							goto IL_0191;
						}
						goto IL_0191;
					}
					num = Marshal.GetLastWin32Error();
				}
				if (ex != null)
				{
					throw ex;
				}
				if (num != 0 && num != 18)
				{
					__Error.WinIOError(num, userPath);
				}
			}
			if (!Win32Native.RemoveDirectory(fullPath))
			{
				int num = Marshal.GetLastWin32Error();
				if (num == 2)
				{
					num = 3;
				}
				if (num == 5)
				{
					throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[] { userPath }));
				}
				__Error.WinIOError(num, fullPath);
			}
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x000B180C File Offset: 0x000B080C
		private static SafeFileHandle OpenHandle(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			string pathRoot = Path.GetPathRoot(fullPathInternal);
			if (pathRoot == fullPathInternal && pathRoot[1] == Path.VolumeSeparatorChar)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_PathIsVolume"));
			}
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { Directory.GetDemandDir(fullPathInternal, true) }, false, false).Demand();
			SafeFileHandle safeFileHandle = Win32Native.SafeCreateFile(fullPathInternal, 1073741824, FileShare.Write | FileShare.Delete, null, FileMode.Open, 33554432, Win32Native.NULL);
			if (safeFileHandle.IsInvalid)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
			return safeFileHandle;
		}

		// Token: 0x04001BC0 RID: 7104
		private const int FILE_ATTRIBUTE_DIRECTORY = 16;

		// Token: 0x04001BC1 RID: 7105
		private const int GENERIC_WRITE = 1073741824;

		// Token: 0x04001BC2 RID: 7106
		private const int FILE_SHARE_WRITE = 2;

		// Token: 0x04001BC3 RID: 7107
		private const int FILE_SHARE_DELETE = 4;

		// Token: 0x04001BC4 RID: 7108
		private const int OPEN_EXISTING = 3;

		// Token: 0x04001BC5 RID: 7109
		private const int FILE_FLAG_BACKUP_SEMANTICS = 33554432;

		// Token: 0x02000594 RID: 1428
		private sealed class SearchData
		{
			// Token: 0x0600350C RID: 13580 RVA: 0x000B189F File Offset: 0x000B089F
			public SearchData()
			{
			}

			// Token: 0x0600350D RID: 13581 RVA: 0x000B18A7 File Offset: 0x000B08A7
			public SearchData(string fullPath, string userPath, SearchOption searchOption)
			{
				this.fullPath = fullPath;
				this.userPath = userPath;
				this.searchOption = searchOption;
			}

			// Token: 0x04001BC6 RID: 7110
			public string fullPath;

			// Token: 0x04001BC7 RID: 7111
			public string userPath;

			// Token: 0x04001BC8 RID: 7112
			public SearchOption searchOption;
		}
	}
}

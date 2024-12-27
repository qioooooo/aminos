using System;
using System.Collections;
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
	// Token: 0x0200059E RID: 1438
	[ComVisible(true)]
	public static class File
	{
		// Token: 0x06003562 RID: 13666 RVA: 0x000B2BE5 File Offset: 0x000B1BE5
		public static StreamReader OpenText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamReader(path);
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000B2BFB File Offset: 0x000B1BFB
		public static StreamWriter CreateText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, false);
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000B2C12 File Offset: 0x000B1C12
		public static StreamWriter AppendText(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return new StreamWriter(path, true);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000B2C29 File Offset: 0x000B1C29
		public static void Copy(string sourceFileName, string destFileName)
		{
			File.Copy(sourceFileName, destFileName, false);
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000B2C33 File Offset: 0x000B1C33
		public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			File.InternalCopy(sourceFileName, destFileName, overwrite);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000B2C40 File Offset: 0x000B1C40
		internal static string InternalCopy(string sourceFileName, string destFileName, bool overwrite)
		{
			if (sourceFileName == null || destFileName == null)
			{
				throw new ArgumentNullException((sourceFileName == null) ? "sourceFileName" : "destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
			}
			if (sourceFileName.Length == 0 || destFileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), (sourceFileName.Length == 0) ? "sourceFileName" : "destFileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { fullPathInternal }, false, false).Demand();
			string fullPathInternal2 = Path.GetFullPathInternal(destFileName);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { fullPathInternal2 }, false, false).Demand();
			if (!Win32Native.CopyFile(fullPathInternal, fullPathInternal2, !overwrite))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				string text = destFileName;
				if (lastWin32Error != 80)
				{
					using (SafeFileHandle safeFileHandle = Win32Native.UnsafeCreateFile(fullPathInternal, int.MinValue, FileShare.Read, null, FileMode.Open, 0, IntPtr.Zero))
					{
						if (safeFileHandle.IsInvalid)
						{
							text = sourceFileName;
						}
					}
					if (lastWin32Error == 5 && Directory.InternalExists(fullPathInternal2))
					{
						throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_FileIsDirectory_Name"), new object[] { destFileName }), 5, fullPathInternal2);
					}
				}
				__Error.WinIOError(lastWin32Error, text);
			}
			return fullPathInternal2;
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000B2D8C File Offset: 0x000B1D8C
		public static FileStream Create(string path)
		{
			return File.Create(path, 4096, FileOptions.None);
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000B2D9A File Offset: 0x000B1D9A
		public static FileStream Create(string path, int bufferSize)
		{
			return File.Create(path, bufferSize, FileOptions.None);
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000B2DA4 File Offset: 0x000B1DA4
		public static FileStream Create(string path, int bufferSize, FileOptions options)
		{
			return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000B2DB1 File Offset: 0x000B1DB1
		public static FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
		{
			return new FileStream(path, FileMode.Create, FileSystemRights.ReadData | FileSystemRights.WriteData | FileSystemRights.AppendData | FileSystemRights.ReadExtendedAttributes | FileSystemRights.WriteExtendedAttributes | FileSystemRights.ReadAttributes | FileSystemRights.WriteAttributes | FileSystemRights.ReadPermissions, FileShare.None, bufferSize, options, fileSecurity);
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000B2DC4 File Offset: 0x000B1DC4
		public static void Delete(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { fullPathInternal }, false, false).Demand();
			if (Environment.IsWin9X() && Directory.InternalExists(fullPathInternal))
			{
				throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[] { path }));
			}
			if (!Win32Native.DeleteFile(fullPathInternal))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 2)
				{
					return;
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000B2E54 File Offset: 0x000B1E54
		public static void Decrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!Environment.RunningOnWinNT)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[] { fullPathInternal }, false, false).Demand();
			if (!Win32Native.DecryptFile(fullPathInternal, 0))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 5)
				{
					DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(fullPathInternal));
					if (!string.Equals("NTFS", driveInfo.DriveFormat))
					{
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_EncryptionNeedsNTFS"));
					}
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000B2EF4 File Offset: 0x000B1EF4
		public static void Encrypt(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!Environment.RunningOnWinNT)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[] { fullPathInternal }, false, false).Demand();
			if (!Win32Native.EncryptFile(fullPathInternal))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 5)
				{
					DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(fullPathInternal));
					if (!string.Equals("NTFS", driveInfo.DriveFormat))
					{
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_EncryptionNeedsNTFS"));
					}
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000B2F94 File Offset: 0x000B1F94
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
				path = Path.GetFullPathInternal(path);
				new FileIOPermission(FileIOPermissionAccess.Read, new string[] { path }, false, false).Demand();
				return File.InternalExists(path);
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

		// Token: 0x06003570 RID: 13680 RVA: 0x000B3030 File Offset: 0x000B2030
		internal static bool InternalExists(string path)
		{
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			return File.FillAttributeInfo(path, ref win32_FILE_ATTRIBUTE_DATA, false, true) == 0 && win32_FILE_ATTRIBUTE_DATA.fileAttributes != -1 && (win32_FILE_ATTRIBUTE_DATA.fileAttributes & 16) == 0;
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000B306C File Offset: 0x000B206C
		public static FileStream Open(string path, FileMode mode)
		{
			return File.Open(path, mode, (mode == FileMode.Append) ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000B307E File Offset: 0x000B207E
		public static FileStream Open(string path, FileMode mode, FileAccess access)
		{
			return File.Open(path, mode, access, FileShare.None);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000B3089 File Offset: 0x000B2089
		public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			return new FileStream(path, mode, access, share);
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000B3094 File Offset: 0x000B2094
		public static void SetCreationTime(string path, DateTime creationTime)
		{
			File.SetCreationTimeUtc(path, creationTime.ToUniversalTime());
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000B30A4 File Offset: 0x000B20A4
		public unsafe static void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
		{
			SafeFileHandle safeFileHandle;
			using (File.OpenFile(path, FileAccess.Write, out safeFileHandle))
			{
				Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(creationTimeUtc.ToFileTimeUtc());
				if (!Win32Native.SetFileTime(safeFileHandle, &file_TIME, null, null))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					__Error.WinIOError(lastWin32Error, path);
				}
			}
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000B310C File Offset: 0x000B210C
		public static DateTime GetCreationTime(string path)
		{
			return File.GetCreationTimeUtc(path).ToLocalTime();
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000B3128 File Offset: 0x000B2128
		public static DateTime GetCreationTimeUtc(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { fullPathInternal }, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, false);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			long num2 = (long)(((ulong)win32_FILE_ATTRIBUTE_DATA.ftCreationTimeHigh << 32) | (ulong)win32_FILE_ATTRIBUTE_DATA.ftCreationTimeLow);
			return DateTime.FromFileTimeUtc(num2);
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x000B3190 File Offset: 0x000B2190
		public static void SetLastAccessTime(string path, DateTime lastAccessTime)
		{
			File.SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x000B31A0 File Offset: 0x000B21A0
		public unsafe static void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
		{
			SafeFileHandle safeFileHandle;
			using (File.OpenFile(path, FileAccess.Write, out safeFileHandle))
			{
				Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(lastAccessTimeUtc.ToFileTimeUtc());
				if (!Win32Native.SetFileTime(safeFileHandle, null, &file_TIME, null))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					__Error.WinIOError(lastWin32Error, path);
				}
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000B3208 File Offset: 0x000B2208
		public static DateTime GetLastAccessTime(string path)
		{
			return File.GetLastAccessTimeUtc(path).ToLocalTime();
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000B3224 File Offset: 0x000B2224
		public static DateTime GetLastAccessTimeUtc(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { fullPathInternal }, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, false);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			long num2 = (long)(((ulong)win32_FILE_ATTRIBUTE_DATA.ftLastAccessTimeHigh << 32) | (ulong)win32_FILE_ATTRIBUTE_DATA.ftLastAccessTimeLow);
			return DateTime.FromFileTimeUtc(num2);
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x000B328C File Offset: 0x000B228C
		public static void SetLastWriteTime(string path, DateTime lastWriteTime)
		{
			File.SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x000B329C File Offset: 0x000B229C
		public unsafe static void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
		{
			SafeFileHandle safeFileHandle;
			using (File.OpenFile(path, FileAccess.Write, out safeFileHandle))
			{
				Win32Native.FILE_TIME file_TIME = new Win32Native.FILE_TIME(lastWriteTimeUtc.ToFileTimeUtc());
				if (!Win32Native.SetFileTime(safeFileHandle, null, null, &file_TIME))
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					__Error.WinIOError(lastWin32Error, path);
				}
			}
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x000B3304 File Offset: 0x000B2304
		public static DateTime GetLastWriteTime(string path)
		{
			return File.GetLastWriteTimeUtc(path).ToLocalTime();
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x000B3320 File Offset: 0x000B2320
		public static DateTime GetLastWriteTimeUtc(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { fullPathInternal }, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, false);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			long num2 = (long)(((ulong)win32_FILE_ATTRIBUTE_DATA.ftLastWriteTimeHigh << 32) | (ulong)win32_FILE_ATTRIBUTE_DATA.ftLastWriteTimeLow);
			return DateTime.FromFileTimeUtc(num2);
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000B3388 File Offset: 0x000B2388
		public static FileAttributes GetAttributes(string path)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Read, new string[] { fullPathInternal }, false, false).Demand();
			Win32Native.WIN32_FILE_ATTRIBUTE_DATA win32_FILE_ATTRIBUTE_DATA = default(Win32Native.WIN32_FILE_ATTRIBUTE_DATA);
			int num = File.FillAttributeInfo(fullPathInternal, ref win32_FILE_ATTRIBUTE_DATA, false, true);
			if (num != 0)
			{
				__Error.WinIOError(num, fullPathInternal);
			}
			return (FileAttributes)win32_FILE_ATTRIBUTE_DATA.fileAttributes;
		}

		// Token: 0x06003581 RID: 13697 RVA: 0x000B33DC File Offset: 0x000B23DC
		public static void SetAttributes(string path, FileAttributes fileAttributes)
		{
			string fullPathInternal = Path.GetFullPathInternal(path);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { fullPathInternal }, false, false).Demand();
			if (!Win32Native.SetFileAttributes(fullPathInternal, (int)fileAttributes))
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error == 87 || (lastWin32Error == 5 && Environment.IsWin9X()))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_InvalidFileAttrs"));
				}
				__Error.WinIOError(lastWin32Error, fullPathInternal);
			}
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x000B3441 File Offset: 0x000B2441
		public static FileSecurity GetAccessControl(string path)
		{
			return File.GetAccessControl(path, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x000B344B File Offset: 0x000B244B
		public static FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
		{
			return new FileSecurity(path, includeSections);
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000B3454 File Offset: 0x000B2454
		public static void SetAccessControl(string path, FileSecurity fileSecurity)
		{
			if (fileSecurity == null)
			{
				throw new ArgumentNullException("fileSecurity");
			}
			string fullPathInternal = Path.GetFullPathInternal(path);
			fileSecurity.Persist(fullPathInternal);
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000B347D File Offset: 0x000B247D
		public static FileStream OpenRead(string path)
		{
			return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x000B3488 File Offset: 0x000B2488
		public static FileStream OpenWrite(string path)
		{
			return new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x000B3493 File Offset: 0x000B2493
		public static string ReadAllText(string path)
		{
			return File.ReadAllText(path, Encoding.UTF8);
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x000B34A0 File Offset: 0x000B24A0
		public static string ReadAllText(string path, Encoding encoding)
		{
			string text;
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				text = streamReader.ReadToEnd();
			}
			return text;
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000B34DC File Offset: 0x000B24DC
		public static void WriteAllText(string path, string contents)
		{
			File.WriteAllText(path, contents, StreamWriter.UTF8NoBOM);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000B34EC File Offset: 0x000B24EC
		public static void WriteAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000B3528 File Offset: 0x000B2528
		public static byte[] ReadAllBytes(string path)
		{
			byte[] array;
			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				int num = 0;
				long length = fileStream.Length;
				if (length > 2147483647L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_FileTooLong2GB"));
				}
				int i = (int)length;
				array = new byte[i];
				while (i > 0)
				{
					int num2 = fileStream.Read(array, num, i);
					if (num2 == 0)
					{
						__Error.EndOfFile();
					}
					num += num2;
					i -= num2;
				}
			}
			return array;
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000B35B4 File Offset: 0x000B25B4
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000B3600 File Offset: 0x000B2600
		public static string[] ReadAllLines(string path)
		{
			return File.ReadAllLines(path, Encoding.UTF8);
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000B3610 File Offset: 0x000B2610
		public static string[] ReadAllLines(string path, Encoding encoding)
		{
			ArrayList arrayList = new ArrayList();
			using (StreamReader streamReader = new StreamReader(path, encoding))
			{
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					arrayList.Add(text);
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000B3670 File Offset: 0x000B2670
		public static void WriteAllLines(string path, string[] contents)
		{
			File.WriteAllLines(path, contents, StreamWriter.UTF8NoBOM);
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000B3680 File Offset: 0x000B2680
		public static void WriteAllLines(string path, string[] contents, Encoding encoding)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
			{
				foreach (string text in contents)
				{
					streamWriter.WriteLine(text);
				}
			}
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000B36DC File Offset: 0x000B26DC
		public static void AppendAllText(string path, string contents)
		{
			File.AppendAllText(path, contents, StreamWriter.UTF8NoBOM);
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000B36EC File Offset: 0x000B26EC
		public static void AppendAllText(string path, string contents, Encoding encoding)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, true, encoding))
			{
				streamWriter.Write(contents);
			}
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000B3728 File Offset: 0x000B2728
		public static void Move(string sourceFileName, string destFileName)
		{
			if (sourceFileName == null || destFileName == null)
			{
				throw new ArgumentNullException((sourceFileName == null) ? "sourceFileName" : "destFileName", Environment.GetResourceString("ArgumentNull_FileName"));
			}
			if (sourceFileName.Length == 0 || destFileName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), (sourceFileName.Length == 0) ? "sourceFileName" : "destFileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceFileName);
			new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[] { fullPathInternal }, false, false).Demand();
			string fullPathInternal2 = Path.GetFullPathInternal(destFileName);
			new FileIOPermission(FileIOPermissionAccess.Write, new string[] { fullPathInternal2 }, false, false).Demand();
			if (!File.InternalExists(fullPathInternal))
			{
				__Error.WinIOError(2, fullPathInternal);
			}
			if (!Win32Native.MoveFile(fullPathInternal, fullPathInternal2))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000B37EB File Offset: 0x000B27EB
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
		{
			File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000B37F8 File Offset: 0x000B27F8
		public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
		{
			if (sourceFileName == null)
			{
				throw new ArgumentNullException("sourceFileName");
			}
			if (destinationFileName == null)
			{
				throw new ArgumentNullException("destinationFileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(sourceFileName);
			string fullPathInternal2 = Path.GetFullPathInternal(destinationFileName);
			string text = null;
			if (destinationBackupFileName != null)
			{
				text = Path.GetFullPathInternal(destinationBackupFileName);
			}
			FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.Write, new string[] { fullPathInternal, fullPathInternal2 });
			if (destinationBackupFileName != null)
			{
				fileIOPermission.AddPathList(FileIOPermissionAccess.Write, text);
			}
			fileIOPermission.Demand();
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			int num = 1;
			if (ignoreMetadataErrors)
			{
				num |= 2;
			}
			if (!Win32Native.ReplaceFile(fullPathInternal2, fullPathInternal, text, num, IntPtr.Zero, IntPtr.Zero))
			{
				__Error.WinIOError();
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000B38B0 File Offset: 0x000B28B0
		internal static int FillAttributeInfo(string path, ref Win32Native.WIN32_FILE_ATTRIBUTE_DATA data, bool tryagain, bool returnErrorOnNotFound)
		{
			int num = 0;
			if (Environment.OSInfo == Environment.OSName.Win95 || tryagain)
			{
				Win32Native.WIN32_FIND_DATA win32_FIND_DATA = new Win32Native.WIN32_FIND_DATA();
				string text = path.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
				int num2 = Win32Native.SetErrorMode(1);
				try
				{
					bool flag = false;
					SafeFindHandle safeFindHandle = Win32Native.FindFirstFile(text, win32_FIND_DATA);
					try
					{
						if (safeFindHandle.IsInvalid)
						{
							flag = true;
							num = Marshal.GetLastWin32Error();
							if ((num == 2 || num == 3 || num == 21) && !returnErrorOnNotFound)
							{
								num = 0;
								data.fileAttributes = -1;
							}
							return num;
						}
					}
					finally
					{
						try
						{
							safeFindHandle.Close();
						}
						catch
						{
							if (!flag)
							{
								__Error.WinIOError();
							}
						}
					}
				}
				finally
				{
					Win32Native.SetErrorMode(num2);
				}
				data.fileAttributes = win32_FIND_DATA.dwFileAttributes;
				data.ftCreationTimeLow = (uint)win32_FIND_DATA.ftCreationTime_dwLowDateTime;
				data.ftCreationTimeHigh = (uint)win32_FIND_DATA.ftCreationTime_dwHighDateTime;
				data.ftLastAccessTimeLow = (uint)win32_FIND_DATA.ftLastAccessTime_dwLowDateTime;
				data.ftLastAccessTimeHigh = (uint)win32_FIND_DATA.ftLastAccessTime_dwHighDateTime;
				data.ftLastWriteTimeLow = (uint)win32_FIND_DATA.ftLastWriteTime_dwLowDateTime;
				data.ftLastWriteTimeHigh = (uint)win32_FIND_DATA.ftLastWriteTime_dwHighDateTime;
				data.fileSizeHigh = win32_FIND_DATA.nFileSizeHigh;
				data.fileSizeLow = win32_FIND_DATA.nFileSizeLow;
				return num;
			}
			bool flag2 = false;
			int num3 = Win32Native.SetErrorMode(1);
			try
			{
				flag2 = Win32Native.GetFileAttributesEx(path, 0, ref data);
			}
			finally
			{
				Win32Native.SetErrorMode(num3);
			}
			if (!flag2)
			{
				num = Marshal.GetLastWin32Error();
				if (num != 2 && num != 3 && num != 21)
				{
					return File.FillAttributeInfo(path, ref data, true, returnErrorOnNotFound);
				}
				if (!returnErrorOnNotFound)
				{
					num = 0;
					data.fileAttributes = -1;
				}
			}
			return num;
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000B3A54 File Offset: 0x000B2A54
		private static FileStream OpenFile(string path, FileAccess access, out SafeFileHandle handle)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, access, FileShare.ReadWrite, 1);
			handle = fileStream.SafeFileHandle;
			if (handle.IsInvalid)
			{
				int num = Marshal.GetLastWin32Error();
				string fullPathInternal = Path.GetFullPathInternal(path);
				if (num == 3 && fullPathInternal.Equals(Directory.GetDirectoryRoot(fullPathInternal)))
				{
					num = 5;
				}
				__Error.WinIOError(num, path);
			}
			return fileStream;
		}

		// Token: 0x04001BDE RID: 7134
		private const int GetFileExInfoStandard = 0;

		// Token: 0x04001BDF RID: 7135
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001BE0 RID: 7136
		private const int ERROR_ACCESS_DENIED = 5;
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.IO
{
	// Token: 0x0200058E RID: 1422
	internal static class __Error
	{
		// Token: 0x06003482 RID: 13442 RVA: 0x000AE3E8 File Offset: 0x000AD3E8
		internal static void EndOfFile()
		{
			throw new EndOfStreamException(Environment.GetResourceString("IO.EOF_ReadBeyondEOF"));
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x000AE3F9 File Offset: 0x000AD3F9
		internal static void FileNotOpen()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_FileClosed"));
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x000AE40B File Offset: 0x000AD40B
		internal static void StreamIsClosed()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_StreamClosed"));
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000AE41D File Offset: 0x000AD41D
		internal static void MemoryStreamNotExpandable()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_MemStreamNotExpandable"));
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000AE42E File Offset: 0x000AD42E
		internal static void ReaderClosed()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ReaderClosed"));
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x000AE440 File Offset: 0x000AD440
		internal static void ReadNotSupported()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnreadableStream"));
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x000AE451 File Offset: 0x000AD451
		internal static void SeekNotSupported()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000AE462 File Offset: 0x000AD462
		internal static void WrongAsyncResult()
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_WrongAsyncResult"));
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000AE473 File Offset: 0x000AD473
		internal static void EndReadCalledTwice()
		{
			throw new ArgumentException(Environment.GetResourceString("InvalidOperation_EndReadCalledMultiple"));
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000AE484 File Offset: 0x000AD484
		internal static void EndWriteCalledTwice()
		{
			throw new ArgumentException(Environment.GetResourceString("InvalidOperation_EndWriteCalledMultiple"));
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000AE498 File Offset: 0x000AD498
		internal static string GetDisplayablePath(string path, bool isInvalidPath)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			bool flag = false;
			if (path.Length < 2)
			{
				return path;
			}
			if (Path.IsDirectorySeparator(path[0]) && Path.IsDirectorySeparator(path[1]))
			{
				flag = true;
			}
			else if (path[1] == Path.VolumeSeparatorChar)
			{
				flag = true;
			}
			if (!flag && !isInvalidPath)
			{
				return path;
			}
			bool flag2 = false;
			try
			{
				if (!isInvalidPath)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, new string[] { path }, false, false).Demand();
					flag2 = true;
				}
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
			if (!flag2)
			{
				if (Path.IsDirectorySeparator(path[path.Length - 1]))
				{
					path = Environment.GetResourceString("IO.IO_NoPermissionToDirectoryName");
				}
				else
				{
					path = Path.GetFileName(path);
				}
			}
			return path;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x000AE578 File Offset: 0x000AD578
		internal static void WinIOError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			__Error.WinIOError(lastWin32Error, string.Empty);
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000AE598 File Offset: 0x000AD598
		internal static void WinIOError(int errorCode, string maybeFullPath)
		{
			bool flag = errorCode == 123 || errorCode == 161;
			string displayablePath = __Error.GetDisplayablePath(maybeFullPath, flag);
			if (errorCode <= 80)
			{
				if (errorCode <= 15)
				{
					switch (errorCode)
					{
					case 2:
						if (displayablePath.Length == 0)
						{
							throw new FileNotFoundException(Environment.GetResourceString("IO.FileNotFound"));
						}
						throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { displayablePath }), displayablePath);
					case 3:
						if (displayablePath.Length == 0)
						{
							throw new DirectoryNotFoundException(Environment.GetResourceString("IO.PathNotFound_NoPathName"));
						}
						throw new DirectoryNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.PathNotFound_Path"), new object[] { displayablePath }));
					case 4:
						break;
					case 5:
						if (displayablePath.Length == 0)
						{
							throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_IODenied_NoPathName"));
						}
						throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("UnauthorizedAccess_IODenied_Path"), new object[] { displayablePath }));
					default:
						if (errorCode == 15)
						{
							throw new DriveNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.DriveNotFound_Drive"), new object[] { displayablePath }));
						}
						break;
					}
				}
				else if (errorCode != 32)
				{
					if (errorCode == 80)
					{
						if (displayablePath.Length != 0)
						{
							throw new IOException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.IO_FileExists_Name"), new object[] { displayablePath }), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
						}
					}
				}
				else
				{
					if (displayablePath.Length == 0)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_SharingViolation_NoFileName"), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
					}
					throw new IOException(Environment.GetResourceString("IO.IO_SharingViolation_File", new object[] { displayablePath }), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
				}
			}
			else if (errorCode <= 183)
			{
				if (errorCode == 87)
				{
					throw new IOException(Win32Native.GetMessage(errorCode), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
				}
				if (errorCode == 183)
				{
					if (displayablePath.Length != 0)
					{
						throw new IOException(Environment.GetResourceString("IO.IO_AlreadyExists_Name", new object[] { displayablePath }), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
					}
				}
			}
			else
			{
				if (errorCode == 206)
				{
					throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
				}
				if (errorCode == 995)
				{
					throw new OperationCanceledException();
				}
			}
			throw new IOException(Win32Native.GetMessage(errorCode), Win32Native.MakeHRFromErrorCode(errorCode), maybeFullPath);
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000AE814 File Offset: 0x000AD814
		internal static void WinIODriveError(string driveName)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			__Error.WinIODriveError(driveName, lastWin32Error);
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000AE830 File Offset: 0x000AD830
		internal static void WinIODriveError(string driveName, int errorCode)
		{
			if (errorCode == 3 || errorCode == 15)
			{
				throw new DriveNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.DriveNotFound_Drive"), new object[] { driveName }));
			}
			__Error.WinIOError(errorCode, driveName);
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000AE875 File Offset: 0x000AD875
		internal static void WriteNotSupported()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnwritableStream"));
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000AE886 File Offset: 0x000AD886
		internal static void WriterClosed()
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_WriterClosed"));
		}

		// Token: 0x04001B9B RID: 7067
		internal const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x04001B9C RID: 7068
		internal const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x04001B9D RID: 7069
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04001B9E RID: 7070
		internal const int ERROR_INVALID_PARAMETER = 87;
	}
}

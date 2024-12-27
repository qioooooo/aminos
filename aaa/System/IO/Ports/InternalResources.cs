using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace System.IO.Ports
{
	// Token: 0x020007A9 RID: 1961
	internal static class InternalResources
	{
		// Token: 0x06003C2D RID: 15405 RVA: 0x00101308 File Offset: 0x00100308
		internal static void EndOfFile()
		{
			throw new EndOfStreamException(SR.GetString("IO_EOF_ReadBeyondEOF"));
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0010131C File Offset: 0x0010031C
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = SafeNativeMethods.FormatMessage(12800, new HandleRef(null, IntPtr.Zero), errorCode, 0, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			return SR.GetString("IO_UnknownError", new object[] { errorCode });
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0010137F File Offset: 0x0010037F
		internal static void FileNotOpen()
		{
			throw new ObjectDisposedException(null, SR.GetString("Port_not_open"));
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x00101391 File Offset: 0x00100391
		internal static void WrongAsyncResult()
		{
			throw new ArgumentException(SR.GetString("Arg_WrongAsyncResult"));
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x001013A2 File Offset: 0x001003A2
		internal static void EndReadCalledTwice()
		{
			throw new ArgumentException(SR.GetString("InvalidOperation_EndReadCalledMultiple"));
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x001013B3 File Offset: 0x001003B3
		internal static void EndWriteCalledTwice()
		{
			throw new ArgumentException(SR.GetString("InvalidOperation_EndWriteCalledMultiple"));
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x001013C4 File Offset: 0x001003C4
		internal static void WinIOError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			InternalResources.WinIOError(lastWin32Error, string.Empty);
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x001013E4 File Offset: 0x001003E4
		internal static void WinIOError(string str)
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			InternalResources.WinIOError(lastWin32Error, str);
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x00101400 File Offset: 0x00100400
		internal static void WinIOError(int errorCode, string str)
		{
			switch (errorCode)
			{
			case 2:
			case 3:
				if (str.Length == 0)
				{
					throw new IOException(SR.GetString("IO_PortNotFound"));
				}
				throw new IOException(SR.GetString("IO_PortNotFoundFileName", new object[] { str }));
			case 4:
				break;
			case 5:
				if (str.Length == 0)
				{
					throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_NoPathName"));
				}
				throw new UnauthorizedAccessException(SR.GetString("UnauthorizedAccess_IODenied_Path", new object[] { str }));
			default:
				if (errorCode != 32)
				{
					if (errorCode == 206)
					{
						throw new PathTooLongException(SR.GetString("IO_PathTooLong"));
					}
				}
				else
				{
					if (str.Length == 0)
					{
						throw new IOException(SR.GetString("IO_SharingViolation_NoFileName"));
					}
					throw new IOException(SR.GetString("IO_SharingViolation_File", new object[] { str }));
				}
				break;
			}
			throw new IOException(InternalResources.GetMessage(errorCode), InternalResources.MakeHRFromErrorCode(errorCode));
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x001014F9 File Offset: 0x001004F9
		internal static int MakeHRFromErrorCode(int errorCode)
		{
			return -2147024896 | errorCode;
		}
	}
}

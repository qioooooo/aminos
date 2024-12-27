using System;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.Data.SqlTypes
{
	// Token: 0x0200037A RID: 890
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06002F76 RID: 12150
		[DllImport("NtDll.dll", CharSet = CharSet.Unicode)]
		internal static extern uint NtCreateFile(out SafeFileHandle fileHandle, int desiredAccess, ref UnsafeNativeMethods.OBJECT_ATTRIBUTES objectAttributes, out UnsafeNativeMethods.IO_STATUS_BLOCK ioStatusBlock, ref long allocationSize, uint fileAttributes, FileShare shareAccess, uint createDisposition, uint createOptions, SafeHandle eaBuffer, uint eaLength);

		// Token: 0x06002F77 RID: 12151
		[DllImport("Kernel32.dll", SetLastError = true)]
		internal static extern UnsafeNativeMethods.FileType GetFileType(SafeFileHandle hFile);

		// Token: 0x06002F78 RID: 12152
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int GetFullPathName(string path, int numBufferChars, StringBuilder buffer, IntPtr lpFilePartOrNull);

		// Token: 0x06002F79 RID: 12153 RVA: 0x002B0AC4 File Offset: 0x002AFEC4
		internal static string SafeGetFullPathName(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(path.Length + 1);
			int num = UnsafeNativeMethods.GetFullPathName(path, stringBuilder.Capacity, stringBuilder, IntPtr.Zero);
			if (num > stringBuilder.Capacity)
			{
				stringBuilder.Capacity = num;
				num = UnsafeNativeMethods.GetFullPathName(path, stringBuilder.Capacity, stringBuilder, IntPtr.Zero);
			}
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 0)
			{
				throw ADP.Argument(Res.GetString("SqlFileStream_InvalidPath"), "path");
			}
			Win32Exception ex = new Win32Exception(lastWin32Error);
			ADP.TraceExceptionAsReturnValue(ex);
			throw ex;
		}

		// Token: 0x06002F7A RID: 12154
		[DllImport("Kernel32.dll")]
		internal static extern uint SetErrorMode(uint mode);

		// Token: 0x06002F7B RID: 12155
		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool DeviceIoControl(SafeFileHandle fileHandle, uint ioControlCode, IntPtr inBuffer, uint cbInBuffer, IntPtr outBuffer, uint cbOutBuffer, out uint cbBytesReturned, IntPtr overlapped);

		// Token: 0x06002F7C RID: 12156
		[DllImport("NtDll.dll")]
		internal static extern uint RtlNtStatusToDosError(uint status);

		// Token: 0x06002F7D RID: 12157 RVA: 0x002B0B4C File Offset: 0x002AFF4C
		internal static uint CTL_CODE(ushort deviceType, ushort function, byte method, byte access)
		{
			if (function > 4095)
			{
				throw ADP.ArgumentOutOfRange("function");
			}
			return (uint)(((int)deviceType << 16) | ((int)access << 14) | ((int)function << 2) | (int)method);
		}

		// Token: 0x04001D5E RID: 7518
		internal const ushort FILE_DEVICE_FILE_SYSTEM = 9;

		// Token: 0x04001D5F RID: 7519
		internal const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x04001D60 RID: 7520
		internal const int ERROR_MR_MID_NOT_FOUND = 317;

		// Token: 0x04001D61 RID: 7521
		internal const uint STATUS_INVALID_PARAMETER = 3221225485U;

		// Token: 0x04001D62 RID: 7522
		internal const uint STATUS_SHARING_VIOLATION = 3221225539U;

		// Token: 0x04001D63 RID: 7523
		internal const uint STATUS_OBJECT_NAME_NOT_FOUND = 3221225524U;

		// Token: 0x04001D64 RID: 7524
		internal const uint SEM_FAILCRITICALERRORS = 1U;

		// Token: 0x04001D65 RID: 7525
		internal const int FILE_READ_DATA = 1;

		// Token: 0x04001D66 RID: 7526
		internal const int FILE_WRITE_DATA = 2;

		// Token: 0x04001D67 RID: 7527
		internal const int FILE_READ_ATTRIBUTES = 128;

		// Token: 0x04001D68 RID: 7528
		internal const int SYNCHRONIZE = 1048576;

		// Token: 0x0200037B RID: 891
		internal enum FileType : uint
		{
			// Token: 0x04001D6A RID: 7530
			Unknown,
			// Token: 0x04001D6B RID: 7531
			Disk,
			// Token: 0x04001D6C RID: 7532
			Char,
			// Token: 0x04001D6D RID: 7533
			Pipe,
			// Token: 0x04001D6E RID: 7534
			Remote = 32768U
		}

		// Token: 0x0200037C RID: 892
		internal struct OBJECT_ATTRIBUTES
		{
			// Token: 0x04001D6F RID: 7535
			internal int length;

			// Token: 0x04001D70 RID: 7536
			internal IntPtr rootDirectory;

			// Token: 0x04001D71 RID: 7537
			internal SafeHandle objectName;

			// Token: 0x04001D72 RID: 7538
			internal int attributes;

			// Token: 0x04001D73 RID: 7539
			internal IntPtr securityDescriptor;

			// Token: 0x04001D74 RID: 7540
			internal SafeHandle securityQualityOfService;
		}

		// Token: 0x0200037D RID: 893
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct UNICODE_STRING
		{
			// Token: 0x04001D75 RID: 7541
			internal ushort length;

			// Token: 0x04001D76 RID: 7542
			internal ushort maximumLength;

			// Token: 0x04001D77 RID: 7543
			internal string buffer;
		}

		// Token: 0x0200037E RID: 894
		internal enum SecurityImpersonationLevel
		{
			// Token: 0x04001D79 RID: 7545
			SecurityAnonymous,
			// Token: 0x04001D7A RID: 7546
			SecurityIdentification,
			// Token: 0x04001D7B RID: 7547
			SecurityImpersonation,
			// Token: 0x04001D7C RID: 7548
			SecurityDelegation
		}

		// Token: 0x0200037F RID: 895
		internal struct SECURITY_QUALITY_OF_SERVICE
		{
			// Token: 0x04001D7D RID: 7549
			internal uint length;

			// Token: 0x04001D7E RID: 7550
			[MarshalAs(UnmanagedType.I4)]
			internal int impersonationLevel;

			// Token: 0x04001D7F RID: 7551
			internal byte contextDynamicTrackingMode;

			// Token: 0x04001D80 RID: 7552
			internal byte effectiveOnly;
		}

		// Token: 0x02000380 RID: 896
		internal struct IO_STATUS_BLOCK
		{
			// Token: 0x04001D81 RID: 7553
			internal uint status;

			// Token: 0x04001D82 RID: 7554
			internal IntPtr information;
		}

		// Token: 0x02000381 RID: 897
		internal struct FILE_FULL_EA_INFORMATION
		{
			// Token: 0x04001D83 RID: 7555
			internal uint nextEntryOffset;

			// Token: 0x04001D84 RID: 7556
			internal byte flags;

			// Token: 0x04001D85 RID: 7557
			internal byte EaNameLength;

			// Token: 0x04001D86 RID: 7558
			internal ushort EaValueLength;

			// Token: 0x04001D87 RID: 7559
			internal byte EaName;
		}
	}
}

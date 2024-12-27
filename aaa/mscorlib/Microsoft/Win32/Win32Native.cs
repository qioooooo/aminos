using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x02000432 RID: 1074
	[SuppressUnmanagedCodeSecurity]
	internal static class Win32Native
	{
		// Token: 0x06002C22 RID: 11298
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void SetLastError(int errorCode);

		// Token: 0x06002C23 RID: 11299
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetVersionEx([In] [Out] Win32Native.OSVERSIONINFO ver);

		// Token: 0x06002C24 RID: 11300
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetVersionEx([In] [Out] Win32Native.OSVERSIONINFOEX ver);

		// Token: 0x06002C25 RID: 11301
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void GetSystemInfo(ref Win32Native.SYSTEM_INFO lpSystemInfo);

		// Token: 0x06002C26 RID: 11302
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
		internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

		// Token: 0x06002C27 RID: 11303 RVA: 0x00096A04 File Offset: 0x00095A04
		internal static string GetMessage(int errorCode)
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			int num = Win32Native.FormatMessage(12800, Win32Native.NULL, errorCode, 0, stringBuilder, stringBuilder.Capacity, Win32Native.NULL);
			if (num != 0)
			{
				return stringBuilder.ToString();
			}
			return Environment.GetResourceString("UnknownError_Num", new object[] { errorCode });
		}

		// Token: 0x06002C28 RID: 11304
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", EntryPoint = "LocalAlloc")]
		internal static extern IntPtr LocalAlloc_NoSafeHandle(int uFlags, IntPtr sizetdwBytes);

		// Token: 0x06002C29 RID: 11305
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeLocalAllocHandle LocalAlloc([In] int uFlags, [In] IntPtr sizetdwBytes);

		// Token: 0x06002C2A RID: 11306
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x06002C2B RID: 11307
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void ZeroMemory(IntPtr handle, uint length);

		// Token: 0x06002C2C RID: 11308
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GlobalMemoryStatusEx([In] [Out] Win32Native.MEMORYSTATUSEX buffer);

		// Token: 0x06002C2D RID: 11309
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GlobalMemoryStatus([In] [Out] Win32Native.MEMORYSTATUS buffer);

		// Token: 0x06002C2E RID: 11310
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern IntPtr VirtualQuery(void* address, ref Win32Native.MEMORY_BASIC_INFORMATION buffer, IntPtr sizeOfBuffer);

		// Token: 0x06002C2F RID: 11311
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern void* VirtualAlloc(void* address, UIntPtr numBytes, int commitOrReserve, int pageProtectionMode);

		// Token: 0x06002C30 RID: 11312
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern bool VirtualFree(void* address, UIntPtr numBytes, int pageFreeMode);

		// Token: 0x06002C31 RID: 11313
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string methodName);

		// Token: 0x06002C32 RID: 11314
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetModuleHandle(string moduleName);

		// Token: 0x06002C33 RID: 11315 RVA: 0x00096A64 File Offset: 0x00095A64
		[SecurityCritical]
		internal static bool DoesWin32MethodExist(string moduleName, string methodName)
		{
			IntPtr moduleHandle = Win32Native.GetModuleHandle(moduleName);
			if (moduleHandle == IntPtr.Zero)
			{
				return false;
			}
			IntPtr procAddress = Win32Native.GetProcAddress(moduleHandle, methodName);
			return procAddress != IntPtr.Zero;
		}

		// Token: 0x06002C34 RID: 11316
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern uint GetTempPath(int bufferLen, StringBuilder buffer);

		// Token: 0x06002C35 RID: 11317
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern uint GetTempPath2(int bufferLen, StringBuilder buffer);

		// Token: 0x06002C36 RID: 11318
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern IntPtr lstrcpy(IntPtr dst, string src);

		// Token: 0x06002C37 RID: 11319
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern IntPtr lstrcpy(StringBuilder dst, IntPtr src);

		// Token: 0x06002C38 RID: 11320
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int lstrlen(sbyte[] ptr);

		// Token: 0x06002C39 RID: 11321
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int lstrlen(IntPtr ptr);

		// Token: 0x06002C3A RID: 11322
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		internal static extern int lstrlenA(IntPtr ptr);

		// Token: 0x06002C3B RID: 11323
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int lstrlenW(IntPtr ptr);

		// Token: 0x06002C3C RID: 11324
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr SysAllocStringLen(string src, int len);

		// Token: 0x06002C3D RID: 11325
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oleaut32.dll")]
		internal static extern int SysStringLen(IntPtr bstr);

		// Token: 0x06002C3E RID: 11326
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oleaut32.dll")]
		internal static extern void SysFreeString(IntPtr bstr);

		// Token: 0x06002C3F RID: 11327
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory")]
		internal static extern void CopyMemoryUni(IntPtr pdst, string psrc, IntPtr sizetcb);

		// Token: 0x06002C40 RID: 11328
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory")]
		internal static extern void CopyMemoryUni(StringBuilder pdst, IntPtr psrc, IntPtr sizetcb);

		// Token: 0x06002C41 RID: 11329
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory")]
		internal static extern void CopyMemoryAnsi(IntPtr pdst, string psrc, IntPtr sizetcb);

		// Token: 0x06002C42 RID: 11330
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "RtlMoveMemory")]
		internal static extern void CopyMemoryAnsi(StringBuilder pdst, IntPtr psrc, IntPtr sizetcb);

		// Token: 0x06002C43 RID: 11331
		[DllImport("kernel32.dll")]
		internal static extern int GetACP();

		// Token: 0x06002C44 RID: 11332
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetEvent(SafeWaitHandle handle);

		// Token: 0x06002C45 RID: 11333
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool ResetEvent(SafeWaitHandle handle);

		// Token: 0x06002C46 RID: 11334
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern uint WaitForMultipleObjects(uint nCount, IntPtr[] handles, bool bWaitAll, uint dwMilliseconds);

		// Token: 0x06002C47 RID: 11335
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeWaitHandle CreateEvent(Win32Native.SECURITY_ATTRIBUTES lpSecurityAttributes, bool isManualReset, bool initialState, string name);

		// Token: 0x06002C48 RID: 11336
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeWaitHandle OpenEvent(int desiredAccess, bool inheritHandle, string name);

		// Token: 0x06002C49 RID: 11337
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeWaitHandle CreateMutex(Win32Native.SECURITY_ATTRIBUTES lpSecurityAttributes, bool initialOwner, string name);

		// Token: 0x06002C4A RID: 11338
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeWaitHandle OpenMutex(int desiredAccess, bool inheritHandle, string name);

		// Token: 0x06002C4B RID: 11339
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool ReleaseMutex(SafeWaitHandle handle);

		// Token: 0x06002C4C RID: 11340
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetFullPathName([In] char[] path, int numBufferChars, [Out] char[] buffer, IntPtr mustBeZero);

		// Token: 0x06002C4D RID: 11341
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal unsafe static extern int GetFullPathName(char* path, int numBufferChars, char* buffer, IntPtr mustBeZero);

		// Token: 0x06002C4E RID: 11342
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetLongPathName(string path, StringBuilder longPathBuffer, int bufferLength);

		// Token: 0x06002C4F RID: 11343
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetLongPathName([In] char[] path, [Out] char[] longPathBuffer, int bufferLength);

		// Token: 0x06002C50 RID: 11344
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal unsafe static extern int GetLongPathName(char* path, char* longPathBuffer, int bufferLength);

		// Token: 0x06002C51 RID: 11345 RVA: 0x00096A9C File Offset: 0x00095A9C
		internal static SafeFileHandle SafeCreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, Win32Native.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile)
		{
			SafeFileHandle safeFileHandle = Win32Native.CreateFile(lpFileName, dwDesiredAccess, dwShareMode, securityAttrs, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
			if (!safeFileHandle.IsInvalid)
			{
				int fileType = Win32Native.GetFileType(safeFileHandle);
				if (fileType != 1)
				{
					safeFileHandle.Dispose();
					throw new NotSupportedException(Environment.GetResourceString("NotSupported_FileStreamOnNonFiles"));
				}
			}
			return safeFileHandle;
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00096AE4 File Offset: 0x00095AE4
		internal static SafeFileHandle UnsafeCreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, Win32Native.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile)
		{
			return Win32Native.CreateFile(lpFileName, dwDesiredAccess, dwShareMode, securityAttrs, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile);
		}

		// Token: 0x06002C53 RID: 11347
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, FileShare dwShareMode, Win32Native.SECURITY_ATTRIBUTES securityAttrs, FileMode dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06002C54 RID: 11348
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileMappingHandle CreateFileMapping(SafeFileHandle hFile, IntPtr lpAttributes, uint fProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

		// Token: 0x06002C55 RID: 11349
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr MapViewOfFile(SafeFileMappingHandle handle, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, UIntPtr dwNumerOfBytesToMap);

		// Token: 0x06002C56 RID: 11350
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

		// Token: 0x06002C57 RID: 11351
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06002C58 RID: 11352
		[DllImport("kernel32.dll")]
		internal static extern int GetFileType(SafeFileHandle handle);

		// Token: 0x06002C59 RID: 11353
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetEndOfFile(SafeFileHandle hFile);

		// Token: 0x06002C5A RID: 11354
		[DllImport("kernel32.dll", EntryPoint = "SetFilePointer", SetLastError = true)]
		private unsafe static extern int SetFilePointerWin32(SafeFileHandle handle, int lo, int* hi, int origin);

		// Token: 0x06002C5B RID: 11355 RVA: 0x00096B04 File Offset: 0x00095B04
		internal unsafe static long SetFilePointer(SafeFileHandle handle, long offset, SeekOrigin origin, out int hr)
		{
			hr = 0;
			int num = (int)offset;
			int num2 = (int)(offset >> 32);
			num = Win32Native.SetFilePointerWin32(handle, num, &num2, (int)origin);
			if (num == -1 && (hr = Marshal.GetLastWin32Error()) != 0)
			{
				return -1L;
			}
			return (long)(((ulong)num2 << 32) | (ulong)num);
		}

		// Token: 0x06002C5C RID: 11356
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, IntPtr numBytesRead_mustBeZero, NativeOverlapped* overlapped);

		// Token: 0x06002C5D RID: 11357
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr mustBeZero);

		// Token: 0x06002C5E RID: 11358
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, IntPtr numBytesWritten_mustBeZero, NativeOverlapped* lpOverlapped);

		// Token: 0x06002C5F RID: 11359
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero);

		// Token: 0x06002C60 RID: 11360
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetDiskFreeSpaceEx(string drive, out long freeBytesForUser, out long totalBytes, out long freeBytes);

		// Token: 0x06002C61 RID: 11361
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetDriveType(string drive);

		// Token: 0x06002C62 RID: 11362
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetVolumeInformation(string drive, StringBuilder volumeName, int volumeNameBufLen, out int volSerialNumber, out int maxFileNameLen, out int fileSystemFlags, StringBuilder fileSystemName, int fileSystemNameBufLen);

		// Token: 0x06002C63 RID: 11363
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetVolumeLabel(string driveLetter, string volumeName);

		// Token: 0x06002C64 RID: 11364
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetWindowsDirectory(StringBuilder sb, int length);

		// Token: 0x06002C65 RID: 11365
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal unsafe static extern int LCMapStringW(int lcid, int flags, char* src, int cchSrc, char* target, int cchTarget);

		// Token: 0x06002C66 RID: 11366
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal unsafe static extern int FindNLSString(int Locale, int dwFindFlags, char* lpStringSource, int cchSource, char* lpStringValue, int cchValue, IntPtr pcchFound);

		// Token: 0x06002C67 RID: 11367
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetSystemDirectory(StringBuilder sb, int length);

		// Token: 0x06002C68 RID: 11368
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern bool SetFileTime(SafeFileHandle hFile, Win32Native.FILE_TIME* creationTime, Win32Native.FILE_TIME* lastAccessTime, Win32Native.FILE_TIME* lastWriteTime);

		// Token: 0x06002C69 RID: 11369
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int GetFileSize(SafeFileHandle hFile, out int highSize);

		// Token: 0x06002C6A RID: 11370
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool LockFile(SafeFileHandle handle, int offsetLow, int offsetHigh, int countLow, int countHigh);

		// Token: 0x06002C6B RID: 11371
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool UnlockFile(SafeFileHandle handle, int offsetLow, int offsetHigh, int countLow, int countHigh);

		// Token: 0x06002C6C RID: 11372
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr GetStdHandle(int nStdHandle);

		// Token: 0x06002C6D RID: 11373 RVA: 0x00096B44 File Offset: 0x00095B44
		internal static int MakeHRFromErrorCode(int errorCode)
		{
			return -2147024896 | errorCode;
		}

		// Token: 0x06002C6E RID: 11374
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CopyFile(string src, string dst, bool failIfExists);

		// Token: 0x06002C6F RID: 11375
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool CreateDirectory(string path, Win32Native.SECURITY_ATTRIBUTES lpSecurityAttributes);

		// Token: 0x06002C70 RID: 11376
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DeleteFile(string path);

		// Token: 0x06002C71 RID: 11377
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ReplaceFile(string replacedFileName, string replacementFileName, string backupFileName, int dwReplaceFlags, IntPtr lpExclude, IntPtr lpReserved);

		// Token: 0x06002C72 RID: 11378
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DecryptFile(string path, int reservedMustBeZero);

		// Token: 0x06002C73 RID: 11379
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool EncryptFile(string path);

		// Token: 0x06002C74 RID: 11380
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFindHandle FindFirstFile(string fileName, [In] [Out] Win32Native.WIN32_FIND_DATA data);

		// Token: 0x06002C75 RID: 11381
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool FindNextFile(SafeFindHandle hndFindFile, [MarshalAs(UnmanagedType.LPStruct)] [In] [Out] Win32Native.WIN32_FIND_DATA lpFindFileData);

		// Token: 0x06002C76 RID: 11382
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll")]
		internal static extern bool FindClose(IntPtr handle);

		// Token: 0x06002C77 RID: 11383
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetCurrentDirectory(int nBufferLength, StringBuilder lpBuffer);

		// Token: 0x06002C78 RID: 11384
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetFileAttributesEx(string name, int fileInfoLevel, ref Win32Native.WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);

		// Token: 0x06002C79 RID: 11385
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetFileAttributes(string name, int attr);

		// Token: 0x06002C7A RID: 11386
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int GetLogicalDrives();

		// Token: 0x06002C7B RID: 11387
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint GetTempFileName(string tmpPath, string prefix, uint uniqueIdOrZero, StringBuilder tmpFileName);

		// Token: 0x06002C7C RID: 11388
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool MoveFile(string src, string dst);

		// Token: 0x06002C7D RID: 11389
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DeleteVolumeMountPoint(string mountPoint);

		// Token: 0x06002C7E RID: 11390
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool RemoveDirectory(string path);

		// Token: 0x06002C7F RID: 11391
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetCurrentDirectory(string path);

		// Token: 0x06002C80 RID: 11392
		[DllImport("kernel32.dll")]
		internal static extern int SetErrorMode(int newMode);

		// Token: 0x06002C81 RID: 11393
		[DllImport("kernel32.dll")]
		internal unsafe static extern int WideCharToMultiByte(uint cp, uint flags, char* pwzSource, int cchSource, byte* pbDestBuffer, int cbDestBuffer, IntPtr null1, IntPtr null2);

		// Token: 0x06002C82 RID: 11394
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleCtrlHandler(Win32Native.ConsoleCtrlHandlerRoutine handler, bool addOrRemove);

		// Token: 0x06002C83 RID: 11395
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetEnvironmentVariable(string lpName, string lpValue);

		// Token: 0x06002C84 RID: 11396
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetEnvironmentVariable(string lpName, StringBuilder lpValue, int size);

		// Token: 0x06002C85 RID: 11397
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern uint GetCurrentProcessId();

		// Token: 0x06002C86 RID: 11398
		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		internal static extern bool GetUserName(StringBuilder lpBuffer, ref int nSize);

		// Token: 0x06002C87 RID: 11399
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int GetComputerName(StringBuilder nameBuffer, ref int bufferSize);

		// Token: 0x06002C88 RID: 11400
		[DllImport("ole32.dll")]
		internal static extern IntPtr CoTaskMemAlloc(int cb);

		// Token: 0x06002C89 RID: 11401
		[DllImport("ole32.dll")]
		internal static extern IntPtr CoTaskMemRealloc(IntPtr pv, int cb);

		// Token: 0x06002C8A RID: 11402
		[DllImport("ole32.dll")]
		internal static extern void CoTaskMemFree(IntPtr ptr);

		// Token: 0x06002C8B RID: 11403
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

		// Token: 0x06002C8C RID: 11404
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int mode);

		// Token: 0x06002C8D RID: 11405
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool Beep(int frequency, int duration);

		// Token: 0x06002C8E RID: 11406
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out Win32Native.CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

		// Token: 0x06002C8F RID: 11407
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, Win32Native.COORD size);

		// Token: 0x06002C90 RID: 11408
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern Win32Native.COORD GetLargestConsoleWindowSize(IntPtr hConsoleOutput);

		// Token: 0x06002C91 RID: 11409
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool FillConsoleOutputCharacter(IntPtr hConsoleOutput, char character, int nLength, Win32Native.COORD dwWriteCoord, out int pNumCharsWritten);

		// Token: 0x06002C92 RID: 11410
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool FillConsoleOutputAttribute(IntPtr hConsoleOutput, short wColorAttribute, int numCells, Win32Native.COORD startCoord, out int pNumBytesWritten);

		// Token: 0x06002C93 RID: 11411
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput, bool absolute, Win32Native.SMALL_RECT* consoleWindow);

		// Token: 0x06002C94 RID: 11412
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, short attributes);

		// Token: 0x06002C95 RID: 11413
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, Win32Native.COORD cursorPosition);

		// Token: 0x06002C96 RID: 11414
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool GetConsoleCursorInfo(IntPtr hConsoleOutput, out Win32Native.CONSOLE_CURSOR_INFO cci);

		// Token: 0x06002C97 RID: 11415
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, ref Win32Native.CONSOLE_CURSOR_INFO cci);

		// Token: 0x06002C98 RID: 11416
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetConsoleTitle(StringBuilder sb, int capacity);

		// Token: 0x06002C99 RID: 11417
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetConsoleTitle(string title);

		// Token: 0x06002C9A RID: 11418
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool ReadConsoleInput(IntPtr hConsoleInput, out Win32Native.InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

		// Token: 0x06002C9B RID: 11419
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool PeekConsoleInput(IntPtr hConsoleInput, out Win32Native.InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

		// Token: 0x06002C9C RID: 11420
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern bool ReadConsoleOutput(IntPtr hConsoleOutput, Win32Native.CHAR_INFO* pBuffer, Win32Native.COORD bufferSize, Win32Native.COORD bufferCoord, ref Win32Native.SMALL_RECT readRegion);

		// Token: 0x06002C9D RID: 11421
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern bool WriteConsoleOutput(IntPtr hConsoleOutput, Win32Native.CHAR_INFO* buffer, Win32Native.COORD bufferSize, Win32Native.COORD bufferCoord, ref Win32Native.SMALL_RECT writeRegion);

		// Token: 0x06002C9E RID: 11422
		[DllImport("user32.dll")]
		internal static extern short GetKeyState(int virtualKeyCode);

		// Token: 0x06002C9F RID: 11423
		[DllImport("kernel32.dll")]
		internal static extern uint GetConsoleCP();

		// Token: 0x06002CA0 RID: 11424
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleCP(uint codePage);

		// Token: 0x06002CA1 RID: 11425
		[DllImport("kernel32.dll")]
		internal static extern uint GetConsoleOutputCP();

		// Token: 0x06002CA2 RID: 11426
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool SetConsoleOutputCP(uint codePage);

		// Token: 0x06002CA3 RID: 11427
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegConnectRegistry(string machineName, SafeRegistryHandle key, out SafeRegistryHandle result);

		// Token: 0x06002CA4 RID: 11428
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegCreateKeyEx(SafeRegistryHandle hKey, string lpSubKey, int Reserved, string lpClass, int dwOptions, int samDesigner, Win32Native.SECURITY_ATTRIBUTES lpSecurityAttributes, out SafeRegistryHandle hkResult, out int lpdwDisposition);

		// Token: 0x06002CA5 RID: 11429
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegDeleteKey(SafeRegistryHandle hKey, string lpSubKey);

		// Token: 0x06002CA6 RID: 11430
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegDeleteValue(SafeRegistryHandle hKey, string lpValueName);

		// Token: 0x06002CA7 RID: 11431
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegEnumKeyEx(SafeRegistryHandle hKey, int dwIndex, StringBuilder lpName, out int lpcbName, int[] lpReserved, StringBuilder lpClass, int[] lpcbClass, long[] lpftLastWriteTime);

		// Token: 0x06002CA8 RID: 11432
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegEnumValue(SafeRegistryHandle hKey, int dwIndex, StringBuilder lpValueName, ref int lpcbValueName, IntPtr lpReserved_MustBeZero, int[] lpType, byte[] lpData, int[] lpcbData);

		// Token: 0x06002CA9 RID: 11433
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern int RegEnumValueA(SafeRegistryHandle hKey, int dwIndex, StringBuilder lpValueName, ref int lpcbValueName, IntPtr lpReserved_MustBeZero, int[] lpType, byte[] lpData, int[] lpcbData);

		// Token: 0x06002CAA RID: 11434
		[DllImport("advapi32.dll")]
		internal static extern int RegFlushKey(SafeRegistryHandle hKey);

		// Token: 0x06002CAB RID: 11435
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegOpenKeyEx(SafeRegistryHandle hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle hkResult);

		// Token: 0x06002CAC RID: 11436
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryInfoKey(SafeRegistryHandle hKey, StringBuilder lpClass, int[] lpcbClass, IntPtr lpReserved_MustBeZero, ref int lpcSubKeys, int[] lpcbMaxSubKeyLen, int[] lpcbMaxClassLen, ref int lpcValues, int[] lpcbMaxValueNameLen, int[] lpcbMaxValueLen, int[] lpcbSecurityDescriptor, int[] lpftLastWriteTime);

		// Token: 0x06002CAD RID: 11437
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] byte[] lpData, ref int lpcbData);

		// Token: 0x06002CAE RID: 11438
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, ref int lpData, ref int lpcbData);

		// Token: 0x06002CAF RID: 11439
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, ref long lpData, ref int lpcbData);

		// Token: 0x06002CB0 RID: 11440
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] char[] lpData, ref int lpcbData);

		// Token: 0x06002CB1 RID: 11441
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegQueryValueEx(SafeRegistryHandle hKey, string lpValueName, int[] lpReserved, ref int lpType, StringBuilder lpData, ref int lpcbData);

		// Token: 0x06002CB2 RID: 11442
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, byte[] lpData, int cbData);

		// Token: 0x06002CB3 RID: 11443
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, ref int lpData, int cbData);

		// Token: 0x06002CB4 RID: 11444
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, ref long lpData, int cbData);

		// Token: 0x06002CB5 RID: 11445
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int RegSetValueEx(SafeRegistryHandle hKey, string lpValueName, int Reserved, RegistryValueKind dwType, string lpData, int cbData);

		// Token: 0x06002CB6 RID: 11446
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int ExpandEnvironmentStrings(string lpSrc, StringBuilder lpDst, int nSize);

		// Token: 0x06002CB7 RID: 11447
		[DllImport("kernel32.dll")]
		internal static extern IntPtr LocalReAlloc(IntPtr handle, IntPtr sizetcbBytes, int uFlags);

		// Token: 0x06002CB8 RID: 11448
		[DllImport("shfolder.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

		// Token: 0x06002CB9 RID: 11449
		[DllImport("secur32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern byte GetUserNameEx(int format, StringBuilder domainName, ref int domainNameLen);

		// Token: 0x06002CBA RID: 11450
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool LookupAccountName(string machineName, string accountName, byte[] sid, ref int sidLen, StringBuilder domainName, ref int domainNameLen, out int peUse);

		// Token: 0x06002CBB RID: 11451
		[DllImport("user32.dll", ExactSpelling = true)]
		internal static extern IntPtr GetProcessWindowStation();

		// Token: 0x06002CBC RID: 11452
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetUserObjectInformation(IntPtr hObj, int nIndex, [MarshalAs(UnmanagedType.LPStruct)] Win32Native.USEROBJECTFLAGS pvBuffer, int nLength, ref int lpnLengthNeeded);

		// Token: 0x06002CBD RID: 11453
		[DllImport("user32.dll", BestFitMapping = false, SetLastError = true)]
		internal static extern IntPtr SendMessageTimeout(IntPtr hWnd, int Msg, IntPtr wParam, string lParam, uint fuFlags, uint uTimeout, IntPtr lpdwResult);

		// Token: 0x06002CBE RID: 11454
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SystemFunction040([In] [Out] SafeBSTRHandle pDataIn, [In] uint cbDataIn, [In] uint dwFlags);

		// Token: 0x06002CBF RID: 11455
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SystemFunction041([In] [Out] SafeBSTRHandle pDataIn, [In] uint cbDataIn, [In] uint dwFlags);

		// Token: 0x06002CC0 RID: 11456
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int LsaNtStatusToWinError([In] int status);

		// Token: 0x06002CC1 RID: 11457
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptGetFipsAlgorithmMode([MarshalAs(UnmanagedType.U1)] out bool pfEnabled);

		// Token: 0x06002CC2 RID: 11458
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool AdjustTokenPrivileges([In] SafeTokenHandle TokenHandle, [In] bool DisableAllPrivileges, [In] ref Win32Native.TOKEN_PRIVILEGE NewState, [In] uint BufferLength, [In] [Out] ref Win32Native.TOKEN_PRIVILEGE PreviousState, [In] [Out] ref uint ReturnLength);

		// Token: 0x06002CC3 RID: 11459
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool AllocateLocallyUniqueId([In] [Out] ref Win32Native.LUID Luid);

		// Token: 0x06002CC4 RID: 11460
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CheckTokenMembership([In] SafeTokenHandle TokenHandle, [In] byte[] SidToCheck, [In] [Out] ref bool IsMember);

		// Token: 0x06002CC5 RID: 11461
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "ConvertSecurityDescriptorToStringSecurityDescriptorW", SetLastError = true)]
		internal static extern int ConvertSdToStringSd(byte[] securityDescriptor, uint requestedRevision, uint securityInformation, out IntPtr resultString, ref uint resultStringLength);

		// Token: 0x06002CC6 RID: 11462
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "ConvertStringSecurityDescriptorToSecurityDescriptorW", SetLastError = true)]
		internal static extern int ConvertStringSdToSd(string stringSd, uint stringSdRevision, out IntPtr resultSd, ref uint resultSdLength);

		// Token: 0x06002CC7 RID: 11463
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "ConvertStringSidToSidW", SetLastError = true)]
		internal static extern int ConvertStringSidToSid(string stringSid, out IntPtr ByteArray);

		// Token: 0x06002CC8 RID: 11464
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int CreateWellKnownSid(int sidType, byte[] domainSid, [Out] byte[] resultSid, ref uint resultSidLength);

		// Token: 0x06002CC9 RID: 11465
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DuplicateHandle([In] IntPtr hSourceProcessHandle, [In] IntPtr hSourceHandle, [In] IntPtr hTargetProcessHandle, [In] [Out] ref SafeTokenHandle lpTargetHandle, [In] uint dwDesiredAccess, [In] bool bInheritHandle, [In] uint dwOptions);

		// Token: 0x06002CCA RID: 11466
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DuplicateHandle([In] IntPtr hSourceProcessHandle, [In] SafeTokenHandle hSourceHandle, [In] IntPtr hTargetProcessHandle, [In] [Out] ref SafeTokenHandle lpTargetHandle, [In] uint dwDesiredAccess, [In] bool bInheritHandle, [In] uint dwOptions);

		// Token: 0x06002CCB RID: 11467
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DuplicateTokenEx([In] SafeTokenHandle ExistingTokenHandle, [In] TokenAccessLevels DesiredAccess, [In] IntPtr TokenAttributes, [In] Win32Native.SECURITY_IMPERSONATION_LEVEL ImpersonationLevel, [In] global::System.Security.Principal.TokenType TokenType, [In] [Out] ref SafeTokenHandle DuplicateTokenHandle);

		// Token: 0x06002CCC RID: 11468
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DuplicateTokenEx([In] SafeTokenHandle hExistingToken, [In] uint dwDesiredAccess, [In] IntPtr lpTokenAttributes, [In] uint ImpersonationLevel, [In] uint TokenType, [In] [Out] ref SafeTokenHandle phNewToken);

		// Token: 0x06002CCD RID: 11469
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "EqualDomainSid", SetLastError = true)]
		internal static extern int IsEqualDomainSid(byte[] sid1, byte[] sid2, out bool result);

		// Token: 0x06002CCE RID: 11470
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetCurrentProcess();

		// Token: 0x06002CCF RID: 11471
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint GetSecurityDescriptorLength(IntPtr byteArray);

		// Token: 0x06002CD0 RID: 11472
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetSecurityInfo", SetLastError = true)]
		internal static extern uint GetSecurityInfoByHandle(SafeHandle handle, uint objectType, uint securityInformation, out IntPtr sidOwner, out IntPtr sidGroup, out IntPtr dacl, out IntPtr sacl, out IntPtr securityDescriptor);

		// Token: 0x06002CD1 RID: 11473
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetNamedSecurityInfoW", SetLastError = true)]
		internal static extern uint GetSecurityInfoByName(string name, uint objectType, uint securityInformation, out IntPtr sidOwner, out IntPtr sidGroup, out IntPtr dacl, out IntPtr sacl, out IntPtr securityDescriptor);

		// Token: 0x06002CD2 RID: 11474
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetTokenInformation([In] IntPtr TokenHandle, [In] uint TokenInformationClass, [In] SafeLocalAllocHandle TokenInformation, [In] uint TokenInformationLength, out uint ReturnLength);

		// Token: 0x06002CD3 RID: 11475
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetTokenInformation([In] SafeTokenHandle TokenHandle, [In] uint TokenInformationClass, [In] SafeLocalAllocHandle TokenInformation, [In] uint TokenInformationLength, out uint ReturnLength);

		// Token: 0x06002CD4 RID: 11476
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int GetWindowsAccountDomainSid(byte[] sid, [Out] byte[] resultSid, ref uint resultSidLength);

		// Token: 0x06002CD5 RID: 11477
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int IsWellKnownSid(byte[] sid, int type);

		// Token: 0x06002CD6 RID: 11478
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint LsaOpenPolicy(string systemName, ref Win32Native.LSA_OBJECT_ATTRIBUTES attributes, int accessMask, out SafeLsaPolicyHandle handle);

		// Token: 0x06002CD7 RID: 11479
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, EntryPoint = "LookupPrivilegeValueW", SetLastError = true)]
		internal static extern bool LookupPrivilegeValue([In] string lpSystemName, [In] string lpName, [In] [Out] ref Win32Native.LUID Luid);

		// Token: 0x06002CD8 RID: 11480
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint LsaLookupSids(SafeLsaPolicyHandle handle, int count, IntPtr[] sids, ref SafeLsaMemoryHandle referencedDomains, ref SafeLsaMemoryHandle names);

		// Token: 0x06002CD9 RID: 11481
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern int LsaFreeMemory(IntPtr handle);

		// Token: 0x06002CDA RID: 11482
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint LsaLookupNames(SafeLsaPolicyHandle handle, int count, Win32Native.UNICODE_STRING[] names, ref SafeLsaMemoryHandle referencedDomains, ref SafeLsaMemoryHandle sids);

		// Token: 0x06002CDB RID: 11483
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint LsaLookupNames2(SafeLsaPolicyHandle handle, int flags, int count, Win32Native.UNICODE_STRING[] names, ref SafeLsaMemoryHandle referencedDomains, ref SafeLsaMemoryHandle sids);

		// Token: 0x06002CDC RID: 11484
		[DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int LsaConnectUntrusted([In] [Out] ref SafeLsaLogonProcessHandle LsaHandle);

		// Token: 0x06002CDD RID: 11485
		[DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int LsaGetLogonSessionData([In] ref Win32Native.LUID LogonId, [In] [Out] ref SafeLsaReturnBufferHandle ppLogonSessionData);

		// Token: 0x06002CDE RID: 11486
		[DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int LsaLogonUser([In] SafeLsaLogonProcessHandle LsaHandle, [In] ref Win32Native.UNICODE_INTPTR_STRING OriginName, [In] uint LogonType, [In] uint AuthenticationPackage, [In] IntPtr AuthenticationInformation, [In] uint AuthenticationInformationLength, [In] IntPtr LocalGroups, [In] ref Win32Native.TOKEN_SOURCE SourceContext, [In] [Out] ref SafeLsaReturnBufferHandle ProfileBuffer, [In] [Out] ref uint ProfileBufferLength, [In] [Out] ref Win32Native.LUID LogonId, [In] [Out] ref SafeTokenHandle Token, [In] [Out] ref Win32Native.QUOTA_LIMITS Quotas, [In] [Out] ref int SubStatus);

		// Token: 0x06002CDF RID: 11487
		[DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int LsaLookupAuthenticationPackage([In] SafeLsaLogonProcessHandle LsaHandle, [In] ref Win32Native.UNICODE_INTPTR_STRING PackageName, [In] [Out] ref uint AuthenticationPackage);

		// Token: 0x06002CE0 RID: 11488
		[DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int LsaRegisterLogonProcess([In] ref Win32Native.UNICODE_INTPTR_STRING LogonProcessName, [In] [Out] ref SafeLsaLogonProcessHandle LsaHandle, [In] [Out] ref IntPtr SecurityMode);

		// Token: 0x06002CE1 RID: 11489
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("secur32.dll", SetLastError = true)]
		internal static extern int LsaDeregisterLogonProcess(IntPtr handle);

		// Token: 0x06002CE2 RID: 11490
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		internal static extern int LsaClose(IntPtr handle);

		// Token: 0x06002CE3 RID: 11491
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("secur32.dll", SetLastError = true)]
		internal static extern int LsaFreeReturnBuffer(IntPtr handle);

		// Token: 0x06002CE4 RID: 11492
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool OpenProcessToken([In] IntPtr ProcessToken, [In] TokenAccessLevels DesiredAccess, [In] [Out] ref SafeTokenHandle TokenHandle);

		// Token: 0x06002CE5 RID: 11493
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetNamedSecurityInfoW", SetLastError = true)]
		internal static extern uint SetSecurityInfoByName(string name, uint objectType, uint securityInformation, byte[] owner, byte[] group, byte[] dacl, byte[] sacl);

		// Token: 0x06002CE6 RID: 11494
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "SetSecurityInfo", SetLastError = true)]
		internal static extern uint SetSecurityInfoByHandle(SafeHandle handle, uint objectType, uint securityInformation, byte[] owner, byte[] group, byte[] dacl, byte[] sacl);

		// Token: 0x06002CE7 RID: 11495
		[DllImport("mscorwks.dll", CharSet = CharSet.Unicode)]
		internal static extern int CreateAssemblyNameObject(out IAssemblyName ppEnum, string szAssemblyName, uint dwFlags, IntPtr pvReserved);

		// Token: 0x06002CE8 RID: 11496
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto)]
		internal static extern int CreateAssemblyEnum(out IAssemblyEnum ppEnum, IApplicationContext pAppCtx, IAssemblyName pName, uint dwFlags, IntPtr pvReserved);

		// Token: 0x06002CE9 RID: 11497
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern int GetCalendarInfo(int Locale, int Calendar, int CalType, StringBuilder lpCalData, int cchData, IntPtr lpValue);

		// Token: 0x04001586 RID: 5510
		internal const int KEY_QUERY_VALUE = 1;

		// Token: 0x04001587 RID: 5511
		internal const int KEY_SET_VALUE = 2;

		// Token: 0x04001588 RID: 5512
		internal const int KEY_CREATE_SUB_KEY = 4;

		// Token: 0x04001589 RID: 5513
		internal const int KEY_ENUMERATE_SUB_KEYS = 8;

		// Token: 0x0400158A RID: 5514
		internal const int KEY_NOTIFY = 16;

		// Token: 0x0400158B RID: 5515
		internal const int KEY_CREATE_LINK = 32;

		// Token: 0x0400158C RID: 5516
		internal const int KEY_READ = 131097;

		// Token: 0x0400158D RID: 5517
		internal const int KEY_WRITE = 131078;

		// Token: 0x0400158E RID: 5518
		internal const int REG_NONE = 0;

		// Token: 0x0400158F RID: 5519
		internal const int REG_SZ = 1;

		// Token: 0x04001590 RID: 5520
		internal const int REG_EXPAND_SZ = 2;

		// Token: 0x04001591 RID: 5521
		internal const int REG_BINARY = 3;

		// Token: 0x04001592 RID: 5522
		internal const int REG_DWORD = 4;

		// Token: 0x04001593 RID: 5523
		internal const int REG_DWORD_LITTLE_ENDIAN = 4;

		// Token: 0x04001594 RID: 5524
		internal const int REG_DWORD_BIG_ENDIAN = 5;

		// Token: 0x04001595 RID: 5525
		internal const int REG_LINK = 6;

		// Token: 0x04001596 RID: 5526
		internal const int REG_MULTI_SZ = 7;

		// Token: 0x04001597 RID: 5527
		internal const int REG_RESOURCE_LIST = 8;

		// Token: 0x04001598 RID: 5528
		internal const int REG_FULL_RESOURCE_DESCRIPTOR = 9;

		// Token: 0x04001599 RID: 5529
		internal const int REG_RESOURCE_REQUIREMENTS_LIST = 10;

		// Token: 0x0400159A RID: 5530
		internal const int REG_QWORD = 11;

		// Token: 0x0400159B RID: 5531
		internal const int HWND_BROADCAST = 65535;

		// Token: 0x0400159C RID: 5532
		internal const int WM_SETTINGCHANGE = 26;

		// Token: 0x0400159D RID: 5533
		internal const uint CRYPTPROTECTMEMORY_BLOCK_SIZE = 16U;

		// Token: 0x0400159E RID: 5534
		internal const uint CRYPTPROTECTMEMORY_SAME_PROCESS = 0U;

		// Token: 0x0400159F RID: 5535
		internal const uint CRYPTPROTECTMEMORY_CROSS_PROCESS = 1U;

		// Token: 0x040015A0 RID: 5536
		internal const uint CRYPTPROTECTMEMORY_SAME_LOGON = 2U;

		// Token: 0x040015A1 RID: 5537
		internal const int SECURITY_ANONYMOUS = 0;

		// Token: 0x040015A2 RID: 5538
		internal const int SECURITY_SQOS_PRESENT = 1048576;

		// Token: 0x040015A3 RID: 5539
		internal const string MICROSOFT_KERBEROS_NAME = "Kerberos";

		// Token: 0x040015A4 RID: 5540
		internal const uint ANONYMOUS_LOGON_LUID = 998U;

		// Token: 0x040015A5 RID: 5541
		internal const int SECURITY_ANONYMOUS_LOGON_RID = 7;

		// Token: 0x040015A6 RID: 5542
		internal const int SECURITY_AUTHENTICATED_USER_RID = 11;

		// Token: 0x040015A7 RID: 5543
		internal const int SECURITY_LOCAL_SYSTEM_RID = 18;

		// Token: 0x040015A8 RID: 5544
		internal const int SECURITY_BUILTIN_DOMAIN_RID = 32;

		// Token: 0x040015A9 RID: 5545
		internal const int DOMAIN_USER_RID_GUEST = 501;

		// Token: 0x040015AA RID: 5546
		internal const uint SE_PRIVILEGE_DISABLED = 0U;

		// Token: 0x040015AB RID: 5547
		internal const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 1U;

		// Token: 0x040015AC RID: 5548
		internal const uint SE_PRIVILEGE_ENABLED = 2U;

		// Token: 0x040015AD RID: 5549
		internal const uint SE_PRIVILEGE_USED_FOR_ACCESS = 2147483648U;

		// Token: 0x040015AE RID: 5550
		internal const uint SE_GROUP_MANDATORY = 1U;

		// Token: 0x040015AF RID: 5551
		internal const uint SE_GROUP_ENABLED_BY_DEFAULT = 2U;

		// Token: 0x040015B0 RID: 5552
		internal const uint SE_GROUP_ENABLED = 4U;

		// Token: 0x040015B1 RID: 5553
		internal const uint SE_GROUP_OWNER = 8U;

		// Token: 0x040015B2 RID: 5554
		internal const uint SE_GROUP_USE_FOR_DENY_ONLY = 16U;

		// Token: 0x040015B3 RID: 5555
		internal const uint SE_GROUP_LOGON_ID = 3221225472U;

		// Token: 0x040015B4 RID: 5556
		internal const uint SE_GROUP_RESOURCE = 536870912U;

		// Token: 0x040015B5 RID: 5557
		internal const uint DUPLICATE_CLOSE_SOURCE = 1U;

		// Token: 0x040015B6 RID: 5558
		internal const uint DUPLICATE_SAME_ACCESS = 2U;

		// Token: 0x040015B7 RID: 5559
		internal const uint DUPLICATE_SAME_ATTRIBUTES = 4U;

		// Token: 0x040015B8 RID: 5560
		internal const int READ_CONTROL = 131072;

		// Token: 0x040015B9 RID: 5561
		internal const int SYNCHRONIZE = 1048576;

		// Token: 0x040015BA RID: 5562
		internal const int STANDARD_RIGHTS_READ = 131072;

		// Token: 0x040015BB RID: 5563
		internal const int STANDARD_RIGHTS_WRITE = 131072;

		// Token: 0x040015BC RID: 5564
		internal const int SEMAPHORE_MODIFY_STATE = 2;

		// Token: 0x040015BD RID: 5565
		internal const int EVENT_MODIFY_STATE = 2;

		// Token: 0x040015BE RID: 5566
		internal const int MUTEX_MODIFY_STATE = 1;

		// Token: 0x040015BF RID: 5567
		internal const int MUTEX_ALL_ACCESS = 2031617;

		// Token: 0x040015C0 RID: 5568
		internal const int LMEM_FIXED = 0;

		// Token: 0x040015C1 RID: 5569
		internal const int LMEM_ZEROINIT = 64;

		// Token: 0x040015C2 RID: 5570
		internal const int LPTR = 64;

		// Token: 0x040015C3 RID: 5571
		internal const string KERNEL32 = "kernel32.dll";

		// Token: 0x040015C4 RID: 5572
		internal const string USER32 = "user32.dll";

		// Token: 0x040015C5 RID: 5573
		internal const string ADVAPI32 = "advapi32.dll";

		// Token: 0x040015C6 RID: 5574
		internal const string OLE32 = "ole32.dll";

		// Token: 0x040015C7 RID: 5575
		internal const string OLEAUT32 = "oleaut32.dll";

		// Token: 0x040015C8 RID: 5576
		internal const string SHFOLDER = "shfolder.dll";

		// Token: 0x040015C9 RID: 5577
		internal const string SHIM = "mscoree.dll";

		// Token: 0x040015CA RID: 5578
		internal const string CRYPT32 = "crypt32.dll";

		// Token: 0x040015CB RID: 5579
		internal const string SECUR32 = "secur32.dll";

		// Token: 0x040015CC RID: 5580
		internal const string MSCORWKS = "mscorwks.dll";

		// Token: 0x040015CD RID: 5581
		internal const string LSTRCPY = "lstrcpy";

		// Token: 0x040015CE RID: 5582
		internal const string LSTRCPYN = "lstrcpyn";

		// Token: 0x040015CF RID: 5583
		internal const string LSTRLEN = "lstrlen";

		// Token: 0x040015D0 RID: 5584
		internal const string LSTRLENA = "lstrlenA";

		// Token: 0x040015D1 RID: 5585
		internal const string LSTRLENW = "lstrlenW";

		// Token: 0x040015D2 RID: 5586
		internal const string MOVEMEMORY = "RtlMoveMemory";

		// Token: 0x040015D3 RID: 5587
		internal const int SEM_FAILCRITICALERRORS = 1;

		// Token: 0x040015D4 RID: 5588
		internal const int LCMAP_SORTKEY = 1024;

		// Token: 0x040015D5 RID: 5589
		internal const int FIND_STARTSWITH = 1048576;

		// Token: 0x040015D6 RID: 5590
		internal const int FIND_ENDSWITH = 2097152;

		// Token: 0x040015D7 RID: 5591
		internal const int FIND_FROMSTART = 4194304;

		// Token: 0x040015D8 RID: 5592
		internal const int FIND_FROMEND = 8388608;

		// Token: 0x040015D9 RID: 5593
		internal const int STD_INPUT_HANDLE = -10;

		// Token: 0x040015DA RID: 5594
		internal const int STD_OUTPUT_HANDLE = -11;

		// Token: 0x040015DB RID: 5595
		internal const int STD_ERROR_HANDLE = -12;

		// Token: 0x040015DC RID: 5596
		internal const int CTRL_C_EVENT = 0;

		// Token: 0x040015DD RID: 5597
		internal const int CTRL_BREAK_EVENT = 1;

		// Token: 0x040015DE RID: 5598
		internal const int CTRL_CLOSE_EVENT = 2;

		// Token: 0x040015DF RID: 5599
		internal const int CTRL_LOGOFF_EVENT = 5;

		// Token: 0x040015E0 RID: 5600
		internal const int CTRL_SHUTDOWN_EVENT = 6;

		// Token: 0x040015E1 RID: 5601
		internal const short KEY_EVENT = 1;

		// Token: 0x040015E2 RID: 5602
		internal const int FILE_TYPE_DISK = 1;

		// Token: 0x040015E3 RID: 5603
		internal const int FILE_TYPE_CHAR = 2;

		// Token: 0x040015E4 RID: 5604
		internal const int FILE_TYPE_PIPE = 3;

		// Token: 0x040015E5 RID: 5605
		internal const int REPLACEFILE_WRITE_THROUGH = 1;

		// Token: 0x040015E6 RID: 5606
		internal const int REPLACEFILE_IGNORE_MERGE_ERRORS = 2;

		// Token: 0x040015E7 RID: 5607
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040015E8 RID: 5608
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040015E9 RID: 5609
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x040015EA RID: 5610
		internal const int FILE_ATTRIBUTE_READONLY = 1;

		// Token: 0x040015EB RID: 5611
		internal const int FILE_ATTRIBUTE_DIRECTORY = 16;

		// Token: 0x040015EC RID: 5612
		internal const int FILE_ATTRIBUTE_REPARSE_POINT = 1024;

		// Token: 0x040015ED RID: 5613
		internal const int IO_REPARSE_TAG_MOUNT_POINT = -1610612733;

		// Token: 0x040015EE RID: 5614
		internal const int PAGE_READWRITE = 4;

		// Token: 0x040015EF RID: 5615
		internal const int MEM_COMMIT = 4096;

		// Token: 0x040015F0 RID: 5616
		internal const int MEM_RESERVE = 8192;

		// Token: 0x040015F1 RID: 5617
		internal const int MEM_RELEASE = 32768;

		// Token: 0x040015F2 RID: 5618
		internal const int MEM_FREE = 65536;

		// Token: 0x040015F3 RID: 5619
		internal const int ERROR_SUCCESS = 0;

		// Token: 0x040015F4 RID: 5620
		internal const int ERROR_INVALID_FUNCTION = 1;

		// Token: 0x040015F5 RID: 5621
		internal const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x040015F6 RID: 5622
		internal const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x040015F7 RID: 5623
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x040015F8 RID: 5624
		internal const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x040015F9 RID: 5625
		internal const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x040015FA RID: 5626
		internal const int ERROR_INVALID_DATA = 13;

		// Token: 0x040015FB RID: 5627
		internal const int ERROR_INVALID_DRIVE = 15;

		// Token: 0x040015FC RID: 5628
		internal const int ERROR_NO_MORE_FILES = 18;

		// Token: 0x040015FD RID: 5629
		internal const int ERROR_NOT_READY = 21;

		// Token: 0x040015FE RID: 5630
		internal const int ERROR_BAD_LENGTH = 24;

		// Token: 0x040015FF RID: 5631
		internal const int ERROR_SHARING_VIOLATION = 32;

		// Token: 0x04001600 RID: 5632
		internal const int ERROR_NOT_SUPPORTED = 50;

		// Token: 0x04001601 RID: 5633
		internal const int ERROR_FILE_EXISTS = 80;

		// Token: 0x04001602 RID: 5634
		internal const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001603 RID: 5635
		internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;

		// Token: 0x04001604 RID: 5636
		internal const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x04001605 RID: 5637
		internal const int ERROR_INVALID_NAME = 123;

		// Token: 0x04001606 RID: 5638
		internal const int ERROR_BAD_PATHNAME = 161;

		// Token: 0x04001607 RID: 5639
		internal const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x04001608 RID: 5640
		internal const int ERROR_ENVVAR_NOT_FOUND = 203;

		// Token: 0x04001609 RID: 5641
		internal const int ERROR_FILENAME_EXCED_RANGE = 206;

		// Token: 0x0400160A RID: 5642
		internal const int ERROR_NO_DATA = 232;

		// Token: 0x0400160B RID: 5643
		internal const int ERROR_PIPE_NOT_CONNECTED = 233;

		// Token: 0x0400160C RID: 5644
		internal const int ERROR_MORE_DATA = 234;

		// Token: 0x0400160D RID: 5645
		internal const int ERROR_OPERATION_ABORTED = 995;

		// Token: 0x0400160E RID: 5646
		internal const int ERROR_NO_TOKEN = 1008;

		// Token: 0x0400160F RID: 5647
		internal const int ERROR_DLL_INIT_FAILED = 1114;

		// Token: 0x04001610 RID: 5648
		internal const int ERROR_NON_ACCOUNT_SID = 1257;

		// Token: 0x04001611 RID: 5649
		internal const int ERROR_NOT_ALL_ASSIGNED = 1300;

		// Token: 0x04001612 RID: 5650
		internal const int ERROR_UNKNOWN_REVISION = 1305;

		// Token: 0x04001613 RID: 5651
		internal const int ERROR_INVALID_OWNER = 1307;

		// Token: 0x04001614 RID: 5652
		internal const int ERROR_INVALID_PRIMARY_GROUP = 1308;

		// Token: 0x04001615 RID: 5653
		internal const int ERROR_NO_SUCH_PRIVILEGE = 1313;

		// Token: 0x04001616 RID: 5654
		internal const int ERROR_PRIVILEGE_NOT_HELD = 1314;

		// Token: 0x04001617 RID: 5655
		internal const int ERROR_NONE_MAPPED = 1332;

		// Token: 0x04001618 RID: 5656
		internal const int ERROR_INVALID_ACL = 1336;

		// Token: 0x04001619 RID: 5657
		internal const int ERROR_INVALID_SID = 1337;

		// Token: 0x0400161A RID: 5658
		internal const int ERROR_INVALID_SECURITY_DESCR = 1338;

		// Token: 0x0400161B RID: 5659
		internal const int ERROR_BAD_IMPERSONATION_LEVEL = 1346;

		// Token: 0x0400161C RID: 5660
		internal const int ERROR_CANT_OPEN_ANONYMOUS = 1347;

		// Token: 0x0400161D RID: 5661
		internal const int ERROR_NO_SECURITY_ON_OBJECT = 1350;

		// Token: 0x0400161E RID: 5662
		internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 1789;

		// Token: 0x0400161F RID: 5663
		internal const uint STATUS_SUCCESS = 0U;

		// Token: 0x04001620 RID: 5664
		internal const uint STATUS_SOME_NOT_MAPPED = 263U;

		// Token: 0x04001621 RID: 5665
		internal const uint STATUS_NO_MEMORY = 3221225495U;

		// Token: 0x04001622 RID: 5666
		internal const uint STATUS_OBJECT_NAME_NOT_FOUND = 3221225524U;

		// Token: 0x04001623 RID: 5667
		internal const uint STATUS_NONE_MAPPED = 3221225587U;

		// Token: 0x04001624 RID: 5668
		internal const uint STATUS_INSUFFICIENT_RESOURCES = 3221225626U;

		// Token: 0x04001625 RID: 5669
		internal const uint STATUS_ACCESS_DENIED = 3221225506U;

		// Token: 0x04001626 RID: 5670
		internal const int INVALID_FILE_SIZE = -1;

		// Token: 0x04001627 RID: 5671
		internal const int STATUS_ACCOUNT_RESTRICTION = -1073741714;

		// Token: 0x04001628 RID: 5672
		internal const int LCID_SUPPORTED = 2;

		// Token: 0x04001629 RID: 5673
		internal const int ENABLE_PROCESSED_INPUT = 1;

		// Token: 0x0400162A RID: 5674
		internal const int ENABLE_LINE_INPUT = 2;

		// Token: 0x0400162B RID: 5675
		internal const int ENABLE_ECHO_INPUT = 4;

		// Token: 0x0400162C RID: 5676
		internal const int VER_PLATFORM_WIN32s = 0;

		// Token: 0x0400162D RID: 5677
		internal const int VER_PLATFORM_WIN32_WINDOWS = 1;

		// Token: 0x0400162E RID: 5678
		internal const int VER_PLATFORM_WIN32_NT = 2;

		// Token: 0x0400162F RID: 5679
		internal const int VER_PLATFORM_WINCE = 3;

		// Token: 0x04001630 RID: 5680
		internal const int SHGFP_TYPE_CURRENT = 0;

		// Token: 0x04001631 RID: 5681
		internal const int UOI_FLAGS = 1;

		// Token: 0x04001632 RID: 5682
		internal const int WSF_VISIBLE = 1;

		// Token: 0x04001633 RID: 5683
		internal const int CSIDL_APPDATA = 26;

		// Token: 0x04001634 RID: 5684
		internal const int CSIDL_COMMON_APPDATA = 35;

		// Token: 0x04001635 RID: 5685
		internal const int CSIDL_LOCAL_APPDATA = 28;

		// Token: 0x04001636 RID: 5686
		internal const int CSIDL_COOKIES = 33;

		// Token: 0x04001637 RID: 5687
		internal const int CSIDL_FAVORITES = 6;

		// Token: 0x04001638 RID: 5688
		internal const int CSIDL_HISTORY = 34;

		// Token: 0x04001639 RID: 5689
		internal const int CSIDL_INTERNET_CACHE = 32;

		// Token: 0x0400163A RID: 5690
		internal const int CSIDL_PROGRAMS = 2;

		// Token: 0x0400163B RID: 5691
		internal const int CSIDL_RECENT = 8;

		// Token: 0x0400163C RID: 5692
		internal const int CSIDL_SENDTO = 9;

		// Token: 0x0400163D RID: 5693
		internal const int CSIDL_STARTMENU = 11;

		// Token: 0x0400163E RID: 5694
		internal const int CSIDL_STARTUP = 7;

		// Token: 0x0400163F RID: 5695
		internal const int CSIDL_SYSTEM = 37;

		// Token: 0x04001640 RID: 5696
		internal const int CSIDL_TEMPLATES = 21;

		// Token: 0x04001641 RID: 5697
		internal const int CSIDL_DESKTOPDIRECTORY = 16;

		// Token: 0x04001642 RID: 5698
		internal const int CSIDL_PERSONAL = 5;

		// Token: 0x04001643 RID: 5699
		internal const int CSIDL_PROGRAM_FILES = 38;

		// Token: 0x04001644 RID: 5700
		internal const int CSIDL_PROGRAM_FILES_COMMON = 43;

		// Token: 0x04001645 RID: 5701
		internal const int CSIDL_DESKTOP = 0;

		// Token: 0x04001646 RID: 5702
		internal const int CSIDL_DRIVES = 17;

		// Token: 0x04001647 RID: 5703
		internal const int CSIDL_MYMUSIC = 13;

		// Token: 0x04001648 RID: 5704
		internal const int CSIDL_MYPICTURES = 39;

		// Token: 0x04001649 RID: 5705
		internal const int NameSamCompatible = 2;

		// Token: 0x0400164A RID: 5706
		internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		// Token: 0x0400164B RID: 5707
		internal static readonly IntPtr NULL = IntPtr.Zero;

		// Token: 0x02000433 RID: 1075
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class OSVERSIONINFO
		{
			// Token: 0x06002CEB RID: 11499 RVA: 0x00096B64 File Offset: 0x00095B64
			internal OSVERSIONINFO()
			{
				this.OSVersionInfoSize = Marshal.SizeOf(this);
			}

			// Token: 0x0400164C RID: 5708
			internal int OSVersionInfoSize;

			// Token: 0x0400164D RID: 5709
			internal int MajorVersion;

			// Token: 0x0400164E RID: 5710
			internal int MinorVersion;

			// Token: 0x0400164F RID: 5711
			internal int BuildNumber;

			// Token: 0x04001650 RID: 5712
			internal int PlatformId;

			// Token: 0x04001651 RID: 5713
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string CSDVersion;
		}

		// Token: 0x02000434 RID: 1076
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class OSVERSIONINFOEX
		{
			// Token: 0x06002CEC RID: 11500 RVA: 0x00096B78 File Offset: 0x00095B78
			public OSVERSIONINFOEX()
			{
				this.OSVersionInfoSize = Marshal.SizeOf(this);
			}

			// Token: 0x04001652 RID: 5714
			internal int OSVersionInfoSize;

			// Token: 0x04001653 RID: 5715
			internal int MajorVersion;

			// Token: 0x04001654 RID: 5716
			internal int MinorVersion;

			// Token: 0x04001655 RID: 5717
			internal int BuildNumber;

			// Token: 0x04001656 RID: 5718
			internal int PlatformId;

			// Token: 0x04001657 RID: 5719
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			internal string CSDVersion;

			// Token: 0x04001658 RID: 5720
			internal ushort ServicePackMajor;

			// Token: 0x04001659 RID: 5721
			internal ushort ServicePackMinor;

			// Token: 0x0400165A RID: 5722
			internal short SuiteMask;

			// Token: 0x0400165B RID: 5723
			internal byte ProductType;

			// Token: 0x0400165C RID: 5724
			internal byte Reserved;
		}

		// Token: 0x02000435 RID: 1077
		internal struct SYSTEM_INFO
		{
			// Token: 0x0400165D RID: 5725
			internal int dwOemId;

			// Token: 0x0400165E RID: 5726
			internal int dwPageSize;

			// Token: 0x0400165F RID: 5727
			internal IntPtr lpMinimumApplicationAddress;

			// Token: 0x04001660 RID: 5728
			internal IntPtr lpMaximumApplicationAddress;

			// Token: 0x04001661 RID: 5729
			internal IntPtr dwActiveProcessorMask;

			// Token: 0x04001662 RID: 5730
			internal int dwNumberOfProcessors;

			// Token: 0x04001663 RID: 5731
			internal int dwProcessorType;

			// Token: 0x04001664 RID: 5732
			internal int dwAllocationGranularity;

			// Token: 0x04001665 RID: 5733
			internal short wProcessorLevel;

			// Token: 0x04001666 RID: 5734
			internal short wProcessorRevision;
		}

		// Token: 0x02000436 RID: 1078
		[StructLayout(LayoutKind.Sequential)]
		internal class SECURITY_ATTRIBUTES
		{
			// Token: 0x04001667 RID: 5735
			internal int nLength;

			// Token: 0x04001668 RID: 5736
			internal unsafe byte* pSecurityDescriptor = null;

			// Token: 0x04001669 RID: 5737
			internal int bInheritHandle;
		}

		// Token: 0x02000437 RID: 1079
		[Serializable]
		internal struct WIN32_FILE_ATTRIBUTE_DATA
		{
			// Token: 0x0400166A RID: 5738
			internal int fileAttributes;

			// Token: 0x0400166B RID: 5739
			internal uint ftCreationTimeLow;

			// Token: 0x0400166C RID: 5740
			internal uint ftCreationTimeHigh;

			// Token: 0x0400166D RID: 5741
			internal uint ftLastAccessTimeLow;

			// Token: 0x0400166E RID: 5742
			internal uint ftLastAccessTimeHigh;

			// Token: 0x0400166F RID: 5743
			internal uint ftLastWriteTimeLow;

			// Token: 0x04001670 RID: 5744
			internal uint ftLastWriteTimeHigh;

			// Token: 0x04001671 RID: 5745
			internal int fileSizeHigh;

			// Token: 0x04001672 RID: 5746
			internal int fileSizeLow;
		}

		// Token: 0x02000438 RID: 1080
		internal struct FILE_TIME
		{
			// Token: 0x06002CEE RID: 11502 RVA: 0x00096B9C File Offset: 0x00095B9C
			public FILE_TIME(long fileTime)
			{
				this.ftTimeLow = (uint)fileTime;
				this.ftTimeHigh = (uint)(fileTime >> 32);
			}

			// Token: 0x06002CEF RID: 11503 RVA: 0x00096BB1 File Offset: 0x00095BB1
			public long ToTicks()
			{
				return (long)(((ulong)this.ftTimeHigh << 32) + (ulong)this.ftTimeLow);
			}

			// Token: 0x04001673 RID: 5747
			internal uint ftTimeLow;

			// Token: 0x04001674 RID: 5748
			internal uint ftTimeHigh;
		}

		// Token: 0x02000439 RID: 1081
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct KERB_S4U_LOGON
		{
			// Token: 0x04001675 RID: 5749
			internal uint MessageType;

			// Token: 0x04001676 RID: 5750
			internal uint Flags;

			// Token: 0x04001677 RID: 5751
			internal Win32Native.UNICODE_INTPTR_STRING ClientUpn;

			// Token: 0x04001678 RID: 5752
			internal Win32Native.UNICODE_INTPTR_STRING ClientRealm;
		}

		// Token: 0x0200043A RID: 1082
		internal struct LSA_OBJECT_ATTRIBUTES
		{
			// Token: 0x04001679 RID: 5753
			internal int Length;

			// Token: 0x0400167A RID: 5754
			internal IntPtr RootDirectory;

			// Token: 0x0400167B RID: 5755
			internal IntPtr ObjectName;

			// Token: 0x0400167C RID: 5756
			internal int Attributes;

			// Token: 0x0400167D RID: 5757
			internal IntPtr SecurityDescriptor;

			// Token: 0x0400167E RID: 5758
			internal IntPtr SecurityQualityOfService;
		}

		// Token: 0x0200043B RID: 1083
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct UNICODE_STRING
		{
			// Token: 0x0400167F RID: 5759
			internal ushort Length;

			// Token: 0x04001680 RID: 5760
			internal ushort MaximumLength;

			// Token: 0x04001681 RID: 5761
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string Buffer;
		}

		// Token: 0x0200043C RID: 1084
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct UNICODE_INTPTR_STRING
		{
			// Token: 0x06002CF0 RID: 11504 RVA: 0x00096BC5 File Offset: 0x00095BC5
			internal UNICODE_INTPTR_STRING(int length, int maximumLength, IntPtr buffer)
			{
				this.Length = (ushort)length;
				this.MaxLength = (ushort)maximumLength;
				this.Buffer = buffer;
			}

			// Token: 0x04001682 RID: 5762
			internal ushort Length;

			// Token: 0x04001683 RID: 5763
			internal ushort MaxLength;

			// Token: 0x04001684 RID: 5764
			internal IntPtr Buffer;
		}

		// Token: 0x0200043D RID: 1085
		internal struct LSA_TRANSLATED_NAME
		{
			// Token: 0x04001685 RID: 5765
			internal int Use;

			// Token: 0x04001686 RID: 5766
			internal Win32Native.UNICODE_INTPTR_STRING Name;

			// Token: 0x04001687 RID: 5767
			internal int DomainIndex;
		}

		// Token: 0x0200043E RID: 1086
		internal struct LSA_TRANSLATED_SID
		{
			// Token: 0x04001688 RID: 5768
			internal int Use;

			// Token: 0x04001689 RID: 5769
			internal uint Rid;

			// Token: 0x0400168A RID: 5770
			internal int DomainIndex;
		}

		// Token: 0x0200043F RID: 1087
		internal struct LSA_TRANSLATED_SID2
		{
			// Token: 0x0400168B RID: 5771
			internal int Use;

			// Token: 0x0400168C RID: 5772
			internal IntPtr Sid;

			// Token: 0x0400168D RID: 5773
			internal int DomainIndex;

			// Token: 0x0400168E RID: 5774
			private uint Flags;
		}

		// Token: 0x02000440 RID: 1088
		internal struct LSA_TRUST_INFORMATION
		{
			// Token: 0x0400168F RID: 5775
			internal Win32Native.UNICODE_INTPTR_STRING Name;

			// Token: 0x04001690 RID: 5776
			internal IntPtr Sid;
		}

		// Token: 0x02000441 RID: 1089
		internal struct LSA_REFERENCED_DOMAIN_LIST
		{
			// Token: 0x04001691 RID: 5777
			internal int Entries;

			// Token: 0x04001692 RID: 5778
			internal IntPtr Domains;
		}

		// Token: 0x02000442 RID: 1090
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID
		{
			// Token: 0x04001693 RID: 5779
			internal uint LowPart;

			// Token: 0x04001694 RID: 5780
			internal uint HighPart;
		}

		// Token: 0x02000443 RID: 1091
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID_AND_ATTRIBUTES
		{
			// Token: 0x04001695 RID: 5781
			internal Win32Native.LUID Luid;

			// Token: 0x04001696 RID: 5782
			internal uint Attributes;
		}

		// Token: 0x02000444 RID: 1092
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct QUOTA_LIMITS
		{
			// Token: 0x04001697 RID: 5783
			internal IntPtr PagedPoolLimit;

			// Token: 0x04001698 RID: 5784
			internal IntPtr NonPagedPoolLimit;

			// Token: 0x04001699 RID: 5785
			internal IntPtr MinimumWorkingSetSize;

			// Token: 0x0400169A RID: 5786
			internal IntPtr MaximumWorkingSetSize;

			// Token: 0x0400169B RID: 5787
			internal IntPtr PagefileLimit;

			// Token: 0x0400169C RID: 5788
			internal IntPtr TimeLimit;
		}

		// Token: 0x02000445 RID: 1093
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct SECURITY_LOGON_SESSION_DATA
		{
			// Token: 0x0400169D RID: 5789
			internal uint Size;

			// Token: 0x0400169E RID: 5790
			internal Win32Native.LUID LogonId;

			// Token: 0x0400169F RID: 5791
			internal Win32Native.UNICODE_INTPTR_STRING UserName;

			// Token: 0x040016A0 RID: 5792
			internal Win32Native.UNICODE_INTPTR_STRING LogonDomain;

			// Token: 0x040016A1 RID: 5793
			internal Win32Native.UNICODE_INTPTR_STRING AuthenticationPackage;

			// Token: 0x040016A2 RID: 5794
			internal uint LogonType;

			// Token: 0x040016A3 RID: 5795
			internal uint Session;

			// Token: 0x040016A4 RID: 5796
			internal IntPtr Sid;

			// Token: 0x040016A5 RID: 5797
			internal long LogonTime;
		}

		// Token: 0x02000446 RID: 1094
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct SID_AND_ATTRIBUTES
		{
			// Token: 0x040016A6 RID: 5798
			internal IntPtr Sid;

			// Token: 0x040016A7 RID: 5799
			internal uint Attributes;
		}

		// Token: 0x02000447 RID: 1095
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_GROUPS
		{
			// Token: 0x040016A8 RID: 5800
			internal uint GroupCount;

			// Token: 0x040016A9 RID: 5801
			internal Win32Native.SID_AND_ATTRIBUTES Groups;
		}

		// Token: 0x02000448 RID: 1096
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_PRIVILEGE
		{
			// Token: 0x040016AA RID: 5802
			internal uint PrivilegeCount;

			// Token: 0x040016AB RID: 5803
			internal Win32Native.LUID_AND_ATTRIBUTES Privilege;
		}

		// Token: 0x02000449 RID: 1097
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_SOURCE
		{
			// Token: 0x040016AC RID: 5804
			private const int TOKEN_SOURCE_LENGTH = 8;

			// Token: 0x040016AD RID: 5805
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			internal char[] Name;

			// Token: 0x040016AE RID: 5806
			internal Win32Native.LUID SourceIdentifier;
		}

		// Token: 0x0200044A RID: 1098
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_STATISTICS
		{
			// Token: 0x040016AF RID: 5807
			internal Win32Native.LUID TokenId;

			// Token: 0x040016B0 RID: 5808
			internal Win32Native.LUID AuthenticationId;

			// Token: 0x040016B1 RID: 5809
			internal long ExpirationTime;

			// Token: 0x040016B2 RID: 5810
			internal uint TokenType;

			// Token: 0x040016B3 RID: 5811
			internal uint ImpersonationLevel;

			// Token: 0x040016B4 RID: 5812
			internal uint DynamicCharged;

			// Token: 0x040016B5 RID: 5813
			internal uint DynamicAvailable;

			// Token: 0x040016B6 RID: 5814
			internal uint GroupCount;

			// Token: 0x040016B7 RID: 5815
			internal uint PrivilegeCount;

			// Token: 0x040016B8 RID: 5816
			internal Win32Native.LUID ModifiedId;
		}

		// Token: 0x0200044B RID: 1099
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_USER
		{
			// Token: 0x040016B9 RID: 5817
			internal Win32Native.SID_AND_ATTRIBUTES User;
		}

		// Token: 0x0200044C RID: 1100
		[StructLayout(LayoutKind.Sequential)]
		internal class MEMORYSTATUSEX
		{
			// Token: 0x06002CF1 RID: 11505 RVA: 0x00096BDE File Offset: 0x00095BDE
			internal MEMORYSTATUSEX()
			{
				this.length = Marshal.SizeOf(this);
			}

			// Token: 0x040016BA RID: 5818
			internal int length;

			// Token: 0x040016BB RID: 5819
			internal int memoryLoad;

			// Token: 0x040016BC RID: 5820
			internal ulong totalPhys;

			// Token: 0x040016BD RID: 5821
			internal ulong availPhys;

			// Token: 0x040016BE RID: 5822
			internal ulong totalPageFile;

			// Token: 0x040016BF RID: 5823
			internal ulong availPageFile;

			// Token: 0x040016C0 RID: 5824
			internal ulong totalVirtual;

			// Token: 0x040016C1 RID: 5825
			internal ulong availVirtual;

			// Token: 0x040016C2 RID: 5826
			internal ulong availExtendedVirtual;
		}

		// Token: 0x0200044D RID: 1101
		[StructLayout(LayoutKind.Sequential)]
		internal class MEMORYSTATUS
		{
			// Token: 0x06002CF2 RID: 11506 RVA: 0x00096BF2 File Offset: 0x00095BF2
			internal MEMORYSTATUS()
			{
				this.length = Marshal.SizeOf(this);
			}

			// Token: 0x040016C3 RID: 5827
			internal int length;

			// Token: 0x040016C4 RID: 5828
			internal int memoryLoad;

			// Token: 0x040016C5 RID: 5829
			internal uint totalPhys;

			// Token: 0x040016C6 RID: 5830
			internal uint availPhys;

			// Token: 0x040016C7 RID: 5831
			internal uint totalPageFile;

			// Token: 0x040016C8 RID: 5832
			internal uint availPageFile;

			// Token: 0x040016C9 RID: 5833
			internal uint totalVirtual;

			// Token: 0x040016CA RID: 5834
			internal uint availVirtual;
		}

		// Token: 0x0200044E RID: 1102
		internal struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x040016CB RID: 5835
			internal unsafe void* BaseAddress;

			// Token: 0x040016CC RID: 5836
			internal unsafe void* AllocationBase;

			// Token: 0x040016CD RID: 5837
			internal uint AllocationProtect;

			// Token: 0x040016CE RID: 5838
			internal UIntPtr RegionSize;

			// Token: 0x040016CF RID: 5839
			internal uint State;

			// Token: 0x040016D0 RID: 5840
			internal uint Protect;

			// Token: 0x040016D1 RID: 5841
			internal uint Type;
		}

		// Token: 0x0200044F RID: 1103
		[BestFitMapping(false)]
		[Serializable]
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class WIN32_FIND_DATA
		{
			// Token: 0x040016D2 RID: 5842
			internal int dwFileAttributes;

			// Token: 0x040016D3 RID: 5843
			internal int ftCreationTime_dwLowDateTime;

			// Token: 0x040016D4 RID: 5844
			internal int ftCreationTime_dwHighDateTime;

			// Token: 0x040016D5 RID: 5845
			internal int ftLastAccessTime_dwLowDateTime;

			// Token: 0x040016D6 RID: 5846
			internal int ftLastAccessTime_dwHighDateTime;

			// Token: 0x040016D7 RID: 5847
			internal int ftLastWriteTime_dwLowDateTime;

			// Token: 0x040016D8 RID: 5848
			internal int ftLastWriteTime_dwHighDateTime;

			// Token: 0x040016D9 RID: 5849
			internal int nFileSizeHigh;

			// Token: 0x040016DA RID: 5850
			internal int nFileSizeLow;

			// Token: 0x040016DB RID: 5851
			internal int dwReserved0;

			// Token: 0x040016DC RID: 5852
			internal int dwReserved1;

			// Token: 0x040016DD RID: 5853
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string cFileName;

			// Token: 0x040016DE RID: 5854
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			internal string cAlternateFileName;
		}

		// Token: 0x02000450 RID: 1104
		// (Invoke) Token: 0x06002CF5 RID: 11509
		internal delegate bool ConsoleCtrlHandlerRoutine(int controlType);

		// Token: 0x02000451 RID: 1105
		internal struct COORD
		{
			// Token: 0x040016DF RID: 5855
			internal short X;

			// Token: 0x040016E0 RID: 5856
			internal short Y;
		}

		// Token: 0x02000452 RID: 1106
		internal struct SMALL_RECT
		{
			// Token: 0x040016E1 RID: 5857
			internal short Left;

			// Token: 0x040016E2 RID: 5858
			internal short Top;

			// Token: 0x040016E3 RID: 5859
			internal short Right;

			// Token: 0x040016E4 RID: 5860
			internal short Bottom;
		}

		// Token: 0x02000453 RID: 1107
		internal struct CONSOLE_SCREEN_BUFFER_INFO
		{
			// Token: 0x040016E5 RID: 5861
			internal Win32Native.COORD dwSize;

			// Token: 0x040016E6 RID: 5862
			internal Win32Native.COORD dwCursorPosition;

			// Token: 0x040016E7 RID: 5863
			internal short wAttributes;

			// Token: 0x040016E8 RID: 5864
			internal Win32Native.SMALL_RECT srWindow;

			// Token: 0x040016E9 RID: 5865
			internal Win32Native.COORD dwMaximumWindowSize;
		}

		// Token: 0x02000454 RID: 1108
		internal struct CONSOLE_CURSOR_INFO
		{
			// Token: 0x040016EA RID: 5866
			internal int dwSize;

			// Token: 0x040016EB RID: 5867
			internal bool bVisible;
		}

		// Token: 0x02000455 RID: 1109
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct KeyEventRecord
		{
			// Token: 0x040016EC RID: 5868
			internal bool keyDown;

			// Token: 0x040016ED RID: 5869
			internal short repeatCount;

			// Token: 0x040016EE RID: 5870
			internal short virtualKeyCode;

			// Token: 0x040016EF RID: 5871
			internal short virtualScanCode;

			// Token: 0x040016F0 RID: 5872
			internal char uChar;

			// Token: 0x040016F1 RID: 5873
			internal int controlKeyState;
		}

		// Token: 0x02000456 RID: 1110
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct InputRecord
		{
			// Token: 0x040016F2 RID: 5874
			internal short eventType;

			// Token: 0x040016F3 RID: 5875
			internal Win32Native.KeyEventRecord keyEvent;
		}

		// Token: 0x02000457 RID: 1111
		[Flags]
		[Serializable]
		internal enum Color : short
		{
			// Token: 0x040016F5 RID: 5877
			Black = 0,
			// Token: 0x040016F6 RID: 5878
			ForegroundBlue = 1,
			// Token: 0x040016F7 RID: 5879
			ForegroundGreen = 2,
			// Token: 0x040016F8 RID: 5880
			ForegroundRed = 4,
			// Token: 0x040016F9 RID: 5881
			ForegroundYellow = 6,
			// Token: 0x040016FA RID: 5882
			ForegroundIntensity = 8,
			// Token: 0x040016FB RID: 5883
			BackgroundBlue = 16,
			// Token: 0x040016FC RID: 5884
			BackgroundGreen = 32,
			// Token: 0x040016FD RID: 5885
			BackgroundRed = 64,
			// Token: 0x040016FE RID: 5886
			BackgroundYellow = 96,
			// Token: 0x040016FF RID: 5887
			BackgroundIntensity = 128,
			// Token: 0x04001700 RID: 5888
			ForegroundMask = 15,
			// Token: 0x04001701 RID: 5889
			BackgroundMask = 240,
			// Token: 0x04001702 RID: 5890
			ColorMask = 255
		}

		// Token: 0x02000458 RID: 1112
		internal struct CHAR_INFO
		{
			// Token: 0x04001703 RID: 5891
			private ushort charData;

			// Token: 0x04001704 RID: 5892
			private short attributes;
		}

		// Token: 0x02000459 RID: 1113
		[StructLayout(LayoutKind.Sequential)]
		internal class USEROBJECTFLAGS
		{
			// Token: 0x04001705 RID: 5893
			internal int fInherit;

			// Token: 0x04001706 RID: 5894
			internal int fReserved;

			// Token: 0x04001707 RID: 5895
			internal int dwFlags;
		}

		// Token: 0x0200045A RID: 1114
		internal enum SECURITY_IMPERSONATION_LEVEL
		{
			// Token: 0x04001709 RID: 5897
			Anonymous,
			// Token: 0x0400170A RID: 5898
			Identification,
			// Token: 0x0400170B RID: 5899
			Impersonation,
			// Token: 0x0400170C RID: 5900
			Delegation
		}
	}
}

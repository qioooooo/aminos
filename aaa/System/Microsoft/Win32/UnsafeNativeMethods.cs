using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x02000262 RID: 610
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06001533 RID: 5427
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetStdHandle(int type);

		// Token: 0x06001534 RID: 5428
		[DllImport("user32.dll", ExactSpelling = true)]
		public static extern IntPtr GetProcessWindowStation();

		// Token: 0x06001535 RID: 5429
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetUserObjectInformation(HandleRef hObj, int nIndex, [MarshalAs(UnmanagedType.LPStruct)] NativeMethods.USEROBJECTFLAGS pvBuffer, int nLength, ref int lpnLengthNeeded);

		// Token: 0x06001536 RID: 5430
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string modName);

		// Token: 0x06001537 RID: 5431
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern bool GetClassInfo(HandleRef hInst, string lpszClass, [In] [Out] NativeMethods.WNDCLASS_I wc);

		// Token: 0x06001538 RID: 5432
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool IsWindow(HandleRef hWnd);

		// Token: 0x06001539 RID: 5433 RVA: 0x00045E15 File Offset: 0x00044E15
		public static IntPtr SetClassLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetClassLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetClassLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x0600153A RID: 5434
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLong")]
		public static extern IntPtr SetClassLongPtr32(HandleRef hwnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x0600153B RID: 5435
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
		public static extern IntPtr SetClassLongPtr64(HandleRef hwnd, int nIndex, IntPtr dwNewLong);

		// Token: 0x0600153C RID: 5436 RVA: 0x00045E30 File Offset: 0x00044E30
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, HandleRef dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return UnsafeNativeMethods.SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return UnsafeNativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		// Token: 0x0600153D RID: 5437
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x0600153E RID: 5438
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, HandleRef dwNewLong);

		// Token: 0x0600153F RID: 5439
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short RegisterClass(NativeMethods.WNDCLASS wc);

		// Token: 0x06001540 RID: 5440
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern short UnregisterClass(string lpClassName, HandleRef hInstance);

		// Token: 0x06001541 RID: 5441
		[DllImport("user32.dll", BestFitMapping = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateWindowEx(int exStyle, string lpszClassName, string lpszWindowName, int style, int x, int y, int width, int height, HandleRef hWndParent, HandleRef hMenu, HandleRef hInst, [MarshalAs(UnmanagedType.AsAny)] object pvParam);

		// Token: 0x06001542 RID: 5442
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06001543 RID: 5443
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetConsoleCtrlHandler(NativeMethods.ConHndlr handler, int add);

		// Token: 0x06001544 RID: 5444
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06001545 RID: 5445
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool DestroyWindow(HandleRef hWnd);

		// Token: 0x06001546 RID: 5446
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern int MsgWaitForMultipleObjectsEx(int nCount, IntPtr pHandles, int dwMilliseconds, int dwWakeMask, int dwFlags);

		// Token: 0x06001547 RID: 5447
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int DispatchMessage([In] ref NativeMethods.MSG msg);

		// Token: 0x06001548 RID: 5448
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage([In] [Out] ref NativeMethods.MSG msg, HandleRef hwnd, int msgMin, int msgMax, int remove);

		// Token: 0x06001549 RID: 5449
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetTimer(HandleRef hWnd, HandleRef nIDEvent, int uElapse, HandleRef lpTimerProc);

		// Token: 0x0600154A RID: 5450
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool KillTimer(HandleRef hwnd, HandleRef idEvent);

		// Token: 0x0600154B RID: 5451
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool TranslateMessage([In] [Out] ref NativeMethods.MSG msg);

		// Token: 0x0600154C RID: 5452
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		public static extern IntPtr GetProcAddress(HandleRef hModule, string lpProcName);

		// Token: 0x0600154D RID: 5453
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int ReleaseDC(HandleRef hWnd, HandleRef hDC);

		// Token: 0x0600154E RID: 5454
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x0600154F RID: 5455
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetSystemMetrics(int nIndex);

		// Token: 0x06001550 RID: 5456
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetDC(HandleRef hWnd);

		// Token: 0x06001551 RID: 5457
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SelectObject(HandleRef hDC, HandleRef hObject);

		// Token: 0x06001552 RID: 5458
		[DllImport("wtsapi32.dll", CharSet = CharSet.Auto)]
		public static extern bool WTSRegisterSessionNotification(HandleRef hWnd, int dwFlags);

		// Token: 0x06001553 RID: 5459
		[DllImport("wtsapi32.dll", CharSet = CharSet.Auto)]
		public static extern bool WTSUnRegisterSessionNotification(HandleRef hWnd);

		// Token: 0x06001554 RID: 5460
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool LookupAccountSid(string systemName, byte[] sid, char[] name, int[] cbName, char[] referencedDomainName, int[] cbRefDomName, int[] sidNameUse);

		// Token: 0x06001555 RID: 5461
		[DllImport("version.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetFileVersionInfoSize(string lptstrFilename, out int handle);

		// Token: 0x06001556 RID: 5462
		[DllImport("version.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern bool GetFileVersionInfo(string lptstrFilename, int dwHandle, int dwLen, HandleRef lpData);

		// Token: 0x06001557 RID: 5463
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

		// Token: 0x06001558 RID: 5464
		[DllImport("version.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern bool VerQueryValue(HandleRef pBlock, string lpSubBlock, [In] [Out] ref IntPtr lplpBuffer, out int len);

		// Token: 0x06001559 RID: 5465
		[DllImport("version.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
		public static extern int VerLanguageName(int langID, StringBuilder lpBuffer, int nSize);

		// Token: 0x0600155A RID: 5466
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool ReportEvent(SafeHandle hEventLog, short type, ushort category, uint eventID, byte[] userSID, short numStrings, int dataLen, HandleRef strings, byte[] rawData);

		// Token: 0x0600155B RID: 5467
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool ClearEventLog(SafeHandle hEventLog, HandleRef lpctstrBackupFileName);

		// Token: 0x0600155C RID: 5468
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool GetNumberOfEventLogRecords(SafeHandle hEventLog, out int count);

		// Token: 0x0600155D RID: 5469
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool GetOldestEventLogRecord(SafeHandle hEventLog, int[] number);

		// Token: 0x0600155E RID: 5470
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool ReadEventLog(SafeHandle hEventLog, int dwReadFlags, int dwRecordOffset, byte[] buffer, int numberOfBytesToRead, int[] bytesRead, int[] minNumOfBytesNeeded);

		// Token: 0x0600155F RID: 5471
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool NotifyChangeEventLog(SafeHandle hEventLog, SafeHandle hEvent);

		// Token: 0x06001560 RID: 5472
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public unsafe static extern bool ReadDirectoryChangesW(SafeFileHandle hDirectory, HandleRef lpBuffer, int nBufferLength, int bWatchSubtree, int dwNotifyFilter, out int lpBytesReturned, NativeOverlapped* overlappedPointer, HandleRef lpCompletionRoutine);

		// Token: 0x06001561 RID: 5473
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06001562 RID: 5474
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetCommState(SafeFileHandle hFile, ref UnsafeNativeMethods.DCB lpDCB);

		// Token: 0x06001563 RID: 5475
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetCommState(SafeFileHandle hFile, ref UnsafeNativeMethods.DCB lpDCB);

		// Token: 0x06001564 RID: 5476
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetCommModemStatus(SafeFileHandle hFile, ref int lpModemStat);

		// Token: 0x06001565 RID: 5477
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetupComm(SafeFileHandle hFile, int dwInQueue, int dwOutQueue);

		// Token: 0x06001566 RID: 5478
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetCommTimeouts(SafeFileHandle hFile, ref UnsafeNativeMethods.COMMTIMEOUTS lpCommTimeouts);

		// Token: 0x06001567 RID: 5479
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetCommBreak(SafeFileHandle hFile);

		// Token: 0x06001568 RID: 5480
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ClearCommBreak(SafeFileHandle hFile);

		// Token: 0x06001569 RID: 5481
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ClearCommError(SafeFileHandle hFile, ref int lpErrors, ref UnsafeNativeMethods.COMSTAT lpStat);

		// Token: 0x0600156A RID: 5482
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ClearCommError(SafeFileHandle hFile, ref int lpErrors, IntPtr lpStat);

		// Token: 0x0600156B RID: 5483
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool PurgeComm(SafeFileHandle hFile, uint dwFlags);

		// Token: 0x0600156C RID: 5484
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool FlushFileBuffers(SafeFileHandle hFile);

		// Token: 0x0600156D RID: 5485
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool GetCommProperties(SafeFileHandle hFile, ref UnsafeNativeMethods.COMMPROP lpCommProp);

		// Token: 0x0600156E RID: 5486
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, IntPtr numBytesRead, NativeOverlapped* overlapped);

		// Token: 0x0600156F RID: 5487
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int ReadFile(SafeFileHandle handle, byte* bytes, int numBytesToRead, out int numBytesRead, IntPtr overlapped);

		// Token: 0x06001570 RID: 5488
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, IntPtr numBytesWritten, NativeOverlapped* lpOverlapped);

		// Token: 0x06001571 RID: 5489
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern int WriteFile(SafeFileHandle handle, byte* bytes, int numBytesToWrite, out int numBytesWritten, IntPtr lpOverlapped);

		// Token: 0x06001572 RID: 5490
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern int GetFileType(SafeFileHandle hFile);

		// Token: 0x06001573 RID: 5491
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool EscapeCommFunction(SafeFileHandle hFile, int dwFunc);

		// Token: 0x06001574 RID: 5492
		[DllImport("kernel32.dll", SetLastError = true)]
		internal unsafe static extern bool WaitCommEvent(SafeFileHandle hFile, int* lpEvtMask, NativeOverlapped* lpOverlapped);

		// Token: 0x06001575 RID: 5493
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool SetCommMask(SafeFileHandle hFile, int dwEvtMask);

		// Token: 0x06001576 RID: 5494
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal unsafe static extern bool GetOverlappedResult(SafeFileHandle hFile, NativeOverlapped* lpOverlapped, ref int lpNumberOfBytesTransferred, bool bWait);

		// Token: 0x06001577 RID: 5495
		[DllImport("ole32.dll")]
		internal static extern int CoMarshalInterface([MarshalAs(UnmanagedType.Interface)] object pStm, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags);

		// Token: 0x06001578 RID: 5496
		[DllImport("ole32.dll")]
		internal static extern int CoGetStandardMarshal(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out IntPtr ppMarshal);

		// Token: 0x06001579 RID: 5497
		[DllImport("ole32.dll")]
		internal static extern int CoGetMarshalSizeMax(out int pulSize, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags);

		// Token: 0x040011BC RID: 4540
		public const int FILE_READ_DATA = 1;

		// Token: 0x040011BD RID: 4541
		public const int FILE_LIST_DIRECTORY = 1;

		// Token: 0x040011BE RID: 4542
		public const int FILE_WRITE_DATA = 2;

		// Token: 0x040011BF RID: 4543
		public const int FILE_ADD_FILE = 2;

		// Token: 0x040011C0 RID: 4544
		public const int FILE_APPEND_DATA = 4;

		// Token: 0x040011C1 RID: 4545
		public const int FILE_ADD_SUBDIRECTORY = 4;

		// Token: 0x040011C2 RID: 4546
		public const int FILE_CREATE_PIPE_INSTANCE = 4;

		// Token: 0x040011C3 RID: 4547
		public const int FILE_READ_EA = 8;

		// Token: 0x040011C4 RID: 4548
		public const int FILE_WRITE_EA = 16;

		// Token: 0x040011C5 RID: 4549
		public const int FILE_EXECUTE = 32;

		// Token: 0x040011C6 RID: 4550
		public const int FILE_TRAVERSE = 32;

		// Token: 0x040011C7 RID: 4551
		public const int FILE_DELETE_CHILD = 64;

		// Token: 0x040011C8 RID: 4552
		public const int FILE_READ_ATTRIBUTES = 128;

		// Token: 0x040011C9 RID: 4553
		public const int FILE_WRITE_ATTRIBUTES = 256;

		// Token: 0x040011CA RID: 4554
		public const int FILE_SHARE_READ = 1;

		// Token: 0x040011CB RID: 4555
		public const int FILE_SHARE_WRITE = 2;

		// Token: 0x040011CC RID: 4556
		public const int FILE_SHARE_DELETE = 4;

		// Token: 0x040011CD RID: 4557
		public const int FILE_ATTRIBUTE_READONLY = 1;

		// Token: 0x040011CE RID: 4558
		public const int FILE_ATTRIBUTE_HIDDEN = 2;

		// Token: 0x040011CF RID: 4559
		public const int FILE_ATTRIBUTE_SYSTEM = 4;

		// Token: 0x040011D0 RID: 4560
		public const int FILE_ATTRIBUTE_DIRECTORY = 16;

		// Token: 0x040011D1 RID: 4561
		public const int FILE_ATTRIBUTE_ARCHIVE = 32;

		// Token: 0x040011D2 RID: 4562
		public const int FILE_ATTRIBUTE_NORMAL = 128;

		// Token: 0x040011D3 RID: 4563
		public const int FILE_ATTRIBUTE_TEMPORARY = 256;

		// Token: 0x040011D4 RID: 4564
		public const int FILE_ATTRIBUTE_COMPRESSED = 2048;

		// Token: 0x040011D5 RID: 4565
		public const int FILE_ATTRIBUTE_OFFLINE = 4096;

		// Token: 0x040011D6 RID: 4566
		public const int FILE_NOTIFY_CHANGE_FILE_NAME = 1;

		// Token: 0x040011D7 RID: 4567
		public const int FILE_NOTIFY_CHANGE_DIR_NAME = 2;

		// Token: 0x040011D8 RID: 4568
		public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 4;

		// Token: 0x040011D9 RID: 4569
		public const int FILE_NOTIFY_CHANGE_SIZE = 8;

		// Token: 0x040011DA RID: 4570
		public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 16;

		// Token: 0x040011DB RID: 4571
		public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 32;

		// Token: 0x040011DC RID: 4572
		public const int FILE_NOTIFY_CHANGE_CREATION = 64;

		// Token: 0x040011DD RID: 4573
		public const int FILE_NOTIFY_CHANGE_SECURITY = 256;

		// Token: 0x040011DE RID: 4574
		public const int FILE_ACTION_ADDED = 1;

		// Token: 0x040011DF RID: 4575
		public const int FILE_ACTION_REMOVED = 2;

		// Token: 0x040011E0 RID: 4576
		public const int FILE_ACTION_MODIFIED = 3;

		// Token: 0x040011E1 RID: 4577
		public const int FILE_ACTION_RENAMED_OLD_NAME = 4;

		// Token: 0x040011E2 RID: 4578
		public const int FILE_ACTION_RENAMED_NEW_NAME = 5;

		// Token: 0x040011E3 RID: 4579
		public const int FILE_CASE_SENSITIVE_SEARCH = 1;

		// Token: 0x040011E4 RID: 4580
		public const int FILE_CASE_PRESERVED_NAMES = 2;

		// Token: 0x040011E5 RID: 4581
		public const int FILE_UNICODE_ON_DISK = 4;

		// Token: 0x040011E6 RID: 4582
		public const int FILE_PERSISTENT_ACLS = 8;

		// Token: 0x040011E7 RID: 4583
		public const int FILE_FILE_COMPRESSION = 16;

		// Token: 0x040011E8 RID: 4584
		public const int OPEN_EXISTING = 3;

		// Token: 0x040011E9 RID: 4585
		public const int OPEN_ALWAYS = 4;

		// Token: 0x040011EA RID: 4586
		public const int FILE_FLAG_WRITE_THROUGH = -2147483648;

		// Token: 0x040011EB RID: 4587
		public const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x040011EC RID: 4588
		public const int FILE_FLAG_NO_BUFFERING = 536870912;

		// Token: 0x040011ED RID: 4589
		public const int FILE_FLAG_RANDOM_ACCESS = 268435456;

		// Token: 0x040011EE RID: 4590
		public const int FILE_FLAG_SEQUENTIAL_SCAN = 134217728;

		// Token: 0x040011EF RID: 4591
		public const int FILE_FLAG_DELETE_ON_CLOSE = 67108864;

		// Token: 0x040011F0 RID: 4592
		public const int FILE_FLAG_BACKUP_SEMANTICS = 33554432;

		// Token: 0x040011F1 RID: 4593
		public const int FILE_FLAG_POSIX_SEMANTICS = 16777216;

		// Token: 0x040011F2 RID: 4594
		public const int FILE_TYPE_UNKNOWN = 0;

		// Token: 0x040011F3 RID: 4595
		public const int FILE_TYPE_DISK = 1;

		// Token: 0x040011F4 RID: 4596
		public const int FILE_TYPE_CHAR = 2;

		// Token: 0x040011F5 RID: 4597
		public const int FILE_TYPE_PIPE = 3;

		// Token: 0x040011F6 RID: 4598
		public const int FILE_TYPE_REMOTE = 32768;

		// Token: 0x040011F7 RID: 4599
		public const int FILE_VOLUME_IS_COMPRESSED = 32768;

		// Token: 0x040011F8 RID: 4600
		public const int GetFileExInfoStandard = 0;

		// Token: 0x02000263 RID: 611
		public struct WIN32_FILE_ATTRIBUTE_DATA
		{
			// Token: 0x040011F9 RID: 4601
			internal int fileAttributes;

			// Token: 0x040011FA RID: 4602
			internal uint ftCreationTimeLow;

			// Token: 0x040011FB RID: 4603
			internal uint ftCreationTimeHigh;

			// Token: 0x040011FC RID: 4604
			internal uint ftLastAccessTimeLow;

			// Token: 0x040011FD RID: 4605
			internal uint ftLastAccessTimeHigh;

			// Token: 0x040011FE RID: 4606
			internal uint ftLastWriteTimeLow;

			// Token: 0x040011FF RID: 4607
			internal uint ftLastWriteTimeHigh;

			// Token: 0x04001200 RID: 4608
			internal uint fileSizeHigh;

			// Token: 0x04001201 RID: 4609
			internal uint fileSizeLow;
		}

		// Token: 0x02000264 RID: 612
		internal struct DCB
		{
			// Token: 0x04001202 RID: 4610
			public uint DCBlength;

			// Token: 0x04001203 RID: 4611
			public uint BaudRate;

			// Token: 0x04001204 RID: 4612
			public uint Flags;

			// Token: 0x04001205 RID: 4613
			public ushort wReserved;

			// Token: 0x04001206 RID: 4614
			public ushort XonLim;

			// Token: 0x04001207 RID: 4615
			public ushort XoffLim;

			// Token: 0x04001208 RID: 4616
			public byte ByteSize;

			// Token: 0x04001209 RID: 4617
			public byte Parity;

			// Token: 0x0400120A RID: 4618
			public byte StopBits;

			// Token: 0x0400120B RID: 4619
			public byte XonChar;

			// Token: 0x0400120C RID: 4620
			public byte XoffChar;

			// Token: 0x0400120D RID: 4621
			public byte ErrorChar;

			// Token: 0x0400120E RID: 4622
			public byte EofChar;

			// Token: 0x0400120F RID: 4623
			public byte EvtChar;

			// Token: 0x04001210 RID: 4624
			public ushort wReserved1;
		}

		// Token: 0x02000265 RID: 613
		internal struct COMSTAT
		{
			// Token: 0x04001211 RID: 4625
			public uint Flags;

			// Token: 0x04001212 RID: 4626
			public uint cbInQue;

			// Token: 0x04001213 RID: 4627
			public uint cbOutQue;
		}

		// Token: 0x02000266 RID: 614
		internal struct COMMTIMEOUTS
		{
			// Token: 0x04001214 RID: 4628
			public int ReadIntervalTimeout;

			// Token: 0x04001215 RID: 4629
			public int ReadTotalTimeoutMultiplier;

			// Token: 0x04001216 RID: 4630
			public int ReadTotalTimeoutConstant;

			// Token: 0x04001217 RID: 4631
			public int WriteTotalTimeoutMultiplier;

			// Token: 0x04001218 RID: 4632
			public int WriteTotalTimeoutConstant;
		}

		// Token: 0x02000267 RID: 615
		internal struct COMMPROP
		{
			// Token: 0x04001219 RID: 4633
			public ushort wPacketLength;

			// Token: 0x0400121A RID: 4634
			public ushort wPacketVersion;

			// Token: 0x0400121B RID: 4635
			public int dwServiceMask;

			// Token: 0x0400121C RID: 4636
			public int dwReserved1;

			// Token: 0x0400121D RID: 4637
			public int dwMaxTxQueue;

			// Token: 0x0400121E RID: 4638
			public int dwMaxRxQueue;

			// Token: 0x0400121F RID: 4639
			public int dwMaxBaud;

			// Token: 0x04001220 RID: 4640
			public int dwProvSubType;

			// Token: 0x04001221 RID: 4641
			public int dwProvCapabilities;

			// Token: 0x04001222 RID: 4642
			public int dwSettableParams;

			// Token: 0x04001223 RID: 4643
			public int dwSettableBaud;

			// Token: 0x04001224 RID: 4644
			public ushort wSettableData;

			// Token: 0x04001225 RID: 4645
			public ushort wSettableStopParity;

			// Token: 0x04001226 RID: 4646
			public int dwCurrentTxQueue;

			// Token: 0x04001227 RID: 4647
			public int dwCurrentRxQueue;

			// Token: 0x04001228 RID: 4648
			public int dwProvSpec1;

			// Token: 0x04001229 RID: 4649
			public int dwProvSpec2;

			// Token: 0x0400122A RID: 4650
			public char wcProvChar;
		}

		// Token: 0x02000268 RID: 616
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000017-0000-0000-c000-000000000046")]
		[ComImport]
		internal interface IStdMarshal
		{
		}

		// Token: 0x02000269 RID: 617
		[Guid("00000003-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IMarshal
		{
			// Token: 0x0600157A RID: 5498
			[PreserveSig]
			int GetUnmarshalClass(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out Guid pCid);

			// Token: 0x0600157B RID: 5499
			[PreserveSig]
			int GetMarshalSizeMax(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out int pSize);

			// Token: 0x0600157C RID: 5500
			[PreserveSig]
			int MarshalInterface([MarshalAs(UnmanagedType.Interface)] object pStm, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags);

			// Token: 0x0600157D RID: 5501
			[PreserveSig]
			int UnmarshalInterface([MarshalAs(UnmanagedType.Interface)] object pStm, ref Guid riid, out IntPtr ppv);

			// Token: 0x0600157E RID: 5502
			[PreserveSig]
			int ReleaseMarshalData([MarshalAs(UnmanagedType.Interface)] object pStm);

			// Token: 0x0600157F RID: 5503
			[PreserveSig]
			int DisconnectObject(int dwReserved);
		}
	}
}

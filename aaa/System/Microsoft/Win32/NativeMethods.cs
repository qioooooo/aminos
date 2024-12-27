using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x02000276 RID: 630
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal static class NativeMethods
	{
		// Token: 0x0600159E RID: 5534
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetExitCodeProcess(SafeProcessHandle processHandle, out int exitCode);

		// Token: 0x0600159F RID: 5535
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetProcessTimes(SafeProcessHandle handle, out long creation, out long exit, out long kernel, out long user);

		// Token: 0x060015A0 RID: 5536
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetThreadTimes(SafeThreadHandle handle, out long creation, out long exit, out long kernel, out long user);

		// Token: 0x060015A1 RID: 5537
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetStdHandle(int whichHandle);

		// Token: 0x060015A2 RID: 5538
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetConsoleCP();

		// Token: 0x060015A3 RID: 5539
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetConsoleOutputCP();

		// Token: 0x060015A4 RID: 5540
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int WaitForSingleObject(SafeProcessHandle handle, int timeout);

		// Token: 0x060015A5 RID: 5541
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CreatePipe(out SafeFileHandle hReadPipe, out SafeFileHandle hWritePipe, NativeMethods.SECURITY_ATTRIBUTES lpPipeAttributes, int nSize);

		// Token: 0x060015A6 RID: 5542
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CreateProcess([MarshalAs(UnmanagedType.LPTStr)] string lpApplicationName, StringBuilder lpCommandLine, NativeMethods.SECURITY_ATTRIBUTES lpProcessAttributes, NativeMethods.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, int dwCreationFlags, IntPtr lpEnvironment, [MarshalAs(UnmanagedType.LPTStr)] string lpCurrentDirectory, NativeMethods.STARTUPINFO lpStartupInfo, SafeNativeMethods.PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x060015A7 RID: 5543
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool TerminateProcess(SafeProcessHandle processHandle, int exitCode);

		// Token: 0x060015A8 RID: 5544
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetCurrentProcessId();

		// Token: 0x060015A9 RID: 5545
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetCurrentProcess();

		// Token: 0x060015AA RID: 5546 RVA: 0x00045FC8 File Offset: 0x00044FC8
		internal static string GetLocalPath(string fileName)
		{
			Uri uri = new Uri(fileName);
			return uri.LocalPath + uri.Fragment;
		}

		// Token: 0x060015AB RID: 5547
		[SuppressUnmanagedCodeSecurity]
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CreateProcessAsUser(SafeHandle hToken, string lpApplicationName, string lpCommandLine, NativeMethods.SECURITY_ATTRIBUTES lpProcessAttributes, NativeMethods.SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandles, int dwCreationFlags, HandleRef lpEnvironment, string lpCurrentDirectory, NativeMethods.STARTUPINFO lpStartupInfo, SafeNativeMethods.PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x060015AC RID: 5548
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		internal static extern bool CreateProcessWithLogonW(string userName, string domain, IntPtr password, NativeMethods.LogonFlags logonFlags, [MarshalAs(UnmanagedType.LPTStr)] string appName, StringBuilder cmdLine, int creationFlags, IntPtr environmentBlock, [MarshalAs(UnmanagedType.LPTStr)] string lpCurrentDirectory, NativeMethods.STARTUPINFO lpStartupInfo, SafeNativeMethods.PROCESS_INFORMATION lpProcessInformation);

		// Token: 0x060015AD RID: 5549
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileMappingHandle CreateFileMapping(IntPtr hFile, NativeMethods.SECURITY_ATTRIBUTES lpFileMappingAttributes, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, string lpName);

		// Token: 0x060015AE RID: 5550
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileMappingHandle OpenFileMapping(int dwDesiredAccess, bool bInheritHandle, string lpName);

		// Token: 0x060015AF RID: 5551
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int WaitForInputIdle(SafeProcessHandle handle, int milliseconds);

		// Token: 0x060015B0 RID: 5552
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern SafeProcessHandle OpenProcess(int access, bool inherit, int processId);

		// Token: 0x060015B1 RID: 5553
		[DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumProcessModules(SafeProcessHandle handle, IntPtr modules, int size, ref int needed);

		// Token: 0x060015B2 RID: 5554
		[DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumProcesses(int[] processIds, int size, out int needed);

		// Token: 0x060015B3 RID: 5555
		[DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetModuleFileNameEx(HandleRef processHandle, HandleRef moduleHandle, StringBuilder baseName, int size);

		// Token: 0x060015B4 RID: 5556
		[DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetModuleInformation(SafeProcessHandle processHandle, HandleRef moduleHandle, NativeMethods.NtModuleInfo ntModuleInfo, int size);

		// Token: 0x060015B5 RID: 5557
		[DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetModuleBaseName(SafeProcessHandle processHandle, HandleRef moduleHandle, StringBuilder baseName, int size);

		// Token: 0x060015B6 RID: 5558
		[DllImport("psapi.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetModuleFileNameEx(SafeProcessHandle processHandle, HandleRef moduleHandle, StringBuilder baseName, int size);

		// Token: 0x060015B7 RID: 5559
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x060015B8 RID: 5560
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetProcessWorkingSetSize(SafeProcessHandle handle, IntPtr min, IntPtr max);

		// Token: 0x060015B9 RID: 5561
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetProcessWorkingSetSize(SafeProcessHandle handle, out IntPtr min, out IntPtr max);

		// Token: 0x060015BA RID: 5562
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetProcessAffinityMask(SafeProcessHandle handle, IntPtr mask);

		// Token: 0x060015BB RID: 5563
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetProcessAffinityMask(SafeProcessHandle handle, out IntPtr processMask, out IntPtr systemMask);

		// Token: 0x060015BC RID: 5564
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetThreadPriorityBoost(SafeThreadHandle handle, out bool disabled);

		// Token: 0x060015BD RID: 5565
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetThreadPriorityBoost(SafeThreadHandle handle, bool disabled);

		// Token: 0x060015BE RID: 5566
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetProcessPriorityBoost(SafeProcessHandle handle, out bool disabled);

		// Token: 0x060015BF RID: 5567
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetProcessPriorityBoost(SafeProcessHandle handle, bool disabled);

		// Token: 0x060015C0 RID: 5568
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern SafeThreadHandle OpenThread(int access, bool inherit, int threadId);

		// Token: 0x060015C1 RID: 5569
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetThreadPriority(SafeThreadHandle handle, int priority);

		// Token: 0x060015C2 RID: 5570
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetThreadPriority(SafeThreadHandle handle);

		// Token: 0x060015C3 RID: 5571
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetThreadAffinityMask(SafeThreadHandle handle, HandleRef mask);

		// Token: 0x060015C4 RID: 5572
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int SetThreadIdealProcessor(SafeThreadHandle handle, int processor);

		// Token: 0x060015C5 RID: 5573
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateToolhelp32Snapshot(int flags, int processId);

		// Token: 0x060015C6 RID: 5574
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool Process32First(HandleRef handle, IntPtr entry);

		// Token: 0x060015C7 RID: 5575
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool Process32Next(HandleRef handle, IntPtr entry);

		// Token: 0x060015C8 RID: 5576
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool Thread32First(HandleRef handle, NativeMethods.WinThreadEntry entry);

		// Token: 0x060015C9 RID: 5577
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool Thread32Next(HandleRef handle, NativeMethods.WinThreadEntry entry);

		// Token: 0x060015CA RID: 5578
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool Module32First(HandleRef handle, IntPtr entry);

		// Token: 0x060015CB RID: 5579
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool Module32Next(HandleRef handle, IntPtr entry);

		// Token: 0x060015CC RID: 5580
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetPriorityClass(SafeProcessHandle handle);

		// Token: 0x060015CD RID: 5581
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetPriorityClass(SafeProcessHandle handle, int priorityClass);

		// Token: 0x060015CE RID: 5582
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumWindows(NativeMethods.EnumThreadWindowsCallback callback, IntPtr extraData);

		// Token: 0x060015CF RID: 5583
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

		// Token: 0x060015D0 RID: 5584
		[DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ShellExecuteEx(NativeMethods.ShellExecuteInfo info);

		// Token: 0x060015D1 RID: 5585
		[DllImport("ntdll.dll", CharSet = CharSet.Auto)]
		public static extern int NtQueryInformationProcess(SafeProcessHandle processHandle, int query, NativeMethods.NtProcessBasicInfo info, int size, int[] returnedSize);

		// Token: 0x060015D2 RID: 5586
		[DllImport("ntdll.dll", CharSet = CharSet.Auto)]
		public static extern int NtQuerySystemInformation(int query, IntPtr dataPtr, int size, out int returnedSize);

		// Token: 0x060015D3 RID: 5587
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, NativeMethods.SECURITY_ATTRIBUTES lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, SafeFileHandle hTemplateFile);

		// Token: 0x060015D4 RID: 5588
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeFileHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions);

		// Token: 0x060015D5 RID: 5589
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern bool DuplicateHandle(HandleRef hSourceProcessHandle, SafeHandle hSourceHandle, HandleRef hTargetProcess, out SafeWaitHandle targetHandle, int dwDesiredAccess, bool bInheritHandle, int dwOptions);

		// Token: 0x060015D6 RID: 5590
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool OpenProcessToken(HandleRef ProcessHandle, int DesiredAccess, out IntPtr TokenHandle);

		// Token: 0x060015D7 RID: 5591
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool LookupPrivilegeValue([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpName, out NativeMethods.LUID lpLuid);

		// Token: 0x060015D8 RID: 5592
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AdjustTokenPrivileges(HandleRef TokenHandle, bool DisableAllPrivileges, NativeMethods.TokenPrivileges NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

		// Token: 0x060015D9 RID: 5593
		[DllImport("user32.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
		public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x060015DA RID: 5594
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowTextLength(HandleRef hWnd);

		// Token: 0x060015DB RID: 5595
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool IsWindowVisible(HandleRef hWnd);

		// Token: 0x060015DC RID: 5596
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int timeout, out IntPtr pdwResult);

		// Token: 0x060015DD RID: 5597
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowLong(HandleRef hWnd, int nIndex);

		// Token: 0x060015DE RID: 5598
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern int PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

		// Token: 0x060015DF RID: 5599
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

		// Token: 0x060015E0 RID: 5600
		[DllImport("mscoree.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		internal static extern uint GetRequestedRuntimeInfo(string pExe, string pwszVersion, string pConfigurationFile, uint startupFlags, uint reserved, StringBuilder pDirectory, int dwDirectory, out uint dwDirectoryLength, StringBuilder pVersion, int cchBuffer, out uint dwlength);

		// Token: 0x060015E1 RID: 5601
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr VirtualQuery(SafeFileMapViewHandle address, ref NativeMethods.MEMORY_BASIC_INFORMATION buffer, IntPtr sizeOfBuffer);

		// Token: 0x04001250 RID: 4688
		public const int GENERIC_READ = -2147483648;

		// Token: 0x04001251 RID: 4689
		public const int GENERIC_WRITE = 1073741824;

		// Token: 0x04001252 RID: 4690
		public const int FILE_SHARE_READ = 1;

		// Token: 0x04001253 RID: 4691
		public const int FILE_SHARE_WRITE = 2;

		// Token: 0x04001254 RID: 4692
		public const int FILE_SHARE_DELETE = 4;

		// Token: 0x04001255 RID: 4693
		public const int S_OK = 0;

		// Token: 0x04001256 RID: 4694
		public const int E_ABORT = -2147467260;

		// Token: 0x04001257 RID: 4695
		public const int E_NOTIMPL = -2147467263;

		// Token: 0x04001258 RID: 4696
		public const int CREATE_ALWAYS = 2;

		// Token: 0x04001259 RID: 4697
		public const int FILE_ATTRIBUTE_NORMAL = 128;

		// Token: 0x0400125A RID: 4698
		public const int STARTF_USESTDHANDLES = 256;

		// Token: 0x0400125B RID: 4699
		public const int STD_INPUT_HANDLE = -10;

		// Token: 0x0400125C RID: 4700
		public const int STD_OUTPUT_HANDLE = -11;

		// Token: 0x0400125D RID: 4701
		public const int STD_ERROR_HANDLE = -12;

		// Token: 0x0400125E RID: 4702
		public const int STILL_ACTIVE = 259;

		// Token: 0x0400125F RID: 4703
		public const int SW_HIDE = 0;

		// Token: 0x04001260 RID: 4704
		public const int WAIT_OBJECT_0 = 0;

		// Token: 0x04001261 RID: 4705
		public const int WAIT_FAILED = -1;

		// Token: 0x04001262 RID: 4706
		public const int WAIT_TIMEOUT = 258;

		// Token: 0x04001263 RID: 4707
		public const int WAIT_ABANDONED = 128;

		// Token: 0x04001264 RID: 4708
		public const int WAIT_ABANDONED_0 = 128;

		// Token: 0x04001265 RID: 4709
		public const int MOVEFILE_REPLACE_EXISTING = 1;

		// Token: 0x04001266 RID: 4710
		public const int ERROR_CLASS_ALREADY_EXISTS = 1410;

		// Token: 0x04001267 RID: 4711
		public const int ERROR_NONE_MAPPED = 1332;

		// Token: 0x04001268 RID: 4712
		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x04001269 RID: 4713
		public const int ERROR_PROC_NOT_FOUND = 127;

		// Token: 0x0400126A RID: 4714
		public const int ERROR_BAD_EXE_FORMAT = 193;

		// Token: 0x0400126B RID: 4715
		public const int ERROR_INVALID_NAME = 123;

		// Token: 0x0400126C RID: 4716
		public const int MAX_PATH = 260;

		// Token: 0x0400126D RID: 4717
		public const int UIS_SET = 1;

		// Token: 0x0400126E RID: 4718
		public const int WSF_VISIBLE = 1;

		// Token: 0x0400126F RID: 4719
		public const int UIS_CLEAR = 2;

		// Token: 0x04001270 RID: 4720
		public const int UISF_HIDEFOCUS = 1;

		// Token: 0x04001271 RID: 4721
		public const int UISF_HIDEACCEL = 2;

		// Token: 0x04001272 RID: 4722
		public const int USERCLASSTYPE_FULL = 1;

		// Token: 0x04001273 RID: 4723
		public const int UOI_FLAGS = 1;

		// Token: 0x04001274 RID: 4724
		public const int DEFAULT_GUI_FONT = 17;

		// Token: 0x04001275 RID: 4725
		public const int SM_CYSCREEN = 1;

		// Token: 0x04001276 RID: 4726
		public const int COLOR_WINDOW = 5;

		// Token: 0x04001277 RID: 4727
		public const int WS_POPUP = -2147483648;

		// Token: 0x04001278 RID: 4728
		public const int WS_VISIBLE = 268435456;

		// Token: 0x04001279 RID: 4729
		public const int WM_SETTINGCHANGE = 26;

		// Token: 0x0400127A RID: 4730
		public const int WM_SYSCOLORCHANGE = 21;

		// Token: 0x0400127B RID: 4731
		public const int WM_QUERYENDSESSION = 17;

		// Token: 0x0400127C RID: 4732
		public const int WM_QUIT = 18;

		// Token: 0x0400127D RID: 4733
		public const int WM_ENDSESSION = 22;

		// Token: 0x0400127E RID: 4734
		public const int WM_POWERBROADCAST = 536;

		// Token: 0x0400127F RID: 4735
		public const int WM_COMPACTING = 65;

		// Token: 0x04001280 RID: 4736
		public const int WM_DISPLAYCHANGE = 126;

		// Token: 0x04001281 RID: 4737
		public const int WM_FONTCHANGE = 29;

		// Token: 0x04001282 RID: 4738
		public const int WM_PALETTECHANGED = 785;

		// Token: 0x04001283 RID: 4739
		public const int WM_TIMECHANGE = 30;

		// Token: 0x04001284 RID: 4740
		public const int WM_THEMECHANGED = 794;

		// Token: 0x04001285 RID: 4741
		public const int WM_WTSSESSION_CHANGE = 689;

		// Token: 0x04001286 RID: 4742
		public const int ENDSESSION_LOGOFF = -2147483648;

		// Token: 0x04001287 RID: 4743
		public const int WM_TIMER = 275;

		// Token: 0x04001288 RID: 4744
		public const int WM_USER = 1024;

		// Token: 0x04001289 RID: 4745
		public const int WM_CREATETIMER = 1025;

		// Token: 0x0400128A RID: 4746
		public const int WM_KILLTIMER = 1026;

		// Token: 0x0400128B RID: 4747
		public const int WM_REFLECT = 8192;

		// Token: 0x0400128C RID: 4748
		public const int WTS_CONSOLE_CONNECT = 1;

		// Token: 0x0400128D RID: 4749
		public const int WTS_CONSOLE_DISCONNECT = 2;

		// Token: 0x0400128E RID: 4750
		public const int WTS_REMOTE_CONNECT = 3;

		// Token: 0x0400128F RID: 4751
		public const int WTS_REMOTE_DISCONNECT = 4;

		// Token: 0x04001290 RID: 4752
		public const int WTS_SESSION_LOGON = 5;

		// Token: 0x04001291 RID: 4753
		public const int WTS_SESSION_LOGOFF = 6;

		// Token: 0x04001292 RID: 4754
		public const int WTS_SESSION_LOCK = 7;

		// Token: 0x04001293 RID: 4755
		public const int WTS_SESSION_UNLOCK = 8;

		// Token: 0x04001294 RID: 4756
		public const int WTS_SESSION_REMOTE_CONTROL = 9;

		// Token: 0x04001295 RID: 4757
		public const int NOTIFY_FOR_THIS_SESSION = 0;

		// Token: 0x04001296 RID: 4758
		public const int CTRL_C_EVENT = 0;

		// Token: 0x04001297 RID: 4759
		public const int CTRL_BREAK_EVENT = 1;

		// Token: 0x04001298 RID: 4760
		public const int CTRL_CLOSE_EVENT = 2;

		// Token: 0x04001299 RID: 4761
		public const int CTRL_LOGOFF_EVENT = 5;

		// Token: 0x0400129A RID: 4762
		public const int CTRL_SHUTDOWN_EVENT = 6;

		// Token: 0x0400129B RID: 4763
		public const int SPI_GETBEEP = 1;

		// Token: 0x0400129C RID: 4764
		public const int SPI_SETBEEP = 2;

		// Token: 0x0400129D RID: 4765
		public const int SPI_GETMOUSE = 3;

		// Token: 0x0400129E RID: 4766
		public const int SPI_SETMOUSE = 4;

		// Token: 0x0400129F RID: 4767
		public const int SPI_GETBORDER = 5;

		// Token: 0x040012A0 RID: 4768
		public const int SPI_SETBORDER = 6;

		// Token: 0x040012A1 RID: 4769
		public const int SPI_GETKEYBOARDSPEED = 10;

		// Token: 0x040012A2 RID: 4770
		public const int SPI_SETKEYBOARDSPEED = 11;

		// Token: 0x040012A3 RID: 4771
		public const int SPI_LANGDRIVER = 12;

		// Token: 0x040012A4 RID: 4772
		public const int SPI_ICONHORIZONTALSPACING = 13;

		// Token: 0x040012A5 RID: 4773
		public const int SPI_GETSCREENSAVETIMEOUT = 14;

		// Token: 0x040012A6 RID: 4774
		public const int SPI_SETSCREENSAVETIMEOUT = 15;

		// Token: 0x040012A7 RID: 4775
		public const int SPI_GETSCREENSAVEACTIVE = 16;

		// Token: 0x040012A8 RID: 4776
		public const int SPI_SETSCREENSAVEACTIVE = 17;

		// Token: 0x040012A9 RID: 4777
		public const int SPI_GETGRIDGRANULARITY = 18;

		// Token: 0x040012AA RID: 4778
		public const int SPI_SETGRIDGRANULARITY = 19;

		// Token: 0x040012AB RID: 4779
		public const int SPI_SETDESKWALLPAPER = 20;

		// Token: 0x040012AC RID: 4780
		public const int SPI_SETDESKPATTERN = 21;

		// Token: 0x040012AD RID: 4781
		public const int SPI_GETKEYBOARDDELAY = 22;

		// Token: 0x040012AE RID: 4782
		public const int SPI_SETKEYBOARDDELAY = 23;

		// Token: 0x040012AF RID: 4783
		public const int SPI_ICONVERTICALSPACING = 24;

		// Token: 0x040012B0 RID: 4784
		public const int SPI_GETICONTITLEWRAP = 25;

		// Token: 0x040012B1 RID: 4785
		public const int SPI_SETICONTITLEWRAP = 26;

		// Token: 0x040012B2 RID: 4786
		public const int SPI_GETMENUDROPALIGNMENT = 27;

		// Token: 0x040012B3 RID: 4787
		public const int SPI_SETMENUDROPALIGNMENT = 28;

		// Token: 0x040012B4 RID: 4788
		public const int SPI_SETDOUBLECLKWIDTH = 29;

		// Token: 0x040012B5 RID: 4789
		public const int SPI_SETDOUBLECLKHEIGHT = 30;

		// Token: 0x040012B6 RID: 4790
		public const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x040012B7 RID: 4791
		public const int SPI_SETDOUBLECLICKTIME = 32;

		// Token: 0x040012B8 RID: 4792
		public const int SPI_SETMOUSEBUTTONSWAP = 33;

		// Token: 0x040012B9 RID: 4793
		public const int SPI_SETICONTITLELOGFONT = 34;

		// Token: 0x040012BA RID: 4794
		public const int SPI_GETFASTTASKSWITCH = 35;

		// Token: 0x040012BB RID: 4795
		public const int SPI_SETFASTTASKSWITCH = 36;

		// Token: 0x040012BC RID: 4796
		public const int SPI_SETDRAGFULLWINDOWS = 37;

		// Token: 0x040012BD RID: 4797
		public const int SPI_GETDRAGFULLWINDOWS = 38;

		// Token: 0x040012BE RID: 4798
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x040012BF RID: 4799
		public const int SPI_SETNONCLIENTMETRICS = 42;

		// Token: 0x040012C0 RID: 4800
		public const int SPI_GETMINIMIZEDMETRICS = 43;

		// Token: 0x040012C1 RID: 4801
		public const int SPI_SETMINIMIZEDMETRICS = 44;

		// Token: 0x040012C2 RID: 4802
		public const int SPI_GETICONMETRICS = 45;

		// Token: 0x040012C3 RID: 4803
		public const int SPI_SETICONMETRICS = 46;

		// Token: 0x040012C4 RID: 4804
		public const int SPI_SETWORKAREA = 47;

		// Token: 0x040012C5 RID: 4805
		public const int SPI_GETWORKAREA = 48;

		// Token: 0x040012C6 RID: 4806
		public const int SPI_SETPENWINDOWS = 49;

		// Token: 0x040012C7 RID: 4807
		public const int SPI_GETHIGHCONTRAST = 66;

		// Token: 0x040012C8 RID: 4808
		public const int SPI_SETHIGHCONTRAST = 67;

		// Token: 0x040012C9 RID: 4809
		public const int SPI_GETKEYBOARDPREF = 68;

		// Token: 0x040012CA RID: 4810
		public const int SPI_SETKEYBOARDPREF = 69;

		// Token: 0x040012CB RID: 4811
		public const int SPI_GETSCREENREADER = 70;

		// Token: 0x040012CC RID: 4812
		public const int SPI_SETSCREENREADER = 71;

		// Token: 0x040012CD RID: 4813
		public const int SPI_GETANIMATION = 72;

		// Token: 0x040012CE RID: 4814
		public const int SPI_SETANIMATION = 73;

		// Token: 0x040012CF RID: 4815
		public const int SPI_GETFONTSMOOTHING = 74;

		// Token: 0x040012D0 RID: 4816
		public const int SPI_SETFONTSMOOTHING = 75;

		// Token: 0x040012D1 RID: 4817
		public const int SPI_SETDRAGWIDTH = 76;

		// Token: 0x040012D2 RID: 4818
		public const int SPI_SETDRAGHEIGHT = 77;

		// Token: 0x040012D3 RID: 4819
		public const int SPI_SETHANDHELD = 78;

		// Token: 0x040012D4 RID: 4820
		public const int SPI_GETLOWPOWERTIMEOUT = 79;

		// Token: 0x040012D5 RID: 4821
		public const int SPI_GETPOWEROFFTIMEOUT = 80;

		// Token: 0x040012D6 RID: 4822
		public const int SPI_SETLOWPOWERTIMEOUT = 81;

		// Token: 0x040012D7 RID: 4823
		public const int SPI_SETPOWEROFFTIMEOUT = 82;

		// Token: 0x040012D8 RID: 4824
		public const int SPI_GETLOWPOWERACTIVE = 83;

		// Token: 0x040012D9 RID: 4825
		public const int SPI_GETPOWEROFFACTIVE = 84;

		// Token: 0x040012DA RID: 4826
		public const int SPI_SETLOWPOWERACTIVE = 85;

		// Token: 0x040012DB RID: 4827
		public const int SPI_SETPOWEROFFACTIVE = 86;

		// Token: 0x040012DC RID: 4828
		public const int SPI_SETCURSORS = 87;

		// Token: 0x040012DD RID: 4829
		public const int SPI_SETICONS = 88;

		// Token: 0x040012DE RID: 4830
		public const int SPI_GETDEFAULTINPUTLANG = 89;

		// Token: 0x040012DF RID: 4831
		public const int SPI_SETDEFAULTINPUTLANG = 90;

		// Token: 0x040012E0 RID: 4832
		public const int SPI_SETLANGTOGGLE = 91;

		// Token: 0x040012E1 RID: 4833
		public const int SPI_GETWINDOWSEXTENSION = 92;

		// Token: 0x040012E2 RID: 4834
		public const int SPI_SETMOUSETRAILS = 93;

		// Token: 0x040012E3 RID: 4835
		public const int SPI_GETMOUSETRAILS = 94;

		// Token: 0x040012E4 RID: 4836
		public const int SPI_SETSCREENSAVERRUNNING = 97;

		// Token: 0x040012E5 RID: 4837
		public const int SPI_SCREENSAVERRUNNING = 97;

		// Token: 0x040012E6 RID: 4838
		public const int SPI_GETFILTERKEYS = 50;

		// Token: 0x040012E7 RID: 4839
		public const int SPI_SETFILTERKEYS = 51;

		// Token: 0x040012E8 RID: 4840
		public const int SPI_GETTOGGLEKEYS = 52;

		// Token: 0x040012E9 RID: 4841
		public const int SPI_SETTOGGLEKEYS = 53;

		// Token: 0x040012EA RID: 4842
		public const int SPI_GETMOUSEKEYS = 54;

		// Token: 0x040012EB RID: 4843
		public const int SPI_SETMOUSEKEYS = 55;

		// Token: 0x040012EC RID: 4844
		public const int SPI_GETSHOWSOUNDS = 56;

		// Token: 0x040012ED RID: 4845
		public const int SPI_SETSHOWSOUNDS = 57;

		// Token: 0x040012EE RID: 4846
		public const int SPI_GETSTICKYKEYS = 58;

		// Token: 0x040012EF RID: 4847
		public const int SPI_SETSTICKYKEYS = 59;

		// Token: 0x040012F0 RID: 4848
		public const int SPI_GETACCESSTIMEOUT = 60;

		// Token: 0x040012F1 RID: 4849
		public const int SPI_SETACCESSTIMEOUT = 61;

		// Token: 0x040012F2 RID: 4850
		public const int SPI_GETSERIALKEYS = 62;

		// Token: 0x040012F3 RID: 4851
		public const int SPI_SETSERIALKEYS = 63;

		// Token: 0x040012F4 RID: 4852
		public const int SPI_GETSOUNDSENTRY = 64;

		// Token: 0x040012F5 RID: 4853
		public const int SPI_SETSOUNDSENTRY = 65;

		// Token: 0x040012F6 RID: 4854
		public const int SPI_GETSNAPTODEFBUTTON = 95;

		// Token: 0x040012F7 RID: 4855
		public const int SPI_SETSNAPTODEFBUTTON = 96;

		// Token: 0x040012F8 RID: 4856
		public const int SPI_GETMOUSEHOVERWIDTH = 98;

		// Token: 0x040012F9 RID: 4857
		public const int SPI_SETMOUSEHOVERWIDTH = 99;

		// Token: 0x040012FA RID: 4858
		public const int SPI_GETMOUSEHOVERHEIGHT = 100;

		// Token: 0x040012FB RID: 4859
		public const int SPI_SETMOUSEHOVERHEIGHT = 101;

		// Token: 0x040012FC RID: 4860
		public const int SPI_GETMOUSEHOVERTIME = 102;

		// Token: 0x040012FD RID: 4861
		public const int SPI_SETMOUSEHOVERTIME = 103;

		// Token: 0x040012FE RID: 4862
		public const int SPI_GETWHEELSCROLLLINES = 104;

		// Token: 0x040012FF RID: 4863
		public const int SPI_SETWHEELSCROLLLINES = 105;

		// Token: 0x04001300 RID: 4864
		public const int SPI_GETMENUSHOWDELAY = 106;

		// Token: 0x04001301 RID: 4865
		public const int SPI_SETMENUSHOWDELAY = 107;

		// Token: 0x04001302 RID: 4866
		public const int SPI_GETSHOWIMEUI = 110;

		// Token: 0x04001303 RID: 4867
		public const int SPI_SETSHOWIMEUI = 111;

		// Token: 0x04001304 RID: 4868
		public const int SPI_GETMOUSESPEED = 112;

		// Token: 0x04001305 RID: 4869
		public const int SPI_SETMOUSESPEED = 113;

		// Token: 0x04001306 RID: 4870
		public const int SPI_GETSCREENSAVERRUNNING = 114;

		// Token: 0x04001307 RID: 4871
		public const int SPI_GETDESKWALLPAPER = 115;

		// Token: 0x04001308 RID: 4872
		public const int SPI_GETACTIVEWINDOWTRACKING = 4096;

		// Token: 0x04001309 RID: 4873
		public const int SPI_SETACTIVEWINDOWTRACKING = 4097;

		// Token: 0x0400130A RID: 4874
		public const int SPI_GETMENUANIMATION = 4098;

		// Token: 0x0400130B RID: 4875
		public const int SPI_SETMENUANIMATION = 4099;

		// Token: 0x0400130C RID: 4876
		public const int SPI_GETCOMBOBOXANIMATION = 4100;

		// Token: 0x0400130D RID: 4877
		public const int SPI_SETCOMBOBOXANIMATION = 4101;

		// Token: 0x0400130E RID: 4878
		public const int SPI_GETLISTBOXSMOOTHSCROLLING = 4102;

		// Token: 0x0400130F RID: 4879
		public const int SPI_SETLISTBOXSMOOTHSCROLLING = 4103;

		// Token: 0x04001310 RID: 4880
		public const int SPI_GETGRADIENTCAPTIONS = 4104;

		// Token: 0x04001311 RID: 4881
		public const int SPI_SETGRADIENTCAPTIONS = 4105;

		// Token: 0x04001312 RID: 4882
		public const int SPI_GETKEYBOARDCUES = 4106;

		// Token: 0x04001313 RID: 4883
		public const int SPI_SETKEYBOARDCUES = 4107;

		// Token: 0x04001314 RID: 4884
		public const int SPI_GETMENUUNDERLINES = 4106;

		// Token: 0x04001315 RID: 4885
		public const int SPI_SETMENUUNDERLINES = 4107;

		// Token: 0x04001316 RID: 4886
		public const int SPI_GETACTIVEWNDTRKZORDER = 4108;

		// Token: 0x04001317 RID: 4887
		public const int SPI_SETACTIVEWNDTRKZORDER = 4109;

		// Token: 0x04001318 RID: 4888
		public const int SPI_GETHOTTRACKING = 4110;

		// Token: 0x04001319 RID: 4889
		public const int SPI_SETHOTTRACKING = 4111;

		// Token: 0x0400131A RID: 4890
		public const int SPI_GETMENUFADE = 4114;

		// Token: 0x0400131B RID: 4891
		public const int SPI_SETMENUFADE = 4115;

		// Token: 0x0400131C RID: 4892
		public const int SPI_GETSELECTIONFADE = 4116;

		// Token: 0x0400131D RID: 4893
		public const int SPI_SETSELECTIONFADE = 4117;

		// Token: 0x0400131E RID: 4894
		public const int SPI_GETTOOLTIPANIMATION = 4118;

		// Token: 0x0400131F RID: 4895
		public const int SPI_SETTOOLTIPANIMATION = 4119;

		// Token: 0x04001320 RID: 4896
		public const int SPI_GETTOOLTIPFADE = 4120;

		// Token: 0x04001321 RID: 4897
		public const int SPI_SETTOOLTIPFADE = 4121;

		// Token: 0x04001322 RID: 4898
		public const int SPI_GETCURSORSHADOW = 4122;

		// Token: 0x04001323 RID: 4899
		public const int SPI_SETCURSORSHADOW = 4123;

		// Token: 0x04001324 RID: 4900
		public const int SPI_GETUIEFFECTS = 4158;

		// Token: 0x04001325 RID: 4901
		public const int SPI_SETUIEFFECTS = 4159;

		// Token: 0x04001326 RID: 4902
		public const int SPI_GETFOREGROUNDLOCKTIMEOUT = 8192;

		// Token: 0x04001327 RID: 4903
		public const int SPI_SETFOREGROUNDLOCKTIMEOUT = 8193;

		// Token: 0x04001328 RID: 4904
		public const int SPI_GETACTIVEWNDTRKTIMEOUT = 8194;

		// Token: 0x04001329 RID: 4905
		public const int SPI_SETACTIVEWNDTRKTIMEOUT = 8195;

		// Token: 0x0400132A RID: 4906
		public const int SPI_GETFOREGROUNDFLASHCOUNT = 8196;

		// Token: 0x0400132B RID: 4907
		public const int SPI_SETFOREGROUNDFLASHCOUNT = 8197;

		// Token: 0x0400132C RID: 4908
		public const int SPI_GETCARETWIDTH = 8198;

		// Token: 0x0400132D RID: 4909
		public const int SPI_SETCARETWIDTH = 8199;

		// Token: 0x0400132E RID: 4910
		public const uint STATUS_INFO_LENGTH_MISMATCH = 3221225476U;

		// Token: 0x0400132F RID: 4911
		public const int PBT_APMQUERYSUSPEND = 0;

		// Token: 0x04001330 RID: 4912
		public const int PBT_APMQUERYSTANDBY = 1;

		// Token: 0x04001331 RID: 4913
		public const int PBT_APMQUERYSUSPENDFAILED = 2;

		// Token: 0x04001332 RID: 4914
		public const int PBT_APMQUERYSTANDBYFAILED = 3;

		// Token: 0x04001333 RID: 4915
		public const int PBT_APMSUSPEND = 4;

		// Token: 0x04001334 RID: 4916
		public const int PBT_APMSTANDBY = 5;

		// Token: 0x04001335 RID: 4917
		public const int PBT_APMRESUMECRITICAL = 6;

		// Token: 0x04001336 RID: 4918
		public const int PBT_APMRESUMESUSPEND = 7;

		// Token: 0x04001337 RID: 4919
		public const int PBT_APMRESUMESTANDBY = 8;

		// Token: 0x04001338 RID: 4920
		public const int PBT_APMBATTERYLOW = 9;

		// Token: 0x04001339 RID: 4921
		public const int PBT_APMPOWERSTATUSCHANGE = 10;

		// Token: 0x0400133A RID: 4922
		public const int PBT_APMOEMEVENT = 11;

		// Token: 0x0400133B RID: 4923
		public const int STARTF_USESHOWWINDOW = 1;

		// Token: 0x0400133C RID: 4924
		public const int FILE_MAP_WRITE = 2;

		// Token: 0x0400133D RID: 4925
		public const int FILE_MAP_READ = 4;

		// Token: 0x0400133E RID: 4926
		public const int PAGE_READWRITE = 4;

		// Token: 0x0400133F RID: 4927
		public const int GENERIC_EXECUTE = 536870912;

		// Token: 0x04001340 RID: 4928
		public const int GENERIC_ALL = 268435456;

		// Token: 0x04001341 RID: 4929
		public const int ERROR_NOT_READY = 21;

		// Token: 0x04001342 RID: 4930
		public const int ERROR_LOCK_FAILED = 167;

		// Token: 0x04001343 RID: 4931
		public const int ERROR_BUSY = 170;

		// Token: 0x04001344 RID: 4932
		public const int IMPERSONATION_LEVEL_SecurityAnonymous = 0;

		// Token: 0x04001345 RID: 4933
		public const int IMPERSONATION_LEVEL_SecurityIdentification = 1;

		// Token: 0x04001346 RID: 4934
		public const int IMPERSONATION_LEVEL_SecurityImpersonation = 2;

		// Token: 0x04001347 RID: 4935
		public const int IMPERSONATION_LEVEL_SecurityDelegation = 3;

		// Token: 0x04001348 RID: 4936
		public const int TOKEN_TYPE_TokenPrimary = 1;

		// Token: 0x04001349 RID: 4937
		public const int TOKEN_TYPE_TokenImpersonation = 2;

		// Token: 0x0400134A RID: 4938
		public const int TOKEN_ALL_ACCESS = 983551;

		// Token: 0x0400134B RID: 4939
		public const int TOKEN_EXECUTE = 131072;

		// Token: 0x0400134C RID: 4940
		public const int TOKEN_READ = 131080;

		// Token: 0x0400134D RID: 4941
		public const int TOKEN_IMPERSONATE = 4;

		// Token: 0x0400134E RID: 4942
		public const int PIPE_ACCESS_INBOUND = 1;

		// Token: 0x0400134F RID: 4943
		public const int PIPE_ACCESS_OUTBOUND = 2;

		// Token: 0x04001350 RID: 4944
		public const int PIPE_ACCESS_DUPLEX = 3;

		// Token: 0x04001351 RID: 4945
		public const int PIPE_WAIT = 0;

		// Token: 0x04001352 RID: 4946
		public const int PIPE_NOWAIT = 1;

		// Token: 0x04001353 RID: 4947
		public const int PIPE_READMODE_BYTE = 0;

		// Token: 0x04001354 RID: 4948
		public const int PIPE_READMODE_MESSAGE = 2;

		// Token: 0x04001355 RID: 4949
		public const int PIPE_TYPE_BYTE = 0;

		// Token: 0x04001356 RID: 4950
		public const int PIPE_TYPE_MESSAGE = 4;

		// Token: 0x04001357 RID: 4951
		public const int PIPE_SINGLE_INSTANCES = 1;

		// Token: 0x04001358 RID: 4952
		public const int PIPE_UNLIMITED_INSTANCES = 255;

		// Token: 0x04001359 RID: 4953
		public const int FILE_FLAG_OVERLAPPED = 1073741824;

		// Token: 0x0400135A RID: 4954
		public const int PM_REMOVE = 1;

		// Token: 0x0400135B RID: 4955
		public const int QS_KEY = 1;

		// Token: 0x0400135C RID: 4956
		public const int QS_MOUSEMOVE = 2;

		// Token: 0x0400135D RID: 4957
		public const int QS_MOUSEBUTTON = 4;

		// Token: 0x0400135E RID: 4958
		public const int QS_POSTMESSAGE = 8;

		// Token: 0x0400135F RID: 4959
		public const int QS_TIMER = 16;

		// Token: 0x04001360 RID: 4960
		public const int QS_PAINT = 32;

		// Token: 0x04001361 RID: 4961
		public const int QS_SENDMESSAGE = 64;

		// Token: 0x04001362 RID: 4962
		public const int QS_HOTKEY = 128;

		// Token: 0x04001363 RID: 4963
		public const int QS_ALLPOSTMESSAGE = 256;

		// Token: 0x04001364 RID: 4964
		public const int QS_MOUSE = 6;

		// Token: 0x04001365 RID: 4965
		public const int QS_INPUT = 7;

		// Token: 0x04001366 RID: 4966
		public const int QS_ALLEVENTS = 191;

		// Token: 0x04001367 RID: 4967
		public const int QS_ALLINPUT = 255;

		// Token: 0x04001368 RID: 4968
		public const int MWMO_INPUTAVAILABLE = 4;

		// Token: 0x04001369 RID: 4969
		internal const byte ONESTOPBIT = 0;

		// Token: 0x0400136A RID: 4970
		internal const byte ONE5STOPBITS = 1;

		// Token: 0x0400136B RID: 4971
		internal const byte TWOSTOPBITS = 2;

		// Token: 0x0400136C RID: 4972
		internal const int DTR_CONTROL_DISABLE = 0;

		// Token: 0x0400136D RID: 4973
		internal const int DTR_CONTROL_ENABLE = 1;

		// Token: 0x0400136E RID: 4974
		internal const int DTR_CONTROL_HANDSHAKE = 2;

		// Token: 0x0400136F RID: 4975
		internal const int RTS_CONTROL_DISABLE = 0;

		// Token: 0x04001370 RID: 4976
		internal const int RTS_CONTROL_ENABLE = 1;

		// Token: 0x04001371 RID: 4977
		internal const int RTS_CONTROL_HANDSHAKE = 2;

		// Token: 0x04001372 RID: 4978
		internal const int RTS_CONTROL_TOGGLE = 3;

		// Token: 0x04001373 RID: 4979
		internal const int MS_CTS_ON = 16;

		// Token: 0x04001374 RID: 4980
		internal const int MS_DSR_ON = 32;

		// Token: 0x04001375 RID: 4981
		internal const int MS_RING_ON = 64;

		// Token: 0x04001376 RID: 4982
		internal const int MS_RLSD_ON = 128;

		// Token: 0x04001377 RID: 4983
		internal const byte EOFCHAR = 26;

		// Token: 0x04001378 RID: 4984
		internal const int FBINARY = 0;

		// Token: 0x04001379 RID: 4985
		internal const int FPARITY = 1;

		// Token: 0x0400137A RID: 4986
		internal const int FOUTXCTSFLOW = 2;

		// Token: 0x0400137B RID: 4987
		internal const int FOUTXDSRFLOW = 3;

		// Token: 0x0400137C RID: 4988
		internal const int FDTRCONTROL = 4;

		// Token: 0x0400137D RID: 4989
		internal const int FDSRSENSITIVITY = 6;

		// Token: 0x0400137E RID: 4990
		internal const int FTXCONTINUEONXOFF = 7;

		// Token: 0x0400137F RID: 4991
		internal const int FOUTX = 8;

		// Token: 0x04001380 RID: 4992
		internal const int FINX = 9;

		// Token: 0x04001381 RID: 4993
		internal const int FERRORCHAR = 10;

		// Token: 0x04001382 RID: 4994
		internal const int FNULL = 11;

		// Token: 0x04001383 RID: 4995
		internal const int FRTSCONTROL = 12;

		// Token: 0x04001384 RID: 4996
		internal const int FABORTONOERROR = 14;

		// Token: 0x04001385 RID: 4997
		internal const int FDUMMY2 = 15;

		// Token: 0x04001386 RID: 4998
		internal const int PURGE_TXABORT = 1;

		// Token: 0x04001387 RID: 4999
		internal const int PURGE_RXABORT = 2;

		// Token: 0x04001388 RID: 5000
		internal const int PURGE_TXCLEAR = 4;

		// Token: 0x04001389 RID: 5001
		internal const int PURGE_RXCLEAR = 8;

		// Token: 0x0400138A RID: 5002
		internal const byte DEFAULTXONCHAR = 17;

		// Token: 0x0400138B RID: 5003
		internal const byte DEFAULTXOFFCHAR = 19;

		// Token: 0x0400138C RID: 5004
		internal const int SETRTS = 3;

		// Token: 0x0400138D RID: 5005
		internal const int CLRRTS = 4;

		// Token: 0x0400138E RID: 5006
		internal const int SETDTR = 5;

		// Token: 0x0400138F RID: 5007
		internal const int CLRDTR = 6;

		// Token: 0x04001390 RID: 5008
		internal const int EV_RXCHAR = 1;

		// Token: 0x04001391 RID: 5009
		internal const int EV_RXFLAG = 2;

		// Token: 0x04001392 RID: 5010
		internal const int EV_CTS = 8;

		// Token: 0x04001393 RID: 5011
		internal const int EV_DSR = 16;

		// Token: 0x04001394 RID: 5012
		internal const int EV_RLSD = 32;

		// Token: 0x04001395 RID: 5013
		internal const int EV_BREAK = 64;

		// Token: 0x04001396 RID: 5014
		internal const int EV_ERR = 128;

		// Token: 0x04001397 RID: 5015
		internal const int EV_RING = 256;

		// Token: 0x04001398 RID: 5016
		internal const int ALL_EVENTS = 507;

		// Token: 0x04001399 RID: 5017
		internal const int CE_RXOVER = 1;

		// Token: 0x0400139A RID: 5018
		internal const int CE_OVERRUN = 2;

		// Token: 0x0400139B RID: 5019
		internal const int CE_PARITY = 4;

		// Token: 0x0400139C RID: 5020
		internal const int CE_FRAME = 8;

		// Token: 0x0400139D RID: 5021
		internal const int CE_BREAK = 16;

		// Token: 0x0400139E RID: 5022
		internal const int CE_TXFULL = 256;

		// Token: 0x0400139F RID: 5023
		internal const int MAXDWORD = -1;

		// Token: 0x040013A0 RID: 5024
		internal const int NOPARITY = 0;

		// Token: 0x040013A1 RID: 5025
		internal const int ODDPARITY = 1;

		// Token: 0x040013A2 RID: 5026
		internal const int EVENPARITY = 2;

		// Token: 0x040013A3 RID: 5027
		internal const int MARKPARITY = 3;

		// Token: 0x040013A4 RID: 5028
		internal const int SPACEPARITY = 4;

		// Token: 0x040013A5 RID: 5029
		internal const int SDDL_REVISION_1 = 1;

		// Token: 0x040013A6 RID: 5030
		public const int SECURITY_DESCRIPTOR_REVISION = 1;

		// Token: 0x040013A7 RID: 5031
		public const int HKEY_PERFORMANCE_DATA = -2147483644;

		// Token: 0x040013A8 RID: 5032
		public const int DWORD_SIZE = 4;

		// Token: 0x040013A9 RID: 5033
		public const int LARGE_INTEGER_SIZE = 8;

		// Token: 0x040013AA RID: 5034
		public const int PERF_NO_INSTANCES = -1;

		// Token: 0x040013AB RID: 5035
		public const int PERF_SIZE_DWORD = 0;

		// Token: 0x040013AC RID: 5036
		public const int PERF_SIZE_LARGE = 256;

		// Token: 0x040013AD RID: 5037
		public const int PERF_SIZE_ZERO = 512;

		// Token: 0x040013AE RID: 5038
		public const int PERF_SIZE_VARIABLE_LEN = 768;

		// Token: 0x040013AF RID: 5039
		public const int PERF_NO_UNIQUE_ID = -1;

		// Token: 0x040013B0 RID: 5040
		public const int PERF_TYPE_NUMBER = 0;

		// Token: 0x040013B1 RID: 5041
		public const int PERF_TYPE_COUNTER = 1024;

		// Token: 0x040013B2 RID: 5042
		public const int PERF_TYPE_TEXT = 2048;

		// Token: 0x040013B3 RID: 5043
		public const int PERF_TYPE_ZERO = 3072;

		// Token: 0x040013B4 RID: 5044
		public const int PERF_NUMBER_HEX = 0;

		// Token: 0x040013B5 RID: 5045
		public const int PERF_NUMBER_DECIMAL = 65536;

		// Token: 0x040013B6 RID: 5046
		public const int PERF_NUMBER_DEC_1000 = 131072;

		// Token: 0x040013B7 RID: 5047
		public const int PERF_COUNTER_VALUE = 0;

		// Token: 0x040013B8 RID: 5048
		public const int PERF_COUNTER_RATE = 65536;

		// Token: 0x040013B9 RID: 5049
		public const int PERF_COUNTER_FRACTION = 131072;

		// Token: 0x040013BA RID: 5050
		public const int PERF_COUNTER_BASE = 196608;

		// Token: 0x040013BB RID: 5051
		public const int PERF_COUNTER_ELAPSED = 262144;

		// Token: 0x040013BC RID: 5052
		public const int PERF_COUNTER_QUEUELEN = 327680;

		// Token: 0x040013BD RID: 5053
		public const int PERF_COUNTER_HISTOGRAM = 393216;

		// Token: 0x040013BE RID: 5054
		public const int PERF_COUNTER_PRECISION = 458752;

		// Token: 0x040013BF RID: 5055
		public const int PERF_TEXT_UNICODE = 0;

		// Token: 0x040013C0 RID: 5056
		public const int PERF_TEXT_ASCII = 65536;

		// Token: 0x040013C1 RID: 5057
		public const int PERF_TIMER_TICK = 0;

		// Token: 0x040013C2 RID: 5058
		public const int PERF_TIMER_100NS = 1048576;

		// Token: 0x040013C3 RID: 5059
		public const int PERF_OBJECT_TIMER = 2097152;

		// Token: 0x040013C4 RID: 5060
		public const int PERF_DELTA_COUNTER = 4194304;

		// Token: 0x040013C5 RID: 5061
		public const int PERF_DELTA_BASE = 8388608;

		// Token: 0x040013C6 RID: 5062
		public const int PERF_INVERSE_COUNTER = 16777216;

		// Token: 0x040013C7 RID: 5063
		public const int PERF_MULTI_COUNTER = 33554432;

		// Token: 0x040013C8 RID: 5064
		public const int PERF_DISPLAY_NO_SUFFIX = 0;

		// Token: 0x040013C9 RID: 5065
		public const int PERF_DISPLAY_PER_SEC = 268435456;

		// Token: 0x040013CA RID: 5066
		public const int PERF_DISPLAY_PERCENT = 536870912;

		// Token: 0x040013CB RID: 5067
		public const int PERF_DISPLAY_SECONDS = 805306368;

		// Token: 0x040013CC RID: 5068
		public const int PERF_DISPLAY_NOSHOW = 1073741824;

		// Token: 0x040013CD RID: 5069
		public const int PERF_COUNTER_COUNTER = 272696320;

		// Token: 0x040013CE RID: 5070
		public const int PERF_COUNTER_TIMER = 541132032;

		// Token: 0x040013CF RID: 5071
		public const int PERF_COUNTER_QUEUELEN_TYPE = 4523008;

		// Token: 0x040013D0 RID: 5072
		public const int PERF_COUNTER_LARGE_QUEUELEN_TYPE = 4523264;

		// Token: 0x040013D1 RID: 5073
		public const int PERF_COUNTER_100NS_QUEUELEN_TYPE = 5571840;

		// Token: 0x040013D2 RID: 5074
		public const int PERF_COUNTER_OBJ_TIME_QUEUELEN_TYPE = 6620416;

		// Token: 0x040013D3 RID: 5075
		public const int PERF_COUNTER_BULK_COUNT = 272696576;

		// Token: 0x040013D4 RID: 5076
		public const int PERF_COUNTER_TEXT = 2816;

		// Token: 0x040013D5 RID: 5077
		public const int PERF_COUNTER_RAWCOUNT = 65536;

		// Token: 0x040013D6 RID: 5078
		public const int PERF_COUNTER_LARGE_RAWCOUNT = 65792;

		// Token: 0x040013D7 RID: 5079
		public const int PERF_COUNTER_RAWCOUNT_HEX = 0;

		// Token: 0x040013D8 RID: 5080
		public const int PERF_COUNTER_LARGE_RAWCOUNT_HEX = 256;

		// Token: 0x040013D9 RID: 5081
		public const int PERF_SAMPLE_FRACTION = 549585920;

		// Token: 0x040013DA RID: 5082
		public const int PERF_SAMPLE_COUNTER = 4260864;

		// Token: 0x040013DB RID: 5083
		public const int PERF_COUNTER_NODATA = 1073742336;

		// Token: 0x040013DC RID: 5084
		public const int PERF_COUNTER_TIMER_INV = 557909248;

		// Token: 0x040013DD RID: 5085
		public const int PERF_SAMPLE_BASE = 1073939457;

		// Token: 0x040013DE RID: 5086
		public const int PERF_AVERAGE_TIMER = 805438464;

		// Token: 0x040013DF RID: 5087
		public const int PERF_AVERAGE_BASE = 1073939458;

		// Token: 0x040013E0 RID: 5088
		public const int PERF_OBJ_TIME_TIMER = 543229184;

		// Token: 0x040013E1 RID: 5089
		public const int PERF_AVERAGE_BULK = 1073874176;

		// Token: 0x040013E2 RID: 5090
		public const int PERF_OBJ_TIME_TIME = 543229184;

		// Token: 0x040013E3 RID: 5091
		public const int PERF_100NSEC_TIMER = 542180608;

		// Token: 0x040013E4 RID: 5092
		public const int PERF_100NSEC_TIMER_INV = 558957824;

		// Token: 0x040013E5 RID: 5093
		public const int PERF_COUNTER_MULTI_TIMER = 574686464;

		// Token: 0x040013E6 RID: 5094
		public const int PERF_COUNTER_MULTI_TIMER_INV = 591463680;

		// Token: 0x040013E7 RID: 5095
		public const int PERF_COUNTER_MULTI_BASE = 1107494144;

		// Token: 0x040013E8 RID: 5096
		public const int PERF_100NSEC_MULTI_TIMER = 575735040;

		// Token: 0x040013E9 RID: 5097
		public const int PERF_100NSEC_MULTI_TIMER_INV = 592512256;

		// Token: 0x040013EA RID: 5098
		public const int PERF_RAW_FRACTION = 537003008;

		// Token: 0x040013EB RID: 5099
		public const int PERF_LARGE_RAW_FRACTION = 537003264;

		// Token: 0x040013EC RID: 5100
		public const int PERF_RAW_BASE = 1073939459;

		// Token: 0x040013ED RID: 5101
		public const int PERF_LARGE_RAW_BASE = 1073939712;

		// Token: 0x040013EE RID: 5102
		public const int PERF_ELAPSED_TIME = 807666944;

		// Token: 0x040013EF RID: 5103
		public const int PERF_COUNTER_DELTA = 4195328;

		// Token: 0x040013F0 RID: 5104
		public const int PERF_COUNTER_LARGE_DELTA = 4195584;

		// Token: 0x040013F1 RID: 5105
		public const int PERF_PRECISION_SYSTEM_TIMER = 541525248;

		// Token: 0x040013F2 RID: 5106
		public const int PERF_PRECISION_100NS_TIMER = 542573824;

		// Token: 0x040013F3 RID: 5107
		public const int PERF_PRECISION_OBJECT_TIMER = 543622400;

		// Token: 0x040013F4 RID: 5108
		public const uint PDH_FMT_DOUBLE = 512U;

		// Token: 0x040013F5 RID: 5109
		public const uint PDH_FMT_NOSCALE = 4096U;

		// Token: 0x040013F6 RID: 5110
		public const uint PDH_FMT_NOCAP100 = 32768U;

		// Token: 0x040013F7 RID: 5111
		public const int PERF_DETAIL_NOVICE = 100;

		// Token: 0x040013F8 RID: 5112
		public const int PERF_DETAIL_ADVANCED = 200;

		// Token: 0x040013F9 RID: 5113
		public const int PERF_DETAIL_EXPERT = 300;

		// Token: 0x040013FA RID: 5114
		public const int PERF_DETAIL_WIZARD = 400;

		// Token: 0x040013FB RID: 5115
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x040013FC RID: 5116
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040013FD RID: 5117
		public const int FORMAT_MESSAGE_FROM_STRING = 1024;

		// Token: 0x040013FE RID: 5118
		public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;

		// Token: 0x040013FF RID: 5119
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04001400 RID: 5120
		public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x04001401 RID: 5121
		public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;

		// Token: 0x04001402 RID: 5122
		public const int LOAD_WITH_ALTERED_SEARCH_PATH = 8;

		// Token: 0x04001403 RID: 5123
		public const int LOAD_LIBRARY_AS_DATAFILE = 2;

		// Token: 0x04001404 RID: 5124
		public const int SEEK_READ = 2;

		// Token: 0x04001405 RID: 5125
		public const int FORWARDS_READ = 4;

		// Token: 0x04001406 RID: 5126
		public const int BACKWARDS_READ = 8;

		// Token: 0x04001407 RID: 5127
		public const int ERROR_EVENTLOG_FILE_CHANGED = 1503;

		// Token: 0x04001408 RID: 5128
		public const int NtPerfCounterSizeDword = 0;

		// Token: 0x04001409 RID: 5129
		public const int NtPerfCounterSizeLarge = 256;

		// Token: 0x0400140A RID: 5130
		public const int SHGFI_USEFILEATTRIBUTES = 16;

		// Token: 0x0400140B RID: 5131
		public const int SHGFI_TYPENAME = 1024;

		// Token: 0x0400140C RID: 5132
		public const int NtQueryProcessBasicInfo = 0;

		// Token: 0x0400140D RID: 5133
		public const int NtQuerySystemProcessInformation = 5;

		// Token: 0x0400140E RID: 5134
		public const int SEE_MASK_CLASSNAME = 1;

		// Token: 0x0400140F RID: 5135
		public const int SEE_MASK_CLASSKEY = 3;

		// Token: 0x04001410 RID: 5136
		public const int SEE_MASK_IDLIST = 4;

		// Token: 0x04001411 RID: 5137
		public const int SEE_MASK_INVOKEIDLIST = 12;

		// Token: 0x04001412 RID: 5138
		public const int SEE_MASK_ICON = 16;

		// Token: 0x04001413 RID: 5139
		public const int SEE_MASK_HOTKEY = 32;

		// Token: 0x04001414 RID: 5140
		public const int SEE_MASK_NOCLOSEPROCESS = 64;

		// Token: 0x04001415 RID: 5141
		public const int SEE_MASK_CONNECTNETDRV = 128;

		// Token: 0x04001416 RID: 5142
		public const int SEE_MASK_FLAG_DDEWAIT = 256;

		// Token: 0x04001417 RID: 5143
		public const int SEE_MASK_DOENVSUBST = 512;

		// Token: 0x04001418 RID: 5144
		public const int SEE_MASK_FLAG_NO_UI = 1024;

		// Token: 0x04001419 RID: 5145
		public const int SEE_MASK_UNICODE = 16384;

		// Token: 0x0400141A RID: 5146
		public const int SEE_MASK_NO_CONSOLE = 32768;

		// Token: 0x0400141B RID: 5147
		public const int SEE_MASK_ASYNCOK = 1048576;

		// Token: 0x0400141C RID: 5148
		public const int TH32CS_SNAPHEAPLIST = 1;

		// Token: 0x0400141D RID: 5149
		public const int TH32CS_SNAPPROCESS = 2;

		// Token: 0x0400141E RID: 5150
		public const int TH32CS_SNAPTHREAD = 4;

		// Token: 0x0400141F RID: 5151
		public const int TH32CS_SNAPMODULE = 8;

		// Token: 0x04001420 RID: 5152
		public const int TH32CS_INHERIT = -2147483648;

		// Token: 0x04001421 RID: 5153
		public const int PROCESS_TERMINATE = 1;

		// Token: 0x04001422 RID: 5154
		public const int PROCESS_CREATE_THREAD = 2;

		// Token: 0x04001423 RID: 5155
		public const int PROCESS_SET_SESSIONID = 4;

		// Token: 0x04001424 RID: 5156
		public const int PROCESS_VM_OPERATION = 8;

		// Token: 0x04001425 RID: 5157
		public const int PROCESS_VM_READ = 16;

		// Token: 0x04001426 RID: 5158
		public const int PROCESS_VM_WRITE = 32;

		// Token: 0x04001427 RID: 5159
		public const int PROCESS_DUP_HANDLE = 64;

		// Token: 0x04001428 RID: 5160
		public const int PROCESS_CREATE_PROCESS = 128;

		// Token: 0x04001429 RID: 5161
		public const int PROCESS_SET_QUOTA = 256;

		// Token: 0x0400142A RID: 5162
		public const int PROCESS_SET_INFORMATION = 512;

		// Token: 0x0400142B RID: 5163
		public const int PROCESS_QUERY_INFORMATION = 1024;

		// Token: 0x0400142C RID: 5164
		public const int STANDARD_RIGHTS_REQUIRED = 983040;

		// Token: 0x0400142D RID: 5165
		public const int SYNCHRONIZE = 1048576;

		// Token: 0x0400142E RID: 5166
		public const int PROCESS_ALL_ACCESS = 2035711;

		// Token: 0x0400142F RID: 5167
		public const int THREAD_TERMINATE = 1;

		// Token: 0x04001430 RID: 5168
		public const int THREAD_SUSPEND_RESUME = 2;

		// Token: 0x04001431 RID: 5169
		public const int THREAD_GET_CONTEXT = 8;

		// Token: 0x04001432 RID: 5170
		public const int THREAD_SET_CONTEXT = 16;

		// Token: 0x04001433 RID: 5171
		public const int THREAD_SET_INFORMATION = 32;

		// Token: 0x04001434 RID: 5172
		public const int THREAD_QUERY_INFORMATION = 64;

		// Token: 0x04001435 RID: 5173
		public const int THREAD_SET_THREAD_TOKEN = 128;

		// Token: 0x04001436 RID: 5174
		public const int THREAD_IMPERSONATE = 256;

		// Token: 0x04001437 RID: 5175
		public const int THREAD_DIRECT_IMPERSONATION = 512;

		// Token: 0x04001438 RID: 5176
		public const int REG_BINARY = 3;

		// Token: 0x04001439 RID: 5177
		public const int REG_MULTI_SZ = 7;

		// Token: 0x0400143A RID: 5178
		public const int READ_CONTROL = 131072;

		// Token: 0x0400143B RID: 5179
		public const int STANDARD_RIGHTS_READ = 131072;

		// Token: 0x0400143C RID: 5180
		public const int KEY_QUERY_VALUE = 1;

		// Token: 0x0400143D RID: 5181
		public const int KEY_ENUMERATE_SUB_KEYS = 8;

		// Token: 0x0400143E RID: 5182
		public const int KEY_NOTIFY = 16;

		// Token: 0x0400143F RID: 5183
		public const int KEY_READ = 131097;

		// Token: 0x04001440 RID: 5184
		public const int ERROR_BROKEN_PIPE = 109;

		// Token: 0x04001441 RID: 5185
		public const int ERROR_NO_DATA = 232;

		// Token: 0x04001442 RID: 5186
		public const int ERROR_HANDLE_EOF = 38;

		// Token: 0x04001443 RID: 5187
		public const int ERROR_IO_INCOMPLETE = 996;

		// Token: 0x04001444 RID: 5188
		public const int ERROR_IO_PENDING = 997;

		// Token: 0x04001445 RID: 5189
		public const int ERROR_FILE_EXISTS = 80;

		// Token: 0x04001446 RID: 5190
		public const int ERROR_FILENAME_EXCED_RANGE = 206;

		// Token: 0x04001447 RID: 5191
		public const int ERROR_MORE_DATA = 234;

		// Token: 0x04001448 RID: 5192
		public const int ERROR_CANCELLED = 1223;

		// Token: 0x04001449 RID: 5193
		public const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x0400144A RID: 5194
		public const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x0400144B RID: 5195
		public const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x0400144C RID: 5196
		public const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x0400144D RID: 5197
		public const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x0400144E RID: 5198
		public const int ERROR_SHARING_VIOLATION = 32;

		// Token: 0x0400144F RID: 5199
		public const int ERROR_OPERATION_ABORTED = 995;

		// Token: 0x04001450 RID: 5200
		public const int ERROR_NO_ASSOCIATION = 1155;

		// Token: 0x04001451 RID: 5201
		public const int ERROR_DLL_NOT_FOUND = 1157;

		// Token: 0x04001452 RID: 5202
		public const int ERROR_DDE_FAIL = 1156;

		// Token: 0x04001453 RID: 5203
		public const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04001454 RID: 5204
		public const int ERROR_PARTIAL_COPY = 299;

		// Token: 0x04001455 RID: 5205
		public const int ERROR_SUCCESS = 0;

		// Token: 0x04001456 RID: 5206
		public const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x04001457 RID: 5207
		public const int ERROR_COUNTER_TIMEOUT = 1121;

		// Token: 0x04001458 RID: 5208
		public const int RPC_S_SERVER_UNAVAILABLE = 1722;

		// Token: 0x04001459 RID: 5209
		public const int RPC_S_CALL_FAILED = 1726;

		// Token: 0x0400145A RID: 5210
		public const int PDH_NO_DATA = -2147481643;

		// Token: 0x0400145B RID: 5211
		public const int PDH_CALC_NEGATIVE_DENOMINATOR = -2147481642;

		// Token: 0x0400145C RID: 5212
		public const int PDH_CALC_NEGATIVE_VALUE = -2147481640;

		// Token: 0x0400145D RID: 5213
		public const int SE_ERR_FNF = 2;

		// Token: 0x0400145E RID: 5214
		public const int SE_ERR_PNF = 3;

		// Token: 0x0400145F RID: 5215
		public const int SE_ERR_ACCESSDENIED = 5;

		// Token: 0x04001460 RID: 5216
		public const int SE_ERR_OOM = 8;

		// Token: 0x04001461 RID: 5217
		public const int SE_ERR_DLLNOTFOUND = 32;

		// Token: 0x04001462 RID: 5218
		public const int SE_ERR_SHARE = 26;

		// Token: 0x04001463 RID: 5219
		public const int SE_ERR_ASSOCINCOMPLETE = 27;

		// Token: 0x04001464 RID: 5220
		public const int SE_ERR_DDETIMEOUT = 28;

		// Token: 0x04001465 RID: 5221
		public const int SE_ERR_DDEFAIL = 29;

		// Token: 0x04001466 RID: 5222
		public const int SE_ERR_DDEBUSY = 30;

		// Token: 0x04001467 RID: 5223
		public const int SE_ERR_NOASSOC = 31;

		// Token: 0x04001468 RID: 5224
		public const int SE_PRIVILEGE_ENABLED = 2;

		// Token: 0x04001469 RID: 5225
		public const int DUPLICATE_CLOSE_SOURCE = 1;

		// Token: 0x0400146A RID: 5226
		public const int DUPLICATE_SAME_ACCESS = 2;

		// Token: 0x0400146B RID: 5227
		public const int LOGON32_LOGON_BATCH = 4;

		// Token: 0x0400146C RID: 5228
		public const int LOGON32_PROVIDER_DEFAULT = 0;

		// Token: 0x0400146D RID: 5229
		public const int LOGON32_LOGON_INTERACTIVE = 2;

		// Token: 0x0400146E RID: 5230
		public const int TOKEN_ADJUST_PRIVILEGES = 32;

		// Token: 0x0400146F RID: 5231
		public const int TOKEN_QUERY = 8;

		// Token: 0x04001470 RID: 5232
		public const int CREATE_NO_WINDOW = 134217728;

		// Token: 0x04001471 RID: 5233
		public const int CREATE_SUSPENDED = 4;

		// Token: 0x04001472 RID: 5234
		public const int CREATE_UNICODE_ENVIRONMENT = 1024;

		// Token: 0x04001473 RID: 5235
		public const int SMTO_ABORTIFHUNG = 2;

		// Token: 0x04001474 RID: 5236
		public const int GWL_STYLE = -16;

		// Token: 0x04001475 RID: 5237
		public const int GCL_WNDPROC = -24;

		// Token: 0x04001476 RID: 5238
		public const int GWL_WNDPROC = -4;

		// Token: 0x04001477 RID: 5239
		public const int WS_DISABLED = 134217728;

		// Token: 0x04001478 RID: 5240
		public const int WM_NULL = 0;

		// Token: 0x04001479 RID: 5241
		public const int WM_CLOSE = 16;

		// Token: 0x0400147A RID: 5242
		public const int SW_SHOWNORMAL = 1;

		// Token: 0x0400147B RID: 5243
		public const int SW_NORMAL = 1;

		// Token: 0x0400147C RID: 5244
		public const int SW_SHOWMINIMIZED = 2;

		// Token: 0x0400147D RID: 5245
		public const int SW_SHOWMAXIMIZED = 3;

		// Token: 0x0400147E RID: 5246
		public const int SW_MAXIMIZE = 3;

		// Token: 0x0400147F RID: 5247
		public const int SW_SHOWNOACTIVATE = 4;

		// Token: 0x04001480 RID: 5248
		public const int SW_SHOW = 5;

		// Token: 0x04001481 RID: 5249
		public const int SW_MINIMIZE = 6;

		// Token: 0x04001482 RID: 5250
		public const int SW_SHOWMINNOACTIVE = 7;

		// Token: 0x04001483 RID: 5251
		public const int SW_SHOWNA = 8;

		// Token: 0x04001484 RID: 5252
		public const int SW_RESTORE = 9;

		// Token: 0x04001485 RID: 5253
		public const int SW_SHOWDEFAULT = 10;

		// Token: 0x04001486 RID: 5254
		public const int SW_MAX = 10;

		// Token: 0x04001487 RID: 5255
		public const int GW_OWNER = 4;

		// Token: 0x04001488 RID: 5256
		public const int WHITENESS = 16711778;

		// Token: 0x04001489 RID: 5257
		public const int VS_FILE_INFO = 16;

		// Token: 0x0400148A RID: 5258
		public const int VS_VERSION_INFO = 1;

		// Token: 0x0400148B RID: 5259
		public const int VS_USER_DEFINED = 100;

		// Token: 0x0400148C RID: 5260
		public const int VS_FFI_SIGNATURE = -17890115;

		// Token: 0x0400148D RID: 5261
		public const int VS_FFI_STRUCVERSION = 65536;

		// Token: 0x0400148E RID: 5262
		public const int VS_FFI_FILEFLAGSMASK = 63;

		// Token: 0x0400148F RID: 5263
		public const int VS_FF_DEBUG = 1;

		// Token: 0x04001490 RID: 5264
		public const int VS_FF_PRERELEASE = 2;

		// Token: 0x04001491 RID: 5265
		public const int VS_FF_PATCHED = 4;

		// Token: 0x04001492 RID: 5266
		public const int VS_FF_PRIVATEBUILD = 8;

		// Token: 0x04001493 RID: 5267
		public const int VS_FF_INFOINFERRED = 16;

		// Token: 0x04001494 RID: 5268
		public const int VS_FF_SPECIALBUILD = 32;

		// Token: 0x04001495 RID: 5269
		public const int VFT_UNKNOWN = 0;

		// Token: 0x04001496 RID: 5270
		public const int VFT_APP = 1;

		// Token: 0x04001497 RID: 5271
		public const int VFT_DLL = 2;

		// Token: 0x04001498 RID: 5272
		public const int VFT_DRV = 3;

		// Token: 0x04001499 RID: 5273
		public const int VFT_FONT = 4;

		// Token: 0x0400149A RID: 5274
		public const int VFT_VXD = 5;

		// Token: 0x0400149B RID: 5275
		public const int VFT_STATIC_LIB = 7;

		// Token: 0x0400149C RID: 5276
		public const int VFT2_UNKNOWN = 0;

		// Token: 0x0400149D RID: 5277
		public const int VFT2_DRV_PRINTER = 1;

		// Token: 0x0400149E RID: 5278
		public const int VFT2_DRV_KEYBOARD = 2;

		// Token: 0x0400149F RID: 5279
		public const int VFT2_DRV_LANGUAGE = 3;

		// Token: 0x040014A0 RID: 5280
		public const int VFT2_DRV_DISPLAY = 4;

		// Token: 0x040014A1 RID: 5281
		public const int VFT2_DRV_MOUSE = 5;

		// Token: 0x040014A2 RID: 5282
		public const int VFT2_DRV_NETWORK = 6;

		// Token: 0x040014A3 RID: 5283
		public const int VFT2_DRV_SYSTEM = 7;

		// Token: 0x040014A4 RID: 5284
		public const int VFT2_DRV_INSTALLABLE = 8;

		// Token: 0x040014A5 RID: 5285
		public const int VFT2_DRV_SOUND = 9;

		// Token: 0x040014A6 RID: 5286
		public const int VFT2_DRV_COMM = 10;

		// Token: 0x040014A7 RID: 5287
		public const int VFT2_DRV_INPUTMETHOD = 11;

		// Token: 0x040014A8 RID: 5288
		public const int VFT2_FONT_RASTER = 1;

		// Token: 0x040014A9 RID: 5289
		public const int VFT2_FONT_VECTOR = 2;

		// Token: 0x040014AA RID: 5290
		public const int VFT2_FONT_TRUETYPE = 3;

		// Token: 0x040014AB RID: 5291
		public const int GMEM_FIXED = 0;

		// Token: 0x040014AC RID: 5292
		public const int GMEM_MOVEABLE = 2;

		// Token: 0x040014AD RID: 5293
		public const int GMEM_NOCOMPACT = 16;

		// Token: 0x040014AE RID: 5294
		public const int GMEM_NODISCARD = 32;

		// Token: 0x040014AF RID: 5295
		public const int GMEM_ZEROINIT = 64;

		// Token: 0x040014B0 RID: 5296
		public const int GMEM_MODIFY = 128;

		// Token: 0x040014B1 RID: 5297
		public const int GMEM_DISCARDABLE = 256;

		// Token: 0x040014B2 RID: 5298
		public const int GMEM_NOT_BANKED = 4096;

		// Token: 0x040014B3 RID: 5299
		public const int GMEM_SHARE = 8192;

		// Token: 0x040014B4 RID: 5300
		public const int GMEM_DDESHARE = 8192;

		// Token: 0x040014B5 RID: 5301
		public const int GMEM_NOTIFY = 16384;

		// Token: 0x040014B6 RID: 5302
		public const int GMEM_LOWER = 4096;

		// Token: 0x040014B7 RID: 5303
		public const int GMEM_VALID_FLAGS = 32626;

		// Token: 0x040014B8 RID: 5304
		public const int GMEM_INVALID_HANDLE = 32768;

		// Token: 0x040014B9 RID: 5305
		public const int GHND = 66;

		// Token: 0x040014BA RID: 5306
		public const int GPTR = 64;

		// Token: 0x040014BB RID: 5307
		public const int GMEM_DISCARDED = 16384;

		// Token: 0x040014BC RID: 5308
		public const int GMEM_LOCKCOUNT = 255;

		// Token: 0x040014BD RID: 5309
		public const int UOI_NAME = 2;

		// Token: 0x040014BE RID: 5310
		public const int UOI_TYPE = 3;

		// Token: 0x040014BF RID: 5311
		public const int UOI_USER_SID = 4;

		// Token: 0x040014C0 RID: 5312
		public const int VER_PLATFORM_WIN32_NT = 2;

		// Token: 0x040014C1 RID: 5313
		public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		// Token: 0x040014C2 RID: 5314
		public static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x040014C3 RID: 5315
		public static readonly IntPtr HKEY_LOCAL_MACHINE = (IntPtr)(-2147483646);

		// Token: 0x02000277 RID: 631
		[StructLayout(LayoutKind.Sequential)]
		internal class STARTUPINFO
		{
			// Token: 0x060015E3 RID: 5603 RVA: 0x0004601C File Offset: 0x0004501C
			public STARTUPINFO()
			{
				this.cb = Marshal.SizeOf(this);
			}

			// Token: 0x060015E4 RID: 5604 RVA: 0x0004609C File Offset: 0x0004509C
			public void Dispose()
			{
				if (this.hStdInput != null && !this.hStdInput.IsInvalid)
				{
					this.hStdInput.Close();
					this.hStdInput = null;
				}
				if (this.hStdOutput != null && !this.hStdOutput.IsInvalid)
				{
					this.hStdOutput.Close();
					this.hStdOutput = null;
				}
				if (this.hStdError != null && !this.hStdError.IsInvalid)
				{
					this.hStdError.Close();
					this.hStdError = null;
				}
			}

			// Token: 0x040014C4 RID: 5316
			public int cb;

			// Token: 0x040014C5 RID: 5317
			public IntPtr lpReserved = IntPtr.Zero;

			// Token: 0x040014C6 RID: 5318
			public IntPtr lpDesktop = IntPtr.Zero;

			// Token: 0x040014C7 RID: 5319
			public IntPtr lpTitle = IntPtr.Zero;

			// Token: 0x040014C8 RID: 5320
			public int dwX;

			// Token: 0x040014C9 RID: 5321
			public int dwY;

			// Token: 0x040014CA RID: 5322
			public int dwXSize;

			// Token: 0x040014CB RID: 5323
			public int dwYSize;

			// Token: 0x040014CC RID: 5324
			public int dwXCountChars;

			// Token: 0x040014CD RID: 5325
			public int dwYCountChars;

			// Token: 0x040014CE RID: 5326
			public int dwFillAttribute;

			// Token: 0x040014CF RID: 5327
			public int dwFlags;

			// Token: 0x040014D0 RID: 5328
			public short wShowWindow;

			// Token: 0x040014D1 RID: 5329
			public short cbReserved2;

			// Token: 0x040014D2 RID: 5330
			public IntPtr lpReserved2 = IntPtr.Zero;

			// Token: 0x040014D3 RID: 5331
			public SafeFileHandle hStdInput = new SafeFileHandle(IntPtr.Zero, false);

			// Token: 0x040014D4 RID: 5332
			public SafeFileHandle hStdOutput = new SafeFileHandle(IntPtr.Zero, false);

			// Token: 0x040014D5 RID: 5333
			public SafeFileHandle hStdError = new SafeFileHandle(IntPtr.Zero, false);
		}

		// Token: 0x02000278 RID: 632
		[StructLayout(LayoutKind.Sequential)]
		internal class SECURITY_ATTRIBUTES
		{
			// Token: 0x040014D6 RID: 5334
			public int nLength = 12;

			// Token: 0x040014D7 RID: 5335
			public SafeLocalMemHandle lpSecurityDescriptor = new SafeLocalMemHandle(IntPtr.Zero, false);

			// Token: 0x040014D8 RID: 5336
			public bool bInheritHandle;
		}

		// Token: 0x02000279 RID: 633
		[Flags]
		internal enum LogonFlags
		{
			// Token: 0x040014DA RID: 5338
			LOGON_WITH_PROFILE = 1,
			// Token: 0x040014DB RID: 5339
			LOGON_NETCREDENTIALS_ONLY = 2
		}

		// Token: 0x0200027A RID: 634
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class WNDCLASS_I
		{
			// Token: 0x040014DC RID: 5340
			public int style;

			// Token: 0x040014DD RID: 5341
			public IntPtr lpfnWndProc;

			// Token: 0x040014DE RID: 5342
			public int cbClsExtra;

			// Token: 0x040014DF RID: 5343
			public int cbWndExtra;

			// Token: 0x040014E0 RID: 5344
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x040014E1 RID: 5345
			public IntPtr hIcon = IntPtr.Zero;

			// Token: 0x040014E2 RID: 5346
			public IntPtr hCursor = IntPtr.Zero;

			// Token: 0x040014E3 RID: 5347
			public IntPtr hbrBackground = IntPtr.Zero;

			// Token: 0x040014E4 RID: 5348
			public IntPtr lpszMenuName = IntPtr.Zero;

			// Token: 0x040014E5 RID: 5349
			public IntPtr lpszClassName = IntPtr.Zero;
		}

		// Token: 0x0200027B RID: 635
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class WNDCLASS
		{
			// Token: 0x040014E6 RID: 5350
			public int style;

			// Token: 0x040014E7 RID: 5351
			public NativeMethods.WndProc lpfnWndProc;

			// Token: 0x040014E8 RID: 5352
			public int cbClsExtra;

			// Token: 0x040014E9 RID: 5353
			public int cbWndExtra;

			// Token: 0x040014EA RID: 5354
			public IntPtr hInstance = IntPtr.Zero;

			// Token: 0x040014EB RID: 5355
			public IntPtr hIcon = IntPtr.Zero;

			// Token: 0x040014EC RID: 5356
			public IntPtr hCursor = IntPtr.Zero;

			// Token: 0x040014ED RID: 5357
			public IntPtr hbrBackground = IntPtr.Zero;

			// Token: 0x040014EE RID: 5358
			public string lpszMenuName;

			// Token: 0x040014EF RID: 5359
			public string lpszClassName;
		}

		// Token: 0x0200027C RID: 636
		public struct MSG
		{
			// Token: 0x040014F0 RID: 5360
			public IntPtr hwnd;

			// Token: 0x040014F1 RID: 5361
			public int message;

			// Token: 0x040014F2 RID: 5362
			public IntPtr wParam;

			// Token: 0x040014F3 RID: 5363
			public IntPtr lParam;

			// Token: 0x040014F4 RID: 5364
			public int time;

			// Token: 0x040014F5 RID: 5365
			public int pt_x;

			// Token: 0x040014F6 RID: 5366
			public int pt_y;
		}

		// Token: 0x0200027D RID: 637
		public enum StructFormatEnum
		{
			// Token: 0x040014F8 RID: 5368
			Ansi = 1,
			// Token: 0x040014F9 RID: 5369
			Unicode,
			// Token: 0x040014FA RID: 5370
			Auto
		}

		// Token: 0x0200027E RID: 638
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class TEXTMETRIC
		{
			// Token: 0x040014FB RID: 5371
			public int tmHeight;

			// Token: 0x040014FC RID: 5372
			public int tmAscent;

			// Token: 0x040014FD RID: 5373
			public int tmDescent;

			// Token: 0x040014FE RID: 5374
			public int tmInternalLeading;

			// Token: 0x040014FF RID: 5375
			public int tmExternalLeading;

			// Token: 0x04001500 RID: 5376
			public int tmAveCharWidth;

			// Token: 0x04001501 RID: 5377
			public int tmMaxCharWidth;

			// Token: 0x04001502 RID: 5378
			public int tmWeight;

			// Token: 0x04001503 RID: 5379
			public int tmOverhang;

			// Token: 0x04001504 RID: 5380
			public int tmDigitizedAspectX;

			// Token: 0x04001505 RID: 5381
			public int tmDigitizedAspectY;

			// Token: 0x04001506 RID: 5382
			public char tmFirstChar;

			// Token: 0x04001507 RID: 5383
			public char tmLastChar;

			// Token: 0x04001508 RID: 5384
			public char tmDefaultChar;

			// Token: 0x04001509 RID: 5385
			public char tmBreakChar;

			// Token: 0x0400150A RID: 5386
			public byte tmItalic;

			// Token: 0x0400150B RID: 5387
			public byte tmUnderlined;

			// Token: 0x0400150C RID: 5388
			public byte tmStruckOut;

			// Token: 0x0400150D RID: 5389
			public byte tmPitchAndFamily;

			// Token: 0x0400150E RID: 5390
			public byte tmCharSet;
		}

		// Token: 0x0200027F RID: 639
		public enum StructFormat
		{
			// Token: 0x04001510 RID: 5392
			Ansi = 1,
			// Token: 0x04001511 RID: 5393
			Unicode,
			// Token: 0x04001512 RID: 5394
			Auto
		}

		// Token: 0x02000280 RID: 640
		// (Invoke) Token: 0x060015EA RID: 5610
		public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x02000281 RID: 641
		// (Invoke) Token: 0x060015EE RID: 5614
		public delegate int ConHndlr(int signalType);

		// Token: 0x02000282 RID: 642
		[StructLayout(LayoutKind.Sequential)]
		public class PDH_RAW_COUNTER
		{
			// Token: 0x04001513 RID: 5395
			public int CStatus;

			// Token: 0x04001514 RID: 5396
			public long TimeStamp;

			// Token: 0x04001515 RID: 5397
			public long FirstValue;

			// Token: 0x04001516 RID: 5398
			public long SecondValue;

			// Token: 0x04001517 RID: 5399
			public int MultiCount;
		}

		// Token: 0x02000283 RID: 643
		[StructLayout(LayoutKind.Sequential)]
		public class PDH_FMT_COUNTERVALUE
		{
			// Token: 0x04001518 RID: 5400
			public int CStatus;

			// Token: 0x04001519 RID: 5401
			public double data;
		}

		// Token: 0x02000284 RID: 644
		[StructLayout(LayoutKind.Sequential)]
		internal class PERF_COUNTER_BLOCK
		{
			// Token: 0x0400151A RID: 5402
			public int ByteLength;
		}

		// Token: 0x02000285 RID: 645
		[StructLayout(LayoutKind.Sequential)]
		internal class PERF_COUNTER_DEFINITION
		{
			// Token: 0x0400151B RID: 5403
			public int ByteLength;

			// Token: 0x0400151C RID: 5404
			public int CounterNameTitleIndex;

			// Token: 0x0400151D RID: 5405
			public int CounterNameTitlePtr;

			// Token: 0x0400151E RID: 5406
			public int CounterHelpTitleIndex;

			// Token: 0x0400151F RID: 5407
			public int CounterHelpTitlePtr;

			// Token: 0x04001520 RID: 5408
			public int DefaultScale;

			// Token: 0x04001521 RID: 5409
			public int DetailLevel;

			// Token: 0x04001522 RID: 5410
			public int CounterType;

			// Token: 0x04001523 RID: 5411
			public int CounterSize;

			// Token: 0x04001524 RID: 5412
			public int CounterOffset;
		}

		// Token: 0x02000286 RID: 646
		[StructLayout(LayoutKind.Sequential)]
		internal class PERF_DATA_BLOCK
		{
			// Token: 0x04001525 RID: 5413
			public int Signature1;

			// Token: 0x04001526 RID: 5414
			public int Signature2;

			// Token: 0x04001527 RID: 5415
			public int LittleEndian;

			// Token: 0x04001528 RID: 5416
			public int Version;

			// Token: 0x04001529 RID: 5417
			public int Revision;

			// Token: 0x0400152A RID: 5418
			public int TotalByteLength;

			// Token: 0x0400152B RID: 5419
			public int HeaderLength;

			// Token: 0x0400152C RID: 5420
			public int NumObjectTypes;

			// Token: 0x0400152D RID: 5421
			public int DefaultObject;

			// Token: 0x0400152E RID: 5422
			public NativeMethods.SYSTEMTIME SystemTime;

			// Token: 0x0400152F RID: 5423
			public int pad1;

			// Token: 0x04001530 RID: 5424
			public long PerfTime;

			// Token: 0x04001531 RID: 5425
			public long PerfFreq;

			// Token: 0x04001532 RID: 5426
			public long PerfTime100nSec;

			// Token: 0x04001533 RID: 5427
			public int SystemNameLength;

			// Token: 0x04001534 RID: 5428
			public int SystemNameOffset;
		}

		// Token: 0x02000287 RID: 647
		[StructLayout(LayoutKind.Sequential)]
		internal class PERF_INSTANCE_DEFINITION
		{
			// Token: 0x04001535 RID: 5429
			public int ByteLength;

			// Token: 0x04001536 RID: 5430
			public int ParentObjectTitleIndex;

			// Token: 0x04001537 RID: 5431
			public int ParentObjectInstance;

			// Token: 0x04001538 RID: 5432
			public int UniqueID;

			// Token: 0x04001539 RID: 5433
			public int NameOffset;

			// Token: 0x0400153A RID: 5434
			public int NameLength;
		}

		// Token: 0x02000288 RID: 648
		[StructLayout(LayoutKind.Sequential)]
		internal class PERF_OBJECT_TYPE
		{
			// Token: 0x0400153B RID: 5435
			public int TotalByteLength;

			// Token: 0x0400153C RID: 5436
			public int DefinitionLength;

			// Token: 0x0400153D RID: 5437
			public int HeaderLength;

			// Token: 0x0400153E RID: 5438
			public int ObjectNameTitleIndex;

			// Token: 0x0400153F RID: 5439
			public int ObjectNameTitlePtr;

			// Token: 0x04001540 RID: 5440
			public int ObjectHelpTitleIndex;

			// Token: 0x04001541 RID: 5441
			public int ObjectHelpTitlePtr;

			// Token: 0x04001542 RID: 5442
			public int DetailLevel;

			// Token: 0x04001543 RID: 5443
			public int NumCounters;

			// Token: 0x04001544 RID: 5444
			public int DefaultCounter;

			// Token: 0x04001545 RID: 5445
			public int NumInstances;

			// Token: 0x04001546 RID: 5446
			public int CodePage;

			// Token: 0x04001547 RID: 5447
			public long PerfTime;

			// Token: 0x04001548 RID: 5448
			public long PerfFreq;
		}

		// Token: 0x02000289 RID: 649
		[StructLayout(LayoutKind.Sequential)]
		internal class NtModuleInfo
		{
			// Token: 0x04001549 RID: 5449
			public IntPtr BaseOfDll = (IntPtr)0;

			// Token: 0x0400154A RID: 5450
			public int SizeOfImage;

			// Token: 0x0400154B RID: 5451
			public IntPtr EntryPoint = (IntPtr)0;
		}

		// Token: 0x0200028A RID: 650
		[StructLayout(LayoutKind.Sequential)]
		internal class WinProcessEntry
		{
			// Token: 0x0400154C RID: 5452
			public const int sizeofFileName = 260;

			// Token: 0x0400154D RID: 5453
			public int dwSize;

			// Token: 0x0400154E RID: 5454
			public int cntUsage;

			// Token: 0x0400154F RID: 5455
			public int th32ProcessID;

			// Token: 0x04001550 RID: 5456
			public IntPtr th32DefaultHeapID = (IntPtr)0;

			// Token: 0x04001551 RID: 5457
			public int th32ModuleID;

			// Token: 0x04001552 RID: 5458
			public int cntThreads;

			// Token: 0x04001553 RID: 5459
			public int th32ParentProcessID;

			// Token: 0x04001554 RID: 5460
			public int pcPriClassBase;

			// Token: 0x04001555 RID: 5461
			public int dwFlags;
		}

		// Token: 0x0200028B RID: 651
		[StructLayout(LayoutKind.Sequential)]
		internal class WinThreadEntry
		{
			// Token: 0x04001556 RID: 5462
			public int dwSize;

			// Token: 0x04001557 RID: 5463
			public int cntUsage;

			// Token: 0x04001558 RID: 5464
			public int th32ThreadID;

			// Token: 0x04001559 RID: 5465
			public int th32OwnerProcessID;

			// Token: 0x0400155A RID: 5466
			public int tpBasePri;

			// Token: 0x0400155B RID: 5467
			public int tpDeltaPri;

			// Token: 0x0400155C RID: 5468
			public int dwFlags;
		}

		// Token: 0x0200028C RID: 652
		[StructLayout(LayoutKind.Sequential)]
		internal class WinModuleEntry
		{
			// Token: 0x0400155D RID: 5469
			public const int sizeofModuleName = 256;

			// Token: 0x0400155E RID: 5470
			public const int sizeofFileName = 260;

			// Token: 0x0400155F RID: 5471
			public int dwSize;

			// Token: 0x04001560 RID: 5472
			public int th32ModuleID;

			// Token: 0x04001561 RID: 5473
			public int th32ProcessID;

			// Token: 0x04001562 RID: 5474
			public int GlblcntUsage;

			// Token: 0x04001563 RID: 5475
			public int ProccntUsage;

			// Token: 0x04001564 RID: 5476
			public IntPtr modBaseAddr = (IntPtr)0;

			// Token: 0x04001565 RID: 5477
			public int modBaseSize;

			// Token: 0x04001566 RID: 5478
			public IntPtr hModule = (IntPtr)0;
		}

		// Token: 0x0200028D RID: 653
		[StructLayout(LayoutKind.Sequential)]
		internal class ShellExecuteInfo
		{
			// Token: 0x060015FC RID: 5628 RVA: 0x00046268 File Offset: 0x00045268
			public ShellExecuteInfo()
			{
				this.cbSize = Marshal.SizeOf(this);
			}

			// Token: 0x04001567 RID: 5479
			public int cbSize;

			// Token: 0x04001568 RID: 5480
			public int fMask;

			// Token: 0x04001569 RID: 5481
			public IntPtr hwnd = (IntPtr)0;

			// Token: 0x0400156A RID: 5482
			public IntPtr lpVerb = (IntPtr)0;

			// Token: 0x0400156B RID: 5483
			public IntPtr lpFile = (IntPtr)0;

			// Token: 0x0400156C RID: 5484
			public IntPtr lpParameters = (IntPtr)0;

			// Token: 0x0400156D RID: 5485
			public IntPtr lpDirectory = (IntPtr)0;

			// Token: 0x0400156E RID: 5486
			public int nShow;

			// Token: 0x0400156F RID: 5487
			public IntPtr hInstApp = (IntPtr)0;

			// Token: 0x04001570 RID: 5488
			public IntPtr lpIDList = (IntPtr)0;

			// Token: 0x04001571 RID: 5489
			public IntPtr lpClass = (IntPtr)0;

			// Token: 0x04001572 RID: 5490
			public IntPtr hkeyClass = (IntPtr)0;

			// Token: 0x04001573 RID: 5491
			public int dwHotKey;

			// Token: 0x04001574 RID: 5492
			public IntPtr hIcon = (IntPtr)0;

			// Token: 0x04001575 RID: 5493
			public IntPtr hProcess = (IntPtr)0;
		}

		// Token: 0x0200028E RID: 654
		[StructLayout(LayoutKind.Sequential)]
		internal class NtProcessBasicInfo
		{
			// Token: 0x04001576 RID: 5494
			public int ExitStatus;

			// Token: 0x04001577 RID: 5495
			public IntPtr PebBaseAddress = (IntPtr)0;

			// Token: 0x04001578 RID: 5496
			public IntPtr AffinityMask = (IntPtr)0;

			// Token: 0x04001579 RID: 5497
			public int BasePriority;

			// Token: 0x0400157A RID: 5498
			public IntPtr UniqueProcessId = (IntPtr)0;

			// Token: 0x0400157B RID: 5499
			public IntPtr InheritedFromUniqueProcessId = (IntPtr)0;
		}

		// Token: 0x0200028F RID: 655
		internal struct LUID
		{
			// Token: 0x0400157C RID: 5500
			public int LowPart;

			// Token: 0x0400157D RID: 5501
			public int HighPart;
		}

		// Token: 0x02000290 RID: 656
		[StructLayout(LayoutKind.Sequential)]
		internal class TokenPrivileges
		{
			// Token: 0x0400157E RID: 5502
			public int PrivilegeCount = 1;

			// Token: 0x0400157F RID: 5503
			public NativeMethods.LUID Luid;

			// Token: 0x04001580 RID: 5504
			public int Attributes;
		}

		// Token: 0x02000291 RID: 657
		// (Invoke) Token: 0x06001600 RID: 5632
		internal delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

		// Token: 0x02000292 RID: 658
		[StructLayout(LayoutKind.Sequential)]
		internal class SYSTEMTIME
		{
			// Token: 0x06001603 RID: 5635 RVA: 0x00046354 File Offset: 0x00045354
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"[SYSTEMTIME: ",
					this.wDay.ToString(CultureInfo.CurrentCulture),
					"/",
					this.wMonth.ToString(CultureInfo.CurrentCulture),
					"/",
					this.wYear.ToString(CultureInfo.CurrentCulture),
					" ",
					this.wHour.ToString(CultureInfo.CurrentCulture),
					":",
					this.wMinute.ToString(CultureInfo.CurrentCulture),
					":",
					this.wSecond.ToString(CultureInfo.CurrentCulture),
					"]"
				});
			}

			// Token: 0x04001581 RID: 5505
			public short wYear;

			// Token: 0x04001582 RID: 5506
			public short wMonth;

			// Token: 0x04001583 RID: 5507
			public short wDayOfWeek;

			// Token: 0x04001584 RID: 5508
			public short wDay;

			// Token: 0x04001585 RID: 5509
			public short wHour;

			// Token: 0x04001586 RID: 5510
			public short wMinute;

			// Token: 0x04001587 RID: 5511
			public short wSecond;

			// Token: 0x04001588 RID: 5512
			public short wMilliseconds;
		}

		// Token: 0x02000293 RID: 659
		[StructLayout(LayoutKind.Sequential)]
		internal class VS_FIXEDFILEINFO
		{
			// Token: 0x04001589 RID: 5513
			public int dwSignature;

			// Token: 0x0400158A RID: 5514
			public int dwStructVersion;

			// Token: 0x0400158B RID: 5515
			public int dwFileVersionMS;

			// Token: 0x0400158C RID: 5516
			public int dwFileVersionLS;

			// Token: 0x0400158D RID: 5517
			public int dwProductVersionMS;

			// Token: 0x0400158E RID: 5518
			public int dwProductVersionLS;

			// Token: 0x0400158F RID: 5519
			public int dwFileFlagsMask;

			// Token: 0x04001590 RID: 5520
			public int dwFileFlags;

			// Token: 0x04001591 RID: 5521
			public int dwFileOS;

			// Token: 0x04001592 RID: 5522
			public int dwFileType;

			// Token: 0x04001593 RID: 5523
			public int dwFileSubtype;

			// Token: 0x04001594 RID: 5524
			public int dwFileDateMS;

			// Token: 0x04001595 RID: 5525
			public int dwFileDateLS;
		}

		// Token: 0x02000294 RID: 660
		[StructLayout(LayoutKind.Sequential)]
		internal class USEROBJECTFLAGS
		{
			// Token: 0x04001596 RID: 5526
			public int fInherit;

			// Token: 0x04001597 RID: 5527
			public int fReserved;

			// Token: 0x04001598 RID: 5528
			public int dwFlags;
		}

		// Token: 0x02000295 RID: 661
		internal static class Util
		{
			// Token: 0x06001607 RID: 5639 RVA: 0x00046435 File Offset: 0x00045435
			public static int HIWORD(int n)
			{
				return (n >> 16) & 65535;
			}

			// Token: 0x06001608 RID: 5640 RVA: 0x00046441 File Offset: 0x00045441
			public static int LOWORD(int n)
			{
				return n & 65535;
			}
		}

		// Token: 0x02000296 RID: 662
		internal struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x04001599 RID: 5529
			internal IntPtr BaseAddress;

			// Token: 0x0400159A RID: 5530
			internal IntPtr AllocationBase;

			// Token: 0x0400159B RID: 5531
			internal uint AllocationProtect;

			// Token: 0x0400159C RID: 5532
			internal UIntPtr RegionSize;

			// Token: 0x0400159D RID: 5533
			internal uint State;

			// Token: 0x0400159E RID: 5534
			internal uint Protect;

			// Token: 0x0400159F RID: 5535
			internal uint Type;
		}
	}
}

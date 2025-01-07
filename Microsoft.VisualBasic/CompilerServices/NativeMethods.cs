using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.VisualBasic.CompilerServices
{
	[ComVisible(false)]
	internal sealed class NativeMethods
	{
		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int WaitForInputIdle(IntPtr Process, int Milliseconds);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr GetWindow(IntPtr hwnd, int wFlag);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr GetDesktopWindow();

		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] [Out] StringBuilder lpString, int nMaxCount);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern int AttachThreadInput(int idAttach, int idAttachTo, int fAttach);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetForegroundWindow(IntPtr hwnd);

		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr SetFocus(IntPtr hwnd);

		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr FindWindow([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpClassName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpWindowName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern int CloseHandle(IntPtr hObject);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		internal static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

		[DllImport("kernel32", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
		internal static extern void GetStartupInfo([In] [Out] NativeTypes.STARTUPINFO lpStartupInfo);

		[DllImport("kernel32", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
		internal static extern int CreateProcess(string lpApplicationName, string lpCommandLine, NativeTypes.SECURITY_ATTRIBUTES lpProcessAttributes, NativeTypes.SECURITY_ATTRIBUTES lpThreadAttributes, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandles, int dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, NativeTypes.STARTUPINFO lpStartupInfo, NativeTypes.PROCESS_INFORMATION lpProcessInformation);

		[DllImport("kernel32", BestFitMapping = false, CharSet = CharSet.Auto, ThrowOnUnmappableChar = true)]
		internal static extern int GetVolumeInformation([MarshalAs(UnmanagedType.LPTStr)] string lpRootPathName, StringBuilder lpVolumeNameBuffer, int nVolumeNameSize, ref int lpVolumeSerialNumber, ref int lpMaximumComponentLength, ref int lpFileSystemFlags, IntPtr lpFileSystemNameBuffer, int nFileSystemNameSize);

		internal static int SHFileOperation(ref NativeMethods.SHFILEOPSTRUCT lpFileOp)
		{
			if (IntPtr.Size == 4)
			{
				return NativeMethods.SHFileOperation32(ref lpFileOp);
			}
			NativeMethods.SHFILEOPSTRUCT64 shfileopstruct = default(NativeMethods.SHFILEOPSTRUCT64);
			shfileopstruct.hwnd = lpFileOp.hwnd;
			shfileopstruct.wFunc = lpFileOp.wFunc;
			shfileopstruct.pFrom = lpFileOp.pFrom;
			shfileopstruct.pTo = lpFileOp.pTo;
			shfileopstruct.fFlags = lpFileOp.fFlags;
			shfileopstruct.fAnyOperationsAborted = lpFileOp.fAnyOperationsAborted;
			shfileopstruct.hNameMappings = lpFileOp.hNameMappings;
			shfileopstruct.lpszProgressTitle = lpFileOp.lpszProgressTitle;
			int num = NativeMethods.SHFileOperation64(ref shfileopstruct);
			lpFileOp.fAnyOperationsAborted = shfileopstruct.fAnyOperationsAborted;
			return num;
		}

		[DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation", SetLastError = true, ThrowOnUnmappableChar = true)]
		private static extern int SHFileOperation32(ref NativeMethods.SHFILEOPSTRUCT lpFileOp);

		[DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation", SetLastError = true, ThrowOnUnmappableChar = true)]
		private static extern int SHFileOperation64(ref NativeMethods.SHFILEOPSTRUCT64 lpFileOp);

		[DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern void GlobalMemoryStatus(ref NativeMethods.MEMORYSTATUS lpBuffer);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GlobalMemoryStatusEx(ref NativeMethods.MEMORYSTATUSEX lpBuffer);

		[DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string StringSecurityDescriptor, uint StringSDRevision, ref IntPtr SecurityDescriptor, IntPtr SecurityDescriptorSize);

		[DllImport("kernel32", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName, int dwFlags);

		private NativeMethods()
		{
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
		internal struct SHFILEOPSTRUCT
		{
			internal IntPtr hwnd;

			internal uint wFunc;

			[MarshalAs(UnmanagedType.LPTStr)]
			internal string pFrom;

			[MarshalAs(UnmanagedType.LPTStr)]
			internal string pTo;

			internal ushort fFlags;

			internal bool fAnyOperationsAborted;

			internal IntPtr hNameMappings;

			[MarshalAs(UnmanagedType.LPTStr)]
			internal string lpszProgressTitle;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SHFILEOPSTRUCT64
		{
			internal IntPtr hwnd;

			internal uint wFunc;

			[MarshalAs(UnmanagedType.LPTStr)]
			internal string pFrom;

			[MarshalAs(UnmanagedType.LPTStr)]
			internal string pTo;

			internal ushort fFlags;

			internal bool fAnyOperationsAborted;

			internal IntPtr hNameMappings;

			[MarshalAs(UnmanagedType.LPTStr)]
			internal string lpszProgressTitle;
		}

		internal enum SHFileOperationType : uint
		{
			FO_MOVE = 1U,
			FO_COPY,
			FO_DELETE,
			FO_RENAME
		}

		[Flags]
		internal enum ShFileOperationFlags : ushort
		{
			FOF_MULTIDESTFILES = 1,
			FOF_CONFIRMMOUSE = 2,
			FOF_SILENT = 4,
			FOF_RENAMEONCOLLISION = 8,
			FOF_NOCONFIRMATION = 16,
			FOF_WANTMAPPINGHANDLE = 32,
			FOF_ALLOWUNDO = 64,
			FOF_FILESONLY = 128,
			FOF_SIMPLEPROGRESS = 256,
			FOF_NOCONFIRMMKDIR = 512,
			FOF_NOERRORUI = 1024,
			FOF_NOCOPYSECURITYATTRIBS = 2048,
			FOF_NORECURSION = 4096,
			FOF_NO_CONNECTED_ELEMENTS = 8192,
			FOF_WANTNUKEWARNING = 16384,
			FOF_NORECURSEREPARSE = 32768
		}

		internal enum SHChangeEventTypes : uint
		{
			SHCNE_DISKEVENTS = 145439U,
			SHCNE_ALLEVENTS = 2147483647U
		}

		internal enum SHChangeEventParameterFlags : uint
		{
			SHCNF_DWORD = 3U
		}

		internal struct MEMORYSTATUS
		{
			internal uint dwLength;

			internal uint dwMemoryLoad;

			internal uint dwTotalPhys;

			internal uint dwAvailPhys;

			internal uint dwTotalPageFile;

			internal uint dwAvailPageFile;

			internal uint dwTotalVirtual;

			internal uint dwAvailVirtual;
		}

		internal struct MEMORYSTATUSEX
		{
			internal void Init()
			{
				this.dwLength = checked((uint)Marshal.SizeOf(typeof(NativeMethods.MEMORYSTATUSEX)));
			}

			internal uint dwLength;

			internal uint dwMemoryLoad;

			internal ulong ullTotalPhys;

			internal ulong ullAvailPhys;

			internal ulong ullTotalPageFile;

			internal ulong ullAvailPageFile;

			internal ulong ullTotalVirtual;

			internal ulong ullAvailVirtual;

			internal ulong ullAvailExtendedVirtual;
		}
	}
}

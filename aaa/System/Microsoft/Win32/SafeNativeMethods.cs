using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x0200029A RID: 666
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal static class SafeNativeMethods
	{
		// Token: 0x0600160F RID: 5647
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(int dwFlags, SafeHandle lpSource, uint dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr[] arguments);

		// Token: 0x06001610 RID: 5648
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int FormatMessage(int dwFlags, HandleRef lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr arguments);

		// Token: 0x06001611 RID: 5649
		[DllImport("kernel32.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
		public static extern void OutputDebugString(string message);

		// Token: 0x06001612 RID: 5650
		[DllImport("user32.dll", BestFitMapping = true, CharSet = CharSet.Auto)]
		public static extern int MessageBox(HandleRef hWnd, string text, string caption, int type);

		// Token: 0x06001613 RID: 5651
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06001614 RID: 5652
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		public static extern bool CloseHandle(HandleRef handle);

		// Token: 0x06001615 RID: 5653
		[DllImport("kernel32.dll")]
		public static extern bool QueryPerformanceCounter(out long value);

		// Token: 0x06001616 RID: 5654
		[DllImport("kernel32.dll")]
		public static extern bool QueryPerformanceFrequency(out long value);

		// Token: 0x06001617 RID: 5655
		[DllImport("user32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern int RegisterWindowMessage(string msg);

		// Token: 0x06001618 RID: 5656
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetTextMetrics(HandleRef hDC, [In] [Out] NativeMethods.TEXTMETRIC tm);

		// Token: 0x06001619 RID: 5657
		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetStockObject(int nIndex);

		// Token: 0x0600161A RID: 5658
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string libFilename);

		// Token: 0x0600161B RID: 5659
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool FreeLibrary(HandleRef hModule);

		// Token: 0x0600161C RID: 5660
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
		public static extern bool GetComputerName(StringBuilder lpBuffer, int[] nSize);

		// Token: 0x0600161D RID: 5661 RVA: 0x00046461 File Offset: 0x00045461
		public unsafe static int InterlockedCompareExchange(IntPtr pDestination, int exchange, int compare)
		{
			return Interlocked.CompareExchange(ref *(int*)pDestination.ToPointer(), exchange, compare);
		}

		// Token: 0x0600161E RID: 5662
		[DllImport("perfcounter.dll", CharSet = CharSet.Auto)]
		public static extern int FormatFromRawValue(uint dwCounterType, uint dwFormat, ref long pTimeBase, NativeMethods.PDH_RAW_COUNTER pRawValue1, NativeMethods.PDH_RAW_COUNTER pRawValue2, NativeMethods.PDH_FMT_COUNTERVALUE pFmtValue);

		// Token: 0x0600161F RID: 5663
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeWaitHandle CreateSemaphore(NativeMethods.SECURITY_ATTRIBUTES lpSecurityAttributes, int initialCount, int maximumCount, string name);

		// Token: 0x06001620 RID: 5664
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeWaitHandle OpenSemaphore(int desiredAccess, bool inheritHandle, string name);

		// Token: 0x06001621 RID: 5665
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool ReleaseSemaphore(SafeWaitHandle handle, int releaseCount, out int previousCount);

		// Token: 0x040015A5 RID: 5541
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x040015A6 RID: 5542
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040015A7 RID: 5543
		public const int FORMAT_MESSAGE_FROM_STRING = 1024;

		// Token: 0x040015A8 RID: 5544
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040015A9 RID: 5545
		public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x040015AA RID: 5546
		public const int MB_RIGHT = 524288;

		// Token: 0x040015AB RID: 5547
		public const int MB_RTLREADING = 1048576;

		// Token: 0x040015AC RID: 5548
		public const int FORMAT_MESSAGE_MAX_WIDTH_MASK = 255;

		// Token: 0x040015AD RID: 5549
		public const int FORMAT_MESSAGE_FROM_HMODULE = 2048;

		// Token: 0x0200029B RID: 667
		[StructLayout(LayoutKind.Sequential)]
		internal class PROCESS_INFORMATION
		{
			// Token: 0x040015AE RID: 5550
			public IntPtr hProcess = IntPtr.Zero;

			// Token: 0x040015AF RID: 5551
			public IntPtr hThread = IntPtr.Zero;

			// Token: 0x040015B0 RID: 5552
			public int dwProcessId;

			// Token: 0x040015B1 RID: 5553
			public int dwThreadId;
		}
	}
}

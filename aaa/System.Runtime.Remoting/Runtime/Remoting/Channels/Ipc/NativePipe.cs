using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000060 RID: 96
	[SuppressUnmanagedCodeSecurity]
	internal static class NativePipe
	{
		// Token: 0x06000304 RID: 772
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern PipeHandle CreateNamedPipe(string lpName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, SECURITY_ATTRIBUTES pipeSecurityDescriptor);

		// Token: 0x06000305 RID: 773
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ConnectNamedPipe(PipeHandle hNamedPipe, Overlapped lpOverlapped);

		// Token: 0x06000306 RID: 774
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool ImpersonateNamedPipeClient(PipeHandle hNamedPipe);

		// Token: 0x06000307 RID: 775
		[DllImport("advapi32.dll")]
		public static extern bool RevertToSelf();

		// Token: 0x06000308 RID: 776
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern PipeHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr attr, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x06000309 RID: 777
		[DllImport("kernel32.dll", SetLastError = true)]
		public unsafe static extern bool ReadFile(PipeHandle hFile, byte* lpBuffer, int nNumberOfBytesToRead, ref int lpNumberOfBytesRead, IntPtr mustBeZero);

		// Token: 0x0600030A RID: 778
		[DllImport("kernel32.dll", SetLastError = true)]
		public unsafe static extern bool ReadFile(PipeHandle hFile, byte* lpBuffer, int nNumberOfBytesToRead, IntPtr numBytesRead_mustBeZero, NativeOverlapped* lpOverlapped);

		// Token: 0x0600030B RID: 779
		[DllImport("kernel32.dll", SetLastError = true)]
		public unsafe static extern bool WriteFile(PipeHandle hFile, byte* lpBuffer, int nNumberOfBytesToWrite, ref int lpNumberOfBytesWritten, IntPtr lpOverlapped);

		// Token: 0x0600030C RID: 780
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WaitNamedPipe(string name, int timeout);

		// Token: 0x0600030D RID: 781
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);

		// Token: 0x0600030E RID: 782
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int CloseHandle(IntPtr hObject);

		// Token: 0x0400021A RID: 538
		private const string Kernel32 = "kernel32.dll";

		// Token: 0x0400021B RID: 539
		private const string AdvApi32 = "advapi32.dll";

		// Token: 0x0400021C RID: 540
		public const uint PIPE_ACCESS_OUTBOUND = 2U;

		// Token: 0x0400021D RID: 541
		public const uint PIPE_ACCESS_DUPLEX = 3U;

		// Token: 0x0400021E RID: 542
		public const uint PIPE_ACCESS_INBOUND = 1U;

		// Token: 0x0400021F RID: 543
		public const uint PIPE_WAIT = 0U;

		// Token: 0x04000220 RID: 544
		public const uint PIPE_NOWAIT = 1U;

		// Token: 0x04000221 RID: 545
		public const uint PIPE_READMODE_BYTE = 0U;

		// Token: 0x04000222 RID: 546
		public const uint PIPE_READMODE_MESSAGE = 2U;

		// Token: 0x04000223 RID: 547
		public const uint PIPE_TYPE_BYTE = 0U;

		// Token: 0x04000224 RID: 548
		public const uint PIPE_TYPE_MESSAGE = 4U;

		// Token: 0x04000225 RID: 549
		public const uint PIPE_CLIENT_END = 0U;

		// Token: 0x04000226 RID: 550
		public const uint PIPE_SERVER_END = 1U;

		// Token: 0x04000227 RID: 551
		public const uint FILE_FLAG_OVERLAPPED = 1073741824U;

		// Token: 0x04000228 RID: 552
		public const uint FILE_ATTRIBUTE_NORMAL = 128U;

		// Token: 0x04000229 RID: 553
		public const uint FILE_SHARE_READ = 1U;

		// Token: 0x0400022A RID: 554
		public const uint FILE_SHARE_WRITE = 2U;

		// Token: 0x0400022B RID: 555
		public const uint PIPE_UNLIMITED_INSTANCES = 255U;

		// Token: 0x0400022C RID: 556
		public const uint SECURITY_SQOS_PRESENT = 1048576U;

		// Token: 0x0400022D RID: 557
		public const uint SECURITY_ANONYMOUS = 0U;

		// Token: 0x0400022E RID: 558
		public const uint SECURITY_IDENTIFICATION = 65536U;

		// Token: 0x0400022F RID: 559
		public const uint SECURITY_IMPERSONATION = 131072U;

		// Token: 0x04000230 RID: 560
		public const uint SECURITY_DELEGATION = 196608U;

		// Token: 0x04000231 RID: 561
		internal const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000232 RID: 562
		internal const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000233 RID: 563
		internal const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x04000234 RID: 564
		public const uint NMPWAIT_WAIT_FOREVER = 4294967295U;

		// Token: 0x04000235 RID: 565
		public const uint NMPWAIT_NOWAIT = 1U;

		// Token: 0x04000236 RID: 566
		public const uint NMPWAIT_USE_DEFAULT_WAIT = 0U;

		// Token: 0x04000237 RID: 567
		public const uint GENERIC_READ = 2147483648U;

		// Token: 0x04000238 RID: 568
		public const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x04000239 RID: 569
		public const uint GENERIC_EXECUTE = 536870912U;

		// Token: 0x0400023A RID: 570
		public const uint GENERIC_ALL = 268435456U;

		// Token: 0x0400023B RID: 571
		public const uint CREATE_NEW = 1U;

		// Token: 0x0400023C RID: 572
		public const uint CREATE_ALWAYS = 2U;

		// Token: 0x0400023D RID: 573
		public const uint OPEN_EXISTING = 3U;

		// Token: 0x0400023E RID: 574
		public const uint OPEN_ALWAYS = 4U;

		// Token: 0x0400023F RID: 575
		public const uint TRUNCATE_EXISTING = 5U;

		// Token: 0x04000240 RID: 576
		public const uint FILE_FLAG_FIRST_PIPE_INSTANCE = 524288U;

		// Token: 0x04000241 RID: 577
		public const int INVALID_HANDLE_VALUE = -1;

		// Token: 0x04000242 RID: 578
		public const long ERROR_BROKEN_PIPE = 109L;

		// Token: 0x04000243 RID: 579
		public const long ERROR_IO_PENDING = 997L;

		// Token: 0x04000244 RID: 580
		public const long ERROR_PIPE_BUSY = 231L;

		// Token: 0x04000245 RID: 581
		public const long ERROR_NO_DATA = 232L;

		// Token: 0x04000246 RID: 582
		public const long ERROR_PIPE_NOT_CONNECTED = 233L;

		// Token: 0x04000247 RID: 583
		public const long ERROR_PIPE_CONNECTED = 535L;

		// Token: 0x04000248 RID: 584
		public const long ERROR_PIPE_LISTENING = 536L;

		// Token: 0x04000249 RID: 585
		internal static readonly IntPtr NULL = IntPtr.Zero;
	}
}

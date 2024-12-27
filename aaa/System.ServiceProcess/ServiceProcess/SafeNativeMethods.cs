using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.ServiceProcess
{
	// Token: 0x0200001F RID: 31
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	internal static class SafeNativeMethods
	{
		// Token: 0x06000036 RID: 54
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenSCManager(string machineName, string databaseName, int access);

		// Token: 0x06000037 RID: 55
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool CloseServiceHandle(IntPtr handle);

		// Token: 0x06000038 RID: 56
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int LsaClose(IntPtr objectHandle);

		// Token: 0x06000039 RID: 57
		[DllImport("advapi32.dll")]
		public static extern int LsaFreeMemory(IntPtr ptr);

		// Token: 0x0600003A RID: 58
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		public static extern int LsaNtStatusToWinError(int ntStatus);

		// Token: 0x0600003B RID: 59
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool GetServiceKeyName(IntPtr SCMHandle, string displayName, StringBuilder shortName, ref int shortNameLength);

		// Token: 0x0600003C RID: 60
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool GetServiceDisplayName(IntPtr SCMHandle, string shortName, StringBuilder displayName, ref int displayNameLength);
	}
}

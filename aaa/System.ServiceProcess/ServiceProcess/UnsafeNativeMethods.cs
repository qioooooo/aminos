using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.ServiceProcess
{
	// Token: 0x02000038 RID: 56
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06000111 RID: 273
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public unsafe static extern bool ControlService(IntPtr serviceHandle, int control, NativeMethods.SERVICE_STATUS* pStatus);

		// Token: 0x06000112 RID: 274
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public unsafe static extern bool QueryServiceStatus(IntPtr serviceHandle, NativeMethods.SERVICE_STATUS* pStatus);

		// Token: 0x06000113 RID: 275
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool EnumServicesStatus(IntPtr databaseHandle, int serviceType, int serviceState, IntPtr status, int size, out int bytesNeeded, out int servicesReturned, ref int resumeHandle);

		// Token: 0x06000114 RID: 276
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool EnumServicesStatusEx(IntPtr databaseHandle, int infolevel, int serviceType, int serviceState, IntPtr status, int size, out int bytesNeeded, out int servicesReturned, ref int resumeHandle, string group);

		// Token: 0x06000115 RID: 277
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr OpenService(IntPtr databaseHandle, string serviceName, int access);

		// Token: 0x06000116 RID: 278
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool StartService(IntPtr serviceHandle, int argNum, IntPtr argPtrs);

		// Token: 0x06000117 RID: 279
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool EnumDependentServices(IntPtr serviceHandle, int serviceState, IntPtr bufferOfENUM_SERVICE_STATUS, int bufSize, ref int bytesNeeded, ref int numEnumerated);

		// Token: 0x06000118 RID: 280
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool QueryServiceConfig(IntPtr serviceHandle, IntPtr query_service_config_ptr, int bufferSize, out int bytesNeeded);
	}
}

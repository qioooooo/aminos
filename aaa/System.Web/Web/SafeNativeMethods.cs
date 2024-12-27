using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Web
{
	// Token: 0x020000C7 RID: 199
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	internal sealed class SafeNativeMethods
	{
		// Token: 0x060008F7 RID: 2295 RVA: 0x00028BD2 File Offset: 0x00027BD2
		private SafeNativeMethods()
		{
		}

		// Token: 0x060008F8 RID: 2296
		[DllImport("kernel32.dll")]
		internal static extern int GetCurrentProcessId();

		// Token: 0x060008F9 RID: 2297
		[DllImport("kernel32.dll")]
		internal static extern int GetCurrentThreadId();

		// Token: 0x060008FA RID: 2298
		[DllImport("kernel32.dll")]
		internal static extern bool QueryPerformanceCounter([In] [Out] ref long lpPerformanceCount);

		// Token: 0x060008FB RID: 2299
		[DllImport("kernel32.dll")]
		internal static extern bool QueryPerformanceFrequency([In] [Out] ref long lpFrequency);
	}
}

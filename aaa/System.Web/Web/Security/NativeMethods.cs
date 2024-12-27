using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Web.Security
{
	// Token: 0x0200031C RID: 796
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		// Token: 0x0600274D RID: 10061
		[DllImport("Netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DsGetDcNameW")]
		internal static extern int DsGetDcName([In] string computerName, [In] string domainName, [In] IntPtr domainGuid, [In] string siteName, [In] uint flags, out IntPtr domainControllerInfo);

		// Token: 0x0600274E RID: 10062
		[DllImport("Netapi32.dll")]
		internal static extern int NetApiBufferFree([In] IntPtr buffer);

		// Token: 0x0600274F RID: 10063
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		public static extern int FormatMessageW([In] int dwFlags, [In] int lpSource, [In] int dwMessageId, [In] int dwLanguageId, [Out] StringBuilder lpBuffer, [In] int nSize, [In] int arguments);

		// Token: 0x04001E33 RID: 7731
		internal const int ERROR_NO_SUCH_DOMAIN = 1355;

		// Token: 0x04001E34 RID: 7732
		internal const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04001E35 RID: 7733
		internal const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04001E36 RID: 7734
		internal const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
	}
}

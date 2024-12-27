using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000610 RID: 1552
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeWinINetNativeMethods
	{
		// Token: 0x06002FF1 RID: 12273
		[DllImport("wininet.dll")]
		internal static extern bool InternetGetConnectedState(ref uint flags, uint dwReserved);

		// Token: 0x04002DBE RID: 11710
		private const string WININET = "wininet.dll";
	}
}

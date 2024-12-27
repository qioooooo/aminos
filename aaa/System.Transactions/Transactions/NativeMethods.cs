using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions
{
	// Token: 0x0200000E RID: 14
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		// Token: 0x0600003A RID: 58
		[SuppressUnmanagedCodeSecurity]
		[DllImport("Ole32")]
		internal static extern void CoGetContextToken(out IntPtr contextToken);

		// Token: 0x0600003B RID: 59
		[SuppressUnmanagedCodeSecurity]
		[DllImport("Ole32")]
		internal static extern void CoGetDefaultContext(int aptType, ref Guid contextInterface, out SafeIUnknown safeUnknown);
	}
}

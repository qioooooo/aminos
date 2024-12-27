using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000520 RID: 1312
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SECURITY : SafeFreeCredentials
	{
		// Token: 0x06002855 RID: 10325 RVA: 0x000A61A5 File Offset: 0x000A51A5
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeCredentialsHandle(ref this._handle) == 0;
		}

		// Token: 0x0400277A RID: 10106
		private const string SECURITY = "security.Dll";
	}
}

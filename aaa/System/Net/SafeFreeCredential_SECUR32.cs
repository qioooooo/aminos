using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000521 RID: 1313
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SECUR32 : SafeFreeCredentials
	{
		// Token: 0x06002857 RID: 10327 RVA: 0x000A61BD File Offset: 0x000A51BD
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.FreeCredentialsHandle(ref this._handle) == 0;
		}

		// Token: 0x0400277B RID: 10107
		private const string SECUR32 = "secur32.Dll";
	}
}

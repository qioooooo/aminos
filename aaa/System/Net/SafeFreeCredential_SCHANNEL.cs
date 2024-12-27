using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000522 RID: 1314
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeCredential_SCHANNEL : SafeFreeCredentials
	{
		// Token: 0x06002859 RID: 10329 RVA: 0x000A61D5 File Offset: 0x000A51D5
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.FreeCredentialsHandle(ref this._handle) == 0;
		}

		// Token: 0x0400277C RID: 10108
		private const string SCHANNEL = "schannel.Dll";
	}
}

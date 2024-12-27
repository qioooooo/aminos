using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000524 RID: 1316
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SECURITY : SafeDeleteContext
	{
		// Token: 0x06002867 RID: 10343 RVA: 0x000A7606 File Offset: 0x000A6606
		internal SafeDeleteContext_SECURITY()
		{
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000A760E File Offset: 0x000A660E
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.DeleteSecurityContext(ref this._handle) == 0;
		}

		// Token: 0x04002781 RID: 10113
		private const string SECURITY = "security.Dll";
	}
}

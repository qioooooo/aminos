using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000525 RID: 1317
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SECUR32 : SafeDeleteContext
	{
		// Token: 0x06002869 RID: 10345 RVA: 0x000A7631 File Offset: 0x000A6631
		internal SafeDeleteContext_SECUR32()
		{
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000A7639 File Offset: 0x000A6639
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.DeleteSecurityContext(ref this._handle) == 0;
		}

		// Token: 0x04002782 RID: 10114
		private const string SECUR32 = "secur32.Dll";
	}
}

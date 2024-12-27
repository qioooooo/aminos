using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000526 RID: 1318
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeDeleteContext_SCHANNEL : SafeDeleteContext
	{
		// Token: 0x0600286B RID: 10347 RVA: 0x000A765C File Offset: 0x000A665C
		internal SafeDeleteContext_SCHANNEL()
		{
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000A7664 File Offset: 0x000A6664
		protected override bool ReleaseHandle()
		{
			if (this._EffectiveCredential != null)
			{
				this._EffectiveCredential.DangerousRelease();
			}
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.DeleteSecurityContext(ref this._handle) == 0;
		}

		// Token: 0x04002783 RID: 10115
		private const string SCHANNEL = "schannel.Dll";
	}
}

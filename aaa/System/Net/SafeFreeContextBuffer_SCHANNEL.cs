using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000515 RID: 1301
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SCHANNEL : SafeFreeContextBuffer
	{
		// Token: 0x0600282B RID: 10283 RVA: 0x000A5B48 File Offset: 0x000A4B48
		internal SafeFreeContextBuffer_SCHANNEL()
		{
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000A5B50 File Offset: 0x000A4B50
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.FreeContextBuffer(this.handle) == 0;
		}

		// Token: 0x04002768 RID: 10088
		private const string SCHANNEL = "schannel.dll";
	}
}

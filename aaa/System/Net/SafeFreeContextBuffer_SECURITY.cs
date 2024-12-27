using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000514 RID: 1300
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SECURITY : SafeFreeContextBuffer
	{
		// Token: 0x06002829 RID: 10281 RVA: 0x000A5B30 File Offset: 0x000A4B30
		internal SafeFreeContextBuffer_SECURITY()
		{
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000A5B38 File Offset: 0x000A4B38
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeContextBuffer(this.handle) == 0;
		}

		// Token: 0x04002767 RID: 10087
		private const string SECURITY = "security.dll";
	}
}

using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200052D RID: 1325
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBufferChannelBinding_SECURITY : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x06002897 RID: 10391 RVA: 0x000A7E6C File Offset: 0x000A6E6C
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECURITY.FreeContextBuffer(this.handle) == 0;
		}
	}
}

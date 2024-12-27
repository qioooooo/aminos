using System;
using System.Security;

namespace System.Net
{
	// Token: 0x0200052E RID: 1326
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBufferChannelBinding_SCHANNEL : SafeFreeContextBufferChannelBinding
	{
		// Token: 0x06002899 RID: 10393 RVA: 0x000A7E84 File Offset: 0x000A6E84
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.FreeContextBuffer(this.handle) == 0;
		}
	}
}

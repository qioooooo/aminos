using System;
using System.Security;

namespace System.Net
{
	// Token: 0x02000516 RID: 1302
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeContextBuffer_SECUR32 : SafeFreeContextBuffer
	{
		// Token: 0x0600282D RID: 10285 RVA: 0x000A5B60 File Offset: 0x000A4B60
		internal SafeFreeContextBuffer_SECUR32()
		{
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000A5B68 File Offset: 0x000A4B68
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles_SECUR32.FreeContextBuffer(this.handle) == 0;
		}

		// Token: 0x04002769 RID: 10089
		private const string SECUR32 = "secur32.dll";
	}
}

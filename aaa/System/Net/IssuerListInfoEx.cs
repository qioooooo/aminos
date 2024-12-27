using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000405 RID: 1029
	internal struct IssuerListInfoEx
	{
		// Token: 0x060020B4 RID: 8372 RVA: 0x00080DE8 File Offset: 0x0007FDE8
		public unsafe IssuerListInfoEx(SafeHandle handle, byte[] nativeBuffer)
		{
			this.aIssuers = handle;
			fixed (byte* ptr = nativeBuffer)
			{
				this.cIssuers = ((uint*)ptr)[IntPtr.Size / 4];
			}
		}

		// Token: 0x04002070 RID: 8304
		public SafeHandle aIssuers;

		// Token: 0x04002071 RID: 8305
		public uint cIssuers;
	}
}

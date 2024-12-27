using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000517 RID: 1303
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeLocalFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600282F RID: 10287 RVA: 0x000A5B78 File Offset: 0x000A4B78
		private SafeLocalFree()
			: base(true)
		{
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000A5B81 File Offset: 0x000A4B81
		private SafeLocalFree(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000A5B8C File Offset: 0x000A4B8C
		public static SafeLocalFree LocalAlloc(int cb)
		{
			SafeLocalFree safeLocalFree = UnsafeNclNativeMethods.SafeNetHandles.LocalAlloc(0, (UIntPtr)((ulong)((long)cb)));
			if (safeLocalFree.IsInvalid)
			{
				safeLocalFree.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			return safeLocalFree;
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000A5BBC File Offset: 0x000A4BBC
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x0400276A RID: 10090
		private const int LMEM_FIXED = 0;

		// Token: 0x0400276B RID: 10091
		private const int NULL = 0;

		// Token: 0x0400276C RID: 10092
		public static SafeLocalFree Zero = new SafeLocalFree(false);
	}
}

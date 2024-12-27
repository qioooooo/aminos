using System;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200052B RID: 1323
	[SuppressUnmanagedCodeSecurity]
	internal class SafeLocalFreeChannelBinding : ChannelBinding
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000A7B70 File Offset: 0x000A6B70
		public override int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000A7B78 File Offset: 0x000A6B78
		public static SafeLocalFreeChannelBinding LocalAlloc(int cb)
		{
			SafeLocalFreeChannelBinding safeLocalFreeChannelBinding = UnsafeNclNativeMethods.SafeNetHandles.LocalAllocChannelBinding(0, (UIntPtr)((ulong)((long)cb)));
			if (safeLocalFreeChannelBinding.IsInvalid)
			{
				safeLocalFreeChannelBinding.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			safeLocalFreeChannelBinding.size = cb;
			return safeLocalFreeChannelBinding;
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000A7BAF File Offset: 0x000A6BAF
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x0400278A RID: 10122
		private const int LMEM_FIXED = 0;

		// Token: 0x0400278B RID: 10123
		private int size;
	}
}

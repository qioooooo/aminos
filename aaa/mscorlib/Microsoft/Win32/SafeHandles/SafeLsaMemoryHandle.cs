using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200066D RID: 1645
	internal sealed class SafeLsaMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003BF8 RID: 15352 RVA: 0x000CDFFC File Offset: 0x000CCFFC
		private SafeLsaMemoryHandle()
			: base(true)
		{
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x000CE005 File Offset: 0x000CD005
		internal SafeLsaMemoryHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06003BFA RID: 15354 RVA: 0x000CE015 File Offset: 0x000CD015
		internal static SafeLsaMemoryHandle InvalidHandle
		{
			get
			{
				return new SafeLsaMemoryHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x000CE021 File Offset: 0x000CD021
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaFreeMemory(this.handle) == 0;
		}
	}
}

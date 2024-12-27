using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200066F RID: 1647
	internal sealed class SafeLsaReturnBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C00 RID: 15360 RVA: 0x000CE066 File Offset: 0x000CD066
		private SafeLsaReturnBufferHandle()
			: base(true)
		{
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x000CE06F File Offset: 0x000CD06F
		internal SafeLsaReturnBufferHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003C02 RID: 15362 RVA: 0x000CE07F File Offset: 0x000CD07F
		internal static SafeLsaReturnBufferHandle InvalidHandle
		{
			get
			{
				return new SafeLsaReturnBufferHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000CE08B File Offset: 0x000CD08B
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaFreeReturnBuffer(this.handle) >= 0;
		}
	}
}

using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200066B RID: 1643
	internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003BF0 RID: 15344 RVA: 0x000CDF88 File Offset: 0x000CCF88
		private SafeLocalAllocHandle()
			: base(true)
		{
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x000CDF91 File Offset: 0x000CCF91
		internal SafeLocalAllocHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x000CDFA1 File Offset: 0x000CCFA1
		internal static SafeLocalAllocHandle InvalidHandle
		{
			get
			{
				return new SafeLocalAllocHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x000CDFAD File Offset: 0x000CCFAD
		protected override bool ReleaseHandle()
		{
			return Win32Native.LocalFree(this.handle) == IntPtr.Zero;
		}
	}
}

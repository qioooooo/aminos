using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200066E RID: 1646
	internal sealed class SafeLsaPolicyHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003BFC RID: 15356 RVA: 0x000CE031 File Offset: 0x000CD031
		private SafeLsaPolicyHandle()
			: base(true)
		{
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x000CE03A File Offset: 0x000CD03A
		internal SafeLsaPolicyHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06003BFE RID: 15358 RVA: 0x000CE04A File Offset: 0x000CD04A
		internal static SafeLsaPolicyHandle InvalidHandle
		{
			get
			{
				return new SafeLsaPolicyHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x000CE056 File Offset: 0x000CD056
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaClose(this.handle) == 0;
		}
	}
}

using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200066C RID: 1644
	internal sealed class SafeLsaLogonProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003BF4 RID: 15348 RVA: 0x000CDFC4 File Offset: 0x000CCFC4
		private SafeLsaLogonProcessHandle()
			: base(true)
		{
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x000CDFCD File Offset: 0x000CCFCD
		internal SafeLsaLogonProcessHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x000CDFDD File Offset: 0x000CCFDD
		internal static SafeLsaLogonProcessHandle InvalidHandle
		{
			get
			{
				return new SafeLsaLogonProcessHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x000CDFE9 File Offset: 0x000CCFE9
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaDeregisterLogonProcess(this.handle) >= 0;
		}
	}
}

using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000670 RID: 1648
	internal sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C04 RID: 15364 RVA: 0x000CE09E File Offset: 0x000CD09E
		private SafeProcessHandle()
			: base(true)
		{
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x000CE0A7 File Offset: 0x000CD0A7
		internal SafeProcessHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06003C06 RID: 15366 RVA: 0x000CE0B7 File Offset: 0x000CD0B7
		internal static SafeProcessHandle InvalidHandle
		{
			get
			{
				return new SafeProcessHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x000CE0C3 File Offset: 0x000CD0C3
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}

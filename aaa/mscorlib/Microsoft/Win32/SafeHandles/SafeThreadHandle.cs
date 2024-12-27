using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000671 RID: 1649
	internal sealed class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x000CE0D0 File Offset: 0x000CD0D0
		private SafeThreadHandle()
			: base(true)
		{
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x000CE0D9 File Offset: 0x000CD0D9
		internal SafeThreadHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x000CE0E9 File Offset: 0x000CD0E9
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}

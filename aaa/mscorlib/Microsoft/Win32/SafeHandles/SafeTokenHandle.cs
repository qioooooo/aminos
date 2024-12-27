using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000672 RID: 1650
	internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C0B RID: 15371 RVA: 0x000CE0F6 File Offset: 0x000CD0F6
		private SafeTokenHandle()
			: base(true)
		{
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x000CE0FF File Offset: 0x000CD0FF
		internal SafeTokenHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06003C0D RID: 15373 RVA: 0x000CE10F File Offset: 0x000CD10F
		internal static SafeTokenHandle InvalidHandle
		{
			get
			{
				return new SafeTokenHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x000CE11B File Offset: 0x000CD11B
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}

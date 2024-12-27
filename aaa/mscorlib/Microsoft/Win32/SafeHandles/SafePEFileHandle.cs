using System;
using System.Security.Policy;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000465 RID: 1125
	internal sealed class SafePEFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D58 RID: 11608 RVA: 0x00098AFF File Offset: 0x00097AFF
		private SafePEFileHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002D59 RID: 11609 RVA: 0x00098B0F File Offset: 0x00097B0F
		internal static SafePEFileHandle InvalidHandle
		{
			get
			{
				return new SafePEFileHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x00098B1B File Offset: 0x00097B1B
		protected override bool ReleaseHandle()
		{
			Hash._ReleasePEFile(this.handle);
			return true;
		}
	}
}

using System;
using System.Security;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B6 RID: 694
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016BE RID: 5822 RVA: 0x0004837D File Offset: 0x0004737D
		internal SafeThreadHandle()
			: base(true)
		{
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00048386 File Offset: 0x00047386
		internal void InitialSetHandle(IntPtr h)
		{
			base.SetHandle(h);
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0004838F File Offset: 0x0004738F
		protected override bool ReleaseHandle()
		{
			return SafeNativeMethods.CloseHandle(this.handle);
		}
	}
}

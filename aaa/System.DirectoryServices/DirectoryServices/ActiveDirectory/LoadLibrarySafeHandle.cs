using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000EE RID: 238
	[SuppressUnmanagedCodeSecurity]
	internal sealed class LoadLibrarySafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x00025E4E File Offset: 0x00024E4E
		private LoadLibrarySafeHandle()
			: base(true)
		{
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00025E57 File Offset: 0x00024E57
		internal LoadLibrarySafeHandle(IntPtr value)
			: base(true)
		{
			base.SetHandle(value);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00025E67 File Offset: 0x00024E67
		protected override bool ReleaseHandle()
		{
			return UnsafeNativeMethods.FreeLibrary(this.handle) != 0U;
		}
	}
}

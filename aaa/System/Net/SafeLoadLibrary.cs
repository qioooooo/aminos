using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200051A RID: 1306
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeLoadLibrary : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600283E RID: 10302 RVA: 0x000A5CEF File Offset: 0x000A4CEF
		private SafeLoadLibrary()
			: base(true)
		{
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000A5CF8 File Offset: 0x000A4CF8
		private SafeLoadLibrary(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000A5D04 File Offset: 0x000A4D04
		public static SafeLoadLibrary LoadLibraryEx(string library)
		{
			SafeLoadLibrary safeLoadLibrary = (ComNetOS.IsWin9x ? UnsafeNclNativeMethods.SafeNetHandles.LoadLibraryExA(library, null, 0U) : UnsafeNclNativeMethods.SafeNetHandles.LoadLibraryExW(library, null, 0U));
			if (safeLoadLibrary.IsInvalid)
			{
				safeLoadLibrary.SetHandleAsInvalid();
			}
			return safeLoadLibrary;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000A5D3C File Offset: 0x000A4D3C
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.FreeLibrary(this.handle);
		}

		// Token: 0x04002770 RID: 10096
		private const string KERNEL32 = "kernel32.dll";

		// Token: 0x04002771 RID: 10097
		public static readonly SafeLoadLibrary Zero = new SafeLoadLibrary(false);
	}
}

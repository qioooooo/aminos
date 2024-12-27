using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000463 RID: 1123
	internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D53 RID: 11603 RVA: 0x00098AC3 File Offset: 0x00097AC3
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFileMappingHandle()
			: base(true)
		{
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x00098ACC File Offset: 0x00097ACC
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFileMappingHandle(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x00098ADC File Offset: 0x00097ADC
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}

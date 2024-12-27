using System;
using System.ServiceProcess;
using Microsoft.Win32.SafeHandles;

// Token: 0x02000020 RID: 32
internal class SafeServiceHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	// Token: 0x0600003D RID: 61 RVA: 0x000023CF File Offset: 0x000013CF
	internal SafeServiceHandle(IntPtr handle, bool ownsHandle)
		: base(ownsHandle)
	{
		base.SetHandle(handle);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000023DF File Offset: 0x000013DF
	protected override bool ReleaseHandle()
	{
		return SafeNativeMethods.CloseServiceHandle(this.handle);
	}
}

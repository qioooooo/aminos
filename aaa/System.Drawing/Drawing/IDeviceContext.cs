using System;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x02000019 RID: 25
	public interface IDeviceContext : IDisposable
	{
		// Token: 0x0600007D RID: 125
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		IntPtr GetHdc();

		// Token: 0x0600007E RID: 126
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void ReleaseHdc();
	}
}

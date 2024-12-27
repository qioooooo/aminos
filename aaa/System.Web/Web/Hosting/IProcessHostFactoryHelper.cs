using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.Hosting
{
	// Token: 0x020002BC RID: 700
	[Guid("02fd465d-5c5d-4b7e-95b6-82faa031b74a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[ComImport]
	public interface IProcessHostFactoryHelper
	{
		// Token: 0x0600242F RID: 9263
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetProcessHost(IProcessHostSupportFunctions functions);
	}
}

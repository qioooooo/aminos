using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002C8 RID: 712
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c84f668a-cc3f-11d7-b79e-505054503030")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[ComImport]
	public interface IRegiisUtility
	{
		// Token: 0x06002491 RID: 9361
		void ProtectedConfigAction(long actionToPerform, [MarshalAs(UnmanagedType.LPWStr)] [In] string firstArgument, [MarshalAs(UnmanagedType.LPWStr)] [In] string secondArgument, [MarshalAs(UnmanagedType.LPWStr)] [In] string providerName, [MarshalAs(UnmanagedType.LPWStr)] [In] string appPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string site, [MarshalAs(UnmanagedType.LPWStr)] [In] string cspOrLocation, int keySize, out IntPtr exception);

		// Token: 0x06002492 RID: 9362
		void RegisterSystemWebAssembly(int doReg, out IntPtr exception);

		// Token: 0x06002493 RID: 9363
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void RegisterAsnetMmcAssembly(int doReg, [MarshalAs(UnmanagedType.LPWStr)] [In] string assemblyName, [MarshalAs(UnmanagedType.LPWStr)] [In] string binaryDirectory, out IntPtr exception);

		// Token: 0x06002494 RID: 9364
		void RemoveBrowserCaps(out IntPtr exception);
	}
}

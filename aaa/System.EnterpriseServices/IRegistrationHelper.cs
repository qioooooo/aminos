using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000088 RID: 136
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("55e3ea25-55cb-4650-8887-18e8d30bb4bc")]
	public interface IRegistrationHelper
	{
		// Token: 0x06000333 RID: 819
		void InstallAssembly([MarshalAs(UnmanagedType.BStr)] [In] string assembly, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string application, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string tlb, [In] InstallationFlags installFlags);

		// Token: 0x06000334 RID: 820
		void UninstallAssembly([MarshalAs(UnmanagedType.BStr)] [In] string assembly, [MarshalAs(UnmanagedType.BStr)] [In] string application);
	}
}

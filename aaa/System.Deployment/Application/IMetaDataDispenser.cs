using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x0200007C RID: 124
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibType(TypeLibTypeFlags.FRestricted)]
	[Guid("809c652e-7396-11d2-9771-00a0c9b4d50c")]
	[ComImport]
	internal interface IMetaDataDispenser
	{
		// Token: 0x060003AE RID: 942
		[return: MarshalAs(UnmanagedType.Interface)]
		object DefineScope([In] ref Guid rclsid, [In] uint dwCreateFlags, [In] ref Guid riid);

		// Token: 0x060003AF RID: 943
		[return: MarshalAs(UnmanagedType.Interface)]
		object OpenScope([MarshalAs(UnmanagedType.LPWStr)] [In] string szScope, [In] uint dwOpenFlags, [In] ref Guid riid);

		// Token: 0x060003B0 RID: 944
		[return: MarshalAs(UnmanagedType.Interface)]
		object OpenScopeOnMemory([In] IntPtr pData, [In] uint cbData, [In] uint dwOpenFlags, [In] ref Guid riid);
	}
}

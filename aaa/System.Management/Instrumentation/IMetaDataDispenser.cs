using System;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B3 RID: 179
	[TypeLibType(TypeLibTypeFlags.FRestricted)]
	[Guid("809c652e-7396-11d2-9771-00a0c9b4d50c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMetaDataDispenser
	{
		// Token: 0x06000556 RID: 1366
		[return: MarshalAs(UnmanagedType.Interface)]
		object DefineScope([In] ref Guid rclsid, [In] uint dwCreateFlags, [In] ref Guid riid);

		// Token: 0x06000557 RID: 1367
		[return: MarshalAs(UnmanagedType.Interface)]
		object OpenScope([MarshalAs(UnmanagedType.LPWStr)] [In] string szScope, [In] uint dwOpenFlags, [In] ref Guid riid);

		// Token: 0x06000558 RID: 1368
		[return: MarshalAs(UnmanagedType.Interface)]
		object OpenScopeOnMemory([In] IntPtr pData, [In] uint cbData, [In] uint dwOpenFlags, [In] ref Guid riid);
	}
}

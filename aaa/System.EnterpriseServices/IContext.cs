using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000041 RID: 65
	[Guid("000001c0-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IContext
	{
		// Token: 0x06000136 RID: 310
		void SetProperty([MarshalAs(UnmanagedType.LPStruct)] [In] Guid policyId, [In] int flags, [MarshalAs(UnmanagedType.Interface)] [In] object punk);

		// Token: 0x06000137 RID: 311
		void RemoveProperty([MarshalAs(UnmanagedType.LPStruct)] [In] Guid policyId);

		// Token: 0x06000138 RID: 312
		void GetProperty([MarshalAs(UnmanagedType.LPStruct)] [In] Guid policyId, out int flags, [MarshalAs(UnmanagedType.Interface)] out object pUnk);
	}
}

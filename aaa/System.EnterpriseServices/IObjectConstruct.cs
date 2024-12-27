using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000008 RID: 8
	[Guid("41C4F8B3-7439-11D2-98CB-00C04F8EE1C4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IObjectConstruct
	{
		// Token: 0x06000010 RID: 16
		void Construct([MarshalAs(UnmanagedType.Interface)] [In] object obj);
	}
}

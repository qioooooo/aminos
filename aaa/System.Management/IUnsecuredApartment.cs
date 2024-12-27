using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C7 RID: 199
	[TypeLibType(512)]
	[Guid("1CFABA8C-1523-11D1-AD79-00C04FD8FDFF")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IUnsecuredApartment
	{
		// Token: 0x06000614 RID: 1556
		[PreserveSig]
		int CreateObjectStub_([MarshalAs(UnmanagedType.IUnknown)] [In] object pObject, [MarshalAs(UnmanagedType.IUnknown)] out object ppStub);
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000CB RID: 203
	[Guid("631F7D97-D993-11D2-B339-00105A1F4AAF")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemProviderIdentity
	{
		// Token: 0x0600061A RID: 1562
		[PreserveSig]
		int SetRegistrationObject_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pProvReg);
	}
}

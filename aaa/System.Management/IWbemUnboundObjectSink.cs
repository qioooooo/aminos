using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C9 RID: 201
	[TypeLibType(512)]
	[Guid("E246107B-B06E-11D0-AD61-00C04FD8FDFF")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemUnboundObjectSink
	{
		// Token: 0x06000617 RID: 1559
		[PreserveSig]
		int IndicateToConsumer_([MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pLogicalConsumer, [In] int lNumObjects, [MarshalAs(UnmanagedType.Interface)] [In] ref IWbemClassObject_DoNotMarshal apObjects);
	}
}

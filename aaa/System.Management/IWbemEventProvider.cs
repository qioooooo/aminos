using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x0200009A RID: 154
	[Guid("E245105B-B06E-11D0-AD61-00C04FD8FDFF")]
	[InterfaceType(1)]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemEventProvider
	{
		// Token: 0x06000472 RID: 1138
		[PreserveSig]
		int ProvideEvents_([MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink, [In] int lFlags);
	}
}

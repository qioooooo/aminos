using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C3 RID: 195
	[Guid("027947E1-D731-11CE-A357-000000000001")]
	[InterfaceType(1)]
	[TypeLibType(512)]
	[ComImport]
	internal interface IEnumWbemClassObject
	{
		// Token: 0x060005EA RID: 1514
		[PreserveSig]
		int Reset_();

		// Token: 0x060005EB RID: 1515
		[PreserveSig]
		int Next_([In] int lTimeout, [In] uint uCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [In] [Out] IWbemClassObject_DoNotMarshal[] apObjects, out uint puReturned);

		// Token: 0x060005EC RID: 1516
		[PreserveSig]
		int NextAsync_([In] uint uCount, [MarshalAs(UnmanagedType.Interface)] [In] IWbemObjectSink pSink);

		// Token: 0x060005ED RID: 1517
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IEnumWbemClassObject ppEnum);

		// Token: 0x060005EE RID: 1518
		[PreserveSig]
		int Skip_([In] int lTimeout, [In] uint nCount);
	}
}

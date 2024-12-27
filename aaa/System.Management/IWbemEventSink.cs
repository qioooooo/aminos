using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000CF RID: 207
	[Guid("3AE0080A-7E3A-4366-BF89-0FEEDC931659")]
	[TypeLibType(512)]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemEventSink
	{
		// Token: 0x0600061F RID: 1567
		[PreserveSig]
		int Indicate_([In] int lObjectCount, [MarshalAs(UnmanagedType.Interface)] [In] ref IWbemClassObject_DoNotMarshal apObjArray);

		// Token: 0x06000620 RID: 1568
		[PreserveSig]
		int SetStatus_([In] int lFlags, [MarshalAs(UnmanagedType.Error)] [In] int hResult, [MarshalAs(UnmanagedType.BStr)] [In] string strParam, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObjParam);

		// Token: 0x06000621 RID: 1569
		[PreserveSig]
		int IndicateWithSD_([In] int lNumObjects, [MarshalAs(UnmanagedType.IUnknown)] [In] ref object apObjects, [In] int lSDLength, [In] ref byte pSD);

		// Token: 0x06000622 RID: 1570
		[PreserveSig]
		int SetSinkSecurity_([In] int lSDLength, [In] ref byte pSD);

		// Token: 0x06000623 RID: 1571
		[PreserveSig]
		int IsActive_();

		// Token: 0x06000624 RID: 1572
		[PreserveSig]
		int GetRestrictedSink_([In] int lNumQueries, [MarshalAs(UnmanagedType.LPWStr)] [In] ref string awszQueries, [MarshalAs(UnmanagedType.IUnknown)] [In] object pCallback, [MarshalAs(UnmanagedType.Interface)] out IWbemEventSink ppSink);

		// Token: 0x06000625 RID: 1573
		[PreserveSig]
		int SetBatchingParameters_([In] int lFlags, [In] uint dwMaxBufferSize, [In] uint dwMaxSendLatency);
	}
}

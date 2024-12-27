using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C1 RID: 193
	[InterfaceType(1)]
	[Guid("44ACA675-E8FC-11D0-A07C-00C04FB68820")]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemCallResult
	{
		// Token: 0x060005E4 RID: 1508
		[PreserveSig]
		int GetResultObject_([In] int lTimeout, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = System.Management.MarshalWbemObject)] out IWbemClassObjectFreeThreaded ppResultObject);

		// Token: 0x060005E5 RID: 1509
		[PreserveSig]
		int GetResultString_([In] int lTimeout, [MarshalAs(UnmanagedType.BStr)] out string pstrResultString);

		// Token: 0x060005E6 RID: 1510
		[PreserveSig]
		int GetResultServices_([In] int lTimeout, [MarshalAs(UnmanagedType.Interface)] out IWbemServices ppServices);

		// Token: 0x060005E7 RID: 1511
		[PreserveSig]
		int GetCallStatus_([In] int lTimeout, out int plStatus);
	}
}

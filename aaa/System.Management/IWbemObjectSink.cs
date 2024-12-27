using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Management
{
	// Token: 0x020000C2 RID: 194
	[Guid("7C857801-7381-11CF-884D-00AA004B2E24")]
	[InterfaceType(1)]
	[SuppressUnmanagedCodeSecurity]
	[TypeLibType(512)]
	[ComImport]
	internal interface IWbemObjectSink
	{
		// Token: 0x060005E8 RID: 1512
		[SuppressUnmanagedCodeSecurity]
		[PreserveSig]
		int Indicate_([In] int lObjectCount, [MarshalAs(UnmanagedType.LPArray)] [In] IntPtr[] apObjArray);

		// Token: 0x060005E9 RID: 1513
		[PreserveSig]
		int SetStatus_([In] int lFlags, [MarshalAs(UnmanagedType.Error)] [In] int hResult, [MarshalAs(UnmanagedType.BStr)] [In] string strParam, [In] IntPtr pObjParam);
	}
}

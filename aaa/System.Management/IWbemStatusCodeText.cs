using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C8 RID: 200
	[Guid("EB87E1BC-3233-11D2-AEC9-00C04FB68820")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemStatusCodeText
	{
		// Token: 0x06000615 RID: 1557
		[PreserveSig]
		int GetErrorCodeText_([MarshalAs(UnmanagedType.Error)] [In] int hRes, [In] uint LocaleId, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string MessageText);

		// Token: 0x06000616 RID: 1558
		[PreserveSig]
		int GetFacilityCodeText_([MarshalAs(UnmanagedType.Error)] [In] int hRes, [In] uint LocaleId, [In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string MessageText);
	}
}

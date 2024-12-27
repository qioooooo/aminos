using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000BD RID: 189
	[TypeLibType(512)]
	[InterfaceType(1)]
	[Guid("DC12A680-737F-11CF-884D-00AA004B2E24")]
	[ComImport]
	internal interface IWbemQualifierSet_DoNotMarshal
	{
		// Token: 0x060005BC RID: 1468
		[PreserveSig]
		int Get_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] [Out] ref object pVal, [In] [Out] ref int plFlavor);

		// Token: 0x060005BD RID: 1469
		[PreserveSig]
		int Put_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] ref object pVal, [In] int lFlavor);

		// Token: 0x060005BE RID: 1470
		[PreserveSig]
		int Delete_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName);

		// Token: 0x060005BF RID: 1471
		[PreserveSig]
		int GetNames_([In] int lFlags, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060005C0 RID: 1472
		[PreserveSig]
		int BeginEnumeration_([In] int lFlags);

		// Token: 0x060005C1 RID: 1473
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] [Out] ref string pstrName, [In] [Out] ref object pVal, [In] [Out] ref int plFlavor);

		// Token: 0x060005C2 RID: 1474
		[PreserveSig]
		int EndEnumeration_();
	}
}

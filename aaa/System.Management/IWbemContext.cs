using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000BF RID: 191
	[InterfaceType(1)]
	[TypeLibType(512)]
	[Guid("44ACA674-E8FC-11D0-A07C-00C04FB68820")]
	[ComImport]
	internal interface IWbemContext
	{
		// Token: 0x060005C4 RID: 1476
		[PreserveSig]
		int Clone_([MarshalAs(UnmanagedType.Interface)] out IWbemContext ppNewCopy);

		// Token: 0x060005C5 RID: 1477
		[PreserveSig]
		int GetNames_([In] int lFlags, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] pNames);

		// Token: 0x060005C6 RID: 1478
		[PreserveSig]
		int BeginEnumeration_([In] int lFlags);

		// Token: 0x060005C7 RID: 1479
		[PreserveSig]
		int Next_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] out string pstrName, out object pValue);

		// Token: 0x060005C8 RID: 1480
		[PreserveSig]
		int EndEnumeration_();

		// Token: 0x060005C9 RID: 1481
		[PreserveSig]
		int SetValue_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, [In] ref object pValue);

		// Token: 0x060005CA RID: 1482
		[PreserveSig]
		int GetValue_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags, out object pValue);

		// Token: 0x060005CB RID: 1483
		[PreserveSig]
		int DeleteValue_([MarshalAs(UnmanagedType.LPWStr)] [In] string wszName, [In] int lFlags);

		// Token: 0x060005CC RID: 1484
		[PreserveSig]
		int DeleteAll_();
	}
}

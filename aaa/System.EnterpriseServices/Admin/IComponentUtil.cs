using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000058 RID: 88
	[Guid("6EB22873-8A19-11D0-81B6-00A0C9231C29")]
	[ComImport]
	internal interface IComponentUtil
	{
		// Token: 0x060001A6 RID: 422
		[DispId(1)]
		void InstallComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrDLLFile, [MarshalAs(UnmanagedType.BStr)] [In] string bstrTypelibFile, [MarshalAs(UnmanagedType.BStr)] [In] string bstrProxyStubDLLFile);

		// Token: 0x060001A7 RID: 423
		[DispId(2)]
		void ImportComponent([MarshalAs(UnmanagedType.BStr)] [In] string bstrCLSID);

		// Token: 0x060001A8 RID: 424
		[DispId(3)]
		void ImportComponentByName([MarshalAs(UnmanagedType.BStr)] [In] string bstrProgID);

		// Token: 0x060001A9 RID: 425
		[DispId(4)]
		void GetCLSIDs([MarshalAs(UnmanagedType.BStr)] [In] string bstrDLLFile, [MarshalAs(UnmanagedType.BStr)] [In] string bstrTypelibFile, [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)] out object[] CLSIDS);
	}
}

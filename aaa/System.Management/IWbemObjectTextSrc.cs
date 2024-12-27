using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C5 RID: 197
	[TypeLibType(512)]
	[InterfaceType(1)]
	[Guid("BFBF883A-CAD7-11D3-A11B-00105A1F515A")]
	[ComImport]
	internal interface IWbemObjectTextSrc
	{
		// Token: 0x060005F0 RID: 1520
		[PreserveSig]
		int GetText_([In] int lFlags, [MarshalAs(UnmanagedType.Interface)] [In] IWbemClassObject_DoNotMarshal pObj, [In] uint uObjTextFormat, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.BStr)] out string strText);

		// Token: 0x060005F1 RID: 1521
		[PreserveSig]
		int CreateFromText_([In] int lFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strText, [In] uint uObjTextFormat, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IWbemClassObject_DoNotMarshal pNewObj);
	}
}

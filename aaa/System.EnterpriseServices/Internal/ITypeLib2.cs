using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000CD RID: 205
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020411-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface ITypeLib2
	{
		// Token: 0x060004B3 RID: 1203
		int GetTypeInfoCount();

		// Token: 0x060004B4 RID: 1204
		int GetTypeInfo(int index, out ITypeInfo ti);

		// Token: 0x060004B5 RID: 1205
		int GetTypeInfoType(int index, out global::System.Runtime.InteropServices.ComTypes.TYPEKIND tkind);

		// Token: 0x060004B6 RID: 1206
		int GetTypeInfoOfGuid(ref Guid guid, ITypeInfo ti);

		// Token: 0x060004B7 RID: 1207
		int GetLibAttr(out global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR tlibattr);

		// Token: 0x060004B8 RID: 1208
		int GetTypeComp(out ITypeComp tcomp);

		// Token: 0x060004B9 RID: 1209
		int GetDocumentation(int index, [MarshalAs(UnmanagedType.BStr)] out string name, [MarshalAs(UnmanagedType.BStr)] out string docString, out int helpContext, [MarshalAs(UnmanagedType.BStr)] out string helpFile);

		// Token: 0x060004BA RID: 1210
		int IsName([MarshalAs(UnmanagedType.LPWStr)] ref string nameBuf, int hashVal, out int isName);

		// Token: 0x060004BB RID: 1211
		int FindName([MarshalAs(UnmanagedType.LPWStr)] ref string szNameBuf, int hashVal, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface, SizeParamIndex = 5)] out ITypeInfo[] tis, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4, SizeParamIndex = 5)] out int[] memIds, ref short foundCount);

		// Token: 0x060004BC RID: 1212
		void ReleaseTLibAttr(global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR libattr);

		// Token: 0x060004BD RID: 1213
		int GetCustData(ref Guid guid, out object value);

		// Token: 0x060004BE RID: 1214
		int GetLibStatistics(out int uniqueNames, out int chUniqueNames);

		// Token: 0x060004BF RID: 1215
		int GetDocumentation2(int index, int lcid, [MarshalAs(UnmanagedType.BStr)] out string helpString, out int helpStringContext, [MarshalAs(UnmanagedType.BStr)] string helpStringDll);

		// Token: 0x060004C0 RID: 1216
		int GetAllCustData(out IntPtr custdata);
	}
}

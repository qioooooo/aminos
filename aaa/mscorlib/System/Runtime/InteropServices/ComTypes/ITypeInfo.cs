using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000580 RID: 1408
	[Guid("00020401-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ITypeInfo
	{
		// Token: 0x060033D3 RID: 13267
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x060033D4 RID: 13268
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x060033D5 RID: 13269
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x060033D6 RID: 13270
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x060033D7 RID: 13271
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x060033D8 RID: 13272
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x060033D9 RID: 13273
		void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);

		// Token: 0x060033DA RID: 13274
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x060033DB RID: 13275
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);

		// Token: 0x060033DC RID: 13276
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x060033DD RID: 13277
		void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);

		// Token: 0x060033DE RID: 13278
		void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);

		// Token: 0x060033DF RID: 13279
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x060033E0 RID: 13280
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x060033E1 RID: 13281
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x060033E2 RID: 13282
		void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);

		// Token: 0x060033E3 RID: 13283
		[PreserveSig]
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x060033E4 RID: 13284
		[PreserveSig]
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x060033E5 RID: 13285
		[PreserveSig]
		void ReleaseVarDesc(IntPtr pVarDesc);
	}
}

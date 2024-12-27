using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000586 RID: 1414
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020412-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeInfo2 : ITypeInfo
	{
		// Token: 0x060033FE RID: 13310
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x060033FF RID: 13311
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x06003400 RID: 13312
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x06003401 RID: 13313
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x06003402 RID: 13314
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x06003403 RID: 13315
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x06003404 RID: 13316
		void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);

		// Token: 0x06003405 RID: 13317
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x06003406 RID: 13318
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);

		// Token: 0x06003407 RID: 13319
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06003408 RID: 13320
		void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);

		// Token: 0x06003409 RID: 13321
		void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);

		// Token: 0x0600340A RID: 13322
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x0600340B RID: 13323
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, [In] ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x0600340C RID: 13324
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x0600340D RID: 13325
		void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);

		// Token: 0x0600340E RID: 13326
		[PreserveSig]
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x0600340F RID: 13327
		[PreserveSig]
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x06003410 RID: 13328
		[PreserveSig]
		void ReleaseVarDesc(IntPtr pVarDesc);

		// Token: 0x06003411 RID: 13329
		void GetTypeKind(out TYPEKIND pTypeKind);

		// Token: 0x06003412 RID: 13330
		void GetTypeFlags(out int pTypeFlags);

		// Token: 0x06003413 RID: 13331
		void GetFuncIndexOfMemId(int memid, INVOKEKIND invKind, out int pFuncIndex);

		// Token: 0x06003414 RID: 13332
		void GetVarIndexOfMemId(int memid, out int pVarIndex);

		// Token: 0x06003415 RID: 13333
		void GetCustData(ref Guid guid, out object pVarVal);

		// Token: 0x06003416 RID: 13334
		void GetFuncCustData(int index, ref Guid guid, out object pVarVal);

		// Token: 0x06003417 RID: 13335
		void GetParamCustData(int indexFunc, int indexParam, ref Guid guid, out object pVarVal);

		// Token: 0x06003418 RID: 13336
		void GetVarCustData(int index, ref Guid guid, out object pVarVal);

		// Token: 0x06003419 RID: 13337
		void GetImplTypeCustData(int index, ref Guid guid, out object pVarVal);

		// Token: 0x0600341A RID: 13338
		[LCIDConversion(1)]
		void GetDocumentation2(int memid, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

		// Token: 0x0600341B RID: 13339
		void GetAllCustData(IntPtr pCustData);

		// Token: 0x0600341C RID: 13340
		void GetAllFuncCustData(int index, IntPtr pCustData);

		// Token: 0x0600341D RID: 13341
		void GetAllParamCustData(int indexFunc, int indexParam, IntPtr pCustData);

		// Token: 0x0600341E RID: 13342
		void GetAllVarCustData(int index, IntPtr pCustData);

		// Token: 0x0600341F RID: 13343
		void GetAllImplTypeCustData(int index, IntPtr pCustData);
	}
}

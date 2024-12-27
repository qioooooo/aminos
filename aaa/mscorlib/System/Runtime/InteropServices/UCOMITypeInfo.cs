using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054C RID: 1356
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ITypeInfo instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("00020401-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMITypeInfo
	{
		// Token: 0x0600334B RID: 13131
		void GetTypeAttr(out IntPtr ppTypeAttr);

		// Token: 0x0600334C RID: 13132
		void GetTypeComp(out UCOMITypeComp ppTComp);

		// Token: 0x0600334D RID: 13133
		void GetFuncDesc(int index, out IntPtr ppFuncDesc);

		// Token: 0x0600334E RID: 13134
		void GetVarDesc(int index, out IntPtr ppVarDesc);

		// Token: 0x0600334F RID: 13135
		void GetNames(int memid, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] [Out] string[] rgBstrNames, int cMaxNames, out int pcNames);

		// Token: 0x06003350 RID: 13136
		void GetRefTypeOfImplType(int index, out int href);

		// Token: 0x06003351 RID: 13137
		void GetImplTypeFlags(int index, out int pImplTypeFlags);

		// Token: 0x06003352 RID: 13138
		void GetIDsOfNames([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] [In] string[] rgszNames, int cNames, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] int[] pMemId);

		// Token: 0x06003353 RID: 13139
		void Invoke([MarshalAs(UnmanagedType.IUnknown)] object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, out object pVarResult, out EXCEPINFO pExcepInfo, out int puArgErr);

		// Token: 0x06003354 RID: 13140
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06003355 RID: 13141
		void GetDllEntry(int memid, INVOKEKIND invKind, out string pBstrDllName, out string pBstrName, out short pwOrdinal);

		// Token: 0x06003356 RID: 13142
		void GetRefTypeInfo(int hRef, out UCOMITypeInfo ppTI);

		// Token: 0x06003357 RID: 13143
		void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);

		// Token: 0x06003358 RID: 13144
		void CreateInstance([MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

		// Token: 0x06003359 RID: 13145
		void GetMops(int memid, out string pBstrMops);

		// Token: 0x0600335A RID: 13146
		void GetContainingTypeLib(out UCOMITypeLib ppTLB, out int pIndex);

		// Token: 0x0600335B RID: 13147
		void ReleaseTypeAttr(IntPtr pTypeAttr);

		// Token: 0x0600335C RID: 13148
		void ReleaseFuncDesc(IntPtr pFuncDesc);

		// Token: 0x0600335D RID: 13149
		void ReleaseVarDesc(IntPtr pVarDesc);
	}
}

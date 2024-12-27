using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000585 RID: 1413
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020411-0000-0000-C000-000000000046")]
	[ComImport]
	public interface ITypeLib2 : ITypeLib
	{
		// Token: 0x060033F0 RID: 13296
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x060033F1 RID: 13297
		void GetTypeInfo(int index, out ITypeInfo ppTI);

		// Token: 0x060033F2 RID: 13298
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x060033F3 RID: 13299
		void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

		// Token: 0x060033F4 RID: 13300
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x060033F5 RID: 13301
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x060033F6 RID: 13302
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x060033F7 RID: 13303
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x060033F8 RID: 13304
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x060033F9 RID: 13305
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);

		// Token: 0x060033FA RID: 13306
		void GetCustData(ref Guid guid, out object pVarVal);

		// Token: 0x060033FB RID: 13307
		[LCIDConversion(1)]
		void GetDocumentation2(int index, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);

		// Token: 0x060033FC RID: 13308
		void GetLibStatistics(IntPtr pcUniqueNames, out int pcchUniqueNames);

		// Token: 0x060033FD RID: 13309
		void GetAllCustData(IntPtr pCustData);
	}
}

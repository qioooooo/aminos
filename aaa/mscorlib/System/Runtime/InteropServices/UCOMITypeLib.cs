using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000550 RID: 1360
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.ITypeLib instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("00020402-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMITypeLib
	{
		// Token: 0x0600335E RID: 13150
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x0600335F RID: 13151
		void GetTypeInfo(int index, out UCOMITypeInfo ppTI);

		// Token: 0x06003360 RID: 13152
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x06003361 RID: 13153
		void GetTypeInfoOfGuid(ref Guid guid, out UCOMITypeInfo ppTInfo);

		// Token: 0x06003362 RID: 13154
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x06003363 RID: 13155
		void GetTypeComp(out UCOMITypeComp ppTComp);

		// Token: 0x06003364 RID: 13156
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x06003365 RID: 13157
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x06003366 RID: 13158
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] UCOMITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x06003367 RID: 13159
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);
	}
}

using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000584 RID: 1412
	[Guid("00020402-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ITypeLib
	{
		// Token: 0x060033E6 RID: 13286
		[PreserveSig]
		int GetTypeInfoCount();

		// Token: 0x060033E7 RID: 13287
		void GetTypeInfo(int index, out ITypeInfo ppTI);

		// Token: 0x060033E8 RID: 13288
		void GetTypeInfoType(int index, out TYPEKIND pTKind);

		// Token: 0x060033E9 RID: 13289
		void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);

		// Token: 0x060033EA RID: 13290
		void GetLibAttr(out IntPtr ppTLibAttr);

		// Token: 0x060033EB RID: 13291
		void GetTypeComp(out ITypeComp ppTComp);

		// Token: 0x060033EC RID: 13292
		void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);

		// Token: 0x060033ED RID: 13293
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal);

		// Token: 0x060033EE RID: 13294
		void FindName([MarshalAs(UnmanagedType.LPWStr)] string szNameBuf, int lHashVal, [MarshalAs(UnmanagedType.LPArray)] [Out] ITypeInfo[] ppTInfo, [MarshalAs(UnmanagedType.LPArray)] [Out] int[] rgMemId, ref short pcFound);

		// Token: 0x060033EF RID: 13295
		[PreserveSig]
		void ReleaseTLibAttr(IntPtr pTLibAttr);
	}
}

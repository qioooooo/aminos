using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000084 RID: 132
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020406-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface ICreateTypeLib
	{
		// Token: 0x060002E9 RID: 745
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateTypeInfo([MarshalAs(UnmanagedType.LPStr)] [In] string szName, int tkind);

		// Token: 0x060002EA RID: 746
		void SetName(string szName);

		// Token: 0x060002EB RID: 747
		void SetVersion(short wMajorVerNum, short wMinorVerNum);

		// Token: 0x060002EC RID: 748
		void SetGuid([MarshalAs(UnmanagedType.LPStruct)] [In] Guid guid);

		// Token: 0x060002ED RID: 749
		void SetDocString([MarshalAs(UnmanagedType.LPStr)] [In] string szDoc);

		// Token: 0x060002EE RID: 750
		void SetHelpFileName([MarshalAs(UnmanagedType.LPStr)] [In] string szHelpFileName);

		// Token: 0x060002EF RID: 751
		void SetHelpContext(int dwHelpContext);

		// Token: 0x060002F0 RID: 752
		void SetLcid(int lcid);

		// Token: 0x060002F1 RID: 753
		void SetLibFlags(int uLibFlags);

		// Token: 0x060002F2 RID: 754
		void SaveAllChanges();
	}
}

using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000563 RID: 1379
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IPersistFile
	{
		// Token: 0x060033B9 RID: 13241
		void GetClassID(out Guid pClassID);

		// Token: 0x060033BA RID: 13242
		[PreserveSig]
		int IsDirty();

		// Token: 0x060033BB RID: 13243
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x060033BC RID: 13244
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x060033BD RID: 13245
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x060033BE RID: 13246
		void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}

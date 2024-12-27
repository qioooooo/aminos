using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000530 RID: 1328
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IPersistFile instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Guid("0000010b-0000-0000-C000-000000000046")]
	[ComImport]
	public interface UCOMIPersistFile
	{
		// Token: 0x06003331 RID: 13105
		void GetClassID(out Guid pClassID);

		// Token: 0x06003332 RID: 13106
		[PreserveSig]
		int IsDirty();

		// Token: 0x06003333 RID: 13107
		void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);

		// Token: 0x06003334 RID: 13108
		void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06003335 RID: 13109
		void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

		// Token: 0x06003336 RID: 13110
		void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
	}
}

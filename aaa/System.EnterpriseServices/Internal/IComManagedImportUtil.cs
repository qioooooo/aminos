using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000BA RID: 186
	[Guid("c3f8f66b-91be-4c99-a94f-ce3b0a951039")]
	public interface IComManagedImportUtil
	{
		// Token: 0x06000461 RID: 1121
		[DispId(4)]
		void GetComponentInfo([MarshalAs(UnmanagedType.BStr)] string assemblyPath, [MarshalAs(UnmanagedType.BStr)] out string numComponents, [MarshalAs(UnmanagedType.BStr)] out string componentInfo);

		// Token: 0x06000462 RID: 1122
		[DispId(5)]
		void InstallAssembly([MarshalAs(UnmanagedType.BStr)] string filename, [MarshalAs(UnmanagedType.BStr)] string parname, [MarshalAs(UnmanagedType.BStr)] string appname);
	}
}

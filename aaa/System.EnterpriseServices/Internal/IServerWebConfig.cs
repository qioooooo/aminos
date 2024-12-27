using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000C8 RID: 200
	[Guid("6261e4b5-572a-4142-a2f9-1fe1a0c97097")]
	public interface IServerWebConfig
	{
		// Token: 0x0600049A RID: 1178
		[DispId(1)]
		void AddElement([MarshalAs(UnmanagedType.BStr)] string FilePath, [MarshalAs(UnmanagedType.BStr)] string AssemblyName, [MarshalAs(UnmanagedType.BStr)] string TypeName, [MarshalAs(UnmanagedType.BStr)] string ProgId, [MarshalAs(UnmanagedType.BStr)] string Mode, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x0600049B RID: 1179
		[DispId(2)]
		void Create([MarshalAs(UnmanagedType.BStr)] string FilePath, [MarshalAs(UnmanagedType.BStr)] string FileRootName, [MarshalAs(UnmanagedType.BStr)] out string Error);
	}
}

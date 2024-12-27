using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000C6 RID: 198
	[Guid("d8013ef0-730b-45e2-ba24-874b7242c425")]
	public interface IComSoapIISVRoot
	{
		// Token: 0x06000496 RID: 1174
		[DispId(1)]
		void Create([MarshalAs(UnmanagedType.BStr)] string RootWeb, [MarshalAs(UnmanagedType.BStr)] string PhysicalDirectory, [MarshalAs(UnmanagedType.BStr)] string VirtualDirectory, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x06000497 RID: 1175
		[DispId(2)]
		void Delete([MarshalAs(UnmanagedType.BStr)] string RootWeb, [MarshalAs(UnmanagedType.BStr)] string PhysicalDirectory, [MarshalAs(UnmanagedType.BStr)] string VirtualDirectory, [MarshalAs(UnmanagedType.BStr)] out string Error);
	}
}

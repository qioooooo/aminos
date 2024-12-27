using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000C7 RID: 199
	[Guid("d8013ff0-730b-45e2-ba24-874b7242c425")]
	public interface IComSoapMetadata
	{
		// Token: 0x06000498 RID: 1176
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string Generate([MarshalAs(UnmanagedType.BStr)] string SrcTypeLibFileName, [MarshalAs(UnmanagedType.BStr)] string OutPath);

		// Token: 0x06000499 RID: 1177
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GenerateSigned([MarshalAs(UnmanagedType.BStr)] string SrcTypeLibFileName, [MarshalAs(UnmanagedType.BStr)] string OutPath, [MarshalAs(UnmanagedType.Bool)] bool InstallGac, [MarshalAs(UnmanagedType.BStr)] out string Error);
	}
}

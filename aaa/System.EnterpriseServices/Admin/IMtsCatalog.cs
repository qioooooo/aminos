using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000057 RID: 87
	[SuppressUnmanagedCodeSecurity]
	[Guid("6EB22870-8A19-11D0-81B6-00A0C9231C29")]
	[ComImport]
	internal interface IMtsCatalog
	{
		// Token: 0x060001A2 RID: 418
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetCollection([MarshalAs(UnmanagedType.BStr)] [In] string bstrCollName);

		// Token: 0x060001A3 RID: 419
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		object Connect([MarshalAs(UnmanagedType.BStr)] [In] string connectStr);

		// Token: 0x060001A4 RID: 420
		[DispId(3)]
		int MajorVersion();

		// Token: 0x060001A5 RID: 421
		[DispId(4)]
		int MinorVersion();
	}
}

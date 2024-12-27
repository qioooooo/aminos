using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200001C RID: 28
	[Guid("2A005C0D-A5DE-11CF-9E66-00AA00A3F464")]
	[ComImport]
	internal interface ISharedPropertyGroupManager
	{
		// Token: 0x06000057 RID: 87
		ISharedPropertyGroup CreatePropertyGroup([MarshalAs(UnmanagedType.BStr)] [In] string name, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref PropertyLockMode dwIsoMode, [MarshalAs(UnmanagedType.I4)] [In] [Out] ref PropertyReleaseMode dwRelMode, out bool fExist);

		// Token: 0x06000058 RID: 88
		ISharedPropertyGroup Group(string name);

		// Token: 0x06000059 RID: 89
		void GetEnumerator(out IEnumerator pEnum);
	}
}

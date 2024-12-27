using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200001B RID: 27
	[Guid("2A005C07-A5DE-11CF-9E66-00AA00A3F464")]
	[ComImport]
	internal interface ISharedPropertyGroup
	{
		// Token: 0x06000053 RID: 83
		ISharedProperty CreatePropertyByPosition([MarshalAs(UnmanagedType.I4)] [In] int position, out bool fExists);

		// Token: 0x06000054 RID: 84
		ISharedProperty PropertyByPosition(int position);

		// Token: 0x06000055 RID: 85
		ISharedProperty CreateProperty([MarshalAs(UnmanagedType.BStr)] [In] string name, out bool fExists);

		// Token: 0x06000056 RID: 86
		ISharedProperty Property([MarshalAs(UnmanagedType.BStr)] [In] string name);
	}
}

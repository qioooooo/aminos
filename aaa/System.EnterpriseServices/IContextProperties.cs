using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000007 RID: 7
	[Guid("D396DA85-BF8F-11d1-BBAE-00C04FC2FA5F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IContextProperties
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11
		int Count { get; }

		// Token: 0x0600000C RID: 12
		object GetProperty([MarshalAs(UnmanagedType.BStr)] [In] string name);

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000D RID: 13
		IEnumerator Enumerate { get; }

		// Token: 0x0600000E RID: 14
		void SetProperty([MarshalAs(UnmanagedType.BStr)] [In] string name, [MarshalAs(UnmanagedType.Struct)] [In] object value);

		// Token: 0x0600000F RID: 15
		void RemoveProperty([MarshalAs(UnmanagedType.BStr)] [In] string name);
	}
}

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000006 RID: 6
	[Guid("51372AF4-CAE7-11CF-BE81-00AA00A2FA25")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IGetContextProperties
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8
		int Count { get; }

		// Token: 0x06000009 RID: 9
		object GetProperty([MarshalAs(UnmanagedType.BStr)] [In] string name);

		// Token: 0x0600000A RID: 10
		void GetEnumerator(out IEnumerator pEnum);
	}
}

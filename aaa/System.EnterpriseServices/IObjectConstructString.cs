using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000009 RID: 9
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[Guid("41C4F8B2-7439-11D2-98CB-00C04F8EE1C4")]
	[ComImport]
	internal interface IObjectConstructString
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17
		string ConstructString
		{
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009A RID: 154
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EBE5A1ED-FEBC-42c4-A9E1-E087C6E36635")]
	[ComImport]
	internal interface IPermissionSetEntry
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600029A RID: 666
		PermissionSetEntry AllData { get; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600029B RID: 667
		string Id
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600029C RID: 668
		string XmlSegment
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}

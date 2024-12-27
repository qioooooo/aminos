using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BF RID: 447
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EBE5A1ED-FEBC-42c4-A9E1-E087C6E36635")]
	[ComImport]
	internal interface IPermissionSetEntry
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600147F RID: 5247
		PermissionSetEntry AllData { get; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001480 RID: 5248
		string Id
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001481 RID: 5249
		string XmlSegment
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}

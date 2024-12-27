using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200009D RID: 157
	[Guid("2474ECB4-8EFD-4410-9F31-B3E7C4A07731")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyRequestEntry
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600029E RID: 670
		AssemblyRequestEntry AllData { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600029F RID: 671
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002A0 RID: 672
		string permissionSetID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}

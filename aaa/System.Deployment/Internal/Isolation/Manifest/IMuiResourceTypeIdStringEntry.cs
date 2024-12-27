using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000179 RID: 377
	[Guid("11df5cad-c183-479b-9a44-3842b71639ce")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceTypeIdStringEntry
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600076A RID: 1898
		MuiResourceTypeIdStringEntry AllData { get; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600076B RID: 1899
		object StringIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600076C RID: 1900
		object IntegerIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000188 RID: 392
	[Guid("0C66F299-E08E-48c5-9264-7CCBEB4D5CBB")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IFileAssociationEntry
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600079A RID: 1946
		FileAssociationEntry AllData { get; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600079B RID: 1947
		string Extension
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600079C RID: 1948
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600079D RID: 1949
		string ProgID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x0600079E RID: 1950
		string DefaultIcon
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600079F RID: 1951
		string Parameter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}

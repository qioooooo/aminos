using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019B RID: 411
	[Guid("0C66F299-E08E-48c5-9264-7CCBEB4D5CBB")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IFileAssociationEntry
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001434 RID: 5172
		FileAssociationEntry AllData { get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001435 RID: 5173
		string Extension
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001436 RID: 5174
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001437 RID: 5175
		string ProgID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001438 RID: 5176
		string DefaultIcon
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001439 RID: 5177
		string Parameter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}

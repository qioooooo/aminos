using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BC RID: 444
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1583EFE9-832F-4d08-B041-CAC5ACEDB948")]
	[ComImport]
	internal interface IEntryPointEntry
	{
		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001478 RID: 5240
		EntryPointEntry AllData { get; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001479 RID: 5241
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600147A RID: 5242
		string CommandLine_File
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600147B RID: 5243
		string CommandLine_Parameters
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600147C RID: 5244
		IReferenceIdentity Identity { get; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600147D RID: 5245
		uint Flags { get; }
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000097 RID: 151
	[Guid("1583EFE9-832F-4d08-B041-CAC5ACEDB948")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEntryPointEntry
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000293 RID: 659
		EntryPointEntry AllData { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000294 RID: 660
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000295 RID: 661
		string CommandLine_File
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000296 RID: 662
		string CommandLine_Parameters
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000297 RID: 663
		IReferenceIdentity Identity { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000298 RID: 664
		uint Flags { get; }
	}
}

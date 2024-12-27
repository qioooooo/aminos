using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A6 RID: 166
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CF168CF4-4E8F-4d92-9D2A-60E5CA21CF85")]
	[ComImport]
	internal interface IDependentOSMetadataEntry
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002B1 RID: 689
		DependentOSMetadataEntry AllData { get; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002B2 RID: 690
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002B3 RID: 691
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002B4 RID: 692
		ushort MajorVersion { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002B5 RID: 693
		ushort MinorVersion { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060002B6 RID: 694
		ushort BuildNumber { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002B7 RID: 695
		byte ServicePackMajor { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002B8 RID: 696
		byte ServicePackMinor { get; }
	}
}

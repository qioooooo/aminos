using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008B RID: 139
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C31FF59E-CD25-47b8-9EF3-CF4433EB97CC")]
	[ComImport]
	internal interface IAssemblyReferenceDependentAssemblyEntry
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000279 RID: 633
		AssemblyReferenceDependentAssemblyEntry AllData { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600027A RID: 634
		string Group
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600027B RID: 635
		string Codebase
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600027C RID: 636
		ulong Size { get; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600027D RID: 637
		object HashValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600027E RID: 638
		uint HashAlgorithm { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600027F RID: 639
		uint Flags { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000280 RID: 640
		string ResourceFallbackCulture
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000281 RID: 641
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000282 RID: 642
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000283 RID: 643
		ISection HashElements { get; }
	}
}

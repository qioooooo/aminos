using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B0 RID: 432
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C31FF59E-CD25-47b8-9EF3-CF4433EB97CC")]
	[ComImport]
	internal interface IAssemblyReferenceDependentAssemblyEntry
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600145E RID: 5214
		AssemblyReferenceDependentAssemblyEntry AllData { get; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600145F RID: 5215
		string Group
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001460 RID: 5216
		string Codebase
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001461 RID: 5217
		ulong Size { get; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001462 RID: 5218
		object HashValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06001463 RID: 5219
		uint HashAlgorithm { get; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06001464 RID: 5220
		uint Flags { get; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06001465 RID: 5221
		string ResourceFallbackCulture
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001466 RID: 5222
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06001467 RID: 5223
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001468 RID: 5224
		ISection HashElements { get; }
	}
}

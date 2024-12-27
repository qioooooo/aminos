using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019D RID: 413
	[Guid("C31FF59E-CD25-47b8-9EF3-CF4433EB97CC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyReferenceDependentAssemblyEntry
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060007C4 RID: 1988
		AssemblyReferenceDependentAssemblyEntry AllData { get; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060007C5 RID: 1989
		string Group
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060007C6 RID: 1990
		string Codebase
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060007C7 RID: 1991
		ulong Size { get; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060007C8 RID: 1992
		object HashValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060007C9 RID: 1993
		uint HashAlgorithm { get; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060007CA RID: 1994
		uint Flags { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060007CB RID: 1995
		string ResourceFallbackCulture
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060007CC RID: 1996
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060007CD RID: 1997
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060007CE RID: 1998
		ISection HashElements { get; }
	}
}

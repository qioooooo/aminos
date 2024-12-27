using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018E RID: 398
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5A7A54D7-5AD5-418e-AB7A-CF823A8D48D0")]
	[ComImport]
	internal interface ISubcategoryMembershipEntry
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007A6 RID: 1958
		SubcategoryMembershipEntry AllData { get; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007A7 RID: 1959
		string Subcategory
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007A8 RID: 1960
		ISection CategoryMembershipData { get; }
	}
}

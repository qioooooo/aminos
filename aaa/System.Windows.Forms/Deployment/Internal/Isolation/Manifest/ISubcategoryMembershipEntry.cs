using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200007C RID: 124
	[Guid("5A7A54D7-5AD5-418e-AB7A-CF823A8D48D0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISubcategoryMembershipEntry
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600025B RID: 603
		SubcategoryMembershipEntry AllData { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600025C RID: 604
		string Subcategory
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600025D RID: 605
		ISection CategoryMembershipData { get; }
	}
}

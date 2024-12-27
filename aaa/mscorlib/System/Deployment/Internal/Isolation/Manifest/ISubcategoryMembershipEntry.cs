using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A1 RID: 417
	[Guid("5A7A54D7-5AD5-418e-AB7A-CF823A8D48D0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISubcategoryMembershipEntry
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001440 RID: 5184
		SubcategoryMembershipEntry AllData { get; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001441 RID: 5185
		string Subcategory
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001442 RID: 5186
		ISection CategoryMembershipData { get; }
	}
}

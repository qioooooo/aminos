using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000191 RID: 401
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("97FDCA77-B6F2-4718-A1EB-29D0AECE9C03")]
	[ComImport]
	internal interface ICategoryMembershipEntry
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007AA RID: 1962
		CategoryMembershipEntry AllData { get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007AB RID: 1963
		IDefinitionIdentity Identity { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007AC RID: 1964
		ISection SubcategoryMembership { get; }
	}
}

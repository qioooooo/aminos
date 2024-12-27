using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A4 RID: 420
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("97FDCA77-B6F2-4718-A1EB-29D0AECE9C03")]
	[ComImport]
	internal interface ICategoryMembershipEntry
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001444 RID: 5188
		CategoryMembershipEntry AllData { get; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06001445 RID: 5189
		IDefinitionIdentity Identity { get; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001446 RID: 5190
		ISection SubcategoryMembership { get; }
	}
}

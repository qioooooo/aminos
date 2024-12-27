using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200007F RID: 127
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("97FDCA77-B6F2-4718-A1EB-29D0AECE9C03")]
	[ComImport]
	internal interface ICategoryMembershipEntry
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600025F RID: 607
		CategoryMembershipEntry AllData { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000260 RID: 608
		IDefinitionIdentity Identity { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000261 RID: 609
		ISection SubcategoryMembership { get; }
	}
}

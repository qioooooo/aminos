using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018F RID: 399
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipEntry
	{
		// Token: 0x040006DA RID: 1754
		public IDefinitionIdentity Identity;

		// Token: 0x040006DB RID: 1755
		public ISection SubcategoryMembership;
	}
}

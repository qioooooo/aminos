using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A2 RID: 418
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipEntry
	{
		// Token: 0x0400074A RID: 1866
		public IDefinitionIdentity Identity;

		// Token: 0x0400074B RID: 1867
		public ISection SubcategoryMembership;
	}
}

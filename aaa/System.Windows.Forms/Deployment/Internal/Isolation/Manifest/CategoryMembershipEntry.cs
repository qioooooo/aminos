using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200007D RID: 125
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipEntry
	{
		// Token: 0x04000C50 RID: 3152
		public IDefinitionIdentity Identity;

		// Token: 0x04000C51 RID: 3153
		public ISection SubcategoryMembership;
	}
}

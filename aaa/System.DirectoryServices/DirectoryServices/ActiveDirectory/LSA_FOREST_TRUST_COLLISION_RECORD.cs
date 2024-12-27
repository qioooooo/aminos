using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000122 RID: 290
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_FOREST_TRUST_COLLISION_RECORD
	{
		// Token: 0x040006C8 RID: 1736
		public int Index;

		// Token: 0x040006C9 RID: 1737
		public ForestTrustCollisionType Type;

		// Token: 0x040006CA RID: 1738
		public int Flags;

		// Token: 0x040006CB RID: 1739
		public LSA_UNICODE_STRING Name;
	}
}

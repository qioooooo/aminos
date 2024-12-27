using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200011A RID: 282
	[StructLayout(LayoutKind.Explicit)]
	internal sealed class LSA_FOREST_TRUST_RECORD
	{
		// Token: 0x040006A6 RID: 1702
		[FieldOffset(0)]
		public int Flags;

		// Token: 0x040006A7 RID: 1703
		[FieldOffset(4)]
		public LSA_FOREST_TRUST_RECORD_TYPE ForestTrustType;

		// Token: 0x040006A8 RID: 1704
		[FieldOffset(8)]
		public LARGE_INTEGER Time;

		// Token: 0x040006A9 RID: 1705
		[FieldOffset(16)]
		public LSA_UNICODE_STRING TopLevelName;

		// Token: 0x040006AA RID: 1706
		[FieldOffset(16)]
		public LSA_FOREST_TRUST_BINARY_DATA Data;

		// Token: 0x040006AB RID: 1707
		[FieldOffset(16)]
		public LSA_FOREST_TRUST_DOMAIN_INFO DomainInfo;
	}
}

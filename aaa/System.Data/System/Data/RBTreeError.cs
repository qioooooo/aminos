using System;

namespace System.Data
{
	// Token: 0x020000D0 RID: 208
	internal enum RBTreeError
	{
		// Token: 0x040008C5 RID: 2245
		InvalidPageSize = 1,
		// Token: 0x040008C6 RID: 2246
		PagePositionInSlotInUse = 3,
		// Token: 0x040008C7 RID: 2247
		NoFreeSlots,
		// Token: 0x040008C8 RID: 2248
		InvalidStateinInsert,
		// Token: 0x040008C9 RID: 2249
		InvalidNextSizeInDelete = 7,
		// Token: 0x040008CA RID: 2250
		InvalidStateinDelete,
		// Token: 0x040008CB RID: 2251
		InvalidNodeSizeinDelete,
		// Token: 0x040008CC RID: 2252
		InvalidStateinEndDelete,
		// Token: 0x040008CD RID: 2253
		CannotRotateInvalidsuccessorNodeinDelete,
		// Token: 0x040008CE RID: 2254
		IndexOutOFRangeinGetNodeByIndex = 13,
		// Token: 0x040008CF RID: 2255
		RBDeleteFixup,
		// Token: 0x040008D0 RID: 2256
		UnsupportedAccessMethod1,
		// Token: 0x040008D1 RID: 2257
		UnsupportedAccessMethod2,
		// Token: 0x040008D2 RID: 2258
		UnsupportedAccessMethodInNonNillRootSubtree,
		// Token: 0x040008D3 RID: 2259
		AttachedNodeWithZerorbTreeNodeId,
		// Token: 0x040008D4 RID: 2260
		CompareNodeInDataRowTree,
		// Token: 0x040008D5 RID: 2261
		CompareSateliteTreeNodeInDataRowTree,
		// Token: 0x040008D6 RID: 2262
		NestedSatelliteTreeEnumerator
	}
}

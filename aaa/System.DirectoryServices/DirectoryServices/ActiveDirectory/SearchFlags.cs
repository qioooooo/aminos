using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000076 RID: 118
	internal enum SearchFlags
	{
		// Token: 0x040002F3 RID: 755
		None,
		// Token: 0x040002F4 RID: 756
		IsIndexed,
		// Token: 0x040002F5 RID: 757
		IsIndexedOverContainer,
		// Token: 0x040002F6 RID: 758
		IsInAnr = 4,
		// Token: 0x040002F7 RID: 759
		IsOnTombstonedObject = 8,
		// Token: 0x040002F8 RID: 760
		IsTupleIndexed = 32
	}
}

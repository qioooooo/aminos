using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002B9 RID: 697
	[Flags]
	public enum SqlBulkCopyOptions
	{
		// Token: 0x040016F3 RID: 5875
		Default = 0,
		// Token: 0x040016F4 RID: 5876
		KeepIdentity = 1,
		// Token: 0x040016F5 RID: 5877
		CheckConstraints = 2,
		// Token: 0x040016F6 RID: 5878
		TableLock = 4,
		// Token: 0x040016F7 RID: 5879
		KeepNulls = 8,
		// Token: 0x040016F8 RID: 5880
		FireTriggers = 16,
		// Token: 0x040016F9 RID: 5881
		UseInternalTransaction = 32
	}
}

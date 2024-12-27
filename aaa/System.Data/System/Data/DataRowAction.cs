using System;

namespace System.Data
{
	// Token: 0x02000083 RID: 131
	[Flags]
	public enum DataRowAction
	{
		// Token: 0x04000748 RID: 1864
		Nothing = 0,
		// Token: 0x04000749 RID: 1865
		Delete = 1,
		// Token: 0x0400074A RID: 1866
		Change = 2,
		// Token: 0x0400074B RID: 1867
		Rollback = 4,
		// Token: 0x0400074C RID: 1868
		Commit = 8,
		// Token: 0x0400074D RID: 1869
		Add = 16,
		// Token: 0x0400074E RID: 1870
		ChangeOriginal = 32,
		// Token: 0x0400074F RID: 1871
		ChangeCurrentAndOriginal = 64
	}
}

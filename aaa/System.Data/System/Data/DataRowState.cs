using System;

namespace System.Data
{
	// Token: 0x02000090 RID: 144
	[Flags]
	public enum DataRowState
	{
		// Token: 0x04000779 RID: 1913
		Detached = 1,
		// Token: 0x0400077A RID: 1914
		Unchanged = 2,
		// Token: 0x0400077B RID: 1915
		Added = 4,
		// Token: 0x0400077C RID: 1916
		Deleted = 8,
		// Token: 0x0400077D RID: 1917
		Modified = 16
	}
}

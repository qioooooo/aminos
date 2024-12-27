using System;

namespace System.Web
{
	// Token: 0x0200002D RID: 45
	internal enum FileAction
	{
		// Token: 0x04000D9F RID: 3487
		Dispose = -2,
		// Token: 0x04000DA0 RID: 3488
		Error,
		// Token: 0x04000DA1 RID: 3489
		Overwhelming,
		// Token: 0x04000DA2 RID: 3490
		Added,
		// Token: 0x04000DA3 RID: 3491
		Removed,
		// Token: 0x04000DA4 RID: 3492
		Modified,
		// Token: 0x04000DA5 RID: 3493
		RenamedOldName,
		// Token: 0x04000DA6 RID: 3494
		RenamedNewName
	}
}

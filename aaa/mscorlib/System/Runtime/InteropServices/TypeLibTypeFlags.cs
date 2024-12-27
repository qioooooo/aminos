using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D7 RID: 1239
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibTypeFlags
	{
		// Token: 0x040018B8 RID: 6328
		FAppObject = 1,
		// Token: 0x040018B9 RID: 6329
		FCanCreate = 2,
		// Token: 0x040018BA RID: 6330
		FLicensed = 4,
		// Token: 0x040018BB RID: 6331
		FPreDeclId = 8,
		// Token: 0x040018BC RID: 6332
		FHidden = 16,
		// Token: 0x040018BD RID: 6333
		FControl = 32,
		// Token: 0x040018BE RID: 6334
		FDual = 64,
		// Token: 0x040018BF RID: 6335
		FNonExtensible = 128,
		// Token: 0x040018C0 RID: 6336
		FOleAutomation = 256,
		// Token: 0x040018C1 RID: 6337
		FRestricted = 512,
		// Token: 0x040018C2 RID: 6338
		FAggregatable = 1024,
		// Token: 0x040018C3 RID: 6339
		FReplaceable = 2048,
		// Token: 0x040018C4 RID: 6340
		FDispatchable = 4096,
		// Token: 0x040018C5 RID: 6341
		FReverseBind = 8192
	}
}

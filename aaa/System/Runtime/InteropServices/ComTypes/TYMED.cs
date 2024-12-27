using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000275 RID: 629
	[Flags]
	public enum TYMED
	{
		// Token: 0x04001248 RID: 4680
		TYMED_HGLOBAL = 1,
		// Token: 0x04001249 RID: 4681
		TYMED_FILE = 2,
		// Token: 0x0400124A RID: 4682
		TYMED_ISTREAM = 4,
		// Token: 0x0400124B RID: 4683
		TYMED_ISTORAGE = 8,
		// Token: 0x0400124C RID: 4684
		TYMED_GDI = 16,
		// Token: 0x0400124D RID: 4685
		TYMED_MFPICT = 32,
		// Token: 0x0400124E RID: 4686
		TYMED_ENHMF = 64,
		// Token: 0x0400124F RID: 4687
		TYMED_NULL = 0
	}
}

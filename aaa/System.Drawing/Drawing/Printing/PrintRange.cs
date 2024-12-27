using System;

namespace System.Drawing.Printing
{
	// Token: 0x02000129 RID: 297
	[Serializable]
	public enum PrintRange
	{
		// Token: 0x04000C77 RID: 3191
		AllPages,
		// Token: 0x04000C78 RID: 3192
		SomePages = 2,
		// Token: 0x04000C79 RID: 3193
		Selection = 1,
		// Token: 0x04000C7A RID: 3194
		CurrentPage = 4194304
	}
}

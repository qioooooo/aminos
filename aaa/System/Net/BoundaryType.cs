using System;

namespace System.Net
{
	// Token: 0x020003AB RID: 939
	internal enum BoundaryType
	{
		// Token: 0x04001D7A RID: 7546
		ContentLength,
		// Token: 0x04001D7B RID: 7547
		Chunked,
		// Token: 0x04001D7C RID: 7548
		Multipart = 3,
		// Token: 0x04001D7D RID: 7549
		None,
		// Token: 0x04001D7E RID: 7550
		Invalid
	}
}

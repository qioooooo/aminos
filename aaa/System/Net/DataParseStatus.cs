using System;

namespace System.Net
{
	// Token: 0x020004C1 RID: 1217
	internal enum DataParseStatus
	{
		// Token: 0x04002558 RID: 9560
		NeedMoreData,
		// Token: 0x04002559 RID: 9561
		ContinueParsing,
		// Token: 0x0400255A RID: 9562
		Done,
		// Token: 0x0400255B RID: 9563
		Invalid,
		// Token: 0x0400255C RID: 9564
		DataTooBig
	}
}

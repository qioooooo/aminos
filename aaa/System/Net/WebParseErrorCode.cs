using System;

namespace System.Net
{
	// Token: 0x020004C4 RID: 1220
	internal enum WebParseErrorCode
	{
		// Token: 0x04002568 RID: 9576
		Generic,
		// Token: 0x04002569 RID: 9577
		InvalidHeaderName,
		// Token: 0x0400256A RID: 9578
		InvalidContentLength,
		// Token: 0x0400256B RID: 9579
		IncompleteHeaderLine,
		// Token: 0x0400256C RID: 9580
		CrLfError,
		// Token: 0x0400256D RID: 9581
		InvalidChunkFormat,
		// Token: 0x0400256E RID: 9582
		UnexpectedServerResponse
	}
}

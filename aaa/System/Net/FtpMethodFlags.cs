using System;

namespace System.Net
{
	// Token: 0x020003BE RID: 958
	[Flags]
	internal enum FtpMethodFlags
	{
		// Token: 0x04001DF9 RID: 7673
		None = 0,
		// Token: 0x04001DFA RID: 7674
		IsDownload = 1,
		// Token: 0x04001DFB RID: 7675
		IsUpload = 2,
		// Token: 0x04001DFC RID: 7676
		TakesParameter = 4,
		// Token: 0x04001DFD RID: 7677
		MayTakeParameter = 8,
		// Token: 0x04001DFE RID: 7678
		DoesNotTakeParameter = 16,
		// Token: 0x04001DFF RID: 7679
		ParameterIsDirectory = 32,
		// Token: 0x04001E00 RID: 7680
		ShouldParseForResponseUri = 64,
		// Token: 0x04001E01 RID: 7681
		HasHttpCommand = 128
	}
}

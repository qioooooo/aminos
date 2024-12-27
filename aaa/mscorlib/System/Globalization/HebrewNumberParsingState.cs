using System;

namespace System.Globalization
{
	// Token: 0x020003CD RID: 973
	internal enum HebrewNumberParsingState
	{
		// Token: 0x04001378 RID: 4984
		InvalidHebrewNumber,
		// Token: 0x04001379 RID: 4985
		NotHebrewDigit,
		// Token: 0x0400137A RID: 4986
		FoundEndOfHebrewNumber,
		// Token: 0x0400137B RID: 4987
		ContinueParsing
	}
}

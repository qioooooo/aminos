using System;

namespace System.Globalization
{
	// Token: 0x02000392 RID: 914
	[Flags]
	internal enum DateTimeFormatFlags
	{
		// Token: 0x04001023 RID: 4131
		None = 0,
		// Token: 0x04001024 RID: 4132
		UseGenitiveMonth = 1,
		// Token: 0x04001025 RID: 4133
		UseLeapYearMonth = 2,
		// Token: 0x04001026 RID: 4134
		UseSpacesInMonthNames = 4,
		// Token: 0x04001027 RID: 4135
		UseHebrewRule = 8,
		// Token: 0x04001028 RID: 4136
		UseSpacesInDayNames = 16,
		// Token: 0x04001029 RID: 4137
		UseDigitPrefixInTokens = 32,
		// Token: 0x0400102A RID: 4138
		NotInitialized = -1
	}
}

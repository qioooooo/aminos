using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000390 RID: 912
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum DateTimeStyles
	{
		// Token: 0x04001014 RID: 4116
		None = 0,
		// Token: 0x04001015 RID: 4117
		AllowLeadingWhite = 1,
		// Token: 0x04001016 RID: 4118
		AllowTrailingWhite = 2,
		// Token: 0x04001017 RID: 4119
		AllowInnerWhite = 4,
		// Token: 0x04001018 RID: 4120
		AllowWhiteSpaces = 7,
		// Token: 0x04001019 RID: 4121
		NoCurrentDateDefault = 8,
		// Token: 0x0400101A RID: 4122
		AdjustToUniversal = 16,
		// Token: 0x0400101B RID: 4123
		AssumeLocal = 32,
		// Token: 0x0400101C RID: 4124
		AssumeUniversal = 64,
		// Token: 0x0400101D RID: 4125
		RoundtripKind = 128
	}
}

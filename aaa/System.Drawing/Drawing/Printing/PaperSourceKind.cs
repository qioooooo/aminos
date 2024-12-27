using System;

namespace System.Drawing.Printing
{
	// Token: 0x02000112 RID: 274
	[Serializable]
	public enum PaperSourceKind
	{
		// Token: 0x04000C1A RID: 3098
		Upper = 1,
		// Token: 0x04000C1B RID: 3099
		Lower,
		// Token: 0x04000C1C RID: 3100
		Middle,
		// Token: 0x04000C1D RID: 3101
		Manual,
		// Token: 0x04000C1E RID: 3102
		Envelope,
		// Token: 0x04000C1F RID: 3103
		ManualFeed,
		// Token: 0x04000C20 RID: 3104
		AutomaticFeed,
		// Token: 0x04000C21 RID: 3105
		TractorFeed,
		// Token: 0x04000C22 RID: 3106
		SmallFormat,
		// Token: 0x04000C23 RID: 3107
		LargeFormat,
		// Token: 0x04000C24 RID: 3108
		LargeCapacity,
		// Token: 0x04000C25 RID: 3109
		Cassette = 14,
		// Token: 0x04000C26 RID: 3110
		FormSource,
		// Token: 0x04000C27 RID: 3111
		Custom = 257
	}
}

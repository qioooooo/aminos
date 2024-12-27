using System;

namespace System.IO.Compression
{
	// Token: 0x0200020B RID: 523
	internal enum GZIPHeaderState
	{
		// Token: 0x0400100B RID: 4107
		ReadingID1,
		// Token: 0x0400100C RID: 4108
		ReadingID2,
		// Token: 0x0400100D RID: 4109
		ReadingCM,
		// Token: 0x0400100E RID: 4110
		ReadingFLG,
		// Token: 0x0400100F RID: 4111
		ReadingMMTime,
		// Token: 0x04001010 RID: 4112
		ReadingXFL,
		// Token: 0x04001011 RID: 4113
		ReadingOS,
		// Token: 0x04001012 RID: 4114
		ReadingXLen1,
		// Token: 0x04001013 RID: 4115
		ReadingXLen2,
		// Token: 0x04001014 RID: 4116
		ReadingXLenData,
		// Token: 0x04001015 RID: 4117
		ReadingFileName,
		// Token: 0x04001016 RID: 4118
		ReadingComment,
		// Token: 0x04001017 RID: 4119
		ReadingCRC16Part1,
		// Token: 0x04001018 RID: 4120
		ReadingCRC16Part2,
		// Token: 0x04001019 RID: 4121
		Done,
		// Token: 0x0400101A RID: 4122
		ReadingCRC,
		// Token: 0x0400101B RID: 4123
		ReadingFileSize
	}
}

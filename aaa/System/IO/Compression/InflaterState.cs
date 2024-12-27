using System;

namespace System.IO.Compression
{
	// Token: 0x0200020F RID: 527
	internal enum InflaterState
	{
		// Token: 0x04001037 RID: 4151
		ReadingGZIPHeader,
		// Token: 0x04001038 RID: 4152
		ReadingBFinal = 2,
		// Token: 0x04001039 RID: 4153
		ReadingBType,
		// Token: 0x0400103A RID: 4154
		ReadingNumLitCodes,
		// Token: 0x0400103B RID: 4155
		ReadingNumDistCodes,
		// Token: 0x0400103C RID: 4156
		ReadingNumCodeLengthCodes,
		// Token: 0x0400103D RID: 4157
		ReadingCodeLengthCodes,
		// Token: 0x0400103E RID: 4158
		ReadingTreeCodesBefore,
		// Token: 0x0400103F RID: 4159
		ReadingTreeCodesAfter,
		// Token: 0x04001040 RID: 4160
		DecodeTop,
		// Token: 0x04001041 RID: 4161
		HaveInitialLength,
		// Token: 0x04001042 RID: 4162
		HaveFullLength,
		// Token: 0x04001043 RID: 4163
		HaveDistCode,
		// Token: 0x04001044 RID: 4164
		UncompressedAligning = 15,
		// Token: 0x04001045 RID: 4165
		UncompressedByte1,
		// Token: 0x04001046 RID: 4166
		UncompressedByte2,
		// Token: 0x04001047 RID: 4167
		UncompressedByte3,
		// Token: 0x04001048 RID: 4168
		UncompressedByte4,
		// Token: 0x04001049 RID: 4169
		DecodingUncompressed,
		// Token: 0x0400104A RID: 4170
		StartReadingGZIPFooter,
		// Token: 0x0400104B RID: 4171
		ReadingGZIPFooter,
		// Token: 0x0400104C RID: 4172
		VerifyingGZIPFooter,
		// Token: 0x0400104D RID: 4173
		Done
	}
}

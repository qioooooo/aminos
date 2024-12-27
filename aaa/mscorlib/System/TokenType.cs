using System;

namespace System
{
	// Token: 0x0200038F RID: 911
	internal enum TokenType
	{
		// Token: 0x04000FF3 RID: 4083
		NumberToken = 1,
		// Token: 0x04000FF4 RID: 4084
		YearNumberToken,
		// Token: 0x04000FF5 RID: 4085
		Am,
		// Token: 0x04000FF6 RID: 4086
		Pm,
		// Token: 0x04000FF7 RID: 4087
		MonthToken,
		// Token: 0x04000FF8 RID: 4088
		EndOfString,
		// Token: 0x04000FF9 RID: 4089
		DayOfWeekToken,
		// Token: 0x04000FFA RID: 4090
		TimeZoneToken,
		// Token: 0x04000FFB RID: 4091
		EraToken,
		// Token: 0x04000FFC RID: 4092
		DateWordToken,
		// Token: 0x04000FFD RID: 4093
		UnknownToken,
		// Token: 0x04000FFE RID: 4094
		HebrewNumber,
		// Token: 0x04000FFF RID: 4095
		JapaneseEraToken,
		// Token: 0x04001000 RID: 4096
		TEraToken,
		// Token: 0x04001001 RID: 4097
		IgnorableSymbol,
		// Token: 0x04001002 RID: 4098
		SEP_Unk = 256,
		// Token: 0x04001003 RID: 4099
		SEP_End = 512,
		// Token: 0x04001004 RID: 4100
		SEP_Space = 768,
		// Token: 0x04001005 RID: 4101
		SEP_Am = 1024,
		// Token: 0x04001006 RID: 4102
		SEP_Pm = 1280,
		// Token: 0x04001007 RID: 4103
		SEP_Date = 1536,
		// Token: 0x04001008 RID: 4104
		SEP_Time = 1792,
		// Token: 0x04001009 RID: 4105
		SEP_YearSuff = 2048,
		// Token: 0x0400100A RID: 4106
		SEP_MonthSuff = 2304,
		// Token: 0x0400100B RID: 4107
		SEP_DaySuff = 2560,
		// Token: 0x0400100C RID: 4108
		SEP_HourSuff = 2816,
		// Token: 0x0400100D RID: 4109
		SEP_MinuteSuff = 3072,
		// Token: 0x0400100E RID: 4110
		SEP_SecondSuff = 3328,
		// Token: 0x0400100F RID: 4111
		SEP_LocalTimeMark = 3584,
		// Token: 0x04001010 RID: 4112
		SEP_DateOrOffset = 3840,
		// Token: 0x04001011 RID: 4113
		RegularTokenMask = 255,
		// Token: 0x04001012 RID: 4114
		SeparatorTokenMask = 65280
	}
}

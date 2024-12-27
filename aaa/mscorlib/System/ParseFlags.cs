using System;

namespace System
{
	// Token: 0x0200038C RID: 908
	[Flags]
	internal enum ParseFlags
	{
		// Token: 0x04000FCA RID: 4042
		HaveYear = 1,
		// Token: 0x04000FCB RID: 4043
		HaveMonth = 2,
		// Token: 0x04000FCC RID: 4044
		HaveDay = 4,
		// Token: 0x04000FCD RID: 4045
		HaveHour = 8,
		// Token: 0x04000FCE RID: 4046
		HaveMinute = 16,
		// Token: 0x04000FCF RID: 4047
		HaveSecond = 32,
		// Token: 0x04000FD0 RID: 4048
		HaveTime = 64,
		// Token: 0x04000FD1 RID: 4049
		HaveDate = 128,
		// Token: 0x04000FD2 RID: 4050
		TimeZoneUsed = 256,
		// Token: 0x04000FD3 RID: 4051
		TimeZoneUtc = 512,
		// Token: 0x04000FD4 RID: 4052
		ParsedMonthName = 1024,
		// Token: 0x04000FD5 RID: 4053
		CaptureOffset = 2048,
		// Token: 0x04000FD6 RID: 4054
		YearDefault = 4096,
		// Token: 0x04000FD7 RID: 4055
		Rfc1123Pattern = 8192,
		// Token: 0x04000FD8 RID: 4056
		UtcSortPattern = 16384
	}
}

using System;

namespace System.Diagnostics
{
	// Token: 0x020002AD RID: 685
	[Serializable]
	internal enum LoggingLevels
	{
		// Token: 0x04000A26 RID: 2598
		TraceLevel0,
		// Token: 0x04000A27 RID: 2599
		TraceLevel1,
		// Token: 0x04000A28 RID: 2600
		TraceLevel2,
		// Token: 0x04000A29 RID: 2601
		TraceLevel3,
		// Token: 0x04000A2A RID: 2602
		TraceLevel4,
		// Token: 0x04000A2B RID: 2603
		StatusLevel0 = 20,
		// Token: 0x04000A2C RID: 2604
		StatusLevel1,
		// Token: 0x04000A2D RID: 2605
		StatusLevel2,
		// Token: 0x04000A2E RID: 2606
		StatusLevel3,
		// Token: 0x04000A2F RID: 2607
		StatusLevel4,
		// Token: 0x04000A30 RID: 2608
		WarningLevel = 40,
		// Token: 0x04000A31 RID: 2609
		ErrorLevel = 50,
		// Token: 0x04000A32 RID: 2610
		PanicLevel = 100
	}
}

using System;

namespace System.Xml.Schema
{
	// Token: 0x020002A0 RID: 672
	[Flags]
	internal enum XsdDateTimeFlags
	{
		// Token: 0x0400135D RID: 4957
		DateTime = 1,
		// Token: 0x0400135E RID: 4958
		Time = 2,
		// Token: 0x0400135F RID: 4959
		Date = 4,
		// Token: 0x04001360 RID: 4960
		GYearMonth = 8,
		// Token: 0x04001361 RID: 4961
		GYear = 16,
		// Token: 0x04001362 RID: 4962
		GMonthDay = 32,
		// Token: 0x04001363 RID: 4963
		GDay = 64,
		// Token: 0x04001364 RID: 4964
		GMonth = 128,
		// Token: 0x04001365 RID: 4965
		XdrDateTimeNoTz = 256,
		// Token: 0x04001366 RID: 4966
		XdrDateTime = 512,
		// Token: 0x04001367 RID: 4967
		XdrTimeNoTz = 1024,
		// Token: 0x04001368 RID: 4968
		AllXsd = 255
	}
}

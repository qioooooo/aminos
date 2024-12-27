using System;

namespace System.Globalization
{
	// Token: 0x020003A2 RID: 930
	[Serializable]
	internal class EraInfo
	{
		// Token: 0x06002599 RID: 9625 RVA: 0x00069DAA File Offset: 0x00068DAA
		internal EraInfo(int era, long ticks, int yearOffset, int minEraYear, int maxEraYear)
		{
			this.era = era;
			this.ticks = ticks;
			this.yearOffset = yearOffset;
			this.minEraYear = minEraYear;
			this.maxEraYear = maxEraYear;
		}

		// Token: 0x040010ED RID: 4333
		internal int era;

		// Token: 0x040010EE RID: 4334
		internal long ticks;

		// Token: 0x040010EF RID: 4335
		internal int yearOffset;

		// Token: 0x040010F0 RID: 4336
		internal int minEraYear;

		// Token: 0x040010F1 RID: 4337
		internal int maxEraYear;
	}
}

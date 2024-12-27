using System;

namespace System.Net.Cache
{
	// Token: 0x0200056E RID: 1390
	public enum HttpCacheAgeControl
	{
		// Token: 0x04002920 RID: 10528
		None,
		// Token: 0x04002921 RID: 10529
		MinFresh,
		// Token: 0x04002922 RID: 10530
		MaxAge,
		// Token: 0x04002923 RID: 10531
		MaxStale = 4,
		// Token: 0x04002924 RID: 10532
		MaxAgeAndMinFresh = 3,
		// Token: 0x04002925 RID: 10533
		MaxAgeAndMaxStale = 6
	}
}

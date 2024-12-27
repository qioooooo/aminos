using System;

namespace System.Web.Services.Configuration
{
	// Token: 0x02000141 RID: 321
	[Flags]
	public enum WebServiceProtocols
	{
		// Token: 0x0400063A RID: 1594
		Unknown = 0,
		// Token: 0x0400063B RID: 1595
		HttpSoap = 1,
		// Token: 0x0400063C RID: 1596
		HttpGet = 2,
		// Token: 0x0400063D RID: 1597
		HttpPost = 4,
		// Token: 0x0400063E RID: 1598
		Documentation = 8,
		// Token: 0x0400063F RID: 1599
		HttpPostLocalhost = 16,
		// Token: 0x04000640 RID: 1600
		HttpSoap12 = 32,
		// Token: 0x04000641 RID: 1601
		AnyHttpSoap = 33
	}
}

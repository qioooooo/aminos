using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000073 RID: 115
	public enum SoapMessageStage
	{
		// Token: 0x04000339 RID: 825
		BeforeSerialize = 1,
		// Token: 0x0400033A RID: 826
		AfterSerialize,
		// Token: 0x0400033B RID: 827
		BeforeDeserialize = 4,
		// Token: 0x0400033C RID: 828
		AfterDeserialize = 8
	}
}

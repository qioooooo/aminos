using System;

namespace System.Windows.Forms
{
	// Token: 0x0200024F RID: 591
	[SRDescription("ICurrencyManagerProviderDescr")]
	public interface ICurrencyManagerProvider
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001E90 RID: 7824
		CurrencyManager CurrencyManager { get; }

		// Token: 0x06001E91 RID: 7825
		CurrencyManager GetRelatedCurrencyManager(string dataMember);
	}
}

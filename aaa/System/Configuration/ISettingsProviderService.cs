using System;

namespace System.Configuration
{
	// Token: 0x020006FC RID: 1788
	public interface ISettingsProviderService
	{
		// Token: 0x06003716 RID: 14102
		SettingsProvider GetSettingsProvider(SettingsProperty property);
	}
}

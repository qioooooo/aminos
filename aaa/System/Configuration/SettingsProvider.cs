using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x020006FD RID: 1789
	public abstract class SettingsProvider : ProviderBase
	{
		// Token: 0x06003717 RID: 14103
		public abstract SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection);

		// Token: 0x06003718 RID: 14104
		public abstract void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection);

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003719 RID: 14105
		// (set) Token: 0x0600371A RID: 14106
		public abstract string ApplicationName { get; set; }
	}
}

using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x0200071A RID: 1818
	public class SettingsProviderCollection : ProviderCollection
	{
		// Token: 0x060037A6 RID: 14246 RVA: 0x000EBD24 File Offset: 0x000EAD24
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is SettingsProvider))
			{
				throw new ArgumentException(SR.GetString("Config_provider_must_implement_type", new object[] { typeof(SettingsProvider).ToString() }), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x17000CE7 RID: 3303
		public SettingsProvider this[string name]
		{
			get
			{
				return (SettingsProvider)base[name];
			}
		}
	}
}

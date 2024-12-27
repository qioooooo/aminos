using System;
using System.Configuration.Provider;

namespace System.Configuration
{
	// Token: 0x02000089 RID: 137
	public class ProtectedConfigurationProviderCollection : ProviderCollection
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x00019670 File Offset: 0x00018670
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			if (!(provider is ProtectedConfigurationProvider))
			{
				throw new ArgumentException(SR.GetString("Config_provider_must_implement_type", new object[] { typeof(ProtectedConfigurationProvider).ToString() }), "provider");
			}
			base.Add(provider);
		}

		// Token: 0x17000171 RID: 369
		public ProtectedConfigurationProvider this[string name]
		{
			get
			{
				return (ProtectedConfigurationProvider)base[name];
			}
		}
	}
}

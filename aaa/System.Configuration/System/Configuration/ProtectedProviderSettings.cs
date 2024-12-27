using System;

namespace System.Configuration
{
	// Token: 0x0200008B RID: 139
	public class ProtectedProviderSettings : ConfigurationElement
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x000199F7 File Offset: 0x000189F7
		public ProtectedProviderSettings()
		{
			this._properties = new ConfigurationPropertyCollection();
			this._properties.Add(this._propProviders);
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00019A33 File Offset: 0x00018A33
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00019A3B File Offset: 0x00018A3B
		[ConfigurationProperty("", IsDefaultCollection = true, Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base[this._propProviders];
			}
		}

		// Token: 0x04000372 RID: 882
		private ConfigurationPropertyCollection _properties;

		// Token: 0x04000373 RID: 883
		private readonly ConfigurationProperty _propProviders = new ConfigurationProperty(null, typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}

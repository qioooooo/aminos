using System;

namespace System.Configuration
{
	// Token: 0x02000721 RID: 1825
	public sealed class ClientSettingsSection : ConfigurationSection
	{
		// Token: 0x060037B0 RID: 14256 RVA: 0x000EBE5D File Offset: 0x000EAE5D
		static ClientSettingsSection()
		{
			ClientSettingsSection._properties.Add(ClientSettingsSection._propSettings);
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x060037B2 RID: 14258 RVA: 0x000EBE97 File Offset: 0x000EAE97
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ClientSettingsSection._properties;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x060037B3 RID: 14259 RVA: 0x000EBE9E File Offset: 0x000EAE9E
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public SettingElementCollection Settings
		{
			get
			{
				return (SettingElementCollection)base[ClientSettingsSection._propSettings];
			}
		}

		// Token: 0x040031D9 RID: 12761
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040031DA RID: 12762
		private static readonly ConfigurationProperty _propSettings = new ConfigurationProperty(null, typeof(SettingElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}

using System;

namespace System.Configuration
{
	// Token: 0x02000053 RID: 83
	public sealed class ConnectionStringsSection : ConfigurationSection
	{
		// Token: 0x06000369 RID: 873 RVA: 0x000126AE File Offset: 0x000116AE
		static ConnectionStringsSection()
		{
			ConnectionStringsSection._properties.Add(ConnectionStringsSection._propConnectionStrings);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000126E8 File Offset: 0x000116E8
		protected internal override object GetRuntimeObject()
		{
			this.SetReadOnly();
			return this;
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000126F1 File Offset: 0x000116F1
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ConnectionStringsSection._properties;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600036D RID: 877 RVA: 0x000126F8 File Offset: 0x000116F8
		[ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				return (ConnectionStringSettingsCollection)base[ConnectionStringsSection._propConnectionStrings];
			}
		}

		// Token: 0x040002CF RID: 719
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040002D0 RID: 720
		private static readonly ConfigurationProperty _propConnectionStrings = new ConfigurationProperty(null, typeof(ConnectionStringSettingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}

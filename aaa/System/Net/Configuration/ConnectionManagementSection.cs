using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200064B RID: 1611
	public sealed class ConnectionManagementSection : ConfigurationSection
	{
		// Token: 0x060031E2 RID: 12770 RVA: 0x000D4DB5 File Offset: 0x000D3DB5
		public ConnectionManagementSection()
		{
			this.properties.Add(this.connectionManagement);
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x000D4DF1 File Offset: 0x000D3DF1
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ConnectionManagementElementCollection ConnectionManagement
		{
			get
			{
				return (ConnectionManagementElementCollection)base[this.connectionManagement];
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x000D4E04 File Offset: 0x000D3E04
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002EE7 RID: 12007
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EE8 RID: 12008
		private readonly ConfigurationProperty connectionManagement = new ConfigurationProperty(null, typeof(ConnectionManagementElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}

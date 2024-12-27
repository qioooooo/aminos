using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000649 RID: 1609
	public sealed class ConnectionManagementElement : ConfigurationElement
	{
		// Token: 0x060031CD RID: 12749 RVA: 0x000D4C08 File Offset: 0x000D3C08
		public ConnectionManagementElement()
		{
			this.properties.Add(this.address);
			this.properties.Add(this.maxconnection);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x000D4C85 File Offset: 0x000D3C85
		public ConnectionManagementElement(string address, int maxConnection)
			: this()
		{
			this.Address = address;
			this.MaxConnection = maxConnection;
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060031CF RID: 12751 RVA: 0x000D4C9B File Offset: 0x000D3C9B
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x060031D0 RID: 12752 RVA: 0x000D4CA3 File Offset: 0x000D3CA3
		// (set) Token: 0x060031D1 RID: 12753 RVA: 0x000D4CB6 File Offset: 0x000D3CB6
		[ConfigurationProperty("address", IsRequired = true, IsKey = true)]
		public string Address
		{
			get
			{
				return (string)base[this.address];
			}
			set
			{
				base[this.address] = value;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060031D2 RID: 12754 RVA: 0x000D4CC5 File Offset: 0x000D3CC5
		// (set) Token: 0x060031D3 RID: 12755 RVA: 0x000D4CD8 File Offset: 0x000D3CD8
		[ConfigurationProperty("maxconnection", IsRequired = true, DefaultValue = 1)]
		public int MaxConnection
		{
			get
			{
				return (int)base[this.maxconnection];
			}
			set
			{
				base[this.maxconnection] = value;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000D4CEC File Offset: 0x000D3CEC
		internal string Key
		{
			get
			{
				return this.Address;
			}
		}

		// Token: 0x04002EE4 RID: 12004
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EE5 RID: 12005
		private readonly ConfigurationProperty address = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04002EE6 RID: 12006
		private readonly ConfigurationProperty maxconnection = new ConfigurationProperty("maxconnection", typeof(int), 1, ConfigurationPropertyOptions.None);
	}
}

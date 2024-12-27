using System;

namespace System.Configuration
{
	// Token: 0x02000051 RID: 81
	public sealed class ConnectionStringSettings : ConfigurationElement
	{
		// Token: 0x0600034D RID: 845 RVA: 0x00012494 File Offset: 0x00011494
		static ConnectionStringSettings()
		{
			ConnectionStringSettings._properties.Add(ConnectionStringSettings._propName);
			ConnectionStringSettings._properties.Add(ConnectionStringSettings._propConnectionString);
			ConnectionStringSettings._properties.Add(ConnectionStringSettings._propProviderName);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00012537 File Offset: 0x00011537
		public ConnectionStringSettings()
		{
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001253F File Offset: 0x0001153F
		public ConnectionStringSettings(string name, string connectionString)
			: this()
		{
			this.Name = name;
			this.ConnectionString = connectionString;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00012555 File Offset: 0x00011555
		public ConnectionStringSettings(string name, string connectionString, string providerName)
			: this()
		{
			this.Name = name;
			this.ConnectionString = connectionString;
			this.ProviderName = providerName;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00012572 File Offset: 0x00011572
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0001257A File Offset: 0x0001157A
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ConnectionStringSettings._properties;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00012581 File Offset: 0x00011581
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00012593 File Offset: 0x00011593
		[ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[ConnectionStringSettings._propName];
			}
			set
			{
				base[ConnectionStringSettings._propName] = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000355 RID: 853 RVA: 0x000125A1 File Offset: 0x000115A1
		// (set) Token: 0x06000356 RID: 854 RVA: 0x000125B3 File Offset: 0x000115B3
		[ConfigurationProperty("connectionString", Options = ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
		public string ConnectionString
		{
			get
			{
				return (string)base[ConnectionStringSettings._propConnectionString];
			}
			set
			{
				base[ConnectionStringSettings._propConnectionString] = value;
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x000125C1 File Offset: 0x000115C1
		public override string ToString()
		{
			return this.ConnectionString;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000358 RID: 856 RVA: 0x000125C9 File Offset: 0x000115C9
		// (set) Token: 0x06000359 RID: 857 RVA: 0x000125DB File Offset: 0x000115DB
		[ConfigurationProperty("providerName", DefaultValue = "System.Data.SqlClient")]
		public string ProviderName
		{
			get
			{
				return (string)base[ConnectionStringSettings._propProviderName];
			}
			set
			{
				base[ConnectionStringSettings._propProviderName] = value;
			}
		}

		// Token: 0x040002CA RID: 714
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040002CB RID: 715
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, ConfigurationProperty.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040002CC RID: 716
		private static readonly ConfigurationProperty _propConnectionString = new ConfigurationProperty("connectionString", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040002CD RID: 717
		private static readonly ConfigurationProperty _propProviderName = new ConfigurationProperty("providerName", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}

using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000652 RID: 1618
	public sealed class Ipv6Element : ConfigurationElement
	{
		// Token: 0x06003217 RID: 12823 RVA: 0x000D5A64 File Offset: 0x000D4A64
		public Ipv6Element()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x000D5AB4 File Offset: 0x000D4AB4
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06003219 RID: 12825 RVA: 0x000D5ABC File Offset: 0x000D4ABC
		// (set) Token: 0x0600321A RID: 12826 RVA: 0x000D5ACF File Offset: 0x000D4ACF
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get
			{
				return (bool)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x04002F01 RID: 12033
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F02 RID: 12034
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

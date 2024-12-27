using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000655 RID: 1621
	public sealed class ModuleElement : ConfigurationElement
	{
		// Token: 0x06003220 RID: 12832 RVA: 0x000D5B24 File Offset: 0x000D4B24
		public ModuleElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06003221 RID: 12833 RVA: 0x000D5B64 File Offset: 0x000D4B64
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x000D5B6C File Offset: 0x000D4B6C
		// (set) Token: 0x06003223 RID: 12835 RVA: 0x000D5B7F File Offset: 0x000D4B7F
		[ConfigurationProperty("type")]
		public string Type
		{
			get
			{
				return (string)base[this.type];
			}
			set
			{
				base[this.type] = value;
			}
		}

		// Token: 0x04002F04 RID: 12036
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F05 RID: 12037
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}

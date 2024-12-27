using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000657 RID: 1623
	public sealed class PerformanceCountersElement : ConfigurationElement
	{
		// Token: 0x0600322D RID: 12845 RVA: 0x000D5C58 File Offset: 0x000D4C58
		public PerformanceCountersElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x000D5CA8 File Offset: 0x000D4CA8
		// (set) Token: 0x0600322F RID: 12847 RVA: 0x000D5CBB File Offset: 0x000D4CBB
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

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x000D5CCF File Offset: 0x000D4CCF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F06 RID: 12038
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F07 RID: 12039
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

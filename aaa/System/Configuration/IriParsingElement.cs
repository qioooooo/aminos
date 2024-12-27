using System;

namespace System.Configuration
{
	// Token: 0x02000673 RID: 1651
	public sealed class IriParsingElement : ConfigurationElement
	{
		// Token: 0x060032F8 RID: 13048 RVA: 0x000D7BF0 File Offset: 0x000D6BF0
		public IriParsingElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x060032F9 RID: 13049 RVA: 0x000D7C40 File Offset: 0x000D6C40
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x060032FA RID: 13050 RVA: 0x000D7C48 File Offset: 0x000D6C48
		// (set) Token: 0x060032FB RID: 13051 RVA: 0x000D7C5B File Offset: 0x000D6C5B
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

		// Token: 0x04002F79 RID: 12153
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F7A RID: 12154
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

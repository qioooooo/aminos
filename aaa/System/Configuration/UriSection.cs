using System;

namespace System.Configuration
{
	// Token: 0x02000671 RID: 1649
	public sealed class UriSection : ConfigurationSection
	{
		// Token: 0x060032EF RID: 13039 RVA: 0x000D7A9C File Offset: 0x000D6A9C
		public UriSection()
		{
			this.properties.Add(this.idn);
			this.properties.Add(this.iriParsing);
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x060032F0 RID: 13040 RVA: 0x000D7B14 File Offset: 0x000D6B14
		[ConfigurationProperty("idn")]
		public IdnElement Idn
		{
			get
			{
				return (IdnElement)base[this.idn];
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x000D7B27 File Offset: 0x000D6B27
		[ConfigurationProperty("iriParsing")]
		public IriParsingElement IriParsing
		{
			get
			{
				return (IriParsingElement)base[this.iriParsing];
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x060032F2 RID: 13042 RVA: 0x000D7B3A File Offset: 0x000D6B3A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F73 RID: 12147
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F74 RID: 12148
		private readonly ConfigurationProperty idn = new ConfigurationProperty("idn", typeof(IdnElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F75 RID: 12149
		private readonly ConfigurationProperty iriParsing = new ConfigurationProperty("iriParsing", typeof(IriParsingElement), null, ConfigurationPropertyOptions.None);
	}
}

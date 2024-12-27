using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200064D RID: 1613
	public sealed class DefaultProxySection : ConfigurationSection
	{
		// Token: 0x060031E9 RID: 12777 RVA: 0x000D4F44 File Offset: 0x000D3F44
		public DefaultProxySection()
		{
			this.properties.Add(this.bypasslist);
			this.properties.Add(this.module);
			this.properties.Add(this.proxy);
			this.properties.Add(this.enabled);
			this.properties.Add(this.useDefaultCredentials);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000D5050 File Offset: 0x000D4050
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "defaultProxy" }), ex);
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x060031EB RID: 12779 RVA: 0x000D50AC File Offset: 0x000D40AC
		[ConfigurationProperty("bypasslist")]
		public BypassElementCollection BypassList
		{
			get
			{
				return (BypassElementCollection)base[this.bypasslist];
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x060031EC RID: 12780 RVA: 0x000D50BF File Offset: 0x000D40BF
		[ConfigurationProperty("module")]
		public ModuleElement Module
		{
			get
			{
				return (ModuleElement)base[this.module];
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x060031ED RID: 12781 RVA: 0x000D50D2 File Offset: 0x000D40D2
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060031EE RID: 12782 RVA: 0x000D50DA File Offset: 0x000D40DA
		[ConfigurationProperty("proxy")]
		public ProxyElement Proxy
		{
			get
			{
				return (ProxyElement)base[this.proxy];
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x000D50ED File Offset: 0x000D40ED
		// (set) Token: 0x060031F0 RID: 12784 RVA: 0x000D5100 File Offset: 0x000D4100
		[ConfigurationProperty("enabled", DefaultValue = true)]
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

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x000D5114 File Offset: 0x000D4114
		// (set) Token: 0x060031F2 RID: 12786 RVA: 0x000D5127 File Offset: 0x000D4127
		[ConfigurationProperty("useDefaultCredentials", DefaultValue = false)]
		public bool UseDefaultCredentials
		{
			get
			{
				return (bool)base[this.useDefaultCredentials];
			}
			set
			{
				base[this.useDefaultCredentials] = value;
			}
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000D513C File Offset: 0x000D413C
		protected override void Reset(ConfigurationElement parentElement)
		{
			DefaultProxySection defaultProxySection = new DefaultProxySection();
			defaultProxySection.InitializeDefault();
			base.Reset(defaultProxySection);
		}

		// Token: 0x04002EEB RID: 12011
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EEC RID: 12012
		private readonly ConfigurationProperty bypasslist = new ConfigurationProperty("bypasslist", typeof(BypassElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002EED RID: 12013
		private readonly ConfigurationProperty module = new ConfigurationProperty("module", typeof(ModuleElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002EEE RID: 12014
		private readonly ConfigurationProperty proxy = new ConfigurationProperty("proxy", typeof(ProxyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002EEF RID: 12015
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF0 RID: 12016
		private readonly ConfigurationProperty useDefaultCredentials = new ConfigurationProperty("useDefaultCredentials", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

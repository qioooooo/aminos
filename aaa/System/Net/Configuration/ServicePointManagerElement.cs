using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000660 RID: 1632
	public sealed class ServicePointManagerElement : ConfigurationElement
	{
		// Token: 0x0600327B RID: 12923 RVA: 0x000D6860 File Offset: 0x000D5860
		public ServicePointManagerElement()
		{
			this.properties.Add(this.checkCertificateName);
			this.properties.Add(this.checkCertificateRevocationList);
			this.properties.Add(this.dnsRefreshTimeout);
			this.properties.Add(this.enableDnsRoundRobin);
			this.properties.Add(this.expect100Continue);
			this.properties.Add(this.useNagleAlgorithm);
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000D69B8 File Offset: 0x000D59B8
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			PropertyInformation[] array = new PropertyInformation[]
			{
				base.ElementInformation.Properties["checkCertificateName"],
				base.ElementInformation.Properties["checkCertificateRevocationList"]
			};
			foreach (PropertyInformation propertyInformation in array)
			{
				if (propertyInformation.ValueOrigin == PropertyValueOrigin.SetHere)
				{
					try
					{
						ExceptionHelper.UnmanagedPermission.Demand();
					}
					catch (Exception ex)
					{
						throw new ConfigurationErrorsException(SR.GetString("net_config_property_permission", new object[] { propertyInformation.Name }), ex);
					}
				}
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600327D RID: 12925 RVA: 0x000D6A74 File Offset: 0x000D5A74
		// (set) Token: 0x0600327E RID: 12926 RVA: 0x000D6A87 File Offset: 0x000D5A87
		[ConfigurationProperty("checkCertificateName", DefaultValue = true)]
		public bool CheckCertificateName
		{
			get
			{
				return (bool)base[this.checkCertificateName];
			}
			set
			{
				base[this.checkCertificateName] = value;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600327F RID: 12927 RVA: 0x000D6A9B File Offset: 0x000D5A9B
		// (set) Token: 0x06003280 RID: 12928 RVA: 0x000D6AAE File Offset: 0x000D5AAE
		[ConfigurationProperty("checkCertificateRevocationList", DefaultValue = false)]
		public bool CheckCertificateRevocationList
		{
			get
			{
				return (bool)base[this.checkCertificateRevocationList];
			}
			set
			{
				base[this.checkCertificateRevocationList] = value;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003281 RID: 12929 RVA: 0x000D6AC2 File Offset: 0x000D5AC2
		// (set) Token: 0x06003282 RID: 12930 RVA: 0x000D6AD5 File Offset: 0x000D5AD5
		[ConfigurationProperty("dnsRefreshTimeout", DefaultValue = 120000)]
		public int DnsRefreshTimeout
		{
			get
			{
				return (int)base[this.dnsRefreshTimeout];
			}
			set
			{
				base[this.dnsRefreshTimeout] = value;
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06003283 RID: 12931 RVA: 0x000D6AE9 File Offset: 0x000D5AE9
		// (set) Token: 0x06003284 RID: 12932 RVA: 0x000D6AFC File Offset: 0x000D5AFC
		[ConfigurationProperty("enableDnsRoundRobin", DefaultValue = false)]
		public bool EnableDnsRoundRobin
		{
			get
			{
				return (bool)base[this.enableDnsRoundRobin];
			}
			set
			{
				base[this.enableDnsRoundRobin] = value;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06003285 RID: 12933 RVA: 0x000D6B10 File Offset: 0x000D5B10
		// (set) Token: 0x06003286 RID: 12934 RVA: 0x000D6B23 File Offset: 0x000D5B23
		[ConfigurationProperty("expect100Continue", DefaultValue = true)]
		public bool Expect100Continue
		{
			get
			{
				return (bool)base[this.expect100Continue];
			}
			set
			{
				base[this.expect100Continue] = value;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06003287 RID: 12935 RVA: 0x000D6B37 File Offset: 0x000D5B37
		// (set) Token: 0x06003288 RID: 12936 RVA: 0x000D6B4A File Offset: 0x000D5B4A
		[ConfigurationProperty("useNagleAlgorithm", DefaultValue = true)]
		public bool UseNagleAlgorithm
		{
			get
			{
				return (bool)base[this.useNagleAlgorithm];
			}
			set
			{
				base[this.useNagleAlgorithm] = value;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x000D6B5E File Offset: 0x000D5B5E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F43 RID: 12099
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F44 RID: 12100
		private readonly ConfigurationProperty checkCertificateName = new ConfigurationProperty("checkCertificateName", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002F45 RID: 12101
		private readonly ConfigurationProperty checkCertificateRevocationList = new ConfigurationProperty("checkCertificateRevocationList", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F46 RID: 12102
		private readonly ConfigurationProperty dnsRefreshTimeout = new ConfigurationProperty("dnsRefreshTimeout", typeof(int), 120000, null, new TimeoutValidator(true), ConfigurationPropertyOptions.None);

		// Token: 0x04002F47 RID: 12103
		private readonly ConfigurationProperty enableDnsRoundRobin = new ConfigurationProperty("enableDnsRoundRobin", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F48 RID: 12104
		private readonly ConfigurationProperty expect100Continue = new ConfigurationProperty("expect100Continue", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002F49 RID: 12105
		private readonly ConfigurationProperty useNagleAlgorithm = new ConfigurationProperty("useNagleAlgorithm", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}

using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x02000650 RID: 1616
	public sealed class HttpCachePolicyElement : ConfigurationElement
	{
		// Token: 0x06003203 RID: 12803 RVA: 0x000D57C4 File Offset: 0x000D47C4
		public HttpCachePolicyElement()
		{
			this.properties.Add(this.maximumAge);
			this.properties.Add(this.maximumStale);
			this.properties.Add(this.minimumFresh);
			this.properties.Add(this.policyLevel);
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x000D58B6 File Offset: 0x000D48B6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x000D58BE File Offset: 0x000D48BE
		// (set) Token: 0x06003206 RID: 12806 RVA: 0x000D58D1 File Offset: 0x000D48D1
		[ConfigurationProperty("maximumAge", DefaultValue = "10675199.02:48:05.4775807")]
		public TimeSpan MaximumAge
		{
			get
			{
				return (TimeSpan)base[this.maximumAge];
			}
			set
			{
				base[this.maximumAge] = value;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x000D58E5 File Offset: 0x000D48E5
		// (set) Token: 0x06003208 RID: 12808 RVA: 0x000D58F8 File Offset: 0x000D48F8
		[ConfigurationProperty("maximumStale", DefaultValue = "-10675199.02:48:05.4775808")]
		public TimeSpan MaximumStale
		{
			get
			{
				return (TimeSpan)base[this.maximumStale];
			}
			set
			{
				base[this.maximumStale] = value;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x000D590C File Offset: 0x000D490C
		// (set) Token: 0x0600320A RID: 12810 RVA: 0x000D591F File Offset: 0x000D491F
		[ConfigurationProperty("minimumFresh", DefaultValue = "-10675199.02:48:05.4775808")]
		public TimeSpan MinimumFresh
		{
			get
			{
				return (TimeSpan)base[this.minimumFresh];
			}
			set
			{
				base[this.minimumFresh] = value;
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600320B RID: 12811 RVA: 0x000D5933 File Offset: 0x000D4933
		// (set) Token: 0x0600320C RID: 12812 RVA: 0x000D5946 File Offset: 0x000D4946
		[ConfigurationProperty("policyLevel", IsRequired = true, DefaultValue = HttpRequestCacheLevel.Default)]
		public HttpRequestCacheLevel PolicyLevel
		{
			get
			{
				return (HttpRequestCacheLevel)base[this.policyLevel];
			}
			set
			{
				base[this.policyLevel] = value;
			}
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x000D595A File Offset: 0x000D495A
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.wasReadFromConfig = true;
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000D596C File Offset: 0x000D496C
		protected override void Reset(ConfigurationElement parentElement)
		{
			if (parentElement != null)
			{
				HttpCachePolicyElement httpCachePolicyElement = (HttpCachePolicyElement)parentElement;
				this.wasReadFromConfig = httpCachePolicyElement.wasReadFromConfig;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x0600320F RID: 12815 RVA: 0x000D5996 File Offset: 0x000D4996
		internal bool WasReadFromConfig
		{
			get
			{
				return this.wasReadFromConfig;
			}
		}

		// Token: 0x04002EF8 RID: 12024
		private bool wasReadFromConfig;

		// Token: 0x04002EF9 RID: 12025
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EFA RID: 12026
		private readonly ConfigurationProperty maximumAge = new ConfigurationProperty("maximumAge", typeof(TimeSpan), TimeSpan.MaxValue, ConfigurationPropertyOptions.None);

		// Token: 0x04002EFB RID: 12027
		private readonly ConfigurationProperty maximumStale = new ConfigurationProperty("maximumStale", typeof(TimeSpan), TimeSpan.MinValue, ConfigurationPropertyOptions.None);

		// Token: 0x04002EFC RID: 12028
		private readonly ConfigurationProperty minimumFresh = new ConfigurationProperty("minimumFresh", typeof(TimeSpan), TimeSpan.MinValue, ConfigurationPropertyOptions.None);

		// Token: 0x04002EFD RID: 12029
		private readonly ConfigurationProperty policyLevel = new ConfigurationProperty("policyLevel", typeof(HttpRequestCacheLevel), HttpRequestCacheLevel.Default, ConfigurationPropertyOptions.None);
	}
}

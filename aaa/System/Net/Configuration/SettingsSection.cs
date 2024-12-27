using System;
using System.Collections;
using System.Configuration;
using System.Net.Cache;

namespace System.Net.Configuration
{
	// Token: 0x0200065E RID: 1630
	public sealed class SettingsSection : ConfigurationSection
	{
		// Token: 0x06003258 RID: 12888 RVA: 0x000D63C8 File Offset: 0x000D53C8
		internal static void EnsureConfigLoaded()
		{
			try
			{
				AuthenticationManager.EnsureConfigLoaded();
				bool isCachingEnabled = RequestCacheManager.IsCachingEnabled;
				int defaultConnectionLimit = global::System.Net.ServicePointManager.DefaultConnectionLimit;
				bool expect100Continue = global::System.Net.ServicePointManager.Expect100Continue;
				ArrayList prefixList = WebRequest.PrefixList;
				IWebProxy internalDefaultWebProxy = WebRequest.InternalDefaultWebProxy;
				NetworkingPerfCounters.Initialize();
			}
			catch
			{
			}
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x000D6414 File Offset: 0x000D5414
		public SettingsSection()
		{
			this.properties.Add(this.httpWebRequest);
			this.properties.Add(this.ipv6);
			this.properties.Add(this.servicePointManager);
			this.properties.Add(this.socket);
			this.properties.Add(this.webProxyScript);
			this.properties.Add(this.performanceCounters);
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x000D6540 File Offset: 0x000D5540
		[ConfigurationProperty("httpWebRequest")]
		public HttpWebRequestElement HttpWebRequest
		{
			get
			{
				return (HttpWebRequestElement)base[this.httpWebRequest];
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x000D6553 File Offset: 0x000D5553
		[ConfigurationProperty("ipv6")]
		public Ipv6Element Ipv6
		{
			get
			{
				return (Ipv6Element)base[this.ipv6];
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x000D6566 File Offset: 0x000D5566
		[ConfigurationProperty("servicePointManager")]
		public ServicePointManagerElement ServicePointManager
		{
			get
			{
				return (ServicePointManagerElement)base[this.servicePointManager];
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x000D6579 File Offset: 0x000D5579
		[ConfigurationProperty("socket")]
		public SocketElement Socket
		{
			get
			{
				return (SocketElement)base[this.socket];
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x000D658C File Offset: 0x000D558C
		[ConfigurationProperty("webProxyScript")]
		public WebProxyScriptElement WebProxyScript
		{
			get
			{
				return (WebProxyScriptElement)base[this.webProxyScript];
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600325F RID: 12895 RVA: 0x000D659F File Offset: 0x000D559F
		[ConfigurationProperty("performanceCounters")]
		public PerformanceCountersElement PerformanceCounters
		{
			get
			{
				return (PerformanceCountersElement)base[this.performanceCounters];
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x000D65B2 File Offset: 0x000D55B2
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F2B RID: 12075
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F2C RID: 12076
		private readonly ConfigurationProperty httpWebRequest = new ConfigurationProperty("httpWebRequest", typeof(HttpWebRequestElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F2D RID: 12077
		private readonly ConfigurationProperty ipv6 = new ConfigurationProperty("ipv6", typeof(Ipv6Element), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F2E RID: 12078
		private readonly ConfigurationProperty servicePointManager = new ConfigurationProperty("servicePointManager", typeof(ServicePointManagerElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F2F RID: 12079
		private readonly ConfigurationProperty socket = new ConfigurationProperty("socket", typeof(SocketElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F30 RID: 12080
		private readonly ConfigurationProperty webProxyScript = new ConfigurationProperty("webProxyScript", typeof(WebProxyScriptElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F31 RID: 12081
		private readonly ConfigurationProperty performanceCounters = new ConfigurationProperty("performanceCounters", typeof(PerformanceCountersElement), null, ConfigurationPropertyOptions.None);
	}
}

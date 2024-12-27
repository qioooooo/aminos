using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x0200065C RID: 1628
	public sealed class RequestCachingSection : ConfigurationSection
	{
		// Token: 0x0600323D RID: 12861 RVA: 0x000D5EE8 File Offset: 0x000D4EE8
		public RequestCachingSection()
		{
			this.properties.Add(this.disableAllCaching);
			this.properties.Add(this.defaultPolicyLevel);
			this.properties.Add(this.isPrivateCache);
			this.properties.Add(this.defaultHttpCachePolicy);
			this.properties.Add(this.defaultFtpCachePolicy);
			this.properties.Add(this.unspecifiedMaximumAge);
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x000D6035 File Offset: 0x000D5035
		[ConfigurationProperty("defaultHttpCachePolicy")]
		public HttpCachePolicyElement DefaultHttpCachePolicy
		{
			get
			{
				return (HttpCachePolicyElement)base[this.defaultHttpCachePolicy];
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x0600323F RID: 12863 RVA: 0x000D6048 File Offset: 0x000D5048
		[ConfigurationProperty("defaultFtpCachePolicy")]
		public FtpCachePolicyElement DefaultFtpCachePolicy
		{
			get
			{
				return (FtpCachePolicyElement)base[this.defaultFtpCachePolicy];
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003240 RID: 12864 RVA: 0x000D605B File Offset: 0x000D505B
		// (set) Token: 0x06003241 RID: 12865 RVA: 0x000D606E File Offset: 0x000D506E
		[ConfigurationProperty("defaultPolicyLevel", DefaultValue = RequestCacheLevel.BypassCache)]
		public RequestCacheLevel DefaultPolicyLevel
		{
			get
			{
				return (RequestCacheLevel)base[this.defaultPolicyLevel];
			}
			set
			{
				base[this.defaultPolicyLevel] = value;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003242 RID: 12866 RVA: 0x000D6082 File Offset: 0x000D5082
		// (set) Token: 0x06003243 RID: 12867 RVA: 0x000D6095 File Offset: 0x000D5095
		[ConfigurationProperty("disableAllCaching", DefaultValue = false)]
		public bool DisableAllCaching
		{
			get
			{
				return (bool)base[this.disableAllCaching];
			}
			set
			{
				base[this.disableAllCaching] = value;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x000D60A9 File Offset: 0x000D50A9
		// (set) Token: 0x06003245 RID: 12869 RVA: 0x000D60BC File Offset: 0x000D50BC
		[ConfigurationProperty("isPrivateCache", DefaultValue = true)]
		public bool IsPrivateCache
		{
			get
			{
				return (bool)base[this.isPrivateCache];
			}
			set
			{
				base[this.isPrivateCache] = value;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x06003246 RID: 12870 RVA: 0x000D60D0 File Offset: 0x000D50D0
		// (set) Token: 0x06003247 RID: 12871 RVA: 0x000D60E3 File Offset: 0x000D50E3
		[ConfigurationProperty("unspecifiedMaximumAge", DefaultValue = "1.00:00:00")]
		public TimeSpan UnspecifiedMaximumAge
		{
			get
			{
				return (TimeSpan)base[this.unspecifiedMaximumAge];
			}
			set
			{
				base[this.unspecifiedMaximumAge] = value;
			}
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000D60F8 File Offset: 0x000D50F8
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			bool flag = this.DisableAllCaching;
			base.DeserializeElement(reader, serializeCollectionKey);
			if (flag)
			{
				this.DisableAllCaching = true;
			}
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000D6120 File Offset: 0x000D5120
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
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[] { "requestCaching" }), ex);
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x000D617C File Offset: 0x000D517C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F1A RID: 12058
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F1B RID: 12059
		private readonly ConfigurationProperty defaultHttpCachePolicy = new ConfigurationProperty("defaultHttpCachePolicy", typeof(HttpCachePolicyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F1C RID: 12060
		private readonly ConfigurationProperty defaultFtpCachePolicy = new ConfigurationProperty("defaultFtpCachePolicy", typeof(FtpCachePolicyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F1D RID: 12061
		private readonly ConfigurationProperty defaultPolicyLevel = new ConfigurationProperty("defaultPolicyLevel", typeof(RequestCacheLevel), RequestCacheLevel.BypassCache, ConfigurationPropertyOptions.None);

		// Token: 0x04002F1E RID: 12062
		private readonly ConfigurationProperty disableAllCaching = new ConfigurationProperty("disableAllCaching", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F1F RID: 12063
		private readonly ConfigurationProperty isPrivateCache = new ConfigurationProperty("isPrivateCache", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002F20 RID: 12064
		private readonly ConfigurationProperty unspecifiedMaximumAge = new ConfigurationProperty("unspecifiedMaximumAge", typeof(TimeSpan), TimeSpan.FromDays(1.0), ConfigurationPropertyOptions.None);
	}
}

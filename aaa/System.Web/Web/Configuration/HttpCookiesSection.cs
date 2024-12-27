using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001F8 RID: 504
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCookiesSection : ConfigurationSection
	{
		// Token: 0x06001B7B RID: 7035 RVA: 0x0007F4E8 File Offset: 0x0007E4E8
		static HttpCookiesSection()
		{
			HttpCookiesSection._properties.Add(HttpCookiesSection._propHttpOnlyCookies);
			HttpCookiesSection._properties.Add(HttpCookiesSection._propRequireSSL);
			HttpCookiesSection._properties.Add(HttpCookiesSection._propDomain);
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001B7D RID: 7037 RVA: 0x0007F593 File Offset: 0x0007E593
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpCookiesSection._properties;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x0007F59A File Offset: 0x0007E59A
		// (set) Token: 0x06001B7F RID: 7039 RVA: 0x0007F5AC File Offset: 0x0007E5AC
		[ConfigurationProperty("httpOnlyCookies", DefaultValue = false)]
		public bool HttpOnlyCookies
		{
			get
			{
				return (bool)base[HttpCookiesSection._propHttpOnlyCookies];
			}
			set
			{
				base[HttpCookiesSection._propHttpOnlyCookies] = value;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x0007F5BF File Offset: 0x0007E5BF
		// (set) Token: 0x06001B81 RID: 7041 RVA: 0x0007F5D1 File Offset: 0x0007E5D1
		[ConfigurationProperty("requireSSL", DefaultValue = false)]
		public bool RequireSSL
		{
			get
			{
				return (bool)base[HttpCookiesSection._propRequireSSL];
			}
			set
			{
				base[HttpCookiesSection._propRequireSSL] = value;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x0007F5E4 File Offset: 0x0007E5E4
		// (set) Token: 0x06001B83 RID: 7043 RVA: 0x0007F5F6 File Offset: 0x0007E5F6
		[ConfigurationProperty("domain", DefaultValue = "")]
		public string Domain
		{
			get
			{
				return (string)base[HttpCookiesSection._propDomain];
			}
			set
			{
				base[HttpCookiesSection._propDomain] = value;
			}
		}

		// Token: 0x0400186C RID: 6252
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400186D RID: 6253
		private static readonly ConfigurationProperty _propHttpOnlyCookies = new ConfigurationProperty("httpOnlyCookies", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400186E RID: 6254
		private static readonly ConfigurationProperty _propRequireSSL = new ConfigurationProperty("requireSSL", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400186F RID: 6255
		private static readonly ConfigurationProperty _propDomain = new ConfigurationProperty("domain", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}

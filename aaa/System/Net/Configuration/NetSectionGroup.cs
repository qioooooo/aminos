using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000656 RID: 1622
	public sealed class NetSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06003225 RID: 12837 RVA: 0x000D5B96 File Offset: 0x000D4B96
		[ConfigurationProperty("authenticationModules")]
		public AuthenticationModulesSection AuthenticationModules
		{
			get
			{
				return (AuthenticationModulesSection)base.Sections["authenticationModules"];
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06003226 RID: 12838 RVA: 0x000D5BAD File Offset: 0x000D4BAD
		[ConfigurationProperty("connectionManagement")]
		public ConnectionManagementSection ConnectionManagement
		{
			get
			{
				return (ConnectionManagementSection)base.Sections["connectionManagement"];
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003227 RID: 12839 RVA: 0x000D5BC4 File Offset: 0x000D4BC4
		[ConfigurationProperty("defaultProxy")]
		public DefaultProxySection DefaultProxy
		{
			get
			{
				return (DefaultProxySection)base.Sections["defaultProxy"];
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06003228 RID: 12840 RVA: 0x000D5BDB File Offset: 0x000D4BDB
		public MailSettingsSectionGroup MailSettings
		{
			get
			{
				return (MailSettingsSectionGroup)base.SectionGroups["mailSettings"];
			}
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000D5BF2 File Offset: 0x000D4BF2
		public static NetSectionGroup GetSectionGroup(Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			return config.GetSectionGroup("system.net") as NetSectionGroup;
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x0600322A RID: 12842 RVA: 0x000D5C12 File Offset: 0x000D4C12
		[ConfigurationProperty("requestCaching")]
		public RequestCachingSection RequestCaching
		{
			get
			{
				return (RequestCachingSection)base.Sections["requestCaching"];
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x0600322B RID: 12843 RVA: 0x000D5C29 File Offset: 0x000D4C29
		[ConfigurationProperty("settings")]
		public SettingsSection Settings
		{
			get
			{
				return (SettingsSection)base.Sections["settings"];
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x000D5C40 File Offset: 0x000D4C40
		[ConfigurationProperty("webRequestModules")]
		public WebRequestModulesSection WebRequestModules
		{
			get
			{
				return (WebRequestModulesSection)base.Sections["webRequestModules"];
			}
		}
	}
}

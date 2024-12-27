using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000250 RID: 592
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SystemWebCachingSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x0008ADD0 File Offset: 0x00089DD0
		[ConfigurationProperty("cache")]
		public CacheSection Cache
		{
			get
			{
				return (CacheSection)base.Sections["cache"];
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x0008ADE7 File Offset: 0x00089DE7
		[ConfigurationProperty("outputCache")]
		public OutputCacheSection OutputCache
		{
			get
			{
				return (OutputCacheSection)base.Sections["outputCache"];
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0008ADFE File Offset: 0x00089DFE
		[ConfigurationProperty("outputCacheSettings")]
		public OutputCacheSettingsSection OutputCacheSettings
		{
			get
			{
				return (OutputCacheSettingsSection)base.Sections["outputCacheSettings"];
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x0008AE15 File Offset: 0x00089E15
		[ConfigurationProperty("sqlCacheDependency")]
		public SqlCacheDependencySection SqlCacheDependency
		{
			get
			{
				return (SqlCacheDependencySection)base.Sections["sqlCacheDependency"];
			}
		}
	}
}

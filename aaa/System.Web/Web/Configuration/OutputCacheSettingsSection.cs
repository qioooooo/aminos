using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200021B RID: 539
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class OutputCacheSettingsSection : ConfigurationSection
	{
		// Token: 0x06001CEC RID: 7404 RVA: 0x00083B22 File Offset: 0x00082B22
		static OutputCacheSettingsSection()
		{
			OutputCacheSettingsSection._properties.Add(OutputCacheSettingsSection._propOutputCacheProfiles);
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x00083B60 File Offset: 0x00082B60
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return OutputCacheSettingsSection._properties;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x00083B67 File Offset: 0x00082B67
		[ConfigurationProperty("outputCacheProfiles")]
		public OutputCacheProfileCollection OutputCacheProfiles
		{
			get
			{
				return (OutputCacheProfileCollection)base[OutputCacheSettingsSection._propOutputCacheProfiles];
			}
		}

		// Token: 0x0400191B RID: 6427
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400191C RID: 6428
		private static readonly ConfigurationProperty _propOutputCacheProfiles = new ConfigurationProperty("outputCacheProfiles", typeof(OutputCacheProfileCollection), null, ConfigurationPropertyOptions.None);
	}
}

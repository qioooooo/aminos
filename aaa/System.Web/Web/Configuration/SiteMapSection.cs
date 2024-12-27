using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200024A RID: 586
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SiteMapSection : ConfigurationSection
	{
		// Token: 0x06001F04 RID: 7940 RVA: 0x0008A4C4 File Offset: 0x000894C4
		static SiteMapSection()
		{
			SiteMapSection._properties.Add(SiteMapSection._propDefaultProvider);
			SiteMapSection._properties.Add(SiteMapSection._propEnabled);
			SiteMapSection._properties.Add(SiteMapSection._propProviders);
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06001F06 RID: 7942 RVA: 0x0008A570 File Offset: 0x00089570
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SiteMapSection._properties;
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x0008A577 File Offset: 0x00089577
		// (set) Token: 0x06001F08 RID: 7944 RVA: 0x0008A589 File Offset: 0x00089589
		[ConfigurationProperty("defaultProvider", DefaultValue = "AspNetXmlSiteMapProvider")]
		[StringValidator(MinLength = 1)]
		public string DefaultProvider
		{
			get
			{
				return (string)base[SiteMapSection._propDefaultProvider];
			}
			set
			{
				base[SiteMapSection._propDefaultProvider] = value;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x0008A597 File Offset: 0x00089597
		// (set) Token: 0x06001F0A RID: 7946 RVA: 0x0008A5A9 File Offset: 0x000895A9
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get
			{
				return (bool)base[SiteMapSection._propEnabled];
			}
			set
			{
				base[SiteMapSection._propEnabled] = value;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06001F0B RID: 7947 RVA: 0x0008A5BC File Offset: 0x000895BC
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base[SiteMapSection._propProviders];
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x0008A5D0 File Offset: 0x000895D0
		internal SiteMapProviderCollection ProvidersInternal
		{
			get
			{
				if (this._siteMapProviders == null)
				{
					lock (this)
					{
						if (this._siteMapProviders == null)
						{
							SiteMapProviderCollection siteMapProviderCollection = new SiteMapProviderCollection();
							ProvidersHelper.InstantiateProviders(this.Providers, siteMapProviderCollection, typeof(SiteMapProvider));
							this._siteMapProviders = siteMapProviderCollection;
						}
					}
				}
				return this._siteMapProviders;
			}
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0008A638 File Offset: 0x00089638
		internal void ValidateDefaultProvider()
		{
			if (!string.IsNullOrEmpty(this.DefaultProvider) && this.Providers[this.DefaultProvider] == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_provider_must_exist", new object[] { this.DefaultProvider }), base.ElementInformation.Properties[SiteMapSection._propDefaultProvider.Name].Source, base.ElementInformation.Properties[SiteMapSection._propDefaultProvider.Name].LineNumber);
			}
		}

		// Token: 0x04001A37 RID: 6711
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A38 RID: 6712
		private static readonly ConfigurationProperty _propDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), "AspNetXmlSiteMapProvider", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001A39 RID: 6713
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001A3A RID: 6714
		private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A3B RID: 6715
		private SiteMapProviderCollection _siteMapProviders;
	}
}

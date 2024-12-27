using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200025C RID: 604
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TrustSection : ConfigurationSection
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x0008C2D8 File Offset: 0x0008B2D8
		static TrustSection()
		{
			TrustSection._properties.Add(TrustSection._propLevel);
			TrustSection._properties.Add(TrustSection._propOriginUrl);
			TrustSection._properties.Add(TrustSection._propProcessRequestInApplicationTrust);
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x0008C388 File Offset: 0x0008B388
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TrustSection._properties;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001FEA RID: 8170 RVA: 0x0008C38F File Offset: 0x0008B38F
		// (set) Token: 0x06001FEB RID: 8171 RVA: 0x0008C3A1 File Offset: 0x0008B3A1
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("level", IsRequired = true, DefaultValue = "Full")]
		public string Level
		{
			get
			{
				return (string)base[TrustSection._propLevel];
			}
			set
			{
				base[TrustSection._propLevel] = value;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001FEC RID: 8172 RVA: 0x0008C3AF File Offset: 0x0008B3AF
		// (set) Token: 0x06001FED RID: 8173 RVA: 0x0008C3C1 File Offset: 0x0008B3C1
		[ConfigurationProperty("originUrl", DefaultValue = "")]
		public string OriginUrl
		{
			get
			{
				return (string)base[TrustSection._propOriginUrl];
			}
			set
			{
				base[TrustSection._propOriginUrl] = value;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001FEE RID: 8174 RVA: 0x0008C3CF File Offset: 0x0008B3CF
		// (set) Token: 0x06001FEF RID: 8175 RVA: 0x0008C3E1 File Offset: 0x0008B3E1
		[ConfigurationProperty("processRequestInApplicationTrust", DefaultValue = true)]
		public bool ProcessRequestInApplicationTrust
		{
			get
			{
				return (bool)base[TrustSection._propProcessRequestInApplicationTrust];
			}
			set
			{
				base[TrustSection._propProcessRequestInApplicationTrust] = value;
			}
		}

		// Token: 0x04001A74 RID: 6772
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A75 RID: 6773
		private static readonly ConfigurationProperty _propLevel = new ConfigurationProperty("level", typeof(string), "Full", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04001A76 RID: 6774
		private static readonly ConfigurationProperty _propOriginUrl = new ConfigurationProperty("originUrl", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001A77 RID: 6775
		private static readonly ConfigurationProperty _propProcessRequestInApplicationTrust = new ConfigurationProperty("processRequestInApplicationTrust", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}

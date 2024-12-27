using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Security;

namespace System.Web.Configuration
{
	// Token: 0x0200019C RID: 412
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AnonymousIdentificationSection : ConfigurationSection
	{
		// Token: 0x0600113D RID: 4413 RVA: 0x0004D5EC File Offset: 0x0004C5EC
		static AnonymousIdentificationSection()
		{
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propEnabled);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookieName);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookieTimeout);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookiePath);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookieRequireSSL);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookieSlidingExpiration);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookieProtection);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propCookieless);
			AnonymousIdentificationSection._properties.Add(AnonymousIdentificationSection._propDomain);
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x0004D7CE File Offset: 0x0004C7CE
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AnonymousIdentificationSection._properties;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0004D7D5 File Offset: 0x0004C7D5
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x0004D7E7 File Offset: 0x0004C7E7
		[ConfigurationProperty("enabled", DefaultValue = false)]
		public bool Enabled
		{
			get
			{
				return (bool)base[AnonymousIdentificationSection._propEnabled];
			}
			set
			{
				base[AnonymousIdentificationSection._propEnabled] = value;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x0004D7FA File Offset: 0x0004C7FA
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x0004D80C File Offset: 0x0004C80C
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("cookieName", DefaultValue = ".ASPXANONYMOUS")]
		public string CookieName
		{
			get
			{
				return (string)base[AnonymousIdentificationSection._propCookieName];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookieName] = value;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x0004D81A File Offset: 0x0004C81A
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x0004D82C File Offset: 0x0004C82C
		[TypeConverter(typeof(TimeSpanMinutesOrInfiniteConverter))]
		[ConfigurationProperty("cookieTimeout", DefaultValue = "69.10:40:00")]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan CookieTimeout
		{
			get
			{
				return (TimeSpan)base[AnonymousIdentificationSection._propCookieTimeout];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookieTimeout] = value;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x0004D83F File Offset: 0x0004C83F
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x0004D851 File Offset: 0x0004C851
		[ConfigurationProperty("cookiePath", DefaultValue = "/")]
		[StringValidator(MinLength = 1)]
		public string CookiePath
		{
			get
			{
				return (string)base[AnonymousIdentificationSection._propCookiePath];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookiePath] = value;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0004D85F File Offset: 0x0004C85F
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x0004D871 File Offset: 0x0004C871
		[ConfigurationProperty("cookieRequireSSL", DefaultValue = false)]
		public bool CookieRequireSSL
		{
			get
			{
				return (bool)base[AnonymousIdentificationSection._propCookieRequireSSL];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookieRequireSSL] = value;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x0004D884 File Offset: 0x0004C884
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x0004D896 File Offset: 0x0004C896
		[ConfigurationProperty("cookieSlidingExpiration", DefaultValue = true)]
		public bool CookieSlidingExpiration
		{
			get
			{
				return (bool)base[AnonymousIdentificationSection._propCookieSlidingExpiration];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookieSlidingExpiration] = value;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x0004D8A9 File Offset: 0x0004C8A9
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x0004D8BB File Offset: 0x0004C8BB
		[ConfigurationProperty("cookieProtection", DefaultValue = CookieProtection.Validation)]
		public CookieProtection CookieProtection
		{
			get
			{
				return (CookieProtection)base[AnonymousIdentificationSection._propCookieProtection];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookieProtection] = value;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x0004D8CE File Offset: 0x0004C8CE
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x0004D8E0 File Offset: 0x0004C8E0
		[ConfigurationProperty("cookieless", DefaultValue = HttpCookieMode.UseCookies)]
		public HttpCookieMode Cookieless
		{
			get
			{
				return (HttpCookieMode)base[AnonymousIdentificationSection._propCookieless];
			}
			set
			{
				base[AnonymousIdentificationSection._propCookieless] = value;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x0004D8F3 File Offset: 0x0004C8F3
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x0004D905 File Offset: 0x0004C905
		[ConfigurationProperty("domain")]
		public string Domain
		{
			get
			{
				return (string)base[AnonymousIdentificationSection._propDomain];
			}
			set
			{
				base[AnonymousIdentificationSection._propDomain] = value;
			}
		}

		// Token: 0x04001699 RID: 5785
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400169A RID: 5786
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400169B RID: 5787
		private static readonly ConfigurationProperty _propCookieName = new ConfigurationProperty("cookieName", typeof(string), ".ASPXANONYMOUS", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400169C RID: 5788
		private static readonly ConfigurationProperty _propCookieTimeout = new ConfigurationProperty("cookieTimeout", typeof(TimeSpan), TimeSpan.FromMinutes(100000.0), StdValidatorsAndConverters.TimeSpanMinutesOrInfiniteConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400169D RID: 5789
		private static readonly ConfigurationProperty _propCookiePath = new ConfigurationProperty("cookiePath", typeof(string), "/", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400169E RID: 5790
		private static readonly ConfigurationProperty _propCookieRequireSSL = new ConfigurationProperty("cookieRequireSSL", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400169F RID: 5791
		private static readonly ConfigurationProperty _propCookieSlidingExpiration = new ConfigurationProperty("cookieSlidingExpiration", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040016A0 RID: 5792
		private static readonly ConfigurationProperty _propCookieProtection = new ConfigurationProperty("cookieProtection", typeof(CookieProtection), CookieProtection.Validation, ConfigurationPropertyOptions.None);

		// Token: 0x040016A1 RID: 5793
		private static readonly ConfigurationProperty _propCookieless = new ConfigurationProperty("cookieless", typeof(HttpCookieMode), HttpCookieMode.UseCookies, ConfigurationPropertyOptions.None);

		// Token: 0x040016A2 RID: 5794
		private static readonly ConfigurationProperty _propDomain = new ConfigurationProperty("domain", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}

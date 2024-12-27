using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001DC RID: 476
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthenticationConfiguration : ConfigurationElement
	{
		// Token: 0x06001A8C RID: 6796 RVA: 0x0007B964 File Offset: 0x0007A964
		static FormsAuthenticationConfiguration()
		{
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propCredentials);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propName);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propLoginUrl);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propDefaultUrl);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propProtection);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propTimeout);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propPath);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propRequireSSL);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propSlidingExpiration);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propCookieless);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propDomain);
			FormsAuthenticationConfiguration._properties.Add(FormsAuthenticationConfiguration._propEnableCrossAppRedirects);
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0007BC10 File Offset: 0x0007AC10
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return FormsAuthenticationConfiguration._properties;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x0007BC17 File Offset: 0x0007AC17
		[ConfigurationProperty("credentials")]
		public FormsAuthenticationCredentials Credentials
		{
			get
			{
				return (FormsAuthenticationCredentials)base[FormsAuthenticationConfiguration._propCredentials];
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A90 RID: 6800 RVA: 0x0007BC29 File Offset: 0x0007AC29
		// (set) Token: 0x06001A91 RID: 6801 RVA: 0x0007BC3B File Offset: 0x0007AC3B
		[ConfigurationProperty("name", DefaultValue = ".ASPXAUTH")]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[FormsAuthenticationConfiguration._propName];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base[FormsAuthenticationConfiguration._propName] = FormsAuthenticationConfiguration._propName.DefaultValue;
					return;
				}
				base[FormsAuthenticationConfiguration._propName] = value;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x0007BC67 File Offset: 0x0007AC67
		// (set) Token: 0x06001A93 RID: 6803 RVA: 0x0007BC79 File Offset: 0x0007AC79
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("loginUrl", DefaultValue = "login.aspx")]
		public string LoginUrl
		{
			get
			{
				return (string)base[FormsAuthenticationConfiguration._propLoginUrl];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base[FormsAuthenticationConfiguration._propLoginUrl] = FormsAuthenticationConfiguration._propLoginUrl.DefaultValue;
					return;
				}
				base[FormsAuthenticationConfiguration._propLoginUrl] = value;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A94 RID: 6804 RVA: 0x0007BCA5 File Offset: 0x0007ACA5
		// (set) Token: 0x06001A95 RID: 6805 RVA: 0x0007BCB7 File Offset: 0x0007ACB7
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("defaultUrl", DefaultValue = "default.aspx")]
		public string DefaultUrl
		{
			get
			{
				return (string)base[FormsAuthenticationConfiguration._propDefaultUrl];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base[FormsAuthenticationConfiguration._propDefaultUrl] = FormsAuthenticationConfiguration._propDefaultUrl.DefaultValue;
					return;
				}
				base[FormsAuthenticationConfiguration._propDefaultUrl] = value;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A96 RID: 6806 RVA: 0x0007BCE3 File Offset: 0x0007ACE3
		// (set) Token: 0x06001A97 RID: 6807 RVA: 0x0007BCF5 File Offset: 0x0007ACF5
		[ConfigurationProperty("protection", DefaultValue = FormsProtectionEnum.All)]
		public FormsProtectionEnum Protection
		{
			get
			{
				return (FormsProtectionEnum)base[FormsAuthenticationConfiguration._propProtection];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propProtection] = value;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A98 RID: 6808 RVA: 0x0007BD08 File Offset: 0x0007AD08
		// (set) Token: 0x06001A99 RID: 6809 RVA: 0x0007BD1A File Offset: 0x0007AD1A
		[TimeSpanValidator(MinValueString = "00:01:00", MaxValueString = "10675199.02:48:05.4775807")]
		[ConfigurationProperty("timeout", DefaultValue = "00:30:00")]
		[TypeConverter(typeof(TimeSpanMinutesConverter))]
		public TimeSpan Timeout
		{
			get
			{
				return (TimeSpan)base[FormsAuthenticationConfiguration._propTimeout];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propTimeout] = value;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A9A RID: 6810 RVA: 0x0007BD2D File Offset: 0x0007AD2D
		// (set) Token: 0x06001A9B RID: 6811 RVA: 0x0007BD3F File Offset: 0x0007AD3F
		[ConfigurationProperty("path", DefaultValue = "/")]
		[StringValidator(MinLength = 1)]
		public string Path
		{
			get
			{
				return (string)base[FormsAuthenticationConfiguration._propPath];
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base[FormsAuthenticationConfiguration._propPath] = FormsAuthenticationConfiguration._propPath.DefaultValue;
					return;
				}
				base[FormsAuthenticationConfiguration._propPath] = value;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x0007BD6B File Offset: 0x0007AD6B
		// (set) Token: 0x06001A9D RID: 6813 RVA: 0x0007BD7D File Offset: 0x0007AD7D
		[ConfigurationProperty("requireSSL", DefaultValue = false)]
		public bool RequireSSL
		{
			get
			{
				return (bool)base[FormsAuthenticationConfiguration._propRequireSSL];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propRequireSSL] = value;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A9E RID: 6814 RVA: 0x0007BD90 File Offset: 0x0007AD90
		// (set) Token: 0x06001A9F RID: 6815 RVA: 0x0007BDA2 File Offset: 0x0007ADA2
		[ConfigurationProperty("slidingExpiration", DefaultValue = true)]
		public bool SlidingExpiration
		{
			get
			{
				return (bool)base[FormsAuthenticationConfiguration._propSlidingExpiration];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propSlidingExpiration] = value;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x0007BDB5 File Offset: 0x0007ADB5
		// (set) Token: 0x06001AA1 RID: 6817 RVA: 0x0007BDC7 File Offset: 0x0007ADC7
		[ConfigurationProperty("enableCrossAppRedirects", DefaultValue = false)]
		public bool EnableCrossAppRedirects
		{
			get
			{
				return (bool)base[FormsAuthenticationConfiguration._propEnableCrossAppRedirects];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propEnableCrossAppRedirects] = value;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001AA2 RID: 6818 RVA: 0x0007BDDA File Offset: 0x0007ADDA
		// (set) Token: 0x06001AA3 RID: 6819 RVA: 0x0007BDEC File Offset: 0x0007ADEC
		[ConfigurationProperty("cookieless", DefaultValue = HttpCookieMode.UseDeviceProfile)]
		public HttpCookieMode Cookieless
		{
			get
			{
				return (HttpCookieMode)base[FormsAuthenticationConfiguration._propCookieless];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propCookieless] = value;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001AA4 RID: 6820 RVA: 0x0007BDFF File Offset: 0x0007ADFF
		// (set) Token: 0x06001AA5 RID: 6821 RVA: 0x0007BE11 File Offset: 0x0007AE11
		[ConfigurationProperty("domain", DefaultValue = "")]
		public string Domain
		{
			get
			{
				return (string)base[FormsAuthenticationConfiguration._propDomain];
			}
			set
			{
				base[FormsAuthenticationConfiguration._propDomain] = value;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x0007BE1F File Offset: 0x0007AE1F
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return FormsAuthenticationConfiguration.s_elemProperty;
			}
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0007BE28 File Offset: 0x0007AE28
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("forms");
			}
			FormsAuthenticationConfiguration formsAuthenticationConfiguration = (FormsAuthenticationConfiguration)value;
			if (StringUtil.StringStartsWith(formsAuthenticationConfiguration.LoginUrl, "\\\\") || (formsAuthenticationConfiguration.LoginUrl.Length > 1 && formsAuthenticationConfiguration.LoginUrl[1] == ':'))
			{
				throw new ConfigurationErrorsException(SR.GetString("Auth_bad_url"), formsAuthenticationConfiguration.ElementInformation.Properties["loginUrl"].Source, formsAuthenticationConfiguration.ElementInformation.Properties["loginUrl"].LineNumber);
			}
			if (StringUtil.StringStartsWith(formsAuthenticationConfiguration.DefaultUrl, "\\\\") || (formsAuthenticationConfiguration.DefaultUrl.Length > 1 && formsAuthenticationConfiguration.DefaultUrl[1] == ':'))
			{
				throw new ConfigurationErrorsException(SR.GetString("Auth_bad_url"), formsAuthenticationConfiguration.ElementInformation.Properties["defaultUrl"].Source, formsAuthenticationConfiguration.ElementInformation.Properties["defaultUrl"].LineNumber);
			}
		}

		// Token: 0x040017E2 RID: 6114
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(FormsAuthenticationConfiguration), new ValidatorCallback(FormsAuthenticationConfiguration.Validate)));

		// Token: 0x040017E3 RID: 6115
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017E4 RID: 6116
		private static readonly ConfigurationProperty _propCredentials = new ConfigurationProperty("credentials", typeof(FormsAuthenticationCredentials), null, ConfigurationPropertyOptions.None);

		// Token: 0x040017E5 RID: 6117
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), ".ASPXAUTH", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040017E6 RID: 6118
		private static readonly ConfigurationProperty _propLoginUrl = new ConfigurationProperty("loginUrl", typeof(string), "login.aspx", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040017E7 RID: 6119
		private static readonly ConfigurationProperty _propDefaultUrl = new ConfigurationProperty("defaultUrl", typeof(string), "default.aspx", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040017E8 RID: 6120
		private static readonly ConfigurationProperty _propProtection = new ConfigurationProperty("protection", typeof(FormsProtectionEnum), FormsProtectionEnum.All, ConfigurationPropertyOptions.None);

		// Token: 0x040017E9 RID: 6121
		private static readonly ConfigurationProperty _propTimeout = new ConfigurationProperty("timeout", typeof(TimeSpan), TimeSpan.FromMinutes(30.0), StdValidatorsAndConverters.TimeSpanMinutesConverter, new TimeSpanValidator(TimeSpan.FromMinutes(1.0), TimeSpan.MaxValue), ConfigurationPropertyOptions.None);

		// Token: 0x040017EA RID: 6122
		private static readonly ConfigurationProperty _propPath = new ConfigurationProperty("path", typeof(string), "/", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040017EB RID: 6123
		private static readonly ConfigurationProperty _propRequireSSL = new ConfigurationProperty("requireSSL", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x040017EC RID: 6124
		private static readonly ConfigurationProperty _propSlidingExpiration = new ConfigurationProperty("slidingExpiration", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040017ED RID: 6125
		private static readonly ConfigurationProperty _propCookieless = new ConfigurationProperty("cookieless", typeof(HttpCookieMode), HttpCookieMode.UseDeviceProfile, ConfigurationPropertyOptions.None);

		// Token: 0x040017EE RID: 6126
		private static readonly ConfigurationProperty _propDomain = new ConfigurationProperty("domain", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x040017EF RID: 6127
		private static readonly ConfigurationProperty _propEnableCrossAppRedirects = new ConfigurationProperty("enableCrossAppRedirects", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}

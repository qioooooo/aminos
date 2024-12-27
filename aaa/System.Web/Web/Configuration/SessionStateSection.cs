using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.SessionState;

namespace System.Web.Configuration
{
	// Token: 0x02000249 RID: 585
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SessionStateSection : ConfigurationSection
	{
		// Token: 0x06001EE0 RID: 7904 RVA: 0x00089D78 File Offset: 0x00088D78
		static SessionStateSection()
		{
			SessionStateSection._properties.Add(SessionStateSection._propMode);
			SessionStateSection._properties.Add(SessionStateSection._propStateConnectionString);
			SessionStateSection._properties.Add(SessionStateSection._propStateNetworkTimeout);
			SessionStateSection._properties.Add(SessionStateSection._propSqlConnectionString);
			SessionStateSection._properties.Add(SessionStateSection._propSqlCommandTimeout);
			SessionStateSection._properties.Add(SessionStateSection._propCustomProvider);
			SessionStateSection._properties.Add(SessionStateSection._propCookieless);
			SessionStateSection._properties.Add(SessionStateSection._propCookieName);
			SessionStateSection._properties.Add(SessionStateSection._propTimeout);
			SessionStateSection._properties.Add(SessionStateSection._propAllowCustomSqlDatabase);
			SessionStateSection._properties.Add(SessionStateSection._propProviders);
			SessionStateSection._properties.Add(SessionStateSection._propRegenerateExpiredSessionId);
			SessionStateSection._properties.Add(SessionStateSection._propPartitionResolverType);
			SessionStateSection._properties.Add(SessionStateSection._propUseHostingIdentity);
			SessionStateSection._properties.Add(SessionStateSection._propSessionIDManagerType);
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x0008A0D2 File Offset: 0x000890D2
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SessionStateSection._properties;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x0008A0D9 File Offset: 0x000890D9
		// (set) Token: 0x06001EE4 RID: 7908 RVA: 0x0008A0EB File Offset: 0x000890EB
		[ConfigurationProperty("mode", DefaultValue = SessionStateMode.InProc)]
		public SessionStateMode Mode
		{
			get
			{
				return (SessionStateMode)base[SessionStateSection._propMode];
			}
			set
			{
				base[SessionStateSection._propMode] = value;
			}
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x0008A0FE File Offset: 0x000890FE
		// (set) Token: 0x06001EE6 RID: 7910 RVA: 0x0008A110 File Offset: 0x00089110
		[ConfigurationProperty("stateConnectionString", DefaultValue = "tcpip=loopback:42424")]
		public string StateConnectionString
		{
			get
			{
				return (string)base[SessionStateSection._propStateConnectionString];
			}
			set
			{
				base[SessionStateSection._propStateConnectionString] = value;
			}
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x0008A11E File Offset: 0x0008911E
		// (set) Token: 0x06001EE8 RID: 7912 RVA: 0x0008A130 File Offset: 0x00089130
		[ConfigurationProperty("stateNetworkTimeout", DefaultValue = "00:00:10")]
		[TypeConverter(typeof(TimeSpanSecondsOrInfiniteConverter))]
		public TimeSpan StateNetworkTimeout
		{
			get
			{
				return (TimeSpan)base[SessionStateSection._propStateNetworkTimeout];
			}
			set
			{
				base[SessionStateSection._propStateNetworkTimeout] = value;
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x0008A143 File Offset: 0x00089143
		// (set) Token: 0x06001EEA RID: 7914 RVA: 0x0008A155 File Offset: 0x00089155
		[ConfigurationProperty("sqlConnectionString", DefaultValue = "data source=localhost;Integrated Security=SSPI")]
		public string SqlConnectionString
		{
			get
			{
				return (string)base[SessionStateSection._propSqlConnectionString];
			}
			set
			{
				base[SessionStateSection._propSqlConnectionString] = value;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x0008A163 File Offset: 0x00089163
		// (set) Token: 0x06001EEC RID: 7916 RVA: 0x0008A175 File Offset: 0x00089175
		[ConfigurationProperty("sqlCommandTimeout", DefaultValue = "00:00:30")]
		[TypeConverter(typeof(TimeSpanSecondsOrInfiniteConverter))]
		public TimeSpan SqlCommandTimeout
		{
			get
			{
				return (TimeSpan)base[SessionStateSection._propSqlCommandTimeout];
			}
			set
			{
				base[SessionStateSection._propSqlCommandTimeout] = value;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x0008A188 File Offset: 0x00089188
		// (set) Token: 0x06001EEE RID: 7918 RVA: 0x0008A19A File Offset: 0x0008919A
		[ConfigurationProperty("customProvider", DefaultValue = "")]
		public string CustomProvider
		{
			get
			{
				return (string)base[SessionStateSection._propCustomProvider];
			}
			set
			{
				base[SessionStateSection._propCustomProvider] = value;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001EEF RID: 7919 RVA: 0x0008A1A8 File Offset: 0x000891A8
		// (set) Token: 0x06001EF0 RID: 7920 RVA: 0x0008A1DB File Offset: 0x000891DB
		[ConfigurationProperty("cookieless")]
		public HttpCookieMode Cookieless
		{
			get
			{
				if (!this.cookielessCached)
				{
					this.cookielessCache = this.ConvertToCookieMode((string)base[SessionStateSection._propCookieless]);
					this.cookielessCached = true;
				}
				return this.cookielessCache;
			}
			set
			{
				base[SessionStateSection._propCookieless] = value.ToString();
				this.cookielessCache = value;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x0008A1FA File Offset: 0x000891FA
		// (set) Token: 0x06001EF2 RID: 7922 RVA: 0x0008A20C File Offset: 0x0008920C
		[ConfigurationProperty("cookieName", DefaultValue = "ASP.NET_SessionId")]
		public string CookieName
		{
			get
			{
				return (string)base[SessionStateSection._propCookieName];
			}
			set
			{
				base[SessionStateSection._propCookieName] = value;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x0008A21A File Offset: 0x0008921A
		// (set) Token: 0x06001EF4 RID: 7924 RVA: 0x0008A22C File Offset: 0x0008922C
		[TimeSpanValidator(MinValueString = "00:01:00", MaxValueString = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(TimeSpanMinutesOrInfiniteConverter))]
		[ConfigurationProperty("timeout", DefaultValue = "00:20:00")]
		public TimeSpan Timeout
		{
			get
			{
				return (TimeSpan)base[SessionStateSection._propTimeout];
			}
			set
			{
				base[SessionStateSection._propTimeout] = value;
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x0008A23F File Offset: 0x0008923F
		// (set) Token: 0x06001EF6 RID: 7926 RVA: 0x0008A251 File Offset: 0x00089251
		[ConfigurationProperty("allowCustomSqlDatabase", DefaultValue = false)]
		public bool AllowCustomSqlDatabase
		{
			get
			{
				return (bool)base[SessionStateSection._propAllowCustomSqlDatabase];
			}
			set
			{
				base[SessionStateSection._propAllowCustomSqlDatabase] = value;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x0008A264 File Offset: 0x00089264
		// (set) Token: 0x06001EF8 RID: 7928 RVA: 0x0008A291 File Offset: 0x00089291
		[ConfigurationProperty("regenerateExpiredSessionId", DefaultValue = true)]
		public bool RegenerateExpiredSessionId
		{
			get
			{
				if (!this.regenerateExpiredSessionIdCached)
				{
					this.regenerateExpiredSessionIdCache = (bool)base[SessionStateSection._propRegenerateExpiredSessionId];
					this.regenerateExpiredSessionIdCached = true;
				}
				return this.regenerateExpiredSessionIdCache;
			}
			set
			{
				base[SessionStateSection._propRegenerateExpiredSessionId] = value;
				this.regenerateExpiredSessionIdCache = value;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x0008A2AB File Offset: 0x000892AB
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base[SessionStateSection._propProviders];
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x0008A2BD File Offset: 0x000892BD
		// (set) Token: 0x06001EFB RID: 7931 RVA: 0x0008A2CF File Offset: 0x000892CF
		[ConfigurationProperty("partitionResolverType", DefaultValue = "")]
		public string PartitionResolverType
		{
			get
			{
				return (string)base[SessionStateSection._propPartitionResolverType];
			}
			set
			{
				base[SessionStateSection._propPartitionResolverType] = value;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x0008A2DD File Offset: 0x000892DD
		// (set) Token: 0x06001EFD RID: 7933 RVA: 0x0008A2EF File Offset: 0x000892EF
		[ConfigurationProperty("useHostingIdentity", DefaultValue = true)]
		public bool UseHostingIdentity
		{
			get
			{
				return (bool)base[SessionStateSection._propUseHostingIdentity];
			}
			set
			{
				base[SessionStateSection._propUseHostingIdentity] = value;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x0008A302 File Offset: 0x00089302
		// (set) Token: 0x06001EFF RID: 7935 RVA: 0x0008A314 File Offset: 0x00089314
		[ConfigurationProperty("sessionIDManagerType", DefaultValue = "")]
		public string SessionIDManagerType
		{
			get
			{
				return (string)base[SessionStateSection._propSessionIDManagerType];
			}
			set
			{
				base[SessionStateSection._propSessionIDManagerType] = value;
			}
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0008A324 File Offset: 0x00089324
		private HttpCookieMode ConvertToCookieMode(string s)
		{
			if (s == "true")
			{
				return HttpCookieMode.UseUri;
			}
			if (s == "false")
			{
				return HttpCookieMode.UseCookies;
			}
			Type typeFromHandle = typeof(HttpCookieMode);
			if (Enum.IsDefined(typeFromHandle, s))
			{
				return (HttpCookieMode)((int)Enum.Parse(typeFromHandle, s));
			}
			string text = "true, false";
			foreach (string text2 in Enum.GetNames(typeFromHandle))
			{
				if (text == null)
				{
					text = text2;
				}
				else
				{
					text = text + ", " + text2;
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("Invalid_enum_attribute", new object[] { "cookieless", text }), base.ElementInformation.Properties["cookieless"].Source, base.ElementInformation.Properties["cookieless"].LineNumber);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0008A40F File Offset: 0x0008940F
		protected override void PostDeserialize()
		{
			this.ConvertToCookieMode((string)base[SessionStateSection._propCookieless]);
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0008A428 File Offset: 0x00089428
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return SessionStateSection.s_elemProperty;
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0008A430 File Offset: 0x00089430
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("sessionState");
			}
			SessionStateSection sessionStateSection = (SessionStateSection)value;
			if (sessionStateSection.Timeout.TotalMinutes > 525600.0 && (sessionStateSection.Mode == SessionStateMode.InProc || sessionStateSection.Mode == SessionStateMode.StateServer))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_cache_based_session_timeout"), sessionStateSection.ElementInformation.Properties["timeout"].Source, sessionStateSection.ElementInformation.Properties["timeout"].LineNumber);
			}
		}

		// Token: 0x04001A22 RID: 6690
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(SessionStateSection), new ValidatorCallback(SessionStateSection.Validate)));

		// Token: 0x04001A23 RID: 6691
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A24 RID: 6692
		private static readonly ConfigurationProperty _propMode = new ConfigurationProperty("mode", typeof(SessionStateMode), SessionStateMode.InProc, ConfigurationPropertyOptions.None);

		// Token: 0x04001A25 RID: 6693
		private static readonly ConfigurationProperty _propStateConnectionString = new ConfigurationProperty("stateConnectionString", typeof(string), "tcpip=loopback:42424", ConfigurationPropertyOptions.None);

		// Token: 0x04001A26 RID: 6694
		private static readonly ConfigurationProperty _propStateNetworkTimeout = new ConfigurationProperty("stateNetworkTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(10.0), StdValidatorsAndConverters.TimeSpanSecondsOrInfiniteConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001A27 RID: 6695
		private static readonly ConfigurationProperty _propSqlConnectionString = new ConfigurationProperty("sqlConnectionString", typeof(string), "data source=localhost;Integrated Security=SSPI", ConfigurationPropertyOptions.None);

		// Token: 0x04001A28 RID: 6696
		private static readonly ConfigurationProperty _propSqlCommandTimeout = new ConfigurationProperty("sqlCommandTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(30.0), StdValidatorsAndConverters.TimeSpanSecondsOrInfiniteConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A29 RID: 6697
		private static readonly ConfigurationProperty _propCustomProvider = new ConfigurationProperty("customProvider", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001A2A RID: 6698
		private static readonly ConfigurationProperty _propCookieless = new ConfigurationProperty("cookieless", typeof(string), HttpCookieMode.UseCookies.ToString(), ConfigurationPropertyOptions.None);

		// Token: 0x04001A2B RID: 6699
		private static readonly ConfigurationProperty _propCookieName = new ConfigurationProperty("cookieName", typeof(string), "ASP.NET_SessionId", ConfigurationPropertyOptions.None);

		// Token: 0x04001A2C RID: 6700
		private static readonly ConfigurationProperty _propTimeout = new ConfigurationProperty("timeout", typeof(TimeSpan), TimeSpan.FromMinutes(20.0), StdValidatorsAndConverters.TimeSpanMinutesOrInfiniteConverter, new TimeSpanValidator(TimeSpan.FromMinutes(1.0), TimeSpan.MaxValue), ConfigurationPropertyOptions.None);

		// Token: 0x04001A2D RID: 6701
		private static readonly ConfigurationProperty _propAllowCustomSqlDatabase = new ConfigurationProperty("allowCustomSqlDatabase", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001A2E RID: 6702
		private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A2F RID: 6703
		private static readonly ConfigurationProperty _propRegenerateExpiredSessionId = new ConfigurationProperty("regenerateExpiredSessionId", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001A30 RID: 6704
		private static readonly ConfigurationProperty _propPartitionResolverType = new ConfigurationProperty("partitionResolverType", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001A31 RID: 6705
		private static readonly ConfigurationProperty _propUseHostingIdentity = new ConfigurationProperty("useHostingIdentity", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001A32 RID: 6706
		private static readonly ConfigurationProperty _propSessionIDManagerType = new ConfigurationProperty("sessionIDManagerType", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001A33 RID: 6707
		private HttpCookieMode cookielessCache = HttpCookieMode.UseCookies;

		// Token: 0x04001A34 RID: 6708
		private bool cookielessCached;

		// Token: 0x04001A35 RID: 6709
		private bool regenerateExpiredSessionIdCache;

		// Token: 0x04001A36 RID: 6710
		private bool regenerateExpiredSessionIdCached;
	}
}

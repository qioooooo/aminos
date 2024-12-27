using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001EB RID: 491
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HealthMonitoringSection : ConfigurationSection
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x0007D228 File Offset: 0x0007C228
		static HealthMonitoringSection()
		{
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propHeartbeatInterval);
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propEnabled);
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propBufferModes);
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propProviders);
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propProfileSettingsCollection);
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propRuleSettingsCollection);
			HealthMonitoringSection._properties.Add(HealthMonitoringSection._propEventMappingSettingsCollection);
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x0007D3A1 File Offset: 0x0007C3A1
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HealthMonitoringSection._properties;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0007D3A8 File Offset: 0x0007C3A8
		// (set) Token: 0x06001B14 RID: 6932 RVA: 0x0007D3BA File Offset: 0x0007C3BA
		[TypeConverter(typeof(TimeSpanSecondsConverter))]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "24.20:31:23")]
		[ConfigurationProperty("heartbeatInterval", DefaultValue = "00:00:00")]
		public TimeSpan HeartbeatInterval
		{
			get
			{
				return (TimeSpan)base[HealthMonitoringSection._propHeartbeatInterval];
			}
			set
			{
				base[HealthMonitoringSection._propHeartbeatInterval] = value;
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0007D3CD File Offset: 0x0007C3CD
		// (set) Token: 0x06001B16 RID: 6934 RVA: 0x0007D3DF File Offset: 0x0007C3DF
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get
			{
				return (bool)base[HealthMonitoringSection._propEnabled];
			}
			set
			{
				base[HealthMonitoringSection._propEnabled] = value;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0007D3F2 File Offset: 0x0007C3F2
		[ConfigurationProperty("bufferModes")]
		public BufferModesCollection BufferModes
		{
			get
			{
				return (BufferModesCollection)base[HealthMonitoringSection._propBufferModes];
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x0007D404 File Offset: 0x0007C404
		[ConfigurationProperty("providers")]
		public ProviderSettingsCollection Providers
		{
			get
			{
				return (ProviderSettingsCollection)base[HealthMonitoringSection._propProviders];
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x0007D416 File Offset: 0x0007C416
		[ConfigurationProperty("profiles")]
		public ProfileSettingsCollection Profiles
		{
			get
			{
				return (ProfileSettingsCollection)base[HealthMonitoringSection._propProfileSettingsCollection];
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0007D428 File Offset: 0x0007C428
		[ConfigurationProperty("rules")]
		public RuleSettingsCollection Rules
		{
			get
			{
				return (RuleSettingsCollection)base[HealthMonitoringSection._propRuleSettingsCollection];
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x0007D43A File Offset: 0x0007C43A
		[ConfigurationProperty("eventMappings")]
		public EventMappingSettingsCollection EventMappings
		{
			get
			{
				return (EventMappingSettingsCollection)base[HealthMonitoringSection._propEventMappingSettingsCollection];
			}
		}

		// Token: 0x04001819 RID: 6169
		private const int MAX_HEARTBEAT_VALUE = 2147483;

		// Token: 0x0400181A RID: 6170
		private const bool DEFAULT_HEALTH_MONITORING_ENABLED = true;

		// Token: 0x0400181B RID: 6171
		private const int DEFAULT_HEARTBEATINTERVAL = 0;

		// Token: 0x0400181C RID: 6172
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400181D RID: 6173
		private static readonly ConfigurationProperty _propHeartbeatInterval = new ConfigurationProperty("heartbeatInterval", typeof(TimeSpan), TimeSpan.FromSeconds(0.0), StdValidatorsAndConverters.TimeSpanSecondsConverter, new TimeSpanValidator(TimeSpan.Zero, TimeSpan.FromSeconds(2147483.0)), ConfigurationPropertyOptions.None);

		// Token: 0x0400181E RID: 6174
		private static readonly ConfigurationProperty _propEnabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x0400181F RID: 6175
		private static readonly ConfigurationProperty _propBufferModes = new ConfigurationProperty("bufferModes", typeof(BufferModesCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001820 RID: 6176
		private static readonly ConfigurationProperty _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001821 RID: 6177
		private static readonly ConfigurationProperty _propProfileSettingsCollection = new ConfigurationProperty("profiles", typeof(ProfileSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001822 RID: 6178
		private static readonly ConfigurationProperty _propRuleSettingsCollection = new ConfigurationProperty("rules", typeof(RuleSettingsCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04001823 RID: 6179
		private static readonly ConfigurationProperty _propEventMappingSettingsCollection = new ConfigurationProperty("eventMappings", typeof(EventMappingSettingsCollection), null, ConfigurationPropertyOptions.None);
	}
}

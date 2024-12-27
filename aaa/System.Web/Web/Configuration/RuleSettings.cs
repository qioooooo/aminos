using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000242 RID: 578
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RuleSettings : ConfigurationElement
	{
		// Token: 0x06001EAC RID: 7852 RVA: 0x00089740 File Offset: 0x00088740
		static RuleSettings()
		{
			RuleSettings._properties.Add(RuleSettings._propName);
			RuleSettings._properties.Add(RuleSettings._propEventName);
			RuleSettings._properties.Add(RuleSettings._propProvider);
			RuleSettings._properties.Add(RuleSettings._propProfile);
			RuleSettings._properties.Add(RuleSettings._propMinInstances);
			RuleSettings._properties.Add(RuleSettings._propMaxLimit);
			RuleSettings._properties.Add(RuleSettings._propMinInterval);
			RuleSettings._properties.Add(RuleSettings._propCustom);
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x0008990E File Offset: 0x0008890E
		internal RuleSettings()
		{
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x00089916 File Offset: 0x00088916
		public RuleSettings(string name, string eventName, string provider)
			: this()
		{
			this.Name = name;
			this.EventName = eventName;
			this.Provider = provider;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x00089933 File Offset: 0x00088933
		public RuleSettings(string name, string eventName, string provider, string profile, int minInstances, int maxLimit, TimeSpan minInterval)
			: this(name, eventName, provider)
		{
			this.Profile = profile;
			this.MinInstances = minInstances;
			this.MaxLimit = maxLimit;
			this.MinInterval = minInterval;
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x0008995E File Offset: 0x0008895E
		public RuleSettings(string name, string eventName, string provider, string profile, int minInstances, int maxLimit, TimeSpan minInterval, string custom)
			: this(name, eventName, provider)
		{
			this.Profile = profile;
			this.MinInstances = minInstances;
			this.MaxLimit = maxLimit;
			this.MinInterval = minInterval;
			this.Custom = custom;
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x00089991 File Offset: 0x00088991
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return RuleSettings._properties;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001EB2 RID: 7858 RVA: 0x00089998 File Offset: 0x00088998
		// (set) Token: 0x06001EB3 RID: 7859 RVA: 0x000899AA File Offset: 0x000889AA
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[RuleSettings._propName];
			}
			set
			{
				base[RuleSettings._propName] = value;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x000899B8 File Offset: 0x000889B8
		// (set) Token: 0x06001EB5 RID: 7861 RVA: 0x000899CA File Offset: 0x000889CA
		[ConfigurationProperty("eventName", IsRequired = true, DefaultValue = "")]
		public string EventName
		{
			get
			{
				return (string)base[RuleSettings._propEventName];
			}
			set
			{
				base[RuleSettings._propEventName] = value;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000899D8 File Offset: 0x000889D8
		// (set) Token: 0x06001EB7 RID: 7863 RVA: 0x000899EA File Offset: 0x000889EA
		[ConfigurationProperty("custom", DefaultValue = "")]
		public string Custom
		{
			get
			{
				return (string)base[RuleSettings._propCustom];
			}
			set
			{
				base[RuleSettings._propCustom] = value;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x000899F8 File Offset: 0x000889F8
		// (set) Token: 0x06001EB9 RID: 7865 RVA: 0x00089A0A File Offset: 0x00088A0A
		[ConfigurationProperty("profile", DefaultValue = "")]
		public string Profile
		{
			get
			{
				return (string)base[RuleSettings._propProfile];
			}
			set
			{
				base[RuleSettings._propProfile] = value;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x00089A18 File Offset: 0x00088A18
		// (set) Token: 0x06001EBB RID: 7867 RVA: 0x00089A2A File Offset: 0x00088A2A
		[ConfigurationProperty("provider", DefaultValue = "")]
		public string Provider
		{
			get
			{
				return (string)base[RuleSettings._propProvider];
			}
			set
			{
				base[RuleSettings._propProvider] = value;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x00089A38 File Offset: 0x00088A38
		// (set) Token: 0x06001EBD RID: 7869 RVA: 0x00089A4A File Offset: 0x00088A4A
		[ConfigurationProperty("minInstances", DefaultValue = 1)]
		[IntegerValidator(MinValue = 1)]
		public int MinInstances
		{
			get
			{
				return (int)base[RuleSettings._propMinInstances];
			}
			set
			{
				base[RuleSettings._propMinInstances] = value;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x00089A5D File Offset: 0x00088A5D
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x00089A6F File Offset: 0x00088A6F
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("maxLimit", DefaultValue = 2147483647)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		public int MaxLimit
		{
			get
			{
				return (int)base[RuleSettings._propMaxLimit];
			}
			set
			{
				base[RuleSettings._propMaxLimit] = value;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x00089A82 File Offset: 0x00088A82
		// (set) Token: 0x06001EC1 RID: 7873 RVA: 0x00089A94 File Offset: 0x00088A94
		[ConfigurationProperty("minInterval", DefaultValue = "00:00:00")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		public TimeSpan MinInterval
		{
			get
			{
				return (TimeSpan)base[RuleSettings._propMinInterval];
			}
			set
			{
				base[RuleSettings._propMinInterval] = value;
			}
		}

		// Token: 0x04001A08 RID: 6664
		internal static int DEFAULT_MIN_INSTANCES = 1;

		// Token: 0x04001A09 RID: 6665
		internal static int DEFAULT_MAX_LIMIT = int.MaxValue;

		// Token: 0x04001A0A RID: 6666
		internal static TimeSpan DEFAULT_MIN_INTERVAL = TimeSpan.Zero;

		// Token: 0x04001A0B RID: 6667
		internal static string DEFAULT_CUSTOM_EVAL = null;

		// Token: 0x04001A0C RID: 6668
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A0D RID: 6669
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001A0E RID: 6670
		private static readonly ConfigurationProperty _propEventName = new ConfigurationProperty("eventName", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04001A0F RID: 6671
		private static readonly ConfigurationProperty _propProvider = new ConfigurationProperty("provider", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001A10 RID: 6672
		private static readonly ConfigurationProperty _propProfile = new ConfigurationProperty("profile", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04001A11 RID: 6673
		private static readonly ConfigurationProperty _propMinInstances = new ConfigurationProperty("minInstances", typeof(int), RuleSettings.DEFAULT_MIN_INSTANCES, null, StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001A12 RID: 6674
		private static readonly ConfigurationProperty _propMaxLimit = new ConfigurationProperty("maxLimit", typeof(int), RuleSettings.DEFAULT_MAX_LIMIT, new InfiniteIntConverter(), StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001A13 RID: 6675
		private static readonly ConfigurationProperty _propMinInterval = new ConfigurationProperty("minInterval", typeof(TimeSpan), RuleSettings.DEFAULT_MIN_INTERVAL, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001A14 RID: 6676
		private static readonly ConfigurationProperty _propCustom = new ConfigurationProperty("custom", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}

using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200022D RID: 557
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileSettings : ConfigurationElement
	{
		// Token: 0x06001DFA RID: 7674 RVA: 0x00086E34 File Offset: 0x00085E34
		static ProfileSettings()
		{
			ProfileSettings._properties.Add(ProfileSettings._propName);
			ProfileSettings._properties.Add(ProfileSettings._propMinInstances);
			ProfileSettings._properties.Add(ProfileSettings._propMaxLimit);
			ProfileSettings._properties.Add(ProfileSettings._propMinInterval);
			ProfileSettings._properties.Add(ProfileSettings._propCustom);
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x00086F58 File Offset: 0x00085F58
		internal ProfileSettings()
		{
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x00086F60 File Offset: 0x00085F60
		public ProfileSettings(string name)
			: this()
		{
			this.Name = name;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x00086F6F File Offset: 0x00085F6F
		public ProfileSettings(string name, int minInstances, int maxLimit, TimeSpan minInterval)
			: this(name)
		{
			this.MinInstances = minInstances;
			this.MaxLimit = maxLimit;
			this.MinInterval = minInterval;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x00086F8E File Offset: 0x00085F8E
		public ProfileSettings(string name, int minInstances, int maxLimit, TimeSpan minInterval, string custom)
			: this(name)
		{
			this.MinInstances = minInstances;
			this.MaxLimit = maxLimit;
			this.MinInterval = minInterval;
			this.Custom = custom;
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06001DFF RID: 7679 RVA: 0x00086FB5 File Offset: 0x00085FB5
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProfileSettings._properties;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001E00 RID: 7680 RVA: 0x00086FBC File Offset: 0x00085FBC
		// (set) Token: 0x06001E01 RID: 7681 RVA: 0x00086FCE File Offset: 0x00085FCE
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[ProfileSettings._propName];
			}
			set
			{
				base[ProfileSettings._propName] = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x00086FDC File Offset: 0x00085FDC
		// (set) Token: 0x06001E03 RID: 7683 RVA: 0x00086FEE File Offset: 0x00085FEE
		[ConfigurationProperty("minInstances", DefaultValue = 1)]
		[IntegerValidator(MinValue = 1)]
		public int MinInstances
		{
			get
			{
				return (int)base[ProfileSettings._propMinInstances];
			}
			set
			{
				base[ProfileSettings._propMinInstances] = value;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x00087001 File Offset: 0x00086001
		// (set) Token: 0x06001E05 RID: 7685 RVA: 0x00087013 File Offset: 0x00086013
		[TypeConverter(typeof(InfiniteIntConverter))]
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("maxLimit", DefaultValue = 2147483647)]
		public int MaxLimit
		{
			get
			{
				return (int)base[ProfileSettings._propMaxLimit];
			}
			set
			{
				base[ProfileSettings._propMaxLimit] = value;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x00087026 File Offset: 0x00086026
		// (set) Token: 0x06001E07 RID: 7687 RVA: 0x00087038 File Offset: 0x00086038
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[ConfigurationProperty("minInterval", DefaultValue = "00:00:00")]
		public TimeSpan MinInterval
		{
			get
			{
				return (TimeSpan)base[ProfileSettings._propMinInterval];
			}
			set
			{
				base[ProfileSettings._propMinInterval] = value;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0008704B File Offset: 0x0008604B
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x0008705D File Offset: 0x0008605D
		[ConfigurationProperty("custom", DefaultValue = "")]
		public string Custom
		{
			get
			{
				return (string)base[ProfileSettings._propCustom];
			}
			set
			{
				base[ProfileSettings._propCustom] = value;
			}
		}

		// Token: 0x0400199C RID: 6556
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400199D RID: 6557
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x0400199E RID: 6558
		private static readonly ConfigurationProperty _propMinInstances = new ConfigurationProperty("minInstances", typeof(int), RuleSettings.DEFAULT_MIN_INSTANCES, null, StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400199F RID: 6559
		private static readonly ConfigurationProperty _propMaxLimit = new ConfigurationProperty("maxLimit", typeof(int), RuleSettings.DEFAULT_MAX_LIMIT, new InfiniteIntConverter(), StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040019A0 RID: 6560
		private static readonly ConfigurationProperty _propMinInterval = new ConfigurationProperty("minInterval", typeof(TimeSpan), RuleSettings.DEFAULT_MIN_INTERVAL, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x040019A1 RID: 6561
		private static readonly ConfigurationProperty _propCustom = new ConfigurationProperty("custom", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
	}
}

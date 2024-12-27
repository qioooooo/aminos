using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001AE RID: 430
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BufferModeSettings : ConfigurationElement
	{
		// Token: 0x060018E8 RID: 6376 RVA: 0x00077878 File Offset: 0x00076878
		static BufferModeSettings()
		{
			BufferModeSettings._properties.Add(BufferModeSettings._propName);
			BufferModeSettings._properties.Add(BufferModeSettings._propMaxBufferSize);
			BufferModeSettings._properties.Add(BufferModeSettings._propMaxFlushSize);
			BufferModeSettings._properties.Add(BufferModeSettings._propUrgentFlushThreshold);
			BufferModeSettings._properties.Add(BufferModeSettings._propRegularFlushInterval);
			BufferModeSettings._properties.Add(BufferModeSettings._propUrgentFlushInterval);
			BufferModeSettings._properties.Add(BufferModeSettings._propMaxBufferThreads);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x00077A53 File Offset: 0x00076A53
		internal BufferModeSettings()
		{
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x00077A5B File Offset: 0x00076A5B
		public BufferModeSettings(string name, int maxBufferSize, int maxFlushSize, int urgentFlushThreshold, TimeSpan regularFlushInterval, TimeSpan urgentFlushInterval, int maxBufferThreads)
			: this()
		{
			this.Name = name;
			this.MaxBufferSize = maxBufferSize;
			this.MaxFlushSize = maxFlushSize;
			this.UrgentFlushThreshold = urgentFlushThreshold;
			this.RegularFlushInterval = regularFlushInterval;
			this.UrgentFlushInterval = urgentFlushInterval;
			this.MaxBufferThreads = maxBufferThreads;
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060018EB RID: 6379 RVA: 0x00077A98 File Offset: 0x00076A98
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return BufferModeSettings._properties;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x00077A9F File Offset: 0x00076A9F
		// (set) Token: 0x060018ED RID: 6381 RVA: 0x00077AB1 File Offset: 0x00076AB1
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[BufferModeSettings._propName];
			}
			set
			{
				base[BufferModeSettings._propName] = value;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x00077ABF File Offset: 0x00076ABF
		// (set) Token: 0x060018EF RID: 6383 RVA: 0x00077AD1 File Offset: 0x00076AD1
		[IntegerValidator(MinValue = 1)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		[ConfigurationProperty("maxBufferSize", IsRequired = true, DefaultValue = 2147483647)]
		public int MaxBufferSize
		{
			get
			{
				return (int)base[BufferModeSettings._propMaxBufferSize];
			}
			set
			{
				base[BufferModeSettings._propMaxBufferSize] = value;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x00077AE4 File Offset: 0x00076AE4
		// (set) Token: 0x060018F1 RID: 6385 RVA: 0x00077AF6 File Offset: 0x00076AF6
		[IntegerValidator(MinValue = 1)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		[ConfigurationProperty("maxFlushSize", IsRequired = true, DefaultValue = 2147483647)]
		public int MaxFlushSize
		{
			get
			{
				return (int)base[BufferModeSettings._propMaxFlushSize];
			}
			set
			{
				base[BufferModeSettings._propMaxFlushSize] = value;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x00077B09 File Offset: 0x00076B09
		// (set) Token: 0x060018F3 RID: 6387 RVA: 0x00077B1B File Offset: 0x00076B1B
		[ConfigurationProperty("urgentFlushThreshold", IsRequired = true, DefaultValue = 2147483647)]
		[IntegerValidator(MinValue = 1)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		public int UrgentFlushThreshold
		{
			get
			{
				return (int)base[BufferModeSettings._propUrgentFlushThreshold];
			}
			set
			{
				base[BufferModeSettings._propUrgentFlushThreshold] = value;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x00077B2E File Offset: 0x00076B2E
		// (set) Token: 0x060018F5 RID: 6389 RVA: 0x00077B40 File Offset: 0x00076B40
		[ConfigurationProperty("regularFlushInterval", IsRequired = true, DefaultValue = "00:00:01")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan RegularFlushInterval
		{
			get
			{
				return (TimeSpan)base[BufferModeSettings._propRegularFlushInterval];
			}
			set
			{
				base[BufferModeSettings._propRegularFlushInterval] = value;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060018F6 RID: 6390 RVA: 0x00077B53 File Offset: 0x00076B53
		// (set) Token: 0x060018F7 RID: 6391 RVA: 0x00077B65 File Offset: 0x00076B65
		[ConfigurationProperty("urgentFlushInterval", IsRequired = true, DefaultValue = "00:00:00")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		public TimeSpan UrgentFlushInterval
		{
			get
			{
				return (TimeSpan)base[BufferModeSettings._propUrgentFlushInterval];
			}
			set
			{
				base[BufferModeSettings._propUrgentFlushInterval] = value;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00077B78 File Offset: 0x00076B78
		// (set) Token: 0x060018F9 RID: 6393 RVA: 0x00077B8A File Offset: 0x00076B8A
		[TypeConverter(typeof(InfiniteIntConverter))]
		[ConfigurationProperty("maxBufferThreads", DefaultValue = 1)]
		[IntegerValidator(MinValue = 1)]
		public int MaxBufferThreads
		{
			get
			{
				return (int)base[BufferModeSettings._propMaxBufferThreads];
			}
			set
			{
				base[BufferModeSettings._propMaxBufferThreads] = value;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060018FA RID: 6394 RVA: 0x00077B9D File Offset: 0x00076B9D
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return BufferModeSettings.s_elemProperty;
			}
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x00077BA4 File Offset: 0x00076BA4
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("bufferMode");
			}
			BufferModeSettings bufferModeSettings = (BufferModeSettings)value;
			if (bufferModeSettings.UrgentFlushThreshold > bufferModeSettings.MaxBufferSize)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_attribute1_must_less_than_or_equal_attribute2", new object[]
				{
					bufferModeSettings.UrgentFlushThreshold.ToString(CultureInfo.InvariantCulture),
					"urgentFlushThreshold",
					bufferModeSettings.MaxBufferSize.ToString(CultureInfo.InvariantCulture),
					"maxBufferSize"
				}), bufferModeSettings.ElementInformation.Properties["urgentFlushThreshold"].Source, bufferModeSettings.ElementInformation.Properties["urgentFlushThreshold"].LineNumber);
			}
			if (bufferModeSettings.MaxFlushSize > bufferModeSettings.MaxBufferSize)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_attribute1_must_less_than_or_equal_attribute2", new object[]
				{
					bufferModeSettings.MaxFlushSize.ToString(CultureInfo.InvariantCulture),
					"maxFlushSize",
					bufferModeSettings.MaxBufferSize.ToString(CultureInfo.InvariantCulture),
					"maxBufferSize"
				}), bufferModeSettings.ElementInformation.Properties["maxFlushSize"].Source, bufferModeSettings.ElementInformation.Properties["maxFlushSize"].LineNumber);
			}
			if (!(bufferModeSettings.UrgentFlushInterval < bufferModeSettings.RegularFlushInterval))
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_attribute1_must_less_than_attribute2", new object[]
				{
					bufferModeSettings.UrgentFlushInterval.ToString(),
					"urgentFlushInterval",
					bufferModeSettings.RegularFlushInterval.ToString(),
					"regularFlushInterval"
				}), bufferModeSettings.ElementInformation.Properties["urgentFlushInterval"].Source, bufferModeSettings.ElementInformation.Properties["urgentFlushInterval"].LineNumber);
			}
		}

		// Token: 0x040016F1 RID: 5873
		private const int DefaultMaxBufferThreads = 1;

		// Token: 0x040016F2 RID: 5874
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(BufferModeSettings), new ValidatorCallback(BufferModeSettings.Validate)));

		// Token: 0x040016F3 RID: 5875
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040016F4 RID: 5876
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040016F5 RID: 5877
		private static readonly ConfigurationProperty _propMaxBufferSize = new ConfigurationProperty("maxBufferSize", typeof(int), int.MaxValue, new InfiniteIntConverter(), StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040016F6 RID: 5878
		private static readonly ConfigurationProperty _propMaxFlushSize = new ConfigurationProperty("maxFlushSize", typeof(int), int.MaxValue, new InfiniteIntConverter(), StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040016F7 RID: 5879
		private static readonly ConfigurationProperty _propUrgentFlushThreshold = new ConfigurationProperty("urgentFlushThreshold", typeof(int), int.MaxValue, new InfiniteIntConverter(), StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040016F8 RID: 5880
		private static readonly ConfigurationProperty _propRegularFlushInterval = new ConfigurationProperty("regularFlushInterval", typeof(TimeSpan), TimeSpan.FromSeconds(1.0), StdValidatorsAndConverters.InfiniteTimeSpanConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040016F9 RID: 5881
		private static readonly ConfigurationProperty _propUrgentFlushInterval = new ConfigurationProperty("urgentFlushInterval", typeof(TimeSpan), TimeSpan.Zero, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x040016FA RID: 5882
		private static readonly ConfigurationProperty _propMaxBufferThreads = new ConfigurationProperty("maxBufferThreads", typeof(int), 1, new InfiniteIntConverter(), StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.None);
	}
}

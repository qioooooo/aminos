using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001FF RID: 511
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpRuntimeSection : ConfigurationSection
	{
		// Token: 0x06001BCE RID: 7118 RVA: 0x0007FFF0 File Offset: 0x0007EFF0
		static HttpRuntimeSection()
		{
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propExecutionTimeout);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propMaxRequestLength);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propRequestLengthDiskThreshold);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propUseFullyQualifiedRedirectUrl);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propMinFreeThreads);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propMinLocalRequestFreeThreads);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propAppRequestQueueLimit);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propEnableKernelOutputCache);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propEnableVersionHeader);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propRequireRootedSaveAsPath);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propEnable);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propShutdownTimeout);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propDelayNotificationTimeout);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propWaitChangeNotification);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propMaxWaitChangeNotification);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propEnableHeaderChecking);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propSendCacheControlHeader);
			HttpRuntimeSection._properties.Add(HttpRuntimeSection._propApartmentThreading);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x000803D1 File Offset: 0x0007F3D1
		public HttpRuntimeSection()
		{
			this._MaxRequestLengthBytes = -1;
			this._RequestLengthDiskThresholdBytes = -1;
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x000803EE File Offset: 0x0007F3EE
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpRuntimeSection._properties;
			}
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000803F5 File Offset: 0x0007F3F5
		// (set) Token: 0x06001BD2 RID: 7122 RVA: 0x00080422 File Offset: 0x0007F422
		[ConfigurationProperty("executionTimeout", DefaultValue = "00:01:50")]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(TimeSpanSecondsConverter))]
		public TimeSpan ExecutionTimeout
		{
			get
			{
				if (!this.executionTimeoutCached)
				{
					this.executionTimeoutCache = (TimeSpan)base[HttpRuntimeSection._propExecutionTimeout];
					this.executionTimeoutCached = true;
				}
				return this.executionTimeoutCache;
			}
			set
			{
				base[HttpRuntimeSection._propExecutionTimeout] = value;
				this.executionTimeoutCache = value;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x0008043C File Offset: 0x0007F43C
		// (set) Token: 0x06001BD4 RID: 7124 RVA: 0x00080450 File Offset: 0x0007F450
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("maxRequestLength", DefaultValue = 4096)]
		public int MaxRequestLength
		{
			get
			{
				return (int)base[HttpRuntimeSection._propMaxRequestLength];
			}
			set
			{
				if (value < this.RequestLengthDiskThreshold)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_max_request_length_smaller_than_max_request_length_disk_threshold"), base.ElementInformation.Properties[HttpRuntimeSection._propMaxRequestLength.Name].Source, base.ElementInformation.Properties[HttpRuntimeSection._propMaxRequestLength.Name].LineNumber);
				}
				base[HttpRuntimeSection._propMaxRequestLength] = value;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x000804C5 File Offset: 0x0007F4C5
		// (set) Token: 0x06001BD6 RID: 7126 RVA: 0x000804D8 File Offset: 0x0007F4D8
		[IntegerValidator(MinValue = 1)]
		[ConfigurationProperty("requestLengthDiskThreshold", DefaultValue = 80)]
		public int RequestLengthDiskThreshold
		{
			get
			{
				return (int)base[HttpRuntimeSection._propRequestLengthDiskThreshold];
			}
			set
			{
				if (value > this.MaxRequestLength)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_max_request_length_disk_threshold_exceeds_max_request_length"), base.ElementInformation.Properties[HttpRuntimeSection._propRequestLengthDiskThreshold.Name].Source, base.ElementInformation.Properties[HttpRuntimeSection._propRequestLengthDiskThreshold.Name].LineNumber);
				}
				base[HttpRuntimeSection._propRequestLengthDiskThreshold] = value;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x0008054D File Offset: 0x0007F54D
		// (set) Token: 0x06001BD8 RID: 7128 RVA: 0x0008055F File Offset: 0x0007F55F
		[ConfigurationProperty("useFullyQualifiedRedirectUrl", DefaultValue = false)]
		public bool UseFullyQualifiedRedirectUrl
		{
			get
			{
				return (bool)base[HttpRuntimeSection._propUseFullyQualifiedRedirectUrl];
			}
			set
			{
				base[HttpRuntimeSection._propUseFullyQualifiedRedirectUrl] = value;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001BD9 RID: 7129 RVA: 0x00080572 File Offset: 0x0007F572
		// (set) Token: 0x06001BDA RID: 7130 RVA: 0x00080584 File Offset: 0x0007F584
		[ConfigurationProperty("minFreeThreads", DefaultValue = 8)]
		[IntegerValidator(MinValue = 0)]
		public int MinFreeThreads
		{
			get
			{
				return (int)base[HttpRuntimeSection._propMinFreeThreads];
			}
			set
			{
				base[HttpRuntimeSection._propMinFreeThreads] = value;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x00080597 File Offset: 0x0007F597
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x000805A9 File Offset: 0x0007F5A9
		[ConfigurationProperty("minLocalRequestFreeThreads", DefaultValue = 4)]
		[IntegerValidator(MinValue = 0)]
		public int MinLocalRequestFreeThreads
		{
			get
			{
				return (int)base[HttpRuntimeSection._propMinLocalRequestFreeThreads];
			}
			set
			{
				base[HttpRuntimeSection._propMinLocalRequestFreeThreads] = value;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x000805BC File Offset: 0x0007F5BC
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x000805CE File Offset: 0x0007F5CE
		[IntegerValidator(MinValue = 1)]
		[ConfigurationProperty("appRequestQueueLimit", DefaultValue = 5000)]
		public int AppRequestQueueLimit
		{
			get
			{
				return (int)base[HttpRuntimeSection._propAppRequestQueueLimit];
			}
			set
			{
				base[HttpRuntimeSection._propAppRequestQueueLimit] = value;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x000805E1 File Offset: 0x0007F5E1
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x000805F3 File Offset: 0x0007F5F3
		[ConfigurationProperty("enableKernelOutputCache", DefaultValue = true)]
		public bool EnableKernelOutputCache
		{
			get
			{
				return (bool)base[HttpRuntimeSection._propEnableKernelOutputCache];
			}
			set
			{
				base[HttpRuntimeSection._propEnableKernelOutputCache] = value;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x00080606 File Offset: 0x0007F606
		// (set) Token: 0x06001BE2 RID: 7138 RVA: 0x00080633 File Offset: 0x0007F633
		[ConfigurationProperty("enableVersionHeader", DefaultValue = true)]
		public bool EnableVersionHeader
		{
			get
			{
				if (!this.enableVersionHeaderCached)
				{
					this.enableVersionHeaderCache = (bool)base[HttpRuntimeSection._propEnableVersionHeader];
					this.enableVersionHeaderCached = true;
				}
				return this.enableVersionHeaderCache;
			}
			set
			{
				base[HttpRuntimeSection._propEnableVersionHeader] = value;
				this.enableVersionHeaderCache = value;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x0008064D File Offset: 0x0007F64D
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x0008065F File Offset: 0x0007F65F
		[ConfigurationProperty("apartmentThreading", DefaultValue = false)]
		public bool ApartmentThreading
		{
			get
			{
				return (bool)base[HttpRuntimeSection._propApartmentThreading];
			}
			set
			{
				base[HttpRuntimeSection._propApartmentThreading] = value;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x00080672 File Offset: 0x0007F672
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x00080684 File Offset: 0x0007F684
		[ConfigurationProperty("requireRootedSaveAsPath", DefaultValue = true)]
		public bool RequireRootedSaveAsPath
		{
			get
			{
				return (bool)base[HttpRuntimeSection._propRequireRootedSaveAsPath];
			}
			set
			{
				base[HttpRuntimeSection._propRequireRootedSaveAsPath] = value;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00080697 File Offset: 0x0007F697
		// (set) Token: 0x06001BE8 RID: 7144 RVA: 0x000806A9 File Offset: 0x0007F6A9
		[ConfigurationProperty("enable", DefaultValue = true)]
		public bool Enable
		{
			get
			{
				return (bool)base[HttpRuntimeSection._propEnable];
			}
			set
			{
				base[HttpRuntimeSection._propEnable] = value;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x000806BC File Offset: 0x0007F6BC
		// (set) Token: 0x06001BEA RID: 7146 RVA: 0x000806E9 File Offset: 0x0007F6E9
		[ConfigurationProperty("sendCacheControlHeader", DefaultValue = true)]
		public bool SendCacheControlHeader
		{
			get
			{
				if (!this.sendCacheControlHeaderCached)
				{
					this.sendCacheControlHeaderCache = (bool)base[HttpRuntimeSection._propSendCacheControlHeader];
					this.sendCacheControlHeaderCached = true;
				}
				return this.sendCacheControlHeaderCache;
			}
			set
			{
				base[HttpRuntimeSection._propSendCacheControlHeader] = value;
				this.sendCacheControlHeaderCache = value;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001BEB RID: 7147 RVA: 0x00080703 File Offset: 0x0007F703
		// (set) Token: 0x06001BEC RID: 7148 RVA: 0x00080715 File Offset: 0x0007F715
		[TypeConverter(typeof(TimeSpanSecondsConverter))]
		[ConfigurationProperty("shutdownTimeout", DefaultValue = "00:01:30")]
		public TimeSpan ShutdownTimeout
		{
			get
			{
				return (TimeSpan)base[HttpRuntimeSection._propShutdownTimeout];
			}
			set
			{
				base[HttpRuntimeSection._propShutdownTimeout] = value;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001BED RID: 7149 RVA: 0x00080728 File Offset: 0x0007F728
		// (set) Token: 0x06001BEE RID: 7150 RVA: 0x0008073A File Offset: 0x0007F73A
		[TypeConverter(typeof(TimeSpanSecondsConverter))]
		[ConfigurationProperty("delayNotificationTimeout", DefaultValue = "00:00:05")]
		public TimeSpan DelayNotificationTimeout
		{
			get
			{
				return (TimeSpan)base[HttpRuntimeSection._propDelayNotificationTimeout];
			}
			set
			{
				base[HttpRuntimeSection._propDelayNotificationTimeout] = value;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0008074D File Offset: 0x0007F74D
		// (set) Token: 0x06001BF0 RID: 7152 RVA: 0x0008075F File Offset: 0x0007F75F
		[ConfigurationProperty("waitChangeNotification", DefaultValue = 0)]
		[IntegerValidator(MinValue = 0)]
		public int WaitChangeNotification
		{
			get
			{
				return (int)base[HttpRuntimeSection._propWaitChangeNotification];
			}
			set
			{
				base[HttpRuntimeSection._propWaitChangeNotification] = value;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x00080772 File Offset: 0x0007F772
		// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x00080784 File Offset: 0x0007F784
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("maxWaitChangeNotification", DefaultValue = 0)]
		public int MaxWaitChangeNotification
		{
			get
			{
				return (int)base[HttpRuntimeSection._propMaxWaitChangeNotification];
			}
			set
			{
				base[HttpRuntimeSection._propMaxWaitChangeNotification] = value;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x00080797 File Offset: 0x0007F797
		// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x000807A9 File Offset: 0x0007F7A9
		[ConfigurationProperty("enableHeaderChecking", DefaultValue = true)]
		public bool EnableHeaderChecking
		{
			get
			{
				return (bool)base[HttpRuntimeSection._propEnableHeaderChecking];
			}
			set
			{
				base[HttpRuntimeSection._propEnableHeaderChecking] = value;
			}
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000807BC File Offset: 0x0007F7BC
		private int BytesFromKilobytes(int kilobytes)
		{
			long num = (long)kilobytes * 1024L;
			if (num >= 2147483647L)
			{
				return int.MaxValue;
			}
			return (int)num;
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x000807E4 File Offset: 0x0007F7E4
		internal int MaxRequestLengthBytes
		{
			get
			{
				if (this._MaxRequestLengthBytes < 0)
				{
					this._MaxRequestLengthBytes = this.BytesFromKilobytes(this.MaxRequestLength);
				}
				return this._MaxRequestLengthBytes;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x00080807 File Offset: 0x0007F807
		internal int RequestLengthDiskThresholdBytes
		{
			get
			{
				if (this._RequestLengthDiskThresholdBytes < 0)
				{
					this._RequestLengthDiskThresholdBytes = this.BytesFromKilobytes(this.RequestLengthDiskThreshold);
				}
				return this._RequestLengthDiskThresholdBytes;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x0008082C File Offset: 0x0007F82C
		internal string VersionHeader
		{
			get
			{
				if (!this.EnableVersionHeader)
				{
					return null;
				}
				if (HttpRuntimeSection.s_versionHeader == null)
				{
					string text = null;
					try
					{
						string systemWebVersion = VersionInfo.SystemWebVersion;
						int num = systemWebVersion.LastIndexOf('.');
						if (num > 0)
						{
							text = systemWebVersion.Substring(0, num);
						}
					}
					catch
					{
					}
					if (text == null)
					{
						text = string.Empty;
					}
					HttpRuntimeSection.s_versionHeader = text;
				}
				return HttpRuntimeSection.s_versionHeader;
			}
		}

		// Token: 0x04001885 RID: 6277
		internal const int DefaultExecutionTimeout = 110;

		// Token: 0x04001886 RID: 6278
		internal const int DefaultMaxRequestLength = 4194304;

		// Token: 0x04001887 RID: 6279
		internal const int DefaultRequestLengthDiskThreshold = 81920;

		// Token: 0x04001888 RID: 6280
		internal const int DefaultMinFreeThreads = 8;

		// Token: 0x04001889 RID: 6281
		internal const int DefaultMinLocalRequestFreeThreads = 4;

		// Token: 0x0400188A RID: 6282
		internal const int DefaultAppRequestQueueLimit = 100;

		// Token: 0x0400188B RID: 6283
		internal const int DefaultShutdownTimeout = 90;

		// Token: 0x0400188C RID: 6284
		internal const int DefaultDelayNotificationTimeout = 5;

		// Token: 0x0400188D RID: 6285
		internal const int DefaultWaitChangeNotification = 0;

		// Token: 0x0400188E RID: 6286
		internal const int DefaultMaxWaitChangeNotification = 0;

		// Token: 0x0400188F RID: 6287
		internal const bool DefaultEnableKernelOutputCache = true;

		// Token: 0x04001890 RID: 6288
		internal const bool DefaultRequireRootedSaveAsPath = true;

		// Token: 0x04001891 RID: 6289
		internal const bool DefaultSendCacheControlHeader = true;

		// Token: 0x04001892 RID: 6290
		private bool enableVersionHeaderCache = true;

		// Token: 0x04001893 RID: 6291
		private bool enableVersionHeaderCached;

		// Token: 0x04001894 RID: 6292
		private TimeSpan executionTimeoutCache;

		// Token: 0x04001895 RID: 6293
		private bool executionTimeoutCached;

		// Token: 0x04001896 RID: 6294
		private bool sendCacheControlHeaderCached;

		// Token: 0x04001897 RID: 6295
		private bool sendCacheControlHeaderCache;

		// Token: 0x04001898 RID: 6296
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001899 RID: 6297
		private static readonly ConfigurationProperty _propExecutionTimeout = new ConfigurationProperty("executionTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(110.0), StdValidatorsAndConverters.TimeSpanSecondsConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400189A RID: 6298
		private static readonly ConfigurationProperty _propMaxRequestLength = new ConfigurationProperty("maxRequestLength", typeof(int), 4096, null, new IntegerValidator(0, 2097151), ConfigurationPropertyOptions.None);

		// Token: 0x0400189B RID: 6299
		private static readonly ConfigurationProperty _propRequestLengthDiskThreshold = new ConfigurationProperty("requestLengthDiskThreshold", typeof(int), 80, null, StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400189C RID: 6300
		private static readonly ConfigurationProperty _propUseFullyQualifiedRedirectUrl = new ConfigurationProperty("useFullyQualifiedRedirectUrl", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400189D RID: 6301
		private static readonly ConfigurationProperty _propMinFreeThreads = new ConfigurationProperty("minFreeThreads", typeof(int), 8, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400189E RID: 6302
		private static readonly ConfigurationProperty _propMinLocalRequestFreeThreads = new ConfigurationProperty("minLocalRequestFreeThreads", typeof(int), 4, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400189F RID: 6303
		private static readonly ConfigurationProperty _propAppRequestQueueLimit = new ConfigurationProperty("appRequestQueueLimit", typeof(int), 5000, null, StdValidatorsAndConverters.NonZeroPositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018A0 RID: 6304
		private static readonly ConfigurationProperty _propEnableKernelOutputCache = new ConfigurationProperty("enableKernelOutputCache", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018A1 RID: 6305
		private static readonly ConfigurationProperty _propEnableVersionHeader = new ConfigurationProperty("enableVersionHeader", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018A2 RID: 6306
		private static readonly ConfigurationProperty _propRequireRootedSaveAsPath = new ConfigurationProperty("requireRootedSaveAsPath", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018A3 RID: 6307
		private static readonly ConfigurationProperty _propEnable = new ConfigurationProperty("enable", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018A4 RID: 6308
		private static readonly ConfigurationProperty _propShutdownTimeout = new ConfigurationProperty("shutdownTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(90.0), StdValidatorsAndConverters.TimeSpanSecondsConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x040018A5 RID: 6309
		private static readonly ConfigurationProperty _propDelayNotificationTimeout = new ConfigurationProperty("delayNotificationTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(5.0), StdValidatorsAndConverters.TimeSpanSecondsConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x040018A6 RID: 6310
		private static readonly ConfigurationProperty _propWaitChangeNotification = new ConfigurationProperty("waitChangeNotification", typeof(int), 0, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018A7 RID: 6311
		private static readonly ConfigurationProperty _propMaxWaitChangeNotification = new ConfigurationProperty("maxWaitChangeNotification", typeof(int), 0, null, StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x040018A8 RID: 6312
		private static readonly ConfigurationProperty _propEnableHeaderChecking = new ConfigurationProperty("enableHeaderChecking", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018A9 RID: 6313
		private static readonly ConfigurationProperty _propSendCacheControlHeader = new ConfigurationProperty("sendCacheControlHeader", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x040018AA RID: 6314
		private static readonly ConfigurationProperty _propApartmentThreading = new ConfigurationProperty("apartmentThreading", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x040018AB RID: 6315
		private int _MaxRequestLengthBytes;

		// Token: 0x040018AC RID: 6316
		private int _RequestLengthDiskThresholdBytes;

		// Token: 0x040018AD RID: 6317
		private static string s_versionHeader = null;
	}
}

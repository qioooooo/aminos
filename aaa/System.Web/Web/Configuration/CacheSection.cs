using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001B1 RID: 433
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CacheSection : ConfigurationSection
	{
		// Token: 0x06001914 RID: 6420 RVA: 0x0007803C File Offset: 0x0007703C
		static CacheSection()
		{
			CacheSection._properties.Add(CacheSection._propDisableMemoryCollection);
			CacheSection._properties.Add(CacheSection._propDisableExpiration);
			CacheSection._properties.Add(CacheSection._propPrivateBytesLimit);
			CacheSection._properties.Add(CacheSection._propPercentagePhysicalMemoryUsedLimit);
			CacheSection._properties.Add(CacheSection._propPrivateBytesPollTime);
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001916 RID: 6422 RVA: 0x0007817C File Offset: 0x0007717C
		// (set) Token: 0x06001917 RID: 6423 RVA: 0x0007818E File Offset: 0x0007718E
		[ConfigurationProperty("disableMemoryCollection", DefaultValue = false)]
		public bool DisableMemoryCollection
		{
			get
			{
				return (bool)base[CacheSection._propDisableMemoryCollection];
			}
			set
			{
				base[CacheSection._propDisableMemoryCollection] = value;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001918 RID: 6424 RVA: 0x000781A1 File Offset: 0x000771A1
		// (set) Token: 0x06001919 RID: 6425 RVA: 0x000781B3 File Offset: 0x000771B3
		[ConfigurationProperty("disableExpiration", DefaultValue = false)]
		public bool DisableExpiration
		{
			get
			{
				return (bool)base[CacheSection._propDisableExpiration];
			}
			set
			{
				base[CacheSection._propDisableExpiration] = value;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600191A RID: 6426 RVA: 0x000781C6 File Offset: 0x000771C6
		// (set) Token: 0x0600191B RID: 6427 RVA: 0x000781D8 File Offset: 0x000771D8
		[LongValidator(MinValue = 0L)]
		[ConfigurationProperty("privateBytesLimit", DefaultValue = 0L)]
		public long PrivateBytesLimit
		{
			get
			{
				return (long)base[CacheSection._propPrivateBytesLimit];
			}
			set
			{
				base[CacheSection._propPrivateBytesLimit] = value;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600191C RID: 6428 RVA: 0x000781EB File Offset: 0x000771EB
		// (set) Token: 0x0600191D RID: 6429 RVA: 0x000781FD File Offset: 0x000771FD
		[IntegerValidator(MinValue = 0, MaxValue = 100)]
		[ConfigurationProperty("percentagePhysicalMemoryUsedLimit", DefaultValue = 0)]
		public int PercentagePhysicalMemoryUsedLimit
		{
			get
			{
				return (int)base[CacheSection._propPercentagePhysicalMemoryUsedLimit];
			}
			set
			{
				base[CacheSection._propPercentagePhysicalMemoryUsedLimit] = value;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600191E RID: 6430 RVA: 0x00078210 File Offset: 0x00077210
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CacheSection._properties;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600191F RID: 6431 RVA: 0x00078217 File Offset: 0x00077217
		// (set) Token: 0x06001920 RID: 6432 RVA: 0x00078229 File Offset: 0x00077229
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[ConfigurationProperty("privateBytesPollTime", DefaultValue = "00:02:00")]
		public TimeSpan PrivateBytesPollTime
		{
			get
			{
				return (TimeSpan)base[CacheSection._propPrivateBytesPollTime];
			}
			set
			{
				base[CacheSection._propPrivateBytesPollTime] = value;
			}
		}

		// Token: 0x04001701 RID: 5889
		internal static TimeSpan DefaultPrivateBytesPollTime = new TimeSpan(0, 2, 0);

		// Token: 0x04001702 RID: 5890
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001703 RID: 5891
		private static readonly ConfigurationProperty _propDisableMemoryCollection = new ConfigurationProperty("disableMemoryCollection", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001704 RID: 5892
		private static readonly ConfigurationProperty _propDisableExpiration = new ConfigurationProperty("disableExpiration", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001705 RID: 5893
		private static readonly ConfigurationProperty _propPrivateBytesLimit = new ConfigurationProperty("privateBytesLimit", typeof(long), 0L, null, new LongValidator(0L, long.MaxValue), ConfigurationPropertyOptions.None);

		// Token: 0x04001706 RID: 5894
		private static readonly ConfigurationProperty _propPercentagePhysicalMemoryUsedLimit = new ConfigurationProperty("percentagePhysicalMemoryUsedLimit", typeof(int), 0, null, new IntegerValidator(0, 100), ConfigurationPropertyOptions.None);

		// Token: 0x04001707 RID: 5895
		private static readonly ConfigurationProperty _propPrivateBytesPollTime = new ConfigurationProperty("privateBytesPollTime", typeof(TimeSpan), CacheSection.DefaultPrivateBytesPollTime, StdValidatorsAndConverters.InfiniteTimeSpanConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);
	}
}

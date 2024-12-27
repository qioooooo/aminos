using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000226 RID: 550
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProcessModelSection : ConfigurationSection
	{
		// Token: 0x06001D51 RID: 7505 RVA: 0x00085714 File Offset: 0x00084714
		static ProcessModelSection()
		{
			ProcessModelSection._properties.Add(ProcessModelSection._propEnable);
			ProcessModelSection._properties.Add(ProcessModelSection._propTimeout);
			ProcessModelSection._properties.Add(ProcessModelSection._propIdleTimeout);
			ProcessModelSection._properties.Add(ProcessModelSection._propShutdownTimeout);
			ProcessModelSection._properties.Add(ProcessModelSection._propRequestLimit);
			ProcessModelSection._properties.Add(ProcessModelSection._propRequestQueueLimit);
			ProcessModelSection._properties.Add(ProcessModelSection._propRestartQueueLimit);
			ProcessModelSection._properties.Add(ProcessModelSection._propMemoryLimit);
			ProcessModelSection._properties.Add(ProcessModelSection._propWebGarden);
			ProcessModelSection._properties.Add(ProcessModelSection._propCpuMask);
			ProcessModelSection._properties.Add(ProcessModelSection._propUserName);
			ProcessModelSection._properties.Add(ProcessModelSection._propPassword);
			ProcessModelSection._properties.Add(ProcessModelSection._propLogLevel);
			ProcessModelSection._properties.Add(ProcessModelSection._propClientConnectedCheck);
			ProcessModelSection._properties.Add(ProcessModelSection._propComAuthenticationLevel);
			ProcessModelSection._properties.Add(ProcessModelSection._propComImpersonationLevel);
			ProcessModelSection._properties.Add(ProcessModelSection._propResponseDeadlockInterval);
			ProcessModelSection._properties.Add(ProcessModelSection._propResponseRestartDeadlockInterval);
			ProcessModelSection._properties.Add(ProcessModelSection._propAutoConfig);
			ProcessModelSection._properties.Add(ProcessModelSection._propMaxWorkerThreads);
			ProcessModelSection._properties.Add(ProcessModelSection._propMaxIOThreads);
			ProcessModelSection._properties.Add(ProcessModelSection._propMinWorkerThreads);
			ProcessModelSection._properties.Add(ProcessModelSection._propMinIOThreads);
			ProcessModelSection._properties.Add(ProcessModelSection._propServerErrorMessageFile);
			ProcessModelSection._properties.Add(ProcessModelSection._propPingFrequency);
			ProcessModelSection._properties.Add(ProcessModelSection._propPingTimeout);
			ProcessModelSection._properties.Add(ProcessModelSection._propMaxAppDomains);
			ProcessModelSection.cpuCount = SystemInfo.GetNumProcessCPUs();
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x00085D3D File Offset: 0x00084D3D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ProcessModelSection._properties;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001D54 RID: 7508 RVA: 0x00085D44 File Offset: 0x00084D44
		// (set) Token: 0x06001D55 RID: 7509 RVA: 0x00085D56 File Offset: 0x00084D56
		[ConfigurationProperty("enable", DefaultValue = true)]
		public bool Enable
		{
			get
			{
				return (bool)base[ProcessModelSection._propEnable];
			}
			set
			{
				base[ProcessModelSection._propEnable] = value;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001D56 RID: 7510 RVA: 0x00085D69 File Offset: 0x00084D69
		// (set) Token: 0x06001D57 RID: 7511 RVA: 0x00085D7B File Offset: 0x00084D7B
		[ConfigurationProperty("timeout", DefaultValue = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		public TimeSpan Timeout
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propTimeout];
			}
			set
			{
				base[ProcessModelSection._propTimeout] = value;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x00085D8E File Offset: 0x00084D8E
		// (set) Token: 0x06001D59 RID: 7513 RVA: 0x00085DA0 File Offset: 0x00084DA0
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[ConfigurationProperty("idleTimeout", DefaultValue = "10675199.02:48:05.4775807")]
		public TimeSpan IdleTimeout
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propIdleTimeout];
			}
			set
			{
				base[ProcessModelSection._propIdleTimeout] = value;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001D5A RID: 7514 RVA: 0x00085DB3 File Offset: 0x00084DB3
		// (set) Token: 0x06001D5B RID: 7515 RVA: 0x00085DC5 File Offset: 0x00084DC5
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[ConfigurationProperty("shutdownTimeout", DefaultValue = "00:00:05")]
		public TimeSpan ShutdownTimeout
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propShutdownTimeout];
			}
			set
			{
				base[ProcessModelSection._propShutdownTimeout] = value;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001D5C RID: 7516 RVA: 0x00085DD8 File Offset: 0x00084DD8
		// (set) Token: 0x06001D5D RID: 7517 RVA: 0x00085DEA File Offset: 0x00084DEA
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("requestLimit", DefaultValue = 2147483647)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		public int RequestLimit
		{
			get
			{
				return (int)base[ProcessModelSection._propRequestLimit];
			}
			set
			{
				base[ProcessModelSection._propRequestLimit] = value;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001D5E RID: 7518 RVA: 0x00085DFD File Offset: 0x00084DFD
		// (set) Token: 0x06001D5F RID: 7519 RVA: 0x00085E0F File Offset: 0x00084E0F
		[ConfigurationProperty("requestQueueLimit", DefaultValue = 5000)]
		[IntegerValidator(MinValue = 0)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		public int RequestQueueLimit
		{
			get
			{
				return (int)base[ProcessModelSection._propRequestQueueLimit];
			}
			set
			{
				base[ProcessModelSection._propRequestQueueLimit] = value;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001D60 RID: 7520 RVA: 0x00085E22 File Offset: 0x00084E22
		// (set) Token: 0x06001D61 RID: 7521 RVA: 0x00085E34 File Offset: 0x00084E34
		[IntegerValidator(MinValue = 0)]
		[ConfigurationProperty("restartQueueLimit", DefaultValue = 10)]
		[TypeConverter(typeof(InfiniteIntConverter))]
		public int RestartQueueLimit
		{
			get
			{
				return (int)base[ProcessModelSection._propRestartQueueLimit];
			}
			set
			{
				base[ProcessModelSection._propRestartQueueLimit] = value;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x00085E47 File Offset: 0x00084E47
		// (set) Token: 0x06001D63 RID: 7523 RVA: 0x00085E59 File Offset: 0x00084E59
		[ConfigurationProperty("memoryLimit", DefaultValue = 60)]
		public int MemoryLimit
		{
			get
			{
				return (int)base[ProcessModelSection._propMemoryLimit];
			}
			set
			{
				base[ProcessModelSection._propMemoryLimit] = value;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x00085E6C File Offset: 0x00084E6C
		// (set) Token: 0x06001D65 RID: 7525 RVA: 0x00085E7E File Offset: 0x00084E7E
		[ConfigurationProperty("webGarden", DefaultValue = false)]
		public bool WebGarden
		{
			get
			{
				return (bool)base[ProcessModelSection._propWebGarden];
			}
			set
			{
				base[ProcessModelSection._propWebGarden] = value;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001D66 RID: 7526 RVA: 0x00085E91 File Offset: 0x00084E91
		// (set) Token: 0x06001D67 RID: 7527 RVA: 0x00085EAA File Offset: 0x00084EAA
		[ConfigurationProperty("cpuMask", DefaultValue = "0xffffffff")]
		public int CpuMask
		{
			get
			{
				return Convert.ToInt32((string)base[ProcessModelSection._propCpuMask], 16);
			}
			set
			{
				base[ProcessModelSection._propCpuMask] = "0x" + Convert.ToString(value, 16);
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001D68 RID: 7528 RVA: 0x00085EC9 File Offset: 0x00084EC9
		// (set) Token: 0x06001D69 RID: 7529 RVA: 0x00085EDB File Offset: 0x00084EDB
		[ConfigurationProperty("userName", DefaultValue = "machine")]
		public string UserName
		{
			get
			{
				return (string)base[ProcessModelSection._propUserName];
			}
			set
			{
				base[ProcessModelSection._propUserName] = value;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x00085EE9 File Offset: 0x00084EE9
		// (set) Token: 0x06001D6B RID: 7531 RVA: 0x00085EFB File Offset: 0x00084EFB
		[ConfigurationProperty("password", DefaultValue = "AutoGenerate")]
		public string Password
		{
			get
			{
				return (string)base[ProcessModelSection._propPassword];
			}
			set
			{
				base[ProcessModelSection._propPassword] = value;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x00085F09 File Offset: 0x00084F09
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x00085F1B File Offset: 0x00084F1B
		[ConfigurationProperty("logLevel", DefaultValue = ProcessModelLogLevel.Errors)]
		public ProcessModelLogLevel LogLevel
		{
			get
			{
				return (ProcessModelLogLevel)base[ProcessModelSection._propLogLevel];
			}
			set
			{
				base[ProcessModelSection._propLogLevel] = value;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x00085F2E File Offset: 0x00084F2E
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x00085F40 File Offset: 0x00084F40
		[ConfigurationProperty("clientConnectedCheck", DefaultValue = "00:00:05")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		public TimeSpan ClientConnectedCheck
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propClientConnectedCheck];
			}
			set
			{
				base[ProcessModelSection._propClientConnectedCheck] = value;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x00085F53 File Offset: 0x00084F53
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x00085F65 File Offset: 0x00084F65
		[ConfigurationProperty("comAuthenticationLevel", DefaultValue = ProcessModelComAuthenticationLevel.Connect)]
		public ProcessModelComAuthenticationLevel ComAuthenticationLevel
		{
			get
			{
				return (ProcessModelComAuthenticationLevel)base[ProcessModelSection._propComAuthenticationLevel];
			}
			set
			{
				base[ProcessModelSection._propComAuthenticationLevel] = value;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x00085F78 File Offset: 0x00084F78
		// (set) Token: 0x06001D73 RID: 7539 RVA: 0x00085F8A File Offset: 0x00084F8A
		[ConfigurationProperty("comImpersonationLevel", DefaultValue = ProcessModelComImpersonationLevel.Impersonate)]
		public ProcessModelComImpersonationLevel ComImpersonationLevel
		{
			get
			{
				return (ProcessModelComImpersonationLevel)base[ProcessModelSection._propComImpersonationLevel];
			}
			set
			{
				base[ProcessModelSection._propComImpersonationLevel] = value;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x00085F9D File Offset: 0x00084F9D
		// (set) Token: 0x06001D75 RID: 7541 RVA: 0x00085FAF File Offset: 0x00084FAF
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		[ConfigurationProperty("responseDeadlockInterval", DefaultValue = "00:03:00")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		public TimeSpan ResponseDeadlockInterval
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propResponseDeadlockInterval];
			}
			set
			{
				base[ProcessModelSection._propResponseDeadlockInterval] = value;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001D76 RID: 7542 RVA: 0x00085FC2 File Offset: 0x00084FC2
		// (set) Token: 0x06001D77 RID: 7543 RVA: 0x00085FD4 File Offset: 0x00084FD4
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[ConfigurationProperty("responseRestartDeadlockInterval", DefaultValue = "00:03:00")]
		public TimeSpan ResponseRestartDeadlockInterval
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propResponseRestartDeadlockInterval];
			}
			set
			{
				base[ProcessModelSection._propResponseRestartDeadlockInterval] = value;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x00085FE7 File Offset: 0x00084FE7
		// (set) Token: 0x06001D79 RID: 7545 RVA: 0x00085FF9 File Offset: 0x00084FF9
		[ConfigurationProperty("autoConfig", DefaultValue = false)]
		public bool AutoConfig
		{
			get
			{
				return (bool)base[ProcessModelSection._propAutoConfig];
			}
			set
			{
				base[ProcessModelSection._propAutoConfig] = value;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x0008600C File Offset: 0x0008500C
		// (set) Token: 0x06001D7B RID: 7547 RVA: 0x0008601E File Offset: 0x0008501E
		[ConfigurationProperty("maxWorkerThreads", DefaultValue = 20)]
		[IntegerValidator(MinValue = 1, MaxValue = 2147483646)]
		public int MaxWorkerThreads
		{
			get
			{
				return (int)base[ProcessModelSection._propMaxWorkerThreads];
			}
			set
			{
				base[ProcessModelSection._propMaxWorkerThreads] = value;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x00086031 File Offset: 0x00085031
		// (set) Token: 0x06001D7D RID: 7549 RVA: 0x00086043 File Offset: 0x00085043
		[IntegerValidator(MinValue = 1, MaxValue = 2147483646)]
		[ConfigurationProperty("maxIoThreads", DefaultValue = 20)]
		public int MaxIOThreads
		{
			get
			{
				return (int)base[ProcessModelSection._propMaxIOThreads];
			}
			set
			{
				base[ProcessModelSection._propMaxIOThreads] = value;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x00086056 File Offset: 0x00085056
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x00086068 File Offset: 0x00085068
		[ConfigurationProperty("minWorkerThreads", DefaultValue = 1)]
		[IntegerValidator(MinValue = 1, MaxValue = 2147483646)]
		public int MinWorkerThreads
		{
			get
			{
				return (int)base[ProcessModelSection._propMinWorkerThreads];
			}
			set
			{
				base[ProcessModelSection._propMinWorkerThreads] = value;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x0008607B File Offset: 0x0008507B
		// (set) Token: 0x06001D81 RID: 7553 RVA: 0x0008608D File Offset: 0x0008508D
		[IntegerValidator(MinValue = 1, MaxValue = 2147483646)]
		[ConfigurationProperty("minIoThreads", DefaultValue = 1)]
		public int MinIOThreads
		{
			get
			{
				return (int)base[ProcessModelSection._propMinIOThreads];
			}
			set
			{
				base[ProcessModelSection._propMinIOThreads] = value;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x000860A0 File Offset: 0x000850A0
		// (set) Token: 0x06001D83 RID: 7555 RVA: 0x000860B2 File Offset: 0x000850B2
		[ConfigurationProperty("serverErrorMessageFile", DefaultValue = "")]
		public string ServerErrorMessageFile
		{
			get
			{
				return (string)base[ProcessModelSection._propServerErrorMessageFile];
			}
			set
			{
				base[ProcessModelSection._propServerErrorMessageFile] = value;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x000860C0 File Offset: 0x000850C0
		// (set) Token: 0x06001D85 RID: 7557 RVA: 0x000860D2 File Offset: 0x000850D2
		[ConfigurationProperty("pingFrequency", DefaultValue = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		public TimeSpan PingFrequency
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propPingFrequency];
			}
			set
			{
				base[ProcessModelSection._propPingFrequency] = value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x000860E5 File Offset: 0x000850E5
		// (set) Token: 0x06001D87 RID: 7559 RVA: 0x000860F7 File Offset: 0x000850F7
		[TypeConverter(typeof(InfiniteTimeSpanConverter))]
		[ConfigurationProperty("pingTimeout", DefaultValue = "10675199.02:48:05.4775807")]
		public TimeSpan PingTimeout
		{
			get
			{
				return (TimeSpan)base[ProcessModelSection._propPingTimeout];
			}
			set
			{
				base[ProcessModelSection._propPingTimeout] = value;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x0008610A File Offset: 0x0008510A
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x0008611C File Offset: 0x0008511C
		[IntegerValidator(MinValue = 1, MaxValue = 2147483646)]
		[ConfigurationProperty("maxAppDomains", DefaultValue = 2000)]
		public int MaxAppDomains
		{
			get
			{
				return (int)base[ProcessModelSection._propMaxAppDomains];
			}
			set
			{
				base[ProcessModelSection._propMaxAppDomains] = value;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x0008612F File Offset: 0x0008512F
		internal int CpuCount
		{
			get
			{
				return ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x00086136 File Offset: 0x00085136
		internal int DefaultMaxWorkerThreadsForAutoConfig
		{
			get
			{
				return 100 * ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x00086140 File Offset: 0x00085140
		internal int DefaultMaxIoThreadsForAutoConfig
		{
			get
			{
				return 100 * ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x0008614A File Offset: 0x0008514A
		internal int MaxWorkerThreadsTimesCpuCount
		{
			get
			{
				return this.MaxWorkerThreads * ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001D8E RID: 7566 RVA: 0x00086158 File Offset: 0x00085158
		internal int MaxIoThreadsTimesCpuCount
		{
			get
			{
				return this.MaxIOThreads * ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x00086166 File Offset: 0x00085166
		internal int MinWorkerThreadsTimesCpuCount
		{
			get
			{
				return this.MinWorkerThreads * ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x00086174 File Offset: 0x00085174
		internal int MinIoThreadsTimesCpuCount
		{
			get
			{
				return this.MinIOThreads * ProcessModelSection.cpuCount;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x00086182 File Offset: 0x00085182
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return ProcessModelSection.s_elemProperty;
			}
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0008618C File Offset: 0x0008518C
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			ProcessModelSection processModelSection = (ProcessModelSection)value;
			int num = -1;
			try
			{
				num = processModelSection.CpuMask;
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_non_zero_hexadecimal_attribute", new object[] { "cpuMask" }), processModelSection.ElementInformation.Properties["cpuMask"].Source, processModelSection.ElementInformation.Properties["cpuMask"].LineNumber);
			}
			if (num == 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Invalid_non_zero_hexadecimal_attribute", new object[] { "cpuMask" }), processModelSection.ElementInformation.Properties["cpuMask"].Source, processModelSection.ElementInformation.Properties["cpuMask"].LineNumber);
			}
		}

		// Token: 0x04001961 RID: 6497
		internal const string sectionName = "system.web/processModel";

		// Token: 0x04001962 RID: 6498
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(ProcessModelSection), new ValidatorCallback(ProcessModelSection.Validate)));

		// Token: 0x04001963 RID: 6499
		internal static TimeSpan DefaultClientConnectedCheck = new TimeSpan(0, 0, 5);

		// Token: 0x04001964 RID: 6500
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001965 RID: 6501
		private static readonly ConfigurationProperty _propEnable = new ConfigurationProperty("enable", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001966 RID: 6502
		private static readonly ConfigurationProperty _propTimeout = new ConfigurationProperty("timeout", typeof(TimeSpan), TimeSpan.MaxValue, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001967 RID: 6503
		private static readonly ConfigurationProperty _propIdleTimeout = new ConfigurationProperty("idleTimeout", typeof(TimeSpan), TimeSpan.MaxValue, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001968 RID: 6504
		private static readonly ConfigurationProperty _propShutdownTimeout = new ConfigurationProperty("shutdownTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(5.0), StdValidatorsAndConverters.InfiniteTimeSpanConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001969 RID: 6505
		private static readonly ConfigurationProperty _propRequestLimit = new ConfigurationProperty("requestLimit", typeof(int), int.MaxValue, new InfiniteIntConverter(), StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400196A RID: 6506
		private static readonly ConfigurationProperty _propRequestQueueLimit = new ConfigurationProperty("requestQueueLimit", typeof(int), 5000, new InfiniteIntConverter(), StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400196B RID: 6507
		private static readonly ConfigurationProperty _propRestartQueueLimit = new ConfigurationProperty("restartQueueLimit", typeof(int), 10, new InfiniteIntConverter(), StdValidatorsAndConverters.PositiveIntegerValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400196C RID: 6508
		private static readonly ConfigurationProperty _propMemoryLimit = new ConfigurationProperty("memoryLimit", typeof(int), 60, ConfigurationPropertyOptions.None);

		// Token: 0x0400196D RID: 6509
		private static readonly ConfigurationProperty _propWebGarden = new ConfigurationProperty("webGarden", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x0400196E RID: 6510
		private static readonly ConfigurationProperty _propCpuMask = new ConfigurationProperty("cpuMask", typeof(string), "0xffffffff", ConfigurationPropertyOptions.None);

		// Token: 0x0400196F RID: 6511
		private static readonly ConfigurationProperty _propUserName = new ConfigurationProperty("userName", typeof(string), "machine", ConfigurationPropertyOptions.None);

		// Token: 0x04001970 RID: 6512
		private static readonly ConfigurationProperty _propPassword = new ConfigurationProperty("password", typeof(string), "AutoGenerate", ConfigurationPropertyOptions.None);

		// Token: 0x04001971 RID: 6513
		private static readonly ConfigurationProperty _propLogLevel = new ConfigurationProperty("logLevel", typeof(ProcessModelLogLevel), ProcessModelLogLevel.Errors, ConfigurationPropertyOptions.None);

		// Token: 0x04001972 RID: 6514
		private static readonly ConfigurationProperty _propClientConnectedCheck = new ConfigurationProperty("clientConnectedCheck", typeof(TimeSpan), ProcessModelSection.DefaultClientConnectedCheck, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001973 RID: 6515
		private static readonly ConfigurationProperty _propComAuthenticationLevel = new ConfigurationProperty("comAuthenticationLevel", typeof(ProcessModelComAuthenticationLevel), ProcessModelComAuthenticationLevel.Connect, ConfigurationPropertyOptions.None);

		// Token: 0x04001974 RID: 6516
		private static readonly ConfigurationProperty _propComImpersonationLevel = new ConfigurationProperty("comImpersonationLevel", typeof(ProcessModelComImpersonationLevel), ProcessModelComImpersonationLevel.Impersonate, ConfigurationPropertyOptions.None);

		// Token: 0x04001975 RID: 6517
		private static readonly ConfigurationProperty _propResponseDeadlockInterval = new ConfigurationProperty("responseDeadlockInterval", typeof(TimeSpan), TimeSpan.FromMinutes(3.0), StdValidatorsAndConverters.InfiniteTimeSpanConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001976 RID: 6518
		private static readonly ConfigurationProperty _propResponseRestartDeadlockInterval = new ConfigurationProperty("responseRestartDeadlockInterval", typeof(TimeSpan), TimeSpan.FromMinutes(3.0), StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x04001977 RID: 6519
		private static readonly ConfigurationProperty _propAutoConfig = new ConfigurationProperty("autoConfig", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04001978 RID: 6520
		private static readonly ConfigurationProperty _propMaxWorkerThreads = new ConfigurationProperty("maxWorkerThreads", typeof(int), 20, null, new IntegerValidator(1, 2147483646), ConfigurationPropertyOptions.None);

		// Token: 0x04001979 RID: 6521
		private static readonly ConfigurationProperty _propMaxIOThreads = new ConfigurationProperty("maxIoThreads", typeof(int), 20, null, new IntegerValidator(1, 2147483646), ConfigurationPropertyOptions.None);

		// Token: 0x0400197A RID: 6522
		private static readonly ConfigurationProperty _propMinWorkerThreads = new ConfigurationProperty("minWorkerThreads", typeof(int), 1, null, new IntegerValidator(1, 2147483646), ConfigurationPropertyOptions.None);

		// Token: 0x0400197B RID: 6523
		private static readonly ConfigurationProperty _propMinIOThreads = new ConfigurationProperty("minIoThreads", typeof(int), 1, null, new IntegerValidator(1, 2147483646), ConfigurationPropertyOptions.None);

		// Token: 0x0400197C RID: 6524
		private static readonly ConfigurationProperty _propServerErrorMessageFile = new ConfigurationProperty("serverErrorMessageFile", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x0400197D RID: 6525
		private static readonly ConfigurationProperty _propPingFrequency = new ConfigurationProperty("pingFrequency", typeof(TimeSpan), TimeSpan.MaxValue, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x0400197E RID: 6526
		private static readonly ConfigurationProperty _propPingTimeout = new ConfigurationProperty("pingTimeout", typeof(TimeSpan), TimeSpan.MaxValue, StdValidatorsAndConverters.InfiniteTimeSpanConverter, null, ConfigurationPropertyOptions.None);

		// Token: 0x0400197F RID: 6527
		private static readonly ConfigurationProperty _propMaxAppDomains = new ConfigurationProperty("maxAppDomains", typeof(int), 2000, null, new IntegerValidator(1, 2147483646), ConfigurationPropertyOptions.None);

		// Token: 0x04001980 RID: 6528
		private static int cpuCount;
	}
}

using System;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001F0 RID: 496
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HostingEnvironmentSection : ConfigurationSection
	{
		// Token: 0x06001B32 RID: 6962 RVA: 0x0007E174 File Offset: 0x0007D174
		static HostingEnvironmentSection()
		{
			HostingEnvironmentSection._properties.Add(HostingEnvironmentSection._propIdleTimeout);
			HostingEnvironmentSection._properties.Add(HostingEnvironmentSection._propShutdownTimeout);
			HostingEnvironmentSection._properties.Add(HostingEnvironmentSection._propShadowCopyBinAssemblies);
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x0007E24F File Offset: 0x0007D24F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HostingEnvironmentSection._properties;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x0007E256 File Offset: 0x0007D256
		// (set) Token: 0x06001B36 RID: 6966 RVA: 0x0007E268 File Offset: 0x0007D268
		[TypeConverter(typeof(TimeSpanSecondsConverter))]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		[ConfigurationProperty("shutdownTimeout", DefaultValue = "00:00:30")]
		public TimeSpan ShutdownTimeout
		{
			get
			{
				return (TimeSpan)base[HostingEnvironmentSection._propShutdownTimeout];
			}
			set
			{
				base[HostingEnvironmentSection._propShutdownTimeout] = value;
			}
		}

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x0007E27B File Offset: 0x0007D27B
		// (set) Token: 0x06001B38 RID: 6968 RVA: 0x0007E28D File Offset: 0x0007D28D
		[ConfigurationProperty("idleTimeout", DefaultValue = "10675199.02:48:05.4775807")]
		[TypeConverter(typeof(TimeSpanMinutesOrInfiniteConverter))]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan IdleTimeout
		{
			get
			{
				return (TimeSpan)base[HostingEnvironmentSection._propIdleTimeout];
			}
			set
			{
				base[HostingEnvironmentSection._propIdleTimeout] = value;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x0007E2A0 File Offset: 0x0007D2A0
		// (set) Token: 0x06001B3A RID: 6970 RVA: 0x0007E2B2 File Offset: 0x0007D2B2
		[ConfigurationProperty("shadowCopyBinAssemblies", DefaultValue = true)]
		public bool ShadowCopyBinAssemblies
		{
			get
			{
				return (bool)base[HostingEnvironmentSection._propShadowCopyBinAssemblies];
			}
			set
			{
				base[HostingEnvironmentSection._propShadowCopyBinAssemblies] = value;
			}
		}

		// Token: 0x0400183A RID: 6202
		internal const int DefaultShutdownTimeout = 30;

		// Token: 0x0400183B RID: 6203
		internal const string sectionName = "system.web/hostingEnvironment";

		// Token: 0x0400183C RID: 6204
		internal static readonly TimeSpan DefaultIdleTimeout = TimeSpan.MaxValue;

		// Token: 0x0400183D RID: 6205
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400183E RID: 6206
		private static readonly ConfigurationProperty _propIdleTimeout = new ConfigurationProperty("idleTimeout", typeof(TimeSpan), HostingEnvironmentSection.DefaultIdleTimeout, StdValidatorsAndConverters.TimeSpanMinutesOrInfiniteConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x0400183F RID: 6207
		private static readonly ConfigurationProperty _propShutdownTimeout = new ConfigurationProperty("shutdownTimeout", typeof(TimeSpan), TimeSpan.FromSeconds(30.0), StdValidatorsAndConverters.TimeSpanSecondsConverter, StdValidatorsAndConverters.PositiveTimeSpanValidator, ConfigurationPropertyOptions.None);

		// Token: 0x04001840 RID: 6208
		private static readonly ConfigurationProperty _propShadowCopyBinAssemblies = new ConfigurationProperty("shadowCopyBinAssemblies", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}

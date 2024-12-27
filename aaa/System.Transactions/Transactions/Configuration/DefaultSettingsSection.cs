using System;
using System.Configuration;
using System.Globalization;

namespace System.Transactions.Configuration
{
	// Token: 0x0200009B RID: 155
	public sealed class DefaultSettingsSection : ConfigurationSection
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0003AE08 File Offset: 0x0003A208
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x0003AE28 File Offset: 0x0003A228
		[ConfigurationProperty("distributedTransactionManagerName", DefaultValue = "")]
		public string DistributedTransactionManagerName
		{
			get
			{
				return (string)base["distributedTransactionManagerName"];
			}
			set
			{
				base["distributedTransactionManagerName"] = value;
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0003AE44 File Offset: 0x0003A244
		internal static DefaultSettingsSection GetSection()
		{
			DefaultSettingsSection defaultSettingsSection = (DefaultSettingsSection)global::System.Configuration.PrivilegedConfigurationManager.GetSection(ConfigurationStrings.DefaultSettingsSectionPath);
			if (defaultSettingsSection == null)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, SR.GetString("ConfigurationSectionNotFound"), new object[] { ConfigurationStrings.DefaultSettingsSectionPath }));
			}
			return defaultSettingsSection;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0003AE90 File Offset: 0x0003A290
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0003AEB0 File Offset: 0x0003A2B0
		[ConfigurationProperty("timeout", DefaultValue = "00:01:00")]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan Timeout
		{
			get
			{
				return (TimeSpan)base["timeout"];
			}
			set
			{
				if (!ConfigurationStrings.IsValidTimeSpan(value))
				{
					throw new ArgumentOutOfRangeException("Timeout", SR.GetString("ConfigInvalidTimeSpanValue"));
				}
				base["timeout"] = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0003AEEC File Offset: 0x0003A2EC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return new ConfigurationPropertyCollection
				{
					new ConfigurationProperty("distributedTransactionManagerName", typeof(string), "", ConfigurationPropertyOptions.None),
					new ConfigurationProperty("timeout", typeof(TimeSpan), "00:01:00", null, new TimeSpanValidator(TimeSpan.Zero, TimeSpan.MaxValue), ConfigurationPropertyOptions.None)
				};
			}
		}
	}
}

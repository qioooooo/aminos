using System;
using System.Configuration;
using System.Globalization;

namespace System.Transactions.Configuration
{
	// Token: 0x0200009C RID: 156
	public sealed class MachineSettingsSection : ConfigurationSection
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x0003AF64 File Offset: 0x0003A364
		internal static MachineSettingsSection GetSection()
		{
			MachineSettingsSection machineSettingsSection = (MachineSettingsSection)global::System.Configuration.PrivilegedConfigurationManager.GetSection(ConfigurationStrings.MachineSettingsSectionPath);
			if (machineSettingsSection == null)
			{
				throw new ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, SR.GetString("ConfigurationSectionNotFound"), new object[] { ConfigurationStrings.MachineSettingsSectionPath }));
			}
			return machineSettingsSection;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0003AFB0 File Offset: 0x0003A3B0
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0003AFD0 File Offset: 0x0003A3D0
		[ConfigurationProperty("maxTimeout", DefaultValue = "00:10:00")]
		[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "10675199.02:48:05.4775807")]
		public TimeSpan MaxTimeout
		{
			get
			{
				return (TimeSpan)base["maxTimeout"];
			}
			set
			{
				if (!ConfigurationStrings.IsValidTimeSpan(value))
				{
					throw new ArgumentOutOfRangeException("MaxTimeout", SR.GetString("ConfigInvalidTimeSpanValue"));
				}
				base["maxTimeout"] = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0003B00C File Offset: 0x0003A40C
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return new ConfigurationPropertyCollection
				{
					new ConfigurationProperty("maxTimeout", typeof(TimeSpan), "00:10:00", null, new TimeSpanValidator(TimeSpan.Zero, TimeSpan.MaxValue), ConfigurationPropertyOptions.None)
				};
			}
		}
	}
}

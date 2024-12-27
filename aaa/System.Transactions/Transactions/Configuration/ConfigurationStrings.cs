using System;
using System.Globalization;

namespace System.Transactions.Configuration
{
	// Token: 0x0200009A RID: 154
	internal static class ConfigurationStrings
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0003AD7C File Offset: 0x0003A17C
		internal static string DefaultSettingsSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("defaultSettings");
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0003AD94 File Offset: 0x0003A194
		internal static string GetSectionPath(string sectionName)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[] { "system.transactions", sectionName });
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0003ADC4 File Offset: 0x0003A1C4
		internal static bool IsValidTimeSpan(TimeSpan span)
		{
			return span >= TimeSpan.Zero;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0003ADDC File Offset: 0x0003A1DC
		internal static string MachineSettingsSectionPath
		{
			get
			{
				return ConfigurationStrings.GetSectionPath("machineSettings");
			}
		}

		// Token: 0x04000245 RID: 581
		internal const string DefaultDistributedTransactionManagerName = "";

		// Token: 0x04000246 RID: 582
		internal const string DefaultMaxTimeout = "00:10:00";

		// Token: 0x04000247 RID: 583
		internal const string DefaultTimeout = "00:01:00";

		// Token: 0x04000248 RID: 584
		internal const string TimeSpanZero = "00:00:00";

		// Token: 0x04000249 RID: 585
		internal const string DefaultSettingsSectionName = "defaultSettings";

		// Token: 0x0400024A RID: 586
		internal const string DistributedTransactionManagerName = "distributedTransactionManagerName";

		// Token: 0x0400024B RID: 587
		internal const string MaxTimeout = "maxTimeout";

		// Token: 0x0400024C RID: 588
		internal const string MachineSettingsSectionName = "machineSettings";

		// Token: 0x0400024D RID: 589
		internal const string SectionGroupName = "system.transactions";

		// Token: 0x0400024E RID: 590
		internal const string Timeout = "timeout";
	}
}

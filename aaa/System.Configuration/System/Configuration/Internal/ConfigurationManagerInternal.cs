using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000B9 RID: 185
	internal sealed class ConfigurationManagerInternal : IConfigurationManagerInternal
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x0001F7D9 File Offset: 0x0001E7D9
		private ConfigurationManagerInternal()
		{
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001F7E1 File Offset: 0x0001E7E1
		bool IConfigurationManagerInternal.SupportsUserConfig
		{
			get
			{
				return ConfigurationManager.SupportsUserConfig;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001F7E8 File Offset: 0x0001E7E8
		bool IConfigurationManagerInternal.SetConfigurationSystemInProgress
		{
			get
			{
				return ConfigurationManager.SetConfigurationSystemInProgress;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0001F7EF File Offset: 0x0001E7EF
		string IConfigurationManagerInternal.MachineConfigPath
		{
			get
			{
				return ClientConfigurationHost.MachineConfigFilePath;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001F7F6 File Offset: 0x0001E7F6
		string IConfigurationManagerInternal.ApplicationConfigUri
		{
			get
			{
				return ClientConfigPaths.Current.ApplicationConfigUri;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001F802 File Offset: 0x0001E802
		string IConfigurationManagerInternal.ExeProductName
		{
			get
			{
				return ClientConfigPaths.Current.ProductName;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001F80E File Offset: 0x0001E80E
		string IConfigurationManagerInternal.ExeProductVersion
		{
			get
			{
				return ClientConfigPaths.Current.ProductVersion;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001F81A File Offset: 0x0001E81A
		string IConfigurationManagerInternal.ExeRoamingConfigDirectory
		{
			get
			{
				return ClientConfigPaths.Current.RoamingConfigDirectory;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001F826 File Offset: 0x0001E826
		string IConfigurationManagerInternal.ExeRoamingConfigPath
		{
			get
			{
				return ClientConfigPaths.Current.RoamingConfigFilename;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001F832 File Offset: 0x0001E832
		string IConfigurationManagerInternal.ExeLocalConfigDirectory
		{
			get
			{
				return ClientConfigPaths.Current.LocalConfigDirectory;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001F83E File Offset: 0x0001E83E
		string IConfigurationManagerInternal.ExeLocalConfigPath
		{
			get
			{
				return ClientConfigPaths.Current.LocalConfigFilename;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0001F84A File Offset: 0x0001E84A
		string IConfigurationManagerInternal.UserConfigFilename
		{
			get
			{
				return "user.config";
			}
		}
	}
}

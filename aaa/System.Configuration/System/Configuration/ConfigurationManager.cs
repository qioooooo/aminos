using System;
using System.Collections.Specialized;
using System.Configuration.Internal;

namespace System.Configuration
{
	// Token: 0x02000032 RID: 50
	public static class ConfigurationManager
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0001000E File Offset: 0x0000F00E
		internal static bool SetConfigurationSystemInProgress
		{
			get
			{
				return ConfigurationManager.InitState.NotStarted < ConfigurationManager.s_initState && ConfigurationManager.s_initState < ConfigurationManager.InitState.Completed;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00010024 File Offset: 0x0000F024
		internal static void SetConfigurationSystem(IInternalConfigSystem configSystem, bool initComplete)
		{
			lock (ConfigurationManager.s_initLock)
			{
				if (ConfigurationManager.s_initState != ConfigurationManager.InitState.NotStarted)
				{
					throw new InvalidOperationException(SR.GetString("Config_system_already_set"));
				}
				ConfigurationManager.s_configSystem = configSystem;
				if (initComplete)
				{
					ConfigurationManager.s_initState = ConfigurationManager.InitState.Completed;
				}
				else
				{
					ConfigurationManager.s_initState = ConfigurationManager.InitState.Usable;
				}
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00010084 File Offset: 0x0000F084
		private static void EnsureConfigurationSystem()
		{
			lock (ConfigurationManager.s_initLock)
			{
				if (ConfigurationManager.s_initState < ConfigurationManager.InitState.Usable)
				{
					ConfigurationManager.s_initState = ConfigurationManager.InitState.Started;
					try
					{
						try
						{
							ConfigurationManager.s_configSystem = new ClientConfigurationSystem();
							ConfigurationManager.s_initState = ConfigurationManager.InitState.Usable;
						}
						catch (Exception ex)
						{
							ConfigurationManager.s_initError = new ConfigurationErrorsException(SR.GetString("Config_client_config_init_error"), ex);
							throw ConfigurationManager.s_initError;
						}
						catch
						{
							ConfigurationManager.s_initError = new ConfigurationErrorsException(SR.GetString("Config_client_config_init_error"));
							throw ConfigurationManager.s_initError;
						}
					}
					catch
					{
						ConfigurationManager.s_initState = ConfigurationManager.InitState.Completed;
						throw;
					}
				}
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0001013C File Offset: 0x0000F13C
		internal static void SetInitError(Exception initError)
		{
			ConfigurationManager.s_initError = initError;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00010144 File Offset: 0x0000F144
		internal static void CompleteConfigInit()
		{
			lock (ConfigurationManager.s_initLock)
			{
				ConfigurationManager.s_initState = ConfigurationManager.InitState.Completed;
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0001017C File Offset: 0x0000F17C
		private static void PrepareConfigSystem()
		{
			if (ConfigurationManager.s_initState < ConfigurationManager.InitState.Usable)
			{
				ConfigurationManager.EnsureConfigurationSystem();
			}
			if (ConfigurationManager.s_initError != null)
			{
				throw ConfigurationManager.s_initError;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600027C RID: 636 RVA: 0x00010198 File Offset: 0x0000F198
		internal static bool SupportsUserConfig
		{
			get
			{
				ConfigurationManager.PrepareConfigSystem();
				return ConfigurationManager.s_configSystem.SupportsUserConfig;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600027D RID: 637 RVA: 0x000101AC File Offset: 0x0000F1AC
		public static NameValueCollection AppSettings
		{
			get
			{
				object section = ConfigurationManager.GetSection("appSettings");
				if (section == null || !(section is NameValueCollection))
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_appsettings_declaration_invalid"));
				}
				return (NameValueCollection)section;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600027E RID: 638 RVA: 0x000101E8 File Offset: 0x0000F1E8
		public static ConnectionStringSettingsCollection ConnectionStrings
		{
			get
			{
				object section = ConfigurationManager.GetSection("connectionStrings");
				if (section == null || section.GetType() != typeof(ConnectionStringsSection))
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_connectionstrings_declaration_invalid"));
				}
				ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)section;
				return connectionStringsSection.ConnectionStrings;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00010234 File Offset: 0x0000F234
		public static object GetSection(string sectionName)
		{
			if (string.IsNullOrEmpty(sectionName))
			{
				return null;
			}
			ConfigurationManager.PrepareConfigSystem();
			return ConfigurationManager.s_configSystem.GetSection(sectionName);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0001025D File Offset: 0x0000F25D
		public static void RefreshSection(string sectionName)
		{
			if (string.IsNullOrEmpty(sectionName))
			{
				return;
			}
			ConfigurationManager.PrepareConfigSystem();
			ConfigurationManager.s_configSystem.RefreshConfig(sectionName);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00010278 File Offset: 0x0000F278
		public static Configuration OpenMachineConfiguration()
		{
			return ConfigurationManager.OpenExeConfigurationImpl(null, true, ConfigurationUserLevel.None, null);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00010283 File Offset: 0x0000F283
		public static Configuration OpenMappedMachineConfiguration(ConfigurationFileMap fileMap)
		{
			return ConfigurationManager.OpenExeConfigurationImpl(fileMap, true, ConfigurationUserLevel.None, null);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0001028E File Offset: 0x0000F28E
		public static Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
		{
			return ConfigurationManager.OpenExeConfigurationImpl(null, false, userLevel, null);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00010299 File Offset: 0x0000F299
		public static Configuration OpenExeConfiguration(string exePath)
		{
			return ConfigurationManager.OpenExeConfigurationImpl(null, false, ConfigurationUserLevel.None, exePath);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000102A4 File Offset: 0x0000F2A4
		public static Configuration OpenMappedExeConfiguration(ExeConfigurationFileMap fileMap, ConfigurationUserLevel userLevel)
		{
			return ConfigurationManager.OpenExeConfigurationImpl(fileMap, false, userLevel, null);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000102B0 File Offset: 0x0000F2B0
		private static Configuration OpenExeConfigurationImpl(ConfigurationFileMap fileMap, bool isMachine, ConfigurationUserLevel userLevel, string exePath)
		{
			if (!isMachine && ((fileMap == null && exePath == null) || (fileMap != null && ((ExeConfigurationFileMap)fileMap).ExeConfigFilename == null)) && ConfigurationManager.s_configSystem != null && ConfigurationManager.s_configSystem.GetType() != typeof(ClientConfigurationSystem))
			{
				throw new ArgumentException(SR.GetString("Config_configmanager_open_noexe"));
			}
			return ClientConfigurationHost.OpenExeConfiguration(fileMap, isMachine, userLevel, exePath);
		}

		// Token: 0x0400026B RID: 619
		private static IInternalConfigSystem s_configSystem;

		// Token: 0x0400026C RID: 620
		private static ConfigurationManager.InitState s_initState = ConfigurationManager.InitState.NotStarted;

		// Token: 0x0400026D RID: 621
		private static object s_initLock = new object();

		// Token: 0x0400026E RID: 622
		private static Exception s_initError;

		// Token: 0x02000033 RID: 51
		private enum InitState
		{
			// Token: 0x04000270 RID: 624
			NotStarted,
			// Token: 0x04000271 RID: 625
			Started,
			// Token: 0x04000272 RID: 626
			Usable,
			// Token: 0x04000273 RID: 627
			Completed
		}
	}
}

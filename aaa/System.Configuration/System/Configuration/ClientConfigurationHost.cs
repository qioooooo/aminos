using System;
using System.Configuration.Internal;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Principal;

namespace System.Configuration
{
	// Token: 0x0200001B RID: 27
	internal sealed class ClientConfigurationHost : DelegatingConfigHost, IInternalConfigClientHost
	{
		// Token: 0x0600017C RID: 380 RVA: 0x0000B8BA File Offset: 0x0000A8BA
		internal ClientConfigurationHost()
		{
			base.Host = new InternalConfigHost();
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000B8CD File Offset: 0x0000A8CD
		internal ClientConfigPaths ConfigPaths
		{
			get
			{
				if (this._configPaths == null)
				{
					this._configPaths = ClientConfigPaths.GetPaths(this._exePath, this._initComplete);
				}
				return this._configPaths;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000B8F4 File Offset: 0x0000A8F4
		internal void RefreshConfigPaths()
		{
			if (this._configPaths != null && !this._configPaths.HasEntryAssembly && this._exePath == null)
			{
				ClientConfigPaths.RefreshCurrent();
				this._configPaths = null;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000B920 File Offset: 0x0000A920
		internal static string MachineConfigFilePath
		{
			[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
			get
			{
				if (ClientConfigurationHost.s_machineConfigFilePath == null)
				{
					string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
					ClientConfigurationHost.s_machineConfigFilePath = Path.Combine(Path.Combine(runtimeDirectory, "Config"), "machine.config");
				}
				return ClientConfigurationHost.s_machineConfigFilePath;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000B959 File Offset: 0x0000A959
		internal bool HasRoamingConfig
		{
			get
			{
				if (this._fileMap != null)
				{
					return !string.IsNullOrEmpty(this._fileMap.RoamingUserConfigFilename);
				}
				return this.ConfigPaths.HasRoamingConfig;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000B982 File Offset: 0x0000A982
		internal bool HasLocalConfig
		{
			get
			{
				if (this._fileMap != null)
				{
					return !string.IsNullOrEmpty(this._fileMap.LocalUserConfigFilename);
				}
				return this.ConfigPaths.HasLocalConfig;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000B9AB File Offset: 0x0000A9AB
		internal bool IsAppConfigHttp
		{
			get
			{
				return !this.IsFile(this.GetStreamName("MACHINE/EXE"));
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000B9C1 File Offset: 0x0000A9C1
		bool IInternalConfigClientHost.IsExeConfig(string configPath)
		{
			return StringUtil.EqualsIgnoreCase(configPath, "MACHINE/EXE");
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000B9CE File Offset: 0x0000A9CE
		bool IInternalConfigClientHost.IsRoamingUserConfig(string configPath)
		{
			return StringUtil.EqualsIgnoreCase(configPath, "MACHINE/EXE/ROAMING_USER");
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000B9DB File Offset: 0x0000A9DB
		bool IInternalConfigClientHost.IsLocalUserConfig(string configPath)
		{
			return StringUtil.EqualsIgnoreCase(configPath, "MACHINE/EXE/ROAMING_USER/LOCAL_USER");
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000B9E8 File Offset: 0x0000A9E8
		private bool IsUserConfig(string configPath)
		{
			return StringUtil.EqualsIgnoreCase(configPath, "MACHINE/EXE/ROAMING_USER") || StringUtil.EqualsIgnoreCase(configPath, "MACHINE/EXE/ROAMING_USER/LOCAL_USER");
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000BA04 File Offset: 0x0000AA04
		string IInternalConfigClientHost.GetExeConfigPath()
		{
			return "MACHINE/EXE";
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000BA0B File Offset: 0x0000AA0B
		string IInternalConfigClientHost.GetRoamingUserConfigPath()
		{
			return "MACHINE/EXE/ROAMING_USER";
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000BA12 File Offset: 0x0000AA12
		string IInternalConfigClientHost.GetLocalUserConfigPath()
		{
			return "MACHINE/EXE/ROAMING_USER/LOCAL_USER";
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000BA1C File Offset: 0x0000AA1C
		public override void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
		{
			try
			{
				ConfigurationFileMap configurationFileMap = (ConfigurationFileMap)hostInitParams[0];
				this._exePath = (string)hostInitParams[1];
				base.Host.Init(configRoot, hostInitParams);
				this._initComplete = configRoot.IsDesignTime;
				if (configurationFileMap != null && !string.IsNullOrEmpty(this._exePath))
				{
					throw ExceptionUtil.UnexpectedError("ClientConfigurationHost::Init");
				}
				if (string.IsNullOrEmpty(this._exePath))
				{
					this._exePath = null;
				}
				if (configurationFileMap != null)
				{
					this._fileMap = new ExeConfigurationFileMap();
					if (!string.IsNullOrEmpty(configurationFileMap.MachineConfigFilename))
					{
						this._fileMap.MachineConfigFilename = Path.GetFullPath(configurationFileMap.MachineConfigFilename);
					}
					ExeConfigurationFileMap exeConfigurationFileMap = configurationFileMap as ExeConfigurationFileMap;
					if (exeConfigurationFileMap != null)
					{
						if (!string.IsNullOrEmpty(exeConfigurationFileMap.ExeConfigFilename))
						{
							this._fileMap.ExeConfigFilename = Path.GetFullPath(exeConfigurationFileMap.ExeConfigFilename);
						}
						if (!string.IsNullOrEmpty(exeConfigurationFileMap.RoamingUserConfigFilename))
						{
							this._fileMap.RoamingUserConfigFilename = Path.GetFullPath(exeConfigurationFileMap.RoamingUserConfigFilename);
						}
						if (!string.IsNullOrEmpty(exeConfigurationFileMap.LocalUserConfigFilename))
						{
							this._fileMap.LocalUserConfigFilename = Path.GetFullPath(exeConfigurationFileMap.LocalUserConfigFilename);
						}
					}
				}
			}
			catch (SecurityException)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_client_config_init_security"));
			}
			catch
			{
				throw ExceptionUtil.UnexpectedError("ClientConfigurationHost::Init");
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000BB84 File Offset: 0x0000AB84
		public override void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
		{
			locationSubPath = null;
			configPath = (string)hostInitConfigurationParams[2];
			locationConfigPath = null;
			this.Init(configRoot, hostInitConfigurationParams);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000BBA1 File Offset: 0x0000ABA1
		public override bool IsInitDelayed(IInternalConfigRecord configRecord)
		{
			return !this._initComplete && this.IsUserConfig(configRecord.ConfigPath);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000BBBC File Offset: 0x0000ABBC
		public override void RequireCompleteInit(IInternalConfigRecord record)
		{
			lock (this)
			{
				if (!this._initComplete)
				{
					this._initComplete = true;
					ClientConfigPaths.RefreshCurrent();
					this._configPaths = null;
					ClientConfigPaths configPaths = this.ConfigPaths;
				}
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000BC0C File Offset: 0x0000AC0C
		public override bool IsConfigRecordRequired(string configPath)
		{
			string name = ConfigPathUtility.GetName(configPath);
			string text;
			if ((text = name) != null)
			{
				if (text == "MACHINE" || text == "EXE")
				{
					return true;
				}
				if (text == "ROAMING_USER")
				{
					return this.HasRoamingConfig || this.HasLocalConfig;
				}
				if (text == "LOCAL_USER")
				{
					return this.HasLocalConfig;
				}
			}
			return false;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000BC74 File Offset: 0x0000AC74
		public override string GetStreamName(string configPath)
		{
			string name = ConfigPathUtility.GetName(configPath);
			if (this._fileMap != null)
			{
				string text;
				if ((text = name) != null && !(text == "MACHINE"))
				{
					if (text == "EXE")
					{
						return this._fileMap.ExeConfigFilename;
					}
					if (text == "ROAMING_USER")
					{
						return this._fileMap.RoamingUserConfigFilename;
					}
					if (text == "LOCAL_USER")
					{
						return this._fileMap.LocalUserConfigFilename;
					}
				}
				return this._fileMap.MachineConfigFilename;
			}
			string text2;
			if ((text2 = name) != null && !(text2 == "MACHINE"))
			{
				if (text2 == "EXE")
				{
					return this.ConfigPaths.ApplicationConfigUri;
				}
				if (text2 == "ROAMING_USER")
				{
					return this.ConfigPaths.RoamingConfigFilename;
				}
				if (text2 == "LOCAL_USER")
				{
					return this.ConfigPaths.LocalConfigFilename;
				}
			}
			return ClientConfigurationHost.MachineConfigFilePath;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0000BD5C File Offset: 0x0000AD5C
		public override string GetStreamNameForConfigSource(string streamName, string configSource)
		{
			if (this.IsFile(streamName))
			{
				return base.Host.GetStreamNameForConfigSource(streamName, configSource);
			}
			int num = streamName.LastIndexOf('/');
			if (num < 0)
			{
				return null;
			}
			string text = streamName.Substring(0, num + 1);
			return text + configSource.Replace('\\', '/');
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000BDAC File Offset: 0x0000ADAC
		public override object GetStreamVersion(string streamName)
		{
			if (this.IsFile(streamName))
			{
				return base.Host.GetStreamVersion(streamName);
			}
			return ClientConfigurationHost.s_version;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000BDCC File Offset: 0x0000ADCC
		public override Stream OpenStreamForRead(string streamName)
		{
			if (this.IsFile(streamName))
			{
				return base.Host.OpenStreamForRead(streamName);
			}
			if (streamName == null)
			{
				return null;
			}
			WebClient webClient = new WebClient();
			try
			{
				webClient.Credentials = CredentialCache.DefaultCredentials;
			}
			catch
			{
			}
			byte[] array = null;
			try
			{
				array = webClient.DownloadData(streamName);
			}
			catch
			{
			}
			if (array == null)
			{
				return null;
			}
			return new MemoryStream(array);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000BE44 File Offset: 0x0000AE44
		public override Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
		{
			if (!this.IsFile(streamName))
			{
				throw ExceptionUtil.UnexpectedError("ClientConfigurationHost::OpenStreamForWrite");
			}
			return base.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000BE68 File Offset: 0x0000AE68
		public override void DeleteStream(string streamName)
		{
			if (!this.IsFile(streamName))
			{
				throw ExceptionUtil.UnexpectedError("ClientConfigurationHost::Delete");
			}
			base.Host.DeleteStream(streamName);
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000BE8A File Offset: 0x0000AE8A
		public override bool SupportsRefresh
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000BE8D File Offset: 0x0000AE8D
		public override bool SupportsPath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000BE90 File Offset: 0x0000AE90
		public override bool SupportsLocation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000BE94 File Offset: 0x0000AE94
		public override bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition)
		{
			string text;
			if (allowExeDefinition <= ConfigurationAllowExeDefinition.MachineToApplication)
			{
				if (allowExeDefinition == ConfigurationAllowExeDefinition.MachineOnly)
				{
					text = "MACHINE";
					goto IL_0049;
				}
				if (allowExeDefinition == ConfigurationAllowExeDefinition.MachineToApplication)
				{
					text = "MACHINE/EXE";
					goto IL_0049;
				}
			}
			else
			{
				if (allowExeDefinition == ConfigurationAllowExeDefinition.MachineToRoamingUser)
				{
					text = "MACHINE/EXE/ROAMING_USER";
					goto IL_0049;
				}
				if (allowExeDefinition == ConfigurationAllowExeDefinition.MachineToLocalUser)
				{
					return true;
				}
			}
			throw ExceptionUtil.UnexpectedError("ClientConfigurationHost::IsDefinitionAllowed");
			IL_0049:
			return configPath.Length <= text.Length;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000BEFC File Offset: 0x0000AEFC
		public override void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo)
		{
			if (this.IsDefinitionAllowed(configPath, allowDefinition, allowExeDefinition))
			{
				return;
			}
			if (allowExeDefinition == ConfigurationAllowExeDefinition.MachineOnly)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_allow_exedefinition_error_machine"), errorInfo);
			}
			if (allowExeDefinition == ConfigurationAllowExeDefinition.MachineToApplication)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_allow_exedefinition_error_application"), errorInfo);
			}
			if (allowExeDefinition != ConfigurationAllowExeDefinition.MachineToRoamingUser)
			{
				throw ExceptionUtil.UnexpectedError("ClientConfigurationHost::VerifyDefinitionAllowed");
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_allow_exedefinition_error_roaminguser"), errorInfo);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000BF6A File Offset: 0x0000AF6A
		public override bool PrefetchAll(string configPath, string streamName)
		{
			return !this.IsFile(streamName);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000BF76 File Offset: 0x0000AF76
		public override bool PrefetchSection(string sectionGroupName, string sectionName)
		{
			return sectionGroupName == "system.net";
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000BF83 File Offset: 0x0000AF83
		public override bool IsTrustedConfigPath(string configPath)
		{
			return configPath == "MACHINE";
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000BF90 File Offset: 0x0000AF90
		[SecurityPermission(SecurityAction.Assert, ControlEvidence = true)]
		public override void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady)
		{
			bool flag = this.IsFile(configRecord.StreamName);
			string text;
			if (flag)
			{
				text = UrlPath.ConvertFileNameToUrl(configRecord.StreamName);
			}
			else
			{
				text = configRecord.StreamName;
			}
			Evidence evidence = new Evidence();
			evidence.AddHost(new Url(text));
			evidence.AddHost(Zone.CreateFromUrl(text));
			if (!flag)
			{
				evidence.AddHost(Site.CreateFromUrl(text));
			}
			permissionSet = SecurityManager.ResolvePolicy(evidence);
			isHostReady = true;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000BFFA File Offset: 0x0000AFFA
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		public override IDisposable Impersonate()
		{
			return WindowsIdentity.Impersonate(IntPtr.Zero);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000C006 File Offset: 0x0000B006
		public override object CreateDeprecatedConfigContext(string configPath)
		{
			return null;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C009 File Offset: 0x0000B009
		public override object CreateConfigurationContext(string configPath, string locationSubPath)
		{
			return new ExeContext(this.GetUserLevel(configPath), this.ConfigPaths.ApplicationUri);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000C024 File Offset: 0x0000B024
		private ConfigurationUserLevel GetUserLevel(string configPath)
		{
			string name;
			if ((name = ConfigPathUtility.GetName(configPath)) != null)
			{
				if (name == "MACHINE")
				{
					return ConfigurationUserLevel.None;
				}
				if (name == "EXE")
				{
					return ConfigurationUserLevel.None;
				}
				if (name == "LOCAL_USER")
				{
					return ConfigurationUserLevel.PerUserRoamingAndLocal;
				}
				if (name == "ROAMING_USER")
				{
					return ConfigurationUserLevel.PerUserRoaming;
				}
			}
			return ConfigurationUserLevel.None;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000C088 File Offset: 0x0000B088
		internal static Configuration OpenExeConfiguration(ConfigurationFileMap fileMap, bool isMachine, ConfigurationUserLevel userLevel, string exePath)
		{
			if (userLevel != ConfigurationUserLevel.None && userLevel != ConfigurationUserLevel.PerUserRoaming && userLevel != ConfigurationUserLevel.PerUserRoamingAndLocal)
			{
				throw ExceptionUtil.ParameterInvalid("userLevel");
			}
			if (fileMap != null)
			{
				if (string.IsNullOrEmpty(fileMap.MachineConfigFilename))
				{
					throw ExceptionUtil.ParameterNullOrEmpty("fileMap.MachineConfigFilename");
				}
				ExeConfigurationFileMap exeConfigurationFileMap = fileMap as ExeConfigurationFileMap;
				if (exeConfigurationFileMap != null)
				{
					if (userLevel != ConfigurationUserLevel.None)
					{
						if (userLevel != ConfigurationUserLevel.PerUserRoaming)
						{
							if (userLevel != ConfigurationUserLevel.PerUserRoamingAndLocal)
							{
								goto IL_00A1;
							}
							if (string.IsNullOrEmpty(exeConfigurationFileMap.LocalUserConfigFilename))
							{
								throw ExceptionUtil.ParameterNullOrEmpty("fileMap.LocalUserConfigFilename");
							}
						}
						if (string.IsNullOrEmpty(exeConfigurationFileMap.RoamingUserConfigFilename))
						{
							throw ExceptionUtil.ParameterNullOrEmpty("fileMap.RoamingUserConfigFilename");
						}
					}
					if (string.IsNullOrEmpty(exeConfigurationFileMap.ExeConfigFilename))
					{
						throw ExceptionUtil.ParameterNullOrEmpty("fileMap.ExeConfigFilename");
					}
				}
			}
			IL_00A1:
			string text = null;
			if (isMachine)
			{
				text = "MACHINE";
			}
			else if (userLevel != ConfigurationUserLevel.None)
			{
				if (userLevel != ConfigurationUserLevel.PerUserRoaming)
				{
					if (userLevel == ConfigurationUserLevel.PerUserRoamingAndLocal)
					{
						text = "MACHINE/EXE/ROAMING_USER/LOCAL_USER";
					}
				}
				else
				{
					text = "MACHINE/EXE/ROAMING_USER";
				}
			}
			else
			{
				text = "MACHINE/EXE";
			}
			return new Configuration(null, typeof(ClientConfigurationHost), new object[] { fileMap, exePath, text });
		}

		// Token: 0x040001F2 RID: 498
		internal const string MachineConfigName = "MACHINE";

		// Token: 0x040001F3 RID: 499
		internal const string ExeConfigName = "EXE";

		// Token: 0x040001F4 RID: 500
		internal const string RoamingUserConfigName = "ROAMING_USER";

		// Token: 0x040001F5 RID: 501
		internal const string LocalUserConfigName = "LOCAL_USER";

		// Token: 0x040001F6 RID: 502
		internal const string MachineConfigPath = "MACHINE";

		// Token: 0x040001F7 RID: 503
		internal const string ExeConfigPath = "MACHINE/EXE";

		// Token: 0x040001F8 RID: 504
		internal const string RoamingUserConfigPath = "MACHINE/EXE/ROAMING_USER";

		// Token: 0x040001F9 RID: 505
		internal const string LocalUserConfigPath = "MACHINE/EXE/ROAMING_USER/LOCAL_USER";

		// Token: 0x040001FA RID: 506
		private const string ConfigExtension = ".config";

		// Token: 0x040001FB RID: 507
		private const string MachineConfigFilename = "machine.config";

		// Token: 0x040001FC RID: 508
		private const string MachineConfigSubdirectory = "Config";

		// Token: 0x040001FD RID: 509
		private static object s_init = new object();

		// Token: 0x040001FE RID: 510
		private static object s_version = new object();

		// Token: 0x040001FF RID: 511
		private static string s_machineConfigFilePath;

		// Token: 0x04000200 RID: 512
		private string _exePath;

		// Token: 0x04000201 RID: 513
		private ClientConfigPaths _configPaths;

		// Token: 0x04000202 RID: 514
		private ExeConfigurationFileMap _fileMap;

		// Token: 0x04000203 RID: 515
		private bool _initComplete;
	}
}

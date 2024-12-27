using System;
using System.Configuration.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.Hosting;

namespace System.Web.Configuration
{
	// Token: 0x020001F7 RID: 503
	internal class HttpConfigurationSystem : IInternalConfigSystem
	{
		// Token: 0x06001B66 RID: 7014 RVA: 0x0007F180 File Offset: 0x0007E180
		private HttpConfigurationSystem()
		{
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x0007F188 File Offset: 0x0007E188
		internal static void EnsureInit(IConfigMapPath configMapPath, bool listenToFileChanges, bool initComplete)
		{
			if (!HttpConfigurationSystem.s_inited)
			{
				lock (HttpConfigurationSystem.s_initLock)
				{
					if (!HttpConfigurationSystem.s_inited)
					{
						HttpConfigurationSystem.s_initComplete = initComplete;
						if (configMapPath == null)
						{
							configMapPath = IISMapPath.GetInstance();
						}
						HttpConfigurationSystem.s_configMapPath = configMapPath;
						Type type = Type.GetType("System.Configuration.Internal.ConfigSystem, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
						HttpConfigurationSystem.s_configSystem = (IConfigSystem)Activator.CreateInstance(type, true);
						HttpConfigurationSystem.s_configSystem.Init(typeof(WebConfigurationHost), new object[]
						{
							true,
							HttpConfigurationSystem.s_configMapPath,
							null,
							HostingEnvironment.ApplicationVirtualPath,
							HostingEnvironment.SiteNameNoDemand,
							HostingEnvironment.SiteID
						});
						HttpConfigurationSystem.s_configRoot = HttpConfigurationSystem.s_configSystem.Root;
						HttpConfigurationSystem.s_configHost = (WebConfigurationHost)HttpConfigurationSystem.s_configSystem.Host;
						HttpConfigurationSystem httpConfigurationSystem = new HttpConfigurationSystem();
						if (listenToFileChanges)
						{
							HttpConfigurationSystem.s_configRoot.ConfigChanged += httpConfigurationSystem.OnConfigurationChanged;
						}
						Type type2 = Type.GetType("System.Configuration.Internal.InternalConfigSettingsFactory, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
						HttpConfigurationSystem.s_configSettingsFactory = (IInternalConfigSettingsFactory)Activator.CreateInstance(type2, true);
						HttpConfigurationSystem.s_configSettingsFactory.SetConfigurationSystem(httpConfigurationSystem, initComplete);
						HttpConfigurationSystem.s_httpConfigSystem = httpConfigurationSystem;
						HttpConfigurationSystem.s_inited = true;
					}
				}
			}
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x0007F2D8 File Offset: 0x0007E2D8
		internal static void CompleteInit()
		{
			HttpConfigurationSystem.s_configSettingsFactory.CompleteInit();
			HttpConfigurationSystem.s_configSettingsFactory = null;
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001B69 RID: 7017 RVA: 0x0007F2EC File Offset: 0x0007E2EC
		internal static bool UseHttpConfigurationSystem
		{
			get
			{
				if (!HttpConfigurationSystem.s_inited)
				{
					lock (HttpConfigurationSystem.s_initLock)
					{
						if (!HttpConfigurationSystem.s_inited)
						{
							HttpConfigurationSystem.s_inited = true;
						}
					}
				}
				return HttpConfigurationSystem.s_httpConfigSystem != null;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001B6A RID: 7018 RVA: 0x0007F344 File Offset: 0x0007E344
		internal static bool IsSet
		{
			get
			{
				return HttpConfigurationSystem.s_httpConfigSystem != null;
			}
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x0007F351 File Offset: 0x0007E351
		object IInternalConfigSystem.GetSection(string configKey)
		{
			return HttpConfigurationSystem.GetSection(configKey);
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x0007F359 File Offset: 0x0007E359
		void IInternalConfigSystem.RefreshConfig(string sectionName)
		{
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x0007F35B File Offset: 0x0007E35B
		bool IInternalConfigSystem.SupportsUserConfig
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0007F360 File Offset: 0x0007E360
		internal static object GetSection(string sectionName)
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				return httpContext.GetSection(sectionName);
			}
			return HttpConfigurationSystem.GetApplicationSection(sectionName);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0007F384 File Offset: 0x0007E384
		internal static object GetSection(string sectionName, VirtualPath path)
		{
			CachedPathData virtualPathData = CachedPathData.GetVirtualPathData(path, true);
			return virtualPathData.ConfigRecord.GetSection(sectionName);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0007F3A5 File Offset: 0x0007E3A5
		internal static object GetSection(string sectionName, string path)
		{
			return HttpConfigurationSystem.GetSection(sectionName, VirtualPath.CreateNonRelativeAllowNull(path));
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0007F3B4 File Offset: 0x0007E3B4
		internal static object GetApplicationSection(string sectionName)
		{
			CachedPathData applicationPathData = CachedPathData.GetApplicationPathData();
			return applicationPathData.ConfigRecord.GetSection(sectionName);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0007F3D4 File Offset: 0x0007E3D4
		internal static IInternalConfigRecord GetUniqueConfigRecord(string configPath)
		{
			if (!HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				return null;
			}
			return HttpConfigurationSystem.s_configRoot.GetUniqueConfigRecord(configPath);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0007F3F7 File Offset: 0x0007E3F7
		internal static void AddFileDependency(string file)
		{
			if (string.IsNullOrEmpty(file))
			{
				return;
			}
			if (HttpConfigurationSystem.UseHttpConfigurationSystem)
			{
				if (HttpConfigurationSystem.s_fileChangeEventHandler == null)
				{
					HttpConfigurationSystem.s_fileChangeEventHandler = new FileChangeEventHandler(HttpConfigurationSystem.s_httpConfigSystem.OnConfigFileChanged);
				}
				HttpRuntime.FileChangesMonitor.StartMonitoringFile(file, HttpConfigurationSystem.s_fileChangeEventHandler);
			}
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0007F436 File Offset: 0x0007E436
		internal void OnConfigurationChanged(object sender, InternalConfigEventArgs e)
		{
			HttpRuntime.OnConfigChange();
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0007F43D File Offset: 0x0007E43D
		internal void OnConfigFileChanged(object sender, FileChangeEvent e)
		{
			HttpRuntime.OnConfigChange();
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x0007F444 File Offset: 0x0007E444
		internal static string MsCorLibDirectory
		{
			[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
			get
			{
				if (HttpConfigurationSystem.s_MsCorLibDirectory == null)
				{
					HttpConfigurationSystem.s_MsCorLibDirectory = RuntimeEnvironment.GetRuntimeDirectory();
				}
				return HttpConfigurationSystem.s_MsCorLibDirectory;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001B77 RID: 7031 RVA: 0x0007F45C File Offset: 0x0007E45C
		internal static string MachineConfigurationDirectory
		{
			get
			{
				if (HttpConfigurationSystem.s_MachineConfigurationDirectory == null)
				{
					HttpConfigurationSystem.s_MachineConfigurationDirectory = Path.Combine(HttpConfigurationSystem.MsCorLibDirectory, "Config");
				}
				return HttpConfigurationSystem.s_MachineConfigurationDirectory;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x0007F47E File Offset: 0x0007E47E
		internal static string MachineConfigurationFilePath
		{
			get
			{
				if (HttpConfigurationSystem.s_MachineConfigurationFilePath == null)
				{
					HttpConfigurationSystem.s_MachineConfigurationFilePath = Path.Combine(HttpConfigurationSystem.MachineConfigurationDirectory, "machine.config");
				}
				return HttpConfigurationSystem.s_MachineConfigurationFilePath;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x0007F4A0 File Offset: 0x0007E4A0
		// (set) Token: 0x06001B7A RID: 7034 RVA: 0x0007F4C2 File Offset: 0x0007E4C2
		internal static string RootWebConfigurationFilePath
		{
			get
			{
				if (HttpConfigurationSystem.s_RootWebConfigurationFilePath == null)
				{
					HttpConfigurationSystem.s_RootWebConfigurationFilePath = Path.Combine(HttpConfigurationSystem.MachineConfigurationDirectory, "web.config");
				}
				return HttpConfigurationSystem.s_RootWebConfigurationFilePath;
			}
			set
			{
				HttpConfigurationSystem.s_RootWebConfigurationFilePath = value;
				if (HttpConfigurationSystem.s_RootWebConfigurationFilePath == null)
				{
					HttpConfigurationSystem.s_RootWebConfigurationFilePath = Path.Combine(HttpConfigurationSystem.MachineConfigurationDirectory, "web.config");
				}
			}
		}

		// Token: 0x04001856 RID: 6230
		private const string InternalConfigSettingsFactoryTypeString = "System.Configuration.Internal.InternalConfigSettingsFactory, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x04001857 RID: 6231
		internal const string ConfigSystemTypeString = "System.Configuration.Internal.ConfigSystem, System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

		// Token: 0x04001858 RID: 6232
		internal const string MachineConfigSubdirectory = "Config";

		// Token: 0x04001859 RID: 6233
		internal const string MachineConfigFilename = "machine.config";

		// Token: 0x0400185A RID: 6234
		internal const string RootWebConfigFilename = "web.config";

		// Token: 0x0400185B RID: 6235
		internal const string WebConfigFileName = "web.config";

		// Token: 0x0400185C RID: 6236
		internal const string InetsrvDirectoryName = "inetsrv";

		// Token: 0x0400185D RID: 6237
		internal const string ApplicationHostConfigFileName = "applicationHost.config";

		// Token: 0x0400185E RID: 6238
		private static object s_initLock = new object();

		// Token: 0x0400185F RID: 6239
		private static volatile bool s_inited;

		// Token: 0x04001860 RID: 6240
		private static HttpConfigurationSystem s_httpConfigSystem;

		// Token: 0x04001861 RID: 6241
		private static IConfigSystem s_configSystem;

		// Token: 0x04001862 RID: 6242
		private static IConfigMapPath s_configMapPath;

		// Token: 0x04001863 RID: 6243
		private static WebConfigurationHost s_configHost;

		// Token: 0x04001864 RID: 6244
		private static FileChangeEventHandler s_fileChangeEventHandler;

		// Token: 0x04001865 RID: 6245
		private static string s_MsCorLibDirectory;

		// Token: 0x04001866 RID: 6246
		private static string s_MachineConfigurationDirectory;

		// Token: 0x04001867 RID: 6247
		private static string s_MachineConfigurationFilePath;

		// Token: 0x04001868 RID: 6248
		private static string s_RootWebConfigurationFilePath;

		// Token: 0x04001869 RID: 6249
		private static IInternalConfigRoot s_configRoot;

		// Token: 0x0400186A RID: 6250
		private static IInternalConfigSettingsFactory s_configSettingsFactory;

		// Token: 0x0400186B RID: 6251
		private static bool s_initComplete;
	}
}

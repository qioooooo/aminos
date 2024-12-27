using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Profile;
using System.Web.UI;
using System.Web.Util;
using System.Xml;

namespace System.Web.Compilation
{
	// Token: 0x02000133 RID: 307
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Medium)]
	public sealed class BuildManager
	{
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0003FF3B File Offset: 0x0003EF3B
		internal static BuildManager TheBuildManager
		{
			get
			{
				return BuildManager._theBuildManager;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000E19 RID: 3609 RVA: 0x0003FF42 File Offset: 0x0003EF42
		internal static bool OptimizeCompilations
		{
			get
			{
				return BuildManager._theBuildManager._optimizeCompilations;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0003FF4E File Offset: 0x0003EF4E
		internal static string WebHashFilePath
		{
			get
			{
				return BuildManager._theBuildManager._webHashFilePath;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0003FF5A File Offset: 0x0003EF5A
		internal static CompilationStage CompilationStage
		{
			get
			{
				return BuildManager._theBuildManager._compilationStage;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0003FF66 File Offset: 0x0003EF66
		internal static VirtualPath ScriptVirtualDir
		{
			get
			{
				return BuildManager._theBuildManager._scriptVirtualDir;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0003FF72 File Offset: 0x0003EF72
		internal static VirtualPath GlobalAsaxVirtualPath
		{
			get
			{
				return BuildManager._theBuildManager._globalAsaxVirtualPath;
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0003FF7E File Offset: 0x0003EF7E
		private BuildManager()
		{
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0003FF98 File Offset: 0x0003EF98
		internal static bool InitializeBuildManager()
		{
			if (BuildManager._initializeException != null)
			{
				throw new HttpException(BuildManager._initializeException.Message, BuildManager._initializeException);
			}
			if (!BuildManager._theBuildManagerInitialized)
			{
				if (!HttpRuntime.FusionInited)
				{
					return false;
				}
				if (HttpRuntime.TrustLevel == null)
				{
					return false;
				}
				BuildManager._theBuildManagerInitialized = true;
				try
				{
					BuildManager._theBuildManager.Initialize();
				}
				catch (Exception ex)
				{
					BuildManager._theBuildManagerInitialized = false;
					BuildManager._initializeException = ex;
					throw;
				}
			}
			return true;
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0004000C File Offset: 0x0003F00C
		internal static ClientBuildManagerCallback CBMCallback
		{
			get
			{
				return BuildManager._theBuildManager._cbmCallback;
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00040018 File Offset: 0x0003F018
		internal static void ReportParseError(ParserError parseError)
		{
			if (BuildManager.CBMCallback != null)
			{
				BuildManager._parseErrorReported = true;
				BuildManager.CBMCallback.ReportParseError(parseError);
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00040032 File Offset: 0x0003F032
		private void ReportTopLevelCompilationException()
		{
			this.ReportErrorsFromException(this._topLevelFileCompilationException);
			throw new HttpException(this._topLevelFileCompilationException.Message, this._topLevelFileCompilationException);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00040058 File Offset: 0x0003F058
		private void ReportErrorsFromException(Exception e)
		{
			if (BuildManager.CBMCallback == null)
			{
				return;
			}
			if (e is HttpCompileException)
			{
				CompilerResults results = ((HttpCompileException)e).Results;
				using (IEnumerator enumerator = results.Errors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						CompilerError compilerError = (CompilerError)obj;
						BuildManager.CBMCallback.ReportCompilerError(compilerError);
					}
					return;
				}
			}
			if (e is HttpParseException)
			{
				foreach (object obj2 in ((HttpParseException)e).ParserErrors)
				{
					ParserError parserError = (ParserError)obj2;
					BuildManager.ReportParseError(parserError);
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x00040130 File Offset: 0x0003F130
		private List<Assembly> TopLevelReferencedAssemblies
		{
			get
			{
				return this._topLevelReferencedAssemblies;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x00040138 File Offset: 0x0003F138
		private IDictionary<string, AssemblyReferenceInfo> TopLevelAssembliesIndexTable
		{
			get
			{
				return this._topLevelAssembliesIndexTable;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00040140 File Offset: 0x0003F140
		internal static Dictionary<string, string> GenerateFileTable
		{
			get
			{
				if (BuildManager._theBuildManager._generatedFileTable == null)
				{
					BuildManager._theBuildManager._generatedFileTable = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
				return BuildManager._theBuildManager._generatedFileTable;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0004016C File Offset: 0x0003F16C
		public static IList CodeAssemblies
		{
			get
			{
				BuildManager._theBuildManager.EnsureTopLevelFilesCompiled();
				return BuildManager._theBuildManager._codeAssemblies;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00040182 File Offset: 0x0003F182
		internal static Assembly AppResourcesAssembly
		{
			get
			{
				return BuildManager._theBuildManager._appResourcesAssembly;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0004018E File Offset: 0x0003F18E
		// (set) Token: 0x06000E2A RID: 3626 RVA: 0x0004019A File Offset: 0x0003F19A
		internal static bool ThrowOnFirstParseError
		{
			get
			{
				return BuildManager._theBuildManager._throwOnFirstParseError;
			}
			set
			{
				BuildManager._theBuildManager._throwOnFirstParseError = value;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x000401A7 File Offset: 0x0003F1A7
		// (set) Token: 0x06000E2C RID: 3628 RVA: 0x000401B3 File Offset: 0x0003F1B3
		internal static bool PerformingPrecompilation
		{
			get
			{
				return BuildManager._theBuildManager._performingPrecompilation;
			}
			set
			{
				BuildManager._theBuildManager._performingPrecompilation = value;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x000401C0 File Offset: 0x0003F1C0
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x000401CC File Offset: 0x0003F1CC
		internal static bool SkipTopLevelCompilationExceptions
		{
			get
			{
				return BuildManager._theBuildManager._skipTopLevelCompilationExceptions;
			}
			set
			{
				BuildManager._theBuildManager._skipTopLevelCompilationExceptions = value;
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x000401DC File Offset: 0x0003F1DC
		internal static ICollection GetReferencedAssemblies(CompilationSection compConfig, int removeIndex)
		{
			AssemblySet assemblySet = new AssemblySet();
			foreach (object obj in compConfig.Assemblies)
			{
				AssemblyInfo assemblyInfo = (AssemblyInfo)obj;
				Assembly[] array = assemblyInfo.AssemblyInternal;
				if (array == null)
				{
					lock (compConfig)
					{
						array = assemblyInfo.AssemblyInternal;
						if (array == null)
						{
							array = (assemblyInfo.AssemblyInternal = compConfig.LoadAssembly(assemblyInfo));
						}
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						assemblySet.Add(array[i]);
					}
				}
			}
			for (int j = 0; j < removeIndex; j++)
			{
				assemblySet.Add(BuildManager.TheBuildManager.TopLevelReferencedAssemblies[j]);
			}
			return assemblySet;
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x000402C8 File Offset: 0x0003F2C8
		internal static ICollection GetReferencedAssemblies(CompilationSection compConfig)
		{
			AssemblySet assemblySet = AssemblySet.Create(BuildManager.TheBuildManager.TopLevelReferencedAssemblies);
			foreach (object obj in compConfig.Assemblies)
			{
				AssemblyInfo assemblyInfo = (AssemblyInfo)obj;
				Assembly[] array = assemblyInfo.AssemblyInternal;
				if (array == null)
				{
					lock (compConfig)
					{
						array = assemblyInfo.AssemblyInternal;
						if (array == null)
						{
							array = (assemblyInfo.AssemblyInternal = compConfig.LoadAssembly(assemblyInfo));
						}
					}
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						assemblySet.Add(array[i]);
					}
				}
			}
			return assemblySet;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00040394 File Offset: 0x0003F394
		public static ICollection GetReferencedAssemblies()
		{
			RuntimeConfig appConfig = RuntimeConfig.GetAppConfig();
			CompilationSection compilation = appConfig.Compilation;
			BuildManager._theBuildManager.EnsureTopLevelFilesCompiled();
			return BuildManager.GetReferencedAssemblies(compilation);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000403C0 File Offset: 0x0003F3C0
		private void Initialize()
		{
			AppDomain.CurrentDomain.AssemblyResolve += this.ResolveAssembly;
			this._globalAsaxVirtualPath = HttpRuntime.AppDomainAppVirtualPathObject.SimpleCombine("global.asax");
			this._webHashFilePath = Path.Combine(HttpRuntime.CodegenDirInternal, "hash\\hash.web");
			this._skipTopLevelCompilationExceptions = BuildManagerHost.InClientBuildManager;
			this.SetPrecompilationInfo(HostingEnvironment.HostingParameters);
			if (this._precompTargetPhysicalDir != null)
			{
				this.FailIfPrecompiledApp();
				this.PrecompilationModeInitialize();
			}
			else if (BuildManager.IsPrecompiledApp)
			{
				this.PrecompiledAppRuntimeModeInitialize();
			}
			else
			{
				this.RegularAppRuntimeModeInitialize();
			}
			this._scriptVirtualDir = Util.GetScriptLocation();
			this._excludedTopLevelDirectories = new CaseInsensitiveStringSet();
			this._excludedTopLevelDirectories.Add("bin");
			this._excludedTopLevelDirectories.Add("App_Code");
			this._excludedTopLevelDirectories.Add("App_GlobalResources");
			this._excludedTopLevelDirectories.Add("App_LocalResources");
			this._excludedTopLevelDirectories.Add("App_WebReferences");
			this._excludedTopLevelDirectories.Add("App_Themes");
			this._forbiddenTopLevelDirectories = new CaseInsensitiveStringSet();
			this._forbiddenTopLevelDirectories.Add("App_Code");
			this._forbiddenTopLevelDirectories.Add("App_GlobalResources");
			this._forbiddenTopLevelDirectories.Add("App_LocalResources");
			this._forbiddenTopLevelDirectories.Add("App_WebReferences");
			this._forbiddenTopLevelDirectories.Add("App_Themes");
			this.LoadLicensesAssemblyIfExists();
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00040528 File Offset: 0x0003F528
		private void RegularAppRuntimeModeInitialize()
		{
			this._memoryCache = new MemoryBuildResultCache(HttpRuntime.CacheInternal);
			StandardDiskBuildResultCache standardDiskBuildResultCache = new StandardDiskBuildResultCache(HttpRuntime.CodegenDirInternal);
			this._caches = new BuildResultCache[] { this._memoryCache, standardDiskBuildResultCache };
			this.CheckTopLevelFilesUpToDate(standardDiskBuildResultCache);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00040574 File Offset: 0x0003F574
		private void PrecompiledAppRuntimeModeInitialize()
		{
			this._memoryCache = new MemoryBuildResultCache(HttpRuntime.CacheInternal);
			BuildResultCache buildResultCache = new PrecompiledSiteDiskBuildResultCache(HttpRuntime.BinDirectoryInternal);
			StandardDiskBuildResultCache standardDiskBuildResultCache = new StandardDiskBuildResultCache(HttpRuntime.CodegenDirInternal);
			this._caches = new BuildResultCache[] { this._memoryCache, buildResultCache, standardDiskBuildResultCache };
			this.CheckTopLevelFilesUpToDate(standardDiskBuildResultCache);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x000405D0 File Offset: 0x0003F5D0
		private void PrecompilationModeInitialize()
		{
			this._memoryCache = new MemoryBuildResultCache(HttpRuntime.CacheInternal);
			StandardDiskBuildResultCache standardDiskBuildResultCache = new StandardDiskBuildResultCache(HttpRuntime.CodegenDirInternal);
			string text = Path.Combine(this._precompTargetPhysicalDir, "bin");
			BuildResultCache buildResultCache;
			if (BuildManager.PrecompilingForUpdatableDeployment)
			{
				buildResultCache = new UpdatablePrecompilerDiskBuildResultCache(text);
			}
			else
			{
				buildResultCache = new PrecompilerDiskBuildResultCache(text);
			}
			this._caches = new BuildResultCache[] { this._memoryCache, buildResultCache, standardDiskBuildResultCache };
			this.CheckTopLevelFilesUpToDate(standardDiskBuildResultCache);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00040648 File Offset: 0x0003F648
		private void LoadLicensesAssemblyIfExists()
		{
			string text = Path.Combine(HttpRuntime.BinDirectoryInternal, "App_Licenses.dll");
			if (File.Exists(text))
			{
				Assembly.Load("App_Licenses");
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00040678 File Offset: 0x0003F678
		private void CheckTopLevelFilesUpToDate(StandardDiskBuildResultCache diskCache)
		{
			bool flag = false;
			try
			{
				CompilationLock.GetLock(ref flag);
				this.CheckTopLevelFilesUpToDate2(diskCache);
			}
			finally
			{
				if (flag)
				{
					CompilationLock.ReleaseLock();
				}
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000406B0 File Offset: 0x0003F6B0
		private void CheckTopLevelFilesUpToDate2(StandardDiskBuildResultCache diskCache)
		{
			long preservedSpecialFilesCombinedHash = diskCache.GetPreservedSpecialFilesCombinedHash();
			if (preservedSpecialFilesCombinedHash != 0L)
			{
				diskCache.RemoveOldTempFiles();
			}
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddObject(HttpRuntime.AppDomainAppPathInternal);
			string fullyQualifiedName = typeof(HttpRuntime).Module.FullyQualifiedName;
			hashCodeCombiner.AddFile(fullyQualifiedName);
			string machineConfigurationFilePath = HttpConfigurationSystem.MachineConfigurationFilePath;
			hashCodeCombiner.AddFile(machineConfigurationFilePath);
			string rootWebConfigurationFilePath = HttpConfigurationSystem.RootWebConfigurationFilePath;
			hashCodeCombiner.AddFile(rootWebConfigurationFilePath);
			RuntimeConfig appConfig = RuntimeConfig.GetAppConfig();
			CompilationSection compilation = appConfig.Compilation;
			if (!BuildManagerHost.InClientBuildManager)
			{
				this._optimizeCompilations = compilation.OptimizeCompilations;
			}
			if (!BuildManager.OptimizeCompilations)
			{
				string binDirectoryInternal = HttpRuntime.BinDirectoryInternal;
				hashCodeCombiner.AddDirectory(binDirectoryInternal);
				hashCodeCombiner.AddResourcesDirectory(HttpRuntime.ResourcesDirectoryVirtualPath.MapPathInternal());
				hashCodeCombiner.AddDirectory(HttpRuntime.WebRefDirectoryVirtualPath.MapPathInternal());
				hashCodeCombiner.AddDirectory(HttpRuntime.CodeDirectoryVirtualPath.MapPathInternal());
				hashCodeCombiner.AddFile(BuildManager.GlobalAsaxVirtualPath.MapPathInternal());
			}
			hashCodeCombiner.AddObject(compilation.RecompilationHash);
			ProfileSection profile = appConfig.Profile;
			hashCodeCombiner.AddObject(profile.RecompilationHash);
			hashCodeCombiner.AddObject(appConfig.Globalization.FileEncoding);
			TrustSection trust = appConfig.Trust;
			hashCodeCombiner.AddObject(trust.Level);
			hashCodeCombiner.AddObject(trust.OriginUrl);
			hashCodeCombiner.AddObject(ProfileManager.Enabled);
			hashCodeCombiner.AddObject(BuildManager.PrecompilingWithDebugInfo);
			BuildManager.s_topLevelHash = hashCodeCombiner.CombinedHash;
			if (BuildManager.PrecompilingForCleanBuild || hashCodeCombiner.CombinedHash != preservedSpecialFilesCombinedHash)
			{
				bool precompilingForCleanBuild = BuildManager.PrecompilingForCleanBuild;
				diskCache.RemoveAllCodegenFiles();
				diskCache.SavePreservedSpecialFilesCombinedHash(hashCodeCombiner.CombinedHash);
			}
			HttpRuntime.FileChangesMonitor.StartMonitoringFile(this._webHashFilePath, new FileChangeEventHandler(this.OnWebHashFileChange));
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00040850 File Offset: 0x0003F850
		private void OnWebHashFileChange(object sender, FileChangeEvent e)
		{
			HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.BuildManagerChange, "Change in " + this._webHashFilePath);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0004086A File Offset: 0x0003F86A
		internal static bool IsReservedAssemblyName(string assemblyName)
		{
			return string.Compare(assemblyName, "App_Code", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(assemblyName, "App_GlobalResources", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(assemblyName, "App_WebReferences", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(assemblyName, "App_global.asax", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x000408A8 File Offset: 0x0003F8A8
		private Assembly CompileCodeDirectory(VirtualPath virtualDir, CodeDirectoryType dirType, string assemblyName, StringSet excludedSubdirectories)
		{
			bool flag = true;
			if (BuildManager.IsPrecompiledApp)
			{
				flag = this.IsUpdatablePrecompiledAppInternal && dirType == CodeDirectoryType.LocalResources;
			}
			AssemblyReferenceInfo assemblyReferenceInfo = new AssemblyReferenceInfo(this._topLevelReferencedAssemblies.Count);
			this._topLevelAssembliesIndexTable[virtualDir.VirtualPathString] = assemblyReferenceInfo;
			Assembly codeDirectoryAssembly = CodeDirectoryCompiler.GetCodeDirectoryAssembly(virtualDir, dirType, assemblyName, excludedSubdirectories, flag);
			if (codeDirectoryAssembly != null)
			{
				assemblyReferenceInfo.Assembly = codeDirectoryAssembly;
				if (dirType != CodeDirectoryType.LocalResources)
				{
					this._topLevelReferencedAssemblies.Add(codeDirectoryAssembly);
					if (dirType == CodeDirectoryType.MainCode || dirType == CodeDirectoryType.SubCode)
					{
						if (this._codeAssemblies == null)
						{
							this._codeAssemblies = new ArrayList();
						}
						this._codeAssemblies.Add(codeDirectoryAssembly);
					}
					if (this._assemblyResolveMapping == null)
					{
						this._assemblyResolveMapping = new Hashtable(StringComparer.OrdinalIgnoreCase);
					}
					this._assemblyResolveMapping[assemblyName] = codeDirectoryAssembly;
					if (dirType == CodeDirectoryType.MainCode)
					{
						this._profileType = ProfileBuildProvider.GetProfileTypeFromAssembly(codeDirectoryAssembly, BuildManager.IsPrecompiledApp);
						this._assemblyResolveMapping["__code"] = codeDirectoryAssembly;
					}
				}
			}
			return codeDirectoryAssembly;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00040990 File Offset: 0x0003F990
		private void CompileResourcesDirectory()
		{
			VirtualPath resourcesDirectoryVirtualPath = HttpRuntime.ResourcesDirectoryVirtualPath;
			this._appResourcesAssembly = this.CompileCodeDirectory(resourcesDirectoryVirtualPath, CodeDirectoryType.AppResources, "App_GlobalResources", null);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000409B7 File Offset: 0x0003F9B7
		private void CompileWebRefDirectory()
		{
			this.CompileCodeDirectory(HttpRuntime.WebRefDirectoryVirtualPath, CodeDirectoryType.WebReferences, "App_WebReferences", null);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000409CC File Offset: 0x0003F9CC
		private void EnsureExcludedCodeSubDirectoriesComputed()
		{
			if (this._excludedCodeSubdirectories != null)
			{
				return;
			}
			this._excludedCodeSubdirectories = new CaseInsensitiveStringSet();
			CodeSubDirectoriesCollection codeSubDirectories = CompilationUtil.GetCodeSubDirectories();
			if (codeSubDirectories != null)
			{
				foreach (object obj in codeSubDirectories)
				{
					CodeSubDirectory codeSubDirectory = (CodeSubDirectory)obj;
					this._excludedCodeSubdirectories.Add(codeSubDirectory.DirectoryName);
				}
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00040A48 File Offset: 0x0003FA48
		private void CompileCodeDirectories()
		{
			VirtualPath codeDirectoryVirtualPath = HttpRuntime.CodeDirectoryVirtualPath;
			CodeSubDirectoriesCollection codeSubDirectories = CompilationUtil.GetCodeSubDirectories();
			if (codeSubDirectories != null)
			{
				foreach (object obj in codeSubDirectories)
				{
					CodeSubDirectory codeSubDirectory = (CodeSubDirectory)obj;
					VirtualPath virtualPath = codeDirectoryVirtualPath.SimpleCombineWithDir(codeSubDirectory.DirectoryName);
					string text = "App_SubCode_" + codeSubDirectory.AssemblyName;
					this.CompileCodeDirectory(virtualPath, CodeDirectoryType.SubCode, text, null);
				}
			}
			this.EnsureExcludedCodeSubDirectoriesComputed();
			this.CompileCodeDirectory(codeDirectoryVirtualPath, CodeDirectoryType.MainCode, "App_Code", this._excludedCodeSubdirectories);
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00040AF0 File Offset: 0x0003FAF0
		private void CompileGlobalAsax()
		{
			this._globalAsaxBuildResult = ApplicationBuildProvider.GetGlobalAsaxBuildResult(BuildManager.IsPrecompiledApp);
			HttpApplicationFactory.SetupFileChangeNotifications();
			if (this._globalAsaxBuildResult != null)
			{
				Type type = this._globalAsaxBuildResult.ResultType;
				while (type.Assembly != typeof(HttpRuntime).Assembly)
				{
					this._topLevelReferencedAssemblies.Add(type.Assembly);
					type = type.BaseType;
				}
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00040B57 File Offset: 0x0003FB57
		internal static void CallAppInitializeMethod()
		{
			BuildManager._theBuildManager.EnsureTopLevelFilesCompiled();
			CodeDirectoryCompiler.CallAppInitializeMethod();
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x00040B68 File Offset: 0x0003FB68
		internal void EnsureTopLevelFilesCompiled()
		{
			if (this._topLevelFileCompilationException != null && !BuildManager.SkipTopLevelCompilationExceptions)
			{
				this.ReportTopLevelCompilationException();
			}
			if (this._topLevelFilesCompiledStarted)
			{
				return;
			}
			using (new ApplicationImpersonationContext())
			{
				bool flag = false;
				BuildManager._parseErrorReported = false;
				try
				{
					CompilationLock.GetLock(ref flag);
					if (this._topLevelFileCompilationException != null && !BuildManager.SkipTopLevelCompilationExceptions)
					{
						this.ReportTopLevelCompilationException();
					}
					if (!this._topLevelFilesCompiledStarted)
					{
						this._topLevelFilesCompiledStarted = true;
						this._topLevelReferencedAssemblies = new List<Assembly>();
						this._topLevelAssembliesIndexTable = new Dictionary<string, AssemblyReferenceInfo>(StringComparer.OrdinalIgnoreCase);
						this._topLevelReferencedAssemblies.Add(typeof(HttpRuntime).Assembly);
						this._topLevelReferencedAssemblies.Add(typeof(Component).Assembly);
						this._compilationStage = CompilationStage.TopLevelFiles;
						this.CompileResourcesDirectory();
						this.CompileWebRefDirectory();
						this.CompileCodeDirectories();
						this._compilationStage = CompilationStage.GlobalAsax;
						this.CompileGlobalAsax();
						this._compilationStage = CompilationStage.BrowserCapabilities;
						BrowserCapabilitiesCompiler.GetBrowserCapabilitiesType();
						HttpCapabilitiesBase emptyHttpCapabilitiesBase = HttpCapabilitiesBase.EmptyHttpCapabilitiesBase;
						this._compilationStage = CompilationStage.AfterTopLevelFiles;
					}
				}
				catch (Exception ex)
				{
					this._topLevelFileCompilationException = ex;
					if (!BuildManager.SkipTopLevelCompilationExceptions)
					{
						if (!BuildManager._parseErrorReported && !(ex is HttpCompileException))
						{
							this.ReportTopLevelCompilationException();
						}
						throw;
					}
				}
				finally
				{
					this._topLevelFilesCompiledCompleted = true;
					if (flag)
					{
						CompilationLock.ReleaseLock();
					}
				}
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00040CF4 File Offset: 0x0003FCF4
		private static string GenerateRandomFileName()
		{
			byte[] array = new byte[6];
			lock (BuildManager._rng)
			{
				BuildManager._rng.GetBytes(array);
			}
			string text = Convert.ToBase64String(array).ToLower(CultureInfo.InvariantCulture);
			text = text.Replace('/', '-');
			return text.Replace('+', '_');
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00040D60 File Offset: 0x0003FD60
		internal static string GenerateRandomAssemblyName(string baseName)
		{
			return BuildManager.GenerateRandomAssemblyName(baseName, true);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00040D69 File Offset: 0x0003FD69
		internal static string GenerateRandomAssemblyName(string baseName, bool topLevel)
		{
			if (BuildManager.PrecompilingForDeployment)
			{
				return baseName;
			}
			if (BuildManager.OptimizeCompilations && topLevel)
			{
				return baseName;
			}
			return baseName = baseName + "." + BuildManager.GenerateRandomFileName();
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00040D93 File Offset: 0x0003FD93
		private static string GetGeneratedAssemblyBaseName(VirtualPath virtualPath)
		{
			return BuildManager.GetCacheKeyFromVirtualPath(virtualPath);
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00040D9B File Offset: 0x0003FD9B
		public static Type GetType(string typeName, bool throwOnError)
		{
			return BuildManager.GetType(typeName, throwOnError, false);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00040DA8 File Offset: 0x0003FDA8
		public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			Type type = null;
			if (Util.TypeNameContainsAssembly(typeName))
			{
				type = Type.GetType(typeName, throwOnError, ignoreCase);
				if (type != null)
				{
					return type;
				}
			}
			if (!BuildManager.InitializeBuildManager())
			{
				return Type.GetType(typeName, throwOnError, ignoreCase);
			}
			try
			{
				type = typeof(BuildManager).Assembly.GetType(typeName, false, ignoreCase);
			}
			catch (ArgumentException ex)
			{
				throw new HttpException(SR.GetString("Invalid_type", new object[] { typeName }), ex);
			}
			if (type != null)
			{
				return type;
			}
			BuildManager._theBuildManager.EnsureTopLevelFilesCompiled();
			type = Util.GetTypeFromAssemblies(BuildManager.TheBuildManager.TopLevelReferencedAssemblies, typeName, ignoreCase);
			if (type != null)
			{
				return type;
			}
			AssemblyCollection assembliesForAppLevel = CompilationUtil.GetAssembliesForAppLevel();
			if (assembliesForAppLevel != null)
			{
				Type typeFromAssemblies = CompilationUtil.GetTypeFromAssemblies(assembliesForAppLevel, typeName, ignoreCase);
				if (typeFromAssemblies != null)
				{
					if (type != null && typeFromAssemblies != type)
					{
						throw new HttpException(SR.GetString("Ambiguous_type", new object[]
						{
							typeName,
							Util.GetAssemblySafePathFromType(type),
							Util.GetAssemblySafePathFromType(typeFromAssemblies)
						}));
					}
					type = typeFromAssemblies;
				}
			}
			if (type == null && throwOnError)
			{
				throw new HttpException(SR.GetString("Invalid_type", new object[] { typeName }));
			}
			return type;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00040EC8 File Offset: 0x0003FEC8
		internal static Type GetTypeFromCodeAssembly(string typeName, bool ignoreCase)
		{
			if (BuildManager.CodeAssemblies == null)
			{
				return null;
			}
			return Util.GetTypeFromAssemblies(BuildManager.CodeAssemblies, typeName, ignoreCase);
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00040EDF File Offset: 0x0003FEDF
		internal static BuildProvider CreateBuildProvider(VirtualPath virtualPath, CompilationSection compConfig, ICollection referencedAssemblies, bool failIfUnknown)
		{
			return BuildManager.CreateBuildProvider(virtualPath, BuildProviderAppliesTo.Web, compConfig, referencedAssemblies, failIfUnknown);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00040EEC File Offset: 0x0003FEEC
		internal static BuildProvider CreateBuildProvider(VirtualPath virtualPath, BuildProviderAppliesTo neededFor, CompilationSection compConfig, ICollection referencedAssemblies, bool failIfUnknown)
		{
			string extension = virtualPath.Extension;
			Type buildProviderTypeFromExtension = CompilationUtil.GetBuildProviderTypeFromExtension(compConfig, extension, neededFor, failIfUnknown);
			if (buildProviderTypeFromExtension == null)
			{
				return null;
			}
			object obj = HttpRuntime.CreatePublicInstance(buildProviderTypeFromExtension);
			BuildProvider buildProvider = (BuildProvider)obj;
			buildProvider.SetVirtualPath(virtualPath);
			buildProvider.SetReferencedAssemblies(referencedAssemblies);
			return buildProvider;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00040F2D File Offset: 0x0003FF2D
		internal static void ValidateCodeFileVirtualPath(VirtualPath virtualPath)
		{
			BuildManager._theBuildManager.ValidateVirtualPathInternal(virtualPath, false, true);
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00040F3C File Offset: 0x0003FF3C
		private void ValidateVirtualPathInternal(VirtualPath virtualPath, bool allowCrossApp, bool codeFile)
		{
			if (!allowCrossApp)
			{
				virtualPath.FailIfNotWithinAppRoot();
			}
			else if (!virtualPath.IsWithinAppRoot)
			{
				return;
			}
			if (HttpRuntime.AppDomainAppVirtualPathObject == virtualPath)
			{
				return;
			}
			int length = HttpRuntime.AppDomainAppVirtualPathString.Length;
			string virtualPathString = virtualPath.VirtualPathString;
			if (virtualPathString.Length < length)
			{
				return;
			}
			int num = virtualPathString.IndexOf('/', length);
			if (num < 0)
			{
				return;
			}
			string text = virtualPathString.Substring(length, num - length);
			if (this._forbiddenTopLevelDirectories.Contains(text))
			{
				throw new HttpException(SR.GetString("Illegal_special_dir", new object[] { virtualPathString, text }));
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00040FD4 File Offset: 0x0003FFD4
		internal static long GetBuildResultHashCodeIfCached(HttpContext context, string virtualPath)
		{
			BuildResult vpathBuildResult = BuildManager.GetVPathBuildResult(context, VirtualPath.Create(virtualPath), true, false);
			if (vpathBuildResult == null)
			{
				return 0L;
			}
			string virtualPathDependenciesHash = vpathBuildResult.VirtualPathDependenciesHash;
			return vpathBuildResult.ComputeHashCode(BuildManager.s_topLevelHash, (long)StringUtil.GetStringHashCode(virtualPathDependenciesHash));
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0004100F File Offset: 0x0004000F
		internal static BuildResult GetVPathBuildResult(VirtualPath virtualPath)
		{
			return BuildManager.GetVPathBuildResult(null, virtualPath, false, false, false);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0004101B File Offset: 0x0004001B
		internal static BuildResult GetVPathBuildResult(HttpContext context, VirtualPath virtualPath)
		{
			return BuildManager.GetVPathBuildResult(context, virtualPath, false, false, false);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00041027 File Offset: 0x00040027
		internal static BuildResult GetVPathBuildResult(HttpContext context, VirtualPath virtualPath, bool noBuild, bool allowCrossApp)
		{
			return BuildManager.GetVPathBuildResult(context, virtualPath, noBuild, allowCrossApp, false);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00041033 File Offset: 0x00040033
		internal static BuildResult GetVPathBuildResult(HttpContext context, VirtualPath virtualPath, bool noBuild, bool allowCrossApp, bool allowBuildInPrecompile)
		{
			if (HttpRuntime.IsFullTrust)
			{
				return BuildManager.GetVPathBuildResultWithNoAssert(context, virtualPath, noBuild, allowCrossApp, allowBuildInPrecompile);
			}
			return BuildManager.GetVPathBuildResultWithAssert(context, virtualPath, noBuild, allowCrossApp, allowBuildInPrecompile);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00041053 File Offset: 0x00040053
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		internal static BuildResult GetVPathBuildResultWithAssert(HttpContext context, VirtualPath virtualPath, bool noBuild, bool allowCrossApp, bool allowBuildInPrecompile)
		{
			return BuildManager.GetVPathBuildResultWithNoAssert(context, virtualPath, noBuild, allowCrossApp, allowBuildInPrecompile);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00041060 File Offset: 0x00040060
		internal static BuildResult GetVPathBuildResultWithNoAssert(HttpContext context, VirtualPath virtualPath, bool noBuild, bool allowCrossApp, bool allowBuildInPrecompile)
		{
			BuildResult vpathBuildResultInternal;
			using (new ApplicationImpersonationContext())
			{
				vpathBuildResultInternal = BuildManager._theBuildManager.GetVPathBuildResultInternal(virtualPath, noBuild, allowCrossApp, allowBuildInPrecompile);
			}
			return vpathBuildResultInternal;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000410A0 File Offset: 0x000400A0
		private BuildResult GetVPathBuildResultInternal(VirtualPath virtualPath, bool noBuild, bool allowCrossApp, bool allowBuildInPrecompile)
		{
			if (this._compilationStage == CompilationStage.TopLevelFiles)
			{
				throw new HttpException(SR.GetString("Too_early_for_webfile", new object[] { virtualPath }));
			}
			BuildResult buildResult = this.GetVPathBuildResultFromCacheInternal(virtualPath);
			if (buildResult != null)
			{
				return buildResult;
			}
			if (noBuild)
			{
				return null;
			}
			this.ValidateVirtualPathInternal(virtualPath, allowCrossApp, false);
			Util.CheckVirtualFileExists(virtualPath);
			if (this.IsNonUpdatablePrecompiledApp && !allowBuildInPrecompile)
			{
				throw new HttpException(SR.GetString("Cant_update_precompiled_app", new object[] { virtualPath }));
			}
			bool flag = false;
			try
			{
				CompilationLock.GetLock(ref flag);
				buildResult = this.GetVPathBuildResultFromCacheInternal(virtualPath);
				if (buildResult != null)
				{
					return buildResult;
				}
				VirtualPathSet virtualPathSet = CallContext.GetData("CircRefChk") as VirtualPathSet;
				if (virtualPathSet == null)
				{
					virtualPathSet = new VirtualPathSet();
					CallContext.SetData("CircRefChk", virtualPathSet);
				}
				if (virtualPathSet.Contains(virtualPath))
				{
					throw new HttpException(SR.GetString("Circular_include"));
				}
				virtualPathSet.Add(virtualPath);
				try
				{
					this.EnsureTopLevelFilesCompiled();
					buildResult = this.CompileWebFile(virtualPath);
				}
				finally
				{
					virtualPathSet.Remove(virtualPath);
				}
			}
			finally
			{
				if (flag)
				{
					CompilationLock.ReleaseLock();
				}
			}
			return buildResult;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000411C0 File Offset: 0x000401C0
		private BuildResult CompileWebFile(VirtualPath virtualPath)
		{
			BuildResult buildResult = null;
			if (this._topLevelFilesCompiledCompleted)
			{
				VirtualPath parent = virtualPath.Parent;
				if (this.IsBatchEnabledForDirectory(parent))
				{
					this.BatchCompileWebDirectory(null, parent, true);
					string cacheKeyFromVirtualPath = BuildManager.GetCacheKeyFromVirtualPath(virtualPath);
					buildResult = this._memoryCache.GetBuildResult(cacheKeyFromVirtualPath);
					if (buildResult != null)
					{
						if (buildResult is BuildResultCompileError)
						{
							throw ((BuildResultCompileError)buildResult).CompileException;
						}
						return buildResult;
					}
				}
			}
			DateTime utcNow = DateTime.UtcNow;
			string text = "App_Web_" + BuildManager.GenerateRandomAssemblyName(BuildManager.GetGeneratedAssemblyBaseName(virtualPath), false);
			BuildProvidersCompiler buildProvidersCompiler = new BuildProvidersCompiler(virtualPath, text);
			BuildProvider buildProvider = BuildManager.CreateBuildProvider(virtualPath, buildProvidersCompiler.CompConfig, buildProvidersCompiler.ReferencedAssemblies, true);
			buildProvidersCompiler.SetBuildProviders(new SingleObjectCollection(buildProvider));
			try
			{
				CompilerResults compilerResults = buildProvidersCompiler.PerformBuild();
				buildResult = buildProvider.GetBuildResult(compilerResults);
			}
			catch (HttpCompileException ex)
			{
				if (ex.DontCache)
				{
					throw;
				}
				buildResult = new BuildResultCompileError(virtualPath, ex);
				buildProvider.SetBuildResultDependencies(buildResult);
				ex.VirtualPathDependencies = buildProvider.VirtualPathDependencies;
				this.CacheVPathBuildResultInternal(virtualPath, buildResult, utcNow);
				ex.DontCache = true;
				throw;
			}
			if (buildResult == null)
			{
				return null;
			}
			this.CacheVPathBuildResultInternal(virtualPath, buildResult, utcNow);
			return buildResult;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x000412E0 File Offset: 0x000402E0
		private void EnsureFirstTimeDirectoryInit(VirtualPath virtualDir)
		{
			if (BuildManager.PrecompilingForUpdatableDeployment)
			{
				return;
			}
			if (virtualDir == null)
			{
				return;
			}
			if (this._localResourcesAssemblies.Contains(virtualDir))
			{
				return;
			}
			if (!virtualDir.IsWithinAppRoot)
			{
				return;
			}
			VirtualPath virtualPath = virtualDir.SimpleCombineWithDir("App_LocalResources");
			bool flag;
			try
			{
				flag = virtualPath.DirectoryExists();
			}
			catch
			{
				this._localResourcesAssemblies[virtualDir] = null;
				return;
			}
			try
			{
				HttpRuntime.StartListeningToLocalResourcesDirectory(virtualPath);
			}
			catch
			{
				if (flag)
				{
					throw;
				}
			}
			Assembly assembly = null;
			if (flag)
			{
				string localResourcesAssemblyName = BuildManager.GetLocalResourcesAssemblyName(virtualDir);
				bool flag2 = false;
				try
				{
					CompilationLock.GetLock(ref flag2);
					assembly = this.CompileCodeDirectory(virtualPath, CodeDirectoryType.LocalResources, localResourcesAssemblyName, null);
				}
				finally
				{
					if (flag2)
					{
						CompilationLock.ReleaseLock();
					}
				}
			}
			this._localResourcesAssemblies[virtualDir] = assembly;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000413B0 File Offset: 0x000403B0
		private void EnsureFirstTimeDirectoryInitForDependencies(ICollection dependencies)
		{
			foreach (object obj in dependencies)
			{
				string text = (string)obj;
				VirtualPath virtualPath = VirtualPath.Create(text);
				VirtualPath parent = virtualPath.Parent;
				this.EnsureFirstTimeDirectoryInit(parent);
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00041418 File Offset: 0x00040418
		internal static Assembly GetLocalResourcesAssembly(VirtualPath virtualDir)
		{
			return (Assembly)BuildManager._theBuildManager._localResourcesAssemblies[virtualDir];
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004142F File Offset: 0x0004042F
		internal static string GetLocalResourcesAssemblyName(VirtualPath virtualDir)
		{
			return "App_LocalResources." + BuildManager.GetGeneratedAssemblyBaseName(virtualDir);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00041441 File Offset: 0x00040441
		private bool IsBatchEnabledForDirectory(VirtualPath virtualDir)
		{
			return !BuildManager.CompileWithFixedAssemblyNames && (BuildManager.PrecompilingForDeployment || ((!BuildManagerHost.InClientBuildManager || BuildManager.PerformingPrecompilation) && CompilationUtil.IsBatchingEnabled(virtualDir.VirtualPathString)));
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00041470 File Offset: 0x00040470
		private bool BatchCompileWebDirectory(VirtualDirectory vdir, VirtualPath virtualDir, bool ignoreErrors)
		{
			if (virtualDir == null)
			{
				virtualDir = vdir.VirtualPathObject;
			}
			if (vdir == null)
			{
				vdir = HostingEnvironment.VirtualPathProvider.GetDirectory(virtualDir);
			}
			CaseInsensitiveStringSet caseInsensitiveStringSet = CallContext.GetData("BatchCompileChk") as CaseInsensitiveStringSet;
			if (caseInsensitiveStringSet == null)
			{
				caseInsensitiveStringSet = new CaseInsensitiveStringSet();
				CallContext.SetData("BatchCompileChk", caseInsensitiveStringSet);
			}
			if (caseInsensitiveStringSet.Contains(vdir.VirtualPath))
			{
				return false;
			}
			caseInsensitiveStringSet.Add(vdir.VirtualPath);
			if (this._precompilingApp)
			{
				ignoreErrors = false;
			}
			return this.BatchCompileWebDirectoryInternal(vdir, ignoreErrors);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x000414F4 File Offset: 0x000404F4
		private bool BatchCompileWebDirectoryInternal(VirtualDirectory vdir, bool ignoreErrors)
		{
			WebDirectoryBatchCompiler webDirectoryBatchCompiler = new WebDirectoryBatchCompiler(vdir);
			if (ignoreErrors)
			{
				webDirectoryBatchCompiler.SetIgnoreErrors();
				try
				{
					webDirectoryBatchCompiler.Process();
					return true;
				}
				catch
				{
					return false;
				}
			}
			webDirectoryBatchCompiler.Process();
			return true;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00041538 File Offset: 0x00040538
		internal static Type GetGlobalAsaxType()
		{
			return BuildManager._theBuildManager.GetGlobalAsaxTypeInternal();
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00041544 File Offset: 0x00040544
		private Type GetGlobalAsaxTypeInternal()
		{
			this.EnsureTopLevelFilesCompiled();
			if (this._globalAsaxBuildResult == null)
			{
				return typeof(HttpApplication);
			}
			return this._globalAsaxBuildResult.ResultType;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0004156A File Offset: 0x0004056A
		internal static BuildResultCompiledGlobalAsaxType GetGlobalAsaxBuildResult()
		{
			return BuildManager._theBuildManager.GetGlobalAsaxBuildResultInternal();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00041576 File Offset: 0x00040576
		private BuildResultCompiledGlobalAsaxType GetGlobalAsaxBuildResultInternal()
		{
			this.EnsureTopLevelFilesCompiled();
			return this._globalAsaxBuildResult;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00041584 File Offset: 0x00040584
		internal string[] GetCodeDirectories()
		{
			VirtualPath codeDirectoryVirtualPath = HttpRuntime.CodeDirectoryVirtualPath;
			if (!codeDirectoryVirtualPath.DirectoryExists())
			{
				return new string[0];
			}
			CodeSubDirectoriesCollection codeSubDirectories = CompilationUtil.GetCodeSubDirectories();
			int num = 1;
			if (codeSubDirectories != null)
			{
				num += codeSubDirectories.Count;
			}
			string[] array = new string[num];
			int num2 = 0;
			if (codeSubDirectories != null)
			{
				foreach (object obj in codeSubDirectories)
				{
					CodeSubDirectory codeSubDirectory = (CodeSubDirectory)obj;
					VirtualPath virtualPath = codeDirectoryVirtualPath.SimpleCombineWithDir(codeSubDirectory.DirectoryName);
					array[num2++] = virtualPath.VirtualPathString;
				}
			}
			array[num2++] = codeDirectoryVirtualPath.VirtualPathString;
			return array;
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00041640 File Offset: 0x00040640
		internal void GetCodeDirectoryInformation(VirtualPath virtualCodeDir, out Type codeDomProviderType, out CompilerParameters compilerParameters, out string generatedFilesDir)
		{
			CompilationStage compilationStage = this._compilationStage;
			try
			{
				this.GetCodeDirectoryInformationInternal(virtualCodeDir, out codeDomProviderType, out compilerParameters, out generatedFilesDir);
			}
			finally
			{
				this._compilationStage = compilationStage;
			}
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0004167C File Offset: 0x0004067C
		private void GetCodeDirectoryInformationInternal(VirtualPath virtualCodeDir, out Type codeDomProviderType, out CompilerParameters compilerParameters, out string generatedFilesDir)
		{
			StringSet stringSet = null;
			CodeDirectoryType codeDirectoryType;
			if (virtualCodeDir == HttpRuntime.CodeDirectoryVirtualPath)
			{
				this.EnsureExcludedCodeSubDirectoriesComputed();
				stringSet = this._excludedCodeSubdirectories;
				codeDirectoryType = CodeDirectoryType.MainCode;
				this._compilationStage = CompilationStage.TopLevelFiles;
			}
			else if (virtualCodeDir == HttpRuntime.ResourcesDirectoryVirtualPath)
			{
				codeDirectoryType = CodeDirectoryType.AppResources;
				this._compilationStage = CompilationStage.TopLevelFiles;
			}
			else if (string.Compare(virtualCodeDir.VirtualPathString, 0, HttpRuntime.WebRefDirectoryVirtualPath.VirtualPathString, 0, HttpRuntime.WebRefDirectoryVirtualPath.VirtualPathString.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				virtualCodeDir = HttpRuntime.WebRefDirectoryVirtualPath;
				codeDirectoryType = CodeDirectoryType.WebReferences;
				this._compilationStage = CompilationStage.TopLevelFiles;
			}
			else if (string.Compare(virtualCodeDir.FileName, "App_LocalResources", StringComparison.OrdinalIgnoreCase) == 0)
			{
				codeDirectoryType = CodeDirectoryType.LocalResources;
				this._compilationStage = CompilationStage.AfterTopLevelFiles;
			}
			else
			{
				codeDirectoryType = CodeDirectoryType.SubCode;
				this._compilationStage = CompilationStage.TopLevelFiles;
			}
			AssemblyReferenceInfo assemblyReferenceInfo = BuildManager.TheBuildManager.TopLevelAssembliesIndexTable[virtualCodeDir.VirtualPathString];
			if (assemblyReferenceInfo == null)
			{
				throw new InvalidOperationException(SR.GetString("Invalid_CodeSubDirectory_Not_Exist", new object[] { virtualCodeDir }));
			}
			CodeDirectoryCompiler.GetCodeDirectoryInformation(virtualCodeDir, codeDirectoryType, stringSet, assemblyReferenceInfo.ReferenceIndex, out codeDomProviderType, out compilerParameters, out generatedFilesDir);
			Assembly assembly = assemblyReferenceInfo.Assembly;
			if (assembly != null)
			{
				compilerParameters.OutputAssembly = assembly.Location;
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0004178B File Offset: 0x0004078B
		internal static Type GetProfileType()
		{
			return BuildManager._theBuildManager.GetProfileTypeInternal();
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00041797 File Offset: 0x00040797
		private Type GetProfileTypeInternal()
		{
			this.EnsureTopLevelFilesCompiled();
			return this._profileType;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x000417A8 File Offset: 0x000407A8
		public static ICollection GetVirtualPathDependencies(string virtualPath)
		{
			CompilationSection compilation = RuntimeConfig.GetRootWebConfig().Compilation;
			BuildProvider buildProvider = BuildManager.CreateBuildProvider(VirtualPath.Create(virtualPath), compilation, null, false);
			if (buildProvider == null)
			{
				return null;
			}
			return buildProvider.GetBuildResultVirtualPathDependencies();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x000417DC File Offset: 0x000407DC
		internal static string GetCacheKeyFromVirtualPath(VirtualPath virtualPath)
		{
			bool flag;
			return BuildManager.GetCacheKeyFromVirtualPath(virtualPath, out flag);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x000417F4 File Offset: 0x000407F4
		private static string GetCacheKeyFromVirtualPath(VirtualPath virtualPath, out bool keyFromVPP)
		{
			string text = virtualPath.GetCacheKey();
			if (text != null)
			{
				keyFromVPP = true;
				return text.ToLowerInvariant();
			}
			keyFromVPP = false;
			text = BuildManager._keyCache[virtualPath.VirtualPathString] as string;
			if (text != null)
			{
				return text;
			}
			text = BuildManager.GetCacheKeyFromVirtualPathInternal(virtualPath);
			BuildManager._keyCache[virtualPath.VirtualPathString] = text;
			return text;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0004184C File Offset: 0x0004084C
		private static string GetCacheKeyFromVirtualPathInternal(VirtualPath virtualPath)
		{
			string text = virtualPath.AppRelativeVirtualPathString.ToLowerInvariant();
			text = UrlPath.RemoveSlashFromPathIfNeeded(text);
			int num = text.LastIndexOf('/');
			if (text == "~")
			{
				return "root";
			}
			string text2 = text.Substring(num + 1);
			string text3;
			if (num <= 0)
			{
				text3 = "/";
			}
			else
			{
				text3 = text.Substring(0, num);
			}
			return text2 + "." + StringUtil.GetStringHashCode(text3).ToString("x", CultureInfo.InvariantCulture);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x000418CB File Offset: 0x000408CB
		internal static BuildResult GetVPathBuildResultFromCache(VirtualPath virtualPath)
		{
			return BuildManager.TheBuildManager.GetVPathBuildResultFromCacheInternal(virtualPath);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000418D8 File Offset: 0x000408D8
		private BuildResult GetVPathBuildResultFromCacheInternal(VirtualPath virtualPath)
		{
			bool flag;
			string cacheKeyFromVirtualPath = BuildManager.GetCacheKeyFromVirtualPath(virtualPath, out flag);
			return this.GetBuildResultFromCacheInternal(cacheKeyFromVirtualPath, flag, virtualPath, 0L);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x000418F9 File Offset: 0x000408F9
		internal static BuildResult GetBuildResultFromCache(string cacheKey)
		{
			return BuildManager._theBuildManager.GetBuildResultFromCacheInternal(cacheKey, false, null, 0L);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0004190A File Offset: 0x0004090A
		internal static BuildResult GetBuildResultFromCache(string cacheKey, VirtualPath virtualPath)
		{
			return BuildManager._theBuildManager.GetBuildResultFromCacheInternal(cacheKey, false, virtualPath, 0L);
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0004191C File Offset: 0x0004091C
		private BuildResult GetBuildResultFromCacheInternal(string cacheKey, bool keyFromVPP, VirtualPath virtualPath, long hashCode)
		{
			if (!BuildManager._theBuildManagerInitialized)
			{
				return null;
			}
			BuildResult buildResult = this._memoryCache.GetBuildResult(cacheKey, virtualPath, hashCode);
			if (buildResult != null)
			{
				return this.PostProcessFoundBuildResult(buildResult, keyFromVPP, virtualPath);
			}
			BuildResult buildResult2;
			lock (this)
			{
				int i = 0;
				while (i < this._caches.Length)
				{
					buildResult = this._caches[i].GetBuildResult(cacheKey, virtualPath, hashCode);
					if (buildResult != null)
					{
						if (buildResult.VirtualPathDependencies != null)
						{
							this.EnsureFirstTimeDirectoryInitForDependencies(buildResult.VirtualPathDependencies);
							break;
						}
						break;
					}
					else
					{
						if (i == 0 && virtualPath != null)
						{
							VirtualPath parent = virtualPath.Parent;
							this.EnsureFirstTimeDirectoryInit(parent);
						}
						i++;
					}
				}
				if (buildResult == null)
				{
					buildResult2 = null;
				}
				else
				{
					buildResult = this.PostProcessFoundBuildResult(buildResult, keyFromVPP, virtualPath);
					if (buildResult == null)
					{
						buildResult2 = null;
					}
					else
					{
						for (int j = 0; j < i; j++)
						{
							this._caches[j].CacheBuildResult(cacheKey, buildResult, DateTime.UtcNow);
						}
						buildResult2 = buildResult;
					}
				}
			}
			return buildResult2;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00041A0C File Offset: 0x00040A0C
		private BuildResult PostProcessFoundBuildResult(BuildResult result, bool keyFromVPP, VirtualPath virtualPath)
		{
			if (!keyFromVPP && virtualPath != null)
			{
				if (AppSettings.VerifyVirtualPathFromDiskCache)
				{
					string text = virtualPath.AppRelativeVirtualPathString;
					if (text.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
					{
						text = text.Substring(1);
					}
					if (!result.VirtualPath.VirtualPathString.EndsWith(text, StringComparison.OrdinalIgnoreCase))
					{
						return null;
					}
				}
				else if (virtualPath != result.VirtualPath)
				{
					return null;
				}
			}
			if (result is BuildResultCompileError)
			{
				HttpCompileException compileException = ((BuildResultCompileError)result).CompileException;
				if (!BuildManager.PerformingPrecompilation)
				{
					this.ReportErrorsFromException(compileException);
				}
				throw compileException;
			}
			return result;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00041A94 File Offset: 0x00040A94
		internal static bool CacheVPathBuildResult(VirtualPath virtualPath, BuildResult result, DateTime utcStart)
		{
			return BuildManager._theBuildManager.CacheVPathBuildResultInternal(virtualPath, result, utcStart);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00041AA4 File Offset: 0x00040AA4
		private bool CacheVPathBuildResultInternal(VirtualPath virtualPath, BuildResult result, DateTime utcStart)
		{
			string cacheKeyFromVirtualPath = BuildManager.GetCacheKeyFromVirtualPath(virtualPath);
			return BuildManager.CacheBuildResult(cacheKeyFromVirtualPath, result, utcStart);
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00041AC0 File Offset: 0x00040AC0
		internal static bool CacheBuildResult(string cacheKey, BuildResult result, DateTime utcStart)
		{
			return BuildManager._theBuildManager.CacheBuildResultInternal(cacheKey, result, 0L, utcStart);
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00041AD4 File Offset: 0x00040AD4
		private bool CacheBuildResultInternal(string cacheKey, BuildResult result, long hashCode, DateTime utcStart)
		{
			result.EnsureVirtualPathDependenciesHashComputed();
			for (int i = 0; i < this._caches.Length; i++)
			{
				this._caches[i].CacheBuildResult(cacheKey, result, hashCode, utcStart);
			}
			if (!TimeStampChecker.CheckFilesStillValid(cacheKey, result.VirtualPathDependencies))
			{
				this._memoryCache.RemoveAssemblyAndCleanupDependencies(result as BuildResultCompiledAssemblyBase);
				return false;
			}
			return true;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00041B30 File Offset: 0x00040B30
		internal void SetPrecompilationInfo(HostingEnvironmentParameters hostingParameters)
		{
			if (hostingParameters == null || hostingParameters.ClientBuildManagerParameter == null)
			{
				return;
			}
			this._precompilationFlags = hostingParameters.ClientBuildManagerParameter.PrecompilationFlags;
			this._strongNameKeyFile = hostingParameters.ClientBuildManagerParameter.StrongNameKeyFile;
			this._strongNameKeyContainer = hostingParameters.ClientBuildManagerParameter.StrongNameKeyContainer;
			this._precompTargetPhysicalDir = hostingParameters.PrecompilationTargetPhysicalDirectory;
			if (this._precompTargetPhysicalDir == null)
			{
				return;
			}
			if (Util.IsNonEmptyDirectory(this._precompTargetPhysicalDir))
			{
				if ((this._precompilationFlags & PrecompilationFlags.OverwriteTarget) == PrecompilationFlags.Default)
				{
					throw new HttpException(SR.GetString("Dir_not_empty"));
				}
				bool flag;
				if (!BuildManager.ReadPrecompMarkerFile(this._precompTargetPhysicalDir, out flag))
				{
					throw new HttpException(SR.GetString("Dir_not_empty_not_precomp"));
				}
				if (!this.DeletePrecompTargetDirectory())
				{
					Thread.Sleep(250);
					if (!this.DeletePrecompTargetDirectory())
					{
						Thread.Sleep(1000);
						if (!this.DeletePrecompTargetDirectory())
						{
							throw new HttpException(SR.GetString("Cant_delete_dir"));
						}
					}
				}
			}
			this.CreatePrecompMarkerFile();
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00041C1C File Offset: 0x00040C1C
		private bool DeletePrecompTargetDirectory()
		{
			try
			{
				if (this._precompTargetPhysicalDir != null)
				{
					foreach (object obj in ((IEnumerable)FileEnumerator.Create(this._precompTargetPhysicalDir)))
					{
						FileData fileData = (FileData)obj;
						if (fileData.IsDirectory)
						{
							Directory.Delete(fileData.FullName, true);
						}
						else
						{
							Util.DeleteFileNoException(fileData.FullName);
						}
					}
				}
			}
			catch
			{
			}
			return !Util.IsNonEmptyDirectory(this._precompTargetPhysicalDir);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00041CBC File Offset: 0x00040CBC
		private void FailIfPrecompiledApp()
		{
			if (BuildManager.IsPrecompiledApp)
			{
				throw new HttpException(SR.GetString("Already_precomp"));
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00041CD8 File Offset: 0x00040CD8
		internal void PrecompileApp(ClientBuildManagerCallback callback)
		{
			bool skipTopLevelCompilationExceptions = BuildManager.SkipTopLevelCompilationExceptions;
			try
			{
				this._cbmCallback = callback;
				BuildManager.ThrowOnFirstParseError = false;
				BuildManager.SkipTopLevelCompilationExceptions = false;
				this.PrecompileApp(HttpRuntime.AppDomainAppVirtualPathObject);
			}
			finally
			{
				BuildManager.SkipTopLevelCompilationExceptions = skipTopLevelCompilationExceptions;
				BuildManager.ThrowOnFirstParseError = true;
				this._cbmCallback = null;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00041D30 File Offset: 0x00040D30
		private void PrecompileApp(VirtualPath startingVirtualDir)
		{
			using (new ApplicationImpersonationContext())
			{
				try
				{
					BuildManager.PerformingPrecompilation = true;
					this.PrecompileAppInternal(startingVirtualDir);
				}
				catch
				{
					this.DeletePrecompTargetDirectory();
					throw;
				}
				finally
				{
					BuildManager.PerformingPrecompilation = false;
				}
			}
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00041D9C File Offset: 0x00040D9C
		private void PrecompileAppInternal(VirtualPath startingVirtualDir)
		{
			this.FailIfPrecompiledApp();
			VirtualDirectory directory = startingVirtualDir.GetDirectory();
			this.EnsureTopLevelFilesCompiled();
			try
			{
				BuildManager._parseErrorReported = false;
				this.PrecompileWebDirectoriesRecursive(directory, true);
				this.PrecompileThemeDirectories();
			}
			catch (HttpParseException ex)
			{
				if (!BuildManager._parseErrorReported)
				{
					this.ReportErrorsFromException(ex);
				}
				throw;
			}
			if (this._precompTargetPhysicalDir != null)
			{
				string text = Path.Combine(this._precompTargetPhysicalDir, "bin");
				this.CopyCompiledAssembliesToDestinationBin(HttpRuntime.CodegenDirInternal, text);
			}
			if (this._precompTargetPhysicalDir != null)
			{
				this.CopyStaticFilesRecursive(directory, this._precompTargetPhysicalDir, true);
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00041E30 File Offset: 0x00040E30
		private void CreatePrecompMarkerFile()
		{
			Directory.CreateDirectory(this._precompTargetPhysicalDir);
			string text = Path.Combine(this._precompTargetPhysicalDir, "PrecompiledApp.config");
			using (StreamWriter streamWriter = new StreamWriter(text, false, Encoding.UTF8))
			{
				streamWriter.Write("<precompiledApp version=\"2\" updatable=\"");
				if (BuildManager.PrecompilingForUpdatableDeployment)
				{
					streamWriter.Write("true");
				}
				else
				{
					streamWriter.Write("false");
				}
				streamWriter.Write("\"/>");
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00041EB8 File Offset: 0x00040EB8
		private static bool ReadPrecompMarkerFile(string appRoot, out bool updatable)
		{
			updatable = false;
			string text = Path.Combine(appRoot, "PrecompiledApp.config");
			if (!File.Exists(text))
			{
				return false;
			}
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(text);
			}
			catch
			{
				return false;
			}
			XmlNode documentElement = xmlDocument.DocumentElement;
			if (documentElement == null || documentElement.Name != "precompiledApp")
			{
				return false;
			}
			HandlerBase.GetAndRemoveBooleanAttribute(documentElement, "updatable", ref updatable);
			return true;
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x00041F30 File Offset: 0x00040F30
		internal static bool PrecompilingForDeployment
		{
			get
			{
				return BuildManager._theBuildManager._precompTargetPhysicalDir != null;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00041F42 File Offset: 0x00040F42
		internal static bool PrecompilingForUpdatableDeployment
		{
			get
			{
				return BuildManager.PrecompilingForDeployment && (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.Updatable) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x00041F5F File Offset: 0x00040F5F
		private static bool PrecompilingForCleanBuild
		{
			get
			{
				return (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.Clean) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x00041F73 File Offset: 0x00040F73
		internal static bool PrecompilingWithDebugInfo
		{
			get
			{
				return BuildManager.PrecompilingForDeployment && (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.ForceDebug) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00041F90 File Offset: 0x00040F90
		internal static bool PrecompilingWithCodeAnalysisSymbol
		{
			get
			{
				return (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.CodeAnalysis) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00041FA5 File Offset: 0x00040FA5
		private static bool CompileWithFixedAssemblyNames
		{
			get
			{
				return (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.FixedNames) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00041FBD File Offset: 0x00040FBD
		internal static bool CompileWithAllowPartiallyTrustedCallersAttribute
		{
			get
			{
				return (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.AllowPartiallyTrustedCallers) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00041FD2 File Offset: 0x00040FD2
		internal static bool CompileWithDelaySignAttribute
		{
			get
			{
				return (BuildManager._theBuildManager._precompilationFlags & PrecompilationFlags.DelaySign) != PrecompilationFlags.Default;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x00041FE7 File Offset: 0x00040FE7
		internal static string StrongNameKeyFile
		{
			get
			{
				return BuildManager._theBuildManager._strongNameKeyFile;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00041FF3 File Offset: 0x00040FF3
		internal static string StrongNameKeyContainer
		{
			get
			{
				return BuildManager._theBuildManager._strongNameKeyContainer;
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00042000 File Offset: 0x00041000
		internal static TextWriter GetUpdatableDeploymentTargetWriter(VirtualPath virtualPath, Encoding fileEncoding)
		{
			if (!BuildManager.PrecompilingForUpdatableDeployment)
			{
				return null;
			}
			string text = virtualPath.AppRelativeVirtualPathString;
			text = text.Substring(2);
			string text2 = Path.Combine(BuildManager._theBuildManager._precompTargetPhysicalDir, text);
			string directoryName = Path.GetDirectoryName(text2);
			Directory.CreateDirectory(directoryName);
			return new StreamWriter(text2, false, fileEncoding);
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0004204C File Offset: 0x0004104C
		private bool IsPrecompiledAppInternal
		{
			get
			{
				if (!this._isPrecompiledAppComputed)
				{
					this._isPrecompiledApp = BuildManager.ReadPrecompMarkerFile(HttpRuntime.AppDomainAppPathInternal, out this._isUpdatablePrecompiledApp);
					this._isPrecompiledAppComputed = true;
				}
				return this._isPrecompiledApp;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00042079 File Offset: 0x00041079
		internal static bool IsPrecompiledApp
		{
			get
			{
				return BuildManager._theBuildManager.IsPrecompiledAppInternal;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00042085 File Offset: 0x00041085
		private bool IsUpdatablePrecompiledAppInternal
		{
			get
			{
				return BuildManager.IsPrecompiledApp && this._isUpdatablePrecompiledApp;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00042096 File Offset: 0x00041096
		internal static bool IsUpdatablePrecompiledApp
		{
			get
			{
				return BuildManager._theBuildManager.IsUpdatablePrecompiledAppInternal;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x000420A2 File Offset: 0x000410A2
		private bool IsNonUpdatablePrecompiledApp
		{
			get
			{
				return BuildManager.IsPrecompiledApp && !this._isUpdatablePrecompiledApp;
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x000420B8 File Offset: 0x000410B8
		private void PrecompileWebDirectoriesRecursive(VirtualDirectory vdir, bool topLevel)
		{
			foreach (object obj in vdir.Directories)
			{
				VirtualDirectory virtualDirectory = (VirtualDirectory)obj;
				if ((!topLevel || !this._excludedTopLevelDirectories.Contains(virtualDirectory.Name)) && !(virtualDirectory.Name == "_vti_cnf"))
				{
					this.PrecompileWebDirectoriesRecursive(virtualDirectory, false);
				}
			}
			try
			{
				this._precompilingApp = true;
				if (this.IsBatchEnabledForDirectory(vdir.VirtualPathObject))
				{
					this.BatchCompileWebDirectory(vdir, null, false);
				}
				else
				{
					NonBatchDirectoryCompiler nonBatchDirectoryCompiler = new NonBatchDirectoryCompiler(vdir);
					nonBatchDirectoryCompiler.Process();
				}
			}
			finally
			{
				this._precompilingApp = false;
			}
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x00042180 File Offset: 0x00041180
		private void PrecompileThemeDirectories()
		{
			string text = Path.Combine(HttpRuntime.AppDomainAppPathInternal, "App_Themes");
			if (Directory.Exists(text))
			{
				string[] directories = Directory.GetDirectories(text);
				foreach (string text2 in directories)
				{
					string fileName = Path.GetFileName(text2);
					ThemeDirectoryCompiler.GetThemeBuildResultType(null, fileName);
				}
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x000421D8 File Offset: 0x000411D8
		private void CopyStaticFilesRecursive(VirtualDirectory sourceVdir, string destPhysicalDir, bool topLevel)
		{
			string text = HostingEnvironment.MapPathInternal(sourceVdir.VirtualPath);
			BuildManager.VerifyUnrelatedSourceAndDest(text, destPhysicalDir);
			bool flag = false;
			foreach (object obj in sourceVdir.Children)
			{
				VirtualFileBase virtualFileBase = (VirtualFileBase)obj;
				string text2 = Path.Combine(destPhysicalDir, virtualFileBase.Name);
				if (virtualFileBase.IsDirectory)
				{
					if ((!topLevel || (!StringUtil.EqualsIgnoreCase(virtualFileBase.Name, "App_Code") && !StringUtil.EqualsIgnoreCase(virtualFileBase.Name, "App_GlobalResources") && !StringUtil.EqualsIgnoreCase(virtualFileBase.Name, "App_WebReferences"))) && (BuildManager.PrecompilingForUpdatableDeployment || !StringUtil.EqualsIgnoreCase(virtualFileBase.Name, "App_LocalResources")))
					{
						this.CopyStaticFilesRecursive(virtualFileBase as VirtualDirectory, text2, false);
					}
				}
				else
				{
					if (!flag)
					{
						flag = true;
						Directory.CreateDirectory(destPhysicalDir);
					}
					this.CopyPrecompiledFile(virtualFileBase as VirtualFile, text2);
				}
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x000422DC File Offset: 0x000412DC
		private void CopyCompiledAssembliesToDestinationBin(string fromDir, string toDir)
		{
			bool flag = false;
			foreach (object obj in ((IEnumerable)FileEnumerator.Create(fromDir)))
			{
				FileData fileData = (FileData)obj;
				if (!flag)
				{
					Directory.CreateDirectory(toDir);
				}
				flag = true;
				if (fileData.IsDirectory)
				{
					if (Util.IsCultureName(fileData.Name))
					{
						string text = Path.Combine(fromDir, fileData.Name);
						string text2 = Path.Combine(toDir, fileData.Name);
						this.CopyCompiledAssembliesToDestinationBin(text, text2);
					}
				}
				else
				{
					string extension = Path.GetExtension(fileData.Name);
					if (!(extension != ".dll") || !(extension != ".pdb"))
					{
						string text3 = Path.Combine(fromDir, fileData.Name);
						string text4 = Path.Combine(toDir, fileData.Name);
						File.Copy(text3, text4, true);
					}
				}
			}
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000423D4 File Offset: 0x000413D4
		private void CopyPrecompiledFile(VirtualFile vfile, string destPhysicalPath)
		{
			bool flag;
			if (CompilationUtil.NeedToCopyFile(vfile.VirtualPathObject, BuildManager.PrecompilingForUpdatableDeployment, out flag))
			{
				string text = HostingEnvironment.MapPathInternal(vfile.VirtualPath);
				if (File.Exists(destPhysicalPath))
				{
					BuildResultCompiledType buildResultCompiledType = BuildManager.GetVPathBuildResult(null, vfile.VirtualPathObject, true, false) as BuildResultCompiledType;
					Encoding encodingFromConfigPath = Util.GetEncodingFromConfigPath(vfile.VirtualPathObject);
					string text2 = Util.StringFromFile(destPhysicalPath, ref encodingFromConfigPath);
					text2 = text2.Replace("__ASPNET_INHERITS", Util.GetAssemblyQualifiedTypeName(buildResultCompiledType.ResultType));
					StreamWriter streamWriter = new StreamWriter(destPhysicalPath, false, encodingFromConfigPath);
					streamWriter.Write(text2);
					streamWriter.Close();
				}
				else
				{
					File.Copy(text, destPhysicalPath, false);
				}
				Util.ClearReadOnlyAttribute(destPhysicalPath);
				return;
			}
			if (flag)
			{
				StreamWriter streamWriter2 = new StreamWriter(destPhysicalPath);
				streamWriter2.Write(SR.GetString("Precomp_stub_file"));
				streamWriter2.Close();
			}
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000424A0 File Offset: 0x000414A0
		internal static void VerifyUnrelatedSourceAndDest(string sourcePhysicalDir, string destPhysicalDir)
		{
			sourcePhysicalDir = FileUtil.FixUpPhysicalDirectory(sourcePhysicalDir);
			destPhysicalDir = FileUtil.FixUpPhysicalDirectory(destPhysicalDir);
			if (StringUtil.StringStartsWithIgnoreCase(sourcePhysicalDir, destPhysicalDir) || StringUtil.StringStartsWithIgnoreCase(destPhysicalDir, sourcePhysicalDir))
			{
				throw new HttpException(SR.GetString("Illegal_precomp_dir", new object[] { destPhysicalDir, sourcePhysicalDir }));
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000424F0 File Offset: 0x000414F0
		internal static void ReportDirectoryCompilationProgress(VirtualPath virtualDir)
		{
			if (BuildManager.CBMCallback == null)
			{
				return;
			}
			if (!virtualDir.DirectoryExists())
			{
				return;
			}
			string @string = SR.GetString("Directory_progress", new object[] { virtualDir.VirtualPathString });
			BuildManager.CBMCallback.ReportProgress(@string);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00042535 File Offset: 0x00041535
		public static Type GetCompiledType(string virtualPath)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			return BuildManager.GetCompiledType(VirtualPath.Create(virtualPath));
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00042550 File Offset: 0x00041550
		internal static Type GetCompiledType(VirtualPath virtualPath, ClientBuildManagerCallback callback)
		{
			bool skipTopLevelCompilationExceptions = BuildManager.SkipTopLevelCompilationExceptions;
			bool throwOnFirstParseError = BuildManager.ThrowOnFirstParseError;
			Type compiledType;
			try
			{
				BuildManager.SkipTopLevelCompilationExceptions = false;
				BuildManager.ThrowOnFirstParseError = false;
				BuildManager._theBuildManager._cbmCallback = callback;
				compiledType = BuildManager.GetCompiledType(virtualPath);
			}
			finally
			{
				BuildManager._theBuildManager._cbmCallback = null;
				BuildManager.SkipTopLevelCompilationExceptions = skipTopLevelCompilationExceptions;
				BuildManager.ThrowOnFirstParseError = throwOnFirstParseError;
			}
			return compiledType;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000425B4 File Offset: 0x000415B4
		internal static Type GetCompiledType(VirtualPath virtualPath)
		{
			ITypedWebObjectFactory virtualPathObjectFactory = BuildManager.GetVirtualPathObjectFactory(virtualPath, null, false, false);
			BuildResultCompiledType buildResultCompiledType = virtualPathObjectFactory as BuildResultCompiledType;
			if (buildResultCompiledType == null)
			{
				return null;
			}
			return buildResultCompiledType.ResultType;
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x000425E0 File Offset: 0x000415E0
		public static object CreateInstanceFromVirtualPath(string virtualPath, Type requiredBaseType)
		{
			VirtualPath virtualPath2 = VirtualPath.CreateNonRelative(virtualPath);
			return BuildManager.CreateInstanceFromVirtualPath(virtualPath2, requiredBaseType, null, false, false);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00042600 File Offset: 0x00041600
		internal static object CreateInstanceFromVirtualPath(VirtualPath virtualPath, Type requiredBaseType, HttpContext context, bool allowCrossApp, bool noAssert)
		{
			ITypedWebObjectFactory virtualPathObjectFactory = BuildManager.GetVirtualPathObjectFactory(virtualPath, context, allowCrossApp, noAssert);
			if (virtualPathObjectFactory == null)
			{
				return null;
			}
			Util.CheckAssignableType(requiredBaseType, virtualPathObjectFactory.InstantiatedType);
			object obj;
			using (new ClientImpersonationContext(context))
			{
				obj = virtualPathObjectFactory.CreateInstance();
			}
			return obj;
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00042654 File Offset: 0x00041654
		private static ITypedWebObjectFactory GetVirtualPathObjectFactory(VirtualPath virtualPath, HttpContext context, bool allowCrossApp, bool noAssert)
		{
			if (virtualPath == null)
			{
				throw new ArgumentNullException("virtualPath");
			}
			if (BuildManager._theBuildManager._topLevelFileCompilationException != null)
			{
				BuildManager._theBuildManager.ReportTopLevelCompilationException();
			}
			BuildResult buildResult;
			if (HttpRuntime.IsFullTrust || noAssert)
			{
				buildResult = BuildManager.GetVPathBuildResultWithNoAssert(context, virtualPath, false, allowCrossApp, false);
			}
			else
			{
				buildResult = BuildManager.GetVPathBuildResultWithAssert(context, virtualPath, false, allowCrossApp, false);
			}
			return buildResult as ITypedWebObjectFactory;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x000426B8 File Offset: 0x000416B8
		public static Assembly GetCompiledAssembly(string virtualPath)
		{
			BuildResult vpathBuildResult = BuildManager.GetVPathBuildResult(VirtualPath.Create(virtualPath));
			if (vpathBuildResult == null)
			{
				return null;
			}
			BuildResultCompiledAssemblyBase buildResultCompiledAssemblyBase = vpathBuildResult as BuildResultCompiledAssemblyBase;
			if (buildResultCompiledAssemblyBase == null)
			{
				return null;
			}
			return buildResultCompiledAssemblyBase.ResultAssembly;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x000426E8 File Offset: 0x000416E8
		public static string GetCompiledCustomString(string virtualPath)
		{
			BuildResult vpathBuildResult = BuildManager.GetVPathBuildResult(VirtualPath.Create(virtualPath));
			if (vpathBuildResult == null)
			{
				return null;
			}
			BuildResultCustomString buildResultCustomString = vpathBuildResult as BuildResultCustomString;
			if (buildResultCustomString == null)
			{
				return null;
			}
			return buildResultCustomString.CustomString;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00042718 File Offset: 0x00041718
		public static BuildDependencySet GetCachedBuildDependencySet(HttpContext context, string virtualPath)
		{
			BuildResult vpathBuildResult = BuildManager.GetVPathBuildResult(context, VirtualPath.Create(virtualPath), true, false);
			if (vpathBuildResult == null)
			{
				return null;
			}
			return new BuildDependencySet(vpathBuildResult);
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00042740 File Offset: 0x00041740
		private Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			if (this._assemblyResolveMapping == null)
			{
				return null;
			}
			string name = e.Name;
			Assembly assembly = (Assembly)this._assemblyResolveMapping[name];
			if (assembly != null)
			{
				return assembly;
			}
			string normalizedCodeAssemblyName = BuildManager.GetNormalizedCodeAssemblyName(name);
			if (normalizedCodeAssemblyName != null)
			{
				return (Assembly)this._assemblyResolveMapping[normalizedCodeAssemblyName];
			}
			return null;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00042794 File Offset: 0x00041794
		internal static string GetNormalizedCodeAssemblyName(string assemblyName)
		{
			if (assemblyName.StartsWith("App_Code", StringComparison.Ordinal))
			{
				return "App_Code";
			}
			CodeSubDirectoriesCollection codeSubDirectories = CompilationUtil.GetCodeSubDirectories();
			foreach (object obj in codeSubDirectories)
			{
				CodeSubDirectory codeSubDirectory = (CodeSubDirectory)obj;
				if (assemblyName.StartsWith("App_SubCode_" + codeSubDirectory.AssemblyName + ".", StringComparison.Ordinal))
				{
					return codeSubDirectory.AssemblyName;
				}
			}
			return null;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00042828 File Offset: 0x00041828
		internal static string GetNormalizedTypeName(Type t)
		{
			string fullName = t.Assembly.FullName;
			string normalizedCodeAssemblyName = BuildManager.GetNormalizedCodeAssemblyName(fullName);
			if (normalizedCodeAssemblyName == null)
			{
				return t.AssemblyQualifiedName;
			}
			return t.FullName + ", " + normalizedCodeAssemblyName;
		}

		// Token: 0x0400155E RID: 5470
		internal const string AssemblyNamePrefix = "App_";

		// Token: 0x0400155F RID: 5471
		internal const string WebAssemblyNamePrefix = "App_Web_";

		// Token: 0x04001560 RID: 5472
		internal const string AppThemeAssemblyNamePrefix = "App_Theme_";

		// Token: 0x04001561 RID: 5473
		internal const string GlobalThemeAssemblyNamePrefix = "App_GlobalTheme_";

		// Token: 0x04001562 RID: 5474
		internal const string AppBrowserCapAssemblyNamePrefix = "App_Browsers";

		// Token: 0x04001563 RID: 5475
		private const string CodeDirectoryAssemblyName = "App_Code";

		// Token: 0x04001564 RID: 5476
		internal const string SubCodeDirectoryAssemblyNamePrefix = "App_SubCode_";

		// Token: 0x04001565 RID: 5477
		private const string ResourcesDirectoryAssemblyName = "App_GlobalResources";

		// Token: 0x04001566 RID: 5478
		private const string LocalResourcesDirectoryAssemblyName = "App_LocalResources";

		// Token: 0x04001567 RID: 5479
		private const string WebRefDirectoryAssemblyName = "App_WebReferences";

		// Token: 0x04001568 RID: 5480
		internal const string GlobalAsaxAssemblyName = "App_global.asax";

		// Token: 0x04001569 RID: 5481
		private const string LicensesAssemblyName = "App_Licenses";

		// Token: 0x0400156A RID: 5482
		internal const string UpdatableInheritReplacementToken = "__ASPNET_INHERITS";

		// Token: 0x0400156B RID: 5483
		private const string precompMarkerFileName = "PrecompiledApp.config";

		// Token: 0x0400156C RID: 5484
		private const string CircularReferenceCheckerSlotName = "CircRefChk";

		// Token: 0x0400156D RID: 5485
		private const string BatchCompilationSlotName = "BatchCompileChk";

		// Token: 0x0400156E RID: 5486
		private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

		// Token: 0x0400156F RID: 5487
		private static bool _theBuildManagerInitialized;

		// Token: 0x04001570 RID: 5488
		private static Exception _initializeException;

		// Token: 0x04001571 RID: 5489
		private static BuildManager _theBuildManager = new BuildManager();

		// Token: 0x04001572 RID: 5490
		private static long s_topLevelHash;

		// Token: 0x04001573 RID: 5491
		private string _precompTargetPhysicalDir;

		// Token: 0x04001574 RID: 5492
		private PrecompilationFlags _precompilationFlags;

		// Token: 0x04001575 RID: 5493
		private bool _isPrecompiledApp;

		// Token: 0x04001576 RID: 5494
		private bool _isPrecompiledAppComputed;

		// Token: 0x04001577 RID: 5495
		private bool _isUpdatablePrecompiledApp;

		// Token: 0x04001578 RID: 5496
		private bool _precompilingApp;

		// Token: 0x04001579 RID: 5497
		private string _strongNameKeyFile;

		// Token: 0x0400157A RID: 5498
		private string _strongNameKeyContainer;

		// Token: 0x0400157B RID: 5499
		private bool _optimizeCompilations;

		// Token: 0x0400157C RID: 5500
		private string _webHashFilePath;

		// Token: 0x0400157D RID: 5501
		private BuildResultCache[] _caches;

		// Token: 0x0400157E RID: 5502
		private MemoryBuildResultCache _memoryCache;

		// Token: 0x0400157F RID: 5503
		private bool _topLevelFilesCompiledStarted;

		// Token: 0x04001580 RID: 5504
		private bool _topLevelFilesCompiledCompleted;

		// Token: 0x04001581 RID: 5505
		private Exception _topLevelFileCompilationException;

		// Token: 0x04001582 RID: 5506
		private BuildResultCompiledGlobalAsaxType _globalAsaxBuildResult;

		// Token: 0x04001583 RID: 5507
		private Type _profileType;

		// Token: 0x04001584 RID: 5508
		private StringSet _excludedTopLevelDirectories;

		// Token: 0x04001585 RID: 5509
		private StringSet _forbiddenTopLevelDirectories;

		// Token: 0x04001586 RID: 5510
		private StringSet _excludedCodeSubdirectories;

		// Token: 0x04001587 RID: 5511
		private CompilationStage _compilationStage;

		// Token: 0x04001588 RID: 5512
		private VirtualPath _scriptVirtualDir;

		// Token: 0x04001589 RID: 5513
		private VirtualPath _globalAsaxVirtualPath;

		// Token: 0x0400158A RID: 5514
		private ClientBuildManagerCallback _cbmCallback;

		// Token: 0x0400158B RID: 5515
		private static bool _parseErrorReported;

		// Token: 0x0400158C RID: 5516
		private List<Assembly> _topLevelReferencedAssemblies;

		// Token: 0x0400158D RID: 5517
		private Dictionary<string, AssemblyReferenceInfo> _topLevelAssembliesIndexTable;

		// Token: 0x0400158E RID: 5518
		private Dictionary<string, string> _generatedFileTable;

		// Token: 0x0400158F RID: 5519
		private ArrayList _codeAssemblies;

		// Token: 0x04001590 RID: 5520
		private IDictionary _assemblyResolveMapping;

		// Token: 0x04001591 RID: 5521
		private Assembly _appResourcesAssembly;

		// Token: 0x04001592 RID: 5522
		private bool _throwOnFirstParseError = true;

		// Token: 0x04001593 RID: 5523
		private bool _performingPrecompilation;

		// Token: 0x04001594 RID: 5524
		private bool _skipTopLevelCompilationExceptions;

		// Token: 0x04001595 RID: 5525
		private Hashtable _localResourcesAssemblies = new Hashtable();

		// Token: 0x04001596 RID: 5526
		private static SimpleRecyclingCache _keyCache = new SimpleRecyclingCache();
	}
}

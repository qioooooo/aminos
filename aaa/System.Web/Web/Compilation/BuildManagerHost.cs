using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000136 RID: 310
	internal class BuildManagerHost : MarshalByRefObject, IRegisteredObject
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x00042894 File Offset: 0x00041894
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0004289B File Offset: 0x0004189B
		internal static bool InClientBuildManager
		{
			get
			{
				return BuildManagerHost._inClientBuildManager;
			}
			set
			{
				BuildManagerHost._inClientBuildManager = true;
			}
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000428A3 File Offset: 0x000418A3
		public BuildManagerHost()
		{
			HostingEnvironment.RegisterObject(this);
			AppDomain.CurrentDomain.AssemblyResolve += this.ResolveAssembly;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x000428D2 File Offset: 0x000418D2
		void IRegisteredObject.Stop(bool immediate)
		{
			this.WaitForPendingCallsToFinish();
			HostingEnvironment.UnregisterObject(this);
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x000428E0 File Offset: 0x000418E0
		internal IApplicationHost ApplicationHost
		{
			get
			{
				return HostingEnvironment.ApplicationHost;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x000428E8 File Offset: 0x000418E8
		internal string CodeGenDir
		{
			get
			{
				this.AddPendingCall();
				string codegenDirInternal;
				try
				{
					codegenDirInternal = HttpRuntime.CodegenDirInternal;
				}
				finally
				{
					this.RemovePendingCall();
				}
				return codegenDirInternal;
			}
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0004291C File Offset: 0x0004191C
		internal void RegisterAssembly(string assemblyName, string assemblyLocation)
		{
			if (this._assemblyCollection == null)
			{
				lock (this._lock)
				{
					if (this._assemblyCollection == null)
					{
						this._assemblyCollection = Hashtable.Synchronized(new Hashtable());
					}
				}
			}
			AssemblyName assemblyName2 = new AssemblyName(assemblyName);
			this._assemblyCollection[assemblyName2.FullName] = assemblyLocation;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00042988 File Offset: 0x00041988
		private Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			if (this._assemblyCollection == null)
			{
				return null;
			}
			string text = (string)this._assemblyCollection[e.Name];
			if (text == null)
			{
				return null;
			}
			return Assembly.LoadFrom(text);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x000429C1 File Offset: 0x000419C1
		private void WaitForPendingCallsToFinish()
		{
			while (this._pendingCallsCount > 0)
			{
				if (this._ignorePendingCalls)
				{
					return;
				}
				Thread.Sleep(250);
			}
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x000429E1 File Offset: 0x000419E1
		internal void AddPendingCall()
		{
			Interlocked.Increment(ref this._pendingCallsCount);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x000429EF File Offset: 0x000419EF
		internal void RemovePendingCall()
		{
			Interlocked.Decrement(ref this._pendingCallsCount);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000429FD File Offset: 0x000419FD
		private void OnAppDomainShutdown(object o, BuildManagerHostUnloadEventArgs args)
		{
			this._client.OnAppDomainShutdown(args.Reason);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00042A10 File Offset: 0x00041A10
		internal void CompileApplicationDependencies()
		{
			this.AddPendingCall();
			try
			{
				this._buildManager.EnsureTopLevelFilesCompiled();
			}
			finally
			{
				this.RemovePendingCall();
			}
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00042A48 File Offset: 0x00041A48
		internal void PrecompileApp(ClientBuildManagerCallback callback)
		{
			this.AddPendingCall();
			try
			{
				this._buildManager.PrecompileApp(callback);
			}
			finally
			{
				this.RemovePendingCall();
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00042A80 File Offset: 0x00041A80
		internal IDictionary GetBrowserDefinitions()
		{
			this.AddPendingCall();
			IDictionary dictionary;
			try
			{
				dictionary = BrowserCapabilitiesCompiler.BrowserCapabilitiesFactory.InternalGetBrowserElements();
			}
			finally
			{
				this.RemovePendingCall();
			}
			return dictionary;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00042AB8 File Offset: 0x00041AB8
		internal string[] GetVirtualCodeDirectories()
		{
			this.AddPendingCall();
			string[] codeDirectories;
			try
			{
				codeDirectories = this._buildManager.GetCodeDirectories();
			}
			finally
			{
				this.RemovePendingCall();
			}
			return codeDirectories;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00042AF4 File Offset: 0x00041AF4
		internal void GetCodeDirectoryInformation(VirtualPath virtualCodeDir, out Type codeDomProviderType, out CompilerParameters compParams, out string generatedFilesDir)
		{
			this.AddPendingCall();
			try
			{
				BuildManager.SkipTopLevelCompilationExceptions = true;
				this._buildManager.EnsureTopLevelFilesCompiled();
				virtualCodeDir = virtualCodeDir.CombineWithAppRoot();
				this._buildManager.GetCodeDirectoryInformation(virtualCodeDir, out codeDomProviderType, out compParams, out generatedFilesDir);
			}
			finally
			{
				BuildManager.SkipTopLevelCompilationExceptions = false;
				this.RemovePendingCall();
			}
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00042B50 File Offset: 0x00041B50
		internal void GetCompilerParams(VirtualPath virtualPath, out Type codeDomProviderType, out CompilerParameters compParams)
		{
			this.AddPendingCall();
			try
			{
				BuildManager.SkipTopLevelCompilationExceptions = true;
				this._buildManager.EnsureTopLevelFilesCompiled();
				this.GetCompilerParamsAndBuildProvider(virtualPath, out codeDomProviderType, out compParams);
				if (compParams != null)
				{
					this.FixupReferencedAssemblies(virtualPath, compParams);
				}
			}
			finally
			{
				BuildManager.SkipTopLevelCompilationExceptions = false;
				this.RemovePendingCall();
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00042BAC File Offset: 0x00041BAC
		internal string[] GetCompiledTypeAndAssemblyName(VirtualPath virtualPath, ClientBuildManagerCallback callback)
		{
			this.AddPendingCall();
			string[] array;
			try
			{
				virtualPath.CombineWithAppRoot();
				Type compiledType = BuildManager.GetCompiledType(virtualPath, callback);
				if (compiledType == null)
				{
					array = null;
				}
				else
				{
					string assemblyPathFromType = Util.GetAssemblyPathFromType(compiledType);
					array = new string[] { compiledType.FullName, assemblyPathFromType };
				}
			}
			finally
			{
				this.RemovePendingCall();
			}
			return array;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00042C0C File Offset: 0x00041C0C
		internal string GetGeneratedSourceFile(VirtualPath virtualPath)
		{
			this.AddPendingCall();
			string text2;
			try
			{
				if (!virtualPath.DirectoryExists())
				{
					throw new ArgumentException(SR.GetString("GetGeneratedSourceFile_Directory_Only", new object[] { virtualPath.VirtualPathString }), "virtualPath");
				}
				Type type;
				CompilerParameters compilerParameters;
				string text;
				this.GetCodeDirectoryInformation(virtualPath, out type, out compilerParameters, out text);
				text2 = BuildManager.GenerateFileTable[virtualPath.VirtualPathStringNoTrailingSlash];
			}
			finally
			{
				this.RemovePendingCall();
			}
			return text2;
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00042C88 File Offset: 0x00041C88
		internal string GetGeneratedFileVirtualPath(string filePath)
		{
			this.AddPendingCall();
			string text;
			try
			{
				foreach (KeyValuePair<string, string> keyValuePair in BuildManager.GenerateFileTable)
				{
					if (filePath.Equals(keyValuePair.Value, StringComparison.Ordinal))
					{
						return keyValuePair.Key;
					}
				}
				text = null;
			}
			finally
			{
				this.RemovePendingCall();
			}
			return text;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00042CF0 File Offset: 0x00041CF0
		internal string[] GetTopLevelAssemblyReferences(VirtualPath virtualPath)
		{
			this.AddPendingCall();
			ArrayList arrayList = new ArrayList();
			try
			{
				virtualPath.CombineWithAppRoot();
				CompilationSection compilation = RuntimeConfig.GetConfig(virtualPath).Compilation;
				foreach (object obj in compilation.Assemblies)
				{
					AssemblyInfo assemblyInfo = (AssemblyInfo)obj;
					Assembly[] assemblyInternal = assemblyInfo.AssemblyInternal;
					for (int i = 0; i < assemblyInternal.Length; i++)
					{
						if (assemblyInternal[i] != null)
						{
							arrayList.Add(Util.GetAssemblyCodeBase(assemblyInternal[i]));
						}
					}
				}
			}
			finally
			{
				this.RemovePendingCall();
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00042DC4 File Offset: 0x00041DC4
		internal string GenerateCode(VirtualPath virtualPath, string virtualFileString, out IDictionary linePragmasTable)
		{
			this.AddPendingCall();
			string text2;
			try
			{
				string text = null;
				Type type;
				CompilerParameters compilerParameters;
				CodeCompileUnit codeCompileUnit = this.GenerateCodeCompileUnit(virtualPath, virtualFileString, out type, out compilerParameters, out linePragmasTable);
				if (codeCompileUnit != null && type != null)
				{
					CodeDomProvider codeDomProvider = CompilationUtil.CreateCodeDomProvider(type);
					CodeGeneratorOptions codeGeneratorOptions = new CodeGeneratorOptions();
					codeGeneratorOptions.BlankLinesBetweenMembers = false;
					codeGeneratorOptions.IndentString = string.Empty;
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					codeDomProvider.GenerateCodeFromCompileUnit(codeCompileUnit, stringWriter, codeGeneratorOptions);
					text = stringWriter.ToString();
				}
				text2 = text;
			}
			finally
			{
				this.RemovePendingCall();
			}
			return text2;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00042E50 File Offset: 0x00041E50
		internal CodeCompileUnit GenerateCodeCompileUnit(VirtualPath virtualPath, string virtualFileString, out Type codeDomProviderType, out CompilerParameters compilerParameters, out IDictionary linePragmasTable)
		{
			this.AddPendingCall();
			CodeCompileUnit codeCompileUnit2;
			try
			{
				BuildManager.SkipTopLevelCompilationExceptions = true;
				this._buildManager.EnsureTopLevelFilesCompiled();
				if (virtualFileString == null)
				{
					using (Stream stream = virtualPath.OpenFile())
					{
						TextReader textReader = Util.ReaderFromStream(stream, virtualPath);
						virtualFileString = textReader.ReadToEnd();
					}
				}
				this._virtualPathProvider.RegisterVirtualFile(virtualPath, virtualFileString);
				string text = BuildManager.GetCacheKeyFromVirtualPath(virtualPath) + "_CBMResult";
				BuildResultCodeCompileUnit buildResultCodeCompileUnit = (BuildResultCodeCompileUnit)BuildManager.GetBuildResultFromCache(text, virtualPath);
				if (buildResultCodeCompileUnit == null)
				{
					lock (this._lock)
					{
						DateTime utcNow = DateTime.UtcNow;
						BuildProvider compilerParamsAndBuildProvider = this.GetCompilerParamsAndBuildProvider(virtualPath, out codeDomProviderType, out compilerParameters);
						if (compilerParamsAndBuildProvider == null)
						{
							linePragmasTable = null;
							return null;
						}
						CodeCompileUnit codeCompileUnit = compilerParamsAndBuildProvider.GetCodeCompileUnit(out linePragmasTable);
						buildResultCodeCompileUnit = new BuildResultCodeCompileUnit(codeDomProviderType, codeCompileUnit, compilerParameters, linePragmasTable);
						buildResultCodeCompileUnit.VirtualPath = virtualPath;
						buildResultCodeCompileUnit.SetCacheKey(text);
						this.FixupReferencedAssemblies(virtualPath, compilerParameters);
						if (codeCompileUnit != null)
						{
							foreach (string text2 in compilerParameters.ReferencedAssemblies)
							{
								codeCompileUnit.ReferencedAssemblies.Add(text2);
							}
						}
						ICollection virtualPathDependencies = compilerParamsAndBuildProvider.VirtualPathDependencies;
						if (virtualPathDependencies != null)
						{
							buildResultCodeCompileUnit.AddVirtualPathDependencies(virtualPathDependencies);
						}
						BuildManager.CacheBuildResult(text, buildResultCodeCompileUnit, utcNow);
						return codeCompileUnit;
					}
				}
				codeDomProviderType = buildResultCodeCompileUnit.CodeDomProviderType;
				compilerParameters = buildResultCodeCompileUnit.CompilerParameters;
				linePragmasTable = buildResultCodeCompileUnit.LinePragmasTable;
				this.FixupReferencedAssemblies(virtualPath, compilerParameters);
				codeCompileUnit2 = buildResultCodeCompileUnit.CodeCompileUnit;
			}
			finally
			{
				if (virtualFileString != null)
				{
					this._virtualPathProvider.RevertVirtualFile(virtualPath);
				}
				BuildManager.SkipTopLevelCompilationExceptions = false;
				this.RemovePendingCall();
			}
			return codeCompileUnit2;
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0004305C File Offset: 0x0004205C
		internal bool IsCodeAssembly(string assemblyName)
		{
			return BuildManager.GetNormalizedCodeAssemblyName(assemblyName) != null;
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0004306C File Offset: 0x0004206C
		private void FixupReferencedAssemblies(VirtualPath virtualPath, CompilerParameters compilerParameters)
		{
			CompilationSection compilation = RuntimeConfig.GetConfig(virtualPath).Compilation;
			ICollection referencedAssemblies = BuildManager.GetReferencedAssemblies(compilation);
			Util.AddAssembliesToStringCollection(referencedAssemblies, compilerParameters.ReferencedAssemblies);
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00043098 File Offset: 0x00042098
		private BuildProvider GetCompilerParamsAndBuildProvider(VirtualPath virtualPath, out Type codeDomProviderType, out CompilerParameters compilerParameters)
		{
			virtualPath.CombineWithAppRoot();
			CompilationSection compilation = RuntimeConfig.GetConfig(virtualPath).Compilation;
			ICollection referencedAssemblies = BuildManager.GetReferencedAssemblies(compilation);
			BuildProvider buildProvider;
			if (StringUtil.EqualsIgnoreCase(virtualPath.VirtualPathString, BuildManager.GlobalAsaxVirtualPath.VirtualPathString))
			{
				ApplicationBuildProvider applicationBuildProvider = new ApplicationBuildProvider();
				applicationBuildProvider.SetVirtualPath(virtualPath);
				applicationBuildProvider.SetReferencedAssemblies(referencedAssemblies);
				buildProvider = applicationBuildProvider;
			}
			else
			{
				buildProvider = BuildManager.CreateBuildProvider(virtualPath, compilation, referencedAssemblies, true);
			}
			buildProvider.IgnoreParseErrors = true;
			buildProvider.IgnoreControlProperties = true;
			buildProvider.ThrowOnFirstParseError = false;
			CompilerType codeCompilerType = buildProvider.CodeCompilerType;
			if (codeCompilerType == null)
			{
				codeDomProviderType = null;
				compilerParameters = null;
				return null;
			}
			codeDomProviderType = codeCompilerType.CodeDomProviderType;
			compilerParameters = codeCompilerType.CompilerParameters;
			IAssemblyDependencyParser assemblyDependencyParser = buildProvider.AssemblyDependencyParser;
			if (assemblyDependencyParser != null && assemblyDependencyParser.AssemblyDependencies != null)
			{
				Util.AddAssembliesToStringCollection(assemblyDependencyParser.AssemblyDependencies, compilerParameters.ReferencedAssemblies);
			}
			AssemblyBuilder.FixUpCompilerParameters(codeDomProviderType, compilerParameters);
			return buildProvider;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00043167 File Offset: 0x00042167
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0004316C File Offset: 0x0004216C
		internal void Configure(ClientBuildManager client)
		{
			this.AddPendingCall();
			try
			{
				this._virtualPathProvider = new BuildManagerHost.ClientVirtualPathProvider();
				HostingEnvironment.RegisterVirtualPathProviderInternal(this._virtualPathProvider);
				this._client = client;
				this._onAppDomainUnload = new EventHandler(this.OnAppDomainUnload);
				Thread.GetDomain().DomainUnload += this._onAppDomainUnload;
				this._buildManager = BuildManager.TheBuildManager;
				HttpRuntime.AppDomainShutdown += this.OnAppDomainShutdown;
			}
			finally
			{
				this.RemovePendingCall();
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x000431F4 File Offset: 0x000421F4
		internal Exception InitializationException
		{
			get
			{
				return HostingEnvironment.InitializationException;
			}
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x000431FB File Offset: 0x000421FB
		private void OnAppDomainUnload(object unusedObject, EventArgs unusedEventArgs)
		{
			Thread.GetDomain().DomainUnload -= this._onAppDomainUnload;
			if (this._client != null)
			{
				this._client.OnAppDomainUnloaded(HttpRuntime.ShutdownReason);
				this._client = null;
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0004322C File Offset: 0x0004222C
		internal bool UnloadAppDomain()
		{
			this._ignorePendingCalls = true;
			HttpRuntime.SetUserForcedShutdown();
			return HttpRuntime.ShutdownAppDomain(ApplicationShutdownReason.UnloadAppDomainCalled, "CBM called UnloadAppDomain");
		}

		// Token: 0x0400159F RID: 5535
		private ClientBuildManager _client;

		// Token: 0x040015A0 RID: 5536
		private BuildManager _buildManager;

		// Token: 0x040015A1 RID: 5537
		private int _pendingCallsCount;

		// Token: 0x040015A2 RID: 5538
		private EventHandler _onAppDomainUnload;

		// Token: 0x040015A3 RID: 5539
		private bool _ignorePendingCalls;

		// Token: 0x040015A4 RID: 5540
		private IDictionary _assemblyCollection;

		// Token: 0x040015A5 RID: 5541
		private object _lock = new object();

		// Token: 0x040015A6 RID: 5542
		private static bool _inClientBuildManager;

		// Token: 0x040015A7 RID: 5543
		private BuildManagerHost.ClientVirtualPathProvider _virtualPathProvider;

		// Token: 0x02000138 RID: 312
		internal class ClientVirtualPathProvider : VirtualPathProvider
		{
			// Token: 0x06000EDF RID: 3807 RVA: 0x0004352D File Offset: 0x0004252D
			internal ClientVirtualPathProvider()
			{
				this._stringDictionary = new HybridDictionary(true);
			}

			// Token: 0x06000EE0 RID: 3808 RVA: 0x00043541 File Offset: 0x00042541
			public override bool FileExists(string virtualPath)
			{
				return this._stringDictionary.Contains(virtualPath) || base.FileExists(virtualPath);
			}

			// Token: 0x06000EE1 RID: 3809 RVA: 0x0004355A File Offset: 0x0004255A
			public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
			{
				if (virtualPath != null)
				{
					virtualPath = UrlPath.MakeVirtualPathAppAbsolute(virtualPath);
					if (this._stringDictionary.Contains(virtualPath))
					{
						return null;
					}
				}
				return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
			}

			// Token: 0x06000EE2 RID: 3810 RVA: 0x00043580 File Offset: 0x00042580
			public override VirtualFile GetFile(string virtualPath)
			{
				string text = (string)this._stringDictionary[virtualPath];
				if (text == null)
				{
					return base.GetFile(virtualPath);
				}
				return new BuildManagerHost.ClientVirtualPathProvider.ClientVirtualFile(virtualPath, text);
			}

			// Token: 0x06000EE3 RID: 3811 RVA: 0x000435B4 File Offset: 0x000425B4
			public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
			{
				HashCodeCombiner hashCodeCombiner = null;
				ArrayList arrayList = new ArrayList();
				foreach (object obj in virtualPathDependencies)
				{
					string text = (string)obj;
					if (this._stringDictionary.Contains(text))
					{
						if (hashCodeCombiner == null)
						{
							hashCodeCombiner = new HashCodeCombiner();
						}
						hashCodeCombiner.AddInt(this._stringDictionary[text].GetHashCode());
					}
					else
					{
						arrayList.Add(text);
					}
				}
				if (hashCodeCombiner == null)
				{
					return base.GetFileHash(virtualPath, virtualPathDependencies);
				}
				hashCodeCombiner.AddObject(base.GetFileHash(virtualPath, arrayList));
				return hashCodeCombiner.CombinedHashString;
			}

			// Token: 0x06000EE4 RID: 3812 RVA: 0x00043664 File Offset: 0x00042664
			internal void RegisterVirtualFile(VirtualPath virtualPath, string virtualFileString)
			{
				this._stringDictionary[virtualPath.VirtualPathString] = virtualFileString;
			}

			// Token: 0x06000EE5 RID: 3813 RVA: 0x00043678 File Offset: 0x00042678
			internal void RevertVirtualFile(VirtualPath virtualPath)
			{
				this._stringDictionary.Remove(virtualPath.VirtualPathString);
			}

			// Token: 0x040015A9 RID: 5545
			private IDictionary _stringDictionary;

			// Token: 0x0200013B RID: 315
			internal class ClientVirtualFile : VirtualFile
			{
				// Token: 0x06000EEF RID: 3823 RVA: 0x000436CF File Offset: 0x000426CF
				internal ClientVirtualFile(string virtualPath, string virtualFileString)
					: base(virtualPath)
				{
					this._virtualFileString = virtualFileString;
				}

				// Token: 0x06000EF0 RID: 3824 RVA: 0x000436E0 File Offset: 0x000426E0
				public override Stream Open()
				{
					Stream stream = new MemoryStream();
					StreamWriter streamWriter = new StreamWriter(stream, Encoding.Unicode);
					streamWriter.Write(this._virtualFileString);
					streamWriter.Flush();
					stream.Seek(0L, SeekOrigin.Begin);
					return stream;
				}

				// Token: 0x040015AB RID: 5547
				private string _virtualFileString;
			}
		}
	}
}

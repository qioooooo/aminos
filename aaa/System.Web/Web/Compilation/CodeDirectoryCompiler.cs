using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000163 RID: 355
	internal class CodeDirectoryCompiler
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x00046CCE File Offset: 0x00045CCE
		internal static bool IsResourceCodeDirectoryType(CodeDirectoryType dirType)
		{
			return dirType == CodeDirectoryType.AppResources || dirType == CodeDirectoryType.LocalResources;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00046CDC File Offset: 0x00045CDC
		internal static Assembly GetCodeDirectoryAssembly(VirtualPath virtualDir, CodeDirectoryType dirType, string assemblyName, StringSet excludedSubdirectories, bool isDirectoryAllowed)
		{
			string text = virtualDir.MapPath();
			if (!isDirectoryAllowed && Directory.Exists(text))
			{
				throw new HttpException(SR.GetString("Bar_dir_in_precompiled_app", new object[] { virtualDir }));
			}
			bool flag = CodeDirectoryCompiler.IsResourceCodeDirectoryType(dirType);
			BuildResult buildResult = BuildManager.GetBuildResultFromCache(assemblyName);
			Assembly assembly = null;
			if (buildResult != null && buildResult is BuildResultCompiledAssembly)
			{
				if (buildResult is BuildResultMainCodeAssembly)
				{
					CodeDirectoryCompiler._mainCodeBuildResult = (BuildResultMainCodeAssembly)buildResult;
				}
				assembly = ((BuildResultCompiledAssembly)buildResult).ResultAssembly;
				if (!flag)
				{
					return assembly;
				}
				if (!isDirectoryAllowed)
				{
					return assembly;
				}
				BuildResultResourceAssembly buildResultResourceAssembly = (BuildResultResourceAssembly)buildResult;
				string directoryHash = HashCodeCombiner.GetDirectoryHash(virtualDir);
				if (directoryHash == buildResultResourceAssembly.ResourcesDependenciesHash)
				{
					return assembly;
				}
			}
			if (!isDirectoryAllowed)
			{
				return null;
			}
			if (dirType != CodeDirectoryType.LocalResources && !StringUtil.StringStartsWithIgnoreCase(text, HttpRuntime.AppDomainAppPathInternal))
			{
				throw new HttpException(SR.GetString("Virtual_codedir", new object[] { virtualDir.VirtualPathString }));
			}
			if (!Directory.Exists(text))
			{
				if (dirType != CodeDirectoryType.MainCode)
				{
					return null;
				}
				if (!ProfileBuildProvider.HasCompilableProfile)
				{
					return null;
				}
			}
			BuildManager.ReportDirectoryCompilationProgress(virtualDir);
			DateTime utcNow = DateTime.UtcNow;
			CodeDirectoryCompiler codeDirectoryCompiler = new CodeDirectoryCompiler(virtualDir, dirType, excludedSubdirectories);
			string text2;
			if (assembly != null)
			{
				text2 = assembly.GetName().Name;
				codeDirectoryCompiler._onlyBuildLocalizedResources = true;
			}
			else
			{
				text2 = BuildManager.GenerateRandomAssemblyName(assemblyName);
			}
			BuildProvidersCompiler buildProvidersCompiler = new BuildProvidersCompiler(virtualDir, flag, text2);
			codeDirectoryCompiler._bpc = buildProvidersCompiler;
			codeDirectoryCompiler.FindBuildProviders();
			buildProvidersCompiler.SetBuildProviders(codeDirectoryCompiler._buildProviders);
			CompilerResults compilerResults = buildProvidersCompiler.PerformBuild();
			if (compilerResults != null)
			{
				DateTime dateTime = DateTime.UtcNow.AddMilliseconds(3000.0);
				do
				{
					IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(compilerResults.PathToAssembly);
					if (moduleHandle == IntPtr.Zero)
					{
						goto IL_01D7;
					}
					Thread.Sleep(250);
				}
				while (!(DateTime.UtcNow > dateTime));
				throw new HttpException(SR.GetString("Assembly_already_loaded", new object[] { compilerResults.PathToAssembly }));
				IL_01D7:
				assembly = compilerResults.CompiledAssembly;
			}
			if (assembly == null)
			{
				return null;
			}
			if (dirType == CodeDirectoryType.MainCode)
			{
				CodeDirectoryCompiler._mainCodeBuildResult = new BuildResultMainCodeAssembly(assembly);
				buildResult = CodeDirectoryCompiler._mainCodeBuildResult;
			}
			else if (flag)
			{
				buildResult = new BuildResultResourceAssembly(assembly);
			}
			else
			{
				buildResult = new BuildResultCompiledAssembly(assembly);
			}
			buildResult.VirtualPath = virtualDir;
			if (BuildManager.OptimizeCompilations && dirType != CodeDirectoryType.LocalResources)
			{
				buildResult.AddVirtualPathDependencies(new SingleObjectCollection(virtualDir.AppRelativeVirtualPathString));
			}
			if (dirType != CodeDirectoryType.LocalResources)
			{
				buildResult.CacheToMemory = false;
			}
			BuildManager.CacheBuildResult(assemblyName, buildResult, utcNow);
			return assembly;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00046F35 File Offset: 0x00045F35
		internal static void CallAppInitializeMethod()
		{
			if (CodeDirectoryCompiler._mainCodeBuildResult != null)
			{
				CodeDirectoryCompiler._mainCodeBuildResult.CallAppInitializeMethod();
			}
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00046F48 File Offset: 0x00045F48
		internal static void GetCodeDirectoryInformation(VirtualPath virtualDir, CodeDirectoryType dirType, StringSet excludedSubdirectories, int index, out Type codeDomProviderType, out CompilerParameters compilerParameters, out string generatedFilesDir)
		{
			generatedFilesDir = HttpRuntime.CodegenDirInternal + "\\Sources_" + virtualDir.FileName;
			bool flag = CodeDirectoryCompiler.IsResourceCodeDirectoryType(dirType);
			BuildProvidersCompiler buildProvidersCompiler = new BuildProvidersCompiler(virtualDir, flag, generatedFilesDir, index);
			CodeDirectoryCompiler codeDirectoryCompiler = new CodeDirectoryCompiler(virtualDir, dirType, excludedSubdirectories);
			codeDirectoryCompiler._bpc = buildProvidersCompiler;
			codeDirectoryCompiler.FindBuildProviders();
			buildProvidersCompiler.SetBuildProviders(codeDirectoryCompiler._buildProviders);
			buildProvidersCompiler.GenerateSources(out codeDomProviderType, out compilerParameters);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00046FAC File Offset: 0x00045FAC
		private CodeDirectoryCompiler(VirtualPath virtualDir, CodeDirectoryType dirType, StringSet excludedSubdirectories)
		{
			this._virtualDir = virtualDir;
			this._dirType = dirType;
			this._excludedSubdirectories = excludedSubdirectories;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00046FD4 File Offset: 0x00045FD4
		private void FindBuildProviders()
		{
			if (this._dirType == CodeDirectoryType.MainCode && ProfileBuildProvider.HasCompilableProfile)
			{
				this._buildProviders.Add(ProfileBuildProvider.Create());
			}
			VirtualDirectory directory = HostingEnvironment.VirtualPathProvider.GetDirectory(this._virtualDir);
			this.ProcessDirectoryRecursive(directory, true);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0004701C File Offset: 0x0004601C
		private void ProcessDirectoryRecursive(VirtualDirectory vdir, bool topLevel)
		{
			if (this._dirType == CodeDirectoryType.WebReferences)
			{
				BuildProvider buildProvider = new WebReferencesBuildProvider(vdir);
				buildProvider.SetVirtualPath(vdir.VirtualPathObject);
				this._buildProviders.Add(buildProvider);
			}
			foreach (object obj in vdir.Children)
			{
				VirtualFileBase virtualFileBase = (VirtualFileBase)obj;
				if (virtualFileBase.IsDirectory)
				{
					if ((!topLevel || this._excludedSubdirectories == null || !this._excludedSubdirectories.Contains(virtualFileBase.Name)) && !(virtualFileBase.Name == "_vti_cnf"))
					{
						this.ProcessDirectoryRecursive(virtualFileBase as VirtualDirectory, false);
					}
				}
				else if (this._dirType != CodeDirectoryType.WebReferences && (!CodeDirectoryCompiler.IsResourceCodeDirectoryType(this._dirType) || !this._onlyBuildLocalizedResources || Util.GetCultureName(virtualFileBase.VirtualPath) != null))
				{
					BuildProvider buildProvider2 = BuildManager.CreateBuildProvider(virtualFileBase.VirtualPathObject, CodeDirectoryCompiler.IsResourceCodeDirectoryType(this._dirType) ? BuildProviderAppliesTo.Resources : BuildProviderAppliesTo.Code, this._bpc.CompConfig, this._bpc.ReferencedAssemblies, false);
					if (buildProvider2 != null)
					{
						if (this._dirType == CodeDirectoryType.LocalResources && buildProvider2 is BaseResourcesBuildProvider)
						{
							((BaseResourcesBuildProvider)buildProvider2).DontGenerateStronglyTypedClass();
						}
						this._buildProviders.Add(buildProvider2);
					}
				}
			}
		}

		// Token: 0x04001631 RID: 5681
		internal const string sourcesDirectoryPrefix = "Sources_";

		// Token: 0x04001632 RID: 5682
		private VirtualPath _virtualDir;

		// Token: 0x04001633 RID: 5683
		private CodeDirectoryType _dirType;

		// Token: 0x04001634 RID: 5684
		private StringSet _excludedSubdirectories;

		// Token: 0x04001635 RID: 5685
		private BuildProvidersCompiler _bpc;

		// Token: 0x04001636 RID: 5686
		private BuildProviderSet _buildProviders = new BuildProviderSet();

		// Token: 0x04001637 RID: 5687
		private bool _onlyBuildLocalizedResources;

		// Token: 0x04001638 RID: 5688
		internal static BuildResultMainCodeAssembly _mainCodeBuildResult;
	}
}

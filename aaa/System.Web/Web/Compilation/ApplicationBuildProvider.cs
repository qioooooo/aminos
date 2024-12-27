using System;
using System.CodeDom.Compiler;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000125 RID: 293
	internal class ApplicationBuildProvider : BaseTemplateBuildProvider
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x00036910 File Offset: 0x00035910
		internal static BuildResultCompiledGlobalAsaxType GetGlobalAsaxBuildResult(bool isPrecompiledApp)
		{
			string text = "App_global.asax";
			BuildResultCompiledGlobalAsaxType buildResultCompiledGlobalAsaxType = BuildManager.GetBuildResultFromCache(text) as BuildResultCompiledGlobalAsaxType;
			if (buildResultCompiledGlobalAsaxType != null)
			{
				return buildResultCompiledGlobalAsaxType;
			}
			if (isPrecompiledApp)
			{
				return null;
			}
			VirtualPath globalAsaxVirtualPath = BuildManager.GlobalAsaxVirtualPath;
			if (!globalAsaxVirtualPath.FileExists())
			{
				return null;
			}
			ApplicationBuildProvider applicationBuildProvider = new ApplicationBuildProvider();
			applicationBuildProvider.SetVirtualPath(globalAsaxVirtualPath);
			DateTime utcNow = DateTime.UtcNow;
			BuildProvidersCompiler buildProvidersCompiler = new BuildProvidersCompiler(globalAsaxVirtualPath, BuildManager.GenerateRandomAssemblyName("App_global.asax"));
			buildProvidersCompiler.SetBuildProviders(new SingleObjectCollection(applicationBuildProvider));
			CompilerResults compilerResults = buildProvidersCompiler.PerformBuild();
			buildResultCompiledGlobalAsaxType = (BuildResultCompiledGlobalAsaxType)applicationBuildProvider.GetBuildResult(compilerResults);
			buildResultCompiledGlobalAsaxType.CacheToMemory = false;
			BuildManager.CacheBuildResult(text, buildResultCompiledGlobalAsaxType, utcNow);
			return buildResultCompiledGlobalAsaxType;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x000369A5 File Offset: 0x000359A5
		protected override TemplateParser CreateParser()
		{
			return new ApplicationFileParser();
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000369AC File Offset: 0x000359AC
		internal override BaseCodeDomTreeGenerator CreateCodeDomTreeGenerator(TemplateParser parser)
		{
			return new ApplicationFileCodeDomTreeGenerator((ApplicationFileParser)parser);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000369BC File Offset: 0x000359BC
		internal override BuildResultCompiledType CreateBuildResult(Type t)
		{
			BuildResultCompiledGlobalAsaxType buildResultCompiledGlobalAsaxType = new BuildResultCompiledGlobalAsaxType(t);
			if (base.Parser.ApplicationObjects != null || base.Parser.SessionObjects != null)
			{
				buildResultCompiledGlobalAsaxType.HasAppOrSessionObjects = true;
			}
			return buildResultCompiledGlobalAsaxType;
		}
	}
}

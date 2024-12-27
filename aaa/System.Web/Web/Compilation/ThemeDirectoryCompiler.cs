using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000195 RID: 405
	internal static class ThemeDirectoryCompiler
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x0004CC3F File Offset: 0x0004BC3F
		internal static VirtualPath GetAppThemeVirtualDir(string themeName)
		{
			return HttpRuntime.AppDomainAppVirtualPathObject.SimpleCombineWithDir("App_Themes/" + themeName);
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0004CC56 File Offset: 0x0004BC56
		internal static VirtualPath GetGlobalThemeVirtualDir(string themeName)
		{
			return BuildManager.ScriptVirtualDir.SimpleCombineWithDir("Themes/" + themeName);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x0004CC70 File Offset: 0x0004BC70
		[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
		internal static BuildResultCompiledType GetThemeBuildResultType(HttpContext context, string themeName)
		{
			BuildResultCompiledType themeBuildResultType;
			using (new ApplicationImpersonationContext())
			{
				themeBuildResultType = ThemeDirectoryCompiler.GetThemeBuildResultType(themeName);
			}
			return themeBuildResultType;
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0004CCA8 File Offset: 0x0004BCA8
		private static BuildResultCompiledType GetThemeBuildResultType(string themeName)
		{
			string text = null;
			string text2 = "Theme_" + Util.MakeValidTypeNameFromString(themeName);
			BuildResultCompiledType buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetBuildResultFromCache(text2);
			if (buildResultCompiledType == null)
			{
				text = "GlobalTheme_" + themeName;
				buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetBuildResultFromCache(text);
			}
			if (buildResultCompiledType != null)
			{
				return buildResultCompiledType;
			}
			bool flag = false;
			try
			{
				CompilationLock.GetLock(ref flag);
				buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetBuildResultFromCache(text2);
				if (buildResultCompiledType == null)
				{
					buildResultCompiledType = (BuildResultCompiledType)BuildManager.GetBuildResultFromCache(text);
				}
				if (buildResultCompiledType != null)
				{
					return buildResultCompiledType;
				}
				VirtualPath appThemeVirtualDir = ThemeDirectoryCompiler.GetAppThemeVirtualDir(themeName);
				VirtualPath virtualPath = appThemeVirtualDir;
				string text3 = text2;
				PageThemeBuildProvider pageThemeBuildProvider;
				if (appThemeVirtualDir.DirectoryExists())
				{
					pageThemeBuildProvider = new PageThemeBuildProvider(appThemeVirtualDir);
				}
				else
				{
					VirtualPath globalThemeVirtualDir = ThemeDirectoryCompiler.GetGlobalThemeVirtualDir(themeName);
					if (!globalThemeVirtualDir.DirectoryExists())
					{
						throw new HttpException(SR.GetString("Page_theme_not_found", new object[] { themeName }));
					}
					virtualPath = globalThemeVirtualDir;
					text3 = text;
					pageThemeBuildProvider = new GlobalPageThemeBuildProvider(globalThemeVirtualDir);
				}
				DateTime utcNow = DateTime.UtcNow;
				VirtualDirectory directory = virtualPath.GetDirectory();
				ThemeDirectoryCompiler.AddThemeFilesToBuildProvider(directory, pageThemeBuildProvider, true);
				BuildProvidersCompiler buildProvidersCompiler = new BuildProvidersCompiler(virtualPath, pageThemeBuildProvider.AssemblyNamePrefix + BuildManager.GenerateRandomAssemblyName(themeName));
				buildProvidersCompiler.SetBuildProviders(new SingleObjectCollection(pageThemeBuildProvider));
				CompilerResults compilerResults = buildProvidersCompiler.PerformBuild();
				buildResultCompiledType = (BuildResultCompiledType)pageThemeBuildProvider.GetBuildResult(compilerResults);
				BuildManager.CacheBuildResult(text3, buildResultCompiledType, utcNow);
			}
			finally
			{
				if (flag)
				{
					CompilationLock.ReleaseLock();
				}
			}
			return buildResultCompiledType;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0004CE14 File Offset: 0x0004BE14
		private static void AddThemeFilesToBuildProvider(VirtualDirectory vdir, PageThemeBuildProvider themeBuildProvider, bool topLevel)
		{
			foreach (object obj in vdir.Children)
			{
				VirtualFileBase virtualFileBase = (VirtualFileBase)obj;
				if (virtualFileBase.IsDirectory)
				{
					ThemeDirectoryCompiler.AddThemeFilesToBuildProvider(virtualFileBase as VirtualDirectory, themeBuildProvider, false);
				}
				else
				{
					string extension = Path.GetExtension(virtualFileBase.Name);
					if (StringUtil.EqualsIgnoreCase(extension, ".skin") && topLevel)
					{
						themeBuildProvider.AddSkinFile(virtualFileBase.VirtualPathObject);
					}
					else if (StringUtil.EqualsIgnoreCase(extension, ".css"))
					{
						themeBuildProvider.AddCssFile(virtualFileBase.VirtualPathObject);
					}
				}
			}
		}

		// Token: 0x04001691 RID: 5777
		internal const string skinExtension = ".skin";
	}
}

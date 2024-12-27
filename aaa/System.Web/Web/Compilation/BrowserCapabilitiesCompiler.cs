using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x0200012E RID: 302
	internal static class BrowserCapabilitiesCompiler
	{
		// Token: 0x06000DD1 RID: 3537 RVA: 0x0003C344 File Offset: 0x0003B344
		static BrowserCapabilitiesCompiler()
		{
			Assembly assembly = null;
			string browserCapAssemblyPublicKeyToken = BrowserCapabilitiesCodeGenerator.BrowserCapAssemblyPublicKeyToken;
			if (browserCapAssemblyPublicKeyToken != null)
			{
				try
				{
					assembly = Assembly.Load("ASP.BrowserCapsFactory, Version=2.0.0.0, Culture=neutral, PublicKeyToken=" + browserCapAssemblyPublicKeyToken);
				}
				catch (FileNotFoundException)
				{
				}
			}
			if (assembly != null && assembly.GlobalAssemblyCache)
			{
				BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseType = assembly.GetType("ASP.BrowserCapabilitiesFactory", true);
				BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseTypeForCompilation = BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseType;
				return;
			}
			BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseTypeForCompilation = typeof(BrowserCapabilitiesFactory);
			if (AppSettings.UseLegacyBrowserCaps)
			{
				BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseType = BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseTypeForCompilation;
				return;
			}
			BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseType = typeof(BrowserCapabilitiesFactory2);
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0003C3F8 File Offset: 0x0003B3F8
		internal static BrowserCapabilitiesFactoryBase BrowserCapabilitiesFactory
		{
			get
			{
				if (BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseInstance != null)
				{
					return BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseInstance;
				}
				Type browserCapabilitiesType = BrowserCapabilitiesCompiler.GetBrowserCapabilitiesType();
				lock (BrowserCapabilitiesCompiler._lockObject)
				{
					if (BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseInstance == null && browserCapabilitiesType != null)
					{
						BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseInstance = (BrowserCapabilitiesFactoryBase)Activator.CreateInstance(browserCapabilitiesType);
					}
				}
				return BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseInstance;
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0003C45C File Offset: 0x0003B45C
		internal static Type GetBrowserCapabilitiesFactoryBaseType()
		{
			return BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseTypeForCompilation;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0003C464 File Offset: 0x0003B464
		internal static Type GetBrowserCapabilitiesType()
		{
			InternalSecurityPermissions.Unrestricted.Assert();
			BuildResult buildResult = null;
			try
			{
				buildResult = BuildManager.GetBuildResultFromCache("__browserCapabilitiesCompiler");
				if (buildResult == null)
				{
					DateTime utcNow = DateTime.UtcNow;
					VirtualDirectory directory = BrowserCapabilitiesCompiler.AppBrowsersVirtualDir.GetDirectory();
					string text = HostingEnvironment.MapPathInternal(BrowserCapabilitiesCompiler.AppBrowsersVirtualDir);
					if (directory != null && Directory.Exists(text))
					{
						ArrayList arrayList = new ArrayList();
						ArrayList arrayList2 = new ArrayList();
						bool flag = BrowserCapabilitiesCompiler.AddBrowserFilesToList(directory, arrayList, false);
						if (flag)
						{
							BrowserCapabilitiesCompiler.AddBrowserFilesToList(directory, arrayList2, true);
						}
						else
						{
							arrayList2 = arrayList;
						}
						if (arrayList2.Count > 0)
						{
							ApplicationBrowserCapabilitiesBuildProvider applicationBrowserCapabilitiesBuildProvider = new ApplicationBrowserCapabilitiesBuildProvider();
							foreach (object obj in arrayList)
							{
								string text2 = (string)obj;
								applicationBrowserCapabilitiesBuildProvider.AddFile(text2);
							}
							BuildProvidersCompiler buildProvidersCompiler = new BuildProvidersCompiler(null, BuildManager.GenerateRandomAssemblyName("App_Browsers"));
							buildProvidersCompiler.SetBuildProviders(new SingleObjectCollection(applicationBrowserCapabilitiesBuildProvider));
							CompilerResults compilerResults = buildProvidersCompiler.PerformBuild();
							Assembly compiledAssembly = compilerResults.CompiledAssembly;
							Type type = compiledAssembly.GetType("ASP.ApplicationBrowserCapabilitiesFactory");
							buildResult = new BuildResultCompiledType(type);
							buildResult.VirtualPath = BrowserCapabilitiesCompiler.AppBrowsersVirtualDir;
							buildResult.AddVirtualPathDependencies(arrayList2);
							BuildManager.CacheBuildResult("__browserCapabilitiesCompiler", buildResult, utcNow);
						}
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (buildResult == null)
			{
				return BrowserCapabilitiesCompiler._browserCapabilitiesFactoryBaseType;
			}
			return ((BuildResultCompiledType)buildResult).ResultType;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0003C5F8 File Offset: 0x0003B5F8
		private static bool AddBrowserFilesToList(VirtualDirectory directory, IList list, bool doRecurse)
		{
			bool flag = false;
			foreach (object obj in directory.Children)
			{
				VirtualFileBase virtualFileBase = (VirtualFileBase)obj;
				if (virtualFileBase.IsDirectory)
				{
					if (doRecurse)
					{
						BrowserCapabilitiesCompiler.AddBrowserFilesToList((VirtualDirectory)virtualFileBase, list, true);
					}
					flag = true;
				}
				else
				{
					string extension = Path.GetExtension(virtualFileBase.Name);
					if (StringUtil.EqualsIgnoreCase(extension, ".browser"))
					{
						list.Add(virtualFileBase.VirtualPath);
					}
				}
			}
			return flag;
		}

		// Token: 0x0400152F RID: 5423
		private const string browerCapabilitiesTypeName = "BrowserCapabilities";

		// Token: 0x04001530 RID: 5424
		private const string browerCapabilitiesCacheKey = "__browserCapabilitiesCompiler";

		// Token: 0x04001531 RID: 5425
		internal static readonly VirtualPath AppBrowsersVirtualDir = HttpRuntime.AppDomainAppVirtualPathObject.SimpleCombineWithDir("App_Browsers");

		// Token: 0x04001532 RID: 5426
		private static Type _browserCapabilitiesFactoryBaseType;

		// Token: 0x04001533 RID: 5427
		private static Type _browserCapabilitiesFactoryBaseTypeForCompilation;

		// Token: 0x04001534 RID: 5428
		private static Type _browserCapabilitiesFactoryType;

		// Token: 0x04001535 RID: 5429
		private static BrowserCapabilitiesFactoryBase _browserCapabilitiesFactoryBaseInstance;

		// Token: 0x04001536 RID: 5430
		private static object _lockObject = new object();
	}
}

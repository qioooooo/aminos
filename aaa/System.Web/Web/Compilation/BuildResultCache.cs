using System;
using System.IO;
using System.Reflection;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000153 RID: 339
	internal abstract class BuildResultCache
	{
		// Token: 0x06000F8A RID: 3978 RVA: 0x00045617 File Offset: 0x00044617
		internal BuildResult GetBuildResult(string cacheKey)
		{
			return this.GetBuildResult(cacheKey, null, 0L);
		}

		// Token: 0x06000F8B RID: 3979
		internal abstract BuildResult GetBuildResult(string cacheKey, VirtualPath virtualPath, long hashCode);

		// Token: 0x06000F8C RID: 3980 RVA: 0x00045623 File Offset: 0x00044623
		internal void CacheBuildResult(string cacheKey, BuildResult result, DateTime utcStart)
		{
			this.CacheBuildResult(cacheKey, result, 0L, utcStart);
		}

		// Token: 0x06000F8D RID: 3981
		internal abstract void CacheBuildResult(string cacheKey, BuildResult result, long hashCode, DateTime utcStart);

		// Token: 0x06000F8E RID: 3982 RVA: 0x00045630 File Offset: 0x00044630
		internal static string GetAssemblyCacheKey(string assemblyPath)
		{
			string assemblyNameFromFileName = Util.GetAssemblyNameFromFileName(Path.GetFileName(assemblyPath));
			return BuildResultCache.GetAssemblyCacheKeyFromName(assemblyNameFromFileName);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x0004564F File Offset: 0x0004464F
		internal static string GetAssemblyCacheKey(Assembly assembly)
		{
			return BuildResultCache.GetAssemblyCacheKeyFromName(assembly.GetName().Name);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00045661 File Offset: 0x00044661
		internal static string GetAssemblyCacheKeyFromName(string assemblyName)
		{
			return "y" + assemblyName.ToLowerInvariant();
		}
	}
}

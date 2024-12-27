using System;

namespace System.Web.Compilation
{
	// Token: 0x02000159 RID: 345
	internal class UpdatablePrecompilerDiskBuildResultCache : PrecompilerDiskBuildResultCache
	{
		// Token: 0x06000FBA RID: 4026 RVA: 0x000464A0 File Offset: 0x000454A0
		internal UpdatablePrecompilerDiskBuildResultCache(string cacheDir)
			: base(cacheDir)
		{
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x000464A9 File Offset: 0x000454A9
		internal override void CacheBuildResult(string cacheKey, BuildResult result, long hashCode, DateTime utcStart)
		{
			if (result is BuildResultCompiledTemplateType)
			{
				return;
			}
			base.CacheBuildResult(cacheKey, result, hashCode, utcStart);
		}
	}
}

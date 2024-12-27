using System;

namespace System.Web.Compilation
{
	// Token: 0x0200015A RID: 346
	internal class PrecompiledSiteDiskBuildResultCache : PrecompBaseDiskBuildResultCache
	{
		// Token: 0x06000FBC RID: 4028 RVA: 0x000464BF File Offset: 0x000454BF
		internal PrecompiledSiteDiskBuildResultCache(string cacheDir)
			: base(cacheDir)
		{
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x000464C8 File Offset: 0x000454C8
		protected override bool PrecompilationMode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x000464CB File Offset: 0x000454CB
		internal override void CacheBuildResult(string cacheKey, BuildResult result, long hashCode, DateTime utcStart)
		{
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000464CD File Offset: 0x000454CD
		internal override void RemoveAssemblyAndRelatedFiles(string baseName)
		{
		}
	}
}

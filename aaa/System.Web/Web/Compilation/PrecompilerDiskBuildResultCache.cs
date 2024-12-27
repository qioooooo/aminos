using System;

namespace System.Web.Compilation
{
	// Token: 0x02000158 RID: 344
	internal class PrecompilerDiskBuildResultCache : PrecompBaseDiskBuildResultCache
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x00046491 File Offset: 0x00045491
		internal PrecompilerDiskBuildResultCache(string cacheDir)
			: base(cacheDir)
		{
			base.EnsureDiskCacheDirectoryCreated();
		}
	}
}

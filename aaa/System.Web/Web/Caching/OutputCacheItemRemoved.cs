using System;
using System.Threading;
using System.Web.Hosting;

namespace System.Web.Caching
{
	// Token: 0x020000B3 RID: 179
	internal class OutputCacheItemRemoved
	{
		// Token: 0x06000891 RID: 2193 RVA: 0x000265EA File Offset: 0x000255EA
		internal OutputCacheItemRemoved()
		{
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000265F4 File Offset: 0x000255F4
		internal void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
		{
			Interlocked.Decrement(ref OutputCacheModule.s_cEntries);
			PerfCounters.DecrementCounter(AppPerfCounter.OUTPUT_CACHE_ENTRIES);
			PerfCounters.IncrementCounter(AppPerfCounter.OUTPUT_CACHE_TURNOVER_RATE);
			CachedRawResponse cachedRawResponse = value as CachedRawResponse;
			if (cachedRawResponse != null)
			{
				string kernelCacheUrl = cachedRawResponse._kernelCacheUrl;
				if (kernelCacheUrl != null && HttpRuntime.CacheInternal[key] == null)
				{
					if (HttpRuntime.UseIntegratedPipeline)
					{
						UnsafeIISMethods.MgdFlushKernelCache(kernelCacheUrl);
						return;
					}
					UnsafeNativeMethods.InvalidateKernelCache(kernelCacheUrl);
				}
			}
		}
	}
}

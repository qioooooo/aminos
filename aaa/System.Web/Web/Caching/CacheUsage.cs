using System;
using System.Threading;

namespace System.Web.Caching
{
	// Token: 0x0200011B RID: 283
	internal class CacheUsage
	{
		// Token: 0x06000CDB RID: 3291 RVA: 0x00034E6C File Offset: 0x00033E6C
		internal CacheUsage(CacheSingle cacheSingle)
		{
			this._cacheSingle = cacheSingle;
			this._buckets = new UsageBucket[5];
			byte b = 0;
			while ((int)b < this._buckets.Length)
			{
				this._buckets[(int)b] = new UsageBucket(this, b);
				b += 1;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00034EB5 File Offset: 0x00033EB5
		internal CacheSingle CacheSingle
		{
			get
			{
				return this._cacheSingle;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00034EC0 File Offset: 0x00033EC0
		internal void Add(CacheEntry cacheEntry)
		{
			byte usageBucket = cacheEntry.UsageBucket;
			this._buckets[(int)usageBucket].AddCacheEntry(cacheEntry);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00034EE4 File Offset: 0x00033EE4
		internal void Remove(CacheEntry cacheEntry)
		{
			byte usageBucket = cacheEntry.UsageBucket;
			if (usageBucket != 255)
			{
				this._buckets[(int)usageBucket].RemoveCacheEntry(cacheEntry);
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00034F10 File Offset: 0x00033F10
		internal void Update(CacheEntry cacheEntry)
		{
			byte usageBucket = cacheEntry.UsageBucket;
			if (usageBucket != 255)
			{
				this._buckets[(int)usageBucket].UpdateCacheEntry(cacheEntry);
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00034F3C File Offset: 0x00033F3C
		internal int FlushUnderUsedItems(int toFlush, ref int publicEntriesFlushed)
		{
			int num = 0;
			if (Interlocked.Exchange(ref this._inFlush, 1) == 0)
			{
				try
				{
					foreach (UsageBucket usageBucket in this._buckets)
					{
						int num2 = usageBucket.FlushUnderUsedItems(toFlush - num, false, ref publicEntriesFlushed);
						num += num2;
						if (num >= toFlush)
						{
							break;
						}
					}
					if (num < toFlush)
					{
						foreach (UsageBucket usageBucket2 in this._buckets)
						{
							int num3 = usageBucket2.FlushUnderUsedItems(toFlush - num, true, ref publicEntriesFlushed);
							num += num3;
							if (num >= toFlush)
							{
								break;
							}
						}
					}
				}
				finally
				{
					Interlocked.Exchange(ref this._inFlush, 0);
				}
			}
			return num;
		}

		// Token: 0x040014AB RID: 5291
		private const byte NUMBUCKETS = 5;

		// Token: 0x040014AC RID: 5292
		private const int MAX_REMOVE = 1024;

		// Token: 0x040014AD RID: 5293
		internal static readonly TimeSpan NEWADD_INTERVAL = new TimeSpan(0, 0, 10);

		// Token: 0x040014AE RID: 5294
		internal static readonly TimeSpan CORRELATED_REQUEST_TIMEOUT = new TimeSpan(0, 0, 1);

		// Token: 0x040014AF RID: 5295
		internal static readonly TimeSpan MIN_LIFETIME_FOR_USAGE = CacheUsage.NEWADD_INTERVAL;

		// Token: 0x040014B0 RID: 5296
		private readonly CacheSingle _cacheSingle;

		// Token: 0x040014B1 RID: 5297
		internal readonly UsageBucket[] _buckets;

		// Token: 0x040014B2 RID: 5298
		private int _inFlush;
	}
}

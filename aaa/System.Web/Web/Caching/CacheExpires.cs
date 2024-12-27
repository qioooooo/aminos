using System;
using System.Threading;

namespace System.Web.Caching
{
	// Token: 0x02000121 RID: 289
	internal sealed class CacheExpires
	{
		// Token: 0x06000D04 RID: 3332 RVA: 0x00035F94 File Offset: 0x00034F94
		internal CacheExpires(CacheSingle cacheSingle)
		{
			DateTime utcNow = DateTime.UtcNow;
			this._cacheSingle = cacheSingle;
			this._buckets = new ExpiresBucket[30];
			byte b = 0;
			while ((int)b < this._buckets.Length)
			{
				this._buckets[(int)b] = new ExpiresBucket(this, b, utcNow);
				b += 1;
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00035FE8 File Offset: 0x00034FE8
		private int UtcCalcExpiresBucket(DateTime utcDate)
		{
			long num = utcDate.Ticks % CacheExpires._tsPerCycle.Ticks;
			return (int)((num / CacheExpires._tsPerBucket.Ticks + 1L) % 30L);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00036024 File Offset: 0x00035024
		private int FlushExpiredItems(bool checkDelta, bool useInsertBlock)
		{
			int num = 0;
			if (Interlocked.Exchange(ref this._inFlush, 1) == 0)
			{
				try
				{
					DateTime utcNow = DateTime.UtcNow;
					if (!checkDelta || utcNow - this._utcLastFlush >= CacheExpires.MIN_FLUSH_INTERVAL || utcNow < this._utcLastFlush)
					{
						this._utcLastFlush = utcNow;
						foreach (ExpiresBucket expiresBucket in this._buckets)
						{
							num += expiresBucket.FlushExpiredItems(utcNow, useInsertBlock);
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

		// Token: 0x06000D07 RID: 3335 RVA: 0x000360C4 File Offset: 0x000350C4
		internal int FlushExpiredItems(bool useInsertBlock)
		{
			return this.FlushExpiredItems(true, useInsertBlock);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000360CE File Offset: 0x000350CE
		private void TimerCallback(object state)
		{
			this.FlushExpiredItems(false, false);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x000360DC File Offset: 0x000350DC
		internal void EnableExpirationTimer(bool enable)
		{
			if (enable)
			{
				if (this._timer == null)
				{
					DateTime utcNow = DateTime.UtcNow;
					TimeSpan timeSpan = CacheExpires._tsPerBucket - new TimeSpan(utcNow.Ticks % CacheExpires._tsPerBucket.Ticks);
					this._timer = new Timer(new TimerCallback(this.TimerCallback), null, timeSpan.Ticks / 10000L, CacheExpires._tsPerBucket.Ticks / 10000L);
					return;
				}
			}
			else if (this._timer != null)
			{
				this._timer.Dispose();
				this._timer = null;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00036177 File Offset: 0x00035177
		internal CacheSingle CacheSingle
		{
			get
			{
				return this._cacheSingle;
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00036180 File Offset: 0x00035180
		internal void Add(CacheEntry cacheEntry)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (utcNow > cacheEntry.UtcExpires)
			{
				cacheEntry.UtcExpires = utcNow;
			}
			int num = this.UtcCalcExpiresBucket(cacheEntry.UtcExpires);
			this._buckets[num].AddCacheEntry(cacheEntry);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000361C4 File Offset: 0x000351C4
		internal void Remove(CacheEntry cacheEntry)
		{
			byte expiresBucket = cacheEntry.ExpiresBucket;
			if (expiresBucket != 255)
			{
				this._buckets[(int)expiresBucket].RemoveCacheEntry(cacheEntry);
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000361F0 File Offset: 0x000351F0
		internal void UtcUpdate(CacheEntry cacheEntry, DateTime utcNewExpires)
		{
			int expiresBucket = (int)cacheEntry.ExpiresBucket;
			int num = this.UtcCalcExpiresBucket(utcNewExpires);
			if (expiresBucket != num)
			{
				if (expiresBucket != 255)
				{
					this._buckets[expiresBucket].RemoveCacheEntry(cacheEntry);
					cacheEntry.UtcExpires = utcNewExpires;
					this._buckets[num].AddCacheEntry(cacheEntry);
					return;
				}
			}
			else if (expiresBucket != 255)
			{
				this._buckets[expiresBucket].UtcUpdateCacheEntry(cacheEntry, utcNewExpires);
			}
		}

		// Token: 0x040014D5 RID: 5333
		private const int NUMBUCKETS = 30;

		// Token: 0x040014D6 RID: 5334
		internal static readonly TimeSpan MIN_UPDATE_DELTA = new TimeSpan(0, 0, 1);

		// Token: 0x040014D7 RID: 5335
		internal static readonly TimeSpan MIN_FLUSH_INTERVAL = new TimeSpan(0, 0, 1);

		// Token: 0x040014D8 RID: 5336
		internal static readonly TimeSpan _tsPerBucket = new TimeSpan(0, 0, 20);

		// Token: 0x040014D9 RID: 5337
		private static readonly TimeSpan _tsPerCycle = new TimeSpan(30L * CacheExpires._tsPerBucket.Ticks);

		// Token: 0x040014DA RID: 5338
		private readonly CacheSingle _cacheSingle;

		// Token: 0x040014DB RID: 5339
		private readonly ExpiresBucket[] _buckets;

		// Token: 0x040014DC RID: 5340
		private Timer _timer;

		// Token: 0x040014DD RID: 5341
		private DateTime _utcLastFlush;

		// Token: 0x040014DE RID: 5342
		private int _inFlush;
	}
}

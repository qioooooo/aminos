using System;
using System.Collections;
using System.Threading;

namespace System.Web.Caching
{
	// Token: 0x02000101 RID: 257
	internal class CacheMultiple : CacheInternal
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x0002FD88 File Offset: 0x0002ED88
		internal CacheMultiple(CacheCommon cacheCommon, int numSingleCaches)
			: base(cacheCommon)
		{
			this._cacheIndexMask = numSingleCaches - 1;
			this._caches = new CacheSingle[numSingleCaches];
			for (int i = 0; i < numSingleCaches; i++)
			{
				this._caches[i] = new CacheSingle(cacheCommon, this, i);
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002FDD0 File Offset: 0x0002EDD0
		protected override void Dispose(bool disposing)
		{
			if (disposing && Interlocked.Exchange(ref this._disposed, 1) == 0)
			{
				foreach (CacheSingle cacheSingle in this._caches)
				{
					cacheSingle.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0002FE14 File Offset: 0x0002EE14
		internal override int PublicCount
		{
			get
			{
				int num = 0;
				foreach (CacheSingle cacheSingle in this._caches)
				{
					num += cacheSingle.PublicCount;
				}
				return num;
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002FE48 File Offset: 0x0002EE48
		internal override IDictionaryEnumerator CreateEnumerator()
		{
			IDictionaryEnumerator[] array = new IDictionaryEnumerator[this._caches.Length];
			int i = 0;
			int num = this._caches.Length;
			while (i < num)
			{
				array[i] = this._caches[i].CreateEnumerator();
				i++;
			}
			return new AggregateEnumerator(array);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0002FE90 File Offset: 0x0002EE90
		internal CacheSingle GetCacheSingle(int hashCode)
		{
			hashCode = Math.Abs(hashCode);
			int num = hashCode & this._cacheIndexMask;
			return this._caches[num];
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0002FEB8 File Offset: 0x0002EEB8
		internal override CacheEntry UpdateCache(CacheKey cacheKey, CacheEntry newEntry, bool replace, CacheItemRemovedReason removedReason, out object valueOld)
		{
			int hashCode = cacheKey.Key.GetHashCode();
			CacheSingle cacheSingle = this.GetCacheSingle(hashCode);
			return cacheSingle.UpdateCache(cacheKey, newEntry, replace, removedReason, out valueOld);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002FEE8 File Offset: 0x0002EEE8
		internal override bool ReviewMemoryStats()
		{
			bool flag = false;
			foreach (CacheSingle cacheSingle in this._caches)
			{
				flag |= cacheSingle.ReviewMemoryStats();
			}
			return flag;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0002FF1C File Offset: 0x0002EF1C
		internal override void EnableExpirationTimer(bool enable)
		{
			foreach (CacheSingle cacheSingle in this._caches)
			{
				cacheSingle.EnableExpirationTimer(enable);
			}
		}

		// Token: 0x040013F2 RID: 5106
		private int _disposed;

		// Token: 0x040013F3 RID: 5107
		private CacheSingle[] _caches;

		// Token: 0x040013F4 RID: 5108
		private int _cacheIndexMask;
	}
}

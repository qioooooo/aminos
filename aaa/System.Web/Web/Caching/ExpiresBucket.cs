using System;

namespace System.Web.Caching
{
	// Token: 0x02000120 RID: 288
	internal sealed class ExpiresBucket
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x000350C4 File Offset: 0x000340C4
		internal ExpiresBucket(CacheExpires cacheExpires, byte bucket, DateTime utcNow)
		{
			this._cacheExpires = cacheExpires;
			this._bucket = bucket;
			this._counts = new int[4];
			this.ResetCounts(utcNow);
			this.InitZeroPages();
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000350F3 File Offset: 0x000340F3
		private void InitZeroPages()
		{
			this._pages = null;
			this._minEntriesInUse = -1;
			this._freePageList._head = -1;
			this._freePageList._tail = -1;
			this._freeEntryList._head = -1;
			this._freeEntryList._tail = -1;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00035134 File Offset: 0x00034134
		private void ResetCounts(DateTime utcNow)
		{
			this._utcLastCountReset = utcNow;
			this._utcMinExpires = DateTime.MaxValue;
			for (int i = 0; i < this._counts.Length; i++)
			{
				this._counts[i] = 0;
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00035170 File Offset: 0x00034170
		private int GetCountIndex(DateTime utcExpires)
		{
			return Math.Max(0, (int)((utcExpires - this._utcLastCountReset).Ticks / ExpiresBucket.COUNT_INTERVAL.Ticks));
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x000351A8 File Offset: 0x000341A8
		private void AddCount(DateTime utcExpires)
		{
			int countIndex = this.GetCountIndex(utcExpires);
			for (int i = this._counts.Length - 1; i >= countIndex; i--)
			{
				this._counts[i]++;
			}
			if (utcExpires < this._utcMinExpires)
			{
				this._utcMinExpires = utcExpires;
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00035200 File Offset: 0x00034200
		private void RemoveCount(DateTime utcExpires)
		{
			int countIndex = this.GetCountIndex(utcExpires);
			for (int i = this._counts.Length - 1; i >= countIndex; i--)
			{
				this._counts[i]--;
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00035244 File Offset: 0x00034244
		private int GetExpiresCount(DateTime utcExpires)
		{
			if (utcExpires < this._utcMinExpires)
			{
				return 0;
			}
			int countIndex = this.GetCountIndex(utcExpires);
			if (countIndex >= this._counts.Length)
			{
				return this._cEntriesInUse;
			}
			return this._counts[countIndex];
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00035284 File Offset: 0x00034284
		private void AddToListHead(int pageIndex, ref ExpiresPageList list)
		{
			this._pages[pageIndex]._pagePrev = -1;
			this._pages[pageIndex]._pageNext = list._head;
			if (list._head != -1)
			{
				this._pages[list._head]._pagePrev = pageIndex;
			}
			else
			{
				list._tail = pageIndex;
			}
			list._head = pageIndex;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000352EC File Offset: 0x000342EC
		private void AddToListTail(int pageIndex, ref ExpiresPageList list)
		{
			this._pages[pageIndex]._pageNext = -1;
			this._pages[pageIndex]._pagePrev = list._tail;
			if (list._tail != -1)
			{
				this._pages[list._tail]._pageNext = pageIndex;
			}
			else
			{
				list._head = pageIndex;
			}
			list._tail = pageIndex;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00035354 File Offset: 0x00034354
		private int RemoveFromListHead(ref ExpiresPageList list)
		{
			int head = list._head;
			this.RemoveFromList(head, ref list);
			return head;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00035374 File Offset: 0x00034374
		private void RemoveFromList(int pageIndex, ref ExpiresPageList list)
		{
			if (this._pages[pageIndex]._pagePrev != -1)
			{
				this._pages[this._pages[pageIndex]._pagePrev]._pageNext = this._pages[pageIndex]._pageNext;
			}
			else
			{
				list._head = this._pages[pageIndex]._pageNext;
			}
			if (this._pages[pageIndex]._pageNext != -1)
			{
				this._pages[this._pages[pageIndex]._pageNext]._pagePrev = this._pages[pageIndex]._pagePrev;
			}
			else
			{
				list._tail = this._pages[pageIndex]._pagePrev;
			}
			this._pages[pageIndex]._pagePrev = -1;
			this._pages[pageIndex]._pageNext = -1;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00035463 File Offset: 0x00034463
		private void MoveToListHead(int pageIndex, ref ExpiresPageList list)
		{
			if (list._head == pageIndex)
			{
				return;
			}
			this.RemoveFromList(pageIndex, ref list);
			this.AddToListHead(pageIndex, ref list);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0003547F File Offset: 0x0003447F
		private void MoveToListTail(int pageIndex, ref ExpiresPageList list)
		{
			if (list._tail == pageIndex)
			{
				return;
			}
			this.RemoveFromList(pageIndex, ref list);
			this.AddToListTail(pageIndex, ref list);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0003549C File Offset: 0x0003449C
		private void UpdateMinEntries()
		{
			if (this._cPagesInUse <= 1)
			{
				this._minEntriesInUse = -1;
				return;
			}
			int num = this._cPagesInUse * 127;
			this._minEntriesInUse = (int)((double)num * 0.5);
			if (this._minEntriesInUse - 1 > (this._cPagesInUse - 1) * 127)
			{
				this._minEntriesInUse = -1;
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000354F4 File Offset: 0x000344F4
		private void RemovePage(int pageIndex)
		{
			this.RemoveFromList(pageIndex, ref this._freeEntryList);
			this.AddToListHead(pageIndex, ref this._freePageList);
			this._pages[pageIndex]._entries = null;
			this._cPagesInUse--;
			if (this._cPagesInUse == 0)
			{
				this.InitZeroPages();
				return;
			}
			this.UpdateMinEntries();
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00035550 File Offset: 0x00034550
		private ExpiresEntryRef GetFreeExpiresEntry()
		{
			int head = this._freeEntryList._head;
			ExpiresEntry[] entries = this._pages[head]._entries;
			int index = entries[0]._next.Index;
			entries[0]._next = entries[index]._next;
			ExpiresEntry[] array = entries;
			int num = 0;
			array[num]._cFree = array[num]._cFree - 1;
			if (entries[0]._cFree == 0)
			{
				this.RemoveFromList(head, ref this._freeEntryList);
			}
			return new ExpiresEntryRef(head, index);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x000355DC File Offset: 0x000345DC
		private void AddExpiresEntryToFreeList(ExpiresEntryRef entryRef)
		{
			ExpiresEntry[] entries = this._pages[entryRef.PageIndex]._entries;
			int index = entryRef.Index;
			entries[index]._cFree = 0;
			entries[index]._next = entries[0]._next;
			entries[0]._next = entryRef;
			this._cEntriesInUse--;
			int pageIndex = entryRef.PageIndex;
			ExpiresEntry[] array = entries;
			int num = 0;
			array[num]._cFree = array[num]._cFree + 1;
			if (entries[0]._cFree == 1)
			{
				this.AddToListHead(pageIndex, ref this._freeEntryList);
				return;
			}
			if (entries[0]._cFree == 127)
			{
				this.RemovePage(pageIndex);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0003569C File Offset: 0x0003469C
		private void Expand()
		{
			if (this._freePageList._head == -1)
			{
				int num;
				if (this._pages == null)
				{
					num = 0;
				}
				else
				{
					num = this._pages.Length;
				}
				int num2 = num * 2;
				num2 = Math.Max(num + 10, num2);
				num2 = Math.Min(num2, num + 340);
				ExpiresPage[] array = new ExpiresPage[num2];
				for (int i = 0; i < num; i++)
				{
					array[i] = this._pages[i];
				}
				for (int j = num; j < array.Length; j++)
				{
					array[j]._pagePrev = j - 1;
					array[j]._pageNext = j + 1;
				}
				array[num]._pagePrev = -1;
				array[array.Length - 1]._pageNext = -1;
				this._freePageList._head = num;
				this._freePageList._tail = array.Length - 1;
				this._pages = array;
			}
			int num3 = this.RemoveFromListHead(ref this._freePageList);
			this.AddToListHead(num3, ref this._freeEntryList);
			ExpiresEntry[] array2 = new ExpiresEntry[128];
			array2[0]._cFree = 127;
			for (int k = 0; k < array2.Length - 1; k++)
			{
				array2[k]._next = new ExpiresEntryRef(num3, k + 1);
			}
			array2[array2.Length - 1]._next = ExpiresEntryRef.INVALID;
			this._pages[num3]._entries = array2;
			this._cPagesInUse++;
			this.UpdateMinEntries();
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00035838 File Offset: 0x00034838
		private void Reduce()
		{
			if (this._cEntriesInUse >= this._minEntriesInUse || this._blockReduce)
			{
				return;
			}
			int num = 63;
			int tail = this._freeEntryList._tail;
			int num2 = this._freeEntryList._head;
			for (;;)
			{
				int pageNext = this._pages[num2]._pageNext;
				if (this._pages[num2]._entries[0]._cFree > num)
				{
					this.MoveToListTail(num2, ref this._freeEntryList);
				}
				else
				{
					this.MoveToListHead(num2, ref this._freeEntryList);
				}
				if (num2 == tail)
				{
					break;
				}
				num2 = pageNext;
			}
			while (this._freeEntryList._tail != -1)
			{
				ExpiresEntry[] entries = this._pages[this._freeEntryList._tail]._entries;
				int num3 = this._cPagesInUse * 127 - entries[0]._cFree - this._cEntriesInUse;
				if (num3 < 127 - entries[0]._cFree)
				{
					return;
				}
				for (int i = 1; i < entries.Length; i++)
				{
					if (entries[i]._cacheEntry != null)
					{
						ExpiresEntryRef freeExpiresEntry = this.GetFreeExpiresEntry();
						CacheEntry cacheEntry = entries[i]._cacheEntry;
						cacheEntry.ExpiresEntryRef = freeExpiresEntry;
						ExpiresEntry[] entries2 = this._pages[freeExpiresEntry.PageIndex]._entries;
						entries2[freeExpiresEntry.Index] = entries[i];
						ExpiresEntry[] array = entries;
						int num4 = 0;
						array[num4]._cFree = array[num4]._cFree + 1;
					}
				}
				this.RemovePage(this._freeEntryList._tail);
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x000359DC File Offset: 0x000349DC
		internal void AddCacheEntry(CacheEntry cacheEntry)
		{
			lock (this)
			{
				if ((byte)(cacheEntry.State & (CacheEntry.EntryState)3) != 0)
				{
					ExpiresEntryRef expiresEntryRef = cacheEntry.ExpiresEntryRef;
					if (cacheEntry.ExpiresBucket == 255 && expiresEntryRef.IsInvalid)
					{
						if (this._freeEntryList._head == -1)
						{
							this.Expand();
						}
						ExpiresEntryRef freeExpiresEntry = this.GetFreeExpiresEntry();
						cacheEntry.ExpiresBucket = this._bucket;
						cacheEntry.ExpiresEntryRef = freeExpiresEntry;
						ExpiresEntry[] entries = this._pages[freeExpiresEntry.PageIndex]._entries;
						int index = freeExpiresEntry.Index;
						entries[index]._cacheEntry = cacheEntry;
						entries[index]._utcExpires = cacheEntry.UtcExpires;
						this.AddCount(cacheEntry.UtcExpires);
						this._cEntriesInUse++;
						if ((byte)(cacheEntry.State & (CacheEntry.EntryState)3) == 0)
						{
							this.RemoveCacheEntryNoLock(cacheEntry);
						}
					}
				}
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00035AD8 File Offset: 0x00034AD8
		private void RemoveCacheEntryNoLock(CacheEntry cacheEntry)
		{
			ExpiresEntryRef expiresEntryRef = cacheEntry.ExpiresEntryRef;
			if (cacheEntry.ExpiresBucket != this._bucket || expiresEntryRef.IsInvalid)
			{
				return;
			}
			ExpiresEntry[] entries = this._pages[expiresEntryRef.PageIndex]._entries;
			int index = expiresEntryRef.Index;
			this.RemoveCount(entries[index]._utcExpires);
			cacheEntry.ExpiresBucket = byte.MaxValue;
			cacheEntry.ExpiresEntryRef = ExpiresEntryRef.INVALID;
			entries[index]._cacheEntry = null;
			this.AddExpiresEntryToFreeList(expiresEntryRef);
			if (this._cEntriesInUse == 0)
			{
				this.ResetCounts(DateTime.UtcNow);
			}
			this.Reduce();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00035B7C File Offset: 0x00034B7C
		internal void RemoveCacheEntry(CacheEntry cacheEntry)
		{
			lock (this)
			{
				this.RemoveCacheEntryNoLock(cacheEntry);
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x00035BB4 File Offset: 0x00034BB4
		internal void UtcUpdateCacheEntry(CacheEntry cacheEntry, DateTime utcExpires)
		{
			lock (this)
			{
				ExpiresEntryRef expiresEntryRef = cacheEntry.ExpiresEntryRef;
				if (cacheEntry.ExpiresBucket == this._bucket && !expiresEntryRef.IsInvalid)
				{
					ExpiresEntry[] entries = this._pages[expiresEntryRef.PageIndex]._entries;
					int index = expiresEntryRef.Index;
					this.RemoveCount(entries[index]._utcExpires);
					this.AddCount(utcExpires);
					entries[index]._utcExpires = utcExpires;
					cacheEntry.UtcExpires = utcExpires;
				}
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00035C50 File Offset: 0x00034C50
		internal int FlushExpiredItems(DateTime utcNow, bool useInsertBlock)
		{
			if (this._cEntriesInUse == 0 || this.GetExpiresCount(utcNow) == 0)
			{
				return 0;
			}
			ExpiresEntryRef expiresEntryRef = ExpiresEntryRef.INVALID;
			int num = 0;
			try
			{
				if (useInsertBlock)
				{
					this._cacheExpires.CacheSingle.BlockInsertIfNeeded();
				}
				lock (this)
				{
					if (this._cEntriesInUse == 0 || this.GetExpiresCount(utcNow) == 0)
					{
						return 0;
					}
					this.ResetCounts(utcNow);
					int num2 = this._cPagesInUse;
					for (int i = 0; i < this._pages.Length; i++)
					{
						ExpiresEntry[] array = this._pages[i]._entries;
						if (array != null)
						{
							int num3 = 127 - array[0]._cFree;
							for (int j = 1; j < array.Length; j++)
							{
								CacheEntry cacheEntry = array[j]._cacheEntry;
								if (cacheEntry != null)
								{
									if (array[j]._utcExpires > utcNow)
									{
										this.AddCount(array[j]._utcExpires);
									}
									else
									{
										cacheEntry.ExpiresBucket = byte.MaxValue;
										cacheEntry.ExpiresEntryRef = ExpiresEntryRef.INVALID;
										array[j]._cFree = 1;
										array[j]._next = expiresEntryRef;
										expiresEntryRef = new ExpiresEntryRef(i, j);
										num++;
										this._cEntriesInFlush++;
									}
									num3--;
									if (num3 == 0)
									{
										break;
									}
								}
							}
							num2--;
							if (num2 == 0)
							{
								break;
							}
						}
					}
					if (num == 0)
					{
						return 0;
					}
					this._blockReduce = true;
				}
			}
			finally
			{
				if (useInsertBlock)
				{
					this._cacheExpires.CacheSingle.UnblockInsert();
				}
			}
			CacheSingle cacheSingle = this._cacheExpires.CacheSingle;
			ExpiresEntryRef expiresEntryRef2 = expiresEntryRef;
			while (!expiresEntryRef2.IsInvalid)
			{
				ExpiresEntry[] array = this._pages[expiresEntryRef2.PageIndex]._entries;
				int num4 = expiresEntryRef2.Index;
				ExpiresEntryRef expiresEntryRef3 = array[num4]._next;
				CacheEntry cacheEntry = array[num4]._cacheEntry;
				array[num4]._cacheEntry = null;
				cacheSingle.Remove(cacheEntry, CacheItemRemovedReason.Expired);
				expiresEntryRef2 = expiresEntryRef3;
			}
			try
			{
				if (useInsertBlock)
				{
					this._cacheExpires.CacheSingle.BlockInsertIfNeeded();
				}
				lock (this)
				{
					expiresEntryRef2 = expiresEntryRef;
					while (!expiresEntryRef2.IsInvalid)
					{
						ExpiresEntry[] array = this._pages[expiresEntryRef2.PageIndex]._entries;
						int num4 = expiresEntryRef2.Index;
						ExpiresEntryRef expiresEntryRef3 = array[num4]._next;
						this._cEntriesInFlush--;
						this.AddExpiresEntryToFreeList(expiresEntryRef2);
						expiresEntryRef2 = expiresEntryRef3;
					}
					this._blockReduce = false;
					this.Reduce();
				}
			}
			finally
			{
				if (useInsertBlock)
				{
					this._cacheExpires.CacheSingle.UnblockInsert();
				}
			}
			return num;
		}

		// Token: 0x040014C1 RID: 5313
		internal const int NUM_ENTRIES = 127;

		// Token: 0x040014C2 RID: 5314
		private const int LENGTH_ENTRIES = 128;

		// Token: 0x040014C3 RID: 5315
		private const int MIN_PAGES_INCREMENT = 10;

		// Token: 0x040014C4 RID: 5316
		private const int MAX_PAGES_INCREMENT = 340;

		// Token: 0x040014C5 RID: 5317
		private const double MIN_LOAD_FACTOR = 0.5;

		// Token: 0x040014C6 RID: 5318
		private const int COUNTS_LENGTH = 4;

		// Token: 0x040014C7 RID: 5319
		private static readonly TimeSpan COUNT_INTERVAL = new TimeSpan(CacheExpires._tsPerBucket.Ticks / 4L);

		// Token: 0x040014C8 RID: 5320
		private readonly CacheExpires _cacheExpires;

		// Token: 0x040014C9 RID: 5321
		private readonly byte _bucket;

		// Token: 0x040014CA RID: 5322
		private ExpiresPage[] _pages;

		// Token: 0x040014CB RID: 5323
		private int _cEntriesInUse;

		// Token: 0x040014CC RID: 5324
		private int _cPagesInUse;

		// Token: 0x040014CD RID: 5325
		private int _cEntriesInFlush;

		// Token: 0x040014CE RID: 5326
		private int _minEntriesInUse;

		// Token: 0x040014CF RID: 5327
		private ExpiresPageList _freePageList;

		// Token: 0x040014D0 RID: 5328
		private ExpiresPageList _freeEntryList;

		// Token: 0x040014D1 RID: 5329
		private bool _blockReduce;

		// Token: 0x040014D2 RID: 5330
		private DateTime _utcMinExpires;

		// Token: 0x040014D3 RID: 5331
		private int[] _counts;

		// Token: 0x040014D4 RID: 5332
		private DateTime _utcLastCountReset;
	}
}

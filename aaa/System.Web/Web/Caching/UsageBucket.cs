using System;

namespace System.Web.Caching
{
	// Token: 0x0200011A RID: 282
	internal sealed class UsageBucket
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x00033616 File Offset: 0x00032616
		internal UsageBucket(CacheUsage cacheUsage, byte bucket)
		{
			this._cacheUsage = cacheUsage;
			this._bucket = bucket;
			this.InitZeroPages();
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00033632 File Offset: 0x00032632
		private void InitZeroPages()
		{
			this._pages = null;
			this._minEntriesInUse = -1;
			this._freePageList._head = -1;
			this._freePageList._tail = -1;
			this._freeEntryList._head = -1;
			this._freeEntryList._tail = -1;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00033674 File Offset: 0x00032674
		private void AddToListHead(int pageIndex, ref UsagePageList list)
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

		// Token: 0x06000CCB RID: 3275 RVA: 0x000336DC File Offset: 0x000326DC
		private void AddToListTail(int pageIndex, ref UsagePageList list)
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

		// Token: 0x06000CCC RID: 3276 RVA: 0x00033744 File Offset: 0x00032744
		private int RemoveFromListHead(ref UsagePageList list)
		{
			int head = list._head;
			this.RemoveFromList(head, ref list);
			return head;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00033764 File Offset: 0x00032764
		private void RemoveFromList(int pageIndex, ref UsagePageList list)
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

		// Token: 0x06000CCE RID: 3278 RVA: 0x00033853 File Offset: 0x00032853
		private void MoveToListHead(int pageIndex, ref UsagePageList list)
		{
			if (list._head == pageIndex)
			{
				return;
			}
			this.RemoveFromList(pageIndex, ref list);
			this.AddToListHead(pageIndex, ref list);
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0003386F File Offset: 0x0003286F
		private void MoveToListTail(int pageIndex, ref UsagePageList list)
		{
			if (list._tail == pageIndex)
			{
				return;
			}
			this.RemoveFromList(pageIndex, ref list);
			this.AddToListTail(pageIndex, ref list);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0003388C File Offset: 0x0003288C
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

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000338E4 File Offset: 0x000328E4
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

		// Token: 0x06000CD2 RID: 3282 RVA: 0x00033940 File Offset: 0x00032940
		private UsageEntryRef GetFreeUsageEntry()
		{
			int head = this._freeEntryList._head;
			UsageEntry[] entries = this._pages[head]._entries;
			int ref1Index = entries[0]._ref1._next.Ref1Index;
			entries[0]._ref1._next = entries[ref1Index]._ref1._next;
			UsageEntry[] array = entries;
			int num = 0;
			array[num]._cFree = array[num]._cFree - 1;
			if (entries[0]._cFree == 0)
			{
				this.RemoveFromList(head, ref this._freeEntryList);
			}
			return new UsageEntryRef(head, ref1Index);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000339DC File Offset: 0x000329DC
		private void AddUsageEntryToFreeList(UsageEntryRef entryRef)
		{
			UsageEntry[] entries = this._pages[entryRef.PageIndex]._entries;
			int ref1Index = entryRef.Ref1Index;
			entries[ref1Index]._utcDate = DateTime.MinValue;
			entries[ref1Index]._ref1._prev = UsageEntryRef.INVALID;
			entries[ref1Index]._ref2._next = UsageEntryRef.INVALID;
			entries[ref1Index]._ref2._prev = UsageEntryRef.INVALID;
			entries[ref1Index]._ref1._next = entries[0]._ref1._next;
			entries[0]._ref1._next = entryRef;
			this._cEntriesInUse--;
			int pageIndex = entryRef.PageIndex;
			UsageEntry[] array = entries;
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

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00033AF0 File Offset: 0x00032AF0
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
				UsagePage[] array = new UsagePage[num2];
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
			UsageEntry[] array2 = new UsageEntry[128];
			array2[0]._cFree = 127;
			for (int k = 0; k < array2.Length - 1; k++)
			{
				array2[k]._ref1._next = new UsageEntryRef(num3, k + 1);
			}
			array2[array2.Length - 1]._ref1._next = UsageEntryRef.INVALID;
			this._pages[num3]._entries = array2;
			this._cPagesInUse++;
			this.UpdateMinEntries();
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00033C94 File Offset: 0x00032C94
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
				UsageEntry[] entries = this._pages[this._freeEntryList._tail]._entries;
				int num3 = this._cPagesInUse * 127 - entries[0]._cFree - this._cEntriesInUse;
				if (num3 < 127 - entries[0]._cFree)
				{
					return;
				}
				for (int i = 1; i < entries.Length; i++)
				{
					if (entries[i]._cacheEntry != null)
					{
						UsageEntryRef freeUsageEntry = this.GetFreeUsageEntry();
						UsageEntryRef usageEntryRef = new UsageEntryRef(freeUsageEntry.PageIndex, -freeUsageEntry.Ref1Index);
						UsageEntryRef usageEntryRef2 = new UsageEntryRef(this._freeEntryList._tail, i);
						UsageEntryRef usageEntryRef3 = new UsageEntryRef(usageEntryRef2.PageIndex, -usageEntryRef2.Ref1Index);
						CacheEntry cacheEntry = entries[i]._cacheEntry;
						cacheEntry.UsageEntryRef = freeUsageEntry;
						UsageEntry[] entries2 = this._pages[freeUsageEntry.PageIndex]._entries;
						entries2[freeUsageEntry.Ref1Index] = entries[i];
						UsageEntry[] array = entries;
						int num4 = 0;
						array[num4]._cFree = array[num4]._cFree + 1;
						UsageEntryRef usageEntryRef4 = entries2[freeUsageEntry.Ref1Index]._ref1._prev;
						UsageEntryRef usageEntryRef5 = entries2[freeUsageEntry.Ref1Index]._ref1._next;
						if (usageEntryRef5 == usageEntryRef3)
						{
							usageEntryRef5 = usageEntryRef;
						}
						if (usageEntryRef4.IsRef1)
						{
							this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref1Index]._ref1._next = freeUsageEntry;
						}
						else if (usageEntryRef4.IsRef2)
						{
							this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref2Index]._ref2._next = freeUsageEntry;
						}
						else
						{
							this._lastRefHead = freeUsageEntry;
						}
						if (usageEntryRef5.IsRef1)
						{
							this._pages[usageEntryRef5.PageIndex]._entries[usageEntryRef5.Ref1Index]._ref1._prev = freeUsageEntry;
						}
						else if (usageEntryRef5.IsRef2)
						{
							this._pages[usageEntryRef5.PageIndex]._entries[usageEntryRef5.Ref2Index]._ref2._prev = freeUsageEntry;
						}
						else
						{
							this._lastRefTail = freeUsageEntry;
						}
						usageEntryRef4 = entries2[freeUsageEntry.Ref1Index]._ref2._prev;
						if (usageEntryRef4 == usageEntryRef2)
						{
							usageEntryRef4 = freeUsageEntry;
						}
						usageEntryRef5 = entries2[freeUsageEntry.Ref1Index]._ref2._next;
						if (usageEntryRef4.IsRef1)
						{
							this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref1Index]._ref1._next = usageEntryRef;
						}
						else if (usageEntryRef4.IsRef2)
						{
							this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref2Index]._ref2._next = usageEntryRef;
						}
						else
						{
							this._lastRefHead = usageEntryRef;
						}
						if (usageEntryRef5.IsRef1)
						{
							this._pages[usageEntryRef5.PageIndex]._entries[usageEntryRef5.Ref1Index]._ref1._prev = usageEntryRef;
						}
						else if (usageEntryRef5.IsRef2)
						{
							this._pages[usageEntryRef5.PageIndex]._entries[usageEntryRef5.Ref2Index]._ref2._prev = usageEntryRef;
						}
						else
						{
							this._lastRefTail = usageEntryRef;
						}
						if (this._addRef2Head == usageEntryRef3)
						{
							this._addRef2Head = usageEntryRef;
						}
					}
				}
				this.RemovePage(this._freeEntryList._tail);
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00034108 File Offset: 0x00033108
		internal void AddCacheEntry(CacheEntry cacheEntry)
		{
			lock (this)
			{
				if (this._freeEntryList._head == -1)
				{
					this.Expand();
				}
				UsageEntryRef freeUsageEntry = this.GetFreeUsageEntry();
				UsageEntryRef usageEntryRef = new UsageEntryRef(freeUsageEntry.PageIndex, -freeUsageEntry.Ref1Index);
				cacheEntry.UsageEntryRef = freeUsageEntry;
				UsageEntry[] entries = this._pages[freeUsageEntry.PageIndex]._entries;
				int ref1Index = freeUsageEntry.Ref1Index;
				entries[ref1Index]._cacheEntry = cacheEntry;
				entries[ref1Index]._utcDate = DateTime.UtcNow;
				entries[ref1Index]._ref1._prev = UsageEntryRef.INVALID;
				entries[ref1Index]._ref2._next = this._addRef2Head;
				if (this._lastRefHead.IsInvalid)
				{
					entries[ref1Index]._ref1._next = usageEntryRef;
					entries[ref1Index]._ref2._prev = freeUsageEntry;
					this._lastRefTail = usageEntryRef;
				}
				else
				{
					entries[ref1Index]._ref1._next = this._lastRefHead;
					if (this._lastRefHead.IsRef1)
					{
						this._pages[this._lastRefHead.PageIndex]._entries[this._lastRefHead.Ref1Index]._ref1._prev = freeUsageEntry;
					}
					else if (this._lastRefHead.IsRef2)
					{
						this._pages[this._lastRefHead.PageIndex]._entries[this._lastRefHead.Ref2Index]._ref2._prev = freeUsageEntry;
					}
					else
					{
						this._lastRefTail = freeUsageEntry;
					}
					UsageEntryRef usageEntryRef2;
					UsageEntryRef usageEntryRef3;
					if (this._addRef2Head.IsInvalid)
					{
						usageEntryRef2 = this._lastRefTail;
						usageEntryRef3 = UsageEntryRef.INVALID;
					}
					else
					{
						usageEntryRef2 = this._pages[this._addRef2Head.PageIndex]._entries[this._addRef2Head.Ref2Index]._ref2._prev;
						usageEntryRef3 = this._addRef2Head;
					}
					entries[ref1Index]._ref2._prev = usageEntryRef2;
					if (usageEntryRef2.IsRef1)
					{
						this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref1Index]._ref1._next = usageEntryRef;
					}
					else if (usageEntryRef2.IsRef2)
					{
						this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref2Index]._ref2._next = usageEntryRef;
					}
					else
					{
						this._lastRefHead = usageEntryRef;
					}
					if (usageEntryRef3.IsRef1)
					{
						this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref1Index]._ref1._prev = usageEntryRef;
					}
					else if (usageEntryRef3.IsRef2)
					{
						this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref2Index]._ref2._prev = usageEntryRef;
					}
					else
					{
						this._lastRefTail = usageEntryRef;
					}
				}
				this._lastRefHead = freeUsageEntry;
				this._addRef2Head = usageEntryRef;
				this._cEntriesInUse++;
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003444C File Offset: 0x0003344C
		private void RemoveEntryFromLastRefList(UsageEntryRef entryRef)
		{
			UsageEntry[] entries = this._pages[entryRef.PageIndex]._entries;
			int ref1Index = entryRef.Ref1Index;
			UsageEntryRef usageEntryRef = entries[ref1Index]._ref1._prev;
			UsageEntryRef usageEntryRef2 = entries[ref1Index]._ref1._next;
			if (usageEntryRef.IsRef1)
			{
				this._pages[usageEntryRef.PageIndex]._entries[usageEntryRef.Ref1Index]._ref1._next = usageEntryRef2;
			}
			else if (usageEntryRef.IsRef2)
			{
				this._pages[usageEntryRef.PageIndex]._entries[usageEntryRef.Ref2Index]._ref2._next = usageEntryRef2;
			}
			else
			{
				this._lastRefHead = usageEntryRef2;
			}
			if (usageEntryRef2.IsRef1)
			{
				this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref1Index]._ref1._prev = usageEntryRef;
			}
			else if (usageEntryRef2.IsRef2)
			{
				this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref2Index]._ref2._prev = usageEntryRef;
			}
			else
			{
				this._lastRefTail = usageEntryRef;
			}
			usageEntryRef = entries[ref1Index]._ref2._prev;
			usageEntryRef2 = entries[ref1Index]._ref2._next;
			UsageEntryRef usageEntryRef3 = new UsageEntryRef(entryRef.PageIndex, -entryRef.Ref1Index);
			if (usageEntryRef.IsRef1)
			{
				this._pages[usageEntryRef.PageIndex]._entries[usageEntryRef.Ref1Index]._ref1._next = usageEntryRef2;
			}
			else if (usageEntryRef.IsRef2)
			{
				this._pages[usageEntryRef.PageIndex]._entries[usageEntryRef.Ref2Index]._ref2._next = usageEntryRef2;
			}
			else
			{
				this._lastRefHead = usageEntryRef2;
			}
			if (usageEntryRef2.IsRef1)
			{
				this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref1Index]._ref1._prev = usageEntryRef;
			}
			else if (usageEntryRef2.IsRef2)
			{
				this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref2Index]._ref2._prev = usageEntryRef;
			}
			else
			{
				this._lastRefTail = usageEntryRef;
			}
			if (this._addRef2Head == usageEntryRef3)
			{
				this._addRef2Head = usageEntryRef2;
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x000346D4 File Offset: 0x000336D4
		internal void RemoveCacheEntry(CacheEntry cacheEntry)
		{
			lock (this)
			{
				UsageEntryRef usageEntryRef = cacheEntry.UsageEntryRef;
				if (!usageEntryRef.IsInvalid)
				{
					UsageEntry[] entries = this._pages[usageEntryRef.PageIndex]._entries;
					int ref1Index = usageEntryRef.Ref1Index;
					cacheEntry.UsageEntryRef = UsageEntryRef.INVALID;
					entries[ref1Index]._cacheEntry = null;
					this.RemoveEntryFromLastRefList(usageEntryRef);
					this.AddUsageEntryToFreeList(usageEntryRef);
					this.Reduce();
				}
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00034760 File Offset: 0x00033760
		internal void UpdateCacheEntry(CacheEntry cacheEntry)
		{
			lock (this)
			{
				UsageEntryRef usageEntryRef = cacheEntry.UsageEntryRef;
				if (!usageEntryRef.IsInvalid)
				{
					UsageEntry[] entries = this._pages[usageEntryRef.PageIndex]._entries;
					int ref1Index = usageEntryRef.Ref1Index;
					UsageEntryRef usageEntryRef2 = new UsageEntryRef(usageEntryRef.PageIndex, -usageEntryRef.Ref1Index);
					UsageEntryRef usageEntryRef3 = entries[ref1Index]._ref2._prev;
					UsageEntryRef usageEntryRef4 = entries[ref1Index]._ref2._next;
					if (usageEntryRef3.IsRef1)
					{
						this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref1Index]._ref1._next = usageEntryRef4;
					}
					else if (usageEntryRef3.IsRef2)
					{
						this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref2Index]._ref2._next = usageEntryRef4;
					}
					else
					{
						this._lastRefHead = usageEntryRef4;
					}
					if (usageEntryRef4.IsRef1)
					{
						this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref1Index]._ref1._prev = usageEntryRef3;
					}
					else if (usageEntryRef4.IsRef2)
					{
						this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref2Index]._ref2._prev = usageEntryRef3;
					}
					else
					{
						this._lastRefTail = usageEntryRef3;
					}
					if (this._addRef2Head == usageEntryRef2)
					{
						this._addRef2Head = usageEntryRef4;
					}
					entries[ref1Index]._ref2 = entries[ref1Index]._ref1;
					usageEntryRef3 = entries[ref1Index]._ref2._prev;
					usageEntryRef4 = entries[ref1Index]._ref2._next;
					if (usageEntryRef3.IsRef1)
					{
						this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref1Index]._ref1._next = usageEntryRef2;
					}
					else if (usageEntryRef3.IsRef2)
					{
						this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref2Index]._ref2._next = usageEntryRef2;
					}
					else
					{
						this._lastRefHead = usageEntryRef2;
					}
					if (usageEntryRef4.IsRef1)
					{
						this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref1Index]._ref1._prev = usageEntryRef2;
					}
					else if (usageEntryRef4.IsRef2)
					{
						this._pages[usageEntryRef4.PageIndex]._entries[usageEntryRef4.Ref2Index]._ref2._prev = usageEntryRef2;
					}
					else
					{
						this._lastRefTail = usageEntryRef2;
					}
					entries[ref1Index]._ref1._prev = UsageEntryRef.INVALID;
					entries[ref1Index]._ref1._next = this._lastRefHead;
					if (this._lastRefHead.IsRef1)
					{
						this._pages[this._lastRefHead.PageIndex]._entries[this._lastRefHead.Ref1Index]._ref1._prev = usageEntryRef;
					}
					else if (this._lastRefHead.IsRef2)
					{
						this._pages[this._lastRefHead.PageIndex]._entries[this._lastRefHead.Ref2Index]._ref2._prev = usageEntryRef;
					}
					else
					{
						this._lastRefTail = usageEntryRef;
					}
					this._lastRefHead = usageEntryRef;
				}
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00034B10 File Offset: 0x00033B10
		internal int FlushUnderUsedItems(int maxFlush, bool force, ref int publicEntriesFlushed)
		{
			if (this._cEntriesInUse == 0)
			{
				return 0;
			}
			UsageEntryRef usageEntryRef = UsageEntryRef.INVALID;
			int num = 0;
			try
			{
				this._cacheUsage.CacheSingle.BlockInsertIfNeeded();
				lock (this)
				{
					if (this._cEntriesInUse == 0)
					{
						return 0;
					}
					DateTime utcNow = DateTime.UtcNow;
					UsageEntryRef usageEntryRef2 = this._lastRefTail;
					while (this._cEntriesInFlush < maxFlush && !usageEntryRef2.IsInvalid)
					{
						UsageEntryRef usageEntryRef3 = this._pages[usageEntryRef2.PageIndex]._entries[usageEntryRef2.Ref2Index]._ref2._prev;
						while (usageEntryRef3.IsRef1)
						{
							usageEntryRef3 = this._pages[usageEntryRef3.PageIndex]._entries[usageEntryRef3.Ref1Index]._ref1._prev;
						}
						UsageEntry[] array = this._pages[usageEntryRef2.PageIndex]._entries;
						int num2 = usageEntryRef2.Ref2Index;
						if (force)
						{
							goto IL_0108;
						}
						DateTime utcDate = array[num2]._utcDate;
						if (!(utcNow - utcDate <= CacheUsage.NEWADD_INTERVAL) || !(utcNow >= utcDate))
						{
							goto IL_0108;
						}
						IL_017B:
						usageEntryRef2 = usageEntryRef3;
						continue;
						IL_0108:
						UsageEntryRef usageEntryRef4 = new UsageEntryRef(usageEntryRef2.PageIndex, usageEntryRef2.Ref2Index);
						CacheEntry cacheEntry = array[num2]._cacheEntry;
						cacheEntry.UsageEntryRef = UsageEntryRef.INVALID;
						if (cacheEntry.IsPublic)
						{
							publicEntriesFlushed++;
						}
						this.RemoveEntryFromLastRefList(usageEntryRef4);
						array[num2]._ref1._next = usageEntryRef;
						usageEntryRef = usageEntryRef4;
						num++;
						this._cEntriesInFlush++;
						goto IL_017B;
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
				this._cacheUsage.CacheSingle.UnblockInsert();
			}
			CacheSingle cacheSingle = this._cacheUsage.CacheSingle;
			UsageEntryRef usageEntryRef5 = usageEntryRef;
			while (!usageEntryRef5.IsInvalid)
			{
				UsageEntry[] array = this._pages[usageEntryRef5.PageIndex]._entries;
				int num2 = usageEntryRef5.Ref1Index;
				UsageEntryRef usageEntryRef6 = array[num2]._ref1._next;
				CacheEntry cacheEntry = array[num2]._cacheEntry;
				array[num2]._cacheEntry = null;
				cacheSingle.Remove(cacheEntry, CacheItemRemovedReason.Underused);
				usageEntryRef5 = usageEntryRef6;
			}
			try
			{
				this._cacheUsage.CacheSingle.BlockInsertIfNeeded();
				lock (this)
				{
					usageEntryRef5 = usageEntryRef;
					while (!usageEntryRef5.IsInvalid)
					{
						UsageEntry[] array = this._pages[usageEntryRef5.PageIndex]._entries;
						int num2 = usageEntryRef5.Ref1Index;
						UsageEntryRef usageEntryRef6 = array[num2]._ref1._next;
						this._cEntriesInFlush--;
						this.AddUsageEntryToFreeList(usageEntryRef5);
						usageEntryRef5 = usageEntryRef6;
					}
					this._blockReduce = false;
					this.Reduce();
				}
			}
			finally
			{
				this._cacheUsage.CacheSingle.UnblockInsert();
			}
			return num;
		}

		// Token: 0x04001499 RID: 5273
		internal const int NUM_ENTRIES = 127;

		// Token: 0x0400149A RID: 5274
		private const int LENGTH_ENTRIES = 128;

		// Token: 0x0400149B RID: 5275
		private const int MIN_PAGES_INCREMENT = 10;

		// Token: 0x0400149C RID: 5276
		private const int MAX_PAGES_INCREMENT = 340;

		// Token: 0x0400149D RID: 5277
		private const double MIN_LOAD_FACTOR = 0.5;

		// Token: 0x0400149E RID: 5278
		private CacheUsage _cacheUsage;

		// Token: 0x0400149F RID: 5279
		private byte _bucket;

		// Token: 0x040014A0 RID: 5280
		private UsagePage[] _pages;

		// Token: 0x040014A1 RID: 5281
		private int _cEntriesInUse;

		// Token: 0x040014A2 RID: 5282
		private int _cPagesInUse;

		// Token: 0x040014A3 RID: 5283
		private int _cEntriesInFlush;

		// Token: 0x040014A4 RID: 5284
		private int _minEntriesInUse;

		// Token: 0x040014A5 RID: 5285
		private UsagePageList _freePageList;

		// Token: 0x040014A6 RID: 5286
		private UsagePageList _freeEntryList;

		// Token: 0x040014A7 RID: 5287
		private UsageEntryRef _lastRefHead;

		// Token: 0x040014A8 RID: 5288
		private UsageEntryRef _lastRefTail;

		// Token: 0x040014A9 RID: 5289
		private UsageEntryRef _addRef2Head;

		// Token: 0x040014AA RID: 5290
		private bool _blockReduce;
	}
}

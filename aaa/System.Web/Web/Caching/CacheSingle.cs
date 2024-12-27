using System;
using System.Collections;
using System.Threading;

namespace System.Web.Caching
{
	// Token: 0x02000100 RID: 256
	internal sealed class CacheSingle : CacheInternal
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x0002F1C4 File Offset: 0x0002E1C4
		internal CacheSingle(CacheCommon cacheCommon, CacheMultiple cacheMultiple, int iSubCache)
			: base(cacheCommon)
		{
			this._cacheMultiple = cacheMultiple;
			this._iSubCache = iSubCache;
			this._entries = new Hashtable(CacheKeyComparer.GetInstance());
			this._expires = new CacheExpires(this);
			this._usage = new CacheUsage(this);
			this._lock = new object();
			this._maxCount = 1073741823;
			this._maxCountOverload = 1073741873;
			this._insertBlock = new ManualResetEvent(true);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002F23C File Offset: 0x0002E23C
		protected override void Dispose(bool disposing)
		{
			if (disposing && Interlocked.Exchange(ref this._disposed, 1) == 0)
			{
				if (this._expires != null)
				{
					this._expires.EnableExpirationTimer(false);
				}
				CacheEntry[] array = null;
				lock (this._lock)
				{
					array = new CacheEntry[this._entries.Count];
					int num = 0;
					foreach (object obj in this._entries)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						array[num++] = (CacheEntry)dictionaryEntry.Value;
					}
				}
				foreach (CacheEntry cacheEntry in array)
				{
					base.Remove(cacheEntry, CacheItemRemovedReason.Removed);
				}
				this._insertBlock.Set();
				this.ReleaseInsertBlock();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002F34C File Offset: 0x0002E34C
		private ManualResetEvent UseInsertBlock()
		{
			while (this._disposed != 1)
			{
				int insertBlockCalls = this._insertBlockCalls;
				if (insertBlockCalls < 0)
				{
					return null;
				}
				if (Interlocked.CompareExchange(ref this._insertBlockCalls, insertBlockCalls + 1, insertBlockCalls) == insertBlockCalls)
				{
					return this._insertBlock;
				}
			}
			return null;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002F38C File Offset: 0x0002E38C
		private void ReleaseInsertBlock()
		{
			if (Interlocked.Decrement(ref this._insertBlockCalls) < 0)
			{
				ManualResetEvent insertBlock = this._insertBlock;
				this._insertBlock = null;
				insertBlock.Close();
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002F3BC File Offset: 0x0002E3BC
		private void SetInsertBlock()
		{
			ManualResetEvent manualResetEvent = null;
			try
			{
				manualResetEvent = this.UseInsertBlock();
				if (manualResetEvent != null)
				{
					manualResetEvent.Set();
				}
			}
			finally
			{
				if (manualResetEvent != null)
				{
					this.ReleaseInsertBlock();
				}
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0002F3F8 File Offset: 0x0002E3F8
		private void ResetInsertBlock()
		{
			ManualResetEvent manualResetEvent = null;
			try
			{
				manualResetEvent = this.UseInsertBlock();
				if (manualResetEvent != null)
				{
					manualResetEvent.Reset();
				}
			}
			finally
			{
				if (manualResetEvent != null)
				{
					this.ReleaseInsertBlock();
				}
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002F434 File Offset: 0x0002E434
		private bool WaitInsertBlock()
		{
			bool flag = false;
			ManualResetEvent manualResetEvent = null;
			try
			{
				manualResetEvent = this.UseInsertBlock();
				if (manualResetEvent != null)
				{
					flag = manualResetEvent.WaitOne(CacheSingle.INSERT_BLOCK_WAIT, false);
				}
			}
			finally
			{
				if (manualResetEvent != null)
				{
					this.ReleaseInsertBlock();
				}
			}
			return flag;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002F47C File Offset: 0x0002E47C
		internal void BlockInsertIfNeeded()
		{
			if (this._cacheCommon._cacheMemoryStats.IsAboveHighPressure())
			{
				this._useInsertBlock = true;
				this.ResetInsertBlock();
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002F49D File Offset: 0x0002E49D
		internal void UnblockInsert()
		{
			if (this._useInsertBlock)
			{
				this._useInsertBlock = false;
				this.SetInsertBlock();
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x0002F4B4 File Offset: 0x0002E4B4
		internal override int PublicCount
		{
			get
			{
				return this._publicCount;
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002F4BC File Offset: 0x0002E4BC
		internal override IDictionaryEnumerator CreateEnumerator()
		{
			Hashtable hashtable = new Hashtable(this._publicCount);
			DateTime utcNow = DateTime.UtcNow;
			lock (this._lock)
			{
				foreach (object obj in this._entries)
				{
					CacheEntry cacheEntry = (CacheEntry)((DictionaryEntry)obj).Value;
					if (cacheEntry.IsPublic && cacheEntry.State == CacheEntry.EntryState.AddedToCache && (!this._cacheCommon._enableExpiration || utcNow <= cacheEntry.UtcExpires))
					{
						hashtable[cacheEntry.Key] = cacheEntry.Value;
					}
				}
			}
			return hashtable.GetEnumerator();
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0002F59C File Offset: 0x0002E59C
		internal override CacheEntry UpdateCache(CacheKey cacheKey, CacheEntry newEntry, bool replace, CacheItemRemovedReason removedReason, out object valueOld)
		{
			CacheEntry cacheEntry = null;
			CacheEntry cacheEntry2 = null;
			CacheDependency cacheDependency = null;
			bool flag = false;
			bool flag2 = false;
			DateTime dateTime = DateTime.MinValue;
			CacheEntry.EntryState entryState = CacheEntry.EntryState.NotInCache;
			bool flag3 = false;
			CacheItemRemovedReason cacheItemRemovedReason = CacheItemRemovedReason.Removed;
			valueOld = null;
			bool flag4 = !replace && newEntry == null;
			bool flag5 = !replace && newEntry != null;
			DateTime utcNow;
			for (;;)
			{
				if (flag)
				{
					this.UpdateCache(cacheKey, null, true, CacheItemRemovedReason.Expired, out valueOld);
					flag = false;
				}
				cacheEntry = null;
				utcNow = DateTime.UtcNow;
				if (this._useInsertBlock && newEntry != null && newEntry.HasUsage())
				{
					this.WaitInsertBlock();
				}
				bool flag6 = false;
				if (!flag4)
				{
					try
					{
					}
					finally
					{
						Monitor.Enter(this._lock);
						flag6 = true;
					}
				}
				try
				{
					cacheEntry = (CacheEntry)this._entries[cacheKey];
					if (cacheEntry != null)
					{
						entryState = cacheEntry.State;
						bool flag7 = this._cacheCommon._enableExpiration && cacheEntry.UtcExpires < utcNow;
						if (flag7)
						{
							if (flag4)
							{
								if (entryState == CacheEntry.EntryState.AddedToCache)
								{
									flag = true;
									continue;
								}
								cacheEntry = null;
							}
							else
							{
								replace = true;
								removedReason = CacheItemRemovedReason.Expired;
							}
						}
						else
						{
							flag2 = this._cacheCommon._enableExpiration && cacheEntry.SlidingExpiration > TimeSpan.Zero;
						}
					}
					if (!flag4)
					{
						if (replace && cacheEntry != null)
						{
							bool flag8 = entryState != CacheEntry.EntryState.AddingToCache;
							if (flag8)
							{
								cacheEntry2 = cacheEntry;
								cacheEntry2.State = CacheEntry.EntryState.RemovingFromCache;
								this._entries.Remove(cacheEntry2);
							}
							else if (newEntry == null)
							{
								cacheEntry = null;
							}
						}
						if (newEntry != null)
						{
							bool flag9 = true;
							if (cacheEntry != null && cacheEntry2 == null)
							{
								flag9 = false;
								cacheItemRemovedReason = CacheItemRemovedReason.Removed;
							}
							if (flag9)
							{
								cacheDependency = newEntry.Dependency;
								if (cacheDependency != null && cacheDependency.HasChanged)
								{
									flag9 = false;
									cacheItemRemovedReason = CacheItemRemovedReason.DependencyChanged;
								}
							}
							if (flag9)
							{
								newEntry.State = CacheEntry.EntryState.AddingToCache;
								this._entries.Add(newEntry, newEntry);
								if (flag5)
								{
									cacheEntry = null;
								}
								else
								{
									cacheEntry = newEntry;
								}
							}
							else
							{
								if (!flag5)
								{
									cacheEntry = null;
									flag3 = true;
								}
								else
								{
									flag3 = cacheEntry == null;
								}
								if (!flag3)
								{
									newEntry = null;
								}
							}
						}
					}
				}
				finally
				{
					if (flag6)
					{
						Monitor.Exit(this._lock);
					}
				}
				break;
			}
			if (flag4)
			{
				if (cacheEntry != null)
				{
					if (flag2)
					{
						dateTime = utcNow + cacheEntry.SlidingExpiration;
						if (dateTime - cacheEntry.UtcExpires >= CacheExpires.MIN_UPDATE_DELTA || dateTime < cacheEntry.UtcExpires)
						{
							this._expires.UtcUpdate(cacheEntry, dateTime);
						}
					}
					this.UtcUpdateUsageRecursive(cacheEntry, utcNow);
				}
				if (cacheKey.IsPublic)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.API_CACHE_RATIO_BASE);
					if (cacheEntry != null)
					{
						PerfCounters.IncrementCounter(AppPerfCounter.API_CACHE_HITS);
					}
					else
					{
						PerfCounters.IncrementCounter(AppPerfCounter.API_CACHE_MISSES);
					}
				}
				PerfCounters.IncrementCounter(AppPerfCounter.TOTAL_CACHE_RATIO_BASE);
				if (cacheEntry != null)
				{
					PerfCounters.IncrementCounter(AppPerfCounter.TOTAL_CACHE_HITS);
				}
				else
				{
					PerfCounters.IncrementCounter(AppPerfCounter.TOTAL_CACHE_MISSES);
				}
			}
			else
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				if (cacheEntry2 != null)
				{
					if (cacheEntry2.InExpires())
					{
						this._expires.Remove(cacheEntry2);
					}
					if (cacheEntry2.InUsage())
					{
						this._usage.Remove(cacheEntry2);
					}
					cacheEntry2.State = CacheEntry.EntryState.RemovedFromCache;
					valueOld = cacheEntry2.Value;
					num--;
					num3++;
					if (cacheEntry2.IsPublic)
					{
						num2--;
						num4++;
					}
				}
				if (newEntry != null)
				{
					if (flag3)
					{
						newEntry.State = CacheEntry.EntryState.RemovedFromCache;
						newEntry.Close(cacheItemRemovedReason);
						newEntry = null;
					}
					else
					{
						if (this._cacheCommon._enableExpiration && newEntry.HasExpiration())
						{
							this._expires.Add(newEntry);
						}
						if (this._cacheCommon._enableMemoryCollection && newEntry.HasUsage() && (!newEntry.HasExpiration() || newEntry.SlidingExpiration > TimeSpan.Zero || newEntry.UtcExpires - utcNow >= CacheUsage.MIN_LIFETIME_FOR_USAGE))
						{
							this._usage.Add(newEntry);
						}
						newEntry.State = CacheEntry.EntryState.AddedToCache;
						num++;
						num3++;
						if (newEntry.IsPublic)
						{
							num2++;
							num4++;
						}
					}
				}
				if (cacheEntry2 != null)
				{
					cacheEntry2.Close(removedReason);
				}
				if (newEntry != null)
				{
					newEntry.MonitorDependencyChanges();
					if (cacheDependency != null && cacheDependency.HasChanged)
					{
						base.Remove(newEntry, CacheItemRemovedReason.DependencyChanged);
					}
				}
				if (num == 1)
				{
					Interlocked.Increment(ref this._totalCount);
					PerfCounters.IncrementCounter(AppPerfCounter.TOTAL_CACHE_ENTRIES);
				}
				else if (num == -1)
				{
					Interlocked.Decrement(ref this._totalCount);
					PerfCounters.DecrementCounter(AppPerfCounter.TOTAL_CACHE_ENTRIES);
				}
				if (num2 == 1)
				{
					Interlocked.Increment(ref this._publicCount);
					PerfCounters.IncrementCounter(AppPerfCounter.API_CACHE_ENTRIES);
				}
				else if (num2 == -1)
				{
					Interlocked.Decrement(ref this._publicCount);
					PerfCounters.DecrementCounter(AppPerfCounter.API_CACHE_ENTRIES);
				}
				if (num3 > 0)
				{
					PerfCounters.IncrementCounterEx(AppPerfCounter.TOTAL_CACHE_TURNOVER_RATE, num3);
				}
				if (num4 > 0)
				{
					PerfCounters.IncrementCounterEx(AppPerfCounter.API_CACHE_TURNOVER_RATE, num4);
				}
			}
			return cacheEntry;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002FA08 File Offset: 0x0002EA08
		private void UtcUpdateUsageRecursive(CacheEntry entry, DateTime utcNow)
		{
			if (utcNow - entry.UtcLastUsageUpdate > CacheUsage.CORRELATED_REQUEST_TIMEOUT || utcNow < entry.UtcLastUsageUpdate)
			{
				entry.UtcLastUsageUpdate = utcNow;
				if (entry.InUsage())
				{
					CacheSingle cacheSingle;
					if (this._cacheMultiple == null)
					{
						cacheSingle = this;
					}
					else
					{
						cacheSingle = this._cacheMultiple.GetCacheSingle(entry.Key.GetHashCode());
					}
					cacheSingle._usage.Update(entry);
				}
				CacheDependency dependency = entry.Dependency;
				if (dependency != null)
				{
					CacheEntry[] cacheEntries = dependency.CacheEntries;
					if (cacheEntries != null)
					{
						foreach (CacheEntry cacheEntry in cacheEntries)
						{
							this.UtcUpdateUsageRecursive(cacheEntry, utcNow);
						}
					}
				}
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0002FAB4 File Offset: 0x0002EAB4
		private int CalcMaxCount(CacheMemoryPressure pressure)
		{
			int pressureLast = pressure.PressureLast;
			int pressureAvg = pressure.PressureAvg;
			int pressureHigh = pressure.PressureHigh;
			int pressureLow = pressure.PressureLow;
			int pressureMiddle = pressure.PressureMiddle;
			int totalCount = this._totalCount;
			int num;
			if (pressureLast > pressureMiddle)
			{
				if (pressureLast >= pressureHigh)
				{
					num = (int)((long)totalCount / 2L);
				}
				else
				{
					num = (int)((long)totalCount * (long)(2 * pressureMiddle - pressureLast) / (long)pressureMiddle);
					num = Math.Max((int)(19L * (long)totalCount / 20L), num);
				}
			}
			else if (pressureLast == pressureMiddle)
			{
				if (this._maxCount < 1073741823)
				{
					num = this._maxCount;
				}
				else
				{
					num = totalCount;
				}
			}
			else if (pressureLast >= pressureLow)
			{
				if (pressureAvg < pressureMiddle)
				{
					num = (int)((long)totalCount * (long)(2 * pressureMiddle - pressureLast) / (long)pressureMiddle);
				}
				else
				{
					num = totalCount;
				}
				if (this._maxCount < 1073741823)
				{
					num = Math.Max(num, this._maxCount);
				}
			}
			else
			{
				num = 1073741823;
			}
			return num;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0002FB9C File Offset: 0x0002EB9C
		internal override bool ReviewMemoryStats()
		{
			int num = this.CalcMaxCount(this._cacheCommon._cacheMemoryStats.TotalMemoryPressure);
			if (this._cacheCommon._cacheMemoryStats.PrivateBytesPressure.HasLimit())
			{
				num = Math.Min(num, this.CalcMaxCount(this._cacheCommon._cacheMemoryStats.PrivateBytesPressure));
			}
			num = Math.Max(num, 10);
			num = Math.Min(num, 1073741823);
			this._maxCount = num;
			this._maxCountOverload = this._maxCount + 50;
			return this.TrimIfNeeded();
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002FC28 File Offset: 0x0002EC28
		private bool NeedsTrim()
		{
			bool flag = false;
			if (this._cacheCommon._enableMemoryCollection && (this._totalCount > this._maxCountOverload || (this._totalCount > this._maxCount && this._cacheCommon._cacheMemoryStats.IsAboveMediumPressure())))
			{
				flag = !this._flushUndercount || DateTime.UtcNow - this._utcLastTrimCompleted >= CacheSingle.FLUSH_UNDERCOUNT_WAIT;
			}
			return flag;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0002FC9A File Offset: 0x0002EC9A
		private bool TrimIfNeeded()
		{
			if (this.NeedsTrim())
			{
				this.Trim();
				return true;
			}
			return false;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0002FCB0 File Offset: 0x0002ECB0
		private void Trim()
		{
			int num = 0;
			int num2 = 0;
			try
			{
				do
				{
					int num3 = this._totalCount - this._maxCount;
					int num4 = 0;
					if (num3 > 0)
					{
						num4 = this._expires.FlushExpiredItems(true);
						if (num4 < num3)
						{
							int num5 = this._usage.FlushUnderUsedItems(num3 - num4, ref num);
							num4 += num5;
							num2 += num5;
						}
					}
					this._utcLastTrimCompleted = DateTime.UtcNow;
					this._flushUndercount = num4 < num3;
				}
				while (this.NeedsTrim());
				PerfCounters.IncrementCounterEx(AppPerfCounter.CACHE_TOTAL_TRIMS, num2);
				PerfCounters.IncrementCounterEx(AppPerfCounter.CACHE_API_TRIMS, num);
				PerfCounters.IncrementCounterEx(AppPerfCounter.CACHE_OUTPUT_TRIMS, num2 - num);
			}
			catch
			{
			}
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0002FD54 File Offset: 0x0002ED54
		internal override void EnableExpirationTimer(bool enable)
		{
			if (this._expires != null)
			{
				this._expires.EnableExpirationTimer(enable);
			}
		}

		// Token: 0x040013DD RID: 5085
		private const int MAX_COUNT = 1073741823;

		// Token: 0x040013DE RID: 5086
		private const int MIN_COUNT = 10;

		// Token: 0x040013DF RID: 5087
		private const int MAX_OVERLOAD_COUNT = 50;

		// Token: 0x040013E0 RID: 5088
		private static readonly TimeSpan FLUSH_UNDERCOUNT_WAIT = new TimeSpan(0, 0, 1);

		// Token: 0x040013E1 RID: 5089
		private static readonly TimeSpan INSERT_BLOCK_WAIT = new TimeSpan(0, 0, 10);

		// Token: 0x040013E2 RID: 5090
		private Hashtable _entries;

		// Token: 0x040013E3 RID: 5091
		private CacheExpires _expires;

		// Token: 0x040013E4 RID: 5092
		private CacheUsage _usage;

		// Token: 0x040013E5 RID: 5093
		private object _lock;

		// Token: 0x040013E6 RID: 5094
		private int _disposed;

		// Token: 0x040013E7 RID: 5095
		private int _totalCount;

		// Token: 0x040013E8 RID: 5096
		private int _publicCount;

		// Token: 0x040013E9 RID: 5097
		private int _maxCount;

		// Token: 0x040013EA RID: 5098
		private int _maxCountOverload;

		// Token: 0x040013EB RID: 5099
		private bool _flushUndercount;

		// Token: 0x040013EC RID: 5100
		private ManualResetEvent _insertBlock;

		// Token: 0x040013ED RID: 5101
		private bool _useInsertBlock;

		// Token: 0x040013EE RID: 5102
		private int _insertBlockCalls;

		// Token: 0x040013EF RID: 5103
		private DateTime _utcLastTrimCompleted;

		// Token: 0x040013F0 RID: 5104
		private int _iSubCache;

		// Token: 0x040013F1 RID: 5105
		private CacheMultiple _cacheMultiple;
	}
}

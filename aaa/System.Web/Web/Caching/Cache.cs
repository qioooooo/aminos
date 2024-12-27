using System;
using System.Collections;
using System.Security.Permissions;
using System.Web.Management;
using System.Web.Util;

namespace System.Web.Caching
{
	// Token: 0x020000FB RID: 251
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class Cache : IEnumerable
	{
		// Token: 0x06000BAF RID: 2991 RVA: 0x0002E6A9 File Offset: 0x0002D6A9
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public Cache()
		{
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0002E6B1 File Offset: 0x0002D6B1
		internal Cache(int dummy)
		{
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002E6B9 File Offset: 0x0002D6B9
		internal void SetCacheInternal(CacheInternal cacheInternal)
		{
			this._cacheInternal = cacheInternal;
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0002E6C2 File Offset: 0x0002D6C2
		public int Count
		{
			get
			{
				return this._cacheInternal.PublicCount;
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002E6CF File Offset: 0x0002D6CF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this._cacheInternal).GetEnumerator();
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002E6DC File Offset: 0x0002D6DC
		public IDictionaryEnumerator GetEnumerator()
		{
			return this._cacheInternal.GetEnumerator();
		}

		// Token: 0x17000341 RID: 833
		public object this[string key]
		{
			get
			{
				return this.Get(key);
			}
			set
			{
				this.Insert(key, value);
			}
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0002E6FC File Offset: 0x0002D6FC
		public object Get(string key)
		{
			return this._cacheInternal.DoGet(true, key, CacheGetOptions.None);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0002E70C File Offset: 0x0002D70C
		internal object Get(string key, CacheGetOptions getOptions)
		{
			return this._cacheInternal.DoGet(true, key, getOptions);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002E71C File Offset: 0x0002D71C
		public void Insert(string key, object value)
		{
			this._cacheInternal.DoInsert(true, key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002E748 File Offset: 0x0002D748
		public void Insert(string key, object value, CacheDependency dependencies)
		{
			this._cacheInternal.DoInsert(true, key, value, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0002E774 File Offset: 0x0002D774
		public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
			this._cacheInternal.DoInsert(true, key, value, dependencies, dateTime, slidingExpiration, CacheItemPriority.Normal, null, true);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0002E7A0 File Offset: 0x0002D7A0
		public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
			this._cacheInternal.DoInsert(true, key, value, dependencies, dateTime, slidingExpiration, priority, onRemoveCallback, true);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002E7D0 File Offset: 0x0002D7D0
		public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
		{
			if (dependencies == null && absoluteExpiration == Cache.NoAbsoluteExpiration && slidingExpiration == Cache.NoSlidingExpiration)
			{
				throw new ArgumentException(SR.GetString("Invalid_Parameters_To_Insert"));
			}
			if (onUpdateCallback == null)
			{
				throw new ArgumentNullException("onUpdateCallback");
			}
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
			this._cacheInternal.DoInsert(true, key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null, true);
			string[] array = new string[] { key };
			CacheDependency cacheDependency = new CacheDependency(null, array);
			if (dependencies == null)
			{
				dependencies = cacheDependency;
			}
			else
			{
				AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
				aggregateCacheDependency.Add(new CacheDependency[] { dependencies, cacheDependency });
				dependencies = aggregateCacheDependency;
			}
			this._cacheInternal.DoInsert(false, "w" + key, new Cache.SentinelEntry(key, cacheDependency, onUpdateCallback), dependencies, dateTime, slidingExpiration, CacheItemPriority.NotRemovable, Cache.s_sentinelRemovedCallback, true);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0002E8B0 File Offset: 0x0002D8B0
		public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
			return this._cacheInternal.DoInsert(true, key, value, dependencies, dateTime, slidingExpiration, priority, onRemoveCallback, false);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0002E8DC File Offset: 0x0002D8DC
		public object Remove(string key)
		{
			CacheKey cacheKey = new CacheKey(key, true);
			return this._cacheInternal.DoRemove(cacheKey, CacheItemRemovedReason.Removed);
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0002E8FE File Offset: 0x0002D8FE
		public long EffectivePrivateBytesLimit
		{
			get
			{
				return this._cacheInternal.EffectivePrivateBytesLimit;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0002E90B File Offset: 0x0002D90B
		public long EffectivePercentagePhysicalMemoryLimit
		{
			get
			{
				return this._cacheInternal.EffectivePercentagePhysicalMemoryLimit;
			}
		}

		// Token: 0x040013AA RID: 5034
		public static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;

		// Token: 0x040013AB RID: 5035
		public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

		// Token: 0x040013AC RID: 5036
		private CacheInternal _cacheInternal;

		// Token: 0x040013AD RID: 5037
		private static CacheItemRemovedCallback s_sentinelRemovedCallback = new CacheItemRemovedCallback(Cache.SentinelEntry.OnCacheItemRemovedCallback);

		// Token: 0x020000FC RID: 252
		private class SentinelEntry
		{
			// Token: 0x06000BC3 RID: 3011 RVA: 0x0002E93F File Offset: 0x0002D93F
			public SentinelEntry(string key, CacheDependency expensiveObjectDependency, CacheItemUpdateCallback callback)
			{
				this._key = key;
				this._expensiveObjectDependency = expensiveObjectDependency;
				this._cacheItemUpdateCallback = callback;
			}

			// Token: 0x17000344 RID: 836
			// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0002E95C File Offset: 0x0002D95C
			public string Key
			{
				get
				{
					return this._key;
				}
			}

			// Token: 0x17000345 RID: 837
			// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0002E964 File Offset: 0x0002D964
			public CacheDependency ExpensiveObjectDependency
			{
				get
				{
					return this._expensiveObjectDependency;
				}
			}

			// Token: 0x17000346 RID: 838
			// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0002E96C File Offset: 0x0002D96C
			public CacheItemUpdateCallback CacheItemUpdateCallback
			{
				get
				{
					return this._cacheItemUpdateCallback;
				}
			}

			// Token: 0x06000BC7 RID: 3015 RVA: 0x0002E974 File Offset: 0x0002D974
			public static void OnCacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
			{
				Cache.SentinelEntry sentinelEntry = value as Cache.SentinelEntry;
				switch (reason)
				{
				case CacheItemRemovedReason.Expired:
				{
					CacheItemUpdateReason cacheItemUpdateReason = CacheItemUpdateReason.Expired;
					goto IL_0034;
				}
				case CacheItemRemovedReason.Underused:
					break;
				case CacheItemRemovedReason.DependencyChanged:
				{
					CacheItemUpdateReason cacheItemUpdateReason = CacheItemUpdateReason.DependencyChanged;
					if (!sentinelEntry.ExpensiveObjectDependency.HasChanged)
					{
						goto IL_0034;
					}
					break;
				}
				default:
					return;
				}
				return;
				IL_0034:
				CacheItemUpdateCallback cacheItemUpdateCallback = sentinelEntry.CacheItemUpdateCallback;
				try
				{
					CacheItemUpdateReason cacheItemUpdateReason;
					object obj;
					CacheDependency cacheDependency;
					DateTime dateTime;
					TimeSpan timeSpan;
					cacheItemUpdateCallback(sentinelEntry.Key, cacheItemUpdateReason, out obj, out cacheDependency, out dateTime, out timeSpan);
					if (obj != null)
					{
						HttpRuntime.Cache.Insert(sentinelEntry.Key, obj, cacheDependency, dateTime, timeSpan, sentinelEntry.CacheItemUpdateCallback);
					}
					else
					{
						HttpRuntime.Cache.Remove(sentinelEntry.Key);
					}
				}
				catch (Exception ex)
				{
					HttpRuntime.Cache.Remove(sentinelEntry.Key);
					try
					{
						WebBaseEvent.RaiseRuntimeError(ex, value);
					}
					catch
					{
					}
				}
			}

			// Token: 0x040013AE RID: 5038
			private string _key;

			// Token: 0x040013AF RID: 5039
			private CacheDependency _expensiveObjectDependency;

			// Token: 0x040013B0 RID: 5040
			private CacheItemUpdateCallback _cacheItemUpdateCallback;
		}
	}
}

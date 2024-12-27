using System;
using System.Collections;
using System.Web.Management;

namespace System.Web.Caching
{
	// Token: 0x02000108 RID: 264
	internal sealed class CacheEntry : CacheKey, ICacheDependencyChanged
	{
		// Token: 0x06000C4A RID: 3146 RVA: 0x000310F8 File Offset: 0x000300F8
		internal CacheEntry(string key, object value, CacheDependency dependency, CacheItemRemovedCallback onRemovedHandler, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, bool isPublic)
			: base(key, isPublic)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (slidingExpiration < TimeSpan.Zero || CacheEntry.OneYear < slidingExpiration)
			{
				throw new ArgumentOutOfRangeException("slidingExpiration");
			}
			if (utcAbsoluteExpiration != Cache.NoAbsoluteExpiration && slidingExpiration != Cache.NoSlidingExpiration)
			{
				throw new ArgumentException(SR.GetString("Invalid_expiration_combination"));
			}
			if (priority < CacheItemPriority.Low || CacheItemPriority.NotRemovable < priority)
			{
				throw new ArgumentOutOfRangeException("priority");
			}
			this._value = value;
			this._dependency = dependency;
			this._onRemovedTargets = onRemovedHandler;
			this._utcCreated = DateTime.UtcNow;
			this._slidingExpiration = slidingExpiration;
			if (this._slidingExpiration > TimeSpan.Zero)
			{
				this._utcExpires = this._utcCreated + this._slidingExpiration;
			}
			else
			{
				this._utcExpires = utcAbsoluteExpiration;
			}
			this._expiresEntryRef = ExpiresEntryRef.INVALID;
			this._expiresBucket = byte.MaxValue;
			this._usageEntryRef = UsageEntryRef.INVALID;
			if (priority == CacheItemPriority.NotRemovable)
			{
				this._usageBucket = byte.MaxValue;
				return;
			}
			this._usageBucket = (byte)(priority - CacheItemPriority.Low);
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x0003121D File Offset: 0x0003021D
		internal object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00031225 File Offset: 0x00030225
		internal DateTime UtcCreated
		{
			get
			{
				return this._utcCreated;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0003122D File Offset: 0x0003022D
		// (set) Token: 0x06000C4E RID: 3150 RVA: 0x00031239 File Offset: 0x00030239
		internal CacheEntry.EntryState State
		{
			get
			{
				return (CacheEntry.EntryState)(this._bits & 31);
			}
			set
			{
				this._bits = (byte)(((int)this._bits & -32) | (int)value);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0003124D File Offset: 0x0003024D
		// (set) Token: 0x06000C50 RID: 3152 RVA: 0x00031255 File Offset: 0x00030255
		internal DateTime UtcExpires
		{
			get
			{
				return this._utcExpires;
			}
			set
			{
				this._utcExpires = value;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x0003125E File Offset: 0x0003025E
		internal TimeSpan SlidingExpiration
		{
			get
			{
				return this._slidingExpiration;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00031266 File Offset: 0x00030266
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x0003126E File Offset: 0x0003026E
		internal byte ExpiresBucket
		{
			get
			{
				return this._expiresBucket;
			}
			set
			{
				this._expiresBucket = value;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x00031277 File Offset: 0x00030277
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x0003127F File Offset: 0x0003027F
		internal ExpiresEntryRef ExpiresEntryRef
		{
			get
			{
				return this._expiresEntryRef;
			}
			set
			{
				this._expiresEntryRef = value;
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00031288 File Offset: 0x00030288
		internal bool HasExpiration()
		{
			return this._utcExpires < DateTime.MaxValue;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003129A File Offset: 0x0003029A
		internal bool InExpires()
		{
			return !this._expiresEntryRef.IsInvalid;
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x000312AA File Offset: 0x000302AA
		internal byte UsageBucket
		{
			get
			{
				return this._usageBucket;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x000312B2 File Offset: 0x000302B2
		// (set) Token: 0x06000C5A RID: 3162 RVA: 0x000312BA File Offset: 0x000302BA
		internal UsageEntryRef UsageEntryRef
		{
			get
			{
				return this._usageEntryRef;
			}
			set
			{
				this._usageEntryRef = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x000312C3 File Offset: 0x000302C3
		// (set) Token: 0x06000C5C RID: 3164 RVA: 0x000312CB File Offset: 0x000302CB
		internal DateTime UtcLastUsageUpdate
		{
			get
			{
				return this._utcLastUpdate;
			}
			set
			{
				this._utcLastUpdate = value;
			}
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000312D4 File Offset: 0x000302D4
		internal bool HasUsage()
		{
			return this._usageBucket != byte.MaxValue;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000312E6 File Offset: 0x000302E6
		internal bool InUsage()
		{
			return !this._usageEntryRef.IsInvalid;
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x000312F6 File Offset: 0x000302F6
		internal CacheDependency Dependency
		{
			get
			{
				return this._dependency;
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00031300 File Offset: 0x00030300
		internal void MonitorDependencyChanges()
		{
			CacheDependency dependency = this._dependency;
			if (dependency != null && this.State == CacheEntry.EntryState.AddedToCache)
			{
				if (!dependency.Use())
				{
					throw new InvalidOperationException(SR.GetString("Cache_dependency_used_more_that_once"));
				}
				dependency.SetCacheDependencyChanged(this);
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0003133F File Offset: 0x0003033F
		void ICacheDependencyChanged.DependencyChanged(object sender, EventArgs e)
		{
			if (this.State == CacheEntry.EntryState.AddedToCache)
			{
				HttpRuntime.CacheInternal.Remove(this, CacheItemRemovedReason.DependencyChanged);
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00031358 File Offset: 0x00030358
		private void CallCacheItemRemovedCallback(CacheItemRemovedCallback callback, CacheItemRemovedReason reason)
		{
			if (base.IsPublic)
			{
				try
				{
					if (HttpContext.Current == null)
					{
						using (new ApplicationImpersonationContext())
						{
							callback(this._key, this._value, reason);
							goto IL_0047;
						}
					}
					callback(this._key, this._value, reason);
					IL_0047:
					return;
				}
				catch (Exception ex)
				{
					HttpApplicationFactory.RaiseError(ex);
					try
					{
						WebBaseEvent.RaiseRuntimeError(ex, this);
					}
					catch
					{
					}
					return;
				}
			}
			try
			{
				using (new ApplicationImpersonationContext())
				{
					callback(this._key, this._value, reason);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00031430 File Offset: 0x00030430
		internal void Close(CacheItemRemovedReason reason)
		{
			this.State = CacheEntry.EntryState.Closed;
			object obj = null;
			object[] array = null;
			lock (this)
			{
				if (this._onRemovedTargets != null)
				{
					obj = this._onRemovedTargets;
					if (obj is Hashtable)
					{
						ICollection keys = ((Hashtable)obj).Keys;
						array = new object[keys.Count];
						keys.CopyTo(array, 0);
					}
				}
			}
			if (obj != null)
			{
				if (array != null)
				{
					foreach (object obj2 in array)
					{
						if (obj2 is CacheDependency)
						{
							((CacheDependency)obj2).ItemRemoved();
						}
						else
						{
							this.CallCacheItemRemovedCallback((CacheItemRemovedCallback)obj2, reason);
						}
					}
				}
				else if (obj is CacheItemRemovedCallback)
				{
					this.CallCacheItemRemovedCallback((CacheItemRemovedCallback)obj, reason);
				}
				else
				{
					((CacheDependency)obj).ItemRemoved();
				}
			}
			if (this._dependency != null)
			{
				this._dependency.DisposeInternal();
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00031520 File Offset: 0x00030520
		internal void AddCacheDependencyNotify(CacheDependency dependency)
		{
			lock (this)
			{
				if (this._onRemovedTargets == null)
				{
					this._onRemovedTargets = dependency;
				}
				else if (this._onRemovedTargets is Hashtable)
				{
					Hashtable hashtable = (Hashtable)this._onRemovedTargets;
					hashtable[dependency] = dependency;
				}
				else
				{
					Hashtable hashtable2 = new Hashtable(2);
					hashtable2[this._onRemovedTargets] = this._onRemovedTargets;
					hashtable2[dependency] = dependency;
					this._onRemovedTargets = hashtable2;
				}
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x000315AC File Offset: 0x000305AC
		internal void RemoveCacheDependencyNotify(CacheDependency dependency)
		{
			lock (this)
			{
				if (this._onRemovedTargets != null)
				{
					if (this._onRemovedTargets == dependency)
					{
						this._onRemovedTargets = null;
					}
					else
					{
						Hashtable hashtable = (Hashtable)this._onRemovedTargets;
						hashtable.Remove(dependency);
						if (hashtable.Count == 0)
						{
							this._onRemovedTargets = null;
						}
					}
				}
			}
		}

		// Token: 0x04001411 RID: 5137
		private const CacheItemPriority CacheItemPriorityMin = CacheItemPriority.Low;

		// Token: 0x04001412 RID: 5138
		private const CacheItemPriority CacheItemPriorityMax = CacheItemPriority.NotRemovable;

		// Token: 0x04001413 RID: 5139
		private const byte EntryStateMask = 31;

		// Token: 0x04001414 RID: 5140
		private static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;

		// Token: 0x04001415 RID: 5141
		private static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

		// Token: 0x04001416 RID: 5142
		private static readonly TimeSpan OneYear = new TimeSpan(365, 0, 0, 0);

		// Token: 0x04001417 RID: 5143
		private object _value;

		// Token: 0x04001418 RID: 5144
		private DateTime _utcCreated;

		// Token: 0x04001419 RID: 5145
		private DateTime _utcExpires;

		// Token: 0x0400141A RID: 5146
		private TimeSpan _slidingExpiration;

		// Token: 0x0400141B RID: 5147
		private byte _expiresBucket;

		// Token: 0x0400141C RID: 5148
		private ExpiresEntryRef _expiresEntryRef;

		// Token: 0x0400141D RID: 5149
		private byte _usageBucket;

		// Token: 0x0400141E RID: 5150
		private UsageEntryRef _usageEntryRef;

		// Token: 0x0400141F RID: 5151
		private DateTime _utcLastUpdate;

		// Token: 0x04001420 RID: 5152
		private CacheDependency _dependency;

		// Token: 0x04001421 RID: 5153
		private object _onRemovedTargets;

		// Token: 0x02000109 RID: 265
		internal enum EntryState : byte
		{
			// Token: 0x04001423 RID: 5155
			NotInCache,
			// Token: 0x04001424 RID: 5156
			AddingToCache,
			// Token: 0x04001425 RID: 5157
			AddedToCache,
			// Token: 0x04001426 RID: 5158
			RemovingFromCache = 4,
			// Token: 0x04001427 RID: 5159
			RemovedFromCache = 8,
			// Token: 0x04001428 RID: 5160
			Closed = 16
		}
	}
}

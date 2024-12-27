using System;
using System.Collections;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.Caching
{
	// Token: 0x020000FE RID: 254
	internal abstract class CacheInternal : IEnumerable, IDisposable
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000BD1 RID: 3025
		internal abstract int PublicCount { get; }

		// Token: 0x06000BD2 RID: 3026
		internal abstract IDictionaryEnumerator CreateEnumerator();

		// Token: 0x06000BD3 RID: 3027
		internal abstract CacheEntry UpdateCache(CacheKey cacheKey, CacheEntry newEntry, bool replace, CacheItemRemovedReason removedReason, out object valueOld);

		// Token: 0x06000BD4 RID: 3028
		internal abstract bool ReviewMemoryStats();

		// Token: 0x06000BD5 RID: 3029
		internal abstract void EnableExpirationTimer(bool enable);

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0002EEA0 File Offset: 0x0002DEA0
		internal static CacheInternal Create()
		{
			int num = 0;
			if (num == 0)
			{
				uint num2 = (uint)SystemInfo.GetNumProcessCPUs();
				num = 1;
				for (num2 -= 1U; num2 > 0U; num2 >>= 1)
				{
					num <<= 1;
				}
			}
			CacheCommon cacheCommon = new CacheCommon();
			CacheInternal cacheInternal;
			if (num == 1)
			{
				cacheInternal = new CacheSingle(cacheCommon, null, 0);
			}
			else
			{
				cacheInternal = new CacheMultiple(cacheCommon, num);
			}
			cacheCommon.SetCacheInternal(cacheInternal);
			cacheCommon.ResetFromConfigSettings();
			return cacheInternal;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0002EEF7 File Offset: 0x0002DEF7
		protected CacheInternal(CacheCommon cacheCommon)
		{
			this._cacheCommon = cacheCommon;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0002EF06 File Offset: 0x0002DF06
		protected virtual void Dispose(bool disposing)
		{
			this._cacheCommon.Dispose(disposing);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0002EF14 File Offset: 0x0002DF14
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0002EF1D File Offset: 0x0002DF1D
		internal void ReadCacheInternalConfig(CacheSection cacheSection)
		{
			this._cacheCommon.ReadCacheInternalConfig(cacheSection);
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0002EF2B File Offset: 0x0002DF2B
		internal Cache CachePublic
		{
			get
			{
				return this._cacheCommon._cachePublic;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0002EF38 File Offset: 0x0002DF38
		internal long EffectivePrivateBytesLimit
		{
			get
			{
				return this._cacheCommon._cacheMemoryStats.PrivateBytesPressure.PressureHighMemoryLimit;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0002EF4F File Offset: 0x0002DF4F
		internal long EffectivePercentagePhysicalMemoryLimit
		{
			get
			{
				return this._cacheCommon._cacheMemoryStats.TotalMemoryPressure.MemoryLimit;
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0002EF66 File Offset: 0x0002DF66
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.CreateEnumerator();
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0002EF6E File Offset: 0x0002DF6E
		public IDictionaryEnumerator GetEnumerator()
		{
			return this.CreateEnumerator();
		}

		// Token: 0x1700034B RID: 843
		internal object this[string key]
		{
			get
			{
				return this.Get(key);
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002EF7F File Offset: 0x0002DF7F
		internal object Get(string key)
		{
			return this.DoGet(false, key, CacheGetOptions.None);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0002EF8A File Offset: 0x0002DF8A
		internal object Get(string key, CacheGetOptions getOptions)
		{
			return this.DoGet(false, key, getOptions);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002EF98 File Offset: 0x0002DF98
		internal object DoGet(bool isPublic, string key, CacheGetOptions getOptions)
		{
			CacheKey cacheKey = new CacheKey(key, isPublic);
			object obj;
			CacheEntry cacheEntry = this.UpdateCache(cacheKey, null, false, CacheItemRemovedReason.Removed, out obj);
			if (cacheEntry == null)
			{
				return null;
			}
			if ((getOptions & CacheGetOptions.ReturnCacheEntry) != CacheGetOptions.None)
			{
				return cacheEntry;
			}
			return cacheEntry.Value;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002EFCC File Offset: 0x0002DFCC
		internal void UtcInsert(string key, object value)
		{
			this.DoInsert(false, key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002EFF4 File Offset: 0x0002DFF4
		internal void UtcInsert(string key, object value, CacheDependency dependencies)
		{
			this.DoInsert(false, key, value, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002F01C File Offset: 0x0002E01C
		internal void UtcInsert(string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration)
		{
			this.DoInsert(false, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, CacheItemPriority.Normal, null, true);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002F03C File Offset: 0x0002E03C
		internal void UtcInsert(string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			this.DoInsert(false, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback, true);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0002F060 File Offset: 0x0002E060
		internal object UtcAdd(string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			return this.DoInsert(false, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback, false);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002F080 File Offset: 0x0002E080
		internal object DoInsert(bool isPublic, string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback, bool replace)
		{
			object obj2;
			try
			{
				CacheEntry cacheEntry = new CacheEntry(key, value, dependencies, onRemoveCallback, utcAbsoluteExpiration, slidingExpiration, priority, isPublic);
				object obj;
				cacheEntry = this.UpdateCache(cacheEntry, cacheEntry, replace, CacheItemRemovedReason.Removed, out obj);
				if (cacheEntry != null)
				{
					obj2 = cacheEntry.Value;
				}
				else
				{
					obj2 = null;
				}
			}
			finally
			{
				if (dependencies != null)
				{
					((IDisposable)dependencies).Dispose();
				}
			}
			return obj2;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0002F0DC File Offset: 0x0002E0DC
		internal object Remove(string key)
		{
			CacheKey cacheKey = new CacheKey(key, false);
			return this.DoRemove(cacheKey, CacheItemRemovedReason.Removed);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0002F0F9 File Offset: 0x0002E0F9
		internal object Remove(CacheKey cacheKey, CacheItemRemovedReason reason)
		{
			return this.DoRemove(cacheKey, reason);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0002F104 File Offset: 0x0002E104
		internal object DoRemove(CacheKey cacheKey, CacheItemRemovedReason reason)
		{
			object obj;
			this.UpdateCache(cacheKey, null, true, reason, out obj);
			return obj;
		}

		// Token: 0x040013C1 RID: 5057
		internal const string PrefixFIRST = "A";

		// Token: 0x040013C2 RID: 5058
		internal const string PrefixResourceProvider = "A";

		// Token: 0x040013C3 RID: 5059
		internal const string PrefixOutputCache = "a";

		// Token: 0x040013C4 RID: 5060
		internal const string PrefixSqlCacheDependency = "b";

		// Token: 0x040013C5 RID: 5061
		internal const string PrefixMemoryBuildResult = "c";

		// Token: 0x040013C6 RID: 5062
		internal const string PrefixPathData = "d";

		// Token: 0x040013C7 RID: 5063
		internal const string PrefixHttpCapabilities = "e";

		// Token: 0x040013C8 RID: 5064
		internal const string PrefixMapPath = "f";

		// Token: 0x040013C9 RID: 5065
		internal const string PrefixHttpSys = "g";

		// Token: 0x040013CA RID: 5066
		internal const string PrefixFileSecurity = "h";

		// Token: 0x040013CB RID: 5067
		internal const string PrefixInProcSessionState = "j";

		// Token: 0x040013CC RID: 5068
		internal const string PrefixStateApplication = "k";

		// Token: 0x040013CD RID: 5069
		internal const string PrefixPartialCachingControl = "l";

		// Token: 0x040013CE RID: 5070
		internal const string UNUSED = "m";

		// Token: 0x040013CF RID: 5071
		internal const string PrefixAdRotator = "n";

		// Token: 0x040013D0 RID: 5072
		internal const string PrefixWebServiceDataSource = "o";

		// Token: 0x040013D1 RID: 5073
		internal const string PrefixLoadXPath = "p";

		// Token: 0x040013D2 RID: 5074
		internal const string PrefixLoadXml = "q";

		// Token: 0x040013D3 RID: 5075
		internal const string PrefixLoadTransform = "r";

		// Token: 0x040013D4 RID: 5076
		internal const string PrefixAspCompatThreading = "s";

		// Token: 0x040013D5 RID: 5077
		internal const string PrefixDataSourceControl = "u";

		// Token: 0x040013D6 RID: 5078
		internal const string PrefixValidationSentinel = "w";

		// Token: 0x040013D7 RID: 5079
		internal const string PrefixWebEventResource = "x";

		// Token: 0x040013D8 RID: 5080
		internal const string PrefixAssemblyPath = "y";

		// Token: 0x040013D9 RID: 5081
		internal const string PrefixBrowserCapsHash = "z";

		// Token: 0x040013DA RID: 5082
		internal const string PrefixLAST = "z";

		// Token: 0x040013DB RID: 5083
		protected CacheCommon _cacheCommon;
	}
}

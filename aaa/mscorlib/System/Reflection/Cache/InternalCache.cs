using System;
using System.Diagnostics;

namespace System.Reflection.Cache
{
	// Token: 0x02000844 RID: 2116
	[Serializable]
	internal class InternalCache
	{
		// Token: 0x06004E05 RID: 19973 RVA: 0x0010F7F4 File Offset: 0x0010E7F4
		internal InternalCache(string cacheName)
		{
		}

		// Token: 0x17000D95 RID: 3477
		internal object this[CacheObjType cacheType]
		{
			get
			{
				InternalCacheItem[] cache = this.m_cache;
				int numItems = this.m_numItems;
				int num = this.FindObjectPosition(cache, numItems, cacheType, false);
				if (num >= 0)
				{
					bool loggingNotEnabled = BCLDebug.m_loggingNotEnabled;
					return cache[num].Value;
				}
				bool loggingNotEnabled2 = BCLDebug.m_loggingNotEnabled;
				return null;
			}
			set
			{
				bool loggingNotEnabled = BCLDebug.m_loggingNotEnabled;
				lock (this)
				{
					int num = this.FindObjectPosition(this.m_cache, this.m_numItems, cacheType, true);
					if (num > 0)
					{
						this.m_cache[num].Value = value;
						this.m_cache[num].Key = cacheType;
						if (num == this.m_numItems)
						{
							this.m_numItems++;
						}
					}
					else if (this.m_cache == null)
					{
						bool loggingNotEnabled2 = BCLDebug.m_loggingNotEnabled;
						this.m_cache = new InternalCacheItem[2];
						this.m_cache[0].Value = value;
						this.m_cache[0].Key = cacheType;
						this.m_numItems = 1;
					}
					else
					{
						bool loggingNotEnabled3 = BCLDebug.m_loggingNotEnabled;
						InternalCacheItem[] array = new InternalCacheItem[this.m_numItems * 2];
						for (int i = 0; i < this.m_numItems; i++)
						{
							array[i] = this.m_cache[i];
						}
						array[this.m_numItems].Value = value;
						array[this.m_numItems].Key = cacheType;
						this.m_cache = array;
						this.m_numItems++;
					}
				}
			}
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0010F99C File Offset: 0x0010E99C
		private int FindObjectPosition(InternalCacheItem[] cache, int itemCount, CacheObjType cacheType, bool findEmpty)
		{
			if (cache == null)
			{
				return -1;
			}
			if (itemCount > cache.Length)
			{
				itemCount = cache.Length;
			}
			for (int i = 0; i < itemCount; i++)
			{
				if (cacheType == cache[i].Key)
				{
					return i;
				}
			}
			if (findEmpty && itemCount < cache.Length - 1)
			{
				return itemCount + 1;
			}
			return -1;
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x0010F9E7 File Offset: 0x0010E9E7
		[Conditional("_LOGGING")]
		private void LogAction(CacheAction action, CacheObjType cacheType)
		{
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x0010F9E9 File Offset: 0x0010E9E9
		[Conditional("_LOGGING")]
		private void LogAction(CacheAction action, CacheObjType cacheType, object obj)
		{
		}

		// Token: 0x04002821 RID: 10273
		private const int MinCacheSize = 2;

		// Token: 0x04002822 RID: 10274
		private InternalCacheItem[] m_cache;

		// Token: 0x04002823 RID: 10275
		private int m_numItems;
	}
}

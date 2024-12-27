using System;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200000B RID: 11
	internal sealed class NameCache
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00004FF4 File Offset: 0x00003FF4
		internal object GetCachedValue(string name)
		{
			this.name = name;
			this.probe = Math.Abs(name.GetHashCode()) % 353;
			NameCacheEntry nameCacheEntry = NameCache.nameCache[this.probe];
			if (nameCacheEntry == null)
			{
				nameCacheEntry = new NameCacheEntry();
				nameCacheEntry.name = name;
				return null;
			}
			if (nameCacheEntry.name == name)
			{
				return nameCacheEntry.value;
			}
			return null;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005054 File Offset: 0x00004054
		internal void SetCachedValue(object value)
		{
			NameCacheEntry nameCacheEntry = new NameCacheEntry();
			nameCacheEntry.name = this.name;
			nameCacheEntry.value = value;
			NameCache.nameCache[this.probe] = nameCacheEntry;
		}

		// Token: 0x04000040 RID: 64
		private const int MAX_CACHE_ENTRIES = 353;

		// Token: 0x04000041 RID: 65
		private static NameCacheEntry[] nameCache = new NameCacheEntry[353];

		// Token: 0x04000042 RID: 66
		private int probe;

		// Token: 0x04000043 RID: 67
		private string name;
	}
}

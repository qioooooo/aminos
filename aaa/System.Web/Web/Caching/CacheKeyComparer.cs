using System;
using System.Collections;

namespace System.Web.Caching
{
	// Token: 0x020000FF RID: 255
	internal sealed class CacheKeyComparer : IEqualityComparer
	{
		// Token: 0x06000BED RID: 3053 RVA: 0x0002F11F File Offset: 0x0002E11F
		internal static CacheKeyComparer GetInstance()
		{
			if (CacheKeyComparer.s_comparerInstance == null)
			{
				CacheKeyComparer.s_comparerInstance = new CacheKeyComparer();
			}
			return CacheKeyComparer.s_comparerInstance;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0002F137 File Offset: 0x0002E137
		private CacheKeyComparer()
		{
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0002F13F File Offset: 0x0002E13F
		bool IEqualityComparer.Equals(object x, object y)
		{
			return this.Compare(x, y) == 0;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0002F14C File Offset: 0x0002E14C
		private int Compare(object x, object y)
		{
			CacheKey cacheKey = (CacheKey)x;
			CacheKey cacheKey2 = (CacheKey)y;
			if (cacheKey.IsPublic)
			{
				if (cacheKey2.IsPublic)
				{
					return string.Compare(cacheKey.Key, cacheKey2.Key, StringComparison.Ordinal);
				}
				return 1;
			}
			else
			{
				if (!cacheKey2.IsPublic)
				{
					return string.Compare(cacheKey.Key, cacheKey2.Key, StringComparison.Ordinal);
				}
				return -1;
			}
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0002F1A8 File Offset: 0x0002E1A8
		int IEqualityComparer.GetHashCode(object obj)
		{
			CacheKey cacheKey = (CacheKey)obj;
			return cacheKey.GetHashCode();
		}

		// Token: 0x040013DC RID: 5084
		private static CacheKeyComparer s_comparerInstance;
	}
}

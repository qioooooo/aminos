using System;

namespace System.Web.Caching
{
	// Token: 0x020000F6 RID: 246
	// (Invoke) Token: 0x06000BAC RID: 2988
	public delegate void CacheItemUpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration);
}

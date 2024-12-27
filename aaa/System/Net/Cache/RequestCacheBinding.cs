using System;

namespace System.Net.Cache
{
	// Token: 0x0200056A RID: 1386
	internal class RequestCacheBinding
	{
		// Token: 0x06002A8D RID: 10893 RVA: 0x000B4E9C File Offset: 0x000B3E9C
		internal RequestCacheBinding(RequestCache requestCache, RequestCacheValidator cacheValidator, RequestCachePolicy policy)
		{
			this.m_RequestCache = requestCache;
			this.m_CacheValidator = cacheValidator;
			this.m_Policy = policy;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x000B4EB9 File Offset: 0x000B3EB9
		internal RequestCache Cache
		{
			get
			{
				return this.m_RequestCache;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06002A8F RID: 10895 RVA: 0x000B4EC1 File Offset: 0x000B3EC1
		internal RequestCacheValidator Validator
		{
			get
			{
				return this.m_CacheValidator;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000B4EC9 File Offset: 0x000B3EC9
		internal RequestCachePolicy Policy
		{
			get
			{
				return this.m_Policy;
			}
		}

		// Token: 0x04002909 RID: 10505
		private RequestCache m_RequestCache;

		// Token: 0x0400290A RID: 10506
		private RequestCacheValidator m_CacheValidator;

		// Token: 0x0400290B RID: 10507
		private RequestCachePolicy m_Policy;
	}
}

using System;

namespace System.Net.Cache
{
	// Token: 0x0200056C RID: 1388
	public class RequestCachePolicy
	{
		// Token: 0x06002A91 RID: 10897 RVA: 0x000B4ED1 File Offset: 0x000B3ED1
		public RequestCachePolicy()
			: this(RequestCacheLevel.Default)
		{
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000B4EDA File Offset: 0x000B3EDA
		public RequestCachePolicy(RequestCacheLevel level)
		{
			if (level < RequestCacheLevel.Default || level > RequestCacheLevel.NoCacheNoStore)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.m_Level = level;
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002A93 RID: 10899 RVA: 0x000B4EFC File Offset: 0x000B3EFC
		public RequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000B4F04 File Offset: 0x000B3F04
		public override string ToString()
		{
			return "Level:" + this.m_Level.ToString();
		}

		// Token: 0x04002914 RID: 10516
		private RequestCacheLevel m_Level;
	}
}

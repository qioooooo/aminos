using System;
using System.Text;

namespace System.Net.Cache
{
	// Token: 0x02000562 RID: 1378
	internal class ResponseCacheControl
	{
		// Token: 0x06002A27 RID: 10791 RVA: 0x000B1528 File Offset: 0x000B0528
		internal ResponseCacheControl()
		{
			this.MaxAge = (this.SMaxAge = -1);
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002A28 RID: 10792 RVA: 0x000B154C File Offset: 0x000B054C
		internal bool IsNotEmpty
		{
			get
			{
				return this.Public || this.Private || this.NoCache || this.NoStore || this.MustRevalidate || this.ProxyRevalidate || this.MaxAge != -1 || this.SMaxAge != -1;
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000B15A0 File Offset: 0x000B05A0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Public)
			{
				stringBuilder.Append(" public");
			}
			if (this.Private)
			{
				stringBuilder.Append(" private");
				if (this.PrivateHeaders != null)
				{
					stringBuilder.Append('=');
					for (int i = 0; i < this.PrivateHeaders.Length - 1; i++)
					{
						stringBuilder.Append(this.PrivateHeaders[i]).Append(',');
					}
					stringBuilder.Append(this.PrivateHeaders[this.PrivateHeaders.Length - 1]);
				}
			}
			if (this.NoCache)
			{
				stringBuilder.Append(" no-cache");
				if (this.NoCacheHeaders != null)
				{
					stringBuilder.Append('=');
					for (int j = 0; j < this.NoCacheHeaders.Length - 1; j++)
					{
						stringBuilder.Append(this.NoCacheHeaders[j]).Append(',');
					}
					stringBuilder.Append(this.NoCacheHeaders[this.NoCacheHeaders.Length - 1]);
				}
			}
			if (this.NoStore)
			{
				stringBuilder.Append(" no-store");
			}
			if (this.MustRevalidate)
			{
				stringBuilder.Append(" must-revalidate");
			}
			if (this.ProxyRevalidate)
			{
				stringBuilder.Append(" proxy-revalidate");
			}
			if (this.MaxAge != -1)
			{
				stringBuilder.Append(" max-age=").Append(this.MaxAge);
			}
			if (this.SMaxAge != -1)
			{
				stringBuilder.Append(" s-maxage=").Append(this.SMaxAge);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040028D2 RID: 10450
		internal bool Public;

		// Token: 0x040028D3 RID: 10451
		internal bool Private;

		// Token: 0x040028D4 RID: 10452
		internal string[] PrivateHeaders;

		// Token: 0x040028D5 RID: 10453
		internal bool NoCache;

		// Token: 0x040028D6 RID: 10454
		internal string[] NoCacheHeaders;

		// Token: 0x040028D7 RID: 10455
		internal bool NoStore;

		// Token: 0x040028D8 RID: 10456
		internal bool MustRevalidate;

		// Token: 0x040028D9 RID: 10457
		internal bool ProxyRevalidate;

		// Token: 0x040028DA RID: 10458
		internal int MaxAge;

		// Token: 0x040028DB RID: 10459
		internal int SMaxAge;
	}
}

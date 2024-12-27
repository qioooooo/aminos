using System;

namespace System.Web.Caching
{
	// Token: 0x020000B2 RID: 178
	internal class CachedRawResponse
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x000265CD File Offset: 0x000255CD
		internal CachedRawResponse(HttpRawResponse rawResponse, HttpCachePolicySettings settings, string kernelCacheUrl)
		{
			this._rawResponse = rawResponse;
			this._settings = settings;
			this._kernelCacheUrl = kernelCacheUrl;
		}

		// Token: 0x040011D1 RID: 4561
		internal readonly HttpRawResponse _rawResponse;

		// Token: 0x040011D2 RID: 4562
		internal readonly HttpCachePolicySettings _settings;

		// Token: 0x040011D3 RID: 4563
		internal readonly string _kernelCacheUrl;
	}
}

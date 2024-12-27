using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x02000564 RID: 1380
	internal abstract class RequestCache
	{
		// Token: 0x06002A38 RID: 10808 RVA: 0x000B263F File Offset: 0x000B163F
		protected RequestCache(bool isPrivateCache, bool canWrite)
		{
			this._IsPrivateCache = isPrivateCache;
			this._CanWrite = canWrite;
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000B2655 File Offset: 0x000B1655
		internal bool IsPrivateCache
		{
			get
			{
				return this._IsPrivateCache;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x000B265D File Offset: 0x000B165D
		internal bool CanWrite
		{
			get
			{
				return this._CanWrite;
			}
		}

		// Token: 0x06002A3B RID: 10811
		internal abstract Stream Retrieve(string key, out RequestCacheEntry cacheEntry);

		// Token: 0x06002A3C RID: 10812
		internal abstract Stream Store(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06002A3D RID: 10813
		internal abstract void Remove(string key);

		// Token: 0x06002A3E RID: 10814
		internal abstract void Update(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06002A3F RID: 10815
		internal abstract bool TryRetrieve(string key, out RequestCacheEntry cacheEntry, out Stream readStream);

		// Token: 0x06002A40 RID: 10816
		internal abstract bool TryStore(string key, long contentLength, DateTime expiresUtc, DateTime lastModifiedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, out Stream writeStream);

		// Token: 0x06002A41 RID: 10817
		internal abstract bool TryRemove(string key);

		// Token: 0x06002A42 RID: 10818
		internal abstract bool TryUpdate(string key, DateTime expiresUtc, DateTime lastModifiedUtc, DateTime lastSynchronizedUtc, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata);

		// Token: 0x06002A43 RID: 10819
		internal abstract void UnlockEntry(Stream retrieveStream);

		// Token: 0x040028DE RID: 10462
		internal static readonly char[] LineSplits = new char[] { '\r', '\n' };

		// Token: 0x040028DF RID: 10463
		private bool _IsPrivateCache;

		// Token: 0x040028E0 RID: 10464
		private bool _CanWrite;
	}
}

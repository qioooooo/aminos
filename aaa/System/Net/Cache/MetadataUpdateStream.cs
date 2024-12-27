using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200057E RID: 1406
	internal class MetadataUpdateStream : Stream, ICloseEx
	{
		// Token: 0x06002AE9 RID: 10985 RVA: 0x000B6C6C File Offset: 0x000B5C6C
		internal MetadataUpdateStream(Stream parentStream, RequestCache cache, string key, DateTime expiresGMT, DateTime lastModifiedGMT, DateTime lastSynchronizedGMT, TimeSpan maxStale, StringCollection entryMetadata, StringCollection systemMetadata, bool isStrictCacheErrors)
		{
			if (parentStream == null)
			{
				throw new ArgumentNullException("parentStream");
			}
			this.m_ParentStream = parentStream;
			this.m_Cache = cache;
			this.m_Key = key;
			this.m_Expires = expiresGMT;
			this.m_LastModified = lastModifiedGMT;
			this.m_LastSynchronized = lastSynchronizedGMT;
			this.m_MaxStale = maxStale;
			this.m_EntryMetadata = entryMetadata;
			this.m_SystemMetadata = systemMetadata;
			this.m_IsStrictCacheErrors = isStrictCacheErrors;
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x000B6CDA File Offset: 0x000B5CDA
		private MetadataUpdateStream(Stream parentStream, RequestCache cache, string key, bool isStrictCacheErrors)
		{
			if (parentStream == null)
			{
				throw new ArgumentNullException("parentStream");
			}
			this.m_ParentStream = parentStream;
			this.m_Cache = cache;
			this.m_Key = key;
			this.m_CacheDestroy = true;
			this.m_IsStrictCacheErrors = isStrictCacheErrors;
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x000B6D14 File Offset: 0x000B5D14
		public override bool CanRead
		{
			get
			{
				return this.m_ParentStream.CanRead;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06002AEC RID: 10988 RVA: 0x000B6D21 File Offset: 0x000B5D21
		public override bool CanSeek
		{
			get
			{
				return this.m_ParentStream.CanSeek;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06002AED RID: 10989 RVA: 0x000B6D2E File Offset: 0x000B5D2E
		public override bool CanWrite
		{
			get
			{
				return this.m_ParentStream.CanWrite;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002AEE RID: 10990 RVA: 0x000B6D3B File Offset: 0x000B5D3B
		public override long Length
		{
			get
			{
				return this.m_ParentStream.Length;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x000B6D48 File Offset: 0x000B5D48
		// (set) Token: 0x06002AF0 RID: 10992 RVA: 0x000B6D55 File Offset: 0x000B5D55
		public override long Position
		{
			get
			{
				return this.m_ParentStream.Position;
			}
			set
			{
				this.m_ParentStream.Position = value;
			}
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x000B6D63 File Offset: 0x000B5D63
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.m_ParentStream.Seek(offset, origin);
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x000B6D72 File Offset: 0x000B5D72
		public override void SetLength(long value)
		{
			this.m_ParentStream.SetLength(value);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x000B6D80 File Offset: 0x000B5D80
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.m_ParentStream.Write(buffer, offset, count);
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x000B6D90 File Offset: 0x000B5D90
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.m_ParentStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000B6DA4 File Offset: 0x000B5DA4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_ParentStream.EndWrite(asyncResult);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000B6DB2 File Offset: 0x000B5DB2
		public override void Flush()
		{
			this.m_ParentStream.Flush();
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000B6DBF File Offset: 0x000B5DBF
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.m_ParentStream.Read(buffer, offset, count);
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000B6DCF File Offset: 0x000B5DCF
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.m_ParentStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000B6DE3 File Offset: 0x000B5DE3
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_ParentStream.EndRead(asyncResult);
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000B6DF1 File Offset: 0x000B5DF1
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000B6E01 File Offset: 0x000B5E01
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06002AFC RID: 11004 RVA: 0x000B6E0B File Offset: 0x000B5E0B
		public override bool CanTimeout
		{
			get
			{
				return this.m_ParentStream.CanTimeout;
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000B6E18 File Offset: 0x000B5E18
		// (set) Token: 0x06002AFE RID: 11006 RVA: 0x000B6E25 File Offset: 0x000B5E25
		public override int ReadTimeout
		{
			get
			{
				return this.m_ParentStream.ReadTimeout;
			}
			set
			{
				this.m_ParentStream.ReadTimeout = value;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000B6E33 File Offset: 0x000B5E33
		// (set) Token: 0x06002B00 RID: 11008 RVA: 0x000B6E40 File Offset: 0x000B5E40
		public override int WriteTimeout
		{
			get
			{
				return this.m_ParentStream.WriteTimeout;
			}
			set
			{
				this.m_ParentStream.WriteTimeout = value;
			}
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000B6E50 File Offset: 0x000B5E50
		protected virtual void Dispose(bool disposing, CloseExState closeState)
		{
			if (Interlocked.Increment(ref this._Disposed) == 1)
			{
				ICloseEx closeEx = this.m_ParentStream as ICloseEx;
				if (closeEx != null)
				{
					closeEx.CloseEx(closeState);
				}
				else
				{
					this.m_ParentStream.Close();
				}
				if (this.m_CacheDestroy)
				{
					if (this.m_IsStrictCacheErrors)
					{
						this.m_Cache.Remove(this.m_Key);
					}
					else
					{
						this.m_Cache.TryRemove(this.m_Key);
					}
				}
				else if (this.m_IsStrictCacheErrors)
				{
					this.m_Cache.Update(this.m_Key, this.m_Expires, this.m_LastModified, this.m_LastSynchronized, this.m_MaxStale, this.m_EntryMetadata, this.m_SystemMetadata);
				}
				else
				{
					this.m_Cache.TryUpdate(this.m_Key, this.m_Expires, this.m_LastModified, this.m_LastSynchronized, this.m_MaxStale, this.m_EntryMetadata, this.m_SystemMetadata);
				}
				if (!disposing)
				{
					this.m_Cache = null;
					this.m_Key = null;
					this.m_EntryMetadata = null;
					this.m_SystemMetadata = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x04002993 RID: 10643
		private Stream m_ParentStream;

		// Token: 0x04002994 RID: 10644
		private RequestCache m_Cache;

		// Token: 0x04002995 RID: 10645
		private string m_Key;

		// Token: 0x04002996 RID: 10646
		private DateTime m_Expires;

		// Token: 0x04002997 RID: 10647
		private DateTime m_LastModified;

		// Token: 0x04002998 RID: 10648
		private DateTime m_LastSynchronized;

		// Token: 0x04002999 RID: 10649
		private TimeSpan m_MaxStale;

		// Token: 0x0400299A RID: 10650
		private StringCollection m_EntryMetadata;

		// Token: 0x0400299B RID: 10651
		private StringCollection m_SystemMetadata;

		// Token: 0x0400299C RID: 10652
		private bool m_CacheDestroy;

		// Token: 0x0400299D RID: 10653
		private bool m_IsStrictCacheErrors;

		// Token: 0x0400299E RID: 10654
		private int _Disposed;
	}
}

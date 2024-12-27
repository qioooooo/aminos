using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200057F RID: 1407
	internal class RangeStream : Stream, ICloseEx
	{
		// Token: 0x06002B02 RID: 11010 RVA: 0x000B6F68 File Offset: 0x000B5F68
		internal RangeStream(Stream parentStream, long offset, long size)
		{
			this.m_ParentStream = parentStream;
			this.m_Offset = offset;
			this.m_Size = size;
			if (this.m_ParentStream.CanSeek)
			{
				this.m_ParentStream.Position = offset;
				this.m_Position = offset;
				return;
			}
			throw new NotSupportedException(SR.GetString("net_cache_non_seekable_stream_not_supported"));
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x000B6FC0 File Offset: 0x000B5FC0
		public override bool CanRead
		{
			get
			{
				return this.m_ParentStream.CanRead;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x000B6FCD File Offset: 0x000B5FCD
		public override bool CanSeek
		{
			get
			{
				return this.m_ParentStream.CanSeek;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x000B6FDA File Offset: 0x000B5FDA
		public override bool CanWrite
		{
			get
			{
				return this.m_ParentStream.CanWrite;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x000B6FE7 File Offset: 0x000B5FE7
		public override long Length
		{
			get
			{
				long length = this.m_ParentStream.Length;
				return this.m_Size;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x000B6FFB File Offset: 0x000B5FFB
		// (set) Token: 0x06002B08 RID: 11016 RVA: 0x000B700F File Offset: 0x000B600F
		public override long Position
		{
			get
			{
				return this.m_ParentStream.Position - this.m_Offset;
			}
			set
			{
				value += this.m_Offset;
				if (value > this.m_Offset + this.m_Size)
				{
					value = this.m_Offset + this.m_Size;
				}
				this.m_ParentStream.Position = value;
			}
		}

		// Token: 0x06002B09 RID: 11017 RVA: 0x000B7048 File Offset: 0x000B6048
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				offset += this.m_Offset;
				if (offset > this.m_Offset + this.m_Size)
				{
					offset = this.m_Offset + this.m_Size;
				}
				if (offset < this.m_Offset)
				{
					offset = this.m_Offset;
					goto IL_00D0;
				}
				goto IL_00D0;
			case SeekOrigin.End:
				offset -= this.m_Offset + this.m_Size;
				if (offset > 0L)
				{
					offset = 0L;
				}
				if (offset < -this.m_Size)
				{
					offset = -this.m_Size;
					goto IL_00D0;
				}
				goto IL_00D0;
			}
			if (this.m_Position + offset > this.m_Offset + this.m_Size)
			{
				offset = this.m_Offset + this.m_Size - this.m_Position;
			}
			if (this.m_Position + offset < this.m_Offset)
			{
				offset = this.m_Offset - this.m_Position;
			}
			IL_00D0:
			this.m_Position = this.m_ParentStream.Seek(offset, origin);
			return this.m_Position - this.m_Offset;
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000B7145 File Offset: 0x000B6145
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000B7158 File Offset: 0x000B6158
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
			}
			this.m_ParentStream.Write(buffer, offset, count);
			this.m_Position += (long)count;
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000B71AA File Offset: 0x000B61AA
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_Position + (long)offset > this.m_Offset + this.m_Size)
			{
				throw new NotSupportedException(SR.GetString("net_cache_unsupported_partial_stream"));
			}
			return this.m_ParentStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000B71E6 File Offset: 0x000B61E6
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_ParentStream.EndWrite(asyncResult);
			this.m_Position = this.m_ParentStream.Position;
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000B7205 File Offset: 0x000B6205
		public override void Flush()
		{
			this.m_ParentStream.Flush();
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000B7214 File Offset: 0x000B6214
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.m_Position >= this.m_Offset + this.m_Size)
			{
				return 0;
			}
			if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				count = (int)(this.m_Offset + this.m_Size - this.m_Position);
			}
			int num = this.m_ParentStream.Read(buffer, offset, count);
			this.m_Position += (long)num;
			return num;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000B7288 File Offset: 0x000B6288
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.m_Position >= this.m_Offset + this.m_Size)
			{
				count = 0;
			}
			else if (this.m_Position + (long)count > this.m_Offset + this.m_Size)
			{
				count = (int)(this.m_Offset + this.m_Size - this.m_Position);
			}
			return this.m_ParentStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000B72F0 File Offset: 0x000B62F0
		public override int EndRead(IAsyncResult asyncResult)
		{
			int num = this.m_ParentStream.EndRead(asyncResult);
			this.m_Position += (long)num;
			return num;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000B731A File Offset: 0x000B631A
		protected sealed override void Dispose(bool disposing)
		{
			this.Dispose(disposing, CloseExState.Normal);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000B732A File Offset: 0x000B632A
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			this.Dispose(true, closeState);
			GC.SuppressFinalize(this);
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002B14 RID: 11028 RVA: 0x000B733A File Offset: 0x000B633A
		public override bool CanTimeout
		{
			get
			{
				return this.m_ParentStream.CanTimeout;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002B15 RID: 11029 RVA: 0x000B7347 File Offset: 0x000B6347
		// (set) Token: 0x06002B16 RID: 11030 RVA: 0x000B7354 File Offset: 0x000B6354
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

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002B17 RID: 11031 RVA: 0x000B7362 File Offset: 0x000B6362
		// (set) Token: 0x06002B18 RID: 11032 RVA: 0x000B736F File Offset: 0x000B636F
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

		// Token: 0x06002B19 RID: 11033 RVA: 0x000B7380 File Offset: 0x000B6380
		protected virtual void Dispose(bool disposing, CloseExState closeState)
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
			base.Dispose(disposing);
		}

		// Token: 0x0400299F RID: 10655
		private Stream m_ParentStream;

		// Token: 0x040029A0 RID: 10656
		private long m_Offset;

		// Token: 0x040029A1 RID: 10657
		private long m_Size;

		// Token: 0x040029A2 RID: 10658
		private long m_Position;
	}
}

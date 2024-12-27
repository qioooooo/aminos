using System;
using System.IO;
using System.Net.Sockets;

namespace System.Net
{
	// Token: 0x0200067B RID: 1659
	internal class DelegatedStream : Stream
	{
		// Token: 0x06003348 RID: 13128 RVA: 0x000D8909 File Offset: 0x000D7909
		protected DelegatedStream()
		{
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x000D8911 File Offset: 0x000D7911
		protected DelegatedStream(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.stream = stream;
			this.netStream = stream as NetworkStream;
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x000D893A File Offset: 0x000D793A
		protected Stream BaseStream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600334B RID: 13131 RVA: 0x000D8942 File Offset: 0x000D7942
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x000D894F File Offset: 0x000D794F
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600334D RID: 13133 RVA: 0x000D895C File Offset: 0x000D795C
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x000D8969 File Offset: 0x000D7969
		public override long Length
		{
			get
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				return this.stream.Length;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600334F RID: 13135 RVA: 0x000D898E File Offset: 0x000D798E
		// (set) Token: 0x06003350 RID: 13136 RVA: 0x000D89B3 File Offset: 0x000D79B3
		public override long Position
		{
			get
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				return this.stream.Position;
			}
			set
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException(SR.GetString("SeekNotSupported"));
				}
				this.stream.Position = value;
			}
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000D89DC File Offset: 0x000D79DC
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			IAsyncResult asyncResult;
			if (this.netStream != null)
			{
				asyncResult = this.netStream.UnsafeBeginRead(buffer, offset, count, callback, state);
			}
			else
			{
				asyncResult = this.stream.BeginRead(buffer, offset, count, callback, state);
			}
			return asyncResult;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x000D8A34 File Offset: 0x000D7A34
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			IAsyncResult asyncResult;
			if (this.netStream != null)
			{
				asyncResult = this.netStream.UnsafeBeginWrite(buffer, offset, count, callback, state);
			}
			else
			{
				asyncResult = this.stream.BeginWrite(buffer, offset, count, callback, state);
			}
			return asyncResult;
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x000D8A8C File Offset: 0x000D7A8C
		public override void Close()
		{
			this.stream.Close();
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x000D8A9C File Offset: 0x000D7A9C
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.EndRead(asyncResult);
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x000D8ACF File Offset: 0x000D7ACF
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			this.stream.EndWrite(asyncResult);
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x000D8AF5 File Offset: 0x000D7AF5
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x000D8B04 File Offset: 0x000D7B04
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException(SR.GetString("ReadNotSupported"));
			}
			return this.stream.Read(buffer, offset, count);
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x000D8B3C File Offset: 0x000D7B3C
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.GetString("SeekNotSupported"));
			}
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000D8B70 File Offset: 0x000D7B70
		public override void SetLength(long value)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException(SR.GetString("SeekNotSupported"));
			}
			this.stream.SetLength(value);
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000D8B96 File Offset: 0x000D7B96
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this.CanWrite)
			{
				throw new NotSupportedException(SR.GetString("WriteNotSupported"));
			}
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x04002F84 RID: 12164
		private Stream stream;

		// Token: 0x04002F85 RID: 12165
		private NetworkStream netStream;
	}
}

using System;
using System.IO;

namespace System.Security.Cryptography
{
	// Token: 0x02000878 RID: 2168
	internal sealed class TailStream : Stream
	{
		// Token: 0x06004F62 RID: 20322 RVA: 0x0011517F File Offset: 0x0011417F
		public TailStream(int bufferSize)
		{
			this._Buffer = new byte[bufferSize];
			this._BufferSize = bufferSize;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x0011519A File Offset: 0x0011419A
		public void Clear()
		{
			this.Close();
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x001151A4 File Offset: 0x001141A4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._Buffer != null)
					{
						Array.Clear(this._Buffer, 0, this._Buffer.Length);
					}
					this._Buffer = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06004F65 RID: 20325 RVA: 0x001151F4 File Offset: 0x001141F4
		public byte[] Buffer
		{
			get
			{
				return (byte[])this._Buffer.Clone();
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06004F66 RID: 20326 RVA: 0x00115206 File Offset: 0x00114206
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06004F67 RID: 20327 RVA: 0x00115209 File Offset: 0x00114209
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x0011520C File Offset: 0x0011420C
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06004F69 RID: 20329 RVA: 0x0011520F File Offset: 0x0011420F
		public override long Length
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06004F6A RID: 20330 RVA: 0x00115220 File Offset: 0x00114220
		// (set) Token: 0x06004F6B RID: 20331 RVA: 0x00115231 File Offset: 0x00114231
		public override long Position
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
			}
			set
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
			}
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x00115242 File Offset: 0x00114242
		public override void Flush()
		{
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00115244 File Offset: 0x00114244
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x00115255 File Offset: 0x00114255
		public override void SetLength(long value)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnseekableStream"));
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x00115266 File Offset: 0x00114266
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnreadableStream"));
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x00115278 File Offset: 0x00114278
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (this._BufferFull)
			{
				if (count > this._BufferSize)
				{
					global::System.Buffer.InternalBlockCopy(buffer, offset + count - this._BufferSize, this._Buffer, 0, this._BufferSize);
					return;
				}
				global::System.Buffer.InternalBlockCopy(this._Buffer, this._BufferSize - count, this._Buffer, 0, this._BufferSize - count);
				global::System.Buffer.InternalBlockCopy(buffer, offset, this._Buffer, this._BufferSize - count, count);
				return;
			}
			else
			{
				if (count > this._BufferSize)
				{
					global::System.Buffer.InternalBlockCopy(buffer, offset + count - this._BufferSize, this._Buffer, 0, this._BufferSize);
					this._BufferFull = true;
					return;
				}
				if (count + this._BufferIndex >= this._BufferSize)
				{
					global::System.Buffer.InternalBlockCopy(this._Buffer, this._BufferIndex + count - this._BufferSize, this._Buffer, 0, this._BufferSize - count);
					global::System.Buffer.InternalBlockCopy(buffer, offset, this._Buffer, this._BufferIndex, count);
					this._BufferFull = true;
					return;
				}
				global::System.Buffer.InternalBlockCopy(buffer, offset, this._Buffer, this._BufferIndex, count);
				this._BufferIndex += count;
				return;
			}
		}

		// Token: 0x040028C3 RID: 10435
		private byte[] _Buffer;

		// Token: 0x040028C4 RID: 10436
		private int _BufferSize;

		// Token: 0x040028C5 RID: 10437
		private int _BufferIndex;

		// Token: 0x040028C6 RID: 10438
		private bool _BufferFull;
	}
}

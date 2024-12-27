using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x02000592 RID: 1426
	[ComVisible(true)]
	public sealed class BufferedStream : Stream
	{
		// Token: 0x060034CE RID: 13518 RVA: 0x000AF9F3 File Offset: 0x000AE9F3
		private BufferedStream()
		{
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x000AF9FB File Offset: 0x000AE9FB
		public BufferedStream(Stream stream)
			: this(stream, 4096)
		{
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x000AFA0C File Offset: 0x000AEA0C
		public BufferedStream(Stream stream, int bufferSize)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_MustBePositive"), new object[] { "bufferSize" }));
			}
			this._s = stream;
			this._bufferSize = bufferSize;
			if (!this._s.CanRead && !this._s.CanWrite)
			{
				__Error.StreamIsClosed();
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x060034D1 RID: 13521 RVA: 0x000AFA8D File Offset: 0x000AEA8D
		public override bool CanRead
		{
			get
			{
				return this._s != null && this._s.CanRead;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x060034D2 RID: 13522 RVA: 0x000AFAA4 File Offset: 0x000AEAA4
		public override bool CanWrite
		{
			get
			{
				return this._s != null && this._s.CanWrite;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x060034D3 RID: 13523 RVA: 0x000AFABB File Offset: 0x000AEABB
		public override bool CanSeek
		{
			get
			{
				return this._s != null && this._s.CanSeek;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x060034D4 RID: 13524 RVA: 0x000AFAD2 File Offset: 0x000AEAD2
		public override long Length
		{
			get
			{
				if (this._s == null)
				{
					__Error.StreamIsClosed();
				}
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				return this._s.Length;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x060034D5 RID: 13525 RVA: 0x000AFAFC File Offset: 0x000AEAFC
		// (set) Token: 0x060034D6 RID: 13526 RVA: 0x000AFB4C File Offset: 0x000AEB4C
		public override long Position
		{
			get
			{
				if (this._s == null)
				{
					__Error.StreamIsClosed();
				}
				if (!this._s.CanSeek)
				{
					__Error.SeekNotSupported();
				}
				return this._s.Position + (long)(this._readPos - this._readLen + this._writePos);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (this._s == null)
				{
					__Error.StreamIsClosed();
				}
				if (!this._s.CanSeek)
				{
					__Error.SeekNotSupported();
				}
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				this._readPos = 0;
				this._readLen = 0;
				this._s.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x000AFBC0 File Offset: 0x000AEBC0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._s != null)
				{
					try
					{
						this.Flush();
					}
					finally
					{
						this._s.Close();
					}
				}
			}
			finally
			{
				this._s = null;
				this._buffer = null;
				base.Dispose(disposing);
			}
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x000AFC20 File Offset: 0x000AEC20
		public override void Flush()
		{
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			else if (this._readPos < this._readLen && this._s.CanSeek)
			{
				this.FlushRead();
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x000AFC7A File Offset: 0x000AEC7A
		private void FlushRead()
		{
			if (this._readPos - this._readLen != 0)
			{
				this._s.Seek((long)(this._readPos - this._readLen), SeekOrigin.Current);
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x000AFCB4 File Offset: 0x000AECB4
		private void FlushWrite()
		{
			this._s.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			this._s.Flush();
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x000AFCE0 File Offset: 0x000AECE0
		public override int Read([In] [Out] byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			int num = this._readLen - this._readPos;
			if (num == 0)
			{
				if (!this._s.CanRead)
				{
					__Error.ReadNotSupported();
				}
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				if (count >= this._bufferSize)
				{
					num = this._s.Read(array, offset, count);
					this._readPos = 0;
					this._readLen = 0;
					return num;
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
				num = this._s.Read(this._buffer, 0, this._bufferSize);
				if (num == 0)
				{
					return 0;
				}
				this._readPos = 0;
				this._readLen = num;
			}
			if (num > count)
			{
				num = count;
			}
			Buffer.InternalBlockCopy(this._buffer, this._readPos, array, offset, num);
			this._readPos += num;
			if (num < count)
			{
				int num2 = this._s.Read(array, offset + num, count - num);
				num += num2;
				this._readPos = 0;
				this._readLen = 0;
			}
			return num;
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x000AFE50 File Offset: 0x000AEE50
		public override int ReadByte()
		{
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			if (this._readLen == 0 && !this._s.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (this._readPos == this._readLen)
			{
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
				this._readLen = this._s.Read(this._buffer, 0, this._bufferSize);
				this._readPos = 0;
			}
			if (this._readPos == this._readLen)
			{
				return -1;
			}
			return (int)this._buffer[this._readPos++];
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x000AFF08 File Offset: 0x000AEF08
		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			if (this._writePos == 0)
			{
				if (!this._s.CanWrite)
				{
					__Error.WriteNotSupported();
				}
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				else
				{
					this._readPos = 0;
					this._readLen = 0;
				}
			}
			if (this._writePos > 0)
			{
				int num = this._bufferSize - this._writePos;
				if (num > 0)
				{
					if (num > count)
					{
						num = count;
					}
					Buffer.InternalBlockCopy(array, offset, this._buffer, this._writePos, num);
					this._writePos += num;
					if (count == num)
					{
						return;
					}
					offset += num;
					count -= num;
				}
				this._s.Write(this._buffer, 0, this._writePos);
				this._writePos = 0;
			}
			if (count >= this._bufferSize)
			{
				this._s.Write(array, offset, count);
				return;
			}
			if (count == 0)
			{
				return;
			}
			if (this._buffer == null)
			{
				this._buffer = new byte[this._bufferSize];
			}
			Buffer.InternalBlockCopy(array, offset, this._buffer, 0, count);
			this._writePos = count;
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x000B0080 File Offset: 0x000AF080
		public override void WriteByte(byte value)
		{
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			if (this._writePos == 0)
			{
				if (!this._s.CanWrite)
				{
					__Error.WriteNotSupported();
				}
				if (this._readPos < this._readLen)
				{
					this.FlushRead();
				}
				else
				{
					this._readPos = 0;
					this._readLen = 0;
				}
				if (this._buffer == null)
				{
					this._buffer = new byte[this._bufferSize];
				}
			}
			if (this._writePos == this._bufferSize)
			{
				this.FlushWrite();
			}
			this._buffer[this._writePos++] = value;
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x000B0120 File Offset: 0x000AF120
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			if (!this._s.CanSeek)
			{
				__Error.SeekNotSupported();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			else if (origin == SeekOrigin.Current)
			{
				offset -= (long)(this._readLen - this._readPos);
			}
			long num = this._s.Position + (long)(this._readPos - this._readLen);
			long num2 = this._s.Seek(offset, origin);
			if (this._readLen > 0)
			{
				if (num == num2)
				{
					if (this._readPos > 0)
					{
						Buffer.InternalBlockCopy(this._buffer, this._readPos, this._buffer, 0, this._readLen - this._readPos);
						this._readLen -= this._readPos;
						this._readPos = 0;
					}
					if (this._readLen > 0)
					{
						this._s.Seek((long)this._readLen, SeekOrigin.Current);
					}
				}
				else if (num - (long)this._readPos < num2 && num2 < num + (long)this._readLen - (long)this._readPos)
				{
					int num3 = (int)(num2 - num);
					Buffer.InternalBlockCopy(this._buffer, this._readPos + num3, this._buffer, 0, this._readLen - (this._readPos + num3));
					this._readLen -= this._readPos + num3;
					this._readPos = 0;
					if (this._readLen > 0)
					{
						this._s.Seek((long)this._readLen, SeekOrigin.Current);
					}
				}
				else
				{
					this._readPos = 0;
					this._readLen = 0;
				}
			}
			return num2;
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x000B02B4 File Offset: 0x000AF2B4
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NegFileSize"));
			}
			if (this._s == null)
			{
				__Error.StreamIsClosed();
			}
			if (!this._s.CanSeek)
			{
				__Error.SeekNotSupported();
			}
			if (!this._s.CanWrite)
			{
				__Error.WriteNotSupported();
			}
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			else if (this._readPos < this._readLen)
			{
				this.FlushRead();
			}
			this._readPos = 0;
			this._readLen = 0;
			this._s.SetLength(value);
		}

		// Token: 0x04001BB9 RID: 7097
		private const int _DefaultBufferSize = 4096;

		// Token: 0x04001BBA RID: 7098
		private Stream _s;

		// Token: 0x04001BBB RID: 7099
		private byte[] _buffer;

		// Token: 0x04001BBC RID: 7100
		private int _readPos;

		// Token: 0x04001BBD RID: 7101
		private int _readLen;

		// Token: 0x04001BBE RID: 7102
		private int _writePos;

		// Token: 0x04001BBF RID: 7103
		private int _bufferSize;
	}
}

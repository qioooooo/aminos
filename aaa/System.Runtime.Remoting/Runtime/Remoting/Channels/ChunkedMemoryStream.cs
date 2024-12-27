using System;
using System.IO;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200000B RID: 11
	internal class ChunkedMemoryStream : Stream
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002748 File Offset: 0x00001748
		public ChunkedMemoryStream(IByteBufferPool bufferPool)
		{
			this._bufferPool = bufferPool;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002757 File Offset: 0x00001757
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000275A File Offset: 0x0000175A
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000275D File Offset: 0x0000175D
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002760 File Offset: 0x00001760
		public override long Length
		{
			get
			{
				if (this._bClosed)
				{
					throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
				}
				int num = 0;
				ChunkedMemoryStream.MemoryChunk next;
				for (ChunkedMemoryStream.MemoryChunk memoryChunk = this._chunks; memoryChunk != null; memoryChunk = next)
				{
					next = memoryChunk.Next;
					if (next != null)
					{
						num += memoryChunk.Buffer.Length;
					}
					else
					{
						num += this._writeOffset;
					}
				}
				return (long)num;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000027B8 File Offset: 0x000017B8
		// (set) Token: 0x0600002A RID: 42 RVA: 0x0000281C File Offset: 0x0000181C
		public override long Position
		{
			get
			{
				if (this._bClosed)
				{
					throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
				}
				if (this._readChunk == null)
				{
					return 0L;
				}
				int num = 0;
				for (ChunkedMemoryStream.MemoryChunk memoryChunk = this._chunks; memoryChunk != this._readChunk; memoryChunk = memoryChunk.Next)
				{
					num += memoryChunk.Buffer.Length;
				}
				num += this._readOffset;
				return (long)num;
			}
			set
			{
				if (this._bClosed)
				{
					throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				ChunkedMemoryStream.MemoryChunk readChunk = this._readChunk;
				int readOffset = this._readOffset;
				this._readChunk = null;
				this._readOffset = 0;
				int num = (int)value;
				for (ChunkedMemoryStream.MemoryChunk memoryChunk = this._chunks; memoryChunk != null; memoryChunk = memoryChunk.Next)
				{
					if (num < memoryChunk.Buffer.Length || (num == memoryChunk.Buffer.Length && memoryChunk.Next == null))
					{
						this._readChunk = memoryChunk;
						this._readOffset = num;
						break;
					}
					num -= memoryChunk.Buffer.Length;
				}
				if (this._readChunk == null)
				{
					this._readChunk = readChunk;
					this._readOffset = readOffset;
					throw new ArgumentOutOfRangeException("value");
				}
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000028E0 File Offset: 0x000018E0
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this._bClosed)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
			}
			switch (origin)
			{
			case SeekOrigin.Begin:
				this.Position = offset;
				break;
			case SeekOrigin.Current:
				this.Position += offset;
				break;
			case SeekOrigin.End:
				this.Position = this.Length + offset;
				break;
			}
			return this.Position;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002948 File Offset: 0x00001948
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002950 File Offset: 0x00001950
		protected override void Dispose(bool disposing)
		{
			try
			{
				this._bClosed = true;
				if (disposing)
				{
					this.ReleaseMemoryChunks(this._chunks);
				}
				this._chunks = null;
				this._writeChunk = null;
				this._readChunk = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000029A4 File Offset: 0x000019A4
		public override void Flush()
		{
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000029A8 File Offset: 0x000019A8
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._bClosed)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
			}
			if (this._readChunk == null)
			{
				if (this._chunks == null)
				{
					return 0;
				}
				this._readChunk = this._chunks;
				this._readOffset = 0;
			}
			byte[] array = this._readChunk.Buffer;
			int num = array.Length;
			if (this._readChunk.Next == null)
			{
				num = this._writeOffset;
			}
			int num2 = 0;
			while (count > 0)
			{
				if (this._readOffset == num)
				{
					if (this._readChunk.Next == null)
					{
						break;
					}
					this._readChunk = this._readChunk.Next;
					this._readOffset = 0;
					array = this._readChunk.Buffer;
					num = array.Length;
					if (this._readChunk.Next == null)
					{
						num = this._writeOffset;
					}
				}
				int num3 = Math.Min(count, num - this._readOffset);
				Buffer.BlockCopy(array, this._readOffset, buffer, offset, num3);
				offset += num3;
				count -= num3;
				this._readOffset += num3;
				num2 += num3;
			}
			return num2;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002AB4 File Offset: 0x00001AB4
		public override int ReadByte()
		{
			if (this._bClosed)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
			}
			if (this._readChunk == null)
			{
				if (this._chunks == null)
				{
					return 0;
				}
				this._readChunk = this._chunks;
				this._readOffset = 0;
			}
			byte[] array = this._readChunk.Buffer;
			int num = array.Length;
			if (this._readChunk.Next == null)
			{
				num = this._writeOffset;
			}
			if (this._readOffset == num)
			{
				if (this._readChunk.Next == null)
				{
					return -1;
				}
				this._readChunk = this._readChunk.Next;
				this._readOffset = 0;
				array = this._readChunk.Buffer;
				num = array.Length;
				if (this._readChunk.Next == null)
				{
					num = this._writeOffset;
				}
			}
			return (int)array[this._readOffset++];
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002B8C File Offset: 0x00001B8C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._bClosed)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
			}
			if (this._chunks == null)
			{
				this._chunks = this.AllocateMemoryChunk();
				this._writeChunk = this._chunks;
				this._writeOffset = 0;
			}
			byte[] array = this._writeChunk.Buffer;
			int num = array.Length;
			while (count > 0)
			{
				if (this._writeOffset == num)
				{
					this._writeChunk.Next = this.AllocateMemoryChunk();
					this._writeChunk = this._writeChunk.Next;
					this._writeOffset = 0;
					array = this._writeChunk.Buffer;
					num = array.Length;
				}
				int num2 = Math.Min(count, num - this._writeOffset);
				Buffer.BlockCopy(buffer, offset, array, this._writeOffset, num2);
				offset += num2;
				count -= num2;
				this._writeOffset += num2;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002C68 File Offset: 0x00001C68
		public override void WriteByte(byte value)
		{
			if (this._bClosed)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
			}
			if (this._chunks == null)
			{
				this._chunks = this.AllocateMemoryChunk();
				this._writeChunk = this._chunks;
				this._writeOffset = 0;
			}
			byte[] array = this._writeChunk.Buffer;
			int num = array.Length;
			if (this._writeOffset == num)
			{
				this._writeChunk.Next = this.AllocateMemoryChunk();
				this._writeChunk = this._writeChunk.Next;
				this._writeOffset = 0;
				array = this._writeChunk.Buffer;
				num = array.Length;
			}
			array[this._writeOffset++] = value;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002D1C File Offset: 0x00001D1C
		public virtual byte[] ToArray()
		{
			int num = (int)this.Length;
			byte[] array = new byte[this.Length];
			ChunkedMemoryStream.MemoryChunk readChunk = this._readChunk;
			int readOffset = this._readOffset;
			this._readChunk = this._chunks;
			this._readOffset = 0;
			this.Read(array, 0, num);
			this._readChunk = readChunk;
			this._readOffset = readOffset;
			return array;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002D78 File Offset: 0x00001D78
		public virtual void WriteTo(Stream stream)
		{
			if (this._bClosed)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Stream_StreamIsClosed"));
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this._readChunk == null)
			{
				if (this._chunks == null)
				{
					return;
				}
				this._readChunk = this._chunks;
				this._readOffset = 0;
			}
			byte[] array = this._readChunk.Buffer;
			int num = array.Length;
			if (this._readChunk.Next == null)
			{
				num = this._writeOffset;
			}
			for (;;)
			{
				if (this._readOffset == num)
				{
					if (this._readChunk.Next == null)
					{
						break;
					}
					this._readChunk = this._readChunk.Next;
					this._readOffset = 0;
					array = this._readChunk.Buffer;
					num = array.Length;
					if (this._readChunk.Next == null)
					{
						num = this._writeOffset;
					}
				}
				int num2 = num - this._readOffset;
				stream.Write(array, this._readOffset, num2);
				this._readOffset = num;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002E68 File Offset: 0x00001E68
		private ChunkedMemoryStream.MemoryChunk AllocateMemoryChunk()
		{
			return new ChunkedMemoryStream.MemoryChunk
			{
				Buffer = this._bufferPool.GetBuffer(),
				Next = null
			};
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E94 File Offset: 0x00001E94
		private void ReleaseMemoryChunks(ChunkedMemoryStream.MemoryChunk chunk)
		{
			if (this._bufferPool is ByteBufferAllocator)
			{
				return;
			}
			while (chunk != null)
			{
				this._bufferPool.ReturnBuffer(chunk.Buffer);
				chunk = chunk.Next;
			}
		}

		// Token: 0x04000047 RID: 71
		private static IByteBufferPool s_defaultBufferPool = new ByteBufferAllocator(1024);

		// Token: 0x04000048 RID: 72
		private ChunkedMemoryStream.MemoryChunk _chunks;

		// Token: 0x04000049 RID: 73
		private IByteBufferPool _bufferPool;

		// Token: 0x0400004A RID: 74
		private bool _bClosed;

		// Token: 0x0400004B RID: 75
		private ChunkedMemoryStream.MemoryChunk _writeChunk;

		// Token: 0x0400004C RID: 76
		private int _writeOffset;

		// Token: 0x0400004D RID: 77
		private ChunkedMemoryStream.MemoryChunk _readChunk;

		// Token: 0x0400004E RID: 78
		private int _readOffset;

		// Token: 0x0200000C RID: 12
		private class MemoryChunk
		{
			// Token: 0x0400004F RID: 79
			public byte[] Buffer;

			// Token: 0x04000050 RID: 80
			public ChunkedMemoryStream.MemoryChunk Next;
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005A9 RID: 1449
	[ComVisible(true)]
	[Serializable]
	public class MemoryStream : Stream
	{
		// Token: 0x0600361A RID: 13850 RVA: 0x000B67DE File Offset: 0x000B57DE
		public MemoryStream()
			: this(0)
		{
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x000B67E8 File Offset: 0x000B57E8
		public MemoryStream(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NegativeCapacity"));
			}
			this._buffer = new byte[capacity];
			this._capacity = capacity;
			this._expandable = true;
			this._writable = true;
			this._exposable = true;
			this._origin = 0;
			this._isOpen = true;
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000B684A File Offset: 0x000B584A
		public MemoryStream(byte[] buffer)
			: this(buffer, true)
		{
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000B6854 File Offset: 0x000B5854
		public MemoryStream(byte[] buffer, bool writable)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			this._buffer = buffer;
			this._length = (this._capacity = buffer.Length);
			this._writable = writable;
			this._exposable = false;
			this._origin = 0;
			this._isOpen = true;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000B68B4 File Offset: 0x000B58B4
		public MemoryStream(byte[] buffer, int index, int count)
			: this(buffer, index, count, true, false)
		{
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000B68C1 File Offset: 0x000B58C1
		public MemoryStream(byte[] buffer, int index, int count, bool writable)
			: this(buffer, index, count, writable, false)
		{
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000B68D0 File Offset: 0x000B58D0
		public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			this._buffer = buffer;
			this._position = index;
			this._origin = index;
			this._length = (this._capacity = index + count);
			this._writable = writable;
			this._exposable = publiclyVisible;
			this._expandable = false;
			this._isOpen = true;
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06003621 RID: 13857 RVA: 0x000B698C File Offset: 0x000B598C
		public override bool CanRead
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x000B6994 File Offset: 0x000B5994
		public override bool CanSeek
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06003623 RID: 13859 RVA: 0x000B699C File Offset: 0x000B599C
		public override bool CanWrite
		{
			get
			{
				return this._writable;
			}
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000B69A4 File Offset: 0x000B59A4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._isOpen = false;
					this._writable = false;
					this._expandable = false;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000B69E4 File Offset: 0x000B59E4
		private bool EnsureCapacity(int value)
		{
			if (value < 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
			}
			if (value > this._capacity)
			{
				int num = value;
				if (num < 256)
				{
					num = 256;
				}
				if (num < this._capacity * 2)
				{
					num = this._capacity * 2;
				}
				this.Capacity = num;
				return true;
			}
			return false;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000B6A3C File Offset: 0x000B5A3C
		public override void Flush()
		{
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000B6A3E File Offset: 0x000B5A3E
		public virtual byte[] GetBuffer()
		{
			if (!this._exposable)
			{
				throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_MemStreamBuffer"));
			}
			return this._buffer;
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000B6A5E File Offset: 0x000B5A5E
		internal byte[] InternalGetBuffer()
		{
			return this._buffer;
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000B6A66 File Offset: 0x000B5A66
		internal void InternalGetOriginAndLength(out int origin, out int length)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			origin = this._origin;
			length = this._length;
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000B6A85 File Offset: 0x000B5A85
		internal int InternalGetPosition()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			return this._position;
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000B6A9C File Offset: 0x000B5A9C
		internal int InternalReadInt32()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			int num = (this._position += 4);
			if (num > this._length)
			{
				this._position = this._length;
				__Error.EndOfFile();
			}
			return (int)this._buffer[num - 4] | ((int)this._buffer[num - 3] << 8) | ((int)this._buffer[num - 2] << 16) | ((int)this._buffer[num - 1] << 24);
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000B6B18 File Offset: 0x000B5B18
		internal int InternalEmulateRead(int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			int num = this._length - this._position;
			if (num > count)
			{
				num = count;
			}
			if (num < 0)
			{
				num = 0;
			}
			this._position += num;
			return num;
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x000B6B5B File Offset: 0x000B5B5B
		// (set) Token: 0x0600362E RID: 13870 RVA: 0x000B6B78 File Offset: 0x000B5B78
		public virtual int Capacity
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._capacity - this._origin;
			}
			set
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				if (value != this._capacity)
				{
					if (!this._expandable)
					{
						__Error.MemoryStreamNotExpandable();
					}
					if (value < this._length)
					{
						throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
					}
					if (value > 0)
					{
						byte[] array = new byte[value];
						if (this._length > 0)
						{
							Buffer.InternalBlockCopy(this._buffer, 0, array, 0, this._length);
						}
						this._buffer = array;
					}
					else
					{
						this._buffer = null;
					}
					this._capacity = value;
				}
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600362F RID: 13871 RVA: 0x000B6C05 File Offset: 0x000B5C05
		public override long Length
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return (long)(this._length - this._origin);
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x000B6C22 File Offset: 0x000B5C22
		// (set) Token: 0x06003631 RID: 13873 RVA: 0x000B6C40 File Offset: 0x000B5C40
		public override long Position
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return (long)(this._position - this._origin);
			}
			set
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				if (value > 2147483647L)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
				}
				this._position = this._origin + (int)value;
			}
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000B6CA4 File Offset: 0x000B5CA4
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int num = this._length - this._position;
			if (num > count)
			{
				num = count;
			}
			if (num <= 0)
			{
				return 0;
			}
			if (num <= 8)
			{
				int num2 = num;
				while (--num2 >= 0)
				{
					buffer[offset + num2] = this._buffer[this._position + num2];
				}
			}
			else
			{
				Buffer.InternalBlockCopy(this._buffer, this._position, buffer, offset, num);
			}
			this._position += num;
			return num;
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000B6D84 File Offset: 0x000B5D84
		public override int ReadByte()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (this._position >= this._length)
			{
				return -1;
			}
			return (int)this._buffer[this._position++];
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000B6DC8 File Offset: 0x000B5DC8
		public override long Seek(long offset, SeekOrigin loc)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (offset > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
			}
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = this._origin + (int)offset;
				break;
			case SeekOrigin.Current:
				if (offset + (long)this._position < (long)this._origin)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position += (int)offset;
				break;
			case SeekOrigin.End:
				if ((long)this._length + offset < (long)this._origin)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = this._length + (int)offset;
				break;
			default:
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSeekOrigin"));
			}
			return (long)this._position;
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000B6EBC File Offset: 0x000B5EBC
		public override void SetLength(long value)
		{
			if (!this._writable)
			{
				__Error.WriteNotSupported();
			}
			if (value > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
			}
			if (value < 0L || value > (long)(2147483647 - this._origin))
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
			}
			int num = this._origin + (int)value;
			if (!this.EnsureCapacity(num) && num > this._length)
			{
				Array.Clear(this._buffer, this._length, num - this._length);
			}
			this._length = num;
			if (this._position > num)
			{
				this._position = num;
			}
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000B6F6C File Offset: 0x000B5F6C
		public virtual byte[] ToArray()
		{
			byte[] array = new byte[this._length - this._origin];
			Buffer.InternalBlockCopy(this._buffer, this._origin, array, 0, this._length - this._origin);
			return array;
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000B6FB0 File Offset: 0x000B5FB0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (!this._writable)
			{
				__Error.WriteNotSupported();
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			int num = this._position + count;
			if (num < 0)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
			}
			if (num > this._length)
			{
				bool flag = this._position > this._length;
				if (num > this._capacity)
				{
					bool flag2 = this.EnsureCapacity(num);
					if (flag2)
					{
						flag = false;
					}
				}
				if (flag)
				{
					Array.Clear(this._buffer, this._length, num - this._length);
				}
				this._length = num;
			}
			if (count <= 8)
			{
				int num2 = count;
				while (--num2 >= 0)
				{
					this._buffer[this._position + num2] = buffer[offset + num2];
				}
			}
			else
			{
				Buffer.InternalBlockCopy(buffer, offset, this._buffer, this._position, count);
			}
			this._position = num;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000B70E8 File Offset: 0x000B60E8
		public override void WriteByte(byte value)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (!this._writable)
			{
				__Error.WriteNotSupported();
			}
			if (this._position >= this._length)
			{
				int num = this._position + 1;
				bool flag = this._position > this._length;
				if (num >= this._capacity)
				{
					bool flag2 = this.EnsureCapacity(num);
					if (flag2)
					{
						flag = false;
					}
				}
				if (flag)
				{
					Array.Clear(this._buffer, this._length, this._position - this._length);
				}
				this._length = num;
			}
			this._buffer[this._position++] = value;
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000B718C File Offset: 0x000B618C
		public virtual void WriteTo(Stream stream)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream", Environment.GetResourceString("ArgumentNull_Stream"));
			}
			stream.Write(this._buffer, this._origin, this._length - this._origin);
		}

		// Token: 0x04001C3A RID: 7226
		private const int MemStreamMaxLength = 2147483647;

		// Token: 0x04001C3B RID: 7227
		private byte[] _buffer;

		// Token: 0x04001C3C RID: 7228
		private int _origin;

		// Token: 0x04001C3D RID: 7229
		private int _position;

		// Token: 0x04001C3E RID: 7230
		private int _length;

		// Token: 0x04001C3F RID: 7231
		private int _capacity;

		// Token: 0x04001C40 RID: 7232
		private bool _expandable;

		// Token: 0x04001C41 RID: 7233
		private bool _writable;

		// Token: 0x04001C42 RID: 7234
		private bool _exposable;

		// Token: 0x04001C43 RID: 7235
		private bool _isOpen;
	}
}

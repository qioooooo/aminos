using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.IO
{
	// Token: 0x020005AC RID: 1452
	[CLSCompliant(false)]
	public class UnmanagedMemoryStream : Stream
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x000B8A4A File Offset: 0x000B7A4A
		protected UnmanagedMemoryStream()
		{
			this._mem = null;
			this._isOpen = false;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000B8A61 File Offset: 0x000B7A61
		public unsafe UnmanagedMemoryStream(byte* pointer, long length)
		{
			this.Initialize(pointer, length, length, FileAccess.Read, false);
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000B8A74 File Offset: 0x000B7A74
		public unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access)
		{
			this.Initialize(pointer, length, capacity, access, false);
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000B8A88 File Offset: 0x000B7A88
		internal unsafe UnmanagedMemoryStream(byte* pointer, long length, long capacity, FileAccess access, bool skipSecurityCheck)
		{
			this.Initialize(pointer, length, capacity, access, skipSecurityCheck);
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000B8A9D File Offset: 0x000B7A9D
		protected unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access)
		{
			this.Initialize(pointer, length, capacity, access, false);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000B8AAC File Offset: 0x000B7AAC
		internal unsafe void Initialize(byte* pointer, long length, long capacity, FileAccess access, bool skipSecurityCheck)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			if (length < 0L || capacity < 0L)
			{
				throw new ArgumentOutOfRangeException((length < 0L) ? "length" : "capacity", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (length > capacity)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_LengthGreaterThanCapacity"));
			}
			if (pointer + capacity < pointer)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamWrapAround"));
			}
			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
			{
				throw new ArgumentOutOfRangeException("access", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
			}
			if (this._isOpen)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CalledTwice"));
			}
			if (!skipSecurityCheck)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			}
			this._mem = pointer;
			this._length = length;
			this._capacity = capacity;
			this._access = access;
			this._isOpen = true;
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x000B8B94 File Offset: 0x000B7B94
		public override bool CanRead
		{
			get
			{
				return this._isOpen && (this._access & FileAccess.Read) != (FileAccess)0;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06003665 RID: 13925 RVA: 0x000B8BAE File Offset: 0x000B7BAE
		public override bool CanSeek
		{
			get
			{
				return this._isOpen;
			}
		}

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x000B8BB6 File Offset: 0x000B7BB6
		public override bool CanWrite
		{
			get
			{
				return this._isOpen && (this._access & FileAccess.Write) != (FileAccess)0;
			}
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000B8BD0 File Offset: 0x000B7BD0
		protected override void Dispose(bool disposing)
		{
			this._isOpen = false;
			base.Dispose(disposing);
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000B8BE0 File Offset: 0x000B7BE0
		public override void Flush()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06003669 RID: 13929 RVA: 0x000B8BEF File Offset: 0x000B7BEF
		public override long Length
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._length;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600366A RID: 13930 RVA: 0x000B8C04 File Offset: 0x000B7C04
		public long Capacity
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._capacity;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600366B RID: 13931 RVA: 0x000B8C19 File Offset: 0x000B7C19
		// (set) Token: 0x0600366C RID: 13932 RVA: 0x000B8C30 File Offset: 0x000B7C30
		public override long Position
		{
			get
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return this._position;
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
				if (value > 2147483647L || this._mem + value < this._mem)
				{
					throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
				}
				this._position = value;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x000B8C9C File Offset: 0x000B7C9C
		// (set) Token: 0x0600366E RID: 13934 RVA: 0x000B8CE4 File Offset: 0x000B7CE4
		public unsafe byte* PositionPointer
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				long position = this._position;
				if (position > this._capacity)
				{
					throw new IndexOutOfRangeException(Environment.GetResourceString("IndexOutOfRange_UMSPosition"));
				}
				byte* ptr = this._mem + position;
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				return ptr;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (!this._isOpen)
				{
					__Error.StreamIsClosed();
				}
				if (new IntPtr((long)(value - this._mem)).ToInt64() > 9223372036854775807L)
				{
					throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamLength"));
				}
				if (value < this._mem)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = (long)(value - this._mem);
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x000B8D60 File Offset: 0x000B7D60
		internal unsafe byte* Pointer
		{
			get
			{
				return this._mem;
			}
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x000B8D68 File Offset: 0x000B7D68
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Read) == (FileAccess)0)
			{
				__Error.ReadNotSupported();
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
			long position = this._position;
			long num = this._length - position;
			if (num > (long)count)
			{
				num = (long)count;
			}
			if (num <= 0L)
			{
				return 0;
			}
			int num2 = (int)num;
			if (num2 < 0)
			{
				num2 = 0;
			}
			Buffer.memcpy(this._mem + position, 0, buffer, offset, num2);
			this._position = position + num;
			return num2;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000B8E38 File Offset: 0x000B7E38
		public unsafe override int ReadByte()
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Read) == (FileAccess)0)
			{
				__Error.ReadNotSupported();
			}
			long position = this._position;
			if (position >= this._length)
			{
				return -1;
			}
			this._position = position + 1L;
			return (int)this._mem[position];
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000B8E88 File Offset: 0x000B7E88
		public override long Seek(long offset, SeekOrigin loc)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (offset > 9223372036854775807L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamLength"));
			}
			switch (loc)
			{
			case SeekOrigin.Begin:
				if (offset < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = offset;
				break;
			case SeekOrigin.Current:
				if (offset + this._position < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position += offset;
				break;
			case SeekOrigin.End:
				if (this._length + offset < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
				}
				this._position = this._length + offset;
				break;
			default:
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSeekOrigin"));
			}
			return this._position;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000B8F68 File Offset: 0x000B7F68
		public override void SetLength(long value)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Write) == (FileAccess)0)
			{
				__Error.WriteNotSupported();
			}
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (value > this._capacity)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_FixedCapacity"));
			}
			long length = this._length;
			if (value > length)
			{
				Buffer.ZeroMemory(this._mem + length, value - length);
			}
			this._length = value;
			if (this._position > value)
			{
				this._position = value;
			}
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000B8FF8 File Offset: 0x000B7FF8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Write) == (FileAccess)0)
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
			long position = this._position;
			long length = this._length;
			long num = position + (long)count;
			if (num < 0L)
			{
				throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
			}
			if (num > length)
			{
				if (num > this._capacity)
				{
					throw new NotSupportedException(Environment.GetResourceString("IO.IO_FixedCapacity"));
				}
				this._length = num;
			}
			if (position > length)
			{
				Buffer.ZeroMemory(this._mem + length, position - length);
			}
			Buffer.memcpy(buffer, offset, this._mem + position, 0, count);
			this._position = num;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000B9100 File Offset: 0x000B8100
		public unsafe override void WriteByte(byte value)
		{
			if (!this._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if ((this._access & FileAccess.Write) == (FileAccess)0)
			{
				__Error.WriteNotSupported();
			}
			long position = this._position;
			long length = this._length;
			long num = position + 1L;
			if (position >= length)
			{
				if (num < 0L)
				{
					throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
				}
				if (num > this._capacity)
				{
					throw new NotSupportedException(Environment.GetResourceString("IO.IO_FixedCapacity"));
				}
				this._length = num;
				if (position > length)
				{
					Buffer.ZeroMemory(this._mem + length, position - length);
				}
			}
			this._mem[position] = value;
			this._position = num;
		}

		// Token: 0x04001C4F RID: 7247
		private const long UnmanagedMemStreamMaxLength = 9223372036854775807L;

		// Token: 0x04001C50 RID: 7248
		private unsafe byte* _mem;

		// Token: 0x04001C51 RID: 7249
		private long _length;

		// Token: 0x04001C52 RID: 7250
		private long _capacity;

		// Token: 0x04001C53 RID: 7251
		private long _position;

		// Token: 0x04001C54 RID: 7252
		private FileAccess _access;

		// Token: 0x04001C55 RID: 7253
		internal bool _isOpen;
	}
}

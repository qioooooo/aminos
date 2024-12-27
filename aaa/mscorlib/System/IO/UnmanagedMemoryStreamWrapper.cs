using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005BB RID: 1467
	internal sealed class UnmanagedMemoryStreamWrapper : MemoryStream
	{
		// Token: 0x0600374B RID: 14155 RVA: 0x000BB44F File Offset: 0x000BA44F
		internal UnmanagedMemoryStreamWrapper(UnmanagedMemoryStream stream)
		{
			this._unmanagedStream = stream;
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x000BB45E File Offset: 0x000BA45E
		public override bool CanRead
		{
			get
			{
				return this._unmanagedStream.CanRead;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x000BB46B File Offset: 0x000BA46B
		public override bool CanSeek
		{
			get
			{
				return this._unmanagedStream.CanSeek;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x000BB478 File Offset: 0x000BA478
		public override bool CanWrite
		{
			get
			{
				return this._unmanagedStream.CanWrite;
			}
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x000BB488 File Offset: 0x000BA488
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._unmanagedStream.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x000BB4C0 File Offset: 0x000BA4C0
		public override void Flush()
		{
			this._unmanagedStream.Flush();
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x000BB4CD File Offset: 0x000BA4CD
		public override byte[] GetBuffer()
		{
			throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_MemStreamBuffer"));
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x000BB4DE File Offset: 0x000BA4DE
		// (set) Token: 0x06003753 RID: 14163 RVA: 0x000BB4EC File Offset: 0x000BA4EC
		public override int Capacity
		{
			get
			{
				return (int)this._unmanagedStream.Capacity;
			}
			set
			{
				throw new IOException(Environment.GetResourceString("IO.IO_FixedCapacity"));
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x000BB4FD File Offset: 0x000BA4FD
		public override long Length
		{
			get
			{
				return this._unmanagedStream.Length;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x000BB50A File Offset: 0x000BA50A
		// (set) Token: 0x06003756 RID: 14166 RVA: 0x000BB517 File Offset: 0x000BA517
		public override long Position
		{
			get
			{
				return this._unmanagedStream.Position;
			}
			set
			{
				this._unmanagedStream.Position = value;
			}
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000BB525 File Offset: 0x000BA525
		public override int Read([In] [Out] byte[] buffer, int offset, int count)
		{
			return this._unmanagedStream.Read(buffer, offset, count);
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000BB535 File Offset: 0x000BA535
		public override int ReadByte()
		{
			return this._unmanagedStream.ReadByte();
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x000BB542 File Offset: 0x000BA542
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this._unmanagedStream.Seek(offset, loc);
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000BB554 File Offset: 0x000BA554
		public override byte[] ToArray()
		{
			if (!this._unmanagedStream._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (!this._unmanagedStream.CanRead)
			{
				__Error.ReadNotSupported();
			}
			byte[] array = new byte[this._unmanagedStream.Length];
			Buffer.memcpy(this._unmanagedStream.Pointer, 0, array, 0, (int)this._unmanagedStream.Length);
			return array;
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000BB5B7 File Offset: 0x000BA5B7
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._unmanagedStream.Write(buffer, offset, count);
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000BB5C7 File Offset: 0x000BA5C7
		public override void WriteByte(byte value)
		{
			this._unmanagedStream.WriteByte(value);
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000BB5D8 File Offset: 0x000BA5D8
		public override void WriteTo(Stream stream)
		{
			if (!this._unmanagedStream._isOpen)
			{
				__Error.StreamIsClosed();
			}
			if (!this._unmanagedStream.CanRead)
			{
				__Error.ReadNotSupported();
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream", Environment.GetResourceString("ArgumentNull_Stream"));
			}
			byte[] array = this.ToArray();
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x04001C8E RID: 7310
		private UnmanagedMemoryStream _unmanagedStream;
	}
}

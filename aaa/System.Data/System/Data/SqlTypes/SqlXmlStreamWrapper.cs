using System;
using System.Data.Common;
using System.IO;

namespace System.Data.SqlTypes
{
	// Token: 0x02000379 RID: 889
	internal sealed class SqlXmlStreamWrapper : Stream
	{
		// Token: 0x06002F62 RID: 12130 RVA: 0x002B04C4 File Offset: 0x002AF8C4
		internal SqlXmlStreamWrapper(Stream stream)
		{
			this.m_stream = stream;
			this.m_lPosition = 0L;
			this.m_isClosed = false;
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002F63 RID: 12131 RVA: 0x002B04F0 File Offset: 0x002AF8F0
		public override bool CanRead
		{
			get
			{
				return !this.IsStreamClosed() && this.m_stream.CanRead;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002F64 RID: 12132 RVA: 0x002B0514 File Offset: 0x002AF914
		public override bool CanSeek
		{
			get
			{
				return !this.IsStreamClosed() && this.m_stream.CanSeek;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002F65 RID: 12133 RVA: 0x002B0538 File Offset: 0x002AF938
		public override bool CanWrite
		{
			get
			{
				return !this.IsStreamClosed() && this.m_stream.CanWrite;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002F66 RID: 12134 RVA: 0x002B055C File Offset: 0x002AF95C
		public override long Length
		{
			get
			{
				this.ThrowIfStreamClosed("get_Length");
				this.ThrowIfStreamCannotSeek("get_Length");
				return this.m_stream.Length;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002F67 RID: 12135 RVA: 0x002B058C File Offset: 0x002AF98C
		// (set) Token: 0x06002F68 RID: 12136 RVA: 0x002B05B8 File Offset: 0x002AF9B8
		public override long Position
		{
			get
			{
				this.ThrowIfStreamClosed("get_Position");
				this.ThrowIfStreamCannotSeek("get_Position");
				return this.m_lPosition;
			}
			set
			{
				this.ThrowIfStreamClosed("set_Position");
				this.ThrowIfStreamCannotSeek("set_Position");
				if (value < 0L || value > this.m_stream.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_lPosition = value;
			}
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x002B0600 File Offset: 0x002AFA00
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfStreamClosed("Seek");
			this.ThrowIfStreamCannotSeek("Seek");
			switch (origin)
			{
			case SeekOrigin.Begin:
				if (offset < 0L || offset > this.m_stream.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this.m_lPosition = offset;
				break;
			case SeekOrigin.Current:
			{
				long num = this.m_lPosition + offset;
				if (num < 0L || num > this.m_stream.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this.m_lPosition = num;
				break;
			}
			case SeekOrigin.End:
			{
				long num = this.m_stream.Length + offset;
				if (num < 0L || num > this.m_stream.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this.m_lPosition = num;
				break;
			}
			default:
				throw ADP.InvalidSeekOrigin("offset");
			}
			return this.m_lPosition;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x002B06DC File Offset: 0x002AFADC
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfStreamClosed("Read");
			this.ThrowIfStreamCannotRead("Read");
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.m_stream.CanSeek && this.m_stream.Position != this.m_lPosition)
			{
				this.m_stream.Seek(this.m_lPosition, SeekOrigin.Begin);
			}
			int num = this.m_stream.Read(buffer, offset, count);
			this.m_lPosition += (long)num;
			return num;
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x002B078C File Offset: 0x002AFB8C
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.ThrowIfStreamClosed("Write");
			this.ThrowIfStreamCannotWrite("Write");
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.m_stream.CanSeek && this.m_stream.Position != this.m_lPosition)
			{
				this.m_stream.Seek(this.m_lPosition, SeekOrigin.Begin);
			}
			this.m_stream.Write(buffer, offset, count);
			this.m_lPosition += (long)count;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x002B083C File Offset: 0x002AFC3C
		public override int ReadByte()
		{
			this.ThrowIfStreamClosed("ReadByte");
			this.ThrowIfStreamCannotRead("ReadByte");
			if (this.m_stream.CanSeek && this.m_lPosition >= this.m_stream.Length)
			{
				return -1;
			}
			if (this.m_stream.CanSeek && this.m_stream.Position != this.m_lPosition)
			{
				this.m_stream.Seek(this.m_lPosition, SeekOrigin.Begin);
			}
			int num = this.m_stream.ReadByte();
			this.m_lPosition += 1L;
			return num;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x002B08D0 File Offset: 0x002AFCD0
		public override void WriteByte(byte value)
		{
			this.ThrowIfStreamClosed("WriteByte");
			this.ThrowIfStreamCannotWrite("WriteByte");
			if (this.m_stream.CanSeek && this.m_stream.Position != this.m_lPosition)
			{
				this.m_stream.Seek(this.m_lPosition, SeekOrigin.Begin);
			}
			this.m_stream.WriteByte(value);
			this.m_lPosition += 1L;
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x002B0944 File Offset: 0x002AFD44
		public override void SetLength(long value)
		{
			this.ThrowIfStreamClosed("SetLength");
			this.ThrowIfStreamCannotSeek("SetLength");
			this.m_stream.SetLength(value);
			if (this.m_lPosition > value)
			{
				this.m_lPosition = value;
			}
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x002B0984 File Offset: 0x002AFD84
		public override void Flush()
		{
			if (this.m_stream != null)
			{
				this.m_stream.Flush();
			}
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x002B09A4 File Offset: 0x002AFDA4
		protected override void Dispose(bool disposing)
		{
			try
			{
				this.m_isClosed = true;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x002B09E0 File Offset: 0x002AFDE0
		private void ThrowIfStreamCannotSeek(string method)
		{
			if (!this.m_stream.CanSeek)
			{
				throw new NotSupportedException(SQLResource.InvalidOpStreamNonSeekable(method));
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x002B0A08 File Offset: 0x002AFE08
		private void ThrowIfStreamCannotRead(string method)
		{
			if (!this.m_stream.CanRead)
			{
				throw new NotSupportedException(SQLResource.InvalidOpStreamNonReadable(method));
			}
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x002B0A30 File Offset: 0x002AFE30
		private void ThrowIfStreamCannotWrite(string method)
		{
			if (!this.m_stream.CanWrite)
			{
				throw new NotSupportedException(SQLResource.InvalidOpStreamNonWritable(method));
			}
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x002B0A58 File Offset: 0x002AFE58
		private void ThrowIfStreamClosed(string method)
		{
			if (this.IsStreamClosed())
			{
				throw new ObjectDisposedException(SQLResource.InvalidOpStreamClosed(method));
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x002B0A7C File Offset: 0x002AFE7C
		private bool IsStreamClosed()
		{
			return this.m_isClosed || this.m_stream == null || (!this.m_stream.CanRead && !this.m_stream.CanWrite && !this.m_stream.CanSeek);
		}

		// Token: 0x04001D5B RID: 7515
		private Stream m_stream;

		// Token: 0x04001D5C RID: 7516
		private long m_lPosition;

		// Token: 0x04001D5D RID: 7517
		private bool m_isClosed;
	}
}

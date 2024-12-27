using System;
using System.Data.Common;
using System.IO;

namespace System.Data.SqlTypes
{
	// Token: 0x02000345 RID: 837
	internal sealed class StreamOnSqlBytes : Stream
	{
		// Token: 0x06002C10 RID: 11280 RVA: 0x002A4018 File Offset: 0x002A3418
		internal StreamOnSqlBytes(SqlBytes sb)
		{
			this.m_sb = sb;
			this.m_lPosition = 0L;
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002C11 RID: 11281 RVA: 0x002A403C File Offset: 0x002A343C
		public override bool CanRead
		{
			get
			{
				return this.m_sb != null && !this.m_sb.IsNull;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002C12 RID: 11282 RVA: 0x002A4064 File Offset: 0x002A3464
		public override bool CanSeek
		{
			get
			{
				return this.m_sb != null;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x002A4080 File Offset: 0x002A3480
		public override bool CanWrite
		{
			get
			{
				return this.m_sb != null && (!this.m_sb.IsNull || this.m_sb.m_rgbBuf != null);
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002C14 RID: 11284 RVA: 0x002A40B8 File Offset: 0x002A34B8
		public override long Length
		{
			get
			{
				this.CheckIfStreamClosed("get_Length");
				return this.m_sb.Length;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002C15 RID: 11285 RVA: 0x002A40DC File Offset: 0x002A34DC
		// (set) Token: 0x06002C16 RID: 11286 RVA: 0x002A40FC File Offset: 0x002A34FC
		public override long Position
		{
			get
			{
				this.CheckIfStreamClosed("get_Position");
				return this.m_lPosition;
			}
			set
			{
				this.CheckIfStreamClosed("set_Position");
				if (value < 0L || value > this.m_sb.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_lPosition = value;
			}
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x002A413C File Offset: 0x002A353C
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckIfStreamClosed("Seek");
			switch (origin)
			{
			case SeekOrigin.Begin:
				if (offset < 0L || offset > this.m_sb.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this.m_lPosition = offset;
				break;
			case SeekOrigin.Current:
			{
				long num = this.m_lPosition + offset;
				if (num < 0L || num > this.m_sb.Length)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this.m_lPosition = num;
				break;
			}
			case SeekOrigin.End:
			{
				long num = this.m_sb.Length + offset;
				if (num < 0L || num > this.m_sb.Length)
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

		// Token: 0x06002C18 RID: 11288 RVA: 0x002A4210 File Offset: 0x002A3610
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.CheckIfStreamClosed("Read");
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
			int num = (int)this.m_sb.Read(this.m_lPosition, buffer, offset, count);
			this.m_lPosition += (long)num;
			return num;
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x002A4288 File Offset: 0x002A3688
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.CheckIfStreamClosed("Write");
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
			this.m_sb.Write(this.m_lPosition, buffer, offset, count);
			this.m_lPosition += (long)count;
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x002A4300 File Offset: 0x002A3700
		public override int ReadByte()
		{
			this.CheckIfStreamClosed("ReadByte");
			if (this.m_lPosition >= this.m_sb.Length)
			{
				return -1;
			}
			int num = (int)this.m_sb[this.m_lPosition];
			this.m_lPosition += 1L;
			return num;
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x002A4350 File Offset: 0x002A3750
		public override void WriteByte(byte value)
		{
			this.CheckIfStreamClosed("WriteByte");
			this.m_sb[this.m_lPosition] = value;
			this.m_lPosition += 1L;
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x002A438C File Offset: 0x002A378C
		public override void SetLength(long value)
		{
			this.CheckIfStreamClosed("SetLength");
			this.m_sb.SetLength(value);
			if (this.m_lPosition > value)
			{
				this.m_lPosition = value;
			}
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x002A43C0 File Offset: 0x002A37C0
		public override void Flush()
		{
			if (this.m_sb.FStream())
			{
				this.m_sb.m_stream.Flush();
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x002A43EC File Offset: 0x002A37EC
		protected override void Dispose(bool disposing)
		{
			try
			{
				this.m_sb = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x002A4428 File Offset: 0x002A3828
		private bool FClosed()
		{
			return this.m_sb == null;
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x002A4440 File Offset: 0x002A3840
		private void CheckIfStreamClosed(string methodname)
		{
			if (this.FClosed())
			{
				throw ADP.StreamClosed(methodname);
			}
		}

		// Token: 0x04001C6D RID: 7277
		private SqlBytes m_sb;

		// Token: 0x04001C6E RID: 7278
		private long m_lPosition;
	}
}

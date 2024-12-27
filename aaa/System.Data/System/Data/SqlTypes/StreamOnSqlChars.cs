using System;
using System.Data.Common;
using System.IO;

namespace System.Data.SqlTypes
{
	// Token: 0x02000347 RID: 839
	internal sealed class StreamOnSqlChars : SqlStreamChars
	{
		// Token: 0x06002C41 RID: 11329 RVA: 0x002A4D8C File Offset: 0x002A418C
		internal StreamOnSqlChars(SqlChars s)
		{
			this.m_sqlchars = s;
			this.m_lPosition = 0L;
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x002A4DB0 File Offset: 0x002A41B0
		public override bool IsNull
		{
			get
			{
				return this.m_sqlchars == null || this.m_sqlchars.IsNull;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x002A4DD4 File Offset: 0x002A41D4
		public override bool CanRead
		{
			get
			{
				return this.m_sqlchars != null && !this.m_sqlchars.IsNull;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x002A4DFC File Offset: 0x002A41FC
		public override bool CanSeek
		{
			get
			{
				return this.m_sqlchars != null;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x002A4E18 File Offset: 0x002A4218
		public override bool CanWrite
		{
			get
			{
				return this.m_sqlchars != null && (!this.m_sqlchars.IsNull || this.m_sqlchars.m_rgchBuf != null);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002C46 RID: 11334 RVA: 0x002A4E50 File Offset: 0x002A4250
		public override long Length
		{
			get
			{
				this.CheckIfStreamClosed("get_Length");
				return this.m_sqlchars.Length;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002C47 RID: 11335 RVA: 0x002A4E74 File Offset: 0x002A4274
		// (set) Token: 0x06002C48 RID: 11336 RVA: 0x002A4E94 File Offset: 0x002A4294
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
				if (value < 0L || value > this.m_sqlchars.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_lPosition = value;
			}
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x002A4ED4 File Offset: 0x002A42D4
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.CheckIfStreamClosed("Seek");
			switch (origin)
			{
			case SeekOrigin.Begin:
				if (offset < 0L || offset > this.m_sqlchars.Length)
				{
					throw ADP.ArgumentOutOfRange("offset");
				}
				this.m_lPosition = offset;
				break;
			case SeekOrigin.Current:
			{
				long num = this.m_lPosition + offset;
				if (num < 0L || num > this.m_sqlchars.Length)
				{
					throw ADP.ArgumentOutOfRange("offset");
				}
				this.m_lPosition = num;
				break;
			}
			case SeekOrigin.End:
			{
				long num = this.m_sqlchars.Length + offset;
				if (num < 0L || num > this.m_sqlchars.Length)
				{
					throw ADP.ArgumentOutOfRange("offset");
				}
				this.m_lPosition = num;
				break;
			}
			default:
				throw ADP.ArgumentOutOfRange("offset");
			}
			return this.m_lPosition;
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x002A4FA8 File Offset: 0x002A43A8
		public override int Read(char[] buffer, int offset, int count)
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
			int num = (int)this.m_sqlchars.Read(this.m_lPosition, buffer, offset, count);
			this.m_lPosition += (long)num;
			return num;
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x002A5020 File Offset: 0x002A4420
		public override void Write(char[] buffer, int offset, int count)
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
			this.m_sqlchars.Write(this.m_lPosition, buffer, offset, count);
			this.m_lPosition += (long)count;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x002A5098 File Offset: 0x002A4498
		public override int ReadChar()
		{
			this.CheckIfStreamClosed("ReadChar");
			if (this.m_lPosition >= this.m_sqlchars.Length)
			{
				return -1;
			}
			int num = (int)this.m_sqlchars[this.m_lPosition];
			this.m_lPosition += 1L;
			return num;
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x002A50E8 File Offset: 0x002A44E8
		public override void WriteChar(char value)
		{
			this.CheckIfStreamClosed("WriteChar");
			this.m_sqlchars[this.m_lPosition] = value;
			this.m_lPosition += 1L;
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x002A5124 File Offset: 0x002A4524
		public override void SetLength(long value)
		{
			this.CheckIfStreamClosed("SetLength");
			this.m_sqlchars.SetLength(value);
			if (this.m_lPosition > value)
			{
				this.m_lPosition = value;
			}
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x002A5158 File Offset: 0x002A4558
		public override void Flush()
		{
			if (this.m_sqlchars.FStream())
			{
				this.m_sqlchars.m_stream.Flush();
			}
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x002A5184 File Offset: 0x002A4584
		protected override void Dispose(bool disposing)
		{
			this.m_sqlchars = null;
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x002A5198 File Offset: 0x002A4598
		private bool FClosed()
		{
			return this.m_sqlchars == null;
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x002A51B0 File Offset: 0x002A45B0
		private void CheckIfStreamClosed(string methodname)
		{
			if (this.FClosed())
			{
				throw ADP.StreamClosed(methodname);
			}
		}

		// Token: 0x04001C77 RID: 7287
		private SqlChars m_sqlchars;

		// Token: 0x04001C78 RID: 7288
		private long m_lPosition;
	}
}

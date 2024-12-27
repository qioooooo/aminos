using System;
using System.IO;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000024 RID: 36
	internal class BufferedResponseStream : Stream
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00002F3F File Offset: 0x00001F3F
		internal BufferedResponseStream(Stream outputStream, int buffersize)
		{
			this.buffer = new byte[buffersize];
			this.outputStream = outputStream;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00002F61 File Offset: 0x00001F61
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002F64 File Offset: 0x00001F64
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00002F67 File Offset: 0x00001F67
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002F6A File Offset: 0x00001F6A
		public override long Length
		{
			get
			{
				throw new NotSupportedException(Res.GetString("StreamDoesNotSeek"));
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00002F7B File Offset: 0x00001F7B
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00002F8C File Offset: 0x00001F8C
		public override long Position
		{
			get
			{
				throw new NotSupportedException(Res.GetString("StreamDoesNotSeek"));
			}
			set
			{
				throw new NotSupportedException(Res.GetString("StreamDoesNotSeek"));
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002FA0 File Offset: 0x00001FA0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.outputStream.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700002B RID: 43
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00002FD8 File Offset: 0x00001FD8
		internal bool FlushEnabled
		{
			set
			{
				this.flushEnabled = value;
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002FE1 File Offset: 0x00001FE1
		public override void Flush()
		{
			if (!this.flushEnabled)
			{
				return;
			}
			this.FlushWrite();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00002FF2 File Offset: 0x00001FF2
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException(Res.GetString("StreamDoesNotRead"));
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003003 File Offset: 0x00002003
		public override int EndRead(IAsyncResult asyncResult)
		{
			throw new NotSupportedException(Res.GetString("StreamDoesNotRead"));
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003014 File Offset: 0x00002014
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(Res.GetString("StreamDoesNotSeek"));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003025 File Offset: 0x00002025
		public override void SetLength(long value)
		{
			throw new NotSupportedException(Res.GetString("StreamDoesNotSeek"));
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003036 File Offset: 0x00002036
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException(Res.GetString("StreamDoesNotRead"));
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003047 File Offset: 0x00002047
		public override int ReadByte()
		{
			throw new NotSupportedException(Res.GetString("StreamDoesNotRead"));
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003058 File Offset: 0x00002058
		public override void Write(byte[] array, int offset, int count)
		{
			if (this.position > 0)
			{
				int num = this.buffer.Length - this.position;
				if (num > 0)
				{
					if (num > count)
					{
						num = count;
					}
					Array.Copy(array, offset, this.buffer, this.position, num);
					this.position += num;
					if (count == num)
					{
						return;
					}
					offset += num;
					count -= num;
				}
				this.FlushWrite();
			}
			if (count >= this.buffer.Length)
			{
				this.outputStream.Write(array, offset, count);
				return;
			}
			Array.Copy(array, offset, this.buffer, this.position, count);
			this.position = count;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000030F4 File Offset: 0x000020F4
		private void FlushWrite()
		{
			if (this.position > 0)
			{
				this.outputStream.Write(this.buffer, 0, this.position);
				this.position = 0;
			}
			this.outputStream.Flush();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000312C File Offset: 0x0000212C
		public override void WriteByte(byte value)
		{
			if (this.position == this.buffer.Length)
			{
				this.FlushWrite();
			}
			this.buffer[this.position++] = value;
		}

		// Token: 0x04000237 RID: 567
		private Stream outputStream;

		// Token: 0x04000238 RID: 568
		private byte[] buffer;

		// Token: 0x04000239 RID: 569
		private int position;

		// Token: 0x0400023A RID: 570
		private bool flushEnabled = true;
	}
}

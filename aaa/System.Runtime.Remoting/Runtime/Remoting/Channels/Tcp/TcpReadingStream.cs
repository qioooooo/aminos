using System;
using System.IO;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000045 RID: 69
	internal abstract class TcpReadingStream : Stream
	{
		// Token: 0x06000250 RID: 592 RVA: 0x0000C074 File Offset: 0x0000B074
		public void ReadToEnd()
		{
			byte[] array = new byte[64];
			int num;
			do
			{
				num = this.Read(array, 0, 64);
			}
			while (num > 0);
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000C098 File Offset: 0x0000B098
		public virtual bool FoundEnd
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000C09B File Offset: 0x0000B09B
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000C09E File Offset: 0x0000B09E
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000C0A1 File Offset: 0x0000B0A1
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000C0A4 File Offset: 0x0000B0A4
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000C0AB File Offset: 0x0000B0AB
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000C0B2 File Offset: 0x0000B0B2
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000C0B9 File Offset: 0x0000B0B9
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000C0C0 File Offset: 0x0000B0C0
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000C0C7 File Offset: 0x0000B0C7
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000C0CE File Offset: 0x0000B0CE
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}

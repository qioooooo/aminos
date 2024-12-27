using System;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000046 RID: 70
	internal sealed class TcpFixedLengthReadingStream : TcpReadingStream
	{
		// Token: 0x0600025D RID: 605 RVA: 0x0000C0DD File Offset: 0x0000B0DD
		internal TcpFixedLengthReadingStream(SocketHandler inputStream, int contentLength)
		{
			this._inputStream = inputStream;
			this._bytesLeft = contentLength;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000C0F3 File Offset: 0x0000B0F3
		public override bool FoundEnd
		{
			get
			{
				return this._bytesLeft == 0;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000C100 File Offset: 0x0000B100
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._inputStream.OnInputStreamClosed();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000C138 File Offset: 0x0000B138
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._bytesLeft == 0)
			{
				return 0;
			}
			int num = this._inputStream.Read(buffer, offset, Math.Min(this._bytesLeft, count));
			if (num > 0)
			{
				this._bytesLeft -= num;
			}
			return num;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000C17C File Offset: 0x0000B17C
		public override int ReadByte()
		{
			if (this._bytesLeft == 0)
			{
				return -1;
			}
			this._bytesLeft--;
			return this._inputStream.ReadByte();
		}

		// Token: 0x040001A9 RID: 425
		private SocketHandler _inputStream;

		// Token: 0x040001AA RID: 426
		private int _bytesLeft;
	}
}

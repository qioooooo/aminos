using System;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000047 RID: 71
	internal sealed class TcpChunkedReadingStream : TcpReadingStream
	{
		// Token: 0x06000262 RID: 610 RVA: 0x0000C1A1 File Offset: 0x0000B1A1
		internal TcpChunkedReadingStream(SocketHandler inputStream)
		{
			this._inputStream = inputStream;
			this._bytesLeft = 0;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000C1C3 File Offset: 0x0000B1C3
		public override bool FoundEnd
		{
			get
			{
				return this._bFoundEnd;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000C1CC File Offset: 0x0000B1CC
		protected override void Dispose(bool disposing)
		{
			try
			{
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000C1F4 File Offset: 0x0000B1F4
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			while (!this._bFoundEnd && count > 0)
			{
				if (this._bytesLeft == 0)
				{
					this._bytesLeft = this._inputStream.ReadInt32();
					if (this._bytesLeft == 0)
					{
						this.ReadTrailer();
						this._bFoundEnd = true;
					}
				}
				if (!this._bFoundEnd)
				{
					int num2 = Math.Min(this._bytesLeft, count);
					int num3 = this._inputStream.Read(buffer, offset, num2);
					if (num3 <= 0)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_ChunkedEncodingError"));
					}
					this._bytesLeft -= num3;
					count -= num3;
					offset += num3;
					num += num3;
					if (this._bytesLeft == 0)
					{
						this.ReadTrailer();
					}
				}
			}
			return num;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000C2A8 File Offset: 0x0000B2A8
		public override int ReadByte()
		{
			if (this.Read(this._byteBuffer, 0, 1) == 0)
			{
				return -1;
			}
			return (int)this._byteBuffer[0];
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000C2D4 File Offset: 0x0000B2D4
		private void ReadTrailer()
		{
			int num = this._inputStream.ReadByte();
			if (num != 13)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_ChunkedEncodingError"));
			}
			num = this._inputStream.ReadByte();
			if (num != 10)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_ChunkedEncodingError"));
			}
		}

		// Token: 0x040001AB RID: 427
		private SocketHandler _inputStream;

		// Token: 0x040001AC RID: 428
		private int _bytesLeft;

		// Token: 0x040001AD RID: 429
		private bool _bFoundEnd;

		// Token: 0x040001AE RID: 430
		private byte[] _byteBuffer = new byte[1];
	}
}

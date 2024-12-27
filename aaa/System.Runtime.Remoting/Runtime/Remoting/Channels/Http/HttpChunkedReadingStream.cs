using System;
using System.Text;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000035 RID: 53
	internal sealed class HttpChunkedReadingStream : HttpReadingStream
	{
		// Token: 0x060001BC RID: 444 RVA: 0x00008AD1 File Offset: 0x00007AD1
		internal HttpChunkedReadingStream(HttpSocketHandler inputStream)
		{
			this._inputStream = inputStream;
			this._bytesLeft = 0;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00008AF3 File Offset: 0x00007AF3
		public override bool FoundEnd
		{
			get
			{
				return this._bFoundEnd;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008AFC File Offset: 0x00007AFC
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

		// Token: 0x060001BF RID: 447 RVA: 0x00008B24 File Offset: 0x00007B24
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			while (!this._bFoundEnd && count > 0)
			{
				if (this._bytesLeft == 0)
				{
					for (;;)
					{
						byte b = (byte)this._inputStream.ReadByte();
						if (b == 13)
						{
							break;
						}
						int num2 = HttpChannelHelper.CharacterHexDigitToDecimal(b);
						if (num2 < 0 || num2 > 15)
						{
							goto IL_0054;
						}
						this._bytesLeft = this._bytesLeft * 16 + num2;
					}
					if ((ushort)this._inputStream.ReadByte() != 10)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_ChunkedEncodingError"));
					}
					if (this._bytesLeft == 0)
					{
						string text;
						do
						{
							text = this._inputStream.ReadToEndOfLine();
						}
						while (text.Length != 0);
						this._bFoundEnd = true;
						goto IL_009A;
					}
					goto IL_009A;
					IL_0054:
					throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_ChunkedEncodingError"));
				}
				IL_009A:
				if (!this._bFoundEnd)
				{
					int num3 = Math.Min(this._bytesLeft, count);
					int num4 = this._inputStream.Read(buffer, offset, num3);
					if (num4 <= 0)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_ChunkedEncodingError"));
					}
					this._bytesLeft -= num4;
					count -= num4;
					offset += num4;
					num += num4;
					if (this._bytesLeft == 0)
					{
						char c = (char)this._inputStream.ReadByte();
						if (c != '\r')
						{
							throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_ChunkedEncodingError"));
						}
						c = (char)this._inputStream.ReadByte();
						if (c != '\n')
						{
							throw new RemotingException(CoreChannel.GetResourceString("Remoting_Http_ChunkedEncodingError"));
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008C8C File Offset: 0x00007C8C
		public override int ReadByte()
		{
			if (this.Read(this._byteBuffer, 0, 1) == 0)
			{
				return -1;
			}
			return (int)this._byteBuffer[0];
		}

		// Token: 0x04000143 RID: 323
		private static byte[] _trailer = Encoding.ASCII.GetBytes("0\r\n\r\n\r\n");

		// Token: 0x04000144 RID: 324
		private static byte[] _endChunk = Encoding.ASCII.GetBytes("\r\n");

		// Token: 0x04000145 RID: 325
		private HttpSocketHandler _inputStream;

		// Token: 0x04000146 RID: 326
		private int _bytesLeft;

		// Token: 0x04000147 RID: 327
		private bool _bFoundEnd;

		// Token: 0x04000148 RID: 328
		private byte[] _byteBuffer = new byte[1];
	}
}

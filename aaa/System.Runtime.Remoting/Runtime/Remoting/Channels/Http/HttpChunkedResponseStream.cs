using System;
using System.IO;
using System.Text;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000032 RID: 50
	internal sealed class HttpChunkedResponseStream : HttpServerResponseStream
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00008688 File Offset: 0x00007688
		internal HttpChunkedResponseStream(Stream outputStream)
		{
			this._outputStream = outputStream;
			this._chunk = CoreChannel.BufferPool.GetBuffer();
			this._chunkSize = this._chunk.Length - 2;
			this._chunkOffset = 0;
			this._chunk[this._chunkSize - 2] = 13;
			this._chunk[this._chunkSize - 1] = 10;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000086F8 File Offset: 0x000076F8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._chunkOffset > 0)
					{
						this.FlushChunk();
					}
					this._outputStream.Write(HttpChunkedResponseStream._trailer, 0, HttpChunkedResponseStream._trailer.Length);
					this._outputStream.Flush();
				}
				CoreChannel.BufferPool.ReturnBuffer(this._chunk);
				this._chunk = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000876C File Offset: 0x0000776C
		public override void Flush()
		{
			if (this._chunkOffset > 0)
			{
				this.FlushChunk();
			}
			this._outputStream.Flush();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008788 File Offset: 0x00007788
		public override void Write(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				if (this._chunkOffset == 0 && count >= this._chunkSize)
				{
					this.WriteChunk(buffer, offset, count);
					return;
				}
				int num = Math.Min(this._chunkSize - this._chunkOffset, count);
				Array.Copy(buffer, offset, this._chunk, this._chunkOffset, num);
				this._chunkOffset += num;
				count -= num;
				offset += num;
				if (this._chunkOffset == this._chunkSize)
				{
					this.FlushChunk();
				}
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000880A File Offset: 0x0000780A
		public override void WriteByte(byte value)
		{
			this._byteBuffer[0] = value;
			this.Write(this._byteBuffer, 0, 1);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008823 File Offset: 0x00007823
		private void FlushChunk()
		{
			this.WriteChunk(this._chunk, 0, this._chunkOffset);
			this._chunkOffset = 0;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00008840 File Offset: 0x00007840
		private void WriteChunk(byte[] buffer, int offset, int count)
		{
			byte[] array = this.IntToHexChars(count);
			this._outputStream.Write(array, 0, array.Length);
			if (buffer == this._chunk)
			{
				this._outputStream.Write(this._chunk, offset, count + 2);
				return;
			}
			this._outputStream.Write(buffer, offset, count);
			this._outputStream.Write(HttpChunkedResponseStream._endChunk, 0, HttpChunkedResponseStream._endChunk.Length);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000088AC File Offset: 0x000078AC
		private byte[] IntToHexChars(int i)
		{
			string text = "";
			while (i > 0)
			{
				int num = i % 16;
				switch (num)
				{
				case 10:
					text = 'A' + text;
					break;
				case 11:
					text = 'B' + text;
					break;
				case 12:
					text = 'C' + text;
					break;
				case 13:
					text = 'D' + text;
					break;
				case 14:
					text = 'E' + text;
					break;
				case 15:
					text = 'F' + text;
					break;
				default:
					text = (char)(num + 48) + text;
					break;
				}
				i /= 16;
			}
			text += "\r\n";
			return Encoding.ASCII.GetBytes(text);
		}

		// Token: 0x0400013A RID: 314
		private static byte[] _trailer = Encoding.ASCII.GetBytes("0\r\n\r\n");

		// Token: 0x0400013B RID: 315
		private static byte[] _endChunk = Encoding.ASCII.GetBytes("\r\n");

		// Token: 0x0400013C RID: 316
		private Stream _outputStream;

		// Token: 0x0400013D RID: 317
		private byte[] _chunk;

		// Token: 0x0400013E RID: 318
		private int _chunkSize;

		// Token: 0x0400013F RID: 319
		private int _chunkOffset;

		// Token: 0x04000140 RID: 320
		private byte[] _byteBuffer = new byte[1];
	}
}

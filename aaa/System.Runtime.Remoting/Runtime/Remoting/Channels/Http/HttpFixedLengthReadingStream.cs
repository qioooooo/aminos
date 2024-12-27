using System;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000034 RID: 52
	internal sealed class HttpFixedLengthReadingStream : HttpReadingStream
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x00008A1F File Offset: 0x00007A1F
		internal HttpFixedLengthReadingStream(HttpSocketHandler inputStream, int contentLength)
		{
			this._inputStream = inputStream;
			this._bytesLeft = contentLength;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00008A35 File Offset: 0x00007A35
		public override bool FoundEnd
		{
			get
			{
				return this._bytesLeft == 0;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008A40 File Offset: 0x00007A40
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

		// Token: 0x060001BA RID: 442 RVA: 0x00008A68 File Offset: 0x00007A68
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

		// Token: 0x060001BB RID: 443 RVA: 0x00008AAC File Offset: 0x00007AAC
		public override int ReadByte()
		{
			if (this._bytesLeft == 0)
			{
				return -1;
			}
			this._bytesLeft--;
			return this._inputStream.ReadByte();
		}

		// Token: 0x04000141 RID: 321
		private HttpSocketHandler _inputStream;

		// Token: 0x04000142 RID: 322
		private int _bytesLeft;
	}
}

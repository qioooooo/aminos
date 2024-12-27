using System;
using System.IO;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000031 RID: 49
	internal sealed class HttpFixedLengthResponseStream : HttpServerResponseStream
	{
		// Token: 0x0600019C RID: 412 RVA: 0x0000860E File Offset: 0x0000760E
		internal HttpFixedLengthResponseStream(Stream outputStream, int length)
		{
			this._outputStream = outputStream;
			HttpFixedLengthResponseStream._length = length;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008624 File Offset: 0x00007624
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._outputStream.Flush();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000865C File Offset: 0x0000765C
		public override void Flush()
		{
			this._outputStream.Flush();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008669 File Offset: 0x00007669
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._outputStream.Write(buffer, offset, count);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008679 File Offset: 0x00007679
		public override void WriteByte(byte value)
		{
			this._outputStream.WriteByte(value);
		}

		// Token: 0x04000138 RID: 312
		private Stream _outputStream;

		// Token: 0x04000139 RID: 313
		private static int _length;
	}
}

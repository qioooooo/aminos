using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020006B7 RID: 1719
	internal class SevenBitStream : DelegatedStream
	{
		// Token: 0x06003515 RID: 13589 RVA: 0x000E1A85 File Offset: 0x000E0A85
		internal SevenBitStream(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x000E1A90 File Offset: 0x000E0A90
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.CheckBytes(buffer, offset, count);
			return base.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000E1AEC File Offset: 0x000E0AEC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.CheckBytes(buffer, offset, count);
			base.Write(buffer, offset, count);
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000E1B44 File Offset: 0x000E0B44
		private void CheckBytes(byte[] buffer, int offset, int count)
		{
			for (int i = count; i < offset + count; i++)
			{
				if (buffer[i] > 127)
				{
					throw new FormatException(SR.GetString("Mail7BitStreamInvalidCharacter"));
				}
			}
		}
	}
}

using System;
using System.IO;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000069 RID: 105
	internal class SoapExtensionStream : Stream
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000D470 File Offset: 0x0000C470
		internal SoapExtensionStream()
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000D478 File Offset: 0x0000C478
		private bool EnsureStreamReady()
		{
			if (this.streamReady)
			{
				return true;
			}
			throw new InvalidOperationException(Res.GetString("WebBadStreamState"));
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000D493 File Offset: 0x0000C493
		public override bool CanRead
		{
			get
			{
				this.EnsureStreamReady();
				return this.innerStream.CanRead;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000D4A7 File Offset: 0x0000C4A7
		public override bool CanSeek
		{
			get
			{
				this.EnsureStreamReady();
				return this.innerStream.CanSeek;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000D4BB File Offset: 0x0000C4BB
		public override bool CanWrite
		{
			get
			{
				this.EnsureStreamReady();
				return this.innerStream.CanWrite;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000D4CF File Offset: 0x0000C4CF
		internal bool HasWritten
		{
			get
			{
				return this.hasWritten;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x0000D4D7 File Offset: 0x0000C4D7
		public override long Length
		{
			get
			{
				this.EnsureStreamReady();
				return this.innerStream.Length;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000D4EB File Offset: 0x0000C4EB
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000D4FF File Offset: 0x0000C4FF
		public override long Position
		{
			get
			{
				this.EnsureStreamReady();
				return this.innerStream.Position;
			}
			set
			{
				this.EnsureStreamReady();
				this.hasWritten = true;
				this.innerStream.Position = value;
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000D51C File Offset: 0x0000C51C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.EnsureStreamReady();
					this.hasWritten = true;
					this.innerStream.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000D560 File Offset: 0x0000C560
		public override void Flush()
		{
			this.EnsureStreamReady();
			this.hasWritten = true;
			this.innerStream.Flush();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000D57B File Offset: 0x0000C57B
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.EnsureStreamReady();
			return this.innerStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000D596 File Offset: 0x0000C596
		public override int EndRead(IAsyncResult asyncResult)
		{
			this.EnsureStreamReady();
			return this.innerStream.EndRead(asyncResult);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000D5AB File Offset: 0x0000C5AB
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			this.EnsureStreamReady();
			this.hasWritten = true;
			return this.innerStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000D5CD File Offset: 0x0000C5CD
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.EnsureStreamReady();
			this.hasWritten = true;
			this.innerStream.EndWrite(asyncResult);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000D5E9 File Offset: 0x0000C5E9
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.EnsureStreamReady();
			return this.innerStream.Seek(offset, origin);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000D5FF File Offset: 0x0000C5FF
		public override void SetLength(long value)
		{
			this.EnsureStreamReady();
			this.innerStream.SetLength(value);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000D614 File Offset: 0x0000C614
		public override int Read(byte[] buffer, int offset, int count)
		{
			this.EnsureStreamReady();
			return this.innerStream.Read(buffer, offset, count);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000D62B File Offset: 0x0000C62B
		public override int ReadByte()
		{
			this.EnsureStreamReady();
			return this.innerStream.ReadByte();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000D63F File Offset: 0x0000C63F
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.EnsureStreamReady();
			this.hasWritten = true;
			this.innerStream.Write(buffer, offset, count);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000D65D File Offset: 0x0000C65D
		public override void WriteByte(byte value)
		{
			this.EnsureStreamReady();
			this.hasWritten = true;
			this.innerStream.WriteByte(value);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000D679 File Offset: 0x0000C679
		internal void SetInnerStream(Stream stream)
		{
			this.innerStream = stream;
			this.hasWritten = false;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000D689 File Offset: 0x0000C689
		internal void SetStreamReady()
		{
			this.streamReady = true;
		}

		// Token: 0x04000312 RID: 786
		internal Stream innerStream;

		// Token: 0x04000313 RID: 787
		private bool hasWritten;

		// Token: 0x04000314 RID: 788
		private bool streamReady;
	}
}

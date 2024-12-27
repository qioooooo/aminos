using System;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x020002C0 RID: 704
	internal class SqlClientWrapperSmiStream : Stream
	{
		// Token: 0x06002383 RID: 9091 RVA: 0x00272254 File Offset: 0x00271654
		internal SqlClientWrapperSmiStream(SmiEventSink_Default sink, SmiStream stream)
		{
			this._sink = sink;
			this._stream = stream;
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x00272278 File Offset: 0x00271678
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002385 RID: 9093 RVA: 0x00272290 File Offset: 0x00271690
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x002722A8 File Offset: 0x002716A8
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x002722C0 File Offset: 0x002716C0
		public override long Length
		{
			get
			{
				long length = this._stream.GetLength(this._sink);
				this._sink.ProcessMessagesAndThrow();
				return length;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x002722EC File Offset: 0x002716EC
		// (set) Token: 0x06002389 RID: 9097 RVA: 0x00272318 File Offset: 0x00271718
		public override long Position
		{
			get
			{
				long position = this._stream.GetPosition(this._sink);
				this._sink.ProcessMessagesAndThrow();
				return position;
			}
			set
			{
				this._stream.SetPosition(this._sink, value);
				this._sink.ProcessMessagesAndThrow();
			}
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x00272344 File Offset: 0x00271744
		public override void Flush()
		{
			this._stream.Flush(this._sink);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x00272370 File Offset: 0x00271770
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = this._stream.Seek(this._sink, offset, origin);
			this._sink.ProcessMessagesAndThrow();
			return num;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x002723A0 File Offset: 0x002717A0
		public override void SetLength(long value)
		{
			this._stream.SetLength(this._sink, value);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x002723CC File Offset: 0x002717CC
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._stream.Read(this._sink, buffer, offset, count);
			this._sink.ProcessMessagesAndThrow();
			return num;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x002723FC File Offset: 0x002717FC
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(this._sink, buffer, offset, count);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x0400170B RID: 5899
		private SmiEventSink_Default _sink;

		// Token: 0x0400170C RID: 5900
		private SmiStream _stream;
	}
}

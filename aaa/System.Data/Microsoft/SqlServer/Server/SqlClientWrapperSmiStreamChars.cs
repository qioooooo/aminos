using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x020002C3 RID: 707
	internal class SqlClientWrapperSmiStreamChars : SqlStreamChars
	{
		// Token: 0x060023B0 RID: 9136 RVA: 0x002725D8 File Offset: 0x002719D8
		internal SqlClientWrapperSmiStreamChars(SmiEventSink_Default sink, SmiStream stream)
		{
			this._sink = sink;
			this._stream = stream;
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x002725FC File Offset: 0x002719FC
		public override bool IsNull
		{
			get
			{
				return null == this._stream;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x00272614 File Offset: 0x00271A14
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060023B3 RID: 9139 RVA: 0x0027262C File Offset: 0x00271A2C
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060023B4 RID: 9140 RVA: 0x00272644 File Offset: 0x00271A44
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060023B5 RID: 9141 RVA: 0x0027265C File Offset: 0x00271A5C
		public override long Length
		{
			get
			{
				long length = this._stream.GetLength(this._sink);
				this._sink.ProcessMessagesAndThrow();
				if (length > 0L)
				{
					return length / 2L;
				}
				return length;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x00272694 File Offset: 0x00271A94
		// (set) Token: 0x060023B7 RID: 9143 RVA: 0x002726C4 File Offset: 0x00271AC4
		public override long Position
		{
			get
			{
				long num = this._stream.GetPosition(this._sink) / 2L;
				this._sink.ProcessMessagesAndThrow();
				return num;
			}
			set
			{
				if (value < 0L)
				{
					throw ADP.ArgumentOutOfRange("Position");
				}
				this._stream.SetPosition(this._sink, value * 2L);
				this._sink.ProcessMessagesAndThrow();
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x00272704 File Offset: 0x00271B04
		public override void Flush()
		{
			this._stream.Flush(this._sink);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x00272730 File Offset: 0x00271B30
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = this._stream.Seek(this._sink, offset * 2L, origin);
			this._sink.ProcessMessagesAndThrow();
			return num;
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x00272760 File Offset: 0x00271B60
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw ADP.ArgumentOutOfRange("value");
			}
			this._stream.SetLength(this._sink, value * 2L);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x002727A0 File Offset: 0x00271BA0
		public override int Read(char[] buffer, int offset, int count)
		{
			int num = this._stream.Read(this._sink, buffer, offset * 2, count);
			this._sink.ProcessMessagesAndThrow();
			return num;
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x002727D0 File Offset: 0x00271BD0
		public override void Write(char[] buffer, int offset, int count)
		{
			this._stream.Write(this._sink, buffer, offset, count);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x002727FC File Offset: 0x00271BFC
		internal int Read(byte[] buffer, int offset, int count)
		{
			int num = this._stream.Read(this._sink, buffer, offset, count);
			this._sink.ProcessMessagesAndThrow();
			return num;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0027282C File Offset: 0x00271C2C
		internal void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(this._sink, buffer, offset, count);
			this._sink.ProcessMessagesAndThrow();
		}

		// Token: 0x0400170D RID: 5901
		private SmiEventSink_Default _sink;

		// Token: 0x0400170E RID: 5902
		private SmiStream _stream;
	}
}

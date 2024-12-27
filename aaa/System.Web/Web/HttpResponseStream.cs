using System;
using System.IO;

namespace System.Web
{
	// Token: 0x0200009D RID: 157
	internal class HttpResponseStream : Stream
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x000236CE File Offset: 0x000226CE
		internal HttpResponseStream(HttpWriter writer)
		{
			this._writer = writer;
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x000236DD File Offset: 0x000226DD
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x000236E0 File Offset: 0x000226E0
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x000236E3 File Offset: 0x000226E3
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x000236E6 File Offset: 0x000226E6
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x000236ED File Offset: 0x000226ED
		// (set) Token: 0x06000800 RID: 2048 RVA: 0x000236F4 File Offset: 0x000226F4
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000236FC File Offset: 0x000226FC
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._writer.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00023734 File Offset: 0x00022734
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00023741 File Offset: 0x00022741
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00023748 File Offset: 0x00022748
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002374F File Offset: 0x0002274F
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00023758 File Offset: 0x00022758
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._writer.IgnoringFurtherWrites)
			{
				return;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length - offset;
			if (offset < 0 || num <= 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (num < count)
			{
				count = num;
			}
			this._writer.WriteFromStream(buffer, offset, count);
		}

		// Token: 0x04001182 RID: 4482
		private HttpWriter _writer;
	}
}

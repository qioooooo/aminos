using System;
using System.Security.Permissions;

namespace System.IO.Compression
{
	// Token: 0x0200020D RID: 525
	public class GZipStream : Stream
	{
		// Token: 0x060011C9 RID: 4553 RVA: 0x0003B463 File Offset: 0x0003A463
		public GZipStream(Stream stream, CompressionMode mode)
			: this(stream, mode, false)
		{
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0003B46E File Offset: 0x0003A46E
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
		{
			this.deflateStream = new DeflateStream(stream, mode, leaveOpen, true);
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x0003B485 File Offset: 0x0003A485
		public override bool CanRead
		{
			get
			{
				return this.deflateStream != null && this.deflateStream.CanRead;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x0003B49C File Offset: 0x0003A49C
		public override bool CanWrite
		{
			get
			{
				return this.deflateStream != null && this.deflateStream.CanWrite;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x0003B4B3 File Offset: 0x0003A4B3
		public override bool CanSeek
		{
			get
			{
				return this.deflateStream != null && this.deflateStream.CanSeek;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x0003B4CA File Offset: 0x0003A4CA
		public override long Length
		{
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x0003B4DB File Offset: 0x0003A4DB
		// (set) Token: 0x060011D0 RID: 4560 RVA: 0x0003B4EC File Offset: 0x0003A4EC
		public override long Position
		{
			get
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("NotSupported"));
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0003B4FD File Offset: 0x0003A4FD
		public override void Flush()
		{
			if (this.deflateStream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
			this.deflateStream.Flush();
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x0003B523 File Offset: 0x0003A523
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0003B534 File Offset: 0x0003A534
		public override void SetLength(long value)
		{
			throw new NotSupportedException(SR.GetString("NotSupported"));
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0003B545 File Offset: 0x0003A545
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.BeginRead(array, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0003B571 File Offset: 0x0003A571
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.EndRead(asyncResult);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0003B597 File Offset: 0x0003A597
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.BeginWrite(array, offset, count, asyncCallback, asyncState);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0003B5C3 File Offset: 0x0003A5C3
		public override void EndWrite(IAsyncResult asyncResult)
		{
			if (this.deflateStream == null)
			{
				throw new InvalidOperationException(SR.GetString("ObjectDisposed_StreamClosed"));
			}
			this.deflateStream.EndWrite(asyncResult);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0003B5E9 File Offset: 0x0003A5E9
		public override int Read(byte[] array, int offset, int count)
		{
			if (this.deflateStream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
			return this.deflateStream.Read(array, offset, count);
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0003B612 File Offset: 0x0003A612
		public override void Write(byte[] array, int offset, int count)
		{
			if (this.deflateStream == null)
			{
				throw new ObjectDisposedException(null, SR.GetString("ObjectDisposed_StreamClosed"));
			}
			this.deflateStream.Write(array, offset, count);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0003B63C File Offset: 0x0003A63C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.deflateStream != null)
				{
					this.deflateStream.Close();
				}
				this.deflateStream = null;
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x0003B680 File Offset: 0x0003A680
		public Stream BaseStream
		{
			get
			{
				if (this.deflateStream != null)
				{
					return this.deflateStream.BaseStream;
				}
				return null;
			}
		}

		// Token: 0x04001029 RID: 4137
		private DeflateStream deflateStream;
	}
}

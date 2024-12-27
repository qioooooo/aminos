using System;
using System.IO;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200005B RID: 91
	internal sealed class PipeStream : Stream
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000E391 File Offset: 0x0000D391
		public PipeStream(IpcPort port)
		{
			if (port == null)
			{
				throw new ArgumentNullException("port");
			}
			this._port = port;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000E3AE File Offset: 0x0000D3AE
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000E3B1 File Offset: 0x0000D3B1
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000E3B4 File Offset: 0x0000D3B4
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000E3B7 File Offset: 0x0000D3B7
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000E3BE File Offset: 0x0000D3BE
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000E3C5 File Offset: 0x0000D3C5
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

		// Token: 0x060002EC RID: 748 RVA: 0x0000E3CC File Offset: 0x0000D3CC
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000E3D4 File Offset: 0x0000D3D4
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (this._timeout <= 0)
			{
				return this._port.Read(buffer, offset, size);
			}
			IAsyncResult asyncResult = this._port.BeginRead(buffer, offset, size, null, null);
			if (this._timeout > 0 && !asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne(this._timeout, false);
				if (!asyncResult.IsCompleted)
				{
					throw new RemotingTimeoutException();
				}
			}
			return this._port.EndRead(asyncResult);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000E448 File Offset: 0x0000D448
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._port.Write(buffer, offset, count);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000E458 File Offset: 0x0000D458
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._port.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000E490 File Offset: 0x0000D490
		public override void Flush()
		{
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000E494 File Offset: 0x0000D494
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this._port.BeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000E4B5 File Offset: 0x0000D4B5
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._port.EndRead(asyncResult);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000E4C3 File Offset: 0x0000D4C3
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000E4CA File Offset: 0x0000D4CA
		public override void EndWrite(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000E4D1 File Offset: 0x0000D4D1
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400020D RID: 525
		private IpcPort _port;

		// Token: 0x0400020E RID: 526
		private int _timeout;
	}
}

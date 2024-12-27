using System;
using System.IO;
using System.Net.Sockets;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200001B RID: 27
	internal sealed class SocketStream : Stream
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00004FC3 File Offset: 0x00003FC3
		public SocketStream(Socket socket)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket");
			}
			this._socket = socket;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004FE0 File Offset: 0x00003FE0
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00004FE3 File Offset: 0x00003FE3
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004FE6 File Offset: 0x00003FE6
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004FE9 File Offset: 0x00003FE9
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004FF0 File Offset: 0x00003FF0
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004FF7 File Offset: 0x00003FF7
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

		// Token: 0x060000CE RID: 206 RVA: 0x00004FFE File Offset: 0x00003FFE
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005008 File Offset: 0x00004008
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (this._timeout <= 0)
			{
				return this._socket.Receive(buffer, offset, Math.Min(size, 4194304), SocketFlags.None);
			}
			IAsyncResult asyncResult = this._socket.BeginReceive(buffer, offset, Math.Min(size, 4194304), SocketFlags.None, null, null);
			if (this._timeout > 0 && !asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne(this._timeout, false);
				if (!asyncResult.IsCompleted)
				{
					throw new RemotingTimeoutException();
				}
			}
			return this._socket.EndReceive(asyncResult);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005094 File Offset: 0x00004094
		public override void Write(byte[] buffer, int offset, int count)
		{
			int i = count;
			while (i > 0)
			{
				count = Math.Min(i, 65536);
				this._socket.Send(buffer, offset, count, SocketFlags.None);
				i -= count;
				offset += count;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000050D0 File Offset: 0x000040D0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._socket.Close();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00005108 File Offset: 0x00004108
		public override void Flush()
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000510C File Offset: 0x0000410C
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this._socket.BeginReceive(buffer, offset, Math.Min(size, 4194304), SocketFlags.None, callback, state);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005138 File Offset: 0x00004138
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._socket.EndReceive(asyncResult);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005148 File Offset: 0x00004148
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this._socket.BeginSend(buffer, offset, size, SocketFlags.None, callback, state);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000516A File Offset: 0x0000416A
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._socket.EndSend(asyncResult);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005179 File Offset: 0x00004179
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040000AB RID: 171
		private const int maxSocketWrite = 65536;

		// Token: 0x040000AC RID: 172
		private const int maxSocketRead = 4194304;

		// Token: 0x040000AD RID: 173
		private Socket _socket;

		// Token: 0x040000AE RID: 174
		private int _timeout;
	}
}

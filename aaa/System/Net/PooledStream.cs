using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020004B9 RID: 1209
	internal class PooledStream : Stream
	{
		// Token: 0x0600255A RID: 9562 RVA: 0x0009505F File Offset: 0x0009405F
		internal PooledStream(object owner)
		{
			this.m_Owner = new WeakReference(owner);
			this.m_PooledCount = -1;
			this.m_Initalizing = true;
			this.m_NetworkStream = new NetworkStream();
			this.m_CreateTime = DateTime.UtcNow;
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x00095097 File Offset: 0x00094097
		internal PooledStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
		{
			this.m_ConnectionPool = connectionPool;
			this.m_Lifetime = lifetime;
			this.m_CheckLifetime = checkLifetime;
			this.m_Initalizing = true;
			this.m_NetworkStream = new NetworkStream();
			this.m_CreateTime = DateTime.UtcNow;
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x000950D1 File Offset: 0x000940D1
		internal bool JustConnected
		{
			get
			{
				if (this.m_JustConnected)
				{
					this.m_JustConnected = false;
					return true;
				}
				return false;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x000950E5 File Offset: 0x000940E5
		internal IPAddress ServerAddress
		{
			get
			{
				return this.m_ServerAddress;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x000950ED File Offset: 0x000940ED
		internal bool IsInitalizing
		{
			get
			{
				return this.m_Initalizing;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x000950F8 File Offset: 0x000940F8
		// (set) Token: 0x06002560 RID: 9568 RVA: 0x00095141 File Offset: 0x00094141
		internal bool CanBePooled
		{
			get
			{
				if (this.m_Initalizing)
				{
					return true;
				}
				if (!this.m_NetworkStream.Connected)
				{
					return false;
				}
				WeakReference owner = this.m_Owner;
				return !this.m_ConnectionIsDoomed && (owner == null || !owner.IsAlive);
			}
			set
			{
				this.m_ConnectionIsDoomed |= !value;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002561 RID: 9569 RVA: 0x00095154 File Offset: 0x00094154
		internal bool IsEmancipated
		{
			get
			{
				WeakReference owner = this.m_Owner;
				return 0 >= this.m_PooledCount && (owner == null || !owner.IsAlive);
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002562 RID: 9570 RVA: 0x00095188 File Offset: 0x00094188
		// (set) Token: 0x06002563 RID: 9571 RVA: 0x000951B0 File Offset: 0x000941B0
		internal object Owner
		{
			get
			{
				WeakReference owner = this.m_Owner;
				if (owner != null && owner.IsAlive)
				{
					return owner.Target;
				}
				return null;
			}
			set
			{
				lock (this)
				{
					if (this.m_Owner != null)
					{
						this.m_Owner.Target = value;
					}
				}
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002564 RID: 9572 RVA: 0x000951F4 File Offset: 0x000941F4
		internal ConnectionPool Pool
		{
			get
			{
				return this.m_ConnectionPool;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002565 RID: 9573 RVA: 0x000951FC File Offset: 0x000941FC
		internal virtual ServicePoint ServicePoint
		{
			get
			{
				return this.Pool.ServicePoint;
			}
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x00095209 File Offset: 0x00094209
		internal bool Activate(object owningObject, GeneralAsyncDelegate asyncCallback)
		{
			return this.Activate(owningObject, asyncCallback != null, -1, asyncCallback);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x0009521C File Offset: 0x0009421C
		protected bool Activate(object owningObject, bool async, int timeout, GeneralAsyncDelegate asyncCallback)
		{
			bool flag;
			try
			{
				if (this.m_Initalizing)
				{
					IPAddress ipaddress = null;
					this.m_AsyncCallback = asyncCallback;
					Socket connection = this.ServicePoint.GetConnection(this, owningObject, async, out ipaddress, ref this.m_AbortSocket, ref this.m_AbortSocket6, timeout);
					if (connection != null)
					{
						this.m_NetworkStream.InitNetworkStream(connection, FileAccess.ReadWrite);
						this.m_ServerAddress = ipaddress;
						this.m_Initalizing = false;
						this.m_JustConnected = true;
						this.m_AbortSocket = null;
						this.m_AbortSocket6 = null;
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					if (async && asyncCallback != null)
					{
						asyncCallback(owningObject, this);
					}
					flag = true;
				}
			}
			catch
			{
				this.m_Initalizing = false;
				throw;
			}
			return flag;
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000952C4 File Offset: 0x000942C4
		internal void Deactivate()
		{
			this.m_AsyncCallback = null;
			if (!this.m_ConnectionIsDoomed && this.m_CheckLifetime)
			{
				this.CheckLifetime();
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000952E4 File Offset: 0x000942E4
		internal virtual void ConnectionCallback(object owningObject, Exception e, Socket socket, IPAddress address)
		{
			object obj = null;
			if (e != null)
			{
				this.m_Initalizing = false;
				obj = e;
			}
			else
			{
				try
				{
					this.m_NetworkStream.InitNetworkStream(socket, FileAccess.ReadWrite);
					obj = this;
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					obj = ex;
				}
				catch
				{
					throw;
				}
				this.m_ServerAddress = address;
				this.m_Initalizing = false;
				this.m_JustConnected = true;
			}
			if (this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(owningObject, obj);
			}
			this.m_AbortSocket = null;
			this.m_AbortSocket6 = null;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x0009537C File Offset: 0x0009437C
		protected void CheckLifetime()
		{
			bool flag = !this.m_ConnectionIsDoomed;
			if (flag)
			{
				TimeSpan timeSpan = DateTime.UtcNow.Subtract(this.m_CreateTime);
				this.m_ConnectionIsDoomed = 0 < TimeSpan.Compare(this.m_Lifetime, timeSpan);
			}
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000953C0 File Offset: 0x000943C0
		internal void UpdateLifetime()
		{
			int connectionLeaseTimeout = this.ServicePoint.ConnectionLeaseTimeout;
			TimeSpan maxValue;
			if (connectionLeaseTimeout == -1)
			{
				maxValue = TimeSpan.MaxValue;
				this.m_CheckLifetime = false;
			}
			else
			{
				maxValue = new TimeSpan(0, 0, 0, 0, connectionLeaseTimeout);
				this.m_CheckLifetime = true;
			}
			if (maxValue != this.m_Lifetime)
			{
				this.m_Lifetime = maxValue;
			}
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x00095414 File Offset: 0x00094414
		internal void Destroy()
		{
			this.m_Owner = null;
			this.m_ConnectionIsDoomed = true;
			this.Close(0);
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x0009542C File Offset: 0x0009442C
		internal void PrePush(object expectedOwner)
		{
			lock (this)
			{
				if (expectedOwner == null)
				{
					if (this.m_Owner != null && this.m_Owner.Target != null)
					{
						throw new InternalException();
					}
				}
				else if (this.m_Owner == null || this.m_Owner.Target != expectedOwner)
				{
					throw new InternalException();
				}
				this.m_PooledCount++;
				if (1 != this.m_PooledCount)
				{
					throw new InternalException();
				}
				if (this.m_Owner != null)
				{
					this.m_Owner.Target = null;
				}
			}
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000954C8 File Offset: 0x000944C8
		internal void PostPop(object newOwner)
		{
			lock (this)
			{
				if (this.m_Owner == null)
				{
					this.m_Owner = new WeakReference(newOwner);
				}
				else
				{
					if (this.m_Owner.Target != null)
					{
						throw new InternalException();
					}
					this.m_Owner.Target = newOwner;
				}
				this.m_PooledCount--;
				if (this.Pool != null)
				{
					if (this.m_PooledCount != 0)
					{
						throw new InternalException();
					}
				}
				else if (-1 != this.m_PooledCount)
				{
					throw new InternalException();
				}
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x00095560 File Offset: 0x00094560
		protected bool UsingSecureStream
		{
			get
			{
				return this.m_NetworkStream is TlsStream;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002570 RID: 9584 RVA: 0x00095570 File Offset: 0x00094570
		// (set) Token: 0x06002571 RID: 9585 RVA: 0x00095578 File Offset: 0x00094578
		internal NetworkStream NetworkStream
		{
			get
			{
				return this.m_NetworkStream;
			}
			set
			{
				this.m_Initalizing = false;
				this.m_NetworkStream = value;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x00095588 File Offset: 0x00094588
		protected Socket Socket
		{
			get
			{
				return this.m_NetworkStream.InternalSocket;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x00095595 File Offset: 0x00094595
		public override bool CanRead
		{
			get
			{
				return this.m_NetworkStream.CanRead;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x000955A2 File Offset: 0x000945A2
		public override bool CanSeek
		{
			get
			{
				return this.m_NetworkStream.CanSeek;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x000955AF File Offset: 0x000945AF
		public override bool CanWrite
		{
			get
			{
				return this.m_NetworkStream.CanWrite;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x000955BC File Offset: 0x000945BC
		public override bool CanTimeout
		{
			get
			{
				return this.m_NetworkStream.CanTimeout;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002577 RID: 9591 RVA: 0x000955C9 File Offset: 0x000945C9
		// (set) Token: 0x06002578 RID: 9592 RVA: 0x000955D6 File Offset: 0x000945D6
		public override int ReadTimeout
		{
			get
			{
				return this.m_NetworkStream.ReadTimeout;
			}
			set
			{
				this.m_NetworkStream.ReadTimeout = value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x000955E4 File Offset: 0x000945E4
		// (set) Token: 0x0600257A RID: 9594 RVA: 0x000955F1 File Offset: 0x000945F1
		public override int WriteTimeout
		{
			get
			{
				return this.m_NetworkStream.WriteTimeout;
			}
			set
			{
				this.m_NetworkStream.WriteTimeout = value;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x000955FF File Offset: 0x000945FF
		public override long Length
		{
			get
			{
				return this.m_NetworkStream.Length;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x0009560C File Offset: 0x0009460C
		// (set) Token: 0x0600257D RID: 9597 RVA: 0x00095619 File Offset: 0x00094619
		public override long Position
		{
			get
			{
				return this.m_NetworkStream.Position;
			}
			set
			{
				this.m_NetworkStream.Position = value;
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00095627 File Offset: 0x00094627
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.m_NetworkStream.Seek(offset, origin);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00095638 File Offset: 0x00094638
		public override int Read(byte[] buffer, int offset, int size)
		{
			return this.m_NetworkStream.Read(buffer, offset, size);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00095655 File Offset: 0x00094655
		public override void Write(byte[] buffer, int offset, int size)
		{
			this.m_NetworkStream.Write(buffer, offset, size);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00095665 File Offset: 0x00094665
		internal void MultipleWrite(BufferOffsetSize[] buffers)
		{
			this.m_NetworkStream.MultipleWrite(buffers);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00095674 File Offset: 0x00094674
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.CloseSocket();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000956A4 File Offset: 0x000946A4
		internal void CloseSocket()
		{
			Socket abortSocket = this.m_AbortSocket;
			Socket abortSocket2 = this.m_AbortSocket6;
			this.m_NetworkStream.Close();
			if (abortSocket != null)
			{
				abortSocket.Close();
			}
			if (abortSocket2 != null)
			{
				abortSocket2.Close();
			}
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000956DC File Offset: 0x000946DC
		public void Close(int timeout)
		{
			Socket abortSocket = this.m_AbortSocket;
			Socket abortSocket2 = this.m_AbortSocket6;
			this.m_NetworkStream.Close(timeout);
			if (abortSocket != null)
			{
				abortSocket.Close(timeout);
			}
			if (abortSocket2 != null)
			{
				abortSocket2.Close(timeout);
			}
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00095717 File Offset: 0x00094717
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x0009572B File Offset: 0x0009472B
		internal virtual IAsyncResult UnsafeBeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.UnsafeBeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x0009573F File Offset: 0x0009473F
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_NetworkStream.EndRead(asyncResult);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0009574D File Offset: 0x0009474D
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginWrite(buffer, offset, size, callback, state);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00095761 File Offset: 0x00094761
		internal virtual IAsyncResult UnsafeBeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.UnsafeBeginWrite(buffer, offset, size, callback, state);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00095775 File Offset: 0x00094775
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_NetworkStream.EndWrite(asyncResult);
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00095783 File Offset: 0x00094783
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		internal IAsyncResult BeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginMultipleWrite(buffers, callback, state);
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00095793 File Offset: 0x00094793
		internal void EndMultipleWrite(IAsyncResult asyncResult)
		{
			this.m_NetworkStream.EndMultipleWrite(asyncResult);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x000957A1 File Offset: 0x000947A1
		public override void Flush()
		{
			this.m_NetworkStream.Flush();
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000957AE File Offset: 0x000947AE
		public override void SetLength(long value)
		{
			this.m_NetworkStream.SetLength(value);
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000957BC File Offset: 0x000947BC
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			this.m_NetworkStream.SetSocketTimeoutOption(mode, timeout, silent);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000957CC File Offset: 0x000947CC
		internal bool Poll(int microSeconds, SelectMode mode)
		{
			return this.m_NetworkStream.Poll(microSeconds, mode);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000957DB File Offset: 0x000947DB
		internal bool PollRead()
		{
			return this.m_NetworkStream.PollRead();
		}

		// Token: 0x0400251B RID: 9499
		private bool m_CheckLifetime;

		// Token: 0x0400251C RID: 9500
		private TimeSpan m_Lifetime;

		// Token: 0x0400251D RID: 9501
		private DateTime m_CreateTime;

		// Token: 0x0400251E RID: 9502
		private bool m_ConnectionIsDoomed;

		// Token: 0x0400251F RID: 9503
		private ConnectionPool m_ConnectionPool;

		// Token: 0x04002520 RID: 9504
		private WeakReference m_Owner;

		// Token: 0x04002521 RID: 9505
		private int m_PooledCount;

		// Token: 0x04002522 RID: 9506
		private bool m_Initalizing;

		// Token: 0x04002523 RID: 9507
		private IPAddress m_ServerAddress;

		// Token: 0x04002524 RID: 9508
		private NetworkStream m_NetworkStream;

		// Token: 0x04002525 RID: 9509
		private Socket m_AbortSocket;

		// Token: 0x04002526 RID: 9510
		private Socket m_AbortSocket6;

		// Token: 0x04002527 RID: 9511
		private bool m_JustConnected;

		// Token: 0x04002528 RID: 9512
		private GeneralAsyncDelegate m_AsyncCallback;
	}
}

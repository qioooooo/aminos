using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000018 RID: 24
	internal class SocketCache
	{
		// Token: 0x06000090 RID: 144 RVA: 0x000044E4 File Offset: 0x000034E4
		static SocketCache()
		{
			SocketCache.InitializeSocketTimeoutHandler();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004508 File Offset: 0x00003508
		internal SocketCache(SocketHandlerFactory handlerFactory, SocketCachePolicy socketCachePolicy, TimeSpan socketTimeout)
		{
			this._handlerFactory = handlerFactory;
			this._socketCachePolicy = socketCachePolicy;
			this._socketTimeout = socketTimeout;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004525 File Offset: 0x00003525
		// (set) Token: 0x06000093 RID: 147 RVA: 0x0000452D File Offset: 0x0000352D
		internal TimeSpan SocketTimeout
		{
			get
			{
				return this._socketTimeout;
			}
			set
			{
				this._socketTimeout = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00004536 File Offset: 0x00003536
		// (set) Token: 0x06000095 RID: 149 RVA: 0x0000453E File Offset: 0x0000353E
		internal int ReceiveTimeout
		{
			get
			{
				return this._receiveTimeout;
			}
			set
			{
				this._receiveTimeout = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004547 File Offset: 0x00003547
		// (set) Token: 0x06000097 RID: 151 RVA: 0x0000454F File Offset: 0x0000354F
		internal SocketCachePolicy CachePolicy
		{
			get
			{
				return this._socketCachePolicy;
			}
			set
			{
				this._socketCachePolicy = value;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004558 File Offset: 0x00003558
		private static void InitializeSocketTimeoutHandler()
		{
			SocketCache._socketTimeoutDelegate = new WaitOrTimerCallback(SocketCache.TimeoutSockets);
			SocketCache._socketTimeoutWaitHandle = new AutoResetEvent(false);
			SocketCache._registeredWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(SocketCache._socketTimeoutWaitHandle, SocketCache._socketTimeoutDelegate, "TcpChannelSocketTimeout", SocketCache._socketTimeoutPollTime, true);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004598 File Offset: 0x00003598
		private static void TimeoutSockets(object state, bool wasSignalled)
		{
			DateTime utcNow = DateTime.UtcNow;
			lock (SocketCache._connections)
			{
				foreach (object obj in SocketCache._connections)
				{
					RemoteConnection remoteConnection = (RemoteConnection)((DictionaryEntry)obj).Value;
					remoteConnection.TimeoutSockets(utcNow);
				}
			}
			SocketCache._registeredWaitHandle.Unregister(null);
			SocketCache._registeredWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(SocketCache._socketTimeoutWaitHandle, SocketCache._socketTimeoutDelegate, "TcpChannelSocketTimeout", SocketCache._socketTimeoutPollTime, true);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004658 File Offset: 0x00003658
		internal SocketHandler CreateSocketHandler(Socket socket, string machineAndPort)
		{
			socket.ReceiveTimeout = this._receiveTimeout;
			return this._handlerFactory(socket, this, machineAndPort);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004674 File Offset: 0x00003674
		public SocketHandler GetSocket(string machinePortAndSid, bool openNew)
		{
			RemoteConnection remoteConnection = (RemoteConnection)SocketCache._connections[machinePortAndSid];
			if (openNew || remoteConnection == null)
			{
				remoteConnection = new RemoteConnection(this, machinePortAndSid);
				lock (SocketCache._connections)
				{
					SocketCache._connections[machinePortAndSid] = remoteConnection;
				}
			}
			return remoteConnection.GetSocket();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000046D8 File Offset: 0x000036D8
		public void ReleaseSocket(string machinePortAndSid, SocketHandler socket)
		{
			RemoteConnection remoteConnection = (RemoteConnection)SocketCache._connections[machinePortAndSid];
			if (remoteConnection != null)
			{
				remoteConnection.ReleaseSocket(socket);
				return;
			}
			socket.Close();
		}

		// Token: 0x04000093 RID: 147
		private static Hashtable _connections = new Hashtable();

		// Token: 0x04000094 RID: 148
		private SocketHandlerFactory _handlerFactory;

		// Token: 0x04000095 RID: 149
		private static RegisteredWaitHandle _registeredWaitHandle;

		// Token: 0x04000096 RID: 150
		private static WaitOrTimerCallback _socketTimeoutDelegate;

		// Token: 0x04000097 RID: 151
		private static AutoResetEvent _socketTimeoutWaitHandle;

		// Token: 0x04000098 RID: 152
		private static TimeSpan _socketTimeoutPollTime = TimeSpan.FromSeconds(10.0);

		// Token: 0x04000099 RID: 153
		private SocketCachePolicy _socketCachePolicy;

		// Token: 0x0400009A RID: 154
		private TimeSpan _socketTimeout;

		// Token: 0x0400009B RID: 155
		private int _receiveTimeout;
	}
}

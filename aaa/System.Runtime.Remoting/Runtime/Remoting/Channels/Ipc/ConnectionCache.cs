using System;
using System.Collections;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x0200005D RID: 93
	internal class ConnectionCache
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0000E502 File Offset: 0x0000D502
		static ConnectionCache()
		{
			ConnectionCache.InitializeConnectionTimeoutHandler();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000E539 File Offset: 0x0000D539
		private static void InitializeConnectionTimeoutHandler()
		{
			ConnectionCache._socketTimeoutDelegate = new WaitOrTimerCallback(ConnectionCache.TimeoutConnections);
			ConnectionCache._socketTimeoutWaitHandle = new AutoResetEvent(false);
			ConnectionCache._registeredWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(ConnectionCache._socketTimeoutWaitHandle, ConnectionCache._socketTimeoutDelegate, "IpcConnectionTimeout", ConnectionCache._socketTimeoutPollTime, true);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000E578 File Offset: 0x0000D578
		private static void TimeoutConnections(object state, bool wasSignalled)
		{
			DateTime utcNow = DateTime.UtcNow;
			lock (ConnectionCache._connections)
			{
				foreach (object obj in ConnectionCache._connections)
				{
					PortConnection portConnection = (PortConnection)((DictionaryEntry)obj).Value;
					if (DateTime.Now - portConnection.LastUsed > ConnectionCache._portLifetime)
					{
						portConnection.Port.Dispose();
					}
				}
			}
			ConnectionCache._registeredWaitHandle.Unregister(null);
			ConnectionCache._registeredWaitHandle = ThreadPool.UnsafeRegisterWaitForSingleObject(ConnectionCache._socketTimeoutWaitHandle, ConnectionCache._socketTimeoutDelegate, "IpcConnectionTimeout", ConnectionCache._socketTimeoutPollTime, true);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000E654 File Offset: 0x0000D654
		public IpcPort GetConnection(string portName, bool secure, TokenImpersonationLevel level, int timeout)
		{
			PortConnection portConnection = null;
			lock (ConnectionCache._connections)
			{
				bool flag = true;
				if (secure)
				{
					try
					{
						WindowsIdentity current = WindowsIdentity.GetCurrent(true);
						if (current != null)
						{
							flag = false;
							current.Dispose();
						}
					}
					catch (Exception)
					{
						flag = false;
					}
				}
				if (flag)
				{
					portConnection = (PortConnection)ConnectionCache._connections[portName];
				}
				if (portConnection == null || portConnection.Port.IsDisposed)
				{
					portConnection = new PortConnection(IpcPort.Connect(portName, secure, level, timeout));
					portConnection.Port.Cacheable = flag;
				}
				else
				{
					ConnectionCache._connections.Remove(portName);
				}
			}
			return portConnection.Port;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000E708 File Offset: 0x0000D708
		public void ReleaseConnection(IpcPort port)
		{
			string name = port.Name;
			PortConnection portConnection = (PortConnection)ConnectionCache._connections[name];
			if (port.Cacheable && (portConnection == null || portConnection.Port.IsDisposed))
			{
				lock (ConnectionCache._connections)
				{
					ConnectionCache._connections[name] = new PortConnection(port);
					return;
				}
			}
			port.Dispose();
		}

		// Token: 0x04000211 RID: 529
		private static Hashtable _connections = new Hashtable();

		// Token: 0x04000212 RID: 530
		private static RegisteredWaitHandle _registeredWaitHandle;

		// Token: 0x04000213 RID: 531
		private static WaitOrTimerCallback _socketTimeoutDelegate;

		// Token: 0x04000214 RID: 532
		private static AutoResetEvent _socketTimeoutWaitHandle;

		// Token: 0x04000215 RID: 533
		private static TimeSpan _socketTimeoutPollTime = TimeSpan.FromSeconds(10.0);

		// Token: 0x04000216 RID: 534
		private static TimeSpan _portLifetime = TimeSpan.FromSeconds(10.0);
	}
}

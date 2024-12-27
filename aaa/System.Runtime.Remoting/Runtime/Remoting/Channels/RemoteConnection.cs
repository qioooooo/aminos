using System;
using System.Net;
using System.Net.Sockets;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000015 RID: 21
	internal class RemoteConnection
	{
		// Token: 0x0600007D RID: 125 RVA: 0x0000403C File Offset: 0x0000303C
		internal RemoteConnection(SocketCache socketCache, string machineAndPort)
		{
			this._socketCache = socketCache;
			this._cachedSocketList = new CachedSocketList(socketCache.SocketTimeout, socketCache.CachePolicy);
			Uri uri = new Uri("dummy://" + machineAndPort);
			this._port = uri.Port;
			this._machineAndPort = machineAndPort;
			this._addressList = Dns.GetHostAddresses(uri.DnsSafeHost);
			this.connectIPv6 = Socket.OSSupportsIPv6 && this.HasIPv6Address(this._addressList);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000040C0 File Offset: 0x000030C0
		internal SocketHandler GetSocket()
		{
			SocketHandler socket = this._cachedSocketList.GetSocket();
			if (socket != null)
			{
				return socket;
			}
			return this.CreateNewSocket();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000040E4 File Offset: 0x000030E4
		internal void ReleaseSocket(SocketHandler socket)
		{
			socket.ReleaseControl();
			this._cachedSocketList.ReturnSocket(socket);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000040F8 File Offset: 0x000030F8
		private bool HasIPv6Address(IPAddress[] addressList)
		{
			foreach (IPAddress ipaddress in addressList)
			{
				if (ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000412A File Offset: 0x0000312A
		private void DisableNagleDelays(Socket socket)
		{
			socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004138 File Offset: 0x00003138
		private SocketHandler CreateNewSocket()
		{
			if (this._addressList.Length == 1)
			{
				return this.CreateNewSocket(new IPEndPoint(this._addressList[0], this._port));
			}
			if (this._lkgIPEndPoint != null)
			{
				try
				{
					return this.CreateNewSocket(this._lkgIPEndPoint);
				}
				catch (Exception)
				{
					this._lkgIPEndPoint = null;
				}
			}
			if (this.connectIPv6)
			{
				try
				{
					return this.CreateNewSocket(AddressFamily.InterNetworkV6);
				}
				catch (Exception)
				{
				}
			}
			return this.CreateNewSocket(AddressFamily.InterNetwork);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000041C8 File Offset: 0x000031C8
		private SocketHandler CreateNewSocket(EndPoint ipEndPoint)
		{
			Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			this.DisableNagleDelays(socket);
			socket.Connect(ipEndPoint);
			this._lkgIPEndPoint = socket.RemoteEndPoint;
			return this._socketCache.CreateSocketHandler(socket, this._machineAndPort);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004210 File Offset: 0x00003210
		private SocketHandler CreateNewSocket(AddressFamily family)
		{
			Socket socket = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
			this.DisableNagleDelays(socket);
			socket.Connect(this._addressList, this._port);
			this._lkgIPEndPoint = socket.RemoteEndPoint;
			return this._socketCache.CreateSocketHandler(socket, this._machineAndPort);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000425D File Offset: 0x0000325D
		internal void TimeoutSockets(DateTime currentTime)
		{
			this._cachedSocketList.TimeoutSockets(currentTime, this._socketCache.SocketTimeout);
		}

		// Token: 0x04000084 RID: 132
		private static char[] colonSep = new char[] { ':' };

		// Token: 0x04000085 RID: 133
		private CachedSocketList _cachedSocketList;

		// Token: 0x04000086 RID: 134
		private SocketCache _socketCache;

		// Token: 0x04000087 RID: 135
		private string _machineAndPort;

		// Token: 0x04000088 RID: 136
		private IPAddress[] _addressList;

		// Token: 0x04000089 RID: 137
		private int _port;

		// Token: 0x0400008A RID: 138
		private EndPoint _lkgIPEndPoint;

		// Token: 0x0400008B RID: 139
		private bool connectIPv6;
	}
}

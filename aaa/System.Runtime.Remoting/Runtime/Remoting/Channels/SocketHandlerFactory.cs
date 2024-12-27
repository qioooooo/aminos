using System;
using System.Net.Sockets;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000014 RID: 20
	// (Invoke) Token: 0x0600007A RID: 122
	internal delegate SocketHandler SocketHandlerFactory(Socket socket, SocketCache socketCache, string machineAndPort);
}

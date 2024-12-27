using System;
using System.Net;
using System.Net.Sockets;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000012 RID: 18
	internal class ExclusiveTcpListener : TcpListener
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00003D16 File Offset: 0x00002D16
		internal ExclusiveTcpListener(IPAddress localaddr, int port)
			: base(localaddr, port)
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003D20 File Offset: 0x00002D20
		internal void Start(bool exclusiveAddressUse)
		{
			bool flag = exclusiveAddressUse && Environment.OSVersion.Platform == PlatformID.Win32NT && base.Server != null && !base.Active;
			if (flag)
			{
				base.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, 1);
			}
			try
			{
				base.Start();
			}
			catch (SocketException)
			{
				if (!flag)
				{
					throw;
				}
				base.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, 0);
				base.Start();
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003DA4 File Offset: 0x00002DA4
		internal bool IsListening
		{
			get
			{
				return base.Active;
			}
		}
	}
}

using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000324 RID: 804
	internal class RoutingInfo
	{
		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x0029CBD8 File Offset: 0x0029BFD8
		internal byte Protocol
		{
			get
			{
				return this._protocol;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x0029CBEC File Offset: 0x0029BFEC
		internal ushort Port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x0029CC00 File Offset: 0x0029C000
		internal string ServerName
		{
			get
			{
				return this._serverName;
			}
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x0029CC14 File Offset: 0x0029C014
		internal RoutingInfo(byte protocol, ushort port, string servername)
		{
			this._protocol = protocol;
			this._port = port;
			this._serverName = servername;
		}

		// Token: 0x04001B8A RID: 7050
		private byte _protocol;

		// Token: 0x04001B8B RID: 7051
		private ushort _port;

		// Token: 0x04001B8C RID: 7052
		private string _serverName;
	}
}

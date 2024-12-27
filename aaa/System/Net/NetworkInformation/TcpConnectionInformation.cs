using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063A RID: 1594
	public abstract class TcpConnectionInformation
	{
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600315D RID: 12637
		public abstract IPEndPoint LocalEndPoint { get; }

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x0600315E RID: 12638
		public abstract IPEndPoint RemoteEndPoint { get; }

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x0600315F RID: 12639
		public abstract TcpState State { get; }
	}
}

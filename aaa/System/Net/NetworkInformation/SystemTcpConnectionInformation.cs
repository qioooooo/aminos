using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200063B RID: 1595
	internal class SystemTcpConnectionInformation : TcpConnectionInformation
	{
		// Token: 0x06003161 RID: 12641 RVA: 0x000D4280 File Offset: 0x000D3280
		internal SystemTcpConnectionInformation(MibTcpRow row)
		{
			this.state = row.state;
			int num = ((int)row.localPort3 << 24) | ((int)row.localPort4 << 16) | ((int)row.localPort1 << 8) | (int)row.localPort2;
			int num2 = ((this.state == TcpState.Listen) ? 0 : (((int)row.remotePort3 << 24) | ((int)row.remotePort4 << 16) | ((int)row.remotePort1 << 8) | (int)row.remotePort2));
			this.localEndPoint = new IPEndPoint((long)((ulong)row.localAddr), num);
			this.remoteEndPoint = new IPEndPoint((long)((ulong)row.remoteAddr), num2);
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x000D4324 File Offset: 0x000D3324
		public override TcpState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003163 RID: 12643 RVA: 0x000D432C File Offset: 0x000D332C
		public override IPEndPoint LocalEndPoint
		{
			get
			{
				return this.localEndPoint;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003164 RID: 12644 RVA: 0x000D4334 File Offset: 0x000D3334
		public override IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.remoteEndPoint;
			}
		}

		// Token: 0x04002E7F RID: 11903
		private IPEndPoint localEndPoint;

		// Token: 0x04002E80 RID: 11904
		private IPEndPoint remoteEndPoint;

		// Token: 0x04002E81 RID: 11905
		private TcpState state;
	}
}

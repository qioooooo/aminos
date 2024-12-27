using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000607 RID: 1543
	internal struct MibTcpRow
	{
		// Token: 0x04002D98 RID: 11672
		internal TcpState state;

		// Token: 0x04002D99 RID: 11673
		internal uint localAddr;

		// Token: 0x04002D9A RID: 11674
		internal byte localPort1;

		// Token: 0x04002D9B RID: 11675
		internal byte localPort2;

		// Token: 0x04002D9C RID: 11676
		internal byte localPort3;

		// Token: 0x04002D9D RID: 11677
		internal byte localPort4;

		// Token: 0x04002D9E RID: 11678
		internal uint remoteAddr;

		// Token: 0x04002D9F RID: 11679
		internal byte remotePort1;

		// Token: 0x04002DA0 RID: 11680
		internal byte remotePort2;

		// Token: 0x04002DA1 RID: 11681
		internal byte remotePort3;

		// Token: 0x04002DA2 RID: 11682
		internal byte remotePort4;
	}
}

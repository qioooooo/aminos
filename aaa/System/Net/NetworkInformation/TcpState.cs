using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000640 RID: 1600
	public enum TcpState
	{
		// Token: 0x04002E85 RID: 11909
		Unknown,
		// Token: 0x04002E86 RID: 11910
		Closed,
		// Token: 0x04002E87 RID: 11911
		Listen,
		// Token: 0x04002E88 RID: 11912
		SynSent,
		// Token: 0x04002E89 RID: 11913
		SynReceived,
		// Token: 0x04002E8A RID: 11914
		Established,
		// Token: 0x04002E8B RID: 11915
		FinWait1,
		// Token: 0x04002E8C RID: 11916
		FinWait2,
		// Token: 0x04002E8D RID: 11917
		CloseWait,
		// Token: 0x04002E8E RID: 11918
		Closing,
		// Token: 0x04002E8F RID: 11919
		LastAck,
		// Token: 0x04002E90 RID: 11920
		TimeWait,
		// Token: 0x04002E91 RID: 11921
		DeleteTcb
	}
}

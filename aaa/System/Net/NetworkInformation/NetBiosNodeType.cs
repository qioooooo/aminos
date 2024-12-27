using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200061E RID: 1566
	public enum NetBiosNodeType
	{
		// Token: 0x04002DDF RID: 11743
		Unknown,
		// Token: 0x04002DE0 RID: 11744
		Broadcast,
		// Token: 0x04002DE1 RID: 11745
		Peer2Peer,
		// Token: 0x04002DE2 RID: 11746
		Mixed = 4,
		// Token: 0x04002DE3 RID: 11747
		Hybrid = 8
	}
}

using System;

namespace System.Management
{
	// Token: 0x0200002A RID: 42
	public enum AuthenticationLevel
	{
		// Token: 0x04000125 RID: 293
		Default,
		// Token: 0x04000126 RID: 294
		None,
		// Token: 0x04000127 RID: 295
		Connect,
		// Token: 0x04000128 RID: 296
		Call,
		// Token: 0x04000129 RID: 297
		Packet,
		// Token: 0x0400012A RID: 298
		PacketIntegrity,
		// Token: 0x0400012B RID: 299
		PacketPrivacy,
		// Token: 0x0400012C RID: 300
		Unchanged = -1
	}
}

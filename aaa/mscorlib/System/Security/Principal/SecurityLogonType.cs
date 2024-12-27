using System;

namespace System.Security.Principal
{
	// Token: 0x020004B9 RID: 1209
	[Serializable]
	internal enum SecurityLogonType
	{
		// Token: 0x04001863 RID: 6243
		Interactive = 2,
		// Token: 0x04001864 RID: 6244
		Network,
		// Token: 0x04001865 RID: 6245
		Batch,
		// Token: 0x04001866 RID: 6246
		Service,
		// Token: 0x04001867 RID: 6247
		Proxy,
		// Token: 0x04001868 RID: 6248
		Unlock
	}
}

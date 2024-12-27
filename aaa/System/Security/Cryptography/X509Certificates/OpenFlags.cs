using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000344 RID: 836
	[Flags]
	public enum OpenFlags
	{
		// Token: 0x04001B28 RID: 6952
		ReadOnly = 0,
		// Token: 0x04001B29 RID: 6953
		ReadWrite = 1,
		// Token: 0x04001B2A RID: 6954
		MaxAllowed = 2,
		// Token: 0x04001B2B RID: 6955
		OpenExistingOnly = 4,
		// Token: 0x04001B2C RID: 6956
		IncludeArchived = 8
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004BD RID: 1213
	[ComVisible(true)]
	[Serializable]
	public enum WindowsBuiltInRole
	{
		// Token: 0x04001880 RID: 6272
		Administrator = 544,
		// Token: 0x04001881 RID: 6273
		User,
		// Token: 0x04001882 RID: 6274
		Guest,
		// Token: 0x04001883 RID: 6275
		PowerUser,
		// Token: 0x04001884 RID: 6276
		AccountOperator,
		// Token: 0x04001885 RID: 6277
		SystemOperator,
		// Token: 0x04001886 RID: 6278
		PrintOperator,
		// Token: 0x04001887 RID: 6279
		BackupOperator,
		// Token: 0x04001888 RID: 6280
		Replicator
	}
}

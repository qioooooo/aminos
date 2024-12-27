using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000E3 RID: 227
	[ComVisible(false)]
	[Serializable]
	public enum TransactionStatus
	{
		// Token: 0x04000215 RID: 533
		Commited,
		// Token: 0x04000216 RID: 534
		LocallyOk,
		// Token: 0x04000217 RID: 535
		NoTransaction,
		// Token: 0x04000218 RID: 536
		Aborting,
		// Token: 0x04000219 RID: 537
		Aborted
	}
}

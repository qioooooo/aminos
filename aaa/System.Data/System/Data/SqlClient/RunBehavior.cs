using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000321 RID: 801
	internal enum RunBehavior
	{
		// Token: 0x04001B78 RID: 7032
		UntilDone = 1,
		// Token: 0x04001B79 RID: 7033
		ReturnImmediately,
		// Token: 0x04001B7A RID: 7034
		Clean = 5,
		// Token: 0x04001B7B RID: 7035
		Attention = 13
	}
}

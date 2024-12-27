using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000303 RID: 771
	public enum SqlNotificationSource
	{
		// Token: 0x04001941 RID: 6465
		Data,
		// Token: 0x04001942 RID: 6466
		Timeout,
		// Token: 0x04001943 RID: 6467
		Object,
		// Token: 0x04001944 RID: 6468
		Database,
		// Token: 0x04001945 RID: 6469
		System,
		// Token: 0x04001946 RID: 6470
		Statement,
		// Token: 0x04001947 RID: 6471
		Environment,
		// Token: 0x04001948 RID: 6472
		Execution,
		// Token: 0x04001949 RID: 6473
		Owner,
		// Token: 0x0400194A RID: 6474
		Unknown = -1,
		// Token: 0x0400194B RID: 6475
		Client = -2
	}
}

using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000302 RID: 770
	public enum SqlNotificationInfo
	{
		// Token: 0x0400192D RID: 6445
		Truncate,
		// Token: 0x0400192E RID: 6446
		Insert,
		// Token: 0x0400192F RID: 6447
		Update,
		// Token: 0x04001930 RID: 6448
		Delete,
		// Token: 0x04001931 RID: 6449
		Drop,
		// Token: 0x04001932 RID: 6450
		Alter,
		// Token: 0x04001933 RID: 6451
		Restart,
		// Token: 0x04001934 RID: 6452
		Error,
		// Token: 0x04001935 RID: 6453
		Query,
		// Token: 0x04001936 RID: 6454
		Invalid,
		// Token: 0x04001937 RID: 6455
		Options,
		// Token: 0x04001938 RID: 6456
		Isolation,
		// Token: 0x04001939 RID: 6457
		Expired,
		// Token: 0x0400193A RID: 6458
		Resource,
		// Token: 0x0400193B RID: 6459
		PreviousFire,
		// Token: 0x0400193C RID: 6460
		TemplateLimit,
		// Token: 0x0400193D RID: 6461
		Merge,
		// Token: 0x0400193E RID: 6462
		Unknown = -1,
		// Token: 0x0400193F RID: 6463
		AlreadyChanged = -2
	}
}

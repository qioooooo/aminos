using System;

namespace System.Diagnostics
{
	// Token: 0x02000754 RID: 1876
	public enum EventLogEntryType
	{
		// Token: 0x040032B2 RID: 12978
		Error = 1,
		// Token: 0x040032B3 RID: 12979
		Warning,
		// Token: 0x040032B4 RID: 12980
		Information = 4,
		// Token: 0x040032B5 RID: 12981
		SuccessAudit = 8,
		// Token: 0x040032B6 RID: 12982
		FailureAudit = 16
	}
}

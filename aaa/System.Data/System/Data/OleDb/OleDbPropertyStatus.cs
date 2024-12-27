using System;

namespace System.Data.OleDb
{
	// Token: 0x02000238 RID: 568
	internal enum OleDbPropertyStatus
	{
		// Token: 0x04001464 RID: 5220
		Ok,
		// Token: 0x04001465 RID: 5221
		NotSupported,
		// Token: 0x04001466 RID: 5222
		BadValue,
		// Token: 0x04001467 RID: 5223
		BadOption,
		// Token: 0x04001468 RID: 5224
		BadColumn,
		// Token: 0x04001469 RID: 5225
		NotAllSettable,
		// Token: 0x0400146A RID: 5226
		NotSettable,
		// Token: 0x0400146B RID: 5227
		NotSet,
		// Token: 0x0400146C RID: 5228
		Conflicting,
		// Token: 0x0400146D RID: 5229
		NotAvailable
	}
}

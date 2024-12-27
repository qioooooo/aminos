using System;

namespace System.EnterpriseServices.Admin
{
	// Token: 0x02000053 RID: 83
	[Serializable]
	internal enum ServiceStatusOptions
	{
		// Token: 0x040000AC RID: 172
		Stopped,
		// Token: 0x040000AD RID: 173
		StartPending,
		// Token: 0x040000AE RID: 174
		StopPending,
		// Token: 0x040000AF RID: 175
		Running,
		// Token: 0x040000B0 RID: 176
		ContinuePending,
		// Token: 0x040000B1 RID: 177
		PausePending,
		// Token: 0x040000B2 RID: 178
		Paused,
		// Token: 0x040000B3 RID: 179
		UnknownState
	}
}

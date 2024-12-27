using System;

namespace System.Diagnostics
{
	// Token: 0x0200076F RID: 1903
	[Flags]
	public enum PerformanceCounterPermissionAccess
	{
		// Token: 0x04003345 RID: 13125
		[Obsolete("This member has been deprecated.  Use System.Diagnostics.PerformanceCounter.PerformanceCounterPermissionAccess.Read instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Browse = 1,
		// Token: 0x04003346 RID: 13126
		[Obsolete("This member has been deprecated.  Use System.Diagnostics.PerformanceCounter.PerformanceCounterPermissionAccess.Write instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Instrument = 3,
		// Token: 0x04003347 RID: 13127
		None = 0,
		// Token: 0x04003348 RID: 13128
		Read = 1,
		// Token: 0x04003349 RID: 13129
		Write = 2,
		// Token: 0x0400334A RID: 13130
		Administer = 7
	}
}

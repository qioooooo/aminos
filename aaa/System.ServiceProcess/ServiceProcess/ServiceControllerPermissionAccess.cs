using System;

namespace System.ServiceProcess
{
	// Token: 0x02000029 RID: 41
	[Flags]
	public enum ServiceControllerPermissionAccess
	{
		// Token: 0x04000204 RID: 516
		None = 0,
		// Token: 0x04000205 RID: 517
		Browse = 2,
		// Token: 0x04000206 RID: 518
		Control = 6
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x02000619 RID: 1561
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum HostProtectionResource
	{
		// Token: 0x04001D52 RID: 7506
		None = 0,
		// Token: 0x04001D53 RID: 7507
		Synchronization = 1,
		// Token: 0x04001D54 RID: 7508
		SharedState = 2,
		// Token: 0x04001D55 RID: 7509
		ExternalProcessMgmt = 4,
		// Token: 0x04001D56 RID: 7510
		SelfAffectingProcessMgmt = 8,
		// Token: 0x04001D57 RID: 7511
		ExternalThreading = 16,
		// Token: 0x04001D58 RID: 7512
		SelfAffectingThreading = 32,
		// Token: 0x04001D59 RID: 7513
		SecurityInfrastructure = 64,
		// Token: 0x04001D5A RID: 7514
		UI = 128,
		// Token: 0x04001D5B RID: 7515
		MayLeakOnAbort = 256,
		// Token: 0x04001D5C RID: 7516
		All = 511
	}
}

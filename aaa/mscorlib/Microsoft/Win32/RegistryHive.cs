using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x0200045D RID: 1117
	[ComVisible(true)]
	[Serializable]
	public enum RegistryHive
	{
		// Token: 0x0400171C RID: 5916
		ClassesRoot = -2147483648,
		// Token: 0x0400171D RID: 5917
		CurrentUser,
		// Token: 0x0400171E RID: 5918
		LocalMachine,
		// Token: 0x0400171F RID: 5919
		Users,
		// Token: 0x04001720 RID: 5920
		PerformanceData,
		// Token: 0x04001721 RID: 5921
		CurrentConfig,
		// Token: 0x04001722 RID: 5922
		DynData
	}
}

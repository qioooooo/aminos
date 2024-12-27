using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x0200065C RID: 1628
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum HostSecurityManagerOptions
	{
		// Token: 0x04001E68 RID: 7784
		None = 0,
		// Token: 0x04001E69 RID: 7785
		HostAppDomainEvidence = 1,
		// Token: 0x04001E6A RID: 7786
		HostPolicyLevel = 2,
		// Token: 0x04001E6B RID: 7787
		HostAssemblyEvidence = 4,
		// Token: 0x04001E6C RID: 7788
		HostDetermineApplicationTrust = 8,
		// Token: 0x04001E6D RID: 7789
		HostResolvePolicy = 16,
		// Token: 0x04001E6E RID: 7790
		AllFlags = 31
	}
}

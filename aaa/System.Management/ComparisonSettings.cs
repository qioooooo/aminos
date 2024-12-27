using System;

namespace System.Management
{
	// Token: 0x02000007 RID: 7
	[Flags]
	public enum ComparisonSettings
	{
		// Token: 0x04000062 RID: 98
		IncludeAll = 0,
		// Token: 0x04000063 RID: 99
		IgnoreQualifiers = 1,
		// Token: 0x04000064 RID: 100
		IgnoreObjectSource = 2,
		// Token: 0x04000065 RID: 101
		IgnoreDefaultValues = 4,
		// Token: 0x04000066 RID: 102
		IgnoreClass = 8,
		// Token: 0x04000067 RID: 103
		IgnoreCase = 16,
		// Token: 0x04000068 RID: 104
		IgnoreFlavor = 32
	}
}

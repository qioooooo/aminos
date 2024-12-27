using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000097 RID: 151
	[Flags]
	public enum SyncFromAllServersOptions
	{
		// Token: 0x04000414 RID: 1044
		None = 0,
		// Token: 0x04000415 RID: 1045
		AbortIfServerUnavailable = 1,
		// Token: 0x04000416 RID: 1046
		SyncAdjacentServerOnly = 2,
		// Token: 0x04000417 RID: 1047
		CheckServerAlivenessOnly = 8,
		// Token: 0x04000418 RID: 1048
		SkipInitialCheck = 16,
		// Token: 0x04000419 RID: 1049
		PushChangeOutward = 32,
		// Token: 0x0400041A RID: 1050
		CrossSite = 64
	}
}

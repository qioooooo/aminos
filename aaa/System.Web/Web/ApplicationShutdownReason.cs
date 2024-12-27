using System;

namespace System.Web
{
	// Token: 0x0200008A RID: 138
	public enum ApplicationShutdownReason
	{
		// Token: 0x04001139 RID: 4409
		None,
		// Token: 0x0400113A RID: 4410
		HostingEnvironment,
		// Token: 0x0400113B RID: 4411
		ChangeInGlobalAsax,
		// Token: 0x0400113C RID: 4412
		ConfigurationChange,
		// Token: 0x0400113D RID: 4413
		UnloadAppDomainCalled,
		// Token: 0x0400113E RID: 4414
		ChangeInSecurityPolicyFile,
		// Token: 0x0400113F RID: 4415
		BinDirChangeOrDirectoryRename,
		// Token: 0x04001140 RID: 4416
		BrowsersDirChangeOrDirectoryRename,
		// Token: 0x04001141 RID: 4417
		CodeDirChangeOrDirectoryRename,
		// Token: 0x04001142 RID: 4418
		ResourcesDirChangeOrDirectoryRename,
		// Token: 0x04001143 RID: 4419
		IdleTimeout,
		// Token: 0x04001144 RID: 4420
		PhysicalApplicationPathChanged,
		// Token: 0x04001145 RID: 4421
		HttpRuntimeClose,
		// Token: 0x04001146 RID: 4422
		InitializationError,
		// Token: 0x04001147 RID: 4423
		MaxRecompilationsReached,
		// Token: 0x04001148 RID: 4424
		BuildManagerChange
	}
}

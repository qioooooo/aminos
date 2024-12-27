﻿using System;

namespace System.Security.Principal
{
	// Token: 0x02000931 RID: 2353
	[Flags]
	internal enum PolicyRights
	{
		// Token: 0x04002C76 RID: 11382
		POLICY_VIEW_LOCAL_INFORMATION = 1,
		// Token: 0x04002C77 RID: 11383
		POLICY_VIEW_AUDIT_INFORMATION = 2,
		// Token: 0x04002C78 RID: 11384
		POLICY_GET_PRIVATE_INFORMATION = 4,
		// Token: 0x04002C79 RID: 11385
		POLICY_TRUST_ADMIN = 8,
		// Token: 0x04002C7A RID: 11386
		POLICY_CREATE_ACCOUNT = 16,
		// Token: 0x04002C7B RID: 11387
		POLICY_CREATE_SECRET = 32,
		// Token: 0x04002C7C RID: 11388
		POLICY_CREATE_PRIVILEGE = 64,
		// Token: 0x04002C7D RID: 11389
		POLICY_SET_DEFAULT_QUOTA_LIMITS = 128,
		// Token: 0x04002C7E RID: 11390
		POLICY_SET_AUDIT_REQUIREMENTS = 256,
		// Token: 0x04002C7F RID: 11391
		POLICY_AUDIT_LOG_ADMIN = 512,
		// Token: 0x04002C80 RID: 11392
		POLICY_SERVER_ADMIN = 1024,
		// Token: 0x04002C81 RID: 11393
		POLICY_LOOKUP_NAMES = 2048,
		// Token: 0x04002C82 RID: 11394
		POLICY_NOTIFICATION = 4096
	}
}
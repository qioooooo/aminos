using System;

namespace System.Security.Principal
{
	// Token: 0x020004BB RID: 1211
	[Serializable]
	internal enum TokenInformationClass
	{
		// Token: 0x0400186D RID: 6253
		TokenUser = 1,
		// Token: 0x0400186E RID: 6254
		TokenGroups,
		// Token: 0x0400186F RID: 6255
		TokenPrivileges,
		// Token: 0x04001870 RID: 6256
		TokenOwner,
		// Token: 0x04001871 RID: 6257
		TokenPrimaryGroup,
		// Token: 0x04001872 RID: 6258
		TokenDefaultDacl,
		// Token: 0x04001873 RID: 6259
		TokenSource,
		// Token: 0x04001874 RID: 6260
		TokenType,
		// Token: 0x04001875 RID: 6261
		TokenImpersonationLevel,
		// Token: 0x04001876 RID: 6262
		TokenStatistics,
		// Token: 0x04001877 RID: 6263
		TokenRestrictedSids,
		// Token: 0x04001878 RID: 6264
		TokenSessionId,
		// Token: 0x04001879 RID: 6265
		TokenGroupsAndPrivileges,
		// Token: 0x0400187A RID: 6266
		TokenSessionReference,
		// Token: 0x0400187B RID: 6267
		TokenSandBoxInert
	}
}

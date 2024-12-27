using System;

namespace System.Web.Management
{
	// Token: 0x020002CD RID: 717
	[Flags]
	public enum SqlFeatures
	{
		// Token: 0x04001C80 RID: 7296
		None = 0,
		// Token: 0x04001C81 RID: 7297
		Membership = 1,
		// Token: 0x04001C82 RID: 7298
		Profile = 2,
		// Token: 0x04001C83 RID: 7299
		RoleManager = 4,
		// Token: 0x04001C84 RID: 7300
		Personalization = 8,
		// Token: 0x04001C85 RID: 7301
		SqlWebEventProvider = 16,
		// Token: 0x04001C86 RID: 7302
		All = 1073741855
	}
}

using System;

namespace System.Web.Security
{
	// Token: 0x02000341 RID: 833
	public enum MembershipCreateStatus
	{
		// Token: 0x04001EC0 RID: 7872
		Success,
		// Token: 0x04001EC1 RID: 7873
		InvalidUserName,
		// Token: 0x04001EC2 RID: 7874
		InvalidPassword,
		// Token: 0x04001EC3 RID: 7875
		InvalidQuestion,
		// Token: 0x04001EC4 RID: 7876
		InvalidAnswer,
		// Token: 0x04001EC5 RID: 7877
		InvalidEmail,
		// Token: 0x04001EC6 RID: 7878
		DuplicateUserName,
		// Token: 0x04001EC7 RID: 7879
		DuplicateEmail,
		// Token: 0x04001EC8 RID: 7880
		UserRejected,
		// Token: 0x04001EC9 RID: 7881
		InvalidProviderUserKey,
		// Token: 0x04001ECA RID: 7882
		DuplicateProviderUserKey,
		// Token: 0x04001ECB RID: 7883
		ProviderError
	}
}

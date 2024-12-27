using System;

namespace System.Web
{
	// Token: 0x02000057 RID: 87
	public enum HttpCacheability
	{
		// Token: 0x04000F58 RID: 3928
		NoCache = 1,
		// Token: 0x04000F59 RID: 3929
		Private,
		// Token: 0x04000F5A RID: 3930
		Server,
		// Token: 0x04000F5B RID: 3931
		ServerAndNoCache = 3,
		// Token: 0x04000F5C RID: 3932
		Public,
		// Token: 0x04000F5D RID: 3933
		ServerAndPrivate
	}
}

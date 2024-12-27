using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200006E RID: 110
	internal class LdapRequestState
	{
		// Token: 0x04000225 RID: 549
		internal DirectoryResponse response;

		// Token: 0x04000226 RID: 550
		internal LdapAsyncResult ldapAsync;

		// Token: 0x04000227 RID: 551
		internal Exception exception;

		// Token: 0x04000228 RID: 552
		internal bool abortCalled;
	}
}

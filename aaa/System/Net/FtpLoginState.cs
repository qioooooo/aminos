using System;

namespace System.Net
{
	// Token: 0x020004DB RID: 1243
	internal enum FtpLoginState : byte
	{
		// Token: 0x04002634 RID: 9780
		NotLoggedIn,
		// Token: 0x04002635 RID: 9781
		LoggedIn,
		// Token: 0x04002636 RID: 9782
		LoggedInButNeedsRelogin,
		// Token: 0x04002637 RID: 9783
		ReloginFailed
	}
}

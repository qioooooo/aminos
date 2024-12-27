using System;

namespace System.Net
{
	// Token: 0x0200037C RID: 892
	[Flags]
	public enum AuthenticationSchemes
	{
		// Token: 0x04001C86 RID: 7302
		None = 0,
		// Token: 0x04001C87 RID: 7303
		Digest = 1,
		// Token: 0x04001C88 RID: 7304
		Negotiate = 2,
		// Token: 0x04001C89 RID: 7305
		Ntlm = 4,
		// Token: 0x04001C8A RID: 7306
		Basic = 8,
		// Token: 0x04001C8B RID: 7307
		Anonymous = 32768,
		// Token: 0x04001C8C RID: 7308
		IntegratedWindowsAuthentication = 6
	}
}

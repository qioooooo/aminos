using System;

namespace System.Net
{
	// Token: 0x020003E8 RID: 1000
	public interface IAuthenticationModule
	{
		// Token: 0x0600206F RID: 8303
		Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials);

		// Token: 0x06002070 RID: 8304
		Authorization PreAuthenticate(WebRequest request, ICredentials credentials);

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002071 RID: 8305
		bool CanPreAuthenticate { get; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002072 RID: 8306
		string AuthenticationType { get; }
	}
}

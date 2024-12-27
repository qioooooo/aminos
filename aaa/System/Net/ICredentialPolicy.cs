using System;

namespace System.Net
{
	// Token: 0x02000376 RID: 886
	public interface ICredentialPolicy
	{
		// Token: 0x06001BBC RID: 7100
		bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authenticationModule);
	}
}

using System;

namespace System.Net
{
	// Token: 0x0200039A RID: 922
	public interface ICredentialsByHost
	{
		// Token: 0x06001CC1 RID: 7361
		NetworkCredential GetCredential(string host, int port, string authenticationType);
	}
}

using System;

namespace System.Net
{
	// Token: 0x02000399 RID: 921
	public interface ICredentials
	{
		// Token: 0x06001CC0 RID: 7360
		NetworkCredential GetCredential(Uri uri, string authType);
	}
}

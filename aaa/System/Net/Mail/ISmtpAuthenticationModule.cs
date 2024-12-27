using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net.Mail
{
	// Token: 0x02000696 RID: 1686
	internal interface ISmtpAuthenticationModule
	{
		// Token: 0x06003400 RID: 13312
		Authorization Authenticate(string challenge, NetworkCredential credentials, object sessionCookie, string spn, ChannelBinding channelBindingToken);

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003401 RID: 13313
		string AuthenticationType { get; }

		// Token: 0x06003402 RID: 13314
		void CloseContext(object sessionCookie);
	}
}

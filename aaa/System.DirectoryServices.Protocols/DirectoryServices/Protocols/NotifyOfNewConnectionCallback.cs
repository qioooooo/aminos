using System;
using System.Net;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200007E RID: 126
	// (Invoke) Token: 0x060002A7 RID: 679
	public delegate bool NotifyOfNewConnectionCallback(LdapConnection primaryConnection, LdapConnection referralFromConnection, string newDistinguishedName, LdapDirectoryIdentifier identifier, LdapConnection newConnection, NetworkCredential credential, long currentUserToken, int errorCodeFromBind);
}

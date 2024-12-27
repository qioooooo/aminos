using System;
using System.Net;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200007D RID: 125
	// (Invoke) Token: 0x060002A3 RID: 675
	public delegate LdapConnection QueryForConnectionCallback(LdapConnection primaryConnection, LdapConnection referralFromConnection, string newDistinguishedName, LdapDirectoryIdentifier identifier, NetworkCredential credential, long currentUserToken);
}

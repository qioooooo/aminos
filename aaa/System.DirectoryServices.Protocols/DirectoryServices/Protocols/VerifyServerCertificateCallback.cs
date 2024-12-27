using System;
using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000081 RID: 129
	// (Invoke) Token: 0x060002B3 RID: 691
	public delegate bool VerifyServerCertificateCallback(LdapConnection connection, X509Certificate certificate);
}

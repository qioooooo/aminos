using System;
using System.Security.Cryptography.X509Certificates;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000080 RID: 128
	// (Invoke) Token: 0x060002AF RID: 687
	public delegate X509Certificate QueryClientCertificateCallback(LdapConnection connection, byte[][] trustedCAs);
}

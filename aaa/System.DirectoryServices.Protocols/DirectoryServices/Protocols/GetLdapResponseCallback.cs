using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000071 RID: 113
	// (Invoke) Token: 0x06000244 RID: 580
	internal delegate DirectoryResponse GetLdapResponseCallback(int messageId, LdapOperation operation, ResultAll resultType, TimeSpan requestTimeout, bool exceptionOnTimeOut);
}

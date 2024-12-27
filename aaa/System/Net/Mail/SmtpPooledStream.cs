using System;

namespace System.Net.Mail
{
	// Token: 0x020006DB RID: 1755
	internal class SmtpPooledStream : PooledStream
	{
		// Token: 0x06003616 RID: 13846 RVA: 0x000E6E3C File Offset: 0x000E5E3C
		internal SmtpPooledStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
			: base(connectionPool, lifetime, checkLifetime)
		{
		}

		// Token: 0x04003150 RID: 12624
		internal bool previouslyUsed;

		// Token: 0x04003151 RID: 12625
		internal bool dsnEnabled;

		// Token: 0x04003152 RID: 12626
		internal ICredentialsByHost creds;
	}
}

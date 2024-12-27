using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000070 RID: 112
	internal class LdapPartialAsyncResult : LdapAsyncResult
	{
		// Token: 0x06000242 RID: 578 RVA: 0x0000A6F4 File Offset: 0x000096F4
		public LdapPartialAsyncResult(int messageID, AsyncCallback callbackRoutine, object state, bool partialResults, LdapConnection con, bool partialCallback, TimeSpan requestTimeout)
			: base(callbackRoutine, state, partialResults)
		{
			this.messageID = messageID;
			this.con = con;
			this.partialResults = true;
			this.partialCallback = partialCallback;
			this.requestTimeout = requestTimeout;
			this.startTime = DateTime.Now;
		}

		// Token: 0x0400022D RID: 557
		internal LdapConnection con;

		// Token: 0x0400022E RID: 558
		internal int messageID = -1;

		// Token: 0x0400022F RID: 559
		internal bool partialCallback;

		// Token: 0x04000230 RID: 560
		internal ResultsStatus resultStatus;

		// Token: 0x04000231 RID: 561
		internal TimeSpan requestTimeout;

		// Token: 0x04000232 RID: 562
		internal SearchResponse response;

		// Token: 0x04000233 RID: 563
		internal Exception exception;

		// Token: 0x04000234 RID: 564
		internal DateTime startTime;
	}
}

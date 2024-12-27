using System;

namespace System.Web
{
	// Token: 0x02000096 RID: 150
	internal interface IHttpResponseElement
	{
		// Token: 0x060007C5 RID: 1989
		long GetSize();

		// Token: 0x060007C6 RID: 1990
		byte[] GetBytes();

		// Token: 0x060007C7 RID: 1991
		void Send(HttpWorkerRequest wr);
	}
}

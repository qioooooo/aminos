using System;

namespace System.Web
{
	// Token: 0x0200007B RID: 123
	internal class HttpForbiddenHandler : IHttpHandler
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x00015401 File Offset: 0x00014401
		internal HttpForbiddenHandler()
		{
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001540C File Offset: 0x0001440C
		public void ProcessRequest(HttpContext context)
		{
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_NOT_FOUND);
			throw new HttpException(403, SR.GetString("Path_forbidden", new object[] { context.Request.Path }));
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001544A File Offset: 0x0001444A
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}

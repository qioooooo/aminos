using System;

namespace System.Web
{
	// Token: 0x0200007A RID: 122
	internal class HttpNotFoundHandler : IHttpHandler
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x000153B7 File Offset: 0x000143B7
		internal HttpNotFoundHandler()
		{
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000153C0 File Offset: 0x000143C0
		public void ProcessRequest(HttpContext context)
		{
			PerfCounters.IncrementCounter(AppPerfCounter.REQUESTS_NOT_FOUND);
			throw new HttpException(404, SR.GetString("Path_not_found", new object[] { context.Request.Path }));
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x000153FE File Offset: 0x000143FE
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}

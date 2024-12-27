using System;

namespace System.Web
{
	// Token: 0x0200007C RID: 124
	internal class HttpMethodNotAllowedHandler : IHttpHandler
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x0001544D File Offset: 0x0001444D
		internal HttpMethodNotAllowedHandler()
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00015458 File Offset: 0x00014458
		public void ProcessRequest(HttpContext context)
		{
			throw new HttpException(405, SR.GetString("Path_forbidden", new object[] { context.Request.HttpMethod }));
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001548F File Offset: 0x0001448F
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}

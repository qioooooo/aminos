using System;

namespace System.Web
{
	// Token: 0x0200007D RID: 125
	internal class HttpNotImplementedHandler : IHttpHandler
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x00015492 File Offset: 0x00014492
		internal HttpNotImplementedHandler()
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001549C File Offset: 0x0001449C
		public void ProcessRequest(HttpContext context)
		{
			throw new HttpException(501, SR.GetString("Method_for_path_not_implemented", new object[]
			{
				context.Request.HttpMethod,
				context.Request.Path
			}));
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x000154E1 File Offset: 0x000144E1
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}

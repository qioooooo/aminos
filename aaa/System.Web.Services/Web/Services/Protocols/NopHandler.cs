using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000091 RID: 145
	internal class NopHandler : IHttpHandler
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x00012DF6 File Offset: 0x00011DF6
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00012DF9 File Offset: 0x00011DF9
		public void ProcessRequest(HttpContext context)
		{
		}
	}
}

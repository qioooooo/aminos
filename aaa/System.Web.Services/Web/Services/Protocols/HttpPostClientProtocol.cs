using System;
using System.Net;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000044 RID: 68
	public class HttpPostClientProtocol : HttpSimpleClientProtocol
	{
		// Token: 0x06000176 RID: 374 RVA: 0x000064B4 File Offset: 0x000054B4
		protected override WebRequest GetWebRequest(Uri uri)
		{
			WebRequest webRequest = base.GetWebRequest(uri);
			webRequest.Method = "POST";
			return webRequest;
		}
	}
}

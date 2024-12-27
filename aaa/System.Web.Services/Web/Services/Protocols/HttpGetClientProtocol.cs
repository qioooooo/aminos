using System;
using System.Net;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200003F RID: 63
	public class HttpGetClientProtocol : HttpSimpleClientProtocol
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00006010 File Offset: 0x00005010
		protected override WebRequest GetWebRequest(Uri uri)
		{
			WebRequest webRequest = base.GetWebRequest(uri);
			webRequest.Method = "GET";
			return webRequest;
		}
	}
}

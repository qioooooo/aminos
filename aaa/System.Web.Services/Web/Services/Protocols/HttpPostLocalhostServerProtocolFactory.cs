using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000045 RID: 69
	internal class HttpPostLocalhostServerProtocolFactory : ServerProtocolFactory
	{
		// Token: 0x06000177 RID: 375 RVA: 0x000064D8 File Offset: 0x000054D8
		protected override ServerProtocol CreateIfRequestCompatible(HttpRequest request)
		{
			if (request.PathInfo.Length < 2)
			{
				return null;
			}
			if (request.HttpMethod != "POST")
			{
				return new UnsupportedRequestProtocol(405);
			}
			string text = request.ServerVariables["LOCAL_ADDR"];
			string text2 = request.ServerVariables["REMOTE_ADDR"];
			if (!request.Url.IsLoopback && (text == null || text2 == null || !(text == text2)))
			{
				return null;
			}
			return new HttpPostServerProtocol();
		}
	}
}

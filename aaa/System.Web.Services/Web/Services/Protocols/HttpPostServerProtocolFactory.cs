using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000046 RID: 70
	internal class HttpPostServerProtocolFactory : ServerProtocolFactory
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00006567 File Offset: 0x00005567
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
			return new HttpPostServerProtocol();
		}
	}
}

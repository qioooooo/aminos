using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000040 RID: 64
	internal class HttpGetServerProtocolFactory : ServerProtocolFactory
	{
		// Token: 0x06000163 RID: 355 RVA: 0x00006031 File Offset: 0x00005031
		protected override ServerProtocol CreateIfRequestCompatible(HttpRequest request)
		{
			if (request.PathInfo.Length < 2)
			{
				return null;
			}
			if (request.HttpMethod != "GET")
			{
				return new UnsupportedRequestProtocol(405);
			}
			return new HttpGetServerProtocol();
		}
	}
}

using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000033 RID: 51
	internal class DocumentationServerProtocolFactory : ServerProtocolFactory
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00004F6D File Offset: 0x00003F6D
		protected override ServerProtocol CreateIfRequestCompatible(HttpRequest request)
		{
			if (request.PathInfo.Length > 0)
			{
				return null;
			}
			if (request.HttpMethod != "GET")
			{
				return new UnsupportedRequestProtocol(405);
			}
			return new DocumentationServerProtocol();
		}
	}
}

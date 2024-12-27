using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000087 RID: 135
	public class UrlParameterReader : ValueCollectionParameterReader
	{
		// Token: 0x06000394 RID: 916 RVA: 0x00011DCC File Offset: 0x00010DCC
		public override object[] Read(HttpRequest request)
		{
			return base.Read(request.QueryString);
		}
	}
}

using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000037 RID: 55
	public class HtmlFormParameterReader : ValueCollectionParameterReader
	{
		// Token: 0x0600013B RID: 315 RVA: 0x000054BD File Offset: 0x000044BD
		public override object[] Read(HttpRequest request)
		{
			if (!ContentType.MatchesBase(request.ContentType, "application/x-www-form-urlencoded"))
			{
				return null;
			}
			return base.Read(request.Form);
		}

		// Token: 0x0400027C RID: 636
		internal const string MimeType = "application/x-www-form-urlencoded";
	}
}

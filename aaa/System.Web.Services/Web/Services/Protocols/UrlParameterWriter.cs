using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000088 RID: 136
	public class UrlParameterWriter : UrlEncodedParameterWriter
	{
		// Token: 0x06000396 RID: 918 RVA: 0x00011DE4 File Offset: 0x00010DE4
		public override string GetRequestUrl(string url, object[] parameters)
		{
			if (parameters.Length == 0)
			{
				return url;
			}
			StringBuilder stringBuilder = new StringBuilder(url);
			stringBuilder.Append('?');
			TextWriter textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			base.Encode(textWriter, parameters);
			textWriter.Flush();
			return stringBuilder.ToString();
		}
	}
}

using System;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000059 RID: 89
	internal class ContentType
	{
		// Token: 0x060001FC RID: 508 RVA: 0x00009F21 File Offset: 0x00008F21
		private ContentType()
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009F2C File Offset: 0x00008F2C
		internal static string GetBase(string contentType)
		{
			int num = contentType.IndexOf(';');
			if (num >= 0)
			{
				return contentType.Substring(0, num);
			}
			return contentType;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009F50 File Offset: 0x00008F50
		internal static string GetMediaType(string contentType)
		{
			string @base = ContentType.GetBase(contentType);
			int num = @base.IndexOf('/');
			if (num >= 0)
			{
				return @base.Substring(0, num);
			}
			return @base;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009F7B File Offset: 0x00008F7B
		internal static string GetCharset(string contentType)
		{
			return ContentType.GetParameter(contentType, "charset");
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009F88 File Offset: 0x00008F88
		internal static string GetAction(string contentType)
		{
			return ContentType.GetParameter(contentType, "action");
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009FA0 File Offset: 0x00008FA0
		private static string GetParameter(string contentType, string paramName)
		{
			string[] array = contentType.Split(new char[] { ';' });
			for (int i = 1; i < array.Length; i++)
			{
				string text = array[i].TrimStart(null);
				if (string.Compare(text, 0, paramName, 0, paramName.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					int num = text.IndexOf('=', paramName.Length);
					if (num >= 0)
					{
						return text.Substring(num + 1).Trim(new char[] { ' ', '\'', '"', '\t' });
					}
				}
			}
			return null;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A022 File Offset: 0x00009022
		internal static bool MatchesBase(string contentType, string baseContentType)
		{
			return string.Compare(ContentType.GetBase(contentType), baseContentType, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A034 File Offset: 0x00009034
		internal static bool IsApplication(string contentType)
		{
			return string.Compare(ContentType.GetMediaType(contentType), "application", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A04C File Offset: 0x0000904C
		internal static bool IsSoap(string contentType)
		{
			string @base = ContentType.GetBase(contentType);
			return string.Compare(@base, "text/xml", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(@base, "application/soap+xml", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A080 File Offset: 0x00009080
		internal static bool IsXml(string contentType)
		{
			string @base = ContentType.GetBase(contentType);
			return string.Compare(@base, "text/xml", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(@base, "application/xml", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A0B4 File Offset: 0x000090B4
		internal static bool IsHtml(string contentType)
		{
			string @base = ContentType.GetBase(contentType);
			return string.Compare(@base, "text/html", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A0D7 File Offset: 0x000090D7
		internal static string Compose(string contentType, Encoding encoding)
		{
			return ContentType.Compose(contentType, encoding, null);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000A0E4 File Offset: 0x000090E4
		internal static string Compose(string contentType, Encoding encoding, string action)
		{
			if (encoding == null && action == null)
			{
				return contentType;
			}
			StringBuilder stringBuilder = new StringBuilder(contentType);
			if (encoding != null)
			{
				stringBuilder.Append("; charset=");
				stringBuilder.Append(encoding.WebName);
			}
			if (action != null)
			{
				stringBuilder.Append("; action=\"");
				stringBuilder.Append(action);
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040002C8 RID: 712
		internal const string TextBase = "text";

		// Token: 0x040002C9 RID: 713
		internal const string TextXml = "text/xml";

		// Token: 0x040002CA RID: 714
		internal const string TextPlain = "text/plain";

		// Token: 0x040002CB RID: 715
		internal const string TextHtml = "text/html";

		// Token: 0x040002CC RID: 716
		internal const string ApplicationBase = "application";

		// Token: 0x040002CD RID: 717
		internal const string ApplicationXml = "application/xml";

		// Token: 0x040002CE RID: 718
		internal const string ApplicationSoap = "application/soap+xml";

		// Token: 0x040002CF RID: 719
		internal const string ApplicationOctetStream = "application/octet-stream";

		// Token: 0x040002D0 RID: 720
		internal const string ContentEncoding = "Content-Encoding";
	}
}

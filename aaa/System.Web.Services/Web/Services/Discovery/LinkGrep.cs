using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000B1 RID: 177
	internal class LinkGrep
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x0001751B File Offset: 0x0001651B
		private LinkGrep()
		{
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00017524 File Offset: 0x00016524
		private static string ReadEntireStream(TextReader input)
		{
			char[] array = new char[4096];
			int num = 0;
			for (;;)
			{
				int num2 = input.Read(array, num, array.Length - num);
				if (num2 == 0)
				{
					break;
				}
				num += num2;
				if (num == array.Length)
				{
					char[] array2 = new char[array.Length * 2];
					Array.Copy(array, 0, array2, 0, array.Length);
					array = array2;
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001757C File Offset: 0x0001657C
		internal static string SearchForLink(Stream stream)
		{
			string text = LinkGrep.ReadEntireStream(new StreamReader(stream));
			int num = 0;
			Match match;
			if ((match = LinkGrep.doctypeDirectiveRegex.Match(text, num)).Success)
			{
				num += match.Length;
			}
			string text2;
			for (;;)
			{
				bool flag = false;
				if ((match = LinkGrep.whitespaceRegex.Match(text, num)).Success)
				{
					flag = true;
				}
				else if ((match = LinkGrep.textRegex.Match(text, num)).Success)
				{
					flag = true;
				}
				num += match.Length;
				if (num == text.Length)
				{
					goto IL_01F0;
				}
				if ((match = LinkGrep.tagRegex.Match(text, num)).Success)
				{
					flag = true;
					string value = match.Groups["tagname"].Value;
					if (string.Compare(value, "link", StringComparison.OrdinalIgnoreCase) == 0)
					{
						CaptureCollection captures = match.Groups["attrname"].Captures;
						CaptureCollection captures2 = match.Groups["attrval"].Captures;
						int count = captures.Count;
						bool flag2 = false;
						bool flag3 = false;
						text2 = null;
						for (int i = 0; i < count; i++)
						{
							string text3 = captures[i].ToString();
							string text4 = captures2[i].ToString();
							if (string.Compare(text3, "type", StringComparison.OrdinalIgnoreCase) == 0 && ContentType.MatchesBase(text4, "text/xml"))
							{
								flag2 = true;
							}
							else if (string.Compare(text3, "rel", StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(text4, "alternate", StringComparison.OrdinalIgnoreCase) == 0)
							{
								flag3 = true;
							}
							else if (string.Compare(text3, "href", StringComparison.OrdinalIgnoreCase) == 0)
							{
								text2 = text4;
							}
							if (flag2 && flag3 && text2 != null)
							{
								return text2;
							}
						}
					}
					else if (value == "body")
					{
						goto Block_16;
					}
				}
				else if ((match = LinkGrep.endtagRegex.Match(text, num)).Success)
				{
					flag = true;
				}
				else if ((match = LinkGrep.commentRegex.Match(text, num)).Success)
				{
					flag = true;
				}
				num += match.Length;
				if (num == text.Length || !flag)
				{
					goto IL_01F0;
				}
			}
			return text2;
			Block_16:
			IL_01F0:
			return null;
		}

		// Token: 0x040003D2 RID: 978
		private static readonly Regex tagRegex = new Regex("\\G<(?<prefix>[\\w:.-]+(?=:)|):?(?<tagname>[\\w.-]+)(?:\\s+(?<attrprefix>[\\w:.-]+(?=:)|):?(?<attrname>[\\w.-]+)\\s*=\\s*(?:\"(?<attrval>[^\"]*)\"|'(?<attrval>[^']*)'|(?<attrval>[a-zA-Z0-9\\-._:]+)))*\\s*(?<empty>/)?>");

		// Token: 0x040003D3 RID: 979
		private static readonly Regex doctypeDirectiveRegex = new Regex("\\G<!doctype\\b(([\\s\\w]+)|(\".*\"))*>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

		// Token: 0x040003D4 RID: 980
		private static readonly Regex endtagRegex = new Regex("\\G</(?<prefix>[\\w:-]+(?=:)|):?(?<tagname>[\\w-]+)\\s*>");

		// Token: 0x040003D5 RID: 981
		private static readonly Regex commentRegex = new Regex("\\G<!--(?>[^-]*-)+?->");

		// Token: 0x040003D6 RID: 982
		private static readonly Regex whitespaceRegex = new Regex("\\G\\s+(?=<|\\Z)");

		// Token: 0x040003D7 RID: 983
		private static readonly Regex textRegex = new Regex("\\G[^<]+");
	}
}

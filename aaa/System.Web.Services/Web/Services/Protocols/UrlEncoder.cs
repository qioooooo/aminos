using System;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000058 RID: 88
	internal class UrlEncoder
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x00009CDB File Offset: 0x00008CDB
		private UrlEncoder()
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009CE3 File Offset: 0x00008CE3
		internal static string EscapeString(string s, Encoding e)
		{
			return UrlEncoder.EscapeStringInternal(s, (e == null) ? new ASCIIEncoding() : e, false);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00009CF7 File Offset: 0x00008CF7
		internal static string UrlEscapeString(string s, Encoding e)
		{
			return UrlEncoder.EscapeStringInternal(s, (e == null) ? new ASCIIEncoding() : e, true);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00009D0C File Offset: 0x00008D0C
		private static string EscapeStringInternal(string s, Encoding e, bool escapeUriStuff)
		{
			if (s == null)
			{
				return null;
			}
			byte[] bytes = e.GetBytes(s);
			StringBuilder stringBuilder = new StringBuilder(bytes.Length);
			foreach (byte b in bytes)
			{
				char c = (char)b;
				if (b > 127 || b < 32 || c == '%' || (escapeUriStuff && !UrlEncoder.IsSafe(c)))
				{
					UrlEncoder.HexEscape8(stringBuilder, c);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00009D7C File Offset: 0x00008D7C
		internal static string UrlEscapeStringUnicode(string s)
		{
			int length = s.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (UrlEncoder.IsSafe(c))
				{
					stringBuilder.Append(c);
				}
				else if (c == ' ')
				{
					stringBuilder.Append('+');
				}
				else if ((c & 'ﾀ') == '\0')
				{
					UrlEncoder.HexEscape8(stringBuilder, c);
				}
				else
				{
					UrlEncoder.HexEscape16(stringBuilder, c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00009DEC File Offset: 0x00008DEC
		private static void HexEscape8(StringBuilder sb, char c)
		{
			sb.Append('%');
			sb.Append(UrlEncoder.HexUpperChars[(int)((c >> 4) & '\u000f')]);
			sb.Append(UrlEncoder.HexUpperChars[(int)(c & '\u000f')]);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009E1C File Offset: 0x00008E1C
		private static void HexEscape16(StringBuilder sb, char c)
		{
			sb.Append("%u");
			sb.Append(UrlEncoder.HexUpperChars[(int)((c >> 12) & '\u000f')]);
			sb.Append(UrlEncoder.HexUpperChars[(int)((c >> 8) & '\u000f')]);
			sb.Append(UrlEncoder.HexUpperChars[(int)((c >> 4) & '\u000f')]);
			sb.Append(UrlEncoder.HexUpperChars[(int)(c & '\u000f')]);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009E80 File Offset: 0x00008E80
		private static bool IsSafe(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}
			if (ch != '!')
			{
				switch (ch)
				{
				case '\'':
				case '(':
				case ')':
				case '*':
				case '-':
				case '.':
					return true;
				case '+':
				case ',':
					break;
				default:
					if (ch == '_')
					{
						return true;
					}
					break;
				}
				return false;
			}
			return true;
		}

		// Token: 0x040002C6 RID: 710
		private const int Max16BitUtf8SequenceLength = 4;

		// Token: 0x040002C7 RID: 711
		internal static readonly char[] HexUpperChars = new char[]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};
	}
}

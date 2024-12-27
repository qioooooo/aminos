using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Web
{
	// Token: 0x0200008D RID: 141
	public sealed class HttpUtility
	{
		// Token: 0x0600074D RID: 1869 RVA: 0x00020250 File Offset: 0x0001F250
		public static string HtmlDecode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('&') < 0)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(stringBuilder);
			HttpUtility.HtmlDecode(s, stringWriter);
			return stringBuilder.ToString();
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002028C File Offset: 0x0001F28C
		public static void HtmlDecode(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			if (s.IndexOf('&') < 0)
			{
				output.Write(s);
				return;
			}
			int length = s.Length;
			int i = 0;
			while (i < length)
			{
				char c = s[i];
				if (c != '&')
				{
					goto IL_00FC;
				}
				int num = s.IndexOfAny(HttpUtility.s_entityEndingChars, i + 1);
				if (num <= 0 || s[num] != ';')
				{
					goto IL_00FC;
				}
				string text = s.Substring(i + 1, num - i - 1);
				if (text.Length > 1 && text[0] == '#')
				{
					try
					{
						if (text[1] == 'x' || text[1] == 'X')
						{
							c = (char)int.Parse(text.Substring(2), NumberStyles.AllowHexSpecifier);
						}
						else
						{
							c = (char)int.Parse(text.Substring(1));
						}
						i = num;
						goto IL_00FC;
					}
					catch (FormatException)
					{
						i++;
						goto IL_00FC;
					}
					catch (ArgumentException)
					{
						i++;
						goto IL_00FC;
					}
				}
				i = num;
				char c2 = HtmlEntities.Lookup(text);
				if (c2 != '\0')
				{
					c = c2;
					goto IL_00FC;
				}
				output.Write('&');
				output.Write(text);
				output.Write(';');
				IL_0103:
				i++;
				continue;
				IL_00FC:
				output.Write(c);
				goto IL_0103;
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x000203C4 File Offset: 0x0001F3C4
		public static string HtmlEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			int num = HttpUtility.IndexOfHtmlEncodingChars(s, 0);
			if (num == -1)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder(s.Length + 5);
			int length = s.Length;
			int num2 = 0;
			do
			{
				if (num > num2)
				{
					stringBuilder.Append(s, num2, num - num2);
				}
				char c = s[num];
				if (c <= '>')
				{
					char c2 = c;
					if (c2 != '"')
					{
						if (c2 != '&')
						{
							switch (c2)
							{
							case '<':
								stringBuilder.Append("&lt;");
								break;
							case '>':
								stringBuilder.Append("&gt;");
								break;
							}
						}
						else
						{
							stringBuilder.Append("&amp;");
						}
					}
					else
					{
						stringBuilder.Append("&quot;");
					}
				}
				else
				{
					stringBuilder.Append("&#");
					StringBuilder stringBuilder2 = stringBuilder;
					int num3 = (int)c;
					stringBuilder2.Append(num3.ToString(NumberFormatInfo.InvariantInfo));
					stringBuilder.Append(';');
				}
				num2 = num + 1;
				if (num2 >= length)
				{
					goto IL_00F8;
				}
				num = HttpUtility.IndexOfHtmlEncodingChars(s, num2);
			}
			while (num != -1);
			stringBuilder.Append(s, num2, length - num2);
			IL_00F8:
			return stringBuilder.ToString();
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x000204D0 File Offset: 0x0001F4D0
		public unsafe static void HtmlEncode(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			int num = HttpUtility.IndexOfHtmlEncodingChars(s, 0);
			if (num == -1)
			{
				output.Write(s);
				return;
			}
			int num2 = s.Length - num;
			fixed (char* ptr = s)
			{
				char* ptr2 = ptr;
				while (num-- > 0)
				{
					output.Write(*(ptr2++));
				}
				while (num2-- > 0)
				{
					char c = *(ptr2++);
					if (c <= '>')
					{
						char c2 = c;
						if (c2 != '"')
						{
							if (c2 != '&')
							{
								switch (c2)
								{
								case '<':
									output.Write("&lt;");
									continue;
								case '>':
									output.Write("&gt;");
									continue;
								}
								output.Write(c);
							}
							else
							{
								output.Write("&amp;");
							}
						}
						else
						{
							output.Write("&quot;");
						}
					}
					else if (c >= '\u00a0' && c < 'Ā')
					{
						output.Write("&#");
						int num3 = (int)c;
						output.Write(num3.ToString(NumberFormatInfo.InvariantInfo));
						output.Write(';');
					}
					else
					{
						output.Write(c);
					}
				}
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000205F4 File Offset: 0x0001F5F4
		private unsafe static int IndexOfHtmlEncodingChars(string s, int startPos)
		{
			int i = s.Length - startPos;
			fixed (char* ptr = s)
			{
				char* ptr2 = ptr + startPos;
				while (i > 0)
				{
					char c = *ptr2;
					if (c <= '>')
					{
						char c2 = c;
						if (c2 != '"' && c2 != '&')
						{
							switch (c2)
							{
							case '<':
							case '>':
								break;
							case '=':
								goto IL_007A;
							default:
								goto IL_007A;
							}
						}
						return s.Length - i;
					}
					if (c >= '\u00a0' && c < 'Ā')
					{
						return s.Length - i;
					}
					IL_007A:
					ptr2++;
					i--;
				}
			}
			return -1;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00020690 File Offset: 0x0001F690
		public static string HtmlAttributeEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			int num = HttpUtility.IndexOfHtmlAttributeEncodingChars(s, 0);
			if (num == -1)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder(s.Length + 5);
			int length = s.Length;
			int num2 = 0;
			do
			{
				if (num > num2)
				{
					stringBuilder.Append(s, num2, num - num2);
				}
				char c = s[num];
				char c2 = c;
				if (c2 != '"')
				{
					if (c2 != '&')
					{
						if (c2 == '<')
						{
							stringBuilder.Append("&lt;");
						}
					}
					else
					{
						stringBuilder.Append("&amp;");
					}
				}
				else
				{
					stringBuilder.Append("&quot;");
				}
				num2 = num + 1;
				if (num2 >= length)
				{
					goto IL_00A3;
				}
				num = HttpUtility.IndexOfHtmlAttributeEncodingChars(s, num2);
			}
			while (num != -1);
			stringBuilder.Append(s, num2, length - num2);
			IL_00A3:
			return stringBuilder.ToString();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00020748 File Offset: 0x0001F748
		internal static void HtmlAttributeEncodeInternal(string s, HttpWriter writer)
		{
			int num = HttpUtility.IndexOfHtmlAttributeEncodingChars(s, 0);
			if (num == -1)
			{
				writer.Write(s);
				return;
			}
			int length = s.Length;
			int num2 = 0;
			for (;;)
			{
				if (num > num2)
				{
					writer.WriteString(s, num2, num - num2);
				}
				char c = s[num];
				char c2 = c;
				if (c2 != '"')
				{
					if (c2 != '&')
					{
						if (c2 == '<')
						{
							writer.Write("&lt;");
						}
					}
					else
					{
						writer.Write("&amp;");
					}
				}
				else
				{
					writer.Write("&quot;");
				}
				num2 = num + 1;
				if (num2 >= length)
				{
					break;
				}
				num = HttpUtility.IndexOfHtmlAttributeEncodingChars(s, num2);
				if (num == -1)
				{
					goto Block_7;
				}
			}
			return;
			Block_7:
			writer.WriteString(s, num2, length - num2);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000207E8 File Offset: 0x0001F7E8
		public unsafe static void HtmlAttributeEncode(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			int num = HttpUtility.IndexOfHtmlAttributeEncodingChars(s, 0);
			if (num == -1)
			{
				output.Write(s);
				return;
			}
			int num2 = s.Length - num;
			fixed (char* ptr = s)
			{
				char* ptr2 = ptr;
				while (num-- > 0)
				{
					output.Write(*(ptr2++));
				}
				while (num2-- > 0)
				{
					char c = *(ptr2++);
					if (c <= '<')
					{
						char c2 = c;
						if (c2 != '"')
						{
							if (c2 != '&')
							{
								if (c2 == '<')
								{
									output.Write("&lt;");
								}
								else
								{
									output.Write(c);
								}
							}
							else
							{
								output.Write("&amp;");
							}
						}
						else
						{
							output.Write("&quot;");
						}
					}
					else
					{
						output.Write(c);
					}
				}
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000208AC File Offset: 0x0001F8AC
		private unsafe static int IndexOfHtmlAttributeEncodingChars(string s, int startPos)
		{
			int i = s.Length - startPos;
			fixed (char* ptr = s)
			{
				char* ptr2 = ptr + startPos;
				while (i > 0)
				{
					char c = *ptr2;
					if (c <= '<')
					{
						char c2 = c;
						if (c2 == '"' || c2 == '&' || c2 == '<')
						{
							return s.Length - i;
						}
					}
					ptr2++;
					i--;
				}
			}
			return -1;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002091C File Offset: 0x0001F91C
		internal static string FormatPlainTextSpacesAsHtml(string s)
		{
			if (s == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(stringBuilder);
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (c == ' ')
				{
					stringWriter.Write("&nbsp;");
				}
				else
				{
					stringWriter.Write(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00020978 File Offset: 0x0001F978
		internal static string FormatPlainTextAsHtml(string s)
		{
			if (s == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(stringBuilder);
			HttpUtility.FormatPlainTextAsHtml(s, stringWriter);
			return stringBuilder.ToString();
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000209A4 File Offset: 0x0001F9A4
		internal static void FormatPlainTextAsHtml(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			int length = s.Length;
			char c = '\0';
			int i = 0;
			while (i < length)
			{
				char c2 = s[i];
				char c3 = c2;
				if (c3 <= '\r')
				{
					if (c3 != '\n')
					{
						if (c3 != '\r')
						{
							goto IL_00D2;
						}
					}
					else
					{
						output.Write("<br>");
					}
				}
				else
				{
					switch (c3)
					{
					case ' ':
						if (c == ' ')
						{
							output.Write("&nbsp;");
						}
						else
						{
							output.Write(c2);
						}
						break;
					case '!':
						goto IL_00D2;
					case '"':
						output.Write("&quot;");
						break;
					default:
						if (c3 != '&')
						{
							switch (c3)
							{
							case '<':
								output.Write("&lt;");
								break;
							case '=':
								goto IL_00D2;
							case '>':
								output.Write("&gt;");
								break;
							default:
								goto IL_00D2;
							}
						}
						else
						{
							output.Write("&amp;");
						}
						break;
					}
				}
				IL_0113:
				c = c2;
				i++;
				continue;
				IL_00D2:
				if (c2 >= '\u00a0' && c2 < 'Ā')
				{
					output.Write("&#");
					int num = (int)c2;
					output.Write(num.ToString(NumberFormatInfo.InvariantInfo));
					output.Write(';');
					goto IL_0113;
				}
				output.Write(c2);
				goto IL_0113;
			}
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00020AD1 File Offset: 0x0001FAD1
		public static NameValueCollection ParseQueryString(string query)
		{
			return HttpUtility.ParseQueryString(query, Encoding.UTF8);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00020AE0 File Offset: 0x0001FAE0
		public static NameValueCollection ParseQueryString(string query, Encoding encoding)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (query.Length > 0 && query[0] == '?')
			{
				query = query.Substring(1);
			}
			return new HttpValueCollection(query, false, true, encoding);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00020B2F File Offset: 0x0001FB2F
		public static string UrlEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlEncode(str, Encoding.UTF8);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00020B44 File Offset: 0x0001FB44
		public static string UrlPathEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			int num = str.IndexOf('?');
			if (num >= 0)
			{
				return HttpUtility.UrlPathEncode(str.Substring(0, num)) + str.Substring(num);
			}
			return HttpUtility.UrlEncodeSpaces(HttpUtility.UrlEncodeNonAscii(str, Encoding.UTF8));
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00020B90 File Offset: 0x0001FB90
		internal static string AspCompatUrlEncode(string s)
		{
			s = HttpUtility.UrlEncode(s);
			s = s.Replace("!", "%21");
			s = s.Replace("*", "%2A");
			s = s.Replace("(", "%28");
			s = s.Replace(")", "%29");
			s = s.Replace("-", "%2D");
			s = s.Replace(".", "%2E");
			s = s.Replace("_", "%5F");
			s = s.Replace("\\", "%5C");
			return s;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00020C36 File Offset: 0x0001FC36
		public static string UrlEncode(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytes(str, e));
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00020C50 File Offset: 0x0001FC50
		internal static string UrlEncodeNonAscii(string str, Encoding e)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			if (e == null)
			{
				e = Encoding.UTF8;
			}
			byte[] array = e.GetBytes(str);
			array = HttpUtility.UrlEncodeBytesToBytesInternalNonAscii(array, 0, array.Length, false);
			return Encoding.ASCII.GetString(array);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00020C90 File Offset: 0x0001FC90
		internal static string UrlEncodeSpaces(string str)
		{
			if (str != null && str.IndexOf(' ') >= 0)
			{
				str = str.Replace(" ", "%20");
			}
			return str;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00020CB3 File Offset: 0x0001FCB3
		public static string UrlEncode(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytes(bytes));
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00020CCA File Offset: 0x0001FCCA
		public static string UrlEncode(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(HttpUtility.UrlEncodeToBytes(bytes, offset, count));
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00020CE3 File Offset: 0x0001FCE3
		public static byte[] UrlEncodeToBytes(string str)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlEncodeToBytes(str, Encoding.UTF8);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00020CF8 File Offset: 0x0001FCF8
		public static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = e.GetBytes(str);
			return HttpUtility.UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00020D1D File Offset: 0x0001FD1D
		public static byte[] UrlEncodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpUtility.UrlEncodeToBytes(bytes, 0, bytes.Length);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00020D30 File Offset: 0x0001FD30
		public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null && count == 0)
			{
				return null;
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return HttpUtility.UrlEncodeBytesToBytesInternal(bytes, offset, count, true);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00020D88 File Offset: 0x0001FD88
		public static string UrlEncodeUnicode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlEncodeUnicodeStringToStringInternal(str, false);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00020D96 File Offset: 0x0001FD96
		public static byte[] UrlEncodeUnicodeToBytes(string str)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.ASCII.GetBytes(HttpUtility.UrlEncodeUnicode(str));
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00020DAD File Offset: 0x0001FDAD
		public static string UrlDecode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlDecode(str, Encoding.UTF8);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00020DBF File Offset: 0x0001FDBF
		public static string UrlDecode(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlDecodeStringFromStringInternal(str, e);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00020DCD File Offset: 0x0001FDCD
		public static string UrlDecode(byte[] bytes, Encoding e)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpUtility.UrlDecode(bytes, 0, bytes.Length, e);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00020DE0 File Offset: 0x0001FDE0
		public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
		{
			if (bytes == null && count == 0)
			{
				return null;
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return HttpUtility.UrlDecodeStringFromBytesInternal(bytes, offset, count, e);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00020E38 File Offset: 0x0001FE38
		public static byte[] UrlDecodeToBytes(string str)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlDecodeToBytes(str, Encoding.UTF8);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00020E4A File Offset: 0x0001FE4A
		public static byte[] UrlDecodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			return HttpUtility.UrlDecodeToBytes(e.GetBytes(str));
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00020E5D File Offset: 0x0001FE5D
		public static byte[] UrlDecodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpUtility.UrlDecodeToBytes(bytes, 0, (bytes != null) ? bytes.Length : 0);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00020E74 File Offset: 0x0001FE74
		public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null && count == 0)
			{
				return null;
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return HttpUtility.UrlDecodeBytesFromBytesInternal(bytes, offset, count);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00020ECC File Offset: 0x0001FECC
		private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = (char)bytes[offset + i];
				if (c == ' ')
				{
					num++;
				}
				else if (!HttpUtility.IsSafe(c))
				{
					num2++;
				}
			}
			if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				char c2 = (char)b;
				if (HttpUtility.IsSafe(c2))
				{
					array[num3++] = b;
				}
				else if (c2 == ' ')
				{
					array[num3++] = 43;
				}
				else
				{
					array[num3++] = 37;
					array[num3++] = (byte)HttpUtility.IntToHex((b >> 4) & 15);
					array[num3++] = (byte)HttpUtility.IntToHex((int)(b & 15));
				}
			}
			return array;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00020FA1 File Offset: 0x0001FFA1
		private static bool IsNonAsciiByte(byte b)
		{
			return b >= 127 || b < 32;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00020FB0 File Offset: 0x0001FFB0
		private static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
		{
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (HttpUtility.IsNonAsciiByte(bytes[offset + i]))
				{
					num++;
				}
			}
			if (!alwaysCreateReturnValue && num == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num * 2];
			int num2 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				if (HttpUtility.IsNonAsciiByte(b))
				{
					array[num2++] = 37;
					array[num2++] = (byte)HttpUtility.IntToHex((b >> 4) & 15);
					array[num2++] = (byte)HttpUtility.IntToHex((int)(b & 15));
				}
				else
				{
					array[num2++] = b;
				}
			}
			return array;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002104C File Offset: 0x0002004C
		private static string UrlEncodeUnicodeStringToStringInternal(string s, bool ignoreAscii)
		{
			int length = s.Length;
			StringBuilder stringBuilder = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if ((c & 'ﾀ') == '\0')
				{
					if (ignoreAscii || HttpUtility.IsSafe(c))
					{
						stringBuilder.Append(c);
					}
					else if (c == ' ')
					{
						stringBuilder.Append('+');
					}
					else
					{
						stringBuilder.Append('%');
						stringBuilder.Append(HttpUtility.IntToHex((int)((c >> 4) & '\u000f')));
						stringBuilder.Append(HttpUtility.IntToHex((int)(c & '\u000f')));
					}
				}
				else
				{
					stringBuilder.Append("%u");
					stringBuilder.Append(HttpUtility.IntToHex((int)((c >> 12) & '\u000f')));
					stringBuilder.Append(HttpUtility.IntToHex((int)((c >> 8) & '\u000f')));
					stringBuilder.Append(HttpUtility.IntToHex((int)((c >> 4) & '\u000f')));
					stringBuilder.Append(HttpUtility.IntToHex((int)(c & '\u000f')));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002113C File Offset: 0x0002013C
		internal static string CollapsePercentUFromStringInternal(string s, Encoding e)
		{
			int length = s.Length;
			HttpUtility.UrlDecoder urlDecoder = new HttpUtility.UrlDecoder(length, e);
			int num = s.IndexOf("%u", StringComparison.Ordinal);
			if (num == -1)
			{
				return s;
			}
			int i = 0;
			while (i < length)
			{
				char c = s[i];
				if (c != '%' || i >= length - 5 || s[i + 1] != 'u')
				{
					goto IL_00C8;
				}
				int num2 = HttpUtility.HexToInt(s[i + 2]);
				int num3 = HttpUtility.HexToInt(s[i + 3]);
				int num4 = HttpUtility.HexToInt(s[i + 4]);
				int num5 = HttpUtility.HexToInt(s[i + 5]);
				if (num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0)
				{
					goto IL_00C8;
				}
				c = (char)((num2 << 12) | (num3 << 8) | (num4 << 4) | num5);
				i += 5;
				urlDecoder.AddChar(c);
				IL_00E5:
				i++;
				continue;
				IL_00C8:
				if ((c & 'ﾀ') == '\0')
				{
					urlDecoder.AddByte((byte)c);
					goto IL_00E5;
				}
				urlDecoder.AddChar(c);
				goto IL_00E5;
			}
			return urlDecoder.GetString();
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00021240 File Offset: 0x00020240
		private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
		{
			int length = s.Length;
			HttpUtility.UrlDecoder urlDecoder = new HttpUtility.UrlDecoder(length, e);
			int i = 0;
			while (i < length)
			{
				char c = s[i];
				if (c == '+')
				{
					c = ' ';
					goto IL_0106;
				}
				if (c != '%' || i >= length - 2)
				{
					goto IL_0106;
				}
				if (s[i + 1] == 'u' && i < length - 5)
				{
					int num = HttpUtility.HexToInt(s[i + 2]);
					int num2 = HttpUtility.HexToInt(s[i + 3]);
					int num3 = HttpUtility.HexToInt(s[i + 4]);
					int num4 = HttpUtility.HexToInt(s[i + 5]);
					if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
					{
						goto IL_0106;
					}
					c = (char)((num << 12) | (num2 << 8) | (num3 << 4) | num4);
					i += 5;
					urlDecoder.AddChar(c);
				}
				else
				{
					int num5 = HttpUtility.HexToInt(s[i + 1]);
					int num6 = HttpUtility.HexToInt(s[i + 2]);
					if (num5 < 0 || num6 < 0)
					{
						goto IL_0106;
					}
					byte b = (byte)((num5 << 4) | num6);
					i += 2;
					urlDecoder.AddByte(b);
				}
				IL_0120:
				i++;
				continue;
				IL_0106:
				if ((c & 'ﾀ') == '\0')
				{
					urlDecoder.AddByte((byte)c);
					goto IL_0120;
				}
				urlDecoder.AddChar(c);
				goto IL_0120;
			}
			return urlDecoder.GetString();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00021380 File Offset: 0x00020380
		private static string UrlDecodeStringFromBytesInternal(byte[] buf, int offset, int count, Encoding e)
		{
			HttpUtility.UrlDecoder urlDecoder = new HttpUtility.UrlDecoder(count, e);
			int i = 0;
			while (i < count)
			{
				int num = offset + i;
				byte b = buf[num];
				if (b == 43)
				{
					b = 32;
					goto IL_00DA;
				}
				if (b != 37 || i >= count - 2)
				{
					goto IL_00DA;
				}
				if (buf[num + 1] == 117 && i < count - 5)
				{
					int num2 = HttpUtility.HexToInt((char)buf[num + 2]);
					int num3 = HttpUtility.HexToInt((char)buf[num + 3]);
					int num4 = HttpUtility.HexToInt((char)buf[num + 4]);
					int num5 = HttpUtility.HexToInt((char)buf[num + 5]);
					if (num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0)
					{
						goto IL_00DA;
					}
					char c = (char)((num2 << 12) | (num3 << 8) | (num4 << 4) | num5);
					i += 5;
					urlDecoder.AddChar(c);
				}
				else
				{
					int num6 = HttpUtility.HexToInt((char)buf[num + 1]);
					int num7 = HttpUtility.HexToInt((char)buf[num + 2]);
					if (num6 >= 0 && num7 >= 0)
					{
						b = (byte)((num6 << 4) | num7);
						i += 2;
						goto IL_00DA;
					}
					goto IL_00DA;
				}
				IL_00E1:
				i++;
				continue;
				IL_00DA:
				urlDecoder.AddByte(b);
				goto IL_00E1;
			}
			return urlDecoder.GetString();
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00021480 File Offset: 0x00020480
		private static byte[] UrlDecodeBytesFromBytesInternal(byte[] buf, int offset, int count)
		{
			int num = 0;
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				int num2 = offset + i;
				byte b = buf[num2];
				if (b == 43)
				{
					b = 32;
				}
				else if (b == 37 && i < count - 2)
				{
					int num3 = HttpUtility.HexToInt((char)buf[num2 + 1]);
					int num4 = HttpUtility.HexToInt((char)buf[num2 + 2]);
					if (num3 >= 0 && num4 >= 0)
					{
						b = (byte)((num3 << 4) | num4);
						i += 2;
					}
				}
				array[num++] = b;
			}
			if (num < array.Length)
			{
				byte[] array2 = new byte[num];
				Array.Copy(array, array2, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00021517 File Offset: 0x00020517
		private static int HexToInt(char h)
		{
			if (h >= '0' && h <= '9')
			{
				return (int)(h - '0');
			}
			if (h >= 'a' && h <= 'f')
			{
				return (int)(h - 'a' + '\n');
			}
			if (h < 'A' || h > 'F')
			{
				return -1;
			}
			return (int)(h - 'A' + '\n');
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002154D File Offset: 0x0002054D
		internal static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00021564 File Offset: 0x00020564
		internal static bool IsSafe(char ch)
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

		// Token: 0x0600077C RID: 1916 RVA: 0x000215CC File Offset: 0x000205CC
		internal static string FormatHttpDateTime(DateTime dt)
		{
			if (dt < DateTime.MaxValue.AddDays(-1.0) && dt > DateTime.MinValue.AddDays(1.0))
			{
				dt = dt.ToUniversalTime();
			}
			return dt.ToString("R", DateTimeFormatInfo.InvariantInfo);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002162F File Offset: 0x0002062F
		internal static string FormatHttpDateTimeUtc(DateTime dt)
		{
			return dt.ToString("R", DateTimeFormatInfo.InvariantInfo);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00021644 File Offset: 0x00020644
		internal static string FormatHttpCookieDateTime(DateTime dt)
		{
			if (dt < DateTime.MaxValue.AddDays(-1.0) && dt > DateTime.MinValue.AddDays(1.0))
			{
				dt = dt.ToUniversalTime();
			}
			return dt.ToString("ddd, dd-MMM-yyyy HH':'mm':'ss 'GMT'", DateTimeFormatInfo.InvariantInfo);
		}

		// Token: 0x0400114F RID: 4431
		private static char[] s_entityEndingChars = new char[] { ';', '&' };

		// Token: 0x0200008E RID: 142
		private class UrlDecoder
		{
			// Token: 0x06000780 RID: 1920 RVA: 0x000216CC File Offset: 0x000206CC
			private void FlushBytes()
			{
				if (this._numBytes > 0)
				{
					this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
					this._numBytes = 0;
				}
			}

			// Token: 0x06000781 RID: 1921 RVA: 0x0002171A File Offset: 0x0002071A
			internal UrlDecoder(int bufferSize, Encoding encoding)
			{
				this._bufferSize = bufferSize;
				this._encoding = encoding;
				this._charBuffer = new char[bufferSize];
			}

			// Token: 0x06000782 RID: 1922 RVA: 0x0002173C File Offset: 0x0002073C
			internal void AddChar(char ch)
			{
				if (this._numBytes > 0)
				{
					this.FlushBytes();
				}
				this._charBuffer[this._numChars++] = ch;
			}

			// Token: 0x06000783 RID: 1923 RVA: 0x00021774 File Offset: 0x00020774
			internal void AddByte(byte b)
			{
				if (this._byteBuffer == null)
				{
					this._byteBuffer = new byte[this._bufferSize];
				}
				this._byteBuffer[this._numBytes++] = b;
			}

			// Token: 0x06000784 RID: 1924 RVA: 0x000217B3 File Offset: 0x000207B3
			internal string GetString()
			{
				if (this._numBytes > 0)
				{
					this.FlushBytes();
				}
				if (this._numChars > 0)
				{
					return new string(this._charBuffer, 0, this._numChars);
				}
				return string.Empty;
			}

			// Token: 0x04001150 RID: 4432
			private int _bufferSize;

			// Token: 0x04001151 RID: 4433
			private int _numChars;

			// Token: 0x04001152 RID: 4434
			private char[] _charBuffer;

			// Token: 0x04001153 RID: 4435
			private int _numBytes;

			// Token: 0x04001154 RID: 4436
			private byte[] _byteBuffer;

			// Token: 0x04001155 RID: 4437
			private Encoding _encoding;
		}
	}
}

using System;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000026 RID: 38
	internal static class HttpChannelHelper
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00005DF6 File Offset: 0x00004DF6
		internal static int StartsWithHttp(string url)
		{
			int length = url.Length;
			if (StringHelper.StartsWithAsciiIgnoreCasePrefixLower(url, "http://"))
			{
				return "http://".Length;
			}
			if (StringHelper.StartsWithAsciiIgnoreCasePrefixLower(url, "https://"))
			{
				return "https://".Length;
			}
			return -1;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005E30 File Offset: 0x00004E30
		internal static string ParseURL(string url, out string objectURI)
		{
			objectURI = null;
			int num = HttpChannelHelper.StartsWithHttp(url);
			if (num == -1)
			{
				return null;
			}
			num = url.IndexOf('/', num);
			if (-1 == num)
			{
				return url;
			}
			string text = url.Substring(0, num);
			objectURI = url.Substring(num);
			return text;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005E70 File Offset: 0x00004E70
		internal static string GetObjectUriFromRequestUri(string uri)
		{
			int num = uri.Length;
			int num2 = HttpChannelHelper.StartsWithHttp(uri);
			int num3;
			if (num2 != -1)
			{
				num3 = uri.IndexOf('/', num2);
				if (num3 != -1)
				{
					num2 = num3 + 1;
				}
				else
				{
					num2 = num;
				}
			}
			else
			{
				num2 = 0;
				if (uri[num2] == '/')
				{
					num2++;
				}
			}
			num3 = uri.IndexOf('?');
			if (num3 != -1)
			{
				num = num3;
			}
			if (num2 < num)
			{
				return CoreChannel.RemoveApplicationNameFromUri(uri.Substring(num2, num - num2));
			}
			return "";
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005EE4 File Offset: 0x00004EE4
		internal static void ParseContentType(string contentType, out string value, out string charset)
		{
			charset = null;
			if (contentType == null)
			{
				value = null;
				return;
			}
			string[] array = contentType.Split(HttpChannelHelper.s_semicolonSeparator);
			value = array[0];
			if (array.Length > 0)
			{
				foreach (string text in array)
				{
					int num = text.IndexOf('=');
					if (num != -1)
					{
						string text2 = text.Substring(0, num).Trim();
						if (string.Compare(text2, "charset", StringComparison.OrdinalIgnoreCase) == 0)
						{
							if (num + 1 < text.Length)
							{
								charset = text.Substring(num + 1);
							}
							else
							{
								charset = null;
							}
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005F74 File Offset: 0x00004F74
		internal static string ReplaceChannelUriWithThisString(string url, string channelUri)
		{
			string text;
			HttpChannelHelper.ParseURL(url, out text);
			return channelUri + text;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005F94 File Offset: 0x00004F94
		internal static string ReplaceMachineNameWithThisString(string url, string newMachineName)
		{
			string text2;
			string text = HttpChannelHelper.ParseURL(url, out text2);
			int num = HttpChannelHelper.StartsWithHttp(url);
			if (num == -1)
			{
				return url;
			}
			int num2 = text.IndexOf(':', num);
			if (num2 == -1)
			{
				num2 = text.Length;
			}
			return url.Substring(0, num) + newMachineName + url.Substring(num2);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005FE8 File Offset: 0x00004FE8
		internal static void DecodeUriInPlace(byte[] uriBytes, out int length)
		{
			int num = 0;
			int num2 = uriBytes.Length;
			length = num2;
			int i = 0;
			while (i < num2)
			{
				if (uriBytes[i] == 37)
				{
					int num3 = i - num * 2;
					uriBytes[num3] = (byte)(16 * HttpChannelHelper.CharacterHexDigitToDecimal(uriBytes[i + 1]) + HttpChannelHelper.CharacterHexDigitToDecimal(uriBytes[i + 2]));
					num++;
					length -= 2;
					i += 3;
				}
				else
				{
					if (num != 0)
					{
						int num4 = i - num * 2;
						uriBytes[num4] = uriBytes[i];
					}
					i++;
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006054 File Offset: 0x00005054
		internal static int CharacterHexDigitToDecimal(byte b)
		{
			switch (b)
			{
			case 65:
				return 10;
			case 66:
				return 11;
			case 67:
				return 12;
			case 68:
				return 13;
			case 69:
				return 14;
			case 70:
				break;
			default:
				switch (b)
				{
				case 97:
					return 10;
				case 98:
					return 11;
				case 99:
					return 12;
				case 100:
					return 13;
				case 101:
					return 14;
				case 102:
					break;
				default:
					return (int)(b - 48);
				}
				break;
			}
			return 15;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000060C0 File Offset: 0x000050C0
		internal static char DecimalToCharacterHexDigit(int i)
		{
			switch (i)
			{
			case 10:
				return 'A';
			case 11:
				return 'B';
			case 12:
				return 'C';
			case 13:
				return 'D';
			case 14:
				return 'E';
			case 15:
				return 'F';
			default:
				return (char)(i + 48);
			}
		}

		// Token: 0x040000D7 RID: 215
		private const string _http = "http://";

		// Token: 0x040000D8 RID: 216
		private const string _https = "https://";

		// Token: 0x040000D9 RID: 217
		private static char[] s_semicolonSeparator = new char[] { ';' };
	}
}

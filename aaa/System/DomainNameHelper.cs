using System;
using System.Globalization;
using System.Net;

namespace System
{
	// Token: 0x02000362 RID: 866
	internal class DomainNameHelper
	{
		// Token: 0x06001B8E RID: 7054 RVA: 0x000676A7 File Offset: 0x000666A7
		private DomainNameHelper()
		{
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x000676B0 File Offset: 0x000666B0
		internal static string ParseCanonicalName(string str, int start, int end, ref bool loopback)
		{
			string text = null;
			for (int i = end - 1; i >= start; i--)
			{
				if (str[i] >= 'A' && str[i] <= 'Z')
				{
					text = str.Substring(start, end - start).ToLower(CultureInfo.InvariantCulture);
					break;
				}
				if (str[i] == ':')
				{
					end = i;
				}
			}
			if (text == null)
			{
				text = str.Substring(start, end - start);
			}
			if (text == "localhost" || text == "loopback")
			{
				loopback = true;
				return "localhost";
			}
			return text;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0006773C File Offset: 0x0006673C
		internal unsafe static bool IsValid(char* name, ushort pos, ref int returnedEnd, ref bool notCanonical, bool notImplicitFile)
		{
			char* ptr = name + pos;
			char* ptr2 = ptr;
			char* ptr3 = name + returnedEnd;
			while (ptr2 < ptr3)
			{
				char c = *ptr2;
				if (c > '\u007f')
				{
					return false;
				}
				if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					ptr3 = ptr2;
					break;
				}
				ptr2++;
			}
			if (ptr3 == ptr)
			{
				return false;
			}
			for (;;)
			{
				ptr2 = ptr;
				while (ptr2 < ptr3 && *ptr2 != '.')
				{
					ptr2++;
				}
				if (ptr == ptr2 || (long)(ptr2 - ptr) > 63L || !DomainNameHelper.IsASCIILetterOrDigit(*(ptr++), ref notCanonical))
				{
					break;
				}
				while (ptr < ptr2)
				{
					if (!DomainNameHelper.IsValidDomainLabelCharacter(*(ptr++), ref notCanonical))
					{
						return false;
					}
				}
				ptr++;
				if (ptr >= ptr3)
				{
					goto Block_13;
				}
			}
			return false;
			Block_13:
			returnedEnd = (int)((ushort)((long)(ptr3 - name)));
			return true;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000677F4 File Offset: 0x000667F4
		internal unsafe static bool IsValidByIri(char* name, ushort pos, ref int returnedEnd, ref bool notCanonical, bool notImplicitFile)
		{
			char* ptr = name + pos;
			char* ptr2 = ptr;
			char* ptr3 = name + returnedEnd;
			while (ptr2 < ptr3)
			{
				char c = *ptr2;
				if (c == '/' || c == '\\' || (notImplicitFile && (c == ':' || c == '?' || c == '#')))
				{
					ptr3 = ptr2;
					break;
				}
				ptr2++;
			}
			if (ptr3 == ptr)
			{
				return false;
			}
			for (;;)
			{
				ptr2 = ptr;
				int num = 0;
				bool flag = false;
				while (ptr2 < ptr3 && *ptr2 != '.' && *ptr2 != '。' && *ptr2 != '．' && *ptr2 != '｡')
				{
					num++;
					if (*ptr2 > 'ÿ')
					{
						num++;
					}
					if (*ptr2 >= '\u00a0')
					{
						flag = true;
					}
					ptr2++;
				}
				if (ptr == ptr2 || (flag ? (num + 4) : num) > 63 || (*(ptr++) < '\u00a0' && !DomainNameHelper.IsASCIILetterOrDigit(*(ptr - 1), ref notCanonical)))
				{
					break;
				}
				while (ptr < ptr2)
				{
					if (*(ptr++) < '\u00a0' && !DomainNameHelper.IsValidDomainLabelCharacter(*(ptr - 1), ref notCanonical))
					{
						return false;
					}
				}
				ptr++;
				if (ptr >= ptr3)
				{
					goto Block_20;
				}
			}
			return false;
			Block_20:
			returnedEnd = (int)((ushort)((long)(ptr3 - name)));
			return true;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00067905 File Offset: 0x00066905
		private static bool IsASCIILetter(char character, ref bool notCanonical)
		{
			if (character >= 'a' && character <= 'z')
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				if (!notCanonical)
				{
					notCanonical = true;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x00067928 File Offset: 0x00066928
		internal unsafe static string IdnEquivalent(char* hostname, int start, int end, ref bool allAscii, ref bool atLeastOneValidIdn)
		{
			string text = null;
			string text2 = DomainNameHelper.IdnEquivalent(hostname, start, end, ref allAscii, ref text);
			if (text2 != null)
			{
				string text3 = (allAscii ? text2 : text);
				fixed (char* ptr = text3)
				{
					int length = text3.Length;
					int i = 0;
					int num = 0;
					bool flag = false;
					do
					{
						bool flag2 = false;
						bool flag3 = false;
						flag = false;
						for (i = num; i < length; i++)
						{
							char c = ptr[i];
							if (!flag3)
							{
								flag3 = true;
								if (i + 3 < length && DomainNameHelper.IsIdnAce(ptr, i))
								{
									i += 4;
									flag2 = true;
									continue;
								}
							}
							if (c == '.' || c == '。' || c == '．' || c == '｡')
							{
								flag = true;
								break;
							}
						}
						if (flag2)
						{
							try
							{
								IdnMapping idnMapping = new IdnMapping();
								idnMapping.GetUnicode(new string(ptr, num, i - num));
								atLeastOneValidIdn = true;
								break;
							}
							catch (ArgumentException)
							{
							}
						}
						num = i + (flag ? 1 : 0);
					}
					while (num < length);
				}
			}
			else
			{
				atLeastOneValidIdn = false;
			}
			return text2;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00067A44 File Offset: 0x00066A44
		internal unsafe static string IdnEquivalent(char* hostname, int start, int end, ref bool allAscii, ref string bidiStrippedHost)
		{
			string text = null;
			if (end <= start)
			{
				return text;
			}
			int i = start;
			allAscii = true;
			while (i < end)
			{
				if (hostname[i] > '\u007f')
				{
					allAscii = false;
					break;
				}
				i++;
			}
			if (!allAscii)
			{
				IdnMapping idnMapping = new IdnMapping();
				bidiStrippedHost = Uri.StripBidiControlCharacter(hostname, start, end - start);
				string ascii;
				try
				{
					ascii = idnMapping.GetAscii(bidiStrippedHost);
					if (!ServicePointManager.AllowDangerousUnicodeDecompositions && DomainNameHelper.ContainsCharactersUnsafeForNormalizedHost(ascii))
					{
						throw new UriFormatException("net_uri_BadUnicodeHostForIdn");
					}
				}
				catch (ArgumentException)
				{
					throw new UriFormatException(SR.GetString("net_uri_BadUnicodeHostForIdn"));
				}
				return ascii;
			}
			string text2 = new string(hostname, start, end - start);
			if (text2 == null)
			{
				return null;
			}
			return text2.ToLowerInvariant();
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00067AF4 File Offset: 0x00066AF4
		private static bool IsIdnAce(string input, int index)
		{
			return input[index] == 'x' && input[index + 1] == 'n' && input[index + 2] == '-' && input[index + 3] == '-';
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x00067B2B File Offset: 0x00066B2B
		private unsafe static bool IsIdnAce(char* input, int index)
		{
			return input[index] == 'x' && input[index + 1] == 'n' && input[index + 2] == '-' && input[index + 3] == '-';
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00067B64 File Offset: 0x00066B64
		internal unsafe static string UnicodeEquivalent(string idnHost, char* hostname, int start, int end)
		{
			IdnMapping idnMapping = new IdnMapping();
			try
			{
				return idnMapping.GetUnicode(idnHost);
			}
			catch (ArgumentException)
			{
			}
			bool flag = true;
			return DomainNameHelper.UnicodeEquivalent(hostname, start, end, ref flag, ref flag);
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x00067BA4 File Offset: 0x00066BA4
		internal unsafe static string UnicodeEquivalent(char* hostname, int start, int end, ref bool allAscii, ref bool atLeastOneValidIdn)
		{
			IdnMapping idnMapping = new IdnMapping();
			allAscii = true;
			atLeastOneValidIdn = false;
			string text = null;
			if (end <= start)
			{
				return text;
			}
			string text2 = Uri.StripBidiControlCharacter(hostname, start, end - start);
			string text3 = null;
			int num = 0;
			int i = 0;
			int length = text2.Length;
			bool flag = false;
			do
			{
				bool flag2 = true;
				bool flag3 = false;
				bool flag4 = false;
				flag = false;
				for (i = num; i < length; i++)
				{
					char c = text2[i];
					if (!flag4)
					{
						flag4 = true;
						if (i + 3 < length && c == 'x' && DomainNameHelper.IsIdnAce(text2, i))
						{
							flag3 = true;
						}
					}
					if (flag2 && c > '\u007f')
					{
						flag2 = false;
						allAscii = false;
					}
					if (c == '.' || c == '。' || c == '．' || c == '｡')
					{
						flag = true;
						break;
					}
				}
				if (!flag2)
				{
					string text4 = text2.Substring(num, i - num);
					try
					{
						text4 = idnMapping.GetAscii(text4);
					}
					catch (ArgumentException)
					{
						throw new UriFormatException(SR.GetString("net_uri_BadUnicodeHostForIdn"));
					}
					text3 += idnMapping.GetUnicode(text4);
					if (flag)
					{
						text3 += ".";
					}
				}
				else
				{
					bool flag5 = false;
					if (flag3)
					{
						try
						{
							text3 += idnMapping.GetUnicode(text2.Substring(num, i - num));
							if (flag)
							{
								text3 += ".";
							}
							flag5 = true;
							atLeastOneValidIdn = true;
						}
						catch (ArgumentException)
						{
						}
					}
					if (!flag5)
					{
						text3 += text2.Substring(num, i - num).ToLowerInvariant();
						if (flag)
						{
							text3 += ".";
						}
					}
				}
				num = i + (flag ? 1 : 0);
			}
			while (num < length);
			return text3;
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x00067D5C File Offset: 0x00066D5C
		private static bool IsASCIILetterOrDigit(char character, ref bool notCanonical)
		{
			if ((character >= 'a' && character <= 'z') || (character >= '0' && character <= '9'))
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				notCanonical = true;
				return true;
			}
			return false;
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x00067D84 File Offset: 0x00066D84
		private static bool IsValidDomainLabelCharacter(char character, ref bool notCanonical)
		{
			if ((character >= 'a' && character <= 'z') || (character >= '0' && character <= '9') || character == '-' || character == '_')
			{
				return true;
			}
			if (character >= 'A' && character <= 'Z')
			{
				notCanonical = true;
				return true;
			}
			return false;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00067DB6 File Offset: 0x00066DB6
		internal static bool ContainsCharactersUnsafeForNormalizedHost(string host)
		{
			return host.IndexOfAny(DomainNameHelper.s_UnsafeForNormalizedHost) != -1;
		}

		// Token: 0x04001C2B RID: 7211
		private const char c_DummyChar = '\uffff';

		// Token: 0x04001C2C RID: 7212
		internal const string Localhost = "localhost";

		// Token: 0x04001C2D RID: 7213
		internal const string Loopback = "loopback";

		// Token: 0x04001C2E RID: 7214
		private static readonly char[] s_UnsafeForNormalizedHost = new char[] { '\\', '/', '?', '@', '#', ':', '[', ']' };
	}
}

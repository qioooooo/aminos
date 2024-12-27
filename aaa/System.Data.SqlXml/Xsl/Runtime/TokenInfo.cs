using System;
using System.Diagnostics;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000C9 RID: 201
	internal class TokenInfo
	{
		// Token: 0x06000977 RID: 2423 RVA: 0x0002C7C8 File Offset: 0x0002B7C8
		private TokenInfo()
		{
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0002C7D0 File Offset: 0x0002B7D0
		[Conditional("DEBUG")]
		public void AssertSeparator(bool isSeparator)
		{
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0002C7D4 File Offset: 0x0002B7D4
		public static TokenInfo CreateSeparator(string formatString, int startIdx, int tokLen)
		{
			return new TokenInfo
			{
				startIdx = startIdx,
				formatString = formatString,
				length = tokLen
			};
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002C800 File Offset: 0x0002B800
		public static TokenInfo CreateFormat(string formatString, int startIdx, int tokLen)
		{
			TokenInfo tokenInfo = new TokenInfo();
			tokenInfo.formatString = null;
			tokenInfo.length = 1;
			bool flag = false;
			char c = formatString[startIdx];
			char c2 = c;
			if (c2 <= 'A')
			{
				if (c2 == '1' || c2 == 'A')
				{
					goto IL_0092;
				}
			}
			else if (c2 == 'I' || c2 == 'a' || c2 == 'i')
			{
				goto IL_0092;
			}
			if (!CharUtil.IsDecimalDigitOne(c))
			{
				if (CharUtil.IsDecimalDigitOne(c + '\u0001'))
				{
					int num = startIdx;
					do
					{
						tokenInfo.length++;
					}
					while (--tokLen > 0 && c == formatString[++num]);
					if (formatString[num] == (c += '\u0001'))
					{
						goto IL_0092;
					}
				}
				flag = true;
			}
			IL_0092:
			if (tokLen != 1)
			{
				flag = true;
			}
			if (flag)
			{
				tokenInfo.startChar = '1';
				tokenInfo.length = 1;
			}
			else
			{
				tokenInfo.startChar = c;
			}
			return tokenInfo;
		}

		// Token: 0x040005CD RID: 1485
		public char startChar;

		// Token: 0x040005CE RID: 1486
		public int startIdx;

		// Token: 0x040005CF RID: 1487
		public string formatString;

		// Token: 0x040005D0 RID: 1488
		public int length;
	}
}

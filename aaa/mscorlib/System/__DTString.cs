using System;
using System.Globalization;
using System.Threading;

namespace System
{
	// Token: 0x02000386 RID: 902
	internal struct __DTString
	{
		// Token: 0x060024A8 RID: 9384 RVA: 0x00064645 File Offset: 0x00063645
		internal __DTString(string str, DateTimeFormatInfo dtfi, bool checkDigitToken)
		{
			this = new __DTString(str, dtfi);
			this.m_checkDigitToken = checkDigitToken;
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x00064658 File Offset: 0x00063658
		internal __DTString(string str, DateTimeFormatInfo dtfi)
		{
			this.Index = -1;
			this.Value = str;
			this.len = this.Value.Length;
			this.m_current = '\0';
			if (dtfi != null)
			{
				this.m_info = dtfi.CompareInfo;
				this.m_checkDigitToken = (dtfi.FormatFlags & DateTimeFormatFlags.UseDigitPrefixInTokens) != DateTimeFormatFlags.None;
				return;
			}
			this.m_info = Thread.CurrentThread.CurrentCulture.CompareInfo;
			this.m_checkDigitToken = false;
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060024AA RID: 9386 RVA: 0x000646CC File Offset: 0x000636CC
		internal CompareInfo CompareInfo
		{
			get
			{
				return this.m_info;
			}
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000646D4 File Offset: 0x000636D4
		internal bool GetNext()
		{
			this.Index++;
			if (this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				return true;
			}
			return false;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0006470C File Offset: 0x0006370C
		internal bool Advance(int count)
		{
			this.Index += count;
			if (this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				return true;
			}
			return false;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x00064744 File Offset: 0x00063744
		internal void GetRegularToken(out TokenType tokenType, out int tokenValue, DateTimeFormatInfo dtfi)
		{
			tokenValue = 0;
			if (this.Index >= this.len)
			{
				tokenType = TokenType.EndOfString;
				return;
			}
			tokenType = TokenType.UnknownToken;
			IL_0019:
			while (!DateTimeParse.IsDigit(this.m_current))
			{
				if (char.IsWhiteSpace(this.m_current))
				{
					while (++this.Index < this.len)
					{
						this.m_current = this.Value[this.Index];
						if (!char.IsWhiteSpace(this.m_current))
						{
							goto IL_0019;
						}
					}
					tokenType = TokenType.EndOfString;
					return;
				}
				dtfi.Tokenize(TokenType.RegularTokenMask, out tokenType, out tokenValue, ref this);
				return;
			}
			tokenValue = (int)(this.m_current - '0');
			int index = this.Index;
			while (++this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				int num = (int)(this.m_current - '0');
				if (num < 0 || num > 9)
				{
					break;
				}
				tokenValue = tokenValue * 10 + num;
			}
			if (this.Index - index > 8)
			{
				tokenType = TokenType.NumberToken;
				tokenValue = -1;
			}
			else if (this.Index - index < 3)
			{
				tokenType = TokenType.NumberToken;
			}
			else
			{
				tokenType = TokenType.YearNumberToken;
			}
			if (!this.m_checkDigitToken)
			{
				return;
			}
			int index2 = this.Index;
			char current = this.m_current;
			this.Index = index;
			this.m_current = this.Value[this.Index];
			TokenType tokenType2;
			int num2;
			if (dtfi.Tokenize(TokenType.RegularTokenMask, out tokenType2, out num2, ref this))
			{
				tokenType = tokenType2;
				tokenValue = num2;
				return;
			}
			this.Index = index2;
			this.m_current = current;
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000648C8 File Offset: 0x000638C8
		internal TokenType GetSeparatorToken(DateTimeFormatInfo dtfi, out int indexBeforeSeparator, out char charBeforeSeparator)
		{
			indexBeforeSeparator = this.Index;
			charBeforeSeparator = this.m_current;
			if (!this.SkipWhiteSpaceCurrent())
			{
				return TokenType.SEP_End;
			}
			TokenType tokenType;
			if (!DateTimeParse.IsDigit(this.m_current))
			{
				int num;
				if (!dtfi.Tokenize(TokenType.SeparatorTokenMask, out tokenType, out num, ref this))
				{
					tokenType = TokenType.SEP_Space;
				}
			}
			else
			{
				tokenType = TokenType.SEP_Space;
			}
			return tokenType;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00064923 File Offset: 0x00063923
		internal bool MatchSpecifiedWord(string target)
		{
			return this.MatchSpecifiedWord(target, target.Length + this.Index);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x0006493C File Offset: 0x0006393C
		internal bool MatchSpecifiedWord(string target, int endIndex)
		{
			int num = endIndex - this.Index;
			return num == target.Length && this.Index + num <= this.len && this.m_info.Compare(this.Value, this.Index, num, target, 0, num, CompareOptions.IgnoreCase) == 0;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x00064990 File Offset: 0x00063990
		internal bool MatchSpecifiedWords(string target, bool checkWordBoundary, ref int matchLength)
		{
			int num = this.Value.Length - this.Index;
			matchLength = target.Length;
			if (matchLength > num || this.m_info.Compare(this.Value, this.Index, matchLength, target, 0, matchLength, CompareOptions.IgnoreCase) != 0)
			{
				int num2 = 0;
				int num3 = this.Index;
				int num4 = target.IndexOfAny(__DTString.WhiteSpaceChecks, num2);
				if (num4 == -1)
				{
					return false;
				}
				for (;;)
				{
					int num5 = num4 - num2;
					if (num3 >= this.Value.Length - num5)
					{
						break;
					}
					if (num5 == 0)
					{
						matchLength--;
					}
					else
					{
						if (!char.IsWhiteSpace(this.Value[num3 + num5]))
						{
							return false;
						}
						if (this.m_info.Compare(this.Value, num3, num5, target, num2, num5, CompareOptions.IgnoreCase) != 0)
						{
							return false;
						}
						num3 = num3 + num5 + 1;
					}
					num2 = num4 + 1;
					while (num3 < this.Value.Length && char.IsWhiteSpace(this.Value[num3]))
					{
						num3++;
						matchLength++;
					}
					if ((num4 = target.IndexOfAny(__DTString.WhiteSpaceChecks, num2)) < 0)
					{
						goto Block_8;
					}
				}
				return false;
				Block_8:
				if (num2 < target.Length)
				{
					int num6 = target.Length - num2;
					if (num3 > this.Value.Length - num6)
					{
						return false;
					}
					if (this.m_info.Compare(this.Value, num3, num6, target, num2, num6, CompareOptions.IgnoreCase) != 0)
					{
						return false;
					}
				}
			}
			if (checkWordBoundary)
			{
				int num7 = this.Index + matchLength;
				if (num7 < this.Value.Length && char.IsLetter(this.Value[num7]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x00064B18 File Offset: 0x00063B18
		internal bool Match(string str)
		{
			if (++this.Index >= this.len)
			{
				return false;
			}
			if (str.Length > this.Value.Length - this.Index)
			{
				return false;
			}
			if (this.m_info.Compare(this.Value, this.Index, str.Length, str, 0, str.Length, CompareOptions.Ordinal) == 0)
			{
				this.Index += str.Length - 1;
				return true;
			}
			return false;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x00064BA0 File Offset: 0x00063BA0
		internal bool Match(char ch)
		{
			if (++this.Index >= this.len)
			{
				return false;
			}
			if (this.Value[this.Index] == ch)
			{
				this.m_current = ch;
				return true;
			}
			this.Index--;
			return false;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x00064BF4 File Offset: 0x00063BF4
		internal int MatchLongestWords(string[] words, ref int maxMatchStrLen)
		{
			int num = -1;
			for (int i = 0; i < words.Length; i++)
			{
				string text = words[i];
				int length = text.Length;
				if (this.MatchSpecifiedWords(text, false, ref length) && length > maxMatchStrLen)
				{
					maxMatchStrLen = length;
					num = i;
				}
			}
			return num;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x00064C34 File Offset: 0x00063C34
		internal int GetRepeatCount()
		{
			char c = this.Value[this.Index];
			int num = this.Index + 1;
			while (num < this.len && this.Value[num] == c)
			{
				num++;
			}
			int num2 = num - this.Index;
			this.Index = num - 1;
			return num2;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00064C90 File Offset: 0x00063C90
		internal bool GetNextDigit()
		{
			return ++this.Index < this.len && DateTimeParse.IsDigit(this.Value[this.Index]);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x00064CCE File Offset: 0x00063CCE
		internal char GetChar()
		{
			return this.Value[this.Index];
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00064CE1 File Offset: 0x00063CE1
		internal int GetDigit()
		{
			return (int)(this.Value[this.Index] - '0');
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00064CF8 File Offset: 0x00063CF8
		internal void SkipWhiteSpaces()
		{
			while (this.Index + 1 < this.len)
			{
				char c = this.Value[this.Index + 1];
				if (!char.IsWhiteSpace(c))
				{
					return;
				}
				this.Index++;
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x00064D44 File Offset: 0x00063D44
		internal bool SkipWhiteSpaceCurrent()
		{
			if (this.Index >= this.len)
			{
				return false;
			}
			if (!char.IsWhiteSpace(this.m_current))
			{
				return true;
			}
			while (++this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
				if (!char.IsWhiteSpace(this.m_current))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00064DB0 File Offset: 0x00063DB0
		internal void TrimTail()
		{
			int num = this.len - 1;
			while (num >= 0 && char.IsWhiteSpace(this.Value[num]))
			{
				num--;
			}
			this.Value = this.Value.Substring(0, num + 1);
			this.len = this.Value.Length;
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00064E0C File Offset: 0x00063E0C
		internal void RemoveTrailingInQuoteSpaces()
		{
			int num = this.len - 1;
			if (num <= 1)
			{
				return;
			}
			char c = this.Value[num];
			if ((c == '\'' || c == '"') && char.IsWhiteSpace(this.Value[num - 1]))
			{
				num--;
				while (num >= 1 && char.IsWhiteSpace(this.Value[num - 1]))
				{
					num--;
				}
				this.Value = this.Value.Remove(num, this.Value.Length - 1 - num);
				this.len = this.Value.Length;
			}
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00064EA8 File Offset: 0x00063EA8
		internal void RemoveLeadingInQuoteSpaces()
		{
			if (this.len <= 2)
			{
				return;
			}
			int num = 0;
			char c = this.Value[num];
			if (c != '\'')
			{
				if (c != '"')
				{
					return;
				}
			}
			while (num + 1 < this.len && char.IsWhiteSpace(this.Value[num + 1]))
			{
				num++;
			}
			if (num != 0)
			{
				this.Value = this.Value.Remove(1, num);
				this.len = this.Value.Length;
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00064F28 File Offset: 0x00063F28
		internal DTSubString GetSubString()
		{
			DTSubString dtsubString = default(DTSubString);
			dtsubString.index = this.Index;
			dtsubString.s = this.Value;
			while (this.Index + dtsubString.length < this.len)
			{
				char c = this.Value[this.Index + dtsubString.length];
				DTSubStringType dtsubStringType;
				if (c >= '0' && c <= '9')
				{
					dtsubStringType = DTSubStringType.Number;
				}
				else
				{
					dtsubStringType = DTSubStringType.Other;
				}
				if (dtsubString.length == 0)
				{
					dtsubString.type = dtsubStringType;
				}
				else if (dtsubString.type != dtsubStringType)
				{
					break;
				}
				dtsubString.length++;
				if (dtsubStringType != DTSubStringType.Number)
				{
					break;
				}
				if (dtsubString.length > 8)
				{
					dtsubString.type = DTSubStringType.Invalid;
					return dtsubString;
				}
				int num = (int)(c - '0');
				dtsubString.value = dtsubString.value * 10 + num;
			}
			if (dtsubString.length == 0)
			{
				dtsubString.type = DTSubStringType.End;
				return dtsubString;
			}
			return dtsubString;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0006500F File Offset: 0x0006400F
		internal void ConsumeSubString(DTSubString sub)
		{
			this.Index = sub.index + sub.length;
			if (this.Index < this.len)
			{
				this.m_current = this.Value[this.Index];
			}
		}

		// Token: 0x04000FA5 RID: 4005
		internal string Value;

		// Token: 0x04000FA6 RID: 4006
		internal int Index;

		// Token: 0x04000FA7 RID: 4007
		internal int len;

		// Token: 0x04000FA8 RID: 4008
		internal char m_current;

		// Token: 0x04000FA9 RID: 4009
		private CompareInfo m_info;

		// Token: 0x04000FAA RID: 4010
		private bool m_checkDigitToken;

		// Token: 0x04000FAB RID: 4011
		private static char[] WhiteSpaceChecks = new char[] { ' ', '\u00a0' };
	}
}

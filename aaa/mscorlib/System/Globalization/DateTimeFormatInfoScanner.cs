using System;
using System.Collections;
using System.Text;

namespace System.Globalization
{
	// Token: 0x02000397 RID: 919
	internal class DateTimeFormatInfoScanner
	{
		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002548 RID: 9544 RVA: 0x00068478 File Offset: 0x00067478
		private Hashtable KnownWords
		{
			get
			{
				if (DateTimeFormatInfoScanner.m_knownWords == null)
				{
					DateTimeFormatInfoScanner.m_knownWords = new Hashtable
					{
						{
							"/",
							string.Empty
						},
						{
							"-",
							string.Empty
						},
						{
							".",
							string.Empty
						},
						{
							"年",
							string.Empty
						},
						{
							"月",
							string.Empty
						},
						{
							"日",
							string.Empty
						},
						{
							"년",
							string.Empty
						},
						{
							"월",
							string.Empty
						},
						{
							"일",
							string.Empty
						},
						{
							"시",
							string.Empty
						},
						{
							"분",
							string.Empty
						},
						{
							"초",
							string.Empty
						},
						{
							"時",
							string.Empty
						},
						{
							"时",
							string.Empty
						},
						{
							"分",
							string.Empty
						},
						{
							"秒",
							string.Empty
						}
					};
				}
				return DateTimeFormatInfoScanner.m_knownWords;
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000685A0 File Offset: 0x000675A0
		internal static int SkipWhiteSpacesAndNonLetter(string pattern, int currentIndex)
		{
			while (currentIndex < pattern.Length)
			{
				char c = pattern[currentIndex];
				if (c == '\\')
				{
					currentIndex++;
					if (currentIndex >= pattern.Length)
					{
						break;
					}
					c = pattern[currentIndex];
					if (c == '\'')
					{
						continue;
					}
				}
				if (char.IsLetter(c) || c == '\'' || c == '.')
				{
					break;
				}
				currentIndex++;
			}
			return currentIndex;
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000685F8 File Offset: 0x000675F8
		internal void AddDateWordOrPostfix(string formatPostfix, string str)
		{
			if (str.Length > 0)
			{
				if (str.Equals("."))
				{
					this.AddIgnorableSymbols(".");
					return;
				}
				if (this.KnownWords[str] == null)
				{
					if (this.m_dateWords == null)
					{
						this.m_dateWords = new ArrayList();
					}
					if (formatPostfix == "MMMM")
					{
						string text = '\ue000' + str;
						if (!this.m_dateWords.Contains(text))
						{
							this.m_dateWords.Add(text);
							return;
						}
					}
					else
					{
						if (!this.m_dateWords.Contains(str))
						{
							this.m_dateWords.Add(str);
						}
						if (str[str.Length - 1] == '.')
						{
							string text2 = str.Substring(0, str.Length - 1);
							if (!this.m_dateWords.Contains(text2))
							{
								this.m_dateWords.Add(text2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000686E0 File Offset: 0x000676E0
		internal int AddDateWords(string pattern, int index, string formatPostfix)
		{
			int num = DateTimeFormatInfoScanner.SkipWhiteSpacesAndNonLetter(pattern, index);
			if (num != index && formatPostfix != null)
			{
				formatPostfix = null;
			}
			index = num;
			StringBuilder stringBuilder = new StringBuilder();
			while (index < pattern.Length)
			{
				char c = pattern[index];
				if (c == '\'')
				{
					this.AddDateWordOrPostfix(formatPostfix, stringBuilder.ToString());
					index++;
					break;
				}
				if (c == '\\')
				{
					index++;
					if (index < pattern.Length)
					{
						stringBuilder.Append(pattern[index]);
						index++;
					}
				}
				else if (char.IsWhiteSpace(c))
				{
					this.AddDateWordOrPostfix(formatPostfix, stringBuilder.ToString());
					if (formatPostfix != null)
					{
						formatPostfix = null;
					}
					stringBuilder.Length = 0;
					index++;
				}
				else
				{
					stringBuilder.Append(c);
					index++;
				}
			}
			return index;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x00068796 File Offset: 0x00067796
		internal static int ScanRepeatChar(string pattern, char ch, int index, out int count)
		{
			count = 1;
			while (++index < pattern.Length && pattern[index] == ch)
			{
				count++;
			}
			return index;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000687BC File Offset: 0x000677BC
		internal void AddIgnorableSymbols(string text)
		{
			if (this.m_dateWords == null)
			{
				this.m_dateWords = new ArrayList();
			}
			string text2 = '\ue001' + text;
			if (!this.m_dateWords.Contains(text2))
			{
				this.m_dateWords.Add(text2);
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x00068808 File Offset: 0x00067808
		internal void ScanDateWord(string pattern)
		{
			this.m_ymdFlags = DateTimeFormatInfoScanner.FoundDatePattern.None;
			for (int i = 0; i < pattern.Length; i++)
			{
				char c = pattern[i];
				char c2 = c;
				if (c2 <= 'M')
				{
					if (c2 == '\'')
					{
						i = this.AddDateWords(pattern, i + 1, null);
						continue;
					}
					if (c2 == '.')
					{
						if (this.m_ymdFlags == DateTimeFormatInfoScanner.FoundDatePattern.FoundYMDPatternFlag)
						{
							this.AddIgnorableSymbols(".");
							this.m_ymdFlags = DateTimeFormatInfoScanner.FoundDatePattern.None;
						}
						i++;
						continue;
					}
					if (c2 == 'M')
					{
						int num;
						i = DateTimeFormatInfoScanner.ScanRepeatChar(pattern, 'M', i, out num);
						if (num >= 4 && i < pattern.Length && pattern[i] == '\'')
						{
							i = this.AddDateWords(pattern, i + 1, "MMMM");
						}
						this.m_ymdFlags |= DateTimeFormatInfoScanner.FoundDatePattern.FoundMonthPatternFlag;
						continue;
					}
				}
				else
				{
					if (c2 == '\\')
					{
						i += 2;
						continue;
					}
					if (c2 != 'd')
					{
						if (c2 == 'y')
						{
							int num;
							i = DateTimeFormatInfoScanner.ScanRepeatChar(pattern, 'y', i, out num);
							this.m_ymdFlags |= DateTimeFormatInfoScanner.FoundDatePattern.FoundYearPatternFlag;
							continue;
						}
					}
					else
					{
						int num;
						i = DateTimeFormatInfoScanner.ScanRepeatChar(pattern, 'd', i, out num);
						if (num <= 2)
						{
							this.m_ymdFlags |= DateTimeFormatInfoScanner.FoundDatePattern.FoundDayPatternFlag;
							continue;
						}
						continue;
					}
				}
				if (this.m_ymdFlags == DateTimeFormatInfoScanner.FoundDatePattern.FoundYMDPatternFlag && !char.IsWhiteSpace(c))
				{
					this.m_ymdFlags = DateTimeFormatInfoScanner.FoundDatePattern.None;
				}
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x00068940 File Offset: 0x00067940
		internal string[] GetDateWordsOfDTFI(DateTimeFormatInfo dtfi)
		{
			string[] array = dtfi.GetAllDateTimePatterns('D');
			for (int i = 0; i < array.Length; i++)
			{
				this.ScanDateWord(array[i]);
			}
			array = dtfi.GetAllDateTimePatterns('d');
			for (int i = 0; i < array.Length; i++)
			{
				this.ScanDateWord(array[i]);
			}
			array = dtfi.GetAllDateTimePatterns('y');
			for (int i = 0; i < array.Length; i++)
			{
				this.ScanDateWord(array[i]);
			}
			this.ScanDateWord(dtfi.MonthDayPattern);
			array = dtfi.GetAllDateTimePatterns('T');
			for (int i = 0; i < array.Length; i++)
			{
				this.ScanDateWord(array[i]);
			}
			array = dtfi.GetAllDateTimePatterns('t');
			for (int i = 0; i < array.Length; i++)
			{
				this.ScanDateWord(array[i]);
			}
			string[] array2 = null;
			if (this.m_dateWords != null && this.m_dateWords.Count > 0)
			{
				array2 = new string[this.m_dateWords.Count];
				for (int i = 0; i < this.m_dateWords.Count; i++)
				{
					array2[i] = (string)this.m_dateWords[i];
				}
			}
			return array2;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x00068A4D File Offset: 0x00067A4D
		internal static FORMATFLAGS GetFormatFlagGenitiveMonth(string[] monthNames, string[] genitveMonthNames, string[] abbrevMonthNames, string[] genetiveAbbrevMonthNames)
		{
			if (DateTimeFormatInfoScanner.EqualStringArrays(monthNames, genitveMonthNames) && DateTimeFormatInfoScanner.EqualStringArrays(abbrevMonthNames, genetiveAbbrevMonthNames))
			{
				return FORMATFLAGS.None;
			}
			return FORMATFLAGS.UseGenitiveMonth;
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x00068A64 File Offset: 0x00067A64
		internal static FORMATFLAGS GetFormatFlagUseSpaceInMonthNames(string[] monthNames, string[] genitveMonthNames, string[] abbrevMonthNames, string[] genetiveAbbrevMonthNames)
		{
			FORMATFLAGS formatflags = FORMATFLAGS.None;
			formatflags |= ((DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(monthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(genitveMonthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(abbrevMonthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(genetiveAbbrevMonthNames)) ? FORMATFLAGS.UseDigitPrefixInTokens : FORMATFLAGS.None);
			return formatflags | ((DateTimeFormatInfoScanner.ArrayElementsHaveSpace(monthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(genitveMonthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(abbrevMonthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(genetiveAbbrevMonthNames)) ? FORMATFLAGS.UseSpacesInMonthNames : FORMATFLAGS.None);
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x00068AC3 File Offset: 0x00067AC3
		internal static FORMATFLAGS GetFormatFlagUseSpaceInDayNames(string[] dayNames, string[] abbrevDayNames)
		{
			if (!DateTimeFormatInfoScanner.ArrayElementsHaveSpace(dayNames) && !DateTimeFormatInfoScanner.ArrayElementsHaveSpace(abbrevDayNames))
			{
				return FORMATFLAGS.None;
			}
			return FORMATFLAGS.UseSpacesInDayNames;
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x00068AD9 File Offset: 0x00067AD9
		internal static FORMATFLAGS GetFormatFlagUseHebrewCalendar(int calID)
		{
			if (calID != 8)
			{
				return FORMATFLAGS.None;
			}
			return (FORMATFLAGS)10;
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x00068AE4 File Offset: 0x00067AE4
		private static bool EqualStringArrays(string[] array1, string[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (!array1[i].Equals(array2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00068B1C File Offset: 0x00067B1C
		private static bool ArrayElementsHaveSpace(string[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array[i].Length; j++)
				{
					if (char.IsWhiteSpace(array[i][j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x00068B60 File Offset: 0x00067B60
		private static bool ArrayElementsBeginWithDigit(string[] array)
		{
			int i = 0;
			while (i < array.Length)
			{
				if (array[i].Length > 0 && array[i][0] >= '0' && array[i][0] <= '9')
				{
					int num = 1;
					while (num < array[i].Length && array[i][num] >= '0' && array[i][num] <= '9')
					{
						num++;
					}
					if (num == array[i].Length)
					{
						return false;
					}
					if (num == array[i].Length - 1)
					{
						char c = array[i][num];
						if (c == '月' || c == '월')
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x040010A2 RID: 4258
		internal const char MonthPostfixChar = '\ue000';

		// Token: 0x040010A3 RID: 4259
		internal const char IgnorableSymbolChar = '\ue001';

		// Token: 0x040010A4 RID: 4260
		internal const string CJKYearSuff = "年";

		// Token: 0x040010A5 RID: 4261
		internal const string CJKMonthSuff = "月";

		// Token: 0x040010A6 RID: 4262
		internal const string CJKDaySuff = "日";

		// Token: 0x040010A7 RID: 4263
		internal const string KoreanYearSuff = "년";

		// Token: 0x040010A8 RID: 4264
		internal const string KoreanMonthSuff = "월";

		// Token: 0x040010A9 RID: 4265
		internal const string KoreanDaySuff = "일";

		// Token: 0x040010AA RID: 4266
		internal const string KoreanHourSuff = "시";

		// Token: 0x040010AB RID: 4267
		internal const string KoreanMinuteSuff = "분";

		// Token: 0x040010AC RID: 4268
		internal const string KoreanSecondSuff = "초";

		// Token: 0x040010AD RID: 4269
		internal const string CJKHourSuff = "時";

		// Token: 0x040010AE RID: 4270
		internal const string ChineseHourSuff = "时";

		// Token: 0x040010AF RID: 4271
		internal const string CJKMinuteSuff = "分";

		// Token: 0x040010B0 RID: 4272
		internal const string CJKSecondSuff = "秒";

		// Token: 0x040010B1 RID: 4273
		internal ArrayList m_dateWords = new ArrayList();

		// Token: 0x040010B2 RID: 4274
		internal static Hashtable m_knownWords;

		// Token: 0x040010B3 RID: 4275
		private DateTimeFormatInfoScanner.FoundDatePattern m_ymdFlags;

		// Token: 0x02000398 RID: 920
		private enum FoundDatePattern
		{
			// Token: 0x040010B5 RID: 4277
			None,
			// Token: 0x040010B6 RID: 4278
			FoundYearPatternFlag,
			// Token: 0x040010B7 RID: 4279
			FoundMonthPatternFlag,
			// Token: 0x040010B8 RID: 4280
			FoundDayPatternFlag = 4,
			// Token: 0x040010B9 RID: 4281
			FoundYMDPatternFlag = 7
		}
	}
}

using System;
using System.Collections;
using System.Text;

namespace System.Globalization
{
	// Token: 0x02000012 RID: 18
	internal class DateTimeFormatInfoScanner
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000A9B0 File Offset: 0x000099B0
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

		// Token: 0x060000E3 RID: 227 RVA: 0x0000AAD8 File Offset: 0x00009AD8
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

		// Token: 0x060000E4 RID: 228 RVA: 0x0000AB30 File Offset: 0x00009B30
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

		// Token: 0x060000E5 RID: 229 RVA: 0x0000AC18 File Offset: 0x00009C18
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

		// Token: 0x060000E6 RID: 230 RVA: 0x0000ACCE File Offset: 0x00009CCE
		internal static int ScanRepeatChar(string pattern, char ch, int index, out int count)
		{
			count = 1;
			while (++index < pattern.Length && pattern[index] == ch)
			{
				count++;
			}
			return index;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000ACF4 File Offset: 0x00009CF4
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

		// Token: 0x060000E8 RID: 232 RVA: 0x0000AD40 File Offset: 0x00009D40
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

		// Token: 0x060000E9 RID: 233 RVA: 0x0000AE78 File Offset: 0x00009E78
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

		// Token: 0x060000EA RID: 234 RVA: 0x0000AF85 File Offset: 0x00009F85
		internal static FORMATFLAGS GetFormatFlagGenitiveMonth(string[] monthNames, string[] genitveMonthNames, string[] abbrevMonthNames, string[] genetiveAbbrevMonthNames)
		{
			if (DateTimeFormatInfoScanner.EqualStringArrays(monthNames, genitveMonthNames) && DateTimeFormatInfoScanner.EqualStringArrays(abbrevMonthNames, genetiveAbbrevMonthNames))
			{
				return FORMATFLAGS.None;
			}
			return FORMATFLAGS.UseGenitiveMonth;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000AF9C File Offset: 0x00009F9C
		internal static FORMATFLAGS GetFormatFlagUseSpaceInMonthNames(string[] monthNames, string[] genitveMonthNames, string[] abbrevMonthNames, string[] genetiveAbbrevMonthNames)
		{
			FORMATFLAGS formatflags = FORMATFLAGS.None;
			formatflags |= ((DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(monthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(genitveMonthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(abbrevMonthNames) || DateTimeFormatInfoScanner.ArrayElementsBeginWithDigit(genetiveAbbrevMonthNames)) ? FORMATFLAGS.UseDigitPrefixInTokens : FORMATFLAGS.None);
			return formatflags | ((DateTimeFormatInfoScanner.ArrayElementsHaveSpace(monthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(genitveMonthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(abbrevMonthNames) || DateTimeFormatInfoScanner.ArrayElementsHaveSpace(genetiveAbbrevMonthNames)) ? FORMATFLAGS.UseSpacesInMonthNames : FORMATFLAGS.None);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000AFFB File Offset: 0x00009FFB
		internal static FORMATFLAGS GetFormatFlagUseSpaceInDayNames(string[] dayNames, string[] abbrevDayNames)
		{
			if (!DateTimeFormatInfoScanner.ArrayElementsHaveSpace(dayNames) && !DateTimeFormatInfoScanner.ArrayElementsHaveSpace(abbrevDayNames))
			{
				return FORMATFLAGS.None;
			}
			return FORMATFLAGS.UseSpacesInDayNames;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000B011 File Offset: 0x0000A011
		internal static FORMATFLAGS GetFormatFlagUseHebrewCalendar(int calID)
		{
			if (calID != 8)
			{
				return FORMATFLAGS.None;
			}
			return (FORMATFLAGS)10;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000B01C File Offset: 0x0000A01C
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

		// Token: 0x060000EF RID: 239 RVA: 0x0000B054 File Offset: 0x0000A054
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

		// Token: 0x060000F0 RID: 240 RVA: 0x0000B098 File Offset: 0x0000A098
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

		// Token: 0x040001D0 RID: 464
		internal const char MonthPostfixChar = '\ue000';

		// Token: 0x040001D1 RID: 465
		internal const char IgnorableSymbolChar = '\ue001';

		// Token: 0x040001D2 RID: 466
		internal const string CJKYearSuff = "年";

		// Token: 0x040001D3 RID: 467
		internal const string CJKMonthSuff = "月";

		// Token: 0x040001D4 RID: 468
		internal const string CJKDaySuff = "日";

		// Token: 0x040001D5 RID: 469
		internal const string KoreanYearSuff = "년";

		// Token: 0x040001D6 RID: 470
		internal const string KoreanMonthSuff = "월";

		// Token: 0x040001D7 RID: 471
		internal const string KoreanDaySuff = "일";

		// Token: 0x040001D8 RID: 472
		internal const string KoreanHourSuff = "시";

		// Token: 0x040001D9 RID: 473
		internal const string KoreanMinuteSuff = "분";

		// Token: 0x040001DA RID: 474
		internal const string KoreanSecondSuff = "초";

		// Token: 0x040001DB RID: 475
		internal const string CJKHourSuff = "時";

		// Token: 0x040001DC RID: 476
		internal const string ChineseHourSuff = "时";

		// Token: 0x040001DD RID: 477
		internal const string CJKMinuteSuff = "分";

		// Token: 0x040001DE RID: 478
		internal const string CJKSecondSuff = "秒";

		// Token: 0x040001DF RID: 479
		internal ArrayList m_dateWords = new ArrayList();

		// Token: 0x040001E0 RID: 480
		internal static Hashtable m_knownWords;

		// Token: 0x040001E1 RID: 481
		private DateTimeFormatInfoScanner.FoundDatePattern m_ymdFlags;

		// Token: 0x02000013 RID: 19
		private enum FoundDatePattern
		{
			// Token: 0x040001E3 RID: 483
			None,
			// Token: 0x040001E4 RID: 484
			FoundYearPatternFlag,
			// Token: 0x040001E5 RID: 485
			FoundMonthPatternFlag,
			// Token: 0x040001E6 RID: 486
			FoundDayPatternFlag = 4,
			// Token: 0x040001E7 RID: 487
			FoundYMDPatternFlag = 7
		}
	}
}

using System;
using System.Globalization;
using System.Text;

namespace System
{
	// Token: 0x02000381 RID: 897
	internal static class DateTimeParse
	{
		// Token: 0x06002453 RID: 9299 RVA: 0x0005F434 File Offset: 0x0005E434
		internal static DateTime ParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0005F46C File Offset: 0x0005E46C
		internal static DateTime ParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, out TimeSpan offset)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			offset = TimeSpan.Zero;
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				offset = dateTimeResult.timeZoneOffset;
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0005F4D4 File Offset: 0x0005E4D4
		internal static bool TryParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result)
		{
			result = DateTime.MinValue;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				return true;
			}
			return false;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0005F51C File Offset: 0x0005E51C
		internal static bool TryParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result, out TimeSpan offset)
		{
			result = DateTime.MinValue;
			offset = TimeSpan.Zero;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExact(s, format, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				offset = dateTimeResult.timeZoneOffset;
				return true;
			}
			return false;
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0005F590 File Offset: 0x0005E590
		internal static bool TryParseExact(string s, string format, DateTimeFormatInfo dtfi, DateTimeStyles style, ref DateTimeResult result)
		{
			if (s == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "s");
				return false;
			}
			if (format == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "format");
				return false;
			}
			if (s.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (format.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
				return false;
			}
			return DateTimeParse.DoStrictParse(s, format, style, dtfi, ref result);
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0005F608 File Offset: 0x0005E608
		internal static DateTime ParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0005F640 File Offset: 0x0005E640
		internal static DateTime ParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, out TimeSpan offset)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			offset = TimeSpan.Zero;
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				offset = dateTimeResult.timeZoneOffset;
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0005F6A8 File Offset: 0x0005E6A8
		internal static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result, out TimeSpan offset)
		{
			result = DateTime.MinValue;
			offset = TimeSpan.Zero;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				offset = dateTimeResult.timeZoneOffset;
				return true;
			}
			return false;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0005F71C File Offset: 0x0005E71C
		internal static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, out DateTime result)
		{
			result = DateTime.MinValue;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParseExactMultiple(s, formats, dtfi, style, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				return true;
			}
			return false;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x0005F764 File Offset: 0x0005E764
		internal static bool TryParseExactMultiple(string s, string[] formats, DateTimeFormatInfo dtfi, DateTimeStyles style, ref DateTimeResult result)
		{
			if (s == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "s");
				return false;
			}
			if (formats == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "formats");
				return false;
			}
			if (s.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (formats.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
				return false;
			}
			for (int i = 0; i < formats.Length; i++)
			{
				if (formats[i] == null || formats[i].Length == 0)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
					return false;
				}
				DateTimeResult dateTimeResult = default(DateTimeResult);
				dateTimeResult.Init();
				dateTimeResult.flags = result.flags;
				if (DateTimeParse.TryParseExact(s, formats[i], dtfi, style, ref dateTimeResult))
				{
					result.parsedDate = dateTimeResult.parsedDate;
					result.timeZoneOffset = dateTimeResult.timeZoneOffset;
					return true;
				}
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x0005F854 File Offset: 0x0005E854
		private static bool MatchWord(ref __DTString str, string target)
		{
			int length = target.Length;
			if (length > str.Value.Length - str.Index)
			{
				return false;
			}
			if (str.CompareInfo.Compare(str.Value, str.Index, length, target, 0, length, CompareOptions.IgnoreCase) != 0)
			{
				return false;
			}
			int num = str.Index + target.Length;
			if (num < str.Value.Length)
			{
				char c = str.Value[num];
				if (char.IsLetter(c))
				{
					return false;
				}
			}
			str.Index = num;
			if (str.Index < str.len)
			{
				str.m_current = str.Value[str.Index];
			}
			return true;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0005F8FF File Offset: 0x0005E8FF
		private static bool GetTimeZoneName(ref __DTString str)
		{
			return DateTimeParse.MatchWord(ref str, "GMT") || DateTimeParse.MatchWord(ref str, "Z");
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0005F920 File Offset: 0x0005E920
		internal static bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0005F934 File Offset: 0x0005E934
		private static bool ParseFraction(ref __DTString str, out double result)
		{
			result = 0.0;
			double num = 0.1;
			int num2 = 0;
			char current;
			while (str.GetNext() && DateTimeParse.IsDigit(current = str.m_current))
			{
				result += (double)(current - '0') * num;
				num *= 0.1;
				num2++;
			}
			return num2 > 0;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0005F994 File Offset: 0x0005E994
		private static bool ParseTimeZone(ref __DTString str, ref TimeSpan result)
		{
			int num = 0;
			DTSubString dtsubString = str.GetSubString();
			if (dtsubString.length != 1)
			{
				return false;
			}
			char c = dtsubString[0];
			if (c != '+' && c != '-')
			{
				return false;
			}
			str.ConsumeSubString(dtsubString);
			dtsubString = str.GetSubString();
			if (dtsubString.type != DTSubStringType.Number)
			{
				return false;
			}
			int value = dtsubString.value;
			int length = dtsubString.length;
			int num2;
			if (length == 1 || length == 2)
			{
				num2 = value;
				str.ConsumeSubString(dtsubString);
				dtsubString = str.GetSubString();
				if (dtsubString.length == 1 && dtsubString[0] == ':')
				{
					str.ConsumeSubString(dtsubString);
					dtsubString = str.GetSubString();
					if (dtsubString.type != DTSubStringType.Number || dtsubString.length < 1 || dtsubString.length > 2)
					{
						return false;
					}
					num = dtsubString.value;
					str.ConsumeSubString(dtsubString);
				}
			}
			else
			{
				if (length != 3 && length != 4)
				{
					return false;
				}
				num2 = value / 100;
				num = value % 100;
				str.ConsumeSubString(dtsubString);
			}
			if (num < 0 || num >= 60)
			{
				return false;
			}
			result = new TimeSpan(num2, num, 0);
			if (c == '-')
			{
				result = result.Negate();
			}
			return true;
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0005FAB8 File Offset: 0x0005EAB8
		private static bool Lex(DateTimeParse.DS dps, ref __DTString str, ref DateTimeToken dtok, ref DateTimeRawInfo raw, ref DateTimeResult result, ref DateTimeFormatInfo dtfi)
		{
			dtok.dtt = DateTimeParse.DTT.Unk;
			TokenType tokenType;
			int num;
			str.GetRegularToken(out tokenType, out num, dtfi);
			switch (tokenType)
			{
			case TokenType.NumberToken:
			case TokenType.YearNumberToken:
				if (raw.numCount == 3 || num == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (dps == DateTimeParse.DS.T_NNt && str.Index < str.len - 1)
				{
					char c = str.Value[str.Index];
					if (c == '.')
					{
						DateTimeParse.ParseFraction(ref str, out raw.fraction);
					}
				}
				if ((dps == DateTimeParse.DS.T_NNt || dps == DateTimeParse.DS.T_Nt) && str.Index < str.len - 1)
				{
					char c2 = str.Value[str.Index];
					int num2 = 0;
					while (char.IsWhiteSpace(c2) && str.Index + num2 < str.len - 1)
					{
						num2++;
						c2 = str.Value[str.Index + num2];
					}
					if (c2 == '+' || c2 == '-')
					{
						str.Index += num2;
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0)
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						result.flags |= ParseFlags.TimeZoneUsed;
						if (!DateTimeParse.ParseTimeZone(ref str, ref result.timeZoneOffset))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
				}
				dtok.num = num;
				if (tokenType != TokenType.YearNumberToken)
				{
					int num3;
					char c3;
					TokenType separatorToken;
					TokenType tokenType2 = (separatorToken = str.GetSeparatorToken(dtfi, out num3, out c3));
					if (separatorToken > TokenType.SEP_YearSuff)
					{
						if (separatorToken <= TokenType.SEP_HourSuff)
						{
							if (separatorToken == TokenType.SEP_MonthSuff || separatorToken == TokenType.SEP_DaySuff)
							{
								dtok.dtt = DateTimeParse.DTT.NumDatesuff;
								dtok.suffix = tokenType2;
								break;
							}
							if (separatorToken != TokenType.SEP_HourSuff)
							{
								goto IL_05C7;
							}
						}
						else if (separatorToken <= TokenType.SEP_SecondSuff)
						{
							if (separatorToken != TokenType.SEP_MinuteSuff && separatorToken != TokenType.SEP_SecondSuff)
							{
								goto IL_05C7;
							}
						}
						else
						{
							if (separatorToken == TokenType.SEP_LocalTimeMark)
							{
								dtok.dtt = DateTimeParse.DTT.NumLocalTimeMark;
								raw.AddNumber(dtok.num);
								break;
							}
							if (separatorToken != TokenType.SEP_DateOrOffset)
							{
								goto IL_05C7;
							}
							if (DateTimeParse.dateParsingStates[(int)dps][4] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][3] > DateTimeParse.DS.ERROR)
							{
								str.Index = num3;
								str.m_current = c3;
								dtok.dtt = DateTimeParse.DTT.NumSpace;
							}
							else
							{
								dtok.dtt = DateTimeParse.DTT.NumDatesep;
							}
							raw.AddNumber(dtok.num);
							break;
						}
						dtok.dtt = DateTimeParse.DTT.NumTimesuff;
						dtok.suffix = tokenType2;
						break;
					}
					if (separatorToken <= TokenType.SEP_Am)
					{
						if (separatorToken == TokenType.SEP_End)
						{
							dtok.dtt = DateTimeParse.DTT.NumEnd;
							raw.AddNumber(dtok.num);
							break;
						}
						if (separatorToken == TokenType.SEP_Space)
						{
							dtok.dtt = DateTimeParse.DTT.NumSpace;
							raw.AddNumber(dtok.num);
							break;
						}
						if (separatorToken != TokenType.SEP_Am)
						{
							goto IL_05C7;
						}
					}
					else if (separatorToken <= TokenType.SEP_Date)
					{
						if (separatorToken != TokenType.SEP_Pm)
						{
							if (separatorToken != TokenType.SEP_Date)
							{
								goto IL_05C7;
							}
							dtok.dtt = DateTimeParse.DTT.NumDatesep;
							raw.AddNumber(dtok.num);
							break;
						}
					}
					else
					{
						if (separatorToken == TokenType.SEP_Time)
						{
							dtok.dtt = DateTimeParse.DTT.NumTimesep;
							raw.AddNumber(dtok.num);
							break;
						}
						if (separatorToken != TokenType.SEP_YearSuff)
						{
							goto IL_05C7;
						}
						dtok.num = dtfi.Calendar.ToFourDigitYear(num);
						dtok.dtt = DateTimeParse.DTT.NumDatesuff;
						dtok.suffix = tokenType2;
						break;
					}
					if (raw.timeMark == DateTimeParse.TM.NotSet)
					{
						raw.timeMark = ((tokenType2 == TokenType.SEP_Am) ? DateTimeParse.TM.AM : DateTimeParse.TM.PM);
						dtok.dtt = DateTimeParse.DTT.NumAmpm;
						raw.AddNumber(dtok.num);
						break;
					}
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					break;
					IL_05C7:
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (raw.year == -1)
				{
					raw.year = num;
					int num3;
					char c3;
					TokenType separatorToken2;
					TokenType tokenType2 = (separatorToken2 = str.GetSeparatorToken(dtfi, out num3, out c3));
					if (separatorToken2 > TokenType.SEP_YearSuff)
					{
						if (separatorToken2 <= TokenType.SEP_HourSuff)
						{
							if (separatorToken2 == TokenType.SEP_MonthSuff || separatorToken2 == TokenType.SEP_DaySuff)
							{
								goto IL_0320;
							}
							if (separatorToken2 != TokenType.SEP_HourSuff)
							{
								goto IL_0344;
							}
						}
						else if (separatorToken2 != TokenType.SEP_MinuteSuff && separatorToken2 != TokenType.SEP_SecondSuff)
						{
							if (separatorToken2 != TokenType.SEP_DateOrOffset)
							{
								goto IL_0344;
							}
							if (DateTimeParse.dateParsingStates[(int)dps][13] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][12] > DateTimeParse.DS.ERROR)
							{
								str.Index = num3;
								str.m_current = c3;
								dtok.dtt = DateTimeParse.DTT.YearSpace;
								return true;
							}
							dtok.dtt = DateTimeParse.DTT.YearDateSep;
							return true;
						}
						dtok.dtt = DateTimeParse.DTT.NumTimesuff;
						dtok.suffix = tokenType2;
						return true;
					}
					if (separatorToken2 <= TokenType.SEP_Am)
					{
						if (separatorToken2 == TokenType.SEP_End)
						{
							dtok.dtt = DateTimeParse.DTT.YearEnd;
							return true;
						}
						if (separatorToken2 == TokenType.SEP_Space)
						{
							dtok.dtt = DateTimeParse.DTT.YearSpace;
							return true;
						}
						if (separatorToken2 != TokenType.SEP_Am)
						{
							goto IL_0344;
						}
					}
					else if (separatorToken2 != TokenType.SEP_Pm)
					{
						if (separatorToken2 == TokenType.SEP_Date)
						{
							dtok.dtt = DateTimeParse.DTT.YearDateSep;
							return true;
						}
						if (separatorToken2 != TokenType.SEP_YearSuff)
						{
							goto IL_0344;
						}
						goto IL_0320;
					}
					if (raw.timeMark == DateTimeParse.TM.NotSet)
					{
						raw.timeMark = ((tokenType2 == TokenType.SEP_Am) ? DateTimeParse.TM.AM : DateTimeParse.TM.PM);
						dtok.dtt = DateTimeParse.DTT.YearSpace;
						return true;
					}
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return true;
					IL_0320:
					dtok.dtt = DateTimeParse.DTT.NumDatesuff;
					dtok.suffix = tokenType2;
					return true;
					IL_0344:
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			case TokenType.Am:
			case TokenType.Pm:
				if (raw.timeMark != DateTimeParse.TM.NotSet)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				raw.timeMark = (DateTimeParse.TM)num;
				break;
			case TokenType.MonthToken:
			{
				if (raw.month != -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				int num3;
				char c3;
				TokenType separatorToken3 = str.GetSeparatorToken(dtfi, out num3, out c3);
				if (separatorToken3 <= TokenType.SEP_Space)
				{
					if (separatorToken3 == TokenType.SEP_End)
					{
						dtok.dtt = DateTimeParse.DTT.MonthEnd;
						goto IL_0823;
					}
					if (separatorToken3 == TokenType.SEP_Space)
					{
						dtok.dtt = DateTimeParse.DTT.MonthSpace;
						goto IL_0823;
					}
				}
				else
				{
					if (separatorToken3 == TokenType.SEP_Date)
					{
						dtok.dtt = DateTimeParse.DTT.MonthDatesep;
						goto IL_0823;
					}
					if (separatorToken3 == TokenType.SEP_DateOrOffset)
					{
						if (DateTimeParse.dateParsingStates[(int)dps][8] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][7] > DateTimeParse.DS.ERROR)
						{
							str.Index = num3;
							str.m_current = c3;
							dtok.dtt = DateTimeParse.DTT.MonthSpace;
							goto IL_0823;
						}
						dtok.dtt = DateTimeParse.DTT.MonthDatesep;
						goto IL_0823;
					}
				}
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
				IL_0823:
				raw.month = num;
				break;
			}
			case TokenType.EndOfString:
				dtok.dtt = DateTimeParse.DTT.End;
				break;
			case TokenType.DayOfWeekToken:
				if (raw.dayOfWeek != -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				raw.dayOfWeek = num;
				dtok.dtt = DateTimeParse.DTT.DayOfWeek;
				break;
			case TokenType.TimeZoneToken:
				dtok.dtt = DateTimeParse.DTT.TimeZone;
				result.flags |= ParseFlags.TimeZoneUsed;
				result.timeZoneOffset = new TimeSpan(0L);
				result.flags |= ParseFlags.TimeZoneUtc;
				break;
			case TokenType.EraToken:
				if (result.era == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.era = num;
				dtok.dtt = DateTimeParse.DTT.Era;
				break;
			case TokenType.UnknownToken:
				if (char.IsLetter(str.m_current))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_UnknowDateTimeWord", str.Index);
					return false;
				}
				if (Environment.GetCompatibilityFlag(CompatibilityFlag.DateTimeParseIgnorePunctuation) && (result.flags & ParseFlags.CaptureOffset) == (ParseFlags)0)
				{
					str.GetNext();
					return true;
				}
				if ((str.m_current == '-' || str.m_current == '+') && (result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0)
				{
					int index = str.Index;
					if (DateTimeParse.ParseTimeZone(ref str, ref result.timeZoneOffset))
					{
						result.flags |= ParseFlags.TimeZoneUsed;
						return true;
					}
					str.Index = index;
				}
				if (DateTimeParse.VerifyValidPunctuation(ref str))
				{
					return true;
				}
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			case TokenType.HebrewNumber:
			{
				int num3;
				char c3;
				if (num < 100)
				{
					dtok.num = num;
					raw.AddNumber(dtok.num);
					TokenType separatorToken4 = str.GetSeparatorToken(dtfi, out num3, out c3);
					if (separatorToken4 <= TokenType.SEP_Space)
					{
						if (separatorToken4 == TokenType.SEP_End)
						{
							dtok.dtt = DateTimeParse.DTT.NumEnd;
							break;
						}
						if (separatorToken4 != TokenType.SEP_Space)
						{
							goto IL_0732;
						}
					}
					else if (separatorToken4 != TokenType.SEP_Date)
					{
						if (separatorToken4 != TokenType.SEP_DateOrOffset)
						{
							goto IL_0732;
						}
						if (DateTimeParse.dateParsingStates[(int)dps][4] == DateTimeParse.DS.ERROR && DateTimeParse.dateParsingStates[(int)dps][3] > DateTimeParse.DS.ERROR)
						{
							str.Index = num3;
							str.m_current = c3;
							dtok.dtt = DateTimeParse.DTT.NumSpace;
							break;
						}
						dtok.dtt = DateTimeParse.DTT.NumDatesep;
						break;
					}
					dtok.dtt = DateTimeParse.DTT.NumDatesep;
					break;
					IL_0732:
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (raw.year != -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				raw.year = num;
				TokenType separatorToken5 = str.GetSeparatorToken(dtfi, out num3, out c3);
				if (separatorToken5 != TokenType.SEP_End)
				{
					if (separatorToken5 != TokenType.SEP_Space)
					{
						if (separatorToken5 == TokenType.SEP_DateOrOffset)
						{
							if (DateTimeParse.dateParsingStates[(int)dps][12] > DateTimeParse.DS.ERROR)
							{
								str.Index = num3;
								str.m_current = c3;
								dtok.dtt = DateTimeParse.DTT.YearSpace;
								break;
							}
						}
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					dtok.dtt = DateTimeParse.DTT.YearSpace;
				}
				else
				{
					dtok.dtt = DateTimeParse.DTT.YearEnd;
				}
				break;
			}
			case TokenType.JapaneseEraToken:
				result.calendar = JapaneseCalendar.GetDefaultInstance();
				dtfi = DateTimeFormatInfo.GetJapaneseCalendarDTFI();
				if (result.era == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.era = num;
				dtok.dtt = DateTimeParse.DTT.Era;
				break;
			case TokenType.TEraToken:
				result.calendar = TaiwanCalendar.GetDefaultInstance();
				dtfi = DateTimeFormatInfo.GetTaiwanCalendarDTFI();
				if (result.era == -1)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.era = num;
				dtok.dtt = DateTimeParse.DTT.Era;
				break;
			}
			return true;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000604E4 File Offset: 0x0005F4E4
		private static bool VerifyValidPunctuation(ref __DTString str)
		{
			char c = str.Value[str.Index];
			if (c == '#')
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < str.len; i++)
				{
					c = str.Value[i];
					if (c == '#')
					{
						if (flag)
						{
							if (flag2)
							{
								return false;
							}
							flag2 = true;
						}
						else
						{
							flag = true;
						}
					}
					else if (c == '\0')
					{
						if (!flag2)
						{
							return false;
						}
					}
					else if (!char.IsWhiteSpace(c) && (!flag || flag2))
					{
						return false;
					}
				}
				if (!flag2)
				{
					return false;
				}
				str.GetNext();
				return true;
			}
			else
			{
				if (c == '\0')
				{
					for (int j = str.Index; j < str.len; j++)
					{
						if (str.Value[j] != '\0')
						{
							return false;
						}
					}
					str.Index = str.len;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000605A4 File Offset: 0x0005F5A4
		private static bool GetYearMonthDayOrder(string datePattern, DateTimeFormatInfo dtfi, out int order)
		{
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = 0;
			bool flag = false;
			int num5 = 0;
			while (num5 < datePattern.Length && num4 < 3)
			{
				char c = datePattern[num5];
				if (c == '\'' || c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == 'y')
					{
						num = num4++;
						while (num5 + 1 < datePattern.Length)
						{
							if (datePattern[num5 + 1] != 'y')
							{
								break;
							}
							num5++;
						}
					}
					else if (c == 'M')
					{
						num2 = num4++;
						while (num5 + 1 < datePattern.Length)
						{
							if (datePattern[num5 + 1] != 'M')
							{
								break;
							}
							num5++;
						}
					}
					else if (c == 'd')
					{
						int num6 = 1;
						while (num5 + 1 < datePattern.Length && datePattern[num5 + 1] == 'd')
						{
							num6++;
							num5++;
						}
						if (num6 <= 2)
						{
							num3 = num4++;
						}
					}
				}
				num5++;
			}
			if (num == 0 && num2 == 1 && num3 == 2)
			{
				order = 0;
				return true;
			}
			if (num2 == 0 && num3 == 1 && num == 2)
			{
				order = 1;
				return true;
			}
			if (num3 == 0 && num2 == 1 && num == 2)
			{
				order = 2;
				return true;
			}
			if (num == 0 && num3 == 1 && num2 == 2)
			{
				order = 3;
				return true;
			}
			order = -1;
			return false;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000606E0 File Offset: 0x0005F6E0
		private static bool GetYearMonthOrder(string pattern, DateTimeFormatInfo dtfi, out int order)
		{
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			bool flag = false;
			int num4 = 0;
			while (num4 < pattern.Length && num3 < 2)
			{
				char c = pattern[num4];
				if (c == '\'' || c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == 'y')
					{
						num = num3++;
						while (num4 + 1 < pattern.Length)
						{
							if (pattern[num4 + 1] != 'y')
							{
								break;
							}
							num4++;
						}
					}
					else if (c == 'M')
					{
						num2 = num3++;
						while (num4 + 1 < pattern.Length && pattern[num4 + 1] == 'M')
						{
							num4++;
						}
					}
				}
				num4++;
			}
			if (num == 0 && num2 == 1)
			{
				order = 4;
				return true;
			}
			if (num2 == 0 && num == 1)
			{
				order = 5;
				return true;
			}
			order = -1;
			return false;
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000607AC File Offset: 0x0005F7AC
		private static bool GetMonthDayOrder(string pattern, DateTimeFormatInfo dtfi, out int order)
		{
			int num = -1;
			int num2 = -1;
			int num3 = 0;
			bool flag = false;
			int num4 = 0;
			while (num4 < pattern.Length && num3 < 2)
			{
				char c = pattern[num4];
				if (c == '\'' || c == '"')
				{
					flag = !flag;
				}
				if (!flag)
				{
					if (c == 'd')
					{
						int num5 = 1;
						while (num4 + 1 < pattern.Length && pattern[num4 + 1] == 'd')
						{
							num5++;
							num4++;
						}
						if (num5 <= 2)
						{
							num2 = num3++;
						}
					}
					else if (c == 'M')
					{
						num = num3++;
						while (num4 + 1 < pattern.Length && pattern[num4 + 1] == 'M')
						{
							num4++;
						}
					}
				}
				num4++;
			}
			if (num == 0 && num2 == 1)
			{
				order = 6;
				return true;
			}
			if (num2 == 0 && num == 1)
			{
				order = 7;
				return true;
			}
			order = -1;
			return false;
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x00060886 File Offset: 0x0005F886
		private static int AdjustYear(ref DateTimeResult result, int year)
		{
			if (year < 100)
			{
				year = result.calendar.ToFourDigitYear(year);
			}
			return year;
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0006089C File Offset: 0x0005F89C
		private static bool SetDateYMD(ref DateTimeResult result, int year, int month, int day)
		{
			if (result.calendar.IsValidDay(year, month, day, result.era))
			{
				result.SetDate(year, month, day);
				return true;
			}
			return false;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000608C0 File Offset: 0x0005F8C0
		private static bool SetDateMDY(ref DateTimeResult result, int month, int day, int year)
		{
			return DateTimeParse.SetDateYMD(ref result, year, month, day);
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000608CB File Offset: 0x0005F8CB
		private static bool SetDateDMY(ref DateTimeResult result, int day, int month, int year)
		{
			return DateTimeParse.SetDateYMD(ref result, year, month, day);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000608D6 File Offset: 0x0005F8D6
		private static bool SetDateYDM(ref DateTimeResult result, int year, int day, int month)
		{
			return DateTimeParse.SetDateYMD(ref result, year, month, day);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000608E1 File Offset: 0x0005F8E1
		private static void GetDefaultYear(ref DateTimeResult result, ref DateTimeStyles styles)
		{
			result.Year = result.calendar.GetYear(DateTimeParse.GetDateTimeNow(ref result, ref styles));
			result.flags |= ParseFlags.YearDefault;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x00060910 File Offset: 0x0005F910
		private static bool GetDayOfNN(ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			DateTimeParse.GetDefaultYear(ref result, ref styles);
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			if (num == 6)
			{
				if (DateTimeParse.SetDateYMD(ref result, result.Year, number, number2))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (DateTimeParse.SetDateYMD(ref result, result.Year, number2, number))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000609D0 File Offset: 0x0005F9D0
		private static bool GetDayOfNNN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			int number3 = raw.GetNumber(2);
			int num;
			if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
				return false;
			}
			if (num == 0)
			{
				if (DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, number), number2, number3))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 1)
			{
				if (DateTimeParse.SetDateMDY(ref result, number, number2, DateTimeParse.AdjustYear(ref result, number3)))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 2)
			{
				if (DateTimeParse.SetDateDMY(ref result, number, number2, DateTimeParse.AdjustYear(ref result, number3)))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 3 && DateTimeParse.SetDateYDM(ref result, DateTimeParse.AdjustYear(ref result, number), number2, number3))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x00060AEC File Offset: 0x0005FAEC
		private static bool GetDayOfMN(ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			if (num == 7)
			{
				int num2;
				if (!DateTimeParse.GetYearMonthOrder(dtfi.YearMonthPattern, dtfi, out num2))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.YearMonthPattern);
					return false;
				}
				if (num2 == 5)
				{
					if (!DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(0)), raw.month, 1))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					return true;
				}
			}
			DateTimeParse.GetDefaultYear(ref result, ref styles);
			if (!DateTimeParse.SetDateYMD(ref result, result.Year, raw.month, raw.GetNumber(0)))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			return true;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x00060BC8 File Offset: 0x0005FBC8
		private static bool GetHebrewDayOfNM(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			result.Month = raw.month;
			if (num == 7 && result.calendar.IsValidDay(result.Year, result.Month, raw.GetNumber(0), result.era))
			{
				result.Day = raw.GetNumber(0);
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x00060C4C File Offset: 0x0005FC4C
		private static bool GetDayOfNM(ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int num;
			if (!DateTimeParse.GetMonthDayOrder(dtfi.MonthDayPattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.MonthDayPattern);
				return false;
			}
			if (num == 6)
			{
				int num2;
				if (!DateTimeParse.GetYearMonthOrder(dtfi.YearMonthPattern, dtfi, out num2))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.YearMonthPattern);
					return false;
				}
				if (num2 == 4)
				{
					if (!DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(0)), raw.month, 1))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					return true;
				}
			}
			DateTimeParse.GetDefaultYear(ref result, ref styles);
			if (!DateTimeParse.SetDateYMD(ref result, result.Year, raw.month, raw.GetNumber(0)))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			return true;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x00060D28 File Offset: 0x0005FD28
		private static bool GetDayOfMNN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			int num;
			if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
				return false;
			}
			if (num == 1)
			{
				int num2;
				if (result.calendar.IsValidDay(num2 = DateTimeParse.AdjustYear(ref result, number2), raw.month, number, result.era))
				{
					result.SetDate(num2, raw.month, number);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
				if (result.calendar.IsValidDay(num2 = DateTimeParse.AdjustYear(ref result, number), raw.month, number2, result.era))
				{
					result.SetDate(num2, raw.month, number2);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 0)
			{
				int num2;
				if (result.calendar.IsValidDay(num2 = DateTimeParse.AdjustYear(ref result, number), raw.month, number2, result.era))
				{
					result.SetDate(num2, raw.month, number2);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
				if (result.calendar.IsValidDay(num2 = DateTimeParse.AdjustYear(ref result, number2), raw.month, number, result.era))
				{
					result.SetDate(num2, raw.month, number);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (num == 2)
			{
				int num2;
				if (result.calendar.IsValidDay(num2 = DateTimeParse.AdjustYear(ref result, number2), raw.month, number, result.era))
				{
					result.SetDate(num2, raw.month, number);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
				if (result.calendar.IsValidDay(num2 = DateTimeParse.AdjustYear(ref result, number), raw.month, number2, result.era))
				{
					result.SetDate(num2, raw.month, number2);
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00060F4C File Offset: 0x0005FF4C
		private static bool GetDayOfYNN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			string text = dtfi.ShortDatePattern;
			if (dtfi.CultureId == 1079)
			{
				text = dtfi.LongDatePattern;
			}
			int num;
			if (DateTimeParse.GetYearMonthDayOrder(text, dtfi, out num) && num == 3)
			{
				if (DateTimeParse.SetDateYMD(ref result, raw.year, number2, number))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (DateTimeParse.SetDateYMD(ref result, raw.year, number, number2))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x00061008 File Offset: 0x00060008
		private static bool GetDayOfNNY(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			int number = raw.GetNumber(0);
			int number2 = raw.GetNumber(1);
			int num;
			if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
				return false;
			}
			if (num == 1 || num == 0)
			{
				if (DateTimeParse.SetDateYMD(ref result, raw.year, number, number2))
				{
					result.flags |= ParseFlags.HaveDate;
					return true;
				}
			}
			else if (DateTimeParse.SetDateYMD(ref result, raw.year, number2, number))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000610C4 File Offset: 0x000600C4
		private static bool GetDayOfYMN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (DateTimeParse.SetDateYMD(ref result, raw.year, raw.month, raw.GetNumber(0)))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0006112C File Offset: 0x0006012C
		private static bool GetDayOfYN(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (DateTimeParse.SetDateYMD(ref result, raw.year, raw.GetNumber(0), 1))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00061190 File Offset: 0x00060190
		private static bool GetDayOfYM(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveDate) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (DateTimeParse.SetDateYMD(ref result, raw.year, raw.month, 1))
			{
				result.flags |= ParseFlags.HaveDate;
				return true;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000611F4 File Offset: 0x000601F4
		private static void AdjustTimeMark(DateTimeFormatInfo dtfi, ref DateTimeRawInfo raw)
		{
			if (raw.timeMark == DateTimeParse.TM.NotSet && dtfi.AMDesignator != null && dtfi.PMDesignator != null)
			{
				if (dtfi.AMDesignator.Length == 0 && dtfi.PMDesignator.Length != 0)
				{
					raw.timeMark = DateTimeParse.TM.AM;
				}
				if (dtfi.PMDesignator.Length == 0 && dtfi.AMDesignator.Length != 0)
				{
					raw.timeMark = DateTimeParse.TM.PM;
				}
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0006125C File Offset: 0x0006025C
		private static bool AdjustHour(ref int hour, DateTimeParse.TM timeMark)
		{
			if (timeMark != DateTimeParse.TM.NotSet)
			{
				if (timeMark == DateTimeParse.TM.AM)
				{
					if (hour < 0 || hour > 12)
					{
						return false;
					}
					hour = ((hour == 12) ? 0 : hour);
				}
				else
				{
					if (hour < 0 || hour > 23)
					{
						return false;
					}
					if (hour < 12)
					{
						hour += 12;
					}
				}
			}
			return true;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0006129C File Offset: 0x0006029C
		private static bool GetTimeOfN(DateTimeFormatInfo dtfi, ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if ((result.flags & ParseFlags.HaveTime) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (raw.timeMark == DateTimeParse.TM.NotSet)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Hour = raw.GetNumber(0);
			result.flags |= ParseFlags.HaveTime;
			return true;
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000612F8 File Offset: 0x000602F8
		private static bool GetTimeOfNN(DateTimeFormatInfo dtfi, ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if ((result.flags & ParseFlags.HaveTime) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Hour = raw.GetNumber(0);
			result.Minute = raw.GetNumber(1);
			result.flags |= ParseFlags.HaveTime;
			return true;
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0006134C File Offset: 0x0006034C
		private static bool GetTimeOfNNN(DateTimeFormatInfo dtfi, ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if ((result.flags & ParseFlags.HaveTime) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Hour = raw.GetNumber(0);
			result.Minute = raw.GetNumber(1);
			result.Second = raw.GetNumber(2);
			result.flags |= ParseFlags.HaveTime;
			return true;
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000613AA File Offset: 0x000603AA
		private static bool GetDateOfDSN(ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if (raw.numCount != 1 || result.Day != -1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Day = raw.GetNumber(0);
			return true;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000613DC File Offset: 0x000603DC
		private static bool GetDateOfNDS(ref DateTimeResult result, ref DateTimeRawInfo raw)
		{
			if (result.Month == -1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (result.Year != -1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			result.Year = DateTimeParse.AdjustYear(ref result, raw.GetNumber(0));
			result.Day = 1;
			return true;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x00061434 File Offset: 0x00060434
		private static bool GetDateOfNNDS(ref DateTimeResult result, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			if ((result.flags & ParseFlags.HaveYear) != (ParseFlags)0)
			{
				if ((result.flags & ParseFlags.HaveMonth) == (ParseFlags)0 && (result.flags & ParseFlags.HaveDay) == (ParseFlags)0 && DateTimeParse.SetDateYMD(ref result, result.Year = DateTimeParse.AdjustYear(ref result, raw.year), raw.GetNumber(0), raw.GetNumber(1)))
				{
					return true;
				}
			}
			else if ((result.flags & ParseFlags.HaveMonth) != (ParseFlags)0 && (result.flags & ParseFlags.HaveYear) == (ParseFlags)0 && (result.flags & ParseFlags.HaveDay) == (ParseFlags)0)
			{
				int num;
				if (!DateTimeParse.GetYearMonthDayOrder(dtfi.ShortDatePattern, dtfi, out num))
				{
					result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadDatePattern", dtfi.ShortDatePattern);
					return false;
				}
				if (num == 0)
				{
					if (DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(0)), result.Month, raw.GetNumber(1)))
					{
						return true;
					}
				}
				else if (DateTimeParse.SetDateYMD(ref result, DateTimeParse.AdjustYear(ref result, raw.GetNumber(1)), result.Month, raw.GetNumber(0)))
				{
					return true;
				}
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00061534 File Offset: 0x00060534
		private static bool ProcessDateTimeSuffix(ref DateTimeResult result, ref DateTimeRawInfo raw, ref DateTimeToken dtok)
		{
			TokenType suffix = dtok.suffix;
			if (suffix <= TokenType.SEP_DaySuff)
			{
				if (suffix != TokenType.SEP_YearSuff)
				{
					if (suffix != TokenType.SEP_MonthSuff)
					{
						if (suffix == TokenType.SEP_DaySuff)
						{
							if ((result.flags & ParseFlags.HaveDay) != (ParseFlags)0)
							{
								return false;
							}
							result.flags |= ParseFlags.HaveDay;
							result.Day = dtok.num;
						}
					}
					else
					{
						if ((result.flags & ParseFlags.HaveMonth) != (ParseFlags)0)
						{
							return false;
						}
						result.flags |= ParseFlags.HaveMonth;
						result.Month = (raw.month = dtok.num);
					}
				}
				else
				{
					if ((result.flags & ParseFlags.HaveYear) != (ParseFlags)0)
					{
						return false;
					}
					result.flags |= ParseFlags.HaveYear;
					result.Year = (raw.year = dtok.num);
				}
			}
			else if (suffix != TokenType.SEP_HourSuff)
			{
				if (suffix != TokenType.SEP_MinuteSuff)
				{
					if (suffix == TokenType.SEP_SecondSuff)
					{
						if ((result.flags & ParseFlags.HaveSecond) != (ParseFlags)0)
						{
							return false;
						}
						result.flags |= ParseFlags.HaveSecond;
						result.Second = dtok.num;
					}
				}
				else
				{
					if ((result.flags & ParseFlags.HaveMinute) != (ParseFlags)0)
					{
						return false;
					}
					result.flags |= ParseFlags.HaveMinute;
					result.Minute = dtok.num;
				}
			}
			else
			{
				if ((result.flags & ParseFlags.HaveHour) != (ParseFlags)0)
				{
					return false;
				}
				result.flags |= ParseFlags.HaveHour;
				result.Hour = dtok.num;
			}
			return true;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000616A4 File Offset: 0x000606A4
		internal static bool ProcessHebrewTerminalState(DateTimeParse.DS dps, ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			switch (dps)
			{
			case DateTimeParse.DS.DX_NM:
				DateTimeParse.GetDefaultYear(ref result, ref styles);
				if (!dtfi.YearMonthAdjustment(ref result.Year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetHebrewDayOfNM(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_015C;
			case DateTimeParse.DS.DX_MNN:
				raw.year = raw.GetNumber(1);
				if (!dtfi.YearMonthAdjustment(ref raw.year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetDayOfMNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_015C;
			case DateTimeParse.DS.DX_YMN:
				if (!dtfi.YearMonthAdjustment(ref raw.year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetDayOfYMN(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_015C;
			case DateTimeParse.DS.DX_YM:
				if (!dtfi.YearMonthAdjustment(ref raw.year, ref raw.month, true))
				{
					result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
					return false;
				}
				if (!DateTimeParse.GetDayOfYM(ref result, ref raw, dtfi))
				{
					return false;
				}
				goto IL_015C;
			case DateTimeParse.DS.TX_N:
				if (!DateTimeParse.GetTimeOfN(dtfi, ref result, ref raw))
				{
					return false;
				}
				goto IL_015C;
			case DateTimeParse.DS.TX_NN:
				if (!DateTimeParse.GetTimeOfNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				goto IL_015C;
			case DateTimeParse.DS.TX_NNN:
				if (!DateTimeParse.GetTimeOfNNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				goto IL_015C;
			}
			result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
			return false;
			IL_015C:
			if (dps > DateTimeParse.DS.ERROR)
			{
				raw.numCount = 0;
			}
			return true;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0006181C File Offset: 0x0006081C
		internal static bool ProcessTerminaltState(DateTimeParse.DS dps, ref DateTimeResult result, ref DateTimeStyles styles, ref DateTimeRawInfo raw, DateTimeFormatInfo dtfi)
		{
			switch (dps)
			{
			case DateTimeParse.DS.DX_NN:
				if (!DateTimeParse.GetDayOfNN(ref result, ref styles, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NNN:
				if (!DateTimeParse.GetDayOfNNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_MN:
				if (!DateTimeParse.GetDayOfMN(ref result, ref styles, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NM:
				if (!DateTimeParse.GetDayOfNM(ref result, ref styles, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_MNN:
				if (!DateTimeParse.GetDayOfMNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_DSN:
				if (!DateTimeParse.GetDateOfDSN(ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NDS:
				if (!DateTimeParse.GetDateOfNDS(ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NNDS:
				if (!DateTimeParse.GetDateOfNNDS(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YNN:
				if (!DateTimeParse.GetDayOfYNN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YMN:
				if (!DateTimeParse.GetDayOfYMN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YN:
				if (!DateTimeParse.GetDayOfYN(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_YM:
				if (!DateTimeParse.GetDayOfYM(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.TX_N:
				if (!DateTimeParse.GetTimeOfN(dtfi, ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.TX_NN:
				if (!DateTimeParse.GetTimeOfNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.TX_NNN:
				if (!DateTimeParse.GetTimeOfNNN(dtfi, ref result, ref raw))
				{
					return false;
				}
				break;
			case DateTimeParse.DS.DX_NNY:
				if (!DateTimeParse.GetDayOfNNY(ref result, ref raw, dtfi))
				{
					return false;
				}
				break;
			}
			if (dps > DateTimeParse.DS.ERROR)
			{
				raw.numCount = 0;
			}
			return true;
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x00061970 File Offset: 0x00060970
		internal static DateTime Parse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000619A8 File Offset: 0x000609A8
		internal static DateTime Parse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, out TimeSpan offset)
		{
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				offset = dateTimeResult.timeZoneOffset;
				return dateTimeResult.parsedDate;
			}
			throw DateTimeParse.GetDateTimeParseException(ref dateTimeResult);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x00061A00 File Offset: 0x00060A00
		internal static bool TryParse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, out DateTime result)
		{
			result = DateTime.MinValue;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				return true;
			}
			return false;
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x00061A44 File Offset: 0x00060A44
		internal static bool TryParse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, out DateTime result, out TimeSpan offset)
		{
			result = DateTime.MinValue;
			offset = TimeSpan.Zero;
			DateTimeResult dateTimeResult = default(DateTimeResult);
			dateTimeResult.Init();
			dateTimeResult.flags |= ParseFlags.CaptureOffset;
			if (DateTimeParse.TryParse(s, dtfi, styles, ref dateTimeResult))
			{
				result = dateTimeResult.parsedDate;
				offset = dateTimeResult.timeZoneOffset;
				return true;
			}
			return false;
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x00061AB4 File Offset: 0x00060AB4
		internal unsafe static bool TryParse(string s, DateTimeFormatInfo dtfi, DateTimeStyles styles, ref DateTimeResult result)
		{
			if (s == null)
			{
				result.SetFailure(ParseFailureKind.ArgumentNull, "ArgumentNull_String", null, "s");
				return false;
			}
			if (s.Length == 0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			DateTimeParse.DS ds = DateTimeParse.DS.BEGIN;
			bool flag = false;
			DateTimeToken dateTimeToken = default(DateTimeToken);
			dateTimeToken.suffix = TokenType.SEP_Unk;
			DateTimeRawInfo dateTimeRawInfo = default(DateTimeRawInfo);
			int* ptr = stackalloc int[4 * 3];
			dateTimeRawInfo.Init(ptr);
			result.calendar = dtfi.Calendar;
			result.era = 0;
			__DTString _DTString = new __DTString(s, dtfi);
			_DTString.GetNext();
			while (DateTimeParse.Lex(ds, ref _DTString, ref dateTimeToken, ref dateTimeRawInfo, ref result, ref dtfi))
			{
				if (dateTimeToken.dtt != DateTimeParse.DTT.Unk)
				{
					if (dateTimeToken.suffix != TokenType.SEP_Unk)
					{
						if (!DateTimeParse.ProcessDateTimeSuffix(ref result, ref dateTimeRawInfo, ref dateTimeToken))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						dateTimeToken.suffix = TokenType.SEP_Unk;
					}
					if (dateTimeToken.dtt == DateTimeParse.DTT.NumLocalTimeMark)
					{
						if (ds == DateTimeParse.DS.D_YNd || ds == DateTimeParse.DS.D_YN)
						{
							return DateTimeParse.ParseISO8601(ref dateTimeRawInfo, ref _DTString, styles, ref result);
						}
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					else
					{
						ds = DateTimeParse.dateParsingStates[(int)ds][(int)dateTimeToken.dtt];
						if (ds == DateTimeParse.DS.ERROR)
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if (ds > DateTimeParse.DS.ERROR)
						{
							if ((dtfi.FormatFlags & DateTimeFormatFlags.UseHebrewRule) != DateTimeFormatFlags.None)
							{
								if (!DateTimeParse.ProcessHebrewTerminalState(ds, ref result, ref styles, ref dateTimeRawInfo, dtfi))
								{
									return false;
								}
							}
							else if (!DateTimeParse.ProcessTerminaltState(ds, ref result, ref styles, ref dateTimeRawInfo, dtfi))
							{
								return false;
							}
							flag = true;
							ds = DateTimeParse.DS.BEGIN;
						}
					}
				}
				if (dateTimeToken.dtt == DateTimeParse.DTT.End || dateTimeToken.dtt == DateTimeParse.DTT.NumEnd || dateTimeToken.dtt == DateTimeParse.DTT.MonthEnd)
				{
					if (!flag)
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					DateTimeParse.AdjustTimeMark(dtfi, ref dateTimeRawInfo);
					if (!DateTimeParse.AdjustHour(ref result.Hour, dateTimeRawInfo.timeMark))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					bool flag2 = result.Year == -1 && result.Month == -1 && result.Day == -1;
					if (!DateTimeParse.CheckDefaultDateTime(ref result, ref result.calendar, styles))
					{
						return false;
					}
					DateTime dateTime;
					if (!result.calendar.TryToDateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, 0, result.era, out dateTime))
					{
						result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
						return false;
					}
					if (dateTimeRawInfo.fraction > 0.0)
					{
						dateTime = dateTime.AddTicks((long)Math.Round(dateTimeRawInfo.fraction * 10000000.0));
					}
					if (dateTimeRawInfo.dayOfWeek != -1 && dateTimeRawInfo.dayOfWeek != (int)result.calendar.GetDayOfWeek(dateTime))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDayOfWeek", null);
						return false;
					}
					result.parsedDate = dateTime;
					return DateTimeParse.DetermineTimeZoneAdjustments(ref result, styles, flag2);
				}
			}
			return false;
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x00061D70 File Offset: 0x00060D70
		private static bool DetermineTimeZoneAdjustments(ref DateTimeResult result, DateTimeStyles styles, bool bTimeOnly)
		{
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
			{
				return DateTimeParse.DateTimeOffsetTimeZonePostProcessing(ref result, styles);
			}
			if ((result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0)
			{
				if ((styles & DateTimeStyles.AssumeLocal) != DateTimeStyles.None)
				{
					if ((styles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.None)
					{
						result.parsedDate = DateTime.SpecifyKind(result.parsedDate, DateTimeKind.Local);
						return true;
					}
					result.flags |= ParseFlags.TimeZoneUsed;
					result.timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(result.parsedDate);
				}
				else
				{
					if ((styles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.None)
					{
						return true;
					}
					if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
					{
						result.parsedDate = DateTime.SpecifyKind(result.parsedDate, DateTimeKind.Utc);
						return true;
					}
					result.flags |= ParseFlags.TimeZoneUsed;
					result.timeZoneOffset = TimeSpan.Zero;
				}
			}
			if ((styles & DateTimeStyles.RoundtripKind) != DateTimeStyles.None && (result.flags & ParseFlags.TimeZoneUtc) != (ParseFlags)0)
			{
				result.parsedDate = DateTime.SpecifyKind(result.parsedDate, DateTimeKind.Utc);
				return true;
			}
			if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
			{
				return DateTimeParse.AdjustTimeZoneToUniversal(ref result);
			}
			return DateTimeParse.AdjustTimeZoneToLocal(ref result, bTimeOnly);
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x00061E70 File Offset: 0x00060E70
		private static bool DateTimeOffsetTimeZonePostProcessing(ref DateTimeResult result, DateTimeStyles styles)
		{
			if ((result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0)
			{
				if ((styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
				{
					result.timeZoneOffset = TimeSpan.Zero;
				}
				else
				{
					result.timeZoneOffset = TimeZone.CurrentTimeZone.GetUtcOffset(result.parsedDate);
				}
			}
			long ticks = result.timeZoneOffset.Ticks;
			long num = result.parsedDate.Ticks - ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_UTCOutOfRange", null);
				return false;
			}
			if (ticks < -504000000000L || ticks > 504000000000L)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_OffsetOutOfRange", null);
				return false;
			}
			if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
			{
				if ((result.flags & ParseFlags.TimeZoneUsed) == (ParseFlags)0 && (styles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.None)
				{
					bool flag = DateTimeParse.AdjustTimeZoneToUniversal(ref result);
					result.timeZoneOffset = TimeSpan.Zero;
					return flag;
				}
				result.parsedDate = new DateTime(num, DateTimeKind.Utc);
				result.timeZoneOffset = TimeSpan.Zero;
			}
			return true;
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x00061F5C File Offset: 0x00060F5C
		private static bool AdjustTimeZoneToUniversal(ref DateTimeResult result)
		{
			long num = result.parsedDate.Ticks;
			num -= result.timeZoneOffset.Ticks;
			if (num < 0L)
			{
				num += 864000000000L;
			}
			if (num < 0L || num > 3155378975999999999L)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_DateOutOfRange", null);
				return false;
			}
			result.parsedDate = new DateTime(num, DateTimeKind.Utc);
			return true;
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x00061FC4 File Offset: 0x00060FC4
		private static bool AdjustTimeZoneToLocal(ref DateTimeResult result, bool bTimeOnly)
		{
			long num = result.parsedDate.Ticks;
			CurrentSystemTimeZone currentSystemTimeZone = (CurrentSystemTimeZone)TimeZone.CurrentTimeZone;
			bool flag = false;
			if (num < 864000000000L)
			{
				num -= result.timeZoneOffset.Ticks;
				num += currentSystemTimeZone.GetUtcOffset(bTimeOnly ? DateTime.Now : result.parsedDate).Ticks;
				if (num < 0L)
				{
					num += 864000000000L;
				}
			}
			else
			{
				num -= result.timeZoneOffset.Ticks;
				if (num < 0L || num > 3155378975999999999L)
				{
					num += currentSystemTimeZone.GetUtcOffset(result.parsedDate).Ticks;
				}
				else
				{
					num += currentSystemTimeZone.GetUtcOffsetFromUniversalTime(new DateTime(num), ref flag);
				}
			}
			if (num < 0L || num > 3155378975999999999L)
			{
				result.parsedDate = DateTime.MinValue;
				result.SetFailure(ParseFailureKind.Format, "Format_DateOutOfRange", null);
				return false;
			}
			result.parsedDate = new DateTime(num, DateTimeKind.Local, flag);
			return true;
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000620BC File Offset: 0x000610BC
		private static bool ParseISO8601(ref DateTimeRawInfo raw, ref __DTString str, DateTimeStyles styles, ref DateTimeResult result)
		{
			if (raw.year >= 0 && raw.GetNumber(0) >= 0)
			{
				raw.GetNumber(1);
			}
			str.Index--;
			int num = 0;
			double num2 = 0.0;
			str.SkipWhiteSpaces();
			int num3;
			if (!DateTimeParse.ParseDigits(ref str, 2, out num3))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			str.SkipWhiteSpaces();
			if (!str.Match(':'))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			str.SkipWhiteSpaces();
			int num4;
			if (!DateTimeParse.ParseDigits(ref str, 2, out num4))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			str.SkipWhiteSpaces();
			if (str.Match(':'))
			{
				str.SkipWhiteSpaces();
				if (!DateTimeParse.ParseDigits(ref str, 2, out num))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (str.Match('.'))
				{
					if (!DateTimeParse.ParseFraction(ref str, out num2))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					str.Index--;
				}
				str.SkipWhiteSpaces();
			}
			if (str.GetNext())
			{
				char @char = str.GetChar();
				if (@char == '+' || @char == '-')
				{
					result.flags |= ParseFlags.TimeZoneUsed;
					if (!DateTimeParse.ParseTimeZone(ref str, ref result.timeZoneOffset))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
				}
				else if (@char == 'Z' || @char == 'z')
				{
					result.flags |= ParseFlags.TimeZoneUsed;
					result.timeZoneOffset = TimeSpan.Zero;
					result.flags |= ParseFlags.TimeZoneUtc;
				}
				else
				{
					str.Index--;
				}
				str.SkipWhiteSpaces();
				if (str.Match('#'))
				{
					if (!DateTimeParse.VerifyValidPunctuation(ref str))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					str.SkipWhiteSpaces();
				}
				if (str.Match('\0') && !DateTimeParse.VerifyValidPunctuation(ref str))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (str.GetNext())
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
			}
			Calendar defaultInstance = GregorianCalendar.GetDefaultInstance();
			DateTime dateTime;
			if (!defaultInstance.TryToDateTime(raw.year, raw.GetNumber(0), raw.GetNumber(1), num3, num4, num, 0, result.era, out dateTime))
			{
				result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
				return false;
			}
			dateTime = dateTime.AddTicks((long)Math.Round(num2 * 10000000.0));
			result.parsedDate = dateTime;
			return DateTimeParse.DetermineTimeZoneAdjustments(ref result, styles, false);
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x00062334 File Offset: 0x00061334
		internal static bool MatchHebrewDigits(ref __DTString str, int digitLen, out int number)
		{
			number = 0;
			HebrewNumberParsingContext hebrewNumberParsingContext = new HebrewNumberParsingContext(0);
			HebrewNumberParsingState hebrewNumberParsingState = HebrewNumberParsingState.ContinueParsing;
			while (hebrewNumberParsingState == HebrewNumberParsingState.ContinueParsing && str.GetNext())
			{
				hebrewNumberParsingState = HebrewNumber.ParseByChar(str.GetChar(), ref hebrewNumberParsingContext);
			}
			if (hebrewNumberParsingState == HebrewNumberParsingState.FoundEndOfHebrewNumber)
			{
				number = hebrewNumberParsingContext.result;
				return true;
			}
			return false;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0006237F File Offset: 0x0006137F
		internal static bool ParseDigits(ref __DTString str, int digitLen, out int result)
		{
			if (digitLen == 1)
			{
				return DateTimeParse.ParseDigits(ref str, 1, 2, out result);
			}
			return DateTimeParse.ParseDigits(ref str, digitLen, digitLen, out result);
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x00062398 File Offset: 0x00061398
		internal static bool ParseDigits(ref __DTString str, int minDigitLen, int maxDigitLen, out int result)
		{
			result = 0;
			int index = str.Index;
			int i;
			for (i = 0; i < maxDigitLen; i++)
			{
				if (!str.GetNextDigit())
				{
					str.Index--;
					break;
				}
				result = result * 10 + str.GetDigit();
			}
			if (i < minDigitLen)
			{
				str.Index = index;
				return false;
			}
			return true;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000623F0 File Offset: 0x000613F0
		private static bool ParseFractionExact(ref __DTString str, int maxDigitLen, ref double result)
		{
			if (!str.GetNextDigit())
			{
				str.Index--;
				return false;
			}
			result = (double)str.GetDigit();
			int i;
			for (i = 1; i < maxDigitLen; i++)
			{
				if (!str.GetNextDigit())
				{
					str.Index--;
					break;
				}
				result = result * 10.0 + (double)str.GetDigit();
			}
			result /= Math.Pow(10.0, (double)i);
			return i == maxDigitLen;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x00062474 File Offset: 0x00061474
		private static bool ParseSign(ref __DTString str, ref bool result)
		{
			if (!str.GetNext())
			{
				return false;
			}
			char @char = str.GetChar();
			if (@char == '+')
			{
				result = true;
				return true;
			}
			if (@char == '-')
			{
				result = false;
				return true;
			}
			return false;
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000624A8 File Offset: 0x000614A8
		private static bool ParseTimeZoneOffset(ref __DTString str, int len, ref TimeSpan result)
		{
			bool flag = true;
			int num = 0;
			int num2;
			switch (len)
			{
			case 1:
			case 2:
				if (!DateTimeParse.ParseSign(ref str, ref flag))
				{
					return false;
				}
				if (!DateTimeParse.ParseDigits(ref str, len, out num2))
				{
					return false;
				}
				break;
			default:
				if (!DateTimeParse.ParseSign(ref str, ref flag))
				{
					return false;
				}
				if (!DateTimeParse.ParseDigits(ref str, 1, out num2))
				{
					return false;
				}
				if (str.Match(":"))
				{
					if (!DateTimeParse.ParseDigits(ref str, 2, out num))
					{
						return false;
					}
				}
				else
				{
					str.Index--;
					if (!DateTimeParse.ParseDigits(ref str, 2, out num))
					{
						return false;
					}
				}
				break;
			}
			if (num < 0 || num >= 60)
			{
				return false;
			}
			result = new TimeSpan(num2, num, 0);
			if (!flag)
			{
				result = result.Negate();
			}
			return true;
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x00062560 File Offset: 0x00061560
		private static bool MatchAbbreviatedMonthName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				int num2 = ((dtfi.GetMonthName(13).Length == 0) ? 12 : 13);
				for (int i = 1; i <= num2; i++)
				{
					string abbreviatedMonthName = dtfi.GetAbbreviatedMonthName(i);
					int length = abbreviatedMonthName.Length;
					if ((dtfi.HasSpacesInMonthNames ? str.MatchSpecifiedWords(abbreviatedMonthName, false, ref length) : str.MatchSpecifiedWord(abbreviatedMonthName)) && length > num)
					{
						num = length;
						result = i;
					}
				}
				if ((dtfi.FormatFlags & DateTimeFormatFlags.UseLeapYearMonth) != DateTimeFormatFlags.None)
				{
					int num3 = str.MatchLongestWords(dtfi.internalGetLeapYearMonthNames(), ref num);
					if (num3 >= 0)
					{
						result = num3 + 1;
					}
				}
			}
			if (result > 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00062614 File Offset: 0x00061614
		private static bool MatchMonthName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				int num2 = ((dtfi.GetMonthName(13).Length == 0) ? 12 : 13);
				for (int i = 1; i <= num2; i++)
				{
					string monthName = dtfi.GetMonthName(i);
					int length = monthName.Length;
					if ((dtfi.HasSpacesInMonthNames ? str.MatchSpecifiedWords(monthName, false, ref length) : str.MatchSpecifiedWord(monthName)) && length > num)
					{
						num = length;
						result = i;
					}
				}
				if ((dtfi.FormatFlags & DateTimeFormatFlags.UseGenitiveMonth) != DateTimeFormatFlags.None)
				{
					int num3 = str.MatchLongestWords(dtfi.MonthGenitiveNames, ref num);
					if (num3 >= 0)
					{
						result = num3 + 1;
					}
				}
				if ((dtfi.FormatFlags & DateTimeFormatFlags.UseLeapYearMonth) != DateTimeFormatFlags.None)
				{
					int num4 = str.MatchLongestWords(dtfi.internalGetLeapYearMonthNames(), ref num);
					if (num4 >= 0)
					{
						result = num4 + 1;
					}
				}
			}
			if (result > 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000626F0 File Offset: 0x000616F0
		private static bool MatchAbbreviatedDayName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				for (DayOfWeek dayOfWeek = DayOfWeek.Sunday; dayOfWeek <= DayOfWeek.Saturday; dayOfWeek++)
				{
					string abbreviatedDayName = dtfi.GetAbbreviatedDayName(dayOfWeek);
					int length = abbreviatedDayName.Length;
					if ((dtfi.HasSpacesInDayNames ? str.MatchSpecifiedWords(abbreviatedDayName, false, ref length) : str.MatchSpecifiedWord(abbreviatedDayName)) && length > num)
					{
						num = length;
						result = (int)dayOfWeek;
					}
				}
			}
			if (result >= 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00062764 File Offset: 0x00061764
		private static bool MatchDayName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			int num = 0;
			result = -1;
			if (str.GetNext())
			{
				for (DayOfWeek dayOfWeek = DayOfWeek.Sunday; dayOfWeek <= DayOfWeek.Saturday; dayOfWeek++)
				{
					string dayName = dtfi.GetDayName(dayOfWeek);
					int length = dayName.Length;
					if ((dtfi.HasSpacesInDayNames ? str.MatchSpecifiedWords(dayName, false, ref length) : str.MatchSpecifiedWord(dayName)) && length > num)
					{
						num = length;
						result = (int)dayOfWeek;
					}
				}
			}
			if (result >= 0)
			{
				str.Index += num - 1;
				return true;
			}
			return false;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000627D8 File Offset: 0x000617D8
		private static bool MatchEraName(ref __DTString str, DateTimeFormatInfo dtfi, ref int result)
		{
			if (str.GetNext())
			{
				int[] eras = dtfi.Calendar.Eras;
				if (eras != null)
				{
					for (int i = 0; i < eras.Length; i++)
					{
						string text = dtfi.GetEraName(eras[i]);
						if (str.MatchSpecifiedWord(text))
						{
							str.Index += text.Length - 1;
							result = eras[i];
							return true;
						}
						text = dtfi.GetAbbreviatedEraName(eras[i]);
						if (str.MatchSpecifiedWord(text))
						{
							str.Index += text.Length - 1;
							result = eras[i];
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0006286C File Offset: 0x0006186C
		private static bool MatchTimeMark(ref __DTString str, DateTimeFormatInfo dtfi, ref DateTimeParse.TM result)
		{
			result = DateTimeParse.TM.NotSet;
			if (dtfi.AMDesignator.Length == 0)
			{
				result = DateTimeParse.TM.AM;
			}
			if (dtfi.PMDesignator.Length == 0)
			{
				result = DateTimeParse.TM.PM;
			}
			if (str.GetNext())
			{
				string text = dtfi.AMDesignator;
				if (text.Length > 0 && str.MatchSpecifiedWord(text))
				{
					str.Index += text.Length - 1;
					result = DateTimeParse.TM.AM;
					return true;
				}
				text = dtfi.PMDesignator;
				if (text.Length > 0 && str.MatchSpecifiedWord(text))
				{
					str.Index += text.Length - 1;
					result = DateTimeParse.TM.PM;
					return true;
				}
				str.Index--;
			}
			return result != DateTimeParse.TM.NotSet;
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x00062920 File Offset: 0x00061920
		private static bool MatchAbbreviatedTimeMark(ref __DTString str, DateTimeFormatInfo dtfi, ref DateTimeParse.TM result)
		{
			if (str.GetNext())
			{
				if (str.GetChar() == dtfi.AMDesignator[0])
				{
					result = DateTimeParse.TM.AM;
					return true;
				}
				if (str.GetChar() == dtfi.PMDesignator[0])
				{
					result = DateTimeParse.TM.PM;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0006295D File Offset: 0x0006195D
		private static bool CheckNewValue(ref int currentValue, int newValue, char patternChar, ref DateTimeResult result)
		{
			if (currentValue == -1)
			{
				currentValue = newValue;
				return true;
			}
			if (newValue != currentValue)
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", patternChar);
				return false;
			}
			return true;
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00062984 File Offset: 0x00061984
		private static DateTime GetDateTimeNow(ref DateTimeResult result, ref DateTimeStyles styles)
		{
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
			{
				if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0)
				{
					return new DateTime(DateTime.UtcNow.Ticks + result.timeZoneOffset.Ticks, DateTimeKind.Unspecified);
				}
				if ((styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
				{
					return DateTime.UtcNow;
				}
			}
			return DateTime.Now;
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000629E0 File Offset: 0x000619E0
		private static bool CheckDefaultDateTime(ref DateTimeResult result, ref Calendar cal, DateTimeStyles styles)
		{
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0 && (result.Month != -1 || result.Day != -1) && (result.Year == -1 || (result.flags & ParseFlags.YearDefault) != (ParseFlags)0) && (result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_MissingIncompleteDate", null);
				return false;
			}
			if (result.Year == -1 || result.Month == -1 || result.Day == -1)
			{
				DateTime dateTimeNow = DateTimeParse.GetDateTimeNow(ref result, ref styles);
				if (result.Month == -1 && result.Day == -1)
				{
					if (result.Year == -1)
					{
						if ((styles & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
						{
							cal = GregorianCalendar.GetDefaultInstance();
							result.Year = (result.Month = (result.Day = 1));
						}
						else
						{
							result.Year = cal.GetYear(dateTimeNow);
							result.Month = cal.GetMonth(dateTimeNow);
							result.Day = cal.GetDayOfMonth(dateTimeNow);
						}
					}
					else
					{
						result.Month = 1;
						result.Day = 1;
					}
				}
				else
				{
					if (result.Year == -1)
					{
						result.Year = cal.GetYear(dateTimeNow);
					}
					if (result.Month == -1)
					{
						result.Month = 1;
					}
					if (result.Day == -1)
					{
						result.Day = 1;
					}
				}
			}
			if (result.Hour == -1)
			{
				result.Hour = 0;
			}
			if (result.Minute == -1)
			{
				result.Minute = 0;
			}
			if (result.Second == -1)
			{
				result.Second = 0;
			}
			if (result.era == -1)
			{
				result.era = 0;
			}
			return true;
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x00062B60 File Offset: 0x00061B60
		private static string ExpandPredefinedFormat(string format, ref DateTimeFormatInfo dtfi, ref ParsingInfo parseInfo, ref DateTimeResult result)
		{
			char c = format[0];
			if (c <= 'R')
			{
				if (c != 'O')
				{
					if (c != 'R')
					{
						goto IL_015B;
					}
					goto IL_0065;
				}
			}
			else if (c != 'U')
			{
				switch (c)
				{
				case 'o':
					break;
				case 'p':
				case 'q':
				case 't':
					goto IL_015B;
				case 'r':
					goto IL_0065;
				case 's':
					dtfi = DateTimeFormatInfo.InvariantInfo;
					parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
					goto IL_015B;
				case 'u':
					parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
					dtfi = DateTimeFormatInfo.InvariantInfo;
					if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
					{
						result.flags |= ParseFlags.UtcSortPattern;
						goto IL_015B;
					}
					goto IL_015B;
				default:
					goto IL_015B;
				}
			}
			else
			{
				parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
				result.flags |= ParseFlags.TimeZoneUsed;
				result.timeZoneOffset = new TimeSpan(0L);
				result.flags |= ParseFlags.TimeZoneUtc;
				if (dtfi.Calendar.GetType() != typeof(GregorianCalendar))
				{
					dtfi = (DateTimeFormatInfo)dtfi.Clone();
					dtfi.Calendar = GregorianCalendar.GetDefaultInstance();
					goto IL_015B;
				}
				goto IL_015B;
			}
			parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
			dtfi = DateTimeFormatInfo.InvariantInfo;
			goto IL_015B;
			IL_0065:
			parseInfo.calendar = GregorianCalendar.GetDefaultInstance();
			dtfi = DateTimeFormatInfo.InvariantInfo;
			if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0)
			{
				result.flags |= ParseFlags.Rfc1123Pattern;
			}
			IL_015B:
			return DateTimeFormat.GetRealFormat(format, dtfi);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00062CD0 File Offset: 0x00061CD0
		private static bool ParseJapaneseEraStart(ref __DTString str, DateTimeFormatInfo dtfi)
		{
			if (GregorianCalendarHelper.EnforceLegacyJapaneseDateParsing || dtfi.Calendar.ID != 3 || !str.GetNext())
			{
				return false;
			}
			if (str.m_current != "元"[0])
			{
				str.Index--;
				return false;
			}
			return true;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00062D20 File Offset: 0x00061D20
		private static bool ParseByFormat(ref __DTString str, ref __DTString format, ref ParsingInfo parseInfo, DateTimeFormatInfo dtfi, ref DateTimeResult result)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			double num9 = 0.0;
			DateTimeParse.TM tm = DateTimeParse.TM.AM;
			char @char = format.GetChar();
			char c = @char;
			if (c <= 'H')
			{
				if (c <= '\'')
				{
					if (c != '"')
					{
						switch (c)
						{
						case '%':
							if (format.Index >= format.Value.Length - 1 || format.Value[format.Index + 1] == '%')
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
								return false;
							}
							return true;
						case '&':
							goto IL_09A1;
						case '\'':
							break;
						default:
							goto IL_09A1;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					if (!DateTimeParse.TryParseQuoteString(format.Value, format.Index, stringBuilder, out num))
					{
						result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_BadQuote", @char);
						return false;
					}
					format.Index += num - 1;
					string text = stringBuilder.ToString();
					for (int i = 0; i < text.Length; i++)
					{
						if (text[i] == ' ' && parseInfo.fAllowInnerWhite)
						{
							str.SkipWhiteSpaces();
						}
						else if (!str.Match(text[i]))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
					if ((result.flags & ParseFlags.CaptureOffset) == (ParseFlags)0)
					{
						return true;
					}
					if ((result.flags & ParseFlags.Rfc1123Pattern) != (ParseFlags)0 && text == "GMT")
					{
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = TimeSpan.Zero;
						return true;
					}
					if ((result.flags & ParseFlags.UtcSortPattern) != (ParseFlags)0 && text == "Z")
					{
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = TimeSpan.Zero;
						return true;
					}
					return true;
				}
				else
				{
					switch (c)
					{
					case '.':
						if (str.Match(@char))
						{
							return true;
						}
						if (format.GetNext() && format.Match('F'))
						{
							format.GetRepeatCount();
							return true;
						}
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					case '/':
						if (!str.Match(dtfi.DateSeparator))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						return true;
					default:
						if (c != ':')
						{
							switch (c)
							{
							case 'F':
								break;
							case 'G':
								goto IL_09A1;
							case 'H':
								num = format.GetRepeatCount();
								if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out num6))
								{
									result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
									return false;
								}
								if (!DateTimeParse.CheckNewValue(ref result.Hour, num6, @char, ref result))
								{
									return false;
								}
								return true;
							default:
								goto IL_09A1;
							}
						}
						else
						{
							if (!str.Match(dtfi.TimeSeparator))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
							return true;
						}
						break;
					}
				}
			}
			else if (c <= 'h')
			{
				switch (c)
				{
				case 'K':
					if (str.Match('Z'))
					{
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && result.timeZoneOffset != TimeSpan.Zero)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'K');
							return false;
						}
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = new TimeSpan(0L);
						result.flags |= ParseFlags.TimeZoneUtc;
						return true;
					}
					else
					{
						if (!str.Match('+') && !str.Match('-'))
						{
							return true;
						}
						str.Index--;
						TimeSpan timeSpan = new TimeSpan(0L);
						if (!DateTimeParse.ParseTimeZoneOffset(ref str, 3, ref timeSpan))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && timeSpan != result.timeZoneOffset)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'K');
							return false;
						}
						result.timeZoneOffset = timeSpan;
						result.flags |= ParseFlags.TimeZoneUsed;
						return true;
					}
					break;
				case 'L':
					goto IL_09A1;
				case 'M':
					num = format.GetRepeatCount();
					if (num <= 2)
					{
						if (!DateTimeParse.ParseDigits(ref str, num, out num3) && (!parseInfo.fCustomNumberParser || !parseInfo.parseNumberDelegate(ref str, num, out num3)))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
					else
					{
						if (num == 3)
						{
							if (!DateTimeParse.MatchAbbreviatedMonthName(ref str, dtfi, ref num3))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
						}
						else if (!DateTimeParse.MatchMonthName(ref str, dtfi, ref num3))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						result.flags |= ParseFlags.ParsedMonthName;
					}
					if (!DateTimeParse.CheckNewValue(ref result.Month, num3, @char, ref result))
					{
						return false;
					}
					return true;
				default:
					switch (c)
					{
					case 'Z':
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && result.timeZoneOffset != TimeSpan.Zero)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'Z');
							return false;
						}
						result.flags |= ParseFlags.TimeZoneUsed;
						result.timeZoneOffset = new TimeSpan(0L);
						result.flags |= ParseFlags.TimeZoneUtc;
						str.Index++;
						if (!DateTimeParse.GetTimeZoneName(ref str))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						str.Index--;
						return true;
					case '[':
						goto IL_09A1;
					case '\\':
						if (!format.GetNext())
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
							return false;
						}
						if (!str.Match(format.GetChar()))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						return true;
					default:
						switch (c)
						{
						case 'd':
							num = format.GetRepeatCount();
							if (num <= 2)
							{
								if (!DateTimeParse.ParseDigits(ref str, num, out num4) && (!parseInfo.fCustomNumberParser || !parseInfo.parseNumberDelegate(ref str, num, out num4)))
								{
									result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
									return false;
								}
								if (!DateTimeParse.CheckNewValue(ref result.Day, num4, @char, ref result))
								{
									return false;
								}
								return true;
							}
							else
							{
								if (num == 3)
								{
									if (!DateTimeParse.MatchAbbreviatedDayName(ref str, dtfi, ref num5))
									{
										result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
										return false;
									}
								}
								else if (!DateTimeParse.MatchDayName(ref str, dtfi, ref num5))
								{
									result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
									return false;
								}
								if (!DateTimeParse.CheckNewValue(ref parseInfo.dayOfWeek, num5, @char, ref result))
								{
									return false;
								}
								return true;
							}
							break;
						case 'e':
							goto IL_09A1;
						case 'f':
							break;
						case 'g':
							num = format.GetRepeatCount();
							if (!DateTimeParse.MatchEraName(ref str, dtfi, ref result.era))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
							return true;
						case 'h':
							parseInfo.fUseHour12 = true;
							num = format.GetRepeatCount();
							if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out num6))
							{
								result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
								return false;
							}
							if (!DateTimeParse.CheckNewValue(ref result.Hour, num6, @char, ref result))
							{
								return false;
							}
							return true;
						default:
							goto IL_09A1;
						}
						break;
					}
					break;
				}
			}
			else if (c != 'm')
			{
				switch (c)
				{
				case 's':
					num = format.GetRepeatCount();
					if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out num8))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					if (!DateTimeParse.CheckNewValue(ref result.Second, num8, @char, ref result))
					{
						return false;
					}
					return true;
				case 't':
					num = format.GetRepeatCount();
					if (num == 1)
					{
						if (!DateTimeParse.MatchAbbreviatedTimeMark(ref str, dtfi, ref tm))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
					}
					else if (!DateTimeParse.MatchTimeMark(ref str, dtfi, ref tm))
					{
						result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
						return false;
					}
					if (parseInfo.timeMark == DateTimeParse.TM.NotSet)
					{
						parseInfo.timeMark = tm;
						return true;
					}
					if (parseInfo.timeMark != tm)
					{
						result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", @char);
						return false;
					}
					return true;
				default:
					switch (c)
					{
					case 'y':
					{
						num = format.GetRepeatCount();
						bool flag;
						if (DateTimeParse.ParseJapaneseEraStart(ref str, dtfi))
						{
							num2 = 1;
							flag = true;
						}
						else if (dtfi.HasForceTwoDigitYears)
						{
							flag = DateTimeParse.ParseDigits(ref str, 1, 4, out num2);
						}
						else
						{
							if (num <= 2)
							{
								parseInfo.fUseTwoDigitYear = true;
							}
							flag = DateTimeParse.ParseDigits(ref str, num, out num2);
						}
						if (!flag && parseInfo.fCustomNumberParser)
						{
							flag = parseInfo.parseNumberDelegate(ref str, num, out num2);
						}
						if (!flag)
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if (!DateTimeParse.CheckNewValue(ref result.Year, num2, @char, ref result))
						{
							return false;
						}
						return true;
					}
					case 'z':
					{
						num = format.GetRepeatCount();
						TimeSpan timeSpan2 = new TimeSpan(0L);
						if (!DateTimeParse.ParseTimeZoneOffset(ref str, num, ref timeSpan2))
						{
							result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
							return false;
						}
						if ((result.flags & ParseFlags.TimeZoneUsed) != (ParseFlags)0 && timeSpan2 != result.timeZoneOffset)
						{
							result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", 'z');
							return false;
						}
						result.timeZoneOffset = timeSpan2;
						result.flags |= ParseFlags.TimeZoneUsed;
						return true;
					}
					default:
						goto IL_09A1;
					}
					break;
				}
			}
			else
			{
				num = format.GetRepeatCount();
				if (!DateTimeParse.ParseDigits(ref str, (num < 2) ? 1 : 2, out num7))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (!DateTimeParse.CheckNewValue(ref result.Minute, num7, @char, ref result))
				{
					return false;
				}
				return true;
			}
			num = format.GetRepeatCount();
			if (num > 7)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (!DateTimeParse.ParseFractionExact(ref str, num, ref num9) && @char == 'f')
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (result.fraction < 0.0)
			{
				result.fraction = num9;
				return true;
			}
			if (num9 != result.fraction)
			{
				result.SetFailure(ParseFailureKind.FormatWithParameter, "Format_RepeatDateTimePattern", @char);
				return false;
			}
			return true;
			IL_09A1:
			if (@char == ' ')
			{
				if (!parseInfo.fAllowInnerWhite && !str.Match(@char))
				{
					if (parseInfo.fAllowTrailingWhite && format.GetNext() && DateTimeParse.ParseByFormat(ref str, ref format, ref parseInfo, dtfi, ref result))
					{
						return true;
					}
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
			}
			else if (format.MatchSpecifiedWord("GMT"))
			{
				format.Index += "GMT".Length - 1;
				result.flags |= ParseFlags.TimeZoneUsed;
				result.timeZoneOffset = TimeSpan.Zero;
				if (!str.Match("GMT"))
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
			}
			else if (!str.Match(@char))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			return true;
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x00063798 File Offset: 0x00062798
		internal static bool TryParseQuoteString(string format, int pos, StringBuilder result, out int returnValue)
		{
			returnValue = 0;
			int length = format.Length;
			int num = pos;
			char c = format[pos++];
			bool flag = false;
			while (pos < length)
			{
				char c2 = format[pos++];
				if (c2 == c)
				{
					flag = true;
					break;
				}
				if (c2 == '\\')
				{
					if (pos >= length)
					{
						return false;
					}
					result.Append(format[pos++]);
				}
				else
				{
					result.Append(c2);
				}
			}
			if (!flag)
			{
				return false;
			}
			returnValue = pos - num;
			return true;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x00063814 File Offset: 0x00062814
		private static bool DoStrictParse(string s, string formatParam, DateTimeStyles styles, DateTimeFormatInfo dtfi, ref DateTimeResult result)
		{
			ParsingInfo parsingInfo = default(ParsingInfo);
			parsingInfo.Init();
			parsingInfo.calendar = dtfi.Calendar;
			parsingInfo.fAllowInnerWhite = (styles & DateTimeStyles.AllowInnerWhite) != DateTimeStyles.None;
			parsingInfo.fAllowTrailingWhite = (styles & DateTimeStyles.AllowTrailingWhite) != DateTimeStyles.None;
			if (formatParam.Length == 1)
			{
				if ((result.flags & ParseFlags.CaptureOffset) != (ParseFlags)0 && formatParam[0] == 'U')
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadFormatSpecifier", null);
					return false;
				}
				formatParam = DateTimeParse.ExpandPredefinedFormat(formatParam, ref dtfi, ref parsingInfo, ref result);
			}
			result.calendar = parsingInfo.calendar;
			if (parsingInfo.calendar.ID == 8)
			{
				parsingInfo.parseNumberDelegate = DateTimeParse.m_hebrewNumberParser;
				parsingInfo.fCustomNumberParser = true;
			}
			result.Hour = (result.Minute = (result.Second = -1));
			__DTString _DTString = new __DTString(formatParam, dtfi, false);
			__DTString _DTString2 = new __DTString(s, dtfi, false);
			if (parsingInfo.fAllowTrailingWhite)
			{
				_DTString.TrimTail();
				_DTString.RemoveTrailingInQuoteSpaces();
				_DTString2.TrimTail();
			}
			if ((styles & DateTimeStyles.AllowLeadingWhite) != DateTimeStyles.None)
			{
				_DTString.SkipWhiteSpaces();
				_DTString.RemoveLeadingInQuoteSpaces();
				_DTString2.SkipWhiteSpaces();
			}
			while (_DTString.GetNext())
			{
				if (parsingInfo.fAllowInnerWhite)
				{
					_DTString2.SkipWhiteSpaces();
				}
				if (!DateTimeParse.ParseByFormat(ref _DTString2, ref _DTString, ref parsingInfo, dtfi, ref result))
				{
					return false;
				}
			}
			if (_DTString2.Index < _DTString2.Value.Length - 1)
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
				return false;
			}
			if (parsingInfo.fUseTwoDigitYear && (dtfi.FormatFlags & DateTimeFormatFlags.UseHebrewRule) == DateTimeFormatFlags.None)
			{
				if (result.Year >= 100)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				result.Year = parsingInfo.calendar.ToFourDigitYear(result.Year);
			}
			if (parsingInfo.fUseHour12)
			{
				if (parsingInfo.timeMark == DateTimeParse.TM.NotSet)
				{
					parsingInfo.timeMark = DateTimeParse.TM.AM;
				}
				if (result.Hour > 12)
				{
					result.SetFailure(ParseFailureKind.Format, "Format_BadDateTime", null);
					return false;
				}
				if (parsingInfo.timeMark == DateTimeParse.TM.AM)
				{
					if (result.Hour == 12)
					{
						result.Hour = 0;
					}
				}
				else
				{
					result.Hour = ((result.Hour == 12) ? 12 : (result.Hour + 12));
				}
			}
			bool flag = result.Year == -1 && result.Month == -1 && result.Day == -1;
			if (!DateTimeParse.CheckDefaultDateTime(ref result, ref parsingInfo.calendar, styles))
			{
				return false;
			}
			if (!flag && dtfi.HasYearMonthAdjustment && !dtfi.YearMonthAdjustment(ref result.Year, ref result.Month, (result.flags & ParseFlags.ParsedMonthName) != (ParseFlags)0))
			{
				result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
				return false;
			}
			if (!parsingInfo.calendar.TryToDateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second, 0, result.era, out result.parsedDate))
			{
				result.SetFailure(ParseFailureKind.FormatBadDateTimeCalendar, "Format_BadDateTimeCalendar", null);
				return false;
			}
			if (result.fraction > 0.0)
			{
				result.parsedDate = result.parsedDate.AddTicks((long)Math.Round(result.fraction * 10000000.0));
			}
			if (parsingInfo.dayOfWeek != -1 && parsingInfo.dayOfWeek != (int)parsingInfo.calendar.GetDayOfWeek(result.parsedDate))
			{
				result.SetFailure(ParseFailureKind.Format, "Format_BadDayOfWeek", null);
				return false;
			}
			return DateTimeParse.DetermineTimeZoneAdjustments(ref result, styles, flag);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x00063BA8 File Offset: 0x00062BA8
		private static Exception GetDateTimeParseException(ref DateTimeResult result)
		{
			switch (result.failure)
			{
			case ParseFailureKind.ArgumentNull:
				return new ArgumentNullException(result.failureArgumentName, Environment.GetResourceString(result.failureMessageID));
			case ParseFailureKind.Format:
				return new FormatException(Environment.GetResourceString(result.failureMessageID));
			case ParseFailureKind.FormatWithParameter:
				return new FormatException(Environment.GetResourceString(result.failureMessageID, new object[] { result.failureMessageFormatArgument }));
			case ParseFailureKind.FormatBadDateTimeCalendar:
				return new FormatException(Environment.GetResourceString(result.failureMessageID, new object[] { result.calendar }));
			default:
				return null;
			}
		}

		// Token: 0x04000F56 RID: 3926
		internal const int MaxDateTimeNumberDigits = 8;

		// Token: 0x04000F57 RID: 3927
		internal const string GMTName = "GMT";

		// Token: 0x04000F58 RID: 3928
		internal const string ZuluName = "Z";

		// Token: 0x04000F59 RID: 3929
		private const int ORDER_YMD = 0;

		// Token: 0x04000F5A RID: 3930
		private const int ORDER_MDY = 1;

		// Token: 0x04000F5B RID: 3931
		private const int ORDER_DMY = 2;

		// Token: 0x04000F5C RID: 3932
		private const int ORDER_YDM = 3;

		// Token: 0x04000F5D RID: 3933
		private const int ORDER_YM = 4;

		// Token: 0x04000F5E RID: 3934
		private const int ORDER_MY = 5;

		// Token: 0x04000F5F RID: 3935
		private const int ORDER_MD = 6;

		// Token: 0x04000F60 RID: 3936
		private const int ORDER_DM = 7;

		// Token: 0x04000F61 RID: 3937
		internal static DateTimeParse.MatchNumberDelegate m_hebrewNumberParser = new DateTimeParse.MatchNumberDelegate(DateTimeParse.MatchHebrewDigits);

		// Token: 0x04000F62 RID: 3938
		private static DateTimeParse.DS[][] dateParsingStates = new DateTimeParse.DS[][]
		{
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.N,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.BEGIN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.N,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.D_YNd,
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.N,
				DateTimeParse.DS.N,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.NN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NN,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.D_YNd,
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_NN,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.DX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_NNY,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NNd,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.D_YMd,
				DateTimeParse.DS.DX_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_M,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_MN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_MNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_MNd,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_NDS,
				DateTimeParse.DS.DX_NNDS,
				DateTimeParse.DS.DX_NNDS,
				DateTimeParse.DS.DX_NNDS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_NDS,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.D_YNd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YM,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.D_YMd,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_Y,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_YN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.DX_YNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YN,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_YM,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.DX_YMN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_YM,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.DX_DS,
				DateTimeParse.DS.DX_DSN,
				DateTimeParse.DS.TX_N,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.TX_TS,
				DateTimeParse.DS.TX_TS,
				DateTimeParse.DS.TX_TS,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.D_Nd,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.D_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.ERROR
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.TX_NN,
				DateTimeParse.DS.TX_NN,
				DateTimeParse.DS.TX_NN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.DX_NM,
				DateTimeParse.DS.D_NM,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.T_Nt,
				DateTimeParse.DS.TX_NN
			},
			new DateTimeParse.DS[]
			{
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.TX_NNN,
				DateTimeParse.DS.TX_NNN,
				DateTimeParse.DS.TX_NNN,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_S,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.ERROR,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.T_NNt,
				DateTimeParse.DS.TX_NNN
			}
		};

		// Token: 0x02000382 RID: 898
		// (Invoke) Token: 0x060024A5 RID: 9381
		internal delegate bool MatchNumberDelegate(ref __DTString str, int digitLen, out int result);

		// Token: 0x02000383 RID: 899
		internal enum DTT
		{
			// Token: 0x04000F64 RID: 3940
			End,
			// Token: 0x04000F65 RID: 3941
			NumEnd,
			// Token: 0x04000F66 RID: 3942
			NumAmpm,
			// Token: 0x04000F67 RID: 3943
			NumSpace,
			// Token: 0x04000F68 RID: 3944
			NumDatesep,
			// Token: 0x04000F69 RID: 3945
			NumTimesep,
			// Token: 0x04000F6A RID: 3946
			MonthEnd,
			// Token: 0x04000F6B RID: 3947
			MonthSpace,
			// Token: 0x04000F6C RID: 3948
			MonthDatesep,
			// Token: 0x04000F6D RID: 3949
			NumDatesuff,
			// Token: 0x04000F6E RID: 3950
			NumTimesuff,
			// Token: 0x04000F6F RID: 3951
			DayOfWeek,
			// Token: 0x04000F70 RID: 3952
			YearSpace,
			// Token: 0x04000F71 RID: 3953
			YearDateSep,
			// Token: 0x04000F72 RID: 3954
			YearEnd,
			// Token: 0x04000F73 RID: 3955
			TimeZone,
			// Token: 0x04000F74 RID: 3956
			Era,
			// Token: 0x04000F75 RID: 3957
			NumUTCTimeMark,
			// Token: 0x04000F76 RID: 3958
			Unk,
			// Token: 0x04000F77 RID: 3959
			NumLocalTimeMark,
			// Token: 0x04000F78 RID: 3960
			Max
		}

		// Token: 0x02000384 RID: 900
		internal enum TM
		{
			// Token: 0x04000F7A RID: 3962
			NotSet = -1,
			// Token: 0x04000F7B RID: 3963
			AM,
			// Token: 0x04000F7C RID: 3964
			PM
		}

		// Token: 0x02000385 RID: 901
		internal enum DS
		{
			// Token: 0x04000F7E RID: 3966
			BEGIN,
			// Token: 0x04000F7F RID: 3967
			N,
			// Token: 0x04000F80 RID: 3968
			NN,
			// Token: 0x04000F81 RID: 3969
			D_Nd,
			// Token: 0x04000F82 RID: 3970
			D_NN,
			// Token: 0x04000F83 RID: 3971
			D_NNd,
			// Token: 0x04000F84 RID: 3972
			D_M,
			// Token: 0x04000F85 RID: 3973
			D_MN,
			// Token: 0x04000F86 RID: 3974
			D_NM,
			// Token: 0x04000F87 RID: 3975
			D_MNd,
			// Token: 0x04000F88 RID: 3976
			D_NDS,
			// Token: 0x04000F89 RID: 3977
			D_Y,
			// Token: 0x04000F8A RID: 3978
			D_YN,
			// Token: 0x04000F8B RID: 3979
			D_YNd,
			// Token: 0x04000F8C RID: 3980
			D_YM,
			// Token: 0x04000F8D RID: 3981
			D_YMd,
			// Token: 0x04000F8E RID: 3982
			D_S,
			// Token: 0x04000F8F RID: 3983
			T_S,
			// Token: 0x04000F90 RID: 3984
			T_Nt,
			// Token: 0x04000F91 RID: 3985
			T_NNt,
			// Token: 0x04000F92 RID: 3986
			ERROR,
			// Token: 0x04000F93 RID: 3987
			DX_NN,
			// Token: 0x04000F94 RID: 3988
			DX_NNN,
			// Token: 0x04000F95 RID: 3989
			DX_MN,
			// Token: 0x04000F96 RID: 3990
			DX_NM,
			// Token: 0x04000F97 RID: 3991
			DX_MNN,
			// Token: 0x04000F98 RID: 3992
			DX_DS,
			// Token: 0x04000F99 RID: 3993
			DX_DSN,
			// Token: 0x04000F9A RID: 3994
			DX_NDS,
			// Token: 0x04000F9B RID: 3995
			DX_NNDS,
			// Token: 0x04000F9C RID: 3996
			DX_YNN,
			// Token: 0x04000F9D RID: 3997
			DX_YMN,
			// Token: 0x04000F9E RID: 3998
			DX_YN,
			// Token: 0x04000F9F RID: 3999
			DX_YM,
			// Token: 0x04000FA0 RID: 4000
			TX_N,
			// Token: 0x04000FA1 RID: 4001
			TX_NN,
			// Token: 0x04000FA2 RID: 4002
			TX_NNN,
			// Token: 0x04000FA3 RID: 4003
			TX_TS,
			// Token: 0x04000FA4 RID: 4004
			DX_NNY
		}
	}
}

using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System
{
	// Token: 0x02000380 RID: 896
	internal static class DateTimeFormat
	{
		// Token: 0x0600243E RID: 9278 RVA: 0x0005E328 File Offset: 0x0005D328
		private unsafe static void FormatDigits(StringBuilder outputBuffer, int value, int len)
		{
			if (len > 2)
			{
				len = 2;
			}
			char* ptr = stackalloc char[2 * 16];
			char* ptr2 = ptr + 16;
			int num = value;
			do
			{
				*(--ptr2) = (char)(num % 10 + 48);
				num /= 10;
			}
			while (num != 0 && ptr2 != ptr);
			int num2 = (int)((long)(ptr + 16 - ptr2));
			while (num2 < len && ptr2 != ptr)
			{
				*(--ptr2) = '0';
				num2++;
			}
			outputBuffer.Append(ptr2, num2);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0005E392 File Offset: 0x0005D392
		private static void HebrewFormatDigits(StringBuilder outputBuffer, int digits)
		{
			outputBuffer.Append(HebrewNumber.ToString(digits));
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x0005E3A4 File Offset: 0x0005D3A4
		private static int ParseRepeatPattern(string format, int pos, char patternChar)
		{
			int length = format.Length;
			int num = pos + 1;
			while (num < length && format[num] == patternChar)
			{
				num++;
			}
			return num - pos;
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0005E3D3 File Offset: 0x0005D3D3
		private static string FormatDayOfWeek(int dayOfWeek, int repeat, DateTimeFormatInfo dtfi)
		{
			if (repeat == 3)
			{
				return dtfi.GetAbbreviatedDayName((DayOfWeek)dayOfWeek);
			}
			return dtfi.GetDayName((DayOfWeek)dayOfWeek);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0005E3E8 File Offset: 0x0005D3E8
		private static string FormatMonth(int month, int repeatCount, DateTimeFormatInfo dtfi)
		{
			if (repeatCount == 3)
			{
				return dtfi.GetAbbreviatedMonthName(month);
			}
			return dtfi.GetMonthName(month);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0005E400 File Offset: 0x0005D400
		private static string FormatHebrewMonthName(DateTime time, int month, int repeatCount, DateTimeFormatInfo dtfi)
		{
			if (dtfi.Calendar.IsLeapYear(dtfi.Calendar.GetYear(time)))
			{
				return dtfi.internalGetMonthName(month, MonthNameStyles.LeapYear, repeatCount == 3);
			}
			if (month >= 7)
			{
				month++;
			}
			if (repeatCount == 3)
			{
				return dtfi.GetAbbreviatedMonthName(month);
			}
			return dtfi.GetMonthName(month);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0005E450 File Offset: 0x0005D450
		internal static int ParseQuoteString(string format, int pos, StringBuilder result)
		{
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
						throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
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
				throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_BadQuote"), new object[] { c }));
			}
			return pos - num;
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0005E4FE File Offset: 0x0005D4FE
		private static int ParseNextChar(string format, int pos)
		{
			if (pos >= format.Length - 1)
			{
				return -1;
			}
			return (int)format[pos + 1];
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0005E518 File Offset: 0x0005D518
		private static bool IsUseGenitiveForm(string format, int index, int tokenLen, char patternToMatch)
		{
			int num = 0;
			int num2 = index - 1;
			while (num2 >= 0 && format[num2] != patternToMatch)
			{
				num2--;
			}
			if (num2 >= 0)
			{
				while (--num2 >= 0 && format[num2] == patternToMatch)
				{
					num++;
				}
				if (num <= 1)
				{
					return true;
				}
			}
			num2 = index + tokenLen;
			while (num2 < format.Length && format[num2] != patternToMatch)
			{
				num2++;
			}
			if (num2 < format.Length)
			{
				num = 0;
				while (++num2 < format.Length && format[num2] == patternToMatch)
				{
					num++;
				}
				if (num <= 1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0005E5B0 File Offset: 0x0005D5B0
		private static string FormatCustomized(DateTime dateTime, string format, DateTimeFormatInfo dtfi, TimeSpan offset)
		{
			Calendar calendar = dtfi.Calendar;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = calendar.ID == 8;
			bool flag2 = calendar.ID == 3;
			bool flag3 = true;
			int i = 0;
			while (i < format.Length)
			{
				char c = format[i];
				char c2 = c;
				int num2;
				if (c2 <= 'H')
				{
					if (c2 <= '\'')
					{
						if (c2 != '"')
						{
							switch (c2)
							{
							case '%':
							{
								int num = DateTimeFormat.ParseNextChar(format, i);
								if (num >= 0 && num != 37)
								{
									stringBuilder.Append(DateTimeFormat.FormatCustomized(dateTime, ((char)num).ToString(), dtfi, offset));
									num2 = 2;
									goto IL_063F;
								}
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							case '&':
								goto IL_0633;
							case '\'':
								break;
							default:
								goto IL_0633;
							}
						}
						StringBuilder stringBuilder2 = new StringBuilder();
						num2 = DateTimeFormat.ParseQuoteString(format, i, stringBuilder2);
						stringBuilder.Append(stringBuilder2);
					}
					else if (c2 != '/')
					{
						if (c2 != ':')
						{
							switch (c2)
							{
							case 'F':
								goto IL_01C5;
							case 'G':
								goto IL_0633;
							case 'H':
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								DateTimeFormat.FormatDigits(stringBuilder, dateTime.Hour, num2);
								break;
							default:
								goto IL_0633;
							}
						}
						else
						{
							stringBuilder.Append(dtfi.TimeSeparator);
							num2 = 1;
						}
					}
					else
					{
						stringBuilder.Append(dtfi.DateSeparator);
						num2 = 1;
					}
				}
				else if (c2 <= 'h')
				{
					switch (c2)
					{
					case 'K':
						num2 = 1;
						DateTimeFormat.FormatCustomizedRoundripTimeZone(dateTime, offset, stringBuilder);
						break;
					case 'L':
						goto IL_0633;
					case 'M':
					{
						num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						int month = calendar.GetMonth(dateTime);
						if (num2 <= 2)
						{
							if (flag)
							{
								DateTimeFormat.HebrewFormatDigits(stringBuilder, month);
							}
							else
							{
								DateTimeFormat.FormatDigits(stringBuilder, month, num2);
							}
						}
						else if (flag)
						{
							stringBuilder.Append(DateTimeFormat.FormatHebrewMonthName(dateTime, month, num2, dtfi));
						}
						else if ((dtfi.FormatFlags & DateTimeFormatFlags.UseGenitiveMonth) != DateTimeFormatFlags.None && num2 >= 4)
						{
							stringBuilder.Append(dtfi.internalGetMonthName(month, DateTimeFormat.IsUseGenitiveForm(format, i, num2, 'd') ? MonthNameStyles.Genitive : MonthNameStyles.Regular, false));
						}
						else
						{
							stringBuilder.Append(DateTimeFormat.FormatMonth(month, num2, dtfi));
						}
						flag3 = false;
						break;
					}
					default:
						if (c2 != '\\')
						{
							switch (c2)
							{
							case 'd':
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								if (num2 <= 2)
								{
									int dayOfMonth = calendar.GetDayOfMonth(dateTime);
									if (flag)
									{
										DateTimeFormat.HebrewFormatDigits(stringBuilder, dayOfMonth);
									}
									else
									{
										DateTimeFormat.FormatDigits(stringBuilder, dayOfMonth, num2);
									}
								}
								else
								{
									int dayOfWeek = (int)calendar.GetDayOfWeek(dateTime);
									stringBuilder.Append(DateTimeFormat.FormatDayOfWeek(dayOfWeek, num2, dtfi));
								}
								flag3 = false;
								break;
							case 'e':
								goto IL_0633;
							case 'f':
								goto IL_01C5;
							case 'g':
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								stringBuilder.Append(dtfi.GetEraName(calendar.GetEra(dateTime)));
								break;
							case 'h':
							{
								num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
								int num3 = dateTime.Hour % 12;
								if (num3 == 0)
								{
									num3 = 12;
								}
								DateTimeFormat.FormatDigits(stringBuilder, num3, num2);
								break;
							}
							default:
								goto IL_0633;
							}
						}
						else
						{
							int num = DateTimeFormat.ParseNextChar(format, i);
							if (num < 0)
							{
								throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
							}
							stringBuilder.Append((char)num);
							num2 = 2;
						}
						break;
					}
				}
				else if (c2 != 'm')
				{
					switch (c2)
					{
					case 's':
						num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						DateTimeFormat.FormatDigits(stringBuilder, dateTime.Second, num2);
						break;
					case 't':
						num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
						if (num2 == 1)
						{
							if (dateTime.Hour < 12)
							{
								if (dtfi.AMDesignator.Length >= 1)
								{
									stringBuilder.Append(dtfi.AMDesignator[0]);
								}
							}
							else if (dtfi.PMDesignator.Length >= 1)
							{
								stringBuilder.Append(dtfi.PMDesignator[0]);
							}
						}
						else
						{
							stringBuilder.Append((dateTime.Hour < 12) ? dtfi.AMDesignator : dtfi.PMDesignator);
						}
						break;
					default:
						switch (c2)
						{
						case 'y':
						{
							int year = calendar.GetYear(dateTime);
							num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							if (flag2 && !GregorianCalendarHelper.FormatJapaneseFirstYearAsANumber && year == 1 && ((i + num2 < format.Length && format[i + num2] == "年"[0]) || (i + num2 < format.Length - 1 && format[i + num2] == '\'' && format[i + num2 + 1] == "年"[0])))
							{
								stringBuilder.Append("元"[0]);
							}
							else if (dtfi.HasForceTwoDigitYears)
							{
								DateTimeFormat.FormatDigits(stringBuilder, year, (num2 <= 2) ? num2 : 2);
							}
							else if (calendar.ID == 8)
							{
								DateTimeFormat.HebrewFormatDigits(stringBuilder, year);
							}
							else if (num2 <= 2)
							{
								DateTimeFormat.FormatDigits(stringBuilder, year % 100, num2);
							}
							else
							{
								string text = "D" + num2;
								stringBuilder.Append(year.ToString(text, CultureInfo.InvariantCulture));
							}
							flag3 = false;
							break;
						}
						case 'z':
							num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
							DateTimeFormat.FormatCustomizedTimeZone(dateTime, offset, format, num2, flag3, stringBuilder);
							break;
						default:
							goto IL_0633;
						}
						break;
					}
				}
				else
				{
					num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
					DateTimeFormat.FormatDigits(stringBuilder, dateTime.Minute, num2);
				}
				IL_063F:
				i += num2;
				continue;
				IL_01C5:
				num2 = DateTimeFormat.ParseRepeatPattern(format, i, c);
				if (num2 > 7)
				{
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				}
				long num4 = dateTime.Ticks % 10000000L;
				num4 /= (long)Math.Pow(10.0, (double)(7 - num2));
				if (c == 'f')
				{
					stringBuilder.Append(((int)num4).ToString(DateTimeFormat.fixedNumberFormats[num2 - 1], CultureInfo.InvariantCulture));
					goto IL_063F;
				}
				int num5 = num2;
				while (num5 > 0 && num4 % 10L == 0L)
				{
					num4 /= 10L;
					num5--;
				}
				if (num5 > 0)
				{
					stringBuilder.Append(((int)num4).ToString(DateTimeFormat.fixedNumberFormats[num5 - 1], CultureInfo.InvariantCulture));
					goto IL_063F;
				}
				if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '.')
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
					goto IL_063F;
				}
				goto IL_063F;
				IL_0633:
				stringBuilder.Append(c);
				num2 = 1;
				goto IL_063F;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0005EC18 File Offset: 0x0005DC18
		private static void FormatCustomizedTimeZone(DateTime dateTime, TimeSpan offset, string format, int tokenLen, bool timeOnly, StringBuilder result)
		{
			bool flag = offset == DateTimeFormat.NullOffset;
			if (flag)
			{
				if (timeOnly && dateTime.Ticks < 864000000000L)
				{
					offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
				}
				else
				{
					if (dateTime.Kind == DateTimeKind.Utc)
					{
						DateTimeFormat.InvalidFormatForUtc(format, dateTime);
						dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
					}
					offset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
				}
			}
			if (offset >= TimeSpan.Zero)
			{
				result.Append('+');
			}
			else
			{
				result.Append('-');
				offset = offset.Negate();
			}
			if (tokenLen <= 1)
			{
				result.AppendFormat(CultureInfo.InvariantCulture, "{0:0}", new object[] { offset.Hours });
				return;
			}
			result.AppendFormat(CultureInfo.InvariantCulture, "{0:00}", new object[] { offset.Hours });
			if (tokenLen >= 3)
			{
				result.AppendFormat(CultureInfo.InvariantCulture, ":{0:00}", new object[] { offset.Minutes });
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0005ED34 File Offset: 0x0005DD34
		private static void FormatCustomizedRoundripTimeZone(DateTime dateTime, TimeSpan offset, StringBuilder result)
		{
			if (offset == DateTimeFormat.NullOffset)
			{
				switch (dateTime.Kind)
				{
				case DateTimeKind.Utc:
					result.Append("Z");
					return;
				case DateTimeKind.Local:
					offset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
					break;
				default:
					return;
				}
			}
			if (offset >= TimeSpan.Zero)
			{
				result.Append('+');
			}
			else
			{
				result.Append('-');
				offset = offset.Negate();
			}
			result.AppendFormat(CultureInfo.InvariantCulture, "{0:00}:{1:00}", new object[] { offset.Hours, offset.Minutes });
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0005EDE4 File Offset: 0x0005DDE4
		internal static string GetRealFormat(string format, DateTimeFormatInfo dtfi)
		{
			char c = format[0];
			if (c > 'U')
			{
				if (c != 'Y')
				{
					switch (c)
					{
					case 'd':
						return dtfi.ShortDatePattern;
					case 'e':
						goto IL_0159;
					case 'f':
						return dtfi.LongDatePattern + " " + dtfi.ShortTimePattern;
					case 'g':
						return dtfi.GeneralShortTimePattern;
					default:
						switch (c)
						{
						case 'm':
							goto IL_0109;
						case 'n':
						case 'p':
						case 'q':
						case 'v':
						case 'w':
						case 'x':
							goto IL_0159;
						case 'o':
							goto IL_0112;
						case 'r':
							goto IL_011A;
						case 's':
							return dtfi.SortableDateTimePattern;
						case 't':
							return dtfi.ShortTimePattern;
						case 'u':
							return dtfi.UniversalSortableDateTimePattern;
						case 'y':
							break;
						default:
							goto IL_0159;
						}
						break;
					}
				}
				return dtfi.YearMonthPattern;
			}
			switch (c)
			{
			case 'D':
				return dtfi.LongDatePattern;
			case 'E':
				goto IL_0159;
			case 'F':
				return dtfi.FullDateTimePattern;
			case 'G':
				return dtfi.GeneralLongTimePattern;
			default:
				switch (c)
				{
				case 'M':
					break;
				case 'N':
				case 'P':
				case 'Q':
				case 'S':
					goto IL_0159;
				case 'O':
					goto IL_0112;
				case 'R':
					goto IL_011A;
				case 'T':
					return dtfi.LongTimePattern;
				case 'U':
					return dtfi.FullDateTimePattern;
				default:
					goto IL_0159;
				}
				break;
			}
			IL_0109:
			return dtfi.MonthDayPattern;
			IL_0112:
			return "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
			IL_011A:
			return dtfi.RFC1123Pattern;
			IL_0159:
			throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0005EF5C File Offset: 0x0005DF5C
		private static string ExpandPredefinedFormat(string format, ref DateTime dateTime, ref DateTimeFormatInfo dtfi, ref TimeSpan offset)
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
					goto IL_005A;
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
					goto IL_005A;
				case 's':
					dtfi = DateTimeFormatInfo.InvariantInfo;
					goto IL_015B;
				case 'u':
					if (offset != DateTimeFormat.NullOffset)
					{
						dateTime -= offset;
					}
					else if (dateTime.Kind == DateTimeKind.Local)
					{
						DateTimeFormat.InvalidFormatForLocal(format, dateTime);
					}
					dtfi = DateTimeFormatInfo.InvariantInfo;
					goto IL_015B;
				default:
					goto IL_015B;
				}
			}
			else
			{
				if (offset != DateTimeFormat.NullOffset)
				{
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				}
				dtfi = (DateTimeFormatInfo)dtfi.Clone();
				if (dtfi.Calendar.GetType() != typeof(GregorianCalendar))
				{
					dtfi.Calendar = GregorianCalendar.GetDefaultInstance();
				}
				dateTime = dateTime.ToUniversalTime();
				goto IL_015B;
			}
			dtfi = DateTimeFormatInfo.InvariantInfo;
			goto IL_015B;
			IL_005A:
			if (offset != DateTimeFormat.NullOffset)
			{
				dateTime -= offset;
			}
			else if (dateTime.Kind == DateTimeKind.Local)
			{
				DateTimeFormat.InvalidFormatForLocal(format, dateTime);
			}
			dtfi = DateTimeFormatInfo.InvariantInfo;
			IL_015B:
			format = DateTimeFormat.GetRealFormat(format, dtfi);
			return format;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0005F0CF File Offset: 0x0005E0CF
		internal static string Format(DateTime dateTime, string format, DateTimeFormatInfo dtfi)
		{
			return DateTimeFormat.Format(dateTime, format, dtfi, DateTimeFormat.NullOffset);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0005F0E0 File Offset: 0x0005E0E0
		internal static string Format(DateTime dateTime, string format, DateTimeFormatInfo dtfi, TimeSpan offset)
		{
			if (format == null || format.Length == 0)
			{
				bool flag = false;
				if (dateTime.Ticks < 864000000000L)
				{
					int id = dtfi.Calendar.ID;
					switch (id)
					{
					case 3:
					case 4:
					case 6:
					case 8:
						break;
					case 5:
					case 7:
						goto IL_0061;
					default:
						if (id != 13 && id != 23)
						{
							goto IL_0061;
						}
						break;
					}
					flag = true;
					dtfi = DateTimeFormatInfo.InvariantInfo;
				}
				IL_0061:
				if (offset == DateTimeFormat.NullOffset)
				{
					if (flag)
					{
						format = "s";
					}
					else
					{
						format = "G";
					}
				}
				else if (flag)
				{
					format = "yyyy'-'MM'-'ddTHH':'mm':'ss zzz";
				}
				else
				{
					format = dtfi.DateTimeOffsetPattern;
				}
			}
			if (format.Length == 1)
			{
				format = DateTimeFormat.ExpandPredefinedFormat(format, ref dateTime, ref dtfi, ref offset);
			}
			return DateTimeFormat.FormatCustomized(dateTime, format, dtfi, offset);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0005F1A4 File Offset: 0x0005E1A4
		internal static string[] GetAllDateTimes(DateTime dateTime, char format, DateTimeFormatInfo dtfi)
		{
			string[] array;
			string[] array2;
			if (format <= 'U')
			{
				switch (format)
				{
				case 'D':
				case 'F':
				case 'G':
					break;
				case 'E':
					goto IL_0153;
				default:
					switch (format)
					{
					case 'M':
					case 'T':
						break;
					case 'N':
					case 'P':
					case 'Q':
					case 'S':
						goto IL_0153;
					case 'O':
					case 'R':
						goto IL_0127;
					case 'U':
					{
						DateTime dateTime2 = dateTime.ToUniversalTime();
						array = dtfi.GetAllDateTimePatterns(format);
						array2 = new string[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2[i] = DateTimeFormat.Format(dateTime2, array[i], dtfi);
						}
						return array2;
					}
					default:
						goto IL_0153;
					}
					break;
				}
			}
			else if (format != 'Y')
			{
				switch (format)
				{
				case 'd':
				case 'f':
				case 'g':
					break;
				case 'e':
					goto IL_0153;
				default:
					switch (format)
					{
					case 'm':
					case 't':
					case 'y':
						break;
					case 'n':
					case 'p':
					case 'q':
					case 'v':
					case 'w':
					case 'x':
						goto IL_0153;
					case 'o':
					case 'r':
					case 's':
					case 'u':
						goto IL_0127;
					default:
						goto IL_0153;
					}
					break;
				}
			}
			array = dtfi.GetAllDateTimePatterns(format);
			array2 = new string[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = DateTimeFormat.Format(dateTime, array[j], dtfi);
			}
			return array2;
			IL_0127:
			return new string[] { DateTimeFormat.Format(dateTime, new string(new char[] { format }), dtfi) };
			IL_0153:
			throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0005F318 File Offset: 0x0005E318
		internal static string[] GetAllDateTimes(DateTime dateTime, DateTimeFormatInfo dtfi)
		{
			ArrayList arrayList = new ArrayList(132);
			for (int i = 0; i < DateTimeFormat.allStandardFormats.Length; i++)
			{
				string[] allDateTimes = DateTimeFormat.GetAllDateTimes(dateTime, DateTimeFormat.allStandardFormats[i], dtfi);
				for (int j = 0; j < allDateTimes.Length; j++)
				{
					arrayList.Add(allDateTimes[j]);
				}
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(0, array, 0, arrayList.Count);
			return array;
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0005F388 File Offset: 0x0005E388
		internal static void InvalidFormatForLocal(string format, DateTime dateTime)
		{
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0005F38A File Offset: 0x0005E38A
		internal static void InvalidFormatForUtc(string format, DateTime dateTime)
		{
			Mda.DateTimeInvalidLocalFormat();
		}

		// Token: 0x04000F4F RID: 3919
		internal const int MaxSecondsFractionDigits = 7;

		// Token: 0x04000F50 RID: 3920
		internal const string RoundtripFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";

		// Token: 0x04000F51 RID: 3921
		internal const string RoundtripDateTimeUnfixed = "yyyy'-'MM'-'ddTHH':'mm':'ss zzz";

		// Token: 0x04000F52 RID: 3922
		private const int DEFAULT_ALL_DATETIMES_SIZE = 132;

		// Token: 0x04000F53 RID: 3923
		internal static readonly TimeSpan NullOffset = TimeSpan.MinValue;

		// Token: 0x04000F54 RID: 3924
		internal static char[] allStandardFormats = new char[]
		{
			'd', 'D', 'f', 'F', 'g', 'G', 'm', 'M', 'o', 'O',
			'r', 'R', 's', 't', 'T', 'u', 'U', 'y', 'Y'
		};

		// Token: 0x04000F55 RID: 3925
		private static string[] fixedNumberFormats = new string[] { "0", "00", "000", "0000", "00000", "000000", "0000000" };
	}
}

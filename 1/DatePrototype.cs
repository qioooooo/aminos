using System;
using System.Globalization;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x0200005B RID: 91
	public class DatePrototype : DateObject
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x00021DC1 File Offset: 0x00020DC1
		internal DatePrototype(ObjectPrototype parent)
			: base(parent, 0.0)
		{
			this.noExpando = true;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00021DDA File Offset: 0x00020DDA
		private static double Day(double time)
		{
			return Math.Floor(time / 86400000.0);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00021DEC File Offset: 0x00020DEC
		private static double TimeWithinDay(double time)
		{
			double num = time % 86400000.0;
			if (num < 0.0)
			{
				num += 86400000.0;
			}
			return num;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00021E20 File Offset: 0x00020E20
		private static int DaysInYear(double year)
		{
			if (year % 4.0 != 0.0)
			{
				return 365;
			}
			if (year % 100.0 != 0.0)
			{
				return 366;
			}
			if (year % 400.0 != 0.0)
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00021E88 File Offset: 0x00020E88
		private static double DayFromYear(double year)
		{
			return 365.0 * (year - 1970.0) + Math.Floor((year - 1969.0) / 4.0) - Math.Floor((year - 1901.0) / 100.0) + Math.Floor((year - 1601.0) / 400.0);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00021EFB File Offset: 0x00020EFB
		private static double TimeFromYear(double year)
		{
			return 86400000.0 * DatePrototype.DayFromYear(year);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00021F10 File Offset: 0x00020F10
		private static double YearFromTime(double time)
		{
			double num = Math.Floor(time / 86400000.0);
			double num2 = 1970.0 + Math.Floor((400.0 * num + 398.0) / 146097.0);
			double num3 = DatePrototype.DayFromYear(num2);
			if (num < num3)
			{
				num2 -= 1.0;
			}
			return num2;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00021F78 File Offset: 0x00020F78
		private static bool InLeapYear(double year)
		{
			return year % 4.0 == 0.0 && (year % 100.0 != 0.0 || year % 400.0 == 0.0);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00021FD0 File Offset: 0x00020FD0
		private static int MonthFromTime(double time)
		{
			int num = 0;
			int i = DatePrototype.DayWithinYear(time) + 1;
			if (DatePrototype.InLeapYear(DatePrototype.YearFromTime(time)))
			{
				while (i > DatePrototype.leapDaysToMonthEnd[num])
				{
					num++;
				}
			}
			else
			{
				while (i > DatePrototype.daysToMonthEnd[num])
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00022016 File Offset: 0x00021016
		private static int DayWithinYear(double time)
		{
			return (int)(DatePrototype.Day(time) - DatePrototype.DayFromYear(DatePrototype.YearFromTime(time)));
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0002202C File Offset: 0x0002102C
		private static int DateFromTime(double time)
		{
			int num = 0;
			int i = DatePrototype.DayWithinYear(time) + 1;
			if (i <= 31)
			{
				return i;
			}
			if (DatePrototype.InLeapYear(DatePrototype.YearFromTime(time)))
			{
				while (i > DatePrototype.leapDaysToMonthEnd[num])
				{
					num++;
				}
				return i - DatePrototype.leapDaysToMonthEnd[num - 1];
			}
			while (i > DatePrototype.daysToMonthEnd[num])
			{
				num++;
			}
			return i - DatePrototype.daysToMonthEnd[num - 1];
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00022090 File Offset: 0x00021090
		private static int WeekDay(double time)
		{
			double num = (DatePrototype.Day(time) + 4.0) % 7.0;
			if (num < 0.0)
			{
				num += 7.0;
			}
			return (int)num;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000220D4 File Offset: 0x000210D4
		private static bool DaylightSavingsTime(double localTime)
		{
			if (!DatePrototype.useDST)
			{
				return false;
			}
			double num = (localTime + 62135596800000.0) * 10000.0;
			if (-9.223372036854776E+18 <= num && num <= 9.223372036854776E+18)
			{
				try
				{
					DateTime dateTime = new DateTime((long)num);
					return TimeZone.CurrentTimeZone.IsDaylightSavingTime(dateTime);
				}
				catch (ArgumentOutOfRangeException)
				{
				}
			}
			int num2 = DatePrototype.MonthFromTime(localTime);
			if (num2 < 3 || num2 > 9)
			{
				return false;
			}
			if (num2 > 3 && num2 < 9)
			{
				return true;
			}
			int num3 = DatePrototype.DateFromTime(localTime);
			if (num2 == 3)
			{
				if (num3 > 7)
				{
					return true;
				}
				int num4 = DatePrototype.WeekDay(localTime);
				if (num4 > 0)
				{
					return num3 > num4;
				}
				return DatePrototype.HourFromTime(localTime) > 1;
			}
			else
			{
				if (num3 < 25)
				{
					return true;
				}
				int num5 = DatePrototype.WeekDay(localTime);
				if (num5 > 0)
				{
					return num3 - num5 < 25;
				}
				return DatePrototype.HourFromTime(localTime) < 1;
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000222AC File Offset: 0x000212AC
		static DatePrototype()
		{
			DateTime dateTime = new DateTime(DateTime.Now.Year, 1, 1);
			double num = (double)(dateTime.Ticks - dateTime.ToUniversalTime().Ticks) / 10000.0;
			DateTime dateTime2 = new DateTime(DateTime.Now.Year, 7, 1);
			double num2 = (double)(dateTime2.Ticks - dateTime2.ToUniversalTime().Ticks) / 10000.0;
			if (num < num2)
			{
				DatePrototype.localStandardTZA = num;
				DatePrototype.localDaylightTZA = num2;
			}
			else
			{
				DatePrototype.localStandardTZA = num2;
				DatePrototype.localDaylightTZA = num;
			}
			DatePrototype.useDST = DatePrototype.localStandardTZA != DatePrototype.localDaylightTZA;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000226E0 File Offset: 0x000216E0
		private static double LocalTime(double utcTime)
		{
			return utcTime + (DatePrototype.DaylightSavingsTime(utcTime + DatePrototype.localStandardTZA) ? DatePrototype.localDaylightTZA : DatePrototype.localStandardTZA);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000226FE File Offset: 0x000216FE
		internal static double UTC(double localTime)
		{
			return localTime - (DatePrototype.DaylightSavingsTime(localTime) ? DatePrototype.localDaylightTZA : DatePrototype.localStandardTZA);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00022718 File Offset: 0x00021718
		private static int HourFromTime(double time)
		{
			double num = Math.Floor(time / 3600000.0) % 24.0;
			if (num < 0.0)
			{
				num += 24.0;
			}
			return (int)num;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0002275C File Offset: 0x0002175C
		private static int MinFromTime(double time)
		{
			double num = Math.Floor(time / 60000.0) % 60.0;
			if (num < 0.0)
			{
				num += 60.0;
			}
			return (int)num;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000227A0 File Offset: 0x000217A0
		private static int SecFromTime(double time)
		{
			double num = Math.Floor(time / 1000.0) % 60.0;
			if (num < 0.0)
			{
				num += 60.0;
			}
			return (int)num;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000227E4 File Offset: 0x000217E4
		private static int msFromTime(double time)
		{
			double num = time % 1000.0;
			if (num < 0.0)
			{
				num += 1000.0;
			}
			return (int)num;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00022818 File Offset: 0x00021818
		internal static double MakeTime(double hour, double min, double sec, double ms)
		{
			if (double.IsInfinity(hour) || double.IsInfinity(min) || double.IsInfinity(sec) || double.IsInfinity(ms) || hour != hour || min != min || sec != sec || ms != ms)
			{
				return double.NaN;
			}
			hour = (double)((int)Runtime.DoubleToInt64(hour));
			min = (double)((int)Runtime.DoubleToInt64(min));
			sec = (double)((int)Runtime.DoubleToInt64(sec));
			ms = (double)((int)Runtime.DoubleToInt64(ms));
			return hour * 3600000.0 + min * 60000.0 + sec * 1000.0 + ms;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x000228AC File Offset: 0x000218AC
		internal static double MakeDay(double year, double month, double date)
		{
			if (double.IsInfinity(year) || double.IsInfinity(month) || double.IsInfinity(date) || year != year || month != month || date != date)
			{
				return double.NaN;
			}
			year = (double)((int)Runtime.DoubleToInt64(year));
			month = (double)((int)Runtime.DoubleToInt64(month));
			date = (double)((int)Runtime.DoubleToInt64(date));
			year += Math.Floor(month / 12.0);
			month %= 12.0;
			if (month < 0.0)
			{
				month += 12.0;
			}
			double num = 0.0;
			if (month > 0.0)
			{
				if (DatePrototype.InLeapYear((double)((int)Runtime.DoubleToInt64(year))))
				{
					num = (double)DatePrototype.leapDaysToMonthEnd[(int)(month - 1.0)];
				}
				else
				{
					num = (double)DatePrototype.daysToMonthEnd[(int)(month - 1.0)];
				}
			}
			return DatePrototype.DayFromYear(year) - 1.0 + num + date;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000229A2 File Offset: 0x000219A2
		internal static double MakeDate(double day, double time)
		{
			if (double.IsInfinity(day) || double.IsInfinity(time))
			{
				return double.NaN;
			}
			return day * 86400000.0 + time;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000229CB File Offset: 0x000219CB
		internal static double TimeClip(double time)
		{
			if (double.IsInfinity(time))
			{
				return double.NaN;
			}
			if (-8640000000000000.0 <= time && time <= 8640000000000000.0)
			{
				return (double)((long)time);
			}
			return double.NaN;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00022A04 File Offset: 0x00021A04
		internal static string DateToLocaleDateString(double time)
		{
			if (double.IsNaN(time))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = DatePrototype.MonthFromTime(time) + 1;
			if (num < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num);
			stringBuilder.Append("/");
			int num2 = DatePrototype.DateFromTime(time);
			if (num2 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num2);
			stringBuilder.Append("/");
			stringBuilder.Append(DatePrototype.YearString(time));
			return stringBuilder.ToString();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00022A94 File Offset: 0x00021A94
		internal static string DateToLocaleString(double time)
		{
			if (double.IsNaN(time))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = DatePrototype.MonthFromTime(time) + 1;
			if (num < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num);
			stringBuilder.Append("/");
			int num2 = DatePrototype.DateFromTime(time);
			if (num2 < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(num2);
			stringBuilder.Append("/");
			stringBuilder.Append(DatePrototype.YearString(time));
			stringBuilder.Append(" ");
			DatePrototype.AppendTime(time, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00022B38 File Offset: 0x00021B38
		internal static string DateToLocaleTimeString(double time)
		{
			if (double.IsNaN(time))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			DatePrototype.AppendTime(time, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00022B68 File Offset: 0x00021B68
		private static void AppendTime(double time, StringBuilder sb)
		{
			int num = DatePrototype.HourFromTime(time);
			if (num < 10)
			{
				sb.Append("0");
			}
			sb.Append(num);
			sb.Append(":");
			int num2 = DatePrototype.MinFromTime(time);
			if (num2 < 10)
			{
				sb.Append("0");
			}
			sb.Append(num2);
			sb.Append(":");
			int num3 = DatePrototype.SecFromTime(time);
			if (num3 < 10)
			{
				sb.Append("0");
			}
			sb.Append(num3);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00022BF0 File Offset: 0x00021BF0
		private static string TimeZoneID(double utcTime)
		{
			int num = (int)(DatePrototype.localStandardTZA / 3600000.0);
			if (DatePrototype.DaylightSavingsTime(utcTime + DatePrototype.localStandardTZA))
			{
				switch (num)
				{
				case -8:
					return "PDT";
				case -7:
					return "MDT";
				case -6:
					return "CDT";
				case -5:
					return "EDT";
				}
			}
			else
			{
				switch (num)
				{
				case -8:
					return "PST";
				case -7:
					return "MST";
				case -6:
					return "CST";
				case -5:
					return "EST";
				}
			}
			return ((num >= 0) ? "UTC+" : "UTC") + num.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00022CA8 File Offset: 0x00021CA8
		private static string YearString(double time)
		{
			double num = DatePrototype.YearFromTime(time);
			if (num > 0.0)
			{
				return num.ToString(CultureInfo.InvariantCulture);
			}
			return (1.0 - num).ToString(CultureInfo.InvariantCulture) + " B.C.";
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00022CF8 File Offset: 0x00021CF8
		internal static string DateToDateString(double utcTime)
		{
			if (double.IsNaN(utcTime))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			double num = DatePrototype.LocalTime(utcTime);
			stringBuilder.Append(DatePrototype.WeekDayName[DatePrototype.WeekDay(num)]);
			stringBuilder.Append(" ");
			int num2 = DatePrototype.MonthFromTime(num);
			stringBuilder.Append(DatePrototype.MonthName[num2]);
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.DateFromTime(num));
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.YearString(num));
			return stringBuilder.ToString();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00022D8C File Offset: 0x00021D8C
		internal static string DateToString(double utcTime)
		{
			if (double.IsNaN(utcTime))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			double num = DatePrototype.LocalTime(utcTime);
			stringBuilder.Append(DatePrototype.WeekDayName[DatePrototype.WeekDay(num)]);
			stringBuilder.Append(" ");
			int num2 = DatePrototype.MonthFromTime(num);
			stringBuilder.Append(DatePrototype.MonthName[num2]);
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.DateFromTime(num));
			stringBuilder.Append(" ");
			DatePrototype.AppendTime(num, stringBuilder);
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.TimeZoneID(utcTime));
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.YearString(num));
			return stringBuilder.ToString();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00022E4C File Offset: 0x00021E4C
		internal static string DateToTimeString(double utcTime)
		{
			if (double.IsNaN(utcTime))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			double num = DatePrototype.LocalTime(utcTime);
			DatePrototype.AppendTime(num, stringBuilder);
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.TimeZoneID(utcTime));
			return stringBuilder.ToString();
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00022E9C File Offset: 0x00021E9C
		internal static string UTCDateToString(double utcTime)
		{
			if (double.IsNaN(utcTime))
			{
				return "NaN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DatePrototype.WeekDayName[DatePrototype.WeekDay(utcTime)]);
			stringBuilder.Append(", ");
			stringBuilder.Append(DatePrototype.DateFromTime(utcTime));
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.MonthName[DatePrototype.MonthFromTime(utcTime)]);
			stringBuilder.Append(" ");
			stringBuilder.Append(DatePrototype.YearString(utcTime));
			stringBuilder.Append(" ");
			DatePrototype.AppendTime(utcTime, stringBuilder);
			stringBuilder.Append(" UTC");
			return stringBuilder.ToString();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00022F46 File Offset: 0x00021F46
		private static bool NotSpecified(object value)
		{
			return value == null || value is Missing;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00022F56 File Offset: 0x00021F56
		private static bool isASCII(char ch)
		{
			return ch < '\u0080';
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00022F60 File Offset: 0x00021F60
		private static bool isalpha(char ch)
		{
			return ('A' <= ch && ch <= 'Z') || ('a' <= ch && ch <= 'z');
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00022F7D File Offset: 0x00021F7D
		private static bool isdigit(char ch)
		{
			return '0' <= ch && ch <= '9';
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00022F90 File Offset: 0x00021F90
		internal static double ParseDate(string str)
		{
			long num = (long)((ulong)int.MinValue);
			int num2 = 0;
			int num3 = 0;
			Ps ps = Ps.Initial;
			long num4 = num;
			long num5 = num;
			long num6 = num;
			long num7 = num;
			long num8 = num;
			long num9 = num;
			str = str.ToLowerInvariant();
			int i = 0;
			int length = str.Length;
			while (i < length)
			{
				char c = str[i++];
				if (c > ' ')
				{
					char c2 = c;
					switch (c2)
					{
					case '(':
					{
						int num10 = 1;
						while (i < length)
						{
							c = str[i++];
							if (c == '(')
							{
								num10++;
							}
							else if (c == ')' && --num10 <= 0)
							{
								break;
							}
						}
						continue;
					}
					case ')':
					case '*':
					case '.':
						break;
					case '+':
						if (num != num4)
						{
							ps = Ps.AddOffset;
							continue;
						}
						continue;
					case ',':
					case '/':
						continue;
					case '-':
						if (num != num4)
						{
							ps = Ps.SubOffset;
							continue;
						}
						continue;
					default:
						if (c2 == ':')
						{
							continue;
						}
						break;
					}
					if (DatePrototype.isalpha(c))
					{
						int num11 = i - 1;
						while (i < length)
						{
							c = str[i++];
							if (!DatePrototype.isalpha(c) && '.' != c)
							{
								break;
							}
						}
						int num12 = i - num11 - ((i < length) ? 1 : 0);
						if ('.' == str[i - ((i < length) ? 2 : 1)])
						{
							num12--;
						}
						while (c == ' ' && i < length)
						{
							c = str[i++];
						}
						if (1 == num12)
						{
							if (num != num8)
							{
								return double.NaN;
							}
							char c3 = str[num11];
							if (c3 <= 'm')
							{
								if (c3 == 'j' || c3 < 'a')
								{
									return double.NaN;
								}
								num8 = -(long)(c3 - 'a' + ((c3 < 'j') ? '\u0001' : '\0')) * 60L;
							}
							else if (c3 <= 'y')
							{
								num8 = (long)(c3 - 'm') * 60L;
							}
							else
							{
								if (c3 != 'z')
								{
									return double.NaN;
								}
								num8 = 0L;
							}
							if ('+' == c)
							{
								ps = Ps.AddOffset;
							}
							else if ('-' == c)
							{
								ps = Ps.SubOffset;
							}
							else
							{
								ps = Ps.Initial;
							}
						}
						else
						{
							for (int j = DatePrototype.Strings.Length - 1; j >= 0; j--)
							{
								string text = DatePrototype.Strings[j];
								if (text.Length >= num12)
								{
									if (string.CompareOrdinal(str, num11, text, 0, num12) != 0)
									{
										if (j == 0)
										{
											return double.NaN;
										}
									}
									else
									{
										switch (DatePrototype.Tokens[j])
										{
										case Tk.BcAd:
											if (num3 != 0)
											{
												return double.NaN;
											}
											num3 = DatePrototype.Values[j];
											goto IL_031B;
										case Tk.AmPm:
											if (num2 != 0)
											{
												return double.NaN;
											}
											num2 = DatePrototype.Values[j];
											goto IL_031B;
										case Tk.Zone:
											if (num != num8)
											{
												return double.NaN;
											}
											num8 = (long)DatePrototype.Values[j];
											if ('+' == c)
											{
												ps = Ps.AddOffset;
												i++;
												goto IL_031B;
											}
											if ('-' == c)
											{
												ps = Ps.SubOffset;
												i++;
												goto IL_031B;
											}
											ps = Ps.Initial;
											goto IL_031B;
										case Tk.Day:
											goto IL_031B;
										case Tk.Month:
											if (num != num5)
											{
												return double.NaN;
											}
											num5 = (long)DatePrototype.Values[j];
											goto IL_031B;
										default:
											goto IL_031B;
										}
									}
								}
							}
							IL_031B:
							if (i < length)
							{
								i--;
							}
						}
					}
					else
					{
						if (!DatePrototype.isdigit(c))
						{
							return double.NaN;
						}
						int num13 = 0;
						int num14 = i;
						do
						{
							num13 = num13 * 10 + (int)c - 48;
							if (i >= length)
							{
								break;
							}
							c = str[i++];
						}
						while (DatePrototype.isdigit(c));
						if (i - num14 > 6)
						{
							return double.NaN;
						}
						while (c == ' ' && i < length)
						{
							c = str[i++];
						}
						switch (ps)
						{
						case Ps.Minutes:
							if (num13 >= 60)
							{
								return double.NaN;
							}
							num7 += (long)(num13 * 60);
							if (c == ':')
							{
								ps = Ps.Seconds;
							}
							else
							{
								ps = Ps.Initial;
								if (i < length)
								{
									i--;
								}
							}
							break;
						case Ps.Seconds:
							if (num13 >= 60)
							{
								return double.NaN;
							}
							num7 += (long)num13;
							ps = Ps.Initial;
							if (i < length)
							{
								i--;
							}
							break;
						case Ps.AddOffset:
							if (num != num9)
							{
								return double.NaN;
							}
							num9 = (long)((num13 < 24) ? (num13 * 60) : (num13 % 100 + num13 / 100 * 60));
							ps = Ps.Initial;
							if (i < length)
							{
								i--;
							}
							break;
						case Ps.SubOffset:
							if (num != num9)
							{
								return double.NaN;
							}
							num9 = (long)((num13 < 24) ? (-num13 * 60) : (-(long)(num13 % 100 + num13 / 100 * 60)));
							ps = Ps.Initial;
							if (i < length)
							{
								i--;
							}
							break;
						case Ps.Date:
							if (num != num6)
							{
								return double.NaN;
							}
							num6 = (long)num13;
							if ('/' == c || '-' == c)
							{
								ps = Ps.Year;
							}
							else
							{
								ps = Ps.Initial;
								if (i < length)
								{
									i--;
								}
							}
							break;
						case Ps.Year:
							if (num != num4)
							{
								return double.NaN;
							}
							num4 = (long)num13;
							ps = Ps.Initial;
							if (i < length)
							{
								i--;
							}
							break;
						default:
							if (num13 >= 70)
							{
								if (num != num4)
								{
									return double.NaN;
								}
								num4 = (long)num13;
								if (i < length)
								{
									i--;
								}
							}
							else
							{
								char c4 = c;
								switch (c4)
								{
								case '-':
								case '/':
									if (num != num5)
									{
										return double.NaN;
									}
									num5 = (long)(num13 - 1);
									ps = Ps.Date;
									continue;
								case '.':
									break;
								default:
									if (c4 == ':')
									{
										if (num != num7)
										{
											return double.NaN;
										}
										if (num13 >= 24)
										{
											return double.NaN;
										}
										num7 = (long)(num13 * 3600);
										ps = Ps.Minutes;
										continue;
									}
									break;
								}
								if (num != num6)
								{
									return double.NaN;
								}
								num6 = (long)num13;
								if (i < length)
								{
									i--;
								}
							}
							break;
						}
					}
				}
			}
			if (num == num4 || num == num5 || num == num6)
			{
				return double.NaN;
			}
			if (num3 != 0)
			{
				if (num3 < 0)
				{
					num4 = -num4 + 1L;
				}
			}
			else if (num4 < 100L)
			{
				num4 += 1900L;
			}
			if (num2 != 0)
			{
				if (num == num7)
				{
					return double.NaN;
				}
				if (num7 >= 43200L && num7 < 46800L)
				{
					if (num2 < 0)
					{
						num7 -= 43200L;
					}
				}
				else if (num2 > 0)
				{
					if (num7 >= 43200L)
					{
						return double.NaN;
					}
					num7 += 43200L;
				}
			}
			else if (num == num7)
			{
				num7 = 0L;
			}
			bool flag = false;
			if (num != num8)
			{
				num7 -= num8 * 60L;
				flag = true;
			}
			if (num != num9)
			{
				num7 -= num9 * 60L;
			}
			double num15 = DatePrototype.MakeDate(DatePrototype.MakeDay((double)num4, (double)num5, (double)num6), (double)(num7 * 1000L));
			if (!flag)
			{
				num15 = DatePrototype.UTC(num15);
			}
			return num15;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00023676 File Offset: 0x00022676
		public static DateConstructor constructor
		{
			get
			{
				return DatePrototype._constructor;
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00023680 File Offset: 0x00022680
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getDate)]
		public static double getDate(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.DateFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000236C0 File Offset: 0x000226C0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getDay)]
		public static double getDay(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.WeekDay(DatePrototype.LocalTime(value));
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00023700 File Offset: 0x00022700
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getFullYear)]
		public static double getFullYear(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return DatePrototype.YearFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00023740 File Offset: 0x00022740
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getHours)]
		public static double getHours(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.HourFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00023780 File Offset: 0x00022780
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getMilliseconds)]
		public static double getMilliseconds(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.msFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000237C0 File Offset: 0x000227C0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getMinutes)]
		public static double getMinutes(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.MinFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00023800 File Offset: 0x00022800
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getMonth)]
		public static double getMonth(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.MonthFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00023840 File Offset: 0x00022840
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getSeconds)]
		public static double getSeconds(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.SecFromTime(DatePrototype.LocalTime(value));
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0002387E File Offset: 0x0002287E
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getTime)]
		public static double getTime(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			return ((DateObject)thisob).value;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000238A0 File Offset: 0x000228A0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getTimezoneOffset)]
		public static double getTimezoneOffset(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (value - DatePrototype.LocalTime(value)) / 60000.0;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000238E4 File Offset: 0x000228E4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCDate)]
		public static double getUTCDate(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.DateFromTime(value);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00023920 File Offset: 0x00022920
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCDay)]
		public static double getUTCDay(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.WeekDay(value);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0002395C File Offset: 0x0002295C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCFullYear)]
		public static double getUTCFullYear(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return DatePrototype.YearFromTime(value);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00023994 File Offset: 0x00022994
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCHours)]
		public static double getUTCHours(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.HourFromTime(value);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000239D0 File Offset: 0x000229D0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCMilliseconds)]
		public static double getUTCMilliseconds(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.msFromTime(value);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00023A0C File Offset: 0x00022A0C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCMinutes)]
		public static double getUTCMinutes(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.MinFromTime(value);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00023A48 File Offset: 0x00022A48
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCMonth)]
		public static double getUTCMonth(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.MonthFromTime(value);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00023A84 File Offset: 0x00022A84
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getUTCSeconds)]
		public static double getUTCSeconds(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			return (double)DatePrototype.SecFromTime(value);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00023AC0 File Offset: 0x00022AC0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getVarDate)]
		public static object getVarDate(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return null;
			}
			checked
			{
				long num;
				try
				{
					num = (long)(unchecked(DatePrototype.LocalTime(value) + 62135596800000.0)) * 10000L;
				}
				catch (OverflowException)
				{
					return null;
				}
				if (num < DateTime.MinValue.Ticks || num > DateTime.MaxValue.Ticks)
				{
					return null;
				}
				DateTime dateTime;
				try
				{
					dateTime = new DateTime(num);
				}
				catch (ArgumentOutOfRangeException)
				{
					return null;
				}
				return dateTime;
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00023B68 File Offset: 0x00022B68
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_getYear)]
		[NotRecommended("getYear")]
		public static double getYear(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			if (value != value)
			{
				return value;
			}
			double num = DatePrototype.YearFromTime(DatePrototype.LocalTime(value));
			if (1900.0 <= num && num <= 1999.0)
			{
				return num - 1900.0;
			}
			return num;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00023BCC File Offset: 0x00022BCC
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setDate)]
		public static double setDate(object thisob, double ddate)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			double num2 = DatePrototype.MakeDay(DatePrototype.YearFromTime(num), (double)DatePrototype.MonthFromTime(num), ddate);
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(num2, DatePrototype.TimeWithinDay(num))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00023C38 File Offset: 0x00022C38
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setFullYear)]
		public static double setFullYear(object thisob, double dyear, object month, object date)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			double num2 = (DatePrototype.NotSpecified(month) ? ((double)DatePrototype.MonthFromTime(num)) : Convert.ToNumber(month));
			double num3 = (DatePrototype.NotSpecified(date) ? ((double)DatePrototype.DateFromTime(num)) : Convert.ToNumber(date));
			double num4 = DatePrototype.MakeDay(dyear, num2, num3);
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(num4, DatePrototype.TimeWithinDay(num))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00023CC8 File Offset: 0x00022CC8
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setHours)]
		public static double setHours(object thisob, double dhour, object min, object sec, object msec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			double num2 = (DatePrototype.NotSpecified(min) ? ((double)DatePrototype.MinFromTime(num)) : Convert.ToNumber(min));
			double num3 = (DatePrototype.NotSpecified(sec) ? ((double)DatePrototype.SecFromTime(num)) : Convert.ToNumber(sec));
			double num4 = (DatePrototype.NotSpecified(msec) ? ((double)DatePrototype.msFromTime(num)) : Convert.ToNumber(msec));
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime(dhour, num2, num3, num4))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00023D70 File Offset: 0x00022D70
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setMinutes)]
		public static double setMinutes(object thisob, double dmin, object sec, object msec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			double num2 = (DatePrototype.NotSpecified(sec) ? ((double)DatePrototype.SecFromTime(num)) : Convert.ToNumber(sec));
			double num3 = (DatePrototype.NotSpecified(msec) ? ((double)DatePrototype.msFromTime(num)) : Convert.ToNumber(msec));
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime((double)DatePrototype.HourFromTime(num), dmin, num2, num3))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00023E04 File Offset: 0x00022E04
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setMilliseconds)]
		public static double setMilliseconds(object thisob, double dmsec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime((double)DatePrototype.HourFromTime(num), (double)DatePrototype.MinFromTime(num), (double)DatePrototype.SecFromTime(num), dmsec))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00023E74 File Offset: 0x00022E74
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setMonth)]
		public static double setMonth(object thisob, double dmonth, object date)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			double num2 = (DatePrototype.NotSpecified(date) ? ((double)DatePrototype.DateFromTime(num)) : Convert.ToNumber(date));
			double num3 = DatePrototype.MakeDay(DatePrototype.YearFromTime(num), dmonth, num2);
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(num3, DatePrototype.TimeWithinDay(num))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00023EF0 File Offset: 0x00022EF0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setSeconds)]
		public static double setSeconds(object thisob, double dsec, object msec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			double num2 = (DatePrototype.NotSpecified(msec) ? ((double)DatePrototype.msFromTime(num)) : Convert.ToNumber(msec));
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime((double)DatePrototype.HourFromTime(num), (double)DatePrototype.MinFromTime(num), dsec, num2))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00023F71 File Offset: 0x00022F71
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setTime)]
		public static double setTime(object thisob, double time)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			time = DatePrototype.TimeClip(time);
			((DateObject)thisob).value = time;
			return time;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00023F9C File Offset: 0x00022F9C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCDate)]
		public static double setUTCDate(object thisob, double ddate)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			double num2 = DatePrototype.MakeDay(DatePrototype.YearFromTime(num), (double)DatePrototype.MonthFromTime(num), ddate);
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(num2, DatePrototype.TimeWithinDay(num)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00023FFC File Offset: 0x00022FFC
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCFullYear)]
		public static double setUTCFullYear(object thisob, double dyear, object month, object date)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			double num2 = (DatePrototype.NotSpecified(month) ? ((double)DatePrototype.MonthFromTime(num)) : Convert.ToNumber(month));
			double num3 = (DatePrototype.NotSpecified(date) ? ((double)DatePrototype.DateFromTime(num)) : Convert.ToNumber(date));
			double num4 = DatePrototype.MakeDay(dyear, num2, num3);
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(num4, DatePrototype.TimeWithinDay(num)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00024080 File Offset: 0x00023080
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCHours)]
		public static double setUTCHours(object thisob, double dhour, object min, object sec, object msec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			double num2 = (DatePrototype.NotSpecified(min) ? ((double)DatePrototype.MinFromTime(num)) : Convert.ToNumber(min));
			double num3 = (DatePrototype.NotSpecified(sec) ? ((double)DatePrototype.SecFromTime(num)) : Convert.ToNumber(sec));
			double num4 = (DatePrototype.NotSpecified(msec) ? ((double)DatePrototype.msFromTime(num)) : Convert.ToNumber(msec));
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime(dhour, num2, num3, num4)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00024120 File Offset: 0x00023120
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCMinutes)]
		public static double setUTCMinutes(object thisob, double dmin, object sec, object msec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			double num2 = (DatePrototype.NotSpecified(sec) ? ((double)DatePrototype.SecFromTime(num)) : Convert.ToNumber(sec));
			double num3 = (DatePrototype.NotSpecified(msec) ? ((double)DatePrototype.msFromTime(num)) : Convert.ToNumber(msec));
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime((double)DatePrototype.HourFromTime(num), dmin, num2, num3)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000241AC File Offset: 0x000231AC
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCMilliseconds)]
		public static double setUTCMilliseconds(object thisob, double dmsec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime((double)DatePrototype.HourFromTime(num), (double)DatePrototype.MinFromTime(num), (double)DatePrototype.SecFromTime(num), dmsec)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00024214 File Offset: 0x00023214
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCMonth)]
		public static double setUTCMonth(object thisob, double dmonth, object date)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			double num2 = (DatePrototype.NotSpecified(date) ? ((double)DatePrototype.DateFromTime(num)) : Convert.ToNumber(date));
			double num3 = DatePrototype.MakeDay(DatePrototype.YearFromTime(num), dmonth, num2);
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(num3, DatePrototype.TimeWithinDay(num)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00024288 File Offset: 0x00023288
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setUTCSeconds)]
		public static double setUTCSeconds(object thisob, double dsec, object msec)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = ((DateObject)thisob).value;
			double num2 = (DatePrototype.NotSpecified(msec) ? ((double)DatePrototype.msFromTime(num)) : Convert.ToNumber(msec));
			num = DatePrototype.TimeClip(DatePrototype.MakeDate(DatePrototype.Day(num), DatePrototype.MakeTime((double)DatePrototype.HourFromTime(num), (double)DatePrototype.MinFromTime(num), dsec, num2)));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00024300 File Offset: 0x00023300
		[NotRecommended("setYear")]
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_setYear)]
		public static double setYear(object thisob, double dyear)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double num = DatePrototype.LocalTime(((DateObject)thisob).value);
			if (double.IsNaN(dyear))
			{
				((DateObject)thisob).value = dyear;
				return dyear;
			}
			dyear = Convert.ToInteger(dyear);
			if (0.0 <= dyear && dyear <= 99.0)
			{
				dyear += 1900.0;
			}
			double num2 = DatePrototype.MakeDay(dyear, (double)DatePrototype.MonthFromTime(num), (double)DatePrototype.DateFromTime(num));
			num = DatePrototype.TimeClip(DatePrototype.UTC(DatePrototype.MakeDate(num2, DatePrototype.TimeWithinDay(num))));
			((DateObject)thisob).value = num;
			return num;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000243B0 File Offset: 0x000233B0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toDateString)]
		public static string toDateString(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			return DatePrototype.DateToDateString(value);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000243E4 File Offset: 0x000233E4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toGMTString)]
		[NotRecommended("toGMTString")]
		public static string toGMTString(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			return DatePrototype.UTCDateToString(value);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00024418 File Offset: 0x00023418
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toLocaleDateString)]
		public static string toLocaleDateString(object thisob)
		{
			object varDate = DatePrototype.getVarDate(thisob);
			if (varDate != null)
			{
				return ((DateTime)varDate).ToLongDateString();
			}
			return DatePrototype.DateToDateString(((DateObject)thisob).value);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00024450 File Offset: 0x00023450
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toLocaleString)]
		public static string toLocaleString(object thisob)
		{
			object varDate = DatePrototype.getVarDate(thisob);
			if (varDate != null)
			{
				return ((DateTime)varDate).ToLongDateString() + " " + ((DateTime)varDate).ToLongTimeString();
			}
			return DatePrototype.DateToString(((DateObject)thisob).value);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000244A0 File Offset: 0x000234A0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toLocaleTimeString)]
		public static string toLocaleTimeString(object thisob)
		{
			object varDate = DatePrototype.getVarDate(thisob);
			if (varDate != null)
			{
				return ((DateTime)varDate).ToLongTimeString();
			}
			return DatePrototype.DateToTimeString(((DateObject)thisob).value);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000244D8 File Offset: 0x000234D8
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toString)]
		public static string toString(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			return DatePrototype.DateToString(value);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0002450C File Offset: 0x0002350C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toTimeString)]
		public static string toTimeString(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			return DatePrototype.DateToTimeString(value);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00024540 File Offset: 0x00023540
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_toUTCString)]
		public static string toUTCString(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			double value = ((DateObject)thisob).value;
			return DatePrototype.UTCDateToString(value);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00024572 File Offset: 0x00023572
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Date_valueOf)]
		public static double valueOf(object thisob)
		{
			if (!(thisob is DateObject))
			{
				throw new JScriptException(JSError.DateExpected);
			}
			return ((DateObject)thisob).value;
		}

		// Token: 0x04000215 RID: 533
		private const double HoursPerDay = 24.0;

		// Token: 0x04000216 RID: 534
		private const double MinutesPerHour = 60.0;

		// Token: 0x04000217 RID: 535
		private const double SecondsPerMinute = 60.0;

		// Token: 0x04000218 RID: 536
		private const double msPerSecond = 1000.0;

		// Token: 0x04000219 RID: 537
		private const double msPerMinute = 60000.0;

		// Token: 0x0400021A RID: 538
		private const double msPerHour = 3600000.0;

		// Token: 0x0400021B RID: 539
		private const double msPerDay = 86400000.0;

		// Token: 0x0400021C RID: 540
		internal const double msTo1970 = 62135596800000.0;

		// Token: 0x0400021D RID: 541
		internal const double ticksPerMillisecond = 10000.0;

		// Token: 0x0400021E RID: 542
		internal const double maxDate = 8640000000000000.0;

		// Token: 0x0400021F RID: 543
		internal const double minDate = -8640000000000000.0;

		// Token: 0x04000220 RID: 544
		internal static readonly DatePrototype ob = new DatePrototype(ObjectPrototype.ob);

		// Token: 0x04000221 RID: 545
		internal static DateConstructor _constructor;

		// Token: 0x04000222 RID: 546
		private static readonly int[] daysToMonthEnd = new int[]
		{
			31, 59, 90, 120, 151, 181, 212, 243, 273, 304,
			334, 365
		};

		// Token: 0x04000223 RID: 547
		private static readonly int[] leapDaysToMonthEnd = new int[]
		{
			31, 60, 91, 121, 152, 182, 213, 244, 274, 305,
			335, 366
		};

		// Token: 0x04000224 RID: 548
		private static readonly double localStandardTZA;

		// Token: 0x04000225 RID: 549
		private static readonly double localDaylightTZA;

		// Token: 0x04000226 RID: 550
		private static readonly bool useDST;

		// Token: 0x04000227 RID: 551
		private static readonly string[] WeekDayName = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

		// Token: 0x04000228 RID: 552
		private static readonly string[] MonthName = new string[]
		{
			"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct",
			"Nov", "Dec"
		};

		// Token: 0x04000229 RID: 553
		private static readonly string[] Strings = new string[]
		{
			"bc", "b.c", "ad", "a.d", "am", "a.m", "pm", "p.m", "est", "edt",
			"cst", "cdt", "mst", "mdt", "pst", "pdt", "gmt", "utc", "sunday", "monday",
			"tuesday", "wednesday", "thursday", "friday", "saturday", "january", "february", "march", "april", "may",
			"june", "july", "august", "september", "october", "november", "december"
		};

		// Token: 0x0400022A RID: 554
		private static readonly Tk[] Tokens = new Tk[]
		{
			Tk.BcAd,
			Tk.BcAd,
			Tk.BcAd,
			Tk.BcAd,
			Tk.AmPm,
			Tk.AmPm,
			Tk.AmPm,
			Tk.AmPm,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Zone,
			Tk.Day,
			Tk.Day,
			Tk.Day,
			Tk.Day,
			Tk.Day,
			Tk.Day,
			Tk.Day,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month,
			Tk.Month
		};

		// Token: 0x0400022B RID: 555
		private static readonly int[] Values = new int[]
		{
			-1, -1, 1, 1, -1, -1, 1, 1, -300, -240,
			-360, -300, -420, -360, -480, -420, 0, 0, 0, 1,
			2, 3, 4, 5, 6, 0, 1, 2, 3, 4,
			5, 6, 7, 8, 9, 10, 11
		};
	}
}

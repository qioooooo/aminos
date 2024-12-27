using System;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x020003A3 RID: 931
	[Serializable]
	internal class GregorianCalendarHelper
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x00069DD7 File Offset: 0x00068DD7
		internal int MaxYear
		{
			get
			{
				return this.m_maxYear;
			}
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00069DE0 File Offset: 0x00068DE0
		internal static EraInfo[] InitEraInfo(int calID)
		{
			int[][] array = CalendarTable.Default.SERARANGES(calID);
			EraInfo[] array2 = new EraInfo[array.Length];
			int num = 9999;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = new EraInfo(array[i][0], new DateTime(array[i][1], array[i][2], array[i][3]).Ticks, array[i][4], array[i][5], num - array[i][4]);
				num = array[i][1];
			}
			return array2;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00069E58 File Offset: 0x00068E58
		internal GregorianCalendarHelper(Calendar cal, EraInfo[] eraInfo)
		{
			this.m_Cal = cal;
			this.m_EraInfo = eraInfo;
			this.m_minDate = this.m_Cal.MinSupportedDateTime;
			this.m_maxYear = this.m_EraInfo[0].maxEraYear;
			this.m_minYear = this.m_EraInfo[0].minEraYear;
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x00069EBB File Offset: 0x00068EBB
		private static bool EnforceJapaneseEraYearRanges
		{
			get
			{
				if (GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 0)
				{
					GregorianCalendarHelper.InitializeJapaneseCalendarConfigSwitches();
				}
				return GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 1;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x00069ED1 File Offset: 0x00068ED1
		internal static bool FormatJapaneseFirstYearAsANumber
		{
			get
			{
				if (GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber == 0)
				{
					GregorianCalendarHelper.InitializeJapaneseCalendarConfigSwitches();
				}
				return GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber == 1;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x00069EE7 File Offset: 0x00068EE7
		internal static bool EnforceLegacyJapaneseDateParsing
		{
			get
			{
				if (GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing == 0)
				{
					GregorianCalendarHelper.InitializeJapaneseCalendarConfigSwitches();
				}
				return GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing == 1;
			}
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00069F00 File Offset: 0x00068F00
		private static void InitializeJapaneseCalendarConfigSwitches()
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.InternalOpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\AppContext", false))
				{
					if (registryKey == null)
					{
						GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = 2;
						GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber = 2;
						GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing = 2;
					}
					else
					{
						string text = registryKey.InternalGetValue("Switch.System.Globalization.EnforceJapaneseEraYearRanges", null, false, false) as string;
						GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = ((text == null) ? 2 : ((text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 2));
						text = registryKey.InternalGetValue("Switch.System.Globalization.FormatJapaneseFirstYearAsANumber", null, false, false) as string;
						GregorianCalendarHelper.s_formatJapaneseFirstYearAsANumber = ((text == null) ? 2 : ((text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 2));
						text = registryKey.InternalGetValue("Switch.System.Globalization.EnforceLegacyJapaneseDateParsing", null, false, false) as string;
						GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing = ((text == null) ? 2 : ((text == "1" || text.Equals("true", StringComparison.OrdinalIgnoreCase)) ? 1 : 2));
					}
				}
			}
			catch
			{
				if (GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 0)
				{
					GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = 2;
				}
				if (GregorianCalendarHelper.s_enforceJapaneseEraYearRanges == 0)
				{
					GregorianCalendarHelper.s_enforceJapaneseEraYearRanges = 2;
				}
				if (GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing == 0)
				{
					GregorianCalendarHelper.s_enforceLegacyJapaneseDateParsing = 2;
				}
			}
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x0006A048 File Offset: 0x00069048
		private int GetYearOffset(int year, int era, bool throwOnError)
		{
			if (year < 0)
			{
				if (throwOnError)
				{
					throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
				}
				return -1;
			}
			else
			{
				if (era == 0)
				{
					era = this.m_Cal.CurrentEraValue;
				}
				int i = 0;
				while (i < this.m_EraInfo.Length)
				{
					if (era == this.m_EraInfo[i].era)
					{
						if (year >= this.m_EraInfo[i].minEraYear)
						{
							if (year <= this.m_EraInfo[i].maxEraYear)
							{
								return this.m_EraInfo[i].yearOffset;
							}
							if (!GregorianCalendarHelper.EnforceJapaneseEraYearRanges)
							{
								int num = year - this.m_EraInfo[i].maxEraYear;
								for (int j = i - 1; j >= 0; j--)
								{
									if (num <= this.m_EraInfo[j].maxEraYear)
									{
										return this.m_EraInfo[i].yearOffset;
									}
									num -= this.m_EraInfo[j].maxEraYear;
								}
							}
						}
						if (throwOnError)
						{
							throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
							{
								this.m_EraInfo[i].minEraYear,
								this.m_EraInfo[i].maxEraYear
							}));
						}
						break;
					}
					else
					{
						i++;
					}
				}
				if (throwOnError)
				{
					throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
				}
				return -1;
			}
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x0006A19D File Offset: 0x0006919D
		internal int GetGregorianYear(int year, int era)
		{
			return this.GetYearOffset(year, era, true) + year;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x0006A1AA File Offset: 0x000691AA
		internal bool IsValidYear(int year, int era)
		{
			return this.GetYearOffset(year, era, false) >= 0;
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x0006A1BC File Offset: 0x000691BC
		internal virtual int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			int i = (int)(ticks / 864000000000L);
			int num = i / 146097;
			i -= num * 146097;
			int num2 = i / 36524;
			if (num2 == 4)
			{
				num2 = 3;
			}
			i -= num2 * 36524;
			int num3 = i / 1461;
			i -= num3 * 1461;
			int num4 = i / 365;
			if (num4 == 4)
			{
				num4 = 3;
			}
			if (part == 0)
			{
				return num * 400 + num2 * 100 + num3 * 4 + num4 + 1;
			}
			i -= num4 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = ((num4 == 3 && (num3 != 24 || num2 == 3)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
			int num5 = i >> 6;
			while (i >= array[num5])
			{
				num5++;
			}
			if (part == 2)
			{
				return num5;
			}
			return i - array[num5 - 1] + 1;
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x0006A2A8 File Offset: 0x000692A8
		internal static long GetAbsoluteDate(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = ((year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					int num2 = num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1;
					return (long)num2;
				}
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x0006A335 File Offset: 0x00069335
		internal static long DateToTicks(int year, int month, int day)
		{
			return GregorianCalendarHelper.GetAbsoluteDate(year, month, day) * 864000000000L;
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x0006A34C File Offset: 0x0006934C
		internal static long TimeToTicks(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 999 }));
			}
			return TimeSpan.TimeToTicks(hour, minute, second) + (long)millisecond * 10000L;
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x0006A3E0 File Offset: 0x000693E0
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < this.m_Cal.MinSupportedDateTime.Ticks || ticks > this.m_Cal.MaxSupportedDateTime.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					this.m_Cal.MinSupportedDateTime,
					this.m_Cal.MaxSupportedDateTime
				}));
			}
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x0006A468 File Offset: 0x00069468
		public DateTime AddMonths(DateTime time, int months)
		{
			this.CheckTicksRange(time.Ticks);
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { -120000, 120000 }));
			}
			int num = this.GetDatePart(time.Ticks, 0);
			int num2 = this.GetDatePart(time.Ticks, 2);
			int num3 = this.GetDatePart(time.Ticks, 3);
			int num4 = num2 - 1 + months;
			if (num4 >= 0)
			{
				num2 = num4 % 12 + 1;
				num += num4 / 12;
			}
			else
			{
				num2 = 12 + (num4 + 1) % 12;
				num += (num4 - 11) / 12;
			}
			int[] array = ((num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long num6 = GregorianCalendarHelper.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num6, this.m_Cal.MinSupportedDateTime, this.m_Cal.MaxSupportedDateTime);
			return new DateTime(num6);
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x0006A5A9 File Offset: 0x000695A9
		public DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x0006A5B6 File Offset: 0x000695B6
		public int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x0006A5C6 File Offset: 0x000695C6
		public DayOfWeek GetDayOfWeek(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return (DayOfWeek)((time.Ticks / 864000000000L + 1L) % 7L);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x0006A5ED File Offset: 0x000695ED
		public int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x0006A600 File Offset: 0x00069600
		public int GetDaysInMonth(int year, int month, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			int[] array = ((year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendarHelper.DaysToMonth366 : GregorianCalendarHelper.DaysToMonth365);
			return array[month] - array[month - 1];
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x0006A65F File Offset: 0x0006965F
		public int GetDaysInYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x0006A68C File Offset: 0x0006968C
		public int GetEra(DateTime time)
		{
			long ticks = time.Ticks;
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return this.m_EraInfo[i].era;
				}
			}
			throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_Era"));
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x0006A6E4 File Offset: 0x000696E4
		public int[] Eras
		{
			get
			{
				if (this.m_eras == null)
				{
					this.m_eras = new int[this.m_EraInfo.Length];
					for (int i = 0; i < this.m_EraInfo.Length; i++)
					{
						this.m_eras[i] = this.m_EraInfo[i].era;
					}
				}
				return (int[])this.m_eras.Clone();
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x0006A744 File Offset: 0x00069744
		public int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x0006A754 File Offset: 0x00069754
		public int GetMonthsInYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return 12;
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x0006A764 File Offset: 0x00069764
		public int GetYear(DateTime time)
		{
			long ticks = time.Ticks;
			int datePart = this.GetDatePart(ticks, 0);
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return datePart - this.m_EraInfo[i].yearOffset;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_NoEra"));
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x0006A7C4 File Offset: 0x000697C4
		public int GetYear(int year, DateTime time)
		{
			long ticks = time.Ticks;
			for (int i = 0; i < this.m_EraInfo.Length; i++)
			{
				if (ticks >= this.m_EraInfo[i].ticks)
				{
					return year - this.m_EraInfo[i].yearOffset;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_NoEra"));
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x0006A81C File Offset: 0x0006981C
		public bool IsLeapDay(int year, int month, int day, int era)
		{
			if (day < 1 || day > this.GetDaysInMonth(year, month, era))
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.GetDaysInMonth(year, month, era)
				}));
			}
			return this.IsLeapYear(year, era) && (month == 2 && day == 29);
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x0006A895 File Offset: 0x00069895
		public int GetLeapMonth(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return 0;
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x0006A8A4 File Offset: 0x000698A4
		public bool IsLeapMonth(int year, int month, int era)
		{
			year = this.GetGregorianYear(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 12 }));
			}
			return false;
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x0006A8FF File Offset: 0x000698FF
		public bool IsLeapYear(int year, int era)
		{
			year = this.GetGregorianYear(year, era);
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x0006A924 File Offset: 0x00069924
		public DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			year = this.GetGregorianYear(year, era);
			long num = GregorianCalendarHelper.DateToTicks(year, month, day) + GregorianCalendarHelper.TimeToTicks(hour, minute, second, millisecond);
			this.CheckTicksRange(num);
			return new DateTime(num);
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x0006A960 File Offset: 0x00069960
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			this.CheckTicksRange(time.Ticks);
			return GregorianCalendar.GetDefaultInstance().GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x0006A97C File Offset: 0x0006997C
		public int ToFourDigitYear(int year, int twoDigitYearMax)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (year < 100)
			{
				int num = year % 100;
				return (twoDigitYearMax / 100 - ((num > twoDigitYearMax % 100) ? 1 : 0)) * 100 + num;
			}
			if (year < this.m_minYear || year > this.m_maxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { this.m_minYear, this.m_maxYear }));
			}
			return year;
		}

		// Token: 0x040010F2 RID: 4338
		internal const long TicksPerMillisecond = 10000L;

		// Token: 0x040010F3 RID: 4339
		internal const long TicksPerSecond = 10000000L;

		// Token: 0x040010F4 RID: 4340
		internal const long TicksPerMinute = 600000000L;

		// Token: 0x040010F5 RID: 4341
		internal const long TicksPerHour = 36000000000L;

		// Token: 0x040010F6 RID: 4342
		internal const long TicksPerDay = 864000000000L;

		// Token: 0x040010F7 RID: 4343
		internal const int MillisPerSecond = 1000;

		// Token: 0x040010F8 RID: 4344
		internal const int MillisPerMinute = 60000;

		// Token: 0x040010F9 RID: 4345
		internal const int MillisPerHour = 3600000;

		// Token: 0x040010FA RID: 4346
		internal const int MillisPerDay = 86400000;

		// Token: 0x040010FB RID: 4347
		internal const int DaysPerYear = 365;

		// Token: 0x040010FC RID: 4348
		internal const int DaysPer4Years = 1461;

		// Token: 0x040010FD RID: 4349
		internal const int DaysPer100Years = 36524;

		// Token: 0x040010FE RID: 4350
		internal const int DaysPer400Years = 146097;

		// Token: 0x040010FF RID: 4351
		internal const int DaysTo10000 = 3652059;

		// Token: 0x04001100 RID: 4352
		internal const long MaxMillis = 315537897600000L;

		// Token: 0x04001101 RID: 4353
		internal const int DatePartYear = 0;

		// Token: 0x04001102 RID: 4354
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04001103 RID: 4355
		internal const int DatePartMonth = 2;

		// Token: 0x04001104 RID: 4356
		internal const int DatePartDay = 3;

		// Token: 0x04001105 RID: 4357
		internal static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334, 365
		};

		// Token: 0x04001106 RID: 4358
		internal static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335, 366
		};

		// Token: 0x04001107 RID: 4359
		internal int m_maxYear = 9999;

		// Token: 0x04001108 RID: 4360
		internal int m_minYear;

		// Token: 0x04001109 RID: 4361
		internal Calendar m_Cal;

		// Token: 0x0400110A RID: 4362
		internal EraInfo[] m_EraInfo;

		// Token: 0x0400110B RID: 4363
		internal int[] m_eras;

		// Token: 0x0400110C RID: 4364
		internal DateTime m_minDate;

		// Token: 0x0400110D RID: 4365
		private static int s_enforceJapaneseEraYearRanges = 0;

		// Token: 0x0400110E RID: 4366
		private static int s_formatJapaneseFirstYearAsANumber = 0;

		// Token: 0x0400110F RID: 4367
		private static int s_enforceLegacyJapaneseDateParsing = 0;
	}
}

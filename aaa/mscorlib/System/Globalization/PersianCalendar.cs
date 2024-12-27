using System;

namespace System.Globalization
{
	// Token: 0x020003AA RID: 938
	[Serializable]
	public class PersianCalendar : Calendar
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x0600264F RID: 9807 RVA: 0x0006EEEC File Offset: 0x0006DEEC
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return PersianCalendar.minDate;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x0006EEF3 File Offset: 0x0006DEF3
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return PersianCalendar.maxDate;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x0006EEFA File Offset: 0x0006DEFA
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x0006EF05 File Offset: 0x0006DF05
		internal override int BaseCalendarID
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x0006EF08 File Offset: 0x0006DF08
		internal override int ID
		{
			get
			{
				return 22;
			}
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x0006EF0C File Offset: 0x0006DF0C
		private long GetAbsoluteDatePersian(int year, int month, int day)
		{
			if (year >= 1 && year <= 9378 && month >= 1 && month <= 12)
			{
				return this.DaysUpToPersianYear(year) + (long)PersianCalendar.DaysToMonth[month - 1] + (long)day - 1L;
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x0006EF4C File Offset: 0x0006DF4C
		private long DaysUpToPersianYear(int PersianYear)
		{
			int num = (PersianYear - 1) / 33;
			int i = (PersianYear - 1) % 33;
			long num2 = (long)num * 12053L + 226894L;
			while (i > 0)
			{
				num2 += 365L;
				if (this.IsLeapYear(i, 0))
				{
					num2 += 1L;
				}
				i--;
			}
			return num2;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x0006EF9C File Offset: 0x0006DF9C
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < PersianCalendar.minDate.Ticks || ticks > PersianCalendar.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					PersianCalendar.minDate,
					PersianCalendar.maxDate
				}));
			}
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x0006F004 File Offset: 0x0006E004
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != PersianCalendar.PersianEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x0006F028 File Offset: 0x0006E028
		internal void CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year < 1 || year > 9378)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9378 }));
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0006F088 File Offset: 0x0006E088
		internal void CheckYearMonthRange(int year, int month, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9378 && month > 10)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 10 }));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x0006F104 File Offset: 0x0006E104
		internal int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			long num = ticks / 864000000000L + 1L;
			int num2 = (int)((num - 226894L) * 33L / 12053L) + 1;
			long num3 = this.DaysUpToPersianYear(num2);
			long num4 = (long)this.GetDaysInYear(num2, 0);
			if (num < num3)
			{
				num3 -= num4;
				num2--;
			}
			else if (num == num3)
			{
				num2--;
				num3 -= (long)this.GetDaysInYear(num2, 0);
			}
			else if (num > num3 + num4)
			{
				num3 += num4;
				num2++;
			}
			if (part == 0)
			{
				return num2;
			}
			num -= num3;
			if (part == 1)
			{
				return (int)num;
			}
			int num5 = 0;
			while (num5 < 12 && num > (long)PersianCalendar.DaysToMonth[num5])
			{
				num5++;
			}
			if (part == 2)
			{
				return num5;
			}
			int num6 = (int)(num - (long)PersianCalendar.DaysToMonth[num5 - 1]);
			if (part == 3)
			{
				return num6;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x0006F1E4 File Offset: 0x0006E1E4
		public override DateTime AddMonths(DateTime time, int months)
		{
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
			int daysInMonth = this.GetDaysInMonth(num, num2);
			if (num3 > daysInMonth)
			{
				num3 = daysInMonth;
			}
			long num5 = this.GetAbsoluteDatePersian(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num5, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(num5);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x0006F2F4 File Offset: 0x0006E2F4
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0006F301 File Offset: 0x0006E301
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0006F311 File Offset: 0x0006E311
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0006F32A File Offset: 0x0006E32A
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x0006F33A File Offset: 0x0006E33A
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			if (month == 10 && year == 9378)
			{
				return 10;
			}
			if (month == 12)
			{
				if (!this.IsLeapYear(year, 0))
				{
					return 29;
				}
				return 30;
			}
			else
			{
				if (month <= 6)
				{
					return 31;
				}
				return 30;
			}
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x0006F373 File Offset: 0x0006E373
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9378)
			{
				return PersianCalendar.DaysToMonth[9] + 10;
			}
			if (!this.IsLeapYear(year, 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0006F3A6 File Offset: 0x0006E3A6
		public override int GetEra(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return PersianCalendar.PersianEra;
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x0006F3BC File Offset: 0x0006E3BC
		public override int[] Eras
		{
			get
			{
				return new int[] { PersianCalendar.PersianEra };
			}
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0006F3D9 File Offset: 0x0006E3D9
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0006F3E9 File Offset: 0x0006E3E9
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9378)
			{
				return 10;
			}
			return 12;
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0006F400 File Offset: 0x0006E400
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0006F410 File Offset: 0x0006E410
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { daysInMonth, month }));
			}
			return this.IsLeapYear(year, era) && month == 12 && day == 30;
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0006F480 File Offset: 0x0006E480
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0006F48B File Offset: 0x0006E48B
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0006F497 File Offset: 0x0006E497
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return PersianCalendar.LeapYears33[year % 33] == 1;
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0006F4B0 File Offset: 0x0006E4B0
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { daysInMonth, month }));
			}
			long absoluteDatePersian = this.GetAbsoluteDatePersian(year, month, day);
			if (absoluteDatePersian >= 0L)
			{
				return new DateTime(absoluteDatePersian * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x0006F547 File Offset: 0x0006E547
		// (set) Token: 0x0600266E RID: 9838 RVA: 0x0006F570 File Offset: 0x0006E570
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 1410);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > 9378)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 99, 9378 }));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0006F5D8 File Offset: 0x0006E5D8
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year > 9378)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9378 }));
			}
			return year;
		}

		// Token: 0x04001157 RID: 4439
		internal const int DateCycle = 33;

		// Token: 0x04001158 RID: 4440
		internal const int DatePartYear = 0;

		// Token: 0x04001159 RID: 4441
		internal const int DatePartDayOfYear = 1;

		// Token: 0x0400115A RID: 4442
		internal const int DatePartMonth = 2;

		// Token: 0x0400115B RID: 4443
		internal const int DatePartDay = 3;

		// Token: 0x0400115C RID: 4444
		internal const int LeapYearsPerCycle = 8;

		// Token: 0x0400115D RID: 4445
		internal const long GregorianOffset = 226894L;

		// Token: 0x0400115E RID: 4446
		internal const long DaysPerCycle = 12053L;

		// Token: 0x0400115F RID: 4447
		internal const int MaxCalendarYear = 9378;

		// Token: 0x04001160 RID: 4448
		internal const int MaxCalendarMonth = 10;

		// Token: 0x04001161 RID: 4449
		internal const int MaxCalendarDay = 10;

		// Token: 0x04001162 RID: 4450
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1410;

		// Token: 0x04001163 RID: 4451
		public static readonly int PersianEra = 1;

		// Token: 0x04001164 RID: 4452
		internal static int[] DaysToMonth = new int[]
		{
			0, 31, 62, 93, 124, 155, 186, 216, 246, 276,
			306, 336
		};

		// Token: 0x04001165 RID: 4453
		internal static int[] LeapYears33 = new int[]
		{
			0, 1, 0, 0, 0, 1, 0, 0, 0, 1,
			0, 0, 0, 1, 0, 0, 0, 1, 0, 0,
			0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
			1, 0, 0
		};

		// Token: 0x04001166 RID: 4454
		internal static DateTime minDate = new DateTime(622, 3, 21);

		// Token: 0x04001167 RID: 4455
		internal static DateTime maxDate = DateTime.MaxValue;
	}
}

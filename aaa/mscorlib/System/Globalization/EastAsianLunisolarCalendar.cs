using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200037A RID: 890
	[ComVisible(true)]
	[Serializable]
	public abstract class EastAsianLunisolarCalendar : Calendar
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x00059F12 File Offset: 0x00058F12
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunisolarCalendar;
			}
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x00059F18 File Offset: 0x00058F18
		public virtual int GetSexagenaryYear(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			return (num - 4) % 60 + 1;
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x00059F50 File Offset: 0x00058F50
		public int GetCelestialStem(int sexagenaryYear)
		{
			if (sexagenaryYear < 1 || sexagenaryYear > 60)
			{
				throw new ArgumentOutOfRangeException("sexagenaryYear", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 60 }));
			}
			return (sexagenaryYear - 1) % 10 + 1;
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x00059FA8 File Offset: 0x00058FA8
		public int GetTerrestrialBranch(int sexagenaryYear)
		{
			if (sexagenaryYear < 1 || sexagenaryYear > 60)
			{
				throw new ArgumentOutOfRangeException("sexagenaryYear", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 60 }));
			}
			return (sexagenaryYear - 1) % 12 + 1;
		}

		// Token: 0x0600235C RID: 9052
		internal abstract int GetYearInfo(int LunarYear, int Index);

		// Token: 0x0600235D RID: 9053
		internal abstract int GetYear(int year, DateTime time);

		// Token: 0x0600235E RID: 9054
		internal abstract int GetGregorianYear(int year, int era);

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600235F RID: 9055
		internal abstract int MinCalendarYear { get; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002360 RID: 9056
		internal abstract int MaxCalendarYear { get; }

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002361 RID: 9057
		internal abstract EraInfo[] CalEraInfo { get; }

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002362 RID: 9058
		internal abstract DateTime MinDate { get; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002363 RID: 9059
		internal abstract DateTime MaxDate { get; }

		// Token: 0x06002364 RID: 9060 RVA: 0x0005A000 File Offset: 0x00059000
		internal int MinEraCalendarYear(int era)
		{
			EraInfo[] calEraInfo = this.CalEraInfo;
			if (calEraInfo == null)
			{
				return this.MinCalendarYear;
			}
			if (era == 0)
			{
				era = this.CurrentEraValue;
			}
			if (era == this.GetEra(this.MinDate))
			{
				return this.GetYear(this.MinCalendarYear, this.MinDate);
			}
			for (int i = 0; i < calEraInfo.Length; i++)
			{
				if (era == calEraInfo[i].era)
				{
					return calEraInfo[i].minEraYear;
				}
			}
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0005A084 File Offset: 0x00059084
		internal int MaxEraCalendarYear(int era)
		{
			EraInfo[] calEraInfo = this.CalEraInfo;
			if (calEraInfo == null)
			{
				return this.MaxCalendarYear;
			}
			if (era == 0)
			{
				era = this.CurrentEraValue;
			}
			if (era == this.GetEra(this.MaxDate))
			{
				return this.GetYear(this.MaxCalendarYear, this.MaxDate);
			}
			for (int i = 0; i < calEraInfo.Length; i++)
			{
				if (era == calEraInfo[i].era)
				{
					return calEraInfo[i].maxEraYear;
				}
			}
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0005A105 File Offset: 0x00059105
		internal EastAsianLunisolarCalendar()
		{
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0005A110 File Offset: 0x00059110
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < this.MinSupportedDateTime.Ticks || ticks > this.MaxSupportedDateTime.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[] { this.MinSupportedDateTime, this.MaxSupportedDateTime }));
			}
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0005A182 File Offset: 0x00059182
		internal void CheckEraRange(int era)
		{
			if (era == 0)
			{
				era = this.CurrentEraValue;
			}
			if (era < this.GetEra(this.MinDate) || era > this.GetEra(this.MaxDate))
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0005A1C4 File Offset: 0x000591C4
		internal int CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			year = this.GetGregorianYear(year, era);
			if (year < this.MinCalendarYear || year > this.MaxCalendarYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					this.MinEraCalendarYear(era),
					this.MaxEraCalendarYear(era)
				}));
			}
			return year;
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x0005A23C File Offset: 0x0005923C
		internal int CheckYearMonthRange(int year, int month, int era)
		{
			year = this.CheckYearRange(year, era);
			if (month == 13 && this.GetYearInfo(year, 0) == 0)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			if (month < 1 || month > 13)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			return year;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x0005A298 File Offset: 0x00059298
		internal int InternalGetDaysInMonth(int year, int month)
		{
			int num = 32768;
			num >>= month - 1;
			int num2;
			if ((this.GetYearInfo(year, 3) & num) == 0)
			{
				num2 = 29;
			}
			else
			{
				num2 = 30;
			}
			return num2;
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0005A2C9 File Offset: 0x000592C9
		public override int GetDaysInMonth(int year, int month, int era)
		{
			year = this.CheckYearMonthRange(year, month, era);
			return this.InternalGetDaysInMonth(year, month);
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0005A2DE File Offset: 0x000592DE
		internal int GergIsleap(int y)
		{
			if (y % 4 != 0)
			{
				return 0;
			}
			if (y % 100 != 0)
			{
				return 1;
			}
			if (y % 400 == 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x0005A2FC File Offset: 0x000592FC
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			year = this.CheckYearMonthRange(year, month, era);
			int num = this.InternalGetDaysInMonth(year, month);
			if (day < 1 || day > num)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { num, month }));
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (this.LunarToGregorian(year, month, day, ref num2, ref num3, ref num4))
			{
				return new DateTime(num2, num3, num4, hour, minute, second, millisecond);
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x0005A39C File Offset: 0x0005939C
		internal void GregorianToLunar(int nSYear, int nSMonth, int nSDate, ref int nLYear, ref int nLMonth, ref int nLDate)
		{
			int num = this.GergIsleap(nSYear);
			int num2 = ((num == 1) ? EastAsianLunisolarCalendar.DaysToMonth366[nSMonth - 1] : EastAsianLunisolarCalendar.DaysToMonth365[nSMonth - 1]);
			num2 += nSDate;
			int i = num2;
			nLYear = nSYear;
			int num3;
			int num4;
			if (nLYear == this.MaxCalendarYear + 1)
			{
				nLYear--;
				i += ((this.GergIsleap(nLYear) == 1) ? 366 : 365);
				num3 = this.GetYearInfo(nLYear, 1);
				num4 = this.GetYearInfo(nLYear, 2);
			}
			else
			{
				num3 = this.GetYearInfo(nLYear, 1);
				num4 = this.GetYearInfo(nLYear, 2);
				if (nSMonth < num3 || (nSMonth == num3 && nSDate < num4))
				{
					nLYear--;
					i += ((this.GergIsleap(nLYear) == 1) ? 366 : 365);
					num3 = this.GetYearInfo(nLYear, 1);
					num4 = this.GetYearInfo(nLYear, 2);
				}
			}
			i -= EastAsianLunisolarCalendar.DaysToMonth365[num3 - 1];
			i -= num4 - 1;
			int num5 = 32768;
			int yearInfo = this.GetYearInfo(nLYear, 3);
			int num6 = (((yearInfo & num5) != 0) ? 30 : 29);
			nLMonth = 1;
			while (i > num6)
			{
				i -= num6;
				nLMonth++;
				num5 >>= 1;
				num6 = (((yearInfo & num5) != 0) ? 30 : 29);
			}
			nLDate = i;
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x0005A4E4 File Offset: 0x000594E4
		internal bool LunarToGregorian(int nLYear, int nLMonth, int nLDate, ref int nSolarYear, ref int nSolarMonth, ref int nSolarDay)
		{
			if (nLDate < 1 || nLDate > 30)
			{
				return false;
			}
			int num = nLDate - 1;
			for (int i = 1; i < nLMonth; i++)
			{
				num += this.InternalGetDaysInMonth(nLYear, i);
			}
			int yearInfo = this.GetYearInfo(nLYear, 1);
			int yearInfo2 = this.GetYearInfo(nLYear, 2);
			int num2 = this.GergIsleap(nLYear);
			int[] array = ((num2 == 1) ? EastAsianLunisolarCalendar.DaysToMonth366 : EastAsianLunisolarCalendar.DaysToMonth365);
			nSolarDay = yearInfo2;
			if (yearInfo > 1)
			{
				nSolarDay += array[yearInfo - 1];
			}
			nSolarDay += num;
			if (nSolarDay > num2 + 365)
			{
				nSolarYear = nLYear + 1;
				nSolarDay -= num2 + 365;
			}
			else
			{
				nSolarYear = nLYear;
			}
			nSolarMonth = 1;
			while (nSolarMonth < 12 && array[nSolarMonth] < nSolarDay)
			{
				nSolarMonth++;
			}
			nSolarDay -= array[nSolarMonth - 1];
			return true;
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x0005A5B8 File Offset: 0x000595B8
		internal DateTime LunarToTime(DateTime time, int year, int month, int day)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.LunarToGregorian(year, month, day, ref num, ref num2, ref num3);
			return GregorianCalendar.GetDefaultInstance().ToDateTime(num, num2, num3, time.Hour, time.Minute, time.Second, time.Millisecond);
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x0005A608 File Offset: 0x00059608
		internal void TimeToLunar(DateTime time, ref int year, ref int month, ref int day)
		{
			Calendar defaultInstance = GregorianCalendar.GetDefaultInstance();
			int year2 = defaultInstance.GetYear(time);
			int month2 = defaultInstance.GetMonth(time);
			int dayOfMonth = defaultInstance.GetDayOfMonth(time);
			this.GregorianToLunar(year2, month2, dayOfMonth, ref year, ref month, ref day);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0005A648 File Offset: 0x00059648
		public override DateTime AddMonths(DateTime time, int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { -120000, 120000 }));
			}
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			int i = num2 + months;
			if (i > 0)
			{
				int num4 = (this.InternalIsLeapYear(num) ? 13 : 12);
				while (i - num4 > 0)
				{
					i -= num4;
					num++;
					num4 = (this.InternalIsLeapYear(num) ? 13 : 12);
				}
				num2 = i;
			}
			else
			{
				while (i <= 0)
				{
					int num5 = (this.InternalIsLeapYear(num - 1) ? 13 : 12);
					i += num5;
					num--;
				}
				num2 = i;
			}
			int num6 = this.InternalGetDaysInMonth(num, num2);
			if (num3 > num6)
			{
				num3 = num6;
			}
			DateTime dateTime = this.LunarToTime(time, num, num2, num3);
			Calendar.CheckAddResult(dateTime.Ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return dateTime;
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x0005A764 File Offset: 0x00059764
		public override DateTime AddYears(DateTime time, int years)
		{
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			num += years;
			if (num2 == 13 && !this.InternalIsLeapYear(num))
			{
				num2 = 12;
				num3 = this.InternalGetDaysInMonth(num, num2);
			}
			int num4 = this.InternalGetDaysInMonth(num, num2);
			if (num3 > num4)
			{
				num3 = num4;
			}
			DateTime dateTime = this.LunarToTime(time, num, num2, num3);
			Calendar.CheckAddResult(dateTime.Ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return dateTime;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x0005A7E4 File Offset: 0x000597E4
		public override int GetDayOfYear(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			for (int i = 1; i < num2; i++)
			{
				num3 += this.InternalGetDaysInMonth(num, i);
			}
			return num3;
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0005A82C File Offset: 0x0005982C
		public override int GetDayOfMonth(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			return num3;
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x0005A85C File Offset: 0x0005985C
		public override int GetDaysInYear(int year, int era)
		{
			year = this.CheckYearRange(year, era);
			int num = 0;
			int num2 = (this.InternalIsLeapYear(year) ? 13 : 12);
			while (num2 != 0)
			{
				num += this.InternalGetDaysInMonth(year, num2--);
			}
			return num;
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x0005A89C File Offset: 0x0005989C
		public override int GetMonth(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			return num2;
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0005A8CC File Offset: 0x000598CC
		public override int GetYear(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.TimeToLunar(time, ref num, ref num2, ref num3);
			return this.GetYear(num, time);
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0005A901 File Offset: 0x00059901
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0005A927 File Offset: 0x00059927
		public override int GetMonthsInYear(int year, int era)
		{
			year = this.CheckYearRange(year, era);
			if (!this.InternalIsLeapYear(year))
			{
				return 12;
			}
			return 13;
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x0005A944 File Offset: 0x00059944
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			year = this.CheckYearMonthRange(year, month, era);
			int num = this.InternalGetDaysInMonth(year, month);
			if (day < 1 || day > num)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { num, month }));
			}
			int yearInfo = this.GetYearInfo(year, 0);
			return yearInfo != 0 && month == yearInfo + 1;
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0005A9BC File Offset: 0x000599BC
		public override bool IsLeapMonth(int year, int month, int era)
		{
			year = this.CheckYearMonthRange(year, month, era);
			int yearInfo = this.GetYearInfo(year, 0);
			return yearInfo != 0 && month == yearInfo + 1;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x0005A9E8 File Offset: 0x000599E8
		public override int GetLeapMonth(int year, int era)
		{
			year = this.CheckYearRange(year, era);
			int yearInfo = this.GetYearInfo(year, 0);
			if (yearInfo > 0)
			{
				return yearInfo + 1;
			}
			return 0;
		}

		// Token: 0x0600237F RID: 9087 RVA: 0x0005AA11 File Offset: 0x00059A11
		internal bool InternalIsLeapYear(int year)
		{
			return this.GetYearInfo(year, 0) != 0;
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x0005AA21 File Offset: 0x00059A21
		public override bool IsLeapYear(int year, int era)
		{
			year = this.CheckYearRange(year, era);
			return this.InternalIsLeapYear(year);
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x0005AA34 File Offset: 0x00059A34
		// (set) Token: 0x06002382 RID: 9090 RVA: 0x0005AA68 File Offset: 0x00059A68
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.BaseCalendarID, this.GetYear(new DateTime(2029, 1, 1)));
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.MaxCalendarYear)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 99, this.MaxCalendarYear }));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x0005AACF File Offset: 0x00059ACF
		public override int ToFourDigitYear(int year)
		{
			year = base.ToFourDigitYear(year);
			this.CheckYearRange(year, 0);
			return year;
		}

		// Token: 0x04000EEA RID: 3818
		internal const int LeapMonth = 0;

		// Token: 0x04000EEB RID: 3819
		internal const int Jan1Month = 1;

		// Token: 0x04000EEC RID: 3820
		internal const int Jan1Date = 2;

		// Token: 0x04000EED RID: 3821
		internal const int nDaysPerMonth = 3;

		// Token: 0x04000EEE RID: 3822
		internal const int DatePartYear = 0;

		// Token: 0x04000EEF RID: 3823
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04000EF0 RID: 3824
		internal const int DatePartMonth = 2;

		// Token: 0x04000EF1 RID: 3825
		internal const int DatePartDay = 3;

		// Token: 0x04000EF2 RID: 3826
		internal const int MaxCalendarMonth = 13;

		// Token: 0x04000EF3 RID: 3827
		internal const int MaxCalendarDay = 30;

		// Token: 0x04000EF4 RID: 3828
		private const int DEFAULT_GREGORIAN_TWO_DIGIT_YEAR_MAX = 2029;

		// Token: 0x04000EF5 RID: 3829
		internal static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334
		};

		// Token: 0x04000EF6 RID: 3830
		internal static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335
		};
	}
}

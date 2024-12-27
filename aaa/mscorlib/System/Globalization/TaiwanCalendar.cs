using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003B1 RID: 945
	[ComVisible(true)]
	[Serializable]
	public class TaiwanCalendar : Calendar
	{
		// Token: 0x060026EC RID: 9964 RVA: 0x000752E7 File Offset: 0x000742E7
		internal static Calendar GetDefaultInstance()
		{
			if (TaiwanCalendar.m_defaultInstance == null)
			{
				TaiwanCalendar.m_defaultInstance = new TaiwanCalendar();
			}
			return TaiwanCalendar.m_defaultInstance;
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x000752FF File Offset: 0x000742FF
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return TaiwanCalendar.calendarMinValue;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x00075306 File Offset: 0x00074306
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x0007530D File Offset: 0x0007430D
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x00075310 File Offset: 0x00074310
		public TaiwanCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, TaiwanCalendar.m_EraInfo);
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x00075329 File Offset: 0x00074329
		internal override int ID
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x0007532C File Offset: 0x0007432C
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x0007533B File Offset: 0x0007433B
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x0007534A File Offset: 0x0007434A
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x0007535A File Offset: 0x0007435A
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00075369 File Offset: 0x00074369
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00075377 File Offset: 0x00074377
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00075385 File Offset: 0x00074385
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x00075393 File Offset: 0x00074393
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000753A2 File Offset: 0x000743A2
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000753B2 File Offset: 0x000743B2
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000753C0 File Offset: 0x000743C0
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000753CE File Offset: 0x000743CE
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000753DC File Offset: 0x000743DC
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000753EE File Offset: 0x000743EE
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000753FD File Offset: 0x000743FD
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x0007540C File Offset: 0x0007440C
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x0007541C File Offset: 0x0007441C
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x00075441 File Offset: 0x00074441
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002704 RID: 9988 RVA: 0x0007544E File Offset: 0x0007444E
		// (set) Token: 0x06002705 RID: 9989 RVA: 0x00075474 File Offset: 0x00074474
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 99);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.helper.MaxYear)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
					{
						99,
						this.helper.MaxYear
					}));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000754E8 File Offset: 0x000744E8
		public override int ToFourDigitYear(int year)
		{
			if (year <= 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedPosNum"));
			}
			if (year > this.helper.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.helper.MaxYear
				}));
			}
			return year;
		}

		// Token: 0x0400118E RID: 4494
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;

		// Token: 0x0400118F RID: 4495
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(4);

		// Token: 0x04001190 RID: 4496
		internal static Calendar m_defaultInstance;

		// Token: 0x04001191 RID: 4497
		internal GregorianCalendarHelper helper;

		// Token: 0x04001192 RID: 4498
		internal static readonly DateTime calendarMinValue = new DateTime(1912, 1, 1);
	}
}

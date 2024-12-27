using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003A8 RID: 936
	[ComVisible(true)]
	[Serializable]
	public class JapaneseCalendar : Calendar
	{
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x0006E530 File Offset: 0x0006D530
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return JapaneseCalendar.calendarMinValue;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002623 RID: 9763 RVA: 0x0006E537 File Offset: 0x0006D537
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x0006E53E File Offset: 0x0006D53E
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x0006E541 File Offset: 0x0006D541
		internal static Calendar GetDefaultInstance()
		{
			if (JapaneseCalendar.m_defaultInstance == null)
			{
				JapaneseCalendar.m_defaultInstance = new JapaneseCalendar();
			}
			return JapaneseCalendar.m_defaultInstance;
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x0006E559 File Offset: 0x0006D559
		public JapaneseCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, JapaneseCalendar.m_EraInfo);
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x0006E572 File Offset: 0x0006D572
		internal override int ID
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x0006E575 File Offset: 0x0006D575
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x0006E584 File Offset: 0x0006D584
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x0006E593 File Offset: 0x0006D593
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x0006E5A3 File Offset: 0x0006D5A3
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x0006E5B2 File Offset: 0x0006D5B2
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x0006E5C0 File Offset: 0x0006D5C0
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x0006E5CE File Offset: 0x0006D5CE
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x0006E5DC File Offset: 0x0006D5DC
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0006E5EB File Offset: 0x0006D5EB
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x0006E5FB File Offset: 0x0006D5FB
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x0006E609 File Offset: 0x0006D609
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x0006E617 File Offset: 0x0006D617
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x0006E625 File Offset: 0x0006D625
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x0006E637 File Offset: 0x0006D637
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x0006E646 File Offset: 0x0006D646
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x0006E655 File Offset: 0x0006D655
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x0006E668 File Offset: 0x0006D668
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x0006E690 File Offset: 0x0006D690
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

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x0006E708 File Offset: 0x0006D708
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x0006E715 File Offset: 0x0006D715
		internal override bool IsValidYear(int year, int era)
		{
			return this.helper.IsValidYear(year, era);
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x0006E724 File Offset: 0x0006D724
		// (set) Token: 0x0600263D RID: 9789 RVA: 0x0006E748 File Offset: 0x0006D748
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

		// Token: 0x04001144 RID: 4420
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;

		// Token: 0x04001145 RID: 4421
		internal static readonly DateTime calendarMinValue = new DateTime(1868, 9, 8);

		// Token: 0x04001146 RID: 4422
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(3);

		// Token: 0x04001147 RID: 4423
		internal static Calendar m_defaultInstance;

		// Token: 0x04001148 RID: 4424
		internal GregorianCalendarHelper helper;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003B7 RID: 951
	[ComVisible(true)]
	[Serializable]
	public class ThaiBuddhistCalendar : Calendar
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x00076B1B File Offset: 0x00075B1B
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600275B RID: 10075 RVA: 0x00076B22 File Offset: 0x00075B22
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x00076B29 File Offset: 0x00075B29
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x00076B39 File Offset: 0x00075B39
		public ThaiBuddhistCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, ThaiBuddhistCalendar.m_EraInfo);
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x00076B52 File Offset: 0x00075B52
		internal override int ID
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x00076B55 File Offset: 0x00075B55
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x00076B64 File Offset: 0x00075B64
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x00076B73 File Offset: 0x00075B73
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x00076B83 File Offset: 0x00075B83
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x00076B92 File Offset: 0x00075B92
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x00076BA0 File Offset: 0x00075BA0
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00076BAE File Offset: 0x00075BAE
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x00076BBC File Offset: 0x00075BBC
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x00076BCB File Offset: 0x00075BCB
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x00076BDB File Offset: 0x00075BDB
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x00076BE9 File Offset: 0x00075BE9
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x00076BF7 File Offset: 0x00075BF7
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x00076C05 File Offset: 0x00075C05
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x00076C17 File Offset: 0x00075C17
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x00076C26 File Offset: 0x00075C26
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x00076C35 File Offset: 0x00075C35
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x00076C48 File Offset: 0x00075C48
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002771 RID: 10097 RVA: 0x00076C6D File Offset: 0x00075C6D
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x00076C7A File Offset: 0x00075C7A
		// (set) Token: 0x06002773 RID: 10099 RVA: 0x00076CA4 File Offset: 0x00075CA4
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 2572);
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

		// Token: 0x06002774 RID: 10100 RVA: 0x00076D15 File Offset: 0x00075D15
		public override int ToFourDigitYear(int year)
		{
			return this.helper.ToFourDigitYear(year, this.TwoDigitYearMax);
		}

		// Token: 0x040011CA RID: 4554
		public const int ThaiBuddhistEra = 1;

		// Token: 0x040011CB RID: 4555
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 2572;

		// Token: 0x040011CC RID: 4556
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(7);

		// Token: 0x040011CD RID: 4557
		internal GregorianCalendarHelper helper;
	}
}

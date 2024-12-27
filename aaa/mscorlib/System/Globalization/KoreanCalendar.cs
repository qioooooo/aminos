using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003AC RID: 940
	[ComVisible(true)]
	[Serializable]
	public class KoreanCalendar : Calendar
	{
		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x0006FE4A File Offset: 0x0006EE4A
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06002692 RID: 9874 RVA: 0x0006FE51 File Offset: 0x0006EE51
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06002693 RID: 9875 RVA: 0x0006FE58 File Offset: 0x0006EE58
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0006FE5B File Offset: 0x0006EE5B
		public KoreanCalendar()
		{
			this.helper = new GregorianCalendarHelper(this, KoreanCalendar.m_EraInfo);
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x0006FE74 File Offset: 0x0006EE74
		internal override int ID
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0006FE77 File Offset: 0x0006EE77
		public override DateTime AddMonths(DateTime time, int months)
		{
			return this.helper.AddMonths(time, months);
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0006FE86 File Offset: 0x0006EE86
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.helper.AddYears(time, years);
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0006FE95 File Offset: 0x0006EE95
		public override int GetDaysInMonth(int year, int month, int era)
		{
			return this.helper.GetDaysInMonth(year, month, era);
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0006FEA5 File Offset: 0x0006EEA5
		public override int GetDaysInYear(int year, int era)
		{
			return this.helper.GetDaysInYear(year, era);
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0006FEB4 File Offset: 0x0006EEB4
		public override int GetDayOfMonth(DateTime time)
		{
			return this.helper.GetDayOfMonth(time);
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0006FEC2 File Offset: 0x0006EEC2
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return this.helper.GetDayOfWeek(time);
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0006FED0 File Offset: 0x0006EED0
		public override int GetDayOfYear(DateTime time)
		{
			return this.helper.GetDayOfYear(time);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0006FEDE File Offset: 0x0006EEDE
		public override int GetMonthsInYear(int year, int era)
		{
			return this.helper.GetMonthsInYear(year, era);
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0006FEED File Offset: 0x0006EEED
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			return this.helper.GetWeekOfYear(time, rule, firstDayOfWeek);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0006FEFD File Offset: 0x0006EEFD
		public override int GetEra(DateTime time)
		{
			return this.helper.GetEra(time);
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0006FF0B File Offset: 0x0006EF0B
		public override int GetMonth(DateTime time)
		{
			return this.helper.GetMonth(time);
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0006FF19 File Offset: 0x0006EF19
		public override int GetYear(DateTime time)
		{
			return this.helper.GetYear(time);
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0006FF27 File Offset: 0x0006EF27
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			return this.helper.IsLeapDay(year, month, day, era);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0006FF39 File Offset: 0x0006EF39
		public override bool IsLeapYear(int year, int era)
		{
			return this.helper.IsLeapYear(year, era);
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0006FF48 File Offset: 0x0006EF48
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			return this.helper.GetLeapMonth(year, era);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0006FF57 File Offset: 0x0006EF57
		public override bool IsLeapMonth(int year, int month, int era)
		{
			return this.helper.IsLeapMonth(year, month, era);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0006FF68 File Offset: 0x0006EF68
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			return this.helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060026A7 RID: 9895 RVA: 0x0006FF8D File Offset: 0x0006EF8D
		public override int[] Eras
		{
			get
			{
				return this.helper.Eras;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x0006FF9A File Offset: 0x0006EF9A
		// (set) Token: 0x060026A9 RID: 9897 RVA: 0x0006FFC4 File Offset: 0x0006EFC4
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 4362);
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

		// Token: 0x060026AA RID: 9898 RVA: 0x00070035 File Offset: 0x0006F035
		public override int ToFourDigitYear(int year)
		{
			return this.helper.ToFourDigitYear(year, this.TwoDigitYearMax);
		}

		// Token: 0x04001172 RID: 4466
		public const int KoreanEra = 1;

		// Token: 0x04001173 RID: 4467
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 4362;

		// Token: 0x04001174 RID: 4468
		internal static EraInfo[] m_EraInfo = GregorianCalendarHelper.InitEraInfo(5);

		// Token: 0x04001175 RID: 4469
		internal GregorianCalendarHelper helper;
	}
}

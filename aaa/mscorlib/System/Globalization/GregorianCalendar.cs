using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x020003A0 RID: 928
	[ComVisible(true)]
	[Serializable]
	public class GregorianCalendar : Calendar
	{
		// Token: 0x06002574 RID: 9588 RVA: 0x00069140 File Offset: 0x00068140
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_type < GregorianCalendarTypes.Localized || this.m_type > GregorianCalendarTypes.TransliteratedFrench)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MemberOutOfRange"), new object[] { "type", "GregorianCalendar" }));
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x00069192 File Offset: 0x00068192
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002576 RID: 9590 RVA: 0x00069199 File Offset: 0x00068199
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002577 RID: 9591 RVA: 0x000691A0 File Offset: 0x000681A0
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000691A3 File Offset: 0x000681A3
		internal static Calendar GetDefaultInstance()
		{
			if (GregorianCalendar.m_defaultInstance == null)
			{
				GregorianCalendar.m_defaultInstance = new GregorianCalendar();
			}
			return GregorianCalendar.m_defaultInstance;
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000691BB File Offset: 0x000681BB
		public GregorianCalendar()
			: this(GregorianCalendarTypes.Localized)
		{
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000691C4 File Offset: 0x000681C4
		public GregorianCalendar(GregorianCalendarTypes type)
		{
			if (type < GregorianCalendarTypes.Localized || type > GregorianCalendarTypes.TransliteratedFrench)
			{
				throw new ArgumentOutOfRangeException("type", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					GregorianCalendarTypes.Localized,
					GregorianCalendarTypes.TransliteratedFrench
				}));
			}
			this.m_type = type;
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x00069221 File Offset: 0x00068221
		// (set) Token: 0x0600257C RID: 9596 RVA: 0x0006922C File Offset: 0x0006822C
		public virtual GregorianCalendarTypes CalendarType
		{
			get
			{
				return this.m_type;
			}
			set
			{
				base.VerifyWritable();
				switch (value)
				{
				case GregorianCalendarTypes.Localized:
				case GregorianCalendarTypes.USEnglish:
					break;
				default:
					switch (value)
					{
					case GregorianCalendarTypes.MiddleEastFrench:
					case GregorianCalendarTypes.Arabic:
					case GregorianCalendarTypes.TransliteratedEnglish:
					case GregorianCalendarTypes.TransliteratedFrench:
						break;
					default:
						throw new ArgumentOutOfRangeException("m_type", Environment.GetResourceString("ArgumentOutOfRange_Enum"));
					}
					break;
				}
				this.m_type = value;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x00069288 File Offset: 0x00068288
		internal override int ID
		{
			get
			{
				return (int)this.m_type;
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00069290 File Offset: 0x00068290
		internal virtual int GetDatePart(long ticks, int part)
		{
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
			int[] array = ((num4 == 3 && (num3 != 24 || num2 == 3)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365);
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

		// Token: 0x0600257F RID: 9599 RVA: 0x00069374 File Offset: 0x00068374
		internal static long GetAbsoluteDate(int year, int month, int day)
		{
			if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
			{
				int[] array = ((year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365);
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					int num2 = num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1;
					return (long)num2;
				}
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00069401 File Offset: 0x00068401
		internal virtual long DateToTicks(int year, int month, int day)
		{
			return GregorianCalendar.GetAbsoluteDate(year, month, day) * 864000000000L;
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00069418 File Offset: 0x00068418
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
			int[] array = ((num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365);
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long num6 = this.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num6, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(num6);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00069543 File Offset: 0x00068543
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00069550 File Offset: 0x00068550
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00069560 File Offset: 0x00068560
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x0006957C File Offset: 0x0006857C
		[ComVisible(false)]
		public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			if (firstDayOfWeek < DayOfWeek.Sunday || firstDayOfWeek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException("firstDayOfWeek", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					DayOfWeek.Sunday,
					DayOfWeek.Saturday
				}));
			}
			switch (rule)
			{
			case CalendarWeekRule.FirstDay:
				return base.GetFirstDayWeekOfYear(time, (int)firstDayOfWeek);
			case CalendarWeekRule.FirstFullWeek:
				return GregorianCalendar.InternalGetWeekOfYearFullDays(this, time, (int)firstDayOfWeek, 7, 365);
			case CalendarWeekRule.FirstFourDayWeek:
				return GregorianCalendar.InternalGetWeekOfYearFullDays(this, time, (int)firstDayOfWeek, 4, 365);
			default:
				throw new ArgumentOutOfRangeException("rule", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					CalendarWeekRule.FirstDay,
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00069640 File Offset: 0x00068640
		internal static int InternalGetWeekOfYearFullDays(Calendar cal, DateTime time, int firstDayOfWeek, int fullDays, int daysOfMinYearMinusOne)
		{
			int num = cal.GetDayOfYear(time) - 1;
			int num2 = cal.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
			int num3 = (firstDayOfWeek - num2 + 14) % 7;
			if (num3 != 0 && num3 >= fullDays)
			{
				num3 -= 7;
			}
			int num4 = num - num3;
			if (num4 >= 0)
			{
				return num4 / 7 + 1;
			}
			int year = cal.GetYear(time);
			if (year <= cal.GetYear(cal.MinSupportedDateTime))
			{
				num = daysOfMinYearMinusOne;
			}
			else
			{
				num = cal.GetDaysInYear(year - 1);
			}
			num2 -= num % 7;
			num3 = (firstDayOfWeek - num2 + 14) % 7;
			if (num3 != 0 && num3 >= fullDays)
			{
				num3 -= 7;
			}
			num4 = num - num3;
			return num4 / 7 + 1;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000696D1 File Offset: 0x000686D1
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000696E4 File Offset: 0x000686E4
		public override int GetDaysInMonth(int year, int month, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			int[] array = ((year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)) ? GregorianCalendar.DaysToMonth366 : GregorianCalendar.DaysToMonth365);
			return array[month] - array[month - 1];
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000697A4 File Offset: 0x000687A4
		public override int GetDaysInYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			if (year % 4 != 0 || (year % 100 == 0 && year % 400 != 0))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00069835 File Offset: 0x00068835
		public override int GetEra(DateTime time)
		{
			return 1;
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x00069838 File Offset: 0x00068838
		public override int[] Eras
		{
			get
			{
				return new int[] { 1 };
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00069851 File Offset: 0x00068851
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00069864 File Offset: 0x00068864
		public override int GetMonthsInYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year >= 1 && year <= 9999)
			{
				return 12;
			}
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000698D8 File Offset: 0x000688D8
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000698E8 File Offset: 0x000688E8
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 12 }));
			}
			if (day < 1 || day > this.GetDaysInMonth(year, month))
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					1,
					this.GetDaysInMonth(year, month)
				}));
			}
			return this.IsLeapYear(year) && (month == 2 && day == 29);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00069A08 File Offset: 0x00068A08
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			return 0;
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00069A7C File Offset: 0x00068A7C
		public override bool IsLeapMonth(int year, int month, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 12 }));
			}
			return false;
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x00069B34 File Offset: 0x00068B34
		public override bool IsLeapYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year >= 1 && year <= 9999)
			{
				return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
			}
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x00069BBF File Offset: 0x00068BBF
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			if (era == 0 || era == 1)
			{
				return new DateTime(year, month, day, hour, minute, second, millisecond);
			}
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00069BEF File Offset: 0x00068BEF
		internal override bool TryToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era, out DateTime result)
		{
			if (era == 0 || era == 1)
			{
				return DateTime.TryCreate(year, month, day, hour, minute, second, millisecond, out result);
			}
			result = DateTime.MinValue;
			return false;
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002595 RID: 9621 RVA: 0x00069C1A File Offset: 0x00068C1A
		// (set) Token: 0x06002596 RID: 9622 RVA: 0x00069C44 File Offset: 0x00068C44
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 2029);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > 9999)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 99, 9999 }));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00069CAC File Offset: 0x00068CAC
		public override int ToFourDigitYear(int year)
		{
			if (year > 9999)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9999 }));
			}
			return base.ToFourDigitYear(year);
		}

		// Token: 0x040010DB RID: 4315
		public const int ADEra = 1;

		// Token: 0x040010DC RID: 4316
		internal const int DatePartYear = 0;

		// Token: 0x040010DD RID: 4317
		internal const int DatePartDayOfYear = 1;

		// Token: 0x040010DE RID: 4318
		internal const int DatePartMonth = 2;

		// Token: 0x040010DF RID: 4319
		internal const int DatePartDay = 3;

		// Token: 0x040010E0 RID: 4320
		internal const int MaxYear = 9999;

		// Token: 0x040010E1 RID: 4321
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 2029;

		// Token: 0x040010E2 RID: 4322
		internal GregorianCalendarTypes m_type;

		// Token: 0x040010E3 RID: 4323
		internal static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334, 365
		};

		// Token: 0x040010E4 RID: 4324
		internal static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335, 366
		};

		// Token: 0x040010E5 RID: 4325
		internal static Calendar m_defaultInstance = null;
	}
}

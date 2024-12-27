using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003AB RID: 939
	[ComVisible(true)]
	[Serializable]
	public class JulianCalendar : Calendar
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002671 RID: 9841 RVA: 0x0006F749 File Offset: 0x0006E749
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002672 RID: 9842 RVA: 0x0006F750 File Offset: 0x0006E750
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002673 RID: 9843 RVA: 0x0006F757 File Offset: 0x0006E757
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0006F75A File Offset: 0x0006E75A
		public JulianCalendar()
		{
			this.twoDigitYearMax = 2029;
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x0006F778 File Offset: 0x0006E778
		internal override int ID
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0006F77C File Offset: 0x0006E77C
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != JulianCalendar.JulianEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x0006F7A0 File Offset: 0x0006E7A0
		internal void CheckYearEraRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year <= 0 || year > this.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, this.MaxYear }));
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0006F7FF File Offset: 0x0006E7FF
		internal void CheckMonthRange(int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0006F820 File Offset: 0x0006E820
		internal void CheckDayRange(int year, int month, int day)
		{
			if (year == 1 && month == 1 && day < 3)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
			}
			int[] array = ((year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365);
			int num = array[month] - array[month - 1];
			if (day < 1 || day > num)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, num }));
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0006F8AC File Offset: 0x0006E8AC
		internal int GetDatePart(long ticks, int part)
		{
			long num = ticks + 1728000000000L;
			int i = (int)(num / 864000000000L);
			int num2 = i / 1461;
			i -= num2 * 1461;
			int num3 = i / 365;
			if (num3 == 4)
			{
				num3 = 3;
			}
			if (part == 0)
			{
				return num2 * 4 + num3 + 1;
			}
			i -= num3 * 365;
			if (part == 1)
			{
				return i + 1;
			}
			int[] array = ((num3 == 3) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365);
			int num4 = i >> 6;
			while (i >= array[num4])
			{
				num4++;
			}
			if (part == 2)
			{
				return num4;
			}
			return i - array[num4 - 1] + 1;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0006F950 File Offset: 0x0006E950
		internal long DateToTicks(int year, int month, int day)
		{
			int[] array = ((year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365);
			int num = year - 1;
			int num2 = num * 365 + num / 4 + array[month - 1] + day - 1;
			return (long)(num2 - 2) * 864000000000L;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0006F998 File Offset: 0x0006E998
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
			int[] array = ((num % 4 == 0 && (num % 100 != 0 || num % 400 == 0)) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365);
			int num5 = array[num2] - array[num2 - 1];
			if (num3 > num5)
			{
				num3 = num5;
			}
			long num6 = this.DateToTicks(num, num2, num3) + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num6, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(num6);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0006FAC3 File Offset: 0x0006EAC3
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0006FAD0 File Offset: 0x0006EAD0
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0006FAE0 File Offset: 0x0006EAE0
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0006FAF9 File Offset: 0x0006EAF9
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0006FB0C File Offset: 0x0006EB0C
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearEraRange(year, era);
			this.CheckMonthRange(month);
			int[] array = ((year % 4 == 0) ? JulianCalendar.DaysToMonth366 : JulianCalendar.DaysToMonth365);
			return array[month] - array[month - 1];
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0006FB43 File Offset: 0x0006EB43
		public override int GetDaysInYear(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 365;
			}
			return 366;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0006FB5A File Offset: 0x0006EB5A
		public override int GetEra(DateTime time)
		{
			return JulianCalendar.JulianEra;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0006FB61 File Offset: 0x0006EB61
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002685 RID: 9861 RVA: 0x0006FB74 File Offset: 0x0006EB74
		public override int[] Eras
		{
			get
			{
				return new int[] { JulianCalendar.JulianEra };
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0006FB91 File Offset: 0x0006EB91
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return 12;
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0006FB9D File Offset: 0x0006EB9D
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0006FBAD File Offset: 0x0006EBAD
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			this.CheckMonthRange(month);
			if (this.IsLeapYear(year, era))
			{
				this.CheckDayRange(year, month, day);
				return month == 2 && day == 29;
			}
			this.CheckDayRange(year, month, day);
			return false;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0006FBE0 File Offset: 0x0006EBE0
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return 0;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0006FBEB File Offset: 0x0006EBEB
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearEraRange(year, era);
			this.CheckMonthRange(month);
			return false;
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x0006FBFD File Offset: 0x0006EBFD
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearEraRange(year, era);
			return year % 4 == 0;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0006FC10 File Offset: 0x0006EC10
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			this.CheckYearEraRange(year, era);
			this.CheckMonthRange(month);
			this.CheckDayRange(year, month, day);
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 999 }));
			}
			if (hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60)
			{
				return new DateTime(this.DateToTicks(year, month, day) + new TimeSpan(0, hour, minute, second, millisecond).Ticks);
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x0600268D RID: 9869 RVA: 0x0006FCD8 File Offset: 0x0006ECD8
		// (set) Token: 0x0600268E RID: 9870 RVA: 0x0006FCE0 File Offset: 0x0006ECE0
		public override int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > this.MaxYear)
				{
					throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 99, this.MaxYear }));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x0006FD48 File Offset: 0x0006ED48
		public override int ToFourDigitYear(int year)
		{
			if (year > this.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { 1, this.MaxYear }));
			}
			return base.ToFourDigitYear(year);
		}

		// Token: 0x04001168 RID: 4456
		private const int DatePartYear = 0;

		// Token: 0x04001169 RID: 4457
		private const int DatePartDayOfYear = 1;

		// Token: 0x0400116A RID: 4458
		private const int DatePartMonth = 2;

		// Token: 0x0400116B RID: 4459
		private const int DatePartDay = 3;

		// Token: 0x0400116C RID: 4460
		private const int JulianDaysPerYear = 365;

		// Token: 0x0400116D RID: 4461
		private const int JulianDaysPer4Years = 1461;

		// Token: 0x0400116E RID: 4462
		public static readonly int JulianEra = 1;

		// Token: 0x0400116F RID: 4463
		private static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334, 365
		};

		// Token: 0x04001170 RID: 4464
		private static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335, 366
		};

		// Token: 0x04001171 RID: 4465
		internal int MaxYear = 9999;
	}
}

using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x020003A6 RID: 934
	[ComVisible(true)]
	[Serializable]
	public class HijriCalendar : Calendar
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060025E4 RID: 9700 RVA: 0x0006CBE8 File Offset: 0x0006BBE8
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return HijriCalendar.calendarMinValue;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x0006CBEF File Offset: 0x0006BBEF
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return HijriCalendar.calendarMaxValue;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060025E6 RID: 9702 RVA: 0x0006CBF6 File Offset: 0x0006BBF6
		[ComVisible(false)]
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunarCalendar;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060025E8 RID: 9704 RVA: 0x0006CC0C File Offset: 0x0006BC0C
		internal override int ID
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x0006CC0F File Offset: 0x0006BC0F
		private long GetAbsoluteDateHijri(int y, int m, int d)
		{
			return this.DaysUpToHijriYear(y) + (long)HijriCalendar.HijriMonthDays[m - 1] + (long)d - 1L - (long)this.HijriAdjustment;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0006CC34 File Offset: 0x0006BC34
		private long DaysUpToHijriYear(int HijriYear)
		{
			int num = (HijriYear - 1) / 30 * 30;
			int i = HijriYear - num - 1;
			long num2 = (long)num * 10631L / 30L + 227013L;
			while (i > 0)
			{
				num2 += (long)(354 + (this.IsLeapYear(i, 0) ? 1 : 0));
				i--;
			}
			return num2;
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060025EB RID: 9707 RVA: 0x0006CC89 File Offset: 0x0006BC89
		// (set) Token: 0x060025EC RID: 9708 RVA: 0x0006CCAC File Offset: 0x0006BCAC
		public int HijriAdjustment
		{
			get
			{
				if (this.m_HijriAdvance == -2147483648)
				{
					this.m_HijriAdvance = this.GetAdvanceHijriDate();
				}
				return this.m_HijriAdvance;
			}
			set
			{
				base.VerifyWritable();
				if (value < -2 || value > 2)
				{
					throw new ArgumentOutOfRangeException("HijriAdjustment", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { -2, 2 }));
				}
				this.m_HijriAdvance = value;
			}
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0006CD0C File Offset: 0x0006BD0C
		private int GetAdvanceHijriDate()
		{
			int num = 0;
			RegistryKey registryKey = null;
			try
			{
				registryKey = Registry.CurrentUser.InternalOpenSubKey(HijriCalendar.m_InternationalRegKey, false);
			}
			catch (ObjectDisposedException)
			{
				return 0;
			}
			catch (ArgumentException)
			{
				return 0;
			}
			if (registryKey != null)
			{
				try
				{
					object obj = registryKey.InternalGetValue(HijriCalendar.m_HijriAdvanceRegKeyEntry, null, false, false);
					if (obj == null)
					{
						return 0;
					}
					string text = obj.ToString();
					if (string.Compare(text, 0, HijriCalendar.m_HijriAdvanceRegKeyEntry, 0, HijriCalendar.m_HijriAdvanceRegKeyEntry.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						if (text.Length == HijriCalendar.m_HijriAdvanceRegKeyEntry.Length)
						{
							num = -1;
						}
						else
						{
							text = text.Substring(HijriCalendar.m_HijriAdvanceRegKeyEntry.Length);
							try
							{
								int num2 = int.Parse(text.ToString(), CultureInfo.InvariantCulture);
								if (num2 >= -2 && num2 <= 2)
								{
									num = num2;
								}
							}
							catch (ArgumentException)
							{
							}
							catch (FormatException)
							{
							}
							catch (OverflowException)
							{
							}
						}
					}
				}
				finally
				{
					registryKey.Close();
				}
			}
			return num;
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0006CE2C File Offset: 0x0006BE2C
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < HijriCalendar.calendarMinValue.Ticks || ticks > HijriCalendar.calendarMaxValue.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					HijriCalendar.calendarMinValue,
					HijriCalendar.calendarMaxValue
				}));
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x0006CE9A File Offset: 0x0006BE9A
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != HijriCalendar.HijriEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x0006CEBC File Offset: 0x0006BEBC
		internal void CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year < 1 || year > 9666)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9666 }));
			}
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x0006CF1C File Offset: 0x0006BF1C
		internal void CheckYearMonthRange(int year, int month, int era)
		{
			this.CheckYearRange(year, era);
			if (year == 9666 && month > 4)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 4 }));
			}
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x0006CF94 File Offset: 0x0006BF94
		internal virtual int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			long num = ticks / 864000000000L + 1L;
			num += (long)this.HijriAdjustment;
			int num2 = (int)((num - 227013L) * 30L / 10631L) + 1;
			long num3 = this.DaysUpToHijriYear(num2);
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
			int num5 = 1;
			num -= num3;
			if (part == 1)
			{
				return (int)num;
			}
			while (num5 <= 12 && num > (long)HijriCalendar.HijriMonthDays[num5 - 1])
			{
				num5++;
			}
			num5--;
			if (part == 2)
			{
				return num5;
			}
			int num6 = (int)(num - (long)HijriCalendar.HijriMonthDays[num5 - 1]);
			if (part == 3)
			{
				return num6;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0006D080 File Offset: 0x0006C080
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
			long num5 = this.GetAbsoluteDateHijri(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num5, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(num5);
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0006D190 File Offset: 0x0006C190
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0006D19D File Offset: 0x0006C19D
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0006D1AD File Offset: 0x0006C1AD
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0006D1C6 File Offset: 0x0006C1C6
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 1);
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x0006D1D6 File Offset: 0x0006C1D6
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
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
				if (month % 2 != 1)
				{
					return 29;
				}
				return 30;
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0006D201 File Offset: 0x0006C201
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			if (!this.IsLeapYear(year, 0))
			{
				return 354;
			}
			return 355;
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0006D220 File Offset: 0x0006C220
		public override int GetEra(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return HijriCalendar.HijriEra;
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x0006D234 File Offset: 0x0006C234
		public override int[] Eras
		{
			get
			{
				return new int[] { HijriCalendar.HijriEra };
			}
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x0006D251 File Offset: 0x0006C251
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0006D261 File Offset: 0x0006C261
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 12;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0006D26D File Offset: 0x0006C26D
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x0006D280 File Offset: 0x0006C280
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { daysInMonth, month }));
			}
			return this.IsLeapYear(year, era) && month == 12 && day == 30;
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x0006D2F0 File Offset: 0x0006C2F0
		[ComVisible(false)]
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0006D2FB File Offset: 0x0006C2FB
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x0006D307 File Offset: 0x0006C307
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return (year * 11 + 14) % 30 < 11;
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x0006D320 File Offset: 0x0006C320
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { daysInMonth, month }));
			}
			long absoluteDateHijri = this.GetAbsoluteDateHijri(year, month, day);
			if (absoluteDateHijri >= 0L)
			{
				return new DateTime(absoluteDateHijri * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x0006D3B7 File Offset: 0x0006C3B7
		// (set) Token: 0x06002605 RID: 9733 RVA: 0x0006D3E0 File Offset: 0x0006C3E0
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 1451);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value < 99 || value > 9666)
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 99, 9666 }));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0006D448 File Offset: 0x0006C448
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year > 9666)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, 9666 }));
			}
			return year;
		}

		// Token: 0x04001124 RID: 4388
		internal const int DatePartYear = 0;

		// Token: 0x04001125 RID: 4389
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04001126 RID: 4390
		internal const int DatePartMonth = 2;

		// Token: 0x04001127 RID: 4391
		internal const int DatePartDay = 3;

		// Token: 0x04001128 RID: 4392
		internal const int MinAdvancedHijri = -2;

		// Token: 0x04001129 RID: 4393
		internal const int MaxAdvancedHijri = 2;

		// Token: 0x0400112A RID: 4394
		internal const int MaxCalendarYear = 9666;

		// Token: 0x0400112B RID: 4395
		internal const int MaxCalendarMonth = 4;

		// Token: 0x0400112C RID: 4396
		internal const int MaxCalendarDay = 3;

		// Token: 0x0400112D RID: 4397
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1451;

		// Token: 0x0400112E RID: 4398
		public static readonly int HijriEra = 1;

		// Token: 0x0400112F RID: 4399
		internal static readonly int[] HijriMonthDays = new int[]
		{
			0, 30, 59, 89, 118, 148, 177, 207, 236, 266,
			295, 325, 355
		};

		// Token: 0x04001130 RID: 4400
		private static string m_InternationalRegKey = "Control Panel\\International";

		// Token: 0x04001131 RID: 4401
		private static string m_HijriAdvanceRegKeyEntry = "AddHijriDate";

		// Token: 0x04001132 RID: 4402
		private int m_HijriAdvance = int.MinValue;

		// Token: 0x04001133 RID: 4403
		internal static readonly DateTime calendarMinValue = new DateTime(622, 7, 18);

		// Token: 0x04001134 RID: 4404
		internal static readonly DateTime calendarMaxValue = DateTime.MaxValue;
	}
}

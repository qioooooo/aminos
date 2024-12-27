using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000371 RID: 881
	[ComVisible(true)]
	[Serializable]
	public abstract class Calendar : ICloneable
	{
		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x00058A4A File Offset: 0x00057A4A
		[ComVisible(false)]
		public virtual DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x00058A51 File Offset: 0x00057A51
		[ComVisible(false)]
		public virtual DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x00058A6E File Offset: 0x00057A6E
		internal virtual int ID
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x00058A71 File Offset: 0x00057A71
		internal virtual int BaseCalendarID
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x00058A79 File Offset: 0x00057A79
		[ComVisible(false)]
		public virtual CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.Unknown;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x00058A7C File Offset: 0x00057A7C
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x00058A84 File Offset: 0x00057A84
		[ComVisible(false)]
		public virtual object Clone()
		{
			object obj = base.MemberwiseClone();
			((Calendar)obj).SetReadOnlyState(false);
			return obj;
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00058AA8 File Offset: 0x00057AA8
		[ComVisible(false)]
		public static Calendar ReadOnly(Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			if (calendar.IsReadOnly)
			{
				return calendar;
			}
			Calendar calendar2 = (Calendar)calendar.MemberwiseClone();
			calendar2.SetReadOnlyState(true);
			return calendar2;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00058AE1 File Offset: 0x00057AE1
		internal void VerifyWritable()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x00058AFB File Offset: 0x00057AFB
		internal void SetReadOnlyState(bool readOnly)
		{
			this.m_isReadOnly = readOnly;
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x00058B04 File Offset: 0x00057B04
		internal virtual int CurrentEraValue
		{
			get
			{
				if (this.m_currentEraValue == -1)
				{
					this.m_currentEraValue = CalendarTable.Default.ICURRENTERA(this.BaseCalendarID);
				}
				return this.m_currentEraValue;
			}
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x00058B2C File Offset: 0x00057B2C
		internal static void CheckAddResult(long ticks, DateTime minValue, DateTime maxValue)
		{
			if (ticks < minValue.Ticks || ticks > maxValue.Ticks)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Argument_ResultCalendarRange"), new object[] { minValue, maxValue }));
			}
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x00058B84 File Offset: 0x00057B84
		internal DateTime Add(DateTime time, double value, int scale)
		{
			long num = (long)(value * (double)scale + ((value >= 0.0) ? 0.5 : (-0.5)));
			if (num <= -315537897600000L || num >= 315537897600000L)
			{
				throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_AddValue"));
			}
			long num2 = time.Ticks + num * 10000L;
			Calendar.CheckAddResult(num2, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(num2);
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x00058C0E File Offset: 0x00057C0E
		public virtual DateTime AddMilliseconds(DateTime time, double milliseconds)
		{
			return this.Add(time, milliseconds, 1);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x00058C19 File Offset: 0x00057C19
		public virtual DateTime AddDays(DateTime time, int days)
		{
			return this.Add(time, (double)days, 86400000);
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x00058C29 File Offset: 0x00057C29
		public virtual DateTime AddHours(DateTime time, int hours)
		{
			return this.Add(time, (double)hours, 3600000);
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x00058C39 File Offset: 0x00057C39
		public virtual DateTime AddMinutes(DateTime time, int minutes)
		{
			return this.Add(time, (double)minutes, 60000);
		}

		// Token: 0x060022DF RID: 8927
		public abstract DateTime AddMonths(DateTime time, int months);

		// Token: 0x060022E0 RID: 8928 RVA: 0x00058C49 File Offset: 0x00057C49
		public virtual DateTime AddSeconds(DateTime time, int seconds)
		{
			return this.Add(time, (double)seconds, 1000);
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00058C59 File Offset: 0x00057C59
		public virtual DateTime AddWeeks(DateTime time, int weeks)
		{
			return this.AddDays(time, weeks * 7);
		}

		// Token: 0x060022E2 RID: 8930
		public abstract DateTime AddYears(DateTime time, int years);

		// Token: 0x060022E3 RID: 8931
		public abstract int GetDayOfMonth(DateTime time);

		// Token: 0x060022E4 RID: 8932
		public abstract DayOfWeek GetDayOfWeek(DateTime time);

		// Token: 0x060022E5 RID: 8933
		public abstract int GetDayOfYear(DateTime time);

		// Token: 0x060022E6 RID: 8934 RVA: 0x00058C65 File Offset: 0x00057C65
		public virtual int GetDaysInMonth(int year, int month)
		{
			return this.GetDaysInMonth(year, month, 0);
		}

		// Token: 0x060022E7 RID: 8935
		public abstract int GetDaysInMonth(int year, int month, int era);

		// Token: 0x060022E8 RID: 8936 RVA: 0x00058C70 File Offset: 0x00057C70
		public virtual int GetDaysInYear(int year)
		{
			return this.GetDaysInYear(year, 0);
		}

		// Token: 0x060022E9 RID: 8937
		public abstract int GetDaysInYear(int year, int era);

		// Token: 0x060022EA RID: 8938
		public abstract int GetEra(DateTime time);

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060022EB RID: 8939
		public abstract int[] Eras { get; }

		// Token: 0x060022EC RID: 8940 RVA: 0x00058C7A File Offset: 0x00057C7A
		public virtual int GetHour(DateTime time)
		{
			return (int)(time.Ticks / 36000000000L % 24L);
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x00058C92 File Offset: 0x00057C92
		public virtual double GetMilliseconds(DateTime time)
		{
			return (double)(time.Ticks / 10000L % 1000L);
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x00058CAA File Offset: 0x00057CAA
		public virtual int GetMinute(DateTime time)
		{
			return (int)(time.Ticks / 600000000L % 60L);
		}

		// Token: 0x060022EF RID: 8943
		public abstract int GetMonth(DateTime time);

		// Token: 0x060022F0 RID: 8944 RVA: 0x00058CBF File Offset: 0x00057CBF
		public virtual int GetMonthsInYear(int year)
		{
			return this.GetMonthsInYear(year, 0);
		}

		// Token: 0x060022F1 RID: 8945
		public abstract int GetMonthsInYear(int year, int era);

		// Token: 0x060022F2 RID: 8946 RVA: 0x00058CC9 File Offset: 0x00057CC9
		public virtual int GetSecond(DateTime time)
		{
			return (int)(time.Ticks / 10000000L % 60L);
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x00058CE0 File Offset: 0x00057CE0
		internal int GetFirstDayWeekOfYear(DateTime time, int firstDayOfWeek)
		{
			int num = this.GetDayOfYear(time) - 1;
			int num2 = this.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
			int num3 = (num2 - firstDayOfWeek + 14) % 7;
			return (num + num3) / 7 + 1;
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x00058D14 File Offset: 0x00057D14
		internal int GetWeekOfYearFullDays(DateTime time, CalendarWeekRule rule, int firstDayOfWeek, int fullDays)
		{
			int num = this.GetDayOfYear(time) - 1;
			int num2 = this.GetDayOfWeek(time) - (DayOfWeek)(num % 7);
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
			return this.GetWeekOfYearFullDays(time.AddDays((double)(-(double)(num + 1))), rule, firstDayOfWeek, fullDays);
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x00058D70 File Offset: 0x00057D70
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
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
				return this.GetFirstDayWeekOfYear(time, (int)firstDayOfWeek);
			case CalendarWeekRule.FirstFullWeek:
				return this.GetWeekOfYearFullDays(time, rule, (int)firstDayOfWeek, 7);
			case CalendarWeekRule.FirstFourDayWeek:
				return this.GetWeekOfYearFullDays(time, rule, (int)firstDayOfWeek, 4);
			default:
				throw new ArgumentOutOfRangeException("rule", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[]
				{
					CalendarWeekRule.FirstDay,
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x060022F6 RID: 8950
		public abstract int GetYear(DateTime time);

		// Token: 0x060022F7 RID: 8951 RVA: 0x00058E2B File Offset: 0x00057E2B
		public virtual bool IsLeapDay(int year, int month, int day)
		{
			return this.IsLeapDay(year, month, day, 0);
		}

		// Token: 0x060022F8 RID: 8952
		public abstract bool IsLeapDay(int year, int month, int day, int era);

		// Token: 0x060022F9 RID: 8953 RVA: 0x00058E37 File Offset: 0x00057E37
		public virtual bool IsLeapMonth(int year, int month)
		{
			return this.IsLeapMonth(year, month, 0);
		}

		// Token: 0x060022FA RID: 8954
		public abstract bool IsLeapMonth(int year, int month, int era);

		// Token: 0x060022FB RID: 8955 RVA: 0x00058E42 File Offset: 0x00057E42
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year)
		{
			return this.GetLeapMonth(year, 0);
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x00058E4C File Offset: 0x00057E4C
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 0;
			}
			int monthsInYear = this.GetMonthsInYear(year, era);
			for (int i = 1; i <= monthsInYear; i++)
			{
				if (this.IsLeapMonth(year, i, era))
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x00058E88 File Offset: 0x00057E88
		public virtual bool IsLeapYear(int year)
		{
			return this.IsLeapYear(year, 0);
		}

		// Token: 0x060022FE RID: 8958
		public abstract bool IsLeapYear(int year, int era);

		// Token: 0x060022FF RID: 8959 RVA: 0x00058E94 File Offset: 0x00057E94
		public virtual DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			return this.ToDateTime(year, month, day, hour, minute, second, millisecond, 0);
		}

		// Token: 0x06002300 RID: 8960
		public abstract DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);

		// Token: 0x06002301 RID: 8961 RVA: 0x00058EB4 File Offset: 0x00057EB4
		internal virtual bool TryToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era, out DateTime result)
		{
			result = DateTime.MinValue;
			bool flag;
			try
			{
				result = this.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
				flag = true;
			}
			catch (ArgumentException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x00058F04 File Offset: 0x00057F04
		internal virtual bool IsValidYear(int year, int era)
		{
			return year >= this.GetYear(this.MinSupportedDateTime) && year <= this.GetYear(this.MaxSupportedDateTime);
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x00058F29 File Offset: 0x00057F29
		internal virtual bool IsValidMonth(int year, int month, int era)
		{
			return this.IsValidYear(year, era) && month >= 1 && month <= this.GetMonthsInYear(year, era);
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x00058F49 File Offset: 0x00057F49
		internal virtual bool IsValidDay(int year, int month, int day, int era)
		{
			return this.IsValidMonth(year, month, era) && day >= 1 && day <= this.GetDaysInMonth(year, month, era);
		}

		// Token: 0x06002305 RID: 8965
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int nativeGetTwoDigitYearMax(int calID);

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x00058F6D File Offset: 0x00057F6D
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x00058F75 File Offset: 0x00057F75
		public virtual int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				this.VerifyWritable();
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x00058F84 File Offset: 0x00057F84
		public virtual int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (year < 100)
			{
				return (this.TwoDigitYearMax / 100 - ((year > this.TwoDigitYearMax % 100) ? 1 : 0)) * 100 + year;
			}
			return year;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x00058FD0 File Offset: 0x00057FD0
		internal static long TimeToTicks(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour >= 24 || minute < 0 || minute >= 60 || second < 0 || second >= 60)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
			if (millisecond < 0 || millisecond >= 1000)
			{
				throw new ArgumentOutOfRangeException("millisecond", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 0, 999 }));
			}
			return TimeSpan.TimeToTicks(hour, minute, second) + (long)millisecond * 10000L;
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x00059064 File Offset: 0x00058064
		internal static int GetSystemTwoDigitYearSetting(int CalID, int defaultYearValue)
		{
			int num = Calendar.nativeGetTwoDigitYearMax(CalID);
			if (num < 0)
			{
				RegistryKey registryKey = null;
				try
				{
					registryKey = Registry.CurrentUser.InternalOpenSubKey("Control Panel\\International\\Calendars\\TwoDigitYearMax", false);
				}
				catch (ObjectDisposedException)
				{
				}
				catch (ArgumentException)
				{
				}
				if (registryKey != null)
				{
					try
					{
						object obj = registryKey.InternalGetValue(CalID.ToString(CultureInfo.InvariantCulture), null, false, false);
						if (obj != null)
						{
							try
							{
								num = int.Parse(obj.ToString(), CultureInfo.InvariantCulture);
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
					finally
					{
						registryKey.Close();
					}
				}
				if (num < 0)
				{
					num = defaultYearValue;
				}
			}
			return num;
		}

		// Token: 0x04000E8F RID: 3727
		internal const long TicksPerMillisecond = 10000L;

		// Token: 0x04000E90 RID: 3728
		internal const long TicksPerSecond = 10000000L;

		// Token: 0x04000E91 RID: 3729
		internal const long TicksPerMinute = 600000000L;

		// Token: 0x04000E92 RID: 3730
		internal const long TicksPerHour = 36000000000L;

		// Token: 0x04000E93 RID: 3731
		internal const long TicksPerDay = 864000000000L;

		// Token: 0x04000E94 RID: 3732
		internal const int MillisPerSecond = 1000;

		// Token: 0x04000E95 RID: 3733
		internal const int MillisPerMinute = 60000;

		// Token: 0x04000E96 RID: 3734
		internal const int MillisPerHour = 3600000;

		// Token: 0x04000E97 RID: 3735
		internal const int MillisPerDay = 86400000;

		// Token: 0x04000E98 RID: 3736
		internal const int DaysPerYear = 365;

		// Token: 0x04000E99 RID: 3737
		internal const int DaysPer4Years = 1461;

		// Token: 0x04000E9A RID: 3738
		internal const int DaysPer100Years = 36524;

		// Token: 0x04000E9B RID: 3739
		internal const int DaysPer400Years = 146097;

		// Token: 0x04000E9C RID: 3740
		internal const int DaysTo10000 = 3652059;

		// Token: 0x04000E9D RID: 3741
		internal const long MaxMillis = 315537897600000L;

		// Token: 0x04000E9E RID: 3742
		internal const int CAL_GREGORIAN = 1;

		// Token: 0x04000E9F RID: 3743
		internal const int CAL_GREGORIAN_US = 2;

		// Token: 0x04000EA0 RID: 3744
		internal const int CAL_JAPAN = 3;

		// Token: 0x04000EA1 RID: 3745
		internal const int CAL_TAIWAN = 4;

		// Token: 0x04000EA2 RID: 3746
		internal const int CAL_KOREA = 5;

		// Token: 0x04000EA3 RID: 3747
		internal const int CAL_HIJRI = 6;

		// Token: 0x04000EA4 RID: 3748
		internal const int CAL_THAI = 7;

		// Token: 0x04000EA5 RID: 3749
		internal const int CAL_HEBREW = 8;

		// Token: 0x04000EA6 RID: 3750
		internal const int CAL_GREGORIAN_ME_FRENCH = 9;

		// Token: 0x04000EA7 RID: 3751
		internal const int CAL_GREGORIAN_ARABIC = 10;

		// Token: 0x04000EA8 RID: 3752
		internal const int CAL_GREGORIAN_XLIT_ENGLISH = 11;

		// Token: 0x04000EA9 RID: 3753
		internal const int CAL_GREGORIAN_XLIT_FRENCH = 12;

		// Token: 0x04000EAA RID: 3754
		internal const int CAL_JULIAN = 13;

		// Token: 0x04000EAB RID: 3755
		internal const int CAL_JAPANESELUNISOLAR = 14;

		// Token: 0x04000EAC RID: 3756
		internal const int CAL_CHINESELUNISOLAR = 15;

		// Token: 0x04000EAD RID: 3757
		internal const int CAL_SAKA = 16;

		// Token: 0x04000EAE RID: 3758
		internal const int CAL_LUNAR_ETO_CHN = 17;

		// Token: 0x04000EAF RID: 3759
		internal const int CAL_LUNAR_ETO_KOR = 18;

		// Token: 0x04000EB0 RID: 3760
		internal const int CAL_LUNAR_ETO_ROKUYOU = 19;

		// Token: 0x04000EB1 RID: 3761
		internal const int CAL_KOREANLUNISOLAR = 20;

		// Token: 0x04000EB2 RID: 3762
		internal const int CAL_TAIWANLUNISOLAR = 21;

		// Token: 0x04000EB3 RID: 3763
		internal const int CAL_PERSIAN = 22;

		// Token: 0x04000EB4 RID: 3764
		internal const int CAL_UMALQURA = 23;

		// Token: 0x04000EB5 RID: 3765
		public const int CurrentEra = 0;

		// Token: 0x04000EB6 RID: 3766
		private const string TwoDigitYearMaxSubKey = "Control Panel\\International\\Calendars\\TwoDigitYearMax";

		// Token: 0x04000EB7 RID: 3767
		internal int m_currentEraValue = -1;

		// Token: 0x04000EB8 RID: 3768
		[OptionalField(VersionAdded = 2)]
		private bool m_isReadOnly;

		// Token: 0x04000EB9 RID: 3769
		internal int twoDigitYearMax = -1;
	}
}

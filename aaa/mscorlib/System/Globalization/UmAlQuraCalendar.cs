using System;

namespace System.Globalization
{
	// Token: 0x020003BA RID: 954
	[Serializable]
	public class UmAlQuraCalendar : Calendar
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060027BF RID: 10175 RVA: 0x00077A82 File Offset: 0x00076A82
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return UmAlQuraCalendar.minDate;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x00077A89 File Offset: 0x00076A89
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return UmAlQuraCalendar.maxDate;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x00077A90 File Offset: 0x00076A90
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunarCalendar;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060027C3 RID: 10179 RVA: 0x00077A9B File Offset: 0x00076A9B
		internal override int BaseCalendarID
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060027C4 RID: 10180 RVA: 0x00077A9E File Offset: 0x00076A9E
		internal override int ID
		{
			get
			{
				return 23;
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x00077AA4 File Offset: 0x00076AA4
		private void ConvertHijriToGregorian(int HijriYear, int HijriMonth, int HijriDay, ref int yg, ref int mg, ref int dg)
		{
			int num = HijriDay - 1;
			int num2 = HijriYear - 1318;
			DateTime dateTime = UmAlQuraCalendar.HijriYearInfo[num2].GregorianDate;
			int num3 = UmAlQuraCalendar.HijriYearInfo[num2].HijriMonthsLengthFlags;
			for (int i = 1; i < HijriMonth; i++)
			{
				num += 29 + (num3 & 1);
				num3 >>= 1;
			}
			dateTime = dateTime.AddDays((double)num);
			yg = dateTime.Year;
			mg = dateTime.Month;
			dg = dateTime.Day;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x00077B24 File Offset: 0x00076B24
		private long GetAbsoluteDateUmAlQura(int year, int month, int day)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			this.ConvertHijriToGregorian(year, month, day, ref num, ref num2, ref num3);
			return GregorianCalendar.GetAbsoluteDate(num, num2, num3);
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00077B50 File Offset: 0x00076B50
		internal void CheckTicksRange(long ticks)
		{
			if (ticks < UmAlQuraCalendar.minDate.Ticks || ticks > UmAlQuraCalendar.maxDate.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					UmAlQuraCalendar.minDate,
					UmAlQuraCalendar.maxDate
				}));
			}
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00077BB8 File Offset: 0x00076BB8
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00077BD8 File Offset: 0x00076BD8
		internal void CheckYearRange(int year, int era)
		{
			this.CheckEraRange(era);
			if (year < 1318 || year > 1450)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1318, 1450 }));
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00077C3D File Offset: 0x00076C3D
		internal void CheckYearMonthRange(int year, int month, int era)
		{
			this.CheckYearRange(year, era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00077C68 File Offset: 0x00076C68
		private void ConvertGregorianToHijri(DateTime time, ref int HijriYear, ref int HijriMonth, ref int HijriDay)
		{
			int num = (int)((time.Ticks - UmAlQuraCalendar.minDate.Ticks) / 864000000000L) / 355;
			while (time.CompareTo(UmAlQuraCalendar.HijriYearInfo[++num].GregorianDate) > 0)
			{
			}
			if (time.CompareTo(UmAlQuraCalendar.HijriYearInfo[num].GregorianDate) != 0)
			{
				num--;
			}
			TimeSpan timeSpan = time.Subtract(UmAlQuraCalendar.HijriYearInfo[num].GregorianDate);
			int num2 = num + 1318;
			int num3 = 1;
			int num4 = 1;
			double num5 = timeSpan.TotalDays;
			int num6 = UmAlQuraCalendar.HijriYearInfo[num].HijriMonthsLengthFlags;
			int num7 = 29 + (num6 & 1);
			while (num5 >= (double)num7)
			{
				num5 -= (double)num7;
				num6 >>= 1;
				num7 = 29 + (num6 & 1);
				num3++;
			}
			num4 += (int)num5;
			HijriDay = num4;
			HijriMonth = num3;
			HijriYear = num2;
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00077D58 File Offset: 0x00076D58
		internal virtual int GetDatePart(DateTime time, int part)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			long ticks = time.Ticks;
			this.CheckTicksRange(ticks);
			this.ConvertGregorianToHijri(time, ref num, ref num2, ref num3);
			if (part == 0)
			{
				return num;
			}
			if (part == 2)
			{
				return num2;
			}
			if (part == 3)
			{
				return num3;
			}
			if (part == 1)
			{
				return (int)(this.GetAbsoluteDateUmAlQura(num, num2, num3) - this.GetAbsoluteDateUmAlQura(num, 1, 1) + 1L);
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x00077DC4 File Offset: 0x00076DC4
		public override DateTime AddMonths(DateTime time, int months)
		{
			if (months < -120000 || months > 120000)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { -120000, 120000 }));
			}
			int num = this.GetDatePart(time, 0);
			int num2 = this.GetDatePart(time, 2);
			int num3 = this.GetDatePart(time, 3);
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
			if (num3 > 29)
			{
				int daysInMonth = this.GetDaysInMonth(num, num2);
				if (num3 > daysInMonth)
				{
					num3 = daysInMonth;
				}
			}
			this.CheckYearRange(num, 1);
			DateTime dateTime = new DateTime(this.GetAbsoluteDateUmAlQura(num, num2, num3) * 864000000000L + time.Ticks % 864000000000L);
			Calendar.CheckAddResult(dateTime.Ticks, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return dateTime;
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x00077ED4 File Offset: 0x00076ED4
		public override DateTime AddYears(DateTime time, int years)
		{
			return this.AddMonths(time, years * 12);
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x00077EE1 File Offset: 0x00076EE1
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time, 3);
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x00077EEB File Offset: 0x00076EEB
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x00077F04 File Offset: 0x00076F04
		public override int GetDayOfYear(DateTime time)
		{
			return this.GetDatePart(time, 1);
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x00077F0E File Offset: 0x00076F0E
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			if ((UmAlQuraCalendar.HijriYearInfo[year - 1318].HijriMonthsLengthFlags & (1 << month - 1)) == 0)
			{
				return 29;
			}
			return 30;
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x00077F40 File Offset: 0x00076F40
		internal int RealGetDaysInYear(int year)
		{
			int num = 0;
			int num2 = UmAlQuraCalendar.HijriYearInfo[year - 1318].HijriMonthsLengthFlags;
			for (int i = 1; i <= 12; i++)
			{
				num += 29 + (num2 & 1);
				num2 >>= 1;
			}
			return num;
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x00077F81 File Offset: 0x00076F81
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return this.RealGetDaysInYear(year);
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x00077F92 File Offset: 0x00076F92
		public override int GetEra(DateTime time)
		{
			this.CheckTicksRange(time.Ticks);
			return 1;
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060027D6 RID: 10198 RVA: 0x00077FA4 File Offset: 0x00076FA4
		public override int[] Eras
		{
			get
			{
				return new int[] { 1 };
			}
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x00077FBD File Offset: 0x00076FBD
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time, 2);
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x00077FC7 File Offset: 0x00076FC7
		public override int GetMonthsInYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 12;
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x00077FD3 File Offset: 0x00076FD3
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time, 0);
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x00077FE0 File Offset: 0x00076FE0
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			if (day >= 1 && day <= 29)
			{
				this.CheckYearMonthRange(year, month, era);
				return false;
			}
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { daysInMonth, month }));
			}
			return false;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x0007804F File Offset: 0x0007704F
		public override int GetLeapMonth(int year, int era)
		{
			this.CheckYearRange(year, era);
			return 0;
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x0007805A File Offset: 0x0007705A
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.CheckYearMonthRange(year, month, era);
			return false;
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x00078066 File Offset: 0x00077066
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckYearRange(year, era);
			return this.RealGetDaysInYear(year) == 355;
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x00078084 File Offset: 0x00077084
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			if (day >= 1 && day <= 29)
			{
				this.CheckYearMonthRange(year, month, era);
			}
			else
			{
				int daysInMonth = this.GetDaysInMonth(year, month, era);
				if (day < 1 || day > daysInMonth)
				{
					throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Day"), new object[] { daysInMonth, month }));
				}
			}
			long absoluteDateUmAlQura = this.GetAbsoluteDateUmAlQura(year, month, day);
			if (absoluteDateUmAlQura >= 0L)
			{
				return new DateTime(absoluteDateUmAlQura * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
			}
			throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadYearMonthDay"));
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x00078130 File Offset: 0x00077130
		// (set) Token: 0x060027E0 RID: 10208 RVA: 0x00078158 File Offset: 0x00077158
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
				if (value != 99 && (value < 1318 || value > 1450))
				{
					throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1318, 1450 }));
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x000781C8 File Offset: 0x000771C8
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year < 1318 || year > 1450)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1318, 1450 }));
			}
			return year;
		}

		// Token: 0x04001203 RID: 4611
		internal const int MinCalendarYear = 1318;

		// Token: 0x04001204 RID: 4612
		internal const int MaxCalendarYear = 1450;

		// Token: 0x04001205 RID: 4613
		public const int UmAlQuraEra = 1;

		// Token: 0x04001206 RID: 4614
		internal const int DateCycle = 30;

		// Token: 0x04001207 RID: 4615
		internal const int DatePartYear = 0;

		// Token: 0x04001208 RID: 4616
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04001209 RID: 4617
		internal const int DatePartMonth = 2;

		// Token: 0x0400120A RID: 4618
		internal const int DatePartDay = 3;

		// Token: 0x0400120B RID: 4619
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1451;

		// Token: 0x0400120C RID: 4620
		private static readonly UmAlQuraCalendar.DateMapping[] HijriYearInfo = new UmAlQuraCalendar.DateMapping[]
		{
			new UmAlQuraCalendar.DateMapping(746, 1900, 4, 30),
			new UmAlQuraCalendar.DateMapping(1769, 1901, 4, 19),
			new UmAlQuraCalendar.DateMapping(3794, 1902, 4, 9),
			new UmAlQuraCalendar.DateMapping(3748, 1903, 3, 30),
			new UmAlQuraCalendar.DateMapping(3402, 1904, 3, 18),
			new UmAlQuraCalendar.DateMapping(2710, 1905, 3, 7),
			new UmAlQuraCalendar.DateMapping(1334, 1906, 2, 24),
			new UmAlQuraCalendar.DateMapping(2741, 1907, 2, 13),
			new UmAlQuraCalendar.DateMapping(3498, 1908, 2, 3),
			new UmAlQuraCalendar.DateMapping(2980, 1909, 1, 23),
			new UmAlQuraCalendar.DateMapping(2889, 1910, 1, 12),
			new UmAlQuraCalendar.DateMapping(2707, 1911, 1, 1),
			new UmAlQuraCalendar.DateMapping(1323, 1911, 12, 21),
			new UmAlQuraCalendar.DateMapping(2647, 1912, 12, 9),
			new UmAlQuraCalendar.DateMapping(1206, 1913, 11, 29),
			new UmAlQuraCalendar.DateMapping(2741, 1914, 11, 18),
			new UmAlQuraCalendar.DateMapping(1450, 1915, 11, 8),
			new UmAlQuraCalendar.DateMapping(3413, 1916, 10, 27),
			new UmAlQuraCalendar.DateMapping(3370, 1917, 10, 17),
			new UmAlQuraCalendar.DateMapping(2646, 1918, 10, 6),
			new UmAlQuraCalendar.DateMapping(1198, 1919, 9, 25),
			new UmAlQuraCalendar.DateMapping(2397, 1920, 9, 13),
			new UmAlQuraCalendar.DateMapping(748, 1921, 9, 3),
			new UmAlQuraCalendar.DateMapping(1749, 1922, 8, 23),
			new UmAlQuraCalendar.DateMapping(1706, 1923, 8, 13),
			new UmAlQuraCalendar.DateMapping(1365, 1924, 8, 1),
			new UmAlQuraCalendar.DateMapping(1195, 1925, 7, 21),
			new UmAlQuraCalendar.DateMapping(2395, 1926, 7, 10),
			new UmAlQuraCalendar.DateMapping(698, 1927, 6, 30),
			new UmAlQuraCalendar.DateMapping(1397, 1928, 6, 18),
			new UmAlQuraCalendar.DateMapping(2994, 1929, 6, 8),
			new UmAlQuraCalendar.DateMapping(1892, 1930, 5, 29),
			new UmAlQuraCalendar.DateMapping(1865, 1931, 5, 18),
			new UmAlQuraCalendar.DateMapping(1621, 1932, 5, 6),
			new UmAlQuraCalendar.DateMapping(683, 1933, 4, 25),
			new UmAlQuraCalendar.DateMapping(1371, 1934, 4, 14),
			new UmAlQuraCalendar.DateMapping(2778, 1935, 4, 4),
			new UmAlQuraCalendar.DateMapping(1748, 1936, 3, 24),
			new UmAlQuraCalendar.DateMapping(3785, 1937, 3, 13),
			new UmAlQuraCalendar.DateMapping(3474, 1938, 3, 3),
			new UmAlQuraCalendar.DateMapping(3365, 1939, 2, 20),
			new UmAlQuraCalendar.DateMapping(2637, 1940, 2, 9),
			new UmAlQuraCalendar.DateMapping(685, 1941, 1, 28),
			new UmAlQuraCalendar.DateMapping(1389, 1942, 1, 17),
			new UmAlQuraCalendar.DateMapping(2922, 1943, 1, 7),
			new UmAlQuraCalendar.DateMapping(2898, 1943, 12, 28),
			new UmAlQuraCalendar.DateMapping(2725, 1944, 12, 16),
			new UmAlQuraCalendar.DateMapping(2635, 1945, 12, 5),
			new UmAlQuraCalendar.DateMapping(1175, 1946, 11, 24),
			new UmAlQuraCalendar.DateMapping(2359, 1947, 11, 13),
			new UmAlQuraCalendar.DateMapping(694, 1948, 11, 2),
			new UmAlQuraCalendar.DateMapping(1397, 1949, 10, 22),
			new UmAlQuraCalendar.DateMapping(3434, 1950, 10, 12),
			new UmAlQuraCalendar.DateMapping(3410, 1951, 10, 2),
			new UmAlQuraCalendar.DateMapping(2710, 1952, 9, 20),
			new UmAlQuraCalendar.DateMapping(2349, 1953, 9, 9),
			new UmAlQuraCalendar.DateMapping(605, 1954, 8, 29),
			new UmAlQuraCalendar.DateMapping(1245, 1955, 8, 18),
			new UmAlQuraCalendar.DateMapping(2778, 1956, 8, 7),
			new UmAlQuraCalendar.DateMapping(1492, 1957, 7, 28),
			new UmAlQuraCalendar.DateMapping(3497, 1958, 7, 17),
			new UmAlQuraCalendar.DateMapping(3410, 1959, 7, 7),
			new UmAlQuraCalendar.DateMapping(2730, 1960, 6, 25),
			new UmAlQuraCalendar.DateMapping(1238, 1961, 6, 14),
			new UmAlQuraCalendar.DateMapping(2486, 1962, 6, 3),
			new UmAlQuraCalendar.DateMapping(884, 1963, 5, 24),
			new UmAlQuraCalendar.DateMapping(1897, 1964, 5, 12),
			new UmAlQuraCalendar.DateMapping(1874, 1965, 5, 2),
			new UmAlQuraCalendar.DateMapping(1701, 1966, 4, 21),
			new UmAlQuraCalendar.DateMapping(1355, 1967, 4, 10),
			new UmAlQuraCalendar.DateMapping(2731, 1968, 3, 29),
			new UmAlQuraCalendar.DateMapping(1370, 1969, 3, 19),
			new UmAlQuraCalendar.DateMapping(2773, 1970, 3, 8),
			new UmAlQuraCalendar.DateMapping(3538, 1971, 2, 26),
			new UmAlQuraCalendar.DateMapping(3492, 1972, 2, 16),
			new UmAlQuraCalendar.DateMapping(3401, 1973, 2, 4),
			new UmAlQuraCalendar.DateMapping(2709, 1974, 1, 24),
			new UmAlQuraCalendar.DateMapping(1325, 1975, 1, 13),
			new UmAlQuraCalendar.DateMapping(2653, 1976, 1, 2),
			new UmAlQuraCalendar.DateMapping(1370, 1976, 12, 22),
			new UmAlQuraCalendar.DateMapping(2773, 1977, 12, 11),
			new UmAlQuraCalendar.DateMapping(1706, 1978, 12, 1),
			new UmAlQuraCalendar.DateMapping(1685, 1979, 11, 20),
			new UmAlQuraCalendar.DateMapping(1323, 1980, 11, 8),
			new UmAlQuraCalendar.DateMapping(2647, 1981, 10, 28),
			new UmAlQuraCalendar.DateMapping(1198, 1982, 10, 18),
			new UmAlQuraCalendar.DateMapping(2422, 1983, 10, 7),
			new UmAlQuraCalendar.DateMapping(1388, 1984, 9, 26),
			new UmAlQuraCalendar.DateMapping(2901, 1985, 9, 15),
			new UmAlQuraCalendar.DateMapping(2730, 1986, 9, 5),
			new UmAlQuraCalendar.DateMapping(2645, 1987, 8, 25),
			new UmAlQuraCalendar.DateMapping(1197, 1988, 8, 13),
			new UmAlQuraCalendar.DateMapping(2397, 1989, 8, 2),
			new UmAlQuraCalendar.DateMapping(730, 1990, 7, 23),
			new UmAlQuraCalendar.DateMapping(1497, 1991, 7, 12),
			new UmAlQuraCalendar.DateMapping(3506, 1992, 7, 1),
			new UmAlQuraCalendar.DateMapping(2980, 1993, 6, 21),
			new UmAlQuraCalendar.DateMapping(2890, 1994, 6, 10),
			new UmAlQuraCalendar.DateMapping(2645, 1995, 5, 30),
			new UmAlQuraCalendar.DateMapping(693, 1996, 5, 18),
			new UmAlQuraCalendar.DateMapping(1397, 1997, 5, 7),
			new UmAlQuraCalendar.DateMapping(2922, 1998, 4, 27),
			new UmAlQuraCalendar.DateMapping(3026, 1999, 4, 17),
			new UmAlQuraCalendar.DateMapping(3012, 2000, 4, 6),
			new UmAlQuraCalendar.DateMapping(2953, 2001, 3, 26),
			new UmAlQuraCalendar.DateMapping(2709, 2002, 3, 15),
			new UmAlQuraCalendar.DateMapping(1325, 2003, 3, 4),
			new UmAlQuraCalendar.DateMapping(1453, 2004, 2, 21),
			new UmAlQuraCalendar.DateMapping(2922, 2005, 2, 10),
			new UmAlQuraCalendar.DateMapping(1748, 2006, 1, 31),
			new UmAlQuraCalendar.DateMapping(3529, 2007, 1, 20),
			new UmAlQuraCalendar.DateMapping(3474, 2008, 1, 10),
			new UmAlQuraCalendar.DateMapping(2726, 2008, 12, 29),
			new UmAlQuraCalendar.DateMapping(2390, 2009, 12, 18),
			new UmAlQuraCalendar.DateMapping(686, 2010, 12, 7),
			new UmAlQuraCalendar.DateMapping(1389, 2011, 11, 26),
			new UmAlQuraCalendar.DateMapping(874, 2012, 11, 15),
			new UmAlQuraCalendar.DateMapping(2901, 2013, 11, 4),
			new UmAlQuraCalendar.DateMapping(2730, 2014, 10, 25),
			new UmAlQuraCalendar.DateMapping(2381, 2015, 10, 14),
			new UmAlQuraCalendar.DateMapping(1181, 2016, 10, 2),
			new UmAlQuraCalendar.DateMapping(2397, 2017, 9, 21),
			new UmAlQuraCalendar.DateMapping(698, 2018, 9, 11),
			new UmAlQuraCalendar.DateMapping(1461, 2019, 8, 31),
			new UmAlQuraCalendar.DateMapping(1450, 2020, 8, 20),
			new UmAlQuraCalendar.DateMapping(3413, 2021, 8, 9),
			new UmAlQuraCalendar.DateMapping(2714, 2022, 7, 30),
			new UmAlQuraCalendar.DateMapping(2350, 2023, 7, 19),
			new UmAlQuraCalendar.DateMapping(622, 2024, 7, 7),
			new UmAlQuraCalendar.DateMapping(1373, 2025, 6, 26),
			new UmAlQuraCalendar.DateMapping(2778, 2026, 6, 16),
			new UmAlQuraCalendar.DateMapping(1748, 2027, 6, 6),
			new UmAlQuraCalendar.DateMapping(1701, 2028, 5, 25),
			new UmAlQuraCalendar.DateMapping(0, 2029, 5, 14)
		};

		// Token: 0x0400120D RID: 4621
		internal static short[] gmonth = new short[]
		{
			31, 31, 28, 31, 30, 31, 30, 31, 31, 30,
			31, 30, 31, 31
		};

		// Token: 0x0400120E RID: 4622
		internal static DateTime minDate = new DateTime(1900, 4, 30);

		// Token: 0x0400120F RID: 4623
		internal static DateTime maxDate = new DateTime(new DateTime(2029, 5, 13, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x020003BB RID: 955
		internal struct DateMapping
		{
			// Token: 0x060027E3 RID: 10211 RVA: 0x00079313 File Offset: 0x00078313
			internal DateMapping(int MonthsLengthFlags, int GYear, int GMonth, int GDay)
			{
				this.HijriMonthsLengthFlags = MonthsLengthFlags;
				this.GregorianDate = new DateTime(GYear, GMonth, GDay);
			}

			// Token: 0x04001210 RID: 4624
			internal int HijriMonthsLengthFlags;

			// Token: 0x04001211 RID: 4625
			internal DateTime GregorianDate;
		}
	}
}

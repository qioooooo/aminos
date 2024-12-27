﻿using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003A4 RID: 932
	[ComVisible(true)]
	[Serializable]
	public class HebrewCalendar : Calendar
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x0006AAD1 File Offset: 0x00069AD1
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return HebrewCalendar.calendarMinValue;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x0006AAD8 File Offset: 0x00069AD8
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return HebrewCalendar.calendarMaxValue;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x0006AADF File Offset: 0x00069ADF
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.LunisolarCalendar;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x0006AAEA File Offset: 0x00069AEA
		internal override int ID
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x0006AAF0 File Offset: 0x00069AF0
		private void CheckHebrewYearValue(int y, int era, string varName)
		{
			this.CheckEraRange(era);
			if (y > 5999 || y < 5343)
			{
				throw new ArgumentOutOfRangeException(varName, string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 5343, 5999 }));
			}
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x0006AB54 File Offset: 0x00069B54
		private void CheckHebrewMonthValue(int year, int month, int era)
		{
			int monthsInYear = this.GetMonthsInYear(year, era);
			if (month < 1 || month > monthsInYear)
			{
				throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, monthsInYear }));
			}
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x0006ABAC File Offset: 0x00069BAC
		private void CheckHebrewDayValue(int year, int month, int day, int era)
		{
			int daysInMonth = this.GetDaysInMonth(year, month, era);
			if (day < 1 || day > daysInMonth)
			{
				throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1, daysInMonth }));
			}
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x0006AC05 File Offset: 0x00069C05
		internal void CheckEraRange(int era)
		{
			if (era != 0 && era != HebrewCalendar.HebrewEra)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x0006AC28 File Offset: 0x00069C28
		private void CheckTicksRange(long ticks)
		{
			if (ticks < HebrewCalendar.calendarMinValue.Ticks || ticks > HebrewCalendar.calendarMaxValue.Ticks)
			{
				throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("ArgumentOutOfRange_CalendarRange"), new object[]
				{
					HebrewCalendar.calendarMinValue,
					HebrewCalendar.calendarMaxValue
				}));
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x0006AC98 File Offset: 0x00069C98
		internal int GetResult(HebrewCalendar.__DateBuffer result, int part)
		{
			switch (part)
			{
			case 0:
				return result.year;
			case 2:
				return result.month;
			case 3:
				return result.day;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_DateTimeParsing"));
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x0006ACE4 File Offset: 0x00069CE4
		internal int GetLunarMonthDay(int gregorianYear, HebrewCalendar.__DateBuffer lunarDate)
		{
			int num = gregorianYear - 1583;
			if (num < 0 || num > 656)
			{
				throw new ArgumentOutOfRangeException("gregorianYear");
			}
			num *= 2;
			lunarDate.day = HebrewCalendar.m_HebrewTable[num];
			int num2 = HebrewCalendar.m_HebrewTable[num + 1];
			int day = lunarDate.day;
			if (day != 0)
			{
				switch (day)
				{
				case 30:
					lunarDate.month = 3;
					break;
				case 31:
					lunarDate.month = 5;
					lunarDate.day = 2;
					break;
				case 32:
					lunarDate.month = 5;
					lunarDate.day = 3;
					break;
				case 33:
					lunarDate.month = 3;
					lunarDate.day = 29;
					break;
				default:
					lunarDate.month = 4;
					break;
				}
			}
			else
			{
				lunarDate.month = 5;
				lunarDate.day = 1;
			}
			return num2;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x0006ADA4 File Offset: 0x00069DA4
		internal virtual int GetDatePart(long ticks, int part)
		{
			this.CheckTicksRange(ticks);
			DateTime dateTime = new DateTime(ticks);
			int year = dateTime.Year;
			int month = dateTime.Month;
			int day = dateTime.Day;
			HebrewCalendar.__DateBuffer _DateBuffer = new HebrewCalendar.__DateBuffer();
			_DateBuffer.year = year + 3760;
			int num = this.GetLunarMonthDay(year, _DateBuffer);
			HebrewCalendar.__DateBuffer _DateBuffer2 = new HebrewCalendar.__DateBuffer();
			_DateBuffer2.year = _DateBuffer.year;
			_DateBuffer2.month = _DateBuffer.month;
			_DateBuffer2.day = _DateBuffer.day;
			long absoluteDate = GregorianCalendar.GetAbsoluteDate(year, month, day);
			if (month == 1 && day == 1)
			{
				return this.GetResult(_DateBuffer2, part);
			}
			long num2 = absoluteDate - GregorianCalendar.GetAbsoluteDate(year, 1, 1);
			if (num2 + (long)_DateBuffer.day <= (long)HebrewCalendar.m_lunarMonthLen[num, _DateBuffer.month])
			{
				_DateBuffer2.day += (int)num2;
				return this.GetResult(_DateBuffer2, part);
			}
			_DateBuffer2.month++;
			_DateBuffer2.day = 1;
			num2 -= (long)(HebrewCalendar.m_lunarMonthLen[num, _DateBuffer.month] - _DateBuffer.day);
			if (num2 > 1L)
			{
				while (num2 > (long)HebrewCalendar.m_lunarMonthLen[num, _DateBuffer2.month])
				{
					num2 -= (long)HebrewCalendar.m_lunarMonthLen[num, _DateBuffer2.month++];
					if (_DateBuffer2.month > 13 || HebrewCalendar.m_lunarMonthLen[num, _DateBuffer2.month] == 0)
					{
						_DateBuffer2.year++;
						num = HebrewCalendar.m_HebrewTable[(year + 1 - 1583) * 2 + 1];
						_DateBuffer2.month = 1;
					}
				}
				_DateBuffer2.day += (int)(num2 - 1L);
			}
			return this.GetResult(_DateBuffer2, part);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x0006AF74 File Offset: 0x00069F74
		public override DateTime AddMonths(DateTime time, int months)
		{
			DateTime dateTime;
			try
			{
				int num = this.GetDatePart(time.Ticks, 0);
				int datePart = this.GetDatePart(time.Ticks, 2);
				int num2 = this.GetDatePart(time.Ticks, 3);
				int i;
				if (months >= 0)
				{
					int num3;
					for (i = datePart + months; i > (num3 = this.GetMonthsInYear(num, 0)); i -= num3)
					{
						num++;
					}
				}
				else if ((i = datePart + months) <= 0)
				{
					months = -months;
					months -= datePart;
					num--;
					int num3;
					while (months > (num3 = this.GetMonthsInYear(num, 0)))
					{
						num--;
						months -= num3;
					}
					num3 = this.GetMonthsInYear(num, 0);
					i = num3 - months;
				}
				int daysInMonth = this.GetDaysInMonth(num, i);
				if (num2 > daysInMonth)
				{
					num2 = daysInMonth;
				}
				dateTime = new DateTime(this.ToDateTime(num, i, num2, 0, 0, 0, 0).Ticks + time.Ticks % 864000000000L);
			}
			catch (ArgumentException)
			{
				throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_AddValue"), new object[0]));
			}
			return dateTime;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x0006B08C File Offset: 0x0006A08C
		public override DateTime AddYears(DateTime time, int years)
		{
			int num = this.GetDatePart(time.Ticks, 0);
			int num2 = this.GetDatePart(time.Ticks, 2);
			int num3 = this.GetDatePart(time.Ticks, 3);
			num += years;
			this.CheckHebrewYearValue(num, 0, "years");
			int monthsInYear = this.GetMonthsInYear(num, 0);
			if (num2 > monthsInYear)
			{
				num2 = monthsInYear;
			}
			int daysInMonth = this.GetDaysInMonth(num, num2);
			if (num3 > daysInMonth)
			{
				num3 = daysInMonth;
			}
			long num4 = this.ToDateTime(num, num2, num3, 0, 0, 0, 0).Ticks + time.Ticks % 864000000000L;
			Calendar.CheckAddResult(num4, this.MinSupportedDateTime, this.MaxSupportedDateTime);
			return new DateTime(num4);
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x0006B13C File Offset: 0x0006A13C
		public override int GetDayOfMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 3);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x0006B14C File Offset: 0x0006A14C
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			return (DayOfWeek)(time.Ticks / 864000000000L + 1L) % (DayOfWeek)7;
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x0006B165 File Offset: 0x0006A165
		internal int GetHebrewYearType(int year, int era)
		{
			this.CheckHebrewYearValue(year, era, "year");
			return HebrewCalendar.m_HebrewTable[(year - 3760 - 1583) * 2 + 1];
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x0006B18C File Offset: 0x0006A18C
		public override int GetDayOfYear(DateTime time)
		{
			int year = this.GetYear(time);
			DateTime dateTime = this.ToDateTime(year, 1, 1, 0, 0, 0, 0, 0);
			return (int)((time.Ticks - dateTime.Ticks) / 864000000000L) + 1;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x0006B1CC File Offset: 0x0006A1CC
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.CheckEraRange(era);
			int hebrewYearType = this.GetHebrewYearType(year, era);
			this.CheckHebrewMonthValue(year, month, era);
			int num = HebrewCalendar.m_lunarMonthLen[hebrewYearType, month];
			if (num == 0)
			{
				throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("ArgumentOutOfRange_Month"));
			}
			return num;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x0006B218 File Offset: 0x0006A218
		public override int GetDaysInYear(int year, int era)
		{
			this.CheckEraRange(era);
			int hebrewYearType = this.GetHebrewYearType(year, era);
			if (hebrewYearType < 4)
			{
				return 352 + hebrewYearType;
			}
			return 382 + (hebrewYearType - 3);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x0006B24A File Offset: 0x0006A24A
		public override int GetEra(DateTime time)
		{
			return HebrewCalendar.HebrewEra;
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x0006B254 File Offset: 0x0006A254
		public override int[] Eras
		{
			get
			{
				return new int[] { HebrewCalendar.HebrewEra };
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x0006B271 File Offset: 0x0006A271
		public override int GetMonth(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 2);
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x0006B281 File Offset: 0x0006A281
		public override int GetMonthsInYear(int year, int era)
		{
			if (!this.IsLeapYear(year, era))
			{
				return 12;
			}
			return 13;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x0006B292 File Offset: 0x0006A292
		public override int GetYear(DateTime time)
		{
			return this.GetDatePart(time.Ticks, 0);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x0006B2A2 File Offset: 0x0006A2A2
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			if (this.IsLeapMonth(year, month, era))
			{
				this.CheckHebrewDayValue(year, month, day, era);
				return true;
			}
			if (this.IsLeapYear(year, 0) && month == 6 && day == 30)
			{
				return true;
			}
			this.CheckHebrewDayValue(year, month, day, era);
			return false;
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x0006B2DE File Offset: 0x0006A2DE
		public override int GetLeapMonth(int year, int era)
		{
			if (this.IsLeapYear(year, era))
			{
				return 7;
			}
			return 0;
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x0006B2F0 File Offset: 0x0006A2F0
		public override bool IsLeapMonth(int year, int month, int era)
		{
			bool flag = this.IsLeapYear(year, era);
			this.CheckHebrewMonthValue(year, month, era);
			return flag && month == 7;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x0006B319 File Offset: 0x0006A319
		public override bool IsLeapYear(int year, int era)
		{
			this.CheckHebrewYearValue(year, era, "year");
			return (7L * (long)year + 1L) % 19L < 7L;
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x0006B338 File Offset: 0x0006A338
		private int GetDayDifference(int lunarYearType, int month1, int day1, int month2, int day2)
		{
			if (month1 == month2)
			{
				return day1 - day2;
			}
			bool flag = month1 > month2;
			if (flag)
			{
				int num = month1;
				int num2 = day1;
				month1 = month2;
				day1 = day2;
				month2 = num;
				day2 = num2;
			}
			int num3 = HebrewCalendar.m_lunarMonthLen[lunarYearType, month1] - day1;
			month1++;
			while (month1 < month2)
			{
				num3 += HebrewCalendar.m_lunarMonthLen[lunarYearType, month1++];
			}
			num3 += day2;
			if (!flag)
			{
				return -num3;
			}
			return num3;
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x0006B3A8 File Offset: 0x0006A3A8
		private DateTime HebrewToGregorian(int hebrewYear, int hebrewMonth, int hebrewDay, int hour, int minute, int second, int millisecond)
		{
			int num = hebrewYear - 3760;
			HebrewCalendar.__DateBuffer _DateBuffer = new HebrewCalendar.__DateBuffer();
			int lunarMonthDay = this.GetLunarMonthDay(num, _DateBuffer);
			if (hebrewMonth == _DateBuffer.month && hebrewDay == _DateBuffer.day)
			{
				return new DateTime(num, 1, 1, hour, minute, second, millisecond);
			}
			int dayDifference = this.GetDayDifference(lunarMonthDay, hebrewMonth, hebrewDay, _DateBuffer.month, _DateBuffer.day);
			DateTime dateTime = new DateTime(num, 1, 1);
			return new DateTime(dateTime.Ticks + (long)dayDifference * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x0006B438 File Offset: 0x0006A438
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			this.CheckHebrewYearValue(year, era, "year");
			this.CheckHebrewMonthValue(year, month, era);
			this.CheckHebrewDayValue(year, month, day, era);
			DateTime dateTime = this.HebrewToGregorian(year, month, day, hour, minute, second, millisecond);
			this.CheckTicksRange(dateTime.Ticks);
			return dateTime;
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x0006B488 File Offset: 0x0006A488
		// (set) Token: 0x060025E0 RID: 9696 RVA: 0x0006B4AF File Offset: 0x0006A4AF
		public override int TwoDigitYearMax
		{
			get
			{
				if (this.twoDigitYearMax == -1)
				{
					this.twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(this.ID, 5790);
				}
				return this.twoDigitYearMax;
			}
			set
			{
				base.VerifyWritable();
				if (value != 99)
				{
					this.CheckHebrewYearValue(value, HebrewCalendar.HebrewEra, "value");
				}
				this.twoDigitYearMax = value;
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x0006B4D4 File Offset: 0x0006A4D4
		public override int ToFourDigitYear(int year)
		{
			if (year < 100)
			{
				return base.ToFourDigitYear(year);
			}
			if (year > 5999 || year < 5343)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 5343, 5999 }));
			}
			return year;
		}

		// Token: 0x04001110 RID: 4368
		internal const int DatePartYear = 0;

		// Token: 0x04001111 RID: 4369
		internal const int DatePartDayOfYear = 1;

		// Token: 0x04001112 RID: 4370
		internal const int DatePartMonth = 2;

		// Token: 0x04001113 RID: 4371
		internal const int DatePartDay = 3;

		// Token: 0x04001114 RID: 4372
		internal const int DatePartDayOfWeek = 4;

		// Token: 0x04001115 RID: 4373
		private const int HebrewYearOf1AD = 3760;

		// Token: 0x04001116 RID: 4374
		private const int FirstGregorianTableYear = 1583;

		// Token: 0x04001117 RID: 4375
		private const int LastGregorianTableYear = 2239;

		// Token: 0x04001118 RID: 4376
		private const int TABLESIZE = 656;

		// Token: 0x04001119 RID: 4377
		private const int m_minHebrewYear = 5343;

		// Token: 0x0400111A RID: 4378
		private const int m_maxHebrewYear = 5999;

		// Token: 0x0400111B RID: 4379
		private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 5790;

		// Token: 0x0400111C RID: 4380
		public static readonly int HebrewEra = 1;

		// Token: 0x0400111D RID: 4381
		private static readonly int[] m_HebrewTable = new int[]
		{
			7, 3, 17, 3, 0, 4, 11, 2, 21, 6,
			1, 3, 13, 2, 25, 4, 5, 3, 16, 2,
			27, 6, 9, 1, 20, 2, 0, 6, 11, 3,
			23, 4, 4, 2, 14, 3, 27, 4, 8, 2,
			18, 3, 28, 6, 11, 1, 22, 5, 2, 3,
			12, 3, 25, 4, 6, 2, 16, 3, 26, 6,
			8, 2, 20, 1, 0, 6, 11, 2, 24, 4,
			4, 3, 15, 2, 25, 6, 8, 1, 19, 2,
			29, 6, 9, 3, 22, 4, 3, 2, 13, 3,
			25, 4, 6, 3, 17, 2, 27, 6, 7, 3,
			19, 2, 31, 4, 11, 3, 23, 4, 5, 2,
			15, 3, 25, 6, 6, 2, 19, 1, 29, 6,
			10, 2, 22, 4, 3, 3, 14, 2, 24, 6,
			6, 1, 17, 3, 28, 5, 8, 3, 20, 1,
			32, 5, 12, 3, 22, 6, 4, 1, 16, 2,
			26, 6, 6, 3, 17, 2, 0, 4, 10, 3,
			22, 4, 3, 2, 14, 3, 24, 6, 5, 2,
			17, 1, 28, 6, 9, 2, 19, 3, 31, 4,
			13, 2, 23, 6, 3, 3, 15, 1, 27, 5,
			7, 3, 17, 3, 29, 4, 11, 2, 21, 6,
			3, 1, 14, 2, 25, 6, 5, 3, 16, 2,
			28, 4, 9, 3, 20, 2, 0, 6, 12, 1,
			23, 6, 4, 2, 14, 3, 26, 4, 8, 2,
			18, 3, 0, 4, 10, 3, 21, 5, 1, 3,
			13, 1, 24, 5, 5, 3, 15, 3, 27, 4,
			8, 2, 19, 3, 29, 6, 10, 2, 22, 4,
			3, 3, 14, 2, 26, 4, 6, 3, 18, 2,
			28, 6, 10, 1, 20, 6, 2, 2, 12, 3,
			24, 4, 5, 2, 16, 3, 28, 4, 8, 3,
			19, 2, 0, 6, 12, 1, 23, 5, 3, 3,
			14, 3, 26, 4, 7, 2, 17, 3, 28, 6,
			9, 2, 21, 4, 1, 3, 13, 2, 25, 4,
			5, 3, 16, 2, 27, 6, 9, 1, 19, 3,
			0, 5, 11, 3, 23, 4, 4, 2, 14, 3,
			25, 6, 7, 1, 18, 2, 28, 6, 9, 3,
			21, 4, 2, 2, 12, 3, 25, 4, 6, 2,
			16, 3, 26, 6, 8, 2, 20, 1, 0, 6,
			11, 2, 22, 6, 4, 1, 15, 2, 25, 6,
			6, 3, 18, 1, 29, 5, 9, 3, 22, 4,
			2, 3, 13, 2, 23, 6, 4, 3, 15, 2,
			27, 4, 7, 3, 19, 2, 31, 4, 11, 3,
			21, 6, 3, 2, 15, 1, 25, 6, 6, 2,
			17, 3, 29, 4, 10, 2, 20, 6, 3, 1,
			13, 3, 24, 5, 4, 3, 16, 1, 27, 5,
			7, 3, 17, 3, 0, 4, 11, 2, 21, 6,
			1, 3, 13, 2, 25, 4, 5, 3, 16, 2,
			29, 4, 9, 3, 19, 6, 30, 2, 13, 1,
			23, 6, 4, 2, 14, 3, 27, 4, 8, 2,
			18, 3, 0, 4, 11, 3, 22, 5, 2, 3,
			14, 1, 26, 5, 6, 3, 16, 3, 28, 4,
			10, 2, 20, 6, 30, 3, 11, 2, 24, 4,
			4, 3, 15, 2, 25, 6, 8, 1, 19, 2,
			29, 6, 9, 3, 22, 4, 3, 2, 13, 3,
			25, 4, 7, 2, 17, 3, 27, 6, 9, 1,
			21, 5, 1, 3, 11, 3, 23, 4, 5, 2,
			15, 3, 25, 6, 6, 2, 19, 1, 29, 6,
			10, 2, 22, 4, 3, 3, 14, 2, 24, 6,
			6, 1, 18, 2, 28, 6, 8, 3, 20, 4,
			2, 2, 12, 3, 24, 4, 4, 3, 16, 2,
			26, 6, 6, 3, 17, 2, 0, 4, 10, 3,
			22, 4, 3, 2, 14, 3, 24, 6, 5, 2,
			17, 1, 28, 6, 9, 2, 21, 4, 1, 3,
			13, 2, 23, 6, 5, 1, 15, 3, 27, 5,
			7, 3, 19, 1, 0, 5, 10, 3, 22, 4,
			2, 3, 13, 2, 24, 6, 4, 3, 15, 2,
			27, 4, 8, 3, 20, 4, 1, 2, 11, 3,
			22, 6, 3, 2, 15, 1, 25, 6, 7, 2,
			17, 3, 29, 4, 10, 2, 21, 6, 1, 3,
			13, 1, 24, 5, 5, 3, 15, 3, 27, 4,
			8, 2, 19, 6, 1, 1, 12, 2, 22, 6,
			3, 3, 14, 2, 26, 4, 6, 3, 18, 2,
			28, 6, 10, 1, 20, 6, 2, 2, 12, 3,
			24, 4, 5, 2, 16, 3, 28, 4, 9, 2,
			19, 6, 30, 3, 12, 1, 23, 5, 3, 3,
			14, 3, 26, 4, 7, 2, 17, 3, 28, 6,
			9, 2, 21, 4, 1, 3, 13, 2, 25, 4,
			5, 3, 16, 2, 27, 6, 9, 1, 19, 6,
			30, 2, 11, 3, 23, 4, 4, 2, 14, 3,
			27, 4, 7, 3, 18, 2, 28, 6, 11, 1,
			22, 5, 2, 3, 12, 3, 25, 4, 6, 2,
			16, 3, 26, 6, 8, 2, 20, 4, 30, 3,
			11, 2, 24, 4, 4, 3, 15, 2, 25, 6,
			8, 1, 18, 3, 29, 5, 9, 3, 22, 4,
			3, 2, 13, 3, 23, 6, 6, 1, 17, 2,
			27, 6, 7, 3, 20, 4, 1, 2, 11, 3,
			23, 4, 5, 2, 15, 3, 25, 6, 6, 2,
			19, 1, 29, 6, 10, 2, 20, 6, 3, 1,
			14, 2, 24, 6, 4, 3, 17, 1, 28, 5,
			8, 3, 20, 4, 1, 3, 12, 2, 22, 6,
			2, 3, 14, 2, 26, 4, 6, 3, 17, 2,
			0, 4, 10, 3, 20, 6, 1, 2, 14, 1,
			24, 6, 5, 2, 15, 3, 28, 4, 9, 2,
			19, 6, 1, 1, 12, 3, 23, 5, 3, 3,
			15, 1, 27, 5, 7, 3, 17, 3, 29, 4,
			11, 2, 21, 6, 1, 3, 12, 2, 25, 4,
			5, 3, 16, 2, 28, 4, 9, 3, 19, 6,
			30, 2, 12, 1, 23, 6, 4, 2, 14, 3,
			26, 4, 8, 2, 18, 3, 0, 4, 10, 3,
			22, 5, 2, 3, 14, 1, 25, 5, 6, 3,
			16, 3, 28, 4, 9, 2, 20, 6, 30, 3,
			11, 2, 23, 4, 4, 3, 15, 2, 27, 4,
			7, 3, 19, 2, 29, 6, 11, 1, 21, 6,
			3, 2, 13, 3, 25, 4, 6, 2, 17, 3,
			27, 6, 9, 1, 20, 5, 30, 3, 10, 3,
			22, 4, 3, 2, 14, 3, 24, 6, 5, 2,
			17, 1, 28, 6, 9, 2, 21, 4, 1, 3,
			13, 2, 23, 6, 5, 1, 16, 2, 27, 6,
			7, 3, 19, 4, 30, 2, 11, 3, 23, 4,
			3, 3, 14, 2, 25, 6, 5, 3, 16, 2,
			28, 4, 9, 3, 21, 4, 2, 2, 12, 3,
			23, 6, 4, 2, 16, 1, 26, 6, 8, 2,
			20, 4, 30, 3, 11, 2, 22, 6, 4, 1,
			14, 3, 25, 5, 6, 3, 18, 1, 29, 5,
			9, 3, 22, 4, 2, 3, 13, 2, 23, 6,
			4, 3, 15, 2, 27, 4, 7, 3, 20, 4,
			1, 2, 11, 3, 21, 6, 3, 2, 15, 1,
			25, 6, 6, 2, 17, 3, 29, 4, 10, 2,
			20, 6, 3, 1, 13, 3, 24, 5, 4, 3,
			17, 1, 28, 5, 8, 3, 18, 6, 1, 1,
			12, 2, 22, 6, 2, 3, 14, 2, 26, 4,
			6, 3, 17, 2, 28, 6, 10, 1, 20, 6,
			1, 2, 12, 3, 24, 4, 5, 2, 15, 3,
			28, 4, 9, 2, 19, 6, 33, 3, 12, 1,
			23, 5, 3, 3, 13, 3, 25, 4, 6, 2,
			16, 3, 26, 6, 8, 2, 20, 4, 30, 3,
			11, 2, 24, 4, 4, 3, 15, 2, 25, 6,
			8, 1, 18, 6, 33, 2, 9, 3, 22, 4,
			3, 2, 13, 3, 25, 4, 6, 3, 17, 2,
			27, 6, 9, 1, 21, 5, 1, 3, 11, 3,
			23, 4, 5, 2, 15, 3, 25, 6, 6, 2,
			19, 4, 33, 3, 10, 2, 22, 4, 3, 3,
			14, 2, 24, 6, 6, 1
		};

		// Token: 0x0400111E RID: 4382
		private static readonly int[,] m_lunarMonthLen = new int[,]
		{
			{
				0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0
			},
			{
				0, 30, 29, 29, 29, 30, 29, 30, 29, 30,
				29, 30, 29, 0
			},
			{
				0, 30, 29, 30, 29, 30, 29, 30, 29, 30,
				29, 30, 29, 0
			},
			{
				0, 30, 30, 30, 29, 30, 29, 30, 29, 30,
				29, 30, 29, 0
			},
			{
				0, 30, 29, 29, 29, 30, 30, 29, 30, 29,
				30, 29, 30, 29
			},
			{
				0, 30, 29, 30, 29, 30, 30, 29, 30, 29,
				30, 29, 30, 29
			},
			{
				0, 30, 30, 30, 29, 30, 30, 29, 30, 29,
				30, 29, 30, 29
			}
		};

		// Token: 0x0400111F RID: 4383
		internal static readonly DateTime calendarMinValue = new DateTime(1583, 1, 1);

		// Token: 0x04001120 RID: 4384
		internal static readonly DateTime calendarMaxValue = new DateTime(new DateTime(2239, 9, 29, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x020003A5 RID: 933
		internal class __DateBuffer
		{
			// Token: 0x04001121 RID: 4385
			internal int year;

			// Token: 0x04001122 RID: 4386
			internal int month;

			// Token: 0x04001123 RID: 4387
			internal int day;
		}
	}
}

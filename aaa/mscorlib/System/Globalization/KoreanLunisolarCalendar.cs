﻿using System;

namespace System.Globalization
{
	// Token: 0x020003AD RID: 941
	[Serializable]
	public class KoreanLunisolarCalendar : EastAsianLunisolarCalendar
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x00070056 File Offset: 0x0006F056
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return KoreanLunisolarCalendar.minDate;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x0007005D File Offset: 0x0006F05D
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return KoreanLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x00070064 File Offset: 0x0006F064
		internal override int MinCalendarYear
		{
			get
			{
				return 918;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x0007006B File Offset: 0x0006F06B
		internal override int MaxCalendarYear
		{
			get
			{
				return 2050;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x00070072 File Offset: 0x0006F072
		internal override DateTime MinDate
		{
			get
			{
				return KoreanLunisolarCalendar.minDate;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x00070079 File Offset: 0x0006F079
		internal override DateTime MaxDate
		{
			get
			{
				return KoreanLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x00070080 File Offset: 0x0006F080
		internal override EraInfo[] CalEraInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x00070084 File Offset: 0x0006F084
		internal override int GetYearInfo(int LunarYear, int Index)
		{
			if (LunarYear < 918 || LunarYear > 2050)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 918, 2050 }));
			}
			return KoreanLunisolarCalendar.yinfo[LunarYear - 918, Index];
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000700F4 File Offset: 0x0006F0F4
		internal override int GetYear(int year, DateTime time)
		{
			return year;
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000700F8 File Offset: 0x0006F0F8
		internal override int GetGregorianYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 918 || year > 2050)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 918, 2050 }));
			}
			return year;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0007017B File Offset: 0x0006F17B
		public override int GetEra(DateTime time)
		{
			base.CheckTicksRange(time.Ticks);
			return 1;
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x0007018B File Offset: 0x0006F18B
		internal override int BaseCalendarID
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x060026B9 RID: 9913 RVA: 0x0007018E File Offset: 0x0006F18E
		internal override int ID
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x00070194 File Offset: 0x0006F194
		public override int[] Eras
		{
			get
			{
				return new int[] { 1 };
			}
		}

		// Token: 0x04001176 RID: 4470
		public const int GregorianEra = 1;

		// Token: 0x04001177 RID: 4471
		internal const int MIN_LUNISOLAR_YEAR = 918;

		// Token: 0x04001178 RID: 4472
		internal const int MAX_LUNISOLAR_YEAR = 2050;

		// Token: 0x04001179 RID: 4473
		internal const int MIN_GREGORIAN_YEAR = 918;

		// Token: 0x0400117A RID: 4474
		internal const int MIN_GREGORIAN_MONTH = 2;

		// Token: 0x0400117B RID: 4475
		internal const int MIN_GREGORIAN_DAY = 14;

		// Token: 0x0400117C RID: 4476
		internal const int MAX_GREGORIAN_YEAR = 2051;

		// Token: 0x0400117D RID: 4477
		internal const int MAX_GREGORIAN_MONTH = 2;

		// Token: 0x0400117E RID: 4478
		internal const int MAX_GREGORIAN_DAY = 10;

		// Token: 0x0400117F RID: 4479
		internal static DateTime minDate = new DateTime(918, 2, 14);

		// Token: 0x04001180 RID: 4480
		internal static DateTime maxDate = new DateTime(new DateTime(2051, 2, 10, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x04001181 RID: 4481
		private static readonly int[,] yinfo = new int[,]
		{
			{ 0, 2, 14, 21936 },
			{ 0, 2, 4, 17872 },
			{ 6, 1, 24, 41688 },
			{ 0, 2, 11, 41648 },
			{ 0, 1, 31, 43344 },
			{ 4, 1, 20, 46248 },
			{ 0, 2, 8, 27936 },
			{ 12, 1, 27, 44384 },
			{ 0, 2, 15, 43872 },
			{ 0, 2, 5, 21936 },
			{ 8, 1, 26, 17848 },
			{ 0, 2, 13, 17776 },
			{ 0, 2, 2, 21168 },
			{ 5, 1, 22, 26960 },
			{ 0, 2, 9, 59728 },
			{ 0, 1, 29, 27296 },
			{ 1, 1, 18, 44368 },
			{ 0, 2, 6, 43856 },
			{ 11, 1, 27, 21344 },
			{ 0, 2, 13, 51904 },
			{ 0, 2, 2, 58720 },
			{ 7, 1, 23, 53928 },
			{ 0, 2, 11, 53920 },
			{ 0, 1, 30, 55632 },
			{ 3, 1, 20, 23208 },
			{ 0, 2, 8, 22176 },
			{ 12, 1, 28, 42704 },
			{ 0, 2, 15, 38352 },
			{ 0, 2, 5, 19152 },
			{ 7, 1, 25, 42200 },
			{ 0, 2, 13, 42192 },
			{ 0, 2, 1, 45664 },
			{ 5, 1, 21, 46416 },
			{ 0, 2, 9, 45936 },
			{ 0, 12, 31, 43728 },
			{ 1, 1, 18, 38352 },
			{ 0, 2, 6, 38320 },
			{ 9, 1, 27, 19128 },
			{ 0, 2, 15, 18864 },
			{ 0, 2, 3, 42160 },
			{ 7, 1, 23, 43672 },
			{ 0, 2, 11, 27296 },
			{ 0, 1, 31, 44368 },
			{ 3, 1, 20, 19880 },
			{ 0, 2, 8, 11104 },
			{ 12, 1, 28, 38256 },
			{ 0, 2, 16, 41840 },
			{ 0, 2, 5, 20848 },
			{ 8, 1, 25, 25776 },
			{ 0, 2, 12, 54448 },
			{ 0, 2, 2, 23184 },
			{ 5, 1, 21, 27472 },
			{ 0, 2, 9, 22224 },
			{ 0, 1, 30, 10976 },
			{ 2, 1, 19, 37744 },
			{ 0, 2, 6, 41696 },
			{ 10, 1, 26, 51560 },
			{ 0, 2, 14, 43344 },
			{ 0, 2, 3, 54432 },
			{ 7, 1, 22, 55952 },
			{ 0, 2, 10, 46496 },
			{ 0, 1, 31, 22224 },
			{ 3, 1, 21, 10968 },
			{ 0, 2, 8, 9680 },
			{ 12, 1, 28, 37592 },
			{ 0, 2, 16, 37552 },
			{ 0, 2, 5, 43344 },
			{ 9, 1, 24, 46248 },
			{ 0, 2, 12, 44192 },
			{ 0, 2, 1, 44368 },
			{ 5, 1, 22, 21936 },
			{ 0, 2, 9, 19376 },
			{ 0, 1, 30, 9648 },
			{ 2, 1, 19, 37560 },
			{ 0, 2, 7, 21168 },
			{ 10, 1, 26, 26968 },
			{ 0, 2, 14, 22864 },
			{ 0, 2, 3, 27296 },
			{ 7, 1, 23, 43856 },
			{ 0, 2, 10, 43872 },
			{ 0, 1, 31, 19296 },
			{ 3, 1, 20, 42352 },
			{ 0, 2, 8, 9584 },
			{ 12, 1, 28, 21168 },
			{ 0, 2, 15, 53920 },
			{ 0, 2, 4, 54608 },
			{ 9, 1, 25, 23208 },
			{ 0, 2, 12, 22176 },
			{ 0, 2, 1, 38608 },
			{ 5, 1, 22, 19176 },
			{ 0, 2, 10, 19152 },
			{ 0, 1, 29, 42192 },
			{ 2, 1, 18, 53864 },
			{ 0, 2, 6, 45664 },
			{ 10, 1, 26, 46416 },
			{ 0, 2, 13, 44368 },
			{ 0, 2, 3, 13728 },
			{ 6, 1, 23, 38352 },
			{ 0, 2, 11, 38320 },
			{ 0, 1, 31, 18864 },
			{ 4, 1, 20, 42200 },
			{ 0, 2, 8, 42160 },
			{ 12, 1, 28, 43608 },
			{ 0, 2, 15, 27296 },
			{ 0, 2, 4, 44368 },
			{ 9, 1, 25, 11688 },
			{ 0, 2, 13, 11088 },
			{ 0, 2, 1, 38256 },
			{ 5, 1, 22, 18800 },
			{ 0, 2, 9, 51568 },
			{ 0, 1, 30, 25776 },
			{ 2, 1, 18, 27216 },
			{ 0, 2, 5, 55952 },
			{ 10, 1, 26, 27472 },
			{ 0, 2, 14, 26320 },
			{ 0, 2, 3, 9952 },
			{ 6, 1, 23, 37744 },
			{ 0, 2, 11, 37600 },
			{ 0, 1, 31, 51552 },
			{ 4, 1, 19, 54440 },
			{ 0, 2, 7, 54432 },
			{ 12, 1, 27, 54928 },
			{ 0, 2, 15, 46464 },
			{ 0, 2, 3, 54960 },
			{ 9, 1, 25, 9944 },
			{ 0, 2, 13, 9680 },
			{ 0, 2, 2, 37552 },
			{ 5, 1, 21, 43352 },
			{ 0, 2, 9, 43344 },
			{ 0, 1, 29, 46240 },
			{ 1, 1, 18, 46424 },
			{ 0, 2, 6, 11600 },
			{ 11, 1, 26, 21936 },
			{ 0, 2, 14, 19376 },
			{ 0, 2, 4, 9648 },
			{ 7, 1, 23, 21176 },
			{ 0, 2, 11, 21168 },
			{ 0, 1, 31, 26960 },
			{ 3, 1, 20, 27304 },
			{ 0, 2, 7, 27296 },
			{ 12, 1, 27, 43864 },
			{ 0, 2, 16, 10064 },
			{ 0, 2, 5, 19296 },
			{ 8, 1, 24, 42352 },
			{ 0, 2, 12, 42336 },
			{ 0, 2, 1, 53856 },
			{ 5, 1, 21, 59696 },
			{ 0, 2, 8, 54608 },
			{ 0, 1, 29, 23200 },
			{ 1, 1, 18, 43856 },
			{ 0, 2, 6, 38608 },
			{ 11, 1, 26, 19176 },
			{ 0, 2, 14, 18896 },
			{ 0, 2, 3, 42192 },
			{ 7, 1, 23, 53864 },
			{ 0, 2, 10, 43616 },
			{ 0, 1, 30, 46416 },
			{ 4, 1, 20, 22184 },
			{ 0, 2, 8, 13728 },
			{ 0, 1, 27, 38352 },
			{ 1, 1, 17, 19352 },
			{ 0, 2, 5, 17840 },
			{ 9, 1, 25, 42168 },
			{ 0, 2, 12, 25776 },
			{ 0, 2, 1, 43600 },
			{ 6, 1, 21, 46408 },
			{ 0, 2, 9, 27472 },
			{ 0, 1, 29, 11680 },
			{ 2, 1, 18, 38320 },
			{ 0, 2, 6, 37744 },
			{ 12, 1, 27, 18800 },
			{ 0, 2, 13, 51568 },
			{ 0, 2, 3, 25776 },
			{ 8, 1, 23, 27216 },
			{ 0, 2, 10, 55888 },
			{ 0, 1, 30, 23360 },
			{ 4, 1, 19, 43880 },
			{ 0, 2, 8, 10976 },
			{ 0, 1, 28, 58896 },
			{ 2, 1, 16, 51568 },
			{ 0, 2, 4, 51552 },
			{ 9, 1, 24, 54440 },
			{ 0, 2, 12, 21664 },
			{ 0, 1, 31, 54864 },
			{ 6, 1, 21, 23208 },
			{ 0, 2, 9, 21968 },
			{ 0, 1, 30, 9936 },
			{ 2, 1, 18, 37608 },
			{ 0, 2, 6, 37552 },
			{ 10, 1, 26, 43352 },
			{ 0, 2, 14, 43344 },
			{ 0, 2, 2, 46240 },
			{ 8, 1, 22, 46416 },
			{ 0, 2, 10, 44368 },
			{ 0, 1, 31, 21936 },
			{ 4, 1, 20, 9656 },
			{ 0, 2, 8, 17776 },
			{ 0, 1, 28, 21168 },
			{ 1, 1, 17, 43352 },
			{ 0, 2, 4, 26960 },
			{ 9, 1, 24, 29352 },
			{ 0, 2, 12, 23200 },
			{ 0, 2, 1, 43856 },
			{ 5, 1, 21, 19304 },
			{ 0, 2, 9, 19296 },
			{ 0, 1, 29, 42352 },
			{ 3, 1, 19, 21104 },
			{ 0, 2, 5, 53856 },
			{ 11, 1, 25, 59696 },
			{ 0, 2, 13, 54608 },
			{ 0, 2, 3, 23200 },
			{ 8, 1, 22, 39824 },
			{ 0, 2, 10, 38608 },
			{ 0, 1, 31, 19168 },
			{ 4, 1, 20, 42216 },
			{ 0, 2, 7, 42192 },
			{ 0, 1, 27, 53840 },
			{ 2, 1, 16, 55592 },
			{ 0, 2, 4, 46400 },
			{ 10, 1, 23, 54952 },
			{ 0, 2, 12, 11680 },
			{ 0, 2, 1, 38352 },
			{ 6, 1, 22, 19160 },
			{ 0, 2, 9, 18864 },
			{ 0, 1, 29, 42160 },
			{ 4, 1, 18, 45656 },
			{ 0, 2, 6, 27216 },
			{ 11, 1, 25, 46376 },
			{ 0, 2, 13, 27456 },
			{ 0, 2, 2, 43872 },
			{ 8, 1, 23, 38320 },
			{ 0, 2, 10, 39280 },
			{ 0, 1, 31, 18800 },
			{ 4, 1, 20, 25784 },
			{ 0, 2, 8, 21680 },
			{ 12, 1, 27, 27216 },
			{ 0, 2, 14, 55888 },
			{ 0, 2, 4, 23232 },
			{ 10, 1, 24, 43880 },
			{ 0, 2, 12, 9952 },
			{ 0, 2, 1, 37600 },
			{ 6, 1, 21, 51568 },
			{ 0, 2, 9, 51552 },
			{ 0, 1, 28, 54432 },
			{ 2, 1, 17, 55888 },
			{ 0, 2, 5, 54608 },
			{ 11, 1, 26, 22184 },
			{ 0, 2, 13, 21936 },
			{ 0, 2, 3, 9680 },
			{ 7, 1, 23, 37608 },
			{ 0, 2, 11, 37488 },
			{ 0, 1, 30, 43344 },
			{ 5, 1, 19, 54440 },
			{ 0, 2, 7, 46240 },
			{ 0, 1, 27, 46416 },
			{ 1, 1, 16, 22184 },
			{ 0, 2, 4, 19888 },
			{ 9, 1, 25, 9648 },
			{ 0, 2, 12, 42352 },
			{ 0, 2, 1, 21168 },
			{ 6, 1, 21, 43352 },
			{ 0, 2, 9, 26960 },
			{ 0, 1, 29, 27296 },
			{ 3, 1, 17, 44368 },
			{ 0, 2, 5, 43856 },
			{ 11, 1, 26, 19304 },
			{ 0, 2, 14, 19168 },
			{ 0, 2, 2, 42352 },
			{ 7, 1, 23, 21104 },
			{ 0, 2, 10, 53392 },
			{ 0, 1, 1, 29848 },
			{ 5, 1, 19, 27304 },
			{ 0, 2, 7, 23200 },
			{ 0, 1, 27, 39760 },
			{ 2, 1, 17, 19304 },
			{ 0, 2, 4, 19168 },
			{ 10, 1, 24, 42216 },
			{ 0, 2, 12, 42192 },
			{ 0, 2, 1, 53856 },
			{ 6, 1, 20, 54568 },
			{ 0, 2, 8, 46368 },
			{ 0, 1, 28, 54944 },
			{ 2, 1, 18, 22224 },
			{ 0, 2, 5, 38352 },
			{ 12, 1, 26, 18904 },
			{ 0, 2, 14, 18864 },
			{ 0, 2, 3, 42160 },
			{ 8, 1, 22, 43608 },
			{ 0, 2, 10, 27216 },
			{ 0, 1, 30, 46400 },
			{ 4, 1, 19, 46496 },
			{ 0, 2, 6, 43872 },
			{ 0, 1, 27, 38320 },
			{ 2, 1, 17, 18872 },
			{ 0, 2, 5, 18800 },
			{ 9, 1, 24, 25784 },
			{ 0, 2, 12, 21680 },
			{ 0, 2, 1, 27216 },
			{ 7, 1, 21, 27944 },
			{ 0, 2, 8, 23232 },
			{ 0, 1, 28, 43872 },
			{ 3, 1, 18, 37744 },
			{ 0, 2, 6, 37600 },
			{ 12, 1, 25, 51568 },
			{ 0, 2, 13, 51552 },
			{ 0, 2, 2, 54432 },
			{ 8, 1, 22, 55888 },
			{ 0, 2, 9, 46416 },
			{ 0, 1, 30, 22176 },
			{ 5, 1, 19, 43736 },
			{ 0, 2, 8, 9680 },
			{ 0, 1, 27, 37600 },
			{ 2, 1, 16, 51544 },
			{ 0, 2, 4, 43344 },
			{ 9, 1, 24, 54440 },
			{ 0, 2, 11, 45728 },
			{ 0, 1, 31, 46416 },
			{ 7, 1, 21, 22184 },
			{ 0, 2, 9, 19872 },
			{ 0, 1, 28, 42416 },
			{ 4, 1, 18, 21176 },
			{ 0, 2, 6, 21168 },
			{ 12, 1, 26, 43320 },
			{ 0, 2, 13, 26928 },
			{ 0, 2, 2, 27296 },
			{ 8, 1, 22, 44368 },
			{ 0, 2, 10, 44624 },
			{ 0, 1, 30, 19296 },
			{ 4, 1, 19, 42352 },
			{ 0, 2, 7, 42352 },
			{ 0, 1, 28, 21104 },
			{ 2, 1, 16, 26928 },
			{ 0, 2, 3, 58672 },
			{ 10, 1, 24, 27800 },
			{ 0, 2, 12, 23200 },
			{ 0, 1, 31, 23248 },
			{ 6, 1, 21, 19304 },
			{ 0, 2, 9, 19168 },
			{ 0, 1, 29, 42208 },
			{ 4, 1, 17, 53864 },
			{ 0, 2, 5, 53856 },
			{ 11, 1, 25, 54568 },
			{ 0, 2, 13, 46368 },
			{ 0, 2, 1, 46752 },
			{ 9, 1, 22, 22224 },
			{ 0, 2, 10, 21872 },
			{ 0, 1, 31, 18896 },
			{ 5, 1, 19, 42200 },
			{ 0, 2, 7, 42160 },
			{ 0, 1, 27, 43600 },
			{ 1, 1, 16, 46376 },
			{ 0, 2, 3, 46368 },
			{ 11, 1, 23, 46528 },
			{ 0, 2, 11, 43872 },
			{ 0, 2, 1, 38320 },
			{ 6, 1, 21, 18872 },
			{ 0, 2, 9, 18800 },
			{ 0, 1, 29, 25776 },
			{ 3, 1, 18, 27224 },
			{ 0, 2, 5, 27216 },
			{ 11, 1, 25, 27432 },
			{ 0, 2, 13, 23232 },
			{ 0, 2, 2, 43872 },
			{ 8, 1, 22, 10984 },
			{ 0, 2, 10, 18912 },
			{ 0, 1, 30, 42192 },
			{ 5, 1, 19, 53848 },
			{ 0, 2, 6, 45648 },
			{ 0, 1, 26, 46368 },
			{ 2, 1, 15, 62096 },
			{ 0, 2, 3, 46496 },
			{ 10, 1, 23, 38352 },
			{ 0, 2, 11, 38320 },
			{ 0, 2, 1, 18864 },
			{ 6, 1, 21, 42168 },
			{ 0, 2, 8, 42160 },
			{ 0, 1, 28, 43600 },
			{ 4, 1, 17, 46376 },
			{ 0, 2, 5, 27968 },
			{ 12, 1, 24, 44384 },
			{ 0, 2, 12, 43872 },
			{ 0, 2, 2, 37744 },
			{ 8, 1, 23, 2424 },
			{ 0, 2, 10, 18800 },
			{ 0, 1, 30, 25776 },
			{ 5, 1, 19, 27216 },
			{ 0, 2, 6, 55888 },
			{ 0, 1, 26, 23200 },
			{ 1, 1, 15, 43872 },
			{ 0, 2, 3, 42720 },
			{ 11, 1, 24, 37608 },
			{ 0, 2, 11, 37600 },
			{ 0, 1, 31, 51552 },
			{ 7, 1, 20, 54440 },
			{ 0, 2, 8, 54432 },
			{ 0, 1, 27, 54608 },
			{ 3, 1, 17, 23208 },
			{ 0, 2, 5, 22176 },
			{ 0, 1, 25, 42704 },
			{ 1, 1, 14, 37608 },
			{ 0, 2, 2, 37552 },
			{ 8, 1, 22, 42328 },
			{ 0, 2, 10, 43344 },
			{ 0, 1, 29, 45728 },
			{ 5, 1, 18, 46416 },
			{ 0, 2, 6, 44368 },
			{ 0, 1, 27, 19872 },
			{ 1, 1, 15, 42448 },
			{ 0, 2, 3, 42352 },
			{ 9, 1, 24, 21176 },
			{ 0, 2, 12, 21104 },
			{ 0, 1, 31, 26928 },
			{ 7, 1, 20, 27288 },
			{ 0, 2, 8, 27296 },
			{ 0, 1, 28, 43856 },
			{ 3, 1, 17, 19368 },
			{ 0, 2, 5, 19296 },
			{ 12, 1, 25, 42608 },
			{ 0, 2, 13, 41696 },
			{ 0, 2, 1, 53600 },
			{ 8, 1, 21, 59696 },
			{ 0, 2, 9, 54432 },
			{ 0, 1, 29, 55968 },
			{ 5, 1, 18, 23376 },
			{ 0, 2, 6, 22224 },
			{ 0, 1, 27, 19168 },
			{ 2, 1, 16, 41704 },
			{ 0, 2, 3, 41680 },
			{ 10, 1, 23, 53592 },
			{ 0, 2, 11, 43600 },
			{ 0, 1, 31, 46368 },
			{ 7, 1, 19, 54944 },
			{ 0, 2, 7, 44448 },
			{ 0, 1, 28, 21968 },
			{ 3, 1, 18, 18904 },
			{ 0, 2, 5, 17840 },
			{ 0, 1, 25, 41648 },
			{ 1, 1, 14, 53592 },
			{ 0, 2, 2, 43600 },
			{ 9, 1, 21, 46376 },
			{ 0, 2, 9, 27424 },
			{ 0, 1, 29, 44384 },
			{ 5, 1, 19, 21936 },
			{ 0, 2, 6, 37744 },
			{ 0, 1, 27, 17776 },
			{ 3, 1, 16, 41656 },
			{ 0, 2, 4, 21168 },
			{ 10, 1, 23, 43600 },
			{ 0, 2, 10, 55632 },
			{ 0, 1, 31, 23200 },
			{ 7, 1, 20, 43872 },
			{ 0, 2, 7, 42720 },
			{ 0, 1, 28, 21216 },
			{ 3, 1, 17, 50544 },
			{ 0, 2, 5, 42336 },
			{ 11, 1, 24, 53928 },
			{ 0, 2, 12, 53920 },
			{ 0, 2, 1, 54608 },
			{ 9, 1, 22, 23208 },
			{ 0, 2, 9, 22176 },
			{ 0, 1, 29, 42704 },
			{ 5, 1, 19, 21224 },
			{ 0, 2, 7, 21168 },
			{ 0, 1, 26, 43216 },
			{ 2, 1, 15, 53928 },
			{ 0, 2, 3, 45728 },
			{ 10, 1, 23, 46416 },
			{ 0, 2, 10, 44368 },
			{ 0, 1, 31, 19872 },
			{ 6, 1, 20, 42448 },
			{ 0, 2, 8, 42352 },
			{ 0, 1, 28, 20912 },
			{ 4, 1, 17, 43192 },
			{ 0, 2, 5, 25904 },
			{ 12, 1, 25, 27288 },
			{ 0, 2, 12, 23200 },
			{ 0, 2, 1, 43856 },
			{ 9, 1, 22, 11176 },
			{ 0, 2, 10, 11104 },
			{ 0, 1, 29, 50032 },
			{ 5, 1, 19, 20848 },
			{ 0, 2, 6, 51552 },
			{ 0, 1, 26, 42160 },
			{ 3, 1, 15, 27280 },
			{ 0, 2, 2, 55968 },
			{ 11, 1, 23, 23376 },
			{ 0, 2, 11, 22224 },
			{ 0, 1, 31, 10976 },
			{ 7, 1, 20, 41704 },
			{ 0, 2, 8, 41680 },
			{ 0, 1, 28, 53584 },
			{ 4, 1, 16, 54440 },
			{ 0, 2, 4, 46368 },
			{ 12, 1, 24, 46736 },
			{ 0, 2, 12, 44448 },
			{ 0, 2, 1, 21968 },
			{ 9, 1, 22, 9688 },
			{ 0, 2, 10, 17840 },
			{ 0, 1, 30, 41648 },
			{ 5, 1, 18, 43352 },
			{ 0, 2, 6, 43344 },
			{ 0, 1, 26, 46368 },
			{ 1, 1, 15, 46416 },
			{ 0, 2, 2, 43872 },
			{ 12, 1, 23, 21936 },
			{ 0, 2, 11, 19312 },
			{ 0, 2, 1, 17776 },
			{ 7, 1, 20, 21176 },
			{ 0, 2, 8, 21168 },
			{ 0, 1, 28, 26960 },
			{ 4, 1, 17, 27816 },
			{ 0, 2, 4, 23200 },
			{ 12, 1, 24, 39760 },
			{ 0, 2, 12, 42720 },
			{ 0, 2, 2, 19168 },
			{ 8, 1, 21, 42352 },
			{ 0, 2, 9, 42336 },
			{ 0, 1, 29, 53920 },
			{ 6, 1, 18, 59728 },
			{ 0, 2, 5, 54608 },
			{ 0, 1, 26, 22176 },
			{ 2, 1, 15, 43728 },
			{ 0, 2, 3, 38368 },
			{ 11, 1, 23, 19176 },
			{ 0, 2, 11, 18864 },
			{ 0, 1, 31, 42192 },
			{ 7, 1, 20, 53872 },
			{ 0, 2, 7, 45728 },
			{ 0, 1, 27, 46416 },
			{ 4, 1, 17, 22184 },
			{ 0, 2, 5, 11680 },
			{ 0, 1, 24, 38320 },
			{ 1, 1, 14, 19128 },
			{ 0, 2, 2, 18864 },
			{ 9, 1, 22, 42168 },
			{ 0, 2, 9, 25776 },
			{ 0, 1, 29, 27280 },
			{ 6, 1, 18, 44368 },
			{ 0, 2, 6, 27472 },
			{ 0, 1, 26, 11104 },
			{ 2, 1, 15, 38256 },
			{ 0, 2, 3, 37744 },
			{ 11, 1, 24, 18800 },
			{ 0, 2, 10, 51552 },
			{ 0, 1, 30, 58544 },
			{ 7, 1, 20, 27280 },
			{ 0, 2, 7, 55968 },
			{ 0, 1, 27, 23248 },
			{ 3, 1, 17, 11112 },
			{ 0, 2, 5, 9952 },
			{ 0, 1, 25, 37600 },
			{ 2, 1, 13, 51560 },
			{ 0, 2, 1, 51536 },
			{ 9, 1, 21, 54440 },
			{ 0, 2, 9, 46240 },
			{ 0, 1, 28, 46736 },
			{ 6, 1, 18, 22224 },
			{ 0, 2, 6, 21936 },
			{ 0, 1, 27, 9680 },
			{ 2, 1, 15, 37592 },
			{ 0, 2, 3, 37552 },
			{ 10, 1, 23, 43352 },
			{ 0, 2, 11, 26960 },
			{ 0, 1, 30, 29856 },
			{ 8, 1, 19, 46416 },
			{ 0, 2, 7, 43872 },
			{ 0, 1, 28, 21424 },
			{ 4, 1, 17, 9656 },
			{ 0, 2, 5, 9584 },
			{ 0, 1, 25, 21168 },
			{ 1, 1, 14, 43352 },
			{ 0, 2, 1, 26960 },
			{ 9, 1, 21, 27304 },
			{ 0, 2, 9, 23200 },
			{ 0, 1, 29, 43856 },
			{ 5, 1, 18, 19304 },
			{ 0, 2, 6, 19168 },
			{ 0, 1, 26, 42352 },
			{ 3, 1, 16, 21104 },
			{ 0, 2, 2, 53920 },
			{ 11, 1, 22, 55632 },
			{ 0, 2, 10, 46416 },
			{ 0, 1, 31, 5792 },
			{ 7, 1, 19, 38608 },
			{ 0, 2, 7, 38352 },
			{ 0, 1, 28, 19168 },
			{ 4, 1, 17, 42200 },
			{ 0, 2, 4, 42192 },
			{ 0, 1, 24, 53840 },
			{ 1, 1, 13, 54608 },
			{ 0, 2, 1, 46416 },
			{ 9, 1, 21, 22184 },
			{ 0, 2, 9, 11680 },
			{ 0, 1, 29, 38320 },
			{ 5, 1, 19, 18872 },
			{ 0, 2, 6, 18800 },
			{ 0, 1, 26, 42160 },
			{ 4, 1, 15, 45656 },
			{ 0, 2, 3, 27280 },
			{ 12, 1, 22, 44368 },
			{ 0, 2, 10, 23376 },
			{ 0, 1, 31, 11104 },
			{ 8, 1, 20, 38256 },
			{ 0, 2, 7, 37616 },
			{ 0, 1, 28, 18800 },
			{ 4, 1, 17, 25776 },
			{ 0, 2, 4, 54432 },
			{ 12, 1, 23, 59984 },
			{ 0, 2, 11, 54928 },
			{ 0, 2, 1, 22224 },
			{ 10, 1, 22, 11112 },
			{ 0, 2, 9, 9952 },
			{ 0, 1, 29, 21216 },
			{ 6, 1, 18, 51560 },
			{ 0, 2, 6, 51536 },
			{ 0, 1, 25, 54432 },
			{ 2, 1, 14, 55888 },
			{ 0, 2, 2, 46480 },
			{ 12, 1, 23, 22224 },
			{ 0, 2, 10, 21936 },
			{ 0, 1, 31, 9680 },
			{ 7, 1, 20, 37592 },
			{ 0, 2, 8, 37552 },
			{ 0, 1, 27, 43344 },
			{ 5, 1, 16, 46248 },
			{ 0, 2, 4, 27808 },
			{ 0, 1, 24, 44368 },
			{ 1, 1, 13, 21936 },
			{ 0, 2, 1, 19376 },
			{ 9, 1, 22, 9656 },
			{ 0, 2, 10, 9584 },
			{ 0, 1, 29, 21168 },
			{ 6, 1, 18, 43344 },
			{ 0, 2, 5, 59728 },
			{ 0, 1, 26, 27296 },
			{ 3, 1, 14, 44368 },
			{ 0, 2, 2, 43856 },
			{ 11, 1, 23, 19304 },
			{ 0, 2, 11, 19168 },
			{ 0, 1, 30, 42352 },
			{ 7, 1, 20, 21096 },
			{ 0, 2, 7, 53856 },
			{ 0, 1, 27, 55632 },
			{ 5, 1, 16, 23208 },
			{ 0, 2, 4, 22176 },
			{ 0, 1, 24, 38608 },
			{ 2, 1, 14, 19176 },
			{ 0, 2, 1, 19168 },
			{ 10, 1, 21, 42200 },
			{ 0, 2, 9, 42192 },
			{ 0, 1, 29, 53840 },
			{ 6, 1, 17, 54600 },
			{ 0, 2, 5, 46416 },
			{ 0, 1, 26, 13728 },
			{ 2, 1, 15, 38352 },
			{ 0, 2, 2, 38320 },
			{ 12, 1, 23, 18872 },
			{ 0, 2, 11, 18800 },
			{ 0, 1, 31, 42160 },
			{ 8, 1, 19, 45656 },
			{ 0, 2, 7, 27216 },
			{ 0, 1, 27, 27968 },
			{ 4, 1, 16, 44456 },
			{ 0, 2, 4, 11104 },
			{ 0, 1, 24, 39024 },
			{ 2, 1, 24, 18808 },
			{ 0, 2, 12, 18800 },
			{ 9, 1, 31, 25776 },
			{ 0, 2, 18, 54440 },
			{ 0, 3, 9, 53968 },
			{ 6, 1, 28, 27464 },
			{ 0, 2, 15, 22224 },
			{ 0, 2, 5, 11104 },
			{ 3, 1, 25, 37616 },
			{ 0, 2, 13, 37600 },
			{ 11, 2, 1, 51560 },
			{ 0, 2, 20, 43344 },
			{ 0, 2, 9, 54432 },
			{ 8, 1, 29, 55888 },
			{ 0, 2, 16, 46288 },
			{ 0, 2, 6, 22192 },
			{ 4, 1, 27, 9944 },
			{ 0, 2, 15, 9680 },
			{ 0, 2, 3, 37584 },
			{ 2, 1, 23, 51608 },
			{ 0, 2, 11, 43344 },
			{ 9, 1, 31, 46248 },
			{ 0, 2, 18, 27296 },
			{ 0, 2, 7, 44368 },
			{ 6, 1, 28, 21928 },
			{ 0, 2, 16, 19376 },
			{ 0, 2, 5, 9648 },
			{ 3, 1, 25, 21176 },
			{ 0, 2, 13, 21168 },
			{ 11, 2, 2, 43344 },
			{ 0, 2, 19, 59728 },
			{ 0, 2, 9, 27296 },
			{ 8, 1, 29, 44368 },
			{ 0, 2, 17, 39760 },
			{ 0, 2, 6, 19296 },
			{ 4, 1, 26, 42352 },
			{ 0, 2, 14, 42224 },
			{ 0, 2, 4, 21088 },
			{ 2, 1, 22, 59696 },
			{ 0, 2, 10, 54608 },
			{ 10, 1, 31, 23208 },
			{ 0, 2, 19, 22176 },
			{ 0, 2, 7, 38608 },
			{ 6, 1, 28, 19176 },
			{ 0, 2, 16, 18912 },
			{ 0, 2, 5, 42192 },
			{ 4, 1, 24, 53864 },
			{ 0, 2, 12, 53840 },
			{ 11, 2, 1, 54568 },
			{ 0, 2, 20, 46400 },
			{ 0, 2, 8, 46496 },
			{ 8, 1, 29, 38352 },
			{ 0, 2, 17, 38320 },
			{ 0, 2, 7, 18864 },
			{ 4, 1, 26, 42168 },
			{ 0, 2, 14, 42160 },
			{ 0, 2, 3, 43600 },
			{ 1, 1, 23, 46376 },
			{ 0, 2, 10, 27968 },
			{ 11, 1, 30, 44456 },
			{ 0, 2, 19, 11104 },
			{ 0, 2, 8, 37744 },
			{ 6, 1, 28, 18808 },
			{ 0, 2, 16, 18800 },
			{ 0, 2, 5, 25776 },
			{ 3, 1, 24, 27216 },
			{ 0, 2, 11, 55888 },
			{ 11, 2, 1, 27464 },
			{ 0, 2, 20, 22224 },
			{ 0, 2, 10, 11168 },
			{ 7, 1, 29, 37616 },
			{ 0, 2, 17, 37600 },
			{ 0, 2, 6, 51552 },
			{ 5, 1, 26, 54440 },
			{ 0, 2, 13, 54432 },
			{ 0, 2, 2, 55888 },
			{ 3, 1, 23, 39592 },
			{ 0, 2, 11, 22176 },
			{ 7, 1, 30, 42704 },
			{ 0, 2, 18, 42448 },
			{ 0, 2, 8, 37584 },
			{ 6, 1, 28, 43352 },
			{ 0, 2, 15, 43344 },
			{ 0, 2, 4, 46240 },
			{ 4, 1, 24, 46416 },
			{ 0, 2, 12, 44368 },
			{ 0, 2, 1, 21920 },
			{ 2, 1, 21, 42448 },
			{ 0, 2, 9, 42416 },
			{ 7, 1, 30, 21176 },
			{ 0, 2, 17, 21168 },
			{ 0, 2, 6, 26928 },
			{ 5, 1, 26, 29864 },
			{ 0, 2, 14, 27296 },
			{ 0, 2, 2, 44432 },
			{ 3, 1, 23, 19880 },
			{ 0, 2, 11, 19296 },
			{ 8, 1, 31, 42352 },
			{ 0, 2, 18, 42208 },
			{ 0, 2, 7, 53856 },
			{ 6, 1, 27, 59696 },
			{ 0, 2, 15, 54560 },
			{ 0, 2, 3, 55968 },
			{ 4, 1, 24, 27472 },
			{ 0, 2, 12, 22224 },
			{ 0, 2, 2, 19168 },
			{ 3, 1, 21, 42216 },
			{ 0, 2, 9, 42192 },
			{ 7, 1, 29, 53848 },
			{ 0, 2, 17, 45136 },
			{ 0, 2, 5, 54560 },
			{ 5, 1, 25, 54944 },
			{ 0, 2, 13, 46496 },
			{ 0, 2, 3, 21968 },
			{ 3, 1, 23, 19160 },
			{ 0, 2, 11, 18896 },
			{ 7, 1, 31, 42168 },
			{ 0, 2, 19, 42160 },
			{ 0, 2, 8, 43600 },
			{ 6, 1, 28, 46376 },
			{ 0, 2, 16, 27936 },
			{ 0, 2, 5, 44448 },
			{ 4, 1, 25, 21936 },
			{ 0, 2, 13, 37744 },
			{ 0, 2, 3, 18800 },
			{ 3, 1, 23, 25784 },
			{ 0, 2, 10, 42192 },
			{ 7, 1, 30, 27216 },
			{ 0, 2, 17, 55888 },
			{ 0, 2, 7, 23200 },
			{ 5, 1, 26, 43872 },
			{ 0, 2, 14, 43744 },
			{ 0, 2, 4, 37600 },
			{ 3, 1, 24, 51568 },
			{ 0, 2, 11, 51552 },
			{ 8, 1, 31, 54440 },
			{ 0, 2, 19, 54432 },
			{ 0, 2, 8, 54608 },
			{ 6, 1, 28, 23208 },
			{ 0, 2, 16, 22176 },
			{ 0, 2, 5, 42704 },
			{ 4, 1, 26, 21224 },
			{ 0, 2, 13, 21200 },
			{ 0, 2, 2, 43344 },
			{ 3, 1, 22, 58536 },
			{ 0, 2, 10, 46240 },
			{ 7, 1, 29, 46416 },
			{ 0, 2, 17, 40272 },
			{ 0, 2, 7, 21920 },
			{ 5, 1, 27, 42448 },
			{ 0, 2, 14, 42416 },
			{ 0, 2, 4, 21168 },
			{ 4, 1, 24, 43192 },
			{ 0, 2, 12, 26928 },
			{ 9, 1, 31, 27288 },
			{ 0, 2, 19, 27296 },
			{ 0, 2, 8, 43856 },
			{ 6, 1, 29, 19880 },
			{ 0, 2, 16, 19296 },
			{ 0, 2, 5, 42352 },
			{ 4, 1, 26, 20848 },
			{ 0, 2, 13, 53600 },
			{ 0, 2, 1, 59696 },
			{ 3, 1, 22, 27280 },
			{ 0, 2, 9, 55968 },
			{ 7, 1, 30, 23376 },
			{ 0, 2, 17, 22224 },
			{ 0, 2, 7, 19168 },
			{ 5, 1, 27, 42200 },
			{ 0, 2, 15, 41680 },
			{ 0, 2, 3, 53592 },
			{ 4, 2, 22, 43600 },
			{ 0, 2, 11, 46368 },
			{ 9, 1, 31, 54928 },
			{ 0, 2, 18, 44448 },
			{ 0, 2, 8, 21968 },
			{ 6, 1, 29, 10968 },
			{ 0, 2, 17, 17840 },
			{ 0, 2, 5, 41648 },
			{ 5, 1, 25, 45400 },
			{ 0, 2, 13, 43344 },
			{ 0, 2, 2, 46368 },
			{ 2, 1, 21, 46480 },
			{ 0, 2, 9, 44384 },
			{ 7, 1, 30, 21936 },
			{ 0, 2, 18, 21360 },
			{ 0, 2, 7, 17776 },
			{ 5, 1, 27, 25272 },
			{ 0, 2, 15, 21168 },
			{ 0, 2, 4, 26960 },
			{ 3, 1, 23, 27816 },
			{ 0, 2, 11, 23200 },
			{ 10, 1, 31, 43856 },
			{ 0, 2, 19, 42704 },
			{ 0, 2, 8, 19168 },
			{ 6, 1, 28, 38256 },
			{ 0, 2, 16, 42336 },
			{ 0, 2, 5, 53920 },
			{ 5, 1, 24, 59728 },
			{ 0, 2, 12, 54608 },
			{ 0, 2, 2, 23200 },
			{ 3, 1, 22, 43856 },
			{ 0, 2, 9, 42704 },
			{ 7, 1, 30, 19176 },
			{ 0, 2, 18, 19120 },
			{ 0, 2, 7, 43216 },
			{ 5, 1, 26, 53928 },
			{ 0, 2, 14, 45728 },
			{ 0, 2, 3, 46416 },
			{ 4, 1, 24, 22184 },
			{ 0, 2, 11, 19872 },
			{ 0, 1, 31, 38352 },
			{ 2, 1, 21, 19128 },
			{ 0, 2, 9, 18864 },
			{ 6, 1, 28, 43192 },
			{ 0, 2, 16, 25776 },
			{ 0, 2, 5, 27280 },
			{ 4, 1, 25, 46416 },
			{ 0, 2, 13, 27472 },
			{ 0, 2, 3, 11168 },
			{ 2, 1, 23, 38320 },
			{ 0, 2, 11, 37744 },
			{ 6, 1, 31, 20848 },
			{ 0, 2, 18, 53600 },
			{ 0, 2, 7, 58544 },
			{ 5, 1, 28, 27280 },
			{ 0, 2, 14, 55952 },
			{ 0, 2, 4, 23376 },
			{ 3, 1, 25, 11112 },
			{ 0, 2, 13, 10976 },
			{ 0, 2, 1, 41696 },
			{ 2, 1, 21, 53608 },
			{ 0, 2, 9, 51536 },
			{ 6, 1, 29, 54440 },
			{ 0, 2, 16, 46368 },
			{ 0, 2, 5, 46736 },
			{ 4, 1, 26, 22224 },
			{ 0, 2, 14, 21968 },
			{ 0, 2, 3, 9680 },
			{ 3, 1, 23, 41688 },
			{ 0, 2, 11, 41648 },
			{ 7, 1, 31, 43352 },
			{ 0, 2, 18, 43344 },
			{ 0, 2, 7, 46240 },
			{ 5, 1, 27, 46416 },
			{ 0, 2, 15, 44368 },
			{ 0, 2, 4, 21936 },
			{ 4, 1, 25, 9656 },
			{ 0, 2, 13, 9584 },
			{ 9, 2, 2, 21176 },
			{ 0, 2, 20, 21168 },
			{ 0, 2, 9, 26960 },
			{ 6, 1, 29, 27816 },
			{ 0, 2, 17, 23200 },
			{ 0, 2, 5, 43856 },
			{ 4, 1, 26, 21352 },
			{ 0, 2, 14, 19168 },
			{ 0, 2, 3, 42352 },
			{ 3, 1, 23, 21168 },
			{ 0, 2, 10, 53920 },
			{ 7, 1, 30, 59728 },
			{ 0, 2, 18, 54608 },
			{ 0, 2, 7, 23200 },
			{ 5, 1, 27, 43728 },
			{ 0, 2, 15, 38352 },
			{ 0, 2, 5, 19168 },
			{ 4, 1, 24, 42328 },
			{ 0, 2, 12, 42192 },
			{ 8, 2, 1, 53848 },
			{ 0, 2, 20, 45712 },
			{ 0, 2, 8, 46416 },
			{ 7, 1, 29, 22184 },
			{ 0, 2, 17, 11680 },
			{ 0, 2, 6, 38352 },
			{ 5, 1, 26, 19128 },
			{ 0, 2, 14, 18864 },
			{ 0, 2, 3, 42160 },
			{ 3, 1, 23, 45656 },
			{ 0, 2, 10, 27280 },
			{ 8, 1, 30, 44360 },
			{ 0, 2, 18, 27472 },
			{ 0, 2, 8, 11104 },
			{ 5, 1, 27, 38320 },
			{ 0, 2, 15, 37744 },
			{ 0, 2, 5, 18800 },
			{ 4, 1, 25, 25776 },
			{ 0, 2, 11, 58528 },
			{ 10, 1, 31, 59984 },
			{ 0, 2, 19, 55952 },
			{ 0, 2, 9, 23248 },
			{ 6, 1, 29, 11112 },
			{ 0, 2, 17, 10976 },
			{ 0, 2, 6, 37600 },
			{ 5, 1, 26, 51560 },
			{ 0, 2, 13, 51536 },
			{ 0, 2, 2, 54432 },
			{ 3, 1, 22, 55888 },
			{ 0, 2, 10, 46736 },
			{ 7, 1, 30, 22224 },
			{ 0, 2, 18, 21936 },
			{ 0, 2, 8, 9680 },
			{ 5, 1, 28, 37592 },
			{ 0, 2, 15, 37552 },
			{ 0, 2, 4, 43344 },
			{ 4, 1, 24, 54440 },
			{ 0, 2, 12, 46240 },
			{ 0, 1, 31, 46416 },
			{ 2, 1, 21, 22184 },
			{ 0, 2, 9, 21936 },
			{ 6, 1, 30, 9656 },
			{ 0, 2, 17, 9584 },
			{ 0, 2, 6, 21168 },
			{ 5, 1, 26, 43344 },
			{ 0, 2, 13, 59728 },
			{ 0, 2, 2, 27296 },
			{ 3, 1, 22, 44368 },
			{ 0, 2, 10, 43856 },
			{ 8, 1, 31, 19304 },
			{ 0, 2, 19, 19168 },
			{ 0, 2, 8, 42352 },
			{ 5, 1, 29, 21096 },
			{ 0, 2, 16, 53856 },
			{ 0, 2, 4, 55632 },
			{ 4, 1, 25, 27304 },
			{ 0, 2, 13, 22176 },
			{ 0, 2, 2, 39632 },
			{ 2, 1, 22, 19176 },
			{ 0, 2, 10, 19168 },
			{ 6, 1, 30, 42200 },
			{ 0, 2, 18, 42192 },
			{ 0, 2, 6, 53840 },
			{ 5, 1, 26, 55624 },
			{ 0, 2, 14, 46416 },
			{ 0, 2, 4, 22176 },
			{ 2, 1, 23, 38608 },
			{ 0, 2, 11, 38352 },
			{ 7, 2, 1, 19160 },
			{ 0, 2, 20, 18864 },
			{ 0, 2, 8, 42160 },
			{ 5, 1, 28, 45656 },
			{ 0, 2, 16, 27280 },
			{ 0, 2, 5, 44352 },
			{ 4, 1, 24, 46504 },
			{ 0, 2, 13, 11104 },
			{ 0, 2, 2, 38320 },
			{ 2, 1, 23, 18872 },
			{ 0, 2, 10, 18800 },
			{ 6, 1, 30, 25776 },
			{ 0, 2, 17, 58528 },
			{ 0, 2, 6, 59984 },
			{ 5, 1, 26, 27976 },
			{ 0, 2, 14, 23376 },
			{ 0, 2, 4, 11104 },
			{ 3, 1, 24, 38256 },
			{ 0, 2, 11, 37600 },
			{ 7, 1, 31, 51560 },
			{ 0, 2, 19, 51536 },
			{ 0, 2, 8, 54432 },
			{ 6, 1, 27, 55888 },
			{ 0, 2, 15, 46736 },
			{ 0, 2, 5, 22224 },
			{ 4, 1, 26, 10968 },
			{ 0, 2, 13, 9680 },
			{ 0, 2, 2, 37584 },
			{ 2, 1, 22, 51544 },
			{ 0, 2, 10, 43344 },
			{ 7, 1, 29, 54440 },
			{ 0, 2, 17, 46240 },
			{ 0, 2, 6, 46416 },
			{ 5, 1, 27, 22184 },
			{ 0, 2, 14, 19888 },
			{ 0, 2, 4, 9648 },
			{ 3, 1, 24, 37560 },
			{ 0, 2, 12, 21168 },
			{ 8, 1, 31, 43352 },
			{ 0, 2, 19, 26960 },
			{ 0, 2, 8, 27296 },
			{ 6, 1, 28, 44368 },
			{ 0, 2, 15, 43856 },
			{ 0, 2, 5, 19296 },
			{ 4, 1, 25, 42352 },
			{ 0, 2, 13, 42352 },
			{ 0, 2, 2, 21104 },
			{ 3, 1, 22, 26928 },
			{ 0, 2, 9, 55632 },
			{ 7, 1, 30, 27304 },
			{ 0, 2, 17, 22176 },
			{ 0, 2, 6, 39632 },
			{ 5, 1, 27, 19176 },
			{ 0, 2, 15, 19168 },
			{ 0, 2, 3, 42208 },
			{ 4, 1, 23, 53864 },
			{ 0, 2, 11, 53840 },
			{ 8, 1, 31, 54600 },
			{ 0, 2, 18, 46400 },
			{ 0, 2, 7, 54944 },
			{ 6, 1, 28, 38608 },
			{ 0, 2, 16, 38320 },
			{ 0, 2, 5, 18864 },
			{ 4, 1, 25, 42200 },
			{ 0, 2, 13, 42160 },
			{ 10, 2, 2, 45656 },
			{ 0, 2, 20, 27216 },
			{ 0, 2, 9, 27968 },
			{ 6, 1, 29, 46504 },
			{ 0, 2, 18, 11104 },
			{ 0, 2, 6, 38320 },
			{ 5, 1, 27, 18872 },
			{ 0, 2, 15, 18800 },
			{ 0, 2, 4, 25776 },
			{ 3, 1, 23, 27216 },
			{ 0, 2, 10, 59984 },
			{ 8, 1, 31, 27976 },
			{ 0, 2, 19, 23248 },
			{ 0, 2, 8, 11104 },
			{ 5, 1, 28, 37744 },
			{ 0, 2, 16, 37600 },
			{ 0, 2, 5, 51552 },
			{ 4, 1, 24, 58536 },
			{ 0, 2, 12, 54432 },
			{ 0, 2, 1, 55888 },
			{ 2, 1, 22, 23208 },
			{ 0, 2, 9, 22208 },
			{ 7, 1, 29, 43736 },
			{ 0, 2, 18, 9680 },
			{ 0, 2, 7, 37584 },
			{ 5, 1, 26, 51544 },
			{ 0, 2, 14, 43344 },
			{ 0, 2, 3, 46240 },
			{ 3, 1, 23, 47696 },
			{ 0, 2, 10, 46416 },
			{ 9, 1, 31, 21928 },
			{ 0, 2, 19, 19360 },
			{ 0, 2, 8, 42416 },
			{ 5, 1, 28, 21176 },
			{ 0, 2, 16, 21168 },
			{ 0, 2, 5, 43344 },
			{ 4, 1, 25, 46248 },
			{ 0, 2, 12, 27296 },
			{ 0, 2, 1, 44368 },
			{ 2, 1, 22, 21928 },
			{ 0, 2, 10, 19296 },
			{ 6, 1, 29, 42352 },
			{ 0, 2, 17, 42352 },
			{ 0, 2, 7, 21104 },
			{ 5, 1, 27, 26928 },
			{ 0, 2, 13, 55600 },
			{ 0, 2, 3, 23200 },
			{ 3, 1, 23, 43856 },
			{ 0, 2, 11, 38608 },
			{ 11, 1, 31, 19176 },
			{ 0, 2, 19, 19168 },
			{ 0, 2, 8, 42192 },
			{ 6, 1, 28, 53864 },
			{ 0, 2, 15, 53840 },
			{ 0, 2, 4, 54560 },
			{ 5, 1, 24, 55968 },
			{ 0, 2, 12, 46752 },
			{ 0, 2, 1, 38608 },
			{ 2, 1, 22, 19160 },
			{ 0, 2, 10, 18864 },
			{ 7, 1, 30, 42168 },
			{ 0, 2, 17, 42160 },
			{ 0, 2, 6, 45648 },
			{ 5, 1, 26, 46376 },
			{ 0, 2, 14, 27968 },
			{ 0, 2, 2, 44448 },
			{ 3, 1, 23, 38320 }
		};
	}
}
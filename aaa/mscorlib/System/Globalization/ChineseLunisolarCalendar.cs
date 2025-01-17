﻿using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200037B RID: 891
	[Serializable]
	public class ChineseLunisolarCalendar : EastAsianLunisolarCalendar
	{
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002385 RID: 9093 RVA: 0x0005AB78 File Offset: 0x00059B78
		[ComVisible(false)]
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return ChineseLunisolarCalendar.minDate;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x0005AB7F File Offset: 0x00059B7F
		[ComVisible(false)]
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return ChineseLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x0005AB86 File Offset: 0x00059B86
		internal override int MinCalendarYear
		{
			get
			{
				return 1901;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06002388 RID: 9096 RVA: 0x0005AB8D File Offset: 0x00059B8D
		internal override int MaxCalendarYear
		{
			get
			{
				return 2100;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x0005AB94 File Offset: 0x00059B94
		internal override DateTime MinDate
		{
			get
			{
				return ChineseLunisolarCalendar.minDate;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x0005AB9B File Offset: 0x00059B9B
		internal override DateTime MaxDate
		{
			get
			{
				return ChineseLunisolarCalendar.maxDate;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x0600238B RID: 9099 RVA: 0x0005ABA2 File Offset: 0x00059BA2
		internal override EraInfo[] CalEraInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x0005ABA8 File Offset: 0x00059BA8
		internal override int GetYearInfo(int LunarYear, int Index)
		{
			if (LunarYear < 1901 || LunarYear > 2100)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1901, 2100 }));
			}
			return ChineseLunisolarCalendar.yinfo[LunarYear - 1901, Index];
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x0005AC18 File Offset: 0x00059C18
		internal override int GetYear(int year, DateTime time)
		{
			return year;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0005AC1C File Offset: 0x00059C1C
		internal override int GetGregorianYear(int year, int era)
		{
			if (era != 0 && era != 1)
			{
				throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("ArgumentOutOfRange_InvalidEraValue"));
			}
			if (year < 1901 || year > 2100)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Range"), new object[] { 1901, 2100 }));
			}
			return year;
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x0005AC9F File Offset: 0x00059C9F
		[ComVisible(false)]
		public override int GetEra(DateTime time)
		{
			base.CheckTicksRange(time.Ticks);
			return 1;
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0005ACAF File Offset: 0x00059CAF
		internal override int ID
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x0005ACB3 File Offset: 0x00059CB3
		internal override int BaseCalendarID
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x0005ACB8 File Offset: 0x00059CB8
		[ComVisible(false)]
		public override int[] Eras
		{
			get
			{
				return new int[] { 1 };
			}
		}

		// Token: 0x04000EF7 RID: 3831
		public const int ChineseEra = 1;

		// Token: 0x04000EF8 RID: 3832
		internal const int MIN_LUNISOLAR_YEAR = 1901;

		// Token: 0x04000EF9 RID: 3833
		internal const int MAX_LUNISOLAR_YEAR = 2100;

		// Token: 0x04000EFA RID: 3834
		internal const int MIN_GREGORIAN_YEAR = 1901;

		// Token: 0x04000EFB RID: 3835
		internal const int MIN_GREGORIAN_MONTH = 2;

		// Token: 0x04000EFC RID: 3836
		internal const int MIN_GREGORIAN_DAY = 19;

		// Token: 0x04000EFD RID: 3837
		internal const int MAX_GREGORIAN_YEAR = 2101;

		// Token: 0x04000EFE RID: 3838
		internal const int MAX_GREGORIAN_MONTH = 1;

		// Token: 0x04000EFF RID: 3839
		internal const int MAX_GREGORIAN_DAY = 28;

		// Token: 0x04000F00 RID: 3840
		internal static DateTime minDate = new DateTime(1901, 2, 19);

		// Token: 0x04000F01 RID: 3841
		internal static DateTime maxDate = new DateTime(new DateTime(2101, 1, 28, 23, 59, 59, 999).Ticks + 9999L);

		// Token: 0x04000F02 RID: 3842
		private static readonly int[,] yinfo = new int[,]
		{
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
			{ 5, 1, 26, 54568 },
			{ 0, 2, 14, 46400 },
			{ 0, 2, 3, 54944 },
			{ 2, 1, 23, 38608 },
			{ 0, 2, 11, 38320 },
			{ 7, 2, 1, 18872 },
			{ 0, 2, 20, 18800 },
			{ 0, 2, 8, 42160 },
			{ 5, 1, 28, 45656 },
			{ 0, 2, 16, 27216 },
			{ 0, 2, 5, 27968 },
			{ 4, 1, 24, 44456 },
			{ 0, 2, 13, 11104 },
			{ 0, 2, 2, 38256 },
			{ 2, 1, 23, 18808 },
			{ 0, 2, 10, 18800 },
			{ 6, 1, 30, 25776 },
			{ 0, 2, 17, 54432 },
			{ 0, 2, 6, 59984 },
			{ 5, 1, 26, 27976 },
			{ 0, 2, 14, 23248 },
			{ 0, 2, 4, 11104 },
			{ 3, 1, 24, 37744 },
			{ 0, 2, 11, 37600 },
			{ 7, 1, 31, 51560 },
			{ 0, 2, 19, 51536 },
			{ 0, 2, 8, 54432 },
			{ 6, 1, 27, 55888 },
			{ 0, 2, 15, 46416 },
			{ 0, 2, 5, 22176 },
			{ 4, 1, 25, 43736 },
			{ 0, 2, 13, 9680 },
			{ 0, 2, 2, 37584 },
			{ 2, 1, 22, 51544 },
			{ 0, 2, 10, 43344 },
			{ 7, 1, 29, 46248 },
			{ 0, 2, 17, 27808 },
			{ 0, 2, 6, 46416 },
			{ 5, 1, 27, 21928 },
			{ 0, 2, 14, 19872 },
			{ 0, 2, 3, 42416 },
			{ 3, 1, 24, 21176 },
			{ 0, 2, 12, 21168 },
			{ 8, 1, 31, 43344 },
			{ 0, 2, 18, 59728 },
			{ 0, 2, 8, 27296 },
			{ 6, 1, 28, 44368 },
			{ 0, 2, 15, 43856 },
			{ 0, 2, 5, 19296 },
			{ 4, 1, 25, 42352 },
			{ 0, 2, 13, 42352 },
			{ 0, 2, 2, 21088 },
			{ 3, 1, 21, 59696 },
			{ 0, 2, 9, 55632 },
			{ 7, 1, 30, 23208 },
			{ 0, 2, 17, 22176 },
			{ 0, 2, 6, 38608 },
			{ 5, 1, 27, 19176 },
			{ 0, 2, 15, 19152 },
			{ 0, 2, 3, 42192 },
			{ 4, 1, 23, 53864 },
			{ 0, 2, 11, 53840 },
			{ 8, 1, 31, 54568 },
			{ 0, 2, 18, 46400 },
			{ 0, 2, 7, 46752 },
			{ 6, 1, 28, 38608 },
			{ 0, 2, 16, 38320 },
			{ 0, 2, 5, 18864 },
			{ 4, 1, 25, 42168 },
			{ 0, 2, 13, 42160 },
			{ 10, 2, 2, 45656 },
			{ 0, 2, 20, 27216 },
			{ 0, 2, 9, 27968 },
			{ 6, 1, 29, 44448 },
			{ 0, 2, 17, 43872 },
			{ 0, 2, 6, 38256 },
			{ 5, 1, 27, 18808 },
			{ 0, 2, 15, 18800 },
			{ 0, 2, 4, 25776 },
			{ 3, 1, 23, 27216 },
			{ 0, 2, 10, 59984 },
			{ 8, 1, 31, 27432 },
			{ 0, 2, 19, 23232 },
			{ 0, 2, 7, 43872 },
			{ 5, 1, 28, 37736 },
			{ 0, 2, 16, 37600 },
			{ 0, 2, 5, 51552 },
			{ 4, 1, 24, 54440 },
			{ 0, 2, 12, 54432 },
			{ 0, 2, 1, 55888 },
			{ 2, 1, 22, 23208 },
			{ 0, 2, 9, 22176 },
			{ 7, 1, 29, 43736 },
			{ 0, 2, 18, 9680 },
			{ 0, 2, 7, 37584 },
			{ 5, 1, 26, 51544 },
			{ 0, 2, 14, 43344 },
			{ 0, 2, 3, 46240 },
			{ 4, 1, 23, 46416 },
			{ 0, 2, 10, 44368 },
			{ 9, 1, 31, 21928 },
			{ 0, 2, 19, 19360 },
			{ 0, 2, 8, 42416 },
			{ 6, 1, 28, 21176 },
			{ 0, 2, 16, 21168 },
			{ 0, 2, 5, 43312 },
			{ 4, 1, 25, 29864 },
			{ 0, 2, 12, 27296 },
			{ 0, 2, 1, 44368 },
			{ 2, 1, 22, 19880 },
			{ 0, 2, 10, 19296 },
			{ 6, 1, 29, 42352 },
			{ 0, 2, 17, 42208 },
			{ 0, 2, 6, 53856 },
			{ 5, 1, 26, 59696 },
			{ 0, 2, 13, 54576 },
			{ 0, 2, 3, 23200 },
			{ 3, 1, 23, 27472 },
			{ 0, 2, 11, 38608 },
			{ 11, 1, 31, 19176 },
			{ 0, 2, 19, 19152 },
			{ 0, 2, 8, 42192 },
			{ 6, 1, 28, 53848 },
			{ 0, 2, 15, 53840 },
			{ 0, 2, 4, 54560 },
			{ 5, 1, 24, 55968 },
			{ 0, 2, 12, 46496 },
			{ 0, 2, 1, 22224 },
			{ 2, 1, 22, 19160 },
			{ 0, 2, 10, 18864 },
			{ 7, 1, 30, 42168 },
			{ 0, 2, 17, 42160 },
			{ 0, 2, 6, 43600 },
			{ 5, 1, 26, 46376 },
			{ 0, 2, 14, 27936 },
			{ 0, 2, 2, 44448 },
			{ 3, 1, 23, 21936 },
			{ 0, 2, 11, 37744 },
			{ 8, 2, 1, 18808 },
			{ 0, 2, 19, 18800 },
			{ 0, 2, 8, 25776 },
			{ 6, 1, 28, 27216 },
			{ 0, 2, 15, 59984 },
			{ 0, 2, 4, 27424 },
			{ 4, 1, 24, 43872 },
			{ 0, 2, 12, 43744 },
			{ 0, 2, 2, 37600 },
			{ 3, 1, 21, 51568 },
			{ 0, 2, 9, 51552 },
			{ 7, 1, 29, 54440 },
			{ 0, 2, 17, 54432 },
			{ 0, 2, 5, 55888 },
			{ 5, 1, 26, 23208 },
			{ 0, 2, 14, 22176 },
			{ 0, 2, 3, 42704 },
			{ 4, 1, 23, 21224 },
			{ 0, 2, 11, 21200 },
			{ 8, 1, 31, 43352 },
			{ 0, 2, 19, 43344 },
			{ 0, 2, 7, 46240 },
			{ 6, 1, 27, 46416 },
			{ 0, 2, 15, 44368 },
			{ 0, 2, 5, 21920 },
			{ 4, 1, 24, 42448 },
			{ 0, 2, 12, 42416 },
			{ 0, 2, 2, 21168 },
			{ 3, 1, 22, 43320 },
			{ 0, 2, 9, 26928 },
			{ 7, 1, 29, 29336 },
			{ 0, 2, 17, 27296 },
			{ 0, 2, 6, 44368 },
			{ 5, 1, 26, 19880 },
			{ 0, 2, 14, 19296 },
			{ 0, 2, 3, 42352 },
			{ 4, 1, 24, 21104 },
			{ 0, 2, 10, 53856 },
			{ 8, 1, 30, 59696 },
			{ 0, 2, 18, 54560 },
			{ 0, 2, 7, 55968 },
			{ 6, 1, 27, 27472 },
			{ 0, 2, 15, 22224 },
			{ 0, 2, 5, 19168 },
			{ 4, 1, 25, 42216 },
			{ 0, 2, 12, 42192 },
			{ 0, 2, 1, 53584 },
			{ 2, 1, 21, 55592 },
			{ 0, 2, 9, 54560 }
		};
	}
}

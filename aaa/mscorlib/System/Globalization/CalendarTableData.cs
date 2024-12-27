using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C3 RID: 963
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CalendarTableData
	{
		// Token: 0x040012C2 RID: 4802
		internal const int sizeofDataFields = 72;

		// Token: 0x040012C3 RID: 4803
		internal ushort iCalendar;

		// Token: 0x040012C4 RID: 4804
		internal ushort iTwoDigitYearMax;

		// Token: 0x040012C5 RID: 4805
		internal uint saShortDate;

		// Token: 0x040012C6 RID: 4806
		internal uint saYearMonth;

		// Token: 0x040012C7 RID: 4807
		internal uint saLongDate;

		// Token: 0x040012C8 RID: 4808
		internal uint saEraNames;

		// Token: 0x040012C9 RID: 4809
		internal uint waaEraRanges;

		// Token: 0x040012CA RID: 4810
		internal uint saDayNames;

		// Token: 0x040012CB RID: 4811
		internal uint saAbbrevDayNames;

		// Token: 0x040012CC RID: 4812
		internal uint saMonthNames;

		// Token: 0x040012CD RID: 4813
		internal uint saAbbrevMonthNames;

		// Token: 0x040012CE RID: 4814
		internal ushort iCurrentEra;

		// Token: 0x040012CF RID: 4815
		internal ushort iFormatFlags;

		// Token: 0x040012D0 RID: 4816
		internal uint sName;

		// Token: 0x040012D1 RID: 4817
		internal uint sMonthDay;

		// Token: 0x040012D2 RID: 4818
		internal uint saAbbrevEraNames;

		// Token: 0x040012D3 RID: 4819
		internal uint saAbbrevEnglishEraNames;

		// Token: 0x040012D4 RID: 4820
		internal uint saLeapYearMonthNames;

		// Token: 0x040012D5 RID: 4821
		internal uint saSuperShortDayNames;

		// Token: 0x040012D6 RID: 4822
		internal ushort _padding1;

		// Token: 0x040012D7 RID: 4823
		internal ushort _padding2;
	}
}

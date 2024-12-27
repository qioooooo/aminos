using System;
using System.Runtime.CompilerServices;

namespace System.Globalization
{
	// Token: 0x02000374 RID: 884
	internal class CalendarTable : BaseInfoTable
	{
		// Token: 0x0600232B RID: 9003 RVA: 0x00059823 File Offset: 0x00058823
		internal CalendarTable(string fileName, bool fromAssembly)
			: base(fileName, fromAssembly)
		{
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x00059830 File Offset: 0x00058830
		internal unsafe override void SetDataItemPointers()
		{
			this.m_itemSize = (uint)this.m_pCultureHeader->sizeCalendarItem;
			this.m_numItem = (uint)this.m_pCultureHeader->numCalendarItems;
			this.m_pDataPool = (ushort*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToDataPool);
			this.m_pItemData = this.m_pDataFileStart + this.m_pCultureHeader->offsetToCalendarItemData - this.m_itemSize;
			this.m_calendars = (CalendarTableData*)(this.m_pDataFileStart + this.m_pCultureHeader->offsetToCalendarItemData - sizeof(CalendarTableData));
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600232D RID: 9005 RVA: 0x000598B5 File Offset: 0x000588B5
		internal static CalendarTable Default
		{
			get
			{
				return CalendarTable.m_defaultInstance;
			}
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000598BC File Offset: 0x000588BC
		internal unsafe int ICURRENTERA(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.CurrentEra(id);
			}
			return (int)this.m_calendars[id].iCurrentEra;
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x000598E2 File Offset: 0x000588E2
		internal unsafe int IFORMATFLAGS(int id)
		{
			return (int)this.m_calendars[id].iFormatFlags;
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x000598F9 File Offset: 0x000588F9
		internal unsafe string[] SDAYNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saDayNames);
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x00059916 File Offset: 0x00058916
		internal unsafe string[] SABBREVDAYNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saAbbrevDayNames);
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x00059933 File Offset: 0x00058933
		internal unsafe string[] SSUPERSHORTDAYNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saSuperShortDayNames);
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x00059950 File Offset: 0x00058950
		internal unsafe string[] SMONTHNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saMonthNames);
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0005996D File Offset: 0x0005896D
		internal unsafe string[] SABBREVMONTHNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saAbbrevMonthNames);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0005998A File Offset: 0x0005898A
		internal unsafe string[] SLEAPYEARMONTHNAMES(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saLeapYearMonthNames);
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000599A7 File Offset: 0x000589A7
		internal unsafe string[] SSHORTDATE(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saShortDate);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000599C4 File Offset: 0x000589C4
		internal unsafe string[] SLONGDATE(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saLongDate);
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000599E1 File Offset: 0x000589E1
		internal unsafe string[] SYEARMONTH(int id)
		{
			return base.GetStringArray(this.m_calendars[id].saYearMonth);
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000599FE File Offset: 0x000589FE
		internal unsafe string SMONTHDAY(int id)
		{
			return base.GetStringPoolString(this.m_calendars[id].sMonthDay);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x00059A1B File Offset: 0x00058A1B
		internal unsafe int[][] SERARANGES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.EraRanges(id);
			}
			return base.GetWordArrayArray(this.m_calendars[id].waaEraRanges);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x00059A47 File Offset: 0x00058A47
		internal unsafe string[] SERANAMES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.EraNames(id);
			}
			return base.GetStringArray(this.m_calendars[id].saEraNames);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x00059A73 File Offset: 0x00058A73
		internal unsafe string[] SABBREVERANAMES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.AbbrevEraNames(id);
			}
			return base.GetStringArray(this.m_calendars[id].saAbbrevEraNames);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x00059A9F File Offset: 0x00058A9F
		internal unsafe string[] SABBREVENGERANAMES(int id)
		{
			if (JapaneseCalendarTable.IsJapaneseCalendar(id))
			{
				return JapaneseCalendarTable.EnglishEraNames(id);
			}
			return base.GetStringArray(this.m_calendars[id].saAbbrevEnglishEraNames);
		}

		// Token: 0x0600233E RID: 9022
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string nativeGetEraName(int culture, int calID);

		// Token: 0x04000EC8 RID: 3784
		private static CalendarTable m_defaultInstance = new CalendarTable("culture.nlp", true);

		// Token: 0x04000EC9 RID: 3785
		private unsafe CalendarTableData* m_calendars;
	}
}

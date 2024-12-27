using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CultureTableData
	{
		// Token: 0x0400006F RID: 111
		internal const int sizeofDataFields = 304;

		// Token: 0x04000070 RID: 112
		internal const int LOCALE_IDIGITS = 17;

		// Token: 0x04000071 RID: 113
		internal const int LOCALE_INEGNUMBER = 4112;

		// Token: 0x04000072 RID: 114
		internal const int LOCALE_ICURRDIGITS = 25;

		// Token: 0x04000073 RID: 115
		internal const int LOCALE_ICURRENCY = 27;

		// Token: 0x04000074 RID: 116
		internal const int LOCALE_INEGCURR = 28;

		// Token: 0x04000075 RID: 117
		internal const int LOCALE_ILZERO = 18;

		// Token: 0x04000076 RID: 118
		internal const int LOCALE_IFIRSTDAYOFWEEK = 4108;

		// Token: 0x04000077 RID: 119
		internal const int LOCALE_IFIRSTWEEKOFYEAR = 4109;

		// Token: 0x04000078 RID: 120
		internal const int LOCALE_ICOUNTRY = 5;

		// Token: 0x04000079 RID: 121
		internal const int LOCALE_IMEASURE = 13;

		// Token: 0x0400007A RID: 122
		internal const int LOCALE_IDIGITSUBSTITUTION = 4116;

		// Token: 0x0400007B RID: 123
		internal const int LOCALE_SGROUPING = 16;

		// Token: 0x0400007C RID: 124
		internal const int LOCALE_SMONGROUPING = 24;

		// Token: 0x0400007D RID: 125
		internal const int LOCALE_SLIST = 12;

		// Token: 0x0400007E RID: 126
		internal const int LOCALE_SDECIMAL = 14;

		// Token: 0x0400007F RID: 127
		internal const int LOCALE_STHOUSAND = 15;

		// Token: 0x04000080 RID: 128
		internal const int LOCALE_SCURRENCY = 20;

		// Token: 0x04000081 RID: 129
		internal const int LOCALE_SMONDECIMALSEP = 22;

		// Token: 0x04000082 RID: 130
		internal const int LOCALE_SMONTHOUSANDSEP = 23;

		// Token: 0x04000083 RID: 131
		internal const int LOCALE_SPOSITIVESIGN = 80;

		// Token: 0x04000084 RID: 132
		internal const int LOCALE_SNEGATIVESIGN = 81;

		// Token: 0x04000085 RID: 133
		internal const int LOCALE_S1159 = 40;

		// Token: 0x04000086 RID: 134
		internal const int LOCALE_S2359 = 41;

		// Token: 0x04000087 RID: 135
		internal const int LOCALE_SNATIVEDIGITS = 19;

		// Token: 0x04000088 RID: 136
		internal const int LOCALE_STIMEFORMAT = 4099;

		// Token: 0x04000089 RID: 137
		internal const int LOCALE_SSHORTDATE = 31;

		// Token: 0x0400008A RID: 138
		internal const int LOCALE_SLONGDATE = 32;

		// Token: 0x0400008B RID: 139
		internal const int LOCALE_SYEARMONTH = 4102;

		// Token: 0x0400008C RID: 140
		internal uint sName;

		// Token: 0x0400008D RID: 141
		internal uint sUnused;

		// Token: 0x0400008E RID: 142
		internal ushort iLanguage;

		// Token: 0x0400008F RID: 143
		internal ushort iParent;

		// Token: 0x04000090 RID: 144
		internal ushort iDigits;

		// Token: 0x04000091 RID: 145
		internal ushort iNegativeNumber;

		// Token: 0x04000092 RID: 146
		internal ushort iCurrencyDigits;

		// Token: 0x04000093 RID: 147
		internal ushort iCurrency;

		// Token: 0x04000094 RID: 148
		internal ushort iNegativeCurrency;

		// Token: 0x04000095 RID: 149
		internal ushort iLeadingZeros;

		// Token: 0x04000096 RID: 150
		internal ushort iFlags;

		// Token: 0x04000097 RID: 151
		internal ushort iFirstDayOfWeek;

		// Token: 0x04000098 RID: 152
		internal ushort iFirstWeekOfYear;

		// Token: 0x04000099 RID: 153
		internal ushort iCountry;

		// Token: 0x0400009A RID: 154
		internal ushort iMeasure;

		// Token: 0x0400009B RID: 155
		internal ushort iDigitSubstitution;

		// Token: 0x0400009C RID: 156
		internal uint waGrouping;

		// Token: 0x0400009D RID: 157
		internal uint waMonetaryGrouping;

		// Token: 0x0400009E RID: 158
		internal uint sListSeparator;

		// Token: 0x0400009F RID: 159
		internal uint sDecimalSeparator;

		// Token: 0x040000A0 RID: 160
		internal uint sThousandSeparator;

		// Token: 0x040000A1 RID: 161
		internal uint sCurrency;

		// Token: 0x040000A2 RID: 162
		internal uint sMonetaryDecimal;

		// Token: 0x040000A3 RID: 163
		internal uint sMonetaryThousand;

		// Token: 0x040000A4 RID: 164
		internal uint sPositiveSign;

		// Token: 0x040000A5 RID: 165
		internal uint sNegativeSign;

		// Token: 0x040000A6 RID: 166
		internal uint sAM1159;

		// Token: 0x040000A7 RID: 167
		internal uint sPM2359;

		// Token: 0x040000A8 RID: 168
		internal uint saNativeDigits;

		// Token: 0x040000A9 RID: 169
		internal uint saTimeFormat;

		// Token: 0x040000AA RID: 170
		internal uint saShortDate;

		// Token: 0x040000AB RID: 171
		internal uint saLongDate;

		// Token: 0x040000AC RID: 172
		internal uint saYearMonth;

		// Token: 0x040000AD RID: 173
		internal uint saDuration;

		// Token: 0x040000AE RID: 174
		internal ushort iDefaultLanguage;

		// Token: 0x040000AF RID: 175
		internal ushort iDefaultAnsiCodePage;

		// Token: 0x040000B0 RID: 176
		internal ushort iDefaultOemCodePage;

		// Token: 0x040000B1 RID: 177
		internal ushort iDefaultMacCodePage;

		// Token: 0x040000B2 RID: 178
		internal ushort iDefaultEbcdicCodePage;

		// Token: 0x040000B3 RID: 179
		internal ushort iGeoId;

		// Token: 0x040000B4 RID: 180
		internal ushort iPaperSize;

		// Token: 0x040000B5 RID: 181
		internal ushort iIntlCurrencyDigits;

		// Token: 0x040000B6 RID: 182
		internal uint waCalendars;

		// Token: 0x040000B7 RID: 183
		internal uint sAbbrevLang;

		// Token: 0x040000B8 RID: 184
		internal uint sISO639Language;

		// Token: 0x040000B9 RID: 185
		internal uint sEnglishLanguage;

		// Token: 0x040000BA RID: 186
		internal uint sNativeLanguage;

		// Token: 0x040000BB RID: 187
		internal uint sEnglishCountry;

		// Token: 0x040000BC RID: 188
		internal uint sNativeCountry;

		// Token: 0x040000BD RID: 189
		internal uint sAbbrevCountry;

		// Token: 0x040000BE RID: 190
		internal uint sISO3166CountryName;

		// Token: 0x040000BF RID: 191
		internal uint sIntlMonetarySymbol;

		// Token: 0x040000C0 RID: 192
		internal uint sEnglishCurrency;

		// Token: 0x040000C1 RID: 193
		internal uint sNativeCurrency;

		// Token: 0x040000C2 RID: 194
		internal uint waFontSignature;

		// Token: 0x040000C3 RID: 195
		internal uint sISO639Language2;

		// Token: 0x040000C4 RID: 196
		internal uint sISO3166CountryName2;

		// Token: 0x040000C5 RID: 197
		internal uint sParent;

		// Token: 0x040000C6 RID: 198
		internal uint saDayNames;

		// Token: 0x040000C7 RID: 199
		internal uint saAbbrevDayNames;

		// Token: 0x040000C8 RID: 200
		internal uint saMonthNames;

		// Token: 0x040000C9 RID: 201
		internal uint saAbbrevMonthNames;

		// Token: 0x040000CA RID: 202
		internal uint saMonthGenitiveNames;

		// Token: 0x040000CB RID: 203
		internal uint saAbbrevMonthGenitiveNames;

		// Token: 0x040000CC RID: 204
		internal uint saNativeCalendarNames;

		// Token: 0x040000CD RID: 205
		internal uint saAltSortID;

		// Token: 0x040000CE RID: 206
		internal ushort iNegativePercent;

		// Token: 0x040000CF RID: 207
		internal ushort iPositivePercent;

		// Token: 0x040000D0 RID: 208
		internal ushort iFormatFlags;

		// Token: 0x040000D1 RID: 209
		internal ushort iLineOrientations;

		// Token: 0x040000D2 RID: 210
		internal ushort iTextInfo;

		// Token: 0x040000D3 RID: 211
		internal ushort iInputLanguageHandle;

		// Token: 0x040000D4 RID: 212
		internal uint iCompareInfo;

		// Token: 0x040000D5 RID: 213
		internal uint sEnglishDisplayName;

		// Token: 0x040000D6 RID: 214
		internal uint sNativeDisplayName;

		// Token: 0x040000D7 RID: 215
		internal uint sPercent;

		// Token: 0x040000D8 RID: 216
		internal uint sNaN;

		// Token: 0x040000D9 RID: 217
		internal uint sPositiveInfinity;

		// Token: 0x040000DA RID: 218
		internal uint sNegativeInfinity;

		// Token: 0x040000DB RID: 219
		internal uint sMonthDay;

		// Token: 0x040000DC RID: 220
		internal uint sAdEra;

		// Token: 0x040000DD RID: 221
		internal uint sAbbrevAdEra;

		// Token: 0x040000DE RID: 222
		internal uint sRegionName;

		// Token: 0x040000DF RID: 223
		internal uint sConsoleFallbackName;

		// Token: 0x040000E0 RID: 224
		internal uint saShortTime;

		// Token: 0x040000E1 RID: 225
		internal uint saSuperShortDayNames;

		// Token: 0x040000E2 RID: 226
		internal uint saDateWords;

		// Token: 0x040000E3 RID: 227
		internal uint sSpecificCulture;

		// Token: 0x040000E4 RID: 228
		internal uint sKeyboardsToInstall;

		// Token: 0x040000E5 RID: 229
		internal uint sScripts;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003C2 RID: 962
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CultureTableData
	{
		// Token: 0x0400124B RID: 4683
		internal const int sizeofDataFields = 304;

		// Token: 0x0400124C RID: 4684
		internal const int LOCALE_IDIGITS = 17;

		// Token: 0x0400124D RID: 4685
		internal const int LOCALE_INEGNUMBER = 4112;

		// Token: 0x0400124E RID: 4686
		internal const int LOCALE_ICURRDIGITS = 25;

		// Token: 0x0400124F RID: 4687
		internal const int LOCALE_ICURRENCY = 27;

		// Token: 0x04001250 RID: 4688
		internal const int LOCALE_INEGCURR = 28;

		// Token: 0x04001251 RID: 4689
		internal const int LOCALE_ILZERO = 18;

		// Token: 0x04001252 RID: 4690
		internal const int LOCALE_IFIRSTDAYOFWEEK = 4108;

		// Token: 0x04001253 RID: 4691
		internal const int LOCALE_IFIRSTWEEKOFYEAR = 4109;

		// Token: 0x04001254 RID: 4692
		internal const int LOCALE_ICOUNTRY = 5;

		// Token: 0x04001255 RID: 4693
		internal const int LOCALE_IMEASURE = 13;

		// Token: 0x04001256 RID: 4694
		internal const int LOCALE_IDIGITSUBSTITUTION = 4116;

		// Token: 0x04001257 RID: 4695
		internal const int LOCALE_SGROUPING = 16;

		// Token: 0x04001258 RID: 4696
		internal const int LOCALE_SMONGROUPING = 24;

		// Token: 0x04001259 RID: 4697
		internal const int LOCALE_SLIST = 12;

		// Token: 0x0400125A RID: 4698
		internal const int LOCALE_SDECIMAL = 14;

		// Token: 0x0400125B RID: 4699
		internal const int LOCALE_STHOUSAND = 15;

		// Token: 0x0400125C RID: 4700
		internal const int LOCALE_SCURRENCY = 20;

		// Token: 0x0400125D RID: 4701
		internal const int LOCALE_SMONDECIMALSEP = 22;

		// Token: 0x0400125E RID: 4702
		internal const int LOCALE_SMONTHOUSANDSEP = 23;

		// Token: 0x0400125F RID: 4703
		internal const int LOCALE_SPOSITIVESIGN = 80;

		// Token: 0x04001260 RID: 4704
		internal const int LOCALE_SNEGATIVESIGN = 81;

		// Token: 0x04001261 RID: 4705
		internal const int LOCALE_S1159 = 40;

		// Token: 0x04001262 RID: 4706
		internal const int LOCALE_S2359 = 41;

		// Token: 0x04001263 RID: 4707
		internal const int LOCALE_SNATIVEDIGITS = 19;

		// Token: 0x04001264 RID: 4708
		internal const int LOCALE_STIMEFORMAT = 4099;

		// Token: 0x04001265 RID: 4709
		internal const int LOCALE_SSHORTDATE = 31;

		// Token: 0x04001266 RID: 4710
		internal const int LOCALE_SLONGDATE = 32;

		// Token: 0x04001267 RID: 4711
		internal const int LOCALE_SYEARMONTH = 4102;

		// Token: 0x04001268 RID: 4712
		internal uint sName;

		// Token: 0x04001269 RID: 4713
		internal uint sUnused;

		// Token: 0x0400126A RID: 4714
		internal ushort iLanguage;

		// Token: 0x0400126B RID: 4715
		internal ushort iParent;

		// Token: 0x0400126C RID: 4716
		internal ushort iDigits;

		// Token: 0x0400126D RID: 4717
		internal ushort iNegativeNumber;

		// Token: 0x0400126E RID: 4718
		internal ushort iCurrencyDigits;

		// Token: 0x0400126F RID: 4719
		internal ushort iCurrency;

		// Token: 0x04001270 RID: 4720
		internal ushort iNegativeCurrency;

		// Token: 0x04001271 RID: 4721
		internal ushort iLeadingZeros;

		// Token: 0x04001272 RID: 4722
		internal ushort iFlags;

		// Token: 0x04001273 RID: 4723
		internal ushort iFirstDayOfWeek;

		// Token: 0x04001274 RID: 4724
		internal ushort iFirstWeekOfYear;

		// Token: 0x04001275 RID: 4725
		internal ushort iCountry;

		// Token: 0x04001276 RID: 4726
		internal ushort iMeasure;

		// Token: 0x04001277 RID: 4727
		internal ushort iDigitSubstitution;

		// Token: 0x04001278 RID: 4728
		internal uint waGrouping;

		// Token: 0x04001279 RID: 4729
		internal uint waMonetaryGrouping;

		// Token: 0x0400127A RID: 4730
		internal uint sListSeparator;

		// Token: 0x0400127B RID: 4731
		internal uint sDecimalSeparator;

		// Token: 0x0400127C RID: 4732
		internal uint sThousandSeparator;

		// Token: 0x0400127D RID: 4733
		internal uint sCurrency;

		// Token: 0x0400127E RID: 4734
		internal uint sMonetaryDecimal;

		// Token: 0x0400127F RID: 4735
		internal uint sMonetaryThousand;

		// Token: 0x04001280 RID: 4736
		internal uint sPositiveSign;

		// Token: 0x04001281 RID: 4737
		internal uint sNegativeSign;

		// Token: 0x04001282 RID: 4738
		internal uint sAM1159;

		// Token: 0x04001283 RID: 4739
		internal uint sPM2359;

		// Token: 0x04001284 RID: 4740
		internal uint saNativeDigits;

		// Token: 0x04001285 RID: 4741
		internal uint saTimeFormat;

		// Token: 0x04001286 RID: 4742
		internal uint saShortDate;

		// Token: 0x04001287 RID: 4743
		internal uint saLongDate;

		// Token: 0x04001288 RID: 4744
		internal uint saYearMonth;

		// Token: 0x04001289 RID: 4745
		internal uint saDuration;

		// Token: 0x0400128A RID: 4746
		internal ushort iDefaultLanguage;

		// Token: 0x0400128B RID: 4747
		internal ushort iDefaultAnsiCodePage;

		// Token: 0x0400128C RID: 4748
		internal ushort iDefaultOemCodePage;

		// Token: 0x0400128D RID: 4749
		internal ushort iDefaultMacCodePage;

		// Token: 0x0400128E RID: 4750
		internal ushort iDefaultEbcdicCodePage;

		// Token: 0x0400128F RID: 4751
		internal ushort iGeoId;

		// Token: 0x04001290 RID: 4752
		internal ushort iPaperSize;

		// Token: 0x04001291 RID: 4753
		internal ushort iIntlCurrencyDigits;

		// Token: 0x04001292 RID: 4754
		internal uint waCalendars;

		// Token: 0x04001293 RID: 4755
		internal uint sAbbrevLang;

		// Token: 0x04001294 RID: 4756
		internal uint sISO639Language;

		// Token: 0x04001295 RID: 4757
		internal uint sEnglishLanguage;

		// Token: 0x04001296 RID: 4758
		internal uint sNativeLanguage;

		// Token: 0x04001297 RID: 4759
		internal uint sEnglishCountry;

		// Token: 0x04001298 RID: 4760
		internal uint sNativeCountry;

		// Token: 0x04001299 RID: 4761
		internal uint sAbbrevCountry;

		// Token: 0x0400129A RID: 4762
		internal uint sISO3166CountryName;

		// Token: 0x0400129B RID: 4763
		internal uint sIntlMonetarySymbol;

		// Token: 0x0400129C RID: 4764
		internal uint sEnglishCurrency;

		// Token: 0x0400129D RID: 4765
		internal uint sNativeCurrency;

		// Token: 0x0400129E RID: 4766
		internal uint waFontSignature;

		// Token: 0x0400129F RID: 4767
		internal uint sISO639Language2;

		// Token: 0x040012A0 RID: 4768
		internal uint sISO3166CountryName2;

		// Token: 0x040012A1 RID: 4769
		internal uint sParent;

		// Token: 0x040012A2 RID: 4770
		internal uint saDayNames;

		// Token: 0x040012A3 RID: 4771
		internal uint saAbbrevDayNames;

		// Token: 0x040012A4 RID: 4772
		internal uint saMonthNames;

		// Token: 0x040012A5 RID: 4773
		internal uint saAbbrevMonthNames;

		// Token: 0x040012A6 RID: 4774
		internal uint saMonthGenitiveNames;

		// Token: 0x040012A7 RID: 4775
		internal uint saAbbrevMonthGenitiveNames;

		// Token: 0x040012A8 RID: 4776
		internal uint saNativeCalendarNames;

		// Token: 0x040012A9 RID: 4777
		internal uint saAltSortID;

		// Token: 0x040012AA RID: 4778
		internal ushort iNegativePercent;

		// Token: 0x040012AB RID: 4779
		internal ushort iPositivePercent;

		// Token: 0x040012AC RID: 4780
		internal ushort iFormatFlags;

		// Token: 0x040012AD RID: 4781
		internal ushort iLineOrientations;

		// Token: 0x040012AE RID: 4782
		internal ushort iTextInfo;

		// Token: 0x040012AF RID: 4783
		internal ushort iInputLanguageHandle;

		// Token: 0x040012B0 RID: 4784
		internal uint iCompareInfo;

		// Token: 0x040012B1 RID: 4785
		internal uint sEnglishDisplayName;

		// Token: 0x040012B2 RID: 4786
		internal uint sNativeDisplayName;

		// Token: 0x040012B3 RID: 4787
		internal uint sPercent;

		// Token: 0x040012B4 RID: 4788
		internal uint sNaN;

		// Token: 0x040012B5 RID: 4789
		internal uint sPositiveInfinity;

		// Token: 0x040012B6 RID: 4790
		internal uint sNegativeInfinity;

		// Token: 0x040012B7 RID: 4791
		internal uint sMonthDay;

		// Token: 0x040012B8 RID: 4792
		internal uint sAdEra;

		// Token: 0x040012B9 RID: 4793
		internal uint sAbbrevAdEra;

		// Token: 0x040012BA RID: 4794
		internal uint sRegionName;

		// Token: 0x040012BB RID: 4795
		internal uint sConsoleFallbackName;

		// Token: 0x040012BC RID: 4796
		internal uint saShortTime;

		// Token: 0x040012BD RID: 4797
		internal uint saSuperShortDayNames;

		// Token: 0x040012BE RID: 4798
		internal uint saDateWords;

		// Token: 0x040012BF RID: 4799
		internal uint sSpecificCulture;

		// Token: 0x040012C0 RID: 4800
		internal uint sKeyboardsToInstall;

		// Token: 0x040012C1 RID: 4801
		internal uint sScripts;
	}
}

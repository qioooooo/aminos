using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Globalization
{
	// Token: 0x02000004 RID: 4
	internal class CultureDefinition
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002234 File Offset: 0x00001234
		internal static void Compile(CultureAndRegionInfoBuilder builder, string outFile)
		{
			string tempFileName = Path.GetTempFileName();
			FileStream fileStream = new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
			CultureDefinition cultureDefinition = new CultureDefinition();
			cultureDefinition.GetDataFromCultureAndRegionInfoBuilder(builder);
			cultureDefinition.BuildBinaryFile(fileStream);
			fileStream.Flush();
			fileStream.Close();
			File.Copy(tempFileName, outFile);
			File.Delete(tempFileName);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000227E File Offset: 0x0000127E
		private CultureDefinition()
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002288 File Offset: 0x00001288
		private static string[] GetAlternativeSortNames(CultureAndRegionInfoBuilder builder)
		{
			if (!builder.IsReplacementCulture || (builder.CultureTypes & CultureTypes.WindowsOnlyCultures) != (CultureTypes)0)
			{
				return null;
			}
			string[] array = new string[15];
			bool flag = false;
			for (int i = 1; i <= 15; i++)
			{
				int num = builder.LCID | (i << 16);
				try
				{
					CultureInfo cultureInfo = CultureInfo.GetCultureInfo(num);
					array[i - 1] = cultureInfo.CompareInfo.Name;
					flag = true;
				}
				catch (ArgumentException)
				{
					array[i - 1] = string.Empty;
				}
			}
			if (!flag)
			{
				return null;
			}
			return array;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002310 File Offset: 0x00001310
		private void StoreWordValues(CultureAndRegionInfoBuilder builder)
		{
			this.m_dataRecord.iLanguage = (ushort)(builder.IsReplacementCulture ? builder.m_LCID : 4096);
			this.m_dataRecord.iDefaultLanguage = this.m_dataRecord.iLanguage;
			this.m_dataRecord.iDefaultAnsiCodePage = (ushort)builder.TextInfo.ANSICodePage;
			this.m_dataRecord.iDefaultOemCodePage = (ushort)builder.TextInfo.OEMCodePage;
			this.m_dataRecord.iDefaultMacCodePage = (ushort)builder.TextInfo.MacCodePage;
			this.m_dataRecord.iDefaultEbcdicCodePage = (ushort)builder.TextInfo.EBCDICCodePage;
			this.m_dataRecord.iParent = (ushort)builder.Parent.LCID;
			this.m_dataRecord.iLineOrientations = builder.LineOrientationsUShortValue;
			this.m_dataRecord.iTextInfo = (ushort)builder.TextInfo.LCID;
			this.m_dataRecord.iCompareInfo = (uint)builder.CompareInfo.LCID;
			this.m_dataRecord.iInputLanguageHandle = (ushort)builder.KeyboardLayoutId;
			this.m_dataRecord.iFlags = this.m_dataRecord.iFlags | (builder.IsNeutralCulture ? 0 : 1);
			if (builder.IsNeutralCulture)
			{
				return;
			}
			this.m_dataRecord.iDigits = (ushort)builder.NumberFormat.NumberDecimalDigits;
			this.m_dataRecord.iNegativeNumber = (ushort)builder.NumberFormat.NumberNegativePattern;
			this.m_dataRecord.iCurrencyDigits = (ushort)builder.NumberFormat.CurrencyDecimalDigits;
			this.m_dataRecord.iCurrency = (ushort)builder.NumberFormat.CurrencyPositivePattern;
			this.m_dataRecord.iNegativeCurrency = (ushort)builder.NumberFormat.CurrencyNegativePattern;
			this.m_dataRecord.iFirstDayOfWeek = (ushort)builder.GregorianDateTimeFormat.FirstDayOfWeek;
			this.m_dataRecord.iDigitSubstitution = (ushort)builder.NumberFormat.DigitSubstitution;
			this.m_dataRecord.iMeasure = Convert.ToUInt16(!builder.IsMetric);
			this.m_dataRecord.iGeoId = (ushort)builder.GeoId;
			this.m_dataRecord.iCountry = (ushort)builder.CountryCode;
			this.m_dataRecord.iLeadingZeros = (builder.LeadingZero ? 1 : 0);
			this.m_dataRecord.iPaperSize = (ushort)builder.PaperSize;
			this.m_dataRecord.iNegativePercent = (ushort)builder.NumberFormat.PercentNegativePattern;
			this.m_dataRecord.iPositivePercent = (ushort)builder.NumberFormat.PercentPositivePattern;
			switch (builder.GregorianDateTimeFormat.CalendarWeekRule)
			{
			case CalendarWeekRule.FirstDay:
			case CalendarWeekRule.FirstFullWeek:
			case CalendarWeekRule.FirstFourDayWeek:
				this.m_dataRecord.iFirstWeekOfYear = (ushort)builder.GregorianDateTimeFormat.CalendarWeekRule;
				return;
			default:
				throw new ArgumentOutOfRangeException("CalendarWeekRule", CultureAndRegionInfoBuilder.GetResourceString("ArgumentOutOfRange_CalendarWeekRule"));
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025C8 File Offset: 0x000015C8
		private void StoreWordArrayValues(CultureAndRegionInfoBuilder builder)
		{
			if (!builder.IsNeutralCulture)
			{
				int[] array = builder.NumberFormat.NumberGroupSizes;
				ushort[] array2 = new ushort[array.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = (ushort)array[i];
				}
				this.m_dataRecord.waGrouping = (uint)this.AddUshortArrayToPool(array2);
				array = builder.NumberFormat.CurrencyGroupSizes;
				ushort[] array3 = new ushort[array.Length];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = (ushort)array[j];
				}
				this.m_dataRecord.waMonetaryGrouping = (uint)this.AddUshortArrayToPool(array3);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000265C File Offset: 0x0000165C
		private void StoreStringValues(CultureAndRegionInfoBuilder builder)
		{
			this.m_dataRecord.sListSeparator = (uint)this.AddStringToPool(builder.TextInfo.ListSeparator);
			this.m_dataRecord.sName = (uint)this.AddStringToPool(builder.CultureName);
			this.m_dataRecord.sUnused = 0U;
			this.m_dataRecord.sEnglishDisplayName = (uint)this.AddStringToPool(builder.CultureEnglishName);
			this.m_dataRecord.sAbbrevLang = (uint)this.AddStringToPool(builder.ThreeLetterWindowsLanguageName);
			this.m_dataRecord.sISO639Language = (uint)this.AddStringToPool(builder.TwoLetterISOLanguageName);
			this.m_dataRecord.sISO639Language2 = (uint)this.AddStringToPool(builder.ThreeLetterISOLanguageName);
			this.m_dataRecord.sNativeLanguage = (uint)this.AddStringToPool((builder.NativeLanguage != null) ? builder.NativeLanguage : builder.CultureNativeName);
			this.m_dataRecord.sNativeDisplayName = (uint)this.AddStringToPool(builder.CultureNativeName);
			this.m_dataRecord.sParent = (uint)this.AddStringToPool(builder.Parent.Name);
			this.m_dataRecord.sConsoleFallbackName = (uint)this.AddStringToPool((builder.ConsoleFallbackUICulture == null) ? builder.CultureName : builder.ConsoleFallbackUICulture.Name);
			if (!builder.IsNeutralCulture)
			{
				this.m_dataRecord.sDecimalSeparator = (uint)this.AddStringToPool(builder.NumberFormat.NumberDecimalSeparator);
				this.m_dataRecord.sThousandSeparator = (uint)this.AddStringToPool(builder.NumberFormat.NumberGroupSeparator);
				this.m_dataRecord.sCurrency = (uint)this.AddStringToPool(builder.NumberFormat.CurrencySymbol);
				this.m_dataRecord.sMonetaryDecimal = (uint)this.AddStringToPool(builder.NumberFormat.CurrencyDecimalSeparator);
				this.m_dataRecord.sMonetaryThousand = (uint)this.AddStringToPool(builder.NumberFormat.CurrencyGroupSeparator);
				this.m_dataRecord.sPositiveSign = (uint)this.AddStringToPool((!builder.ForceWindowsPositivePlus && builder.NumberFormat.PositiveSign == "+") ? "" : builder.NumberFormat.PositiveSign);
				this.m_dataRecord.sNegativeSign = (uint)this.AddStringToPool(builder.NumberFormat.NegativeSign);
				this.m_dataRecord.sAM1159 = (uint)this.AddStringToPool(builder.GregorianDateTimeFormat.AMDesignator);
				this.m_dataRecord.sPM2359 = (uint)this.AddStringToPool(builder.GregorianDateTimeFormat.PMDesignator);
				this.m_dataRecord.sPercent = (uint)this.AddStringToPool(builder.NumberFormat.PercentSymbol);
				this.m_dataRecord.sNaN = (uint)this.AddStringToPool(builder.NumberFormat.NaNSymbol);
				this.m_dataRecord.sPositiveInfinity = (uint)this.AddStringToPool(builder.NumberFormat.PositiveInfinitySymbol);
				this.m_dataRecord.sNegativeInfinity = (uint)this.AddStringToPool(builder.NumberFormat.NegativeInfinitySymbol);
				this.m_dataRecord.sMonthDay = (uint)this.AddStringToPool(CultureDefinition.ConvertToWin32Escape(builder.GregorianDateTimeFormat.MonthDayPattern, builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.sRegionName = (uint)this.AddStringToPool(builder.ActualRegionName);
				this.m_dataRecord.sEnglishCountry = (uint)this.AddStringToPool(builder.RegionEnglishName);
				this.m_dataRecord.sAbbrevCountry = (uint)this.AddStringToPool(builder.ThreeLetterWindowsRegionName);
				this.m_dataRecord.sNativeCountry = (uint)this.AddStringToPool(builder.RegionNativeName);
				this.m_dataRecord.sISO3166CountryName = (uint)this.AddStringToPool(builder.TwoLetterISORegionName);
				this.m_dataRecord.sISO3166CountryName2 = (uint)this.AddStringToPool(builder.ThreeLetterISORegionName);
				this.m_dataRecord.sIntlMonetarySymbol = (uint)this.AddStringToPool(builder.ISOCurrencySymbol);
				this.m_dataRecord.sEnglishCurrency = (uint)this.AddStringToPool(builder.CurrencyEnglishName);
				this.m_dataRecord.sNativeCurrency = (uint)this.AddStringToPool(builder.CurrencyNativeName);
				this.m_dataRecord.waFontSignature = (uint)this.AddStringToPool(builder.GetFontSignature());
				DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)builder.GregorianDateTimeFormat.Clone();
				dateTimeFormatInfo.Calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);
				this.m_dataRecord.sAdEra = (uint)this.AddStringToPool(dateTimeFormatInfo.GetEraName(0));
				this.m_dataRecord.sAbbrevAdEra = (uint)this.AddStringToPool(dateTimeFormatInfo.GetAbbreviatedEraName(0));
			}
			this.m_dataRecord.sSpecificCulture = (uint)this.AddStringToPool(builder.SpecificCultureName);
			this.m_dataRecord.sScripts = (uint)this.AddStringToPool(builder.Scripts);
			this.m_dataRecord.sEnglishLanguage = (uint)((builder.EnglishLanguage != null) ? this.AddStringToPool(builder.EnglishLanguage) : this.AddStringToPool(builder.CultureEnglishName));
			this.m_dataRecord.sKeyboardsToInstall = (uint)((builder.KeyboardsToInstall != null) ? this.AddStringToPool(builder.KeyboardsToInstall) : this.AddStringToPool(""));
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002B18 File Offset: 0x00001B18
		private void StoreStringArrayValues(CultureAndRegionInfoBuilder builder)
		{
			ushort[] array = new ushort[builder.Calendars.Length];
			int num = 0;
			for (int i = 0; i < builder.Calendars.Length; i++)
			{
				array[i] = (ushort)CultureDefinition.CalendarIdOfCalendar(builder.Calendars[i]);
				if ((int)array[i] > num)
				{
					num = (int)array[i];
				}
			}
			this.m_dataRecord.waCalendars = (uint)this.AddUshortArrayToPool(array);
			if (!builder.IsNeutralCulture)
			{
				string[] array2 = new DateTimeFormatInfoScanner().GetDateWordsOfDTFI(builder.GregorianDateTimeFormat);
				if (array2 == null)
				{
					array2 = new string[0];
				}
				this.m_dataRecord.saTimeFormat = (uint)this.AddStringArrayToPool(CultureDefinition.ConvertToWin32Escapes(builder.GregorianDateTimeFormat.GetAllDateTimePatterns('T'), builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.saShortDate = (uint)this.AddStringArrayToPool(CultureDefinition.ConvertToWin32Escapes(builder.GregorianDateTimeFormat.GetAllDateTimePatterns('d'), builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.saLongDate = (uint)this.AddStringArrayToPool(CultureDefinition.ConvertToWin32Escapes(builder.GregorianDateTimeFormat.GetAllDateTimePatterns('D'), builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.saShortTime = (uint)this.AddStringArrayToPool(CultureDefinition.ConvertToWin32Escapes(builder.GregorianDateTimeFormat.GetAllDateTimePatterns('t'), builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.saYearMonth = (uint)this.AddStringArrayToPool(CultureDefinition.ConvertToWin32Escapes(builder.GregorianDateTimeFormat.GetAllDateTimePatterns('Y'), builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.saDuration = (uint)this.AddStringArrayToPool(CultureDefinition.ConvertToWin32Escapes(builder.GetDurationFormats(), builder.GregorianDateTimeFormat.TimeSeparator, builder.GregorianDateTimeFormat.DateSeparator));
				this.m_dataRecord.saNativeDigits = (uint)this.AddStringArrayToPool(builder.NumberFormat.NativeDigits);
				this.m_dataRecord.saDayNames = (uint)this.AddStringArrayToPool(builder.GregorianDateTimeFormat.DayNames);
				this.m_dataRecord.saAbbrevDayNames = (uint)this.AddStringArrayToPool(builder.GregorianDateTimeFormat.AbbreviatedDayNames);
				this.m_dataRecord.saSuperShortDayNames = (uint)this.AddStringArrayToPool(builder.GregorianDateTimeFormat.ShortestDayNames);
				string[] monthNames = builder.GregorianDateTimeFormat.MonthNames;
				string[] abbreviatedMonthNames = builder.GregorianDateTimeFormat.AbbreviatedMonthNames;
				string[] monthGenitiveNames = builder.GregorianDateTimeFormat.MonthGenitiveNames;
				string[] abbreviatedMonthGenitiveNames = builder.GregorianDateTimeFormat.AbbreviatedMonthGenitiveNames;
				FORMATFLAGS formatflags = FORMATFLAGS.None;
				formatflags |= DateTimeFormatInfoScanner.GetFormatFlagGenitiveMonth(monthNames, monthGenitiveNames, abbreviatedMonthNames, abbreviatedMonthGenitiveNames);
				formatflags |= DateTimeFormatInfoScanner.GetFormatFlagUseSpaceInMonthNames(monthNames, monthGenitiveNames, abbreviatedMonthNames, abbreviatedMonthGenitiveNames);
				formatflags |= DateTimeFormatInfoScanner.GetFormatFlagUseSpaceInDayNames(builder.GregorianDateTimeFormat.DayNames, builder.GregorianDateTimeFormat.AbbreviatedDayNames);
				formatflags |= DateTimeFormatInfoScanner.GetFormatFlagUseHebrewCalendar((int)CultureDefinition.CalendarIdOfCalendar(builder.AvailableCalendars[0]));
				this.m_dataRecord.iFormatFlags = (ushort)formatflags;
				this.m_dataRecord.saMonthNames = (uint)this.AddStringArrayToPool(monthNames);
				this.m_dataRecord.saAbbrevMonthNames = (uint)this.AddStringArrayToPool(abbreviatedMonthNames);
				this.m_dataRecord.saDateWords = (uint)this.AddStringArrayToPool(array2);
				this.m_dataRecord.saMonthGenitiveNames = (uint)this.AddStringArrayToPool(monthGenitiveNames);
				this.m_dataRecord.saAbbrevMonthGenitiveNames = (uint)this.AddStringArrayToPool(abbreviatedMonthGenitiveNames);
				string[] array3 = new string[num];
				for (int j = 0; j < builder.AvailableCalendars.Length; j++)
				{
					array3[(int)(array[j] - 1)] = builder.NativeCalendarNames[(int)(array[j] - 1)];
					if (array[j] == 1 && string.IsNullOrEmpty(array3[(int)(array[j] - 1)]))
					{
						DateTimeFormatInfo dateTimeFormatInfo;
						if (CultureDefinition.CalendarIdOfCalendar(builder.GregorianDateTimeFormat.Calendar) == CalendarId.GREGORIAN)
						{
							dateTimeFormatInfo = builder.GregorianDateTimeFormat;
						}
						else
						{
							dateTimeFormatInfo = (DateTimeFormatInfo)builder.GregorianDateTimeFormat.Clone();
							dateTimeFormatInfo.Calendar = builder.AvailableCalendars[j];
						}
						array3[(int)(array[j] - 1)] = dateTimeFormatInfo.NativeCalendarName;
					}
					if (string.IsNullOrEmpty(array3[(int)(array[j] - 1)]))
					{
						array3[(int)(array[j] - 1)] = CultureDefinition.GetCalendarNativeNameFallback(builder.AvailableCalendars[j]);
					}
				}
				this.m_dataRecord.saNativeCalendarNames = (uint)this.AddStringArrayToPool(array3);
				string[] alternativeSortNames = CultureDefinition.GetAlternativeSortNames(builder);
				if (alternativeSortNames != null)
				{
					this.m_dataRecord.saAltSortID = (uint)this.AddStringArrayToPool(alternativeSortNames);
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002F68 File Offset: 0x00001F68
		private static DateTimeFormatInfo CalendarIdToDTFI(Calendar calendar)
		{
			int num;
			switch (CultureDefinition.CalendarIdOfCalendar(calendar))
			{
			case CalendarId.GREGORIAN_US:
				num = 1065;
				goto IL_00B7;
			case CalendarId.JAPAN:
				num = 1041;
				goto IL_00B7;
			case CalendarId.TAIWAN:
				num = 1028;
				goto IL_00B7;
			case CalendarId.KOREA:
				num = 1042;
				goto IL_00B7;
			case CalendarId.HIJRI:
			case CalendarId.GREGORIAN_ARABIC:
			case CalendarId.UMALQURA:
				num = 1025;
				goto IL_00B7;
			case CalendarId.THAI:
				num = 1054;
				goto IL_00B7;
			case CalendarId.HEBREW:
				num = 1037;
				goto IL_00B7;
			case CalendarId.GREGORIAN_ME_FRENCH:
				num = 5121;
				goto IL_00B7;
			case CalendarId.GREGORIAN_XLIT_ENGLISH:
			case CalendarId.GREGORIAN_XLIT_FRENCH:
				num = 2049;
				goto IL_00B7;
			}
			return null;
			IL_00B7:
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(num);
			if ((cultureInfo.CultureTypes & CultureTypes.UserCustomCulture) != (CultureTypes)0)
			{
				return null;
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)cultureInfo.DateTimeFormat.Clone();
			dateTimeFormatInfo.Calendar = calendar;
			return dateTimeFormatInfo;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003058 File Offset: 0x00002058
		internal static string GetCalendarNativeNameFallback(Calendar calendar)
		{
			string text = string.Empty;
			DateTimeFormatInfo dateTimeFormatInfo = CultureDefinition.CalendarIdToDTFI(calendar);
			if (dateTimeFormatInfo != null)
			{
				text = dateTimeFormatInfo.NativeCalendarName;
			}
			return text;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003080 File Offset: 0x00002080
		private void GetDataFromCultureAndRegionInfoBuilder(CultureAndRegionInfoBuilder builder)
		{
			this.m_dataPool = new ArrayList(304);
			this.m_dataPoolIndices = new ArrayList(304);
			this.m_dataPoolAlignment = new ArrayList(304);
			this.m_nextPoolItem = 0;
			this.m_nextPoolItemIndex = 0;
			this.AddStringToPool(null);
			this.StoreWordValues(builder);
			this.StoreStringValues(builder);
			this.StoreStringArrayValues(builder);
			this.StoreWordArrayValues(builder);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000030F0 File Offset: 0x000020F0
		private ushort AddStringToPool(string strAdd)
		{
			if (strAdd == null || strAdd.Length == 0)
			{
				if (this.m_nextPoolItem > 0)
				{
					return 0;
				}
				strAdd = "";
			}
			int num = this.m_dataPool.IndexOf(strAdd);
			if (num >= 0)
			{
				return (ushort)this.m_dataPoolIndices[num];
			}
			this.m_dataPool.Add(strAdd);
			this.m_dataPoolAlignment.Add(false);
			this.m_dataPoolIndices.Add(this.m_nextPoolItemIndex);
			this.m_nextPoolItemIndex += (ushort)(strAdd.Length + 2);
			return (ushort)this.m_dataPoolIndices[this.m_nextPoolItem++];
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000031AC File Offset: 0x000021AC
		private ushort AddUintArrayToPool(uint[] uints)
		{
			if (uints == null || uints.Length == 0)
			{
				return 0;
			}
			int num = this.m_dataPool.IndexOf(uints);
			if (num >= 0)
			{
				return (ushort)this.m_dataPoolIndices[num];
			}
			bool flag = false;
			if ((this.m_nextPoolItemIndex & 1) == 0)
			{
				flag = true;
				this.m_nextPoolItemIndex += 1;
			}
			this.m_dataPool.Add(uints);
			this.m_dataPoolAlignment.Add(flag);
			this.m_dataPoolIndices.Add(this.m_nextPoolItemIndex);
			this.m_nextPoolItemIndex += (ushort)(uints.Length * 2 + 3);
			return (ushort)this.m_dataPoolIndices[this.m_nextPoolItem++];
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003270 File Offset: 0x00002270
		private ushort AddStringArrayToPool(string[] arAdd)
		{
			if (arAdd == null || arAdd.Length == 0)
			{
				return 0;
			}
			uint[] array = new uint[arAdd.Length];
			for (int i = 0; i < arAdd.Length; i++)
			{
				array[i] = (uint)this.AddStringToPool(arAdd[i]);
			}
			return this.AddUintArrayToPool(array);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000032B4 File Offset: 0x000022B4
		private ushort AddUshortArrayToPool(ushort[] arAdd)
		{
			if (arAdd == null || arAdd.Length == 0)
			{
				return 0;
			}
			string text = string.Empty;
			foreach (char c in arAdd)
			{
				text += c;
			}
			return this.AddStringToPool(text);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000032F8 File Offset: 0x000022F8
		private void BuildBinaryFile(FileStream fs)
		{
			uint num = 0U;
			EndianBinaryWriter endianBinaryWriter = new EndianBinaryWriter(fs, false);
			endianBinaryWriter.Write(8U);
			endianBinaryWriter.Write(0U);
			num += 8U;
			num = this.BuildEndianData(endianBinaryWriter, num);
			endianBinaryWriter.Seek(4, SeekOrigin.Begin);
			endianBinaryWriter.Write(0U);
			endianBinaryWriter.Flush();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00003340 File Offset: 0x00002340
		private uint BuildEndianData(EndianBinaryWriter writer, uint startPosition)
		{
			CultureTableHeader cultureTableHeader = default(CultureTableHeader);
			cultureTableHeader.version = 2U;
			cultureTableHeader.hash0 = 0;
			cultureTableHeader.hash1 = 0;
			cultureTableHeader.hash2 = 0;
			cultureTableHeader.hash3 = 0;
			cultureTableHeader.hash4 = 0;
			cultureTableHeader.hash5 = 0;
			cultureTableHeader.hash6 = 0;
			cultureTableHeader.hash7 = 0;
			cultureTableHeader.headerSize = (ushort)Marshal.SizeOf(cultureTableHeader);
			cultureTableHeader.numCultureItems = 1;
			cultureTableHeader.sizeCultureItem = 304;
			cultureTableHeader.numLcidItems = 1;
			cultureTableHeader.numCultureNames = 1;
			cultureTableHeader.numRegionNames = 0;
			cultureTableHeader.regionNameTableOffset = 0U;
			cultureTableHeader.cultureIDTableOffset = 0U;
			cultureTableHeader.numCalendarItems = 0;
			cultureTableHeader.sizeCalendarItem = 0;
			cultureTableHeader.offsetToCalendarItemData = 0U;
			cultureTableHeader.cultureNameTableOffset = startPosition + (uint)cultureTableHeader.headerSize;
			cultureTableHeader.offsetToCultureItemData = cultureTableHeader.cultureNameTableOffset + 8U;
			cultureTableHeader.Unused_numIetfNames = 0;
			cultureTableHeader.Unused_ietfNameTableOffset = 0U;
			cultureTableHeader.offsetToDataPool = cultureTableHeader.offsetToCultureItemData + 304U;
			uint num = startPosition + writer.WriteObject(cultureTableHeader);
			writer.Write((ushort)this.m_dataRecord.sName);
			num += 2U;
			writer.Write(0);
			num += 2U;
			writer.Write((int)this.m_dataRecord.iLanguage);
			num += 4U;
			uint num2 = writer.WriteObject(this.m_dataRecord);
			num += num2;
			for (int i = 0; i < this.m_dataPool.Count; i++)
			{
				string text = this.m_dataPool[i] as string;
				if (text != null)
				{
					writer.Write(Convert.ToUInt16(text.Length));
					num += 2U;
					for (int j = 0; j < text.Length; j++)
					{
						writer.Write((ushort)text[j]);
					}
					num += (uint)(2 * text.Length);
					writer.Write(Convert.ToUInt16(0));
					num += 2U;
				}
				else
				{
					uint[] array = this.m_dataPool[i] as uint[];
					if ((bool)this.m_dataPoolAlignment[i])
					{
						writer.Write(0);
						num += 2U;
					}
					writer.Write(Convert.ToUInt16(array.Length));
					num += 2U;
					for (int k = 0; k < array.Length; k++)
					{
						writer.Write(array[k]);
					}
					num += (uint)(4 * array.Length);
					writer.Write(0U);
					num += 4U;
				}
			}
			return num;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000035A8 File Offset: 0x000025A8
		internal static CalendarId CalendarIdOfCalendar(Calendar calendar)
		{
			if (calendar is GregorianCalendar)
			{
				GregorianCalendarTypes calendarType = ((GregorianCalendar)calendar).CalendarType;
				switch (calendarType)
				{
				case GregorianCalendarTypes.Localized:
					return CalendarId.GREGORIAN;
				case GregorianCalendarTypes.USEnglish:
					return CalendarId.GREGORIAN_US;
				default:
					switch (calendarType)
					{
					case GregorianCalendarTypes.MiddleEastFrench:
						return CalendarId.GREGORIAN_ME_FRENCH;
					case GregorianCalendarTypes.Arabic:
						return CalendarId.GREGORIAN_ARABIC;
					case GregorianCalendarTypes.TransliteratedEnglish:
						return CalendarId.GREGORIAN_XLIT_ENGLISH;
					case GregorianCalendarTypes.TransliteratedFrench:
						return CalendarId.GREGORIAN_XLIT_FRENCH;
					}
					break;
				}
			}
			else
			{
				if (calendar is HebrewCalendar)
				{
					return CalendarId.HEBREW;
				}
				if (calendar is ThaiBuddhistCalendar)
				{
					return CalendarId.THAI;
				}
				if (calendar is TaiwanCalendar)
				{
					return CalendarId.TAIWAN;
				}
				if (calendar is JapaneseCalendar)
				{
					return CalendarId.JAPAN;
				}
				if (calendar is KoreanCalendar)
				{
					return CalendarId.KOREA;
				}
				if (calendar is HijriCalendar)
				{
					return CalendarId.HIJRI;
				}
				if (calendar is UmAlQuraCalendar)
				{
					return CalendarId.UMALQURA;
				}
			}
			throw new NotSupportedException(CultureAndRegionInfoBuilder.GetResourceString("CustomCaledarsNotSupported"));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000365A File Offset: 0x0000265A
		internal static string GetTagFromCalendar(Calendar calendar)
		{
			return CultureDefinition.CalendarTags[(int)CultureDefinition.CalendarIdOfCalendar(calendar)];
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003668 File Offset: 0x00002668
		internal static Calendar GetCalendarFromTag(string calendarTag)
		{
			if (calendarTag.Equals(CultureDefinition.CalendarTags[1]))
			{
				return new GregorianCalendar(GregorianCalendarTypes.Localized);
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[2]))
			{
				return new GregorianCalendar(GregorianCalendarTypes.USEnglish);
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[3]))
			{
				return new JapaneseCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[4]))
			{
				return new TaiwanCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[5]))
			{
				return new KoreanCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[6]))
			{
				return new HijriCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[7]))
			{
				return new ThaiBuddhistCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[8]))
			{
				return new HebrewCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[23]))
			{
				return new UmAlQuraCalendar();
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[9]))
			{
				return new GregorianCalendar(GregorianCalendarTypes.MiddleEastFrench);
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[10]))
			{
				return new GregorianCalendar(GregorianCalendarTypes.Arabic);
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[11]))
			{
				return new GregorianCalendar(GregorianCalendarTypes.TransliteratedEnglish);
			}
			if (calendarTag.Equals(CultureDefinition.CalendarTags[12]))
			{
				return new GregorianCalendar(GregorianCalendarTypes.TransliteratedFrench);
			}
			return null;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003798 File Offset: 0x00002798
		private static string ConvertToWin32Escape(string str, string timeMark, string dateMark)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(str.Length * 2);
			bool flag = dateMark != "/";
			bool flag2 = timeMark != ":";
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			int i = 0;
			while (i < str.Length)
			{
				if (str[i] == '\'')
				{
					if ((i < str.Length - 1 && str[i + 1] == '\'') || i >= str.Length - 1)
					{
						i++;
					}
					else if (flag4)
					{
						flag4 = false;
						flag5 = false;
					}
					else
					{
						if (!flag5)
						{
							flag3 = !flag3;
							flag5 = !flag3;
							goto IL_01E8;
						}
						stringBuilder.Remove(stringBuilder.Length - 1, 1);
						flag5 = false;
						flag3 = true;
					}
				}
				else if (str[i] == '\\')
				{
					if (!flag3)
					{
						flag4 = true;
						flag3 = true;
						if (flag5)
						{
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
						}
						else
						{
							stringBuilder.Append('\'');
						}
					}
					flag5 = false;
					i++;
					if (i >= str.Length)
					{
						stringBuilder.Append('\\');
						break;
					}
					if (str[i] != '\'')
					{
						goto IL_01E8;
					}
					stringBuilder.Append("''");
				}
				else
				{
					if ((flag3 && !flag4) || ((!flag2 || str[i] != ':') && (!flag || str[i] != '/')))
					{
						if (flag4)
						{
							flag4 = false;
							flag3 = false;
							stringBuilder.Append('\'');
						}
						flag5 = false;
						goto IL_01E8;
					}
					if (!flag3)
					{
						flag4 = true;
						flag3 = true;
						if (flag5)
						{
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
						}
						else
						{
							stringBuilder.Append('\'');
						}
					}
					string text = ((str[i] == ':') ? timeMark : dateMark);
					flag5 = false;
					for (int j = 0; j < text.Length; j++)
					{
						if (text[j] != '\'')
						{
							stringBuilder.Append(text[j]);
						}
						else
						{
							stringBuilder.Append("''");
						}
					}
				}
				IL_01FA:
				i++;
				continue;
				IL_01E8:
				if (stringBuilder != null)
				{
					stringBuilder.Append(str[i]);
					goto IL_01FA;
				}
				goto IL_01FA;
			}
			if (flag3)
			{
				stringBuilder.Append('\'');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000039C4 File Offset: 0x000029C4
		private static string[] ConvertToWin32Escapes(string[] array, string timeMark, string dateMark)
		{
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureDefinition.ConvertToWin32Escape(array[i], timeMark, dateMark);
				}
			}
			return array;
		}

		// Token: 0x04000004 RID: 4
		private const uint sizeofNameOffsetItem = 8U;

		// Token: 0x04000005 RID: 5
		private const int MaxAlternativeSortNames = 15;

		// Token: 0x04000006 RID: 6
		internal static readonly string[] CalendarTags = new string[]
		{
			"", "Gregorian", "Gregorian US", "Japanese", "Taiwan", "Korean", "Hijri", "Thai", "Hebrew", "Gregorian ME French",
			"Gregorian Arabic", "Gregorian Transliterated English", "Gregorian Transliterated French", "Julian", "Japenese Lunisolar", "Chinese Lunisolar", "Saka", "Lunar ETO Chinese", "Lunar ETO Korean", "Lunar ETO Rokuyou",
			"Korean Lunisolar", "Taiwan Lunisolar", "Persian", "UmAlQura"
		};

		// Token: 0x04000007 RID: 7
		private CultureTableData m_dataRecord;

		// Token: 0x04000008 RID: 8
		private ArrayList m_dataPool;

		// Token: 0x04000009 RID: 9
		private ArrayList m_dataPoolIndices;

		// Token: 0x0400000A RID: 10
		private ArrayList m_dataPoolAlignment;

		// Token: 0x0400000B RID: 11
		private int m_nextPoolItem;

		// Token: 0x0400000C RID: 12
		private ushort m_nextPoolItemIndex;
	}
}

using System;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Globalization
{
	// Token: 0x0200000C RID: 12
	internal sealed class CultureXmlWriter
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00006621 File Offset: 0x00005621
		internal CultureXmlWriter(CultureAndRegionInfoBuilder carib)
		{
			if (carib == null)
			{
				throw new ArgumentNullException("carib");
			}
			this._carib = carib;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000663E File Offset: 0x0000563E
		internal void Save(string xmlFileName)
		{
			this._tempFileName = Path.GetTempFileName();
			this._stream = new FileStream(this._tempFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
			this.Save(this._stream);
			File.Copy(this._tempFileName, xmlFileName);
			this.Close(true);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006680 File Offset: 0x00005680
		internal void Save(Stream stream)
		{
			try
			{
				this._writer = new XmlTextWriter(stream, Encoding.UTF8);
				this._writer.Formatting = Formatting.Indented;
				this.WriteXml();
			}
			catch
			{
				this.Close(true);
				throw;
			}
			this.Close(false);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000066D4 File Offset: 0x000056D4
		private void WriteXml()
		{
			this._writer.WriteStartDocument();
			this._writer.WriteStartElement("ldml");
			this.WriteIdentityElement();
			this.WriteLayoutElement();
			this.WriteCharactersElement();
			this.WriteMeasurmentElement();
			this.WriteDatesElement();
			this.WriteNumbersElement();
			this._writer.WriteEndDocument();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000672C File Offset: 0x0000572C
		private void WriteIdentityElement()
		{
			this._writer.WriteStartElement("identity");
			this.WriteElementWithAttributeAndString("version", "number", "1.1", "ldml version 1.1");
			this.WriteElementWithAttribute("generation", "date", DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
			this._writer.WriteStartElement("special");
			this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
			this.WriteElementWithAttribute("cultureInfoVersion", "type", "1.0", true);
			this.WriteElementWithAttribute("cultureAndRegionInfoName", "type", this._carib.CultureName, true);
			if (this._carib.IsNeutralCulture)
			{
				this.WriteElementWithAttribute("cultureAndRegionModifier", "type", "neutral", true);
				this.WriteElementWithAttribute("specificCultureName", "type", this._carib.SpecificCultureName, true);
			}
			if (!this._carib.IsNeutralCulture && this._carib.GeoId > 0)
			{
				this.WriteElementWithString("geoId", this._carib.GeoId.ToString(CultureInfo.InvariantCulture), true);
			}
			if (!this._carib.IsNeutralCulture && this._carib.CountryCode > 0)
			{
				this.WriteElementWithString("countryCode", this._carib.CountryCode.ToString(CultureInfo.InvariantCulture), true);
			}
			if (this._carib.Parent != null)
			{
				this.WriteElementWithAttribute("parentName", "type", this._carib.Parent.Name, true);
			}
			if (this._carib.ThreeLetterWindowsLanguageName != null && this._carib.ThreeLetterWindowsLanguageName.Length > 0)
			{
				this.WriteElementWithAttribute("languageNameAbbr", "type", this._carib.ThreeLetterWindowsLanguageName, true);
			}
			if (this._carib.ThreeLetterISOLanguageName != null && this._carib.ThreeLetterISOLanguageName.Length > 0)
			{
				this.WriteElementWithAttributeAndString("languageIsoName", "type", "threeLetters", this._carib.ThreeLetterISOLanguageName, true);
			}
			if (this._carib.TwoLetterISOLanguageName != null && this._carib.TwoLetterISOLanguageName.Length > 0)
			{
				this.WriteElementWithAttributeAndString("languageIsoName", "type", "twoLetters", this._carib.TwoLetterISOLanguageName, true);
			}
			if (this._carib.CultureNativeName != null && this._carib.CultureNativeName.Length > 0)
			{
				this.WriteElementWithAttribute("nativeName", "type", this._carib.CultureNativeName, true);
			}
			if (!this._carib.IsNeutralCulture && this._carib.RegionEnglishName != null && this._carib.RegionEnglishName.Length > 0)
			{
				this.WriteElementWithAttribute("regionEnglishName", "type", this._carib.RegionEnglishName, true);
			}
			if (!this._carib.IsNeutralCulture && this._carib.RegionNativeName != null && this._carib.RegionNativeName.Length > 0)
			{
				this.WriteElementWithAttribute("regionNativeName", "type", this._carib.RegionNativeName, true);
			}
			if (!this._carib.IsNeutralCulture)
			{
				if (this._carib.ThreeLetterISORegionName != null && this._carib.ThreeLetterISORegionName.Length > 0)
				{
					this.WriteElementWithAttributeAndString("regionIsoName", "type", "threeLetters", this._carib.ThreeLetterISORegionName, true);
				}
				if (this._carib.TwoLetterISORegionName != null && this._carib.TwoLetterISORegionName.Length > 0)
				{
					this.WriteElementWithAttributeAndString("regionIsoName", "type", "twoLetters", this._carib.TwoLetterISORegionName, true);
				}
				if (this._carib.ThreeLetterWindowsRegionName != null)
				{
					this.WriteElementWithAttributeAndString("regionWindowsName", "type", "threeLetters", this._carib.ThreeLetterWindowsRegionName, true);
				}
			}
			if (this._carib.CultureEnglishName != null && this._carib.CultureEnglishName.Length > 0)
			{
				this.WriteElementWithAttribute("englishName", "type", this._carib.CultureEnglishName, true);
			}
			if (this._carib.TextInfo != null)
			{
				this.WriteElementWithAttribute("textInfoName", "type", this._carib.TextInfo.CultureName, true);
			}
			if (this._carib.CompareInfo != null)
			{
				this.WriteElementWithAttribute("sortName", "type", this._carib.CompareInfo.Name, true);
			}
			if (this._carib.EnglishLanguage != null && this._carib.EnglishLanguage.Length > 0)
			{
				this.WriteElementWithAttribute("englishLanguage", "type", this._carib.EnglishLanguage, true);
			}
			else if (this._carib.CultureEnglishName != null && this._carib.CultureEnglishName.Length > 0)
			{
				this.WriteElementWithAttribute("englishLanguage", "type", this._carib.CultureEnglishName, true);
			}
			if (this._carib.NativeLanguage != null && this._carib.NativeLanguage.Length > 0)
			{
				this.WriteElementWithAttribute("nativeLanguage", "type", this._carib.NativeLanguage, true);
			}
			else if (this._carib.CultureNativeName != null && this._carib.CultureNativeName.Length > 0)
			{
				this.WriteElementWithAttribute("nativeLanguage", "type", this._carib.CultureNativeName, true);
			}
			if (this._carib.KeyboardsToInstall != null && this._carib.KeyboardsToInstall.Length > 0)
			{
				this.WriteElementWithAttribute("keyboardsToInstall", "type", this._carib.KeyboardsToInstall, true);
			}
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006CFC File Offset: 0x00005CFC
		private void WriteCharactersElement()
		{
			if (this._carib.TextInfo == null)
			{
				return;
			}
			this._writer.WriteStartElement("characters");
			this._writer.WriteStartElement("special");
			this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
			if (this._carib.KeyboardLayoutId != 0)
			{
				this.WriteElementWithString("keyboardLayout", this._carib.KeyboardLayoutId.ToString(CultureInfo.InvariantCulture), true);
			}
			if (this._carib.ConsoleFallbackUICulture != null)
			{
				this.WriteElementWithAttribute("consoleFallbackName", "type", this._carib.ConsoleFallbackUICulture.Name, true);
			}
			this.WriteFontSignatureElement();
			this.WriteScriptsElement();
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006DD4 File Offset: 0x00005DD4
		private void WriteCodePages(bool isDefault)
		{
			int num = (isDefault ? 8 : 12);
			this._writer.WriteStartElement("msLocale", isDefault ? "defaultCodePages" : "codePages", null);
			this._writer.WriteStartElement("msLocale", "ansiCodePage", null);
			short num2 = (short)this._carib.FontSignature[num];
			for (int i = 0; i < 16; i++)
			{
				if (((int)num2 & (1 << i)) != 0)
				{
					this.WriteElementWithAttribute("codePage", "type", this._carib.CodePages[i].ToString(CultureInfo.InvariantCulture), true);
				}
			}
			this._writer.WriteEndElement();
			this._writer.WriteStartElement("msLocale", "ansiOemCodePage", null);
			num2 = (short)this._carib.FontSignature[num + 1];
			for (int j = 0; j < 16; j++)
			{
				if (((int)num2 & (1 << j)) != 0)
				{
					this.WriteElementWithAttribute("codePage", "type", this._carib.CodePages[j + 16].ToString(CultureInfo.InvariantCulture), true);
				}
			}
			this._writer.WriteEndElement();
			this._writer.WriteStartElement("msLocale", "oemCodePage", null);
			for (int k = 0; k < 2; k++)
			{
				int num3 = 32 + k * 16;
				num2 = (short)this._carib.FontSignature[num + k + 2];
				for (int l = 0; l < 16; l++)
				{
					if (((int)num2 & (1 << l)) != 0)
					{
						this.WriteElementWithAttribute("codePage", "type", this._carib.CodePages[num3 + l].ToString(CultureInfo.InvariantCulture), true);
					}
				}
			}
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006FA8 File Offset: 0x00005FA8
		private void WriteFontSignatureElement()
		{
			if (!this._carib.IsNeutralCulture)
			{
				this._writer.WriteStartElement("msLocale", "fontSignature", null);
				this._writer.WriteStartElement("msLocale", "unicodeRanges", null);
				int num = 0;
				short num2;
				for (int i = 0; i < 7; i++)
				{
					num2 = (short)this._carib.FontSignature[i];
					for (int j = 0; j < 16; j++)
					{
						if (((int)num2 & (1 << j)) != 0)
						{
							this.WriteElementWithAttribute("range", "type", num.ToString(CultureInfo.InvariantCulture), true);
						}
						num++;
					}
				}
				num2 = (short)this._carib.FontSignature[7];
				for (int k = 0; k < 11; k++)
				{
					if (((int)num2 & (1 << k)) != 0)
					{
						this.WriteElementWithAttribute("range", "type", num.ToString(CultureInfo.InvariantCulture), true);
					}
					num++;
				}
				if ((num2 & 2048) != 0)
				{
					this.WriteElementWithAttribute("layoutProgress", "type", "horizontalRightToLeft", true);
				}
				if ((num2 & 4096) != 0)
				{
					this.WriteElementWithAttribute("layoutProgress", "type", "verticalBeforeHorizontal", true);
				}
				if ((num2 & 8192) != 0)
				{
					this.WriteElementWithAttribute("layoutProgress", "type", "verticalBottomToTop", true);
				}
				this._writer.WriteEndElement();
				this.WriteCodePages(true);
				this.WriteCodePages(false);
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000711C File Offset: 0x0000611C
		private void WriteScriptsElement()
		{
			this._writer.WriteStartElement("msLocale", "scripts", null);
			string scripts = this._carib.Scripts;
			for (int i = 0; i < scripts.Length; i += 5)
			{
				this.WriteElementWithAttribute("script", "type", scripts.Substring(i, 4), true);
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007180 File Offset: 0x00006180
		private void WriteLayoutElement()
		{
			this._writer.WriteStartElement("layout");
			this._writer.WriteStartElement("special");
			this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
			this._writer.WriteStartElement("msLocale", "direction", null);
			this._writer.WriteAttributeString("type", this._carib.IsRightToLeft ? "right-to-left" : "left-to-right");
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00007228 File Offset: 0x00006228
		private void WriteMeasurmentElement()
		{
			if (this._carib.IsNeutralCulture)
			{
				return;
			}
			this._writer.WriteStartElement("measurement");
			this._writer.WriteStartElement("measurementSystem");
			if (this._carib.IsMetric)
			{
				this._writer.WriteAttributeString("type", "metric");
			}
			else
			{
				this._writer.WriteAttributeString("type", "US");
			}
			this._writer.WriteEndElement();
			this.WritePaperSizeElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000072B8 File Offset: 0x000062B8
		private void WritePaperSizeElement()
		{
			PaperSize paperSize = this._carib.GetPaperSize();
			string text;
			string text2;
			if (paperSize != PaperSize.Letter)
			{
				switch (paperSize)
				{
				case PaperSize.Legal:
					text = "216";
					text2 = "356";
					break;
				case (PaperSize)6:
				case (PaperSize)7:
					return;
				case PaperSize.A3:
					text = "297";
					text2 = "420";
					break;
				case PaperSize.A4:
					text = "210";
					text2 = "297";
					break;
				default:
					return;
				}
			}
			else
			{
				text = "216";
				text2 = "279";
			}
			this._writer.WriteStartElement("paperSize");
			this.WriteElementWithString("height", text2, false);
			this.WriteElementWithString("width", text, false);
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00007360 File Offset: 0x00006360
		private void WriteGregorianDateTimeFormat()
		{
			if (this._carib.GregorianDateTimeFormat != null)
			{
				this._writer.WriteStartElement("months");
				this._writer.WriteStartElement("monthContext");
				this._writer.WriteAttributeString("type", "format");
				this.WriteMonthElements(this._carib.GregorianDateTimeFormat.MonthNames, "wide");
				this.WriteMonthElements(this._carib.GregorianDateTimeFormat.AbbreviatedMonthNames, "abbreviated");
				this.WriteMonthElements(this._carib.GregorianDateTimeFormat.MonthGenitiveNames, "genitive");
				this.WriteMonthElements(this._carib.GregorianDateTimeFormat.AbbreviatedMonthGenitiveNames, "genitiveAbbreviated");
				this._writer.WriteEndElement();
				this._writer.WriteEndElement();
				this._writer.WriteStartElement("days");
				this._writer.WriteStartElement("dayContext");
				this._writer.WriteAttributeString("type", "format");
				this.WriteDayElements(this._carib.GregorianDateTimeFormat.DayNames, "wide");
				this.WriteDayElements(this._carib.GregorianDateTimeFormat.AbbreviatedDayNames, "abbreviated");
				this.WriteDayElements(this._carib.GregorianDateTimeFormat.ShortestDayNames, "shortest");
				this._writer.WriteEndElement();
				this._writer.WriteEndElement();
				this._writer.WriteStartElement("week");
				this.WriteElementWithAttribute("firstDay", "day", Tags.DayNamesTag[(int)this._carib.GregorianDateTimeFormat.FirstDayOfWeek]);
				this._writer.WriteStartElement("special");
				this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
				this.WriteWeekRuleElement();
				this._writer.WriteEndElement();
				this._writer.WriteEndElement();
				this.WriteElementWithString("am", this._carib.GregorianDateTimeFormat.AMDesignator);
				this.WriteElementWithString("pm", this._carib.GregorianDateTimeFormat.PMDesignator);
				this._writer.WriteStartElement("dateFormats");
				this._writer.WriteStartElement("special");
				this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
				this.WriteDateFormatElement(this._carib.GregorianDateTimeFormat.GetAllDateTimePatterns('D'), "dateFormatLength", "long", "dateFormat");
				this.WriteDateFormatElement(this._carib.GregorianDateTimeFormat.GetAllDateTimePatterns('d'), "dateFormatLength", "short", "dateFormat");
				this.WriteDateFormatElement(this._carib.GregorianDateTimeFormat.GetAllDateTimePatterns('y'), "yearMonthFormat", null, "yearMonth");
				this._writer.WriteStartElement("msLocale", "monthDay", null);
				this.WriteElementWithString("pattern", this._carib.GregorianDateTimeFormat.MonthDayPattern, true);
				this._writer.WriteEndElement();
				this._writer.WriteEndElement();
				this._writer.WriteEndElement();
				this._writer.WriteStartElement("timeFormats");
				this._writer.WriteStartElement("special");
				this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
				this.WriteDateFormatElement(this._carib.GregorianDateTimeFormat.GetAllDateTimePatterns('T'), "timeFormatLength", "long", "timeFormat");
				this.WriteDateFormatElement(this._carib.GregorianDateTimeFormat.GetAllDateTimePatterns('t'), "timeFormatLength", "short", "timeFormat");
				this.WriteDateFormatElement(this._carib.DurationFormats, "durationFormats", null, "durationFormat");
				this._writer.WriteEndElement();
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00007740 File Offset: 0x00006740
		private void WriteDatesElement()
		{
			if (this._carib.IsNeutralCulture || this._carib.AvailableCalendars == null)
			{
				return;
			}
			Calendar[] availableCalendars = this._carib.AvailableCalendars;
			this._writer.WriteStartElement("dates");
			this._writer.WriteStartElement("calendars");
			this.WriteElementWithAttribute("default", "type", CultureDefinition.GetTagFromCalendar(availableCalendars[0]));
			for (int i = 0; i < availableCalendars.Length; i++)
			{
				string tagFromCalendar = CultureDefinition.GetTagFromCalendar(availableCalendars[i]);
				this._writer.WriteStartElement("calendar");
				this._writer.WriteAttributeString("type", tagFromCalendar);
				CalendarId calendarId = CultureDefinition.CalendarIdOfCalendar(availableCalendars[i]);
				string text = this._carib.NativeCalendarNames[(int)(calendarId - CalendarId.GREGORIAN)];
				if (calendarId == CalendarId.GREGORIAN && string.IsNullOrEmpty(text) && this._carib.GregorianDateTimeFormat != null)
				{
					DateTimeFormatInfo dateTimeFormatInfo;
					if (CultureDefinition.CalendarIdOfCalendar(this._carib.GregorianDateTimeFormat.Calendar) == CalendarId.GREGORIAN)
					{
						dateTimeFormatInfo = this._carib.GregorianDateTimeFormat;
					}
					else
					{
						dateTimeFormatInfo = (DateTimeFormatInfo)this._carib.GregorianDateTimeFormat.Clone();
						dateTimeFormatInfo.Calendar = availableCalendars[i];
					}
					text = dateTimeFormatInfo.NativeCalendarName;
				}
				if (string.IsNullOrEmpty(text))
				{
					text = CultureDefinition.GetCalendarNativeNameFallback(availableCalendars[i]);
				}
				if (!string.IsNullOrEmpty(text))
				{
					this._writer.WriteStartElement("special");
					this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
					this.WriteElementWithAttribute("calendarNativeName", "type", text, true);
					this._writer.WriteEndElement();
				}
				if (tagFromCalendar.Equals(CultureDefinition.CalendarTags[1]))
				{
					this.WriteGregorianDateTimeFormat();
				}
				this._writer.WriteEndElement();
			}
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00007910 File Offset: 0x00006910
		private void WriteNumbersElement()
		{
			if (this._carib.IsNeutralCulture || (this._carib.NumberFormat == null && this._carib.TextInfo == null && this._carib.CurrencyNativeName == null && this._carib.ISOCurrencySymbol == null && this._carib.CurrencyEnglishName == null))
			{
				return;
			}
			this._writer.WriteStartElement("numbers");
			this._writer.WriteStartElement("symbols");
			if (this._carib.TextInfo != null)
			{
				this.WriteElementWithString("list", this._carib.TextInfo.ListSeparator);
			}
			if (this._carib.NumberFormat != null)
			{
				this.WriteElementWithString("decimal", this._carib.NumberFormat.NumberDecimalSeparator);
				this.WriteElementWithString("group", this._carib.NumberFormat.NumberGroupSeparator);
				this.WriteElementWithString("percentSign", this._carib.NumberFormat.PercentSymbol);
				this.WriteElementWithAttributeAndString("infinity", "type", "positive", this._carib.NumberFormat.PositiveInfinitySymbol);
				this.WriteElementWithAttributeAndString("infinity", "type", "negative", this._carib.NumberFormat.NegativeInfinitySymbol);
				this.WriteElementWithString("nan", this._carib.NumberFormat.NaNSymbol);
				this.WriteElementWithString("PlusSign", (!this._carib.ForceWindowsPositivePlus && this._carib.NumberFormat.PositiveSign == "+") ? "" : this._carib.NumberFormat.PositiveSign);
				this.WriteElementWithString("minusSign", this._carib.NumberFormat.NegativeSign);
				this._writer.WriteStartElement("special");
				this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
				this.WriteElementWithString("currencyDecimalSeparator", this._carib.NumberFormat.CurrencyDecimalSeparator, true);
				this.WriteElementWithString("currencyGroupSeparator", this._carib.NumberFormat.CurrencyGroupSeparator, true);
				this._writer.WriteEndElement();
			}
			this._writer.WriteEndElement();
			if (this._carib.NumberFormat != null)
			{
				this._writer.WriteStartElement("special");
				this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
				this.WriteElementWithString("decimalDigits", this._carib.NumberFormat.NumberDecimalDigits.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("negativePattern", "type", this._carib.NumberFormat.NumberNegativePattern.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("currencyDecimalDigits", "type", this._carib.NumberFormat.CurrencyDecimalDigits.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("currencyPositivePattern", "type", this._carib.NumberFormat.CurrencyPositivePattern.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("currencyNegativePattern", "type", this._carib.NumberFormat.CurrencyNegativePattern.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("percentNegativePattern", "type", this._carib.NumberFormat.PercentNegativePattern.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("percentPositivePattern", "type", this._carib.NumberFormat.PercentPositivePattern.ToString(CultureInfo.InvariantCulture), true);
				this.WriteElementWithAttribute("leadingZero", "type", this._carib.LeadingZero ? "yes" : "no", true);
				int[] numberGroupSizes = this._carib.NumberFormat.NumberGroupSizes;
				for (int i = 0; i < numberGroupSizes.Length; i++)
				{
					this.WriteElementWithAttributeAndString("groupSizes", "type", i.ToString(CultureInfo.InvariantCulture), numberGroupSizes[i].ToString(CultureInfo.InvariantCulture), true);
				}
				int[] currencyGroupSizes = this._carib.NumberFormat.CurrencyGroupSizes;
				for (int j = 0; j < currencyGroupSizes.Length; j++)
				{
					this.WriteElementWithAttributeAndString("currencyGroupSizes", "type", j.ToString(CultureInfo.InvariantCulture), currencyGroupSizes[j].ToString(CultureInfo.InvariantCulture), true);
				}
				string[] nativeDigits = this._carib.NumberFormat.NativeDigits;
				for (int k = 0; k < nativeDigits.Length; k++)
				{
					this.WriteElementWithAttributeAndString("nativeDigits", "type", k.ToString(CultureInfo.InvariantCulture), nativeDigits[k], true);
				}
				switch (this._carib.NumberFormat.DigitSubstitution)
				{
				case DigitShapes.Context:
					this.WriteElementWithAttribute("digitSubstitution", "type", "context", true);
					break;
				case DigitShapes.None:
					this.WriteElementWithAttribute("digitSubstitution", "type", "none", true);
					break;
				case DigitShapes.NativeNational:
					this.WriteElementWithAttribute("digitSubstitution", "type", "nativeNational", true);
					break;
				}
				this._writer.WriteEndElement();
			}
			this._writer.WriteStartElement("currencies");
			this._writer.WriteStartElement("currency");
			this._writer.WriteAttributeString("type", "default");
			if (this._carib.NumberFormat != null)
			{
				this.WriteElementWithString("symbol", this._carib.NumberFormat.CurrencySymbol);
			}
			if (this._carib.CurrencyNativeName != null)
			{
				this.WriteElementWithString("displayName", this._carib.CurrencyNativeName);
			}
			this._writer.WriteStartElement("special");
			this._writer.WriteAttributeString("xmlns", "msLocale", null, "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
			if (this._carib.ISOCurrencySymbol != null)
			{
				this.WriteElementWithString("isoCurrency", this._carib.ISOCurrencySymbol, true);
			}
			if (this._carib.CurrencyEnglishName != null)
			{
				this.WriteElementWithString("currencyEnglishName", this._carib.CurrencyEnglishName, true);
			}
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007F7C File Offset: 0x00006F7C
		private void WriteDateFormatElement(string[] format, string mainTag, string attributeValueTag, string formatTag)
		{
			this._writer.WriteStartElement("msLocale", mainTag, null);
			if (attributeValueTag != null)
			{
				this._writer.WriteAttributeString("type", attributeValueTag);
			}
			this.WriteElementWithAttribute("default", "type", "0", true);
			for (int i = 0; i < format.Length; i++)
			{
				this._writer.WriteStartElement("msLocale", formatTag, null);
				this._writer.WriteAttributeString("type", i.ToString(CultureInfo.InvariantCulture));
				this.WriteElementWithString("pattern", format[i], true);
				this._writer.WriteEndElement();
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00008028 File Offset: 0x00007028
		private void WriteWeekRuleElement()
		{
			int calendarWeekRule = (int)this._carib.GregorianDateTimeFormat.CalendarWeekRule;
			if (calendarWeekRule >= Tags.AllWeekRuleTag.Length)
			{
				throw new ArgumentException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidWeekRule"));
			}
			this.WriteElementWithAttribute("weekRule", "type", Tags.AllWeekRuleTag[calendarWeekRule], true);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00008078 File Offset: 0x00007078
		private void WriteMonthElements(string[] names, string attributeValueTag)
		{
			this._writer.WriteStartElement("monthWidth");
			this._writer.WriteAttributeString("type", attributeValueTag);
			for (int i = 1; i <= names.Length; i++)
			{
				this.WriteElementWithAttributeAndString("month", "type", i.ToString(CultureInfo.InvariantCulture), names[i - 1]);
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000080E0 File Offset: 0x000070E0
		private void WriteDayElements(string[] names, string attributeValueTag)
		{
			if (names.Length != 7)
			{
				throw new ArgumentException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDayName"));
			}
			this._writer.WriteStartElement("dayWidth");
			this._writer.WriteAttributeString("type", attributeValueTag);
			for (int i = 1; i <= names.Length; i++)
			{
				this.WriteElementWithAttributeAndString("day", "type", Tags.DayNamesTag[i - 1], names[i - 1]);
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000815C File Offset: 0x0000715C
		private void Close(bool deleteTempFile)
		{
			if (this._writer != null)
			{
				this._writer.Flush();
				this._writer.Close();
				this._writer = null;
			}
			if (this._tempFileName != null && this._stream != null)
			{
				this._stream.Close();
				this._stream = null;
			}
			if (deleteTempFile && this._tempFileName != null)
			{
				File.Delete(this._tempFileName);
				this._tempFileName = null;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000081CD File Offset: 0x000071CD
		private void WriteElementWithString(string elementName, string text)
		{
			this.WriteElementWithString(elementName, text, false);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000081D8 File Offset: 0x000071D8
		private void WriteElementWithString(string elementName, string text, bool withNamespace)
		{
			if (withNamespace)
			{
				this._writer.WriteStartElement("msLocale", elementName, null);
			}
			else
			{
				this._writer.WriteStartElement(elementName);
			}
			this._writer.WriteString(text);
			this._writer.WriteEndElement();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00008214 File Offset: 0x00007214
		private void WriteElementWithAttributeAndString(string elementName, string attributeName, string attributeValue, string text)
		{
			this.WriteElementWithAttributeAndString(elementName, attributeName, attributeValue, text, false);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00008224 File Offset: 0x00007224
		private void WriteElementWithAttributeAndString(string elementName, string attributeName, string attributeValue, string text, bool withNamespace)
		{
			if (withNamespace)
			{
				this._writer.WriteStartElement("msLocale", elementName, null);
			}
			else
			{
				this._writer.WriteStartElement(elementName);
			}
			this._writer.WriteAttributeString(attributeName, attributeValue);
			this._writer.WriteString(text);
			this._writer.WriteEndElement();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000827A File Offset: 0x0000727A
		private void WriteElementWithAttribute(string elementName, string attributeName, string attributeValue)
		{
			this.WriteElementWithAttribute(elementName, attributeName, attributeValue, false);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00008286 File Offset: 0x00007286
		private void WriteElementWithAttribute(string elementName, string attributeName, string attributeValue, bool withNamespace)
		{
			if (withNamespace)
			{
				this._writer.WriteStartElement("msLocale", elementName, null);
			}
			else
			{
				this._writer.WriteStartElement(elementName);
			}
			this._writer.WriteAttributeString(attributeName, attributeValue);
			this._writer.WriteEndElement();
		}

		// Token: 0x04000106 RID: 262
		private CultureAndRegionInfoBuilder _carib;

		// Token: 0x04000107 RID: 263
		private XmlTextWriter _writer;

		// Token: 0x04000108 RID: 264
		private FileStream _stream;

		// Token: 0x04000109 RID: 265
		private string _tempFileName;
	}
}

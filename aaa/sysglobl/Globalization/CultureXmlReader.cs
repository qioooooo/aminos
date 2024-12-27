using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace System.Globalization
{
	// Token: 0x0200000D RID: 13
	internal sealed class CultureXmlReader
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x000082C4 File Offset: 0x000072C4
		internal CultureXmlReader(string fileName)
		{
			try
			{
				this._reader = XmlReader.Create(fileName, new XmlReaderSettings
				{
					IgnoreWhitespace = true,
					IgnoreComments = true,
					ProhibitDtd = false,
					ValidationType = ValidationType.None
				});
				this._namespaceManager = new XmlNamespaceManager(this._reader.NameTable);
				this._namespaceManager.AddNamespace("msLocale", "http://schemas.microsoft.com/globalization/2004/08/carib/ldml");
				this._document = new XmlDocument();
				try
				{
					this._document.Load(this._reader);
					this.Parse();
				}
				catch (ArgumentException ex)
				{
					throw new XmlSchemaValidationException(ex.Message, ex);
				}
				catch (XmlException ex2)
				{
					throw new XmlSchemaValidationException(ex2.Message, ex2);
				}
				catch (FormatException ex3)
				{
					throw new XmlSchemaValidationException(ex3.Message, ex3);
				}
				catch (NotSupportedException ex4)
				{
					throw new XmlSchemaValidationException(ex4.Message, ex4);
				}
				catch (InvalidOperationException ex5)
				{
					throw new XmlSchemaValidationException(ex5.Message, ex5);
				}
			}
			finally
			{
				this.Close();
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000083F8 File Offset: 0x000073F8
		internal CultureAndRegionInfoBuilder CultureAndRegionInfoBuilder
		{
			get
			{
				return this._carib;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00008400 File Offset: 0x00007400
		private void Parse()
		{
			this._root = this._document.DocumentElement;
			if (this._root == null || !this._document.DocumentElement.LocalName.Equals("ldml"))
			{
				throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDocument"));
			}
			this.ReadIdentityElement();
			this.ReadLayoutElement();
			this.ReadCharactersElement();
			this.ReadMeasurementElement();
			this.ReadDateElement();
			this.ReadNumbersElement();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00008478 File Offset: 0x00007478
		private void ReadIdentityElement()
		{
			XmlNode xmlNode = this._root.SelectSingleNode("identity");
			if (xmlNode == null)
			{
				throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDocument"));
			}
			CultureAndRegionModifiers cultureAndRegionModifiers = CultureAndRegionModifiers.None;
			StringBuilder stringBuilder = new StringBuilder();
			XmlNode specialElementNode = this.GetSpecialElementNode(xmlNode);
			string text;
			if (specialElementNode != null)
			{
				text = this.GetElementAttribute(specialElementNode, "cultureAndRegionInfoName", "type", false, true);
				if (text != null)
				{
					stringBuilder.Append(text);
					XmlNodeList xmlNodeList = specialElementNode.SelectNodes("msLocale:cultureAndRegionModifier", this._namespaceManager);
					if (xmlNodeList != null && xmlNodeList.Count > 0)
					{
						for (int i = 0; i < xmlNodeList.Count; i++)
						{
							if (xmlNodeList[i].Attributes["type"] != null && xmlNodeList[i].Attributes["type"].Value.Equals("neutral"))
							{
								cultureAndRegionModifiers = CultureAndRegionModifiers.Neutral;
								break;
							}
						}
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				text = this.GetElementAttribute(xmlNode, "language", "type", true);
				if (text == null || text.Length == 0)
				{
					throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidCultureName"));
				}
				stringBuilder.Append(text);
				text = this.GetElementAttribute(xmlNode, "script", "type", false);
				if (text != null && text.Length > 0)
				{
					stringBuilder.Append("-");
					stringBuilder.Append(text);
				}
				text = this.GetElementAttribute(xmlNode, "territory", "type", false);
				if (text != null && text.Length > 0)
				{
					stringBuilder.Append("-");
					stringBuilder.Append(text);
				}
				else
				{
					cultureAndRegionModifiers = CultureAndRegionModifiers.Neutral;
				}
				text = this.GetElementAttribute(xmlNode, "variant", "type", false);
				if (text != null && text.Length > 0)
				{
					stringBuilder.Append("-");
					stringBuilder.Append(text);
				}
			}
			text = stringBuilder.ToString();
			CultureInfo cultureInfo = null;
			try
			{
				cultureInfo = CultureInfo.GetCultureInfo(text);
			}
			catch (ArgumentException)
			{
			}
			if (cultureInfo != null && cultureInfo.LCID != 3072 && cultureInfo.LCID != 4096)
			{
				cultureAndRegionModifiers |= CultureAndRegionModifiers.Replacement;
			}
			this._carib = new CultureAndRegionInfoBuilder(text, cultureAndRegionModifiers);
			if (specialElementNode != null)
			{
				if ((cultureAndRegionModifiers & CultureAndRegionModifiers.Neutral) == CultureAndRegionModifiers.None)
				{
					text = this.GetElementText(specialElementNode, "geoId", false, true);
					if (text != null)
					{
						this._carib.GeoId = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
					}
					text = this.GetElementText(specialElementNode, "countryCode", false, true);
					if (text != null)
					{
						this._carib.CountryCode = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
					}
					text = this.GetElementAttribute(specialElementNode, "regionEnglishName", "type", false, true);
					if (text != null)
					{
						this._carib.RegionEnglishName = text;
					}
					text = this.GetElementAttribute(specialElementNode, "regionNativeName", "type", false, true);
					if (text != null)
					{
						this._carib.RegionNativeName = text;
					}
					text = this.GetElementText(specialElementNode, "regionIsoName[@type='threeLetters']", false, true);
					if (text != null)
					{
						this._carib.ThreeLetterISORegionName = text;
					}
					text = this.GetElementText(specialElementNode, "regionIsoName[@type='twoLetters']", false, true);
					if (text != null)
					{
						this._carib.TwoLetterISORegionName = text;
					}
					if (!this._carib.IsReplacementCulture)
					{
						text = this.GetElementText(specialElementNode, "regionWindowsName[@type='threeLetters']", false, true);
						if (text != null)
						{
							this._carib.ThreeLetterWindowsRegionName = text;
						}
					}
				}
				else
				{
					text = this.GetElementAttribute(specialElementNode, "specificCultureName", "type", false, true);
					if (text != null)
					{
						this._carib.SpecificCultureName = text;
					}
				}
				text = this.GetElementAttribute(specialElementNode, "parentName", "type", false, true);
				if (text != null)
				{
					this._carib.Parent = CultureInfo.GetCultureInfo(text);
				}
				if (!this._carib.IsReplacementCulture)
				{
					text = this.GetElementAttribute(specialElementNode, "languageNameAbbr", "type", false, true);
					if (text != null)
					{
						this._carib.ThreeLetterWindowsLanguageName = text;
					}
				}
				text = this.GetElementText(specialElementNode, "languageIsoName[@type='threeLetters']", false, true);
				if (text != null)
				{
					this._carib.ThreeLetterISOLanguageName = text;
				}
				text = this.GetElementText(specialElementNode, "languageIsoName[@type='twoLetters']", false, true);
				if (text != null)
				{
					this._carib.TwoLetterISOLanguageName = text;
				}
				text = this.GetElementAttribute(specialElementNode, "nativeName", "type", false, true);
				if (text != null)
				{
					this._carib.CultureNativeName = text;
				}
				if (string.IsNullOrEmpty(this._carib.CultureNativeName))
				{
					text = this.GetElementAttribute(specialElementNode, "nativeDisplayName", "type", false, true);
					if (text != null)
					{
						this._carib.CultureNativeName = text;
					}
				}
				text = this.GetElementAttribute(specialElementNode, "englishName", "type", false, true);
				if (text != null)
				{
					this._carib.CultureEnglishName = text;
				}
				text = this.GetElementAttribute(specialElementNode, "nativeDisplayName", "type", false, true);
				if (text != null)
				{
					this._carib.CultureNativeName = text;
				}
				if ((cultureAndRegionModifiers & CultureAndRegionModifiers.Replacement) == CultureAndRegionModifiers.None)
				{
					text = this.GetElementAttribute(specialElementNode, "textInfoName", "type", false, true);
					if (text != null)
					{
						this._carib.TextInfo = new CultureInfo(text).TextInfo;
					}
					text = this.GetElementAttribute(specialElementNode, "sortName", "type", false, true);
					if (text != null)
					{
						this._carib.CompareInfo = CultureInfo.GetCultureInfo(text).CompareInfo;
					}
				}
				text = this.GetElementAttribute(specialElementNode, "englishLanguage", "type", false, true);
				if (text != null && text.Length > 0)
				{
					this._carib.EnglishLanguage = text;
				}
				text = this.GetElementAttribute(specialElementNode, "nativeLanguage", "type", false, true);
				if (text != null && text.Length > 0)
				{
					this._carib.NativeLanguage = text;
				}
				text = this.GetElementAttribute(specialElementNode, "keyboardsToInstall", "type", false, true);
				if (text != null && text.Length > 0)
				{
					this._carib.KeyboardsToInstall = text;
				}
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000089F8 File Offset: 0x000079F8
		private void ReadLayoutElement()
		{
			XmlNode xmlNode = this._root.SelectSingleNode("layout");
			if (xmlNode == null)
			{
				return;
			}
			XmlNode specialElementNode = this.GetSpecialElementNode(xmlNode);
			if (specialElementNode != null)
			{
				string elementAttribute = this.GetElementAttribute(specialElementNode, "direction", "type", false, true);
				if (elementAttribute != null)
				{
					if (elementAttribute.Equals("right-to-left"))
					{
						this._carib.IsRightToLeft = true;
						return;
					}
					if (elementAttribute.Equals("left-to-right"))
					{
						this._carib.IsRightToLeft = false;
					}
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00008A70 File Offset: 0x00007A70
		private void ReadCharactersElement()
		{
			XmlNode xmlNode = this._root.SelectSingleNode("characters");
			if (xmlNode == null)
			{
				return;
			}
			XmlNode specialElementNode = this.GetSpecialElementNode(xmlNode);
			if (specialElementNode != null)
			{
				string text = this.GetElementText(specialElementNode, "keyboardLayout", false, true);
				if (text != null)
				{
					this._carib.KeyboardLayoutId = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode, "consoleFallbackName", "type", false, true);
				if (text != null)
				{
					this._carib.ConsoleFallbackUICulture = CultureInfo.GetCultureInfo(text);
				}
				this.ReadFontSignature(specialElementNode);
				this.ReadScripts(specialElementNode);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00008AFC File Offset: 0x00007AFC
		private void ReadScripts(XmlNode parentNode)
		{
			XmlNode xmlNode = parentNode.SelectSingleNode("msLocale:scripts", this._namespaceManager);
			if (xmlNode == null)
			{
				return;
			}
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("msLocale:script", this._namespaceManager);
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				if (xmlNodeList[i].Attributes["type"] != null)
				{
					stringBuilder.Append(xmlNodeList[i].Attributes["type"].Value);
					stringBuilder.Append(';');
				}
			}
			this._carib.Scripts = stringBuilder.ToString();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00008BA8 File Offset: 0x00007BA8
		private void ReadFontSignature(XmlNode parentNode)
		{
			if (this._carib.IsNeutralCulture)
			{
				return;
			}
			XmlNode xmlNode = parentNode.SelectSingleNode("msLocale:fontSignature", this._namespaceManager);
			if (xmlNode == null)
			{
				return;
			}
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("msLocale:unicodeRanges", this._namespaceManager);
			if (xmlNode2 == null)
			{
				return;
			}
			XmlNodeList xmlNodeList = xmlNode2.SelectNodes("msLocale:range", this._namespaceManager);
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return;
			}
			char[] array = new char[16];
			char[] array2 = array;
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				if (xmlNodeList[i].Attributes["type"] != null)
				{
					int num = int.Parse(xmlNodeList[i].Attributes["type"].Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
					if (num < 0 || num > 122)
					{
						return;
					}
					int num2 = num >> 4;
					int num3 = num % 16;
					array2[num2] = (char)((int)array2[num2] | (1 << num3));
				}
			}
			xmlNodeList = xmlNode2.SelectNodes("msLocale:layoutProgress", this._namespaceManager);
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				for (int j = 0; j < xmlNodeList.Count; j++)
				{
					string value;
					if (xmlNodeList[j].Attributes["type"] != null && (value = xmlNodeList[j].Attributes["type"].Value) != null)
					{
						if (!(value == "horizontalRightToLeft"))
						{
							if (!(value == "verticalBeforeHorizontal"))
							{
								if (value == "verticalBottomToTop")
								{
									array2[7] = array2[7] | '\u2000';
								}
							}
							else
							{
								array2[7] = array2[7] | 'က';
							}
						}
						else
						{
							array2[7] = array2[7] | 'ࠀ';
						}
					}
				}
			}
			array2[7] = array2[7] | '耀';
			if (this.ReadCodePages(xmlNode, ref array2, true) && this.ReadCodePages(xmlNode, ref array2, false))
			{
				this._carib.FontSignature = new string(array2);
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00008DA0 File Offset: 0x00007DA0
		private bool ReadCodePages(XmlNode parentNode, ref char[] fontSignature, bool isDefault)
		{
			int num = (isDefault ? 8 : 12);
			XmlNode xmlNode = parentNode.SelectSingleNode("msLocale:" + (isDefault ? "defaultCodePages" : "codePages"), this._namespaceManager);
			return xmlNode != null && this.ReadSpecificCodePages(xmlNode, ref fontSignature, num, 0, 16, "ansiCodePage") && this.ReadSpecificCodePages(xmlNode, ref fontSignature, num + 1, 16, 16, "ansiOemCodePage") && this.ReadSpecificCodePages(xmlNode, ref fontSignature, num + 2, 32, 32, "oemCodePage");
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00008E24 File Offset: 0x00007E24
		private bool ReadSpecificCodePages(XmlNode parentNode, ref char[] fontSignature, int startIndex, int searchStartIndex, int length, string nodeTag)
		{
			XmlNode xmlNode = parentNode.SelectSingleNode("msLocale:" + nodeTag, this._namespaceManager);
			if (xmlNode == null)
			{
				return true;
			}
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("msLocale:codePage", this._namespaceManager);
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				if (xmlNodeList[i].Attributes["type"] != null)
				{
					int num = int.Parse(xmlNodeList[i].Attributes["type"].Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
					int num2 = searchStartIndex;
					while (num2 < searchStartIndex + length && num != this._carib.CodePages[num2])
					{
						num2++;
					}
					if (num2 >= searchStartIndex + length)
					{
						return false;
					}
					int num3 = startIndex + (num2 - searchStartIndex >> 4);
					int num4 = num2 % 16;
					fontSignature[num3] = (char)((int)fontSignature[num3] | (1 << num4));
				}
			}
			return true;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00008F24 File Offset: 0x00007F24
		private void ReadMeasurementElement()
		{
			if (this._carib.IsNeutralCulture)
			{
				return;
			}
			XmlNode xmlNode = this._root.SelectSingleNode("measurement");
			if (xmlNode == null)
			{
				return;
			}
			string elementAttribute = this.GetElementAttribute(xmlNode, "measurementSystem", "type", false, false);
			string text;
			if (elementAttribute != null && (text = elementAttribute) != null)
			{
				if (!(text == "metric"))
				{
					if (text == "US")
					{
						this._carib.IsMetric = false;
					}
				}
				else
				{
					this._carib.IsMetric = true;
				}
			}
			this.ReadPaperSizeElement(xmlNode);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00008FB0 File Offset: 0x00007FB0
		private void ReadPaperSizeElement(XmlNode parentNode)
		{
			XmlNode xmlNode = parentNode.SelectSingleNode("paperSize");
			if (xmlNode == null)
			{
				return;
			}
			string text = this.GetElementText(xmlNode, "height", false, false);
			if (text == null)
			{
				return;
			}
			int num = 0;
			try
			{
				num = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				return;
			}
			catch (OverflowException)
			{
				return;
			}
			text = this.GetElementText(xmlNode, "width", false, false);
			if (text != null)
			{
				int num2 = 0;
				try
				{
					num2 = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				catch (FormatException)
				{
					return;
				}
				catch (OverflowException)
				{
					return;
				}
				if (num2 == 216 && num == 279)
				{
					this._carib.PaperSize = PaperSize.Letter;
					return;
				}
				if (num2 == 216 && num == 356)
				{
					this._carib.PaperSize = PaperSize.Legal;
					return;
				}
				if (num2 == 297 && num == 420)
				{
					this._carib.PaperSize = PaperSize.A3;
					return;
				}
				if (num2 == 210 && num == 297)
				{
					this._carib.PaperSize = PaperSize.A4;
				}
				return;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000090CC File Offset: 0x000080CC
		private void ReadDateElement()
		{
			if (this._carib.IsNeutralCulture)
			{
				return;
			}
			XmlNode xmlNode = this._root.SelectSingleNode("dates");
			if (xmlNode == null)
			{
				return;
			}
			xmlNode = xmlNode.SelectSingleNode("calendars");
			if (xmlNode == null)
			{
				return;
			}
			string elementAttribute = this.GetElementAttribute(xmlNode, "default", "type", false, false);
			DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("calendar[@type='" + CultureDefinition.CalendarTags[1] + "']");
			if (xmlNode2 != null)
			{
				PosixFormatConverter posixFormatConverter = new PosixFormatConverter();
				XmlNode xmlNode3 = xmlNode2.SelectSingleNode("months");
				if (xmlNode3 != null)
				{
					xmlNode3 = xmlNode3.SelectSingleNode("monthContext[@type='format']");
					if (xmlNode3 != null)
					{
						string[] array = this.ReadMonths(xmlNode3, "wide");
						if (array != null)
						{
							dateTimeFormatInfo.MonthNames = array;
						}
						array = this.ReadMonths(xmlNode3, "abbreviated");
						if (array != null)
						{
							dateTimeFormatInfo.AbbreviatedMonthNames = array;
						}
						array = this.ReadMonths(xmlNode3, "genitive");
						if (array != null)
						{
							dateTimeFormatInfo.MonthGenitiveNames = array;
						}
						array = this.ReadMonths(xmlNode3, "genitiveAbbreviated");
						if (array != null)
						{
							dateTimeFormatInfo.AbbreviatedMonthGenitiveNames = array;
						}
					}
				}
				xmlNode3 = xmlNode2.SelectSingleNode("days");
				if (xmlNode3 != null)
				{
					xmlNode3 = xmlNode3.SelectSingleNode("dayContext[@type='format']");
					if (xmlNode3 != null)
					{
						string[] array2 = this.ReadDays(xmlNode3, "wide");
						if (array2 != null)
						{
							dateTimeFormatInfo.DayNames = array2;
						}
						array2 = this.ReadDays(xmlNode3, "abbreviated");
						if (array2 != null)
						{
							dateTimeFormatInfo.AbbreviatedDayNames = array2;
						}
						array2 = this.ReadDays(xmlNode3, "shortest");
						if (array2 != null)
						{
							dateTimeFormatInfo.ShortestDayNames = array2;
						}
					}
				}
				xmlNode3 = xmlNode2.SelectSingleNode("week");
				string text;
				if (xmlNode3 != null)
				{
					text = this.GetElementAttribute(xmlNode3, "firstDay", "day", false);
					if (text != null)
					{
						for (int i = 0; i < Tags.DayNamesTag.Length; i++)
						{
							if (Tags.DayNamesTag[i].Equals(text))
							{
								dateTimeFormatInfo.FirstDayOfWeek = (DayOfWeek)i;
								break;
							}
						}
					}
					xmlNode3 = this.GetSpecialElementNode(xmlNode3);
					if (xmlNode3 != null)
					{
						text = this.GetElementAttribute(xmlNode3, "weekRule", "type", false, true);
						if (text != null)
						{
							for (int j = 0; j < Tags.AllWeekRuleTag.Length; j++)
							{
								if (Tags.AllWeekRuleTag[j].Equals(text))
								{
									dateTimeFormatInfo.CalendarWeekRule = (CalendarWeekRule)j;
									break;
								}
							}
						}
					}
				}
				text = this.GetElementText(xmlNode2, "am", false, false);
				if (text != null)
				{
					dateTimeFormatInfo.AMDesignator = text;
				}
				text = this.GetElementText(xmlNode2, "pm", false, false);
				if (text != null)
				{
					dateTimeFormatInfo.PMDesignator = text;
				}
				xmlNode3 = xmlNode2.SelectSingleNode("dateFormats");
				if (xmlNode3 != null)
				{
					bool flag = false;
					bool flag2 = false;
					XmlNode specialElementNode = this.GetSpecialElementNode(xmlNode3);
					if (specialElementNode != null)
					{
						string[] array3 = this.GetDateTimeFormat(specialElementNode, "dateFormatLength", "dateFormat", "long", true);
						if (array3 != null)
						{
							dateTimeFormatInfo.SetAllDateTimePatterns(array3, 'D');
							flag = true;
						}
						array3 = this.GetDateTimeFormat(specialElementNode, "dateFormatLength", "dateFormat", "short", true);
						if (array3 != null)
						{
							dateTimeFormatInfo.SetAllDateTimePatterns(array3, 'd');
							flag2 = true;
						}
						array3 = this.GetDateTimeFormat(specialElementNode, "yearMonthFormat", "yearMonth", null, true);
						if (array3 != null)
						{
							dateTimeFormatInfo.SetAllDateTimePatterns(array3, 'y');
						}
						XmlNode xmlNode4 = specialElementNode.SelectSingleNode("msLocale:monthDay", this._namespaceManager);
						if (xmlNode4 != null)
						{
							text = this.GetElementText(xmlNode4, "pattern", false, true);
							if (text != null)
							{
								dateTimeFormatInfo.MonthDayPattern = text;
							}
						}
					}
					if (!flag)
					{
						string[] array3 = this.GetDateTimeFormat(xmlNode3, "dateFormatLength", "dateFormat", "long", false);
						if (array3 != null)
						{
							for (int k = 0; k < array3.Length; k++)
							{
								array3[k] = posixFormatConverter.ConvertFromPosix(array3[k]);
							}
							dateTimeFormatInfo.SetAllDateTimePatterns(array3, 'D');
						}
					}
					if (!flag2)
					{
						string[] array3 = this.GetDateTimeFormat(xmlNode3, "dateFormatLength", "dateFormat", "short", false);
						if (array3 != null)
						{
							for (int l = 0; l < array3.Length; l++)
							{
								array3[l] = posixFormatConverter.ConvertFromPosix(array3[l]);
							}
							dateTimeFormatInfo.SetAllDateTimePatterns(array3, 'd');
						}
					}
				}
				xmlNode3 = xmlNode2.SelectSingleNode("timeFormats");
				if (xmlNode3 != null)
				{
					string[] array4 = null;
					XmlNode specialElementNode2 = this.GetSpecialElementNode(xmlNode3);
					if (specialElementNode2 != null)
					{
						string[] dateTimeFormat = this.GetDateTimeFormat(specialElementNode2, "durationFormats", "durationFormat", null, true);
						if (dateTimeFormat != null)
						{
							this._carib.DurationFormats = dateTimeFormat;
						}
						array4 = this.GetDateTimeFormat(specialElementNode2, "timeFormatLength", "timeFormat", "long", true);
						if (array4 != null)
						{
							dateTimeFormatInfo.SetAllDateTimePatterns(array4, 'T');
						}
					}
					if (array4 == null)
					{
						array4 = this.GetDateTimeFormat(xmlNode3, "timeFormatLength", "timeFormat", "long", false);
						if (array4 != null)
						{
							for (int m = 0; m < array4.Length; m++)
							{
								array4[m] = posixFormatConverter.ConvertFromPosix(array4[m]);
							}
							dateTimeFormatInfo.SetAllDateTimePatterns(array4, 'T');
						}
					}
					array4 = null;
					if (specialElementNode2 != null)
					{
						array4 = this.GetDateTimeFormat(specialElementNode2, "timeFormatLength", "timeFormat", "short", true);
						if (array4 != null)
						{
							dateTimeFormatInfo.SetAllDateTimePatterns(array4, 't');
						}
					}
					if (array4 == null)
					{
						array4 = this.GetDateTimeFormat(xmlNode3, "timeFormatLength", "timeFormat", "short", false);
						if (array4 != null)
						{
							for (int n = 0; n < array4.Length; n++)
							{
								array4[n] = posixFormatConverter.ConvertFromPosix(array4[n]);
							}
							dateTimeFormatInfo.SetAllDateTimePatterns(array4, 't');
						}
					}
				}
				this._carib.GregorianDateTimeFormat = dateTimeFormatInfo;
			}
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("calendar");
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				ArrayList arrayList = new ArrayList(2);
				string[] nativeCalendarNames = this._carib.NativeCalendarNames;
				for (int num = 0; num < xmlNodeList.Count; num++)
				{
					if (xmlNodeList[num].Attributes["type"] != null)
					{
						string value = xmlNodeList[num].Attributes["type"].Value;
						Calendar calendarFromTag = CultureDefinition.GetCalendarFromTag(value);
						if (calendarFromTag != null)
						{
							if (value.Equals(elementAttribute))
							{
								arrayList.Insert(0, calendarFromTag);
							}
							else
							{
								arrayList.Add(calendarFromTag);
							}
							CalendarId calendarId = CultureDefinition.CalendarIdOfCalendar(calendarFromTag);
							XmlNode specialElementNode3 = this.GetSpecialElementNode(xmlNodeList[num]);
							if (specialElementNode3 != null)
							{
								nativeCalendarNames[(int)(calendarId - CalendarId.GREGORIAN)] = this.GetElementAttribute(specialElementNode3, "calendarNativeName", "type", false, true);
							}
						}
					}
				}
				this._carib.NativeCalendarNames = nativeCalendarNames;
				if (arrayList.Count > 0)
				{
					this._carib.AvailableCalendars = (Calendar[])arrayList.ToArray(Type.GetType("System.Globalization.Calendar"));
				}
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009738 File Offset: 0x00008738
		private void ReadNumbersElement()
		{
			if (this._carib.IsNeutralCulture)
			{
				return;
			}
			XmlNode xmlNode = this._root.SelectSingleNode("numbers");
			if (xmlNode == null)
			{
				return;
			}
			NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("symbols");
			if (xmlNode2 != null)
			{
				string text = this.GetElementText(xmlNode2, "decimal", false, false);
				if (text != null)
				{
					numberFormatInfo.NumberDecimalSeparator = text;
				}
				text = this.GetElementText(xmlNode2, "group", false, false);
				if (text != null)
				{
					numberFormatInfo.NumberGroupSeparator = text;
				}
				if (!this._carib.IsReplacementCulture && this._carib.TextInfo != null)
				{
					text = this.GetElementText(xmlNode2, "list", false, false);
					if (text != null)
					{
						TextInfo textInfo = this._carib.TextInfo;
						textInfo.ListSeparator = text;
						this._carib.TextInfo = textInfo;
					}
				}
				text = this.GetElementText(xmlNode2, "percentSign", false, false);
				if (text != null)
				{
					numberFormatInfo.PercentSymbol = text;
				}
				text = this.GetElementText(xmlNode2, "infinity[@type='positive']", false, false);
				if (text != null)
				{
					numberFormatInfo.PositiveInfinitySymbol = text;
				}
				text = this.GetElementText(xmlNode2, "infinity[@type='negative']", false, false);
				if (text != null)
				{
					numberFormatInfo.NegativeInfinitySymbol = text;
				}
				text = this.GetElementText(xmlNode2, "nan", false, false);
				if (text != null)
				{
					numberFormatInfo.NaNSymbol = text;
				}
				text = this.GetElementText(xmlNode2, "PlusSign", false, false);
				if (text != null)
				{
					numberFormatInfo.PositiveSign = text;
					if (text == "+")
					{
						this._carib.ForceWindowsPositivePlus = true;
					}
				}
				if (string.IsNullOrEmpty(numberFormatInfo.PositiveSign))
				{
					numberFormatInfo.PositiveSign = "+";
				}
				text = this.GetElementText(xmlNode2, "minusSign", false, false);
				if (text != null)
				{
					numberFormatInfo.NegativeSign = text;
				}
				XmlNode specialElementNode = this.GetSpecialElementNode(xmlNode2);
				if (specialElementNode != null)
				{
					text = this.GetElementText(specialElementNode, "currencyDecimalSeparator", false, true);
					if (text != null)
					{
						numberFormatInfo.CurrencyDecimalSeparator = text;
					}
					text = this.GetElementText(specialElementNode, "currencyGroupSeparator", false, true);
					if (text != null)
					{
						numberFormatInfo.CurrencyGroupSeparator = text;
					}
				}
			}
			xmlNode2 = xmlNode.SelectSingleNode("currencies");
			if (xmlNode2 != null)
			{
				xmlNode2 = xmlNode2.SelectSingleNode("currency[@type='default']");
				if (xmlNode2 != null)
				{
					string text = this.GetElementText(xmlNode2, "symbol", false, false);
					if (text != null)
					{
						numberFormatInfo.CurrencySymbol = text;
					}
					text = this.GetElementText(xmlNode2, "displayName", false, false);
					if (text != null)
					{
						this._carib.CurrencyNativeName = text;
					}
					XmlNode specialElementNode2 = this.GetSpecialElementNode(xmlNode2);
					if (specialElementNode2 != null)
					{
						text = this.GetElementText(specialElementNode2, "isoCurrency", false, true);
						if (text != null)
						{
							this._carib.ISOCurrencySymbol = text;
						}
						text = this.GetElementText(specialElementNode2, "currencyEnglishName", false, true);
						if (text != null)
						{
							this._carib.CurrencyEnglishName = text;
						}
					}
				}
			}
			XmlNode specialElementNode3 = this.GetSpecialElementNode(xmlNode);
			if (specialElementNode3 != null)
			{
				string text = this.GetElementText(specialElementNode3, "decimalDigits", false, true);
				if (text != null)
				{
					numberFormatInfo.NumberDecimalDigits = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode3, "negativePattern", "type", false, true);
				if (text != null)
				{
					numberFormatInfo.NumberNegativePattern = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode3, "currencyDecimalDigits", "type", false, true);
				if (text != null)
				{
					numberFormatInfo.CurrencyDecimalDigits = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode3, "currencyPositivePattern", "type", false, true);
				if (text != null)
				{
					numberFormatInfo.CurrencyPositivePattern = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode3, "currencyNegativePattern", "type", false, true);
				if (text != null)
				{
					numberFormatInfo.CurrencyNegativePattern = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode3, "percentNegativePattern", "type", false, true);
				if (text != null)
				{
					numberFormatInfo.PercentNegativePattern = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				text = this.GetElementAttribute(specialElementNode3, "percentPositivePattern", "type", false, true);
				if (text != null)
				{
					numberFormatInfo.PercentPositivePattern = int.Parse(text, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				int[] array = this.GetPatternsInt(specialElementNode3, "groupSizes");
				if (array != null)
				{
					numberFormatInfo.NumberGroupSizes = array;
				}
				array = this.GetPatternsInt(specialElementNode3, "currencyGroupSizes");
				if (array != null)
				{
					numberFormatInfo.CurrencyGroupSizes = array;
				}
				string[] patterns = this.GetPatterns(specialElementNode3, "nativeDigits");
				if (patterns != null)
				{
					numberFormatInfo.NativeDigits = patterns;
				}
				text = this.GetElementAttribute(specialElementNode3, "leadingZero", "type", false, true);
				if (text != null)
				{
					if (text.Equals("yes"))
					{
						this._carib.LeadingZero = true;
					}
					else
					{
						if (!text.Equals("no"))
						{
							throw new XmlSchemaValidationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidLeadingZero"), new object[] { text }));
						}
						this._carib.LeadingZero = false;
					}
				}
				text = this.GetElementAttribute(specialElementNode3, "digitSubstitution", "type", false, true);
				if (text != null)
				{
					string text2;
					if ((text2 = text) != null)
					{
						if (text2 == "context")
						{
							numberFormatInfo.DigitSubstitution = DigitShapes.Context;
							goto IL_04F0;
						}
						if (text2 == "none")
						{
							numberFormatInfo.DigitSubstitution = DigitShapes.None;
							goto IL_04F0;
						}
						if (text2 == "nativeNational")
						{
							numberFormatInfo.DigitSubstitution = DigitShapes.NativeNational;
							goto IL_04F0;
						}
					}
					throw new XmlSchemaValidationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDigitSubstitution"), new object[] { text }));
				}
			}
			IL_04F0:
			this._carib.NumberFormat = numberFormatInfo;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009C44 File Offset: 0x00008C44
		private int[] GetPatternsInt(XmlNode parentNode, string type)
		{
			XmlNodeList xmlNodeList = parentNode.SelectNodes("msLocale:" + type, this._namespaceManager);
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return null;
			}
			int[] array = new int[xmlNodeList.Count];
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				if (xmlNodeList[i].Attributes["type"] != null)
				{
					int num = int.Parse(xmlNodeList[i].Attributes["type"].Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
					if (num < array.Length)
					{
						XmlNode lastChild = xmlNodeList[i].LastChild;
						if (lastChild != null)
						{
							array[num] = int.Parse(lastChild.Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
						}
					}
				}
			}
			return array;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009D00 File Offset: 0x00008D00
		private string[] GetPatterns(XmlNode parentNode, string type)
		{
			XmlNodeList xmlNodeList = parentNode.SelectNodes("msLocale:" + type, this._namespaceManager);
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return null;
			}
			string[] array = new string[xmlNodeList.Count];
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				if (xmlNodeList[i].Attributes["type"] != null)
				{
					int num = int.Parse(xmlNodeList[i].Attributes["type"].Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
					if (num < array.Length)
					{
						XmlNode lastChild = xmlNodeList[i].LastChild;
						if (lastChild != null)
						{
							array[num] = lastChild.Value;
						}
					}
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == null)
				{
					throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDocument"));
				}
			}
			return array;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009DDC File Offset: 0x00008DDC
		private string[] GetDateTimeFormat(XmlNode parentNode, string elementTag, string subElementTag, string typeTag, bool nameSpaced)
		{
			XmlNode xmlNode;
			if (nameSpaced)
			{
				if (typeTag != null)
				{
					xmlNode = parentNode.SelectSingleNode(string.Concat(new string[] { "msLocale:", elementTag, "[@type='", typeTag, "']" }), this._namespaceManager);
				}
				else
				{
					xmlNode = parentNode.SelectSingleNode("msLocale:" + elementTag, this._namespaceManager);
				}
			}
			else if (typeTag != null)
			{
				xmlNode = parentNode.SelectSingleNode(elementTag + "[@type='" + typeTag + "']");
			}
			else
			{
				xmlNode = parentNode.SelectSingleNode(elementTag);
			}
			if (xmlNode == null)
			{
				return null;
			}
			XmlNodeList xmlNodeList = (nameSpaced ? xmlNode.SelectNodes("msLocale:" + subElementTag, this._namespaceManager) : xmlNode.SelectNodes(subElementTag));
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList(xmlNodeList.Count);
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				if (xmlNodeList[i].Attributes["type"] != null && xmlNodeList[i].Attributes["type"].Value != null)
				{
					string text = this.GetElementText(xmlNodeList[i], "pattern", false, nameSpaced);
					if (text != null)
					{
						arrayList.Add(text);
					}
				}
			}
			if (arrayList.Count == 0)
			{
				throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDocument"));
			}
			string[] array = new string[arrayList.Count];
			for (int j = 0; j < arrayList.Count; j++)
			{
				array[j] = (string)arrayList[j];
			}
			XmlNode xmlNode2 = (nameSpaced ? xmlNode.SelectSingleNode("msLocale:default", this._namespaceManager) : xmlNode.SelectSingleNode("default"));
			if (xmlNode2 != null && xmlNode2.Attributes["type"] != null)
			{
				string text = xmlNode2.Attributes["type"].Value;
				if (text != null)
				{
					xmlNode2 = (nameSpaced ? xmlNode.SelectSingleNode(string.Concat(new string[] { "msLocale:", subElementTag, "[@type='", text, "']" }), this._namespaceManager) : xmlNode.SelectSingleNode(subElementTag + "[@type='" + text + "']"));
					if (xmlNode2 != null)
					{
						text = this.GetElementText(xmlNode2, "pattern", false, nameSpaced);
						if (text != null)
						{
							for (int k = 0; k < array.Length; k++)
							{
								if (text.Equals(array[k]))
								{
									if (k == 0)
									{
										break;
									}
									for (int l = k; l > 0; l--)
									{
										array[l] = array[l - 1];
									}
									array[0] = text;
								}
							}
						}
					}
				}
			}
			return array;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000A094 File Offset: 0x00009094
		private string[] ReadDays(XmlNode parentNode, string typeTag)
		{
			XmlNode xmlNode = parentNode.SelectSingleNode("dayWidth[@type='" + typeTag + "']");
			if (xmlNode == null)
			{
				return null;
			}
			string[] array = new string[Tags.DayNamesTag.Length];
			for (int i = 0; i < Tags.DayNamesTag.Length; i++)
			{
				array[i] = this.GetElementText(xmlNode, "day[@type='" + Tags.DayNamesTag[i] + "']", true, false);
			}
			return array;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000A100 File Offset: 0x00009100
		private string[] ReadMonths(XmlNode parentNode, string typeTag)
		{
			XmlNode xmlNode = parentNode.SelectSingleNode("monthWidth[@type='" + typeTag + "']");
			if (xmlNode == null)
			{
				return null;
			}
			string[] array = new string[13];
			for (int i = 1; i <= 12; i++)
			{
				array[i - 1] = this.GetElementText(xmlNode, "month[@type='" + i.ToString(CultureInfo.InvariantCulture) + "']", true, false);
			}
			array[12] = this.GetElementText(xmlNode, "month[@type='13']", false, false);
			if (array[12] == null)
			{
				array[12] = "";
			}
			return array;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000A18C File Offset: 0x0000918C
		private XmlNode GetSpecialElementNode(XmlNode parentNode)
		{
			XmlNodeList xmlNodeList = parentNode.SelectNodes("special");
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				XmlAttributeCollection attributes = xmlNodeList[i].Attributes;
				for (int j = 0; j < attributes.Count; j++)
				{
					if (attributes[j].LocalName.Equals("msLocale"))
					{
						return xmlNodeList[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000A202 File Offset: 0x00009202
		private string GetElementAttribute(XmlNode parentNode, string elementName, string attributeName, bool shouldThrow)
		{
			return this.GetElementAttribute(parentNode, elementName, attributeName, shouldThrow, false);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000A210 File Offset: 0x00009210
		private string GetElementAttribute(XmlNode parentNode, string elementName, string attributeName, bool shouldThrow, bool nameSpaced)
		{
			XmlNode xmlNode;
			if (nameSpaced)
			{
				xmlNode = parentNode.SelectSingleNode("msLocale:" + elementName, this._namespaceManager);
			}
			else
			{
				xmlNode = parentNode.SelectSingleNode(elementName);
			}
			if (xmlNode != null)
			{
				string value = xmlNode.Attributes[attributeName].Value;
				if (value != null)
				{
					return value;
				}
			}
			if (shouldThrow)
			{
				throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDocument"));
			}
			return null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000A274 File Offset: 0x00009274
		private string GetElementText(XmlNode parentNode, string elementName, bool shouldThrow, bool nameSpaced)
		{
			XmlNode xmlNode;
			if (nameSpaced)
			{
				xmlNode = parentNode.SelectSingleNode("msLocale:" + elementName, this._namespaceManager);
			}
			else
			{
				xmlNode = parentNode.SelectSingleNode(elementName);
			}
			if (xmlNode != null)
			{
				xmlNode = xmlNode.LastChild;
				if (xmlNode == null)
				{
					return string.Empty;
				}
			}
			if (xmlNode != null && xmlNode.Value != null)
			{
				return xmlNode.Value;
			}
			if (shouldThrow)
			{
				throw new XmlSchemaValidationException(CultureAndRegionInfoBuilder.GetResourceString("Xml_InvalidDocument"));
			}
			return null;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000A2E0 File Offset: 0x000092E0
		private void Close()
		{
			if (this._reader != null)
			{
				this._reader.Close();
			}
		}

		// Token: 0x0400010A RID: 266
		private const NumberStyles IntegerStyle = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

		// Token: 0x0400010B RID: 267
		private CultureAndRegionInfoBuilder _carib;

		// Token: 0x0400010C RID: 268
		private XmlReader _reader;

		// Token: 0x0400010D RID: 269
		private XmlDocument _document;

		// Token: 0x0400010E RID: 270
		private XmlElement _root;

		// Token: 0x0400010F RID: 271
		private XmlNamespaceManager _namespaceManager;
	}
}

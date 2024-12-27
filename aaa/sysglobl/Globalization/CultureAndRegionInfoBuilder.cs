using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000006 RID: 6
	[ComVisible(false)]
	public sealed class CultureAndRegionInfoBuilder
	{
		// Token: 0x0600001C RID: 28
		[DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowsDirectory(StringBuilder sb, int length);

		// Token: 0x0600001D RID: 29 RVA: 0x00003ADC File Offset: 0x00002ADC
		public CultureAndRegionInfoBuilder(string cultureName, CultureAndRegionModifiers flags)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("cultureName");
			}
			if (cultureName.Length <= 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidCultureName"), new object[] { cultureName }), "cultureName");
			}
			if ((flags & ~(CultureAndRegionModifiers.Neutral | CultureAndRegionModifiers.Replacement)) != CultureAndRegionModifiers.None)
			{
				throw new ArgumentException(CultureAndRegionInfoBuilder.GetResourceString("InvalidModifierValue"), "flags");
			}
			CultureAndRegionInfoBuilder.ValidateCulturePiece(cultureName, "cultureName", 84, true, true);
			this.m_name = cultureName;
			CultureInfo cultureInfo = null;
			try
			{
				cultureInfo = CultureInfo.GetCultureInfo(this.m_name);
			}
			catch (ArgumentException)
			{
			}
			this.m_LCID = 4096;
			this.m_cultureTypes |= (((flags & CultureAndRegionModifiers.Neutral) != CultureAndRegionModifiers.None) ? CultureTypes.NeutralCultures : CultureTypes.SpecificCultures);
			if ((flags & CultureAndRegionModifiers.Replacement) != CultureAndRegionModifiers.None)
			{
				if (cultureInfo == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_ReplaceNonExistingCulture"), new object[] { this.m_name }));
				}
				CultureTypes cultureTypes = cultureInfo.CultureTypes;
				this.m_LCID = cultureInfo.LCID;
				if ((cultureTypes & CultureTypes.UserCustomCulture) != (CultureTypes)0 && (cultureTypes & CultureTypes.ReplacementCultures) == (CultureTypes)0)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_OverrideCustomNonReplacementCulture"), new object[] { this.m_name }));
				}
				if (cultureInfo.IsNeutralCulture != ((flags & CultureAndRegionModifiers.Neutral) != CultureAndRegionModifiers.None))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InconsistentNeutralFlag"), new object[] { this.m_name }));
				}
				if (!cultureInfo.Name.Equals(this.m_name, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_CannotReplaceAltSortName"), new object[] { this.m_name, cultureInfo.Name }));
				}
				this.m_cultureTypes |= CultureTypes.ReplacementCultures | (cultureTypes & (CultureTypes.InstalledWin32Cultures | CultureTypes.WindowsOnlyCultures | CultureTypes.FrameworkCultures));
				this.m_defaultCalendar = cultureInfo.OptionalCalendars[0];
				this.LoadDataFromCultureInfo(cultureInfo);
				this.m_compareInfo = cultureInfo.CompareInfo;
				this.m_textInfo = (TextInfo)cultureInfo.TextInfo.Clone();
				this.m_threeLetterWindowsLanguageName = cultureInfo.ThreeLetterWindowsLanguageName;
				this.m_name = cultureInfo.Name;
				if (!this.IsNeutralCulture)
				{
					RegionInfo regionInfo = new RegionInfo(cultureInfo.LCID);
					this.LoadDataFromRegionInfo(regionInfo);
					this.m_threeLetterWindowsRegionName = regionInfo.ThreeLetterWindowsRegionName;
					this.m_actualRegionName = regionInfo.Name;
					return;
				}
				this.m_calendars = cultureInfo.OptionalCalendars;
				return;
			}
			else
			{
				if (cultureInfo != null)
				{
					CultureTypes cultureTypes2 = cultureInfo.CultureTypes;
					if ((cultureTypes2 & CultureTypes.UserCustomCulture) == (CultureTypes)0)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_ReplacingWithNoFlag"), new object[] { this.m_name }));
					}
					if ((cultureTypes2 & CultureTypes.ReplacementCultures) != (CultureTypes)0)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_OverrideCustomReplacementCulture"), new object[] { this.m_name }));
					}
					this.LoadDataFromCultureInfo(cultureInfo);
					if (!this.IsNeutralCulture)
					{
						this.LoadDataFromRegionInfo(new RegionInfo(this.m_name));
					}
					this.m_name = cultureInfo.Name;
				}
				if ((flags & CultureAndRegionModifiers.Neutral) != CultureAndRegionModifiers.None)
				{
					this.m_calendars = new Calendar[]
					{
						new GregorianCalendar(GregorianCalendarTypes.Localized)
					};
					return;
				}
				if (cultureInfo == null)
				{
					this.SpecificCultureName = this.m_name;
				}
				return;
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003E84 File Offset: 0x00002E84
		public void LoadDataFromCultureInfo(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.CultureEnglishName = culture.EnglishName;
			this.CultureNativeName = culture.NativeName;
			this.ThreeLetterISOLanguageName = culture.ThreeLetterISOLanguageName;
			this.TwoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			this.KeyboardLayoutId = culture.KeyboardLayoutId;
			this.Parent = culture.Parent;
			if (!this.IsReplacementCulture)
			{
				this.m_compareInfo = culture.CompareInfo;
				this.m_textInfo = (TextInfo)culture.TextInfo.Clone();
				this.ThreeLetterWindowsLanguageName = culture.ThreeLetterWindowsLanguageName;
			}
			this.IsRightToLeft = culture.TextInfo.IsRightToLeft;
			this.m_consoleFallbackCulture = culture.GetConsoleFallbackUICulture();
			if (!this.IsNeutralCulture && !culture.IsNeutralCulture)
			{
				DateTimeFormatInfo dateTimeFormatInfo = culture.DateTimeFormat;
				if (!(dateTimeFormatInfo.Calendar is GregorianCalendar) || ((GregorianCalendar)dateTimeFormatInfo.Calendar).CalendarType != GregorianCalendarTypes.Localized)
				{
					dateTimeFormatInfo = (DateTimeFormatInfo)culture.DateTimeFormat.Clone();
					try
					{
						dateTimeFormatInfo.Calendar = new GregorianCalendar(GregorianCalendarTypes.Localized);
					}
					catch (ArgumentOutOfRangeException)
					{
					}
				}
				this.GregorianDateTimeFormat = dateTimeFormatInfo;
				this.NumberFormat = culture.NumberFormat;
				string win32LocaleInfo = CultureAndRegionInfoBuilder.GetWin32LocaleInfo(culture, 93);
				if (!string.IsNullOrEmpty(win32LocaleInfo))
				{
					this.DurationFormats = CultureAndRegionInfoBuilder.ReescapeWin32Strings(new string[] { win32LocaleInfo });
				}
				this.m_paperSize = CultureAndRegionInfoBuilder.GetWin32PaperSize(culture);
				string win32FontSignature = CultureAndRegionInfoBuilder.GetWin32FontSignature(culture);
				if (win32FontSignature != null)
				{
					this.FontSignature = win32FontSignature;
				}
				this.CountryCode = CultureAndRegionInfoBuilder.GetLocaleInfoInt(culture, 10);
				int localeInfoInt = CultureAndRegionInfoBuilder.GetLocaleInfoInt(culture, 18);
				CultureAndRegionInfoBuilder.ValidateLeadingZero(localeInfoInt);
				this.LeadingZero = localeInfoInt == 1;
			}
			if (!culture.IsNeutralCulture)
			{
				string win32LocaleInfo2 = CultureAndRegionInfoBuilder.GetWin32LocaleInfo(culture, 4097);
				if (!string.IsNullOrEmpty(win32LocaleInfo2))
				{
					this.EnglishLanguage = win32LocaleInfo2;
				}
				string win32LocaleInfo3 = CultureAndRegionInfoBuilder.GetWin32LocaleInfo(culture, 4);
				if (!string.IsNullOrEmpty(win32LocaleInfo3))
				{
					this.NativeLanguage = win32LocaleInfo3;
				}
				string win32LocaleInfo4 = CultureAndRegionInfoBuilder.GetWin32LocaleInfo(culture, 108);
				if (win32LocaleInfo4 != null)
				{
					this.Scripts = win32LocaleInfo4;
				}
				string win32LocaleInfo5 = CultureAndRegionInfoBuilder.GetWin32LocaleInfo(culture, 94);
				if (!string.IsNullOrEmpty(win32LocaleInfo5))
				{
					this.KeyboardsToInstall = win32LocaleInfo5;
				}
			}
			if (!this.IsNeutralCulture)
			{
				this.AvailableCalendars = this.GetCompleteCalendarList(culture.OptionalCalendars);
				this.SpecificCultureName = this.CultureName;
				if (!culture.IsNeutralCulture)
				{
					DateTimeFormatInfo dateTimeFormatInfo2 = (DateTimeFormatInfo)culture.DateTimeFormat.Clone();
					for (int i = 0; i < this.m_calendars.Length; i++)
					{
						try
						{
							dateTimeFormatInfo2.Calendar = this.m_calendars[i];
							CalendarId calendarId = CultureDefinition.CalendarIdOfCalendar(this.m_calendars[i]);
							this.m_calendarNames[(int)(calendarId - CalendarId.GREGORIAN)] = dateTimeFormatInfo2.NativeCalendarName;
						}
						catch (ArgumentOutOfRangeException)
						{
						}
					}
					return;
				}
			}
			else if (culture.IsNeutralCulture)
			{
				if ((culture.LCID & 1023) != 4)
				{
					this.SpecificCultureName = CultureInfo.CreateSpecificCulture(culture.Name).Name;
					return;
				}
			}
			else
			{
				this.SpecificCultureName = culture.Name;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000417C File Offset: 0x0000317C
		public void LoadDataFromRegionInfo(RegionInfo region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			if (this.IsNeutralCulture)
			{
				throw new ArgumentException(CultureAndRegionInfoBuilder.GetResourceString("Argument_SettingRegionInfoForNuetralCulture"));
			}
			if (!this.IsReplacementCulture)
			{
				this.ThreeLetterWindowsRegionName = region.ThreeLetterWindowsRegionName;
			}
			this.RegionEnglishName = region.EnglishName;
			this.RegionNativeName = region.NativeName;
			this.ThreeLetterISORegionName = region.ThreeLetterISORegionName;
			this.TwoLetterISORegionName = region.TwoLetterISORegionName;
			this.IsMetric = region.IsMetric;
			this.ISOCurrencySymbol = region.ISOCurrencySymbol;
			this.GeoId = region.GeoId;
			this.CurrencyNativeName = region.CurrencyNativeName;
			this.CurrencyEnglishName = region.CurrencyEnglishName;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004230 File Offset: 0x00003230
		private void ValidateParent(CultureInfo parent)
		{
			try
			{
				CultureInfo.GetCultureInfo(parent.Name);
			}
			catch (ArgumentException)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidOperation_InvalidParent"), new object[] { parent.Name }));
			}
			CultureInfo cultureInfo = parent;
			int num = 0;
			while (num <= 10 && cultureInfo.LCID != 127)
			{
				if (this.m_name.Equals(cultureInfo.Name, StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidSelfReference"), new object[] { parent.Name, this.m_name }), "Parent");
				}
				num++;
				cultureInfo = cultureInfo.Parent;
			}
			if (num > 10)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_FallbackTooLarge"), new object[] { 10, parent.Name }), "Parent");
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00004338 File Offset: 0x00003338
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00004340 File Offset: 0x00003340
		public CultureInfo Parent
		{
			get
			{
				return this.m_Parent;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Parent");
				}
				this.ValidateParent(value);
				this.m_Parent = (CultureInfo)value.Clone();
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00004368 File Offset: 0x00003368
		internal void ValidateDateTimeFormat(DateTimeFormatInfo format)
		{
			this.VerifyArrayStringSizes("GregorianDateTimeFormat.AbbreviatedDayNames", format.AbbreviatedDayNames, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifyArrayStringSizes("GregorianDateTimeFormat.AbbreviatedMonthNames", format.AbbreviatedMonthNames, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifyArrayStringSizes("GregorianDateTimeFormat.AbbreviatedMonthGenitiveNames", format.AbbreviatedMonthGenitiveNames, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.DateSeparator", format.DateSeparator.Length, CultureAndRegionInfoBuilder.MAX.SLIST);
			this.VerifyArrayStringSizes("GregorianDateTimeFormat.DayNames", format.DayNames, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.LongDatePattern", format.LongDatePattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.MonthDayPattern", format.MonthDayPattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifyArrayStringSizes("GregorianDateTimeFormat.MonthNames", format.MonthNames, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifyArrayStringSizes("GregorianDateTimeFormat.MonthGenitiveNames", format.MonthGenitiveNames, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.ShortDatePattern", format.ShortDatePattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.YearMonthPattern", format.YearMonthPattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.AMDesignator", format.AMDesignator.Length, CultureAndRegionInfoBuilder.MAX.S1159);
			this.VerifySize("GregorianDateTimeFormat.LongTimePattern", format.LongTimePattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.PMDesignator", format.PMDesignator.Length, CultureAndRegionInfoBuilder.MAX.S1159);
			this.VerifySize("GregorianDateTimeFormat.ShortTimePattern", format.ShortTimePattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			this.VerifySize("GregorianDateTimeFormat.TimeSeparator", format.TimeSeparator.Length, CultureAndRegionInfoBuilder.MAX.SLIST);
			this.VerifySize("GregorianDateTimeFormat.FullDateTimePattern", format.FullDateTimePattern.Length, CultureAndRegionInfoBuilder.MAX.STIMEFORMAT);
			switch (format.CalendarWeekRule)
			{
			case CalendarWeekRule.FirstDay:
			case CalendarWeekRule.FirstFullWeek:
			case CalendarWeekRule.FirstFourDayWeek:
				switch (format.FirstDayOfWeek)
				{
				case DayOfWeek.Sunday:
				case DayOfWeek.Monday:
				case DayOfWeek.Tuesday:
				case DayOfWeek.Wednesday:
				case DayOfWeek.Thursday:
				case DayOfWeek.Friday:
				case DayOfWeek.Saturday:
					return;
				default:
					throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_PropertyValueIncorrect"), new object[]
					{
						"GregorianDateTimeFormatInfo.FirstDayOfWeek",
						DayOfWeek.Saturday
					}));
				}
				break;
			default:
				throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_PropertyValueIncorrect"), new object[]
				{
					"GregorianDateTimeFormatInfo.CalendarWeekRule",
					CalendarWeekRule.FirstFourDayWeek
				}));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00004595 File Offset: 0x00003595
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000045A8 File Offset: 0x000035A8
		public DateTimeFormatInfo GregorianDateTimeFormat
		{
			get
			{
				this.CheckNeutral("GregorianDateTimeFormat");
				return this.m_dateTimeFormat;
			}
			set
			{
				this.CheckNeutral("GregorianDateTimeFormat");
				if (value == null)
				{
					throw new ArgumentNullException("GregorianDateTimeFormat");
				}
				this.ValidateDateTimeFormat(value);
				this.m_dateTimeFormat = (DateTimeFormatInfo)value.Clone();
				if (this.AvailableCalendars == null)
				{
					this.AvailableCalendars = new Calendar[] { value.Calendar };
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004608 File Offset: 0x00003608
		private Calendar[] GetCompleteCalendarList(Calendar[] availableCalendars)
		{
			CalendarId calendarId = (this.IsReplacementCulture ? CultureDefinition.CalendarIdOfCalendar(this.m_defaultCalendar) : ((CalendarId)0));
			bool flag = true;
			bool flag2 = this.IsReplacementCulture && calendarId != CalendarId.GREGORIAN;
			for (int i = 0; i < availableCalendars.Length; i++)
			{
				CalendarId calendarId2 = CultureDefinition.CalendarIdOfCalendar(availableCalendars[i]);
				if (calendarId2 == CalendarId.GREGORIAN)
				{
					flag = false;
				}
				if (flag2 && calendarId2 == calendarId)
				{
					flag2 = false;
				}
			}
			int num = 0;
			if (flag)
			{
				num++;
			}
			if (flag2)
			{
				num++;
			}
			if (num > 0)
			{
				Calendar[] array = new Calendar[availableCalendars.Length + num];
				for (int j = 0; j < availableCalendars.Length; j++)
				{
					array[j] = availableCalendars[j];
				}
				num = 0;
				if (flag)
				{
					array[availableCalendars.Length + num] = new GregorianCalendar();
					num++;
				}
				if (flag2)
				{
					array[availableCalendars.Length + num] = this.m_defaultCalendar;
				}
				return array;
			}
			return availableCalendars;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000046DC File Offset: 0x000036DC
		internal static void ValidateDuration(string[] durations)
		{
			if (durations == null)
			{
				throw new ArgumentNullException("durations");
			}
			if (durations.Length <= 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "DurationFormats", durations }));
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000472B File Offset: 0x0000372B
		internal string[] GetDurationFormats()
		{
			return this.m_durations;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00004733 File Offset: 0x00003733
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00004746 File Offset: 0x00003746
		internal string[] DurationFormats
		{
			get
			{
				this.CheckNeutral("DurationFormats");
				return this.m_durations;
			}
			set
			{
				this.CheckNeutral("DurationFormats");
				CultureAndRegionInfoBuilder.ValidateDuration(value);
				this.m_durations = value;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00004760 File Offset: 0x00003760
		internal static int GetLocaleInfoInt(CultureInfo culture, int lcType)
		{
			string win32LocaleInfo = CultureAndRegionInfoBuilder.GetWin32LocaleInfo(culture, lcType);
			int num = 0;
			if (win32LocaleInfo != null)
			{
				try
				{
					num = int.Parse(win32LocaleInfo, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture);
				}
				catch (FormatException)
				{
				}
			}
			return num;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000047A0 File Offset: 0x000037A0
		internal static void ValidateCountryCode(int countryCode)
		{
			if (countryCode < 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "countryCode", countryCode }));
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000047E4 File Offset: 0x000037E4
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000047F7 File Offset: 0x000037F7
		internal int CountryCode
		{
			get
			{
				this.CheckNeutral("CountryCode");
				return this.m_countryCode;
			}
			set
			{
				this.CheckNeutral("CountryCode");
				CultureAndRegionInfoBuilder.ValidateCountryCode(value);
				this.m_countryCode = value;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004814 File Offset: 0x00003814
		internal static void ValidateLeadingZero(int leadingZero)
		{
			if (leadingZero != 0 && leadingZero != 1)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "leadingZero", leadingZero }));
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000485B File Offset: 0x0000385B
		// (set) Token: 0x06000031 RID: 49 RVA: 0x0000486E File Offset: 0x0000386E
		internal bool LeadingZero
		{
			get
			{
				this.CheckNeutral("LeadingZero");
				return this.m_leadingZero;
			}
			set
			{
				this.CheckNeutral("LeadingZero");
				this.m_leadingZero = value;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004884 File Offset: 0x00003884
		internal static void ValidatePaperSize(PaperSize paperSize)
		{
			if (paperSize != PaperSize.Letter)
			{
				switch (paperSize)
				{
				case PaperSize.Legal:
				case PaperSize.A3:
				case PaperSize.A4:
					break;
				default:
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "PaperSize", paperSize }));
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000048E6 File Offset: 0x000038E6
		internal PaperSize GetPaperSize()
		{
			return this.m_paperSize;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000048EE File Offset: 0x000038EE
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00004901 File Offset: 0x00003901
		internal PaperSize PaperSize
		{
			get
			{
				this.CheckNeutral("PaperSize");
				return this.m_paperSize;
			}
			set
			{
				this.CheckNeutral("PaperSize");
				CultureAndRegionInfoBuilder.ValidatePaperSize(value);
				this.m_paperSize = value;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000491C File Offset: 0x0000391C
		internal static PaperSize GetWin32PaperSize(CultureInfo culture)
		{
			PaperSize paperSize = PaperSize.A4;
			int localeInfoInt = CultureAndRegionInfoBuilder.GetLocaleInfoInt(culture, 4106);
			int num = localeInfoInt;
			if (num != 1)
			{
				switch (num)
				{
				case 5:
					paperSize = PaperSize.Legal;
					break;
				case 8:
					paperSize = PaperSize.A3;
					break;
				case 9:
					paperSize = PaperSize.A4;
					break;
				}
			}
			else
			{
				paperSize = PaperSize.Letter;
			}
			return paperSize;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000496C File Offset: 0x0000396C
		internal static void ValidateFontSignature(string fontSignature)
		{
			if (fontSignature == null)
			{
				throw new ArgumentNullException("fontSignature");
			}
			if (fontSignature.Length != 16)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "FontSignature", fontSignature }));
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000049BF File Offset: 0x000039BF
		internal string GetFontSignature()
		{
			return this.m_fontSignature;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000049C7 File Offset: 0x000039C7
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000049DA File Offset: 0x000039DA
		internal string FontSignature
		{
			get
			{
				this.CheckNeutral("FontSignature");
				return this.m_fontSignature;
			}
			set
			{
				this.CheckNeutral("FontSignature");
				CultureAndRegionInfoBuilder.ValidateFontSignature(value);
				this.m_fontSignature = value;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000049F4 File Offset: 0x000039F4
		internal static string GetWin32FontSignature(CultureInfo culture)
		{
			ushort[] array = new ushort[32];
			int num = 0;
			try
			{
				num = CultureAndRegionInfoBuilder.GetLocaleInfoEx(culture.Name, 88, array, array.Length);
			}
			catch (EntryPointNotFoundException)
			{
				if (culture.LCID != 4096 && culture.LCID != 3072)
				{
					num = CultureAndRegionInfoBuilder.GetLocaleInfo(culture.LCID, 88, array, array.Length);
					if (num == 32)
					{
						num = 16;
					}
				}
			}
			if (num == 16)
			{
				StringBuilder stringBuilder = new StringBuilder(16);
				for (int i = 0; i < num; i++)
				{
					stringBuilder.Append((char)array[i]);
				}
				stringBuilder[7] = stringBuilder[7] & '뿿';
				stringBuilder[7] = stringBuilder[7] | '耀';
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004ABC File Offset: 0x00003ABC
		internal static void ValidateScripts(string scripts)
		{
			if (scripts == null)
			{
				throw new ArgumentNullException("scripts");
			}
			bool flag = false;
			for (int i = 0; i < scripts.Length; i++)
			{
				if (i % 5 == 4)
				{
					if (scripts[i] != ';')
					{
						flag = true;
					}
				}
				else if ((scripts[i] < 'a' || scripts[i] > 'z') && (scripts[i] < 'A' || scripts[i] > 'Z'))
				{
					flag = true;
				}
			}
			if (flag || scripts.Length % 5 != 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "Scripts", scripts }));
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00004B68 File Offset: 0x00003B68
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00004B70 File Offset: 0x00003B70
		internal string Scripts
		{
			get
			{
				return this.m_scripts;
			}
			set
			{
				CultureAndRegionInfoBuilder.ValidateScripts(value);
				this.m_scripts = value;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00004B80 File Offset: 0x00003B80
		internal static void ValidateEnglishLanguage(string englishLanguage)
		{
			if (englishLanguage == null)
			{
				throw new ArgumentNullException("englishLanguage");
			}
			if (englishLanguage.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "EnglishLanguage", englishLanguage }));
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00004BD1 File Offset: 0x00003BD1
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00004BD9 File Offset: 0x00003BD9
		internal string EnglishLanguage
		{
			get
			{
				return this.m_englishLanguage;
			}
			set
			{
				CultureAndRegionInfoBuilder.ValidateEnglishLanguage(value);
				this.m_englishLanguage = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00004BE8 File Offset: 0x00003BE8
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00004BF0 File Offset: 0x00003BF0
		internal string NativeLanguage
		{
			get
			{
				return this.m_nativeLanguage;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.m_nativeLanguage = value;
					return;
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "NativeLanguage", value }));
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004C3C File Offset: 0x00003C3C
		internal static void ValidateKeyboardToInstall(string keyboardsToInstall)
		{
			if (keyboardsToInstall == null)
			{
				throw new ArgumentNullException("keyboardsToInstall");
			}
			if (keyboardsToInstall.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValue"), new object[] { "keyboardsToInstall", keyboardsToInstall }));
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00004C8D File Offset: 0x00003C8D
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00004C95 File Offset: 0x00003C95
		internal string KeyboardsToInstall
		{
			get
			{
				return this.m_keyboardsToInstall;
			}
			set
			{
				CultureAndRegionInfoBuilder.ValidateKeyboardToInstall(value);
				this.m_keyboardsToInstall = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00004CA4 File Offset: 0x00003CA4
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00004CAC File Offset: 0x00003CAC
		internal string SpecificCultureName
		{
			get
			{
				return this.m_specificCultureName;
			}
			set
			{
				this.m_specificCultureName = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00004CB5 File Offset: 0x00003CB5
		internal Calendar[] Calendars
		{
			get
			{
				return this.m_calendars;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004CC0 File Offset: 0x00003CC0
		private void ValidateCalendars(Calendar[] calendars, bool validateAddedCalendars)
		{
			for (int i = 0; i < calendars.Length; i++)
			{
				if (calendars[i] == null)
				{
					throw new ArgumentNullException("AvailableCalendars");
				}
			}
			if (validateAddedCalendars && !this.IsNeutralCulture && calendars != this.GetCompleteCalendarList(calendars))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidCalendars"), new object[] { "Calendar []", calendars }));
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004D32 File Offset: 0x00003D32
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00004D45 File Offset: 0x00003D45
		public Calendar[] AvailableCalendars
		{
			get
			{
				this.CheckNeutral("AvailableCalendars");
				return this.m_calendars;
			}
			set
			{
				this.CheckNeutral("AvailableCalendars");
				if (value == null)
				{
					throw new ArgumentNullException("AvailableCalendars");
				}
				this.ValidateCalendars(value, false);
				this.m_calendars = this.GetCompleteCalendarList(value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00004D75 File Offset: 0x00003D75
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00004D80 File Offset: 0x00003D80
		internal string[] NativeCalendarNames
		{
			get
			{
				return this.m_calendarNames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("NativeCalendarNames");
				}
				if (value.Length != 23)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValueNoValue"), new object[] { "NativeCalendarNames" }));
				}
				this.m_calendarNames = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00004DD3 File Offset: 0x00003DD3
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00004DDB File Offset: 0x00003DDB
		internal bool ForceWindowsPositivePlus
		{
			get
			{
				return this.m_forceWindowsPositivePlus;
			}
			set
			{
				this.m_forceWindowsPositivePlus = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00004DE4 File Offset: 0x00003DE4
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00004DEC File Offset: 0x00003DEC
		public CompareInfo CompareInfo
		{
			get
			{
				return this.m_compareInfo;
			}
			set
			{
				this.CheckOverride("CompareInfo");
				if (value == null)
				{
					throw new ArgumentNullException("Compare");
				}
				this.m_compareInfo = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004E0E File Offset: 0x00003E0E
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00004E16 File Offset: 0x00003E16
		public string CultureEnglishName
		{
			get
			{
				return this.m_EnglishName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("CultureEnglishName");
				}
				this.VerifySize("CultureEnglishName", value.Length, 0, 79);
				this.m_EnglishName = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00004E41 File Offset: 0x00003E41
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00004E49 File Offset: 0x00003E49
		public int KeyboardLayoutId
		{
			get
			{
				return this.m_keyboardLayoutId;
			}
			set
			{
				if (value == 0)
				{
					throw new ArgumentOutOfRangeException("KeyboardLayoutId", CultureAndRegionInfoBuilder.GetResourceString("Argument_ArgumentZero"));
				}
				this.m_keyboardLayoutId = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00004E6A File Offset: 0x00003E6A
		public CultureTypes CultureTypes
		{
			get
			{
				return this.m_cultureTypes;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00004E72 File Offset: 0x00003E72
		internal bool IsNeutralCulture
		{
			get
			{
				return (this.CultureTypes & CultureTypes.NeutralCultures) != (CultureTypes)0;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00004E82 File Offset: 0x00003E82
		internal bool IsReplacementCulture
		{
			get
			{
				return (this.CultureTypes & CultureTypes.ReplacementCultures) != (CultureTypes)0;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00004E93 File Offset: 0x00003E93
		public string CultureName
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00004E9B File Offset: 0x00003E9B
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00004EA3 File Offset: 0x00003EA3
		public string CultureNativeName
		{
			get
			{
				return this.m_NativeName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("CultureNativeName");
				}
				this.VerifySize("CultureNativeName", value.Length, 1, 79);
				this.m_NativeName = value;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004ED0 File Offset: 0x00003ED0
		internal void ValidateNumberFormat(NumberFormatInfo format)
		{
			this.VerifySize("NumberFormatInfo.CurrencyDecimalDigits", format.CurrencyDecimalDigits, 99);
			this.VerifySize("NumberFormatInfo.CurrencyDecimalSeparator", format.CurrencyDecimalSeparator.Length, CultureAndRegionInfoBuilder.MAX.SLIST);
			this.VerifySize("NumberFormatInfo.CurrencyGroupSeparator", format.CurrencyGroupSeparator.Length, CultureAndRegionInfoBuilder.MAX.SGROUPING);
			this.VerifySize("NumberFormatInfo.CurrencyNegativePattern", format.CurrencyNegativePattern, 0, 15);
			this.VerifySize("NumberFormatInfo.CurrencyPositivePattern", format.CurrencyPositivePattern, 0, 3);
			this.VerifySize("NumberFormatInfo.CurrencySymbol", format.CurrencySymbol.Length, CultureAndRegionInfoBuilder.MAX.SCURRENCY);
			this.VerifySize("NumberFormatInfo.NegativeSign", format.NegativeSign.Length, CultureAndRegionInfoBuilder.MAX.SPOSSIGN);
			this.VerifySize("NumberFormatInfo.NumberDecimalDigits", format.NumberDecimalDigits, 9);
			this.VerifySize("NumberFormatInfo.NumberDecimalSeparator", format.NumberDecimalSeparator.Length, CultureAndRegionInfoBuilder.MAX.SLIST);
			this.VerifySize("NumberFormatInfo.NumberGroupSeparator", format.NumberGroupSeparator.Length, CultureAndRegionInfoBuilder.MAX.SGROUPING);
			this.VerifySize("NumberFormatInfo.NumberNegativePattern", format.NumberNegativePattern, 9);
			this.VerifySize("NumberFormatInfo.PositiveSign", format.PositiveSign.Length, CultureAndRegionInfoBuilder.MAX.SPOSSIGN);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00004FE0 File Offset: 0x00003FE0
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00004FF3 File Offset: 0x00003FF3
		public NumberFormatInfo NumberFormat
		{
			get
			{
				this.CheckNeutral("NumberFormat");
				return this.m_numberFormat;
			}
			set
			{
				this.CheckNeutral("NumberFormat");
				if (value == null)
				{
					throw new ArgumentNullException("NumberFormat");
				}
				this.ValidateNumberFormat(value);
				this.m_numberFormat = (NumberFormatInfo)value.Clone();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005026 File Offset: 0x00004026
		internal void ValidateTextInfo(TextInfo textInfo)
		{
			this.VerifySize("TextInfo.ListSeparator", textInfo.ListSeparator.Length, 0, (CultureAndRegionInfoBuilder.MAX)3);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00005040 File Offset: 0x00004040
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00005048 File Offset: 0x00004048
		public TextInfo TextInfo
		{
			get
			{
				return this.m_textInfo;
			}
			set
			{
				this.CheckOverride("TextInfo");
				if (value == null)
				{
					throw new ArgumentNullException("TextInfo");
				}
				this.ValidateTextInfo(value);
				this.m_textInfo = (TextInfo)value.Clone();
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000063 RID: 99 RVA: 0x0000507B File Offset: 0x0000407B
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00005083 File Offset: 0x00004083
		public string ThreeLetterISOLanguageName
		{
			get
			{
				return this.m_threeLetterIsoLanguageName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("ThreeLetterISOLanguageName");
				}
				this.VerifySize("ThreeLetterISOLanguageName", value.Length, 1, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "ThreeLetterISOLanguageName", 8, false, false);
				this.m_threeLetterIsoLanguageName = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000050BB File Offset: 0x000040BB
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000050C4 File Offset: 0x000040C4
		public string ThreeLetterWindowsLanguageName
		{
			get
			{
				return this.m_threeLetterWindowsLanguageName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("ThreeLetterWindowsLanguageName");
				}
				this.CheckOverride("ThreeLetterWindowsLanguageName");
				this.VerifySize("ThreeLetterWindowsLanguageName", value.Length, 1, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "ThreeLetterWindowsLanguageName", 8, false, false);
				this.m_threeLetterWindowsLanguageName = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00005112 File Offset: 0x00004112
		// (set) Token: 0x06000068 RID: 104 RVA: 0x0000511A File Offset: 0x0000411A
		public string TwoLetterISOLanguageName
		{
			get
			{
				return this.m_twoLetterIsoLanguageName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("TwoLetterISOLanguageName");
				}
				this.VerifySize("TwoLetterISOLanguageName", value.Length, 1, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "TwoLetterISOLanguageName", 8, false, false);
				this.m_twoLetterIsoLanguageName = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00005154 File Offset: 0x00004154
		internal ushort LineOrientationsUShortValue
		{
			get
			{
				ushort num = 0;
				if (this.IsRightToLeft)
				{
					num |= 32768;
				}
				return num;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00005175 File Offset: 0x00004175
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000517D File Offset: 0x0000417D
		public bool IsRightToLeft
		{
			get
			{
				return this.m_rightToLeft;
			}
			set
			{
				this.m_rightToLeft = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00005186 File Offset: 0x00004186
		public string RegionName
		{
			get
			{
				this.CheckNeutral("RegionName");
				return this.m_name;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00005199 File Offset: 0x00004199
		internal string ActualRegionName
		{
			get
			{
				this.CheckNeutral("ActualRegionName");
				if (this.m_actualRegionName == null)
				{
					return this.RegionName;
				}
				return this.m_actualRegionName;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000051BB File Offset: 0x000041BB
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000051CE File Offset: 0x000041CE
		public string RegionEnglishName
		{
			get
			{
				this.CheckNeutral("RegionEnglishName");
				return this.m_RegionEnglishName;
			}
			set
			{
				this.CheckNeutral("RegionEnglishName");
				if (value == null)
				{
					throw new ArgumentNullException("RegionEnglishName");
				}
				this.VerifySize("RegionEnglishName", value.Length, 0, 79);
				this.m_RegionEnglishName = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00005204 File Offset: 0x00004204
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00005217 File Offset: 0x00004217
		public string RegionNativeName
		{
			get
			{
				this.CheckNeutral("RegionNativeName");
				return this.m_RegionNativeName;
			}
			set
			{
				this.CheckNeutral("RegionNativeName");
				if (value == null)
				{
					throw new ArgumentNullException("RegionNativeName");
				}
				this.VerifySize("RegionNativeName", value.Length, 1, 79);
				this.m_RegionNativeName = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000524D File Offset: 0x0000424D
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00005260 File Offset: 0x00004260
		public string ThreeLetterISORegionName
		{
			get
			{
				this.CheckNeutral("ThreeLetterISORegionName");
				return this.m_threeLetterIsoRegionName;
			}
			set
			{
				this.CheckNeutral("ThreeLetterISORegionName");
				if (value == null)
				{
					throw new ArgumentNullException("ThreeLetterISORegionName");
				}
				this.VerifySize("ThreeLetterISORegionName", value.Length, 1, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "ThreeLetterISORegionName", 8, false, false);
				this.m_threeLetterIsoRegionName = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000052AE File Offset: 0x000042AE
		// (set) Token: 0x06000075 RID: 117 RVA: 0x000052C4 File Offset: 0x000042C4
		public string TwoLetterISORegionName
		{
			get
			{
				this.CheckNeutral("TwoLetterISORegionName");
				return this.m_twoLetterIsoRegionName;
			}
			set
			{
				this.CheckNeutral("TwoLetterISORegionName");
				if (value == null)
				{
					throw new ArgumentNullException("TwoLetterISORegionName");
				}
				this.VerifySize("TwoLetterISORegionName", value.Length, 1, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "TwoLetterISORegionName", 8, false, false);
				this.m_twoLetterIsoRegionName = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00005312 File Offset: 0x00004312
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00005328 File Offset: 0x00004328
		public string ThreeLetterWindowsRegionName
		{
			get
			{
				this.CheckNeutral("ThreeLetterWindowsRegionName");
				return this.m_threeLetterWindowsRegionName;
			}
			set
			{
				this.CheckNeutral("ThreeLetterWindowsRegionName");
				if (value == null)
				{
					throw new ArgumentNullException("ThreeLetterWindowsRegionName");
				}
				this.CheckOverride("ThreeLetterWindowsRegionName");
				this.VerifySize("ThreeLetterWindowsRegionName", value.Length, 1, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "ThreeLetterWindowsRegionName", 8, false, false);
				this.m_threeLetterWindowsRegionName = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00005381 File Offset: 0x00004381
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00005394 File Offset: 0x00004394
		public string ISOCurrencySymbol
		{
			get
			{
				this.CheckNeutral("ISOCurrencySymbol");
				return this.m_ISOCurrencySymbol;
			}
			set
			{
				this.CheckNeutral("ISOCurrencySymbol");
				if (value == null)
				{
					throw new ArgumentNullException("ISOCurrencySymbol");
				}
				this.VerifySize("ISOCurrencySymbol", value.Length, 0, 8);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "ISOCurrencySymbol", 8, false, false);
				this.m_ISOCurrencySymbol = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000053E2 File Offset: 0x000043E2
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000053EA File Offset: 0x000043EA
		public string IetfLanguageTag
		{
			get
			{
				return this.m_name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("IetfLanguageTag");
				}
				this.VerifySize("IetfLanguageTag", value.Length, 1, 84);
				CultureAndRegionInfoBuilder.ValidateCulturePiece(value, "IetfLanguageTag", 84, true, false);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000541D File Offset: 0x0000441D
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00005430 File Offset: 0x00004430
		public bool IsMetric
		{
			get
			{
				this.CheckNeutral("IsMetric");
				return this.m_IsMetric;
			}
			set
			{
				this.CheckNeutral("IsMetric");
				this.m_IsMetric = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00005444 File Offset: 0x00004444
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00005457 File Offset: 0x00004457
		public int GeoId
		{
			get
			{
				this.CheckNeutral("GeoId");
				return this.m_GeoId;
			}
			set
			{
				this.CheckNeutral("GeoId");
				this.m_GeoId = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000546B File Offset: 0x0000446B
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00005480 File Offset: 0x00004480
		public string CurrencyNativeName
		{
			get
			{
				this.CheckNeutral("CurrencyNativeName");
				return this.m_currencyNativeName;
			}
			set
			{
				this.CheckNeutral("CurrencyNativeName");
				if (value == null)
				{
					throw new ArgumentNullException("CurrencyNativeName");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValueNoValue"), new object[] { "CurrencyNativeName" }));
				}
				this.m_currencyNativeName = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000054DF File Offset: 0x000044DF
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000054F4 File Offset: 0x000044F4
		public string CurrencyEnglishName
		{
			get
			{
				this.CheckNeutral("CurrencyEnglishName");
				return this.m_currencyEnglishName;
			}
			set
			{
				this.CheckNeutral("CurrencyEnglishName");
				if (value == null)
				{
					throw new ArgumentNullException("CurrencyEnglishName");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidPropertyValueNoValue"), new object[] { "CurrencyEnglishName" }));
				}
				this.m_currencyEnglishName = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00005553 File Offset: 0x00004553
		public int LCID
		{
			get
			{
				return this.m_LCID;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000555B File Offset: 0x0000455B
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00005563 File Offset: 0x00004563
		public CultureInfo ConsoleFallbackUICulture
		{
			get
			{
				return this.m_consoleFallbackCulture;
			}
			set
			{
				if (value != null && !value.GetConsoleFallbackUICulture().Equals(value))
				{
					throw new ArgumentException(CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidConsoleFallbackUICulture"));
				}
				this.m_consoleFallbackCulture = value;
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005590 File Offset: 0x00004590
		private void ValidateBeforeSaving()
		{
			if (this.TextInfo != null)
			{
				this.ValidateTextInfo(this.TextInfo);
			}
			if (!this.IsNeutralCulture && this.NumberFormat != null)
			{
				this.ValidateNumberFormat(this.NumberFormat);
			}
			if (!this.IsNeutralCulture && this.GregorianDateTimeFormat != null)
			{
				this.ValidateDateTimeFormat(this.GregorianDateTimeFormat);
			}
			if (!this.IsNeutralCulture && this.AvailableCalendars != null)
			{
				this.ValidateCalendars(this.AvailableCalendars, true);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00005608 File Offset: 0x00004608
		public void Save(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (filename.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidFileName"), new object[] { filename }));
			}
			this.ValidateBeforeSaving();
			CultureXmlWriter cultureXmlWriter = new CultureXmlWriter(this);
			cultureXmlWriter.Save(filename);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00005668 File Offset: 0x00004668
		public static CultureAndRegionInfoBuilder CreateFromLdml(string xmlFileName)
		{
			if (xmlFileName == null)
			{
				throw new ArgumentNullException("xmlFileName");
			}
			if (xmlFileName.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_InvalidFileName"), new object[] { xmlFileName }));
			}
			CultureXmlReader cultureXmlReader = new CultureXmlReader(xmlFileName);
			return cultureXmlReader.CultureAndRegionInfoBuilder;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000056C0 File Offset: 0x000046C0
		private bool IsValidRegionInfo(out string propertyName)
		{
			propertyName = null;
			if (this.RegionEnglishName == null)
			{
				propertyName = "RegionEnglishName";
				return false;
			}
			if (this.RegionNativeName == null)
			{
				propertyName = "RegionNativeName";
				return false;
			}
			if (this.ThreeLetterISORegionName == null)
			{
				propertyName = "ThreeLetterISORegionName";
				return false;
			}
			if (this.TwoLetterISORegionName == null)
			{
				propertyName = "TwoLetterISORegionName";
				return false;
			}
			if (this.ThreeLetterWindowsRegionName == null)
			{
				propertyName = "ThreeLetterWindowsRegionName";
				return false;
			}
			if (this.ISOCurrencySymbol == null)
			{
				propertyName = "ISOCurrencySymbol";
				return false;
			}
			if (this.CurrencyNativeName == null)
			{
				propertyName = "CurrencyNativeName";
				return false;
			}
			if (this.CurrencyEnglishName == null)
			{
				propertyName = "CurrencyEnglishName";
				return false;
			}
			return true;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000575C File Offset: 0x0000475C
		public void Register()
		{
			string text = CultureAndRegionInfoBuilder.FilePathFromCustomCultureName(this.CultureName);
			if (File.Exists(text))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CustomCultureAlreadyExists"), new object[] { "Register", this.CultureName }));
			}
			if (!this.IsNeutralCulture)
			{
				if (this.NumberFormat == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "NumberFormat", "NumberFormatInfo" }));
				}
				this.ValidateNumberFormat(this.NumberFormat);
				if (this.GregorianDateTimeFormat == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "GregorianDateTimeFormat", "DateTimeFormatInfo" }));
				}
				this.ValidateDateTimeFormat(this.GregorianDateTimeFormat);
				string text2;
				if (!this.IsValidRegionInfo(out text2))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", text2, "string" }));
				}
			}
			if (this.Calendars == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "AvailableCalendars", "Calendar[]" }));
			}
			this.ValidateCalendars(this.Calendars, true);
			if (this.TextInfo == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "TextInfo", "TextInfo" }));
			}
			this.ValidateTextInfo(this.TextInfo);
			if (this.CompareInfo == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "CompareInfo", "CompareInfo" }));
			}
			if (this.Parent == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "Parent", "CultureInfo" }));
			}
			this.ValidateParent(this.Parent);
			if (this.CultureNativeName == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "CultureNativeName", "string" }));
			}
			if (this.ThreeLetterISOLanguageName == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "ThreeLetterISOLanguageName", "string" }));
			}
			if (this.ThreeLetterWindowsLanguageName == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "ThreeLetterWindowsLanguageName", "string" }));
			}
			if (this.TwoLetterISOLanguageName == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "TwoLetterISOLanguageName", "string" }));
			}
			if (this.CultureEnglishName == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotRegisterCultureDueToMissingObject"), new object[] { "Register", "CultureEnglishName", "string" }));
			}
			CultureDefinition.Compile(this, text);
			CultureInfo.CurrentCulture.ClearCachedData();
			this.RegisterWithRegistry();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00005B5C File Offset: 0x00004B5C
		private static void ChildrenExistenceCheck(string cultureName)
		{
			if ((CultureInfo.GetCultureInfo(cultureName).CultureTypes & CultureTypes.ReplacementCultures) == (CultureTypes)0)
			{
				CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.UserCustomCulture);
				for (int i = 0; i < cultures.Length; i++)
				{
					try
					{
						if (cultures[i].Parent.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase))
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidOperation_CannotUnregisterBecauseOfParent"), new object[] { cultures[i].Name }));
						}
					}
					catch (ArgumentException)
					{
					}
					try
					{
						if (!cultures[i].Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase) && cultures[i].GetConsoleFallbackUICulture().Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase))
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidOperation_CannotUnregisterBecauseOfConsoleUI"), new object[]
							{
								cultureName,
								cultures[i].Name
							}));
						}
					}
					catch (ArgumentException)
					{
					}
				}
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00005C58 File Offset: 0x00004C58
		private static void RenameFile(string sourceFileName, string cultureName)
		{
			int num = sourceFileName.Length - 1;
			while (num > 0 && sourceFileName[num] != '.')
			{
				num--;
			}
			StringBuilder stringBuilder;
			if (num > 0)
			{
				stringBuilder = new StringBuilder(sourceFileName.Substring(0, num + 1));
			}
			else
			{
				stringBuilder = new StringBuilder(sourceFileName);
			}
			stringBuilder.Append("tmp");
			num = 0;
			int length = stringBuilder.Length;
			string text2;
			for (;;)
			{
				string text = num.ToString(CultureInfo.InvariantCulture);
				stringBuilder.Append(text);
				text2 = stringBuilder.ToString();
				bool flag = true;
				if (File.Exists(text2))
				{
					try
					{
						File.Delete(text2);
					}
					catch (IOException)
					{
						flag = false;
					}
					catch (UnauthorizedAccessException)
					{
						flag = false;
					}
				}
				if (flag)
				{
					break;
				}
				num++;
				stringBuilder.Remove(length, text.Length);
				if (num >= 200)
				{
					goto Block_5;
				}
			}
			File.Move(sourceFileName, text2);
			return;
			Block_5:
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CannotUnregister"), new object[] { cultureName }));
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005D60 File Offset: 0x00004D60
		private static bool DeleteFile(string fileName, string cultureName)
		{
			try
			{
				File.Delete(fileName);
			}
			catch (SecurityException ex)
			{
				throw new SecurityException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CustomCultureUnregisterPermissionsProblem"), new object[] { cultureName }), ex);
			}
			catch (FileNotFoundException)
			{
			}
			catch (IOException)
			{
				return false;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00005DE4 File Offset: 0x00004DE4
		public static void Unregister(string cultureName)
		{
			CultureAndRegionInfoBuilder.ValidateCulturePiece(cultureName, "cultureName", 84, true, true);
			if (cultureName.Length <= 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidCultureName"), new object[] { cultureName }), "cultureName");
			}
			string text = CultureAndRegionInfoBuilder.FilePathFromCustomCultureName(cultureName);
			if (!File.Exists(text))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CustomCultureNotFound"), new object[] { cultureName }), "cultureName");
			}
			CultureAndRegionInfoBuilder.ChildrenExistenceCheck(cultureName);
			if (!CultureAndRegionInfoBuilder.DeleteFile(text, cultureName))
			{
				try
				{
					CultureAndRegionInfoBuilder.RenameFile(text, cultureName);
				}
				catch (UnauthorizedAccessException)
				{
					CultureInfo.CurrentCulture.ClearCachedData();
					GC.Collect();
					GC.WaitForPendingFinalizers();
					if (!CultureAndRegionInfoBuilder.DeleteFile(text, cultureName))
					{
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("CustomCultureInUse"), new object[] { cultureName }));
					}
				}
			}
			CultureAndRegionInfoBuilder.UnregisterWithRegistry(cultureName);
			CultureInfo.CurrentCulture.ClearCachedData();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00005EEC File Offset: 0x00004EEC
		private void RegisterWithRegistry()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Nls\\CustomLocale", true);
			if (registryKey == null)
			{
				registryKey = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Control\\Nls\\CustomLocale");
				if (registryKey == null)
				{
					throw new Exception(CultureAndRegionInfoBuilder.GetResourceString("CannotCreateRegistryKey"));
				}
			}
			registryKey.SetValue(this.CultureName, this.CultureName);
			registryKey.Close();
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00005F48 File Offset: 0x00004F48
		private static void UnregisterWithRegistry(string cultureName)
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Nls\\CustomLocale", true);
			if (registryKey != null)
			{
				registryKey.DeleteValue(cultureName, false);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005F74 File Offset: 0x00004F74
		private static string FilePathFromCustomCultureName(string culture)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			stringBuilder.Append(CultureAndRegionInfoBuilder.InternalWindowsDirectory);
			stringBuilder.Append("\\Globalization\\");
			if (!Directory.Exists(stringBuilder.ToString()))
			{
				Directory.CreateDirectory(stringBuilder.ToString());
			}
			stringBuilder.Append(culture);
			stringBuilder.Append(".nlp");
			return stringBuilder.ToString();
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005FD7 File Offset: 0x00004FD7
		internal int[] CodePages
		{
			get
			{
				return CultureAndRegionInfoBuilder.m_codePages;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005FE0 File Offset: 0x00004FE0
		private static void ValidateCulturePiece(string testString, string paramName, int maxLength, bool fAllowDash, bool fAllowUnderscore)
		{
			if (testString == null)
			{
				throw new ArgumentNullException(testString);
			}
			if (testString.Length > maxLength)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidCultureName"), new object[] { testString }));
			}
			if (testString.Length == 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < testString.Length; i++)
			{
				char c = testString[i];
				if ((c > 'Z' || c < 'A') && (c > 'z' || c < 'a') && (c > '9' || c < '0') && (!fAllowUnderscore || c != '_') && (!fAllowDash || c != '-'))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidCultureName"), new object[] { testString }));
				}
				if (c == '-' || (c == '_' && fAllowUnderscore))
				{
					if (i == num || i - num > 8)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidCultureName"), new object[] { testString }));
					}
					num = i + 1;
				}
			}
			if (testString.Length == num || testString.Length - num > 8)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("InvalidCultureName"), new object[] { testString }));
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000612C File Offset: 0x0000512C
		internal void CheckNeutral(string propName)
		{
			if (this.IsNeutralCulture)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("AttemptToSetNonNeutralPropertyOnNeutralCulture"), new object[] { propName, this.CultureName }));
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006170 File Offset: 0x00005170
		internal void CheckOverride(string propName)
		{
			if (this.IsReplacementCulture)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("AttemptToSetNonOverridePropertyOnOverrideCulture"), new object[] { propName, this.CultureName }));
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000061B4 File Offset: 0x000051B4
		private void VerifySize(string propname, int value, CultureAndRegionInfoBuilder.MAX correctvalue)
		{
			this.VerifySize(propname, value, (int)correctvalue);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000061C0 File Offset: 0x000051C0
		private void VerifySize(string propname, int value, int correctvalue)
		{
			if (value > correctvalue)
			{
				throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_PropertyValueIncorrect"), new object[] { propname, correctvalue }));
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006200 File Offset: 0x00005200
		private void VerifySize(string propname, int value, int minvalue, CultureAndRegionInfoBuilder.MAX maxvalue)
		{
			this.VerifySize(propname, value, minvalue, (int)maxvalue);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006210 File Offset: 0x00005210
		private void VerifySize(string propname, int value, int minvalue, int maxvalue)
		{
			if (value < minvalue || value > maxvalue)
			{
				throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_PropertyValueOutOfRange"), new object[] { propname, minvalue, maxvalue }));
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006260 File Offset: 0x00005260
		private void VerifyArrayStringSizes(string propname, string[] rgst, CultureAndRegionInfoBuilder.MAX maxvalue)
		{
			for (int i = 0; i < rgst.Length; i++)
			{
				string text = rgst[i];
				if (text.Length > (int)maxvalue)
				{
					throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, CultureAndRegionInfoBuilder.GetResourceString("Argument_PropertyValueInArrayOutOfRange"), new object[]
					{
						i,
						propname,
						(int)maxvalue
					}));
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000062C0 File Offset: 0x000052C0
		private static object InternalSyncObject
		{
			get
			{
				if (CultureAndRegionInfoBuilder.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref CultureAndRegionInfoBuilder.s_InternalSyncObject, obj, null);
				}
				return CultureAndRegionInfoBuilder.s_InternalSyncObject;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000062EC File Offset: 0x000052EC
		private static ResourceManager InitResourceManager()
		{
			if (CultureAndRegionInfoBuilder.s_SystemResMgr == null)
			{
				lock (CultureAndRegionInfoBuilder.InternalSyncObject)
				{
					if (CultureAndRegionInfoBuilder.s_SystemResMgr == null)
					{
						CultureAndRegionInfoBuilder.s_SystemResMgr = new ResourceManager("SysGlobl", Assembly.GetExecutingAssembly());
					}
				}
			}
			return CultureAndRegionInfoBuilder.s_SystemResMgr;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006348 File Offset: 0x00005348
		internal static string GetResourceString(string key)
		{
			if (CultureAndRegionInfoBuilder.s_SystemResMgr == null)
			{
				CultureAndRegionInfoBuilder.InitResourceManager();
			}
			return CultureAndRegionInfoBuilder.s_SystemResMgr.GetString(key, null);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00006364 File Offset: 0x00005364
		internal static string InternalWindowsDirectory
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				CultureAndRegionInfoBuilder.GetWindowsDirectory(stringBuilder, 260);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006390 File Offset: 0x00005390
		internal static string GetWin32LocaleInfo(CultureInfo locale, int lctype)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			int num = 0;
			try
			{
				num = CultureAndRegionInfoBuilder.GetLocaleInfoEx(locale.Name, lctype, stringBuilder, 1024);
			}
			catch (EntryPointNotFoundException)
			{
				if (locale.LCID != 4096 && locale.LCID != 3072)
				{
					num = CultureAndRegionInfoBuilder.GetLocaleInfo(locale.LCID, lctype, stringBuilder, 1024);
				}
			}
			if (num > 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000640C File Offset: 0x0000540C
		private static string ReescapeWin32String(string str)
		{
			if (str == null)
			{
				return null;
			}
			StringBuilder stringBuilder = null;
			bool flag = false;
			int i = 0;
			while (i < str.Length)
			{
				if (str[i] == '\'')
				{
					if (!flag)
					{
						flag = true;
						goto IL_0091;
					}
					if (i + 1 >= str.Length || str[i + 1] != '\'')
					{
						flag = false;
						goto IL_0091;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(str, 0, i, str.Length * 2);
					}
					stringBuilder.Append("\\'");
					i++;
				}
				else
				{
					if (str[i] != '\\')
					{
						goto IL_0091;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(str, 0, i, str.Length * 2);
					}
					stringBuilder.Append("\\\\");
				}
				IL_00A2:
				i++;
				continue;
				IL_0091:
				if (stringBuilder != null)
				{
					stringBuilder.Append(str[i]);
					goto IL_00A2;
				}
				goto IL_00A2;
			}
			if (stringBuilder == null)
			{
				return str;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000064D8 File Offset: 0x000054D8
		private static string[] ReescapeWin32Strings(string[] array)
		{
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = CultureAndRegionInfoBuilder.ReescapeWin32String(array[i]);
				}
			}
			return array;
		}

		// Token: 0x060000A3 RID: 163
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int GetLocaleInfo(int locale, int lctypes, StringBuilder buffer, int size);

		// Token: 0x060000A4 RID: 164
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		internal static extern int GetLocaleInfo(int locale, int lctypes, ushort[] buffer, int size);

		// Token: 0x060000A5 RID: 165
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetLocaleInfoEx(string locale, int lctypes, StringBuilder buffer, int size);

		// Token: 0x060000A6 RID: 166
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int GetLocaleInfoEx(string locale, int lctypes, ushort[] buffer, int size);

		// Token: 0x04000012 RID: 18
		private const int MAX_PATH = 260;

		// Token: 0x04000013 RID: 19
		private const int MAX_FALLBACK = 10;

		// Token: 0x04000014 RID: 20
		private const int LOCALE_INVARIANT = 127;

		// Token: 0x04000015 RID: 21
		private const int MOVEFILE_REPLACE_EXISTING = 1;

		// Token: 0x04000016 RID: 22
		private const int MOVEFILE_COPY_ALLOWED = 2;

		// Token: 0x04000017 RID: 23
		private const int MOVEFILE_DELAY_UNTIL_REBOOT = 4;

		// Token: 0x04000018 RID: 24
		private const int MOVEFILE_WRITE_THROUGH = 8;

		// Token: 0x04000019 RID: 25
		private const int MAX_REG_VALUE_SIZE = 79;

		// Token: 0x0400001A RID: 26
		private const string customRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Nls\\CustomLocale";

		// Token: 0x0400001B RID: 27
		internal const int LOCALE_CUSTOM_DEFAULT = 3072;

		// Token: 0x0400001C RID: 28
		internal const int LOCALE_CUSTOM_UNSPECIFIED = 4096;

		// Token: 0x0400001D RID: 29
		private const int MAXSIZE_LANGUAGE = 8;

		// Token: 0x0400001E RID: 30
		private const int MAXSIZE_REGION = 8;

		// Token: 0x0400001F RID: 31
		private const int MAXSIZE_SUFFIX = 64;

		// Token: 0x04000020 RID: 32
		private const int MAXSIZE_FULLTAGNAME = 84;

		// Token: 0x04000021 RID: 33
		private const int MAXSIZE_IETFTAGPART = 8;

		// Token: 0x04000022 RID: 34
		private const int ModifiersMask = -4;

		// Token: 0x04000023 RID: 35
		internal const int LOCALE_IDEFAULTCOUNTRY = 10;

		// Token: 0x04000024 RID: 36
		internal const int LOCALE_ILZERO = 18;

		// Token: 0x04000025 RID: 37
		internal const int LOCALE_SDURATION = 93;

		// Token: 0x04000026 RID: 38
		internal const int LOCALE_IPAPERSIZE = 4106;

		// Token: 0x04000027 RID: 39
		internal const int LOCALE_FONTSIGNATURE = 88;

		// Token: 0x04000028 RID: 40
		internal const int LOCALE_SSCRIPTS = 108;

		// Token: 0x04000029 RID: 41
		internal const int LOCALE_SENGLANGUAGE = 4097;

		// Token: 0x0400002A RID: 42
		internal const int LOCALE_SNATIVELANGNAME = 4;

		// Token: 0x0400002B RID: 43
		internal const int LOCALE_SKEYBOARDSTOINSTALL = 94;

		// Token: 0x0400002C RID: 44
		private static object s_InternalSyncObject;

		// Token: 0x0400002D RID: 45
		internal static ResourceManager s_SystemResMgr;

		// Token: 0x0400002E RID: 46
		private CultureTypes m_cultureTypes = CultureTypes.UserCustomCulture;

		// Token: 0x0400002F RID: 47
		internal int m_LCID;

		// Token: 0x04000030 RID: 48
		private string m_name;

		// Token: 0x04000031 RID: 49
		private string m_actualRegionName;

		// Token: 0x04000032 RID: 50
		private string m_EnglishName;

		// Token: 0x04000033 RID: 51
		private string m_NativeName;

		// Token: 0x04000034 RID: 52
		private string m_threeLetterWindowsLanguageName;

		// Token: 0x04000035 RID: 53
		private string m_threeLetterIsoLanguageName;

		// Token: 0x04000036 RID: 54
		private string m_twoLetterIsoLanguageName;

		// Token: 0x04000037 RID: 55
		private bool m_rightToLeft;

		// Token: 0x04000038 RID: 56
		private int m_keyboardLayoutId;

		// Token: 0x04000039 RID: 57
		private CultureInfo m_Parent;

		// Token: 0x0400003A RID: 58
		private DateTimeFormatInfo m_dateTimeFormat;

		// Token: 0x0400003B RID: 59
		private NumberFormatInfo m_numberFormat;

		// Token: 0x0400003C RID: 60
		private TextInfo m_textInfo;

		// Token: 0x0400003D RID: 61
		private CompareInfo m_compareInfo;

		// Token: 0x0400003E RID: 62
		private Calendar m_defaultCalendar;

		// Token: 0x0400003F RID: 63
		private string m_RegionEnglishName;

		// Token: 0x04000040 RID: 64
		private string m_RegionNativeName;

		// Token: 0x04000041 RID: 65
		private string m_ISOCurrencySymbol;

		// Token: 0x04000042 RID: 66
		private string m_threeLetterIsoRegionName;

		// Token: 0x04000043 RID: 67
		private string m_threeLetterWindowsRegionName;

		// Token: 0x04000044 RID: 68
		private string m_twoLetterIsoRegionName;

		// Token: 0x04000045 RID: 69
		private string m_currencyNativeName;

		// Token: 0x04000046 RID: 70
		private string m_currencyEnglishName;

		// Token: 0x04000047 RID: 71
		private int m_GeoId;

		// Token: 0x04000048 RID: 72
		private bool m_IsMetric;

		// Token: 0x04000049 RID: 73
		private CultureInfo m_consoleFallbackCulture;

		// Token: 0x0400004A RID: 74
		private int m_countryCode;

		// Token: 0x0400004B RID: 75
		private bool m_leadingZero;

		// Token: 0x0400004C RID: 76
		private string[] m_durations = new string[] { "HH:mm:ss" };

		// Token: 0x0400004D RID: 77
		private PaperSize m_paperSize = PaperSize.A4;

		// Token: 0x0400004E RID: 78
		private string m_fontSignature = "\u0002\0\0\0\0\0\0耀\u0001\0\0耀\u0001\0\0耀";

		// Token: 0x0400004F RID: 79
		private string m_scripts = "";

		// Token: 0x04000050 RID: 80
		private string m_englishLanguage;

		// Token: 0x04000051 RID: 81
		private string m_nativeLanguage;

		// Token: 0x04000052 RID: 82
		private bool m_forceWindowsPositivePlus;

		// Token: 0x04000053 RID: 83
		private string m_keyboardsToInstall;

		// Token: 0x04000054 RID: 84
		private string m_specificCultureName = string.Empty;

		// Token: 0x04000055 RID: 85
		private Calendar[] m_calendars;

		// Token: 0x04000056 RID: 86
		private string[] m_calendarNames = new string[23];

		// Token: 0x04000057 RID: 87
		private static readonly int[] m_codePages = new int[]
		{
			1252, 1250, 1251, 1253, 1254, 1255, 1256, 1257, 1258, 0,
			0, 0, 0, 0, 0, 0, 874, 932, 936, 949,
			950, 1361, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 1258, 869, 866,
			865, 864, 863, 862, 861, 860, 857, 855, 852, 775,
			737, 708, 850, 437
		};

		// Token: 0x02000007 RID: 7
		private enum MAX
		{
			// Token: 0x04000059 RID: 89
			SLIST = 4,
			// Token: 0x0400005A RID: 90
			SDECIMAL = 4,
			// Token: 0x0400005B RID: 91
			STHOUSAND = 4,
			// Token: 0x0400005C RID: 92
			SGROUPING = 10,
			// Token: 0x0400005D RID: 93
			IDIGITS = 2,
			// Token: 0x0400005E RID: 94
			ILZERO = 2,
			// Token: 0x0400005F RID: 95
			INEGNUMBER = 2,
			// Token: 0x04000060 RID: 96
			SNATIVEDIGITS = 11,
			// Token: 0x04000061 RID: 97
			SCURRENCY = 6,
			// Token: 0x04000062 RID: 98
			SMONDECSEP = 4,
			// Token: 0x04000063 RID: 99
			SMONTHOUSEP = 4,
			// Token: 0x04000064 RID: 100
			SMONGROUPING = 10,
			// Token: 0x04000065 RID: 101
			SPOSSIGN = 5,
			// Token: 0x04000066 RID: 102
			SNEGSIGN = 5,
			// Token: 0x04000067 RID: 103
			STIMEFORMAT = 79,
			// Token: 0x04000068 RID: 104
			STIME = 4,
			// Token: 0x04000069 RID: 105
			S1159 = 15,
			// Token: 0x0400006A RID: 106
			S2359 = 15,
			// Token: 0x0400006B RID: 107
			SSHORTDATE = 79,
			// Token: 0x0400006C RID: 108
			SDATE = 4,
			// Token: 0x0400006D RID: 109
			SYEARMONTH = 79,
			// Token: 0x0400006E RID: 110
			SLONGDATE = 79
		}
	}
}

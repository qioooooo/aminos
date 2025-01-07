using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class DateAndTime
	{
		public static DateTime Today
		{
			get
			{
				return DateTime.Today;
			}
			[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
			set
			{
				Utils.SetDate(value);
			}
		}

		public static DateTime Now
		{
			get
			{
				return DateTime.Now;
			}
		}

		public static DateTime TimeOfDay
		{
			get
			{
				DateTime now = DateTime.Now;
				long ticks = now.TimeOfDay.Ticks;
				now = new DateTime(checked(ticks - ticks % 10000000L));
				return now;
			}
			[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
			set
			{
				Utils.SetTime(value);
			}
		}

		public static string TimeString
		{
			get
			{
				DateTime dateTime = new DateTime(DateTime.Now.TimeOfDay.Ticks);
				DateTime dateTime2 = dateTime;
				return dateTime2.ToString("HH:mm:ss", Utils.GetInvariantCultureInfo());
			}
			[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
			set
			{
				DateTime dateTime;
				try
				{
					dateTime = DateType.FromString(value, Utils.GetInvariantCultureInfo());
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(value, 32),
						"Date"
					})), 5);
				}
				Utils.SetTime(dateTime);
			}
		}

		private static bool IsDBCSCulture()
		{
			return Marshal.SystemMaxDBCSCharSize != 1;
		}

		public static string DateString
		{
			get
			{
				if (DateAndTime.IsDBCSCulture())
				{
					return DateTime.Today.ToString("yyyy\\-MM\\-dd", Utils.GetInvariantCultureInfo());
				}
				return DateTime.Today.ToString("MM\\-dd\\-yyyy", Utils.GetInvariantCultureInfo());
			}
			[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
			set
			{
				DateTime dateTime;
				try
				{
					string text = Utils.ToHalfwidthNumbers(value, Utils.GetCultureInfo());
					if (DateAndTime.IsDBCSCulture())
					{
						dateTime = DateTime.ParseExact(text, DateAndTime.AcceptedDateFormatsDBCS, Utils.GetInvariantCultureInfo(), DateTimeStyles.AllowWhiteSpaces);
					}
					else
					{
						dateTime = DateTime.ParseExact(text, DateAndTime.AcceptedDateFormatsSBCS, Utils.GetInvariantCultureInfo(), DateTimeStyles.AllowWhiteSpaces);
					}
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(value, 32),
						"Date"
					})), 5);
				}
				Utils.SetDate(dateTime);
			}
		}

		public static double Timer
		{
			get
			{
				return (double)(DateTime.Now.Ticks % 864000000000L) / 10000000.0;
			}
		}

		private static Calendar CurrentCalendar
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture.Calendar;
			}
		}

		public static DateTime DateAdd(DateInterval Interval, double Number, DateTime DateValue)
		{
			int num = checked((int)Math.Round(Conversion.Fix(Number)));
			switch (Interval)
			{
			case DateInterval.Year:
				return DateAndTime.CurrentCalendar.AddYears(DateValue, num);
			case DateInterval.Quarter:
				return DateValue.AddMonths(checked(num * 3));
			case DateInterval.Month:
				return DateAndTime.CurrentCalendar.AddMonths(DateValue, num);
			case DateInterval.DayOfYear:
			case DateInterval.Day:
			case DateInterval.Weekday:
				return DateValue.AddDays((double)num);
			case DateInterval.WeekOfYear:
				return DateValue.AddDays((double)num * 7.0);
			case DateInterval.Hour:
				return DateValue.AddHours((double)num);
			case DateInterval.Minute:
				return DateValue.AddMinutes((double)num);
			case DateInterval.Second:
				return DateValue.AddSeconds((double)num);
			default:
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Interval" }));
			}
		}

		public static long DateDiff(DateInterval Interval, DateTime Date1, DateTime Date2, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
		{
			TimeSpan timeSpan = Date2.Subtract(Date1);
			checked
			{
				switch (Interval)
				{
				case DateInterval.Year:
				{
					Calendar calendar = DateAndTime.CurrentCalendar;
					unchecked
					{
						return (long)(checked(calendar.GetYear(Date2) - calendar.GetYear(Date1)));
					}
				}
				case DateInterval.Quarter:
				{
					Calendar calendar = DateAndTime.CurrentCalendar;
					unchecked
					{
						return (long)(checked((calendar.GetYear(Date2) - calendar.GetYear(Date1)) * 4 + (calendar.GetMonth(Date2) - 1) / 3 - (calendar.GetMonth(Date1) - 1) / 3));
					}
				}
				case DateInterval.Month:
				{
					Calendar calendar = DateAndTime.CurrentCalendar;
					unchecked
					{
						return (long)(checked((calendar.GetYear(Date2) - calendar.GetYear(Date1)) * 12 + calendar.GetMonth(Date2) - calendar.GetMonth(Date1)));
					}
				}
				case DateInterval.DayOfYear:
				case DateInterval.Day:
					return (long)Math.Round(Conversion.Fix(timeSpan.TotalDays));
				case DateInterval.WeekOfYear:
					Date1 = Date1.AddDays((double)(0 - DateAndTime.GetDayOfWeek(Date1, DayOfWeek)));
					Date2 = Date2.AddDays((double)(0 - DateAndTime.GetDayOfWeek(Date2, DayOfWeek)));
					return (long)Math.Round(Conversion.Fix(Date2.Subtract(Date1).TotalDays)) / 7L;
				case DateInterval.Weekday:
					return (long)Math.Round(Conversion.Fix(timeSpan.TotalDays)) / 7L;
				case DateInterval.Hour:
					return (long)Math.Round(Conversion.Fix(timeSpan.TotalHours));
				case DateInterval.Minute:
					return (long)Math.Round(Conversion.Fix(timeSpan.TotalMinutes));
				case DateInterval.Second:
					return (long)Math.Round(Conversion.Fix(timeSpan.TotalSeconds));
				default:
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Interval" }));
				}
			}
		}

		private static int GetDayOfWeek(DateTime dt, FirstDayOfWeek weekdayFirst)
		{
			if (weekdayFirst < FirstDayOfWeek.System || weekdayFirst > FirstDayOfWeek.Saturday)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			checked
			{
				if (weekdayFirst == FirstDayOfWeek.System)
				{
					weekdayFirst = (FirstDayOfWeek)(Utils.GetDateTimeFormatInfo().FirstDayOfWeek + 1);
				}
				return (dt.DayOfWeek - (DayOfWeek)weekdayFirst + 8) % 7 + 1;
			}
		}

		public static int DatePart(DateInterval Interval, DateTime DateValue, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.Sunday, FirstWeekOfYear FirstWeekOfYearValue = FirstWeekOfYear.Jan1)
		{
			checked
			{
				switch (Interval)
				{
				case DateInterval.Year:
					return DateAndTime.CurrentCalendar.GetYear(DateValue);
				case DateInterval.Quarter:
					return (DateValue.Month - 1) / 3 + 1;
				case DateInterval.Month:
					return DateAndTime.CurrentCalendar.GetMonth(DateValue);
				case DateInterval.DayOfYear:
					return DateAndTime.CurrentCalendar.GetDayOfYear(DateValue);
				case DateInterval.Day:
					return DateAndTime.CurrentCalendar.GetDayOfMonth(DateValue);
				case DateInterval.WeekOfYear:
				{
					DayOfWeek dayOfWeek;
					if (FirstDayOfWeekValue == FirstDayOfWeek.System)
					{
						dayOfWeek = Utils.GetCultureInfo().DateTimeFormat.FirstDayOfWeek;
					}
					else
					{
						dayOfWeek = (DayOfWeek)(FirstDayOfWeekValue - 1);
					}
					CalendarWeekRule calendarWeekRule;
					switch (FirstWeekOfYearValue)
					{
					case FirstWeekOfYear.System:
						calendarWeekRule = Utils.GetCultureInfo().DateTimeFormat.CalendarWeekRule;
						break;
					case FirstWeekOfYear.Jan1:
						calendarWeekRule = CalendarWeekRule.FirstDay;
						break;
					case FirstWeekOfYear.FirstFourDays:
						calendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
						break;
					case FirstWeekOfYear.FirstFullWeek:
						calendarWeekRule = CalendarWeekRule.FirstFullWeek;
						break;
					}
					return DateAndTime.CurrentCalendar.GetWeekOfYear(DateValue, calendarWeekRule, dayOfWeek);
				}
				case DateInterval.Weekday:
					return DateAndTime.Weekday(DateValue, FirstDayOfWeekValue);
				case DateInterval.Hour:
					return DateAndTime.CurrentCalendar.GetHour(DateValue);
				case DateInterval.Minute:
					return DateAndTime.CurrentCalendar.GetMinute(DateValue);
				case DateInterval.Second:
					return DateAndTime.CurrentCalendar.GetSecond(DateValue);
				default:
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Interval" }));
				}
			}
		}

		public static DateTime DateAdd(string Interval, double Number, object DateValue)
		{
			DateTime dateTime;
			try
			{
				dateTime = Conversions.ToDate(DateValue);
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new InvalidCastException(Utils.GetResourceString("Argument_InvalidDateValue1", new string[] { "DateValue" }));
			}
			return DateAndTime.DateAdd(DateAndTime.DateIntervalFromString(Interval), Number, dateTime);
		}

		public static long DateDiff(string Interval, object Date1, object Date2, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
		{
			DateTime dateTime;
			try
			{
				dateTime = Conversions.ToDate(Date1);
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new InvalidCastException(Utils.GetResourceString("Argument_InvalidDateValue1", new string[] { "Date1" }));
			}
			DateTime dateTime2;
			try
			{
				dateTime2 = Conversions.ToDate(Date2);
			}
			catch (StackOverflowException ex4)
			{
				throw ex4;
			}
			catch (OutOfMemoryException ex5)
			{
				throw ex5;
			}
			catch (ThreadAbortException ex6)
			{
				throw ex6;
			}
			catch (Exception)
			{
				throw new InvalidCastException(Utils.GetResourceString("Argument_InvalidDateValue1", new string[] { "Date2" }));
			}
			return DateAndTime.DateDiff(DateAndTime.DateIntervalFromString(Interval), dateTime, dateTime2, DayOfWeek, WeekOfYear);
		}

		public static int DatePart(string Interval, object DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday, FirstWeekOfYear WeekOfYear = FirstWeekOfYear.Jan1)
		{
			DateTime dateTime;
			try
			{
				dateTime = Conversions.ToDate(DateValue);
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				throw new InvalidCastException(Utils.GetResourceString("Argument_InvalidDateValue1", new string[] { "DateValue" }));
			}
			return DateAndTime.DatePart(DateAndTime.DateIntervalFromString(Interval), dateTime, DayOfWeek, WeekOfYear);
		}

		private static DateInterval DateIntervalFromString(string Interval)
		{
			if (Interval != null)
			{
				Interval = Interval.ToUpperInvariant();
			}
			string text = Interval;
			if (Operators.CompareString(text, "YYYY", false) == 0)
			{
				return DateInterval.Year;
			}
			if (Operators.CompareString(text, "Y", false) == 0)
			{
				return DateInterval.DayOfYear;
			}
			if (Operators.CompareString(text, "M", false) == 0)
			{
				return DateInterval.Month;
			}
			if (Operators.CompareString(text, "D", false) == 0)
			{
				return DateInterval.Day;
			}
			if (Operators.CompareString(text, "H", false) == 0)
			{
				return DateInterval.Hour;
			}
			if (Operators.CompareString(text, "N", false) == 0)
			{
				return DateInterval.Minute;
			}
			if (Operators.CompareString(text, "S", false) == 0)
			{
				return DateInterval.Second;
			}
			if (Operators.CompareString(text, "WW", false) == 0)
			{
				return DateInterval.WeekOfYear;
			}
			if (Operators.CompareString(text, "W", false) == 0)
			{
				return DateInterval.Weekday;
			}
			if (Operators.CompareString(text, "Q", false) == 0)
			{
				return DateInterval.Quarter;
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Interval" }));
		}

		public static DateTime DateSerial(int Year, int Month, int Day)
		{
			Calendar currentCalendar = DateAndTime.CurrentCalendar;
			checked
			{
				if (Year < 0)
				{
					Year = currentCalendar.GetYear(DateTime.Today) + Year;
				}
				else if (Year < 100)
				{
					Year = currentCalendar.ToFourDigitYear(Year);
				}
				if (currentCalendar is GregorianCalendar && Month >= 1 && Month <= 12 && Day >= 1 && Day <= 28)
				{
					DateTime dateTime = new DateTime(Year, Month, Day);
					return dateTime;
				}
				DateTime dateTime2;
				try
				{
					dateTime2 = currentCalendar.ToDateTime(Year, 1, 1, 0, 0, 0, 0);
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Year" })), 5);
				}
				try
				{
					dateTime2 = currentCalendar.AddMonths(dateTime2, Month - 1);
				}
				catch (StackOverflowException ex4)
				{
					throw ex4;
				}
				catch (OutOfMemoryException ex5)
				{
					throw ex5;
				}
				catch (ThreadAbortException ex6)
				{
					throw ex6;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Month" })), 5);
				}
				try
				{
					dateTime2 = currentCalendar.AddDays(dateTime2, Day - 1);
				}
				catch (StackOverflowException ex7)
				{
					throw ex7;
				}
				catch (OutOfMemoryException ex8)
				{
					throw ex8;
				}
				catch (ThreadAbortException ex9)
				{
					throw ex9;
				}
				catch (Exception)
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Day" })), 5);
				}
				return dateTime2;
			}
		}

		public static DateTime TimeSerial(int Hour, int Minute, int Second)
		{
			checked
			{
				int num = Hour * 60 * 60 + Minute * 60 + Second;
				if (num < 0)
				{
					num += 86400;
				}
				DateTime dateTime = new DateTime(unchecked((long)num) * 10000000L);
				return dateTime;
			}
		}

		public static DateTime DateValue(string StringDate)
		{
			return Conversions.ToDate(StringDate).Date;
		}

		public static DateTime TimeValue(string StringTime)
		{
			DateTime dateTime = new DateTime(Conversions.ToDate(StringTime).Ticks % 864000000000L);
			return dateTime;
		}

		public static int Year(DateTime DateValue)
		{
			return DateAndTime.CurrentCalendar.GetYear(DateValue);
		}

		public static int Month(DateTime DateValue)
		{
			return DateAndTime.CurrentCalendar.GetMonth(DateValue);
		}

		public static int Day(DateTime DateValue)
		{
			return DateAndTime.CurrentCalendar.GetDayOfMonth(DateValue);
		}

		public static int Hour(DateTime TimeValue)
		{
			return DateAndTime.CurrentCalendar.GetHour(TimeValue);
		}

		public static int Minute(DateTime TimeValue)
		{
			return DateAndTime.CurrentCalendar.GetMinute(TimeValue);
		}

		public static int Second(DateTime TimeValue)
		{
			return DateAndTime.CurrentCalendar.GetSecond(TimeValue);
		}

		public static int Weekday(DateTime DateValue, FirstDayOfWeek DayOfWeek = FirstDayOfWeek.Sunday)
		{
			checked
			{
				if (DayOfWeek == FirstDayOfWeek.System)
				{
					DayOfWeek = (FirstDayOfWeek)(DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + 1);
				}
				else if (DayOfWeek < FirstDayOfWeek.Sunday || DayOfWeek > FirstDayOfWeek.Saturday)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "DayOfWeek" }));
				}
				int num = (int)(DateAndTime.CurrentCalendar.GetDayOfWeek(DateValue) + 1);
				return (num - (int)DayOfWeek + 7) % 7 + 1;
			}
		}

		public static string MonthName(int Month, bool Abbreviate = false)
		{
			if (Month < 1 || Month > 13)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Month" }));
			}
			string text;
			if (Abbreviate)
			{
				text = Utils.GetDateTimeFormatInfo().GetAbbreviatedMonthName(Month);
			}
			else
			{
				text = Utils.GetDateTimeFormatInfo().GetMonthName(Month);
			}
			if (text.Length == 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Month" }));
			}
			return text;
		}

		public static string WeekdayName(int Weekday, bool Abbreviate = false, FirstDayOfWeek FirstDayOfWeekValue = FirstDayOfWeek.System)
		{
			if (Weekday < 1 || Weekday > 7)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Weekday" }));
			}
			if (FirstDayOfWeekValue < FirstDayOfWeek.System || FirstDayOfWeekValue > FirstDayOfWeek.Saturday)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "FirstDayOfWeekValue" }));
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)Utils.GetCultureInfo().GetFormat(typeof(DateTimeFormatInfo));
			checked
			{
				if (FirstDayOfWeekValue == FirstDayOfWeek.System)
				{
					FirstDayOfWeekValue = (FirstDayOfWeek)(dateTimeFormatInfo.FirstDayOfWeek + 1);
				}
				string text;
				try
				{
					if (Abbreviate)
					{
						text = dateTimeFormatInfo.GetAbbreviatedDayName((DayOfWeek)((Weekday + FirstDayOfWeekValue - 2) % FirstDayOfWeek.Saturday));
					}
					else
					{
						text = dateTimeFormatInfo.GetDayName((DayOfWeek)((Weekday + FirstDayOfWeekValue - 2) % FirstDayOfWeek.Saturday));
					}
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Weekday" }));
				}
				if (text.Length == 0)
				{
					throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Weekday" }));
				}
				return text;
			}
		}

		private static string[] AcceptedDateFormatsDBCS = new string[] { "yyyy-M-d", "y-M-d", "yyyy/M/d", "y/M/d" };

		private static string[] AcceptedDateFormatsSBCS = new string[] { "M-d-yyyy", "M-d-y", "M/d/yyyy", "M/d/y" };
	}
}

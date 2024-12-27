using System;
using System.Globalization;

namespace System.Management
{
	// Token: 0x0200000C RID: 12
	public sealed class ManagementDateTimeConverter
	{
		// Token: 0x0600009A RID: 154 RVA: 0x000058A3 File Offset: 0x000048A3
		private ManagementDateTimeConverter()
		{
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000058AC File Offset: 0x000048AC
		public static DateTime ToDateTime(string dmtfDate)
		{
			int num = DateTime.MinValue.Year;
			int num2 = DateTime.MinValue.Month;
			int num3 = DateTime.MinValue.Day;
			int num4 = DateTime.MinValue.Hour;
			int num5 = DateTime.MinValue.Minute;
			int num6 = DateTime.MinValue.Second;
			int num7 = 0;
			DateTime dateTime = DateTime.MinValue;
			if (dmtfDate == null)
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			if (dmtfDate.Length == 0)
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			if (dmtfDate.Length != 25)
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			IFormatProvider formatProvider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			long num8 = 0L;
			try
			{
				string text = string.Empty;
				text = dmtfDate.Substring(0, 4);
				if ("****" != text)
				{
					num = int.Parse(text, formatProvider);
				}
				text = dmtfDate.Substring(4, 2);
				if ("**" != text)
				{
					num2 = int.Parse(text, formatProvider);
				}
				text = dmtfDate.Substring(6, 2);
				if ("**" != text)
				{
					num3 = int.Parse(text, formatProvider);
				}
				text = dmtfDate.Substring(8, 2);
				if ("**" != text)
				{
					num4 = int.Parse(text, formatProvider);
				}
				text = dmtfDate.Substring(10, 2);
				if ("**" != text)
				{
					num5 = int.Parse(text, formatProvider);
				}
				text = dmtfDate.Substring(12, 2);
				if ("**" != text)
				{
					num6 = int.Parse(text, formatProvider);
				}
				text = dmtfDate.Substring(15, 6);
				if ("******" != text)
				{
					num8 = long.Parse(text, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))) * 10L;
				}
				if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0 || num6 < 0 || num8 < 0L)
				{
					throw new ArgumentOutOfRangeException("dmtfDate");
				}
			}
			catch
			{
				throw new ArgumentOutOfRangeException("dmtfDate");
			}
			dateTime = new DateTime(num, num2, num3, num4, num5, num6, num7);
			dateTime = dateTime.AddTicks(num8);
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			long num9 = currentTimeZone.GetUtcOffset(dateTime).Ticks / 600000000L;
			int num10 = 0;
			string text2 = dmtfDate.Substring(22, 3);
			if ("***" != text2)
			{
				text2 = dmtfDate.Substring(21, 4);
				try
				{
					num10 = int.Parse(text2, formatProvider);
				}
				catch
				{
					throw new ArgumentOutOfRangeException();
				}
				long num11 = (long)num10 - num9;
				dateTime = dateTime.AddMinutes((double)(num11 * -1L));
			}
			return dateTime;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005B9C File Offset: 0x00004B9C
		public static string ToDmtfDateTime(DateTime date)
		{
			string text = string.Empty;
			TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
			TimeSpan utcOffset = currentTimeZone.GetUtcOffset(date);
			long num = utcOffset.Ticks / 600000000L;
			IFormatProvider formatProvider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			if (Math.Abs(num) > 999L)
			{
				date = date.ToUniversalTime();
				text = "+000";
			}
			else if (utcOffset.Ticks >= 0L)
			{
				text = "+" + (utcOffset.Ticks / 600000000L).ToString(formatProvider).PadLeft(3, '0');
			}
			else
			{
				string text2 = num.ToString(formatProvider);
				text = "-" + text2.Substring(1, text2.Length - 1).PadLeft(3, '0');
			}
			string text3 = date.Year.ToString(formatProvider).PadLeft(4, '0');
			text3 += date.Month.ToString(formatProvider).PadLeft(2, '0');
			text3 += date.Day.ToString(formatProvider).PadLeft(2, '0');
			text3 += date.Hour.ToString(formatProvider).PadLeft(2, '0');
			text3 += date.Minute.ToString(formatProvider).PadLeft(2, '0');
			text3 += date.Second.ToString(formatProvider).PadLeft(2, '0');
			text3 += ".";
			DateTime dateTime = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
			string text4 = ((date.Ticks - dateTime.Ticks) * 1000L / 10000L).ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long)));
			if (text4.Length > 6)
			{
				text4 = text4.Substring(0, 6);
			}
			text3 += text4.PadLeft(6, '0');
			return text3 + text;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005DF0 File Offset: 0x00004DF0
		public static TimeSpan ToTimeSpan(string dmtfTimespan)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			IFormatProvider formatProvider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			TimeSpan timeSpan = TimeSpan.MinValue;
			if (dmtfTimespan == null)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (dmtfTimespan.Length == 0)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (dmtfTimespan.Length != 25)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (dmtfTimespan.Substring(21, 4) != ":000")
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			long num5 = 0L;
			try
			{
				string text = string.Empty;
				text = dmtfTimespan.Substring(0, 8);
				num = int.Parse(text, formatProvider);
				text = dmtfTimespan.Substring(8, 2);
				num2 = int.Parse(text, formatProvider);
				text = dmtfTimespan.Substring(10, 2);
				num3 = int.Parse(text, formatProvider);
				text = dmtfTimespan.Substring(12, 2);
				num4 = int.Parse(text, formatProvider);
				text = dmtfTimespan.Substring(15, 6);
				num5 = long.Parse(text, (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long))) * 10L;
			}
			catch
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0L)
			{
				throw new ArgumentOutOfRangeException("dmtfTimespan");
			}
			timeSpan = new TimeSpan(num, num2, num3, num4, 0);
			TimeSpan timeSpan2 = TimeSpan.FromTicks(num5);
			timeSpan += timeSpan2;
			return timeSpan;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005F78 File Offset: 0x00004F78
		public static string ToDmtfTimeInterval(TimeSpan timespan)
		{
			string text = timespan.Days.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int))).PadLeft(8, '0');
			IFormatProvider formatProvider = (IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(int));
			if ((long)timespan.Days > 99999999L || timespan < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException();
			}
			text += timespan.Hours.ToString(formatProvider).PadLeft(2, '0');
			text += timespan.Minutes.ToString(formatProvider).PadLeft(2, '0');
			text += timespan.Seconds.ToString(formatProvider).PadLeft(2, '0');
			text += ".";
			TimeSpan timeSpan = new TimeSpan(timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds, 0);
			string text2 = ((timespan.Ticks - timeSpan.Ticks) * 1000L / 10000L).ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(long)));
			if (text2.Length > 6)
			{
				text2 = text2.Substring(0, 6);
			}
			text += text2.PadLeft(6, '0');
			return text + ":000";
		}

		// Token: 0x0400007B RID: 123
		private const int SIZEOFDMTFDATETIME = 25;

		// Token: 0x0400007C RID: 124
		private const int MAXSIZE_UTC_DMTF = 999;

		// Token: 0x0400007D RID: 125
		private const long MAXDATE_INTIMESPAN = 99999999L;
	}
}

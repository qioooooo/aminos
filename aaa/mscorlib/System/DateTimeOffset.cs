using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct DateTimeOffset : IComparable, IFormattable, ISerializable, IDeserializationCallback, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>
	{
		// Token: 0x06000320 RID: 800 RVA: 0x0000D484 File Offset: 0x0000C484
		public DateTimeOffset(long ticks, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			DateTime dateTime = new DateTime(ticks);
			this.m_dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000D4B4 File Offset: 0x0000C4B4
		public DateTimeOffset(DateTime dateTime)
		{
			TimeSpan utcOffset;
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
			}
			else
			{
				utcOffset = new TimeSpan(0L);
			}
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(utcOffset);
			this.m_dateTime = DateTimeOffset.ValidateDate(dateTime, utcOffset);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000D4FC File Offset: 0x0000C4FC
		public DateTimeOffset(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Local)
			{
				if (offset != TimeZone.CurrentTimeZone.GetUtcOffset(dateTime))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_OffsetLocalMismatch"), "offset");
				}
			}
			else if (dateTime.Kind == DateTimeKind.Utc && offset != TimeSpan.Zero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OffsetUtcMismatch"), "offset");
			}
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			this.m_dateTime = DateTimeOffset.ValidateDate(dateTime, offset);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000D580 File Offset: 0x0000C580
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.m_dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second), offset);
			if (num == 60 && !DateTime.IsValidTimeWithLeapSeconds(this.m_dateTime.Year, this.m_dateTime.Month, this.m_dateTime.Day, this.m_dateTime.Hour, this.m_dateTime.Minute, 60, DateTimeKind.Utc))
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000D620 File Offset: 0x0000C620
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.m_dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond), offset);
			if (num == 60 && !DateTime.IsValidTimeWithLeapSeconds(this.m_dateTime.Year, this.m_dateTime.Month, this.m_dateTime.Day, this.m_dateTime.Hour, this.m_dateTime.Minute, 60, DateTimeKind.Utc))
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000D6C4 File Offset: 0x0000C6C4
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
		{
			this.m_offsetMinutes = DateTimeOffset.ValidateOffset(offset);
			int num = second;
			if (second == 60 && DateTime.s_isLeapSecondsSupportedSystem)
			{
				second = 59;
			}
			this.m_dateTime = DateTimeOffset.ValidateDate(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset);
			if (num == 60 && !DateTime.IsValidTimeWithLeapSeconds(this.m_dateTime.Year, this.m_dateTime.Month, this.m_dateTime.Day, this.m_dateTime.Hour, this.m_dateTime.Minute, 60, DateTimeKind.Utc))
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond"));
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000D768 File Offset: 0x0000C768
		public static DateTimeOffset Now
		{
			get
			{
				return new DateTimeOffset(DateTime.Now);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000D774 File Offset: 0x0000C774
		public static DateTimeOffset UtcNow
		{
			get
			{
				return new DateTimeOffset(DateTime.UtcNow);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000D780 File Offset: 0x0000C780
		public DateTime DateTime
		{
			get
			{
				return this.ClockDateTime;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000D788 File Offset: 0x0000C788
		public DateTime UtcDateTime
		{
			get
			{
				return DateTime.SpecifyKind(this.m_dateTime, DateTimeKind.Utc);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000D798 File Offset: 0x0000C798
		public DateTime LocalDateTime
		{
			get
			{
				return this.UtcDateTime.ToLocalTime();
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D7B4 File Offset: 0x0000C7B4
		public DateTimeOffset ToOffset(TimeSpan offset)
		{
			return new DateTimeOffset((this.m_dateTime + offset).Ticks, offset);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000D7DC File Offset: 0x0000C7DC
		private DateTime ClockDateTime
		{
			get
			{
				return new DateTime((this.m_dateTime + this.Offset).Ticks, DateTimeKind.Unspecified);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000D808 File Offset: 0x0000C808
		public DateTime Date
		{
			get
			{
				return this.ClockDateTime.Date;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000D824 File Offset: 0x0000C824
		public int Day
		{
			get
			{
				return this.ClockDateTime.Day;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000D840 File Offset: 0x0000C840
		public DayOfWeek DayOfWeek
		{
			get
			{
				return this.ClockDateTime.DayOfWeek;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000D85C File Offset: 0x0000C85C
		public int DayOfYear
		{
			get
			{
				return this.ClockDateTime.DayOfYear;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000D878 File Offset: 0x0000C878
		public int Hour
		{
			get
			{
				return this.ClockDateTime.Hour;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000D894 File Offset: 0x0000C894
		public int Millisecond
		{
			get
			{
				return this.ClockDateTime.Millisecond;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000D8B0 File Offset: 0x0000C8B0
		public int Minute
		{
			get
			{
				return this.ClockDateTime.Minute;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000D8CC File Offset: 0x0000C8CC
		public int Month
		{
			get
			{
				return this.ClockDateTime.Month;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000D8E7 File Offset: 0x0000C8E7
		public TimeSpan Offset
		{
			get
			{
				return new TimeSpan(0, (int)this.m_offsetMinutes, 0);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000D8F8 File Offset: 0x0000C8F8
		public int Second
		{
			get
			{
				return this.ClockDateTime.Second;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000D914 File Offset: 0x0000C914
		public long Ticks
		{
			get
			{
				return this.ClockDateTime.Ticks;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000D930 File Offset: 0x0000C930
		public long UtcTicks
		{
			get
			{
				return this.UtcDateTime.Ticks;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000D94C File Offset: 0x0000C94C
		public TimeSpan TimeOfDay
		{
			get
			{
				return this.ClockDateTime.TimeOfDay;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000D968 File Offset: 0x0000C968
		public int Year
		{
			get
			{
				return this.ClockDateTime.Year;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000D984 File Offset: 0x0000C984
		public DateTimeOffset Add(TimeSpan timeSpan)
		{
			return new DateTimeOffset(this.ClockDateTime.Add(timeSpan), this.Offset);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000D9AC File Offset: 0x0000C9AC
		public DateTimeOffset AddDays(double days)
		{
			return new DateTimeOffset(this.ClockDateTime.AddDays(days), this.Offset);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000D9D4 File Offset: 0x0000C9D4
		public DateTimeOffset AddHours(double hours)
		{
			return new DateTimeOffset(this.ClockDateTime.AddHours(hours), this.Offset);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000D9FC File Offset: 0x0000C9FC
		public DateTimeOffset AddMilliseconds(double milliseconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMilliseconds(milliseconds), this.Offset);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000DA24 File Offset: 0x0000CA24
		public DateTimeOffset AddMinutes(double minutes)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMinutes(minutes), this.Offset);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000DA4C File Offset: 0x0000CA4C
		public DateTimeOffset AddMonths(int months)
		{
			return new DateTimeOffset(this.ClockDateTime.AddMonths(months), this.Offset);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000DA74 File Offset: 0x0000CA74
		public DateTimeOffset AddSeconds(double seconds)
		{
			return new DateTimeOffset(this.ClockDateTime.AddSeconds(seconds), this.Offset);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000DA9C File Offset: 0x0000CA9C
		public DateTimeOffset AddTicks(long ticks)
		{
			return new DateTimeOffset(this.ClockDateTime.AddTicks(ticks), this.Offset);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000DAC4 File Offset: 0x0000CAC4
		public DateTimeOffset AddYears(int years)
		{
			return new DateTimeOffset(this.ClockDateTime.AddYears(years), this.Offset);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000DAEB File Offset: 0x0000CAEB
		public static int Compare(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Compare(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000DB00 File Offset: 0x0000CB00
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is DateTimeOffset))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDateTimeOffset"));
			}
			DateTime utcDateTime = ((DateTimeOffset)obj).UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000DB58 File Offset: 0x0000CB58
		public int CompareTo(DateTimeOffset other)
		{
			DateTime utcDateTime = other.UtcDateTime;
			DateTime utcDateTime2 = this.UtcDateTime;
			if (utcDateTime2 > utcDateTime)
			{
				return 1;
			}
			if (utcDateTime2 < utcDateTime)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000DB8C File Offset: 0x0000CB8C
		public override bool Equals(object obj)
		{
			return obj is DateTimeOffset && this.UtcDateTime.Equals(((DateTimeOffset)obj).UtcDateTime);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000DBC0 File Offset: 0x0000CBC0
		public bool Equals(DateTimeOffset other)
		{
			return this.UtcDateTime.Equals(other.UtcDateTime);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000DBE4 File Offset: 0x0000CBE4
		public bool EqualsExact(DateTimeOffset other)
		{
			return this.ClockDateTime == other.ClockDateTime && this.Offset == other.Offset && this.ClockDateTime.Kind == other.ClockDateTime.Kind;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000DC3A File Offset: 0x0000CC3A
		public static bool Equals(DateTimeOffset first, DateTimeOffset second)
		{
			return DateTime.Equals(first.UtcDateTime, second.UtcDateTime);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000DC4F File Offset: 0x0000CC4F
		public static DateTimeOffset FromFileTime(long fileTime)
		{
			return new DateTimeOffset(DateTime.FromFileTime(fileTime));
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000DC5C File Offset: 0x0000CC5C
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			try
			{
				this.m_offsetMinutes = DateTimeOffset.ValidateOffset(this.Offset);
				this.m_dateTime = DateTimeOffset.ValidateDate(this.ClockDateTime, this.Offset);
			}
			catch (ArgumentException ex)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidData"), ex);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000DCB8 File Offset: 0x0000CCB8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("DateTime", this.m_dateTime);
			info.AddValue("OffsetMinutes", this.m_offsetMinutes);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000DCEC File Offset: 0x0000CCEC
		private DateTimeOffset(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_dateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
			this.m_offsetMinutes = (short)info.GetValue("OffsetMinutes", typeof(short));
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000DD48 File Offset: 0x0000CD48
		public override int GetHashCode()
		{
			return this.UtcDateTime.GetHashCode();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000DD6C File Offset: 0x0000CD6C
		public static DateTimeOffset Parse(string input)
		{
			TimeSpan timeSpan;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out timeSpan).Ticks, timeSpan);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000DD95 File Offset: 0x0000CD95
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider)
		{
			return DateTimeOffset.Parse(input, formatProvider, DateTimeStyles.None);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000DDA0 File Offset: 0x0000CDA0
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan timeSpan;
			return new DateTimeOffset(DateTimeParse.Parse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out timeSpan).Ticks, timeSpan);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000DDD7 File Offset: 0x0000CDD7
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			return DateTimeOffset.ParseExact(input, format, formatProvider, DateTimeStyles.None);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000DDE4 File Offset: 0x0000CDE4
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan timeSpan;
			return new DateTimeOffset(DateTimeParse.ParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out timeSpan).Ticks, timeSpan);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000DE1C File Offset: 0x0000CE1C
		public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			TimeSpan timeSpan;
			return new DateTimeOffset(DateTimeParse.ParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out timeSpan).Ticks, timeSpan);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000DE54 File Offset: 0x0000CE54
		public TimeSpan Subtract(DateTimeOffset value)
		{
			return this.UtcDateTime.Subtract(value.UtcDateTime);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000DE78 File Offset: 0x0000CE78
		public DateTimeOffset Subtract(TimeSpan value)
		{
			return new DateTimeOffset(this.ClockDateTime.Subtract(value), this.Offset);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000DEA0 File Offset: 0x0000CEA0
		public long ToFileTime()
		{
			return this.UtcDateTime.ToFileTime();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000DEBC File Offset: 0x0000CEBC
		public DateTimeOffset ToLocalTime()
		{
			return new DateTimeOffset(this.UtcDateTime.ToLocalTime());
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000DEDC File Offset: 0x0000CEDC
		public override string ToString()
		{
			return DateTimeFormat.Format(this.ClockDateTime, null, DateTimeFormatInfo.CurrentInfo, this.Offset);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000DEF5 File Offset: 0x0000CEF5
		public string ToString(string format)
		{
			return DateTimeFormat.Format(this.ClockDateTime, format, DateTimeFormatInfo.CurrentInfo, this.Offset);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000DF0E File Offset: 0x0000CF0E
		public string ToString(IFormatProvider formatProvider)
		{
			return DateTimeFormat.Format(this.ClockDateTime, null, DateTimeFormatInfo.GetInstance(formatProvider), this.Offset);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000DF28 File Offset: 0x0000CF28
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return DateTimeFormat.Format(this.ClockDateTime, format, DateTimeFormatInfo.GetInstance(formatProvider), this.Offset);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000DF42 File Offset: 0x0000CF42
		public DateTimeOffset ToUniversalTime()
		{
			return new DateTimeOffset(this.UtcDateTime);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000DF50 File Offset: 0x0000CF50
		public static bool TryParse(string input, out DateTimeOffset result)
		{
			DateTime dateTime;
			TimeSpan timeSpan;
			bool flag = DateTimeParse.TryParse(input, DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out dateTime, out timeSpan);
			result = new DateTimeOffset(dateTime.Ticks, timeSpan);
			return flag;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000DF84 File Offset: 0x0000CF84
		public static bool TryParse(string input, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan timeSpan;
			bool flag = DateTimeParse.TryParse(input, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out timeSpan);
			result = new DateTimeOffset(dateTime.Ticks, timeSpan);
			return flag;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000DFC4 File Offset: 0x0000CFC4
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan timeSpan;
			bool flag = DateTimeParse.TryParseExact(input, format, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out timeSpan);
			result = new DateTimeOffset(dateTime.Ticks, timeSpan);
			return flag;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000E008 File Offset: 0x0000D008
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			styles = DateTimeOffset.ValidateStyles(styles, "styles");
			DateTime dateTime;
			TimeSpan timeSpan;
			bool flag = DateTimeParse.TryParseExactMultiple(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out dateTime, out timeSpan);
			result = new DateTimeOffset(dateTime.Ticks, timeSpan);
			return flag;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000E04C File Offset: 0x0000D04C
		private static short ValidateOffset(TimeSpan offset)
		{
			long ticks = offset.Ticks;
			if (ticks % 600000000L != 0L)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_OffsetPrecision"), "offset");
			}
			if (ticks < -504000000000L || ticks > 504000000000L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("Argument_OffsetOutOfRange"));
			}
			return (short)(offset.Ticks / 600000000L);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000E0C0 File Offset: 0x0000D0C0
		private static DateTime ValidateDate(DateTime dateTime, TimeSpan offset)
		{
			long num = dateTime.Ticks - offset.Ticks;
			if (num < 0L || num > 3155378975999999999L)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("Argument_UTCOutOfRange"));
			}
			return new DateTime(num, DateTimeKind.Unspecified);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000E10C File Offset: 0x0000D10C
		private static DateTimeStyles ValidateStyles(DateTimeStyles style, string parameterName)
		{
			if ((style & ~(DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal | DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind)) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDateTimeStyles"), parameterName);
			}
			if ((style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None && (style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ConflictingDateTimeStyles"), parameterName);
			}
			if ((style & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DateTimeOffsetInvalidDateTimeStyles"), parameterName);
			}
			style &= ~DateTimeStyles.RoundtripKind;
			style &= ~DateTimeStyles.AssumeLocal;
			return style;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000E176 File Offset: 0x0000D176
		public static implicit operator DateTimeOffset(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000E17E File Offset: 0x0000D17E
		public static DateTimeOffset operator +(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeTz.ClockDateTime + timeSpan, dateTimeTz.Offset);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000E199 File Offset: 0x0000D199
		public static DateTimeOffset operator -(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return new DateTimeOffset(dateTimeTz.ClockDateTime - timeSpan, dateTimeTz.Offset);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000E1B4 File Offset: 0x0000D1B4
		public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime - right.UtcDateTime;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000E1C9 File Offset: 0x0000D1C9
		public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime == right.UtcDateTime;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000E1DE File Offset: 0x0000D1DE
		public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime != right.UtcDateTime;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000E1F3 File Offset: 0x0000D1F3
		public static bool operator <(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime < right.UtcDateTime;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000E208 File Offset: 0x0000D208
		public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime <= right.UtcDateTime;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000E21D File Offset: 0x0000D21D
		public static bool operator >(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime > right.UtcDateTime;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000E232 File Offset: 0x0000D232
		public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime >= right.UtcDateTime;
		}

		// Token: 0x04000103 RID: 259
		internal const long MaxOffset = 504000000000L;

		// Token: 0x04000104 RID: 260
		internal const long MinOffset = -504000000000L;

		// Token: 0x04000105 RID: 261
		public static readonly DateTimeOffset MinValue = new DateTimeOffset(0L, TimeSpan.Zero);

		// Token: 0x04000106 RID: 262
		public static readonly DateTimeOffset MaxValue = new DateTimeOffset(3155378975999999999L, TimeSpan.Zero);

		// Token: 0x04000107 RID: 263
		private DateTime m_dateTime;

		// Token: 0x04000108 RID: 264
		private short m_offsetMinutes;
	}
}

using System;
using System.Data.Common;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Data.SqlTypes
{
	// Token: 0x02000348 RID: 840
	[XmlSchemaProvider("GetXsdType")]
	[Serializable]
	public struct SqlDateTime : INullable, IComparable, IXmlSerializable
	{
		// Token: 0x06002C53 RID: 11347 RVA: 0x002A51CC File Offset: 0x002A45CC
		private SqlDateTime(bool fNull)
		{
			this.m_fNotNull = false;
			this.m_day = 0;
			this.m_time = 0;
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x002A51F0 File Offset: 0x002A45F0
		public SqlDateTime(DateTime value)
		{
			this = SqlDateTime.FromDateTime(value);
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x002A520C File Offset: 0x002A460C
		public SqlDateTime(int year, int month, int day)
		{
			this = new SqlDateTime(year, month, day, 0, 0, 0, 0.0);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x002A5230 File Offset: 0x002A4630
		public SqlDateTime(int year, int month, int day, int hour, int minute, int second)
		{
			this = new SqlDateTime(year, month, day, hour, minute, second, 0.0);
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x002A5258 File Offset: 0x002A4658
		public SqlDateTime(int year, int month, int day, int hour, int minute, int second, double millisecond)
		{
			if (year >= SqlDateTime.MinYear && year <= SqlDateTime.MaxYear && month >= 1 && month <= 12)
			{
				int[] array = (SqlDateTime.IsLeapYear(year) ? SqlDateTime.DaysToMonth366 : SqlDateTime.DaysToMonth365);
				if (day >= 1 && day <= array[month] - array[month - 1])
				{
					int num = year - 1;
					int num2 = num * 365 + num / 4 - num / 100 + num / 400 + array[month - 1] + day - 1;
					num2 -= SqlDateTime.DayBase;
					if (num2 >= SqlDateTime.MinDay && num2 <= SqlDateTime.MaxDay && hour >= 0 && hour < 24 && minute >= 0 && minute < 60 && second >= 0 && second < 60 && millisecond >= 0.0 && millisecond < 1000.0)
					{
						double num3 = millisecond * SqlDateTime.SQLTicksPerMillisecond + 0.5;
						int num4 = hour * SqlDateTime.SQLTicksPerHour + minute * SqlDateTime.SQLTicksPerMinute + second * SqlDateTime.SQLTicksPerSecond + (int)num3;
						if (num4 > SqlDateTime.MaxTime)
						{
							num4 = 0;
							num2++;
						}
						this = new SqlDateTime(num2, num4);
						return;
					}
				}
			}
			throw new SqlTypeException(SQLResource.InvalidDateTimeMessage);
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x002A539C File Offset: 0x002A479C
		public SqlDateTime(int year, int month, int day, int hour, int minute, int second, int bilisecond)
		{
			this = new SqlDateTime(year, month, day, hour, minute, second, (double)bilisecond / 1000.0);
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x002A53C8 File Offset: 0x002A47C8
		public SqlDateTime(int dayTicks, int timeTicks)
		{
			if (dayTicks < SqlDateTime.MinDay || dayTicks > SqlDateTime.MaxDay || timeTicks < SqlDateTime.MinTime || timeTicks > SqlDateTime.MaxTime)
			{
				this.m_fNotNull = false;
				throw new OverflowException(SQLResource.DateTimeOverflowMessage);
			}
			this.m_day = dayTicks;
			this.m_time = timeTicks;
			this.m_fNotNull = true;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x002A541C File Offset: 0x002A481C
		internal SqlDateTime(double dblVal)
		{
			if (dblVal < (double)SqlDateTime.MinDay || dblVal >= (double)(SqlDateTime.MaxDay + 1))
			{
				throw new OverflowException(SQLResource.DateTimeOverflowMessage);
			}
			int num = (int)dblVal;
			int num2 = (int)((dblVal - (double)num) * (double)SqlDateTime.SQLTicksPerDay);
			if (num2 < 0)
			{
				num--;
				num2 += SqlDateTime.SQLTicksPerDay;
			}
			else if (num2 >= SqlDateTime.SQLTicksPerDay)
			{
				num++;
				num2 -= SqlDateTime.SQLTicksPerDay;
			}
			this = new SqlDateTime(num, num2);
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x002A548C File Offset: 0x002A488C
		public bool IsNull
		{
			get
			{
				return !this.m_fNotNull;
			}
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x002A54A4 File Offset: 0x002A48A4
		private static TimeSpan ToTimeSpan(SqlDateTime value)
		{
			long num = (long)((double)value.m_time / SqlDateTime.SQLTicksPerMillisecond + 0.5);
			return new TimeSpan((long)value.m_day * 864000000000L + num * 10000L);
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x002A54EC File Offset: 0x002A48EC
		private static DateTime ToDateTime(SqlDateTime value)
		{
			return SqlDateTime.SQLBaseDate.Add(SqlDateTime.ToTimeSpan(value));
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x002A550C File Offset: 0x002A490C
		internal static DateTime ToDateTime(int daypart, int timepart)
		{
			if (daypart < SqlDateTime.MinDay || daypart > SqlDateTime.MaxDay || timepart < SqlDateTime.MinTime || timepart > SqlDateTime.MaxTime)
			{
				throw new OverflowException(SQLResource.DateTimeOverflowMessage);
			}
			long num = (long)daypart * 864000000000L;
			long num2 = (long)((double)timepart / SqlDateTime.SQLTicksPerMillisecond + 0.5) * 10000L;
			DateTime dateTime = new DateTime(SqlDateTime.SQLBaseDateTicks + num + num2);
			return dateTime;
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x002A5580 File Offset: 0x002A4980
		private static SqlDateTime FromTimeSpan(TimeSpan value)
		{
			if (value < SqlDateTime.MinTimeSpan || value > SqlDateTime.MaxTimeSpan)
			{
				throw new SqlTypeException(SQLResource.DateTimeOverflowMessage);
			}
			int num = value.Days;
			long num2 = value.Ticks - (long)num * 864000000000L;
			if (num2 < 0L)
			{
				num--;
				num2 += 864000000000L;
			}
			int num3 = (int)((double)num2 / 10000.0 * SqlDateTime.SQLTicksPerMillisecond + 0.5);
			if (num3 > SqlDateTime.MaxTime)
			{
				num3 = 0;
				num++;
			}
			return new SqlDateTime(num, num3);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x002A5618 File Offset: 0x002A4A18
		private static SqlDateTime FromDateTime(DateTime value)
		{
			if (value == DateTime.MaxValue)
			{
				return SqlDateTime.MaxValue;
			}
			return SqlDateTime.FromTimeSpan(value.Subtract(SqlDateTime.SQLBaseDate));
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06002C61 RID: 11361 RVA: 0x002A564C File Offset: 0x002A4A4C
		public DateTime Value
		{
			get
			{
				if (this.m_fNotNull)
				{
					return SqlDateTime.ToDateTime(this);
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x002A5674 File Offset: 0x002A4A74
		public int DayTicks
		{
			get
			{
				if (this.m_fNotNull)
				{
					return this.m_day;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06002C63 RID: 11363 RVA: 0x002A5698 File Offset: 0x002A4A98
		public int TimeTicks
		{
			get
			{
				if (this.m_fNotNull)
				{
					return this.m_time;
				}
				throw new SqlNullValueException();
			}
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x002A56BC File Offset: 0x002A4ABC
		public static implicit operator SqlDateTime(DateTime value)
		{
			return new SqlDateTime(value);
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x002A56D0 File Offset: 0x002A4AD0
		public static explicit operator DateTime(SqlDateTime x)
		{
			return SqlDateTime.ToDateTime(x);
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x002A56E4 File Offset: 0x002A4AE4
		public override string ToString()
		{
			if (this.IsNull)
			{
				return SQLResource.NullString;
			}
			return SqlDateTime.ToDateTime(this).ToString(null);
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x002A5714 File Offset: 0x002A4B14
		public static SqlDateTime Parse(string s)
		{
			if (s == SQLResource.NullString)
			{
				return SqlDateTime.Null;
			}
			DateTime dateTime;
			try
			{
				dateTime = DateTime.Parse(s, CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)Thread.CurrentThread.CurrentCulture.GetFormat(typeof(DateTimeFormatInfo));
				dateTime = DateTime.ParseExact(s, SqlDateTime.x_DateTimeFormats, dateTimeFormatInfo, DateTimeStyles.AllowWhiteSpaces);
			}
			return new SqlDateTime(dateTime);
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x002A5798 File Offset: 0x002A4B98
		public static SqlDateTime operator +(SqlDateTime x, TimeSpan t)
		{
			if (!x.IsNull)
			{
				return SqlDateTime.FromDateTime(SqlDateTime.ToDateTime(x) + t);
			}
			return SqlDateTime.Null;
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x002A57C8 File Offset: 0x002A4BC8
		public static SqlDateTime operator -(SqlDateTime x, TimeSpan t)
		{
			if (!x.IsNull)
			{
				return SqlDateTime.FromDateTime(SqlDateTime.ToDateTime(x) - t);
			}
			return SqlDateTime.Null;
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x002A57F8 File Offset: 0x002A4BF8
		public static SqlDateTime Add(SqlDateTime x, TimeSpan t)
		{
			return x + t;
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x002A580C File Offset: 0x002A4C0C
		public static SqlDateTime Subtract(SqlDateTime x, TimeSpan t)
		{
			return x - t;
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x002A5820 File Offset: 0x002A4C20
		public static explicit operator SqlDateTime(SqlString x)
		{
			if (!x.IsNull)
			{
				return SqlDateTime.Parse(x.Value);
			}
			return SqlDateTime.Null;
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x002A5848 File Offset: 0x002A4C48
		private static bool IsLeapYear(int year)
		{
			return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x002A5870 File Offset: 0x002A4C70
		public static SqlBoolean operator ==(SqlDateTime x, SqlDateTime y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_day == y.m_day && x.m_time == y.m_time);
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x002A58C0 File Offset: 0x002A4CC0
		public static SqlBoolean operator !=(SqlDateTime x, SqlDateTime y)
		{
			return !(x == y);
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x002A58DC File Offset: 0x002A4CDC
		public static SqlBoolean operator <(SqlDateTime x, SqlDateTime y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_day < y.m_day || (x.m_day == y.m_day && x.m_time < y.m_time));
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x002A593C File Offset: 0x002A4D3C
		public static SqlBoolean operator >(SqlDateTime x, SqlDateTime y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_day > y.m_day || (x.m_day == y.m_day && x.m_time > y.m_time));
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x002A599C File Offset: 0x002A4D9C
		public static SqlBoolean operator <=(SqlDateTime x, SqlDateTime y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_day < y.m_day || (x.m_day == y.m_day && x.m_time <= y.m_time));
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x002A5A00 File Offset: 0x002A4E00
		public static SqlBoolean operator >=(SqlDateTime x, SqlDateTime y)
		{
			if (!x.IsNull && !y.IsNull)
			{
				return new SqlBoolean(x.m_day > y.m_day || (x.m_day == y.m_day && x.m_time >= y.m_time));
			}
			return SqlBoolean.Null;
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x002A5A64 File Offset: 0x002A4E64
		public static SqlBoolean Equals(SqlDateTime x, SqlDateTime y)
		{
			return x == y;
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x002A5A78 File Offset: 0x002A4E78
		public static SqlBoolean NotEquals(SqlDateTime x, SqlDateTime y)
		{
			return x != y;
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x002A5A8C File Offset: 0x002A4E8C
		public static SqlBoolean LessThan(SqlDateTime x, SqlDateTime y)
		{
			return x < y;
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x002A5AA0 File Offset: 0x002A4EA0
		public static SqlBoolean GreaterThan(SqlDateTime x, SqlDateTime y)
		{
			return x > y;
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x002A5AB4 File Offset: 0x002A4EB4
		public static SqlBoolean LessThanOrEqual(SqlDateTime x, SqlDateTime y)
		{
			return x <= y;
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x002A5AC8 File Offset: 0x002A4EC8
		public static SqlBoolean GreaterThanOrEqual(SqlDateTime x, SqlDateTime y)
		{
			return x >= y;
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x002A5ADC File Offset: 0x002A4EDC
		public SqlString ToSqlString()
		{
			return (SqlString)this;
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x002A5AF4 File Offset: 0x002A4EF4
		public int CompareTo(object value)
		{
			if (value is SqlDateTime)
			{
				SqlDateTime sqlDateTime = (SqlDateTime)value;
				return this.CompareTo(sqlDateTime);
			}
			throw ADP.WrongType(value.GetType(), typeof(SqlDateTime));
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x002A5B30 File Offset: 0x002A4F30
		public int CompareTo(SqlDateTime value)
		{
			if (this.IsNull)
			{
				if (!value.IsNull)
				{
					return -1;
				}
				return 0;
			}
			else
			{
				if (value.IsNull)
				{
					return 1;
				}
				if (this < value)
				{
					return -1;
				}
				if (this > value)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x002A5B88 File Offset: 0x002A4F88
		public override bool Equals(object value)
		{
			if (!(value is SqlDateTime))
			{
				return false;
			}
			SqlDateTime sqlDateTime = (SqlDateTime)value;
			if (sqlDateTime.IsNull || this.IsNull)
			{
				return sqlDateTime.IsNull && this.IsNull;
			}
			return (this == sqlDateTime).Value;
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x002A5BE0 File Offset: 0x002A4FE0
		public override int GetHashCode()
		{
			if (!this.IsNull)
			{
				return this.Value.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x002A5C0C File Offset: 0x002A500C
		XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x002A5C1C File Offset: 0x002A501C
		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			string attribute = reader.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance");
			if (attribute != null && XmlConvert.ToBoolean(attribute))
			{
				this.m_fNotNull = false;
				return;
			}
			DateTime dateTime = XmlConvert.ToDateTime(reader.ReadElementString(), XmlDateTimeSerializationMode.RoundtripKind);
			if (dateTime.Kind != DateTimeKind.Unspecified)
			{
				throw new SqlTypeException(SQLResource.TimeZoneSpecifiedMessage);
			}
			SqlDateTime sqlDateTime = SqlDateTime.FromDateTime(dateTime);
			this.m_day = sqlDateTime.DayTicks;
			this.m_time = sqlDateTime.TimeTicks;
			this.m_fNotNull = true;
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x002A5C98 File Offset: 0x002A5098
		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (this.IsNull)
			{
				writer.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
				return;
			}
			writer.WriteString(XmlConvert.ToString(this.Value, SqlDateTime.x_ISO8601_DateTimeFormat));
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x002A5CE0 File Offset: 0x002A50E0
		public static XmlQualifiedName GetXsdType(XmlSchemaSet schemaSet)
		{
			return new XmlQualifiedName("dateTime", "http://www.w3.org/2001/XMLSchema");
		}

		// Token: 0x04001C79 RID: 7289
		private const DateTimeStyles x_DateTimeStyle = DateTimeStyles.AllowWhiteSpaces;

		// Token: 0x04001C7A RID: 7290
		private bool m_fNotNull;

		// Token: 0x04001C7B RID: 7291
		private int m_day;

		// Token: 0x04001C7C RID: 7292
		private int m_time;

		// Token: 0x04001C7D RID: 7293
		private static readonly double SQLTicksPerMillisecond = 0.3;

		// Token: 0x04001C7E RID: 7294
		public static readonly int SQLTicksPerSecond = 300;

		// Token: 0x04001C7F RID: 7295
		public static readonly int SQLTicksPerMinute = SqlDateTime.SQLTicksPerSecond * 60;

		// Token: 0x04001C80 RID: 7296
		public static readonly int SQLTicksPerHour = SqlDateTime.SQLTicksPerMinute * 60;

		// Token: 0x04001C81 RID: 7297
		private static readonly int SQLTicksPerDay = SqlDateTime.SQLTicksPerHour * 24;

		// Token: 0x04001C82 RID: 7298
		private static readonly long TicksPerSecond = 10000000L;

		// Token: 0x04001C83 RID: 7299
		private static readonly DateTime SQLBaseDate = new DateTime(1900, 1, 1);

		// Token: 0x04001C84 RID: 7300
		private static readonly long SQLBaseDateTicks = SqlDateTime.SQLBaseDate.Ticks;

		// Token: 0x04001C85 RID: 7301
		private static readonly int MinYear = 1753;

		// Token: 0x04001C86 RID: 7302
		private static readonly int MaxYear = 9999;

		// Token: 0x04001C87 RID: 7303
		private static readonly int MinDay = -53690;

		// Token: 0x04001C88 RID: 7304
		private static readonly int MaxDay = 2958463;

		// Token: 0x04001C89 RID: 7305
		private static readonly int MinTime = 0;

		// Token: 0x04001C8A RID: 7306
		private static readonly int MaxTime = SqlDateTime.SQLTicksPerDay - 1;

		// Token: 0x04001C8B RID: 7307
		private static readonly int DayBase = 693595;

		// Token: 0x04001C8C RID: 7308
		private static readonly int[] DaysToMonth365 = new int[]
		{
			0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
			304, 334, 365
		};

		// Token: 0x04001C8D RID: 7309
		private static readonly int[] DaysToMonth366 = new int[]
		{
			0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
			305, 335, 366
		};

		// Token: 0x04001C8E RID: 7310
		private static readonly DateTime MinDateTime = new DateTime(1753, 1, 1);

		// Token: 0x04001C8F RID: 7311
		private static readonly DateTime MaxDateTime = DateTime.MaxValue;

		// Token: 0x04001C90 RID: 7312
		private static readonly TimeSpan MinTimeSpan = SqlDateTime.MinDateTime.Subtract(SqlDateTime.SQLBaseDate);

		// Token: 0x04001C91 RID: 7313
		private static readonly TimeSpan MaxTimeSpan = SqlDateTime.MaxDateTime.Subtract(SqlDateTime.SQLBaseDate);

		// Token: 0x04001C92 RID: 7314
		private static readonly string x_ISO8601_DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff";

		// Token: 0x04001C93 RID: 7315
		private static readonly string[] x_DateTimeFormats = new string[] { "MMM d yyyy hh:mm:ss:ffftt", "MMM d yyyy hh:mm:ss:fff", "d MMM yyyy hh:mm:ss:ffftt", "d MMM yyyy hh:mm:ss:fff", "hh:mm:ss:ffftt", "hh:mm:ss:fff", "yyMMdd", "yyyyMMdd" };

		// Token: 0x04001C94 RID: 7316
		public static readonly SqlDateTime MinValue = new SqlDateTime(SqlDateTime.MinDay, 0);

		// Token: 0x04001C95 RID: 7317
		public static readonly SqlDateTime MaxValue = new SqlDateTime(SqlDateTime.MaxDay, SqlDateTime.MaxTime);

		// Token: 0x04001C96 RID: 7318
		public static readonly SqlDateTime Null = new SqlDateTime(true);
	}
}

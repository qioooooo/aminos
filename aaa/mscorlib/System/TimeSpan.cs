using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
	// Token: 0x02000115 RID: 277
	[ComVisible(true)]
	[Serializable]
	public struct TimeSpan : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>
	{
		// Token: 0x0600102A RID: 4138 RVA: 0x0002DF2E File Offset: 0x0002CF2E
		public TimeSpan(long ticks)
		{
			this._ticks = ticks;
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0002DF37 File Offset: 0x0002CF37
		public TimeSpan(int hours, int minutes, int seconds)
		{
			this._ticks = TimeSpan.TimeToTicks(hours, minutes, seconds);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0002DF47 File Offset: 0x0002CF47
		public TimeSpan(int days, int hours, int minutes, int seconds)
		{
			this = new TimeSpan(days, hours, minutes, seconds, 0);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0002DF58 File Offset: 0x0002CF58
		public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
		{
			long num = ((long)days * 3600L * 24L + (long)hours * 3600L + (long)minutes * 60L + (long)seconds) * 1000L + (long)milliseconds;
			if (num > 922337203685477L || num < -922337203685477L)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			this._ticks = num * 10000L;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x0002DFCA File Offset: 0x0002CFCA
		public long Ticks
		{
			get
			{
				return this._ticks;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0002DFD2 File Offset: 0x0002CFD2
		public int Days
		{
			get
			{
				return (int)(this._ticks / 864000000000L);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0002DFE5 File Offset: 0x0002CFE5
		public int Hours
		{
			get
			{
				return (int)(this._ticks / 36000000000L % 24L);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0002DFFC File Offset: 0x0002CFFC
		public int Milliseconds
		{
			get
			{
				return (int)(this._ticks / 10000L % 1000L);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0002E013 File Offset: 0x0002D013
		public int Minutes
		{
			get
			{
				return (int)(this._ticks / 600000000L % 60L);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0002E027 File Offset: 0x0002D027
		public int Seconds
		{
			get
			{
				return (int)(this._ticks / 10000000L % 60L);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0002E03B File Offset: 0x0002D03B
		public double TotalDays
		{
			get
			{
				return (double)this._ticks * 1.1574074074074074E-12;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x0002E04E File Offset: 0x0002D04E
		public double TotalHours
		{
			get
			{
				return (double)this._ticks * 2.7777777777777777E-11;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0002E064 File Offset: 0x0002D064
		public double TotalMilliseconds
		{
			get
			{
				double num = (double)this._ticks * 0.0001;
				if (num > 922337203685477.0)
				{
					return 922337203685477.0;
				}
				if (num < -922337203685477.0)
				{
					return -922337203685477.0;
				}
				return num;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06001037 RID: 4151 RVA: 0x0002E0B0 File Offset: 0x0002D0B0
		public double TotalMinutes
		{
			get
			{
				return (double)this._ticks * 1.6666666666666667E-09;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x0002E0C3 File Offset: 0x0002D0C3
		public double TotalSeconds
		{
			get
			{
				return (double)this._ticks * 1E-07;
			}
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0002E0D8 File Offset: 0x0002D0D8
		public TimeSpan Add(TimeSpan ts)
		{
			long num = this._ticks + ts._ticks;
			if (this._ticks >> 63 == ts._ticks >> 63 && this._ticks >> 63 != num >> 63)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return new TimeSpan(num);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0002E12E File Offset: 0x0002D12E
		public static int Compare(TimeSpan t1, TimeSpan t2)
		{
			if (t1._ticks > t2._ticks)
			{
				return 1;
			}
			if (t1._ticks < t2._ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0002E158 File Offset: 0x0002D158
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is TimeSpan))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeTimeSpan"));
			}
			long ticks = ((TimeSpan)value)._ticks;
			if (this._ticks > ticks)
			{
				return 1;
			}
			if (this._ticks < ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0002E1A8 File Offset: 0x0002D1A8
		public int CompareTo(TimeSpan value)
		{
			long ticks = value._ticks;
			if (this._ticks > ticks)
			{
				return 1;
			}
			if (this._ticks < ticks)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0002E1D4 File Offset: 0x0002D1D4
		public static TimeSpan FromDays(double value)
		{
			return TimeSpan.Interval(value, 86400000);
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0002E1E4 File Offset: 0x0002D1E4
		public TimeSpan Duration()
		{
			if (this._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Duration"));
			}
			return new TimeSpan((this._ticks >= 0L) ? this._ticks : (-this._ticks));
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0002E231 File Offset: 0x0002D231
		public override bool Equals(object value)
		{
			return value is TimeSpan && this._ticks == ((TimeSpan)value)._ticks;
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0002E250 File Offset: 0x0002D250
		public bool Equals(TimeSpan obj)
		{
			return this._ticks == obj._ticks;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0002E261 File Offset: 0x0002D261
		public static bool Equals(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks == t2._ticks;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0002E273 File Offset: 0x0002D273
		public override int GetHashCode()
		{
			return (int)this._ticks ^ (int)(this._ticks >> 32);
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0002E287 File Offset: 0x0002D287
		public static TimeSpan FromHours(double value)
		{
			return TimeSpan.Interval(value, 3600000);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0002E294 File Offset: 0x0002D294
		private static TimeSpan Interval(double value, int scale)
		{
			if (double.IsNaN(value))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_CannotBeNaN"));
			}
			double num = value * (double)scale;
			double num2 = num + ((value >= 0.0) ? 0.5 : (-0.5));
			if (num2 > 922337203685477.0 || num2 < -922337203685477.0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return new TimeSpan((long)num2 * 10000L);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0002E317 File Offset: 0x0002D317
		public static TimeSpan FromMilliseconds(double value)
		{
			return TimeSpan.Interval(value, 1);
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0002E320 File Offset: 0x0002D320
		public static TimeSpan FromMinutes(double value)
		{
			return TimeSpan.Interval(value, 60000);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0002E32D File Offset: 0x0002D32D
		public TimeSpan Negate()
		{
			if (this._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return new TimeSpan(-this._ticks);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0002E360 File Offset: 0x0002D360
		public static TimeSpan Parse(string s)
		{
			return new TimeSpan(default(TimeSpan.StringParser).Parse(s));
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0002E384 File Offset: 0x0002D384
		public static bool TryParse(string s, out TimeSpan result)
		{
			long num;
			if (default(TimeSpan.StringParser).TryParse(s, out num))
			{
				result = new TimeSpan(num);
				return true;
			}
			result = TimeSpan.Zero;
			return false;
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0002E3C1 File Offset: 0x0002D3C1
		public static TimeSpan FromSeconds(double value)
		{
			return TimeSpan.Interval(value, 1000);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0002E3D0 File Offset: 0x0002D3D0
		public TimeSpan Subtract(TimeSpan ts)
		{
			long num = this._ticks - ts._ticks;
			if (this._ticks >> 63 != ts._ticks >> 63 && this._ticks >> 63 != num >> 63)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return new TimeSpan(num);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0002E426 File Offset: 0x0002D426
		public static TimeSpan FromTicks(long value)
		{
			return new TimeSpan(value);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0002E430 File Offset: 0x0002D430
		internal static long TimeToTicks(int hour, int minute, int second)
		{
			long num = (long)hour * 3600L + (long)minute * 60L + (long)second;
			if (num > 922337203685L || num < -922337203685L)
			{
				throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Overflow_TimeSpanTooLong"));
			}
			return num * 10000000L;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0002E482 File Offset: 0x0002D482
		private string IntToString(int n, int digits)
		{
			return ParseNumbers.IntToString(n, 10, digits, '0', 0);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0002E490 File Offset: 0x0002D490
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)(this._ticks / 864000000000L);
			long num2 = this._ticks % 864000000000L;
			if (this._ticks < 0L)
			{
				stringBuilder.Append("-");
				num = -num;
				num2 = -num2;
			}
			if (num != 0)
			{
				stringBuilder.Append(num);
				stringBuilder.Append(".");
			}
			stringBuilder.Append(this.IntToString((int)(num2 / 36000000000L % 24L), 2));
			stringBuilder.Append(":");
			stringBuilder.Append(this.IntToString((int)(num2 / 600000000L % 60L), 2));
			stringBuilder.Append(":");
			stringBuilder.Append(this.IntToString((int)(num2 / 10000000L % 60L), 2));
			int num3 = (int)(num2 % 10000000L);
			if (num3 != 0)
			{
				stringBuilder.Append(".");
				stringBuilder.Append(this.IntToString(num3, 7));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x0002E593 File Offset: 0x0002D593
		public static TimeSpan operator -(TimeSpan t)
		{
			if (t._ticks == TimeSpan.MinValue._ticks)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return new TimeSpan(-t._ticks);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0002E5C5 File Offset: 0x0002D5C5
		public static TimeSpan operator -(TimeSpan t1, TimeSpan t2)
		{
			return t1.Subtract(t2);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0002E5CF File Offset: 0x0002D5CF
		public static TimeSpan operator +(TimeSpan t)
		{
			return t;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0002E5D2 File Offset: 0x0002D5D2
		public static TimeSpan operator +(TimeSpan t1, TimeSpan t2)
		{
			return t1.Add(t2);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0002E5DC File Offset: 0x0002D5DC
		public static bool operator ==(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks == t2._ticks;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0002E5EE File Offset: 0x0002D5EE
		public static bool operator !=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks != t2._ticks;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0002E603 File Offset: 0x0002D603
		public static bool operator <(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks < t2._ticks;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0002E615 File Offset: 0x0002D615
		public static bool operator <=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks <= t2._ticks;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0002E62A File Offset: 0x0002D62A
		public static bool operator >(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks > t2._ticks;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0002E63C File Offset: 0x0002D63C
		public static bool operator >=(TimeSpan t1, TimeSpan t2)
		{
			return t1._ticks >= t2._ticks;
		}

		// Token: 0x04000533 RID: 1331
		public const long TicksPerMillisecond = 10000L;

		// Token: 0x04000534 RID: 1332
		private const double MillisecondsPerTick = 0.0001;

		// Token: 0x04000535 RID: 1333
		public const long TicksPerSecond = 10000000L;

		// Token: 0x04000536 RID: 1334
		private const double SecondsPerTick = 1E-07;

		// Token: 0x04000537 RID: 1335
		public const long TicksPerMinute = 600000000L;

		// Token: 0x04000538 RID: 1336
		private const double MinutesPerTick = 1.6666666666666667E-09;

		// Token: 0x04000539 RID: 1337
		public const long TicksPerHour = 36000000000L;

		// Token: 0x0400053A RID: 1338
		private const double HoursPerTick = 2.7777777777777777E-11;

		// Token: 0x0400053B RID: 1339
		public const long TicksPerDay = 864000000000L;

		// Token: 0x0400053C RID: 1340
		private const double DaysPerTick = 1.1574074074074074E-12;

		// Token: 0x0400053D RID: 1341
		private const int MillisPerSecond = 1000;

		// Token: 0x0400053E RID: 1342
		private const int MillisPerMinute = 60000;

		// Token: 0x0400053F RID: 1343
		private const int MillisPerHour = 3600000;

		// Token: 0x04000540 RID: 1344
		private const int MillisPerDay = 86400000;

		// Token: 0x04000541 RID: 1345
		private const long MaxSeconds = 922337203685L;

		// Token: 0x04000542 RID: 1346
		private const long MinSeconds = -922337203685L;

		// Token: 0x04000543 RID: 1347
		private const long MaxMilliSeconds = 922337203685477L;

		// Token: 0x04000544 RID: 1348
		private const long MinMilliSeconds = -922337203685477L;

		// Token: 0x04000545 RID: 1349
		public static readonly TimeSpan Zero = new TimeSpan(0L);

		// Token: 0x04000546 RID: 1350
		public static readonly TimeSpan MaxValue = new TimeSpan(long.MaxValue);

		// Token: 0x04000547 RID: 1351
		public static readonly TimeSpan MinValue = new TimeSpan(long.MinValue);

		// Token: 0x04000548 RID: 1352
		internal long _ticks;

		// Token: 0x02000116 RID: 278
		private struct StringParser
		{
			// Token: 0x0600105B RID: 4187 RVA: 0x0002E688 File Offset: 0x0002D688
			internal void NextChar()
			{
				if (this.pos < this.len)
				{
					this.pos++;
				}
				this.ch = ((this.pos < this.len) ? this.str[this.pos] : '\0');
			}

			// Token: 0x0600105C RID: 4188 RVA: 0x0002E6DC File Offset: 0x0002D6DC
			internal char NextNonDigit()
			{
				for (int i = this.pos; i < this.len; i++)
				{
					char c = this.str[i];
					if (c < '0' || c > '9')
					{
						return c;
					}
				}
				return '\0';
			}

			// Token: 0x0600105D RID: 4189 RVA: 0x0002E71C File Offset: 0x0002D71C
			internal long Parse(string s)
			{
				long num;
				if (this.TryParse(s, out num))
				{
					return num;
				}
				switch (this.error)
				{
				case TimeSpan.StringParser.ParseError.Format:
					throw new FormatException(Environment.GetResourceString("Format_InvalidString"));
				case TimeSpan.StringParser.ParseError.Overflow:
					throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanTooLong"));
				case TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds:
					throw new OverflowException(Environment.GetResourceString("Overflow_TimeSpanElementTooLarge"));
				case TimeSpan.StringParser.ParseError.ArgumentNull:
					throw new ArgumentNullException("s");
				default:
					return 0L;
				}
			}

			// Token: 0x0600105E RID: 4190 RVA: 0x0002E794 File Offset: 0x0002D794
			internal bool TryParse(string s, out long value)
			{
				value = 0L;
				if (s == null)
				{
					this.error = TimeSpan.StringParser.ParseError.ArgumentNull;
					return false;
				}
				this.str = s;
				this.len = s.Length;
				this.pos = -1;
				this.NextChar();
				this.SkipBlanks();
				bool flag = false;
				if (this.ch == '-')
				{
					flag = true;
					this.NextChar();
				}
				long num;
				if (this.NextNonDigit() == ':')
				{
					if (!this.ParseTime(out num))
					{
						return false;
					}
				}
				else
				{
					int num2;
					if (!this.ParseInt(10675199, out num2))
					{
						return false;
					}
					num = (long)num2 * 864000000000L;
					if (this.ch == '.')
					{
						this.NextChar();
						long num3;
						if (!this.ParseTime(out num3))
						{
							return false;
						}
						num += num3;
					}
				}
				if (flag)
				{
					num = -num;
					if (num > 0L)
					{
						this.error = TimeSpan.StringParser.ParseError.Overflow;
						return false;
					}
				}
				else if (num < 0L)
				{
					this.error = TimeSpan.StringParser.ParseError.Overflow;
					return false;
				}
				this.SkipBlanks();
				if (this.pos < this.len)
				{
					this.error = TimeSpan.StringParser.ParseError.Format;
					return false;
				}
				value = num;
				return true;
			}

			// Token: 0x0600105F RID: 4191 RVA: 0x0002E884 File Offset: 0x0002D884
			internal bool ParseInt(int max, out int i)
			{
				i = 0;
				int num = this.pos;
				while (this.ch >= '0' && this.ch <= '9')
				{
					if (((long)i & (long)((ulong)(-268435456))) != 0L)
					{
						this.error = TimeSpan.StringParser.ParseError.Overflow;
						return false;
					}
					i = i * 10 + (int)this.ch - 48;
					if (i < 0)
					{
						this.error = TimeSpan.StringParser.ParseError.Overflow;
						return false;
					}
					this.NextChar();
				}
				if (num == this.pos)
				{
					this.error = TimeSpan.StringParser.ParseError.Format;
					return false;
				}
				if (i > max)
				{
					this.error = TimeSpan.StringParser.ParseError.Overflow;
					return false;
				}
				return true;
			}

			// Token: 0x06001060 RID: 4192 RVA: 0x0002E910 File Offset: 0x0002D910
			internal bool ParseTime(out long time)
			{
				time = 0L;
				int num;
				if (!this.ParseInt(23, out num))
				{
					if (this.error == TimeSpan.StringParser.ParseError.Overflow)
					{
						this.error = TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds;
					}
					return false;
				}
				time = (long)num * 36000000000L;
				if (this.ch != ':')
				{
					this.error = TimeSpan.StringParser.ParseError.Format;
					return false;
				}
				this.NextChar();
				if (!this.ParseInt(59, out num))
				{
					if (this.error == TimeSpan.StringParser.ParseError.Overflow)
					{
						this.error = TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds;
					}
					return false;
				}
				time += (long)num * 600000000L;
				if (this.ch == ':')
				{
					this.NextChar();
					if (this.ch != '.')
					{
						if (!this.ParseInt(59, out num))
						{
							if (this.error == TimeSpan.StringParser.ParseError.Overflow)
							{
								this.error = TimeSpan.StringParser.ParseError.OverflowHoursMinutesSeconds;
							}
							return false;
						}
						time += (long)num * 10000000L;
					}
					if (this.ch == '.')
					{
						this.NextChar();
						int num2 = 10000000;
						while (num2 > 1 && this.ch >= '0' && this.ch <= '9')
						{
							num2 /= 10;
							time += (long)((int)(this.ch - '0') * num2);
							this.NextChar();
						}
					}
				}
				return true;
			}

			// Token: 0x06001061 RID: 4193 RVA: 0x0002EA28 File Offset: 0x0002DA28
			internal void SkipBlanks()
			{
				while (this.ch == ' ' || this.ch == '\t')
				{
					this.NextChar();
				}
			}

			// Token: 0x04000549 RID: 1353
			private string str;

			// Token: 0x0400054A RID: 1354
			private char ch;

			// Token: 0x0400054B RID: 1355
			private int pos;

			// Token: 0x0400054C RID: 1356
			private int len;

			// Token: 0x0400054D RID: 1357
			private TimeSpan.StringParser.ParseError error;

			// Token: 0x02000117 RID: 279
			private enum ParseError
			{
				// Token: 0x0400054F RID: 1359
				Format = 1,
				// Token: 0x04000550 RID: 1360
				Overflow,
				// Token: 0x04000551 RID: 1361
				OverflowHoursMinutesSeconds,
				// Token: 0x04000552 RID: 1362
				ArgumentNull
			}
		}
	}
}

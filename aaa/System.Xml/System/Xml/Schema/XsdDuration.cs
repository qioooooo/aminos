using System;
using System.Text;

namespace System.Xml.Schema
{
	// Token: 0x020002A5 RID: 677
	internal struct XsdDuration
	{
		// Token: 0x06002072 RID: 8306 RVA: 0x00094E9C File Offset: 0x00093E9C
		public XsdDuration(bool isNegative, int years, int months, int days, int hours, int minutes, int seconds, int nanoseconds)
		{
			if (years < 0)
			{
				throw new ArgumentOutOfRangeException("years");
			}
			if (months < 0)
			{
				throw new ArgumentOutOfRangeException("months");
			}
			if (days < 0)
			{
				throw new ArgumentOutOfRangeException("days");
			}
			if (hours < 0)
			{
				throw new ArgumentOutOfRangeException("hours");
			}
			if (minutes < 0)
			{
				throw new ArgumentOutOfRangeException("minutes");
			}
			if (seconds < 0)
			{
				throw new ArgumentOutOfRangeException("seconds");
			}
			if (nanoseconds < 0 || nanoseconds > 999999999)
			{
				throw new ArgumentOutOfRangeException("nanoseconds");
			}
			this.years = years;
			this.months = months;
			this.days = days;
			this.hours = hours;
			this.minutes = minutes;
			this.seconds = seconds;
			this.nanoseconds = (uint)nanoseconds;
			if (isNegative)
			{
				this.nanoseconds |= 2147483648U;
			}
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00094F6B File Offset: 0x00093F6B
		public XsdDuration(TimeSpan timeSpan)
		{
			this = new XsdDuration(timeSpan, XsdDuration.DurationType.Duration);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00094F78 File Offset: 0x00093F78
		internal XsdDuration(TimeSpan timeSpan, XsdDuration.DurationType durationType)
		{
			long ticks = timeSpan.Ticks;
			bool flag;
			ulong num;
			if (ticks < 0L)
			{
				flag = true;
				num = (ulong)(-(ulong)ticks);
			}
			else
			{
				flag = false;
				num = (ulong)ticks;
			}
			if (durationType == XsdDuration.DurationType.YearMonthDuration)
			{
				int num2 = (int)(num / 315360000000000UL);
				int num3 = (int)(num % 315360000000000UL / 25920000000000UL);
				if (num3 == 12)
				{
					num2++;
					num3 = 0;
				}
				this = new XsdDuration(flag, num2, num3, 0, 0, 0, 0, 0);
				return;
			}
			this.nanoseconds = (uint)(num % 10000000UL) * 100U;
			if (flag)
			{
				this.nanoseconds |= 2147483648U;
			}
			this.years = 0;
			this.months = 0;
			this.days = (int)(num / 864000000000UL);
			this.hours = (int)(num / 36000000000UL % 24UL);
			this.minutes = (int)(num / 600000000UL % 60UL);
			this.seconds = (int)(num / 10000000UL % 60UL);
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0009506B File Offset: 0x0009406B
		public XsdDuration(string s)
		{
			this = new XsdDuration(s, XsdDuration.DurationType.Duration);
		}

		// Token: 0x06002076 RID: 8310 RVA: 0x00095078 File Offset: 0x00094078
		public XsdDuration(string s, XsdDuration.DurationType durationType)
		{
			XsdDuration xsdDuration;
			Exception ex = XsdDuration.TryParse(s, durationType, out xsdDuration);
			if (ex != null)
			{
				throw ex;
			}
			this.years = xsdDuration.Years;
			this.months = xsdDuration.Months;
			this.days = xsdDuration.Days;
			this.hours = xsdDuration.Hours;
			this.minutes = xsdDuration.Minutes;
			this.seconds = xsdDuration.Seconds;
			this.nanoseconds = (uint)xsdDuration.Nanoseconds;
			if (xsdDuration.IsNegative)
			{
				this.nanoseconds |= 2147483648U;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x0009510A File Offset: 0x0009410A
		public bool IsNegative
		{
			get
			{
				return (this.nanoseconds & 2147483648U) != 0U;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x0009511E File Offset: 0x0009411E
		public int Years
		{
			get
			{
				return this.years;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00095126 File Offset: 0x00094126
		public int Months
		{
			get
			{
				return this.months;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x0009512E File Offset: 0x0009412E
		public int Days
		{
			get
			{
				return this.days;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x00095136 File Offset: 0x00094136
		public int Hours
		{
			get
			{
				return this.hours;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x0009513E File Offset: 0x0009413E
		public int Minutes
		{
			get
			{
				return this.minutes;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x0600207D RID: 8317 RVA: 0x00095146 File Offset: 0x00094146
		public int Seconds
		{
			get
			{
				return this.seconds;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x0009514E File Offset: 0x0009414E
		public int Nanoseconds
		{
			get
			{
				return (int)(this.nanoseconds & 2147483647U);
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x0009515C File Offset: 0x0009415C
		public int Microseconds
		{
			get
			{
				return this.Nanoseconds / 1000;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x0009516A File Offset: 0x0009416A
		public int Milliseconds
		{
			get
			{
				return this.Nanoseconds / 1000000;
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00095178 File Offset: 0x00094178
		public XsdDuration Normalize()
		{
			int num = this.Years;
			int num2 = this.Months;
			int num3 = this.Days;
			int num4 = this.Hours;
			int num5 = this.Minutes;
			int num6 = this.Seconds;
			checked
			{
				try
				{
					if (num2 >= 12)
					{
						num += num2 / 12;
						num2 %= 12;
					}
					if (num6 >= 60)
					{
						num5 += num6 / 60;
						num6 %= 60;
					}
					if (num5 >= 60)
					{
						num4 += num5 / 60;
						num5 %= 60;
					}
					if (num4 >= 24)
					{
						num3 += num4 / 24;
						num4 %= 24;
					}
				}
				catch (OverflowException)
				{
					throw new OverflowException(Res.GetString("XmlConvert_Overflow", new object[]
					{
						this.ToString(),
						"Duration"
					}));
				}
				return new XsdDuration(this.IsNegative, num, num2, num3, num4, num5, num6, this.Nanoseconds);
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00095260 File Offset: 0x00094260
		public TimeSpan ToTimeSpan()
		{
			return this.ToTimeSpan(XsdDuration.DurationType.Duration);
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x0009526C File Offset: 0x0009426C
		internal TimeSpan ToTimeSpan(XsdDuration.DurationType durationType)
		{
			TimeSpan timeSpan;
			Exception ex = this.TryToTimeSpan(durationType, out timeSpan);
			if (ex != null)
			{
				throw ex;
			}
			return timeSpan;
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00095289 File Offset: 0x00094289
		internal Exception TryToTimeSpan(out TimeSpan result)
		{
			return this.TryToTimeSpan(XsdDuration.DurationType.Duration, out result);
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00095294 File Offset: 0x00094294
		internal Exception TryToTimeSpan(XsdDuration.DurationType durationType, out TimeSpan result)
		{
			Exception ex = null;
			ulong num = 0UL;
			checked
			{
				try
				{
					if (durationType != XsdDuration.DurationType.DayTimeDuration)
					{
						num += ((ulong)this.years + (ulong)this.months / 12UL) * 365UL;
						num += (ulong)this.months % 12UL * 30UL;
					}
					if (durationType != XsdDuration.DurationType.YearMonthDuration)
					{
						num += (ulong)this.days;
						num *= 24UL;
						num += (ulong)this.hours;
						num *= 60UL;
						num += (ulong)this.minutes;
						num *= 60UL;
						num += (ulong)this.seconds;
						num *= 10000000UL;
						num += (ulong)this.Nanoseconds / 100UL;
					}
					else
					{
						num *= 864000000000UL;
					}
					if (this.IsNegative)
					{
						if (num == 9223372036854775808UL)
						{
							result = new TimeSpan(long.MinValue);
						}
						else
						{
							result = new TimeSpan(0L - (long)num);
						}
					}
					else
					{
						result = new TimeSpan((long)num);
					}
					return null;
				}
				catch (OverflowException)
				{
					result = TimeSpan.MinValue;
					ex = new OverflowException(Res.GetString("XmlConvert_Overflow", new object[] { durationType, "TimeSpan" }));
				}
				return ex;
			}
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x000953D4 File Offset: 0x000943D4
		public override string ToString()
		{
			return this.ToString(XsdDuration.DurationType.Duration);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x000953E0 File Offset: 0x000943E0
		internal string ToString(XsdDuration.DurationType durationType)
		{
			StringBuilder stringBuilder = new StringBuilder(20);
			if (this.IsNegative)
			{
				stringBuilder.Append('-');
			}
			stringBuilder.Append('P');
			if (durationType != XsdDuration.DurationType.DayTimeDuration)
			{
				if (this.years != 0)
				{
					stringBuilder.Append(XmlConvert.ToString(this.years));
					stringBuilder.Append('Y');
				}
				if (this.months != 0)
				{
					stringBuilder.Append(XmlConvert.ToString(this.months));
					stringBuilder.Append('M');
				}
			}
			if (durationType != XsdDuration.DurationType.YearMonthDuration)
			{
				if (this.days != 0)
				{
					stringBuilder.Append(XmlConvert.ToString(this.days));
					stringBuilder.Append('D');
				}
				if (this.hours != 0 || this.minutes != 0 || this.seconds != 0 || this.Nanoseconds != 0)
				{
					stringBuilder.Append('T');
					if (this.hours != 0)
					{
						stringBuilder.Append(XmlConvert.ToString(this.hours));
						stringBuilder.Append('H');
					}
					if (this.minutes != 0)
					{
						stringBuilder.Append(XmlConvert.ToString(this.minutes));
						stringBuilder.Append('M');
					}
					int num = this.Nanoseconds;
					if (this.seconds != 0 || num != 0)
					{
						stringBuilder.Append(XmlConvert.ToString(this.seconds));
						if (num != 0)
						{
							stringBuilder.Append('.');
							int length = stringBuilder.Length;
							stringBuilder.Length += 9;
							int num2 = stringBuilder.Length - 1;
							for (int i = num2; i >= length; i--)
							{
								int num3 = num % 10;
								stringBuilder[i] = (char)(num3 + 48);
								if (num2 == i && num3 == 0)
								{
									num2--;
								}
								num /= 10;
							}
							stringBuilder.Length = num2 + 1;
						}
						stringBuilder.Append('S');
					}
				}
				if (stringBuilder[stringBuilder.Length - 1] == 'P')
				{
					stringBuilder.Append("T0S");
				}
			}
			else if (stringBuilder[stringBuilder.Length - 1] == 'P')
			{
				stringBuilder.Append("0M");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000955D2 File Offset: 0x000945D2
		internal static Exception TryParse(string s, out XsdDuration result)
		{
			return XsdDuration.TryParse(s, XsdDuration.DurationType.Duration, out result);
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000955DC File Offset: 0x000945DC
		internal static Exception TryParse(string s, XsdDuration.DurationType durationType, out XsdDuration result)
		{
			XsdDuration.Parts parts = XsdDuration.Parts.HasNone;
			result = default(XsdDuration);
			s = s.Trim();
			int length = s.Length;
			int num = 0;
			int i = 0;
			if (num < length)
			{
				if (s[num] == '-')
				{
					num++;
					result.nanoseconds = 2147483648U;
				}
				else
				{
					result.nanoseconds = 0U;
				}
				if (num < length && s[num++] == 'P')
				{
					int num2;
					if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) == null)
					{
						if (num >= length)
						{
							goto IL_02D8;
						}
						if (s[num] == 'Y')
						{
							if (i == 0)
							{
								goto IL_02D8;
							}
							parts |= XsdDuration.Parts.HasYears;
							result.years = num2;
							if (++num == length)
							{
								goto IL_02BB;
							}
							if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) != null)
							{
								goto IL_0301;
							}
							if (num >= length)
							{
								goto IL_02D8;
							}
						}
						if (s[num] == 'M')
						{
							if (i == 0)
							{
								goto IL_02D8;
							}
							parts |= XsdDuration.Parts.HasMonths;
							result.months = num2;
							if (++num == length)
							{
								goto IL_02BB;
							}
							if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) != null)
							{
								goto IL_0301;
							}
							if (num >= length)
							{
								goto IL_02D8;
							}
						}
						if (s[num] == 'D')
						{
							if (i == 0)
							{
								goto IL_02D8;
							}
							parts |= XsdDuration.Parts.HasDays;
							result.days = num2;
							if (++num == length)
							{
								goto IL_02BB;
							}
							if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) != null)
							{
								goto IL_0301;
							}
							if (num >= length)
							{
								goto IL_02D8;
							}
						}
						if (s[num] == 'T')
						{
							if (i != 0)
							{
								goto IL_02D8;
							}
							num++;
							if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) != null)
							{
								goto IL_0301;
							}
							if (num >= length)
							{
								goto IL_02D8;
							}
							if (s[num] == 'H')
							{
								if (i == 0)
								{
									goto IL_02D8;
								}
								parts |= XsdDuration.Parts.HasHours;
								result.hours = num2;
								if (++num == length)
								{
									goto IL_02BB;
								}
								if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) != null)
								{
									goto IL_0301;
								}
								if (num >= length)
								{
									goto IL_02D8;
								}
							}
							if (s[num] == 'M')
							{
								if (i == 0)
								{
									goto IL_02D8;
								}
								parts |= XsdDuration.Parts.HasMinutes;
								result.minutes = num2;
								if (++num == length)
								{
									goto IL_02BB;
								}
								if (XsdDuration.TryParseDigits(s, ref num, false, out num2, out i) != null)
								{
									goto IL_0301;
								}
								if (num >= length)
								{
									goto IL_02D8;
								}
							}
							if (s[num] == '.')
							{
								num++;
								parts |= XsdDuration.Parts.HasSeconds;
								result.seconds = num2;
								if (XsdDuration.TryParseDigits(s, ref num, true, out num2, out i) != null)
								{
									goto IL_0301;
								}
								if (i == 0)
								{
									num2 = 0;
								}
								while (i > 9)
								{
									num2 /= 10;
									i--;
								}
								while (i < 9)
								{
									num2 *= 10;
									i++;
								}
								result.nanoseconds |= (uint)num2;
								if (num >= length || s[num] != 'S')
								{
									goto IL_02D8;
								}
								if (++num == length)
								{
									goto IL_02BB;
								}
							}
							else if (s[num] == 'S')
							{
								if (i == 0)
								{
									goto IL_02D8;
								}
								parts |= XsdDuration.Parts.HasSeconds;
								result.seconds = num2;
								if (++num == length)
								{
									goto IL_02BB;
								}
							}
						}
						if (i != 0 || num != length)
						{
							goto IL_02D8;
						}
						IL_02BB:
						if (parts != XsdDuration.Parts.HasNone)
						{
							if (durationType == XsdDuration.DurationType.DayTimeDuration)
							{
								if ((parts & (XsdDuration.Parts)3) != XsdDuration.Parts.HasNone)
								{
									goto IL_02D8;
								}
							}
							else if (durationType == XsdDuration.DurationType.YearMonthDuration && (parts & (XsdDuration.Parts)(-4)) != XsdDuration.Parts.HasNone)
							{
								goto IL_02D8;
							}
							return null;
						}
						goto IL_02D8;
					}
					IL_0301:
					return new OverflowException(Res.GetString("XmlConvert_Overflow", new object[] { s, durationType }));
				}
			}
			IL_02D8:
			return new FormatException(Res.GetString("XmlConvert_BadFormat", new object[] { s, durationType }));
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00095914 File Offset: 0x00094914
		private static string TryParseDigits(string s, ref int offset, bool eatDigits, out int result, out int numDigits)
		{
			int num = offset;
			int length = s.Length;
			result = 0;
			numDigits = 0;
			while (offset < length && s[offset] >= '0' && s[offset] <= '9')
			{
				int num2 = (int)(s[offset] - '0');
				if (result > (2147483647 - num2) / 10)
				{
					if (!eatDigits)
					{
						return "XmlConvert_Overflow";
					}
					numDigits = offset - num;
					while (offset < length && s[offset] >= '0' && s[offset] <= '9')
					{
						offset++;
					}
					return null;
				}
				else
				{
					result = result * 10 + num2;
					offset++;
				}
			}
			numDigits = offset - num;
			return null;
		}

		// Token: 0x040013AA RID: 5034
		private const uint NegativeBit = 2147483648U;

		// Token: 0x040013AB RID: 5035
		private int years;

		// Token: 0x040013AC RID: 5036
		private int months;

		// Token: 0x040013AD RID: 5037
		private int days;

		// Token: 0x040013AE RID: 5038
		private int hours;

		// Token: 0x040013AF RID: 5039
		private int minutes;

		// Token: 0x040013B0 RID: 5040
		private int seconds;

		// Token: 0x040013B1 RID: 5041
		private uint nanoseconds;

		// Token: 0x020002A6 RID: 678
		private enum Parts
		{
			// Token: 0x040013B3 RID: 5043
			HasNone,
			// Token: 0x040013B4 RID: 5044
			HasYears,
			// Token: 0x040013B5 RID: 5045
			HasMonths,
			// Token: 0x040013B6 RID: 5046
			HasDays = 4,
			// Token: 0x040013B7 RID: 5047
			HasHours = 8,
			// Token: 0x040013B8 RID: 5048
			HasMinutes = 16,
			// Token: 0x040013B9 RID: 5049
			HasSeconds = 32
		}

		// Token: 0x020002A7 RID: 679
		internal enum DurationType
		{
			// Token: 0x040013BB RID: 5051
			Duration,
			// Token: 0x040013BC RID: 5052
			YearMonthDuration,
			// Token: 0x040013BD RID: 5053
			DayTimeDuration
		}
	}
}

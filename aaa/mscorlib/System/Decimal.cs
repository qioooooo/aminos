using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000A1 RID: 161
	[ComVisible(true)]
	[Serializable]
	public struct Decimal : IFormattable, IComparable, IConvertible, IComparable<decimal>, IEquatable<decimal>
	{
		// Token: 0x0600097C RID: 2428 RVA: 0x0001CD34 File Offset: 0x0001BD34
		public Decimal(int value)
		{
			int num = value;
			if (num >= 0)
			{
				this.flags = 0;
			}
			else
			{
				this.flags = int.MinValue;
				num = -num;
			}
			this.lo = num;
			this.mid = 0;
			this.hi = 0;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001CD73 File Offset: 0x0001BD73
		[CLSCompliant(false)]
		public Decimal(uint value)
		{
			this.flags = 0;
			this.lo = (int)value;
			this.mid = 0;
			this.hi = 0;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0001CD94 File Offset: 0x0001BD94
		public Decimal(long value)
		{
			long num = value;
			if (num >= 0L)
			{
				this.flags = 0;
			}
			else
			{
				this.flags = int.MinValue;
				num = -num;
			}
			this.lo = (int)num;
			this.mid = (int)(num >> 32);
			this.hi = 0;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0001CDD9 File Offset: 0x0001BDD9
		[CLSCompliant(false)]
		public Decimal(ulong value)
		{
			this.flags = 0;
			this.lo = (int)value;
			this.mid = (int)(value >> 32);
			this.hi = 0;
		}

		// Token: 0x06000980 RID: 2432
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Decimal(float value);

		// Token: 0x06000981 RID: 2433
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Decimal(double value);

		// Token: 0x06000982 RID: 2434 RVA: 0x0001CDFC File Offset: 0x0001BDFC
		internal Decimal(Currency value)
		{
			decimal num = Currency.ToDecimal(value);
			this.lo = num.lo;
			this.mid = num.mid;
			this.hi = num.hi;
			this.flags = num.flags;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001CE44 File Offset: 0x0001BE44
		public static long ToOACurrency(decimal value)
		{
			return new Currency(value).ToOACurrency();
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0001CE5F File Offset: 0x0001BE5F
		public static decimal FromOACurrency(long cy)
		{
			return Currency.ToDecimal(Currency.FromOACurrency(cy));
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0001CE6C File Offset: 0x0001BE6C
		public Decimal(int[] bits)
		{
			if (bits == null)
			{
				throw new ArgumentNullException("bits");
			}
			if (bits.Length == 4)
			{
				int num = bits[3];
				if ((num & 2130771967) == 0 && (num & 16711680) <= 1835008)
				{
					this.lo = bits[0];
					this.mid = bits[1];
					this.hi = bits[2];
					this.flags = num;
					return;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_DecBitCtor"));
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0001CEDC File Offset: 0x0001BEDC
		public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
		{
			if (scale > 28)
			{
				throw new ArgumentOutOfRangeException("scale", Environment.GetResourceString("ArgumentOutOfRange_DecimalScale"));
			}
			this.lo = lo;
			this.mid = mid;
			this.hi = hi;
			this.flags = (int)scale << 16;
			if (isNegative)
			{
				this.flags |= int.MinValue;
			}
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0001CF3A File Offset: 0x0001BF3A
		private Decimal(int lo, int mid, int hi, int flags)
		{
			this.lo = lo;
			this.mid = mid;
			this.hi = hi;
			this.flags = flags;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0001CF59 File Offset: 0x0001BF59
		internal static decimal Abs(decimal d)
		{
			return new decimal(d.lo, d.mid, d.hi, d.flags & int.MaxValue);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0001CF84 File Offset: 0x0001BF84
		public static decimal Add(decimal d1, decimal d2)
		{
			decimal num = 0m;
			decimal.FCallAdd(ref num, d1, d2);
			return num;
		}

		// Token: 0x0600098A RID: 2442
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallAdd(ref decimal result, decimal d1, decimal d2);

		// Token: 0x0600098B RID: 2443 RVA: 0x0001CFA2 File Offset: 0x0001BFA2
		public static decimal Ceiling(decimal d)
		{
			return -decimal.Floor(-d);
		}

		// Token: 0x0600098C RID: 2444
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Compare(decimal d1, decimal d2);

		// Token: 0x0600098D RID: 2445 RVA: 0x0001CFB4 File Offset: 0x0001BFB4
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is decimal))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDecimal"));
			}
			return decimal.Compare(this, (decimal)value);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0001CFE4 File Offset: 0x0001BFE4
		public int CompareTo(decimal value)
		{
			return decimal.Compare(this, value);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0001CFF4 File Offset: 0x0001BFF4
		public static decimal Divide(decimal d1, decimal d2)
		{
			decimal num = 0m;
			decimal.FCallDivide(ref num, d1, d2);
			return num;
		}

		// Token: 0x06000990 RID: 2448
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallDivide(ref decimal result, decimal d1, decimal d2);

		// Token: 0x06000991 RID: 2449 RVA: 0x0001D012 File Offset: 0x0001C012
		public override bool Equals(object value)
		{
			return value is decimal && decimal.Compare(this, (decimal)value) == 0;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001D032 File Offset: 0x0001C032
		public bool Equals(decimal value)
		{
			return decimal.Compare(this, value) == 0;
		}

		// Token: 0x06000993 RID: 2451
		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern int GetHashCode();

		// Token: 0x06000994 RID: 2452 RVA: 0x0001D043 File Offset: 0x0001C043
		public static bool Equals(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) == 0;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001D050 File Offset: 0x0001C050
		public static decimal Floor(decimal d)
		{
			decimal num = 0m;
			decimal.FCallFloor(ref num, d);
			return num;
		}

		// Token: 0x06000996 RID: 2454
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallFloor(ref decimal result, decimal d);

		// Token: 0x06000997 RID: 2455 RVA: 0x0001D06D File Offset: 0x0001C06D
		public override string ToString()
		{
			return Number.FormatDecimal(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001D080 File Offset: 0x0001C080
		public string ToString(string format)
		{
			return Number.FormatDecimal(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001D093 File Offset: 0x0001C093
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatDecimal(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0001D0A7 File Offset: 0x0001C0A7
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatDecimal(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0001D0BB File Offset: 0x0001C0BB
		public static decimal Parse(string s)
		{
			return Number.ParseDecimal(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001D0CA File Offset: 0x0001C0CA
		public static decimal Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return Number.ParseDecimal(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001D0DE File Offset: 0x0001C0DE
		public static decimal Parse(string s, IFormatProvider provider)
		{
			return Number.ParseDecimal(s, NumberStyles.Number, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001D0EE File Offset: 0x0001C0EE
		public static decimal Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return Number.ParseDecimal(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001D103 File Offset: 0x0001C103
		public static bool TryParse(string s, out decimal result)
		{
			return Number.TryParseDecimal(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001D113 File Offset: 0x0001C113
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out decimal result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return Number.TryParseDecimal(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001D12C File Offset: 0x0001C12C
		public static int[] GetBits(decimal d)
		{
			return new int[] { d.lo, d.mid, d.hi, d.flags };
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001D16C File Offset: 0x0001C16C
		internal static void GetBytes(decimal d, byte[] buffer)
		{
			buffer[0] = (byte)d.lo;
			buffer[1] = (byte)(d.lo >> 8);
			buffer[2] = (byte)(d.lo >> 16);
			buffer[3] = (byte)(d.lo >> 24);
			buffer[4] = (byte)d.mid;
			buffer[5] = (byte)(d.mid >> 8);
			buffer[6] = (byte)(d.mid >> 16);
			buffer[7] = (byte)(d.mid >> 24);
			buffer[8] = (byte)d.hi;
			buffer[9] = (byte)(d.hi >> 8);
			buffer[10] = (byte)(d.hi >> 16);
			buffer[11] = (byte)(d.hi >> 24);
			buffer[12] = (byte)d.flags;
			buffer[13] = (byte)(d.flags >> 8);
			buffer[14] = (byte)(d.flags >> 16);
			buffer[15] = (byte)(d.flags >> 24);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001D250 File Offset: 0x0001C250
		internal static decimal ToDecimal(byte[] buffer)
		{
			int num = (int)buffer[0] | ((int)buffer[1] << 8) | ((int)buffer[2] << 16) | ((int)buffer[3] << 24);
			int num2 = (int)buffer[4] | ((int)buffer[5] << 8) | ((int)buffer[6] << 16) | ((int)buffer[7] << 24);
			int num3 = (int)buffer[8] | ((int)buffer[9] << 8) | ((int)buffer[10] << 16) | ((int)buffer[11] << 24);
			int num4 = (int)buffer[12] | ((int)buffer[13] << 8) | ((int)buffer[14] << 16) | ((int)buffer[15] << 24);
			return new decimal(num, num2, num3, num4);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001D2D0 File Offset: 0x0001C2D0
		private static void InternalAddUInt32RawUnchecked(ref decimal value, uint i)
		{
			uint num = (uint)value.lo;
			uint num2 = num + i;
			value.lo = (int)num2;
			if (num2 < num || num2 < i)
			{
				num = (uint)value.mid;
				num2 = num + 1U;
				value.mid = (int)num2;
				if (num2 < num || num2 < 1U)
				{
					value.hi++;
				}
			}
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001D320 File Offset: 0x0001C320
		private static uint InternalDivRemUInt32(ref decimal value, uint divisor)
		{
			uint num = 0U;
			if (value.hi != 0)
			{
				ulong num2 = (ulong)value.hi;
				value.hi = (int)((uint)(num2 / (ulong)divisor));
				num = (uint)(num2 % (ulong)divisor);
			}
			if (value.mid != 0 || num != 0U)
			{
				ulong num2 = ((ulong)num << 32) | (ulong)value.mid;
				value.mid = (int)((uint)(num2 / (ulong)divisor));
				num = (uint)(num2 % (ulong)divisor);
			}
			if (value.lo != 0 || num != 0U)
			{
				ulong num2 = ((ulong)num << 32) | (ulong)value.lo;
				value.lo = (int)((uint)(num2 / (ulong)divisor));
				num = (uint)(num2 % (ulong)divisor);
			}
			return num;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001D3A8 File Offset: 0x0001C3A8
		private static void InternalRoundFromZero(ref decimal d, int decimalCount)
		{
			int num = (d.flags & 16711680) >> 16;
			int num2 = num - decimalCount;
			if (num2 <= 0)
			{
				return;
			}
			uint num4;
			uint num5;
			do
			{
				int num3 = ((num2 > 9) ? 9 : num2);
				num4 = decimal.Powers10[num3];
				num5 = decimal.InternalDivRemUInt32(ref d, num4);
				num2 -= num3;
			}
			while (num2 > 0);
			if (num5 >= num4 >> 1)
			{
				decimal.InternalAddUInt32RawUnchecked(ref d, 1U);
			}
			d.flags = ((decimalCount << 16) & 16711680) | (d.flags & int.MinValue);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0001D41E File Offset: 0x0001C41E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static decimal Max(decimal d1, decimal d2)
		{
			if (decimal.Compare(d1, d2) < 0)
			{
				return d2;
			}
			return d1;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001D42D File Offset: 0x0001C42D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static decimal Min(decimal d1, decimal d2)
		{
			if (decimal.Compare(d1, d2) >= 0)
			{
				return d2;
			}
			return d1;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001D43C File Offset: 0x0001C43C
		public static decimal Remainder(decimal d1, decimal d2)
		{
			d2.flags = (d2.flags & int.MaxValue) | (d1.flags & int.MinValue);
			if (decimal.Abs(d1) < decimal.Abs(d2))
			{
				return d1;
			}
			d1 -= d2;
			if (d1 == 0m)
			{
				d1.flags = (d1.flags & int.MaxValue) | (d2.flags & int.MinValue);
			}
			decimal num = decimal.Truncate(d1 / d2);
			decimal num2 = num * d2;
			decimal num3 = d1 - num2;
			if ((d1.flags & -2147483648) != (num3.flags & -2147483648))
			{
				if (num3 == 0m)
				{
					num3.flags = (num3.flags & int.MaxValue) | (d1.flags & int.MinValue);
				}
				else
				{
					num3 += d2;
				}
			}
			return num3;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0001D530 File Offset: 0x0001C530
		public static decimal Multiply(decimal d1, decimal d2)
		{
			decimal num = 0m;
			decimal.FCallMultiply(ref num, d1, d2);
			return num;
		}

		// Token: 0x060009AB RID: 2475
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallMultiply(ref decimal result, decimal d1, decimal d2);

		// Token: 0x060009AC RID: 2476 RVA: 0x0001D54E File Offset: 0x0001C54E
		public static decimal Negate(decimal d)
		{
			return new decimal(d.lo, d.mid, d.hi, d.flags ^ int.MinValue);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0001D577 File Offset: 0x0001C577
		public static decimal Round(decimal d)
		{
			return decimal.Round(d, 0);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0001D580 File Offset: 0x0001C580
		public static decimal Round(decimal d, int decimals)
		{
			decimal num = 0m;
			decimal.FCallRound(ref num, d, decimals);
			return num;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0001D59E File Offset: 0x0001C59E
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return decimal.Round(d, 0, mode);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0001D5A8 File Offset: 0x0001C5A8
		public static decimal Round(decimal d, int decimals, MidpointRounding mode)
		{
			if (decimals < 0 || decimals > 28)
			{
				throw new ArgumentOutOfRangeException("decimals", Environment.GetResourceString("ArgumentOutOfRange_DecimalRound"));
			}
			if (mode < MidpointRounding.ToEven || mode > MidpointRounding.AwayFromZero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidEnumValue", new object[] { mode, "MidpointRounding" }), "mode");
			}
			decimal num = d;
			if (mode == MidpointRounding.ToEven)
			{
				decimal.FCallRound(ref num, d, decimals);
			}
			else
			{
				decimal.InternalRoundFromZero(ref num, decimals);
			}
			return num;
		}

		// Token: 0x060009B1 RID: 2481
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallRound(ref decimal result, decimal d, int decimals);

		// Token: 0x060009B2 RID: 2482 RVA: 0x0001D624 File Offset: 0x0001C624
		public static decimal Subtract(decimal d1, decimal d2)
		{
			decimal num = 0m;
			decimal.FCallSubtract(ref num, d1, d2);
			return num;
		}

		// Token: 0x060009B3 RID: 2483
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallSubtract(ref decimal result, decimal d1, decimal d2);

		// Token: 0x060009B4 RID: 2484 RVA: 0x0001D644 File Offset: 0x0001C644
		public static byte ToByte(decimal value)
		{
			uint num = decimal.ToUInt32(value);
			if (num < 0U || num > 255U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)num;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0001D678 File Offset: 0x0001C678
		[CLSCompliant(false)]
		public static sbyte ToSByte(decimal value)
		{
			int num = decimal.ToInt32(value);
			if (num < -128 || num > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)num;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0001D6A8 File Offset: 0x0001C6A8
		public static short ToInt16(decimal value)
		{
			int num = decimal.ToInt32(value);
			if (num < -32768 || num > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)num;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001D6E0 File Offset: 0x0001C6E0
		internal static Currency ToCurrency(decimal d)
		{
			Currency currency = default(Currency);
			decimal.FCallToCurrency(ref currency, d);
			return currency;
		}

		// Token: 0x060009B8 RID: 2488
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallToCurrency(ref Currency result, decimal d);

		// Token: 0x060009B9 RID: 2489
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double ToDouble(decimal d);

		// Token: 0x060009BA RID: 2490
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int FCallToInt32(decimal d);

		// Token: 0x060009BB RID: 2491 RVA: 0x0001D700 File Offset: 0x0001C700
		public static int ToInt32(decimal d)
		{
			if ((d.flags & 16711680) != 0)
			{
				d = decimal.Truncate(d);
			}
			if (d.hi == 0 && d.mid == 0)
			{
				int num = d.lo;
				if (d.flags >= 0)
				{
					if (num >= 0)
					{
						return num;
					}
				}
				else
				{
					num = -num;
					if (num <= 0)
					{
						return num;
					}
				}
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0001D768 File Offset: 0x0001C768
		public static long ToInt64(decimal d)
		{
			if ((d.flags & 16711680) != 0)
			{
				d = decimal.Truncate(d);
			}
			if (d.hi == 0)
			{
				long num = ((long)d.lo & (long)((ulong)(-1))) | ((long)d.mid << 32);
				if (d.flags >= 0)
				{
					if (num >= 0L)
					{
						return num;
					}
				}
				else
				{
					num = -num;
					if (num <= 0L)
					{
						return num;
					}
				}
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0001D7D8 File Offset: 0x0001C7D8
		[CLSCompliant(false)]
		public static ushort ToUInt16(decimal value)
		{
			uint num = decimal.ToUInt32(value);
			if (num < 0U || num > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)num;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0001D80C File Offset: 0x0001C80C
		[CLSCompliant(false)]
		public static uint ToUInt32(decimal d)
		{
			if ((d.flags & 16711680) != 0)
			{
				d = decimal.Truncate(d);
			}
			if (d.hi == 0 && d.mid == 0)
			{
				uint num = (uint)d.lo;
				if (d.flags >= 0 || num == 0U)
				{
					return num;
				}
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0001D868 File Offset: 0x0001C868
		[CLSCompliant(false)]
		public static ulong ToUInt64(decimal d)
		{
			if ((d.flags & 16711680) != 0)
			{
				d = decimal.Truncate(d);
			}
			if (d.hi == 0)
			{
				ulong num = (ulong)d.lo | ((ulong)d.mid << 32);
				if (d.flags >= 0 || num == 0UL)
				{
					return num;
				}
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
		}

		// Token: 0x060009C0 RID: 2496
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float ToSingle(decimal d);

		// Token: 0x060009C1 RID: 2497 RVA: 0x0001D8CC File Offset: 0x0001C8CC
		public static decimal Truncate(decimal d)
		{
			decimal num = 0m;
			decimal.FCallTruncate(ref num, d);
			return num;
		}

		// Token: 0x060009C2 RID: 2498
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FCallTruncate(ref decimal result, decimal d);

		// Token: 0x060009C3 RID: 2499 RVA: 0x0001D8E9 File Offset: 0x0001C8E9
		public static implicit operator decimal(byte value)
		{
			return new decimal((int)value);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0001D8F1 File Offset: 0x0001C8F1
		[CLSCompliant(false)]
		public static implicit operator decimal(sbyte value)
		{
			return new decimal((int)value);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0001D8F9 File Offset: 0x0001C8F9
		public static implicit operator decimal(short value)
		{
			return new decimal((int)value);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0001D901 File Offset: 0x0001C901
		[CLSCompliant(false)]
		public static implicit operator decimal(ushort value)
		{
			return new decimal((int)value);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0001D909 File Offset: 0x0001C909
		public static implicit operator decimal(char value)
		{
			return new decimal((int)value);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0001D911 File Offset: 0x0001C911
		public static implicit operator decimal(int value)
		{
			return new decimal(value);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0001D919 File Offset: 0x0001C919
		[CLSCompliant(false)]
		public static implicit operator decimal(uint value)
		{
			return new decimal(value);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0001D921 File Offset: 0x0001C921
		public static implicit operator decimal(long value)
		{
			return new decimal(value);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0001D929 File Offset: 0x0001C929
		[CLSCompliant(false)]
		public static implicit operator decimal(ulong value)
		{
			return new decimal(value);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0001D931 File Offset: 0x0001C931
		public static explicit operator decimal(float value)
		{
			return new decimal(value);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0001D939 File Offset: 0x0001C939
		public static explicit operator decimal(double value)
		{
			return new decimal(value);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0001D941 File Offset: 0x0001C941
		public static explicit operator byte(decimal value)
		{
			return decimal.ToByte(value);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0001D949 File Offset: 0x0001C949
		[CLSCompliant(false)]
		public static explicit operator sbyte(decimal value)
		{
			return decimal.ToSByte(value);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0001D951 File Offset: 0x0001C951
		public static explicit operator char(decimal value)
		{
			return (char)decimal.ToUInt16(value);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0001D959 File Offset: 0x0001C959
		public static explicit operator short(decimal value)
		{
			return decimal.ToInt16(value);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0001D961 File Offset: 0x0001C961
		[CLSCompliant(false)]
		public static explicit operator ushort(decimal value)
		{
			return decimal.ToUInt16(value);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0001D969 File Offset: 0x0001C969
		public static explicit operator int(decimal value)
		{
			return decimal.ToInt32(value);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0001D971 File Offset: 0x0001C971
		[CLSCompliant(false)]
		public static explicit operator uint(decimal value)
		{
			return decimal.ToUInt32(value);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0001D979 File Offset: 0x0001C979
		public static explicit operator long(decimal value)
		{
			return decimal.ToInt64(value);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0001D981 File Offset: 0x0001C981
		[CLSCompliant(false)]
		public static explicit operator ulong(decimal value)
		{
			return decimal.ToUInt64(value);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001D989 File Offset: 0x0001C989
		public static explicit operator float(decimal value)
		{
			return decimal.ToSingle(value);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0001D991 File Offset: 0x0001C991
		public static explicit operator double(decimal value)
		{
			return decimal.ToDouble(value);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0001D999 File Offset: 0x0001C999
		public static decimal operator +(decimal d)
		{
			return d;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0001D99C File Offset: 0x0001C99C
		public static decimal operator -(decimal d)
		{
			return decimal.Negate(d);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001D9A4 File Offset: 0x0001C9A4
		public static decimal operator ++(decimal d)
		{
			return decimal.Add(d, 1m);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0001D9B2 File Offset: 0x0001C9B2
		public static decimal operator --(decimal d)
		{
			return decimal.Subtract(d, 1m);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001D9C0 File Offset: 0x0001C9C0
		public static decimal operator +(decimal d1, decimal d2)
		{
			return decimal.Add(d1, d2);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001D9C9 File Offset: 0x0001C9C9
		public static decimal operator -(decimal d1, decimal d2)
		{
			return decimal.Subtract(d1, d2);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001D9D2 File Offset: 0x0001C9D2
		public static decimal operator *(decimal d1, decimal d2)
		{
			return decimal.Multiply(d1, d2);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001D9DB File Offset: 0x0001C9DB
		public static decimal operator /(decimal d1, decimal d2)
		{
			return decimal.Divide(d1, d2);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001D9E4 File Offset: 0x0001C9E4
		public static decimal operator %(decimal d1, decimal d2)
		{
			return decimal.Remainder(d1, d2);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0001D9ED File Offset: 0x0001C9ED
		public static bool operator ==(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) == 0;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001D9F9 File Offset: 0x0001C9F9
		public static bool operator !=(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) != 0;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001DA08 File Offset: 0x0001CA08
		public static bool operator <(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) < 0;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0001DA14 File Offset: 0x0001CA14
		public static bool operator <=(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) <= 0;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0001DA23 File Offset: 0x0001CA23
		public static bool operator >(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) > 0;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0001DA2F File Offset: 0x0001CA2F
		public static bool operator >=(decimal d1, decimal d2)
		{
			return decimal.Compare(d1, d2) >= 0;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0001DA3E File Offset: 0x0001CA3E
		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0001DA42 File Offset: 0x0001CA42
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0001DA50 File Offset: 0x0001CA50
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Decimal", "Char" }));
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0001DA8E File Offset: 0x0001CA8E
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0001DA9B File Offset: 0x0001CA9B
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0001DAA8 File Offset: 0x0001CAA8
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0001DAB5 File Offset: 0x0001CAB5
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0001DAC2 File Offset: 0x0001CAC2
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0001DACF File Offset: 0x0001CACF
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0001DADC File Offset: 0x0001CADC
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0001DAE9 File Offset: 0x0001CAE9
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0001DAF6 File Offset: 0x0001CAF6
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001DB03 File Offset: 0x0001CB03
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0001DB10 File Offset: 0x0001CB10
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0001DB18 File Offset: 0x0001CB18
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Decimal", "DateTime" }));
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001DB56 File Offset: 0x0001CB56
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400037E RID: 894
		private const int SignMask = -2147483648;

		// Token: 0x0400037F RID: 895
		private const int ScaleMask = 16711680;

		// Token: 0x04000380 RID: 896
		private const int ScaleShift = 16;

		// Token: 0x04000381 RID: 897
		private const int MaxInt32Scale = 9;

		// Token: 0x04000382 RID: 898
		public const decimal Zero = 0m;

		// Token: 0x04000383 RID: 899
		public const decimal One = 1m;

		// Token: 0x04000384 RID: 900
		public const decimal MinusOne = -1m;

		// Token: 0x04000385 RID: 901
		public const decimal MaxValue = 79228162514264337593543950335m;

		// Token: 0x04000386 RID: 902
		public const decimal MinValue = -79228162514264337593543950335m;

		// Token: 0x04000387 RID: 903
		private static uint[] Powers10 = new uint[] { 1U, 10U, 100U, 1000U, 10000U, 100000U, 1000000U, 10000000U, 100000000U, 1000000000U };

		// Token: 0x04000388 RID: 904
		private int flags;

		// Token: 0x04000389 RID: 905
		private int hi;

		// Token: 0x0400038A RID: 906
		private int lo;

		// Token: 0x0400038B RID: 907
		private int mid;
	}
}

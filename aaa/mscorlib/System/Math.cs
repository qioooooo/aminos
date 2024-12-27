using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace System
{
	// Token: 0x020000D0 RID: 208
	public static class Math
	{
		// Token: 0x06000BA5 RID: 2981
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Acos(double d);

		// Token: 0x06000BA6 RID: 2982
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Asin(double d);

		// Token: 0x06000BA7 RID: 2983
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan(double d);

		// Token: 0x06000BA8 RID: 2984
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Atan2(double y, double x);

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002370B File Offset: 0x0002270B
		public static decimal Ceiling(decimal d)
		{
			return decimal.Ceiling(d);
		}

		// Token: 0x06000BAA RID: 2986
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Ceiling(double a);

		// Token: 0x06000BAB RID: 2987
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cos(double d);

		// Token: 0x06000BAC RID: 2988
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Cosh(double value);

		// Token: 0x06000BAD RID: 2989 RVA: 0x00023713 File Offset: 0x00022713
		public static decimal Floor(decimal d)
		{
			return decimal.Floor(d);
		}

		// Token: 0x06000BAE RID: 2990
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Floor(double d);

		// Token: 0x06000BAF RID: 2991 RVA: 0x0002371C File Offset: 0x0002271C
		private unsafe static double InternalRound(double value, int digits, MidpointRounding mode)
		{
			if (Math.Abs(value) < Math.doubleRoundLimit)
			{
				double num = Math.roundPower10Double[digits];
				value *= num;
				if (mode == MidpointRounding.AwayFromZero)
				{
					double num2 = Math.SplitFractionDouble(&value);
					if (Math.Abs(num2) >= 0.5)
					{
						value += (double)Math.Sign(num2);
					}
				}
				else
				{
					value = Math.Round(value);
				}
				value /= num;
			}
			return value;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0002377C File Offset: 0x0002277C
		private unsafe static double InternalTruncate(double d)
		{
			Math.SplitFractionDouble(&d);
			return d;
		}

		// Token: 0x06000BB1 RID: 2993
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sin(double a);

		// Token: 0x06000BB2 RID: 2994
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tan(double a);

		// Token: 0x06000BB3 RID: 2995
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sinh(double value);

		// Token: 0x06000BB4 RID: 2996
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Tanh(double value);

		// Token: 0x06000BB5 RID: 2997
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Round(double a);

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00023788 File Offset: 0x00022788
		public static double Round(double value, int digits)
		{
			if (digits < 0 || digits > 15)
			{
				throw new ArgumentOutOfRangeException("digits", Environment.GetResourceString("ArgumentOutOfRange_RoundingDigits"));
			}
			return Math.InternalRound(value, digits, MidpointRounding.ToEven);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x000237B0 File Offset: 0x000227B0
		public static double Round(double value, MidpointRounding mode)
		{
			return Math.Round(value, 0, mode);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000237BC File Offset: 0x000227BC
		public static double Round(double value, int digits, MidpointRounding mode)
		{
			if (digits < 0 || digits > 15)
			{
				throw new ArgumentOutOfRangeException("digits", Environment.GetResourceString("ArgumentOutOfRange_RoundingDigits"));
			}
			if (mode < MidpointRounding.ToEven || mode > MidpointRounding.AwayFromZero)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidEnumValue", new object[] { mode, "MidpointRounding" }), "mode");
			}
			return Math.InternalRound(value, digits, mode);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00023825 File Offset: 0x00022825
		public static decimal Round(decimal d)
		{
			return decimal.Round(d, 0);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002382E File Offset: 0x0002282E
		public static decimal Round(decimal d, int decimals)
		{
			return decimal.Round(d, decimals);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00023837 File Offset: 0x00022837
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return decimal.Round(d, 0, mode);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00023841 File Offset: 0x00022841
		public static decimal Round(decimal d, int decimals, MidpointRounding mode)
		{
			return decimal.Round(d, decimals, mode);
		}

		// Token: 0x06000BBD RID: 3005
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern double SplitFractionDouble(double* value);

		// Token: 0x06000BBE RID: 3006 RVA: 0x0002384B File Offset: 0x0002284B
		public static decimal Truncate(decimal d)
		{
			return decimal.Truncate(d);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00023853 File Offset: 0x00022853
		public static double Truncate(double d)
		{
			return Math.InternalTruncate(d);
		}

		// Token: 0x06000BC0 RID: 3008
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Sqrt(double d);

		// Token: 0x06000BC1 RID: 3009
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log(double d);

		// Token: 0x06000BC2 RID: 3010
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Log10(double d);

		// Token: 0x06000BC3 RID: 3011
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Exp(double d);

		// Token: 0x06000BC4 RID: 3012
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Pow(double x, double y);

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002385C File Offset: 0x0002285C
		public static double IEEERemainder(double x, double y)
		{
			double num = x % y;
			if (double.IsNaN(num))
			{
				return double.NaN;
			}
			if (num == 0.0 && double.IsNegative(x))
			{
				return double.NegativeZero;
			}
			double num2 = num - Math.Abs(y) * (double)Math.Sign(x);
			if (Math.Abs(num2) == Math.Abs(num))
			{
				double num3 = x / y;
				double num4 = Math.Round(num3);
				if (Math.Abs(num4) > Math.Abs(num3))
				{
					return num2;
				}
				return num;
			}
			else
			{
				if (Math.Abs(num2) < Math.Abs(num))
				{
					return num2;
				}
				return num;
			}
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000238E6 File Offset: 0x000228E6
		[CLSCompliant(false)]
		public static sbyte Abs(sbyte value)
		{
			if (value >= 0)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000238F4 File Offset: 0x000228F4
		private static sbyte AbsHelper(sbyte value)
		{
			if (value == -128)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002390E File Offset: 0x0002290E
		public static short Abs(short value)
		{
			if (value >= 0)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002391C File Offset: 0x0002291C
		private static short AbsHelper(short value)
		{
			if (value == -32768)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00023939 File Offset: 0x00022939
		public static int Abs(int value)
		{
			if (value >= 0)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00023947 File Offset: 0x00022947
		private static int AbsHelper(int value)
		{
			if (value == -2147483648)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00023963 File Offset: 0x00022963
		public static long Abs(long value)
		{
			if (value >= 0L)
			{
				return value;
			}
			return Math.AbsHelper(value);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00023972 File Offset: 0x00022972
		private static long AbsHelper(long value)
		{
			if (value == -9223372036854775808L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
			}
			return -value;
		}

		// Token: 0x06000BCE RID: 3022
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float Abs(float value);

		// Token: 0x06000BCF RID: 3023
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double Abs(double value);

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00023992 File Offset: 0x00022992
		public static decimal Abs(decimal value)
		{
			return decimal.Abs(value);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0002399A File Offset: 0x0002299A
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static sbyte Max(sbyte val1, sbyte val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000239A3 File Offset: 0x000229A3
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static byte Max(byte val1, byte val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x000239AC File Offset: 0x000229AC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static short Max(short val1, short val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000239B5 File Offset: 0x000229B5
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static ushort Max(ushort val1, ushort val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000239BE File Offset: 0x000229BE
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Max(int val1, int val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x000239C7 File Offset: 0x000229C7
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static uint Max(uint val1, uint val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x000239D0 File Offset: 0x000229D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static long Max(long val1, long val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000239D9 File Offset: 0x000229D9
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static ulong Max(ulong val1, ulong val2)
		{
			if (val1 < val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x000239E2 File Offset: 0x000229E2
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static float Max(float val1, float val2)
		{
			if (val1 > val2)
			{
				return val1;
			}
			if (float.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000239F5 File Offset: 0x000229F5
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static double Max(double val1, double val2)
		{
			if (val1 > val2)
			{
				return val1;
			}
			if (double.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00023A08 File Offset: 0x00022A08
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static decimal Max(decimal val1, decimal val2)
		{
			return decimal.Max(val1, val2);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00023A11 File Offset: 0x00022A11
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static sbyte Min(sbyte val1, sbyte val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00023A1A File Offset: 0x00022A1A
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static byte Min(byte val1, byte val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00023A23 File Offset: 0x00022A23
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static short Min(short val1, short val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00023A2C File Offset: 0x00022A2C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static ushort Min(ushort val1, ushort val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00023A35 File Offset: 0x00022A35
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static int Min(int val1, int val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00023A3E File Offset: 0x00022A3E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public static uint Min(uint val1, uint val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00023A47 File Offset: 0x00022A47
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static long Min(long val1, long val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00023A50 File Offset: 0x00022A50
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static ulong Min(ulong val1, ulong val2)
		{
			if (val1 > val2)
			{
				return val2;
			}
			return val1;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00023A59 File Offset: 0x00022A59
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static float Min(float val1, float val2)
		{
			if (val1 < val2)
			{
				return val1;
			}
			if (float.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00023A6C File Offset: 0x00022A6C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static double Min(double val1, double val2)
		{
			if (val1 < val2)
			{
				return val1;
			}
			if (double.IsNaN(val1))
			{
				return val1;
			}
			return val2;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00023A7F File Offset: 0x00022A7F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static decimal Min(decimal val1, decimal val2)
		{
			return decimal.Min(val1, val2);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00023A88 File Offset: 0x00022A88
		public static double Log(double a, double newBase)
		{
			if (newBase == 1.0)
			{
				return double.NaN;
			}
			if (a != 1.0 && (newBase == 0.0 || double.IsPositiveInfinity(newBase)))
			{
				return double.NaN;
			}
			return Math.Log(a) / Math.Log(newBase);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00023AE2 File Offset: 0x00022AE2
		[CLSCompliant(false)]
		public static int Sign(sbyte value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00023AF1 File Offset: 0x00022AF1
		public static int Sign(short value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00023B00 File Offset: 0x00022B00
		public static int Sign(int value)
		{
			if (value < 0)
			{
				return -1;
			}
			if (value > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00023B0F File Offset: 0x00022B0F
		public static int Sign(long value)
		{
			if (value < 0L)
			{
				return -1;
			}
			if (value > 0L)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00023B20 File Offset: 0x00022B20
		public static int Sign(float value)
		{
			if (value < 0f)
			{
				return -1;
			}
			if (value > 0f)
			{
				return 1;
			}
			if (value == 0f)
			{
				return 0;
			}
			throw new ArithmeticException(Environment.GetResourceString("Arithmetic_NaN"));
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00023B4F File Offset: 0x00022B4F
		public static int Sign(double value)
		{
			if (value < 0.0)
			{
				return -1;
			}
			if (value > 0.0)
			{
				return 1;
			}
			if (value == 0.0)
			{
				return 0;
			}
			throw new ArithmeticException(Environment.GetResourceString("Arithmetic_NaN"));
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00023B8A File Offset: 0x00022B8A
		public static int Sign(decimal value)
		{
			if (value < 0m)
			{
				return -1;
			}
			if (value > 0m)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00023BAD File Offset: 0x00022BAD
		public static long BigMul(int a, int b)
		{
			return (long)a * (long)b;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00023BB4 File Offset: 0x00022BB4
		public static int DivRem(int a, int b, out int result)
		{
			result = a % b;
			return a / b;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00023BBE File Offset: 0x00022BBE
		public static long DivRem(long a, long b, out long result)
		{
			result = a % b;
			return a / b;
		}

		// Token: 0x0400040E RID: 1038
		private const int maxRoundingDigits = 15;

		// Token: 0x0400040F RID: 1039
		public const double PI = 3.141592653589793;

		// Token: 0x04000410 RID: 1040
		public const double E = 2.718281828459045;

		// Token: 0x04000411 RID: 1041
		private static double doubleRoundLimit = 10000000000000000.0;

		// Token: 0x04000412 RID: 1042
		private static double[] roundPower10Double = new double[]
		{
			1.0, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, 10000000.0, 100000000.0, 1000000000.0,
			10000000000.0, 100000000000.0, 1000000000000.0, 10000000000000.0, 100000000000000.0, 1000000000000000.0
		};
	}
}

using System;

namespace Microsoft.JScript
{
	// Token: 0x020000D2 RID: 210
	public class MathObject : JSObject
	{
		// Token: 0x06000987 RID: 2439 RVA: 0x000499EE File Offset: 0x000489EE
		internal MathObject(ScriptObject parent)
			: base(parent)
		{
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x000499F7 File Offset: 0x000489F7
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_abs)]
		public static double abs(double d)
		{
			if (d < 0.0)
			{
				return -d;
			}
			if (d > 0.0)
			{
				return d;
			}
			if (d == d)
			{
				return 0.0;
			}
			return d;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00049A25 File Offset: 0x00048A25
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_acos)]
		public static double acos(double x)
		{
			return Math.Acos(x);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00049A2D File Offset: 0x00048A2D
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_asin)]
		public static double asin(double x)
		{
			return Math.Asin(x);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00049A35 File Offset: 0x00048A35
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_atan)]
		public static double atan(double x)
		{
			return Math.Atan(x);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00049A3D File Offset: 0x00048A3D
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_atan2)]
		public static double atan2(double dy, double dx)
		{
			return Math.Atan2(dy, dx);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00049A46 File Offset: 0x00048A46
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_ceil)]
		public static double ceil(double x)
		{
			return Math.Ceiling(x);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00049A50 File Offset: 0x00048A50
		private static double Compare(double x, double y)
		{
			if (x != 0.0 || y != 0.0)
			{
				if (x == y)
				{
					return 0.0;
				}
				return x - y;
			}
			else
			{
				double num = 1.0 / x;
				double num2 = 1.0 / y;
				if (num < 0.0)
				{
					return (double)((num2 < 0.0) ? 0 : (-1));
				}
				if (num2 < 0.0)
				{
					return 1.0;
				}
				return 0.0;
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00049ADC File Offset: 0x00048ADC
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_cos)]
		public static double cos(double x)
		{
			return Math.Cos(x);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00049AE4 File Offset: 0x00048AE4
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_exp)]
		public static double exp(double x)
		{
			return Math.Exp(x);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00049AEC File Offset: 0x00048AEC
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_floor)]
		public static double floor(double x)
		{
			return Math.Floor(x);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00049AF4 File Offset: 0x00048AF4
		internal override string GetClassName()
		{
			return "Math";
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00049AFB File Offset: 0x00048AFB
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_log)]
		public static double log(double x)
		{
			return Math.Log(x);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00049B04 File Offset: 0x00048B04
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Math_max)]
		public static double max(object x, object y, params object[] args)
		{
			if (x is Missing)
			{
				return double.NegativeInfinity;
			}
			double num = Convert.ToNumber(x);
			if (y is Missing)
			{
				return num;
			}
			double num2 = Convert.ToNumber(y);
			double num3 = MathObject.Compare(num, num2);
			if (num3 != num3)
			{
				return num3;
			}
			double num4 = num;
			if (num3 < 0.0)
			{
				num4 = num2;
			}
			if (args.Length == 0)
			{
				return num4;
			}
			return MathObject.maxv(num4, args, 0);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00049B68 File Offset: 0x00048B68
		private static double maxv(double lhMax, object[] args, int start)
		{
			if (args.Length == start)
			{
				return lhMax;
			}
			double num = Convert.ToNumber(args[start]);
			double num2 = MathObject.Compare(lhMax, num);
			if (num2 != num2)
			{
				return num2;
			}
			if (num2 > 0.0)
			{
				num = lhMax;
			}
			return MathObject.maxv(num, args, start + 1);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00049BAC File Offset: 0x00048BAC
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.Math_min)]
		public static double min(object x, object y, params object[] args)
		{
			if (x is Missing)
			{
				return double.PositiveInfinity;
			}
			double num = Convert.ToNumber(x);
			if (y is Missing)
			{
				return num;
			}
			double num2 = Convert.ToNumber(y);
			double num3 = MathObject.Compare(num, num2);
			if (num3 != num3)
			{
				return num3;
			}
			double num4 = num;
			if (num3 > 0.0)
			{
				num4 = num2;
			}
			if (args.Length == 0)
			{
				return num4;
			}
			return MathObject.minv(num4, args, 0);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00049C10 File Offset: 0x00048C10
		private static double minv(double lhMin, object[] args, int start)
		{
			if (args.Length == start)
			{
				return lhMin;
			}
			double num = Convert.ToNumber(args[start]);
			double num2 = MathObject.Compare(lhMin, num);
			if (num2 != num2)
			{
				return num2;
			}
			if (num2 < 0.0)
			{
				num = lhMin;
			}
			return MathObject.minv(num, args, start + 1);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00049C54 File Offset: 0x00048C54
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_pow)]
		public static double pow(double dx, double dy)
		{
			if (dy == 0.0)
			{
				return 1.0;
			}
			if ((dx == 1.0 || dx == -1.0) && (dy == double.PositiveInfinity || dy == double.NegativeInfinity))
			{
				return double.NaN;
			}
			if (double.IsNaN(dy))
			{
				return double.NaN;
			}
			if (dx == double.NegativeInfinity && dy < 0.0 && Math.IEEERemainder(-dy + 1.0, 2.0) == 0.0)
			{
				return -0.0;
			}
			double num;
			try
			{
				num = Math.Pow(dx, dy);
			}
			catch
			{
				if (dx != dx || dy != dy)
				{
					num = double.NaN;
				}
				else if (dx == 0.0 && dy < 0.0)
				{
					if ((double)((long)dy) == dy && (long)(-(long)dy) % 2L > 0L)
					{
						double num2 = 1.0 / dx;
						if (num2 < 0.0)
						{
							return double.NegativeInfinity;
						}
					}
					num = double.PositiveInfinity;
				}
				else
				{
					num = double.NaN;
				}
			}
			return num;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00049DA0 File Offset: 0x00048DA0
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_random)]
		public static double random()
		{
			return MathObject.internalRandom.NextDouble();
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00049DAC File Offset: 0x00048DAC
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_round)]
		public static double round(double d)
		{
			if (d == 0.0)
			{
				return d;
			}
			return Math.Floor(d + 0.5);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00049DCC File Offset: 0x00048DCC
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_sin)]
		public static double sin(double x)
		{
			return Math.Sin(x);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00049DD4 File Offset: 0x00048DD4
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_sqrt)]
		public static double sqrt(double x)
		{
			return Math.Sqrt(x);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00049DDC File Offset: 0x00048DDC
		[JSFunction(JSFunctionAttributeEnum.None, JSBuiltin.Math_tan)]
		public static double tan(double x)
		{
			return Math.Tan(x);
		}

		// Token: 0x040005E9 RID: 1513
		public const double E = 2.718281828459045;

		// Token: 0x040005EA RID: 1514
		public const double LN10 = 2.302585092994046;

		// Token: 0x040005EB RID: 1515
		public const double LN2 = 0.6931471805599453;

		// Token: 0x040005EC RID: 1516
		public const double LOG2E = 1.4426950408889634;

		// Token: 0x040005ED RID: 1517
		public const double LOG10E = 0.4342944819032518;

		// Token: 0x040005EE RID: 1518
		public const double PI = 3.141592653589793;

		// Token: 0x040005EF RID: 1519
		public const double SQRT1_2 = 0.7071067811865476;

		// Token: 0x040005F0 RID: 1520
		public const double SQRT2 = 1.4142135623730951;

		// Token: 0x040005F1 RID: 1521
		private static readonly Random internalRandom = new Random();

		// Token: 0x040005F2 RID: 1522
		internal static MathObject ob = null;
	}
}

using System;
using System.Globalization;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x020000D5 RID: 213
	public class NumberPrototype : NumberObject
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x0004A188 File Offset: 0x00049188
		internal NumberPrototype(ObjectPrototype parent)
			: base(parent, 0.0)
		{
			this.noExpando = true;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0004A1A6 File Offset: 0x000491A6
		public static NumberConstructor constructor
		{
			get
			{
				return NumberPrototype._constructor;
			}
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0004A1AD File Offset: 0x000491AD
		private static double ThisobToDouble(object thisob)
		{
			thisob = NumberPrototype.valueOf(thisob);
			return ((IConvertible)thisob).ToDouble(null);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0004A1C4 File Offset: 0x000491C4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toExponential)]
		public static string toExponential(object thisob, object fractionDigits)
		{
			double num = NumberPrototype.ThisobToDouble(thisob);
			double num2;
			if (fractionDigits == null || fractionDigits is Missing)
			{
				num2 = 16.0;
			}
			else
			{
				num2 = Convert.ToInteger(fractionDigits);
			}
			if (num2 < 0.0 || num2 > 20.0)
			{
				throw new JScriptException(JSError.FractionOutOfRange);
			}
			StringBuilder stringBuilder = new StringBuilder("#.");
			int num3 = 0;
			while ((double)num3 < num2)
			{
				stringBuilder.Append('0');
				num3++;
			}
			stringBuilder.Append("e+0");
			return num.ToString(stringBuilder.ToString(), CultureInfo.InvariantCulture);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0004A258 File Offset: 0x00049258
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toFixed)]
		public static string toFixed(object thisob, double fractionDigits)
		{
			double num = NumberPrototype.ThisobToDouble(thisob);
			if (double.IsNaN(fractionDigits))
			{
				fractionDigits = 0.0;
			}
			else if (fractionDigits < 0.0 || fractionDigits > 20.0)
			{
				throw new JScriptException(JSError.FractionOutOfRange);
			}
			return num.ToString("f" + ((int)fractionDigits).ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0004A2C9 File Offset: 0x000492C9
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toLocaleString)]
		public static string toLocaleString(object thisob)
		{
			return Convert.ToString(NumberPrototype.valueOf(thisob), PreferredType.LocaleString, true);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0004A2D8 File Offset: 0x000492D8
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toPrecision)]
		public static string toPrecision(object thisob, object precision)
		{
			double num = NumberPrototype.ThisobToDouble(thisob);
			if (precision == null || precision is Missing)
			{
				return Convert.ToString(num);
			}
			double num2 = Convert.ToInteger(precision);
			if (num2 < 1.0 || num2 > 21.0)
			{
				throw new JScriptException(JSError.PrecisionOutOfRange);
			}
			int num3 = (int)num2;
			if (double.IsNaN(num))
			{
				return "NaN";
			}
			if (double.IsInfinity(num))
			{
				if (num <= 0.0)
				{
					return "-Infinity";
				}
				return "Infinity";
			}
			else
			{
				string text;
				if (num >= 0.0)
				{
					text = "";
				}
				else
				{
					text = "-";
					num = -num;
				}
				string text2 = num.ToString("e" + (num3 - 1).ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
				int num4 = int.Parse(text2.Substring(text2.Length - 4), CultureInfo.InvariantCulture);
				text2 = text2.Substring(0, 1) + text2.Substring(2, num3 - 1);
				if (num4 >= num3 || num4 < -6)
				{
					return string.Concat(new string[]
					{
						text,
						text2.Substring(0, 1),
						(num3 > 1) ? ("." + text2.Substring(1)) : "",
						(num4 >= 0) ? "e+" : "e",
						num4.ToString(CultureInfo.InvariantCulture)
					});
				}
				if (num4 == num3 - 1)
				{
					return text + text2;
				}
				if (num4 >= 0)
				{
					return text + text2.Substring(0, num4 + 1) + "." + text2.Substring(num4 + 1);
				}
				return text + "0." + text2.PadLeft(num3 - num4 - 1, '0');
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0004A49C File Offset: 0x0004949C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_toString)]
		public static string toString(object thisob, object radix)
		{
			int num = 10;
			if (radix is IConvertible)
			{
				double num2 = ((IConvertible)radix).ToDouble(CultureInfo.InvariantCulture);
				int num3 = (int)num2;
				if (num2 == (double)num3)
				{
					num = num3;
				}
			}
			if (num < 2 || num > 36)
			{
				num = 10;
			}
			return Convert.ToString(NumberPrototype.valueOf(thisob), num);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0004A4E8 File Offset: 0x000494E8
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Number_valueOf)]
		public static object valueOf(object thisob)
		{
			if (thisob is NumberObject)
			{
				return ((NumberObject)thisob).value;
			}
			switch (Convert.GetTypeCode(thisob))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
				return thisob;
			default:
				throw new JScriptException(JSError.NumberExpected);
			}
		}

		// Token: 0x04000610 RID: 1552
		internal static readonly NumberPrototype ob = new NumberPrototype(ObjectPrototype.ob);

		// Token: 0x04000611 RID: 1553
		internal static NumberConstructor _constructor;
	}
}

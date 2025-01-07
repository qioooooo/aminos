using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class DoubleType
	{
		private DoubleType()
		{
		}

		public static double FromString(string Value)
		{
			return DoubleType.FromString(Value, null);
		}

		public static double FromString(string Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0.0;
			}
			double num2;
			try
			{
				long num;
				if (Utils.IsHexOrOctValue(Value, ref num))
				{
					num2 = (double)num;
				}
				else
				{
					num2 = DoubleType.Parse(Value, NumberFormat);
				}
			}
			catch (FormatException ex)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
				{
					Strings.Left(Value, 32),
					"Double"
				}), ex);
			}
			return num2;
		}

		public static double FromObject(object Value)
		{
			return DoubleType.FromObject(Value, null);
		}

		public static double FromObject(object Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0.0;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return -(convertible.ToBoolean(null) > false);
				case TypeCode.Byte:
					if (Value is byte)
					{
						return (double)((byte)Value);
					}
					return (double)convertible.ToByte(null);
				case TypeCode.Int16:
					if (Value is short)
					{
						return (double)((short)Value);
					}
					return (double)convertible.ToInt16(null);
				case TypeCode.Int32:
					if (Value is int)
					{
						return (double)((int)Value);
					}
					return (double)convertible.ToInt32(null);
				case TypeCode.Int64:
					if (Value is long)
					{
						return (double)((long)Value);
					}
					return (double)convertible.ToInt64(null);
				case TypeCode.Single:
					if (Value is float)
					{
						return (double)((float)Value);
					}
					return (double)convertible.ToSingle(null);
				case TypeCode.Double:
					if (Value is double)
					{
						return (double)Value;
					}
					return convertible.ToDouble(null);
				case TypeCode.Decimal:
					return DoubleType.DecimalToDouble(convertible);
				case TypeCode.String:
					return DoubleType.FromString(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Double"
			}));
		}

		private static double DecimalToDouble(IConvertible ValueInterface)
		{
			return Convert.ToDouble(ValueInterface.ToDecimal(null));
		}

		public static double Parse(string Value)
		{
			return DoubleType.Parse(Value, null);
		}

		internal static bool TryParse(string Value, ref double Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
			NumberFormatInfo normalizedNumberFormat = DecimalType.GetNormalizedNumberFormat(numberFormat);
			Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
			if (numberFormat == normalizedNumberFormat)
			{
				return double.TryParse(Value, NumberStyles.Any, normalizedNumberFormat, out Result);
			}
			bool flag;
			try
			{
				Result = double.Parse(Value, NumberStyles.Any, normalizedNumberFormat);
				flag = true;
			}
			catch (FormatException ex)
			{
				try
				{
					flag = double.TryParse(Value, NumberStyles.Any, numberFormat, out Result);
				}
				catch (ArgumentException ex2)
				{
					flag = false;
				}
			}
			catch (StackOverflowException ex3)
			{
				throw ex3;
			}
			catch (OutOfMemoryException ex4)
			{
				throw ex4;
			}
			catch (ThreadAbortException ex5)
			{
				throw ex5;
			}
			catch (Exception ex6)
			{
				flag = false;
			}
			return flag;
		}

		public static double Parse(string Value, NumberFormatInfo NumberFormat)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			if (NumberFormat == null)
			{
				NumberFormat = cultureInfo.NumberFormat;
			}
			NumberFormatInfo normalizedNumberFormat = DecimalType.GetNormalizedNumberFormat(NumberFormat);
			Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
			double num;
			try
			{
				num = double.Parse(Value, NumberStyles.Any, normalizedNumberFormat);
			}
			catch (FormatException obj) when (NumberFormat != normalizedNumberFormat)
			{
				num = double.Parse(Value, NumberStyles.Any, NumberFormat);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return num;
		}
	}
}

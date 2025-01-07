using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class DecimalType
	{
		private DecimalType()
		{
		}

		public static decimal FromBoolean(bool Value)
		{
			if (Value)
			{
				return -1m;
			}
			return 0m;
		}

		public static decimal FromString(string Value)
		{
			return DecimalType.FromString(Value, null);
		}

		public static decimal FromString(string Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0m;
			}
			decimal num2;
			try
			{
				long num;
				if (Utils.IsHexOrOctValue(Value, ref num))
				{
					num2 = new decimal(num);
				}
				else
				{
					num2 = DecimalType.Parse(Value, NumberFormat);
				}
			}
			catch (OverflowException ex)
			{
				throw ExceptionUtils.VbMakeException(6);
			}
			catch (FormatException ex2)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
				{
					Strings.Left(Value, 32),
					"Decimal"
				}));
			}
			return num2;
		}

		public static decimal FromObject(object Value)
		{
			return DecimalType.FromObject(Value, null);
		}

		public static decimal FromObject(object Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0m;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return DecimalType.FromBoolean(convertible.ToBoolean(null));
				case TypeCode.Byte:
					return new decimal((int)convertible.ToByte(null));
				case TypeCode.Int16:
					return new decimal((int)convertible.ToInt16(null));
				case TypeCode.Int32:
					return new decimal(convertible.ToInt32(null));
				case TypeCode.Int64:
					return new decimal(convertible.ToInt64(null));
				case TypeCode.Single:
					return new decimal(convertible.ToSingle(null));
				case TypeCode.Double:
					return new decimal(convertible.ToDouble(null));
				case TypeCode.Decimal:
					return convertible.ToDecimal(null);
				case TypeCode.String:
					return DecimalType.FromString(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Decimal"
			}));
		}

		public static decimal Parse(string Value, NumberFormatInfo NumberFormat)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			if (NumberFormat == null)
			{
				NumberFormat = cultureInfo.NumberFormat;
			}
			NumberFormatInfo normalizedNumberFormat = DecimalType.GetNormalizedNumberFormat(NumberFormat);
			Value = Utils.ToHalfwidthNumbers(Value, cultureInfo);
			decimal num;
			try
			{
				num = decimal.Parse(Value, NumberStyles.Any, normalizedNumberFormat);
			}
			catch (FormatException obj) when (NumberFormat != normalizedNumberFormat)
			{
				num = decimal.Parse(Value, NumberStyles.Any, NumberFormat);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return num;
		}

		internal static NumberFormatInfo GetNormalizedNumberFormat(NumberFormatInfo InNumberFormat)
		{
			if (InNumberFormat.CurrencyDecimalSeparator != null && InNumberFormat.NumberDecimalSeparator != null && InNumberFormat.CurrencyGroupSeparator != null && InNumberFormat.NumberGroupSeparator != null && InNumberFormat.CurrencyDecimalSeparator.Length == 1 && InNumberFormat.NumberDecimalSeparator.Length == 1 && InNumberFormat.CurrencyGroupSeparator.Length == 1 && InNumberFormat.NumberGroupSeparator.Length == 1 && InNumberFormat.CurrencyDecimalSeparator[0] == InNumberFormat.NumberDecimalSeparator[0] && InNumberFormat.CurrencyGroupSeparator[0] == InNumberFormat.NumberGroupSeparator[0] && InNumberFormat.CurrencyDecimalDigits == InNumberFormat.NumberDecimalDigits)
			{
				return InNumberFormat;
			}
			checked
			{
				if (InNumberFormat.CurrencyDecimalSeparator != null && InNumberFormat.NumberDecimalSeparator != null && InNumberFormat.CurrencyDecimalSeparator.Length == InNumberFormat.NumberDecimalSeparator.Length && InNumberFormat.CurrencyGroupSeparator != null && InNumberFormat.NumberGroupSeparator != null && InNumberFormat.CurrencyGroupSeparator.Length == InNumberFormat.NumberGroupSeparator.Length)
				{
					int num = 0;
					int num2 = InNumberFormat.CurrencyDecimalSeparator.Length - 1;
					for (int i = num; i <= num2; i++)
					{
						if (InNumberFormat.CurrencyDecimalSeparator[i] != InNumberFormat.NumberDecimalSeparator[i])
						{
							goto IL_018E;
						}
					}
					int num3 = 0;
					int num4 = InNumberFormat.CurrencyGroupSeparator.Length - 1;
					for (int i = num3; i <= num4; i++)
					{
						if (InNumberFormat.CurrencyGroupSeparator[i] != InNumberFormat.NumberGroupSeparator[i])
						{
							goto IL_018E;
						}
					}
					return InNumberFormat;
				}
			}
			IL_018E:
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)InNumberFormat.Clone();
			NumberFormatInfo numberFormatInfo2 = numberFormatInfo;
			numberFormatInfo2.CurrencyDecimalSeparator = numberFormatInfo2.NumberDecimalSeparator;
			numberFormatInfo2.CurrencyGroupSeparator = numberFormatInfo2.NumberGroupSeparator;
			numberFormatInfo2.CurrencyDecimalDigits = numberFormatInfo2.NumberDecimalDigits;
			return numberFormatInfo;
		}
	}
}

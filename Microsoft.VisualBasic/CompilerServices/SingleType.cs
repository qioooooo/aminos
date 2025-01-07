using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class SingleType
	{
		private SingleType()
		{
		}

		public static float FromString(string Value)
		{
			return SingleType.FromString(Value, null);
		}

		public static float FromString(string Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0f;
			}
			float num2;
			try
			{
				long num;
				if (Utils.IsHexOrOctValue(Value, ref num))
				{
					num2 = (float)num;
				}
				else
				{
					double num3 = DoubleType.Parse(Value, NumberFormat);
					if ((num3 < -3.4028234663852886E+38 || num3 > 3.4028234663852886E+38) && !double.IsInfinity(num3))
					{
						throw new OverflowException();
					}
					num2 = (float)num3;
				}
			}
			catch (FormatException ex)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
				{
					Strings.Left(Value, 32),
					"Single"
				}), ex);
			}
			return num2;
		}

		public static float FromObject(object Value)
		{
			return SingleType.FromObject(Value, null);
		}

		public static float FromObject(object Value, NumberFormatInfo NumberFormat)
		{
			if (Value == null)
			{
				return 0f;
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
						return (float)((byte)Value);
					}
					return (float)convertible.ToByte(null);
				case TypeCode.Int16:
					if (Value is short)
					{
						return (float)((short)Value);
					}
					return (float)convertible.ToInt16(null);
				case TypeCode.Int32:
					if (Value is int)
					{
						return (float)((int)Value);
					}
					return (float)convertible.ToInt32(null);
				case TypeCode.Int64:
					if (Value is long)
					{
						return (float)((long)Value);
					}
					return (float)convertible.ToInt64(null);
				case TypeCode.Single:
					if (Value is float)
					{
						return (float)Value;
					}
					return convertible.ToSingle(null);
				case TypeCode.Double:
					if (Value is double)
					{
						return (float)((double)Value);
					}
					return (float)convertible.ToDouble(null);
				case TypeCode.Decimal:
					return SingleType.DecimalToSingle(convertible);
				case TypeCode.String:
					return SingleType.FromString(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Single"
			}));
		}

		private static float DecimalToSingle(IConvertible ValueInterface)
		{
			return Convert.ToSingle(ValueInterface.ToDecimal(null));
		}
	}
}

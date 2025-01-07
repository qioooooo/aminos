using System;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class BooleanType
	{
		private BooleanType()
		{
		}

		public static bool FromString(string Value)
		{
			if (Value == null)
			{
				Value = "";
			}
			bool flag;
			try
			{
				CultureInfo cultureInfo = Utils.GetCultureInfo();
				long num;
				if (string.Compare(Value, bool.FalseString, true, cultureInfo) == 0)
				{
					flag = false;
				}
				else if (string.Compare(Value, bool.TrueString, true, cultureInfo) == 0)
				{
					flag = true;
				}
				else if (Utils.IsHexOrOctValue(Value, ref num))
				{
					flag = num != 0L;
				}
				else
				{
					flag = DoubleType.Parse(Value) != 0.0;
				}
			}
			catch (FormatException ex)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
				{
					Strings.Left(Value, 32),
					"Boolean"
				}), ex);
			}
			return flag;
		}

		public static bool FromObject(object Value)
		{
			if (Value == null)
			{
				return false;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					if (Value is bool)
					{
						return (bool)Value;
					}
					return convertible.ToBoolean(null);
				case TypeCode.Byte:
					if (Value is byte)
					{
						return (byte)Value > 0;
					}
					return convertible.ToByte(null) > 0;
				case TypeCode.Int16:
					if (Value is short)
					{
						return (short)Value != 0;
					}
					return convertible.ToInt16(null) != 0;
				case TypeCode.Int32:
					if (Value is int)
					{
						return (int)Value != 0;
					}
					return convertible.ToInt32(null) != 0;
				case TypeCode.Int64:
					if (Value is long)
					{
						return (long)Value != 0L;
					}
					return convertible.ToInt64(null) != 0L;
				case TypeCode.Single:
					if (Value is float)
					{
						return (float)Value != 0f;
					}
					return convertible.ToSingle(null) != 0f;
				case TypeCode.Double:
					if (Value is double)
					{
						return (double)Value != 0.0;
					}
					return convertible.ToDouble(null) != 0.0;
				case TypeCode.Decimal:
					return BooleanType.DecimalToBoolean(convertible);
				case TypeCode.String:
				{
					string text = Value as string;
					if (text != null)
					{
						return BooleanType.FromString(text);
					}
					return BooleanType.FromString(convertible.ToString(null));
				}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Boolean"
			}));
		}

		private static bool DecimalToBoolean(IConvertible ValueInterface)
		{
			return Convert.ToBoolean(ValueInterface.ToDecimal(null));
		}
	}
}

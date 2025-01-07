using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ShortType
	{
		private ShortType()
		{
		}

		public static short FromString(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				short num2;
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(Value, ref num))
					{
						num2 = (short)num;
					}
					else
					{
						num2 = (short)Math.Round(DoubleType.Parse(Value));
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(Value, 32),
						"Short"
					}), ex);
				}
				return num2;
			}
		}

		public static short FromObject(object Value)
		{
			if (Value == null)
			{
				return 0;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
					case TypeCode.Boolean:
						return unchecked(-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (short)((byte)Value);
						}
						return (short)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (short)Value;
						}
						return convertible.ToInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (short)((int)Value);
						}
						return (short)convertible.ToInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (short)((long)Value);
						}
						return (short)convertible.ToInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (short)Math.Round((double)((float)Value));
						}
						return (short)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (short)Math.Round((double)Value);
						}
						return (short)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						return ShortType.DecimalToShort(convertible);
					case TypeCode.String:
						return ShortType.FromString(convertible.ToString(null));
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"Short"
				}));
			}
		}

		private static short DecimalToShort(IConvertible ValueInterface)
		{
			return Convert.ToInt16(ValueInterface.ToDecimal(null));
		}
	}
}

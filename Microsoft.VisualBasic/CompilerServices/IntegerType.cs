using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class IntegerType
	{
		private IntegerType()
		{
		}

		public static int FromString(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				int num2;
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(Value, ref num))
					{
						num2 = (int)num;
					}
					else
					{
						num2 = (int)Math.Round(DoubleType.Parse(Value));
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(Value, 32),
						"Integer"
					}), ex);
				}
				return num2;
			}
		}

		public static int FromObject(object Value)
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
							return (int)((byte)Value);
						}
						return (int)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (int)((short)Value);
						}
						return (int)convertible.ToInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (int)Value;
						}
						return convertible.ToInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (int)((long)Value);
						}
						return (int)convertible.ToInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (int)Math.Round((double)((float)Value));
						}
						return (int)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (int)Math.Round((double)Value);
						}
						return (int)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						return IntegerType.DecimalToInteger(convertible);
					case TypeCode.String:
						return IntegerType.FromString(convertible.ToString(null));
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"Integer"
				}));
			}
		}

		private static int DecimalToInteger(IConvertible ValueInterface)
		{
			return Convert.ToInt32(ValueInterface.ToDecimal(null));
		}
	}
}

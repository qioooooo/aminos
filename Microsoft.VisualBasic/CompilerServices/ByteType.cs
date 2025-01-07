using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ByteType
	{
		private ByteType()
		{
		}

		public static byte FromString(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				byte b;
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(Value, ref num))
					{
						b = (byte)num;
					}
					else
					{
						b = (byte)Math.Round(DoubleType.Parse(Value));
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(Value, 32),
						"Byte"
					}), ex);
				}
				return b;
			}
		}

		public static byte FromObject(object Value)
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
							return (byte)Value;
						}
						return convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (byte)((short)Value);
						}
						return (byte)convertible.ToInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (byte)((int)Value);
						}
						return (byte)convertible.ToInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (byte)((long)Value);
						}
						return (byte)convertible.ToInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (byte)Math.Round((double)((float)Value));
						}
						return (byte)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (byte)Math.Round((double)Value);
						}
						return (byte)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						return ByteType.DecimalToByte(convertible);
					case TypeCode.String:
						return ByteType.FromString(convertible.ToString(null));
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"Byte"
				}));
			}
		}

		private static byte DecimalToByte(IConvertible ValueInterface)
		{
			return Convert.ToByte(ValueInterface.ToDecimal(null));
		}
	}
}

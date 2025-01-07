using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class LongType
	{
		private LongType()
		{
		}

		public static long FromString(string Value)
		{
			if (Value == null)
			{
				return 0L;
			}
			long num2;
			try
			{
				long num;
				if (Utils.IsHexOrOctValue(Value, ref num))
				{
					num2 = num;
				}
				else
				{
					num2 = Convert.ToInt64(DecimalType.Parse(Value, null));
				}
			}
			catch (FormatException ex)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
				{
					Strings.Left(Value, 32),
					"Long"
				}), ex);
			}
			return num2;
		}

		public static long FromObject(object Value)
		{
			if (Value == null)
			{
				return 0L;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return (-((convertible.ToBoolean(null) > false) ? 1L : 0L)) ? 1L : 0L;
				case TypeCode.Byte:
					if (Value is byte)
					{
						return (long)((ulong)((byte)Value));
					}
					return (long)((ulong)convertible.ToByte(null));
				case TypeCode.Int16:
					if (Value is short)
					{
						return (long)((short)Value);
					}
					return (long)convertible.ToInt16(null);
				case TypeCode.Int32:
					if (Value is int)
					{
						return (long)((int)Value);
					}
					return (long)convertible.ToInt32(null);
				case TypeCode.Int64:
					if (Value is long)
					{
						return (long)Value;
					}
					return convertible.ToInt64(null);
				case TypeCode.Single:
					checked
					{
						if (Value is float)
						{
							return (long)Math.Round((double)((float)Value));
						}
						return (long)Math.Round((double)convertible.ToSingle(null));
					}
				case TypeCode.Double:
					checked
					{
						if (Value is double)
						{
							return (long)Math.Round((double)Value);
						}
						return (long)Math.Round(convertible.ToDouble(null));
					}
				case TypeCode.Decimal:
					return LongType.DecimalToLong(convertible);
				case TypeCode.String:
					return LongType.FromString(convertible.ToString(null));
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Long"
			}));
		}

		private static long DecimalToLong(IConvertible ValueInterface)
		{
			return Convert.ToInt64(ValueInterface.ToDecimal(null));
		}
	}
}

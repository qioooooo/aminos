using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class Conversions
	{
		private Conversions()
		{
		}

		public static bool ToBoolean(string Value)
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
					flag = Conversions.ParseDouble(Value) != 0.0;
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

		public static bool ToBoolean(object Value)
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
				case TypeCode.SByte:
					if (Value is sbyte)
					{
						return (sbyte)Value != 0;
					}
					return convertible.ToSByte(null) != 0;
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
				case TypeCode.UInt16:
					if (Value is ushort)
					{
						return (ushort)Value > 0;
					}
					return convertible.ToUInt16(null) > 0;
				case TypeCode.Int32:
					if (Value is int)
					{
						return (int)Value != 0;
					}
					return convertible.ToInt32(null) != 0;
				case TypeCode.UInt32:
					if (Value is uint)
					{
						return (uint)Value > 0U;
					}
					return convertible.ToUInt32(null) > 0U;
				case TypeCode.Int64:
					if (Value is long)
					{
						return (long)Value != 0L;
					}
					return convertible.ToInt64(null) != 0L;
				case TypeCode.UInt64:
					if (Value is ulong)
					{
						return (ulong)Value > 0UL;
					}
					return convertible.ToUInt64(null) > 0UL;
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
					if (Value is decimal)
					{
						return convertible.ToBoolean(null);
					}
					return Convert.ToBoolean(convertible.ToDecimal(null));
				case TypeCode.String:
				{
					string text = Value as string;
					if (text != null)
					{
						return Conversions.ToBoolean(text);
					}
					return Conversions.ToBoolean(convertible.ToString(null));
				}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Boolean"
			}));
		}

		public static byte ToByte(string Value)
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
						b = (byte)Math.Round(Conversions.ParseDouble(Value));
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

		public static byte ToByte(object Value)
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
						unchecked
						{
							if (Value is bool)
							{
								return (-(((bool)Value > false) ? 1 : 0)) ? 1 : 0;
							}
							return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (byte)((sbyte)Value);
						}
						return (byte)convertible.ToSByte(null);
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
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (byte)((ushort)Value);
						}
						return (byte)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (byte)((int)Value);
						}
						return (byte)convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (byte)((uint)Value);
						}
						return (byte)convertible.ToUInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (byte)((long)Value);
						}
						return (byte)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (byte)((ulong)Value);
						}
						return (byte)convertible.ToUInt64(null);
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
						if (Value is decimal)
						{
							return convertible.ToByte(null);
						}
						return Convert.ToByte(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToByte(text);
						}
						return Conversions.ToByte(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"Byte"
				}));
			}
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				sbyte b;
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(Value, ref num))
					{
						b = (sbyte)num;
					}
					else
					{
						b = (sbyte)Math.Round(Conversions.ParseDouble(Value));
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(Value, 32),
						"SByte"
					}), ex);
				}
				return b;
			}
		}

		[CLSCompliant(false)]
		public static sbyte ToSByte(object Value)
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
						unchecked
						{
							if (Value is bool)
							{
								return (-(((bool)Value > false) ? 1 : 0)) ? 1 : 0;
							}
							return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (sbyte)Value;
						}
						return convertible.ToSByte(null);
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (sbyte)((byte)Value);
						}
						return (sbyte)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (sbyte)((short)Value);
						}
						return (sbyte)convertible.ToInt16(null);
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (sbyte)((ushort)Value);
						}
						return (sbyte)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (sbyte)((int)Value);
						}
						return (sbyte)convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (sbyte)((uint)Value);
						}
						return (sbyte)convertible.ToUInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (sbyte)((long)Value);
						}
						return (sbyte)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (sbyte)((ulong)Value);
						}
						return (sbyte)convertible.ToUInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (sbyte)Math.Round((double)((float)Value));
						}
						return (sbyte)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (sbyte)Math.Round((double)Value);
						}
						return (sbyte)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToSByte(null);
						}
						return Convert.ToSByte(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToSByte(text);
						}
						return Conversions.ToSByte(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"SByte"
				}));
			}
		}

		public static short ToShort(string Value)
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
						num2 = (short)Math.Round(Conversions.ParseDouble(Value));
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

		public static short ToShort(object Value)
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
						unchecked
						{
							if (Value is bool)
							{
								return (-(((bool)Value > false) ? 1 : 0)) ? 1 : 0;
							}
							return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (short)((sbyte)Value);
						}
						return (short)convertible.ToSByte(null);
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
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (short)((ushort)Value);
						}
						return (short)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (short)((int)Value);
						}
						return (short)convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (short)((uint)Value);
						}
						return (short)convertible.ToUInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (short)((long)Value);
						}
						return (short)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (short)((ulong)Value);
						}
						return (short)convertible.ToUInt64(null);
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
						if (Value is decimal)
						{
							return convertible.ToInt16(null);
						}
						return Convert.ToInt16(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToShort(text);
						}
						return Conversions.ToShort(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"Short"
				}));
			}
		}

		[CLSCompliant(false)]
		public static ushort ToUShort(string Value)
		{
			if (Value == null)
			{
				return 0;
			}
			checked
			{
				ushort num2;
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(Value, ref num))
					{
						num2 = (ushort)num;
					}
					else
					{
						num2 = (ushort)Math.Round(Conversions.ParseDouble(Value));
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(Value, 32),
						"UShort"
					}), ex);
				}
				return num2;
			}
		}

		[CLSCompliant(false)]
		public static ushort ToUShort(object Value)
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
						unchecked
						{
							if (Value is bool)
							{
								return (-(((bool)Value > false) ? 1 : 0)) ? 1 : 0;
							}
							return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (ushort)((sbyte)Value);
						}
						return (ushort)convertible.ToSByte(null);
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (ushort)((byte)Value);
						}
						return (ushort)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (ushort)((short)Value);
						}
						return (ushort)convertible.ToInt16(null);
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (ushort)Value;
						}
						return convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (ushort)((int)Value);
						}
						return (ushort)convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (ushort)((uint)Value);
						}
						return (ushort)convertible.ToUInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (ushort)((long)Value);
						}
						return (ushort)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (ushort)((ulong)Value);
						}
						return (ushort)convertible.ToUInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (ushort)Math.Round((double)((float)Value));
						}
						return (ushort)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (ushort)Math.Round((double)Value);
						}
						return (ushort)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToUInt16(null);
						}
						return Convert.ToUInt16(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToUShort(text);
						}
						return Conversions.ToUShort(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"UShort"
				}));
			}
		}

		public static int ToInteger(string Value)
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
						num2 = (int)Math.Round(Conversions.ParseDouble(Value));
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

		public static int ToInteger(object Value)
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
						unchecked
						{
							if (Value is bool)
							{
								return (-(((bool)Value > false) ? 1 : 0)) ? 1 : 0;
							}
							return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (int)((sbyte)Value);
						}
						return (int)convertible.ToSByte(null);
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
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (int)((ushort)Value);
						}
						return (int)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (int)Value;
						}
						return convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (int)((uint)Value);
						}
						return (int)convertible.ToUInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (int)((long)Value);
						}
						return (int)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (int)((ulong)Value);
						}
						return (int)convertible.ToUInt64(null);
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
						if (Value is decimal)
						{
							return convertible.ToInt32(null);
						}
						return Convert.ToInt32(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToInteger(text);
						}
						return Conversions.ToInteger(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"Integer"
				}));
			}
		}

		[CLSCompliant(false)]
		public static uint ToUInteger(string Value)
		{
			if (Value == null)
			{
				return 0U;
			}
			checked
			{
				uint num2;
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(Value, ref num))
					{
						num2 = (uint)num;
					}
					else
					{
						num2 = (uint)Math.Round(Conversions.ParseDouble(Value));
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
					{
						Strings.Left(Value, 32),
						"UInteger"
					}), ex);
				}
				return num2;
			}
		}

		[CLSCompliant(false)]
		public static uint ToUInteger(object Value)
		{
			if (Value == null)
			{
				return 0U;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
					case TypeCode.Boolean:
						unchecked
						{
							if (Value is bool)
							{
								return (-(((bool)Value > false) ? 1U : 0U)) ? 1U : 0U;
							}
							return (-((convertible.ToBoolean(null) > false) ? 1U : 0U)) ? 1U : 0U;
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (uint)((sbyte)Value);
						}
						return (uint)convertible.ToSByte(null);
					case TypeCode.Byte:
						if (Value is byte)
						{
							return (uint)((byte)Value);
						}
						return (uint)convertible.ToByte(null);
					case TypeCode.Int16:
						if (Value is short)
						{
							return (uint)((short)Value);
						}
						return (uint)convertible.ToInt16(null);
					case TypeCode.UInt16:
						if (Value is ushort)
						{
							return (uint)((ushort)Value);
						}
						return (uint)convertible.ToUInt16(null);
					case TypeCode.Int32:
						if (Value is int)
						{
							return (uint)((int)Value);
						}
						return (uint)convertible.ToInt32(null);
					case TypeCode.UInt32:
						if (Value is uint)
						{
							return (uint)Value;
						}
						return convertible.ToUInt32(null);
					case TypeCode.Int64:
						if (Value is long)
						{
							return (uint)((long)Value);
						}
						return (uint)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (uint)((ulong)Value);
						}
						return (uint)convertible.ToUInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (uint)Math.Round((double)((float)Value));
						}
						return (uint)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (uint)Math.Round((double)Value);
						}
						return (uint)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToUInt32(null);
						}
						return Convert.ToUInt32(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToUInteger(text);
						}
						return Conversions.ToUInteger(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"UInteger"
				}));
			}
		}

		public static long ToLong(string Value)
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
					num2 = Convert.ToInt64(Conversions.ParseDecimal(Value, null));
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

		public static long ToLong(object Value)
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
					if (Value is bool)
					{
						return (-(((bool)Value > false) ? 1L : 0L)) ? 1L : 0L;
					}
					return (-((convertible.ToBoolean(null) > false) ? 1L : 0L)) ? 1L : 0L;
				case TypeCode.SByte:
					if (Value is sbyte)
					{
						return (long)((sbyte)Value);
					}
					return (long)convertible.ToSByte(null);
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
				case TypeCode.UInt16:
					if (Value is ushort)
					{
						return (long)((ulong)((ushort)Value));
					}
					return (long)((ulong)convertible.ToUInt16(null));
				case TypeCode.Int32:
					if (Value is int)
					{
						return (long)((int)Value);
					}
					return (long)convertible.ToInt32(null);
				case TypeCode.UInt32:
					if (Value is uint)
					{
						return (long)((ulong)((uint)Value));
					}
					return (long)((ulong)convertible.ToUInt32(null));
				case TypeCode.Int64:
					if (Value is long)
					{
						return (long)Value;
					}
					return convertible.ToInt64(null);
				case TypeCode.UInt64:
					checked
					{
						if (Value is ulong)
						{
							return (long)((ulong)Value);
						}
						return (long)convertible.ToUInt64(null);
					}
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
					if (Value is decimal)
					{
						return convertible.ToInt64(null);
					}
					return Convert.ToInt64(convertible.ToDecimal(null));
				case TypeCode.String:
				{
					string text = Value as string;
					if (text != null)
					{
						return Conversions.ToLong(text);
					}
					return Conversions.ToLong(convertible.ToString(null));
				}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Long"
			}));
		}

		[CLSCompliant(false)]
		public static ulong ToULong(string Value)
		{
			if (Value == null)
			{
				return 0UL;
			}
			ulong num2;
			try
			{
				ulong num;
				if (Utils.IsHexOrOctValue(Value, ref num))
				{
					num2 = num;
				}
				else
				{
					num2 = Convert.ToUInt64(Conversions.ParseDecimal(Value, null));
				}
			}
			catch (FormatException ex)
			{
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
				{
					Strings.Left(Value, 32),
					"ULong"
				}), ex);
			}
			return num2;
		}

		[CLSCompliant(false)]
		public static ulong ToULong(object Value)
		{
			if (Value == null)
			{
				return 0UL;
			}
			IConvertible convertible = Value as IConvertible;
			checked
			{
				if (convertible != null)
				{
					switch (convertible.GetTypeCode())
					{
					case TypeCode.Boolean:
						unchecked
						{
							if (Value is bool)
							{
								return (ulong)((-(((bool)Value > false) ? 1L : 0L)) ? 1L : 0L);
							}
							return (ulong)((-((convertible.ToBoolean(null) > false) ? 1L : 0L)) ? 1L : 0L);
						}
					case TypeCode.SByte:
						if (Value is sbyte)
						{
							return (ulong)((sbyte)Value);
						}
						return (ulong)convertible.ToSByte(null);
					case TypeCode.Byte:
						unchecked
						{
							if (Value is byte)
							{
								return (ulong)((byte)Value);
							}
							return (ulong)convertible.ToByte(null);
						}
					case TypeCode.Int16:
						if (Value is short)
						{
							return (ulong)((short)Value);
						}
						return (ulong)convertible.ToInt16(null);
					case TypeCode.UInt16:
						unchecked
						{
							if (Value is ushort)
							{
								return (ulong)((ushort)Value);
							}
							return (ulong)convertible.ToUInt16(null);
						}
					case TypeCode.Int32:
						if (Value is int)
						{
							return (ulong)((int)Value);
						}
						return (ulong)convertible.ToInt32(null);
					case TypeCode.UInt32:
						unchecked
						{
							if (Value is uint)
							{
								return (ulong)((uint)Value);
							}
							return (ulong)convertible.ToUInt32(null);
						}
					case TypeCode.Int64:
						if (Value is long)
						{
							return (ulong)((long)Value);
						}
						return (ulong)convertible.ToInt64(null);
					case TypeCode.UInt64:
						if (Value is ulong)
						{
							return (ulong)Value;
						}
						return convertible.ToUInt64(null);
					case TypeCode.Single:
						if (Value is float)
						{
							return (ulong)Math.Round((double)((float)Value));
						}
						return (ulong)Math.Round((double)convertible.ToSingle(null));
					case TypeCode.Double:
						if (Value is double)
						{
							return (ulong)Math.Round((double)Value);
						}
						return (ulong)Math.Round(convertible.ToDouble(null));
					case TypeCode.Decimal:
						if (Value is decimal)
						{
							return convertible.ToUInt64(null);
						}
						return Convert.ToUInt64(convertible.ToDecimal(null));
					case TypeCode.String:
					{
						string text = Value as string;
						if (text != null)
						{
							return Conversions.ToULong(text);
						}
						return Conversions.ToULong(convertible.ToString(null));
					}
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(Value),
					"ULong"
				}));
			}
		}

		public static decimal ToDecimal(bool Value)
		{
			if (Value)
			{
				return -1m;
			}
			return 0m;
		}

		public static decimal ToDecimal(string Value)
		{
			return Conversions.ToDecimal(Value, null);
		}

		internal static decimal ToDecimal(string Value, NumberFormatInfo NumberFormat)
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
					num2 = Conversions.ParseDecimal(Value, NumberFormat);
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

		public static decimal ToDecimal(object Value)
		{
			return Conversions.ToDecimal(Value, null);
		}

		internal static decimal ToDecimal(object Value, NumberFormatInfo NumberFormat)
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
					if (Value is bool)
					{
						return Conversions.ToDecimal((bool)Value);
					}
					return Conversions.ToDecimal(convertible.ToBoolean(null));
				case TypeCode.SByte:
					if (Value is sbyte)
					{
						return new decimal((int)((sbyte)Value));
					}
					return new decimal((int)convertible.ToSByte(null));
				case TypeCode.Byte:
					if (Value is byte)
					{
						return new decimal((int)((byte)Value));
					}
					return new decimal((int)convertible.ToByte(null));
				case TypeCode.Int16:
					if (Value is short)
					{
						return new decimal((int)((short)Value));
					}
					return new decimal((int)convertible.ToInt16(null));
				case TypeCode.UInt16:
					if (Value is ushort)
					{
						return new decimal((int)((ushort)Value));
					}
					return new decimal((int)convertible.ToUInt16(null));
				case TypeCode.Int32:
					if (Value is int)
					{
						return new decimal((int)Value);
					}
					return new decimal(convertible.ToInt32(null));
				case TypeCode.UInt32:
					if (Value is uint)
					{
						return new decimal((uint)Value);
					}
					return new decimal(convertible.ToUInt32(null));
				case TypeCode.Int64:
					if (Value is long)
					{
						return new decimal((long)Value);
					}
					return new decimal(convertible.ToInt64(null));
				case TypeCode.UInt64:
					if (Value is ulong)
					{
						return new decimal((ulong)Value);
					}
					return new decimal(convertible.ToUInt64(null));
				case TypeCode.Single:
					if (Value is float)
					{
						return new decimal((float)Value);
					}
					return new decimal(convertible.ToSingle(null));
				case TypeCode.Double:
					if (Value is double)
					{
						return new decimal((double)Value);
					}
					return new decimal(convertible.ToDouble(null));
				case TypeCode.Decimal:
					return convertible.ToDecimal(null);
				case TypeCode.String:
					return Conversions.ToDecimal(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Decimal"
			}));
		}

		private static decimal ParseDecimal(string Value, NumberFormatInfo NumberFormat)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			if (NumberFormat == null)
			{
				NumberFormat = cultureInfo.NumberFormat;
			}
			NumberFormatInfo normalizedNumberFormat = Conversions.GetNormalizedNumberFormat(NumberFormat);
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

		private static NumberFormatInfo GetNormalizedNumberFormat(NumberFormatInfo InNumberFormat)
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

		public static float ToSingle(string Value)
		{
			return Conversions.ToSingle(Value, null);
		}

		internal static float ToSingle(string Value, NumberFormatInfo NumberFormat)
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
					double num3 = Conversions.ParseDouble(Value, NumberFormat);
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

		public static float ToSingle(object Value)
		{
			return Conversions.ToSingle(Value, null);
		}

		internal static float ToSingle(object Value, NumberFormatInfo NumberFormat)
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
					if (Value is bool)
					{
						return -((bool)Value > false);
					}
					return -(convertible.ToBoolean(null) > false);
				case TypeCode.SByte:
					if (Value is sbyte)
					{
						return (float)((sbyte)Value);
					}
					return (float)convertible.ToSByte(null);
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
				case TypeCode.UInt16:
					if (Value is ushort)
					{
						return (float)((ushort)Value);
					}
					return (float)convertible.ToUInt16(null);
				case TypeCode.Int32:
					if (Value is int)
					{
						return (float)((int)Value);
					}
					return (float)convertible.ToInt32(null);
				case TypeCode.UInt32:
					if (Value is uint)
					{
						return (uint)Value;
					}
					return convertible.ToUInt32(null);
				case TypeCode.Int64:
					if (Value is long)
					{
						return (float)((long)Value);
					}
					return (float)convertible.ToInt64(null);
				case TypeCode.UInt64:
					if (Value is ulong)
					{
						return (ulong)Value;
					}
					return convertible.ToUInt64(null);
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
					if (Value is decimal)
					{
						return convertible.ToSingle(null);
					}
					return Convert.ToSingle(convertible.ToDecimal(null));
				case TypeCode.String:
					return Conversions.ToSingle(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Single"
			}));
		}

		public static double ToDouble(string Value)
		{
			return Conversions.ToDouble(Value, null);
		}

		internal static double ToDouble(string Value, NumberFormatInfo NumberFormat)
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
					num2 = Conversions.ParseDouble(Value, NumberFormat);
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

		public static double ToDouble(object Value)
		{
			return Conversions.ToDouble(Value, null);
		}

		internal static double ToDouble(object Value, NumberFormatInfo NumberFormat)
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
					if (Value is bool)
					{
						return -((bool)Value > false);
					}
					return -(convertible.ToBoolean(null) > false);
				case TypeCode.SByte:
					if (Value is sbyte)
					{
						return (double)((sbyte)Value);
					}
					return (double)convertible.ToSByte(null);
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
				case TypeCode.UInt16:
					if (Value is ushort)
					{
						return (double)((ushort)Value);
					}
					return (double)convertible.ToUInt16(null);
				case TypeCode.Int32:
					if (Value is int)
					{
						return (double)((int)Value);
					}
					return (double)convertible.ToInt32(null);
				case TypeCode.UInt32:
					if (Value is uint)
					{
						return (uint)Value;
					}
					return convertible.ToUInt32(null);
				case TypeCode.Int64:
					if (Value is long)
					{
						return (double)((long)Value);
					}
					return (double)convertible.ToInt64(null);
				case TypeCode.UInt64:
					if (Value is ulong)
					{
						return (ulong)Value;
					}
					return convertible.ToUInt64(null);
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
					if (Value is decimal)
					{
						return convertible.ToDouble(null);
					}
					return Convert.ToDouble(convertible.ToDecimal(null));
				case TypeCode.String:
					return Conversions.ToDouble(convertible.ToString(null), NumberFormat);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Double"
			}));
		}

		private static double ParseDouble(string Value)
		{
			return Conversions.ParseDouble(Value, null);
		}

		internal static bool TryParseDouble(string Value, ref double Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			NumberFormatInfo numberFormat = cultureInfo.NumberFormat;
			NumberFormatInfo normalizedNumberFormat = Conversions.GetNormalizedNumberFormat(numberFormat);
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

		private static double ParseDouble(string Value, NumberFormatInfo NumberFormat)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			if (NumberFormat == null)
			{
				NumberFormat = cultureInfo.NumberFormat;
			}
			NumberFormatInfo normalizedNumberFormat = Conversions.GetNormalizedNumberFormat(NumberFormat);
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

		public static DateTime ToDate(string Value)
		{
			DateTime dateTime;
			if (Conversions.TryParseDate(Value, ref dateTime))
			{
				return dateTime;
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromStringTo", new string[]
			{
				Strings.Left(Value, 32),
				"Date"
			}));
		}

		public static DateTime ToDate(object Value)
		{
			if (Value == null)
			{
				DateTime dateTime;
				return dateTime;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.DateTime:
					if (Value is DateTime)
					{
						return (DateTime)Value;
					}
					return convertible.ToDateTime(null);
				case TypeCode.String:
				{
					string text = Value as string;
					if (text != null)
					{
						return Conversions.ToDate(text);
					}
					return Conversions.ToDate(convertible.ToString(null));
				}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Date"
			}));
		}

		internal static bool TryParseDate(string Value, ref DateTime Result)
		{
			CultureInfo cultureInfo = Utils.GetCultureInfo();
			return DateTime.TryParse(Utils.ToHalfwidthNumbers(Value, cultureInfo), cultureInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault, out Result);
		}

		public static char ToChar(string Value)
		{
			if (Value == null || Value.Length == 0)
			{
				return '\0';
			}
			return Value[0];
		}

		public static char ToChar(object Value)
		{
			if (Value == null)
			{
				return '\0';
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Char:
					if (Value is char)
					{
						return (char)Value;
					}
					return convertible.ToChar(null);
				case TypeCode.String:
				{
					string text = Value as string;
					if (text != null)
					{
						return Conversions.ToChar(text);
					}
					return Conversions.ToChar(convertible.ToString(null));
				}
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Char"
			}));
		}

		public static char[] ToCharArrayRankOne(string Value)
		{
			if (Value == null)
			{
				Value = "";
			}
			return Value.ToCharArray();
		}

		public static char[] ToCharArrayRankOne(object Value)
		{
			if (Value == null)
			{
				return "".ToCharArray();
			}
			char[] array = Value as char[];
			if (array != null && array.Rank == 1)
			{
				return array;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null && convertible.GetTypeCode() == TypeCode.String)
			{
				return convertible.ToString(null).ToCharArray();
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"Char()"
			}));
		}

		public static string ToString(bool Value)
		{
			if (Value)
			{
				return bool.TrueString;
			}
			return bool.FalseString;
		}

		public static string ToString(byte Value)
		{
			return Value.ToString(null, null);
		}

		public static string ToString(char Value)
		{
			return Value.ToString();
		}

		public static string FromCharArray(char[] Value)
		{
			return new string(Value);
		}

		public static string FromCharAndCount(char Value, int Count)
		{
			return new string(Value, Count);
		}

		public static string FromCharArraySubset(char[] Value, int StartIndex, int Length)
		{
			return new string(Value, StartIndex, Length);
		}

		public static string ToString(short Value)
		{
			return Value.ToString(null, null);
		}

		public static string ToString(int Value)
		{
			return Value.ToString(null, null);
		}

		[CLSCompliant(false)]
		public static string ToString(uint Value)
		{
			return Value.ToString(null, null);
		}

		public static string ToString(long Value)
		{
			return Value.ToString(null, null);
		}

		[CLSCompliant(false)]
		public static string ToString(ulong Value)
		{
			return Value.ToString(null, null);
		}

		public static string ToString(float Value)
		{
			return Conversions.ToString(Value, null);
		}

		public static string ToString(double Value)
		{
			return Conversions.ToString(Value, null);
		}

		public static string ToString(float Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString(null, NumberFormat);
		}

		public static string ToString(double Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G", NumberFormat);
		}

		public static string ToString(DateTime Value)
		{
			long ticks = Value.TimeOfDay.Ticks;
			if (ticks == Value.Ticks || (Value.Year == 1899 && Value.Month == 12 && Value.Day == 30))
			{
				return Value.ToString("T", null);
			}
			if (ticks == 0L)
			{
				return Value.ToString("d", null);
			}
			return Value.ToString("G", null);
		}

		public static string ToString(decimal Value)
		{
			return Conversions.ToString(Value, null);
		}

		public static string ToString(decimal Value, NumberFormatInfo NumberFormat)
		{
			return Value.ToString("G", NumberFormat);
		}

		public static string ToString(object Value)
		{
			if (Value == null)
			{
				return null;
			}
			string text = Value as string;
			if (text != null)
			{
				return text;
			}
			IConvertible convertible = Value as IConvertible;
			if (convertible != null)
			{
				switch (convertible.GetTypeCode())
				{
				case TypeCode.Boolean:
					return Conversions.ToString(convertible.ToBoolean(null));
				case TypeCode.Char:
					return Conversions.ToString(convertible.ToChar(null));
				case TypeCode.SByte:
					return Conversions.ToString((int)convertible.ToSByte(null));
				case TypeCode.Byte:
					return Conversions.ToString(convertible.ToByte(null));
				case TypeCode.Int16:
					return Conversions.ToString((int)convertible.ToInt16(null));
				case TypeCode.UInt16:
					return Conversions.ToString((uint)convertible.ToUInt16(null));
				case TypeCode.Int32:
					return Conversions.ToString(convertible.ToInt32(null));
				case TypeCode.UInt32:
					return Conversions.ToString(convertible.ToUInt32(null));
				case TypeCode.Int64:
					return Conversions.ToString(convertible.ToInt64(null));
				case TypeCode.UInt64:
					return Conversions.ToString(convertible.ToUInt64(null));
				case TypeCode.Single:
					return Conversions.ToString(convertible.ToSingle(null));
				case TypeCode.Double:
					return Conversions.ToString(convertible.ToDouble(null));
				case TypeCode.Decimal:
					return Conversions.ToString(convertible.ToDecimal(null));
				case TypeCode.DateTime:
					return Conversions.ToString(convertible.ToDateTime(null));
				case TypeCode.String:
					return convertible.ToString(null);
				}
			}
			else
			{
				char[] array = Value as char[];
				if (array != null)
				{
					return new string(array);
				}
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(Value),
				"String"
			}));
		}

		public static T ToGenericParameter<T>(object Value)
		{
			if (Value == null)
			{
				return default(T);
			}
			Type typeFromHandle = typeof(T);
			switch (Symbols.GetTypeCode(typeFromHandle))
			{
			case TypeCode.Boolean:
				return (T)((object)Conversions.ToBoolean(Value));
			case TypeCode.Char:
				return (T)((object)Conversions.ToChar(Value));
			case TypeCode.SByte:
				return (T)((object)Conversions.ToSByte(Value));
			case TypeCode.Byte:
				return (T)((object)Conversions.ToByte(Value));
			case TypeCode.Int16:
				return (T)((object)Conversions.ToShort(Value));
			case TypeCode.UInt16:
				return (T)((object)Conversions.ToUShort(Value));
			case TypeCode.Int32:
				return (T)((object)Conversions.ToInteger(Value));
			case TypeCode.UInt32:
				return (T)((object)Conversions.ToUInteger(Value));
			case TypeCode.Int64:
				return (T)((object)Conversions.ToLong(Value));
			case TypeCode.UInt64:
				return (T)((object)Conversions.ToULong(Value));
			case TypeCode.Single:
				return (T)((object)Conversions.ToSingle(Value));
			case TypeCode.Double:
				return (T)((object)Conversions.ToDouble(Value));
			case TypeCode.Decimal:
				return (T)((object)Conversions.ToDecimal(Value));
			case TypeCode.DateTime:
				return (T)((object)Conversions.ToDate(Value));
			case TypeCode.String:
				return (T)((object)Conversions.ToString(Value));
			}
			return (T)((object)Value);
		}

		private static object CastSByteEnum(sbyte Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastByteEnum(byte Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastInt16Enum(short Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastUInt16Enum(ushort Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastInt32Enum(int Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastUInt32Enum(uint Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastInt64Enum(long Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		private static object CastUInt64Enum(ulong Expression, Type TargetType)
		{
			if (Symbols.IsEnum(TargetType))
			{
				return Enum.ToObject(TargetType, Expression);
			}
			return Expression;
		}

		internal static object ForceValueCopy(object Expression, Type TargetType)
		{
			IConvertible convertible = Expression as IConvertible;
			if (convertible == null)
			{
				return Expression;
			}
			switch (convertible.GetTypeCode())
			{
			case TypeCode.Boolean:
				return convertible.ToBoolean(null);
			case TypeCode.Char:
				return convertible.ToChar(null);
			case TypeCode.SByte:
				return Conversions.CastSByteEnum(convertible.ToSByte(null), TargetType);
			case TypeCode.Byte:
				return Conversions.CastByteEnum(convertible.ToByte(null), TargetType);
			case TypeCode.Int16:
				return Conversions.CastInt16Enum(convertible.ToInt16(null), TargetType);
			case TypeCode.UInt16:
				return Conversions.CastUInt16Enum(convertible.ToUInt16(null), TargetType);
			case TypeCode.Int32:
				return Conversions.CastInt32Enum(convertible.ToInt32(null), TargetType);
			case TypeCode.UInt32:
				return Conversions.CastUInt32Enum(convertible.ToUInt32(null), TargetType);
			case TypeCode.Int64:
				return Conversions.CastInt64Enum(convertible.ToInt64(null), TargetType);
			case TypeCode.UInt64:
				return Conversions.CastUInt64Enum(convertible.ToUInt64(null), TargetType);
			case TypeCode.Single:
				return convertible.ToSingle(null);
			case TypeCode.Double:
				return convertible.ToDouble(null);
			case TypeCode.Decimal:
				return convertible.ToDecimal(null);
			case TypeCode.DateTime:
				return convertible.ToDateTime(null);
			}
			return Expression;
		}

		private static object ChangeIntrinsicType(object Expression, Type TargetType)
		{
			switch (Symbols.GetTypeCode(TargetType))
			{
			case TypeCode.Boolean:
				return Conversions.ToBoolean(Expression);
			case TypeCode.Char:
				return Conversions.ToChar(Expression);
			case TypeCode.SByte:
				return Conversions.CastSByteEnum(Conversions.ToSByte(Expression), TargetType);
			case TypeCode.Byte:
				return Conversions.CastByteEnum(Conversions.ToByte(Expression), TargetType);
			case TypeCode.Int16:
				return Conversions.CastInt16Enum(Conversions.ToShort(Expression), TargetType);
			case TypeCode.UInt16:
				return Conversions.CastUInt16Enum(Conversions.ToUShort(Expression), TargetType);
			case TypeCode.Int32:
				return Conversions.CastInt32Enum(Conversions.ToInteger(Expression), TargetType);
			case TypeCode.UInt32:
				return Conversions.CastUInt32Enum(Conversions.ToUInteger(Expression), TargetType);
			case TypeCode.Int64:
				return Conversions.CastInt64Enum(Conversions.ToLong(Expression), TargetType);
			case TypeCode.UInt64:
				return Conversions.CastUInt64Enum(Conversions.ToULong(Expression), TargetType);
			case TypeCode.Single:
				return Conversions.ToSingle(Expression);
			case TypeCode.Double:
				return Conversions.ToDouble(Expression);
			case TypeCode.Decimal:
				return Conversions.ToDecimal(Expression);
			case TypeCode.DateTime:
				return Conversions.ToDate(Expression);
			case TypeCode.String:
				return Conversions.ToString(Expression);
			}
			throw new Exception();
		}

		public static object ChangeType(object Expression, Type TargetType)
		{
			if (TargetType == null)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "TargetType" }));
			}
			if (Expression == null)
			{
				if (Symbols.IsValueType(TargetType))
				{
					ReflectionPermission reflectionPermission = new ReflectionPermission(ReflectionPermissionFlag.NoFlags);
					reflectionPermission.Demand();
					return Activator.CreateInstance(TargetType);
				}
				return null;
			}
			else
			{
				Type type = Expression.GetType();
				if (TargetType.IsByRef)
				{
					TargetType = TargetType.GetElementType();
				}
				if (TargetType == type || Symbols.IsRootObjectType(TargetType))
				{
					return Expression;
				}
				TypeCode typeCode = Symbols.GetTypeCode(TargetType);
				if (Symbols.IsIntrinsicType(typeCode))
				{
					TypeCode typeCode2 = Symbols.GetTypeCode(type);
					if (Symbols.IsIntrinsicType(typeCode2))
					{
						return Conversions.ChangeIntrinsicType(Expression, TargetType);
					}
				}
				if (TargetType.IsInstanceOfType(Expression))
				{
					return Expression;
				}
				if (Symbols.IsCharArrayRankOne(TargetType) && Symbols.IsStringType(type))
				{
					return Conversions.ToCharArrayRankOne((string)Expression);
				}
				if (Symbols.IsStringType(TargetType) && Symbols.IsCharArrayRankOne(type))
				{
					return new string((char[])Expression);
				}
				if (ConversionResolution.ClassifyPredefinedConversion(TargetType, type) == ConversionResolution.ConversionClass.None && (Symbols.IsClassOrValueType(type) || Symbols.IsClassOrValueType(TargetType)) && (!Symbols.IsIntrinsicType(type) || !Symbols.IsIntrinsicType(TargetType)))
				{
					Symbols.Method method = null;
					ConversionResolution.ConversionClass conversionClass = ConversionResolution.ClassifyUserDefinedConversion(TargetType, type, ref method);
					if (method != null)
					{
						Symbols.Container container = new Symbols.Container(method.DeclaringType);
						object obj = container.InvokeMethod(method, new object[] { Expression }, null, BindingFlags.InvokeMethod);
						return Conversions.ChangeType(obj, TargetType);
					}
					if (conversionClass == ConversionResolution.ConversionClass.Ambiguous)
					{
						throw new InvalidCastException(Utils.GetResourceString("AmbiguousCast2", new string[]
						{
							Utils.VBFriendlyName(type),
							Utils.VBFriendlyName(TargetType)
						}));
					}
				}
				throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
				{
					Utils.VBFriendlyName(type),
					Utils.VBFriendlyName(TargetType)
				}));
			}
		}
	}
}

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x02000097 RID: 151
	public static class Convert
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x0001A3D8 File Offset: 0x000193D8
		public static TypeCode GetTypeCode(object value)
		{
			if (value == null)
			{
				return TypeCode.Empty;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				return convertible.GetTypeCode();
			}
			return TypeCode.Object;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001A3FC File Offset: 0x000193FC
		public static bool IsDBNull(object value)
		{
			if (value == global::System.DBNull.Value)
			{
				return true;
			}
			IConvertible convertible = value as IConvertible;
			return convertible != null && convertible.GetTypeCode() == TypeCode.DBNull;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001A428 File Offset: 0x00019428
		public static object ChangeType(object value, TypeCode typeCode)
		{
			return Convert.ChangeType(value, typeCode, Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001A43C File Offset: 0x0001943C
		public static object ChangeType(object value, TypeCode typeCode, IFormatProvider provider)
		{
			if (value == null && (typeCode == TypeCode.Empty || typeCode == TypeCode.String || typeCode == TypeCode.Object))
			{
				return null;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible == null)
			{
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_Empty"));
			case TypeCode.Object:
				return value;
			case TypeCode.DBNull:
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_DBNull"));
			case TypeCode.Boolean:
				return convertible.ToBoolean(provider);
			case TypeCode.Char:
				return convertible.ToChar(provider);
			case TypeCode.SByte:
				return convertible.ToSByte(provider);
			case TypeCode.Byte:
				return convertible.ToByte(provider);
			case TypeCode.Int16:
				return convertible.ToInt16(provider);
			case TypeCode.UInt16:
				return convertible.ToUInt16(provider);
			case TypeCode.Int32:
				return convertible.ToInt32(provider);
			case TypeCode.UInt32:
				return convertible.ToUInt32(provider);
			case TypeCode.Int64:
				return convertible.ToInt64(provider);
			case TypeCode.UInt64:
				return convertible.ToUInt64(provider);
			case TypeCode.Single:
				return convertible.ToSingle(provider);
			case TypeCode.Double:
				return convertible.ToDouble(provider);
			case TypeCode.Decimal:
				return convertible.ToDecimal(provider);
			case TypeCode.DateTime:
				return convertible.ToDateTime(provider);
			case TypeCode.String:
				return convertible.ToString(provider);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_UnknownTypeCode"));
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001A5BC File Offset: 0x000195BC
		internal static object DefaultToType(IConvertible value, Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (value.GetType() == targetType)
			{
				return value;
			}
			if (targetType == Convert.ConvertTypes[3])
			{
				return value.ToBoolean(provider);
			}
			if (targetType == Convert.ConvertTypes[4])
			{
				return value.ToChar(provider);
			}
			if (targetType == Convert.ConvertTypes[5])
			{
				return value.ToSByte(provider);
			}
			if (targetType == Convert.ConvertTypes[6])
			{
				return value.ToByte(provider);
			}
			if (targetType == Convert.ConvertTypes[7])
			{
				return value.ToInt16(provider);
			}
			if (targetType == Convert.ConvertTypes[8])
			{
				return value.ToUInt16(provider);
			}
			if (targetType == Convert.ConvertTypes[9])
			{
				return value.ToInt32(provider);
			}
			if (targetType == Convert.ConvertTypes[10])
			{
				return value.ToUInt32(provider);
			}
			if (targetType == Convert.ConvertTypes[11])
			{
				return value.ToInt64(provider);
			}
			if (targetType == Convert.ConvertTypes[12])
			{
				return value.ToUInt64(provider);
			}
			if (targetType == Convert.ConvertTypes[13])
			{
				return value.ToSingle(provider);
			}
			if (targetType == Convert.ConvertTypes[14])
			{
				return value.ToDouble(provider);
			}
			if (targetType == Convert.ConvertTypes[15])
			{
				return value.ToDecimal(provider);
			}
			if (targetType == Convert.ConvertTypes[16])
			{
				return value.ToDateTime(provider);
			}
			if (targetType == Convert.ConvertTypes[18])
			{
				return value.ToString(provider);
			}
			if (targetType == Convert.ConvertTypes[1])
			{
				return value;
			}
			if (targetType == Convert.EnumType)
			{
				return (Enum)value;
			}
			if (targetType == Convert.ConvertTypes[2])
			{
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_DBNull"));
			}
			if (targetType == Convert.ConvertTypes[0])
			{
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_Empty"));
			}
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				value.GetType().FullName,
				targetType.FullName
			}));
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001A7C6 File Offset: 0x000197C6
		public static object ChangeType(object value, Type conversionType)
		{
			return Convert.ChangeType(value, conversionType, Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001A7DC File Offset: 0x000197DC
		public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
		{
			if (conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			}
			if (value == null)
			{
				if (conversionType.IsValueType)
				{
					throw new InvalidCastException(Environment.GetResourceString("InvalidCast_CannotCastNullToValueType"));
				}
				return null;
			}
			else
			{
				IConvertible convertible = value as IConvertible;
				if (convertible == null)
				{
					if (value.GetType() == conversionType)
					{
						return value;
					}
					throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
				}
				else
				{
					if (conversionType == Convert.ConvertTypes[3])
					{
						return convertible.ToBoolean(provider);
					}
					if (conversionType == Convert.ConvertTypes[4])
					{
						return convertible.ToChar(provider);
					}
					if (conversionType == Convert.ConvertTypes[5])
					{
						return convertible.ToSByte(provider);
					}
					if (conversionType == Convert.ConvertTypes[6])
					{
						return convertible.ToByte(provider);
					}
					if (conversionType == Convert.ConvertTypes[7])
					{
						return convertible.ToInt16(provider);
					}
					if (conversionType == Convert.ConvertTypes[8])
					{
						return convertible.ToUInt16(provider);
					}
					if (conversionType == Convert.ConvertTypes[9])
					{
						return convertible.ToInt32(provider);
					}
					if (conversionType == Convert.ConvertTypes[10])
					{
						return convertible.ToUInt32(provider);
					}
					if (conversionType == Convert.ConvertTypes[11])
					{
						return convertible.ToInt64(provider);
					}
					if (conversionType == Convert.ConvertTypes[12])
					{
						return convertible.ToUInt64(provider);
					}
					if (conversionType == Convert.ConvertTypes[13])
					{
						return convertible.ToSingle(provider);
					}
					if (conversionType == Convert.ConvertTypes[14])
					{
						return convertible.ToDouble(provider);
					}
					if (conversionType == Convert.ConvertTypes[15])
					{
						return convertible.ToDecimal(provider);
					}
					if (conversionType == Convert.ConvertTypes[16])
					{
						return convertible.ToDateTime(provider);
					}
					if (conversionType == Convert.ConvertTypes[18])
					{
						return convertible.ToString(provider);
					}
					if (conversionType == Convert.ConvertTypes[1])
					{
						return value;
					}
					return convertible.ToType(conversionType, provider);
				}
			}
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001A9AA File Offset: 0x000199AA
		public static bool ToBoolean(object value)
		{
			return value != null && ((IConvertible)value).ToBoolean(null);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001A9BD File Offset: 0x000199BD
		public static bool ToBoolean(object value, IFormatProvider provider)
		{
			return value != null && ((IConvertible)value).ToBoolean(provider);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001A9D0 File Offset: 0x000199D0
		public static bool ToBoolean(bool value)
		{
			return value;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001A9D3 File Offset: 0x000199D3
		[CLSCompliant(false)]
		public static bool ToBoolean(sbyte value)
		{
			return value != 0;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001A9DC File Offset: 0x000199DC
		public static bool ToBoolean(char value)
		{
			return ((IConvertible)value).ToBoolean(null);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001A9EA File Offset: 0x000199EA
		public static bool ToBoolean(byte value)
		{
			return value != 0;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001A9F3 File Offset: 0x000199F3
		public static bool ToBoolean(short value)
		{
			return value != 0;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001A9FC File Offset: 0x000199FC
		[CLSCompliant(false)]
		public static bool ToBoolean(ushort value)
		{
			return value != 0;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001AA05 File Offset: 0x00019A05
		public static bool ToBoolean(int value)
		{
			return value != 0;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001AA0E File Offset: 0x00019A0E
		[CLSCompliant(false)]
		public static bool ToBoolean(uint value)
		{
			return value != 0U;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001AA17 File Offset: 0x00019A17
		public static bool ToBoolean(long value)
		{
			return value != 0L;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001AA21 File Offset: 0x00019A21
		[CLSCompliant(false)]
		public static bool ToBoolean(ulong value)
		{
			return value != 0UL;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001AA2B File Offset: 0x00019A2B
		public static bool ToBoolean(string value)
		{
			return value != null && bool.Parse(value);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001AA38 File Offset: 0x00019A38
		public static bool ToBoolean(string value, IFormatProvider provider)
		{
			return value != null && bool.Parse(value);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001AA45 File Offset: 0x00019A45
		public static bool ToBoolean(float value)
		{
			return value != 0f;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001AA52 File Offset: 0x00019A52
		public static bool ToBoolean(double value)
		{
			return value != 0.0;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001AA63 File Offset: 0x00019A63
		public static bool ToBoolean(decimal value)
		{
			return value != 0m;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001AA71 File Offset: 0x00019A71
		public static bool ToBoolean(DateTime value)
		{
			return ((IConvertible)value).ToBoolean(null);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001AA7F File Offset: 0x00019A7F
		public static char ToChar(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToChar(null);
			}
			return '\0';
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001AA92 File Offset: 0x00019A92
		public static char ToChar(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToChar(provider);
			}
			return '\0';
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001AAA5 File Offset: 0x00019AA5
		public static char ToChar(bool value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001AAB3 File Offset: 0x00019AB3
		public static char ToChar(char value)
		{
			return value;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001AAB6 File Offset: 0x00019AB6
		[CLSCompliant(false)]
		public static char ToChar(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001AACE File Offset: 0x00019ACE
		public static char ToChar(byte value)
		{
			return (char)value;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0001AAD1 File Offset: 0x00019AD1
		public static char ToChar(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001AAE9 File Offset: 0x00019AE9
		[CLSCompliant(false)]
		public static char ToChar(ushort value)
		{
			return (char)value;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001AAEC File Offset: 0x00019AEC
		public static char ToChar(int value)
		{
			if (value < 0 || value > 65535)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001AB0C File Offset: 0x00019B0C
		[CLSCompliant(false)]
		public static char ToChar(uint value)
		{
			if (value > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001AB28 File Offset: 0x00019B28
		public static char ToChar(long value)
		{
			if (value < 0L || value > 65535L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001AB4A File Offset: 0x00019B4A
		[CLSCompliant(false)]
		public static char ToChar(ulong value)
		{
			if (value > 65535UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001AB67 File Offset: 0x00019B67
		public static char ToChar(string value)
		{
			return Convert.ToChar(value, null);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001AB70 File Offset: 0x00019B70
		public static char ToChar(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length != 1)
			{
				throw new FormatException(Environment.GetResourceString("Format_NeedSingleChar"));
			}
			return value[0];
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001ABA0 File Offset: 0x00019BA0
		public static char ToChar(float value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001ABAE File Offset: 0x00019BAE
		public static char ToChar(double value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001ABBC File Offset: 0x00019BBC
		public static char ToChar(decimal value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001ABCA File Offset: 0x00019BCA
		public static char ToChar(DateTime value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001ABD8 File Offset: 0x00019BD8
		[CLSCompliant(false)]
		public static sbyte ToSByte(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSByte(null);
			}
			return 0;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001ABEB File Offset: 0x00019BEB
		[CLSCompliant(false)]
		public static sbyte ToSByte(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSByte(provider);
			}
			return 0;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001ABFE File Offset: 0x00019BFE
		[CLSCompliant(false)]
		public static sbyte ToSByte(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001AC06 File Offset: 0x00019C06
		[CLSCompliant(false)]
		public static sbyte ToSByte(sbyte value)
		{
			return value;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001AC09 File Offset: 0x00019C09
		[CLSCompliant(false)]
		public static sbyte ToSByte(char value)
		{
			if (value > '\u007f')
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001AC22 File Offset: 0x00019C22
		[CLSCompliant(false)]
		public static sbyte ToSByte(byte value)
		{
			if (value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001AC3B File Offset: 0x00019C3B
		[CLSCompliant(false)]
		public static sbyte ToSByte(short value)
		{
			if (value < -128 || value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001AC59 File Offset: 0x00019C59
		[CLSCompliant(false)]
		public static sbyte ToSByte(ushort value)
		{
			if (value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001AC72 File Offset: 0x00019C72
		[CLSCompliant(false)]
		public static sbyte ToSByte(int value)
		{
			if (value < -128 || value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001AC90 File Offset: 0x00019C90
		[CLSCompliant(false)]
		public static sbyte ToSByte(uint value)
		{
			if ((ulong)value > 127UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001ACAB File Offset: 0x00019CAB
		[CLSCompliant(false)]
		public static sbyte ToSByte(long value)
		{
			if (value < -128L || value > 127L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001ACCB File Offset: 0x00019CCB
		[CLSCompliant(false)]
		public static sbyte ToSByte(ulong value)
		{
			if (value > 127UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001ACE5 File Offset: 0x00019CE5
		[CLSCompliant(false)]
		public static sbyte ToSByte(float value)
		{
			return Convert.ToSByte((double)value);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001ACEE File Offset: 0x00019CEE
		[CLSCompliant(false)]
		public static sbyte ToSByte(double value)
		{
			return Convert.ToSByte(Convert.ToInt32(value));
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001ACFB File Offset: 0x00019CFB
		[CLSCompliant(false)]
		public static sbyte ToSByte(decimal value)
		{
			return decimal.ToSByte(decimal.Round(value, 0));
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001AD09 File Offset: 0x00019D09
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return sbyte.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001AD1B File Offset: 0x00019D1B
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value, IFormatProvider provider)
		{
			return sbyte.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001AD25 File Offset: 0x00019D25
		[CLSCompliant(false)]
		public static sbyte ToSByte(DateTime value)
		{
			return ((IConvertible)value).ToSByte(null);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001AD33 File Offset: 0x00019D33
		public static byte ToByte(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToByte(null);
			}
			return 0;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001AD46 File Offset: 0x00019D46
		public static byte ToByte(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToByte(provider);
			}
			return 0;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001AD59 File Offset: 0x00019D59
		public static byte ToByte(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001AD61 File Offset: 0x00019D61
		public static byte ToByte(byte value)
		{
			return value;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001AD64 File Offset: 0x00019D64
		public static byte ToByte(char value)
		{
			if (value > 'ÿ')
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001AD80 File Offset: 0x00019D80
		[CLSCompliant(false)]
		public static byte ToByte(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001AD98 File Offset: 0x00019D98
		public static byte ToByte(short value)
		{
			if (value < 0 || value > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001ADB8 File Offset: 0x00019DB8
		[CLSCompliant(false)]
		public static byte ToByte(ushort value)
		{
			if (value > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001ADD4 File Offset: 0x00019DD4
		public static byte ToByte(int value)
		{
			if (value < 0 || value > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001ADF4 File Offset: 0x00019DF4
		[CLSCompliant(false)]
		public static byte ToByte(uint value)
		{
			if (value > 255U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001AE10 File Offset: 0x00019E10
		public static byte ToByte(long value)
		{
			if (value < 0L || value > 255L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001AE32 File Offset: 0x00019E32
		[CLSCompliant(false)]
		public static byte ToByte(ulong value)
		{
			if (value > 255UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001AE4F File Offset: 0x00019E4F
		public static byte ToByte(float value)
		{
			return Convert.ToByte((double)value);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001AE58 File Offset: 0x00019E58
		public static byte ToByte(double value)
		{
			return Convert.ToByte(Convert.ToInt32(value));
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001AE65 File Offset: 0x00019E65
		public static byte ToByte(decimal value)
		{
			return decimal.ToByte(decimal.Round(value, 0));
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001AE73 File Offset: 0x00019E73
		public static byte ToByte(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return byte.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001AE85 File Offset: 0x00019E85
		public static byte ToByte(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return byte.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001AE94 File Offset: 0x00019E94
		public static byte ToByte(DateTime value)
		{
			return ((IConvertible)value).ToByte(null);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001AEA2 File Offset: 0x00019EA2
		public static short ToInt16(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt16(null);
			}
			return 0;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001AEB5 File Offset: 0x00019EB5
		public static short ToInt16(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt16(provider);
			}
			return 0;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001AEC8 File Offset: 0x00019EC8
		public static short ToInt16(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001AED0 File Offset: 0x00019ED0
		public static short ToInt16(char value)
		{
			if (value > '翿')
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001AEEC File Offset: 0x00019EEC
		[CLSCompliant(false)]
		public static short ToInt16(sbyte value)
		{
			return (short)value;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001AEEF File Offset: 0x00019EEF
		public static short ToInt16(byte value)
		{
			return (short)value;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001AEF2 File Offset: 0x00019EF2
		[CLSCompliant(false)]
		public static short ToInt16(ushort value)
		{
			if (value > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001AF0E File Offset: 0x00019F0E
		public static short ToInt16(int value)
		{
			if (value < -32768 || value > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001AF32 File Offset: 0x00019F32
		[CLSCompliant(false)]
		public static short ToInt16(uint value)
		{
			if ((ulong)value > 32767UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001AF50 File Offset: 0x00019F50
		public static short ToInt16(short value)
		{
			return value;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001AF53 File Offset: 0x00019F53
		public static short ToInt16(long value)
		{
			if (value < -32768L || value > 32767L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001AF79 File Offset: 0x00019F79
		[CLSCompliant(false)]
		public static short ToInt16(ulong value)
		{
			if (value > 32767UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001AF96 File Offset: 0x00019F96
		public static short ToInt16(float value)
		{
			return Convert.ToInt16((double)value);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001AF9F File Offset: 0x00019F9F
		public static short ToInt16(double value)
		{
			return Convert.ToInt16(Convert.ToInt32(value));
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001AFAC File Offset: 0x00019FAC
		public static short ToInt16(decimal value)
		{
			return decimal.ToInt16(decimal.Round(value, 0));
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0001AFBA File Offset: 0x00019FBA
		public static short ToInt16(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return short.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0001AFCC File Offset: 0x00019FCC
		public static short ToInt16(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return short.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0001AFDB File Offset: 0x00019FDB
		public static short ToInt16(DateTime value)
		{
			return ((IConvertible)value).ToInt16(null);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001AFE9 File Offset: 0x00019FE9
		[CLSCompliant(false)]
		public static ushort ToUInt16(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt16(null);
			}
			return 0;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001AFFC File Offset: 0x00019FFC
		[CLSCompliant(false)]
		public static ushort ToUInt16(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt16(provider);
			}
			return 0;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001B00F File Offset: 0x0001A00F
		[CLSCompliant(false)]
		public static ushort ToUInt16(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001B017 File Offset: 0x0001A017
		[CLSCompliant(false)]
		public static ushort ToUInt16(char value)
		{
			return (ushort)value;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0001B01A File Offset: 0x0001A01A
		[CLSCompliant(false)]
		public static ushort ToUInt16(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001B032 File Offset: 0x0001A032
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte value)
		{
			return (ushort)value;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001B035 File Offset: 0x0001A035
		[CLSCompliant(false)]
		public static ushort ToUInt16(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0001B04D File Offset: 0x0001A04D
		[CLSCompliant(false)]
		public static ushort ToUInt16(int value)
		{
			if (value < 0 || value > 65535)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0001B06D File Offset: 0x0001A06D
		[CLSCompliant(false)]
		public static ushort ToUInt16(ushort value)
		{
			return value;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001B070 File Offset: 0x0001A070
		[CLSCompliant(false)]
		public static ushort ToUInt16(uint value)
		{
			if (value > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001B08C File Offset: 0x0001A08C
		[CLSCompliant(false)]
		public static ushort ToUInt16(long value)
		{
			if (value < 0L || value > 65535L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001B0AE File Offset: 0x0001A0AE
		[CLSCompliant(false)]
		public static ushort ToUInt16(ulong value)
		{
			if (value > 65535UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001B0CB File Offset: 0x0001A0CB
		[CLSCompliant(false)]
		public static ushort ToUInt16(float value)
		{
			return Convert.ToUInt16((double)value);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001B0D4 File Offset: 0x0001A0D4
		[CLSCompliant(false)]
		public static ushort ToUInt16(double value)
		{
			return Convert.ToUInt16(Convert.ToInt32(value));
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001B0E1 File Offset: 0x0001A0E1
		[CLSCompliant(false)]
		public static ushort ToUInt16(decimal value)
		{
			return decimal.ToUInt16(decimal.Round(value, 0));
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001B0EF File Offset: 0x0001A0EF
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return ushort.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0001B101 File Offset: 0x0001A101
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ushort.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001B110 File Offset: 0x0001A110
		[CLSCompliant(false)]
		public static ushort ToUInt16(DateTime value)
		{
			return ((IConvertible)value).ToUInt16(null);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001B11E File Offset: 0x0001A11E
		public static int ToInt32(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt32(null);
			}
			return 0;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001B131 File Offset: 0x0001A131
		public static int ToInt32(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt32(provider);
			}
			return 0;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001B144 File Offset: 0x0001A144
		public static int ToInt32(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001B14C File Offset: 0x0001A14C
		public static int ToInt32(char value)
		{
			return (int)value;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001B14F File Offset: 0x0001A14F
		[CLSCompliant(false)]
		public static int ToInt32(sbyte value)
		{
			return (int)value;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001B152 File Offset: 0x0001A152
		public static int ToInt32(byte value)
		{
			return (int)value;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001B155 File Offset: 0x0001A155
		public static int ToInt32(short value)
		{
			return (int)value;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001B158 File Offset: 0x0001A158
		[CLSCompliant(false)]
		public static int ToInt32(ushort value)
		{
			return (int)value;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001B15B File Offset: 0x0001A15B
		[CLSCompliant(false)]
		public static int ToInt32(uint value)
		{
			if (value > 2147483647U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return (int)value;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001B176 File Offset: 0x0001A176
		public static int ToInt32(int value)
		{
			return value;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001B179 File Offset: 0x0001A179
		public static int ToInt32(long value)
		{
			if (value < -2147483648L || value > 2147483647L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return (int)value;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001B19F File Offset: 0x0001A19F
		[CLSCompliant(false)]
		public static int ToInt32(ulong value)
		{
			if (value > 2147483647UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return (int)value;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001B1BC File Offset: 0x0001A1BC
		public static int ToInt32(float value)
		{
			return Convert.ToInt32((double)value);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001B1C8 File Offset: 0x0001A1C8
		public static int ToInt32(double value)
		{
			if (value >= 0.0)
			{
				if (value < 2147483647.5)
				{
					int num = (int)value;
					double num2 = value - (double)num;
					if (num2 > 0.5 || (num2 == 0.5 && (num & 1) != 0))
					{
						num++;
					}
					return num;
				}
			}
			else if (value >= -2147483648.5)
			{
				int num3 = (int)value;
				double num4 = value - (double)num3;
				if (num4 < -0.5 || (num4 == -0.5 && (num3 & 1) != 0))
				{
					num3--;
				}
				return num3;
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001B25E File Offset: 0x0001A25E
		public static int ToInt32(decimal value)
		{
			return decimal.FCallToInt32(value);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001B266 File Offset: 0x0001A266
		public static int ToInt32(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return int.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001B278 File Offset: 0x0001A278
		public static int ToInt32(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return int.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001B287 File Offset: 0x0001A287
		public static int ToInt32(DateTime value)
		{
			return ((IConvertible)value).ToInt32(null);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001B295 File Offset: 0x0001A295
		[CLSCompliant(false)]
		public static uint ToUInt32(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt32(null);
			}
			return 0U;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001B2A8 File Offset: 0x0001A2A8
		[CLSCompliant(false)]
		public static uint ToUInt32(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt32(provider);
			}
			return 0U;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001B2BB File Offset: 0x0001A2BB
		[CLSCompliant(false)]
		public static uint ToUInt32(bool value)
		{
			if (!value)
			{
				return 0U;
			}
			return 1U;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001B2C3 File Offset: 0x0001A2C3
		[CLSCompliant(false)]
		public static uint ToUInt32(char value)
		{
			return (uint)value;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001B2C6 File Offset: 0x0001A2C6
		[CLSCompliant(false)]
		public static uint ToUInt32(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001B2DD File Offset: 0x0001A2DD
		[CLSCompliant(false)]
		public static uint ToUInt32(byte value)
		{
			return (uint)value;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001B2E0 File Offset: 0x0001A2E0
		[CLSCompliant(false)]
		public static uint ToUInt32(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001B2F7 File Offset: 0x0001A2F7
		[CLSCompliant(false)]
		public static uint ToUInt32(ushort value)
		{
			return (uint)value;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001B2FA File Offset: 0x0001A2FA
		[CLSCompliant(false)]
		public static uint ToUInt32(int value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001B311 File Offset: 0x0001A311
		[CLSCompliant(false)]
		public static uint ToUInt32(uint value)
		{
			return value;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001B314 File Offset: 0x0001A314
		[CLSCompliant(false)]
		public static uint ToUInt32(long value)
		{
			if (value < 0L || value > (long)((ulong)(-1)))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001B332 File Offset: 0x0001A332
		[CLSCompliant(false)]
		public static uint ToUInt32(ulong value)
		{
			if (value > (ulong)(-1))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001B34B File Offset: 0x0001A34B
		[CLSCompliant(false)]
		public static uint ToUInt32(float value)
		{
			return Convert.ToUInt32((double)value);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001B354 File Offset: 0x0001A354
		[CLSCompliant(false)]
		public static uint ToUInt32(double value)
		{
			if (value >= -0.5 && value < 4294967295.5)
			{
				uint num = (uint)value;
				double num2 = value - num;
				if (num2 > 0.5 || (num2 == 0.5 && (num & 1U) != 0U))
				{
					num += 1U;
				}
				return num;
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001B3B4 File Offset: 0x0001A3B4
		[CLSCompliant(false)]
		public static uint ToUInt32(decimal value)
		{
			return decimal.ToUInt32(decimal.Round(value, 0));
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001B3C2 File Offset: 0x0001A3C2
		[CLSCompliant(false)]
		public static uint ToUInt32(string value)
		{
			if (value == null)
			{
				return 0U;
			}
			return uint.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001B3D4 File Offset: 0x0001A3D4
		[CLSCompliant(false)]
		public static uint ToUInt32(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0U;
			}
			return uint.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001B3E3 File Offset: 0x0001A3E3
		[CLSCompliant(false)]
		public static uint ToUInt32(DateTime value)
		{
			return ((IConvertible)value).ToUInt32(null);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001B3F1 File Offset: 0x0001A3F1
		public static long ToInt64(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt64(null);
			}
			return 0L;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001B405 File Offset: 0x0001A405
		public static long ToInt64(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt64(provider);
			}
			return 0L;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001B419 File Offset: 0x0001A419
		public static long ToInt64(bool value)
		{
			return value ? 1L : 0L;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001B423 File Offset: 0x0001A423
		public static long ToInt64(char value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001B427 File Offset: 0x0001A427
		[CLSCompliant(false)]
		public static long ToInt64(sbyte value)
		{
			return (long)value;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001B42B File Offset: 0x0001A42B
		public static long ToInt64(byte value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001B42F File Offset: 0x0001A42F
		public static long ToInt64(short value)
		{
			return (long)value;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001B433 File Offset: 0x0001A433
		[CLSCompliant(false)]
		public static long ToInt64(ushort value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001B437 File Offset: 0x0001A437
		public static long ToInt64(int value)
		{
			return (long)value;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001B43B File Offset: 0x0001A43B
		[CLSCompliant(false)]
		public static long ToInt64(uint value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001B43F File Offset: 0x0001A43F
		[CLSCompliant(false)]
		public static long ToInt64(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
			}
			return (long)value;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001B45E File Offset: 0x0001A45E
		public static long ToInt64(long value)
		{
			return value;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001B461 File Offset: 0x0001A461
		public static long ToInt64(float value)
		{
			return Convert.ToInt64((double)value);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001B46A File Offset: 0x0001A46A
		public static long ToInt64(double value)
		{
			return checked((long)Math.Round(value));
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001B473 File Offset: 0x0001A473
		public static long ToInt64(decimal value)
		{
			return decimal.ToInt64(decimal.Round(value, 0));
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001B481 File Offset: 0x0001A481
		public static long ToInt64(string value)
		{
			if (value == null)
			{
				return 0L;
			}
			return long.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001B494 File Offset: 0x0001A494
		public static long ToInt64(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0L;
			}
			return long.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001B4A4 File Offset: 0x0001A4A4
		public static long ToInt64(DateTime value)
		{
			return ((IConvertible)value).ToInt64(null);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001B4B2 File Offset: 0x0001A4B2
		[CLSCompliant(false)]
		public static ulong ToUInt64(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt64(null);
			}
			return 0UL;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001B4C6 File Offset: 0x0001A4C6
		[CLSCompliant(false)]
		public static ulong ToUInt64(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt64(provider);
			}
			return 0UL;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001B4DA File Offset: 0x0001A4DA
		[CLSCompliant(false)]
		public static ulong ToUInt64(bool value)
		{
			if (!value)
			{
				return 0UL;
			}
			return 1UL;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001B4E4 File Offset: 0x0001A4E4
		[CLSCompliant(false)]
		public static ulong ToUInt64(char value)
		{
			return (ulong)value;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001B4E8 File Offset: 0x0001A4E8
		[CLSCompliant(false)]
		public static ulong ToUInt64(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)((long)value);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001B500 File Offset: 0x0001A500
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte value)
		{
			return (ulong)value;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001B504 File Offset: 0x0001A504
		[CLSCompliant(false)]
		public static ulong ToUInt64(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)((long)value);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001B51C File Offset: 0x0001A51C
		[CLSCompliant(false)]
		public static ulong ToUInt64(ushort value)
		{
			return (ulong)value;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001B520 File Offset: 0x0001A520
		[CLSCompliant(false)]
		public static ulong ToUInt64(int value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)((long)value);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001B538 File Offset: 0x0001A538
		[CLSCompliant(false)]
		public static ulong ToUInt64(uint value)
		{
			return (ulong)value;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0001B53C File Offset: 0x0001A53C
		[CLSCompliant(false)]
		public static ulong ToUInt64(long value)
		{
			if (value < 0L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)value;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001B554 File Offset: 0x0001A554
		[CLSCompliant(false)]
		public static ulong ToUInt64(ulong value)
		{
			return value;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0001B557 File Offset: 0x0001A557
		[CLSCompliant(false)]
		public static ulong ToUInt64(float value)
		{
			return Convert.ToUInt64((double)value);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001B560 File Offset: 0x0001A560
		[CLSCompliant(false)]
		public static ulong ToUInt64(double value)
		{
			return checked((ulong)Math.Round(value));
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001B569 File Offset: 0x0001A569
		[CLSCompliant(false)]
		public static ulong ToUInt64(decimal value)
		{
			return decimal.ToUInt64(decimal.Round(value, 0));
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001B577 File Offset: 0x0001A577
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ulong.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001B58A File Offset: 0x0001A58A
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ulong.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001B59A File Offset: 0x0001A59A
		[CLSCompliant(false)]
		public static ulong ToUInt64(DateTime value)
		{
			return ((IConvertible)value).ToUInt64(null);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001B5A8 File Offset: 0x0001A5A8
		public static float ToSingle(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSingle(null);
			}
			return 0f;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001B5BF File Offset: 0x0001A5BF
		public static float ToSingle(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSingle(provider);
			}
			return 0f;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001B5D6 File Offset: 0x0001A5D6
		[CLSCompliant(false)]
		public static float ToSingle(sbyte value)
		{
			return (float)value;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001B5DA File Offset: 0x0001A5DA
		public static float ToSingle(byte value)
		{
			return (float)value;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001B5DE File Offset: 0x0001A5DE
		public static float ToSingle(char value)
		{
			return ((IConvertible)value).ToSingle(null);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001B5EC File Offset: 0x0001A5EC
		public static float ToSingle(short value)
		{
			return (float)value;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001B5F0 File Offset: 0x0001A5F0
		[CLSCompliant(false)]
		public static float ToSingle(ushort value)
		{
			return (float)value;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001B5F4 File Offset: 0x0001A5F4
		public static float ToSingle(int value)
		{
			return (float)value;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001B5F8 File Offset: 0x0001A5F8
		[CLSCompliant(false)]
		public static float ToSingle(uint value)
		{
			return value;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001B5FD File Offset: 0x0001A5FD
		public static float ToSingle(long value)
		{
			return (float)value;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001B601 File Offset: 0x0001A601
		[CLSCompliant(false)]
		public static float ToSingle(ulong value)
		{
			return value;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0001B606 File Offset: 0x0001A606
		public static float ToSingle(float value)
		{
			return value;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001B609 File Offset: 0x0001A609
		public static float ToSingle(double value)
		{
			return (float)value;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001B60D File Offset: 0x0001A60D
		public static float ToSingle(decimal value)
		{
			return (float)value;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001B616 File Offset: 0x0001A616
		public static float ToSingle(string value)
		{
			if (value == null)
			{
				return 0f;
			}
			return float.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0001B62C File Offset: 0x0001A62C
		public static float ToSingle(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0f;
			}
			return float.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001B643 File Offset: 0x0001A643
		public static float ToSingle(bool value)
		{
			return (float)(value ? 1 : 0);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001B64D File Offset: 0x0001A64D
		public static float ToSingle(DateTime value)
		{
			return ((IConvertible)value).ToSingle(null);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001B65B File Offset: 0x0001A65B
		public static double ToDouble(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDouble(null);
			}
			return 0.0;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0001B676 File Offset: 0x0001A676
		public static double ToDouble(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDouble(provider);
			}
			return 0.0;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0001B691 File Offset: 0x0001A691
		[CLSCompliant(false)]
		public static double ToDouble(sbyte value)
		{
			return (double)value;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001B695 File Offset: 0x0001A695
		public static double ToDouble(byte value)
		{
			return (double)value;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001B699 File Offset: 0x0001A699
		public static double ToDouble(short value)
		{
			return (double)value;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001B69D File Offset: 0x0001A69D
		public static double ToDouble(char value)
		{
			return ((IConvertible)value).ToDouble(null);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001B6AB File Offset: 0x0001A6AB
		[CLSCompliant(false)]
		public static double ToDouble(ushort value)
		{
			return (double)value;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001B6AF File Offset: 0x0001A6AF
		public static double ToDouble(int value)
		{
			return (double)value;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001B6B3 File Offset: 0x0001A6B3
		[CLSCompliant(false)]
		public static double ToDouble(uint value)
		{
			return value;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001B6B8 File Offset: 0x0001A6B8
		public static double ToDouble(long value)
		{
			return (double)value;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001B6BC File Offset: 0x0001A6BC
		[CLSCompliant(false)]
		public static double ToDouble(ulong value)
		{
			return value;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001B6C1 File Offset: 0x0001A6C1
		public static double ToDouble(float value)
		{
			return (double)value;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001B6C5 File Offset: 0x0001A6C5
		public static double ToDouble(double value)
		{
			return value;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001B6C8 File Offset: 0x0001A6C8
		public static double ToDouble(decimal value)
		{
			return (double)value;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001B6D1 File Offset: 0x0001A6D1
		public static double ToDouble(string value)
		{
			if (value == null)
			{
				return 0.0;
			}
			return double.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001B6EB File Offset: 0x0001A6EB
		public static double ToDouble(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0.0;
			}
			return double.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001B706 File Offset: 0x0001A706
		public static double ToDouble(bool value)
		{
			return (double)(value ? 1 : 0);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001B710 File Offset: 0x0001A710
		public static double ToDouble(DateTime value)
		{
			return ((IConvertible)value).ToDouble(null);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001B71E File Offset: 0x0001A71E
		public static decimal ToDecimal(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDecimal(null);
			}
			return 0m;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001B736 File Offset: 0x0001A736
		public static decimal ToDecimal(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDecimal(provider);
			}
			return 0m;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001B74E File Offset: 0x0001A74E
		[CLSCompliant(false)]
		public static decimal ToDecimal(sbyte value)
		{
			return value;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001B756 File Offset: 0x0001A756
		public static decimal ToDecimal(byte value)
		{
			return value;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001B75E File Offset: 0x0001A75E
		public static decimal ToDecimal(char value)
		{
			return ((IConvertible)value).ToDecimal(null);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001B76C File Offset: 0x0001A76C
		public static decimal ToDecimal(short value)
		{
			return value;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001B774 File Offset: 0x0001A774
		[CLSCompliant(false)]
		public static decimal ToDecimal(ushort value)
		{
			return value;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001B77C File Offset: 0x0001A77C
		public static decimal ToDecimal(int value)
		{
			return value;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001B784 File Offset: 0x0001A784
		[CLSCompliant(false)]
		public static decimal ToDecimal(uint value)
		{
			return value;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001B78C File Offset: 0x0001A78C
		public static decimal ToDecimal(long value)
		{
			return value;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001B794 File Offset: 0x0001A794
		[CLSCompliant(false)]
		public static decimal ToDecimal(ulong value)
		{
			return value;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001B79C File Offset: 0x0001A79C
		public static decimal ToDecimal(float value)
		{
			return (decimal)value;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001B7A5 File Offset: 0x0001A7A5
		public static decimal ToDecimal(double value)
		{
			return (decimal)value;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001B7AE File Offset: 0x0001A7AE
		public static decimal ToDecimal(string value)
		{
			if (value == null)
			{
				return 0m;
			}
			return decimal.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001B7C5 File Offset: 0x0001A7C5
		public static decimal ToDecimal(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0m;
			}
			return decimal.Parse(value, NumberStyles.Number, provider);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001B7DA File Offset: 0x0001A7DA
		public static decimal ToDecimal(decimal value)
		{
			return value;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001B7DD File Offset: 0x0001A7DD
		public static decimal ToDecimal(bool value)
		{
			return value ? 1 : 0;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001B7EB File Offset: 0x0001A7EB
		public static decimal ToDecimal(DateTime value)
		{
			return ((IConvertible)value).ToDecimal(null);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001B7F9 File Offset: 0x0001A7F9
		public static DateTime ToDateTime(DateTime value)
		{
			return value;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001B7FC File Offset: 0x0001A7FC
		public static DateTime ToDateTime(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDateTime(null);
			}
			return DateTime.MinValue;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001B813 File Offset: 0x0001A813
		public static DateTime ToDateTime(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDateTime(provider);
			}
			return DateTime.MinValue;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001B82A File Offset: 0x0001A82A
		public static DateTime ToDateTime(string value)
		{
			if (value == null)
			{
				return new DateTime(0L);
			}
			return DateTime.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001B842 File Offset: 0x0001A842
		public static DateTime ToDateTime(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return new DateTime(0L);
			}
			return DateTime.Parse(value, provider);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001B856 File Offset: 0x0001A856
		[CLSCompliant(false)]
		public static DateTime ToDateTime(sbyte value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001B864 File Offset: 0x0001A864
		public static DateTime ToDateTime(byte value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001B872 File Offset: 0x0001A872
		public static DateTime ToDateTime(short value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001B880 File Offset: 0x0001A880
		[CLSCompliant(false)]
		public static DateTime ToDateTime(ushort value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001B88E File Offset: 0x0001A88E
		public static DateTime ToDateTime(int value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001B89C File Offset: 0x0001A89C
		[CLSCompliant(false)]
		public static DateTime ToDateTime(uint value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001B8AA File Offset: 0x0001A8AA
		public static DateTime ToDateTime(long value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001B8B8 File Offset: 0x0001A8B8
		[CLSCompliant(false)]
		public static DateTime ToDateTime(ulong value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001B8C6 File Offset: 0x0001A8C6
		public static DateTime ToDateTime(bool value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001B8D4 File Offset: 0x0001A8D4
		public static DateTime ToDateTime(char value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001B8E2 File Offset: 0x0001A8E2
		public static DateTime ToDateTime(float value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001B8F0 File Offset: 0x0001A8F0
		public static DateTime ToDateTime(double value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001B8FE File Offset: 0x0001A8FE
		public static DateTime ToDateTime(decimal value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001B90C File Offset: 0x0001A90C
		public static string ToString(object value)
		{
			return Convert.ToString(value, null);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001B918 File Offset: 0x0001A918
		public static string ToString(object value, IFormatProvider provider)
		{
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				return convertible.ToString(provider);
			}
			IFormattable formattable = value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(null, provider);
			}
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001B959 File Offset: 0x0001A959
		public static string ToString(bool value)
		{
			return value.ToString();
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001B962 File Offset: 0x0001A962
		public static string ToString(bool value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001B96C File Offset: 0x0001A96C
		public static string ToString(char value)
		{
			return char.ToString(value);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001B974 File Offset: 0x0001A974
		public static string ToString(char value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001B97E File Offset: 0x0001A97E
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001B98C File Offset: 0x0001A98C
		[CLSCompliant(false)]
		public static string ToString(sbyte value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001B996 File Offset: 0x0001A996
		public static string ToString(byte value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001B9A4 File Offset: 0x0001A9A4
		public static string ToString(byte value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001B9AE File Offset: 0x0001A9AE
		public static string ToString(short value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001B9BC File Offset: 0x0001A9BC
		public static string ToString(short value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001B9C6 File Offset: 0x0001A9C6
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001B9D4 File Offset: 0x0001A9D4
		[CLSCompliant(false)]
		public static string ToString(ushort value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001B9DE File Offset: 0x0001A9DE
		public static string ToString(int value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001B9EC File Offset: 0x0001A9EC
		public static string ToString(int value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001B9F6 File Offset: 0x0001A9F6
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001BA04 File Offset: 0x0001AA04
		[CLSCompliant(false)]
		public static string ToString(uint value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001BA0E File Offset: 0x0001AA0E
		public static string ToString(long value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001BA1C File Offset: 0x0001AA1C
		public static string ToString(long value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001BA26 File Offset: 0x0001AA26
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001BA34 File Offset: 0x0001AA34
		[CLSCompliant(false)]
		public static string ToString(ulong value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001BA3E File Offset: 0x0001AA3E
		public static string ToString(float value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001BA4C File Offset: 0x0001AA4C
		public static string ToString(float value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001BA56 File Offset: 0x0001AA56
		public static string ToString(double value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001BA64 File Offset: 0x0001AA64
		public static string ToString(double value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001BA6E File Offset: 0x0001AA6E
		public static string ToString(decimal value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001BA7C File Offset: 0x0001AA7C
		public static string ToString(decimal value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001BA86 File Offset: 0x0001AA86
		public static string ToString(DateTime value)
		{
			return value.ToString();
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001BA95 File Offset: 0x0001AA95
		public static string ToString(DateTime value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001BA9F File Offset: 0x0001AA9F
		public static string ToString(string value)
		{
			return value;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001BAA2 File Offset: 0x0001AAA2
		public static string ToString(string value, IFormatProvider provider)
		{
			return value;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001BAA8 File Offset: 0x0001AAA8
		public static byte ToByte(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 4608);
			if (num < 0 || num > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)num;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001BB04 File Offset: 0x0001AB04
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 5120);
			if (fromBase != 10 && num <= 255)
			{
				return (sbyte)num;
			}
			if (num < -128 || num > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)num;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001BB6C File Offset: 0x0001AB6C
		public static short ToInt16(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 6144);
			if (fromBase != 10 && num <= 65535)
			{
				return (short)num;
			}
			if (num < -32768 || num > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)num;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001BBDC File Offset: 0x0001ABDC
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 4608);
			if (num < 0 || num > 65535)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)num;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001BC36 File Offset: 0x0001AC36
		public static int ToInt32(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.StringToInt(value, fromBase, 4096);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001BC66 File Offset: 0x0001AC66
		[CLSCompliant(false)]
		public static uint ToUInt32(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return (uint)ParseNumbers.StringToInt(value, fromBase, 4608);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001BC96 File Offset: 0x0001AC96
		public static long ToInt64(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.StringToLong(value, fromBase, 4096);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001BCC6 File Offset: 0x0001ACC6
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return (ulong)ParseNumbers.StringToLong(value, fromBase, 4608);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001BCF6 File Offset: 0x0001ACF6
		public static string ToString(byte value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.IntToString((int)value, toBase, -1, ' ', 64);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001BD26 File Offset: 0x0001AD26
		public static string ToString(short value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.IntToString((int)value, toBase, -1, ' ', 128);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001BD59 File Offset: 0x0001AD59
		public static string ToString(int value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.IntToString(value, toBase, -1, ' ', 0);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001BD88 File Offset: 0x0001AD88
		public static string ToString(long value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.LongToString(value, toBase, -1, ' ', 0);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001BDB7 File Offset: 0x0001ADB7
		public static string ToBase64String(byte[] inArray)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			return Convert.ToBase64String(inArray, 0, inArray.Length, Base64FormattingOptions.None);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001BDD2 File Offset: 0x0001ADD2
		[ComVisible(false)]
		public static string ToBase64String(byte[] inArray, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			return Convert.ToBase64String(inArray, 0, inArray.Length, options);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001BDED File Offset: 0x0001ADED
		public static string ToBase64String(byte[] inArray, int offset, int length)
		{
			return Convert.ToBase64String(inArray, offset, length, Base64FormattingOptions.None);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001BDF8 File Offset: 0x0001ADF8
		[ComVisible(false)]
		public unsafe static string ToBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			int num = inArray.Length;
			if (offset > num - length)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
			}
			if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)options }));
			}
			if (num == 0)
			{
				return string.Empty;
			}
			bool flag = options == Base64FormattingOptions.InsertLineBreaks;
			int num2 = Convert.CalculateOutputLength(length, flag);
			string stringForStringBuilder = string.GetStringForStringBuilder(string.Empty, num2);
			fixed (char* ptr = stringForStringBuilder)
			{
				fixed (byte* ptr2 = inArray)
				{
					int num3 = Convert.ConvertToBase64Array(ptr, ptr2, offset, length, flag);
					stringForStringBuilder.SetLength(num3);
					return stringForStringBuilder;
				}
			}
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001BEFD File Offset: 0x0001AEFD
		public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
		{
			return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, Base64FormattingOptions.None);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001BF0C File Offset: 0x0001AF0C
		[ComVisible(false)]
		public unsafe static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (outArray == null)
			{
				throw new ArgumentNullException("outArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (offsetIn < 0)
			{
				throw new ArgumentOutOfRangeException("offsetIn", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (offsetOut < 0)
			{
				throw new ArgumentOutOfRangeException("offsetOut", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)options }));
			}
			int num = inArray.Length;
			if (offsetIn > num - length)
			{
				throw new ArgumentOutOfRangeException("offsetIn", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
			}
			if (num == 0)
			{
				return 0;
			}
			bool flag = options == Base64FormattingOptions.InsertLineBreaks;
			int num2 = outArray.Length;
			int num3 = Convert.CalculateOutputLength(length, flag);
			if (offsetOut > num2 - num3)
			{
				throw new ArgumentOutOfRangeException("offsetOut", Environment.GetResourceString("ArgumentOutOfRange_OffsetOut"));
			}
			int num4;
			fixed (char* ptr = &outArray[offsetOut])
			{
				fixed (byte* ptr2 = inArray)
				{
					num4 = Convert.ConvertToBase64Array(ptr, ptr2, offsetIn, length, flag);
				}
			}
			return num4;
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001C044 File Offset: 0x0001B044
		private unsafe static int ConvertToBase64Array(char* outChars, byte* inData, int offset, int length, bool insertLineBreaks)
		{
			int num = length % 3;
			int num2 = offset + (length - num);
			int num3 = 0;
			int num4 = 0;
			fixed (char* ptr = Convert.base64Table)
			{
				int i;
				for (i = offset; i < num2; i += 3)
				{
					if (insertLineBreaks)
					{
						if (num4 == 76)
						{
							outChars[num3++] = '\r';
							outChars[num3++] = '\n';
							num4 = 0;
						}
						num4 += 4;
					}
					outChars[num3] = ptr[(inData[i] & 252) >> 2];
					outChars[num3 + 1] = ptr[((int)(inData[i] & 3) << 4) | ((inData[i + 1] & 240) >> 4)];
					outChars[num3 + 2] = ptr[((int)(inData[i + 1] & 15) << 2) | ((inData[i + 2] & 192) >> 6)];
					outChars[num3 + 3] = ptr[inData[i + 2] & 63];
					num3 += 4;
				}
				i = num2;
				if (insertLineBreaks && num != 0 && num4 == 76)
				{
					outChars[num3++] = '\r';
					outChars[num3++] = '\n';
				}
				switch (num)
				{
				case 1:
					outChars[num3] = ptr[(inData[i] & 252) >> 2];
					outChars[num3 + 1] = ptr[(inData[i] & 3) << 4];
					outChars[num3 + 2] = ptr[64];
					outChars[num3 + 3] = ptr[64];
					num3 += 4;
					break;
				case 2:
					outChars[num3] = ptr[(inData[i] & 252) >> 2];
					outChars[num3 + 1] = ptr[((int)(inData[i] & 3) << 4) | ((inData[i + 1] & 240) >> 4)];
					outChars[num3 + 2] = ptr[(inData[i + 1] & 15) << 2];
					outChars[num3 + 3] = ptr[64];
					num3 += 4;
					break;
				}
			}
			return num3;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001C278 File Offset: 0x0001B278
		private static int CalculateOutputLength(int inputLength, bool insertLineBreaks)
		{
			int num = inputLength / 3 * 4;
			num += ((inputLength % 3 != 0) ? 4 : 0);
			if (num == 0)
			{
				return num;
			}
			if (insertLineBreaks)
			{
				int num2 = num / 76;
				if (num % 76 == 0)
				{
					num2--;
				}
				num += num2 * 2;
			}
			return num;
		}

		// Token: 0x06000940 RID: 2368
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] FromBase64String(string s);

		// Token: 0x06000941 RID: 2369
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] FromBase64CharArray(char[] inArray, int offset, int length);

		// Token: 0x04000363 RID: 867
		internal static readonly Type[] ConvertTypes = new Type[]
		{
			typeof(Empty),
			typeof(object),
			typeof(DBNull),
			typeof(bool),
			typeof(char),
			typeof(sbyte),
			typeof(byte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(DateTime),
			typeof(object),
			typeof(string)
		};

		// Token: 0x04000364 RID: 868
		internal static readonly Type EnumType = typeof(Enum);

		// Token: 0x04000365 RID: 869
		internal static readonly char[] base64Table = new char[]
		{
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
			'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
			'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
			'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
			'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
			'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7',
			'8', '9', '+', '/', '='
		};

		// Token: 0x04000366 RID: 870
		public static readonly object DBNull = global::System.DBNull.Value;
	}
}

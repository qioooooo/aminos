using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
	// Token: 0x0200001F RID: 31
	[ComVisible(true)]
	[Serializable]
	public abstract class Enum : ValueType, IComparable, IFormattable, IConvertible
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00005138 File Offset: 0x00004138
		private static FieldInfo GetValueField(Type type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fields == null || fields.Length != 1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumMustHaveUnderlyingValueField"));
			}
			return fields[0];
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000516C File Offset: 0x0000416C
		private static Enum.HashEntry GetHashEntry(Type enumType)
		{
			Enum.HashEntry hashEntry = (Enum.HashEntry)Enum.fieldInfoHash[enumType];
			if (hashEntry == null)
			{
				if (Enum.fieldInfoHash.Count > 100)
				{
					Enum.fieldInfoHash.Clear();
				}
				ulong[] array = null;
				string[] array2 = null;
				if (enumType.BaseType == typeof(Enum))
				{
					Enum.InternalGetEnumValues(enumType, ref array, ref array2);
				}
				else
				{
					FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
					array = new ulong[fields.Length];
					array2 = new string[fields.Length];
					for (int i = 0; i < fields.Length; i++)
					{
						array2[i] = fields[i].Name;
						array[i] = Enum.ToUInt64(fields[i].GetValue(null));
					}
					for (int j = 1; j < array.Length; j++)
					{
						int num = j;
						string text = array2[j];
						ulong num2 = array[j];
						bool flag = false;
						while (array[num - 1] > num2)
						{
							array2[num] = array2[num - 1];
							array[num] = array[num - 1];
							num--;
							flag = true;
							if (num == 0)
							{
								break;
							}
						}
						if (flag)
						{
							array2[num] = text;
							array[num] = num2;
						}
					}
				}
				hashEntry = new Enum.HashEntry(array2, array);
				Enum.fieldInfoHash[enumType] = hashEntry;
			}
			return hashEntry;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005294 File Offset: 0x00004294
		private static string InternalGetValueAsString(Type enumType, object value)
		{
			Enum.HashEntry hashEntry = Enum.GetHashEntry(enumType);
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (underlyingType == Enum.intType || underlyingType == typeof(short) || underlyingType == typeof(long) || underlyingType == typeof(ushort) || underlyingType == typeof(byte) || underlyingType == typeof(sbyte) || underlyingType == typeof(uint) || underlyingType == typeof(ulong))
			{
				ulong num = Enum.ToUInt64(value);
				int num2 = Enum.BinarySearch(hashEntry.values, num);
				if (num2 >= 0)
				{
					return hashEntry.names[num2];
				}
			}
			return null;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005334 File Offset: 0x00004334
		private static string InternalFormattedHexString(object value)
		{
			switch (Convert.GetTypeCode(value))
			{
			case TypeCode.SByte:
				return ((byte)((sbyte)value)).ToString("X2", null);
			case TypeCode.Byte:
				return ((byte)value).ToString("X2", null);
			case TypeCode.Int16:
				return ((ushort)((short)value)).ToString("X4", null);
			case TypeCode.UInt16:
				return ((ushort)value).ToString("X4", null);
			case TypeCode.Int32:
				return ((uint)((int)value)).ToString("X8", null);
			case TypeCode.UInt32:
				return ((uint)value).ToString("X8", null);
			case TypeCode.Int64:
				return ((ulong)((long)value)).ToString("X16", null);
			case TypeCode.UInt64:
				return ((ulong)value).ToString("X16", null);
			default:
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005438 File Offset: 0x00004438
		private static string InternalFormat(Type eT, object value)
		{
			if (eT.IsDefined(typeof(FlagsAttribute), false))
			{
				return Enum.InternalFlagsFormat(eT, value);
			}
			string text = Enum.InternalGetValueAsString(eT, value);
			if (text == null)
			{
				return value.ToString();
			}
			return text;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005474 File Offset: 0x00004474
		private static string InternalFlagsFormat(Type eT, object value)
		{
			ulong num = Enum.ToUInt64(value);
			Enum.HashEntry hashEntry = Enum.GetHashEntry(eT);
			string[] names = hashEntry.names;
			ulong[] values = hashEntry.values;
			int num2 = values.Length - 1;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			ulong num3 = num;
			while (num2 >= 0 && (num2 != 0 || values[num2] != 0UL))
			{
				if ((num & values[num2]) == values[num2])
				{
					num -= values[num2];
					if (!flag)
					{
						stringBuilder.Insert(0, ", ");
					}
					stringBuilder.Insert(0, names[num2]);
					flag = false;
				}
				num2--;
			}
			if (num != 0UL)
			{
				return value.ToString();
			}
			if (num3 != 0UL)
			{
				return stringBuilder.ToString();
			}
			if (values[0] == 0UL)
			{
				return names[0];
			}
			return "0";
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000552C File Offset: 0x0000452C
		private static ulong ToUInt64(object value)
		{
			ulong num;
			switch (Convert.GetTypeCode(value))
			{
			case TypeCode.SByte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
				num = (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture);
				break;
			case TypeCode.Byte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
				num = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
				break;
			default:
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
			}
			return num;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000559C File Offset: 0x0000459C
		private static int BinarySearch(ulong[] array, ulong value)
		{
			int i = 0;
			int num = array.Length - 1;
			while (i <= num)
			{
				int num2 = i + num >> 1;
				ulong num3 = array[num2];
				if (value == num3)
				{
					return num2;
				}
				if (num3 < value)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return ~i;
		}

		// Token: 0x06000105 RID: 261
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int InternalCompareTo(object o1, object o2);

		// Token: 0x06000106 RID: 262
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type InternalGetUnderlyingType(Type enumType);

		// Token: 0x06000107 RID: 263
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalGetEnumValues(Type enumType, ref ulong[] values, ref string[] names);

		// Token: 0x06000108 RID: 264
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object InternalBoxEnum(Type enumType, long value);

		// Token: 0x06000109 RID: 265 RVA: 0x000055D7 File Offset: 0x000045D7
		[ComVisible(true)]
		public static object Parse(Type enumType, string value)
		{
			return Enum.Parse(enumType, value, false);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000055E4 File Offset: 0x000045E4
		[ComVisible(true)]
		public static object Parse(Type enumType, string value, bool ignoreCase)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			value = value.Trim();
			if (value.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustContainEnumInfo"));
			}
			ulong num = 0UL;
			if (char.IsDigit(value[0]) || value[0] == '-' || value[0] == '+')
			{
				Type underlyingType = Enum.GetUnderlyingType(enumType);
				try
				{
					object obj = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
					return Enum.ToObject(enumType, obj);
				}
				catch (FormatException)
				{
				}
			}
			string[] array = value.Split(Enum.enumSeperatorCharArray);
			Enum.HashEntry hashEntry = Enum.GetHashEntry(enumType);
			string[] names = hashEntry.names;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Trim();
				bool flag = false;
				int j = 0;
				while (j < names.Length)
				{
					if (ignoreCase)
					{
						if (string.Compare(names[j], array[i], StringComparison.OrdinalIgnoreCase) == 0)
						{
							goto IL_0122;
						}
					}
					else if (names[j].Equals(array[i]))
					{
						goto IL_0122;
					}
					j++;
					continue;
					IL_0122:
					ulong num2 = hashEntry.values[j];
					num |= num2;
					flag = true;
					break;
				}
				if (!flag)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumValueNotFound"), new object[] { value }));
				}
			}
			return Enum.ToObject(enumType, num);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005790 File Offset: 0x00004790
		[ComVisible(true)]
		public static Type GetUnderlyingType(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (enumType is EnumBuilder)
			{
				return ((EnumBuilder)enumType).UnderlyingSystemType;
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalGetUnderlyingType(enumType);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005800 File Offset: 0x00004800
		[ComVisible(true)]
		public static Array GetValues(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			ulong[] values = Enum.GetHashEntry(enumType).values;
			Array array = Array.CreateInstance(enumType, values.Length);
			for (int i = 0; i < values.Length; i++)
			{
				object obj = Enum.ToObject(enumType, values[i]);
				array.SetValue(obj, i);
			}
			return array;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000588C File Offset: 0x0000488C
		[ComVisible(true)]
		public static string GetName(Type enumType, object value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type.IsEnum || type == Enum.intType || type == typeof(short) || type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong))
			{
				return Enum.InternalGetValueAsString(enumType, value);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnumBaseTypeOrEnum"), "value");
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005980 File Offset: 0x00004980
		[ComVisible(true)]
		public static string[] GetNames(Type enumType)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			string[] names = Enum.GetHashEntry(enumType).names;
			string[] array = new string[names.Length];
			Array.Copy(names, array, names.Length);
			return array;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000059F8 File Offset: 0x000049F8
		[ComVisible(true)]
		public static object ToObject(Type enumType, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			switch (Convert.GetTypeCode(value))
			{
			case TypeCode.SByte:
				return Enum.ToObject(enumType, (sbyte)value);
			case TypeCode.Byte:
				return Enum.ToObject(enumType, (byte)value);
			case TypeCode.Int16:
				return Enum.ToObject(enumType, (short)value);
			case TypeCode.UInt16:
				return Enum.ToObject(enumType, (ushort)value);
			case TypeCode.Int32:
				return Enum.ToObject(enumType, (int)value);
			case TypeCode.UInt32:
				return Enum.ToObject(enumType, (uint)value);
			case TypeCode.Int64:
				return Enum.ToObject(enumType, (long)value);
			case TypeCode.UInt64:
				return Enum.ToObject(enumType, (ulong)value);
			default:
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnumBaseTypeOrEnum"), "value");
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005AC4 File Offset: 0x00004AC4
		[ComVisible(true)]
		public static bool IsDefined(Type enumType, object value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "valueType");
			}
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (type.IsEnum)
			{
				Type underlyingType2 = Enum.GetUnderlyingType(type);
				if (type != enumType)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumAndObjectMustBeSameType"), new object[]
					{
						type.ToString(),
						enumType.ToString()
					}));
				}
				type = underlyingType2;
			}
			else if (type != underlyingType && type != Enum.stringType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumUnderlyingTypeAndObjectMustBeSameType"), new object[]
				{
					type.ToString(),
					underlyingType.ToString()
				}));
			}
			if (type == Enum.stringType)
			{
				string[] names = Enum.GetHashEntry(enumType).names;
				for (int i = 0; i < names.Length; i++)
				{
					if (names[i].Equals((string)value))
					{
						return true;
					}
				}
				return false;
			}
			ulong[] values = Enum.GetHashEntry(enumType).values;
			if (type == Enum.intType || type == typeof(short) || type == typeof(ushort) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong))
			{
				ulong num = Enum.ToUInt64(value);
				return Enum.BinarySearch(values, num) >= 0;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005CB8 File Offset: 0x00004CB8
		[ComVisible(true)]
		public static string Format(Type enumType, object value, string format)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			Type type = value.GetType();
			if (!(type is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "valueType");
			}
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			if (type.IsEnum)
			{
				Type underlyingType2 = Enum.GetUnderlyingType(type);
				if (type != enumType)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumAndObjectMustBeSameType"), new object[]
					{
						type.ToString(),
						enumType.ToString()
					}));
				}
				value = ((Enum)value).GetValue();
			}
			else if (type != underlyingType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType"), new object[]
				{
					type.ToString(),
					underlyingType.ToString()
				}));
			}
			if (format.Length != 1)
			{
				throw new FormatException(Environment.GetResourceString("Format_InvalidEnumFormatSpecification"));
			}
			char c = format[0];
			if (c == 'D' || c == 'd')
			{
				return value.ToString();
			}
			if (c == 'X' || c == 'x')
			{
				return Enum.InternalFormattedHexString(value);
			}
			if (c == 'G' || c == 'g')
			{
				return Enum.InternalFormat(enumType, value);
			}
			if (c == 'F' || c == 'f')
			{
				return Enum.InternalFlagsFormat(enumType, value);
			}
			throw new FormatException(Environment.GetResourceString("Format_InvalidEnumFormatSpecification"));
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005E62 File Offset: 0x00004E62
		private object GetValue()
		{
			return this.InternalGetValue();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005E6C File Offset: 0x00004E6C
		private string ToHexString()
		{
			Type type = base.GetType();
			FieldInfo valueField = Enum.GetValueField(type);
			return Enum.InternalFormattedHexString(((RtFieldInfo)valueField).InternalGetValue(this, false));
		}

		// Token: 0x06000114 RID: 276
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object InternalGetValue();

		// Token: 0x06000115 RID: 277
		[MethodImpl(MethodImplOptions.InternalCall)]
		public override extern bool Equals(object obj);

		// Token: 0x06000116 RID: 278 RVA: 0x00005E99 File Offset: 0x00004E99
		public override int GetHashCode()
		{
			return this.GetValue().GetHashCode();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005EA8 File Offset: 0x00004EA8
		public override string ToString()
		{
			Type type = base.GetType();
			FieldInfo valueField = Enum.GetValueField(type);
			object obj = ((RtFieldInfo)valueField).InternalGetValue(this, false);
			return Enum.InternalFormat(type, obj);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005ED8 File Offset: 0x00004ED8
		[Obsolete("The provider argument is not used. Please use ToString(String).")]
		public string ToString(string format, IFormatProvider provider)
		{
			return this.ToString(format);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005EE4 File Offset: 0x00004EE4
		public int CompareTo(object target)
		{
			if (this == null)
			{
				throw new NullReferenceException();
			}
			int num = Enum.InternalCompareTo(this, target);
			if (num < 2)
			{
				return num;
			}
			if (num == 2)
			{
				Type type = base.GetType();
				Type type2 = target.GetType();
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumAndObjectMustBeSameType"), new object[]
				{
					type2.ToString(),
					type.ToString()
				}));
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005F60 File Offset: 0x00004F60
		public string ToString(string format)
		{
			if (format == null || format.Length == 0)
			{
				format = "G";
			}
			if (string.Compare(format, "G", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.ToString();
			}
			if (string.Compare(format, "D", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.GetValue().ToString();
			}
			if (string.Compare(format, "X", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.ToHexString();
			}
			if (string.Compare(format, "F", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return Enum.InternalFlagsFormat(base.GetType(), this.GetValue());
			}
			throw new FormatException(Environment.GetResourceString("Format_InvalidEnumFormatSpecification"));
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005FF2 File Offset: 0x00004FF2
		[Obsolete("The provider argument is not used. Please use ToString().")]
		public string ToString(IFormatProvider provider)
		{
			return this.ToString();
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005FFC File Offset: 0x00004FFC
		public TypeCode GetTypeCode()
		{
			Type type = base.GetType();
			Type underlyingType = Enum.GetUnderlyingType(type);
			if (underlyingType == typeof(int))
			{
				return TypeCode.Int32;
			}
			if (underlyingType == typeof(sbyte))
			{
				return TypeCode.SByte;
			}
			if (underlyingType == typeof(short))
			{
				return TypeCode.Int16;
			}
			if (underlyingType == typeof(long))
			{
				return TypeCode.Int64;
			}
			if (underlyingType == typeof(uint))
			{
				return TypeCode.UInt32;
			}
			if (underlyingType == typeof(byte))
			{
				return TypeCode.Byte;
			}
			if (underlyingType == typeof(ushort))
			{
				return TypeCode.UInt16;
			}
			if (underlyingType == typeof(ulong))
			{
				return TypeCode.UInt64;
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_UnknownEnumType"));
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000060A2 File Offset: 0x000050A2
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000060B4 File Offset: 0x000050B4
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000060C6 File Offset: 0x000050C6
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000060D8 File Offset: 0x000050D8
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000060EA File Offset: 0x000050EA
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000060FC File Offset: 0x000050FC
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000610E File Offset: 0x0000510E
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006120 File Offset: 0x00005120
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006132 File Offset: 0x00005132
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006144 File Offset: 0x00005144
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006156 File Offset: 0x00005156
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006168 File Offset: 0x00005168
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000617A File Offset: 0x0000517A
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this.GetValue(), CultureInfo.CurrentCulture);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000618C File Offset: 0x0000518C
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[] { "Enum", "DateTime" }));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000061CA File Offset: 0x000051CA
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000061D4 File Offset: 0x000051D4
		[CLSCompliant(false)]
		[ComVisible(true)]
		public static object ToObject(Type enumType, sbyte value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)value);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006234 File Offset: 0x00005234
		[ComVisible(true)]
		public static object ToObject(Type enumType, short value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)value);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006294 File Offset: 0x00005294
		[ComVisible(true)]
		public static object ToObject(Type enumType, int value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)value);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000062F4 File Offset: 0x000052F4
		[ComVisible(true)]
		public static object ToObject(Type enumType, byte value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)((ulong)value));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006354 File Offset: 0x00005354
		[CLSCompliant(false)]
		[ComVisible(true)]
		public static object ToObject(Type enumType, ushort value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)((ulong)value));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000063B4 File Offset: 0x000053B4
		[CLSCompliant(false)]
		[ComVisible(true)]
		public static object ToObject(Type enumType, uint value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)((ulong)value));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006414 File Offset: 0x00005414
		[ComVisible(true)]
		public static object ToObject(Type enumType, long value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, value);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006470 File Offset: 0x00005470
		[ComVisible(true)]
		[CLSCompliant(false)]
		public static object ToObject(Type enumType, ulong value)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			if (!(enumType is RuntimeType))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "enumType");
			}
			if (!enumType.IsEnum)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeEnum"), "enumType");
			}
			return Enum.InternalBoxEnum(enumType, (long)value);
		}

		// Token: 0x04000047 RID: 71
		private const string enumSeperator = ", ";

		// Token: 0x04000048 RID: 72
		private const int maxHashElements = 100;

		// Token: 0x04000049 RID: 73
		private static char[] enumSeperatorCharArray = new char[] { ',' };

		// Token: 0x0400004A RID: 74
		private static Type intType = typeof(int);

		// Token: 0x0400004B RID: 75
		private static Type stringType = typeof(string);

		// Token: 0x0400004C RID: 76
		private static Hashtable fieldInfoHash = Hashtable.Synchronized(new Hashtable());

		// Token: 0x02000020 RID: 32
		private class HashEntry
		{
			// Token: 0x06000136 RID: 310 RVA: 0x00006520 File Offset: 0x00005520
			public HashEntry(string[] names, ulong[] values)
			{
				this.names = names;
				this.values = values;
			}

			// Token: 0x0400004D RID: 77
			public string[] names;

			// Token: 0x0400004E RID: 78
			public ulong[] values;
		}
	}
}

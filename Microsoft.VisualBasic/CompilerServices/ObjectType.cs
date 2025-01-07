using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ObjectType
	{
		// Note: this type is marked as 'beforefieldinit'.
		static ObjectType()
		{
			ObjectType.VType[,] array = new ObjectType.VType[12, 12];
			array[0, 0] = ObjectType.VType.t_bad;
			array[0, 1] = ObjectType.VType.t_bad;
			array[0, 2] = ObjectType.VType.t_bad;
			array[0, 3] = ObjectType.VType.t_bad;
			array[0, 4] = ObjectType.VType.t_bad;
			array[0, 5] = ObjectType.VType.t_bad;
			array[0, 6] = ObjectType.VType.t_bad;
			array[0, 7] = ObjectType.VType.t_bad;
			array[0, 8] = ObjectType.VType.t_bad;
			array[0, 9] = ObjectType.VType.t_bad;
			array[0, 10] = ObjectType.VType.t_bad;
			array[0, 11] = ObjectType.VType.t_bad;
			array[1, 0] = ObjectType.VType.t_bad;
			array[1, 1] = ObjectType.VType.t_bool;
			array[1, 2] = ObjectType.VType.t_bool;
			array[1, 3] = ObjectType.VType.t_i2;
			array[1, 4] = ObjectType.VType.t_i4;
			array[1, 5] = ObjectType.VType.t_i8;
			array[1, 6] = ObjectType.VType.t_dec;
			array[1, 7] = ObjectType.VType.t_r4;
			array[1, 8] = ObjectType.VType.t_r8;
			array[1, 9] = ObjectType.VType.t_bad;
			array[1, 10] = ObjectType.VType.t_r8;
			array[1, 11] = ObjectType.VType.t_bad;
			array[2, 0] = ObjectType.VType.t_bad;
			array[2, 1] = ObjectType.VType.t_bool;
			array[2, 2] = ObjectType.VType.t_ui1;
			array[2, 3] = ObjectType.VType.t_i2;
			array[2, 4] = ObjectType.VType.t_i4;
			array[2, 5] = ObjectType.VType.t_i8;
			array[2, 6] = ObjectType.VType.t_dec;
			array[2, 7] = ObjectType.VType.t_r4;
			array[2, 8] = ObjectType.VType.t_r8;
			array[2, 9] = ObjectType.VType.t_bad;
			array[2, 10] = ObjectType.VType.t_r8;
			array[2, 11] = ObjectType.VType.t_bad;
			array[3, 0] = ObjectType.VType.t_bad;
			array[3, 1] = ObjectType.VType.t_i2;
			array[3, 2] = ObjectType.VType.t_i2;
			array[3, 3] = ObjectType.VType.t_i2;
			array[3, 4] = ObjectType.VType.t_i4;
			array[3, 5] = ObjectType.VType.t_i8;
			array[3, 6] = ObjectType.VType.t_dec;
			array[3, 7] = ObjectType.VType.t_r4;
			array[3, 8] = ObjectType.VType.t_r8;
			array[3, 9] = ObjectType.VType.t_bad;
			array[3, 10] = ObjectType.VType.t_r8;
			array[3, 11] = ObjectType.VType.t_bad;
			array[4, 0] = ObjectType.VType.t_bad;
			array[4, 1] = ObjectType.VType.t_i4;
			array[4, 2] = ObjectType.VType.t_i4;
			array[4, 3] = ObjectType.VType.t_i4;
			array[4, 4] = ObjectType.VType.t_i4;
			array[4, 5] = ObjectType.VType.t_i8;
			array[4, 6] = ObjectType.VType.t_dec;
			array[4, 7] = ObjectType.VType.t_r4;
			array[4, 8] = ObjectType.VType.t_r8;
			array[4, 9] = ObjectType.VType.t_bad;
			array[4, 10] = ObjectType.VType.t_r8;
			array[4, 11] = ObjectType.VType.t_bad;
			array[5, 0] = ObjectType.VType.t_bad;
			array[5, 1] = ObjectType.VType.t_i8;
			array[5, 2] = ObjectType.VType.t_i8;
			array[5, 3] = ObjectType.VType.t_i8;
			array[5, 4] = ObjectType.VType.t_i8;
			array[5, 5] = ObjectType.VType.t_i8;
			array[5, 6] = ObjectType.VType.t_dec;
			array[5, 7] = ObjectType.VType.t_r4;
			array[5, 8] = ObjectType.VType.t_r8;
			array[5, 9] = ObjectType.VType.t_bad;
			array[5, 10] = ObjectType.VType.t_r8;
			array[5, 11] = ObjectType.VType.t_bad;
			array[6, 0] = ObjectType.VType.t_bad;
			array[6, 1] = ObjectType.VType.t_dec;
			array[6, 2] = ObjectType.VType.t_dec;
			array[6, 3] = ObjectType.VType.t_dec;
			array[6, 4] = ObjectType.VType.t_dec;
			array[6, 5] = ObjectType.VType.t_dec;
			array[6, 6] = ObjectType.VType.t_dec;
			array[6, 7] = ObjectType.VType.t_r4;
			array[6, 8] = ObjectType.VType.t_r8;
			array[6, 9] = ObjectType.VType.t_bad;
			array[6, 10] = ObjectType.VType.t_r8;
			array[6, 11] = ObjectType.VType.t_bad;
			array[7, 0] = ObjectType.VType.t_bad;
			array[7, 1] = ObjectType.VType.t_r4;
			array[7, 2] = ObjectType.VType.t_r4;
			array[7, 3] = ObjectType.VType.t_r4;
			array[7, 4] = ObjectType.VType.t_r4;
			array[7, 5] = ObjectType.VType.t_r4;
			array[7, 6] = ObjectType.VType.t_r4;
			array[7, 7] = ObjectType.VType.t_r4;
			array[7, 8] = ObjectType.VType.t_r8;
			array[7, 9] = ObjectType.VType.t_bad;
			array[7, 10] = ObjectType.VType.t_r8;
			array[7, 11] = ObjectType.VType.t_bad;
			array[8, 0] = ObjectType.VType.t_bad;
			array[8, 1] = ObjectType.VType.t_r8;
			array[8, 2] = ObjectType.VType.t_r8;
			array[8, 3] = ObjectType.VType.t_r8;
			array[8, 4] = ObjectType.VType.t_r8;
			array[8, 5] = ObjectType.VType.t_r8;
			array[8, 6] = ObjectType.VType.t_r8;
			array[8, 7] = ObjectType.VType.t_r8;
			array[8, 8] = ObjectType.VType.t_r8;
			array[8, 9] = ObjectType.VType.t_bad;
			array[8, 10] = ObjectType.VType.t_r8;
			array[8, 11] = ObjectType.VType.t_bad;
			array[9, 0] = ObjectType.VType.t_bad;
			array[9, 1] = ObjectType.VType.t_bad;
			array[9, 2] = ObjectType.VType.t_bad;
			array[9, 3] = ObjectType.VType.t_bad;
			array[9, 4] = ObjectType.VType.t_bad;
			array[9, 5] = ObjectType.VType.t_bad;
			array[9, 6] = ObjectType.VType.t_bad;
			array[9, 7] = ObjectType.VType.t_bad;
			array[9, 8] = ObjectType.VType.t_bad;
			array[9, 9] = ObjectType.VType.t_char;
			array[9, 10] = ObjectType.VType.t_str;
			array[9, 11] = ObjectType.VType.t_bad;
			array[10, 0] = ObjectType.VType.t_bad;
			array[10, 1] = ObjectType.VType.t_r8;
			array[10, 2] = ObjectType.VType.t_r8;
			array[10, 3] = ObjectType.VType.t_r8;
			array[10, 4] = ObjectType.VType.t_r8;
			array[10, 5] = ObjectType.VType.t_r8;
			array[10, 6] = ObjectType.VType.t_r8;
			array[10, 7] = ObjectType.VType.t_r8;
			array[10, 8] = ObjectType.VType.t_r8;
			array[10, 9] = ObjectType.VType.t_str;
			array[10, 10] = ObjectType.VType.t_str;
			array[10, 11] = ObjectType.VType.t_date;
			array[11, 0] = ObjectType.VType.t_bad;
			array[11, 1] = ObjectType.VType.t_bad;
			array[11, 2] = ObjectType.VType.t_bad;
			array[11, 3] = ObjectType.VType.t_bad;
			array[11, 4] = ObjectType.VType.t_bad;
			array[11, 5] = ObjectType.VType.t_bad;
			array[11, 6] = ObjectType.VType.t_bad;
			array[11, 7] = ObjectType.VType.t_bad;
			array[11, 8] = ObjectType.VType.t_bad;
			array[11, 9] = ObjectType.VType.t_bad;
			array[11, 10] = ObjectType.VType.t_date;
			array[11, 11] = ObjectType.VType.t_date;
			ObjectType.WiderType = array;
			ObjectType.CC[,] array2 = new ObjectType.CC[13, 13];
			array2[0, 0] = ObjectType.CC.Err;
			array2[0, 1] = ObjectType.CC.Err;
			array2[0, 2] = ObjectType.CC.Err;
			array2[0, 3] = ObjectType.CC.Err;
			array2[0, 4] = ObjectType.CC.Err;
			array2[0, 5] = ObjectType.CC.Err;
			array2[0, 6] = ObjectType.CC.Err;
			array2[0, 7] = ObjectType.CC.Err;
			array2[0, 8] = ObjectType.CC.Err;
			array2[0, 9] = ObjectType.CC.Err;
			array2[0, 10] = ObjectType.CC.Err;
			array2[0, 11] = ObjectType.CC.Err;
			array2[0, 12] = ObjectType.CC.Err;
			array2[1, 0] = ObjectType.CC.Err;
			array2[1, 1] = ObjectType.CC.Same;
			array2[1, 2] = ObjectType.CC.Narr;
			array2[1, 3] = ObjectType.CC.Err;
			array2[1, 4] = ObjectType.CC.Narr;
			array2[1, 5] = ObjectType.CC.Narr;
			array2[1, 6] = ObjectType.CC.Narr;
			array2[1, 7] = ObjectType.CC.Narr;
			array2[1, 8] = ObjectType.CC.Narr;
			array2[1, 9] = ObjectType.CC.Err;
			array2[1, 10] = ObjectType.CC.Narr;
			array2[1, 11] = ObjectType.CC.Err;
			array2[1, 12] = ObjectType.CC.Narr;
			array2[2, 0] = ObjectType.CC.Err;
			array2[2, 1] = ObjectType.CC.Narr;
			array2[2, 2] = ObjectType.CC.Same;
			array2[2, 3] = ObjectType.CC.Err;
			array2[2, 4] = ObjectType.CC.Narr;
			array2[2, 5] = ObjectType.CC.Narr;
			array2[2, 6] = ObjectType.CC.Narr;
			array2[2, 7] = ObjectType.CC.Narr;
			array2[2, 8] = ObjectType.CC.Narr;
			array2[2, 9] = ObjectType.CC.Err;
			array2[2, 10] = ObjectType.CC.Narr;
			array2[2, 11] = ObjectType.CC.Err;
			array2[2, 12] = ObjectType.CC.Narr;
			array2[3, 0] = ObjectType.CC.Err;
			array2[3, 1] = ObjectType.CC.Err;
			array2[3, 2] = ObjectType.CC.Err;
			array2[3, 3] = ObjectType.CC.Same;
			array2[3, 4] = ObjectType.CC.Err;
			array2[3, 5] = ObjectType.CC.Err;
			array2[3, 6] = ObjectType.CC.Err;
			array2[3, 7] = ObjectType.CC.Err;
			array2[3, 8] = ObjectType.CC.Err;
			array2[3, 9] = ObjectType.CC.Err;
			array2[3, 10] = ObjectType.CC.Err;
			array2[3, 11] = ObjectType.CC.Err;
			array2[3, 12] = ObjectType.CC.Narr;
			array2[4, 0] = ObjectType.CC.Err;
			array2[4, 1] = ObjectType.CC.Narr;
			array2[4, 2] = ObjectType.CC.Wide;
			array2[4, 3] = ObjectType.CC.Err;
			array2[4, 4] = ObjectType.CC.Same;
			array2[4, 5] = ObjectType.CC.Narr;
			array2[4, 6] = ObjectType.CC.Narr;
			array2[4, 7] = ObjectType.CC.Narr;
			array2[4, 8] = ObjectType.CC.Narr;
			array2[4, 9] = ObjectType.CC.Err;
			array2[4, 10] = ObjectType.CC.Narr;
			array2[4, 11] = ObjectType.CC.Err;
			array2[4, 12] = ObjectType.CC.Narr;
			array2[5, 0] = ObjectType.CC.Err;
			array2[5, 1] = ObjectType.CC.Narr;
			array2[5, 2] = ObjectType.CC.Wide;
			array2[5, 3] = ObjectType.CC.Err;
			array2[5, 4] = ObjectType.CC.Wide;
			array2[5, 5] = ObjectType.CC.Same;
			array2[5, 6] = ObjectType.CC.Narr;
			array2[5, 7] = ObjectType.CC.Narr;
			array2[5, 8] = ObjectType.CC.Narr;
			array2[5, 9] = ObjectType.CC.Err;
			array2[5, 10] = ObjectType.CC.Narr;
			array2[5, 11] = ObjectType.CC.Err;
			array2[5, 12] = ObjectType.CC.Narr;
			array2[6, 0] = ObjectType.CC.Err;
			array2[6, 1] = ObjectType.CC.Narr;
			array2[6, 2] = ObjectType.CC.Wide;
			array2[6, 3] = ObjectType.CC.Err;
			array2[6, 4] = ObjectType.CC.Wide;
			array2[6, 5] = ObjectType.CC.Wide;
			array2[6, 6] = ObjectType.CC.Same;
			array2[6, 7] = ObjectType.CC.Narr;
			array2[6, 8] = ObjectType.CC.Narr;
			array2[6, 9] = ObjectType.CC.Err;
			array2[6, 10] = ObjectType.CC.Narr;
			array2[6, 11] = ObjectType.CC.Err;
			array2[6, 12] = ObjectType.CC.Narr;
			array2[7, 0] = ObjectType.CC.Err;
			array2[7, 1] = ObjectType.CC.Narr;
			array2[7, 2] = ObjectType.CC.Wide;
			array2[7, 3] = ObjectType.CC.Err;
			array2[7, 4] = ObjectType.CC.Wide;
			array2[7, 5] = ObjectType.CC.Wide;
			array2[7, 6] = ObjectType.CC.Wide;
			array2[7, 7] = ObjectType.CC.Same;
			array2[7, 8] = ObjectType.CC.Narr;
			array2[7, 9] = ObjectType.CC.Err;
			array2[7, 10] = ObjectType.CC.Wide;
			array2[7, 11] = ObjectType.CC.Err;
			array2[7, 12] = ObjectType.CC.Narr;
			array2[8, 0] = ObjectType.CC.Err;
			array2[8, 1] = ObjectType.CC.Narr;
			array2[8, 2] = ObjectType.CC.Wide;
			array2[8, 3] = ObjectType.CC.Err;
			array2[8, 4] = ObjectType.CC.Wide;
			array2[8, 5] = ObjectType.CC.Wide;
			array2[8, 6] = ObjectType.CC.Wide;
			array2[8, 7] = ObjectType.CC.Wide;
			array2[8, 8] = ObjectType.CC.Same;
			array2[8, 9] = ObjectType.CC.Err;
			array2[8, 10] = ObjectType.CC.Wide;
			array2[8, 11] = ObjectType.CC.Err;
			array2[8, 12] = ObjectType.CC.Narr;
			array2[9, 0] = ObjectType.CC.Err;
			array2[9, 1] = ObjectType.CC.Err;
			array2[9, 2] = ObjectType.CC.Err;
			array2[9, 3] = ObjectType.CC.Err;
			array2[9, 4] = ObjectType.CC.Err;
			array2[9, 5] = ObjectType.CC.Err;
			array2[9, 6] = ObjectType.CC.Err;
			array2[9, 7] = ObjectType.CC.Err;
			array2[9, 8] = ObjectType.CC.Err;
			array2[9, 9] = ObjectType.CC.Same;
			array2[9, 10] = ObjectType.CC.Err;
			array2[9, 11] = ObjectType.CC.Err;
			array2[9, 12] = ObjectType.CC.Narr;
			array2[10, 0] = ObjectType.CC.Err;
			array2[10, 1] = ObjectType.CC.Narr;
			array2[10, 2] = ObjectType.CC.Wide;
			array2[10, 3] = ObjectType.CC.Err;
			array2[10, 4] = ObjectType.CC.Wide;
			array2[10, 5] = ObjectType.CC.Wide;
			array2[10, 6] = ObjectType.CC.Wide;
			array2[10, 7] = ObjectType.CC.Narr;
			array2[10, 8] = ObjectType.CC.Narr;
			array2[10, 9] = ObjectType.CC.Err;
			array2[10, 10] = ObjectType.CC.Same;
			array2[10, 11] = ObjectType.CC.Err;
			array2[10, 12] = ObjectType.CC.Narr;
			array2[11, 0] = ObjectType.CC.Err;
			array2[11, 1] = ObjectType.CC.Err;
			array2[11, 2] = ObjectType.CC.Err;
			array2[11, 3] = ObjectType.CC.Err;
			array2[11, 4] = ObjectType.CC.Err;
			array2[11, 5] = ObjectType.CC.Err;
			array2[11, 6] = ObjectType.CC.Err;
			array2[11, 7] = ObjectType.CC.Err;
			array2[11, 8] = ObjectType.CC.Err;
			array2[11, 9] = ObjectType.CC.Err;
			array2[11, 10] = ObjectType.CC.Err;
			array2[11, 11] = ObjectType.CC.Err;
			array2[11, 12] = ObjectType.CC.Err;
			array2[12, 0] = ObjectType.CC.Err;
			array2[12, 1] = ObjectType.CC.Narr;
			array2[12, 2] = ObjectType.CC.Narr;
			array2[12, 3] = ObjectType.CC.Wide;
			array2[12, 4] = ObjectType.CC.Narr;
			array2[12, 5] = ObjectType.CC.Narr;
			array2[12, 6] = ObjectType.CC.Narr;
			array2[12, 7] = ObjectType.CC.Narr;
			array2[12, 8] = ObjectType.CC.Narr;
			array2[12, 9] = ObjectType.CC.Narr;
			array2[12, 10] = ObjectType.CC.Narr;
			array2[12, 11] = ObjectType.CC.Err;
			array2[12, 12] = ObjectType.CC.Same;
			ObjectType.ConversionClassTable = array2;
		}

		private static ObjectType.VType VTypeFromTypeCode(TypeCode typ)
		{
			switch (typ)
			{
			case TypeCode.Boolean:
				return ObjectType.VType.t_bool;
			case TypeCode.Char:
				return ObjectType.VType.t_char;
			case TypeCode.Byte:
				return ObjectType.VType.t_ui1;
			case TypeCode.Int16:
				return ObjectType.VType.t_i2;
			case TypeCode.Int32:
				return ObjectType.VType.t_i4;
			case TypeCode.Int64:
				return ObjectType.VType.t_i8;
			case TypeCode.Single:
				return ObjectType.VType.t_r4;
			case TypeCode.Double:
				return ObjectType.VType.t_r8;
			case TypeCode.Decimal:
				return ObjectType.VType.t_dec;
			case TypeCode.DateTime:
				return ObjectType.VType.t_date;
			case TypeCode.String:
				return ObjectType.VType.t_str;
			}
			return ObjectType.VType.t_bad;
		}

		private static ObjectType.VType2 VType2FromTypeCode(TypeCode typ)
		{
			switch (typ)
			{
			case TypeCode.Boolean:
				return ObjectType.VType2.t_bool;
			case TypeCode.Char:
				return ObjectType.VType2.t_char;
			case TypeCode.Byte:
				return ObjectType.VType2.t_ui1;
			case TypeCode.Int16:
				return ObjectType.VType2.t_i2;
			case TypeCode.Int32:
				return ObjectType.VType2.t_i4;
			case TypeCode.Int64:
				return ObjectType.VType2.t_i8;
			case TypeCode.Single:
				return ObjectType.VType2.t_r4;
			case TypeCode.Double:
				return ObjectType.VType2.t_r8;
			case TypeCode.Decimal:
				return ObjectType.VType2.t_dec;
			case TypeCode.DateTime:
				return ObjectType.VType2.t_date;
			case TypeCode.String:
				return ObjectType.VType2.t_str;
			}
			return ObjectType.VType2.t_bad;
		}

		private static TypeCode TypeCodeFromVType(ObjectType.VType vartyp)
		{
			switch (vartyp)
			{
			case ObjectType.VType.t_bool:
				return TypeCode.Boolean;
			case ObjectType.VType.t_ui1:
				return TypeCode.Byte;
			case ObjectType.VType.t_i2:
				return TypeCode.Int16;
			case ObjectType.VType.t_i4:
				return TypeCode.Int32;
			case ObjectType.VType.t_i8:
				return TypeCode.Int64;
			case ObjectType.VType.t_dec:
				return TypeCode.Decimal;
			case ObjectType.VType.t_r4:
				return TypeCode.Single;
			case ObjectType.VType.t_r8:
				return TypeCode.Double;
			case ObjectType.VType.t_char:
				return TypeCode.Char;
			case ObjectType.VType.t_str:
				return TypeCode.String;
			case ObjectType.VType.t_date:
				return TypeCode.DateTime;
			default:
				return TypeCode.Object;
			}
		}

		internal static Type TypeFromTypeCode(TypeCode vartyp)
		{
			switch (vartyp)
			{
			case TypeCode.Object:
				return typeof(object);
			case TypeCode.DBNull:
				return typeof(DBNull);
			case TypeCode.Boolean:
				return typeof(bool);
			case TypeCode.Char:
				return typeof(char);
			case TypeCode.SByte:
				return typeof(sbyte);
			case TypeCode.Byte:
				return typeof(byte);
			case TypeCode.Int16:
				return typeof(short);
			case TypeCode.UInt16:
				return typeof(ushort);
			case TypeCode.Int32:
				return typeof(int);
			case TypeCode.UInt32:
				return typeof(uint);
			case TypeCode.Int64:
				return typeof(long);
			case TypeCode.UInt64:
				return typeof(ulong);
			case TypeCode.Single:
				return typeof(float);
			case TypeCode.Double:
				return typeof(double);
			case TypeCode.Decimal:
				return typeof(decimal);
			case TypeCode.DateTime:
				return typeof(DateTime);
			case TypeCode.String:
				return typeof(string);
			}
			return null;
		}

		internal static bool IsWiderNumeric(Type Type1, Type Type2)
		{
			TypeCode typeCode = Type.GetTypeCode(Type1);
			TypeCode typeCode2 = Type.GetTypeCode(Type2);
			return Information.IsOldNumericTypeCode(typeCode) && Information.IsOldNumericTypeCode(typeCode2) && typeCode != TypeCode.Boolean && typeCode2 != TypeCode.Boolean && !Type1.IsEnum && ObjectType.WiderType[(int)ObjectType.VTypeFromTypeCode(typeCode), (int)ObjectType.VTypeFromTypeCode(typeCode2)] == ObjectType.VTypeFromTypeCode(typeCode);
		}

		internal static bool IsWideningConversion(Type FromType, Type ToType)
		{
			TypeCode typeCode = Type.GetTypeCode(FromType);
			TypeCode typeCode2 = Type.GetTypeCode(ToType);
			if (typeCode == TypeCode.Object)
			{
				if (FromType == typeof(char[]) && (typeCode2 == TypeCode.String || ToType == typeof(char[])))
				{
					return true;
				}
				if (typeCode2 != TypeCode.Object)
				{
					return false;
				}
				if (FromType.IsArray && ToType.IsArray)
				{
					return FromType.GetArrayRank() == ToType.GetArrayRank() && ToType.GetElementType().IsAssignableFrom(FromType.GetElementType());
				}
				return ToType.IsAssignableFrom(FromType);
			}
			else
			{
				if (typeCode2 == TypeCode.Object)
				{
					return (ToType != typeof(char[]) || typeCode != TypeCode.String) && ToType.IsAssignableFrom(FromType);
				}
				if (ToType.IsEnum)
				{
					return false;
				}
				ObjectType.CC cc = ObjectType.ConversionClassTable[(int)ObjectType.VType2FromTypeCode(typeCode2), (int)ObjectType.VType2FromTypeCode(typeCode)];
				return cc == ObjectType.CC.Wide || cc == ObjectType.CC.Same;
			}
		}

		internal static TypeCode GetWidestType(object obj1, object obj2, bool IsAdd = false)
		{
			IConvertible convertible = obj1 as IConvertible;
			IConvertible convertible2 = obj2 as IConvertible;
			TypeCode typeCode;
			if (convertible != null)
			{
				typeCode = convertible.GetTypeCode();
			}
			else if (obj1 == null)
			{
				typeCode = TypeCode.Empty;
			}
			else if (obj1 is char[] && ((Array)obj1).Rank == 1)
			{
				typeCode = TypeCode.String;
			}
			else
			{
				typeCode = TypeCode.Object;
			}
			TypeCode typeCode2;
			if (convertible2 != null)
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			else if (obj2 == null)
			{
				typeCode2 = TypeCode.Empty;
			}
			else if (obj2 is char[] && ((Array)obj2).Rank == 1)
			{
				typeCode2 = TypeCode.String;
			}
			else
			{
				typeCode2 = TypeCode.Object;
			}
			if (obj1 == null)
			{
				return typeCode2;
			}
			if (obj2 == null)
			{
				return typeCode;
			}
			if (IsAdd && ((typeCode == TypeCode.DBNull && typeCode2 == TypeCode.String) || (typeCode == TypeCode.String && typeCode2 == TypeCode.DBNull)))
			{
				return TypeCode.DBNull;
			}
			return ObjectType.TypeCodeFromVType(ObjectType.WiderType[(int)ObjectType.VTypeFromTypeCode(typeCode), (int)ObjectType.VTypeFromTypeCode(typeCode2)]);
		}

		internal static TypeCode GetWidestType(object obj1, TypeCode type2)
		{
			IConvertible convertible = obj1 as IConvertible;
			TypeCode typeCode;
			if (convertible != null)
			{
				typeCode = convertible.GetTypeCode();
			}
			else if (obj1 == null)
			{
				typeCode = TypeCode.Empty;
			}
			else if (obj1 is char[] && ((Array)obj1).Rank == 1)
			{
				typeCode = TypeCode.String;
			}
			else
			{
				typeCode = TypeCode.Object;
			}
			if (obj1 == null)
			{
				return type2;
			}
			return ObjectType.TypeCodeFromVType(ObjectType.WiderType[(int)ObjectType.VTypeFromTypeCode(typeCode), (int)ObjectType.VTypeFromTypeCode(type2)]);
		}

		public static int ObjTst(object o1, object o2, bool TextCompare)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object && o1 is char[] && (typeCode2 == TypeCode.String || typeCode2 == TypeCode.Empty || (typeCode2 == TypeCode.Object && o2 is char[])))
			{
				o1 = new string(CharArrayType.FromObject(o1));
				convertible = (IConvertible)o1;
				typeCode = TypeCode.String;
			}
			if (typeCode2 == TypeCode.Object && o2 is char[] && (typeCode == TypeCode.String || typeCode == TypeCode.Empty))
			{
				o2 = new string(CharArrayType.FromObject(o2));
				convertible2 = (IConvertible)o2;
				typeCode2 = TypeCode.String;
			}
			checked
			{
				switch (typeCode * (TypeCode)19 + (int)typeCode2)
				{
				case TypeCode.Empty:
					return 0;
				case TypeCode.Boolean:
					return ObjectType.ObjTstInt32(0, ObjectType.ToVBBool(convertible2));
				case TypeCode.Char:
					return ObjectType.ObjTstChar('\0', convertible2.ToChar(null));
				case TypeCode.Byte:
					return ObjectType.ObjTstByte(0, convertible2.ToByte(null));
				case TypeCode.Int16:
					return ObjectType.ObjTstInt16(0, convertible2.ToInt16(null));
				case TypeCode.Int32:
					return ObjectType.ObjTstInt32(0, convertible2.ToInt32(null));
				case TypeCode.Int64:
					return ObjectType.ObjTstInt64(0L, convertible2.ToInt64(null));
				case TypeCode.Single:
					return ObjectType.ObjTstSingle(0f, convertible2.ToSingle(null));
				case TypeCode.Double:
					return ObjectType.ObjTstDouble(0.0, convertible2.ToDouble(null));
				case TypeCode.Decimal:
					return ObjectType.ObjTstDecimal((IConvertible)0, convertible2);
				case TypeCode.DateTime:
					return ObjectType.ObjTstDateTime(DateType.FromObject(null), convertible2.ToDateTime(null));
				case TypeCode.String:
					return ObjectType.ObjTstStringString(null, o2.ToString(), TextCompare);
				case (TypeCode)57:
					return ObjectType.ObjTstInt32(ObjectType.ToVBBool(convertible), 0);
				case (TypeCode)60:
					return ObjectType.ObjTstInt16((short)ObjectType.ToVBBool(convertible), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)63:
				case (TypeCode)64:
					return ObjectType.ObjTstInt16((short)ObjectType.ToVBBool(convertible), convertible2.ToInt16(null));
				case (TypeCode)66:
					return ObjectType.ObjTstInt32(ObjectType.ToVBBool(convertible), convertible2.ToInt32(null));
				case (TypeCode)68:
					return ObjectType.ObjTstInt64(unchecked((long)ObjectType.ToVBBool(convertible)), convertible2.ToInt64(null));
				case (TypeCode)70:
					return ObjectType.ObjTstSingle((float)ObjectType.ToVBBool(convertible), convertible2.ToSingle(null));
				case (TypeCode)71:
					return ObjectType.ObjTstDouble((double)ObjectType.ToVBBool(convertible), convertible2.ToDouble(null));
				case (TypeCode)72:
					return ObjectType.ObjTstDecimal((IConvertible)ObjectType.ToVBBool(convertible), convertible2);
				case (TypeCode)75:
					return ObjectType.ObjTstBoolean(convertible.ToBoolean(null), BooleanType.FromString(convertible2.ToString(null)));
				case (TypeCode)76:
					return ObjectType.ObjTstChar(convertible.ToChar(null), '\0');
				case (TypeCode)80:
					return ObjectType.ObjTstChar(convertible.ToChar(null), convertible2.ToChar(null));
				case (TypeCode)94:
				case (TypeCode)346:
					return ObjectType.ObjTstStringString(convertible.ToString(null), convertible2.ToString(null), TextCompare);
				case (TypeCode)114:
					return ObjectType.ObjTstByte(convertible.ToByte(null), 0);
				case (TypeCode)117:
				case (TypeCode)136:
					return ObjectType.ObjTstInt16(convertible.ToInt16(null), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)120:
					return ObjectType.ObjTstByte(convertible.ToByte(null), convertible2.ToByte(null));
				case (TypeCode)121:
				case (TypeCode)139:
				case (TypeCode)140:
					return ObjectType.ObjTstInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
				case (TypeCode)123:
				case (TypeCode)142:
				case (TypeCode)177:
				case (TypeCode)178:
				case (TypeCode)180:
					return ObjectType.ObjTstInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
				case (TypeCode)125:
				case (TypeCode)144:
				case (TypeCode)182:
				case (TypeCode)215:
				case (TypeCode)216:
				case (TypeCode)218:
				case (TypeCode)220:
					return ObjectType.ObjTstInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
				case (TypeCode)127:
				case (TypeCode)146:
				case (TypeCode)184:
				case (TypeCode)222:
				case (TypeCode)253:
				case (TypeCode)254:
				case (TypeCode)256:
				case (TypeCode)258:
				case (TypeCode)260:
				case (TypeCode)262:
				case (TypeCode)298:
					return ObjectType.ObjTstSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
				case (TypeCode)128:
				case (TypeCode)147:
				case (TypeCode)185:
				case (TypeCode)223:
				case (TypeCode)261:
				case (TypeCode)272:
				case (TypeCode)273:
				case (TypeCode)275:
				case (TypeCode)277:
				case (TypeCode)279:
				case (TypeCode)280:
				case (TypeCode)281:
				case (TypeCode)299:
					return ObjectType.ObjTstDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
				case (TypeCode)129:
				case (TypeCode)148:
				case (TypeCode)186:
				case (TypeCode)224:
				case (TypeCode)291:
				case (TypeCode)292:
				case (TypeCode)294:
				case (TypeCode)296:
				case (TypeCode)300:
					return ObjectType.ObjTstDecimal(convertible, convertible2);
				case (TypeCode)132:
				case (TypeCode)151:
				case (TypeCode)189:
				case (TypeCode)227:
				case (TypeCode)265:
				case (TypeCode)284:
				case (TypeCode)303:
					return ObjectType.ObjTstString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)133:
					return ObjectType.ObjTstInt16(convertible.ToInt16(null), 0);
				case (TypeCode)171:
					return ObjectType.ObjTstInt32(convertible.ToInt32(null), 0);
				case (TypeCode)174:
					return ObjectType.ObjTstInt32(convertible.ToInt32(null), ObjectType.ToVBBool(convertible2));
				case (TypeCode)209:
					return ObjectType.ObjTstInt64(convertible.ToInt64(null), 0L);
				case (TypeCode)212:
					return ObjectType.ObjTstInt64(convertible.ToInt64(null), unchecked((long)ObjectType.ToVBBool(convertible2)));
				case (TypeCode)247:
					return ObjectType.ObjTstSingle(convertible.ToSingle(null), 0f);
				case (TypeCode)250:
					return ObjectType.ObjTstSingle(convertible.ToSingle(null), (float)ObjectType.ToVBBool(convertible2));
				case (TypeCode)266:
					return ObjectType.ObjTstDouble(convertible.ToDouble(null), 0.0);
				case (TypeCode)269:
					return ObjectType.ObjTstDouble(convertible.ToDouble(null), (double)ObjectType.ToVBBool(convertible2));
				case (TypeCode)285:
					return ObjectType.ObjTstDecimal(convertible, (IConvertible)0);
				case (TypeCode)288:
					return ObjectType.ObjTstDecimal(convertible, (IConvertible)ObjectType.ToVBBool(convertible2));
				case (TypeCode)304:
					return ObjectType.ObjTstDateTime(convertible.ToDateTime(null), DateType.FromObject(null));
				case (TypeCode)320:
					return ObjectType.ObjTstDateTime(convertible.ToDateTime(null), convertible2.ToDateTime(null));
				case (TypeCode)322:
					return ObjectType.ObjTstDateTime(convertible.ToDateTime(null), DateType.FromString(convertible2.ToString(null), Utils.GetCultureInfo()));
				case (TypeCode)342:
					return ObjectType.ObjTstStringString(o1.ToString(), null, TextCompare);
				case (TypeCode)345:
					return ObjectType.ObjTstBoolean(BooleanType.FromString(convertible.ToString(null)), convertible2.ToBoolean(null));
				case (TypeCode)348:
				case (TypeCode)349:
				case (TypeCode)351:
				case (TypeCode)353:
				case (TypeCode)355:
				case (TypeCode)356:
				case (TypeCode)357:
					return ObjectType.ObjTstString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)358:
					return ObjectType.ObjTstDateTime(DateType.FromString(convertible.ToString(null), Utils.GetCultureInfo()), convertible2.ToDateTime(null));
				case (TypeCode)360:
					return ObjectType.ObjTstStringString(convertible.ToString(null), convertible2.ToString(null), TextCompare);
				}
				throw ObjectType.GetNoValidOperatorException(o1, o2);
			}
		}

		private static int ObjTstDateTime(DateTime var1, DateTime var2)
		{
			long ticks = var1.Ticks;
			long ticks2 = var2.Ticks;
			if (ticks < ticks2)
			{
				return -1;
			}
			if (ticks > ticks2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstBoolean(bool b1, bool b2)
		{
			if (b1 == b2)
			{
				return 0;
			}
			if (b1 < b2)
			{
				return 1;
			}
			return -1;
		}

		private static int ObjTstDouble(double d1, double d2)
		{
			if (d1 < d2)
			{
				return -1;
			}
			if (d1 > d2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstChar(char ch1, char ch2)
		{
			if (ch1 < ch2)
			{
				return -1;
			}
			if (ch1 > ch2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstByte(byte by1, byte by2)
		{
			if (by1 < by2)
			{
				return -1;
			}
			if (by1 > by2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstSingle(float d1, float d2)
		{
			if (d1 < d2)
			{
				return -1;
			}
			if (d1 > d2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstInt16(short d1, short d2)
		{
			if (d1 < d2)
			{
				return -1;
			}
			if (d1 > d2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstInt32(int d1, int d2)
		{
			if (d1 < d2)
			{
				return -1;
			}
			if (d1 > d2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstInt64(long d1, long d2)
		{
			if (d1 < d2)
			{
				return -1;
			}
			if (d1 > d2)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstDecimal(IConvertible i1, IConvertible i2)
		{
			decimal num = i1.ToDecimal(null);
			decimal num2 = i2.ToDecimal(null);
			if (decimal.Compare(num, num2) < 0)
			{
				return -1;
			}
			if (decimal.Compare(num, num2) > 0)
			{
				return 1;
			}
			return 0;
		}

		private static int ObjTstString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			double num;
			if (tc1 == TypeCode.String)
			{
				num = DoubleType.FromString(conv1.ToString(null));
			}
			else if (tc1 == TypeCode.Boolean)
			{
				num = (double)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToDouble(null);
			}
			double num2;
			if (tc2 == TypeCode.String)
			{
				num2 = DoubleType.FromString(conv2.ToString(null));
			}
			else if (tc2 == TypeCode.Boolean)
			{
				num2 = (double)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToDouble(null);
			}
			return ObjectType.ObjTstDouble(num, num2);
		}

		private static int ObjTstStringString(string s1, string s2, bool TextCompare)
		{
			if (s1 == null)
			{
				if (s2.Length > 0)
				{
					return -1;
				}
				return 0;
			}
			else if (s2 == null)
			{
				if (s1.Length > 0)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (TextCompare)
				{
					return Utils.GetCultureInfo().CompareInfo.Compare(s1, s2, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
				}
				return string.CompareOrdinal(s1, s2);
			}
		}

		public static object PlusObj(object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			IConvertible convertible = obj as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (obj == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return 0;
			case TypeCode.Boolean:
				if (obj is bool)
				{
					return (-(((bool)obj > false) ? 1 : 0)) ? 1 : 0;
				}
				return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return obj;
			case TypeCode.String:
				return DoubleType.FromObject(obj);
			}
			throw ObjectType.GetNoValidOperatorException(obj);
		}

		public static object NegObj(object obj)
		{
			IConvertible convertible = obj as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (obj == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			return ObjectType.InternalNegObj(obj, convertible, typeCode);
		}

		private static object InternalNegObj(object obj, IConvertible conv, TypeCode tc)
		{
			short num;
			double num5;
			checked
			{
				switch (tc)
				{
				case TypeCode.Empty:
					return 0;
				case TypeCode.Object:
				case TypeCode.DBNull:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.DateTime:
				case (TypeCode)17:
					goto IL_0232;
				case TypeCode.Boolean:
					unchecked
					{
						if (obj is bool)
						{
							num = -((-(((bool)obj > false) ? 1 : 0)) ? 1 : 0);
							goto IL_027D;
						}
						num = -((-((conv.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0);
						goto IL_027D;
					}
				case TypeCode.Char:
					goto IL_0232;
				case TypeCode.Byte:
					if (obj is byte)
					{
						num = (short)unchecked(-(short)((byte)obj));
						goto IL_027D;
					}
					num = (short)unchecked(-(short)conv.ToByte(null));
					goto IL_027D;
				case TypeCode.Int16:
				{
					int num2;
					if (obj is short)
					{
						num2 = (int)(0 - (short)obj);
					}
					else
					{
						num2 = (int)(0 - conv.ToInt16(null));
					}
					if (num2 < -32768 || num2 > 32767)
					{
						return num2;
					}
					return (short)num2;
				}
				case TypeCode.Int32:
				{
					long num3;
					if (obj is int)
					{
						num3 = 0L - unchecked((long)((int)obj));
					}
					else
					{
						num3 = 0L - unchecked((long)conv.ToInt32(null));
					}
					if (num3 < -2147483648L || num3 > 2147483647L)
					{
						return num3;
					}
					return (int)num3;
				}
				case TypeCode.Int64:
				{
					long num3;
					decimal num4;
					try
					{
						if (obj is long)
						{
							num3 = 0L - (long)obj;
							goto IL_0284;
						}
						num3 = 0L - conv.ToInt64(null);
						goto IL_0284;
					}
					catch (StackOverflowException ex)
					{
						throw ex;
					}
					catch (OutOfMemoryException ex2)
					{
						throw ex2;
					}
					catch (ThreadAbortException ex3)
					{
						throw ex3;
					}
					catch (Exception)
					{
						num4 = decimal.Negate(conv.ToDecimal(null));
						goto IL_0295;
					}
					break;
					IL_0284:
					return num3;
					IL_0295:
					return num4;
				}
				case TypeCode.Single:
					goto IL_01B9;
				case TypeCode.Double:
					unchecked
					{
						if (obj is double)
						{
							num5 = -(double)obj;
							goto IL_028D;
						}
						num5 = -conv.ToDouble(null);
						goto IL_028D;
					}
				case TypeCode.Decimal:
					break;
				case TypeCode.String:
				{
					string text = obj as string;
					unchecked
					{
						if (text != null)
						{
							num5 = -DoubleType.FromString(text);
							goto IL_028D;
						}
						num5 = -DoubleType.FromString(conv.ToString(null));
						goto IL_028D;
					}
				}
				default:
					goto IL_0232;
				}
			}
			try
			{
				decimal num4;
				if (obj is decimal)
				{
					num4 = decimal.Negate((decimal)obj);
				}
				else
				{
					num4 = decimal.Negate(conv.ToDecimal(null));
				}
				return num4;
			}
			catch (StackOverflowException ex4)
			{
				throw ex4;
			}
			catch (OutOfMemoryException ex5)
			{
				throw ex5;
			}
			catch (ThreadAbortException ex6)
			{
				throw ex6;
			}
			catch (Exception)
			{
				num5 = -conv.ToDouble(null);
				goto IL_028D;
			}
			IL_01B9:
			if (obj is float)
			{
				return -(float)obj;
			}
			return -conv.ToSingle(null);
			IL_0232:
			throw ObjectType.GetNoValidOperatorException(obj);
			IL_027D:
			return num;
			IL_028D:
			return num5;
		}

		public static object NotObj(object obj)
		{
			if (obj == null)
			{
				return -1;
			}
			IConvertible convertible = obj as IConvertible;
			TypeCode typeCode;
			if (convertible != null)
			{
				typeCode = convertible.GetTypeCode();
			}
			else
			{
				typeCode = TypeCode.Object;
			}
			switch (typeCode)
			{
			case TypeCode.Boolean:
				return !convertible.ToBoolean(null);
			case TypeCode.Byte:
			{
				Type type = obj.GetType();
				byte b = ~convertible.ToByte(null);
				if (type.IsEnum)
				{
					return Enum.ToObject(type, b);
				}
				return b;
			}
			case TypeCode.Int16:
			{
				Type type = obj.GetType();
				short num = ~convertible.ToInt16(null);
				if (type.IsEnum)
				{
					return Enum.ToObject(type, num);
				}
				return num;
			}
			case TypeCode.Int32:
			{
				Type type = obj.GetType();
				int num2 = ~convertible.ToInt32(null);
				if (type.IsEnum)
				{
					return Enum.ToObject(type, num2);
				}
				return num2;
			}
			case TypeCode.Int64:
			{
				Type type = obj.GetType();
				long num3 = ~convertible.ToInt64(null);
				if (type.IsEnum)
				{
					return Enum.ToObject(type, num3);
				}
				return num3;
			}
			case TypeCode.Single:
				return ~Convert.ToInt64(convertible.ToDecimal(null));
			case TypeCode.Double:
				return ~Convert.ToInt64(convertible.ToDecimal(null));
			case TypeCode.Decimal:
				return ~Convert.ToInt64(convertible.ToDecimal(null));
			case TypeCode.String:
				return ~LongType.FromString(convertible.ToString(null));
			}
			throw ObjectType.GetNoValidOperatorException(obj);
		}

		public static object BitAndObj(object obj1, object obj2)
		{
			if (obj1 == null && obj2 == null)
			{
				return 0;
			}
			Type type = null;
			Type type2 = null;
			bool isEnum;
			if (obj1 != null)
			{
				type = obj1.GetType();
				isEnum = type.IsEnum;
			}
			bool isEnum2;
			if (obj2 != null)
			{
				type2 = obj2.GetType();
				isEnum2 = type2.IsEnum;
			}
			switch (ObjectType.GetWidestType(obj1, obj2, false))
			{
			case TypeCode.Boolean:
				if (type == type2)
				{
					return BooleanType.FromObject(obj1) & BooleanType.FromObject(obj2);
				}
				return ShortType.FromObject(obj1) & ShortType.FromObject(obj2);
			case TypeCode.Byte:
			{
				byte b = ByteType.FromObject(obj1) & ByteType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, b);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, b);
						}
						break;
					}
				}
				return b;
			}
			case TypeCode.Int16:
			{
				short num = ShortType.FromObject(obj1) & ShortType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num);
						}
						break;
					}
				}
				return num;
			}
			case TypeCode.Int32:
			{
				int num2 = IntegerType.FromObject(obj1) & IntegerType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num2);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num2);
						}
						break;
					}
				}
				return num2;
			}
			case TypeCode.Int64:
			{
				long num3 = LongType.FromObject(obj1) & LongType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num3);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num3);
						}
						break;
					}
				}
				return num3;
			}
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
				return LongType.FromObject(obj1) & LongType.FromObject(obj2);
			}
			throw ObjectType.GetNoValidOperatorException(obj1, obj2);
		}

		public static object BitOrObj(object obj1, object obj2)
		{
			if (obj1 == null && obj2 == null)
			{
				return 0;
			}
			Type type = null;
			Type type2 = null;
			bool isEnum;
			if (obj1 != null)
			{
				type = obj1.GetType();
				isEnum = type.IsEnum;
			}
			bool isEnum2;
			if (obj2 != null)
			{
				type2 = obj2.GetType();
				isEnum2 = type2.IsEnum;
			}
			switch (ObjectType.GetWidestType(obj1, obj2, false))
			{
			case TypeCode.Boolean:
				if (type == type2)
				{
					return BooleanType.FromObject(obj1) | BooleanType.FromObject(obj2);
				}
				return ShortType.FromObject(obj1) | ShortType.FromObject(obj2);
			case TypeCode.Byte:
			{
				byte b = ByteType.FromObject(obj1) | ByteType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, b);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, b);
						}
						break;
					}
				}
				return b;
			}
			case TypeCode.Int16:
			{
				short num = ShortType.FromObject(obj1) | ShortType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num);
						}
						break;
					}
				}
				return num;
			}
			case TypeCode.Int32:
			{
				int num2 = IntegerType.FromObject(obj1) | IntegerType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num2);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num2);
						}
						break;
					}
				}
				return num2;
			}
			case TypeCode.Int64:
			{
				long num3 = LongType.FromObject(obj1) | LongType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num3);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num3);
						}
						break;
					}
				}
				return num3;
			}
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
				return LongType.FromObject(obj1) | LongType.FromObject(obj2);
			}
			throw ObjectType.GetNoValidOperatorException(obj1, obj2);
		}

		public static object BitXorObj(object obj1, object obj2)
		{
			if (obj1 == null && obj2 == null)
			{
				return 0;
			}
			Type type = null;
			Type type2 = null;
			bool isEnum;
			if (obj1 != null)
			{
				type = obj1.GetType();
				isEnum = type.IsEnum;
			}
			bool isEnum2;
			if (obj2 != null)
			{
				type2 = obj2.GetType();
				isEnum2 = type2.IsEnum;
			}
			switch (ObjectType.GetWidestType(obj1, obj2, false))
			{
			case TypeCode.Boolean:
				if (type == type2)
				{
					return BooleanType.FromObject(obj1) ^ BooleanType.FromObject(obj2);
				}
				return ShortType.FromObject(obj1) ^ ShortType.FromObject(obj2);
			case TypeCode.Byte:
			{
				byte b = ByteType.FromObject(obj1) ^ ByteType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, b);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, b);
						}
						break;
					}
				}
				return b;
			}
			case TypeCode.Int16:
			{
				short num = ShortType.FromObject(obj1) ^ ShortType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num);
						}
						break;
					}
				}
				return num;
			}
			case TypeCode.Int32:
			{
				int num2 = IntegerType.FromObject(obj1) ^ IntegerType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num2);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num2);
						}
						break;
					}
				}
				return num2;
			}
			case TypeCode.Int64:
			{
				long num3 = LongType.FromObject(obj1) ^ LongType.FromObject(obj2);
				if (!isEnum || !isEnum2 || type == type2)
				{
					if (isEnum && isEnum2)
					{
						if (isEnum)
						{
							return Enum.ToObject(type, num3);
						}
						if (isEnum2)
						{
							return Enum.ToObject(type2, num3);
						}
						break;
					}
				}
				return num3;
			}
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
				return LongType.FromObject(obj1) ^ LongType.FromObject(obj2);
			}
			throw ObjectType.GetNoValidOperatorException(obj1, obj2);
		}

		public static object AddObj(object o1, object o2)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object && o1 is char[] && (typeCode2 == TypeCode.String || typeCode2 == TypeCode.Empty || (typeCode2 == TypeCode.Object && o2 is char[])))
			{
				o1 = new string(CharArrayType.FromObject(o1));
				convertible = (IConvertible)o1;
				typeCode = TypeCode.String;
			}
			if (typeCode2 == TypeCode.Object && o2 is char[] && (typeCode == TypeCode.String || typeCode == TypeCode.Empty))
			{
				o2 = new string(CharArrayType.FromObject(o2));
				convertible2 = (IConvertible)o2;
				typeCode2 = TypeCode.String;
			}
			checked
			{
				switch (typeCode * (TypeCode)19 + (int)typeCode2)
				{
				case TypeCode.Empty:
					return 0;
				case TypeCode.Boolean:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return o2;
				case TypeCode.String:
				case (TypeCode)56:
					return o2;
				case (TypeCode)57:
				case (TypeCode)114:
				case (TypeCode)133:
				case (TypeCode)171:
				case (TypeCode)209:
				case (TypeCode)247:
				case (TypeCode)266:
				case (TypeCode)285:
					return o1;
				case (TypeCode)60:
					return ObjectType.AddInt16((short)ObjectType.ToVBBool(convertible), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)63:
				case (TypeCode)64:
					return ObjectType.AddInt16((short)ObjectType.ToVBBool(convertible), convertible2.ToInt16(null));
				case (TypeCode)66:
					return ObjectType.AddInt32(ObjectType.ToVBBool(convertible), convertible2.ToInt32(null));
				case (TypeCode)68:
					return ObjectType.AddInt64(unchecked((long)ObjectType.ToVBBool(convertible)), convertible2.ToInt64(null));
				case (TypeCode)70:
					return ObjectType.AddSingle((float)ObjectType.ToVBBool(convertible), convertible2.ToSingle(null));
				case (TypeCode)71:
					return ObjectType.AddDouble((double)ObjectType.ToVBBool(convertible), convertible2.ToDouble(null));
				case (TypeCode)72:
					return ObjectType.AddDecimal(ObjectType.ToVBBoolConv(convertible), convertible2);
				case (TypeCode)75:
				case (TypeCode)345:
					return ObjectType.AddString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)80:
				case (TypeCode)94:
				case (TypeCode)320:
				case (TypeCode)322:
				case (TypeCode)346:
				case (TypeCode)358:
				case (TypeCode)360:
					return StringType.FromObject(o1) + StringType.FromObject(o2);
				case (TypeCode)117:
				case (TypeCode)136:
					return ObjectType.AddInt16(convertible.ToInt16(null), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)120:
					return ObjectType.AddByte(convertible.ToByte(null), convertible2.ToByte(null));
				case (TypeCode)121:
				case (TypeCode)139:
				case (TypeCode)140:
					return ObjectType.AddInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
				case (TypeCode)123:
				case (TypeCode)142:
				case (TypeCode)177:
				case (TypeCode)178:
				case (TypeCode)180:
					return ObjectType.AddInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
				case (TypeCode)125:
				case (TypeCode)144:
				case (TypeCode)182:
				case (TypeCode)215:
				case (TypeCode)216:
				case (TypeCode)218:
				case (TypeCode)220:
					return ObjectType.AddInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
				case (TypeCode)127:
				case (TypeCode)146:
				case (TypeCode)184:
				case (TypeCode)222:
				case (TypeCode)253:
				case (TypeCode)254:
				case (TypeCode)256:
				case (TypeCode)258:
				case (TypeCode)260:
				case (TypeCode)262:
				case (TypeCode)298:
					return ObjectType.AddSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
				case (TypeCode)128:
				case (TypeCode)147:
				case (TypeCode)185:
				case (TypeCode)223:
				case (TypeCode)261:
				case (TypeCode)272:
				case (TypeCode)273:
				case (TypeCode)275:
				case (TypeCode)277:
				case (TypeCode)279:
				case (TypeCode)280:
				case (TypeCode)281:
				case (TypeCode)299:
					return ObjectType.AddDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
				case (TypeCode)129:
				case (TypeCode)148:
				case (TypeCode)186:
				case (TypeCode)224:
				case (TypeCode)291:
				case (TypeCode)292:
				case (TypeCode)294:
				case (TypeCode)296:
				case (TypeCode)300:
					return ObjectType.AddDecimal(convertible, convertible2);
				case (TypeCode)132:
				case (TypeCode)151:
				case (TypeCode)189:
				case (TypeCode)227:
				case (TypeCode)265:
				case (TypeCode)284:
				case (TypeCode)303:
					return ObjectType.AddString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)174:
					return ObjectType.AddInt32(convertible.ToInt32(null), ObjectType.ToVBBool(convertible2));
				case (TypeCode)212:
					return ObjectType.AddInt64(convertible.ToInt64(null), unchecked((long)ObjectType.ToVBBool(convertible2)));
				case (TypeCode)250:
					return ObjectType.AddSingle(convertible.ToSingle(null), (float)ObjectType.ToVBBool(convertible2));
				case (TypeCode)269:
					return ObjectType.AddDouble(convertible.ToDouble(null), (double)ObjectType.ToVBBool(convertible2));
				case (TypeCode)288:
					return ObjectType.AddDecimal(convertible, ObjectType.ToVBBoolConv(convertible2));
				case (TypeCode)342:
				case (TypeCode)344:
					return o1;
				case (TypeCode)348:
				case (TypeCode)349:
				case (TypeCode)351:
				case (TypeCode)353:
				case (TypeCode)355:
				case (TypeCode)356:
				case (TypeCode)357:
					return ObjectType.AddString(convertible, typeCode, convertible2, typeCode2);
				}
				throw ObjectType.GetNoValidOperatorException(o1, o2);
			}
		}

		private static object AddString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			double num;
			if (tc1 == TypeCode.String)
			{
				num = DoubleType.FromString(conv1.ToString(null));
			}
			else if (tc1 == TypeCode.Boolean)
			{
				num = (double)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToDouble(null);
			}
			double num2;
			if (tc2 == TypeCode.String)
			{
				num2 = DoubleType.FromString(conv2.ToString(null));
			}
			else if (tc2 == TypeCode.Boolean)
			{
				num2 = (double)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToDouble(null);
			}
			return num + num2;
		}

		private static object AddByte(byte i1, byte i2)
		{
			checked
			{
				short num = (short)(unchecked(i1 + i2));
				if (num >= 0 && num <= 255)
				{
					return (byte)num;
				}
				return num;
			}
		}

		private static object AddInt16(short i1, short i2)
		{
			checked
			{
				int num = (int)(i1 + i2);
				if (num >= -32768 && num <= 32767)
				{
					return (short)num;
				}
				return num;
			}
		}

		private static object AddInt32(int i1, int i2)
		{
			checked
			{
				long num = unchecked((long)i1) + unchecked((long)i2);
				if (num >= -2147483648L && num <= 2147483647L)
				{
					return (int)num;
				}
				return num;
			}
		}

		private static object AddInt64(long i1, long i2)
		{
			object obj;
			try
			{
				obj = checked(i1 + i2);
			}
			catch (OverflowException ex)
			{
				obj = decimal.Add(new decimal(i1), new decimal(i2));
			}
			return obj;
		}

		private static object AddSingle(float f1, float f2)
		{
			double num = (double)f1 + (double)f2;
			if (num <= 3.4028234663852886E+38 && num >= -3.4028234663852886E+38)
			{
				return (float)num;
			}
			if (double.IsInfinity(num) && (float.IsInfinity(f1) || float.IsInfinity(f2)))
			{
				return (float)num;
			}
			return num;
		}

		private static object AddDouble(double d1, double d2)
		{
			return d1 + d2;
		}

		private static object AddDecimal(IConvertible conv1, IConvertible conv2)
		{
			decimal num;
			if (conv1 != null)
			{
				num = conv1.ToDecimal(null);
			}
			decimal num2 = conv2.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Add(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToDouble(num) + Convert.ToDouble(num2);
			}
			return obj;
		}

		private static int ToVBBool(IConvertible conv)
		{
			if (conv.ToBoolean(null))
			{
				return -1;
			}
			return 0;
		}

		private static IConvertible ToVBBoolConv(IConvertible conv)
		{
			if (conv.ToBoolean(null))
			{
				return (IConvertible)(-1);
			}
			return (IConvertible)0;
		}

		public static object SubObj(object o1, object o2)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			checked
			{
				switch (typeCode * (TypeCode)19 + (int)typeCode2)
				{
				case TypeCode.Empty:
					return 0;
				case TypeCode.Boolean:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return ObjectType.InternalNegObj(o2, convertible2, typeCode2);
				case TypeCode.String:
					return ObjectType.SubStringString(null, convertible2.ToString(null));
				case (TypeCode)57:
				case (TypeCode)114:
				case (TypeCode)133:
				case (TypeCode)171:
				case (TypeCode)209:
				case (TypeCode)247:
				case (TypeCode)266:
				case (TypeCode)285:
					return o1;
				case (TypeCode)60:
					return ObjectType.SubInt16((short)ObjectType.ToVBBool(convertible), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)63:
				case (TypeCode)64:
					return ObjectType.SubInt16((short)ObjectType.ToVBBool(convertible), convertible2.ToInt16(null));
				case (TypeCode)66:
					return ObjectType.SubInt32(ObjectType.ToVBBool(convertible), convertible2.ToInt32(null));
				case (TypeCode)68:
					return ObjectType.SubInt64(unchecked((long)ObjectType.ToVBBool(convertible)), convertible2.ToInt64(null));
				case (TypeCode)70:
					return ObjectType.SubSingle((float)ObjectType.ToVBBool(convertible), convertible2.ToSingle(null));
				case (TypeCode)71:
					return ObjectType.SubDouble((double)ObjectType.ToVBBool(convertible), convertible2.ToDouble(null));
				case (TypeCode)72:
					return ObjectType.SubDecimal(ObjectType.ToVBBoolConv(convertible), convertible2);
				case (TypeCode)75:
				case (TypeCode)132:
				case (TypeCode)151:
				case (TypeCode)189:
				case (TypeCode)227:
				case (TypeCode)265:
				case (TypeCode)284:
				case (TypeCode)303:
				case (TypeCode)345:
				case (TypeCode)348:
				case (TypeCode)349:
				case (TypeCode)351:
				case (TypeCode)353:
				case (TypeCode)355:
				case (TypeCode)356:
				case (TypeCode)357:
					return ObjectType.SubString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)117:
				case (TypeCode)136:
					return ObjectType.SubInt16(convertible.ToInt16(null), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)120:
					return ObjectType.SubByte(convertible.ToByte(null), convertible2.ToByte(null));
				case (TypeCode)121:
				case (TypeCode)139:
				case (TypeCode)140:
					return ObjectType.SubInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
				case (TypeCode)123:
				case (TypeCode)142:
				case (TypeCode)177:
				case (TypeCode)178:
				case (TypeCode)180:
					return ObjectType.SubInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
				case (TypeCode)125:
				case (TypeCode)144:
				case (TypeCode)182:
				case (TypeCode)215:
				case (TypeCode)216:
				case (TypeCode)218:
				case (TypeCode)220:
					return ObjectType.SubInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
				case (TypeCode)127:
				case (TypeCode)146:
				case (TypeCode)184:
				case (TypeCode)222:
				case (TypeCode)253:
				case (TypeCode)254:
				case (TypeCode)256:
				case (TypeCode)258:
				case (TypeCode)260:
				case (TypeCode)262:
				case (TypeCode)298:
					return ObjectType.SubSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
				case (TypeCode)128:
				case (TypeCode)147:
				case (TypeCode)185:
				case (TypeCode)223:
				case (TypeCode)261:
				case (TypeCode)272:
				case (TypeCode)273:
				case (TypeCode)275:
				case (TypeCode)277:
				case (TypeCode)279:
				case (TypeCode)280:
				case (TypeCode)281:
				case (TypeCode)299:
					return ObjectType.SubDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
				case (TypeCode)129:
				case (TypeCode)148:
				case (TypeCode)186:
				case (TypeCode)224:
				case (TypeCode)291:
				case (TypeCode)292:
				case (TypeCode)294:
				case (TypeCode)296:
				case (TypeCode)300:
					return ObjectType.SubDecimal(convertible, convertible2);
				case (TypeCode)174:
					return ObjectType.SubInt32(convertible.ToInt32(null), ObjectType.ToVBBool(convertible2));
				case (TypeCode)212:
					return ObjectType.SubInt64(convertible.ToInt64(null), unchecked((long)ObjectType.ToVBBool(convertible2)));
				case (TypeCode)250:
					return ObjectType.SubSingle(convertible.ToSingle(null), (float)ObjectType.ToVBBool(convertible2));
				case (TypeCode)269:
					return ObjectType.SubDouble(convertible.ToDouble(null), (double)ObjectType.ToVBBool(convertible2));
				case (TypeCode)288:
					return ObjectType.SubDecimal(convertible, ObjectType.ToVBBoolConv(convertible2));
				case (TypeCode)342:
					return ObjectType.SubStringString(convertible.ToString(null), null);
				case (TypeCode)360:
					return ObjectType.SubStringString(convertible.ToString(null), convertible2.ToString(null));
				}
				throw ObjectType.GetNoValidOperatorException(o1, o2);
			}
		}

		private static object SubString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			double num;
			if (tc1 == TypeCode.String)
			{
				num = DoubleType.FromString(conv1.ToString(null));
			}
			else if (tc1 == TypeCode.Boolean)
			{
				num = (double)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToDouble(null);
			}
			double num2;
			if (tc2 == TypeCode.String)
			{
				num2 = DoubleType.FromString(conv2.ToString(null));
			}
			else if (tc2 == TypeCode.Boolean)
			{
				num2 = (double)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToDouble(null);
			}
			return num - num2;
		}

		private static object SubStringString(string s1, string s2)
		{
			double num;
			if (s1 != null)
			{
				num = DoubleType.FromString(s1);
			}
			double num2;
			if (s2 != null)
			{
				num2 = DoubleType.FromString(s2);
			}
			return num - num2;
		}

		private static object SubByte(byte i1, byte i2)
		{
			checked
			{
				short num = (short)(unchecked(i1 - i2));
				if (num >= 0 && num <= 255)
				{
					return (byte)num;
				}
				return num;
			}
		}

		private static object SubInt16(short i1, short i2)
		{
			checked
			{
				int num = (int)(i1 - i2);
				if (num >= -32768 && num <= 32767)
				{
					return (short)num;
				}
				return num;
			}
		}

		private static object SubInt32(int i1, int i2)
		{
			checked
			{
				long num = unchecked((long)i1) - unchecked((long)i2);
				if (num >= -2147483648L && num <= 2147483647L)
				{
					return (int)num;
				}
				return num;
			}
		}

		private static object SubInt64(long i1, long i2)
		{
			object obj;
			try
			{
				obj = checked(i1 - i2);
			}
			catch (StackOverflowException ex)
			{
				throw ex;
			}
			catch (OutOfMemoryException ex2)
			{
				throw ex2;
			}
			catch (ThreadAbortException ex3)
			{
				throw ex3;
			}
			catch (Exception)
			{
				obj = decimal.Subtract(new decimal(i1), new decimal(i2));
			}
			return obj;
		}

		private static object SubSingle(float f1, float f2)
		{
			double num = (double)f1 - (double)f2;
			if (num <= 3.4028234663852886E+38 && num >= -3.4028234663852886E+38)
			{
				return (float)num;
			}
			if (double.IsInfinity(num) && (float.IsInfinity(f1) || float.IsInfinity(f2)))
			{
				return (float)num;
			}
			return num;
		}

		private static object SubDouble(double d1, double d2)
		{
			return d1 - d2;
		}

		private static object SubDecimal(IConvertible conv1, IConvertible conv2)
		{
			decimal num = conv1.ToDecimal(null);
			decimal num2 = conv2.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Subtract(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToDouble(num) - Convert.ToDouble(num2);
			}
			return obj;
		}

		public static object MulObj(object o1, object o2)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			checked
			{
				switch (typeCode * (TypeCode)19 + (int)typeCode2)
				{
				case TypeCode.Empty:
				case TypeCode.Int32:
				case (TypeCode)171:
					return 0;
				case TypeCode.Boolean:
				case TypeCode.Int16:
				case (TypeCode)57:
				case (TypeCode)133:
					return 0;
				case TypeCode.Byte:
				case (TypeCode)114:
					return 0;
				case TypeCode.Int64:
				case (TypeCode)209:
					return 0L;
				case TypeCode.Single:
				case (TypeCode)247:
					return 0f;
				case TypeCode.Double:
				case (TypeCode)266:
					return 0.0;
				case TypeCode.Decimal:
				case (TypeCode)285:
					return 0m;
				case TypeCode.String:
				case (TypeCode)342:
					return 0.0;
				case (TypeCode)60:
					return ObjectType.MulInt16((short)ObjectType.ToVBBool(convertible), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)63:
				case (TypeCode)64:
					return ObjectType.MulInt16((short)ObjectType.ToVBBool(convertible), convertible2.ToInt16(null));
				case (TypeCode)66:
					return ObjectType.MulInt32(ObjectType.ToVBBool(convertible), convertible2.ToInt32(null));
				case (TypeCode)68:
					return ObjectType.MulInt64(unchecked((long)ObjectType.ToVBBool(convertible)), convertible2.ToInt64(null));
				case (TypeCode)70:
					return ObjectType.MulSingle((float)ObjectType.ToVBBool(convertible), convertible2.ToSingle(null));
				case (TypeCode)71:
					return ObjectType.MulDouble((double)ObjectType.ToVBBool(convertible), convertible2.ToDouble(null));
				case (TypeCode)72:
					return ObjectType.MulDecimal(ObjectType.ToVBBoolConv(convertible), convertible2);
				case (TypeCode)75:
				case (TypeCode)132:
				case (TypeCode)151:
				case (TypeCode)189:
				case (TypeCode)227:
				case (TypeCode)265:
				case (TypeCode)284:
				case (TypeCode)303:
				case (TypeCode)345:
				case (TypeCode)348:
				case (TypeCode)349:
				case (TypeCode)351:
				case (TypeCode)353:
				case (TypeCode)355:
				case (TypeCode)356:
				case (TypeCode)357:
					return ObjectType.MulString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)117:
				case (TypeCode)136:
					return ObjectType.MulInt16(convertible.ToInt16(null), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)120:
					return ObjectType.MulByte(convertible.ToByte(null), convertible2.ToByte(null));
				case (TypeCode)121:
				case (TypeCode)139:
				case (TypeCode)140:
					return ObjectType.MulInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
				case (TypeCode)123:
				case (TypeCode)142:
				case (TypeCode)177:
				case (TypeCode)178:
				case (TypeCode)180:
					return ObjectType.MulInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
				case (TypeCode)125:
				case (TypeCode)144:
				case (TypeCode)182:
				case (TypeCode)215:
				case (TypeCode)216:
				case (TypeCode)218:
				case (TypeCode)220:
					return ObjectType.MulInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
				case (TypeCode)127:
				case (TypeCode)146:
				case (TypeCode)184:
				case (TypeCode)222:
				case (TypeCode)253:
				case (TypeCode)254:
				case (TypeCode)256:
				case (TypeCode)258:
				case (TypeCode)260:
				case (TypeCode)262:
				case (TypeCode)298:
					return ObjectType.MulSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
				case (TypeCode)128:
				case (TypeCode)147:
				case (TypeCode)185:
				case (TypeCode)223:
				case (TypeCode)261:
				case (TypeCode)272:
				case (TypeCode)273:
				case (TypeCode)275:
				case (TypeCode)277:
				case (TypeCode)279:
				case (TypeCode)280:
				case (TypeCode)281:
				case (TypeCode)299:
					return ObjectType.MulDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
				case (TypeCode)129:
				case (TypeCode)148:
				case (TypeCode)186:
				case (TypeCode)224:
				case (TypeCode)291:
				case (TypeCode)292:
				case (TypeCode)294:
				case (TypeCode)296:
				case (TypeCode)300:
					return ObjectType.MulDecimal(convertible, convertible2);
				case (TypeCode)174:
					return ObjectType.MulInt32(convertible.ToInt32(null), ObjectType.ToVBBool(convertible2));
				case (TypeCode)212:
					return ObjectType.MulInt64(convertible.ToInt64(null), unchecked((long)ObjectType.ToVBBool(convertible2)));
				case (TypeCode)250:
					return ObjectType.MulSingle(convertible.ToSingle(null), (float)ObjectType.ToVBBool(convertible2));
				case (TypeCode)269:
					return ObjectType.MulDouble(convertible.ToDouble(null), (double)ObjectType.ToVBBool(convertible2));
				case (TypeCode)288:
					return ObjectType.MulDecimal(convertible, ObjectType.ToVBBoolConv(convertible2));
				case (TypeCode)360:
					return ObjectType.MulStringString(convertible.ToString(null), convertible2.ToString(null));
				}
				throw ObjectType.GetNoValidOperatorException(o1, o2);
			}
		}

		private static object MulString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			double num;
			if (tc1 == TypeCode.String)
			{
				num = DoubleType.FromString(conv1.ToString(null));
			}
			else if (tc1 == TypeCode.Boolean)
			{
				num = (double)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToDouble(null);
			}
			double num2;
			if (tc2 == TypeCode.String)
			{
				num2 = DoubleType.FromString(conv2.ToString(null));
			}
			else if (tc2 == TypeCode.Boolean)
			{
				num2 = (double)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToDouble(null);
			}
			return num * num2;
		}

		private static object MulStringString(string s1, string s2)
		{
			double num;
			if (s1 != null)
			{
				num = DoubleType.FromString(s1);
			}
			double num2;
			if (s2 != null)
			{
				num2 = DoubleType.FromString(s2);
			}
			return num * num2;
		}

		private static object MulByte(byte i1, byte i2)
		{
			checked
			{
				int num = (int)(i1 * i2);
				if (num >= 0 && num <= 255)
				{
					return (byte)num;
				}
				if (num >= -32768 && num <= 32767)
				{
					return (short)num;
				}
				return num;
			}
		}

		private static object MulInt16(short i1, short i2)
		{
			checked
			{
				int num = (int)(i1 * i2);
				if (num >= -32768 && num <= 32767)
				{
					return (short)num;
				}
				return num;
			}
		}

		private static object MulInt32(int i1, int i2)
		{
			checked
			{
				long num = unchecked((long)i1) * unchecked((long)i2);
				if (num >= -2147483648L && num <= 2147483647L)
				{
					return (int)num;
				}
				return num;
			}
		}

		private static object MulInt64(long i1, long i2)
		{
			object obj;
			try
			{
				obj = checked(i1 * i2);
			}
			catch (OverflowException ex)
			{
				try
				{
					obj = decimal.Multiply(new decimal(i1), new decimal(i2));
				}
				catch (OverflowException ex2)
				{
					obj = (double)i1 * (double)i2;
				}
			}
			return obj;
		}

		private static object MulSingle(float f1, float f2)
		{
			double num = (double)f1 * (double)f2;
			if (num <= 3.4028234663852886E+38 && num >= -3.4028234663852886E+38)
			{
				return (float)num;
			}
			if (double.IsInfinity(num) && (float.IsInfinity(f1) || float.IsInfinity(f2)))
			{
				return (float)num;
			}
			return num;
		}

		private static object MulDouble(double d1, double d2)
		{
			return d1 * d2;
		}

		private static object MulDecimal(IConvertible conv1, IConvertible conv2)
		{
			decimal num = conv1.ToDecimal(null);
			decimal num2 = conv2.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Multiply(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToDouble(num) * Convert.ToDouble(num2);
			}
			return obj;
		}

		public static object DivObj(object o1, object o2)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return ObjectType.DivDouble(0.0, 0.0);
			case TypeCode.Boolean:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return ObjectType.DivDouble(0.0, convertible2.ToDouble(null));
			case TypeCode.String:
				return ObjectType.DivString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)57:
				return ObjectType.DivDouble((double)ObjectType.ToVBBool(convertible), 0.0);
			case (TypeCode)60:
				return ObjectType.DivDouble((double)ObjectType.ToVBBool(convertible), (double)ObjectType.ToVBBool(convertible2));
			case (TypeCode)63:
			case (TypeCode)64:
			case (TypeCode)66:
			case (TypeCode)68:
			case (TypeCode)71:
				return ObjectType.DivDouble((double)ObjectType.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)70:
				return ObjectType.DivSingle((float)ObjectType.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)72:
				return ObjectType.DivDecimal(ObjectType.ToVBBoolConv(convertible), (IConvertible)convertible2.ToDecimal(null));
			case (TypeCode)75:
				return ObjectType.DivString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)114:
			case (TypeCode)133:
			case (TypeCode)171:
			case (TypeCode)209:
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return ObjectType.DivDouble(convertible.ToDouble(null), 0.0);
			case (TypeCode)117:
			case (TypeCode)136:
			case (TypeCode)174:
			case (TypeCode)212:
			case (TypeCode)269:
				return ObjectType.DivDouble(convertible.ToDouble(null), (double)ObjectType.ToVBBool(convertible2));
			case (TypeCode)120:
			case (TypeCode)121:
			case (TypeCode)123:
			case (TypeCode)125:
			case (TypeCode)128:
			case (TypeCode)139:
			case (TypeCode)140:
			case (TypeCode)142:
			case (TypeCode)144:
			case (TypeCode)147:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)180:
			case (TypeCode)182:
			case (TypeCode)185:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)218:
			case (TypeCode)220:
			case (TypeCode)223:
			case (TypeCode)261:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)275:
			case (TypeCode)277:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return ObjectType.DivDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)184:
			case (TypeCode)222:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)256:
			case (TypeCode)258:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return ObjectType.DivSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)129:
			case (TypeCode)148:
			case (TypeCode)186:
			case (TypeCode)224:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)294:
			case (TypeCode)296:
			case (TypeCode)300:
				return ObjectType.DivDecimal(convertible, convertible2);
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)189:
			case (TypeCode)227:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return ObjectType.DivString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)250:
				return ObjectType.DivSingle(convertible.ToSingle(null), (float)ObjectType.ToVBBool(convertible2));
			case (TypeCode)288:
				return ObjectType.DivDecimal(convertible, ObjectType.ToVBBoolConv(convertible2));
			case (TypeCode)342:
				return ObjectType.DivString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)345:
				return ObjectType.DivString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)351:
			case (TypeCode)353:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return ObjectType.DivString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)360:
				return ObjectType.DivStringString(convertible.ToString(null), convertible2.ToString(null));
			}
			throw ObjectType.GetNoValidOperatorException(o1, o2);
		}

		private static object DivString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			double num;
			if (tc1 == TypeCode.String)
			{
				num = DoubleType.FromString(conv1.ToString(null));
			}
			else if (tc1 == TypeCode.Boolean)
			{
				num = (double)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToDouble(null);
			}
			double num2;
			if (tc2 == TypeCode.String)
			{
				num2 = DoubleType.FromString(conv2.ToString(null));
			}
			else if (tc2 == TypeCode.Boolean)
			{
				num2 = (double)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToDouble(null);
			}
			return num / num2;
		}

		private static object DivStringString(string s1, string s2)
		{
			double num;
			if (s1 != null)
			{
				num = DoubleType.FromString(s1);
			}
			double num2;
			if (s2 != null)
			{
				num2 = DoubleType.FromString(s2);
			}
			return num / num2;
		}

		private static object DivDouble(double d1, double d2)
		{
			return d1 / d2;
		}

		private static object DivSingle(float sng1, float sng2)
		{
			float num = sng1 / sng2;
			if (!float.IsInfinity(num))
			{
				return num;
			}
			if (float.IsInfinity(sng1) || float.IsInfinity(sng2))
			{
				return num;
			}
			return (double)sng1 / (double)sng2;
		}

		private static object DivDecimal(IConvertible conv1, IConvertible conv2)
		{
			decimal num;
			if (conv1 != null)
			{
				num = conv1.ToDecimal(null);
			}
			decimal num2;
			if (conv2 != null)
			{
				num2 = conv2.ToDecimal(null);
			}
			object obj;
			try
			{
				obj = decimal.Divide(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToSingle(num) / Convert.ToSingle(num2);
			}
			return obj;
		}

		public static object PowObj(object obj1, object obj2)
		{
			if (obj1 == null && obj2 == null)
			{
				return 1.0;
			}
			switch (ObjectType.GetWidestType(obj1, obj2, false))
			{
			case TypeCode.Boolean:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
				return Math.Pow(DoubleType.FromObject(obj1), DoubleType.FromObject(obj2));
			}
			throw ObjectType.GetNoValidOperatorException(obj1, obj2);
		}

		public static object ModObj(object o1, object o2)
		{
			IConvertible convertible = o1 as IConvertible;
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode;
			if (convertible != null)
			{
				typeCode = convertible.GetTypeCode();
			}
			else if (o1 == null)
			{
				typeCode = TypeCode.Empty;
			}
			else
			{
				typeCode = TypeCode.Object;
			}
			TypeCode typeCode2;
			if (convertible2 != null)
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			else
			{
				convertible2 = null;
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			checked
			{
				switch (typeCode * (TypeCode)19 + (int)typeCode2)
				{
				case TypeCode.Empty:
					return ObjectType.ModInt32(0, 0);
				case TypeCode.Boolean:
					return ObjectType.ModInt16(0, (short)ObjectType.ToVBBool(convertible2));
				case TypeCode.Byte:
					return ObjectType.ModByte(0, convertible2.ToByte(null));
				case TypeCode.Int16:
					return ObjectType.ModInt16(0, (short)ObjectType.ToVBBool(convertible2));
				case TypeCode.Int32:
					return ObjectType.ModInt32(0, convertible2.ToInt32(null));
				case TypeCode.Int64:
					return ObjectType.ModInt64(0L, convertible2.ToInt64(null));
				case TypeCode.Single:
					return ObjectType.ModSingle(0f, convertible2.ToSingle(null));
				case TypeCode.Double:
					return ObjectType.ModDouble(0.0, convertible2.ToDouble(null));
				case TypeCode.Decimal:
					return ObjectType.ModDecimal(null, convertible2);
				case TypeCode.String:
					return ObjectType.ModString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)57:
					return ObjectType.ModInt16((short)ObjectType.ToVBBool(convertible), 0);
				case (TypeCode)60:
					return ObjectType.ModInt16((short)ObjectType.ToVBBool(convertible), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)63:
				case (TypeCode)64:
					return ObjectType.ModInt16((short)ObjectType.ToVBBool(convertible), convertible2.ToInt16(null));
				case (TypeCode)66:
					return ObjectType.ModInt32(ObjectType.ToVBBool(convertible), convertible2.ToInt32(null));
				case (TypeCode)68:
					return ObjectType.ModInt64(unchecked((long)ObjectType.ToVBBool(convertible)), convertible2.ToInt64(null));
				case (TypeCode)70:
					return ObjectType.ModSingle((float)ObjectType.ToVBBool(convertible), convertible2.ToSingle(null));
				case (TypeCode)71:
					return ObjectType.ModDouble((double)ObjectType.ToVBBool(convertible), convertible2.ToDouble(null));
				case (TypeCode)72:
					return ObjectType.ModDecimal(ObjectType.ToVBBoolConv(convertible), convertible2);
				case (TypeCode)75:
					return ObjectType.ModString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)114:
					return ObjectType.ModByte(convertible.ToByte(null), 0);
				case (TypeCode)117:
				case (TypeCode)136:
					return ObjectType.ModInt16(convertible.ToInt16(null), (short)ObjectType.ToVBBool(convertible2));
				case (TypeCode)120:
					return ObjectType.ModByte(convertible.ToByte(null), convertible2.ToByte(null));
				case (TypeCode)121:
				case (TypeCode)139:
				case (TypeCode)140:
					return ObjectType.ModInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
				case (TypeCode)123:
				case (TypeCode)142:
				case (TypeCode)177:
				case (TypeCode)178:
				case (TypeCode)180:
					return ObjectType.ModInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
				case (TypeCode)125:
				case (TypeCode)144:
				case (TypeCode)182:
				case (TypeCode)215:
				case (TypeCode)216:
				case (TypeCode)218:
				case (TypeCode)220:
					return ObjectType.ModInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
				case (TypeCode)127:
				case (TypeCode)146:
				case (TypeCode)184:
				case (TypeCode)222:
				case (TypeCode)253:
				case (TypeCode)254:
				case (TypeCode)256:
				case (TypeCode)258:
				case (TypeCode)260:
				case (TypeCode)262:
				case (TypeCode)298:
					return ObjectType.ModSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
				case (TypeCode)128:
				case (TypeCode)147:
				case (TypeCode)185:
				case (TypeCode)223:
				case (TypeCode)261:
				case (TypeCode)272:
				case (TypeCode)273:
				case (TypeCode)275:
				case (TypeCode)277:
				case (TypeCode)279:
				case (TypeCode)280:
				case (TypeCode)281:
				case (TypeCode)299:
					return ObjectType.ModDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
				case (TypeCode)129:
				case (TypeCode)148:
				case (TypeCode)186:
				case (TypeCode)224:
				case (TypeCode)291:
				case (TypeCode)292:
				case (TypeCode)294:
				case (TypeCode)296:
				case (TypeCode)300:
					return ObjectType.ModDecimal(convertible, convertible2);
				case (TypeCode)132:
				case (TypeCode)151:
				case (TypeCode)189:
				case (TypeCode)227:
				case (TypeCode)265:
				case (TypeCode)284:
				case (TypeCode)303:
					return ObjectType.ModString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)133:
					return ObjectType.ModInt16(convertible.ToInt16(null), 0);
				case (TypeCode)171:
					return ObjectType.ModInt32(convertible.ToInt32(null), 0);
				case (TypeCode)174:
					return ObjectType.ModInt32(convertible.ToInt32(null), ObjectType.ToVBBool(convertible2));
				case (TypeCode)209:
					return ObjectType.ModInt64(convertible.ToInt64(null), 0L);
				case (TypeCode)212:
					return ObjectType.ModInt64(convertible.ToInt64(null), unchecked((long)ObjectType.ToVBBool(convertible2)));
				case (TypeCode)247:
					return ObjectType.ModSingle(convertible.ToSingle(null), 0f);
				case (TypeCode)250:
					return ObjectType.ModSingle(convertible.ToSingle(null), (float)ObjectType.ToVBBool(convertible2));
				case (TypeCode)266:
					return ObjectType.ModDouble(convertible.ToDouble(null), 0.0);
				case (TypeCode)269:
					return ObjectType.ModDouble(convertible.ToDouble(null), (double)ObjectType.ToVBBool(convertible2));
				case (TypeCode)285:
					return ObjectType.ModDecimal(convertible, null);
				case (TypeCode)288:
					return ObjectType.ModDecimal(convertible, ObjectType.ToVBBoolConv(convertible2));
				case (TypeCode)342:
					return ObjectType.ModString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)345:
					return ObjectType.ModString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)348:
				case (TypeCode)349:
				case (TypeCode)351:
				case (TypeCode)353:
				case (TypeCode)355:
				case (TypeCode)356:
				case (TypeCode)357:
					return ObjectType.ModString(convertible, typeCode, convertible2, typeCode2);
				case (TypeCode)360:
					return ObjectType.ModStringString(convertible.ToString(null), convertible2.ToString(null));
				}
				throw ObjectType.GetNoValidOperatorException(o1, o2);
			}
		}

		private static object ModString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			double num;
			if (tc1 == TypeCode.String)
			{
				num = DoubleType.FromString(conv1.ToString(null));
			}
			else if (tc1 == TypeCode.Boolean)
			{
				num = (double)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToDouble(null);
			}
			double num2;
			if (tc2 == TypeCode.String)
			{
				num2 = DoubleType.FromString(conv2.ToString(null));
			}
			else if (tc2 == TypeCode.Boolean)
			{
				num2 = (double)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToDouble(null);
			}
			return num % num2;
		}

		private static object ModStringString(string s1, string s2)
		{
			double num;
			if (s1 != null)
			{
				num = DoubleType.FromString(s1);
			}
			double num2;
			if (s2 != null)
			{
				num2 = DoubleType.FromString(s2);
			}
			return num % num2;
		}

		private static object ModByte(byte i1, byte i2)
		{
			return i1 % i2;
		}

		private static object ModInt16(short i1, short i2)
		{
			int num = (int)(i1 % i2);
			if (num < -32768 || num > 32767)
			{
				return num;
			}
			return checked((short)num);
		}

		private static object ModInt32(int i1, int i2)
		{
			long num = (long)i1 % (long)i2;
			if (num < -2147483648L || num > 2147483647L)
			{
				return num;
			}
			return checked((int)num);
		}

		private static object ModInt64(long i1, long i2)
		{
			object obj;
			try
			{
				obj = i1 % i2;
			}
			catch (OverflowException ex)
			{
				decimal num = decimal.Remainder(new decimal(i1), new decimal(i2));
				if (decimal.Compare(num, -9223372036854775808m) < 0 || decimal.Compare(num, 9223372036854775807m) > 0)
				{
					obj = num;
				}
				else
				{
					obj = Convert.ToInt64(num);
				}
			}
			return obj;
		}

		private static object ModSingle(float sng1, float sng2)
		{
			return sng1 % sng2;
		}

		private static object ModDouble(double d1, double d2)
		{
			return d1 % d2;
		}

		private static object ModDecimal(IConvertible conv1, IConvertible conv2)
		{
			decimal num;
			if (conv1 != null)
			{
				num = conv1.ToDecimal(null);
			}
			decimal num2;
			if (conv2 != null)
			{
				num2 = conv2.ToDecimal(null);
			}
			return decimal.Remainder(num, num2);
		}

		public static object IDivObj(object o1, object o2)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = o2 as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (o2 == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return ObjectType.IDivideInt32(0, 0);
			case TypeCode.Boolean:
				return ObjectType.IDivideInt64(0L, (long)ObjectType.ToVBBool(convertible2));
			case TypeCode.Byte:
				return ObjectType.IDivideByte(0, convertible2.ToByte(null));
			case TypeCode.Int16:
				return ObjectType.IDivideInt16(0, convertible2.ToInt16(null));
			case TypeCode.Int32:
				return ObjectType.IDivideInt32(0, convertible2.ToInt32(null));
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return ObjectType.IDivideInt64(0L, convertible2.ToInt64(null));
			case TypeCode.String:
				return ObjectType.IDivideInt64(0L, LongType.FromString(convertible2.ToString(null)));
			case (TypeCode)57:
				return ObjectType.IDivideInt16(checked((short)ObjectType.ToVBBool(convertible)), 0);
			case (TypeCode)60:
				return checked(ObjectType.IDivideInt16((short)ObjectType.ToVBBool(convertible), (short)ObjectType.ToVBBool(convertible2)));
			case (TypeCode)63:
			case (TypeCode)64:
				return ObjectType.IDivideInt16(checked((short)ObjectType.ToVBBool(convertible)), convertible2.ToInt16(null));
			case (TypeCode)66:
				return ObjectType.IDivideInt32(ObjectType.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)68:
			case (TypeCode)70:
			case (TypeCode)71:
			case (TypeCode)72:
				return ObjectType.IDivideInt64((long)ObjectType.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)75:
				return ObjectType.IDivideInt64((long)ObjectType.ToVBBool(convertible), LongType.FromString(convertible2.ToString(null)));
			case (TypeCode)114:
				return ObjectType.IDivideByte(convertible.ToByte(null), 0);
			case (TypeCode)117:
			case (TypeCode)136:
				return ObjectType.IDivideInt16(convertible.ToInt16(null), checked((short)ObjectType.ToVBBool(convertible2)));
			case (TypeCode)120:
				return ObjectType.IDivideByte(convertible.ToByte(null), convertible2.ToByte(null));
			case (TypeCode)121:
			case (TypeCode)139:
			case (TypeCode)140:
				return ObjectType.IDivideInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
			case (TypeCode)123:
			case (TypeCode)142:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)180:
				return ObjectType.IDivideInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)125:
			case (TypeCode)127:
			case (TypeCode)128:
			case (TypeCode)129:
			case (TypeCode)144:
			case (TypeCode)146:
			case (TypeCode)147:
			case (TypeCode)148:
			case (TypeCode)182:
			case (TypeCode)184:
			case (TypeCode)185:
			case (TypeCode)186:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)218:
			case (TypeCode)220:
			case (TypeCode)222:
			case (TypeCode)223:
			case (TypeCode)224:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)256:
			case (TypeCode)258:
			case (TypeCode)260:
			case (TypeCode)261:
			case (TypeCode)262:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)275:
			case (TypeCode)277:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)294:
			case (TypeCode)296:
			case (TypeCode)298:
			case (TypeCode)299:
			case (TypeCode)300:
				return ObjectType.IDivideInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)189:
			case (TypeCode)227:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return ObjectType.IDivideString(convertible, typeCode, convertible2, typeCode2);
			case (TypeCode)133:
				return ObjectType.IDivideInt16(convertible.ToInt16(null), 0);
			case (TypeCode)171:
				return ObjectType.IDivideInt32(convertible.ToInt32(null), 0);
			case (TypeCode)174:
				return ObjectType.IDivideInt32(convertible.ToInt32(null), ObjectType.ToVBBool(convertible2));
			case (TypeCode)209:
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return ObjectType.IDivideInt64(convertible.ToInt64(null), 0L);
			case (TypeCode)212:
			case (TypeCode)250:
			case (TypeCode)269:
			case (TypeCode)288:
				return ObjectType.IDivideInt64(convertible.ToInt64(null), (long)ObjectType.ToVBBool(convertible2));
			case (TypeCode)342:
				return ObjectType.IDivideInt64(LongType.FromString(convertible.ToString(null)), 0L);
			case (TypeCode)345:
				return ObjectType.IDivideInt64(LongType.FromString(convertible.ToString(null)), (long)ObjectType.ToVBBool(convertible2));
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)351:
			case (TypeCode)353:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return ObjectType.IDivideInt64(LongType.FromString(convertible.ToString(null)), convertible2.ToInt64(null));
			case (TypeCode)360:
				return ObjectType.IDivideStringString(convertible.ToString(null), convertible2.ToString(null));
			}
			throw ObjectType.GetNoValidOperatorException(o1, o2);
		}

		private static object IDivideString(IConvertible conv1, TypeCode tc1, IConvertible conv2, TypeCode tc2)
		{
			long num;
			if (tc1 == TypeCode.String)
			{
				try
				{
					num = LongType.FromString(conv1.ToString(null));
					goto IL_0040;
				}
				catch (StackOverflowException ex)
				{
					throw ex;
				}
				catch (OutOfMemoryException ex2)
				{
					throw ex2;
				}
				catch (ThreadAbortException ex3)
				{
					throw ex3;
				}
				catch (Exception)
				{
					throw ObjectType.GetNoValidOperatorException(conv1, conv2);
				}
			}
			if (tc1 == TypeCode.Boolean)
			{
				num = (long)ObjectType.ToVBBool(conv1);
			}
			else
			{
				num = conv1.ToInt64(null);
			}
			IL_0040:
			long num2;
			if (tc2 == TypeCode.String)
			{
				try
				{
					num2 = LongType.FromString(conv2.ToString(null));
					goto IL_0082;
				}
				catch (StackOverflowException ex4)
				{
					throw ex4;
				}
				catch (OutOfMemoryException ex5)
				{
					throw ex5;
				}
				catch (ThreadAbortException ex6)
				{
					throw ex6;
				}
				catch (Exception)
				{
					throw ObjectType.GetNoValidOperatorException(conv1, conv2);
				}
			}
			if (tc2 == TypeCode.Boolean)
			{
				num2 = (long)ObjectType.ToVBBool(conv2);
			}
			else
			{
				num2 = conv2.ToInt64(null);
			}
			IL_0082:
			return num / num2;
		}

		private static object IDivideStringString(string s1, string s2)
		{
			long num;
			if (s1 != null)
			{
				num = LongType.FromString(s1);
			}
			long num2;
			if (s2 != null)
			{
				num2 = LongType.FromString(s2);
			}
			return num / num2;
		}

		private static object IDivideByte(byte d1, byte d2)
		{
			return d1 / d2;
		}

		private static object IDivideInt16(short d1, short d2)
		{
			return d1 / d2;
		}

		private static object IDivideInt32(int d1, int d2)
		{
			return d1 / d2;
		}

		private static object IDivideInt64(long d1, long d2)
		{
			return d1 / d2;
		}

		public static object ShiftLeftObj(object o1, int amount)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return 0 << amount;
			case TypeCode.Boolean:
				return (short)(((-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0) << (amount & 15));
			case TypeCode.Byte:
				return (byte)(convertible.ToByte(null) << (amount & 7));
			case TypeCode.Int16:
				return (short)(convertible.ToInt16(null) << (amount & 15));
			case TypeCode.Int32:
				return convertible.ToInt32(null) << amount;
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return convertible.ToInt64(null) << amount;
			case TypeCode.String:
				return LongType.FromString(convertible.ToString(null)) << amount;
			}
			throw ObjectType.GetNoValidOperatorException(o1);
		}

		public static object ShiftRightObj(object o1, int amount)
		{
			IConvertible convertible = o1 as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (o1 == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return 0 >> amount;
			case TypeCode.Boolean:
				return (short)(((-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0) >> (amount & 15));
			case TypeCode.Byte:
				return (byte)((uint)convertible.ToByte(null) >> (amount & 7));
			case TypeCode.Int16:
				return (short)(convertible.ToInt16(null) >> (amount & 15));
			case TypeCode.Int32:
				return convertible.ToInt32(null) >> amount;
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return convertible.ToInt64(null) >> amount;
			case TypeCode.String:
				return LongType.FromString(convertible.ToString(null)) >> amount;
			}
			throw ObjectType.GetNoValidOperatorException(o1);
		}

		public static object XorObj(object obj1, object obj2)
		{
			if (obj1 == null && obj2 == null)
			{
				return false;
			}
			switch (ObjectType.GetWidestType(obj1, obj2, false))
			{
			case TypeCode.Boolean:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
				return BooleanType.FromObject(obj1) ^ BooleanType.FromObject(obj2);
			}
			throw ObjectType.GetNoValidOperatorException(obj1, obj2);
		}

		public static bool LikeObj(object vLeft, object vRight, CompareMethod CompareOption)
		{
			return StringType.StrLike(StringType.FromObject(vLeft), StringType.FromObject(vRight), CompareOption);
		}

		public static object StrCatObj(object vLeft, object vRight)
		{
			bool flag = vLeft is DBNull;
			bool flag2 = vRight is DBNull;
			if (flag && flag2)
			{
				return vLeft;
			}
			if (flag & !flag2)
			{
				vLeft = "";
			}
			else if (flag2 & !flag)
			{
				vRight = "";
			}
			return StringType.FromObject(vLeft) + StringType.FromObject(vRight);
		}

		internal static object CTypeHelper(object obj, TypeCode toType)
		{
			if (obj == null)
			{
				return null;
			}
			switch (toType)
			{
			case TypeCode.Boolean:
				return BooleanType.FromObject(obj);
			case TypeCode.Char:
				return CharType.FromObject(obj);
			case TypeCode.Byte:
				return ByteType.FromObject(obj);
			case TypeCode.Int16:
				return ShortType.FromObject(obj);
			case TypeCode.Int32:
				return IntegerType.FromObject(obj);
			case TypeCode.Int64:
				return LongType.FromObject(obj);
			case TypeCode.Single:
				return SingleType.FromObject(obj);
			case TypeCode.Double:
				return DoubleType.FromObject(obj);
			case TypeCode.Decimal:
				return DecimalType.FromObject(obj);
			case TypeCode.DateTime:
				return DateType.FromObject(obj);
			case TypeCode.String:
				return StringType.FromObject(obj);
			}
			throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
			{
				Utils.VBFriendlyName(obj),
				Utils.VBFriendlyName(ObjectType.TypeFromTypeCode(toType))
			}));
		}

		internal static object CTypeHelper(object obj, Type toType)
		{
			if (obj == null)
			{
				return null;
			}
			if (toType == typeof(object))
			{
				return obj;
			}
			Type type = obj.GetType();
			bool flag;
			if (toType.IsByRef)
			{
				toType = toType.GetElementType();
				flag = true;
			}
			if (type.IsByRef)
			{
				type = type.GetElementType();
			}
			object obj2;
			if (type == toType || toType == typeof(object))
			{
				if (!flag)
				{
					return obj;
				}
				obj2 = ObjectType.GetObjectValuePrimitive(obj);
			}
			else
			{
				TypeCode typeCode = Type.GetTypeCode(toType);
				if (typeCode == TypeCode.Object)
				{
					if (toType == typeof(object) || toType.IsInstanceOfType(obj))
					{
						return obj;
					}
					string text = obj as string;
					if (text != null && toType == typeof(char[]))
					{
						return CharArrayType.FromString(text);
					}
					throw new InvalidCastException(Utils.GetResourceString("InvalidCast_FromTo", new string[]
					{
						Utils.VBFriendlyName(type),
						Utils.VBFriendlyName(toType)
					}));
				}
				else
				{
					obj2 = ObjectType.CTypeHelper(obj, typeCode);
				}
			}
			if (toType.IsEnum)
			{
				return Enum.ToObject(toType, obj2);
			}
			return obj2;
		}

		private static Exception GetNoValidOperatorException(object Operand)
		{
			return new InvalidCastException(Utils.GetResourceString("NoValidOperator_OneOperand", new string[] { Utils.VBFriendlyName(Operand) }));
		}

		private static Exception GetNoValidOperatorException(object Left, object Right)
		{
			string text;
			if (Left == null)
			{
				text = "'Nothing'";
			}
			else
			{
				string text2 = Left as string;
				if (text2 != null)
				{
					text = Utils.GetResourceString("NoValidOperator_StringType1", new string[] { Strings.Left(text2, 32) });
				}
				else
				{
					text = Utils.GetResourceString("NoValidOperator_NonStringType1", new string[] { Utils.VBFriendlyName(Left) });
				}
			}
			string text3;
			if (Right == null)
			{
				text3 = "'Nothing'";
			}
			else
			{
				string text4 = Right as string;
				if (text4 != null)
				{
					text3 = Utils.GetResourceString("NoValidOperator_StringType1", new string[] { Strings.Left(text4, 32) });
				}
				else
				{
					text3 = Utils.GetResourceString("NoValidOperator_NonStringType1", new string[] { Utils.VBFriendlyName(Right) });
				}
			}
			return new InvalidCastException(Utils.GetResourceString("NoValidOperator_TwoOperands", new string[] { text, text3 }));
		}

		public static object GetObjectValuePrimitive(object o)
		{
			if (o == null)
			{
				return null;
			}
			IConvertible convertible = o as IConvertible;
			if (convertible == null)
			{
				return o;
			}
			switch (convertible.GetTypeCode())
			{
			case TypeCode.Boolean:
				return convertible.ToBoolean(null);
			case TypeCode.Char:
				return convertible.ToChar(null);
			case TypeCode.SByte:
				return convertible.ToSByte(null);
			case TypeCode.Byte:
				return convertible.ToByte(null);
			case TypeCode.Int16:
				return convertible.ToInt16(null);
			case TypeCode.UInt16:
				return convertible.ToUInt16(null);
			case TypeCode.Int32:
				return convertible.ToInt32(null);
			case TypeCode.UInt32:
				return convertible.ToUInt32(null);
			case TypeCode.Int64:
				return convertible.ToInt64(null);
			case TypeCode.UInt64:
				return convertible.ToUInt64(null);
			case TypeCode.Single:
				return convertible.ToSingle(null);
			case TypeCode.Double:
				return convertible.ToDouble(null);
			case TypeCode.Decimal:
				return convertible.ToDecimal(null);
			case TypeCode.DateTime:
				return convertible.ToDateTime(null);
			case TypeCode.String:
				return o;
			}
			return o;
		}

		private const int TCMAX = 19;

		private static readonly ObjectType.VType[,] WiderType;

		private static readonly ObjectType.CC[,] ConversionClassTable;

		private enum VType
		{
			t_bad,
			t_bool,
			t_ui1,
			t_i2,
			t_i4,
			t_i8,
			t_dec,
			t_r4,
			t_r8,
			t_char,
			t_str,
			t_date
		}

		private enum VType2
		{
			t_bad,
			t_bool,
			t_ui1,
			t_char,
			t_i2,
			t_i4,
			t_i8,
			t_r4,
			t_r8,
			t_date,
			t_dec,
			t_ref,
			t_str
		}

		private enum CC : byte
		{
			Err,
			Same,
			Narr,
			Wide
		}
	}
}

using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E1 RID: 2017
	internal sealed class Converter
	{
		// Token: 0x060047C3 RID: 18371 RVA: 0x000F8EEA File Offset: 0x000F7EEA
		private Converter()
		{
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x000F8EF4 File Offset: 0x000F7EF4
		internal static InternalPrimitiveTypeE ToCode(Type type)
		{
			InternalPrimitiveTypeE internalPrimitiveTypeE;
			if (type != null && !type.IsPrimitive)
			{
				if (type == Converter.typeofDateTime)
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.DateTime;
				}
				else if (type == Converter.typeofTimeSpan)
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.TimeSpan;
				}
				else if (type == Converter.typeofDecimal)
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.Decimal;
				}
				else
				{
					internalPrimitiveTypeE = InternalPrimitiveTypeE.Invalid;
				}
			}
			else
			{
				internalPrimitiveTypeE = Converter.ToPrimitiveTypeEnum(Type.GetTypeCode(type));
			}
			return internalPrimitiveTypeE;
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x000F8F44 File Offset: 0x000F7F44
		internal static bool IsWriteAsByteArray(InternalPrimitiveTypeE code)
		{
			bool flag = false;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
			case InternalPrimitiveTypeE.Byte:
			case InternalPrimitiveTypeE.Char:
			case InternalPrimitiveTypeE.Double:
			case InternalPrimitiveTypeE.Int16:
			case InternalPrimitiveTypeE.Int32:
			case InternalPrimitiveTypeE.Int64:
			case InternalPrimitiveTypeE.SByte:
			case InternalPrimitiveTypeE.Single:
			case InternalPrimitiveTypeE.UInt16:
			case InternalPrimitiveTypeE.UInt32:
			case InternalPrimitiveTypeE.UInt64:
				flag = true;
				break;
			}
			return flag;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x000F8FA4 File Offset: 0x000F7FA4
		internal static int TypeLength(InternalPrimitiveTypeE code)
		{
			int num = 0;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				num = 1;
				break;
			case InternalPrimitiveTypeE.Byte:
				num = 1;
				break;
			case InternalPrimitiveTypeE.Char:
				num = 2;
				break;
			case InternalPrimitiveTypeE.Double:
				num = 8;
				break;
			case InternalPrimitiveTypeE.Int16:
				num = 2;
				break;
			case InternalPrimitiveTypeE.Int32:
				num = 4;
				break;
			case InternalPrimitiveTypeE.Int64:
				num = 8;
				break;
			case InternalPrimitiveTypeE.SByte:
				num = 1;
				break;
			case InternalPrimitiveTypeE.Single:
				num = 4;
				break;
			case InternalPrimitiveTypeE.UInt16:
				num = 2;
				break;
			case InternalPrimitiveTypeE.UInt32:
				num = 4;
				break;
			case InternalPrimitiveTypeE.UInt64:
				num = 8;
				break;
			}
			return num;
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x000F9030 File Offset: 0x000F8030
		internal static InternalNameSpaceE GetNameSpaceEnum(InternalPrimitiveTypeE code, Type type, WriteObjectInfo objectInfo, out string typeName)
		{
			InternalNameSpaceE internalNameSpaceE = InternalNameSpaceE.None;
			typeName = null;
			if (code != InternalPrimitiveTypeE.Invalid)
			{
				switch (code)
				{
				case InternalPrimitiveTypeE.Boolean:
				case InternalPrimitiveTypeE.Byte:
				case InternalPrimitiveTypeE.Char:
				case InternalPrimitiveTypeE.Double:
				case InternalPrimitiveTypeE.Int16:
				case InternalPrimitiveTypeE.Int32:
				case InternalPrimitiveTypeE.Int64:
				case InternalPrimitiveTypeE.SByte:
				case InternalPrimitiveTypeE.Single:
				case InternalPrimitiveTypeE.TimeSpan:
				case InternalPrimitiveTypeE.DateTime:
				case InternalPrimitiveTypeE.UInt16:
				case InternalPrimitiveTypeE.UInt32:
				case InternalPrimitiveTypeE.UInt64:
					internalNameSpaceE = InternalNameSpaceE.XdrPrimitive;
					typeName = "System." + Converter.ToComType(code);
					break;
				case InternalPrimitiveTypeE.Decimal:
					internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					typeName = "System." + Converter.ToComType(code);
					break;
				}
			}
			if (internalNameSpaceE == InternalNameSpaceE.None && type != null)
			{
				if (type == Converter.typeofString)
				{
					internalNameSpaceE = InternalNameSpaceE.XdrString;
				}
				else if (objectInfo == null)
				{
					typeName = type.FullName;
					if (type.Assembly == Converter.urtAssembly)
					{
						internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						internalNameSpaceE = InternalNameSpaceE.UrtUser;
					}
				}
				else
				{
					typeName = objectInfo.GetTypeFullName();
					if (objectInfo.GetAssemblyString().Equals(Converter.urtAssemblyString))
					{
						internalNameSpaceE = InternalNameSpaceE.UrtSystem;
					}
					else
					{
						internalNameSpaceE = InternalNameSpaceE.UrtUser;
					}
				}
			}
			return internalNameSpaceE;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x000F910E File Offset: 0x000F810E
		internal static Type ToArrayType(InternalPrimitiveTypeE code)
		{
			if (Converter.arrayTypeA == null)
			{
				Converter.InitArrayTypeA();
			}
			return Converter.arrayTypeA[(int)code];
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x000F9124 File Offset: 0x000F8124
		private static void InitTypeA()
		{
			Type[] array = new Type[Converter.primitiveTypeEnumLength];
			array[0] = null;
			array[1] = Converter.typeofBoolean;
			array[2] = Converter.typeofByte;
			array[3] = Converter.typeofChar;
			array[5] = Converter.typeofDecimal;
			array[6] = Converter.typeofDouble;
			array[7] = Converter.typeofInt16;
			array[8] = Converter.typeofInt32;
			array[9] = Converter.typeofInt64;
			array[10] = Converter.typeofSByte;
			array[11] = Converter.typeofSingle;
			array[12] = Converter.typeofTimeSpan;
			array[13] = Converter.typeofDateTime;
			array[14] = Converter.typeofUInt16;
			array[15] = Converter.typeofUInt32;
			array[16] = Converter.typeofUInt64;
			Converter.typeA = array;
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x000F91C8 File Offset: 0x000F81C8
		private static void InitArrayTypeA()
		{
			Type[] array = new Type[Converter.primitiveTypeEnumLength];
			array[0] = null;
			array[1] = Converter.typeofBooleanArray;
			array[2] = Converter.typeofByteArray;
			array[3] = Converter.typeofCharArray;
			array[5] = Converter.typeofDecimalArray;
			array[6] = Converter.typeofDoubleArray;
			array[7] = Converter.typeofInt16Array;
			array[8] = Converter.typeofInt32Array;
			array[9] = Converter.typeofInt64Array;
			array[10] = Converter.typeofSByteArray;
			array[11] = Converter.typeofSingleArray;
			array[12] = Converter.typeofTimeSpanArray;
			array[13] = Converter.typeofDateTimeArray;
			array[14] = Converter.typeofUInt16Array;
			array[15] = Converter.typeofUInt32Array;
			array[16] = Converter.typeofUInt64Array;
			Converter.arrayTypeA = array;
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x000F926A File Offset: 0x000F826A
		internal static Type ToType(InternalPrimitiveTypeE code)
		{
			if (Converter.typeA == null)
			{
				Converter.InitTypeA();
			}
			return Converter.typeA[(int)code];
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x000F9280 File Offset: 0x000F8280
		internal static Array CreatePrimitiveArray(InternalPrimitiveTypeE code, int length)
		{
			Array array = null;
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				array = new bool[length];
				break;
			case InternalPrimitiveTypeE.Byte:
				array = new byte[length];
				break;
			case InternalPrimitiveTypeE.Char:
				array = new char[length];
				break;
			case InternalPrimitiveTypeE.Decimal:
				array = new decimal[length];
				break;
			case InternalPrimitiveTypeE.Double:
				array = new double[length];
				break;
			case InternalPrimitiveTypeE.Int16:
				array = new short[length];
				break;
			case InternalPrimitiveTypeE.Int32:
				array = new int[length];
				break;
			case InternalPrimitiveTypeE.Int64:
				array = new long[length];
				break;
			case InternalPrimitiveTypeE.SByte:
				array = new sbyte[length];
				break;
			case InternalPrimitiveTypeE.Single:
				array = new float[length];
				break;
			case InternalPrimitiveTypeE.TimeSpan:
				array = new TimeSpan[length];
				break;
			case InternalPrimitiveTypeE.DateTime:
				array = new DateTime[length];
				break;
			case InternalPrimitiveTypeE.UInt16:
				array = new ushort[length];
				break;
			case InternalPrimitiveTypeE.UInt32:
				array = new uint[length];
				break;
			case InternalPrimitiveTypeE.UInt64:
				array = new ulong[length];
				break;
			}
			return array;
		}

		// Token: 0x060047CD RID: 18381 RVA: 0x000F9364 File Offset: 0x000F8364
		internal static bool IsPrimitiveArray(Type type, out object typeInformation)
		{
			typeInformation = null;
			bool flag = true;
			if (type == Converter.typeofBooleanArray)
			{
				typeInformation = InternalPrimitiveTypeE.Boolean;
			}
			else if (type == Converter.typeofByteArray)
			{
				typeInformation = InternalPrimitiveTypeE.Byte;
			}
			else if (type == Converter.typeofCharArray)
			{
				typeInformation = InternalPrimitiveTypeE.Char;
			}
			else if (type == Converter.typeofDoubleArray)
			{
				typeInformation = InternalPrimitiveTypeE.Double;
			}
			else if (type == Converter.typeofInt16Array)
			{
				typeInformation = InternalPrimitiveTypeE.Int16;
			}
			else if (type == Converter.typeofInt32Array)
			{
				typeInformation = InternalPrimitiveTypeE.Int32;
			}
			else if (type == Converter.typeofInt64Array)
			{
				typeInformation = InternalPrimitiveTypeE.Int64;
			}
			else if (type == Converter.typeofSByteArray)
			{
				typeInformation = InternalPrimitiveTypeE.SByte;
			}
			else if (type == Converter.typeofSingleArray)
			{
				typeInformation = InternalPrimitiveTypeE.Single;
			}
			else if (type == Converter.typeofUInt16Array)
			{
				typeInformation = InternalPrimitiveTypeE.UInt16;
			}
			else if (type == Converter.typeofUInt32Array)
			{
				typeInformation = InternalPrimitiveTypeE.UInt32;
			}
			else if (type == Converter.typeofUInt64Array)
			{
				typeInformation = InternalPrimitiveTypeE.UInt64;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060047CE RID: 18382 RVA: 0x000F9468 File Offset: 0x000F8468
		private static void InitValueA()
		{
			string[] array = new string[Converter.primitiveTypeEnumLength];
			array[0] = null;
			array[1] = "Boolean";
			array[2] = "Byte";
			array[3] = "Char";
			array[5] = "Decimal";
			array[6] = "Double";
			array[7] = "Int16";
			array[8] = "Int32";
			array[9] = "Int64";
			array[10] = "SByte";
			array[11] = "Single";
			array[12] = "TimeSpan";
			array[13] = "DateTime";
			array[14] = "UInt16";
			array[15] = "UInt32";
			array[16] = "UInt64";
			Converter.valueA = array;
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x000F950A File Offset: 0x000F850A
		internal static string ToComType(InternalPrimitiveTypeE code)
		{
			if (Converter.valueA == null)
			{
				Converter.InitValueA();
			}
			return Converter.valueA[(int)code];
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x000F9520 File Offset: 0x000F8520
		private static void InitTypeCodeA()
		{
			TypeCode[] array = new TypeCode[Converter.primitiveTypeEnumLength];
			array[0] = TypeCode.Object;
			array[1] = TypeCode.Boolean;
			array[2] = TypeCode.Byte;
			array[3] = TypeCode.Char;
			array[5] = TypeCode.Decimal;
			array[6] = TypeCode.Double;
			array[7] = TypeCode.Int16;
			array[8] = TypeCode.Int32;
			array[9] = TypeCode.Int64;
			array[10] = TypeCode.SByte;
			array[11] = TypeCode.Single;
			array[12] = TypeCode.Object;
			array[13] = TypeCode.DateTime;
			array[14] = TypeCode.UInt16;
			array[15] = TypeCode.UInt32;
			array[16] = TypeCode.UInt64;
			Converter.typeCodeA = array;
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x000F958E File Offset: 0x000F858E
		internal static TypeCode ToTypeCode(InternalPrimitiveTypeE code)
		{
			if (Converter.typeCodeA == null)
			{
				Converter.InitTypeCodeA();
			}
			return Converter.typeCodeA[(int)code];
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x000F95A4 File Offset: 0x000F85A4
		private static void InitCodeA()
		{
			Converter.codeA = new InternalPrimitiveTypeE[]
			{
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Boolean,
				InternalPrimitiveTypeE.Char,
				InternalPrimitiveTypeE.SByte,
				InternalPrimitiveTypeE.Byte,
				InternalPrimitiveTypeE.Int16,
				InternalPrimitiveTypeE.UInt16,
				InternalPrimitiveTypeE.Int32,
				InternalPrimitiveTypeE.UInt32,
				InternalPrimitiveTypeE.Int64,
				InternalPrimitiveTypeE.UInt64,
				InternalPrimitiveTypeE.Single,
				InternalPrimitiveTypeE.Double,
				InternalPrimitiveTypeE.Decimal,
				InternalPrimitiveTypeE.DateTime,
				InternalPrimitiveTypeE.Invalid,
				InternalPrimitiveTypeE.Invalid
			};
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x000F961C File Offset: 0x000F861C
		internal static InternalPrimitiveTypeE ToPrimitiveTypeEnum(TypeCode typeCode)
		{
			if (Converter.codeA == null)
			{
				Converter.InitCodeA();
			}
			return Converter.codeA[(int)typeCode];
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x000F9634 File Offset: 0x000F8634
		internal static object FromString(string value, InternalPrimitiveTypeE code)
		{
			object obj;
			if (code != InternalPrimitiveTypeE.Invalid)
			{
				obj = Convert.ChangeType(value, Converter.ToTypeCode(code), CultureInfo.InvariantCulture);
			}
			else
			{
				obj = value;
			}
			return obj;
		}

		// Token: 0x04002499 RID: 9369
		private static int primitiveTypeEnumLength = 17;

		// Token: 0x0400249A RID: 9370
		private static Type[] typeA;

		// Token: 0x0400249B RID: 9371
		private static Type[] arrayTypeA;

		// Token: 0x0400249C RID: 9372
		private static string[] valueA;

		// Token: 0x0400249D RID: 9373
		private static TypeCode[] typeCodeA;

		// Token: 0x0400249E RID: 9374
		private static InternalPrimitiveTypeE[] codeA;

		// Token: 0x0400249F RID: 9375
		internal static Type typeofISerializable = typeof(ISerializable);

		// Token: 0x040024A0 RID: 9376
		internal static Type typeofString = typeof(string);

		// Token: 0x040024A1 RID: 9377
		internal static Type typeofConverter = typeof(Converter);

		// Token: 0x040024A2 RID: 9378
		internal static Type typeofBoolean = typeof(bool);

		// Token: 0x040024A3 RID: 9379
		internal static Type typeofByte = typeof(byte);

		// Token: 0x040024A4 RID: 9380
		internal static Type typeofChar = typeof(char);

		// Token: 0x040024A5 RID: 9381
		internal static Type typeofDecimal = typeof(decimal);

		// Token: 0x040024A6 RID: 9382
		internal static Type typeofDouble = typeof(double);

		// Token: 0x040024A7 RID: 9383
		internal static Type typeofInt16 = typeof(short);

		// Token: 0x040024A8 RID: 9384
		internal static Type typeofInt32 = typeof(int);

		// Token: 0x040024A9 RID: 9385
		internal static Type typeofInt64 = typeof(long);

		// Token: 0x040024AA RID: 9386
		internal static Type typeofSByte = typeof(sbyte);

		// Token: 0x040024AB RID: 9387
		internal static Type typeofSingle = typeof(float);

		// Token: 0x040024AC RID: 9388
		internal static Type typeofTimeSpan = typeof(TimeSpan);

		// Token: 0x040024AD RID: 9389
		internal static Type typeofDateTime = typeof(DateTime);

		// Token: 0x040024AE RID: 9390
		internal static Type typeofUInt16 = typeof(ushort);

		// Token: 0x040024AF RID: 9391
		internal static Type typeofUInt32 = typeof(uint);

		// Token: 0x040024B0 RID: 9392
		internal static Type typeofUInt64 = typeof(ulong);

		// Token: 0x040024B1 RID: 9393
		internal static Type typeofObject = typeof(object);

		// Token: 0x040024B2 RID: 9394
		internal static Type typeofSystemVoid = typeof(void);

		// Token: 0x040024B3 RID: 9395
		internal static Assembly urtAssembly = Assembly.GetAssembly(Converter.typeofString);

		// Token: 0x040024B4 RID: 9396
		internal static string urtAssemblyString = Converter.urtAssembly.FullName;

		// Token: 0x040024B5 RID: 9397
		internal static Type typeofTypeArray = typeof(Type[]);

		// Token: 0x040024B6 RID: 9398
		internal static Type typeofObjectArray = typeof(object[]);

		// Token: 0x040024B7 RID: 9399
		internal static Type typeofStringArray = typeof(string[]);

		// Token: 0x040024B8 RID: 9400
		internal static Type typeofBooleanArray = typeof(bool[]);

		// Token: 0x040024B9 RID: 9401
		internal static Type typeofByteArray = typeof(byte[]);

		// Token: 0x040024BA RID: 9402
		internal static Type typeofCharArray = typeof(char[]);

		// Token: 0x040024BB RID: 9403
		internal static Type typeofDecimalArray = typeof(decimal[]);

		// Token: 0x040024BC RID: 9404
		internal static Type typeofDoubleArray = typeof(double[]);

		// Token: 0x040024BD RID: 9405
		internal static Type typeofInt16Array = typeof(short[]);

		// Token: 0x040024BE RID: 9406
		internal static Type typeofInt32Array = typeof(int[]);

		// Token: 0x040024BF RID: 9407
		internal static Type typeofInt64Array = typeof(long[]);

		// Token: 0x040024C0 RID: 9408
		internal static Type typeofSByteArray = typeof(sbyte[]);

		// Token: 0x040024C1 RID: 9409
		internal static Type typeofSingleArray = typeof(float[]);

		// Token: 0x040024C2 RID: 9410
		internal static Type typeofTimeSpanArray = typeof(TimeSpan[]);

		// Token: 0x040024C3 RID: 9411
		internal static Type typeofDateTimeArray = typeof(DateTime[]);

		// Token: 0x040024C4 RID: 9412
		internal static Type typeofUInt16Array = typeof(ushort[]);

		// Token: 0x040024C5 RID: 9413
		internal static Type typeofUInt32Array = typeof(uint[]);

		// Token: 0x040024C6 RID: 9414
		internal static Type typeofUInt64Array = typeof(ulong[]);

		// Token: 0x040024C7 RID: 9415
		internal static Type typeofMarshalByRefObject = typeof(MarshalByRefObject);
	}
}

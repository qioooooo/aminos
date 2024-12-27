using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;

namespace System
{
	// Token: 0x02000124 RID: 292
	[Serializable]
	internal struct Variant
	{
		// Token: 0x060010FD RID: 4349
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InitVariant();

		// Token: 0x060010FE RID: 4350 RVA: 0x0002FB34 File Offset: 0x0002EB34
		static Variant()
		{
			Variant.InitVariant();
		}

		// Token: 0x060010FF RID: 4351
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern double GetR8FromVar();

		// Token: 0x06001100 RID: 4352
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetR4FromVar();

		// Token: 0x06001101 RID: 4353
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetFieldsR4(float val);

		// Token: 0x06001102 RID: 4354
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetFieldsR8(double val);

		// Token: 0x06001103 RID: 4355
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetFieldsObject(object val);

		// Token: 0x06001104 RID: 4356 RVA: 0x0002FCC4 File Offset: 0x0002ECC4
		internal long GetI8FromVar()
		{
			return ((long)this.m_data2 << 32) | ((long)this.m_data1 & (long)((ulong)(-1)));
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0002FCDB File Offset: 0x0002ECDB
		internal Variant(int flags, object or, int data1, int data2)
		{
			this.m_flags = flags;
			this.m_objref = or;
			this.m_data1 = data1;
			this.m_data2 = data2;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0002FCFA File Offset: 0x0002ECFA
		public Variant(bool val)
		{
			this.m_objref = null;
			this.m_flags = 2;
			this.m_data1 = (val ? 1 : 0);
			this.m_data2 = 0;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0002FD1E File Offset: 0x0002ED1E
		public Variant(sbyte val)
		{
			this.m_objref = null;
			this.m_flags = 4;
			this.m_data1 = (int)val;
			this.m_data2 = (int)((long)val >> 32);
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x0002FD41 File Offset: 0x0002ED41
		public Variant(byte val)
		{
			this.m_objref = null;
			this.m_flags = 5;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0002FD5F File Offset: 0x0002ED5F
		public Variant(short val)
		{
			this.m_objref = null;
			this.m_flags = 6;
			this.m_data1 = (int)val;
			this.m_data2 = (int)((long)val >> 32);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0002FD82 File Offset: 0x0002ED82
		public Variant(ushort val)
		{
			this.m_objref = null;
			this.m_flags = 7;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0002FDA0 File Offset: 0x0002EDA0
		public Variant(char val)
		{
			this.m_objref = null;
			this.m_flags = 3;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0002FDBE File Offset: 0x0002EDBE
		public Variant(int val)
		{
			this.m_objref = null;
			this.m_flags = 8;
			this.m_data1 = val;
			this.m_data2 = val >> 31;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0002FDDF File Offset: 0x0002EDDF
		public Variant(uint val)
		{
			this.m_objref = null;
			this.m_flags = 9;
			this.m_data1 = (int)val;
			this.m_data2 = 0;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0002FDFE File Offset: 0x0002EDFE
		public Variant(long val)
		{
			this.m_objref = null;
			this.m_flags = 10;
			this.m_data1 = (int)val;
			this.m_data2 = (int)(val >> 32);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0002FE22 File Offset: 0x0002EE22
		public Variant(ulong val)
		{
			this.m_objref = null;
			this.m_flags = 11;
			this.m_data1 = (int)val;
			this.m_data2 = (int)(val >> 32);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0002FE46 File Offset: 0x0002EE46
		public Variant(float val)
		{
			this.m_objref = null;
			this.m_flags = 12;
			this.m_data1 = 0;
			this.m_data2 = 0;
			this.SetFieldsR4(val);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0002FE6C File Offset: 0x0002EE6C
		public Variant(double val)
		{
			this.m_objref = null;
			this.m_flags = 13;
			this.m_data1 = 0;
			this.m_data2 = 0;
			this.SetFieldsR8(val);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0002FE94 File Offset: 0x0002EE94
		public Variant(DateTime val)
		{
			this.m_objref = null;
			this.m_flags = 16;
			ulong ticks = (ulong)val.Ticks;
			this.m_data1 = (int)ticks;
			this.m_data2 = (int)(ticks >> 32);
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0002FECB File Offset: 0x0002EECB
		public Variant(decimal val)
		{
			this.m_objref = val;
			this.m_flags = 19;
			this.m_data1 = 0;
			this.m_data2 = 0;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0002FEF0 File Offset: 0x0002EEF0
		public Variant(object obj)
		{
			this.m_data1 = 0;
			this.m_data2 = 0;
			VarEnum varEnum = VarEnum.VT_EMPTY;
			if (obj is DateTime)
			{
				this.m_objref = null;
				this.m_flags = 16;
				ulong ticks = (ulong)((DateTime)obj).Ticks;
				this.m_data1 = (int)ticks;
				this.m_data2 = (int)(ticks >> 32);
				return;
			}
			if (obj is string)
			{
				this.m_flags = 14;
				this.m_objref = obj;
				return;
			}
			if (obj == null)
			{
				this = Variant.Empty;
				return;
			}
			if (obj == global::System.DBNull.Value)
			{
				this = Variant.DBNull;
				return;
			}
			if (obj == Type.Missing)
			{
				this = Variant.Missing;
				return;
			}
			if (obj is Array)
			{
				this.m_flags = 65554;
				this.m_objref = obj;
				return;
			}
			this.m_flags = 0;
			this.m_objref = null;
			if (obj is UnknownWrapper)
			{
				varEnum = VarEnum.VT_UNKNOWN;
				obj = ((UnknownWrapper)obj).WrappedObject;
			}
			else if (obj is DispatchWrapper)
			{
				varEnum = VarEnum.VT_DISPATCH;
				obj = ((DispatchWrapper)obj).WrappedObject;
			}
			else if (obj is ErrorWrapper)
			{
				varEnum = VarEnum.VT_ERROR;
				obj = ((ErrorWrapper)obj).ErrorCode;
			}
			else if (obj is CurrencyWrapper)
			{
				varEnum = VarEnum.VT_CY;
				obj = ((CurrencyWrapper)obj).WrappedObject;
			}
			else if (obj is BStrWrapper)
			{
				varEnum = VarEnum.VT_BSTR;
				obj = ((BStrWrapper)obj).WrappedObject;
			}
			if (obj != null)
			{
				this.SetFieldsObject(obj);
			}
			if (varEnum != VarEnum.VT_EMPTY)
			{
				this.m_flags |= (int)((int)varEnum << 24);
			}
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00030068 File Offset: 0x0002F068
		public unsafe Variant(void* voidPointer, Type pointerType)
		{
			if (pointerType == null)
			{
				throw new ArgumentNullException("pointerType");
			}
			if (!pointerType.IsPointer)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBePointer"), "pointerType");
			}
			this.m_objref = pointerType;
			this.m_flags = 15;
			this.m_data1 = voidPointer;
			this.m_data2 = 0;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x000300BE File Offset: 0x0002F0BE
		internal int CVType
		{
			get
			{
				return this.m_flags & 65535;
			}
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000300CC File Offset: 0x0002F0CC
		public object ToObject()
		{
			switch (this.CVType)
			{
			case 0:
				return null;
			case 2:
				return this.m_data1 != 0;
			case 3:
				return (char)this.m_data1;
			case 4:
				return (sbyte)this.m_data1;
			case 5:
				return (byte)this.m_data1;
			case 6:
				return (short)this.m_data1;
			case 7:
				return (ushort)this.m_data1;
			case 8:
				return this.m_data1;
			case 9:
				return (uint)this.m_data1;
			case 10:
				return this.GetI8FromVar();
			case 11:
				return (ulong)this.GetI8FromVar();
			case 12:
				return this.GetR4FromVar();
			case 13:
				return this.GetR8FromVar();
			case 16:
				return new DateTime(this.GetI8FromVar());
			case 17:
				return new TimeSpan(this.GetI8FromVar());
			case 21:
				return this.BoxEnum();
			case 22:
				return Type.Missing;
			case 23:
				return global::System.DBNull.Value;
			}
			return this.m_objref;
		}

		// Token: 0x06001118 RID: 4376
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern object BoxEnum();

		// Token: 0x06001119 RID: 4377 RVA: 0x00030224 File Offset: 0x0002F224
		internal static void MarshalHelperConvertObjectToVariant(object o, ref Variant v)
		{
			IConvertible convertible = (RemotingServices.IsTransparentProxy(o) ? null : (o as IConvertible));
			if (o == null)
			{
				v = Variant.Empty;
				return;
			}
			if (convertible == null)
			{
				v = new Variant(o);
				return;
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			switch (convertible.GetTypeCode())
			{
			case TypeCode.Empty:
				v = Variant.Empty;
				return;
			case TypeCode.Object:
				v = new Variant(o);
				return;
			case TypeCode.DBNull:
				v = Variant.DBNull;
				return;
			case TypeCode.Boolean:
				v = new Variant(convertible.ToBoolean(invariantCulture));
				return;
			case TypeCode.Char:
				v = new Variant(convertible.ToChar(invariantCulture));
				return;
			case TypeCode.SByte:
				v = new Variant(convertible.ToSByte(invariantCulture));
				return;
			case TypeCode.Byte:
				v = new Variant(convertible.ToByte(invariantCulture));
				return;
			case TypeCode.Int16:
				v = new Variant(convertible.ToInt16(invariantCulture));
				return;
			case TypeCode.UInt16:
				v = new Variant(convertible.ToUInt16(invariantCulture));
				return;
			case TypeCode.Int32:
				v = new Variant(convertible.ToInt32(invariantCulture));
				return;
			case TypeCode.UInt32:
				v = new Variant(convertible.ToUInt32(invariantCulture));
				return;
			case TypeCode.Int64:
				v = new Variant(convertible.ToInt64(invariantCulture));
				return;
			case TypeCode.UInt64:
				v = new Variant(convertible.ToUInt64(invariantCulture));
				return;
			case TypeCode.Single:
				v = new Variant(convertible.ToSingle(invariantCulture));
				return;
			case TypeCode.Double:
				v = new Variant(convertible.ToDouble(invariantCulture));
				return;
			case TypeCode.Decimal:
				v = new Variant(convertible.ToDecimal(invariantCulture));
				return;
			case TypeCode.DateTime:
				v = new Variant(convertible.ToDateTime(invariantCulture));
				return;
			case TypeCode.String:
				v = new Variant(convertible.ToString(invariantCulture));
				return;
			}
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_UnknownTypeCode"), new object[] { convertible.GetTypeCode() }));
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00030437 File Offset: 0x0002F437
		internal static object MarshalHelperConvertVariantToObject(ref Variant v)
		{
			return v.ToObject();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00030440 File Offset: 0x0002F440
		internal static void MarshalHelperCastVariant(object pValue, int vt, ref Variant v)
		{
			IConvertible convertible = pValue as IConvertible;
			if (convertible == null)
			{
				switch (vt)
				{
				case 8:
					if (pValue == null)
					{
						v = new Variant(null);
						v.m_flags = 14;
						return;
					}
					throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_CannotCoerceByRefVariant"), new object[0]));
				case 9:
					v = new Variant(new DispatchWrapper(pValue));
					return;
				case 10:
				case 11:
					break;
				case 12:
					v = new Variant(pValue);
					return;
				case 13:
					v = new Variant(new UnknownWrapper(pValue));
					return;
				default:
					if (vt == 36)
					{
						v = new Variant(pValue);
						return;
					}
					break;
				}
				throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_CannotCoerceByRefVariant"), new object[0]));
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			switch (vt)
			{
			case 0:
				v = Variant.Empty;
				return;
			case 1:
				v = Variant.DBNull;
				return;
			case 2:
				v = new Variant(convertible.ToInt16(invariantCulture));
				return;
			case 3:
				v = new Variant(convertible.ToInt32(invariantCulture));
				return;
			case 4:
				v = new Variant(convertible.ToSingle(invariantCulture));
				return;
			case 5:
				v = new Variant(convertible.ToDouble(invariantCulture));
				return;
			case 6:
				v = new Variant(new CurrencyWrapper(convertible.ToDecimal(invariantCulture)));
				return;
			case 7:
				v = new Variant(convertible.ToDateTime(invariantCulture));
				return;
			case 8:
				v = new Variant(convertible.ToString(invariantCulture));
				return;
			case 9:
				v = new Variant(new DispatchWrapper(convertible));
				return;
			case 10:
				v = new Variant(new ErrorWrapper(convertible.ToInt32(invariantCulture)));
				return;
			case 11:
				v = new Variant(convertible.ToBoolean(invariantCulture));
				return;
			case 12:
				v = new Variant(convertible);
				return;
			case 13:
				v = new Variant(new UnknownWrapper(convertible));
				return;
			case 14:
				v = new Variant(convertible.ToDecimal(invariantCulture));
				return;
			case 16:
				v = new Variant(convertible.ToSByte(invariantCulture));
				return;
			case 17:
				v = new Variant(convertible.ToByte(invariantCulture));
				return;
			case 18:
				v = new Variant(convertible.ToUInt16(invariantCulture));
				return;
			case 19:
				v = new Variant(convertible.ToUInt32(invariantCulture));
				return;
			case 20:
				v = new Variant(convertible.ToInt64(invariantCulture));
				return;
			case 21:
				v = new Variant(convertible.ToUInt64(invariantCulture));
				return;
			case 22:
				v = new Variant(convertible.ToInt32(invariantCulture));
				return;
			case 23:
				v = new Variant(convertible.ToUInt32(invariantCulture));
				return;
			}
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_CannotCoerceByRefVariant"), new object[0]));
		}

		// Token: 0x0400058D RID: 1421
		internal const int CV_EMPTY = 0;

		// Token: 0x0400058E RID: 1422
		internal const int CV_VOID = 1;

		// Token: 0x0400058F RID: 1423
		internal const int CV_BOOLEAN = 2;

		// Token: 0x04000590 RID: 1424
		internal const int CV_CHAR = 3;

		// Token: 0x04000591 RID: 1425
		internal const int CV_I1 = 4;

		// Token: 0x04000592 RID: 1426
		internal const int CV_U1 = 5;

		// Token: 0x04000593 RID: 1427
		internal const int CV_I2 = 6;

		// Token: 0x04000594 RID: 1428
		internal const int CV_U2 = 7;

		// Token: 0x04000595 RID: 1429
		internal const int CV_I4 = 8;

		// Token: 0x04000596 RID: 1430
		internal const int CV_U4 = 9;

		// Token: 0x04000597 RID: 1431
		internal const int CV_I8 = 10;

		// Token: 0x04000598 RID: 1432
		internal const int CV_U8 = 11;

		// Token: 0x04000599 RID: 1433
		internal const int CV_R4 = 12;

		// Token: 0x0400059A RID: 1434
		internal const int CV_R8 = 13;

		// Token: 0x0400059B RID: 1435
		internal const int CV_STRING = 14;

		// Token: 0x0400059C RID: 1436
		internal const int CV_PTR = 15;

		// Token: 0x0400059D RID: 1437
		internal const int CV_DATETIME = 16;

		// Token: 0x0400059E RID: 1438
		internal const int CV_TIMESPAN = 17;

		// Token: 0x0400059F RID: 1439
		internal const int CV_OBJECT = 18;

		// Token: 0x040005A0 RID: 1440
		internal const int CV_DECIMAL = 19;

		// Token: 0x040005A1 RID: 1441
		internal const int CV_ENUM = 21;

		// Token: 0x040005A2 RID: 1442
		internal const int CV_MISSING = 22;

		// Token: 0x040005A3 RID: 1443
		internal const int CV_NULL = 23;

		// Token: 0x040005A4 RID: 1444
		internal const int CV_LAST = 24;

		// Token: 0x040005A5 RID: 1445
		internal const int TypeCodeBitMask = 65535;

		// Token: 0x040005A6 RID: 1446
		internal const int VTBitMask = -16777216;

		// Token: 0x040005A7 RID: 1447
		internal const int VTBitShift = 24;

		// Token: 0x040005A8 RID: 1448
		internal const int ArrayBitMask = 65536;

		// Token: 0x040005A9 RID: 1449
		internal const int EnumI1 = 1048576;

		// Token: 0x040005AA RID: 1450
		internal const int EnumU1 = 2097152;

		// Token: 0x040005AB RID: 1451
		internal const int EnumI2 = 3145728;

		// Token: 0x040005AC RID: 1452
		internal const int EnumU2 = 4194304;

		// Token: 0x040005AD RID: 1453
		internal const int EnumI4 = 5242880;

		// Token: 0x040005AE RID: 1454
		internal const int EnumU4 = 6291456;

		// Token: 0x040005AF RID: 1455
		internal const int EnumI8 = 7340032;

		// Token: 0x040005B0 RID: 1456
		internal const int EnumU8 = 8388608;

		// Token: 0x040005B1 RID: 1457
		internal const int EnumMask = 15728640;

		// Token: 0x040005B2 RID: 1458
		private object m_objref;

		// Token: 0x040005B3 RID: 1459
		private int m_data1;

		// Token: 0x040005B4 RID: 1460
		private int m_data2;

		// Token: 0x040005B5 RID: 1461
		private int m_flags;

		// Token: 0x040005B6 RID: 1462
		private static Type _voidPtr = null;

		// Token: 0x040005B7 RID: 1463
		internal static readonly Type[] ClassTypes = new Type[]
		{
			typeof(Empty),
			typeof(void),
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
			typeof(string),
			typeof(void),
			typeof(DateTime),
			typeof(TimeSpan),
			typeof(object),
			typeof(decimal),
			typeof(object),
			typeof(Missing),
			typeof(DBNull)
		};

		// Token: 0x040005B8 RID: 1464
		internal static readonly Variant Empty = default(Variant);

		// Token: 0x040005B9 RID: 1465
		internal static readonly Variant Missing = new Variant(22, Type.Missing, 0, 0);

		// Token: 0x040005BA RID: 1466
		internal static readonly Variant DBNull = new Variant(23, global::System.DBNull.Value, 0, 0);
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.Serialization;

namespace System.Data.Common
{
	// Token: 0x02000113 RID: 275
	internal abstract class DataStorage
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x0021B7A0 File Offset: 0x0021ABA0
		protected DataStorage(DataColumn column, Type type, object defaultValue)
			: this(column, type, defaultValue, DBNull.Value, false)
		{
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0021B7BC File Offset: 0x0021ABBC
		protected DataStorage(DataColumn column, Type type, object defaultValue, object nullValue)
			: this(column, type, defaultValue, nullValue, false)
		{
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0021B7D8 File Offset: 0x0021ABD8
		protected DataStorage(DataColumn column, Type type, object defaultValue, object nullValue, bool isICloneable)
		{
			this.Column = column;
			this.Table = column.Table;
			this.DataType = type;
			this.StorageTypeCode = DataStorage.GetStorageType(type);
			this.DefaultValue = defaultValue;
			this.NullValue = nullValue;
			this.IsCloneable = isICloneable;
			this.IsCustomDefinedType = DataStorage.IsTypeCustomType(this.StorageTypeCode);
			this.IsStringType = StorageType.String == this.StorageTypeCode || StorageType.SqlString == this.StorageTypeCode;
			this.IsValueType = DataStorage.DetermineIfValueType(this.StorageTypeCode, type);
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x0021B868 File Offset: 0x0021AC68
		internal DataSetDateTime DateTimeMode
		{
			get
			{
				return this.Column.DateTimeMode;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0021B880 File Offset: 0x0021AC80
		internal IFormatProvider FormatProvider
		{
			get
			{
				return this.Table.FormatProvider;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x0021B898 File Offset: 0x0021AC98
		public virtual object Aggregate(int[] recordNos, AggregateType kind)
		{
			if (AggregateType.Count == kind)
			{
				int num = 0;
				for (int i = 0; i < recordNos.Length; i++)
				{
					if (!this.dbNullBits.Get(recordNos[i]))
					{
						num++;
					}
				}
				return num;
			}
			return null;
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0021B8D8 File Offset: 0x0021ACD8
		protected int CompareBits(int recordNo1, int recordNo2)
		{
			bool flag = this.dbNullBits.Get(recordNo1);
			bool flag2 = this.dbNullBits.Get(recordNo2);
			if (!(flag ^ flag2))
			{
				return 0;
			}
			if (flag)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0021B90C File Offset: 0x0021AD0C
		public virtual int Compare(int recordNo1, int recordNo2)
		{
			object obj = this.Get(recordNo1);
			if (obj is IComparable)
			{
				object obj2 = this.Get(recordNo2);
				if (obj2.GetType() == obj.GetType())
				{
					return ((IComparable)obj).CompareTo(obj2);
				}
				this.CompareBits(recordNo1, recordNo2);
			}
			return 0;
		}

		// Token: 0x0600116A RID: 4458
		public abstract int CompareValueTo(int recordNo1, object value);

		// Token: 0x0600116B RID: 4459 RVA: 0x0021B958 File Offset: 0x0021AD58
		public virtual object ConvertValue(object value)
		{
			return value;
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0021B968 File Offset: 0x0021AD68
		protected void CopyBits(int srcRecordNo, int dstRecordNo)
		{
			this.dbNullBits.Set(dstRecordNo, this.dbNullBits.Get(srcRecordNo));
		}

		// Token: 0x0600116D RID: 4461
		public abstract void Copy(int recordNo1, int recordNo2);

		// Token: 0x0600116E RID: 4462
		public abstract object Get(int recordNo);

		// Token: 0x0600116F RID: 4463 RVA: 0x0021B990 File Offset: 0x0021AD90
		protected object GetBits(int recordNo)
		{
			if (this.dbNullBits.Get(recordNo))
			{
				return this.NullValue;
			}
			return this.DefaultValue;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x0021B9B8 File Offset: 0x0021ADB8
		public virtual int GetStringLength(int record)
		{
			return int.MaxValue;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x0021B9CC File Offset: 0x0021ADCC
		protected bool HasValue(int recordNo)
		{
			return !this.dbNullBits.Get(recordNo);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x0021B9E8 File Offset: 0x0021ADE8
		public virtual bool IsNull(int recordNo)
		{
			return this.dbNullBits.Get(recordNo);
		}

		// Token: 0x06001173 RID: 4467
		public abstract void Set(int recordNo, object value);

		// Token: 0x06001174 RID: 4468 RVA: 0x0021BA04 File Offset: 0x0021AE04
		protected void SetNullBit(int recordNo, bool flag)
		{
			this.dbNullBits.Set(recordNo, flag);
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0021BA20 File Offset: 0x0021AE20
		public virtual void SetCapacity(int capacity)
		{
			if (this.dbNullBits == null)
			{
				this.dbNullBits = new BitArray(capacity);
				return;
			}
			this.dbNullBits.Length = capacity;
		}

		// Token: 0x06001176 RID: 4470
		public abstract object ConvertXmlToObject(string s);

		// Token: 0x06001177 RID: 4471 RVA: 0x0021BA50 File Offset: 0x0021AE50
		public virtual object ConvertXmlToObject(XmlReader xmlReader, XmlRootAttribute xmlAttrib)
		{
			return this.ConvertXmlToObject(xmlReader.Value);
		}

		// Token: 0x06001178 RID: 4472
		public abstract string ConvertObjectToXml(object value);

		// Token: 0x06001179 RID: 4473 RVA: 0x0021BA6C File Offset: 0x0021AE6C
		public virtual void ConvertObjectToXml(object value, XmlWriter xmlWriter, XmlRootAttribute xmlAttrib)
		{
			xmlWriter.WriteString(this.ConvertObjectToXml(value));
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0021BA88 File Offset: 0x0021AE88
		public static DataStorage CreateStorage(DataColumn column, Type dataType)
		{
			StorageType storageType = DataStorage.GetStorageType(dataType);
			if (storageType != StorageType.Empty || dataType == null)
			{
				switch (storageType)
				{
				case StorageType.Empty:
					throw ExceptionBuilder.InvalidStorageType(TypeCode.Empty);
				case StorageType.DBNull:
					throw ExceptionBuilder.InvalidStorageType(TypeCode.DBNull);
				case StorageType.Boolean:
					return new BooleanStorage(column);
				case StorageType.Char:
					return new CharStorage(column);
				case StorageType.SByte:
					return new SByteStorage(column);
				case StorageType.Byte:
					return new ByteStorage(column);
				case StorageType.Int16:
					return new Int16Storage(column);
				case StorageType.UInt16:
					return new UInt16Storage(column);
				case StorageType.Int32:
					return new Int32Storage(column);
				case StorageType.UInt32:
					return new UInt32Storage(column);
				case StorageType.Int64:
					return new Int64Storage(column);
				case StorageType.UInt64:
					return new UInt64Storage(column);
				case StorageType.Single:
					return new SingleStorage(column);
				case StorageType.Double:
					return new DoubleStorage(column);
				case StorageType.Decimal:
					return new DecimalStorage(column);
				case StorageType.DateTime:
					return new DateTimeStorage(column);
				case StorageType.TimeSpan:
					return new TimeSpanStorage(column);
				case StorageType.String:
					return new StringStorage(column);
				case StorageType.Guid:
					return new ObjectStorage(column, dataType);
				case StorageType.ByteArray:
					return new ObjectStorage(column, dataType);
				case StorageType.CharArray:
					return new ObjectStorage(column, dataType);
				case StorageType.Type:
					return new ObjectStorage(column, dataType);
				case StorageType.DateTimeOffset:
					return new DateTimeOffsetStorage(column);
				case StorageType.Uri:
					return new ObjectStorage(column, dataType);
				case StorageType.SqlBinary:
					return new SqlBinaryStorage(column);
				case StorageType.SqlBoolean:
					return new SqlBooleanStorage(column);
				case StorageType.SqlByte:
					return new SqlByteStorage(column);
				case StorageType.SqlBytes:
					return new SqlBytesStorage(column);
				case StorageType.SqlChars:
					return new SqlCharsStorage(column);
				case StorageType.SqlDateTime:
					return new SqlDateTimeStorage(column);
				case StorageType.SqlDecimal:
					return new SqlDecimalStorage(column);
				case StorageType.SqlDouble:
					return new SqlDoubleStorage(column);
				case StorageType.SqlGuid:
					return new SqlGuidStorage(column);
				case StorageType.SqlInt16:
					return new SqlInt16Storage(column);
				case StorageType.SqlInt32:
					return new SqlInt32Storage(column);
				case StorageType.SqlInt64:
					return new SqlInt64Storage(column);
				case StorageType.SqlMoney:
					return new SqlMoneyStorage(column);
				case StorageType.SqlSingle:
					return new SqlSingleStorage(column);
				case StorageType.SqlString:
					return new SqlStringStorage(column);
				}
				return new ObjectStorage(column, dataType);
			}
			if (typeof(INullable).IsAssignableFrom(dataType))
			{
				return new SqlUdtStorage(column, dataType);
			}
			return new ObjectStorage(column, dataType);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x0021BC8C File Offset: 0x0021B08C
		internal static StorageType GetStorageType(Type dataType)
		{
			for (int i = 0; i < DataStorage.StorageClassType.Length; i++)
			{
				if (dataType == DataStorage.StorageClassType[i])
				{
					return (StorageType)i;
				}
			}
			TypeCode typeCode = Type.GetTypeCode(dataType);
			if (TypeCode.Object != typeCode)
			{
				return (StorageType)typeCode;
			}
			return StorageType.Empty;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x0021BCC8 File Offset: 0x0021B0C8
		internal static Type GetTypeStorage(StorageType storageType)
		{
			return DataStorage.StorageClassType[(int)storageType];
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x0021BCDC File Offset: 0x0021B0DC
		internal static bool IsTypeCustomType(Type type)
		{
			return DataStorage.IsTypeCustomType(DataStorage.GetStorageType(type));
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0021BCF4 File Offset: 0x0021B0F4
		internal static bool IsTypeCustomType(StorageType typeCode)
		{
			return StorageType.Object == typeCode || typeCode == StorageType.Empty || StorageType.CharArray == typeCode;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0021BD10 File Offset: 0x0021B110
		internal static bool IsSqlType(StorageType storageType)
		{
			return StorageType.SqlBinary <= storageType;
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0021BD28 File Offset: 0x0021B128
		public static bool IsSqlType(Type dataType)
		{
			for (int i = 25; i < DataStorage.StorageClassType.Length; i++)
			{
				if (dataType == DataStorage.StorageClassType[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x0021BD58 File Offset: 0x0021B158
		private static bool DetermineIfValueType(StorageType typeCode, Type dataType)
		{
			bool flag;
			switch (typeCode)
			{
			case StorageType.Boolean:
			case StorageType.Char:
			case StorageType.SByte:
			case StorageType.Byte:
			case StorageType.Int16:
			case StorageType.UInt16:
			case StorageType.Int32:
			case StorageType.UInt32:
			case StorageType.Int64:
			case StorageType.UInt64:
			case StorageType.Single:
			case StorageType.Double:
			case StorageType.Decimal:
			case StorageType.DateTime:
			case StorageType.TimeSpan:
			case StorageType.Guid:
			case StorageType.DateTimeOffset:
			case StorageType.SqlBinary:
			case StorageType.SqlBoolean:
			case StorageType.SqlByte:
			case StorageType.SqlDateTime:
			case StorageType.SqlDecimal:
			case StorageType.SqlDouble:
			case StorageType.SqlGuid:
			case StorageType.SqlInt16:
			case StorageType.SqlInt32:
			case StorageType.SqlInt64:
			case StorageType.SqlMoney:
			case StorageType.SqlSingle:
			case StorageType.SqlString:
				flag = true;
				break;
			case StorageType.String:
			case StorageType.ByteArray:
			case StorageType.CharArray:
			case StorageType.Type:
			case StorageType.Uri:
			case StorageType.SqlBytes:
			case StorageType.SqlChars:
				flag = false;
				break;
			default:
				flag = dataType.IsValueType;
				break;
			}
			return flag;
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0021BE18 File Offset: 0x0021B218
		internal static void ImplementsInterfaces(StorageType typeCode, Type dataType, out bool sqlType, out bool nullable, out bool xmlSerializable, out bool changeTracking, out bool revertibleChangeTracking)
		{
			if (DataStorage.IsSqlType(typeCode))
			{
				sqlType = true;
				nullable = true;
				changeTracking = false;
				revertibleChangeTracking = false;
				xmlSerializable = true;
				return;
			}
			if (typeCode != StorageType.Empty)
			{
				sqlType = false;
				nullable = false;
				changeTracking = false;
				revertibleChangeTracking = false;
				xmlSerializable = false;
				return;
			}
			sqlType = false;
			nullable = typeof(INullable).IsAssignableFrom(dataType);
			changeTracking = typeof(IChangeTracking).IsAssignableFrom(dataType);
			revertibleChangeTracking = typeof(IRevertibleChangeTracking).IsAssignableFrom(dataType);
			xmlSerializable = typeof(IXmlSerializable).IsAssignableFrom(dataType);
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0021BEA4 File Offset: 0x0021B2A4
		internal static bool ImplementsINullableValue(StorageType typeCode, Type dataType)
		{
			return typeCode == StorageType.Empty && dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0021BED0 File Offset: 0x0021B2D0
		public static bool IsObjectNull(object value)
		{
			return value == null || DBNull.Value == value || DataStorage.IsObjectSqlNull(value);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0021BEF0 File Offset: 0x0021B2F0
		public static bool IsObjectSqlNull(object value)
		{
			INullable nullable = value as INullable;
			return nullable != null && nullable.IsNull;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0021BF10 File Offset: 0x0021B310
		internal object GetEmptyStorageInternal(int recordCount)
		{
			return this.GetEmptyStorage(recordCount);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0021BF24 File Offset: 0x0021B324
		internal void CopyValueInternal(int record, object store, BitArray nullbits, int storeIndex)
		{
			this.CopyValue(record, store, nullbits, storeIndex);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0021BF3C File Offset: 0x0021B33C
		internal void SetStorageInternal(object store, BitArray nullbits)
		{
			this.SetStorage(store, nullbits);
		}

		// Token: 0x06001189 RID: 4489
		protected abstract object GetEmptyStorage(int recordCount);

		// Token: 0x0600118A RID: 4490
		protected abstract void CopyValue(int record, object store, BitArray nullbits, int storeIndex);

		// Token: 0x0600118B RID: 4491
		protected abstract void SetStorage(object store, BitArray nullbits);

		// Token: 0x0600118C RID: 4492 RVA: 0x0021BF54 File Offset: 0x0021B354
		protected void SetNullStorage(BitArray nullbits)
		{
			this.dbNullBits = nullbits;
		}

		// Token: 0x04000B5A RID: 2906
		private static readonly Type[] StorageClassType = new Type[]
		{
			null,
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
			typeof(TimeSpan),
			typeof(string),
			typeof(Guid),
			typeof(byte[]),
			typeof(char[]),
			typeof(Type),
			typeof(DateTimeOffset),
			typeof(Uri),
			typeof(SqlBinary),
			typeof(SqlBoolean),
			typeof(SqlByte),
			typeof(SqlBytes),
			typeof(SqlChars),
			typeof(SqlDateTime),
			typeof(SqlDecimal),
			typeof(SqlDouble),
			typeof(SqlGuid),
			typeof(SqlInt16),
			typeof(SqlInt32),
			typeof(SqlInt64),
			typeof(SqlMoney),
			typeof(SqlSingle),
			typeof(SqlString)
		};

		// Token: 0x04000B5B RID: 2907
		internal readonly DataColumn Column;

		// Token: 0x04000B5C RID: 2908
		internal readonly DataTable Table;

		// Token: 0x04000B5D RID: 2909
		internal readonly Type DataType;

		// Token: 0x04000B5E RID: 2910
		internal readonly StorageType StorageTypeCode;

		// Token: 0x04000B5F RID: 2911
		private BitArray dbNullBits;

		// Token: 0x04000B60 RID: 2912
		private readonly object DefaultValue;

		// Token: 0x04000B61 RID: 2913
		internal readonly object NullValue;

		// Token: 0x04000B62 RID: 2914
		internal readonly bool IsCloneable;

		// Token: 0x04000B63 RID: 2915
		internal readonly bool IsCustomDefinedType;

		// Token: 0x04000B64 RID: 2916
		internal readonly bool IsStringType;

		// Token: 0x04000B65 RID: 2917
		internal readonly bool IsValueType;
	}
}

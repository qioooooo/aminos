using System;
using System.Data.SqlTypes;
using System.Globalization;
using System.Xml;

namespace System.Data.Common
{
	// Token: 0x02000164 RID: 356
	internal static class SqlConvert
	{
		// Token: 0x0600161A RID: 5658 RVA: 0x0022DE6C File Offset: 0x0022D26C
		public static SqlByte ConvertToSqlByte(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlByte.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.Byte)
			{
				return (byte)value;
			}
			if (storageType2 == StorageType.SqlByte)
			{
				return (SqlByte)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlByte));
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0022DEC4 File Offset: 0x0022D2C4
		public static SqlInt16 ConvertToSqlInt16(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlInt16.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			switch (storageType2)
			{
			case StorageType.Byte:
				return (short)((byte)value);
			case StorageType.Int16:
				return (short)value;
			default:
				if (storageType2 == StorageType.SqlByte)
				{
					return (SqlByte)value;
				}
				if (storageType2 != StorageType.SqlInt16)
				{
					throw ExceptionBuilder.ConvertFailed(type, typeof(SqlInt16));
				}
				return (SqlInt16)value;
			}
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0022DF48 File Offset: 0x0022D348
		public static SqlInt32 ConvertToSqlInt32(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlInt32.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			switch (storageType2)
			{
			case StorageType.Byte:
				return (int)((byte)value);
			case StorageType.Int16:
				return (int)((short)value);
			case StorageType.UInt16:
				return (int)((ushort)value);
			case StorageType.Int32:
				return (int)value;
			default:
				if (storageType2 == StorageType.SqlByte)
				{
					return (SqlByte)value;
				}
				switch (storageType2)
				{
				case StorageType.SqlInt16:
					return (SqlInt16)value;
				case StorageType.SqlInt32:
					return (SqlInt32)value;
				default:
					throw ExceptionBuilder.ConvertFailed(type, typeof(SqlInt32));
				}
				break;
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0022E004 File Offset: 0x0022D404
		public static SqlInt64 ConvertToSqlInt64(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlInt32.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			switch (storageType2)
			{
			case StorageType.Byte:
				return (long)((ulong)((byte)value));
			case StorageType.Int16:
				return (long)((short)value);
			case StorageType.UInt16:
				return (long)((ulong)((ushort)value));
			case StorageType.Int32:
				return (long)((int)value);
			case StorageType.UInt32:
				return (long)((ulong)((uint)value));
			case StorageType.Int64:
				return (long)value;
			default:
				if (storageType2 == StorageType.SqlByte)
				{
					return (SqlByte)value;
				}
				switch (storageType2)
				{
				case StorageType.SqlInt16:
					return (SqlInt16)value;
				case StorageType.SqlInt32:
					return (SqlInt32)value;
				case StorageType.SqlInt64:
					return (SqlInt64)value;
				default:
					throw ExceptionBuilder.ConvertFailed(type, typeof(SqlInt64));
				}
				break;
			}
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0022E100 File Offset: 0x0022D500
		public static SqlDouble ConvertToSqlDouble(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlDouble.Null;
			}
			Type type = value.GetType();
			switch (DataStorage.GetStorageType(type))
			{
			case StorageType.Byte:
				return (double)((byte)value);
			case StorageType.Int16:
				return (double)((short)value);
			case StorageType.UInt16:
				return (double)((ushort)value);
			case StorageType.Int32:
				return (double)((int)value);
			case StorageType.UInt32:
				return (uint)value;
			case StorageType.Int64:
				return (double)((long)value);
			case StorageType.UInt64:
				return (ulong)value;
			case StorageType.Single:
				return (double)((float)value);
			case StorageType.Double:
				return (double)value;
			case StorageType.SqlByte:
				return (SqlByte)value;
			case StorageType.SqlDecimal:
				return (SqlDecimal)value;
			case StorageType.SqlDouble:
				return (SqlDouble)value;
			case StorageType.SqlInt16:
				return (SqlInt16)value;
			case StorageType.SqlInt32:
				return (SqlInt32)value;
			case StorageType.SqlInt64:
				return (SqlInt64)value;
			case StorageType.SqlMoney:
				return (SqlMoney)value;
			case StorageType.SqlSingle:
				return (SqlSingle)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlDouble));
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0022E2A0 File Offset: 0x0022D6A0
		public static SqlDecimal ConvertToSqlDecimal(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlDecimal.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			switch (storageType2)
			{
			case StorageType.Byte:
				return (long)((ulong)((byte)value));
			case StorageType.Int16:
				return (long)((short)value);
			case StorageType.UInt16:
				return (long)((ulong)((ushort)value));
			case StorageType.Int32:
				return (long)((int)value);
			case StorageType.UInt32:
				return (long)((ulong)((uint)value));
			case StorageType.Int64:
				return (long)value;
			case StorageType.UInt64:
				return (ulong)value;
			case StorageType.Single:
			case StorageType.Double:
				break;
			case StorageType.Decimal:
				return (decimal)value;
			default:
				switch (storageType2)
				{
				case StorageType.SqlByte:
					return (SqlByte)value;
				case StorageType.SqlDecimal:
					return (SqlDecimal)value;
				case StorageType.SqlInt16:
					return (SqlInt16)value;
				case StorageType.SqlInt32:
					return (SqlInt32)value;
				case StorageType.SqlInt64:
					return (SqlInt64)value;
				case StorageType.SqlMoney:
					return (SqlMoney)value;
				}
				break;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlDecimal));
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0022E3F4 File Offset: 0x0022D7F4
		public static SqlSingle ConvertToSqlSingle(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlSingle.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			switch (storageType2)
			{
			case StorageType.Byte:
				return (float)((byte)value);
			case StorageType.Int16:
				return (float)((short)value);
			case StorageType.UInt16:
				return (float)((ushort)value);
			case StorageType.Int32:
				return (float)((int)value);
			case StorageType.UInt32:
				return (uint)value;
			case StorageType.Int64:
				return (float)((long)value);
			case StorageType.UInt64:
				return (ulong)value;
			case StorageType.Single:
				return (float)value;
			default:
				switch (storageType2)
				{
				case StorageType.SqlByte:
					return (SqlByte)value;
				case StorageType.SqlDecimal:
					return (SqlDecimal)value;
				case StorageType.SqlInt16:
					return (SqlInt16)value;
				case StorageType.SqlInt32:
					return (SqlInt32)value;
				case StorageType.SqlInt64:
					return (SqlInt64)value;
				case StorageType.SqlMoney:
					return (SqlMoney)value;
				case StorageType.SqlSingle:
					return (SqlSingle)value;
				}
				throw ExceptionBuilder.ConvertFailed(type, typeof(SqlSingle));
			}
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0022E550 File Offset: 0x0022D950
		public static SqlMoney ConvertToSqlMoney(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlMoney.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			switch (storageType2)
			{
			case StorageType.Byte:
				return (long)((ulong)((byte)value));
			case StorageType.Int16:
				return (long)((short)value);
			case StorageType.UInt16:
				return (long)((ulong)((ushort)value));
			case StorageType.Int32:
				return (long)((int)value);
			case StorageType.UInt32:
				return (long)((ulong)((uint)value));
			case StorageType.Int64:
				return (long)value;
			case StorageType.UInt64:
				return (ulong)value;
			case StorageType.Single:
			case StorageType.Double:
				break;
			case StorageType.Decimal:
				return (decimal)value;
			default:
				if (storageType2 == StorageType.SqlByte)
				{
					return (SqlByte)value;
				}
				switch (storageType2)
				{
				case StorageType.SqlInt16:
					return (SqlInt16)value;
				case StorageType.SqlInt32:
					return (SqlInt32)value;
				case StorageType.SqlInt64:
					return (SqlInt64)value;
				case StorageType.SqlMoney:
					return (SqlMoney)value;
				}
				break;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlMoney));
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0022E684 File Offset: 0x0022DA84
		public static SqlDateTime ConvertToSqlDateTime(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlDateTime.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.DateTime)
			{
				return (DateTime)value;
			}
			if (storageType2 == StorageType.SqlDateTime)
			{
				return (SqlDateTime)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlDateTime));
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0022E6DC File Offset: 0x0022DADC
		public static SqlBoolean ConvertToSqlBoolean(object value)
		{
			if (value == DBNull.Value || value == null)
			{
				return SqlBoolean.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.Boolean)
			{
				return (bool)value;
			}
			if (storageType2 == StorageType.SqlBoolean)
			{
				return (SqlBoolean)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlBoolean));
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0022E738 File Offset: 0x0022DB38
		public static SqlGuid ConvertToSqlGuid(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlGuid.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.Guid)
			{
				return (Guid)value;
			}
			if (storageType2 == StorageType.SqlGuid)
			{
				return (SqlGuid)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlGuid));
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0022E790 File Offset: 0x0022DB90
		public static SqlBinary ConvertToSqlBinary(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlBinary.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.ByteArray)
			{
				return (byte[])value;
			}
			if (storageType2 == StorageType.SqlBinary)
			{
				return (SqlBinary)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlBinary));
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0022E7E8 File Offset: 0x0022DBE8
		public static SqlString ConvertToSqlString(object value)
		{
			if (value == DBNull.Value || value == null)
			{
				return SqlString.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.String)
			{
				return (string)value;
			}
			if (storageType2 == StorageType.SqlString)
			{
				return (SqlString)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlString));
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0022E844 File Offset: 0x0022DC44
		public static SqlChars ConvertToSqlChars(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlChars.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.SqlChars)
			{
				return (SqlChars)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlChars));
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0022E88C File Offset: 0x0022DC8C
		public static SqlBytes ConvertToSqlBytes(object value)
		{
			if (value == DBNull.Value)
			{
				return SqlBytes.Null;
			}
			Type type = value.GetType();
			StorageType storageType = DataStorage.GetStorageType(type);
			StorageType storageType2 = storageType;
			if (storageType2 == StorageType.SqlBytes)
			{
				return (SqlBytes)value;
			}
			throw ExceptionBuilder.ConvertFailed(type, typeof(SqlBytes));
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0022E8D4 File Offset: 0x0022DCD4
		public static DateTimeOffset ConvertStringToDateTimeOffset(string value, IFormatProvider formatProvider)
		{
			return DateTimeOffset.Parse(value, formatProvider);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0022E8E8 File Offset: 0x0022DCE8
		public static object ChangeType(object value, Type type, IFormatProvider formatProvider)
		{
			return SqlConvert.ChangeType2(value, DataStorage.GetStorageType(type), type, formatProvider);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0022E904 File Offset: 0x0022DD04
		public static object ChangeType2(object value, StorageType stype, Type type, IFormatProvider formatProvider)
		{
			switch (stype)
			{
			case StorageType.SqlBinary:
				return SqlConvert.ConvertToSqlBinary(value);
			case StorageType.SqlBoolean:
				return SqlConvert.ConvertToSqlBoolean(value);
			case StorageType.SqlByte:
				return SqlConvert.ConvertToSqlByte(value);
			case StorageType.SqlBytes:
				return SqlConvert.ConvertToSqlBytes(value);
			case StorageType.SqlChars:
				return SqlConvert.ConvertToSqlChars(value);
			case StorageType.SqlDateTime:
				return SqlConvert.ConvertToSqlDateTime(value);
			case StorageType.SqlDecimal:
				return SqlConvert.ConvertToSqlDecimal(value);
			case StorageType.SqlDouble:
				return SqlConvert.ConvertToSqlDouble(value);
			case StorageType.SqlGuid:
				return SqlConvert.ConvertToSqlGuid(value);
			case StorageType.SqlInt16:
				return SqlConvert.ConvertToSqlInt16(value);
			case StorageType.SqlInt32:
				return SqlConvert.ConvertToSqlInt32(value);
			case StorageType.SqlInt64:
				return SqlConvert.ConvertToSqlInt64(value);
			case StorageType.SqlMoney:
				return SqlConvert.ConvertToSqlMoney(value);
			case StorageType.SqlSingle:
				return SqlConvert.ConvertToSqlSingle(value);
			case StorageType.SqlString:
				return SqlConvert.ConvertToSqlString(value);
			default:
			{
				if (DBNull.Value == value || value == null)
				{
					return DBNull.Value;
				}
				Type type2 = value.GetType();
				StorageType storageType = DataStorage.GetStorageType(type2);
				switch (storageType)
				{
				case StorageType.SqlBinary:
				case StorageType.SqlBoolean:
				case StorageType.SqlByte:
				case StorageType.SqlBytes:
				case StorageType.SqlChars:
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
					throw ExceptionBuilder.ConvertFailed(type2, type);
				default:
					if (StorageType.String == stype)
					{
						switch (storageType)
						{
						case StorageType.Boolean:
							return ((IConvertible)((bool)value)).ToString(formatProvider);
						case StorageType.Char:
							return ((IConvertible)((char)value)).ToString(formatProvider);
						case StorageType.SByte:
							return ((IConvertible)((sbyte)value)).ToString(formatProvider);
						case StorageType.Byte:
							return ((IConvertible)((byte)value)).ToString(formatProvider);
						case StorageType.Int16:
							return ((IConvertible)((short)value)).ToString(formatProvider);
						case StorageType.UInt16:
							return ((IConvertible)((ushort)value)).ToString(formatProvider);
						case StorageType.Int32:
							return ((IConvertible)((int)value)).ToString(formatProvider);
						case StorageType.UInt32:
							return ((IConvertible)((uint)value)).ToString(formatProvider);
						case StorageType.Int64:
							return ((IConvertible)((long)value)).ToString(formatProvider);
						case StorageType.UInt64:
							return ((IConvertible)((ulong)value)).ToString(formatProvider);
						case StorageType.Single:
							return ((IConvertible)((float)value)).ToString(formatProvider);
						case StorageType.Double:
							return ((IConvertible)((double)value)).ToString(formatProvider);
						case StorageType.Decimal:
							return ((IConvertible)((decimal)value)).ToString(formatProvider);
						case StorageType.DateTime:
							return ((IConvertible)((DateTime)value)).ToString(formatProvider);
						case StorageType.TimeSpan:
							return XmlConvert.ToString((TimeSpan)value);
						case StorageType.String:
							return (string)value;
						case StorageType.Guid:
							return XmlConvert.ToString((Guid)value);
						case StorageType.CharArray:
							return new string((char[])value);
						case StorageType.DateTimeOffset:
							return ((DateTimeOffset)value).ToString(formatProvider);
						}
						IConvertible convertible = value as IConvertible;
						if (convertible != null)
						{
							return convertible.ToString(formatProvider);
						}
						IFormattable formattable = value as IFormattable;
						if (formattable != null)
						{
							return formattable.ToString(null, formatProvider);
						}
						return value.ToString();
					}
					else
					{
						if (StorageType.TimeSpan == stype)
						{
							StorageType storageType2 = storageType;
							switch (storageType2)
							{
							case StorageType.Int32:
								return new TimeSpan((long)((int)value));
							case StorageType.UInt32:
								break;
							case StorageType.Int64:
								return new TimeSpan((long)value);
							default:
								if (storageType2 == StorageType.String)
								{
									return XmlConvert.ToTimeSpan((string)value);
								}
								break;
							}
							return (TimeSpan)value;
						}
						if (StorageType.DateTimeOffset == stype)
						{
							return (DateTimeOffset)value;
						}
						if (StorageType.String == storageType)
						{
							switch (stype)
							{
							case StorageType.Boolean:
								if ("1" == (string)value)
								{
									return true;
								}
								if ("0" == (string)value)
								{
									return false;
								}
								break;
							case StorageType.Char:
								return ((IConvertible)((string)value)).ToChar(formatProvider);
							case StorageType.SByte:
								return ((IConvertible)((string)value)).ToSByte(formatProvider);
							case StorageType.Byte:
								return ((IConvertible)((string)value)).ToByte(formatProvider);
							case StorageType.Int16:
								return ((IConvertible)((string)value)).ToInt16(formatProvider);
							case StorageType.UInt16:
								return ((IConvertible)((string)value)).ToUInt16(formatProvider);
							case StorageType.Int32:
								return ((IConvertible)((string)value)).ToInt32(formatProvider);
							case StorageType.UInt32:
								return ((IConvertible)((string)value)).ToUInt32(formatProvider);
							case StorageType.Int64:
								return ((IConvertible)((string)value)).ToInt64(formatProvider);
							case StorageType.UInt64:
								return ((IConvertible)((string)value)).ToUInt64(formatProvider);
							case StorageType.Single:
								return ((IConvertible)((string)value)).ToSingle(formatProvider);
							case StorageType.Double:
								return ((IConvertible)((string)value)).ToDouble(formatProvider);
							case StorageType.Decimal:
								return ((IConvertible)((string)value)).ToDecimal(formatProvider);
							case StorageType.DateTime:
								return ((IConvertible)((string)value)).ToDateTime(formatProvider);
							case StorageType.TimeSpan:
								return XmlConvert.ToTimeSpan((string)value);
							case StorageType.String:
								return (string)value;
							case StorageType.Guid:
								return XmlConvert.ToGuid((string)value);
							case StorageType.Uri:
								return new Uri((string)value);
							}
						}
						return Convert.ChangeType(value, type, formatProvider);
					}
					break;
				}
				break;
			}
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0022EE90 File Offset: 0x0022E290
		public static object ChangeTypeForXML(object value, Type type)
		{
			StorageType storageType = DataStorage.GetStorageType(type);
			Type type2 = value.GetType();
			StorageType storageType2 = DataStorage.GetStorageType(type2);
			switch (storageType)
			{
			case StorageType.Boolean:
				if ("1" == (string)value)
				{
					return true;
				}
				if ("0" == (string)value)
				{
					return false;
				}
				return XmlConvert.ToBoolean((string)value);
			case StorageType.Char:
				return XmlConvert.ToChar((string)value);
			case StorageType.SByte:
				return XmlConvert.ToSByte((string)value);
			case StorageType.Byte:
				return XmlConvert.ToByte((string)value);
			case StorageType.Int16:
				return XmlConvert.ToInt16((string)value);
			case StorageType.UInt16:
				return XmlConvert.ToUInt16((string)value);
			case StorageType.Int32:
				return XmlConvert.ToInt32((string)value);
			case StorageType.UInt32:
				return XmlConvert.ToUInt32((string)value);
			case StorageType.Int64:
				return XmlConvert.ToInt64((string)value);
			case StorageType.UInt64:
				return XmlConvert.ToUInt64((string)value);
			case StorageType.Single:
				return XmlConvert.ToSingle((string)value);
			case StorageType.Double:
				return XmlConvert.ToDouble((string)value);
			case StorageType.Decimal:
				return XmlConvert.ToDecimal((string)value);
			case StorageType.DateTime:
				return XmlConvert.ToDateTime((string)value, XmlDateTimeSerializationMode.RoundtripKind);
			case StorageType.TimeSpan:
			{
				StorageType storageType3 = storageType2;
				switch (storageType3)
				{
				case StorageType.Int32:
					return new TimeSpan((long)((int)value));
				case StorageType.UInt32:
					break;
				case StorageType.Int64:
					return new TimeSpan((long)value);
				default:
					if (storageType3 == StorageType.String)
					{
						return XmlConvert.ToTimeSpan((string)value);
					}
					break;
				}
				return (TimeSpan)value;
			}
			case StorageType.Guid:
				return XmlConvert.ToGuid((string)value);
			case StorageType.DateTimeOffset:
				return XmlConvert.ToDateTimeOffset((string)value);
			case StorageType.Uri:
				return new Uri((string)value);
			case StorageType.SqlBinary:
				return new SqlBinary(Convert.FromBase64String((string)value));
			case StorageType.SqlBoolean:
				return new SqlBoolean(XmlConvert.ToBoolean((string)value));
			case StorageType.SqlByte:
				return new SqlByte(XmlConvert.ToByte((string)value));
			case StorageType.SqlBytes:
				return new SqlBytes(Convert.FromBase64String((string)value));
			case StorageType.SqlChars:
				return new SqlChars(((string)value).ToCharArray());
			case StorageType.SqlDateTime:
				return new SqlDateTime(XmlConvert.ToDateTime((string)value, XmlDateTimeSerializationMode.RoundtripKind));
			case StorageType.SqlDecimal:
				return SqlDecimal.Parse((string)value);
			case StorageType.SqlDouble:
				return new SqlDouble(XmlConvert.ToDouble((string)value));
			case StorageType.SqlGuid:
				return new SqlGuid(XmlConvert.ToGuid((string)value));
			case StorageType.SqlInt16:
				return new SqlInt16(XmlConvert.ToInt16((string)value));
			case StorageType.SqlInt32:
				return new SqlInt32(XmlConvert.ToInt32((string)value));
			case StorageType.SqlInt64:
				return new SqlInt64(XmlConvert.ToInt64((string)value));
			case StorageType.SqlMoney:
				return new SqlMoney(XmlConvert.ToDecimal((string)value));
			case StorageType.SqlSingle:
				return new SqlSingle(XmlConvert.ToSingle((string)value));
			case StorageType.SqlString:
				return new SqlString((string)value);
			}
			if (DBNull.Value == value || value == null)
			{
				return DBNull.Value;
			}
			switch (storageType2)
			{
			case StorageType.Boolean:
				return XmlConvert.ToString((bool)value);
			case StorageType.Char:
				return XmlConvert.ToString((char)value);
			case StorageType.SByte:
				return XmlConvert.ToString((sbyte)value);
			case StorageType.Byte:
				return XmlConvert.ToString((byte)value);
			case StorageType.Int16:
				return XmlConvert.ToString((short)value);
			case StorageType.UInt16:
				return XmlConvert.ToString((ushort)value);
			case StorageType.Int32:
				return XmlConvert.ToString((int)value);
			case StorageType.UInt32:
				return XmlConvert.ToString((uint)value);
			case StorageType.Int64:
				return XmlConvert.ToString((long)value);
			case StorageType.UInt64:
				return XmlConvert.ToString((ulong)value);
			case StorageType.Single:
				return XmlConvert.ToString((float)value);
			case StorageType.Double:
				return XmlConvert.ToString((double)value);
			case StorageType.Decimal:
				return XmlConvert.ToString((decimal)value);
			case StorageType.DateTime:
				return XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.RoundtripKind);
			case StorageType.TimeSpan:
				return XmlConvert.ToString((TimeSpan)value);
			case StorageType.String:
				return (string)value;
			case StorageType.Guid:
				return XmlConvert.ToString((Guid)value);
			case StorageType.CharArray:
				return new string((char[])value);
			case StorageType.DateTimeOffset:
				return XmlConvert.ToString((DateTimeOffset)value);
			case StorageType.SqlBinary:
				return Convert.ToBase64String(((SqlBinary)value).Value);
			case StorageType.SqlBoolean:
				return XmlConvert.ToString(((SqlBoolean)value).Value);
			case StorageType.SqlByte:
				return XmlConvert.ToString(((SqlByte)value).Value);
			case StorageType.SqlBytes:
				return Convert.ToBase64String(((SqlBytes)value).Value);
			case StorageType.SqlChars:
				return new string(((SqlChars)value).Value);
			case StorageType.SqlDateTime:
				return XmlConvert.ToString(((SqlDateTime)value).Value, XmlDateTimeSerializationMode.RoundtripKind);
			case StorageType.SqlDecimal:
				return ((SqlDecimal)value).ToString();
			case StorageType.SqlDouble:
				return XmlConvert.ToString(((SqlDouble)value).Value);
			case StorageType.SqlGuid:
				return XmlConvert.ToString(((SqlGuid)value).Value);
			case StorageType.SqlInt16:
				return XmlConvert.ToString(((SqlInt16)value).Value);
			case StorageType.SqlInt32:
				return XmlConvert.ToString(((SqlInt32)value).Value);
			case StorageType.SqlInt64:
				return XmlConvert.ToString(((SqlInt64)value).Value);
			case StorageType.SqlMoney:
				return XmlConvert.ToString(((SqlMoney)value).Value);
			case StorageType.SqlSingle:
				return XmlConvert.ToString(((SqlSingle)value).Value);
			case StorageType.SqlString:
				return ((SqlString)value).Value;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				return convertible.ToString(CultureInfo.InvariantCulture);
			}
			IFormattable formattable = value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(null, CultureInfo.InvariantCulture);
			}
			return value.ToString();
		}
	}
}

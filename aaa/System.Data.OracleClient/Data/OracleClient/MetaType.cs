using System;
using System.Data.Common;

namespace System.Data.OracleClient
{
	// Token: 0x0200001B RID: 27
	internal sealed class MetaType
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00058AA8 File Offset: 0x00057EA8
		static MetaType()
		{
			MetaType.dbTypeMetaType[0] = new MetaType(DbType.AnsiString, OracleType.VarChar, OCI.DATATYPE.VARCHAR2, "VARCHAR2", typeof(string), typeof(OracleString), 0, 4000, false);
			MetaType.dbTypeMetaType[1] = new MetaType(DbType.Binary, OracleType.Raw, OCI.DATATYPE.RAW, "RAW", typeof(byte[]), typeof(OracleBinary), 0, 2000, false);
			MetaType.dbTypeMetaType[2] = new MetaType(DbType.Byte, OracleType.Byte, OCI.DATATYPE.UNSIGNEDINT, "UNSIGNED INTEGER", typeof(byte), typeof(byte), 1, 1, false);
			MetaType.dbTypeMetaType[3] = new MetaType(DbType.Boolean, OracleType.Byte, OCI.DATATYPE.UNSIGNEDINT, "UNSIGNED INTEGER", typeof(byte), typeof(byte), 1, 1, false);
			MetaType.dbTypeMetaType[4] = new MetaType(DbType.Currency, OracleType.Number, OCI.DATATYPE.VARNUM, "NUMBER", typeof(decimal), typeof(OracleNumber), 22, 22, false);
			MetaType.dbTypeMetaType[5] = new MetaType(DbType.Date, OracleType.DateTime, OCI.DATATYPE.DATE, "DATE", typeof(DateTime), typeof(OracleDateTime), 7, 7, false);
			MetaType.dbTypeMetaType[6] = new MetaType(DbType.DateTime, OracleType.DateTime, OCI.DATATYPE.DATE, "DATE", typeof(DateTime), typeof(OracleDateTime), 7, 7, false);
			MetaType.dbTypeMetaType[7] = new MetaType(DbType.Decimal, OracleType.Number, OCI.DATATYPE.VARNUM, "NUMBER", typeof(decimal), typeof(OracleNumber), 22, 22, false);
			MetaType.dbTypeMetaType[8] = new MetaType(DbType.Double, OracleType.Double, OCI.DATATYPE.FLOAT, "FLOAT", typeof(double), typeof(double), 8, 8, false);
			MetaType.dbTypeMetaType[9] = new MetaType(DbType.Guid, OracleType.Raw, OCI.DATATYPE.RAW, "RAW", typeof(byte[]), typeof(OracleBinary), 16, 16, false);
			MetaType.dbTypeMetaType[10] = new MetaType(DbType.Int16, OracleType.Int16, OCI.DATATYPE.INTEGER, "INTEGER", typeof(short), typeof(short), 2, 2, false);
			MetaType.dbTypeMetaType[11] = new MetaType(DbType.Int32, OracleType.Int32, OCI.DATATYPE.INTEGER, "INTEGER", typeof(int), typeof(int), 4, 4, false);
			MetaType.dbTypeMetaType[12] = new MetaType(DbType.Int64, OracleType.Number, OCI.DATATYPE.VARNUM, "NUMBER", typeof(decimal), typeof(OracleNumber), 22, 22, false);
			MetaType.dbTypeMetaType[13] = new MetaType(DbType.Object, OracleType.Blob, OCI.DATATYPE.BLOB, "BLOB", typeof(object), typeof(OracleLob), IntPtr.Size, IntPtr.Size, false);
			MetaType.dbTypeMetaType[14] = new MetaType(DbType.SByte, OracleType.SByte, OCI.DATATYPE.INTEGER, "INTEGER", typeof(sbyte), typeof(sbyte), 1, 1, false);
			MetaType.dbTypeMetaType[15] = new MetaType(DbType.Single, OracleType.Float, OCI.DATATYPE.FLOAT, "FLOAT", typeof(float), typeof(float), 4, 4, false);
			MetaType.dbTypeMetaType[16] = new MetaType(DbType.String, OracleType.NVarChar, OCI.DATATYPE.VARCHAR2, "NVARCHAR2", typeof(string), typeof(OracleString), 0, 4000, true);
			MetaType.dbTypeMetaType[17] = new MetaType(DbType.Time, OracleType.DateTime, OCI.DATATYPE.DATE, "DATE", typeof(DateTime), typeof(OracleDateTime), 7, 7, false);
			MetaType.dbTypeMetaType[18] = new MetaType(DbType.UInt16, OracleType.UInt16, OCI.DATATYPE.UNSIGNEDINT, "UNSIGNED INTEGER", typeof(ushort), typeof(ushort), 2, 2, false);
			MetaType.dbTypeMetaType[19] = new MetaType(DbType.UInt32, OracleType.UInt32, OCI.DATATYPE.UNSIGNEDINT, "UNSIGNED INTEGER", typeof(uint), typeof(uint), 4, 4, false);
			MetaType.dbTypeMetaType[20] = new MetaType(DbType.UInt64, OracleType.Number, OCI.DATATYPE.VARNUM, "NUMBER", typeof(decimal), typeof(OracleNumber), 22, 22, false);
			MetaType.dbTypeMetaType[21] = new MetaType(DbType.VarNumeric, OracleType.Number, OCI.DATATYPE.VARNUM, "NUMBER", typeof(decimal), typeof(OracleNumber), 22, 22, false);
			MetaType.dbTypeMetaType[22] = new MetaType(DbType.AnsiStringFixedLength, OracleType.Char, OCI.DATATYPE.CHAR, "CHAR", typeof(string), typeof(OracleString), 0, 2000, false);
			MetaType.dbTypeMetaType[23] = new MetaType(DbType.StringFixedLength, OracleType.NChar, OCI.DATATYPE.CHAR, "NCHAR", typeof(string), typeof(OracleString), 0, 2000, true);
			MetaType.oracleTypeMetaType = new MetaType[31];
			MetaType.oracleTypeMetaType[1] = new MetaType(DbType.Binary, OracleType.BFile, OCI.DATATYPE.BFILE, "BFILE", typeof(byte[]), typeof(OracleBFile), IntPtr.Size, IntPtr.Size, false);
			MetaType.oracleTypeMetaType[2] = new MetaType(DbType.Binary, OracleType.Blob, OCI.DATATYPE.BLOB, "BLOB", typeof(byte[]), typeof(OracleLob), IntPtr.Size, IntPtr.Size, false);
			MetaType.oracleTypeMetaType[3] = MetaType.dbTypeMetaType[22];
			MetaType.oracleTypeMetaType[4] = new MetaType(DbType.AnsiString, OracleType.Clob, OCI.DATATYPE.CLOB, "CLOB", typeof(string), typeof(OracleLob), IntPtr.Size, IntPtr.Size, false);
			MetaType.oracleTypeMetaType[5] = new MetaType(DbType.Object, OracleType.Cursor, OCI.DATATYPE.RSET, "REF CURSOR", typeof(object), typeof(object), IntPtr.Size, IntPtr.Size, false);
			MetaType.oracleTypeMetaType[6] = MetaType.dbTypeMetaType[6];
			MetaType.oracleTypeMetaType[8] = new MetaType(DbType.Int32, OracleType.IntervalYearToMonth, OCI.DATATYPE.INT_INTERVAL_YM, "INTERVAL YEAR TO MONTH", typeof(int), typeof(OracleMonthSpan), 5, 5, false);
			MetaType.oracleTypeMetaType[7] = new MetaType(DbType.Object, OracleType.IntervalDayToSecond, OCI.DATATYPE.INT_INTERVAL_DS, "INTERVAL DAY TO SECOND", typeof(TimeSpan), typeof(OracleTimeSpan), 11, 11, false);
			MetaType.oracleTypeMetaType[9] = new MetaType(DbType.Binary, OracleType.LongRaw, OCI.DATATYPE.LONGRAW, "LONG RAW", typeof(byte[]), typeof(OracleBinary), int.MaxValue, 32700, false);
			MetaType.oracleTypeMetaType[10] = new MetaType(DbType.AnsiString, OracleType.LongVarChar, OCI.DATATYPE.LONG, "LONG", typeof(string), typeof(OracleString), int.MaxValue, 32700, false);
			MetaType.oracleTypeMetaType[11] = MetaType.dbTypeMetaType[23];
			MetaType.oracleTypeMetaType[12] = new MetaType(DbType.String, OracleType.NClob, OCI.DATATYPE.CLOB, "NCLOB", typeof(string), typeof(OracleLob), IntPtr.Size, IntPtr.Size, true);
			MetaType.oracleTypeMetaType[13] = MetaType.dbTypeMetaType[21];
			MetaType.oracleTypeMetaType[14] = MetaType.dbTypeMetaType[16];
			MetaType.oracleTypeMetaType[15] = MetaType.dbTypeMetaType[1];
			MetaType.oracleTypeMetaType[16] = new MetaType(DbType.AnsiString, OracleType.RowId, OCI.DATATYPE.VARCHAR2, "ROWID", typeof(string), typeof(OracleString), 3950, 3950, false);
			MetaType.oracleTypeMetaType[18] = new MetaType(DbType.DateTime, OracleType.Timestamp, OCI.DATATYPE.INT_TIMESTAMP, "TIMESTAMP", typeof(DateTime), typeof(OracleDateTime), 11, 11, false);
			MetaType.oracleTypeMetaType[19] = new MetaType(DbType.DateTime, OracleType.TimestampLocal, OCI.DATATYPE.INT_TIMESTAMP_LTZ, "TIMESTAMP WITH LOCAL TIME ZONE", typeof(DateTime), typeof(OracleDateTime), 11, 11, false);
			MetaType.oracleTypeMetaType[20] = new MetaType(DbType.DateTime, OracleType.TimestampWithTZ, OCI.DATATYPE.INT_TIMESTAMP_TZ, "TIMESTAMP WITH TIME ZONE", typeof(DateTime), typeof(OracleDateTime), 13, 13, false);
			MetaType.oracleTypeMetaType[22] = MetaType.dbTypeMetaType[0];
			MetaType.oracleTypeMetaType[23] = MetaType.dbTypeMetaType[2];
			MetaType.oracleTypeMetaType[24] = MetaType.dbTypeMetaType[18];
			MetaType.oracleTypeMetaType[25] = MetaType.dbTypeMetaType[19];
			MetaType.oracleTypeMetaType[26] = MetaType.dbTypeMetaType[14];
			MetaType.oracleTypeMetaType[27] = MetaType.dbTypeMetaType[10];
			MetaType.oracleTypeMetaType[28] = MetaType.dbTypeMetaType[11];
			MetaType.oracleTypeMetaType[29] = MetaType.dbTypeMetaType[15];
			MetaType.oracleTypeMetaType[30] = MetaType.dbTypeMetaType[8];
			MetaType.oracleTypeMetaType_LONGVARCHAR = new MetaType(DbType.AnsiString, OracleType.VarChar, OCI.DATATYPE.LONGVARCHAR, "VARCHAR2", typeof(string), typeof(OracleString), 0, int.MaxValue, false);
			MetaType.oracleTypeMetaType_LONGVARRAW = new MetaType(DbType.Binary, OracleType.Raw, OCI.DATATYPE.LONGVARRAW, "RAW", typeof(byte[]), typeof(OracleBinary), 0, int.MaxValue, false);
			MetaType.oracleTypeMetaType_LONGNVARCHAR = new MetaType(DbType.String, OracleType.NVarChar, OCI.DATATYPE.LONGVARCHAR, "NVARCHAR2", typeof(string), typeof(OracleString), 0, int.MaxValue, true);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00059354 File Offset: 0x00058754
		public MetaType(DbType dbType, OracleType oracleType, OCI.DATATYPE ociType, string dataTypeName, Type convertToType, Type noConvertType, int bindSize, int maxBindSize, bool usesNationalCharacterSet)
		{
			this._dbType = dbType;
			this._oracleType = oracleType;
			this._ociType = ociType;
			this._convertToType = convertToType;
			this._noConvertType = noConvertType;
			this._bindSize = bindSize;
			this._maxBindSize = maxBindSize;
			this._dataTypeName = dataTypeName;
			this._usesNationalCharacterSet = usesNationalCharacterSet;
			switch (oracleType)
			{
			case OracleType.Char:
			case OracleType.Clob:
				break;
			default:
				switch (oracleType)
				{
				case OracleType.LongVarChar:
				case OracleType.NChar:
				case OracleType.NClob:
				case OracleType.NVarChar:
					break;
				case OracleType.Number:
					goto IL_0086;
				default:
					if (oracleType != OracleType.VarChar)
					{
						goto IL_0086;
					}
					break;
				}
				break;
			}
			this._isCharacterType = true;
			IL_0086:
			switch (oracleType)
			{
			case OracleType.LongRaw:
			case OracleType.LongVarChar:
				this._isLong = true;
				break;
			}
			switch (oracleType)
			{
			case OracleType.BFile:
			case OracleType.Blob:
			case OracleType.Clob:
				break;
			case OracleType.Char:
				return;
			default:
				if (oracleType != OracleType.NClob)
				{
					return;
				}
				break;
			}
			this._isLob = true;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0005942C File Offset: 0x0005882C
		internal Type BaseType
		{
			get
			{
				return this._convertToType;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00059440 File Offset: 0x00058840
		internal int BindSize
		{
			get
			{
				return this._bindSize;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00059454 File Offset: 0x00058854
		internal string DataTypeName
		{
			get
			{
				return this._dataTypeName;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00059468 File Offset: 0x00058868
		internal DbType DbType
		{
			get
			{
				return this._dbType;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0005947C File Offset: 0x0005887C
		internal bool IsCharacterType
		{
			get
			{
				return this._isCharacterType;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00059490 File Offset: 0x00058890
		internal bool IsLob
		{
			get
			{
				return this._isLob;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000594A4 File Offset: 0x000588A4
		internal bool IsLong
		{
			get
			{
				return this._isLong;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000594B8 File Offset: 0x000588B8
		internal bool IsVariableLength
		{
			get
			{
				return this._bindSize == 0 || int.MaxValue == this._bindSize;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000594DC File Offset: 0x000588DC
		internal int MaxBindSize
		{
			get
			{
				return this._maxBindSize;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000594F0 File Offset: 0x000588F0
		internal Type NoConvertType
		{
			get
			{
				return this._noConvertType;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00059504 File Offset: 0x00058904
		internal OCI.DATATYPE OciType
		{
			get
			{
				return this._ociType;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00059518 File Offset: 0x00058918
		internal OracleType OracleType
		{
			get
			{
				return this._oracleType;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0005952C File Offset: 0x0005892C
		internal bool UsesNationalCharacterSet
		{
			get
			{
				return this._usesNationalCharacterSet;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00059540 File Offset: 0x00058940
		internal static MetaType GetDefaultMetaType()
		{
			return MetaType.dbTypeMetaType[0];
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00059554 File Offset: 0x00058954
		internal static MetaType GetMetaTypeForObject(object value)
		{
			Type type;
			if (value is Type)
			{
				type = (Type)value;
			}
			else
			{
				type = value.GetType();
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Empty:
				throw ADP.InvalidDataType(TypeCode.Empty);
			case TypeCode.Object:
				if (type == typeof(byte[]))
				{
					return MetaType.dbTypeMetaType[1];
				}
				if (type == typeof(Guid))
				{
					return MetaType.dbTypeMetaType[9];
				}
				if (type == typeof(object))
				{
					throw ADP.InvalidDataTypeForValue(type, Type.GetTypeCode(type));
				}
				if (type == typeof(OracleBFile))
				{
					return MetaType.oracleTypeMetaType[1];
				}
				if (type == typeof(OracleBinary))
				{
					return MetaType.oracleTypeMetaType[15];
				}
				if (type == typeof(OracleDateTime))
				{
					return MetaType.oracleTypeMetaType[6];
				}
				if (type == typeof(OracleNumber))
				{
					return MetaType.oracleTypeMetaType[13];
				}
				if (type == typeof(OracleString))
				{
					return MetaType.oracleTypeMetaType[22];
				}
				if (type == typeof(OracleLob))
				{
					OracleLob oracleLob = (OracleLob)value;
					OracleType lobType = oracleLob.LobType;
					switch (lobType)
					{
					case OracleType.Blob:
						return MetaType.oracleTypeMetaType[2];
					case OracleType.Char:
						break;
					case OracleType.Clob:
						return MetaType.oracleTypeMetaType[4];
					default:
						if (lobType == OracleType.NClob)
						{
							return MetaType.oracleTypeMetaType[12];
						}
						break;
					}
				}
				throw ADP.UnknownDataTypeCode(type, Type.GetTypeCode(type));
			case TypeCode.DBNull:
				throw ADP.InvalidDataType(TypeCode.DBNull);
			case TypeCode.Boolean:
				return MetaType.dbTypeMetaType[3];
			case TypeCode.Char:
				return MetaType.dbTypeMetaType[2];
			case TypeCode.SByte:
				return MetaType.dbTypeMetaType[14];
			case TypeCode.Byte:
				return MetaType.dbTypeMetaType[2];
			case TypeCode.Int16:
				return MetaType.dbTypeMetaType[10];
			case TypeCode.UInt16:
				return MetaType.dbTypeMetaType[18];
			case TypeCode.Int32:
				return MetaType.dbTypeMetaType[11];
			case TypeCode.UInt32:
				return MetaType.dbTypeMetaType[19];
			case TypeCode.Int64:
				return MetaType.dbTypeMetaType[12];
			case TypeCode.UInt64:
				return MetaType.dbTypeMetaType[20];
			case TypeCode.Single:
				return MetaType.dbTypeMetaType[15];
			case TypeCode.Double:
				return MetaType.dbTypeMetaType[8];
			case TypeCode.Decimal:
				return MetaType.dbTypeMetaType[7];
			case TypeCode.DateTime:
				return MetaType.dbTypeMetaType[6];
			case TypeCode.String:
				return MetaType.dbTypeMetaType[0];
			}
			throw ADP.UnknownDataTypeCode(type, Type.GetTypeCode(type));
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00059780 File Offset: 0x00058B80
		internal static MetaType GetMetaTypeForType(DbType dbType)
		{
			if (dbType < DbType.AnsiString || dbType > DbType.StringFixedLength)
			{
				throw ADP.InvalidDbType(dbType);
			}
			return MetaType.dbTypeMetaType[(int)dbType];
		}

		// Token: 0x06000195 RID: 405 RVA: 0x000597A4 File Offset: 0x00058BA4
		internal static MetaType GetMetaTypeForType(OracleType oracleType)
		{
			if (oracleType < OracleType.BFile || oracleType - OracleType.BFile > 30)
			{
				throw ADP.InvalidOracleType(oracleType);
			}
			return MetaType.oracleTypeMetaType[(int)oracleType];
		}

		// Token: 0x04000184 RID: 388
		internal const int LongMax = 2147483647;

		// Token: 0x04000185 RID: 389
		private const string N_BFILE = "BFILE";

		// Token: 0x04000186 RID: 390
		private const string N_BLOB = "BLOB";

		// Token: 0x04000187 RID: 391
		private const string N_CHAR = "CHAR";

		// Token: 0x04000188 RID: 392
		private const string N_CLOB = "CLOB";

		// Token: 0x04000189 RID: 393
		private const string N_DATE = "DATE";

		// Token: 0x0400018A RID: 394
		private const string N_FLOAT = "FLOAT";

		// Token: 0x0400018B RID: 395
		private const string N_INTEGER = "INTEGER";

		// Token: 0x0400018C RID: 396
		private const string N_INTERVALYM = "INTERVAL YEAR TO MONTH";

		// Token: 0x0400018D RID: 397
		private const string N_INTERVALDS = "INTERVAL DAY TO SECOND";

		// Token: 0x0400018E RID: 398
		private const string N_LONG = "LONG";

		// Token: 0x0400018F RID: 399
		private const string N_LONGRAW = "LONG RAW";

		// Token: 0x04000190 RID: 400
		private const string N_NCHAR = "NCHAR";

		// Token: 0x04000191 RID: 401
		private const string N_NCLOB = "NCLOB";

		// Token: 0x04000192 RID: 402
		private const string N_NUMBER = "NUMBER";

		// Token: 0x04000193 RID: 403
		private const string N_NVARCHAR2 = "NVARCHAR2";

		// Token: 0x04000194 RID: 404
		private const string N_RAW = "RAW";

		// Token: 0x04000195 RID: 405
		private const string N_REFCURSOR = "REF CURSOR";

		// Token: 0x04000196 RID: 406
		private const string N_ROWID = "ROWID";

		// Token: 0x04000197 RID: 407
		private const string N_TIMESTAMP = "TIMESTAMP";

		// Token: 0x04000198 RID: 408
		private const string N_TIMESTAMPLTZ = "TIMESTAMP WITH LOCAL TIME ZONE";

		// Token: 0x04000199 RID: 409
		private const string N_TIMESTAMPTZ = "TIMESTAMP WITH TIME ZONE";

		// Token: 0x0400019A RID: 410
		private const string N_UNSIGNEDINT = "UNSIGNED INTEGER";

		// Token: 0x0400019B RID: 411
		private const string N_VARCHAR2 = "VARCHAR2";

		// Token: 0x0400019C RID: 412
		private static readonly MetaType[] dbTypeMetaType = new MetaType[24];

		// Token: 0x0400019D RID: 413
		private static readonly MetaType[] oracleTypeMetaType;

		// Token: 0x0400019E RID: 414
		internal static readonly MetaType oracleTypeMetaType_LONGVARCHAR;

		// Token: 0x0400019F RID: 415
		internal static readonly MetaType oracleTypeMetaType_LONGVARRAW;

		// Token: 0x040001A0 RID: 416
		internal static readonly MetaType oracleTypeMetaType_LONGNVARCHAR;

		// Token: 0x040001A1 RID: 417
		private readonly DbType _dbType;

		// Token: 0x040001A2 RID: 418
		private readonly OracleType _oracleType;

		// Token: 0x040001A3 RID: 419
		private readonly OCI.DATATYPE _ociType;

		// Token: 0x040001A4 RID: 420
		private readonly Type _convertToType;

		// Token: 0x040001A5 RID: 421
		private readonly Type _noConvertType;

		// Token: 0x040001A6 RID: 422
		private readonly int _bindSize;

		// Token: 0x040001A7 RID: 423
		private readonly int _maxBindSize;

		// Token: 0x040001A8 RID: 424
		private readonly string _dataTypeName;

		// Token: 0x040001A9 RID: 425
		private readonly bool _isCharacterType;

		// Token: 0x040001AA RID: 426
		private readonly bool _isLob;

		// Token: 0x040001AB RID: 427
		private readonly bool _isLong;

		// Token: 0x040001AC RID: 428
		private readonly bool _usesNationalCharacterSet;
	}
}
